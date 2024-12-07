using Microsoft.Extensions.Configuration;
using Puffix.ImageTools.Infra;
using SkiaSharp;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Puffix.ImageTools.Domain;

public class ChangeImageColorService(IConfiguration configuration, IFileService fileService) : ImageService(configuration, fileService), IChangeImageColorService
{
    private const string COLOR_REGEX_RED_GROUP_NAME = "red";
    private const string COLOR_REGEX_GREEN_GROUP_NAME = "green";
    private const string COLOR_REGEX_BLUE_GROUP_NAME = "blue";
    private const string COLOR_REGEX_PATTERN = @"(\#)?(?<" + COLOR_REGEX_RED_GROUP_NAME + @">[\dabcdef]{2})(?<" + COLOR_REGEX_GREEN_GROUP_NAME + @">[\dabcdef]{2})(?<" + COLOR_REGEX_BLUE_GROUP_NAME + @">[\dabcdef]{2});?";

    private static readonly Regex colorRegex = new Regex(COLOR_REGEX_PATTERN, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

    private readonly Lazy<SKColor> foregroundColorLazy = new(() =>
    {
        string colorValue = configuration[nameof(foregroundColor)];

        return ExtractColor(colorValue, SKColors.White);
    });

    private readonly Lazy<SKColor> backgroundColorLazy = new(() =>
    {
        string colorValue = configuration[nameof(backgroundColor)];

        return ExtractColor(colorValue, SKColor.Empty);
    });

    private SKColor foregroundColor => foregroundColorLazy.Value;
    private SKColor backgroundColor => backgroundColorLazy.Value;

    public void ChangePngImageColor()
    {
        string actionMessage = $"Change the colors of a PNG image file (main color: {foregroundColor}, backgroundcolor: {backgroundColor})";
        string sucessMessage = "The colors of the PNG image file have been converted";
        string errorMessage = "changing the colors of a PNG image file";

        ProcessImages(PNG_FILE_EXTENSION, actionMessage, sucessMessage, errorMessage);
    }

    protected override void ProcessImage(string inImagePath, string outImagePath)
    {
        using FileStream inFileStream = fileService.OpenFile(inImagePath, false);
        using SKBitmap image = SKBitmap.Decode(inFileStream);

        // Navigate the image Pixel by Pixel
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                SKColor pixelColor = image.GetPixel(x, y);

                pixelColor = pixelColor.WithRed(foregroundColor.Red);
                pixelColor = pixelColor.WithGreen(foregroundColor.Green);
                pixelColor = pixelColor.WithBlue(foregroundColor.Blue);

                image.SetPixel(x, y, pixelColor);
            }
        }

        using SKBitmap finalImage = backgroundColor == SKColor.Empty ? image : SetImageBackground(image, backgroundColor);

        using FileStream outFileStream = fileService.OpenOrCreateFile(outImagePath);

        finalImage.Encode(SKEncodedImageFormat.Png, 100)
             .SaveTo(outFileStream);
    }

    private static SKBitmap SetImageBackground(SKBitmap image, SKColor backgroundColor)
    {
        SKImageInfo info = new SKImageInfo(image.Width, image.Height);

        using SKSurface surface = SKSurface.Create(info);
        using SKCanvas canvas = surface.Canvas;
        canvas.Clear(backgroundColor);

        // create a paint object so that drawing can happen at a higher resolution
        using SKPaint paint = new SKPaint
        {
            IsAntialias = true,
        };
        
        // draw the source bitmap over the white
        canvas.DrawBitmap(image, info.Rect, paint);

        // create an image for saving/drawing
        canvas.Flush();

        using SKImage finalImage = surface.Snapshot();

        return SKBitmap.FromImage(finalImage);
    }

    private static SKColor ExtractColor(string colorValue, SKColor defaultColor)
    {
        return !string.IsNullOrWhiteSpace(colorValue) && colorRegex.IsMatch(colorValue) ?
                SKColor.Parse(colorValue) :
                defaultColor;
    }
}
