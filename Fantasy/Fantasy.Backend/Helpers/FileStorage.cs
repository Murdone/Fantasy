using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Fantasy.Backend.Helpers
{
    public class FileStorage : IFileStorage
    {
        private readonly string _connectionString;

        public FileStorage(IConfiguration configuration)
        {
            // Obtiene la cadena de conexión para Azure Storage desde la configuración
            _connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        public async Task RemoveFileAsync(string path, string containerName)
        {
            // Crea un cliente para el contenedor de blobs
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync(); // Asegura que el contenedor exista
            var fileName = Path.GetFileName(path); // Obtiene el nombre del archivo del path
            var blob = client.GetBlobClient(fileName); // Obtiene el cliente del blob
            await blob.DeleteIfExistsAsync(); // Elimina el blob si existe
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
        {
            // Crea un cliente para el contenedor de blobs
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync(); // Asegura que el contenedor exista
            client.SetAccessPolicy(PublicAccessType.Blob); // Establece la política de acceso
            var fileName = $"{Guid.NewGuid()}{extension}"; // Genera un nombre único para el archivo
            var blob = client.GetBlobClient(fileName); // Obtiene el cliente del blob

            using (var ms = new MemoryStream(content)) // Crea un stream de memoria con el contenido
            {
                await blob.UploadAsync(ms); // Sube el contenido al blob
            }

            return blob.Uri.ToString(); // Devuelve la URI del blob
        }

        public async Task<string> EditFileAsync(byte[] content, string extension, string containerName, string path)
        {
            // Elimina el archivo existente, si lo hay
            await RemoveFileAsync(path, containerName);
            // Guarda el nuevo archivo y devuelve su URI
            return await SaveFileAsync(content, extension, containerName);
        }
    }
}