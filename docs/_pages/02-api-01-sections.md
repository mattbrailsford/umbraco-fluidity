---
layout: default
section: API
title: Sections
permalink: /api/sections/index.html
---

A section is a distinct area of the Umbraco back office, such as content, media, etc, and is accessed via an icon + label in the main sidebar of the Umbraco interface. Fluidity allows you to define multiple sections in order to organise the management of your models into logical sections.

### Defining a section

You define a section by calling one of the `AddSection` methods on the root level `FluidityConfig` instance.

#### AddSection(string name, Lambda sectionConfig = null) *: FluiditySectionConfig*
{: .signature}

Adds a section to the Umbraco sidebar with the given name and a default icon.

````csharp
// Example
config.AddSection("Database", sectionConfig => {
    ...
});
````

---

#### AddSection(string name, string icon,  Lambda sectionConfig = null) *: FluiditySectionConfig*
{: .signature}

Adds a section to the Umbraco sidebar with the given name + icon.

````csharp
// Example
config.AddSection("Database", "icon-server-alt", sectionConfig => {
    ...
});
````

**Note:** Due to the nature of how Umbraco works, when you first create your section it will likely not instantly appear. This is because you first need to give access to your back office user account. Go to the user editor for your user account and check the the 'Sections' checkbox next to your section name, click 'Save' and then refresh your browser.

![User sections permissions]({{ site.baseurl }}/img/user-sections.png) 

**Note:** Both of the `AddSection` methods are simple proxies to the Umbraco `SectionService.MakeNew` method which is purely a creational method, meaning if you change the name or icon of an existing section declaration then your changes likely won't take effect. As one of the principles of Fluidity is to work only with public supported methods, this unfortunately means modifying an existing section via the Fluidity API isn't possible. The current workaround to change a section name / icon is to to edit the `applications.config` file in the `Config` folder manually.

### Changing a section alias
{: .mt}

#### SetAlias(string alias) *: FluiditySectionConfig*
{: .signature}

Sets the alias of the section.  

**Optional:** When adding a new section, an alias is automatically generated from the supplied name for you, however you can use the `SetAlias` method to override this should you need a specific alias.

````csharp
// Example
sectionConfig.SetAlias("database");
````

### Setting a section tree
{: .mt}

#### SetTree(string name, Lambda treeConfig = null) *: FluidityTreeConfig*
{: .signature}

Sets the tree with the given name to display in the Umbraco side panel for the current section. See the [Trees API documentation]({{ site.baseurl }}/api/trees/) for more info.

````csharp
// Example
sectionConfig.SetTree("Database", treeConfig => {
    ...
});
````