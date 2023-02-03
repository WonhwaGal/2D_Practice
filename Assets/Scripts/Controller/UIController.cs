using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace PlatformerMVC
{
    public class UIController : IDisposable
    {
        public Action OnCollectingAllStars { get; set; }

        private List<GameObject> _stars;
        private List<GameObject> _hearts;

        private GameObject _menuPanel;
        private List<Button> _levels;

        private GameObject _gameOverPanel;
        private Button _goMenuButton;
        private Button _exitButton;

        private TextMeshProUGUI _scoreText;

        private int _currentLevel;

        private Dictionary<int, int> _coinScores = new Dictionary<int, int>();
        private List<LevelObjectView> _collectedCoins = new List<LevelObjectView>();

        private TextMeshProUGUI _resultText;
        private TextMeshProUGUI _goToMenuText;
        private GameObject _loseImage;
        private GameObject _winImage;

        private PlayerRbController _characterControl;
        private PlayerView _characterView;
        private AudioSource _audioSource;
        private AudioClip _audioClip;

        public UIController(PlayerRbController characterControl, PlayerView characterView, UIView view)
        {
            _characterControl = characterControl;
            _characterView = characterView;
            _stars = view._stars;
            _hearts = view._hearts;
            _menuPanel = view._menuPanel;
            _levels = view._levels;
            _gameOverPanel = view._gameOverPanel;
            _goMenuButton = view._goMenuButton;
            _exitButton = view._exitButton;
            _scoreText = view._scoreText;
            _resultText = view._resultText;
            _goToMenuText = view._goToMenuText;
            _loseImage = view._loseImage;
            _winImage = view._winImage;
            foreach (GameObject star in _stars)
            {
                star.SetActive(false);
            }
            foreach (GameObject heart in _hearts)
            {
                heart.SetActive(true);
            }
            _scoreText.text = "0";

            AsignButtons();
            if (!ReloadScript.Instance.LoadMenu)
            {
                _menuPanel.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                _menuPanel.SetActive(true);
                Time.timeScale = 0.0f;
            }
            _gameOverPanel.SetActive(false);
            _characterControl.onLosing += GameOver;
            _characterView.onGettingHurt += MinusHeart;
            _characterView.OnCollectingCoin += AddCoin;
            _resultText.text = String.Empty;
            _currentLevel = SceneManager.GetActiveScene().buildIndex;
            if (view.TryGetComponent<AudioSource>(out _audioSource))
            {
                _audioClip = Resources.Load<AudioClip>("button");
            }
        }
        private void GameOver()
        {
            foreach (GameObject heart in _hearts)
            {
                heart.SetActive(false);
            }
            _resultText.text = "Увы...";
            _goToMenuText.text = "Заново";
            _goMenuButton.onClick.RemoveAllListeners();
            _goMenuButton.onClick.AddListener(() =>
            {
                _audioSource.PlayOneShot(_audioClip);
                ReloadScript.Instance.LoadMenu = false;
                SceneManager.LoadScene(_currentLevel);
            });
            _gameOverPanel.SetActive(true);
            _winImage.SetActive(false);
            _loseImage.SetActive(true);
            Time.timeScale = 0.0f;
            Dispose();
        }
        private void Win()
        {
            _resultText.text = "Ура!!!";
            _goToMenuText.text = "Дальше";
            _gameOverPanel.SetActive(true);
            _winImage.SetActive(true);
            _loseImage.SetActive(false);
            int _resultScore = _coinScores[_currentLevel];
            if (ReloadScript.Instance.HighestLevel == _currentLevel && _resultScore > 9)
                ReloadScript.Instance.HighestLevel++;

            PlayerPrefs.SetInt("Highest", ReloadScript.Instance.HighestLevel);
            if (PlayerPrefs.HasKey("LevelOne"))
            {
                for (int i = 0; i < _coinScores.Count; i++)
                {
                    if (_coinScores[i] > ReloadScript.Instance.CoinNumbers[i])
                    {
                        ReloadScript.Instance.CoinNumbers[i] = _coinScores[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < _coinScores.Count; i++)
                {
                    ReloadScript.Instance.CoinNumbers[i] = _coinScores[i];
                }

            }
            PlayerPrefs.SetInt("LevelOne", ReloadScript.Instance.CoinNumbers[0]);
            PlayerPrefs.SetInt("LevelTwo", ReloadScript.Instance.CoinNumbers[1]);
            PlayerPrefs.SetInt("LevelThree", ReloadScript.Instance.CoinNumbers[2]);
            ReloadMenu();
        }

        private void MinusHeart()
        {
            if (_hearts[0].activeInHierarchy)
            {
                _hearts[0].SetActive(false);
            }
            else
            {
                _hearts[1].SetActive(false);
            }
        }
        public void AddStar(int indexOfStar)
        {
            bool progress = _stars.TrueForAll(value => value.activeInHierarchy);
            if (indexOfStar < 3)
            {
                _stars[indexOfStar].SetActive(true);
                progress = _stars.TrueForAll(value => value.activeInHierarchy);
                if (progress)
                {
                    OnCollectingAllStars?.Invoke();
                }
            }
            else if (progress) Win();
        }

        private void AddCoin(LevelObjectView _coinView)
        {
            if (!_collectedCoins.Contains(_coinView))
            {
                _collectedCoins.Add(_coinView);
                _coinScores[_currentLevel]++;
                _scoreText.text = _coinScores[_currentLevel].ToString();
            }
        }
        public void PauseGame()
        {
            _gameOverPanel.SetActive(true);
            Time.timeScale = 0.0f;
            _winImage?.SetActive(false);
            _loseImage?.SetActive(false);
            _resultText.text = "Чтобы вернуться, нажми ESC";
        }
        public void UnpauseGame()
        {
            _gameOverPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        private void AsignButtons()
        {
            // Button functions
            for (int i = 0; i < _levels.Count; i++)
            {
                int _sceneToLoadIndex = i;
                _levels[i].onClick.AddListener(() =>                   
                {
                    _audioSource.PlayOneShot(_audioClip);
                    if (_sceneToLoadIndex == SceneManager.GetActiveScene().buildIndex && ReloadScript.Instance.LoadMenu)
                    {
                        _menuPanel.SetActive(false);
                        Time.timeScale = 1.0f;
                        ReloadScript.Instance.LoadMenu = false;
                    }
                    else
                    {
                        ReloadScript.Instance.LoadMenu = false;
                        SceneManager.LoadScene(_sceneToLoadIndex);
                    }
                });
            }
            _goMenuButton.onClick.AddListener(() =>
            {
                _audioSource.PlayOneShot(_audioClip);
                _gameOverPanel.SetActive(false);
                _menuPanel.SetActive(true);
            });
            _exitButton.onClick.AddListener(() =>
            {
                _audioSource.PlayOneShot(_audioClip);
                Application.Quit();
            });
            ReloadMenu();
        }
        private void ColorChildren(TextMeshProUGUI text, Color color)
        {
            text.color = color;
            if (text.transform.childCount > 0)
            {
                for (int k = 0; k < text.transform.childCount; k++)
                {
                    text.transform.GetChild(k).TryGetComponent(out TextMeshProUGUI textChild);
                    textChild.color = color;
                }
            }
        }
        private void ReloadMenu()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                int _sceneToLoadIndex = i;
                if (!_coinScores.ContainsKey(_sceneToLoadIndex)) _coinScores.Add(_sceneToLoadIndex, 0);
                //_levels[i].onClick.AddListener(() =>                   // Button function
                //{
                //    _audioSource.PlayOneShot(_audioClip);
                //    if (_sceneToLoadIndex == SceneManager.GetActiveScene().buildIndex && ReloadScript.Instance.LoadMenu)
                //    {
                //        _menuPanel.SetActive(false);
                //        Time.timeScale = 1.0f;
                //        ReloadScript.Instance.LoadMenu = false;
                //    }
                //    else
                //    {
                //        ReloadScript.Instance.LoadMenu = false;
                //        SceneManager.LoadScene(_sceneToLoadIndex);
                //    }
                //});
                if (_sceneToLoadIndex <= ReloadScript.Instance.HighestLevel)           // Score
                {
                    _levels[i].interactable = true;
                    _levels[i].GetComponent<Image>().color = Color.white;
                    for (int j = 0; j < _levels[i].transform.childCount; j++)
                    {
                        Transform child = _levels[i].transform.GetChild(j);
                        if (child.TryGetComponent(out TextMeshProUGUI text))
                        {
                            text.color = Color.black;
                            if (text.gameObject.CompareTag("Score"))
                            {
                                string score = ReloadScript.Instance.CoinNumbers[i].ToString();
                                text.text = $"{score}";
                                ColorChildren(text, Color.white);
                                if (ReloadScript.Instance.CoinNumbers[i] < 10) text.color = Color.red;
                            }
                        }
                        else if (child.TryGetComponent(out Image image))
                        {
                            image.color = Color.white;
                        }
                    }
                }
            }
        }
        public void Dispose()
        {
            _characterControl.onLosing -= GameOver;
            _characterView.onGettingHurt -= MinusHeart;
        }
    }
}
