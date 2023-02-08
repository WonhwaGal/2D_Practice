using UnityEngine;

namespace PlatformerMVC
{
    public class ContactPooler
    {
        private readonly Collider2D _collider;
        private readonly Collider2D _headCollider;
        private ContactPoint2D[] _contact = new ContactPoint2D[5];
        private ContactPoint2D[] _headContact = new ContactPoint2D[3];
        private int _contactCount = 0;
        private int _headContCount = 0;
        //private float _treshold = 0.2f;
        private float _slideTreshold = 0.5f;
        public bool IsGrounded { get; private set; }
        public bool LeftContact { get; private set; }
        public bool RightContact { get; private set; }
        public bool FormLeftToRight { get; private set; }
        public bool FromRightToLeft { get; private set; }
        public Vector3 Normal { get; private set; }
        public ContactPooler(Collider2D _collider, Collider2D _headCollider)
        {
            this._collider = _collider;
            this._headCollider = _headCollider;
        }
        public void Update()
        {
            IsGrounded = false;
            LeftContact = false;
            RightContact = false;
            FormLeftToRight = false;
            FromRightToLeft = false;
            _contactCount = _collider.GetContacts(_contact);
            _headContCount = _headCollider.GetContacts(_headContact);
            for (int i = 0; i < _contactCount; i++)
            {
                if (_contact[i].normal.y > _slideTreshold)
                {
                    IsGrounded = true;
                    Normal = _contact[i].normal;

                    if (_contact[i].normal.y > 0.8 && _contact[i].normal.x > 0.44f) FormLeftToRight = true;
                    if (_contact[i].normal.y > 0.8 && _contact[i].normal.x < -0.44f) FromRightToLeft = true;
                }
                if (_contact[i].normal.x > _slideTreshold) LeftContact = true;
                if (_contact[i].normal.x < -_slideTreshold) RightContact = true;
            }
            for(int i = 0; i < _headContCount; i++)
            {
                if (_headContact[i].normal.x > _slideTreshold) LeftContact = true;
                if (_headContact[i].normal.x < -_slideTreshold) RightContact = true;
            }
        }
    }
}
