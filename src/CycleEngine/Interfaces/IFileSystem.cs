using System.IO;

namespace CycleEngine.Interfaces
{
    public interface IFileSystem
    {
        public bool Exist(string filePath);
        public Stream Read(string filePath);
        public void Write(string filePath, string content);
    }
}