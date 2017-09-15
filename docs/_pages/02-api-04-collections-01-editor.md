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

#### Editort(FluidityEditorConfig&lt;TEntityType&gt; editorConfig) *: FluidityEditorConfig&lt;TEntityType&gt;*
{: .signature}

Sets the editor config of the given collection with a pre-configured `FluidityEditorConfig<TEntityType>` instance.

````csharp
// Example
collectionConfig.Editor(editorConfig);
````

### Adding a tab to an editor
{: .mt}

### Adding a field to a tab
{: .mt}

### Changing the label of a field
{: .mt}

### Adding a description to a field
{: .mt}

### Making a field mandatory
{: .mt}

### Validating a field
{: .mt}

### Changing the data type of a field
{: .mt}

### Adding a mapper to a field
{: .mt}