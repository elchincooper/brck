// This is NOT a MonoBehaviour. It's a data container.
// It matches the structure of our JSON file.
[System.Serializable]
public class LevelRecipe
{
    public int level;
    public string generatorType;     // "Ring", "Spiral", "Symbol", "Checkerboard"
    public string healthRule;        // "Solid", "Edges", "Growing", "Alternating"
    public string powerupPlacement;  // "Center", "Entry", "Eyes", "None"

    public int baseHealth;
    public float shapeSize;          // Used as radius, length, etc.
    public string symbolName;        // The name of the symbol to load, e.g., "SmileyFace"
}