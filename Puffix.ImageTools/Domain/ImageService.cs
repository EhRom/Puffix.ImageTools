using Microsoft.Extensions.Configuration;
using Puffix.ConsoleLogMagnifier;
using Puffix.ImageTools.Infra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Puffix.ImageTools.Domain;

public abstract class ImageService(IConfiguration configuration, IFileService fileService) : IImageService
{
    protected const string SVG_FILE_EXTENSION = "svg";
    protected const string PNG_FILE_EXTENSION = "png";

    private const string BASE_SEARCH_PATTERN = "*.";

    protected readonly IFileService fileService = fileService;

    private readonly Lazy<string> inputDirectoryPathLazy = new(() =>
    {
        return configuration[nameof(inputDirectoryPath)]!;
    });

    private readonly Lazy<string> outputDirectoryPathLazy = new(() =>
    {
        return configuration[nameof(outputDirectoryPath)]!;
    });

    private readonly Lazy<bool> overwriteFilesLazy = new(() =>
    {
        return configuration.GetValue<bool>(nameof(overwriteFiles));
    });

    private readonly Lazy<bool> generateUniqueFileNamesLazy = new(() =>
    {
        return configuration.GetValue<bool>(nameof(generateUniqueFileNames));
    });

    protected string inputDirectoryPath => inputDirectoryPathLazy.Value;
    protected string outputDirectoryPath => outputDirectoryPathLazy.Value;
    protected bool overwriteFiles => overwriteFilesLazy.Value;
    protected bool generateUniqueFileNames => generateUniqueFileNamesLazy.Value;

    protected void ProcessImages(string fileExtension, string actionMessage, string sucessMessage, string errorMessage)
    {
        string searchPattern = $"{BASE_SEARCH_PATTERN}{fileExtension}";

        Stopwatch stopwatch = Stopwatch.StartNew();

        ConsoleHelper.Write($"List the {fileExtension} files in the '{inputDirectoryPath}' directory.");

        IEnumerable<string> filesToConvert = fileService.ListFiles(inputDirectoryPath, searchPattern);

        int processedFiles = 0;
        foreach (string fileToConvert in filesToConvert)
        {
            try
            {
                string outFilePath = fileService.BuildNewFilePath(fileToConvert, outputDirectoryPath, PNG_FILE_EXTENSION, generateUniqueFileNames);

                if (!generateUniqueFileNames && !overwriteFiles && fileService.ExistsFile(outFilePath))
                    throw new Exception($"The file {outFilePath} already exists (set the 'overwriteFiles' option or 'generateUniqueFileNames' option to true to overwrite files, ).");

                ConsoleHelper.WriteVerbose($"{actionMessage}. File to process: '{fileToConvert}'.");
                ConsoleHelper.WriteVerbose($"Change the colors of the '{fileToConvert}' PNG image file. File to process: '{fileToConvert}'.");
                ProcessImage(fileToConvert, outFilePath);

                ConsoleHelper.WriteSuccess($"{sucessMessage}. Processed file path: '{outFilePath}'.");
                processedFiles++;
            }
            catch (Exception error)
            {
                ConsoleHelper.WriteError($"An error occured while . File to process: '{fileToConvert}'", error);
            }
        }

        stopwatch.Stop();

        ConsoleHelper.Write(ConsoleColor.Magenta, $"{processedFiles} files on {filesToConvert.Count()} processed in {stopwatch}.");
    }

    protected abstract void ProcessImage(string inImagePath, string outImagePath);
}
