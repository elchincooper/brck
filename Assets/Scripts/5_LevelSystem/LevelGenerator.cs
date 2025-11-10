using System.Collections.Generic;
using UnityEngine;

public static class LevelGenerator
{
    // The main function CtrGame calls
    public static LevelData GenerateFromRecipe(LevelRecipe recipe, int currentBallCount)
    {
        LevelData level = new LevelData();
        int baseHealth = recipe.baseHealth + currentBallCount;

        // Call the correct shape generator
        switch (recipe.generatorType)
        {
            case "Ring":
                level = GenerateRing(recipe.shapeSize, baseHealth, recipe.powerupPlacement);
                break;
            
            case "Symbol":
                level = GenerateSymbol(recipe.symbolName, baseHealth, recipe.powerupPlacement);
                break;
            
            // --- [NEW] ---
            // A simple generator for early levels
            case "Rectangle":
                level = GenerateRectangle((int)recipe.shapeSize, 3, baseHealth, recipe.powerupPlacement);
                break;

            case "Spiral":
            case "Checkerboard":
            default:
                Debug.LogWarning("Generator '" + recipe.generatorType + "' not implemented. Defaulting to Rectangle.");
                level = GenerateRectangle(5, 3, baseHealth, "Center"); // Default to a 5x3 rectangle
                break;
        }
        
        // --- Apply Health Rules ---
        // (We will add complex rules like "Edges" later. For now, solid health.)
        switch (recipe.healthRule)
        {
            case "Solid":
            default:
                // All bricks already have baseHealth, so we do nothing.
                break;
        }
        return level;
    }
    
    // --- [NEW] Generator: Rectangle ---
    // This creates a solid block of bricks
    private static LevelData GenerateRectangle(int width, int height, int health, string powerupPlacement)
    {
        LevelData level = new LevelData();
        
        // Logical grid center (assumes 7 columns, -3 to +3)
        float gridCenter = 3f; 
        // Start X position to center the rectangle
        float startX = -Mathf.Floor(width / 2f); 
        
        // Y position starts from the top. Let's say row 4.
        int startY = 4; 

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BrickData brick = new BrickData();
                brick.position = new Vector2(startX + x, startY - y); // Y decreases as we go down
                brick.health = health;
                brick.type = "normal";
                
                // --- [FIX] Powerup Placement ---
                if (powerupPlacement == "Center" && x == width / 2 && y == height / 2)
                {
                    brick.type = "add_ball"; // Place one in the middle
                }
                
                level.bricks.Add(brick);
            }
        }
        return level;
    }

    // --- Shape Generator: Ring ---
    private static LevelData GenerateRing(float radius, int health, string powerupPlacement)
    {
        LevelData level = new LevelData();
        float gridCenter = 3f;
        int startY = 4;

        for (int y = 0; y < 7; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                float logicalX = x - gridCenter;
                float logicalY = startY - y;
                
                Vector2 pos = new Vector2(logicalX, logicalY);
                float distance = pos.magnitude;

                if (distance >= radius - 0.5f && distance <= radius + 0.5f)
                {
                    BrickData brick = new BrickData();
                    brick.position = pos;
                    brick.health = health;
                    brick.type = "normal";
                    level.bricks.Add(brick);
                }
            }
        }
        
        if (powerupPlacement == "Center" && level.bricks.Count > 0)
        {
            // --- [FIX] Powerup Placement ---
            level.bricks[level.bricks.Count / 2].type = "add_ball"; 
        }
        
        return level;
    }

    // --- Shape Generator: Symbol ---
    private static LevelData GenerateSymbol(string symbolName, int health, string powerupPlacement)
    {
        LevelData level = new LevelData();
        float gridCenter = 3f;
        int startY = 4;
        
        string[] smileyPattern = new string[]
        {
            ". X X X X X .", // Y=4
            "X . X . X . X", // Y=3
            "X . . . . . X", // Y=2
            "X . X X X . X", // Y=1
            ". X X X X X ."  // Y=0
        };

        if (symbolName == "SmileyFace")
        {
            for (int y = 0; y < smileyPattern.Length; y++)
            {
                string[] cols = smileyPattern[y].Split(' ');
                for (int x = 0; x < cols.Length; x++)
                {
                    if (cols[x] == "X")
                    {
                        BrickData brick = new BrickData();
                        brick.position = new Vector2(x - gridCenter, startY - y);
                        brick.health = health;
                        brick.type = "normal";
                        
                        // --- [FIX] Powerup Placement ---
                        if (powerupPlacement == "Eyes" && (y == 1 && (x == 2 || x == 4)))
                        {
                             brick.type = "add_ball";
                        }
                        level.bricks.Add(brick);
                    }
                }
            }
        }
        return level;
    }
}