---
layout: default
section: API
title: Sections
permalink: /api/sections/index.html
---

A section is a distinct area of the Umbraco back office, such as content, media, etc, and is accessed via an icon + label in the main sidebar of the Umbraco interface. Fluidity allows you to define multiple sections in order to organise the management of your models into logical sections.

### Defining a Section

You define a section by calling one of the `AddSection` methods on the root level `FluidityConfig` object.

````csharp
FluiditySectionConfig AddSection(string name, Lambda sectionConfig)
````
Adds a section to the Umbraco sidebar with the given name and a default icon.

````csharp
FluiditySectionConfig AddSection(string name, string icon,  Lambda sectionConfig)
````
Adds a section to the Umbraco sidebar with the given name + icon.

### Configuration Options

````csharp
FluiditySectionConfig SetAlias(string alias)
````
Optional. When adding a new section, an alias is automatically generated from the supplied name for you, however you can use the `SetAlias` method to override this should you need a specific alias.

````csharp
FluidityTreeConfig SetTree(string name, Lambda treeConfig)
````
Sets the tree to display in the Umbraco side panel for the current section with the given name. See the [Trees section]({{ site.baseurl }}/api/trees/) for more info.
