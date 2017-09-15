---
layout: default
section: API
title: List View
permalink: /api/collections/list-view/index.html
---

A list view is a list based view of a collections entities and provides additional functionality beyond a normal tree view such as pagination for large collections, custom data views, searching and bulk actions.

### Configuraing a list view

The list view configuration is a sub configuration of a [`FluidityCollectionConfig`]({{ site.baseurl }}/api/collections/) instance and is accessing via it's `ListView` method.

#### ListView(Lambda listViewConfig = null) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Accesses the list view config of the given collection.

````csharp
// Example
collectionConfig.ListView(listViewConfig => {
    ...
});
````
---

#### ListView(FluidityListViewConfig&lt;TEntityType&gt; listViewConfig) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Sets the list view config of the given collection with a pre-configured `FluidityListViewConfig<TEntityType>` instance.

````csharp
// Example
collectionConfig.ListView(listViewConfig);
````

### Changing the page size
{: .mt}

#### SetPageSize(int pageSize) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Sets the number of items to display per page for the given list view.

````csharp
// Example
listViewConfig.SetPageSize(20);
````

### Adding a data view
{: .mt}

### Adding a bulk action
{: .mt}

### Changing the list view layout
{: .mt}

### Adding a field to the list view
{: .mt}

### Changing the heading of a field
{: .mt}

### Formatting the value of a field
{: .mt}