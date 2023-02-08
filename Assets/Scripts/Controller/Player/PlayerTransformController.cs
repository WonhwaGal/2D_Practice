using UnityEngine;

namespace PlatformerMVC
{
    public class PlayerTransformController
    {
        private AnimationConfig _config;
        private SpriteAnimationController _playerAnimator;
        private ContactPooler _contactPooler;
        private LevelObjectView _playerView;

        private Transform _playerT;

        private bool _isJump;
        private bool _isSlide;
        private const float _walkSpeed = 3f;
        private const float _animationSpeed = 10f;
        private const float _movingTreshold = 0.1f;

        private Vector3 _leftScale = new Vector3(-0.8f, 0.8f, 0.8f);
        private Vector3 _rightScale = new Vector3(0.8f, 0.8f, 0.8f);

        private bool _isMoving;

        private const float _jumpForce = 9f;
        private const float _jumpTreshold = 1f;
        private const float _g = -9.8f;
        private float _groundLevel = 1f;
        private float _yVelocity = 0;
        private float _xAxisInput;
        public PlayerTransformController(LevelObjectView player)
        {
            _playerView = player;
            _playerT = player._transform;

            _config = Resources.Load<AnimationConfig>("SpriteAnimCfg");
            _playerAnimator = new SpriteAnimationController(_config);
            _contactPooler = new ContactPooler(_playerView._collider, _playerView._collider);
            _playerAnimator.AddandStartAnimation(player._spriteRenderer, AnimState.Walk, true, _animationSpeed);
        }
        private void MoveTowards()
        {
            _playerT.position += Vector3.right * (Time.deltaTime * _walkSpeed * (_xAxisInput > 0 ? 1 : -1));
            _playerT.localScale = _xAxisInput<0? _leftScale : _rightScale;   
        }
        public bool IsGrounded()
        {
            return _playerT.position.y <= _groundLevel && _yVelocity <= 0;
        }
        public void Update()
        {
            _playerAnimator.Update();
            _xAxisInput = Input.GetAxis("Horizontal");
            if (Input.GetAxis("Vertical") > 0)
            {
                _isJump = true;
                _isSlide = false;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                _isSlide = true;
                _isJump = false;
            }
            else
            {
                _isSlide = false;
                _isJump = false;
            }
            _isMoving = Mathf.Abs(_xAxisInput) > _movingTreshold;

            if (_isMoving)
            {
                MoveTowards();
            }

            if (IsGrounded())
            {
                if (_isSlide)
                {
                    _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Slide, true, _animationSpeed);
                }
                else
                {
                    _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, _isMoving ? AnimState.Walk : AnimState.Idle, true, _animationSpeed);
                }
                if (!_isSlide && _isJump && _yVelocity <= _jumpTreshold)
                {
                    _yVelocity = _jumpForce;
                }
                else if (_yVelocity < 0)
                {
                    _yVelocity = 0;
                    _playerT.position = new Vector3(_playerT.position.x, _groundLevel, _playerT.position.z);
                }
            }
            else
            {
                if (Mathf.Abs(_yVelocity) > _jumpTreshold)
                {
                    _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Jump, true, _animationSpeed);
                }
                _yVelocity += _g * Time.deltaTime;
                _playerT.position +=Vector3.up * (Time.deltaTime * _yVelocity);
            }
        }
    }
}
