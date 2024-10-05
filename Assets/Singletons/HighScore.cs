using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "HighScore", menuName = "HighScore")]
public class HighScore : ScriptableObject
{
    public int score = 0;
    private string _playerInitials;

    // File path to save/load the high score data
    private string FilePath => Path.Combine(Application.dataPath, "Data", $"{name}_data.json");

    // Save data to the file
    public void SaveData()
    {
        // Ensure the Data folder exists
        string directory = Path.Combine(Application.dataPath, "Data");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        HighScoreData data = new HighScoreData()
        {
            savedScore = score,
            savedPlayerInitials = _playerInitials,
        };

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(FilePath, jsonData);
    }

    // Load data from the file
    public void LoadData()
    {
        if (File.Exists(FilePath))
        {
            string jsonData = File.ReadAllText(FilePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(jsonData);

            this.score = data.savedScore;
            this._playerInitials = data.savedPlayerInitials;
        }
    }

    public string PlayerInitials
    {
        get => _playerInitials;
        set
        {
            if (value.Length > 3)
            {
                _playerInitials = value.Substring(0, 3).ToUpper();
            }
            else
            {
                _playerInitials = value.ToUpper();
            }
        }
    }

    // Data class to serialize
    [System.Serializable]
    private class HighScoreData
    {
        public int savedScore;
        public string savedPlayerInitials;
    }
}
