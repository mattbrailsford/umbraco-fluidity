---
layout: default
section: Property Editors
title: Entity Picker
permalink: /property-editors/entity-picker/index.html
---

The Entity Picker property editor is an Umbraco property editor that lets you select one or more entities from a Fluidity collection.

### Configuring an entity picker

To configure an entity picker you'll firstly want to create a data type in the Umbraco back office choosing 'Fluidity Entity Picker' from the property editor dropdown.

![Pre Value]({{ site.baseurl }}/img/pre-value.png) 

From their choose the 'Section' and then 'Collection' you'd like to pick entities from, as well as an optional list view 'Data View' if there are any configured.

You can also set a minimum and maximum number of items to be able to pick if required.

With an entity picker data type defined, finish off the configuration by adding it to the desired document type definition.

![Add to Doc Type]({{ site.baseurl }}/img/add-to-doc-type.png) 

### Using an entity picker
{: .mt}

Using the entity picker should be pretty fimilar as it aims to mimic the content picker as closely as possible.

To pick an entity click the 'Add' link to launch the picker dialog. The dialog should present a pagniated list of entities to pick from. If any searchable fields have been configured for the entity type, you can also perform a search by typing a search term in the search input field.

![Picker Dialog]({{ site.baseurl }}/img/picker-dialog.png) 

To pick your items simply click on the entity names and then click 'Select' in the bottom right hand corner.

The picker should display a summary of the selected entities and can be sorted by dragging the selected entities into the desired order.

![Picker]({{ site.baseurl }}/img/picker.png) 

To save the value either save or save and publish the current document.

### Getting the value of an entity picker
{: .mt}
 
The entity picker property editor comes with a built in [value converter](https://our.umbraco.org/documentation/extending/property-editors/value-converters) meaning that whenever you retrieve the property value from Umbraco it will return the actual selected entities.

````csharp
// Example
foreach(var p in Model.Content.People.Cast<Person>()){
    ...
}
````

**Note:** Due to the fact that the property editor can link to any entity type, the returned value type from the value converter will be `IEnumerable<object>` and so will require the entities to be cast to the desired concrete type.
