using System;
using UnityEngine;

namespace PlatformerMVC
{
    public sealed class SimplePatrolAI
    {
        #region Fields
        private readonly KidsView _view;
        private readonly SimplePatrolAIModel _model;
        private readonly SimpleWaitingAIModel _waitingModel;
        private bool _patrol = false;
        #endregion

        #region Class life cycles
        public SimplePatrolAI(KidsView view, SimplePatrolAIModel model)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _model = model != null ? model : throw new ArgumentNullException(nameof(model));
            _patrol = true;
        }
        public SimplePatrolAI(KidsView view, SimpleWaitingAIModel waitingModel)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _waitingModel = waitingModel != null ? waitingModel : throw new ArgumentNullException(nameof(waitingModel));
        }
        public void FixedUpdate()
        {
            if (_view._shouldAct)
            {
                if (_patrol)
                {
                    var newVelocity = _model.CalculateVelocity(_view._transform.position) * Time.fixedDeltaTime;
                    _view._rb.velocity = newVelocity;
                }
                else
                {
                    var newVelocity = _waitingModel.GoToTarget() * Time.fixedDeltaTime;
                    _view._rb.velocity = newVelocity;
                }
            }
        }
        #endregion
    }

}
