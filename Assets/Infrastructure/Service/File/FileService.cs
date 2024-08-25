using System;
using System.Collections.Generic;

namespace Infrastructure.Service.File
{
    public class FileService : IFileService
    {
        private readonly Dictionary<Type, IFileManager> _fileManagers = new();

        public FileService()
        {
            _fileManagers.Add(typeof(string), new StringFileManager());
            _fileManagers.Add(typeof(byte[]), new ByteArrayFileManager());
        }

        public T Load<T>(string filePath)
        {
            var fileManager = GetFileManager<T>();
            var result = fileManager.Load(filePath);
            var convertedResult = (T)result;
            return convertedResult;
        }

        public void Save<T>(string filePath, object fileContent)
        {
            var fileManager = GetFileManager<T>();
            fileManager.Save(filePath, fileContent);
        }

        private IFileManager GetFileManager<T>()
        {
            var type = typeof(T);

            if (!_fileManagers.TryGetValue(type, out var fileManager))
            {
                throw new ArgumentNullException($"FileService: there is no file manager for type: {type}!");
            }

            return fileManager;
        }
    }
}