---
layout: default
section: API
title: Value Mappers
permalink: /api/value-mappers/index.html
---

A value mapper is a little Fluidity helper class that sits between the editor UI and the database and lets you tweak the stored value of a field. By default Fluidity will save a datatypes value is it would be stored in Umbraco. Value mappers let you change this. 

### Defining a value mapper

To define a mapper you create a class that inherits from the base class `FluidityValueMapper` and implements the methods `EditorToModel` and `ModelToEditor`.

````csharp
// Example
public class MyMapper : FluidityValueMapper
{
    public override object EditorToModel(object input)
    {
        // Tweak the input and return mapped object
        ...
    }

    public override object ModelToEditor(object input)
    {
        // Tweak the input and return mapped object
        ...
    }    
}
````

### Setting a mapper on a field
{: .mt}

A mapper is assigned to a field as part of the [editor]({{ site.baseurl }}/api/collections/editor/) configuration.

#### SetValueMapper&lt;TMapperType&gt;() *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Set the value mapper for the given field.

````csharp
// Example
fieldConfig.SetValueMapper<MyValueMapper>();
````

---

#### SetValueMapper(FluidityMapper mapper) *: FluidityEditorFieldConfig&lt;TEntityType, TValueType&gt;*
{: .signature}

Set the value mapper for the given field.

````csharp
// Example
fieldConfig.SetValueMapper(new MyValueMapper());
````