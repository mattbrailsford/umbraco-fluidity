---
layout: default
section: API
title: Folders
permalink: /api/folders/index.html
---

A folder appears in a tree and is used to help organise the tree structure by grouping things together. Folders can be added to a tree instance or any other folder instance allowing you to create a hierarchical structure of limitless depth and can contain either other folders or [collections]({{ site.baseurl }}/api/collections/). 

### Defining a folder

You define a folder by calling one of the `AddFolder` methods on a given `FluidityTreeConfig` or parent `FluidityFolderConfig` instance.

#### AddFolder(string name, Lambda folderConfig = null) *: FluidityFolderConfig*
{: .signature}

Adds a folder to the current tree with the given name and a default folder icon.

````csharp
// Example
treeConfig.AddFolder("Settings", folderConfig => {
    ...
});
````

---

#### AddFolder(string name, string icon, Lambda folderConfig = null) *: FluidityFolderConfig*
{: .signature}

Adds a folder to the current tree with the given name + icon.

````csharp
// Example
treeConfig.AddFolder("Settings", "icon-settings", folderConfig => {
    ...
});
````

### Changing the folder alias
{: .mt}

#### SetAlias(string alias) *: FluidityFolderConfig*
{: .signature}

Sets the alias of the folder.  

**Optional:** When creating a new folder, an alias is automatically generated from the supplied name for you, however you can use the `SetAlias` method to override this should you need a specific alias.

````csharp
// Example
folderConfig.SetAlias("settings");
````

### Changing the folder icon color
{: .mt}

#### SetIconColor(string color) *: FluidityFolderConfig*
{: .signature}

Sets the folder icon color to the given color.  Possible options are `black`, `green`, `yellow`, `orange`, `blue` or `red`.

````csharp
// Example
folderConfig.SetIconColor("blue");
````

### Adding a child folder to a folder
{: .mt}

#### AddFolder(string name, Lambda folderConfig = null) *: FluidityFolderConfig*
{: .signature}

Adds a child folder to the current folder with the given name and a default folder icon.

````csharp
// Example
folderConfig.AddFolder("Categories", childFolderConfig => {
    ...
});
````

---

#### AddFolder(string name, string icon, Lambda folderConfig = null) *: FluidityFolderConfig*
{: .signature}

Adds a child folder to the current folder with the given name + icon.

````csharp
// Example
folderConfig.AddFolder("Categories", "icon-tags", childFolderConfig => {
    ...
});
````

### Adding a collection to a folder
{: .mt}

#### AddCollection&lt;TEntityType&gt;(Lambda idFieldExpression, string nameSingular, string namePlural, Lambda collectionConfig = null) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a collection to the current folder with the given names and a default icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property. See the [Collections API documentation]({{ site.baseurl }}/api/collections/) for more info.

````csharp
// Example
folderConfig.AddCollection<Person>(p => p.Id, "Person", "People", collectionConfig => {
    ...
});
````

---

#### AddCollection&lt;TEntityType&gt;(Lambda idFieldExpression, string nameSingular, string namePlural, string iconSingular, string iconPlural, Lambda collectionConfig = null) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a collection to the current folder with the given names + icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property. See the [Collections API documentation]({{ site.baseurl }}/api/collections/) for more info.

````csharp
// Example
folderConfig.AddCollection<Person>(p => p.Id, "Person", "People", "icon-umb-users", "icon-umb-users", collectionConfig => {
    ...
});
````
