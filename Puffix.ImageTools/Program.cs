﻿using Microsoft.Extensions.Configuration;
using Puffix.ConsoleLogMagnifier;
using Puffix.ImageTools.Domain;
using Puffix.ImageTools.Infra;
using System;
using System.IO;

ConsoleHelper.WriteInfo("Welcome to the Puffix.ImageTools console.");

// Load configuration.
IoCContainer container;
try
{
    var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
             .Build();

    container = IoCContainer.BuildContainer(configuration);

    ConsoleHelper.WriteVerbose("The configuration is loaded.");
}
catch (Exception error)
{
    ConsoleHelper.WriteError("Error while loading configuration.");
    ConsoleHelper.WriteError(error);

    ConsoleHelper.Write("Press any key to quit.");
    ConsoleHelper.ReadKey();
    return;
}

ConsoleKey consoleKey;
do
{
    ConsoleHelper.Write("Select action :");
    ConsoleHelper.Write("- S to convert a SVG to PNG file");
    ConsoleHelper.Write("- C to change the colors of a PNG image");
    ConsoleHelper.Write("- Q to quit");
    consoleKey = ConsoleHelper.ReadKey();
    
    ConsoleHelper.ClearLastLines(4);

    ConsoleHelper.WriteNewLine(2);

    if (consoleKey == ConsoleKey.S)
    {
        ConsoleHelper.WriteInfo("Convert SVG files to PNG");
        ConsoleHelper.WriteNewLine();

        IConvertSvgToPngService convertSvgToPngService = container.Resolve<IConvertSvgToPngService>();

        convertSvgToPngService.ConvertSvgToPng();
        ConsoleHelper.WriteNewLine(2);
    }
    else if (consoleKey == ConsoleKey.C)
    {
        ConsoleHelper.WriteInfo("Change the color of PNG images");

        IChangeImageColorService changeImageColorService = container.Resolve<IChangeImageColorService>();
        ConsoleHelper.WriteNewLine();

        changeImageColorService.ChangePngImageColor();
        ConsoleHelper.WriteNewLine(2);
    }

} while (consoleKey != ConsoleKey.Q);
