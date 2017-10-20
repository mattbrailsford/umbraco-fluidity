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
        Icon = "download"; // Exclude the "icon-" prefix

        // Set action behaviour
        NavigateToRoute("/my/angular/route");
    }    
}
````

Available configuration options are:

* **Name:** The name of the menu item.
* **Alias:** A unique alias for the menu item.
* **Icon:** An icon to display next to the name in the menu.
* **SeperatorBefore:** Boolean flag to set whether to show a seperator in the menu before this menu item.

In addition, there are three options for the type of action the menu item should perform:

* **NavigateToRoute(string route)** Navigates to the given route when clicked.
* **LaunchDialogView(string view, string dialogTitle)** Opens an angular view in the left hand dialog modal, with the given title.
* **LaunchDialogUrl(string url, string dialogTitle)** Opens a URL in an iframe in side the left hand dialog modal, with the given title.

You can find more detailed information on `MenuItem` class over at the offical [Umbraco Menu Item documentation](https://our.umbraco.org/apidocs/csharp/api/Umbraco.Web.Models.Trees.MenuItem.html)

As well as defining the menu item class, depending on the action type you configure, you will also need to implement the relevant angular view and controller. This is a little out of scope for the Fluidity documentation, however in summary you will want to:

* Create a plugin folder in the root app_plugin folder.
* Create a `package.manifest` file in your plugin folder.
* Create a html view to be loaded.
* Create an angular controller to control the view.
* Hook up the controller with the view using the `ng-controller` attribute.
* Add the controller js file path to the `package.manifest`.
* Build your custom logic.

### Adding a menu item to a collection
{: .mt}

Menu items are added to a collection as part of the collections configuration. See [Collections API documentation]({{ site.baseurl }}/api/collections/#defining-menu-items) for more info.