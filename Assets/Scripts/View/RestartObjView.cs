using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public sealed class RestartObjView : LevelObjectView
    {
        public Vector3 _restartPosition;
        public Quaternion _restartRotation;
        public bool _restartActiveness;
        private List<(Vector3, Quaternion, bool)> _features = new List<(Vector3, Quaternion, bool)> ();
        private List<GameObject> _children = new List<GameObject>();
        private int _childrenCount;

        public int ChildrenCount { get => _childrenCount; set => _childrenCount = value; }
        public List<GameObject> Children { get => _children; set => _children = value; }
        public List<(Vector3, Quaternion, bool)> Features { get => _features; set => _features = value; }

        private void Awake()
        {
            _transform = transform;
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();

            ChildrenCount = transform.childCount;
            if (ChildrenCount != 0)
            {
                for (int i = 0; i < ChildrenCount; i++)
                {
                    Children.Add(transform.GetChild(i).gameObject);

                    Features.Add((transform.GetChild(i).position,
                                  transform.GetChild(i).rotation,
                                  transform.GetChild(i).gameObject.activeInHierarchy));
                }
            }
            else
            {
                _restartPosition = transform.position;
                _restartRotation = transform.rotation;
                _restartActiveness = gameObject.activeInHierarchy;
            }
        }
        public void Restart()
        {
            if (ChildrenCount != 0)
            {
                for (int i = 0; i < ChildrenCount; i++)
                {
                    Children[i].transform.position = Features[i].Item1;
                    Children[i].transform.rotation = Features[i].Item2;
                    Children[i].SetActive(Features[i].Item3);
                    if(Children[i].TryGetComponent<Rigidbody2D>(out Rigidbody2D _rb))
                    {
                        _rb.velocity = Vector3.zero;
                    }
                }
            }
            else
            {
                _transform.position = _restartPosition;
                _transform.rotation = _restartRotation;
                _restartActiveness = gameObject.activeInHierarchy;
                _rb.velocity = Vector3.zero;
            }
        }
    }
}
