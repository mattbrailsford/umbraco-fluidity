---
layout: default
title: Overview
permalink: /overview/installation/index.html
---

## Installation

Fluidity can be installed from either Our Umbraco, the NuGet package repository or by downloading and compiling the source code manually.

### Our Umbraco

To install from Our Umbraco, you can either download the package file directly from the Our website and upload it into the package installer manually, or you can search for and install it automatically via the package gallery found in your Umbraco install.

To in install the package manually, you can download the package file from the following location and then upload it to the package installer when prompted.

[URL]

[SCREENSHOT - Package Installer]

To install from the package gallery, simply search for “Fluidity” in the package galleries search field, click the Fluidity search result and then click the “Install Package” button.

[SCREENSHOT - Package Gallery Search]
[SCREENSHOT - Package Gallery Install]

To complete the installation, follow the on screen instructions presented by the package installer. 

### NuGet Package Repository

To install from NuGet you can run the following command from within the Visual Studio Package Manager Console window.

````bash
PM> Install-Package Our.Umbraco.Fluidity
````

Alternatively you can search for “Our.Umbraco.Fluidity” within the NuGet Package Manager interface.

[SCREENSHOT - Nuget package manager search]

### Install from Source

If you’d prefer to compile Fluidity yourself you can do so by cloning the Fluidity GitHub repository and running the automated build script like so:

````bash
git clone https://github.com/mattbrailsford/umbraco-fluidity.git umbraco-fluidity
cd umbraco-fluidity
.\build.cmd
````