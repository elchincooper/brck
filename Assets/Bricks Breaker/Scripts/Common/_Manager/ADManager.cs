using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class ADManager : Singleton<ADManager>, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidGameId = "1234567";
    [SerializeField] string iosGameId = "1234567";
    [SerializeField] bool testMode = true;

    string gameId;
    string adUnitInterstitial;
    string adUnitRewarded;

    private Action<ShowResult> rewardCallback;
    private bool interstitialLoaded = false;
    private bool rewardedLoaded = false;

    void Awake()
    {
#if UNITY_IOS
        gameId = iosGameId;
        adUnitInterstitial = "Interstitial_iOS";
        adUnitRewarded = "Rewarded_iOS";
#elif UNITY_ANDROID
        gameId = androidGameId;
        adUnitInterstitial = "Interstitial_Android";
        adUnitRewarded = "Rewarded_Android";
#else
        gameId = androidGameId;
        adUnitInterstitial = "Interstitial_Android";
        adUnitRewarded = "Rewarded_Android";
#endif
        InitializeAds();
    }

    void InitializeAds()
    {
        Advertisement.Initialize(gameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadInterstitial();
        LoadRewarded();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadInterstitial()
    {
        Advertisement.Load(adUnitInterstitial, this);
    }

    public void LoadRewarded()
    {
        Advertisement.Load(adUnitRewarded, this);
    }

    public void ShowInterstitialAd()
    {
        if (interstitialLoaded)
        {
            Advertisement.Show(adUnitInterstitial, this);
        }
        else
        {
            Debug.Log("Interstitial ad not ready.");
            LoadInterstitial();
        }
    }

    public void ShowRewardedVideo(Action<ShowResult> onFinish)
    {
        rewardCallback = onFinish;

        if (rewardedLoaded)
        {
            Advertisement.Show(adUnitRewarded, this);
        }
        else
        {
            Debug.Log("Rewarded ad not ready.");
            rewardCallback?.Invoke(ShowResult.Failed);
            LoadRewarded();
        }
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log($"Ad Loaded: {adUnitId}");
        if (adUnitId == adUnitInterstitial) interstitialLoaded = true;
        if (adUnitId == adUnitRewarded) rewardedLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        if (adUnitId == adUnitInterstitial) interstitialLoaded = false;
        if (adUnitId == adUnitRewarded) rewardedLoaded = false;
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log("Ad started: " + adUnitId);
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("Ad clicked: " + adUnitId);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad completed: {adUnitId} - {showCompletionState}");

        if (adUnitId == adUnitRewarded && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            rewardCallback?.Invoke(ShowResult.Finished);
        }
        else if (adUnitId == adUnitRewarded)
        {
            rewardCallback?.Invoke(ShowResult.Skipped);
        }

        if (adUnitId == adUnitRewarded)
            LoadRewarded();
        if (adUnitId == adUnitInterstitial)
            LoadInterstitial();
    }

    // ✅ Compatibility for old scripts like PanelShop
    public bool IsReady
    {
        get { return Advertisement.isInitialized && (rewardedLoaded || interstitialLoaded); }
    }
}