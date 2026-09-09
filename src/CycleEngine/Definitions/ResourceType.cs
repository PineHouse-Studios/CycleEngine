using System.Collections.Generic;

namespace CycleEngine.Definitions
{
    public enum ResourceType
    {
        Audio,
        Background,
        Image,
        Music,
        Video,
        Config,
        CycleEngineScript,
        CycleDialogScript,
        Animations,
        Undefine
    }

    public static class ResourceRef
    {
        private static readonly Dictionary<string, ResourceType> Types = new()
        {
            { "audio", ResourceType.Audio },
            { "background", ResourceType.Background },
            { "image", ResourceType.Image },
            { "music", ResourceType.Music },
            { "video", ResourceType.Video },
            { "config", ResourceType.Config},
            { "engine", ResourceType.CycleEngineScript },
            { "dialog", ResourceType.CycleDialogScript },
            { "animation", ResourceType.Animations }
        };

        public static ResourceType GetType(string key)
        {
            return Types.TryGetValue(key, out ResourceType type) ? type : ResourceType.Undefine;
        }
    }
}