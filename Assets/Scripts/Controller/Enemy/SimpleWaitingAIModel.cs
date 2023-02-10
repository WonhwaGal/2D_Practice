using UnityEngine;

namespace PlatformerMVC
{
    public sealed class SimpleWaitingAIModel
    {
        #region Fields
        private readonly Transform _fromPos;
        private readonly Transform _target;
        private Vector2 direction;
        private readonly AIConfig _config;
        private Vector3 _yShift = new Vector3(0, -1, 0);
        private readonly LayerMask _layerMask;
        #endregion

        public SimpleWaitingAIModel(Transform fromPos, Transform target, AIConfig config, LayerMask layerMask)
        {
            _fromPos = fromPos;
            _target = target;
            _config = config;
            _layerMask = layerMask;
        }
        public Vector2 GoToTarget()
        {
            float disToTarget = Vector2.SqrMagnitude(_target.position - _fromPos.position);

            if (disToTarget < _config.minSqrDistanceToTarget && disToTarget >1.0f)
            {
                if (CheckIfGrounded()) direction = ((Vector2)(_target.position - _fromPos.position)).normalized;
                else direction = Vector2.zero;
            }
            else
            {
                direction = Vector2.zero;
            }
            return _config.speed * direction;
        }
        private bool CheckIfGrounded()
        {
            Vector3 _rayDir = (_target.position - _fromPos.position).normalized + _yShift;
            bool _isGrounded = Physics2D.Raycast(_fromPos.position, _rayDir, 2.0f, _layerMask);
            return _isGrounded;
        }
    }
}
