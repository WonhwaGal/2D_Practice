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

        private readonly List<GameObject> _stars;
        private readonly List<GameObject> _hearts;

        private readonly GameObject _menuPanel;
        private readonly List<Button> _levelButtons;

        private readonly GameObject _gameOverPanel;
        private readonly Button _goMenuButton;
        private readonly Button _exitButton;
        private readonly GameObject _loseImage;
        private readonly GameObject _winImage;

        private TextMeshProUGUI _scoreText;
        private TextMeshProUGUI _resultText;
        private TextMeshProUGUI _goToMenuText;

        private int _currentLevel;

        private Dictionary<int, int> _coinScores = new Dictionary<int, int>();
        private List<LevelObjectView> _collectedCoins = new List<LevelObjectView>();

        private readonly PlayerRbController _characterControl;
        private readonly PlayerView _characterView;
        private AudioSource _audioSource;
        private AudioClip _audioClip;

        public UIController(PlayerRbController characterControl, PlayerView characterView, UIView view)
        {
            _characterControl = characterControl;
            _characterView = characterView;
            _stars = view._stars;
            _hearts = view._hearts;
            _menuPanel = view._menuPanel;
            _levelButtons = view._levelButtons;
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

            if (!ReloadScript.Instance.LoadMenu) _menuPanel.SetActive(false);
            else _menuPanel.SetActive(true);

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
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 1.0f) PauseGame();
                else UnpauseGame();
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
            if (_currentLevel == _levelButtons.Count - 1)
            {
                _resultText.text = "Отлично! Игра пройдена!";
                _goToMenuText.text = "Меню";
            }
            else
            {
                _resultText.text = "Ура!!!";
                _goToMenuText.text = "Дальше";
            }
            _gameOverPanel.SetActive(true);
            _winImage.SetActive(true);
            _loseImage.SetActive(false);
            SaveProgress();
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

        private void SaveProgress()
        {
            ReloadScript.Instance.SaveProgress(_coinScores, _currentLevel);
        }

        private void PauseGame()
        {
            _gameOverPanel.SetActive(true);
            Time.timeScale = 0.0f;
            _winImage?.SetActive(false);
            _loseImage?.SetActive(false);
            _resultText.text = "Чтобы вернуться, нажми ESC";
        }
        private void UnpauseGame()
        {
            _gameOverPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }

        private void AsignButtons()
        {
            // Button functions
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                int _sceneToLoadIndex = i;
                _levelButtons[i].onClick.AddListener(() =>                   
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
                        Time.timeScale = 1.0f;
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
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                int _sceneToLoadIndex = i;
                if (!_coinScores.ContainsKey(_sceneToLoadIndex)) _coinScores.Add(_sceneToLoadIndex, 0);

                if (_sceneToLoadIndex <= ReloadScript.Instance.HighestLevel)           // Score
                {
                    _levelButtons[i].interactable = true;
                    _levelButtons[i].GetComponent<Image>().color = Color.white;
                    for (int j = 0; j < _levelButtons[i].transform.childCount; j++)
                    {
                        Transform child = _levelButtons[i].transform.GetChild(j);
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
