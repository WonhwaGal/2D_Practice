using System;
using UnityEngine;

namespace PlatformerMVC
{
    public sealed class PlayerRbController : IDisposable
    {
        public Action onLosing;
        private AnimationConfig _config;
        private SpriteAnimationController _playerAnimator;
        private ContactPooler _contactPooler;
        private readonly PlayerView _playerView;

        private Transform _playerT;
        private GameObject _playerHeadCollider;
        private Rigidbody2D _rb;

        private bool _isJump;
        private bool _isSlide;
        private const float _walkSpeed = 180f;
        private const float _animationSpeed = 10f;
        private const float _movingTreshold = 0.1f;

        private Vector3 _leftScale;
        private Vector3 _rightScale;

        private bool _isMoving;

        private const float _jumpForce = 7f;
        private const float _jumpTreshold = 5f;
        private float _yVelocity = 0;
        private float _xVelocity = 0;
        private float _xAxisInput;

        private bool _slidingFromLeft;
        private bool _slidingFromRight;

        private int _health = 50;
        private int _tries = 3;

        public bool _startedJump = false;
        private bool _deadFromFall = false;
        private float _startJumpY;
        private float _endJumpY;
        private float _jumpLength;
        private const float _maxFallTime = 0.82f;

        private AudioSource _audioSource;
        private AudioClip _audioClip;
        public PlayerRbController(PlayerView player)
        {
            _playerView = player;
            _playerT = player._transform;
            _rb = player._rb;
            _playerHeadCollider = player._headCollider.gameObject;

            _leftScale = new Vector3(-_playerView.transform.localScale.x, _playerView.transform.localScale.y, _playerView.transform.localScale.z);
            _rightScale = new Vector3(_playerView.transform.localScale.x, _playerView.transform.localScale.y, _playerView.transform.localScale.z);

            _config = Resources.Load<AnimationConfig>("SpriteAnimCfg");
            _playerAnimator = new SpriteAnimationController(_config);
            _contactPooler = new ContactPooler(_playerView._collider, _playerView._headCollider);
            _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Walk, true, _animationSpeed);

            if (_playerView.TryGetComponent(out _audioSource)) _audioClip = Resources.Load<AudioClip>("fall");

            _playerView.TakeDamage += TakeBullet;
            _playerView.onTouchingDeathZone += DeathZoneTouch;
            _playerView.onGettingPancaked += GetPancaked;
        }
        private void TakeBullet(BullView bullet)
        {
            _health -= bullet.DamagePoint;
            bullet.gameObject.SetActive(false);
        }
        private void MoveTowards()
        {
            _xVelocity = Time.fixedDeltaTime * _walkSpeed * (_xAxisInput > 0 ? 1 : -1);
            _rb.velocity = new Vector2(_xVelocity, _yVelocity);
            _playerT.localScale = _xAxisInput < 0 ? _leftScale : _rightScale;   
        }
        private void DeathZoneTouch()
        {
            _health = 0;
        }
        private void CheckHealth()
        {
            if (_health <= 0 || _playerT.position.y < -5.0f)
            {
                _playerT.position = _playerView.StartPos;
                _health = 30;
                _rb.velocity = Vector3.zero;
                _tries--;
                _deadFromFall = false;
                _startedJump = false;
                if (_tries <= 0)
                {
                    onLosing?.Invoke();
                    Debug.Log("LOST");
                }
                else
                {
                    _playerView.onGettingHurt?.Invoke();
                }
            }
        }
        private void GetPancaked()
        {
            if (!_deadFromFall)
            {
                _audioSource.PlayOneShot(_audioClip);
                _deadFromFall = true;
                _jumpLength = 0.8f;
            }
        }
        public void Update()
        {
            CheckHealth();
            _playerAnimator.Update();
            _contactPooler.Update();
            _xAxisInput = Input.GetAxis("Horizontal");
            _yVelocity = _rb.velocity.y;
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
            if (_deadFromFall)
            {
                _jumpLength -= Time.deltaTime/2;
                if (_jumpLength < 0)
                {
                    _health = 0;
                }
            }

            if (Input.GetAxis("Horizontal") > 0 && _contactPooler.FormLeftToRight)
            {
                _slidingFromLeft = true;
            }
            else if (Input.GetAxis("Horizontal") < 0 && _contactPooler.FromRightToLeft)
            {
                _slidingFromRight = true;
            }
            else
            {
                _slidingFromLeft = false;
                _slidingFromRight = false;
            }

            _isMoving = Mathf.Abs(_xAxisInput) > _movingTreshold;
            if (_playerT.position.y < 0.0f)        // ANIMATION SECTION
            {
                _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Swim, true, _animationSpeed);
                _deadFromFall = false;
                _startedJump = false;
            }
            if (_deadFromFall)
            {
                _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Dead, true, _animationSpeed);
                if (_contactPooler.IsGrounded) _rb.velocity = Vector2.zero;
                else _rb.velocity = Vector2.down * 5;
            }
            else if (_isSlide && _contactPooler.IsGrounded && !_deadFromFall)
            {
                _playerT.up = new Vector3(Mathf.Lerp(_playerT.up.x, _contactPooler.Normal.x, Time.deltaTime*1.5f),         // turn while sliding
                                          Mathf.Lerp(_playerT.up.y, _contactPooler.Normal.y, Time.deltaTime*1.5f), 0.0f);
                _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Slide, true, _animationSpeed);
                _playerHeadCollider.SetActive(false);
            }
            else if (_contactPooler.IsGrounded && !_slidingFromRight && !_slidingFromLeft && !_deadFromFall) // eliminate sliding moments
            {
                _playerHeadCollider.SetActive(true);
                _playerT.up = Vector3.up;        //
                _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, _isMoving ? AnimState.Walk : AnimState.Idle, true, _animationSpeed);
            }
            else if (_contactPooler.IsGrounded && !_deadFromFall && (_slidingFromRight || _slidingFromLeft))    // check for sliding
            {
                _playerT.up = new Vector3(Mathf.Lerp(_playerT.up.x, _contactPooler.Normal.x, Time.deltaTime * 1.5f),
                                          Mathf.Lerp(_playerT.up.y, _contactPooler.Normal.y, Time.deltaTime * 1.5f), 0.0f);
                _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Slide, true, _animationSpeed);
            }

            if (_isMoving && !_deadFromFall)              // MOVEMENT SECTION
            {
                MoveTowards();
            }
            else
            {
                _xVelocity = 0;
                _rb.velocity = new Vector2(_xVelocity, _rb.velocity.y);
            }

            if (_contactPooler.IsGrounded)                                             // jUMPING CONDITIONS
            {
                if (!_isSlide && _isJump && _yVelocity <= _jumpTreshold)
                {
                    _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                }
                if (_startedJump)
                {
                    _startedJump = !_startedJump;
                    _endJumpY = Time.time;
                    _jumpLength = _endJumpY - _startJumpY;
                    if (_jumpLength > _maxFallTime)
                    {
                        _audioSource.PlayOneShot(_audioClip);
                        _deadFromFall = true;
                    }
                }
            }
            else
            {
                if (_contactPooler.LeftContact || _contactPooler.RightContact)          // NOT TO STICK TO VERTICAL SURFACES
                {
                    _rb.velocity = new Vector2(0, -Mathf.Pow(_jumpForce, 1.2f));
                }
                if (Mathf.Abs(_yVelocity) > _jumpTreshold) 
                {
                    _playerAnimator.AddandStartAnimation(_playerView._spriteRenderer, AnimState.Jump, true, _animationSpeed);
                    if (_yVelocity < 0)
                    {
                        if (!_startedJump)
                        {
                            _startedJump = !_startedJump;
                            _startJumpY = Time.time;
                        }
                    }
                }
            }
        }
        public void Dispose()
        {
            _playerView.TakeDamage -= TakeBullet;
            _playerView.onTouchingDeathZone -= DeathZoneTouch;
            _playerView.onGettingPancaked -= GetPancaked;
            _playerAnimator.Dispose();
        }
    }
}
