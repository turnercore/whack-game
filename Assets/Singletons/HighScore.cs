using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "HighScore", menuName = "HighScore")]
public class HighScore : ScriptableObject
{
    public int score = 0;
    private string _playerInitials;

    // File path to save/load the high score data
    private string FilePath => Path.Combine(Application.dataPath, "Data", $"{name}_data.json");

    public string PlayerInitials
    {
        get => _playerInitials;
        set
        {
            if (value.Length > 3)
            {
                _playerInitials = value[..3].ToUpper();
            }
            else
            {
                _playerInitials = value.ToUpper();
            }
        }
    }

    // Save data to the file
    public void SaveData()
    {
        // Ensure the Data folder exists
        string directory = Path.Combine(Application.dataPath, "Data");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        HighScoreData data = new HighScoreData() { score = this.score, name = _playerInitials };

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(FilePath, jsonData);
    }

    // Load data from the file
    public void LoadData()
    {
        if (File.Exists(FilePath))
        {
            string jsonData = File.ReadAllText(FilePath);
            HighScoreTable data = JsonUtility.FromJson<HighScoreTable>(jsonData);

            score = data.highScores[0].score;
            PlayerInitials = data.highScores[0].name;
        }
    }

    [System.Serializable]
    private class HighScoreTable
    {
        public HighScoreData[] highScores;
    }

    // Data class to serialize
    [System.Serializable]
    private class HighScoreData
    {
        public int score;
        public string name;
        public DateTime date;
    }
}
