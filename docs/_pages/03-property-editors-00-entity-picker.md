---
layout: default
section: Property Editors
title: Entity Picker
permalink: /property-editors/entity-picker/index.html
---

The Entity Picker property editor is an Umbraco property editor that lets you select one or more entities from a Fluidity collection.

### Configuring an entity picker

### Using an entity picker
{: .mt}

### Getting the value of an entity picker
{: .mt}
 
The entity picker property editor comes with a built in [value converter](https://our.umbraco.org/documentation/extending/property-editors/value-converters) meaning that whenever you retrieve the property value from Umbraco it will return the actual selected entities.

````csharp
// Example
foreach(var p in Model.Content.People.Cast<Person>()){
    ...
}
````

**Note:** Due to the fact that the property editor can link to any entity type, the returned value type from the value converter will be `IEnumrable<object>` and so will require the entities to be cast to the desired concrete type.