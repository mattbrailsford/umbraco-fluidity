---
layout: default
section: API
title: Collections
permalink: /api/collections/index.html
---

A collection is a container for a given entity type and configures how the given entity should display in a tree or list view as well as how it should be edited.

### Defining a collection

You define a collection by calling one of the `AddCollection` methods on a given [`FluidityTreeConfig`]({{ site.baseurl }}/api/trees/) or parent [`FluidityFolderConfig`]({{ site.baseurl }}/api/folders/) instance.

#### AddCollection&lt;TEntityType&gt;(Lambda idFieldExpression, string nameSingular, string namePlural, string description, Lambda collectionConfig = null) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a collection to the given container with the given names and description and default icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property.

````csharp
// Example
folderConfig.AddCollection<Person>(p => p.Id, "Person", "People", "A collection of people", collectionConfig => {
    ...
});
````

---

#### AddCollection&lt;TEntityType&gt;(Lambda idFieldExpression, string nameSingular, string namePlural, string description, string iconSingular, string iconPlural, Lambda collectionConfig = null) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a collection to the given container with the given names, description and icons. An ID property accessor expression is required so that Fluidity knows which property is the ID property.

````csharp
// Example
folderConfig.AddCollection<Person>(p => p.Id, "Person", "People", "A collection of people", "icon-umb-users", "icon-umb-users", collectionConfig => {
    ...
});
````

### Changing a collection alias
{: .mt}

#### SetAlias(string alias) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the alias of the collection.  

**Optional:** When creating a new collection, an alias is automatically generated from the supplied name for you, however you can use the `SetAlias` method to override this should you need a specific alias.

````csharp
// Example
collectionConfig.SetAlias("person");
````

### Changing a collection icon color
{: .mt}

#### SetIconColor(string color) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the collection icon color to the given color.  Possible options are `black`, `green`, `yellow`, `orange`, `blue` or `red`.

````csharp
// Example
collectionConfig.SetIconColor("blue");
````

### Changing a collection connection string
{: .mt}

By default Fluidity will use the Umbraco connection string for it's database connection however you can change this by calling the `SetConnectionString` method on a `FluidityCollectionConfig` instance.

#### SetConnectionString(string connectionStringName) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the connection string name for the given collection repository.

````csharp
// Example
collectionConfig.SetConnectionString("myConnectionStringName");
````

### Changing a collection repository implementation
{: .mt}

By default Fluidity will use a PetaPoco based repository for storing and fetching entities however you can implement your own repository should you need to store your entities via another strategy. To change the repository implementation used by a collection you can use the `SetRepositoryType` method. See [Repositories API documentation]({{ site.baseurl }}/api/repositories/) for more info.

#### SetRepositoryType&lt;TRepositoryType&gt;() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the repository type to the given type for the current collection.

````csharp
// Example
collectionConfig.SetRepositoryType<PersonRepositoryType>();
````

---

#### SetRepositoryType(Type repositoryType) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the repository type to the given type for the current collection.

````csharp
// Example
collectionConfig.SetRepositoryType(typeof(PersonRepositoryType));
````

### Defining an entity name
{: .mt}

Within Umbraco it is expected that an entity has a name property so we need to let Fluidity know which property to use for the name or if our entity doesn't have a single name property, then how to construct a name from an entities other properties. We do this by using either the `SetNameProperty` or `SetNameFormat` methods on a `FluidityCollectionConfig` instance.

#### SetNameProperty(Lambda nameProperytyExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets which property of our entity to use as the name property. Property must be of type `string`. By defining a property as the name property, it's value will be used as the label for the entity in things like trees and list views and will also be editable in the header region of the editor interface. The property will also automatically be added to the searchable properties collection and be used for the default sort property.

````csharp
// Example
collectionConfig.SetNameProperty(p => p.Name);
````
---

#### SetNameFormat(Lambda nameFormatExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets a format expression to use to dynamically create a label for the entity in things like trees and list views. By providing a name format it is assumed there is no single name property available on the entity and as such none of the default behaviours descriped for the `SetNameProperty` method will apply.

````csharp
// Example
collectionConfig.SetNameFormat(p => $"{p.FirstName} {p.LastName}");
````

### Defining time stamp properties
{: .mt}

#### SetDateCreatedProperty(Lambda dateCreatedProperty) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets which property of our entity to use as the date created property. Property must be of type `DateTime`. When set and a new entity is saved via the Fluidity repository, then the given field will be populated with the current date and time.

````csharp
// Example
collectionConfig.SetDateCreatedProperty(p => p.DateCreated);
````

---

#### SetDateModifiedProperty(Lambda dateCreatedProperty) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets which property of our entity to use as the date modified property. Property must be of type `DateTime`. When set and an entity is saved via the Fluidity repository, then the given field will be populated with the current date and time.

````csharp
// Example
collectionConfig.SetDateModifiedProperty(p => p.DateModified);
````

### Defining a deleted flag
{: .mt}

By default in Fluidity any entity that is deleted via the Fluidity repository is completely removed from the system. In some occasions however you may wish to keep the records in the data repository but just mark them as deleted so that they don't appear in the UI. This is where the `SetDeletedProperty` method comes in handy.

#### SetDeletedProperty(Lambda deletedPropertyExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets which property of our entity to use as the deleted property flag. Property must be of type `boolean`. When a deleted property is set, any delete actions will set the deleted flag instead of actualy deleting the entity. In addition, any fetch actions will also pre-filter out any deleted entities.

````csharp
// Example
collectionConfig.SetDeletedProperty(p => p.Deleted);
````

### Defining a default sort order
{: .mt}

#### SetSortProperty(Lambda sortPropertyExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets which property of our entity to sort against, defaulting to ascending sort direction.

````csharp
// Example
collectionConfig.SetSortProperty(p => p.FirstName);
````

---

#### SetSortProperty(Lambda sortPropertyExpression, SortDirection sortDirection) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets which property of our entity to sort against in the provided sort direction.

````csharp
// Example
collectionConfig.SetSortProperty(p => p.FirstName, SortDirection.Descending);
````

### Defining searchable properties
{: .mt}

#### AddSearchableProperty(Lambda searchablePropertyExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds the given property to the searchable properties collection. Property must be of type `String`. When set searches via the list view search and entity picker property editor search fields will search across the properties defined in this collection. If no properties are defined as searchable then these UI elements will be disabled.

````csharp
// Example
collectionConfig.AddSearchableProperty(p => p.FirstName);
````

### Defining encrypted properties
{: .mt}

#### AddEncryptedProperty(Lambda encryptedPropertyExpression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds the given property to the encrypted properties collection. Property must be of type `String`. When set, the property will be encrypted/decrypted on write/read respectively.

````csharp
// Example
collectionConfig.AddEncryptedProperty(p => p.Email);
````

### Applying a global filter
{: .mt}

Sometimes you may only want to work with a sub-set of data within a given table so this is where the `SetFilter` method comes in handy, allowing you to define a global filter to apply to all queries for the given collection.

#### SetFilter(Lambda whereClauseExression) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the filter where clause expression. Expression must be a `boolean` expression.

````csharp
// Example
collectionConfig.SetFilter(p => p.Current);
````

### Defining menu items
{: .mt}
See [Menu Items API documentation]({{ site.baseurl }}/api/menu-items/) for more info.

#### AddContainerMenuItem&lt;TMenuItemType&gt;() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a menu item of the given type to the collection tree node right click menu as well as the list view actions menu.

````csharp
// Example
collectionConfig.AddContainerMenuItem<ExportMenuItem>();
````

---

#### AddContainerMenuItem(MenuItem menuItem) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds the provided menu item to the collection tree node right click menu as well as the list view actions menu.

````csharp
// Example
collectionConfig.AddContainerMenuItem(new ExportMenuItem());
````

---

#### AddEntityMenuItem&lt;TMenuItemType&gt;() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds a menu item of the given type to the entity tree node right click menu as well as the entity editor actions menu.

````csharp
// Example
collectionConfig.AddEntityMenuItem<ExportMenuItem>();
````

---

#### AddEntityMenuItem(MenuItem menuItem) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Adds the provided menu item to the entity tree node right click menu as well as the entity editor actions menu.

````csharp
// Example
collectionConfig.AddEntityMenuItem(new ExportMenuItem());
````

### Showing a collection on the section dashboard
{: .mt}

When navigating to a Fluidity section you are automatically presented with a dashboard interface on which you can add your collections to. This dashboard gives a quick entry point to frequently used collections showing the number of items in the collection as well as links to it's list view (if one is defined) as well as a quick create link (if the collection isn't read only).

#### ShowOnDashboard() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the collection to display on the section dashboard.

````csharp
// Example
collectionConfig.ShowOnDashboard();
````

### Making a collection read only
{: .mt}

#### MakeReadOnly() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the collection as read only and disables any CRUD operations from being performed on the collection via the UI.

````csharp
// Example
collectionConfig.MakeReadOnly();
````

### Disable the option to create
{: .mt}

#### DisableCreate() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Disables the option to create entities on the current collection. An entity could be created via code-behind and then only editing is allowed in the UI for example.

````csharp
// Example
collectionConfig.DisableCreate();
````

### Disable the option to update
{: .mt}

#### DisableUpdate() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Disables the option to update entities on the current collection. An entity can be created, but further editing is not allowed. 

````csharp
// Example
collectionConfig.DisableUpdate();
````

### Disable the option to delete
{: .mt}

#### DisableDelete() *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Disables the option to delete entities on the current collection. Useful if the data needs to be retained and visible. See also [defining a deleted flag](#defining-a-deleted-flag).

````csharp
// Example
collectionConfig.DisableDelete();
````

### Setting a view mode
{: .mt}

#### SetViewMode(FluidityViewMode viewMode) *: FluidityCollectionConfig&lt;TEntityType&gt;*
{: .signature}

Sets the view mode of the current collection. Options are `Tree` or `List`. When set to `Tree` then all entities will appear as nodes in the tree. When set as `List` then entities will be hidden from the tree and show in a [list view]({{ site.baseurl }}/api/collections/list-view/) in the right hand content area.

````csharp
// Example
collectionConfig.SetViewMode(FluidityViewMode.List);
````

### Configuring the list view
{: .mt}

#### ListView(Lambda listViewConfig = null) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Accesses the list view config of the current collection. See [List View API documentation]({{ site.baseurl }}/api/collections/list-view/) for more info.

````csharp
// Example
collectionConfig.ListView(listViewConfig => {
    ...
});
````

### Configuring the editor
{: .mt}

#### Editor(Lambda editorConfig = null) *: FluidityEditorConfig&lt;TEntityType&gt;*
{: .signature}

Accesses the editor config of the current collection. See [Editor API documentation]({{ site.baseurl }}/api/collections/editor/) for more info.

````csharp
// Example
collectionConfig.Editor(editorConfig => {
    ...
});
````