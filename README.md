<h3>Description</h3>
RenameFiles is a c# windows form application that will rename all images/videos in a selected folder to the date it was taken. 
If a file is not an image/video then it will rename that file to no-date-taken-{count}{extension}. 

Supports JPEG, PNG, HEIC, and other common image formats.

<h3>Installation</h3>

### Simple Installation (Recommended)

1. Download `RenameFiles.exe` from the latest release
2. Double-click to run
3. No installation required!

### First Run

- Windows may show "Windows protected your PC" warning
- Click "More info" → "Run anyway"
- This is normal for unsigned applications

### System Requirements

- Windows 10 or later (64-bit)
- No additional software needed (runtime included)

### Building from Source

If you want to build the application yourself:

```bash
# On macOS or Windows
cd RenameFiles
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

The executable will be in `bin/Release/net8.0-windows/win-x64/publish/RenameFiles.exe`

Enjoy the program!
