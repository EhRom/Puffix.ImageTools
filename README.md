# Puffix Image tools

**Puffix Image tools** is a simple console app used to process image files.

Two actions are available:

- Convert SVG images to PNG image files
- Change the foregound color of an image to a single color, and eventually the background color.

The settings of this console application are available in the `appSettings.json` file:

|Parameter|Type|Description|
|---|:---:|---|
|**inputDirectoryPath**|`string`|The source directory from where to process the files. E.g.: `.\\Files\\Input`|
|**outputDirectoryPath**|`string`|The destination directory for the processed files. E.g. `.\\Files\\Output`|
|**overwriteFiles**|`boolean`|Indicate whether to overwrite existing output files. E.g. `true`|
|**generateUniqueFileNames**|`boolean`|Indicate whether to create a unique file name. E.g. `true`|
|**foregroundColor**|`string` / color scheme|Color of the unique foreground color. E.g. `#101923`|
|**backgroundColor**|`string` / color scheme|Color of the background color. E.g. `#FF4500`|
