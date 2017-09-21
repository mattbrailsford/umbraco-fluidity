---
layout: default
section: Overview
title: Installation
permalink: /overview/installation/index.html
---

Fluidity can be installed from either Our Umbraco, the NuGet package repository or by downloading and compiling the source code manually.

### Our Umbraco

To install from Our Umbraco, you can either download the package file directly from the Our website and upload it into the package installer manually, or you can search for and install it automatically via the package gallery found in your Umbraco install.

To manually install the package, you can download the package file from the following location and then upload it to the package installer when prompted.

[{{ site.data.links.umbraco }}]({{ site.data.links.umbraco }})

![Upload Package]({{ site.baseurl }}/img/upload-package.png) 

To install from the package gallery, simply search for **Fluidity** in the package galleries search field, click the Fluidity search result and then click the **Install Package** button.

![Package Search]({{ site.baseurl }}/img/package-search.png) 

![Install Package]({{ site.baseurl }}/img/install-package.png) 

To complete the installation, follow the on screen instructions presented by the package installer. 

### NuGet package repository

To install from NuGet you can run the following command from within the Visual Studio Package Manager Console window.

````bash
PM> Install-Package Our.Umbraco.Fluidity
````

Alternatively you can search for **Our.Umbraco.Fluidity** within the NuGet Package Manager interface.

![NuGet Package Manager]({{ site.baseurl }}/img/nuget-package-manager.png) 

### Install from Source

If you’d prefer to compile Fluidity yourself you can do so by cloning the Fluidity GitHub repository and running the automated build script like so:

````bash
git clone https://github.com/mattbrailsford/umbraco-fluidity.git umbraco-fluidity
cd umbraco-fluidity
.\build.cmd
````

Umbraco package files and nuget files can then be found in the `artifacts` directory ready to be installed by one of the methods above.