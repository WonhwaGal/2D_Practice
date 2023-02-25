using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class ReloadScript : MonoBehaviour
    {
        public static ReloadScript Instance;

        private Dictionary<int, int> _coinNumbers = new Dictionary<int, int>();
        public bool LoadMenu { get; set; }
        public int HighestLevel { get; set; }
        public Dictionary<int, int> CoinNumbers { get => _coinNumbers; set => _coinNumbers = value; }
        private const int _levelCount = 3;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);

            LoadMenu = true;
            LoadProgress();
        }
        public void LoadProgress()
        {
            if (PlayerPrefs.HasKey("Highest")) HighestLevel = PlayerPrefs.GetInt("Highest");
            else HighestLevel = 0;

            if (PlayerPrefs.HasKey("LevelOne")) CoinNumbers.Add(0, PlayerPrefs.GetInt("LevelOne"));
            else CoinNumbers.Add(0, 0);

            if (PlayerPrefs.HasKey("LevelTwo")) CoinNumbers.Add(1, PlayerPrefs.GetInt("LevelTwo"));
            else CoinNumbers.Add(1, 0);

            if (PlayerPrefs.HasKey("LevelThree")) CoinNumbers.Add(2, PlayerPrefs.GetInt("LevelThree"));
            else CoinNumbers.Add(2, 0);
            PlayerPrefs.Save();
        }
        public void SaveProgress(Dictionary<int, int> coinScores, int currentLevel)
        {
            int _resultScore = coinScores[currentLevel];
            if (HighestLevel == currentLevel && _resultScore > 9)
                HighestLevel++;
            PlayerPrefs.SetInt("Highest", HighestLevel);

            if (PlayerPrefs.HasKey("LevelOne"))
            {
                for (int i = 0; i < _levelCount; i++)
                {
                    if (coinScores[i] > CoinNumbers[i])
                    {
                        CoinNumbers[i] = coinScores[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < _levelCount; i++)
                {
                    CoinNumbers[i] = coinScores[i];
                }
            }
            PlayerPrefs.SetInt("LevelOne", CoinNumbers[0]);
            PlayerPrefs.SetInt("LevelTwo", CoinNumbers[1]);
            PlayerPrefs.SetInt("LevelThree", CoinNumbers[2]);
            PlayerPrefs.Save();
        }
    }
}
