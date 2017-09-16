---
layout: default
section: API
title: Editor
permalink: /api/collections/editor/index.html
---

An editor is the user interface used to edit an entity and is made up of tabs and property editors.

### Configuring an editor

The editor configuration is a sub configuration of a [`FluidityCollectionConfig`]({{ site.baseurl }}/api/collections/) instance and is accessing via it's `Editor` method.

#### Editor(Lambda editorConfig = null) *: FluidityEditorConfig&lt;TEntityType&gt;*
{: .signature}

Accesses the editor config of the given collection.

````csharp
// Example
collectionConfig.Editor(editorConfig => {
    ...
});
````
---

#### Editor(FluidityEditorConfig&lt;TEntityType&gt; editorConfig) *: FluidityEditorConfig&lt;TEntityType&gt;*
{: .signature}

Sets the editor config of the given collection with a pre-configured `FluidityEditorConfig<TEntityType>` instance.

````csharp
// Example
collectionConfig.Editor(editorConfig);
````

### Adding a tab to an editor
{: .mt}

#### AddTab(string name, Lambda tabConfig = null) *: FluidityTabConfig&lt;TEntityType&gt;*
{: .signature}

Adds a tab to the editor.

````csharp
// Example
editorConfig.AddTab("General", tabConfig => {
    ...
});
````

### Adding a field to a tab
{: .mt}

#### AddField(Lambda propertyExpression, Lambda propertyConfig = null) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Adds the given property to the editor.

````csharp
// Example
editorConfig.AddField(p => p.LastName, fieldConfig => {
    ...
});
````

### Changing the label of a field
{: .mt}

By default Fluidity will build the label from the property name, including splitting camel case names into sentence case, however you can set an explicit label if you'd prefer.

#### SetLabel(string label) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Sets the label for the editor field.

````csharp
// Example
fieldConfig.SetLabel("Last Name");
````

### Adding a description to a field
{: .mt}

#### SetDescription(string description) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Sets the description for the editor field.

````csharp
// Example
fieldConfig.SetDescription("Enter your age in years");
````

### Making a field mandatory
{: .mt}

#### IsRequired() *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Makes the given field mandatory.

````csharp
// Example
fieldConfig.IsRequired();
````

### Validating a field
{: .mt}

#### SetValidationRegex(string regex) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Defines the regular expression to use when validating the field.

````csharp
// Example
fieldConfig.SetValidationRegex("[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}");
````

### Changing the data type of a field
{: .mt}

By default Fluidity will automatically choose a relevant data type for simple field types, however you can override this should you wish to use an alternative data type.

#### SetDataType(string dataTypeName) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Set the data type of the current field to the Umbraco data type with the given name.

````csharp
// Example
fieldConfig.SetDataType("Richtext Editor");
````

---

#### SetDataType(int dataTypeId) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Set the data type of the current field to the Umbraco data type with the given id.

````csharp
// Example
fieldConfig.SetDataType(-88);
````

### Adding a mapper to a field
{: .mt}