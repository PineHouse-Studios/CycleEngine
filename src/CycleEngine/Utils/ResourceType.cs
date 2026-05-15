using System.Collections.Generic;

namespace CycleEngine.Utils
{
    public enum Resource
    {
        Audio,
        Background,
        Image,
        Music,
        Video,
        CycleEngineScripts,
        CycleDialogScripts,
        Animations,
        Undefine
    }

    public static class ResourceType
    {
        public static readonly Dictionary<string, Resource> Resources = new Dictionary<string, Resource>()
        {
            { "audio", Resource.Audio },
            { "background", Resource.Background },
            { "image", Resource.Image },
            { "music", Resource.Music },
            { "video", Resource.Video },
            { "engine", Resource.CycleEngineScripts },
            { "dialog", Resource.CycleDialogScripts },
            { "animation", Resource.Animations }
        };
    }
}