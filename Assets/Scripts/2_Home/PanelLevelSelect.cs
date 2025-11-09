using UnityEngine;
using UnityEngine.UI;               // Required for Button, Image, etc.
using TMPro;                        // Required for TextMeshPro
using System.Collections.Generic;   // Required for List

public class PanelLevelSelect : PanelBase
{
    [Header("Panel Links")]
    public CtrHome ctrHome; // Assign _CtrHome from the Hierarchy

    [Header("Level Grid Setup")]
    public GameObject levelButtonPrefab;  // Assign your LevelButton_Prefab
    public Transform contentContainer;    // Assign the "Content" object from your Scroll View
    public int totalLevels = 100;         // How many levels to create

    // This function is called by CtrHome when the panel opens
    public new void UIReset()
    {
        base.UIReset(); // This calls the parent's Reset function
    }

    // This is the function CtrHome calls when we click the "Levels" button
    public void SetData()
    {
        // 1. Clear any old buttons (in case we open this panel twice)
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Get the player's progress
        // PlayerPrefs.GetInt will get the saved value. If it finds nothing, it will use "1" as the default.
        // We'll assume the player has always unlocked at least level 1.
        int highestLevelUnlocked = PlayerPrefs.GetInt("HighestLevelUnlocked", 1);


        // 3. The main loop to create all the buttons
        for (int i = 1; i <= totalLevels; i++)
        {
            // Create a new button from the prefab and put it inside the "Content" object
            GameObject newButtonObj = Instantiate(levelButtonPrefab, contentContainer);
            newButtonObj.name = "Level_" + i;

            // Get the components from the new button prefab
            Button button = newButtonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = newButtonObj.GetComponentInChildren<TextMeshProUGUI>();
            
            // Find the "LockIcon" child object we created
            Transform lockIcon = newButtonObj.transform.Find("LockIcon");

            // Set the button's text
            if (buttonText != null)
            {
                buttonText.text = i.ToString();
            }
            
            // This is the level for this button. We must "capture" it
            // in a local variable for the button's click listener to work correctly.
            int levelNumber = i; 

            // 4. Check if this level is locked or unlocked
            if (levelNumber <= highestLevelUnlocked)
            {
                // --- UNLOCKED ---
                if (lockIcon != null)
                {
                    lockIcon.gameObject.SetActive(false); // Hide the lock
                }
                button.interactable = true; // Make the button clickable

                // Add a "click" event
                button.onClick.AddListener(() => {
                    OnLevelButtonClick(levelNumber);
                });
            }
            else
            {
                // --- LOCKED ---
                if (lockIcon != null)
                {
                    lockIcon.gameObject.SetActive(true); // Show the lock
                }
                button.interactable = false; // Make the button non-clickable
            }
        }
    }

    // This is the function that runs when a player clicks an unlocked level button
    void OnLevelButtonClick(int levelNumber)
    {
        Debug.Log("Player clicked level " + levelNumber);

        // 1. Set the static variables to tell CtrGame what to do
        PlayManager.IsLevelMode = true;
        PlayManager.SelectedLevel = levelNumber;

        // 2. Load the main game scene
        // *** YOU MUST CHANGE "Data.scene_play" TO THE REAL SCENE NAME ***
        // (I am guessing the name based on your PlayManager script)
        PlayManager.Instance.LoadScene(Data.scene_play); 
    }


    // This is the function your "Close" button calls
    public void ClosePanel()
    {
        if (ctrHome != null)
        {
            // This tells CtrHome to show the home UI and hide this panel
            ctrHome.SetHomeUI(true, canvasGroup);
        }
    }
}