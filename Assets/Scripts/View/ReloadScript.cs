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
            if (PlayerPrefs.HasKey("Highest"))
            {
                HighestLevel = PlayerPrefs.GetInt("Highest");
            }
            else
            {
                HighestLevel = 0;
            }
            if (PlayerPrefs.HasKey("LevelOne"))
            {
                CoinNumbers.Add(0, PlayerPrefs.GetInt("LevelOne"));
            }
            else
            {
                CoinNumbers.Add(0, 0);
            }
            if (PlayerPrefs.HasKey("LevelTwo"))
            {
                CoinNumbers.Add(1, PlayerPrefs.GetInt("LevelTwo"));
            }
            else
            {
                CoinNumbers.Add(1, 0);
            }
            if (PlayerPrefs.HasKey("LevelThree"))
            {
                CoinNumbers.Add(2, PlayerPrefs.GetInt("LevelThree"));
            }
            else
            {
                CoinNumbers.Add(2, 0);
            }
        }
    }
}
