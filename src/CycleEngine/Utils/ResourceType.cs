using System.Collections.Generic;

namespace CycleEngine.Utils
{
    public enum ResourceType
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

    public static class ResourceRef
    {
        public static readonly Dictionary<string, ResourceType> Types = new Dictionary<string, ResourceType>()
        {
            { "audio", ResourceType.Audio },
            { "background", ResourceType.Background },
            { "image", ResourceType.Image },
            { "music", ResourceType.Music },
            { "video", ResourceType.Video },
            { "engine", ResourceType.CycleEngineScripts },
            { "dialog", ResourceType.CycleDialogScripts },
            { "animation", ResourceType.Animations }
        };
    }
}