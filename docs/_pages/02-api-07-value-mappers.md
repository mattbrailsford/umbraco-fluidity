---
layout: default
section: API
title: Value Mappers
permalink: /api/value-mappers/index.html
---

A value mapper is a little Fluidity helper class that sits between the editor UI and the database and lets you tweak the stored value of a field. By default Fluidity will save a datatypes value is it would be stored in Umbraco. 

### Defining a mapper

To define a mapper you create a class that inherits from the base class `FluidityValueMapper`.

````csharp
// Example
public class MyMapper : FluidityValueMapper
{
    public override object EditorToModel(object input)
    {
        ...
    }

    public override object ModelToEditor(object input)
    {
        ...
    }    
}
````

### Mapping