using UnityEngine;

namespace PlatformerMVC
{
    public class BullController
    {
        private Vector3 _velocity;
        private BullView _view;

        public BullController(BullView view)
        {
            _view = view;
            Active(false);
        }
        public void Active(bool val)
        {
            _view.gameObject.SetActive(val);
        }
        private void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
            float _angle = Vector3.Angle(Vector3.left, _velocity);
            Vector3 _axis = Vector3.Cross(Vector3.left, _velocity);
            _view._transform.rotation = Quaternion.AngleAxis(_angle, _axis);
        }
        public void Throw(Vector3 position, Vector3 velocity)
        {
            _view._transform.position = position;
            SetVelocity(velocity);
            _view._rb.velocity = Vector2.zero; // обнуление скорости
            _view._rb.angularVelocity = 0;   // the same
            Active(true);

            _view._rb.AddForce(velocity, ForceMode2D.Impulse);
        }
    }
}
