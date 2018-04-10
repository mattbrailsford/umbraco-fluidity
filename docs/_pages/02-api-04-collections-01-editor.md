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
editorConfig.AddField(p => p.FirstName, fieldConfig => {
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
fieldConfig.SetLabel("First Name");
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

### Setting the default value of a field
{: .mt}

#### SetDefaultValue(TValueType defaultValue) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Sets the default value to a known constant.

````csharp
// Example
fieldConfig.SetDefaultValue(10);
````

---

#### SetDefaultValue(Func<TValueType> defaultValueFunc) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Sets the default value via a function that gets evaluated at time of entity creation.

````csharp
// Example
fieldConfig.SetDefaultValue(() => DateTime.Now);
````

### Making a field read only
{: .mt}

#### MakeReadOnly() *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Makes the current field read only disabling editing in the UI. A ReadOnly property cannot have a custom DataType, ValueMapper or ValidationRegExp.

````csharp
// Example
fieldConfig.MakeReadOnly();
````

---

#### MakeReadOnly(Func&lt;TValueType, string&gt; format) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Makes the current field read only disabling editing in the UI. A ReadOnly property cannot have a custom DataType, ValueMapper or ValidationRegExp. Provides a custom formatting expression to use when rendering the value as a string.

````csharp
// Example
fieldConfig.MakeReadOnly(distanceProp => $"{distanceProp:## 'km'}");
````

### Making a field mandatory
{: .mt}

#### MakeRequired() *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Makes the given field mandatory.

````csharp
// Example
fieldConfig.MakeRequired();
````

### Validating a field
{: .mt}

#### SetValidationRegex(string regex) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Defines the regular expression to use when validating the field.

````csharp
// Example
fieldConfig.SetValidationRegex("[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}");
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

### Setting a field value mapper
{: .mt}

#### SetValueMapper&lt;TMapperType&gt;() *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Set the value mapper for the current field. See [Value Mapper API documentation]({{ site.baseurl }}/api/value-mappers/) for more info.

````csharp
// Example
fieldConfig.SetValueMapper<MyValueMapper>();
````

---

#### SetValueMapper(FluidityMapper mapper) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Set the value mapper for the current field. See [Value Mapper API documentation]({{ site.baseurl }}/api/value-mappers/) for more info.

````csharp
// Example
fieldConfig.SetValueMapper(new MyValueMapper());
````
