namespace CycleEngine.Definitions
{
    public class ProjectConfig
    {
        public AssetsFolderConfig Assets { get; set; } = new();
    }

    public class AssetsFolderConfig
    {
        public string Audio { get; set; } = "/audios";
        public string Image { get; set; } = "/images";
        public string Music { get; set; } = "/musics";
        public string Script { get; set; } = "/scripts";
        public string Video { get; set; } = "/videos";
        public string Background { get; set; } = "/backgrounds";
        public string Save { get; set; } = "/saves";
    }
}