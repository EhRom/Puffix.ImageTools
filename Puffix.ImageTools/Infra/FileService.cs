using System;
using System.Collections.Generic;
using System.IO;

namespace Puffix.ImageTools.Infra;

public class FileService : IFileService
{
    public bool ExistsFile(string filePath)
    {
        return File.Exists(filePath);
    }

    public bool ExistsDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }

    public IEnumerable<string> ListFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"The directory {directoryPath} does not exist.");

        return Directory.EnumerateFiles(directoryPath);
    }

    public IEnumerable<string> ListFiles(string directoryPath, string filePattern)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"The directory {directoryPath} does not exist.");

        return Directory.EnumerateFiles(directoryPath, filePattern);
    }

    public string ChangeFileExtension(string fileName, string newExtension)
    {
        string fileNameWithoutExtention = Path.GetFileNameWithoutExtension(fileName);
        newExtension = string.IsNullOrEmpty(newExtension) || newExtension.StartsWith('.') ?
                            newExtension : $".{newExtension}";

        fileName = $"{fileNameWithoutExtention}{newExtension}";
        return fileName;
    }

    public string BuildNewFilePath(string filePath, string newDirectoryPath, string newExtension, bool generateUniqueName)
    {
        string fileName = ChangeFileExtension(Path.GetFileName(filePath), newExtension);

        fileName = generateUniqueName ? GenerateUniqueFileName(fileName) : fileName;

        return Path.Combine(newDirectoryPath, fileName);
    }

    private string GenerateUniqueFileName(string fileName)
    {
        string baseFileName = Path.GetFileNameWithoutExtension(fileName);
        string extension = Path.GetExtension(fileName).TrimStart('.');

        string dateString = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss");
        string guidString = Guid.NewGuid().ToString("d");

        return $"{baseFileName}-{dateString}-{guidString}.{extension}";
    }

    public FileStream CreateFile(string filePath)
    {
        string directoryPath = Path.GetDirectoryName(filePath);

        if (!ExistsDirectory(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return File.Create(filePath);
    }

    public FileStream OpenFile(string filePath, bool write)
    {
        return write ? File.OpenWrite(filePath) : File.OpenRead(filePath);
    }

    public FileStream OpenOrCreateFile(string filePath)
    {
        return ExistsFile(filePath) ?
                    OpenFile(filePath, true) :
                    CreateFile(filePath);
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
