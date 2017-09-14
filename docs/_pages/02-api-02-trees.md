---
layout: default
section: API
title: Trees
permalink: /api/trees/index.html
---

A tree is a hierarchical structure to help organise a section into logical sub-sections and is accessed in the main side panel of the Umbraco interface. In Fluidity, a section may only have a single tree definition, however you can use folder nodes to help organise the tree structure how you need it.

### Defining a Tree

You define a tree for a section by calling one of the SetTree methods on the given SectionConfig object.

#### SetTree(string name, Lambda treeConfig = null) *: FluidityTreeConfig*
{: .signature}

Sets the tree to display in the Umbraco side panel for the current section with the given name. 

````csharp
// Example
sectionConfig.SetTree("Database", treeConfig => {
    ...
});
````

### Configuration Options

#### SetAlias(string alias) *: FluidityTreeConfig*
{: .signature}

Sets the alias of the tree.  

**Optional:** When creating a new tree, an alias is automatically generated from the supplied name for you, however you can use the SetAlias method to override this should you need a specific alias.

````csharp
// Example
treeConfig.SetAlias("database");
````

---

#### AddFolder(string name, Lambda folderConfig = null) *: FluidityFolderConfig*
{: .signature}

Adds a folder to the current tree with the given name and a default folder icon. See the Folder section for more info.

````csharp
// Example
treeConfig.AddFolder("Settings", folderConfig => {
    ...
});
````

---

#### AddFolder(string name, string icon, Lambda folderConfig = null) : *: FluidityFolderConfig*
{: .signature}

Adds a folder to the current tree with the given name + icon. See the Folder section for more info.

````csharp
// Example
treeConfig.AddFolder("Settings", "icon-cog", folderConfig => {
    ...
});
````

---

#### AddCollection<TEntityType>(Lambda idFieldExpression, string nameSingular, string namePlural, Lambda collectionConfig) *: FluidityCollectionConfig<TEntityType>*
{: .signature}

Adds a collection to the current tree with the given names and a default icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property. See the Collections section for more info.

````csharp
// Example
treeConfig.AddCollection<Person>(p => p.Id, "Person", "People", collectionConfig => {
    ...
});
````

---

#### AddCollection<TEntityType>(Lambda idFieldExpression, string nameSingular, string namePlural, string iconSingular, string iconPlural, Lambda collectionConfig) *: FluidityCollectionConfig<TEntityType>*
{: .signature}

Adds a collection to the current tree with the given names + icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property. See the Collections section for more info.

````csharp
// Example
treeConfig.AddCollection<Person>(p => p.Id, "Person", "People", "icon-person", "icon-people", collectionConfig => {
    ...
});
````
