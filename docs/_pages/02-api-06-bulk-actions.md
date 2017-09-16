---
layout: default
section: API
title: Bulk Actions
permalink: /api/bulk-actions/index.html
---

Bulk actions provides an API to perform bulk operations from within a list view UI.

### Define a bulk action

To define a bulk action you create a class that inherits from the base class `FluidityBulkAction` and implements the abstract configuration properties.

````csharp
public class ExportBulkAction : FluidityBulkAction
{
    public override string Name => "Export";

    public override string Alias => "export";

    public override string Icon => "icon-download";

    public override string AngularServiceName => "fluidityExportBulkActionService";
}
````

The required configuration options are:

* **Name:** The name of the bulk action
* **Alias:** A unique alias for the bulk action
* **Icon:** An icon to display next to the name in the bulk action button
* **AngularServiceName:** The name of the registered angular service to delegate the bulk action to