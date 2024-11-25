using Microsoft.Extensions.Configuration;
using Puffix.ImageTools.Infra;
using SkiaSharp;
using Svg.Skia;
using System.IO;

namespace Puffix.ImageTools.Domain;

public class ConvertSvgToPngService(IConfiguration configuration, IFileService fileService) : ImageService(configuration, fileService), IConvertSvgToPngService
{
    public void ConvertSvgToPng()
    {
        string actionMessage = "Convert a SVG file to PNG image";
        string sucessMessage = "The file has been converted from SVG to PNG";
        string errorMessage = "converting a SVG file to a PNG image file";

        ProcessImages(SVG_FILE_EXTENSION, actionMessage, sucessMessage, errorMessage);
    }

    protected override void ProcessImage(string inImagePath, string outImagePath)
    {
        using SKSvg svg = new SKSvg();

        if (svg.Load(inImagePath) is { })
        {
            using FileStream fileStream = fileService.OpenOrCreateFile(outImagePath);

            svg.Save(fileStream, SKColors.Empty, SKEncodedImageFormat.Png, 100, 32, 32);
        }
    }
}
