using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CycleEngine.Utils;

namespace CycleEngine.Interfaces
{
    /// <summary>
    /// Storage abstraction for a Cycle project.
    /// Provides read/write access to all files within a project folder.
    /// 
    /// The default implementation is <see cref="???"/>
    /// (backed by the local file system), but other implementations are possible
    /// (e.g. in-memory storage for unit tests, ZIP archive, etc.).
    /// 
    /// Path conventions: paths are relative to the project root, use forward, started with slashes
    /// slashes '/' as separators, and are case-sensitive.
    /// Examples: "/cycleproject.toml", "/scripts/engine/prologue_00.ces", "/images/paul.png"
    /// </summary>
    public interface ICycleProjectStorage
    {
        /// <summary>
        /// Absolute path to the project root. Some backends need this to access
        /// files directly (e.g. Unity's UnityWebRequest loading images via file:// URIs).
        /// </summary>
        string RootPath { get; }
        
        /// <summary>
        /// Checks whether a file exists at the given relative path.
        /// </summary>
        bool Exists(string relativePath);
        
        /// <summary>
        /// Reads the entire contents of a text file as UTF-8.
        /// </summary>
        Task<string> ReadTextAsync(string relativePath, CancellationToken ct = default);
        
        /// <summary>
        /// Reads the entire contents of a text file as UTF-8.
        /// </summary>
        string ReadText(string relativePath);
        
        /// <summary>
        /// Reads the entire contents of a binary file.
        /// Suitable for small files (configs, text). For large files,
        /// prefer <see cref="OpenReadAsync"/> for streaming.
        /// </summary>
        /// <exception cref="CycleResourceNotFoundException">The file does not exist.</exception>
        Task<byte[]> ReadBytesAsync(string relativePath, CancellationToken ct = default);
        
        /// <summary>
        /// Reads the entire contents of a binary file.
        /// Suitable for small files (configs, text). For large files,
        /// prefer <see cref="OpenReadAsync"/> for streaming.
        /// </summary>
        /// <exception cref="CycleResourceNotFoundException">The file does not exist.</exception>
        byte[] ReadBytes(string relativePath, CancellationToken ct = default);
        
        /// <summary>
        /// Opens a file as a stream. Suitable for loading large files such as
        /// images and audio. The caller is responsible for disposing the stream.
        /// </summary>
        /// <exception cref="CycleResourceNotFoundException">The file does not exist.</exception>
        Task<Stream> OpenReadAsync(string relativePath, CancellationToken ct = default);
        
        /// <summary>
        /// Writes a text file as UTF-8. Parent directories are created as needed.
        /// </summary>
        Task WriteTextAsync(string relativePath, string content, CancellationToken ct = default);
        
        /// <summary>
        /// Writes a binary file. Parent directories are created as needed.
        /// </summary>
        Task WriteBytesAsync(string relativePath, byte[] content, CancellationToken ct = default);
        
        /// <summary>
        /// Deletes a file. No-op if the file does not exist.
        /// </summary>
        void Delete(string relativePath);
        
        /// <summary>
        /// Lists files within a directory.
        /// </summary>
        /// <param name="relativeDirectory">
        /// Relative directory path. Empty string refers to the project root.
        /// </param>
        /// <param name="searchPattern">
        /// Glob pattern such as "*.toml". Null or empty matches all files.
        /// </param>
        /// <param name="recursive">Whether to recurse into subdirectories.</param>
        /// <returns>
        /// Relative paths of matching files (relative to the project root).
        /// </returns>
        IEnumerable<string> ListFiles(
            string relativeDirectory = "",
            string? searchPattern = null,
            bool recursive = false);
        
        /// <summary>
        /// Resolves a relative path to an absolute path.
        /// Mainly used by backends that need direct file access
        /// (e.g. a Unity backend loading images via UnityWebRequest).
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The path attempts to escape the project root (path traversal).
        /// </exception>
        string ResolveAbsolutePath(string relativePath);
    }
}