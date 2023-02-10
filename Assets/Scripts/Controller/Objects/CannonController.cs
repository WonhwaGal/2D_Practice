using UnityEngine;

namespace PlatformerMVC
{
    public sealed class CannonController
    {
        private readonly Transform _muzzleT;
        private readonly Transform _target;
        private Vector3 _upShift = new Vector3(0, 1.0f, 0);

        private Vector3 _dir;
        private Vector3 _axis;
        private float _angle;

        public CannonController(Transform muzzle, Transform target)
        {
            _muzzleT = muzzle;
            _target = target;
        }
        public void Update()
        {
            _dir = (_target.position + _upShift) - _muzzleT.position;
            _angle = Vector3.Angle(Vector3.down, _dir);
            _axis = Vector3.Cross(Vector3.down, _dir);
            _muzzleT.rotation = Quaternion.AngleAxis(_angle, _axis);
        }
    }
}
