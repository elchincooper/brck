using System.Collections.Generic;
using UnityEngine;

// This is a static class. It's a "tool" for other scripts to use.
// It doesn't go on a GameObject.
public static class LevelGenerator
{
    // --- The Main "Chef" Function ---
    // This is the function CtrGame will call.
    // It reads the recipe and calls the correct sub-generator.
    public static LevelData GenerateFromRecipe(LevelRecipe recipe, int currentBallCount)
    {
        LevelData level = new LevelData();

        // Calculate the final health based on the recipe and player's ball count
        int baseHealth = recipe.baseHealth + currentBallCount;

        // Call the correct shape generator
        switch (recipe.generatorType)
        {
            case "Ring":
                level = GenerateRing(recipe.shapeSize, baseHealth, recipe.powerupPlacement);
                break;

            case "Spiral":
                // We'd build this function next
                Debug.LogWarning("Generator 'Spiral' not implemented yet.");
                level = GenerateRing(5, baseHealth, "Center"); // Placeholder
                break;

            case "Symbol":
                // We'd build this function next
                Debug.LogWarning("Generator 'Symbol' not implemented yet.");
                level = GenerateRing(6, baseHealth, "None"); // Placeholder
                break;

            case "Checkerboard":
                // We'd build this function next
                Debug.LogWarning("Generator 'Checkerboard' not implemented yet.");
                level = GenerateRing(7, baseHealth, "None"); // Placeholder
                break;
        }

        // --- Apply Health Rules ---
        // After the shape is built, we apply the health rule.
        // (We will build these functions later)
        switch (recipe.healthRule)
        {
            case "Solid":
                // All bricks already have baseHealth, so we do nothing.
                break;
            case "Edges":
                // e.g., Make edges 15 HP, inside 1 HP
                // ApplyEdgeHealth(level, baseHealth * 2, 1);
                break;
            case "Growing":
                // e.g., Health increases toward the center
                // ApplyGrowingHealth(level, baseHealth);
                break;
        }

        return level;
    }


    // --- Shape Generator Example: Ring ---
    // This is one of the functions that builds a shape.
    private static LevelData GenerateRing(float radius, int health, string powerupPlacement)
    {
        LevelData level = new LevelData();

        // We define our grid dimensions. Let's assume 7 columns wide.
        // (You have 7 spawn transforms in CtrBlock.cs)
        const float brickWidth = 1.0f; // This is a guess, we can tune it
        int gridWidth = 7;
        float gridCenter = (gridWidth - 1) / 2f; // e.g., 3

        // Loop through a square grid and check if a point is *inside* the ring
        for (int y = 0; y < radius + 1; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                float posX = (x - gridCenter) * brickWidth;
                float posY = (float)y;

                Vector2 pos = new Vector2(posX, posY);
                float distance = Vector2.Distance(Vector2.zero, pos); // Distance from center (0,0)

                // This is the "ring" logic
                // If distance is *almost* the radius, it's part of the ring.
                if (distance >= radius - 1 && distance <= radius)
                {
                    BrickData brick = new BrickData();
                    brick.position = pos;
                    brick.health = health;
                    brick.type = "normal";

                    level.bricks.Add(brick);
                }
            }
        }

        // --- Apply Power-up Rule ---
        if (powerupPlacement == "Center" && level.bricks.Count > 0)
        {
            // Find the brick closest to the center and make it a power-up
            // (This is just an example of "strategic placement")
            // level.bricks[0].type = "add_ball"; 
        }

        return level;
    }
}