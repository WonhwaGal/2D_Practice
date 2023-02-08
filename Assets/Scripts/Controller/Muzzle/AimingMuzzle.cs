using UnityEngine;

namespace PlatformerMVC
{
    public class AimingMuzzle
    {
        private Transform _muzzleTransform;
        private Transform _aimTransform;
        private Vector3 downShift = new Vector3(0,-5,0);
        public AimingMuzzle(Transform muzzleTransform, Transform aimTransform)
        {
            _muzzleTransform = muzzleTransform;
            _aimTransform = aimTransform;
        }
        public void Update()
        {
            var dir = (_aimTransform.position + downShift) - _muzzleTransform.position;
            var angle = Vector3.Angle(Vector3.down, dir);
            if (Mathf.Abs(angle) <= 45)
            {
                var axis = Vector3.Cross(Vector3.down, dir);
                _muzzleTransform.rotation = Quaternion.AngleAxis(angle, axis);
                _muzzleTransform.rotation = Quaternion.RotateTowards(_muzzleTransform.rotation,
                                                                     _muzzleTransform.rotation,
                                                                     Vector3.Dot(Vector3.down, dir.normalized));
            }
            else
            {
                _muzzleTransform.rotation = Quaternion.RotateTowards(_muzzleTransform.rotation,
                    Quaternion.identity,
                    Vector3.Dot(Vector3.down, dir.normalized));
            }

        }
    }

}
