using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace PlatformerMVC
{
    public class UIManagerNew : IDisposable
    {
        public Action OnCollectingAllStars { get; set; }
        private Canvas _canvas;
        private int _currentLevel;

        private GameObject _menuPanel;
        private TextMeshProUGUI _scoreText;

        private Dictionary<int, int> _coinScores = new Dictionary<int, int>();
        private List<LevelObjectView> _collectedCoins = new List<LevelObjectView>();
        private GameObject _gameOverPanel;
        private Button _goMenuButton;
        private Button _exitButton;
        private TextMeshProUGUI _resultText;
        private TextMeshProUGUI _goToMenuText;
        private GameObject _loseImage;
        private GameObject _winImage;

        private List<GameObject> _stars = new List<GameObject>(0);
        private List<GameObject> _hearts = new List<GameObject>(0);
        private Color _passiveColor = new Color(0.53f, 0.56f, 0.7f, 0.7f);

        private PlayerRbController _characterControl;
        private PlayerView _characterView;
        private AudioSource _audioSource;
        private AudioClip _audioClip;
        public UIManagerNew(PlayerRbController characterControl, PlayerView characterView, Canvas canvas)
        {
            _characterControl = characterControl;
            _characterView = characterView;
            _canvas = canvas;
            Transform _starPanel = _canvas.transform.GetChild(0);
            for (int i = 0; i < _starPanel.childCount; i++)
            {
                _stars.Add(_starPanel.GetChild(i).GetChild(0).gameObject);
                _stars[i].SetActive(false);
            }
            Transform _heartPanel = _canvas.transform.GetChild(1);
            for (int i = 0; i < _heartPanel.childCount; i++)
            {
                _hearts.Add(_heartPanel.GetChild(i).gameObject);
                _hearts[i].SetActive(true);
            }
            _scoreText = _canvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _menuPanel = _canvas.transform.GetChild(3).gameObject;
            _gameOverPanel = _canvas.transform.GetChild(4).gameObject;

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
            //LevelCompleteManager.onRecievingStar += AddStar;
            _resultText.text = String.Empty;
            _currentLevel = SceneManager.GetActiveScene().buildIndex;
            if (canvas.TryGetComponent<AudioSource>(out _audioSource))
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
                        Debug.Log($"level {i} - {ReloadScript.Instance.CoinNumbers[i]} coins");
                        ReloadScript.Instance.CoinNumbers[i] = _coinScores[i];
                        Debug.Log($"total result {ReloadScript.Instance.CoinNumbers[i]}");
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
            Dispose();
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
            Debug.Log("index is " + indexOfStar);
            if (progress) Debug.Log("progress " + progress);
            if (indexOfStar < 3)
            {
                if (progress)
                {
                    OnCollectingAllStars?.Invoke();
                }
                _stars[indexOfStar].SetActive(true);
            }
            else if (progress)
            {
                Win();
            }

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
            //_lvlOneButton - (0);  _lvlTwoButton - (1);  _lvlThreeButton - (2);
            for (int i = 0; i < _menuPanel.transform.childCount; i++)
            {
                Button button = _menuPanel.transform.GetChild(i).GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                int _sceneToLoadIndex = i;
                _coinScores.Add(_sceneToLoadIndex, 0);
                button.onClick.AddListener(() =>                   // Button function
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
                if (_sceneToLoadIndex > ReloadScript.Instance.HighestLevel)           // Score
                {
                    button.interactable = false;
                    for (int j = 0; j < button.transform.childCount; j++)
                    {
                        if (button.transform.GetChild(j).TryGetComponent(out Image image)) image.color = _passiveColor;
                        else if (button.transform.GetChild(j).TryGetComponent(out TextMeshProUGUI text))
                        {
                            text.color = _passiveColor;
                            ColorChildren(text, _passiveColor);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < button.transform.childCount; j++)
                    {
                        if (button.transform.GetChild(j).TryGetComponent(out TextMeshProUGUI text))
                        {
                            if (text.gameObject.CompareTag("Score"))
                            {
                                string score = ReloadScript.Instance.CoinNumbers[i].ToString();
                                text.text = $"{score}";
                                if (ReloadScript.Instance.CoinNumbers[i] < 10) text.color = Color.red;
                                else text.color = Color.white;
                                ColorChildren(text, Color.white);
                            }
                        }
                    }
                }
            }

            _goMenuButton = _gameOverPanel.transform.GetChild(0).gameObject.GetComponent<Button>();
            _goMenuButton.onClick.RemoveAllListeners();
            _goMenuButton.onClick.AddListener(() =>
            {
                _audioSource.PlayOneShot(_audioClip);
                _gameOverPanel.SetActive(false);
                _menuPanel.SetActive(true);
            });
            _exitButton = _gameOverPanel.transform.GetChild(1).gameObject.GetComponent<Button>();
            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(() =>
            {
                _audioSource.PlayOneShot(_audioClip);
                Application.Quit();
            });
            _resultText = _gameOverPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            _goToMenuText = _goMenuButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _loseImage = _gameOverPanel.transform.GetChild(3).gameObject;
            _winImage = _gameOverPanel.transform.GetChild(4).gameObject;
        }
        private void ColorChildren(TextMeshProUGUI text, Color color)
        {
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
            for (int i = 0; i < _menuPanel.transform.childCount; i++)
            {
                Button button = _menuPanel.transform.GetChild(i).GetComponent<Button>();
                int _sceneToLoadIndex = i;
                if (_sceneToLoadIndex > ReloadScript.Instance.HighestLevel)             // Closed level score
                {
                    button.interactable = false;
                    for (int j = 0; j < button.transform.childCount; j++)
                    {
                        if (button.transform.GetChild(j).TryGetComponent(out Image image)) image.color = _passiveColor;
                        else if (button.transform.GetChild(j).TryGetComponent(out TextMeshProUGUI text))
                        {
                            text.color = _passiveColor;
                            ColorChildren(text, _passiveColor);
                        }
                    }
                }
                else      // it is a completed level or next open level
                {
                    button.interactable = true;
                    for (int j = 0; j < button.transform.childCount; j++)
                    {
                        if (button.transform.GetChild(j).TryGetComponent(out TextMeshProUGUI text))
                        {
                            text.color = Color.black;
                            if (text.gameObject.CompareTag("Score"))
                            {
                                string score = ReloadScript.Instance.CoinNumbers[i].ToString();
                                text.text = $"{score}";
                                if (ReloadScript.Instance.CoinNumbers[i] < 10) text.color = Color.red;
                                else text.color = Color.white;
                                ColorChildren(text, Color.white);
                            }

                        }
                        else if(button.transform.GetChild(j).TryGetComponent(out Image image))
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
            //LevelCompleteManager.onRecievingStar -= AddStar;
        }

    }
}
