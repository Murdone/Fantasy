namespace Fantasy.Backend.Helpers;

public interface IFileStorage
{
    Task<string> SaveFileAsync(byte[] content, string extension, string containerName);

    Task RemoveFileAsync(string path, string containerName);

    Task<string> EditFileAsync(byte[] content, string extension, string containerName, string path);
}