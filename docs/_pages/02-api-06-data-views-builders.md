---
layout: default
section: API
title: Data Views Builders
permalink: /api/data-views-builders/index.html
---

Data views builders allow you to create a list views data views list dynamically at run time. By default fluidity will use the hard coded data views defined in your fluidity config, however if you need to build your data views list dynamically, then is is when you'd use a data views builder.

### Defining a data views builder

To define a data views builder you create a class that inherits from the base class `FluidityDataViewsBuilder<TEntityType>` and implements the abstract methods.

````csharp
// Example
public class PersonDataViewsBuilder : FluidityDataViewsBuilder<Person>
{
    public override IEnumerable<FluidityDataViewSummary> GetDataViews()
    {
        // Generate and return a list of data views
    }

    public override Expression<Func<Person, bool>> GetDataViewWhereClause(string dataViewAlias)
    {
        // Return a where clause expression for the supplied data view alias
    }
}
````

The required methods are:

* **GetDataViews:** Returns the list of data views to choose from.
* **GetDataViewWhereClause:** Returns the boolean where clause expression for the given data views alias.

### Setting the data views builder of a list view
{: .mt}

A data views builder is assigned to a list view as part of the list view configuration. See [List View API Documentation]({{ site.baseurl }}/api/collections/list-view/#defining-data-views) for more info.
