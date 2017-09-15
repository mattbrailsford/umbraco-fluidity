---
layout: default
section: API
title: Collections
permalink: /api/collections/index.html
---

A collection is a container for a given entity type and lists all of the entities in your repository in either a tree or list view as well as proiding configuration for how the entity type should be edited.

### Defining a collection

You define a collection by calling one of the `AddCollection` methods on a given `FluidityTreeConfig` or parent `FluidityFolderConfig` instance.

#### AddCollection&lt;TEntityType&gt;(Lambda idFieldExpression, string nameSingular, string namePlural, Lambda collectionConfig = null) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a collection to the given container with the given names and a default icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property.

````csharp
// Example
folderConfig.AddCollection<Person>(p => p.Id, "Person", "People", collectionConfig => {
    ...
});
````

---

#### AddCollection&lt;TEntityType&gt;(Lambda idFieldExpression, string nameSingular, string namePlural, string iconSingular, string iconPlural, Lambda collectionConfig = null) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a collection to the given container with the given names + icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property.

````csharp
// Example
folderConfig.AddCollection<Person>(p => p.Id, "Person", "People", "icon-umb-users", "icon-umb-users", collectionConfig => {
    ...
});
````

### Changing the collection alias
{: .mt}

#### SetAlias(string alias) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the alias of the collection.  

**Optional:** When creating a new collection, an alias is automatically generated from the supplied name for you, however you can use the `SetAlias` method to override this should you need a specific alias.

````csharp
// Example
collectionConfig.SetAlias("person");
````

### Changing the collection icon color
{: .mt}

#### SetIconColor(string color) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the collection icon color to the given color.  Possible options are `black`, `green`, `yellow`, `orange`, `blue` or `red`.

````csharp
// Example
collectionConfig.SetIconColor("blue");
````