---
layout: default
section: API
title: Repositories
permalink: /api/repositories/index.html
---

Repositories are used by Fluidity to access the entity data stores. By default collections will use a generic built in PetaPoco repository however you can define your own repository implementation should you wish to store your entities via an alternative strategy.

### Defining a repository

To define a repository you create a class that inherits from the base class `FluidityRepository<TEntity, TId>` and implements all of its abstract methods.

````csharp
// Example
public class PersonRepository : FluidityRepository<Person, int> {

    protected override int GetIdImpl(Person entity) {
        return entity.Id;
    }

    protected override Person GetImpl(int id) {
        ...
    }

    protected override IEnumerable<Person> GetAllImpl() {
        ...
    }

    protected override PagedResult<Person> GetPagedImpl(int pageNumber, int pageSize, Expression<Func<Person, bool>> whereClause, Expression<Func<Person, object>> orderBy, SortDirection orderDirection) {
        ...
    }

    protected override Person SaveImpl(Person entity) {
        ...
    }

    protected override void DeleteImpl(int id) {
        ...
    }

    protected override long GetTotalRecordCountImpl() {
        ...
    }
}
````

**Note:** For all `Impl` methods there are public alternatives without the `Impl` suffix, however we have seperate inplementation methods in order to ensure all repositories fire the relevant Fluidity events whether triggered via the Fluidity UI or not.

### Changing the repository implementation of a collection
{: .mt}

A repository is assigned to a collection as part of the collection configuration. See [Collection API Documentation]({{ site.baseurl }}/api/collections/#changing-a-collection-repository-implementation) for more info.

### Accessing a repository in code
{: .mt}

If you have created your own repository implementation, then accessing the repository can be as simple as instantiating a new instance of the repository class, however if you are using the built in repository, unfortunately a new instance can't be created in this way.

To help with accessing a repository (default or custom) Fluidity has a couple of factory methods to create the repository instances for you.

#### Fluidity.GetRepository&lt;TEntity, TId&gt;() *: FluidityRepository&lt;TEntity, TId&gt;*
{: .signature}

Creates a repository for the given entity type. Fluidity will search the configuration for the first section / collection with a configuration for the given entity type and use that as repository configuration.

````csharp
// Example
var repo = Fluidity.GetRepository<Person, int>();
````

---

#### Fluidity.GetRepository&lt;TEntity, TId&gt;(string sectionAlias, string collectionAlias) *: FluidityRepository&lt;TEntity, TId&gt;*
{: .signature}

Creates a repository for the given entity type from the given section / collection configuration.

````csharp
// Example
var repo = Fluidity.GetRepository<Person, int>("database", "person");
````

