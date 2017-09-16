---
layout: default
section: API
title: Menu Items
permalink: /api/menu-items/index.html
---

Menu Items are a standard Umbraco concept and are used to configure items that appear in the tree context menu and list view / editor action menus.

### Defining a menu item

To define a menu item you create a class that inherits from the base class `MenuItem` and configure it within the constructor like so.

````csharp
// Example
public class ExportMenuItem : MenuItem
{
    public ExportMenuItem()
    {
        Name = "Export";
        Icon = "icon-download";
        Action = "";
    }    
}
````

Available configuration options are:

* **Name:** The name of the menu item
* **Icon:** An icon to display next to the name in the menu