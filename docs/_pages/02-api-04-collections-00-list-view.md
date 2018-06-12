---
layout: default
section: API
title: List View
permalink: /api/collections/list-view/index.html
---

A list view is a list based view of a collections entities and provides additional functionality beyond a normal tree view such as pagination for large collections, custom data views, searching and bulk actions.

### Configuring a list view

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

### Changing the page size
{: .mt}

#### SetPageSize(int pageSize) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Sets the number of items to display per page for the given list view.

````csharp
// Example
listViewConfig.SetPageSize(20);
````

### Defining data views
{: .mt}

Data views allow you to define multiple, pre-filtered views of the same data source which can be toggled between via the list view UI. This can be useful when entities exist in different states and you want a way to toggle between them.

#### AddDataView(string name, Lambda whereClauseExpression) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Adds a data view with the given name and where clause filter expression. Expression must be a `boolean` expression.

````csharp
// Example
listViewConfig.AddDataView("Active", p => p.IsActive);
````

#### SetDataViewsBuilder&lt;TDataViewsBuilder&gt;() *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Sets the list views data views builder which allows you to define the data views dynamically at run time. See [Data Views Builders API documentation]({{ site.baseurl }}/api/data-views-builders/) for more info.

````csharp
// Example
listViewConfig.SetDataViewsBuilder<PersonDataViewsBuilder>();
````

### Adding a bulk action
{: .mt}

#### AddBulkAction&lt;TBulkActionType&gt;() *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Adds a bulk action of the given type to the list view. See [Bulk Actions API documentation]({{ site.baseurl }}/api/bulk-actions/) for more info.

````csharp
// Example
listViewConfig.AddBulkAction<ExportBulkAction>();
````

---

#### AddBulkAction(FluidityBulkAction bulkAction) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Adds the provided bulk action to the list view. See [Bulk Actions API documentation]({{ site.baseurl }}/api/bulk-actions/) for more info.

````csharp
// Example
listViewConfig.AddBulkAction(new ExportBulkAction());
````

### Changing the list view layout
{: .mt}

By default the list view will use the built in Umbraco table and grid list view layouts however you can provide your own custom layouts. If you provide a layout, then it will replace the defaults, so if you still want the defaults as options, you'll need to add these again explicitly. To do this, you'll need to call `AddLayout&lt;TListViewLayoutType&gt;` for each one you want to add with a `TListViewLayoutType` parameter of `FluidityTableListViewLayout` or `FluidityGridListViewLayout`.

#### AddLayout&lt;TListViewLayoutType&gt;() *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Adds a list view layout of the given type to the list view. See [List View Layouts API documentation]({{ site.baseurl }}/api/list-view-layouts/) for more info.

````csharp
// Example
listViewConfig.AddLayout<MyCustomListViewLayout>();
````

---

#### AddLayout(FluidityListViewLayout listViewLayout) *: FluidityListViewConfig&lt;TEntityType&gt;*
{: .signature}

Adds the provided list view layout to the list view. See [List View Layouts API documentation]({{ site.baseurl }}/api/list-view-layouts/) for more info.

````csharp
// Example
listViewConfig.AddLayout(new MyCustomListViewLayout());
````

### Adding a field to the list view
{: .mt}

#### AddField(Lambda propertyExpression, Lambda propertyConfig = null) *: FluidityListViewFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Adds the given property to the list view.

````csharp
// Example
listViewConfig.AddField(p => p.FirstName, fieldConfig => {
    ...
});
````

### Changing the heading of a field
{: .mt}

#### SetHeading(string heading) *: FluidityListViewFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Sets the heading for the list view field.

````csharp
// Example
fieldConfig.SetHeading("First Name");
````

### Formatting the value of a field
{: .mt}

#### SetFormat(Lambda formatExpression) *: FluidityListViewFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Sets the format expression for the list view field.

````csharp
// Example
fieldConfig.SetFormat((v, p) => $"{v} years old");
````
