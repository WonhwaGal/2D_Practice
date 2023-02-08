using System;
using System.Collections.Generic;

namespace PlatformerMVC
{
    public sealed class FlagManager: IDisposable
    {
        private readonly PlayerView _characterView;
        private readonly List<LevelObjectView> _flags;
        public FlagManager(PlayerView characterView, List<LevelObjectView> flags)
        {
            _characterView = characterView;
            _flags = flags;
            characterView.OnLevelObjectContact += OnLevelObjectContact;
        }
        private void OnLevelObjectContact(LevelObjectView contactView)
        {
            if (_flags.Contains(contactView))
            {
                _characterView.StartPos = contactView._transform.position;
                contactView.gameObject.SetActive(false);
            }
        }
        public void Dispose()
        {
            _characterView.OnLevelObjectContact -= OnLevelObjectContact;
        }
    }
}
