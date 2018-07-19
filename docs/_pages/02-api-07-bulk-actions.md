---
layout: default
section: API
title: Bulk Actions
permalink: /api/bulk-actions/index.html
---

Bulk actions provides an API to perform bulk operations from within a list view UI.

### Defining a bulk action

To define a bulk action you create a class that inherits from the base class `FluidityBulkAction` and implements the abstract configuration properties.

````csharp
// Example
public class DeleteBulkAction : FluidityBulkAction
{
    public override string Name => "Delete";

    public override string Alias => "delete";

    public override string Icon => "icon-trash";

    public override string AngularServiceName => "fluidityDeleteBulkActionService";
}
````

The required configuration options are:

* **Name:** The name of the bulk action.
* **Alias:** A unique alias for the bulk action.
* **Icon:** An icon to display next to the name in the bulk action button.
* **AngularServiceName:** The name of the registered angular service to delegate the bulk action to.

### Defining a bulk action angular service
{: .mt}

The actual logic for a bulk action needs to be encapsulated in an angular service with the following signature:

````javascript
(function () {

    'use strict';

    function fluidityDeleteBulkAction(fluidityResource) {

        return {
            
            performAction: function(section, collection, id) {
                return fluidityResource.deleteEntity(section, collection, id);
            },

            getConfirmMessage: function (count) {
                return "Are you sure you want to delete " + count + " items?";
            },

            getProgressMessage: function(count, total) {
                return count + " of " + total + " items deleted";
            },

            getCompleteMessage: function(total) {
                return total + " items successfully deleted";
            }

        }

    }

    angular.module("umbraco.services").factory("fluidityDeleteBulkActionService", fluidityDeleteBulkAction);

})();
````

The required service methods are:

* **performAction(section, collection, id)** Should perform the bulk action to the current item identified with the id parameter.

The optional service methods are:

* **getConfirmMessage(count)** Should return a string to display as the confirmation message before the bulk action is performed. A generic default message will be used if no `getConfirmMessage` method is defined.
* **getProgressMessage(count, total)** Should return a string to display as the progress message whilst the bulk action is running. A generic default message will be used if no `getProgressMessage` method is defined.
* **getCompleteMessage(total)** Should return a string to display as the complete message once the bulk action has finished running. A generic default message will be used if no `getCompleteMessage` method is defined.

You'll need to ensure your angular service as well as any resources needed by your service are loaded before it can be used. This is a bit out of scope for the Fluidity documentation, however in summary you will want to:

* Create a plugin folder in the root app_plugin folder.
* Create a `package.manifest` file in your plugin folder.
* Add the service js file path to the `package.manifest`.

### Adding a bulk action to a list view
{: .mt}

Bulk actions are added to a list view as part of the list view configuration. See [List View API documentation]({{ site.baseurl }}/api/collections/list-view/#adding-a-bulk-action) for more info.
