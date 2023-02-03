using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace PlatformerMVC
{
    public class UIView : LevelObjectView
    {
        public List<GameObject> _stars;
        public List<GameObject> _hearts;

        public GameObject _menuPanel;
        public List<Button> _levels;

        public GameObject _gameOverPanel;
        public Button _goMenuButton;
        public Button _exitButton;

        public TextMeshProUGUI _scoreText;
        public TextMeshProUGUI _resultText;
        public TextMeshProUGUI _goToMenuText;
        public GameObject _winImage;
        public GameObject _loseImage;
    }
}
