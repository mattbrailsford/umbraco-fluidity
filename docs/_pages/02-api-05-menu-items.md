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
        // Configure menu item
        Name = "Export";
        Alias = "export";
        Icon = "icon-download";

        // Set action behaviour
        NavigateToRoute("/my/angular/route");
    }    
}
````

Available configuration options are:

* **Name:** The name of the menu item
* **Alias:** An unique alias for the menu item
* **Icon:** An icon to display next to the name in the menu
* **SeperatorBefore:** Boolean flag to set whether to show a seperator in the menu before this menu item

In addition, there are three options for the type of action the menu item should perform:

* **NavigateToRoute(string route)** Navigates to the given route when clicked
* **LaunchDialogView(string view, string dialogTitle)** Opens an angular view in the left hand dialog modal, with the given title
* **LaunchDialogUrl(string url, string dialogTitle)** Opens a URL in an iframe in side the left hand dialog modal, with the given title