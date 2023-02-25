using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlatformerMVC
{
    public class UIView : LevelObjectView
    {
        [Header("UI Objects")]
        public List<GameObject> _stars;
        public List<GameObject> _hearts;

        [Space(5), Header("Menu Panel")]
        public GameObject _menuPanel;
        public List<Button> _levelButtons;

        [Space(5), Header("GameOver Panel")]
        public GameObject _gameOverPanel;
        public Button _goMenuButton;
        public Button _exitButton;
        public GameObject _winImage;
        public GameObject _loseImage;

        [Space(5), Header("Text Fields")]
        public TextMeshProUGUI _scoreText;
        public TextMeshProUGUI _resultText;
        public TextMeshProUGUI _goToMenuText;
    }
}
