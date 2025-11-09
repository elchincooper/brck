using System.Collections.Generic; // Required for List
using UnityEngine;                 // Required for Vector2

// This is NOT a MonoBehaviour. It's a simple data container.
// This class defines a single brick.
[System.Serializable]
public class BrickData
{
    public string type;   // "normal", "add_ball", "explosive", etc.
    public Vector2 position; // The (x, y) grid position to spawn at
    public int health;
}

// This is NOT a MonoBehaviour. It's a container that holds
// all the bricks for one level.
[System.Serializable]
public class LevelData
{
    public List<BrickData> bricks;

    // Constructor to create a new, empty list
    public LevelData()
    {
        bricks = new List<BrickData>();
    }
}