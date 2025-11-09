using System.Collections.Generic; // Required for List and Dictionary
using System.IO;                  // Required for File operations
using UnityEngine;                // Required for Application.streamingAssetsPath
using Newtonsoft.Json;            // Required for reading the JSON file

// This is a static class. It doesn't go on a GameObject.
// It's a global utility that we can access from anywhere.
public static class LevelDatabase
{
    // This dictionary stores all recipes, using the level number as the key
    private static Dictionary<int, LevelRecipe> recipes;

    private static bool isInitialized = false;

    // This is the function that loads the "Cookbook"
    private static void Initialize()
    {
        if (isInitialized) return;

        // Find the JSON file in StreamingAssets
        string path = Path.Combine(Application.streamingAssetsPath, "MasterLevelList.json");
        string jsonString;

        if (Application.platform == RuntimePlatform.Android)
        {
            // On Android, we must use UnityWebRequest to read from StreamingAssets
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
            www.SendWebRequest();
            while (!www.isDone) { /* wait */ }
            jsonString = www.downloadHandler.text;
        }
        else
        {
            // On PC/Editor, we can just read the file directly
            jsonString = File.ReadAllText(path);
        }

        // Read the list of recipes from the JSON
        List<LevelRecipe> recipeList = JsonConvert.DeserializeObject<List<LevelRecipe>>(jsonString);

        // Create the dictionary for fast lookups
        recipes = new Dictionary<int, LevelRecipe>();
        foreach (LevelRecipe recipe in recipeList)
        {
            recipes[recipe.level] = recipe;
        }

        isInitialized = true;
        Debug.Log("LevelDatabase Initialized. Loaded " + recipes.Count + " recipes.");
    }

    // This is the public function our CtrGame will call.
    // It gets the recipe for a specific level.
    public static LevelRecipe GetRecipe(int levelNumber)
    {
        // Make sure the database is loaded
        if (!isInitialized)
        {
            Initialize();
        }

        // Try to find the requested level
        if (recipes.ContainsKey(levelNumber))
        {
            return recipes[levelNumber];
        }
        else
        {
            Debug.LogError("LevelDatabase: Could not find recipe for level " + levelNumber);
            return null; // or return a default recipe
        }
    }
}