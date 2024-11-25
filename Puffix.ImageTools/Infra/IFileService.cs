using System.Collections.Generic;
using System.IO;

namespace Puffix.ImageTools.Infra;

public interface IFileService
{
    bool ExistsFile(string filePath);

    bool ExistsDirectory(string directoryPath);

    IEnumerable<string> ListFiles(string directoryPath);

    IEnumerable<string> ListFiles(string directoryPath, string filePattern);

    string ChangeFileExtension(string fileName, string newExtension);

    string BuildNewFilePath(string filePath, string newDirectoryPath, string newExtension, bool generateUniqueName);

    FileStream CreateFile(string filePath);

    FileStream OpenFile(string filePath, bool write);

    FileStream OpenOrCreateFile(string filePath);

    void DeleteFile(string filePath);
}