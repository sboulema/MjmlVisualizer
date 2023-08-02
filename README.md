# MJML Visualizer
A Visual Studio extension that adds a debug visualizer to easily see MJML, HTML source and rendered HTML.

[![Build Status](https://github.com/sboulema/MjmlVisualizer/actions/workflows/workflow.yml/badge.svg)](https://github.com/sboulema/MjmlVisualizer/actions/workflows/workflow.yml)
[![Sponsor](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/sboulema)

## Features
- View MJML
- View HTML
- View rendered HTML
- Optional word wrap

## Support
- Visual Studio 2022

## Installing
[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=SamirBoulema.MjmlVisualizer) [![Visual Studio Marketplace](https://img.shields.io/vscode-marketplace/v/SamirBoulema.MjmlVisualizer.svg?style=flat)](https://marketplace.visualstudio.com/items?itemName=SamirBoulema.MjmlVisualizer)

[GitHub Releases](https://github.com/sboulema/MjmlVisualizer/releases)

[Open VSIX Gallery](https://www.vsixgallery.com/extension/MjmlVisualizer.771344e3-c441-46ee-88bd-2295144a2ef8)

You can easily install the MJML Visualizer by using this Visual Studio extension,
but you can also manually install the MJML Visualizer by copping the `MjmlVisualizer.dll` to 
the following locations:
- `<VisualStudioInstallPath>\Common7\Packages\Debugger\Visualizers`
- `My Documents\<VisualStudioVersion>\Visualizers`

## Uninstalling
Uninstalling the extension will unfortunately not uninstall the MJML Visualizer. Please manually delete the following files:
- `<VisualStudioInstallPath>\Common7\Packages\Debugger\Visualizers\MjmlVisualizer.dll`
- `My Documents\<VisualStudioVersion>\Visualizers\MjmlVisualizer.dll`

## Screenshots
[![Screenshot](https://raw.githubusercontent.com/sboulema/MjmlVisualizer/main/art/Screenshot.png)](https://raw.githubusercontent.com/sboulema/MjmlVisualizer/main/art/Screenshot.png)

## Links
[Displaying html from string in WPF WebBrowser control](https://stackoverflow.com/questions/2585782/displaying-html-from-string-in-wpf-webbrowser-control)

[How do you bind the TextWrapping property of a TextBox to the IsChecked value of a MenuItem?](https://stackoverflow.com/questions/250840/how-do-you-bind-the-textwrapping-property-of-a-textbox-to-the-ischecked-value-of)

[Writing a Custom Debugger Visualizer for Visual Studio](https://wrightfully.com/series/debugger-visualizer)

[MJML.io](https://mjml.io/)

[Hex Visualizer](https://bitbucket.org/mmihajlovic/hex-visualizer/src/master/)

[Walkthrough: Writing a Visualizer in C#](https://learn.microsoft.com/en-us/visualstudio/debugger/walkthrough-writing-a-visualizer-in-csharp?view=vs-2022)