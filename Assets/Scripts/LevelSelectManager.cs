using UnityEngine;
using UnityEngine.SceneManagement; // You MUST add this line!

public class LevelSelectManager : MonoBehaviour
{
    // This function will be called by our Back button
    public void LoadScene(string sceneName)
    {
        // This is the line that actually loads the new scene
        SceneManager.LoadScene(sceneName);
    }
}