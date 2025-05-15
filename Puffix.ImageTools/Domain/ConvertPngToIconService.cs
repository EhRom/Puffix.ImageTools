using Microsoft.Extensions.Configuration;
using Puffix.ImageTools.Infra;
using System.Drawing;
using System.Drawing.Imaging;

namespace Puffix.ImageTools.Domain;

public class ConvertPngToIconService(IConfiguration configuration, IFileService fileService) : ImageService(configuration, fileService), IConvertPngToIconService
{
    public void ConvertPngToIcon()
    {
        string actionMessage = "Convert a PNG file to Icon";
        string sucessMessage = "The file has been converted from PNG to an Icon";
        string errorMessage = "converting a PNG file to an Icon";

        ProcessImages(PNG_FILE_EXTENSION, ICO_FILE_EXTENSION, actionMessage, sucessMessage, errorMessage);
    }

    protected override void ProcessImage(string inImagePath, string outImagePath)
    {
        using Bitmap bitmap = new Bitmap(inImagePath);
        bitmap.Save(outImagePath, ImageFormat.Icon);
    }
}
