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

### Changing the collection connection string
{: .mt}

By default Fluidity will use the Umbraco connection string for it's database connection however you can change this by calling the `SetConnectionString` method on a `FluidityCollectionConfig` instance.

#### SetConnectionString(string connectionStringName) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the connection string name for the given collection repository.

````csharp
// Example
collectionConfig.SetConnectionString("myConnectionStringName");
````

### Defining the entity name
{: .mt}

Within Umbraco it is expected that an entity has a name property so we need to let Fluidity know which property to use for the name or if our entity doesn't have a single name property, then how to construct a name from an entities other properties. We do this by using either the `SetNameProperty` or `SetNameFormat` methods on a `FluidityCollectionConfig` instance.

#### SetNameProperty(Lamda nameProperytyExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets which property of our entity to use as the name property. Property must be of type `string`. By defining a property as the name property, it's value will be used as the label for the entity in things like trees and list views and will also be editable in the header region of the editor interface. The property will also automatically be added to the searchable properties collection.

````csharp
// Example
collectionConfig.SetNameProperty(p => p.Name);
````
---

#### SetNameFormat(Lamda nameFormatExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets a format expression to use to dynamically create a label for the entity in things like trees and list views. By providing a name format it is assumed there is no single name property available on the entity and as such there will be no name input field within the header region of the editor interface and nothing will be added to the searchable properties collection by default.

````csharp
// Example
collectionConfig.SetNameFormat(p => $"{p.FirstName} {p.LastName}");
````