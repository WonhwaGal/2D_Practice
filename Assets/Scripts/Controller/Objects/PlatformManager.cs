using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public sealed class PlatformManager
    {
        private List<PlatformView> _platformViews;
        public PlatformManager(List<PlatformView> _platformViews)
        {
            this._platformViews = _platformViews;
            foreach (PlatformView view in _platformViews)
            {
                view._transform.position = view.points[0].position;
            }
        }
        public void Update()
        {
            foreach (var view in _platformViews)
            {
                if (Vector2.Distance(view._transform.position, view.points[view._index].position) < 0.02f)
                {
                    view._index++;
                    if (view._index == view.points.Length)
                    {
                        view._index = 0;
                    }
                }
                view._transform.position = Vector2.MoveTowards(view._transform.position, view.points[view._index].position, Time.deltaTime * view._speed);
            }
        }
    }
}
