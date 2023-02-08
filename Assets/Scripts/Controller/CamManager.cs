using UnityEngine;

namespace PlatformerMVC
{
    public class CamManager
    {
        private readonly float _moveSpeed;
        private readonly Transform _player;
        private Transform _mainCamT;
        private Vector3 _startPos;
        private Vector3 _newPos;
        Vector3 moveVector;

        private float currentYPos;
        private readonly float _YMax;
        private readonly float _YMin;
        
        public float _xShift;
        public float _yShift;
        private readonly float _changeShiftPoint;
        private const float _raceLength = 40.0f;
        private readonly float _shiftLength;
        private readonly float _endPos;
        private readonly float _startShift;
        private const float _shiftInEnd = -6.0f;

        public CamManager(PlayerView _playerView, float xShift, float yShift, float endPos, float YMax, float YMin, float speed)
        {
            _player = _playerView._transform;
            _xShift = xShift;
            _yShift = yShift;
            _endPos = endPos;
            _changeShiftPoint = _endPos - _raceLength;
            _startShift = _xShift;
            _shiftLength = _startShift - _shiftInEnd;
            _mainCamT = Camera.main.transform;
            _startPos = _mainCamT.position;
            _moveSpeed = speed;
            _YMax = YMax;
            _YMin = YMin;
        }

        public void Update()
        {
            if (_player.position.x > _changeShiftPoint)
            {
                CalculateShift();
            }
            else
            {
                _xShift = _startShift;
            }
            if (_player.position.y < _YMax && _player.position.y > _YMin)
            {
                currentYPos = _player.position.y;
            }
            else if (_player.position.y > _YMax)
            {
                currentYPos = _YMax;
            }
            else if (_player.position.y < _YMin)
            {
                currentYPos = _YMin;
            }

            _newPos = new Vector3(_player.position.x + _xShift, currentYPos + _yShift, _startPos.z);
            moveVector = Vector3.Lerp(_mainCamT.position, _newPos, _moveSpeed * Time.deltaTime);
            _mainCamT.position = moveVector;
        }
        private void CalculateShift()
        {
            float middleRes = (_player.position.x - _changeShiftPoint) / _raceLength;
            _xShift = _startShift - (_shiftLength * middleRes);
        }
    }
}
