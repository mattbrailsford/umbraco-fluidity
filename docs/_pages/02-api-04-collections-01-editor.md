---
layout: default
section: API
title: Editor
permalink: /api/collections/editor/index.html
---

An editor is the user interface used to edit an entity and is made up of tabs and property editors.

### Configuraing an editor

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