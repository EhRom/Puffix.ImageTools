# Puffix Image tools

**Puffix Image tools** is a simple console app used to process image files.

[![Build and package](https://github.com/EhRom/Puffix.ImageTools/actions/workflows/build-and-package.yml/badge.svg)](https://github.com/EhRom/Puffix.ImageTools/actions/workflows/build-and-package.yml)

Two actions are available:

- Convert SVG images to PNG image files
- Change the foregound color of an image to a **single color**, and eventually the background color.
- Convert PNG images to Icons
 
The settings of this console application are available in the `appSettings.json` file:

|Parameter|Type|Description|
|---|:---:|---|
|**inputDirectoryPath**|`string`|The source directory from where to process the files. E.g.: `.\\Files\\Input`|
|**outputDirectoryPath**|`string`|The destination directory for the processed files. E.g. `.\\Files\\Output`|
|**overwriteFiles**|`boolean`|Indicate whether to overwrite existing output files. E.g. `true`|
|**generateUniqueFileNames**|`boolean`|Indicate whether to create a unique file name. E.g. `true`|
|**foregroundColor**|`string` / color scheme|Color of the unique foreground color. E.g. `#101923`|
|**backgroundColor**|`string` / color scheme|Color of the background color. E.g. `#FF3500`|
