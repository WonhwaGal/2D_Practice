using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class LevelCompleteManager : IDisposable
    {
        public static Action<int, int> onRecievingStar;
        private PlayerView _characterView;
        private List<LevelObjectView> _deathZones;
        //private List<LevelObjectView> _winZones;
        private List<RestartObjView> _restartingObjects;
        private List<RestartObjView> _fullReloadObjects;
        private List<GameObject> _fullReloadCopies = new List<GameObject>();
        //public int _pointsToWin = 0;
        //private List<Transform> _starTransforms;
        private AudioSource _audioSource;
        private AudioClip _audioClip;
        public LevelCompleteManager(PlayerView characterView, List<LevelObjectView> deathZones, List<RestartObjView> restartingObjects,
                                    List<RestartObjView> fullReloadObjects = null)
        {
            characterView.OnLevelObjectContact += OnLevelObjectContact;
            _characterView = characterView;
            _deathZones = deathZones;
            //_winZones = winZones;
            _restartingObjects = restartingObjects;
            _fullReloadObjects = fullReloadObjects;
            //_starTransforms = new List<Transform>();
            if (characterView.TryGetComponent(out _audioSource)) _audioClip = Resources.Load<AudioClip>("death zone");
            _characterView.onGettingHurt += RestartObjects;
        }
        private void OnLevelObjectContact(LevelObjectView contactView)
        {
            if (_deathZones.Contains(contactView))
            {
                _audioSource.PlayOneShot(_audioClip);
                _characterView.onTouchingDeathZone?.Invoke();
                RestartObjects();
            }
            //else if (_winZones.Contains(contactView))
            //{
            //    if (!_starTransforms.Contains(contactView._transform))
            //    {
            //        _starTransforms.Add(contactView._transform);
            //        onRecievingStar?.Invoke(_winZones.IndexOf(contactView), _pointsToWin);
            //        _pointsToWin++;
            //    }

            //}
        }
        private void RestartObjects()
        {
            foreach (var copy in _fullReloadCopies)
            {
                copy.SetActive(false);
            }
            if (_fullReloadObjects != null)
            {
                foreach (RestartObjView view in _fullReloadObjects)
                {
                    RestartObjView prefab = Resources.Load<RestartObjView>($"{view.name}");
                    GameObject newObject = GameObject.Instantiate(prefab.gameObject, view._transform.position, view._transform.rotation);
                    view.gameObject.SetActive(false);
                    newObject.gameObject.SetActive(true);
                    _fullReloadCopies.Add(newObject);
                }
            }

            foreach (var obj in _restartingObjects)
            {
                obj.Restart();
            }
        }
        public void Dispose()
        {
            _characterView.OnLevelObjectContact -= OnLevelObjectContact;
            _characterView.onGettingHurt -= RestartObjects;
        }
    }
}
