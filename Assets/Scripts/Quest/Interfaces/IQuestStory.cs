using System;

namespace PlatformerMVC
{
    public interface IQuestStory : IDisposable
    {
        bool IsDone { get; }
    }
}
