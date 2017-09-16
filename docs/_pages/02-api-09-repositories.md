---
layout: default
section: API
title: Repositories
permalink: /api/repositories/index.html
---

Repositories are used by Fluidity to access the entity data stores. By default collections will use a generic built in PetaPoco repository however you can define your own repository implementation should you wish to store your entities via another strategy.

### Defining a repository

To define a repository you create a class that inherits from the base class `FluidityRepository<TEntity, TId>` and implements the abstract methods.

````csharp
public class PersonRepository : FluidityRepository<Person, int> {

    protected override int GetIdImpl(Person entity) 
        return entity.Id;
    }

    protected override Person GetImpl(int id) {
        ...
    }

    protected override IEnumerable<Person> GetAllImpl() {
        ...
    }

    protected override PagedResult<Person> GetPagedImpl(int pageNumber, int pageSize, Expression<Func<Person, object>> orderBy, Direction orderDirection, Expression<Func<Person, bool>> whereClause);
        ...
    }

    protected override Person SaveImpl(Person entity) {
        ...
    }

    protected override void void DeleteImpl(int id) {
        ...
    }

    protected override long GetTotalRecordCountImpl() {
        ...
    }
}
````

**Note:** For all `Impl` methods there are public alternatives without the `Impl` suffix, however we have seperate inplementation methods in order to ensure all repositories fire the relevant Fluidity events.

### Changing the repository implementation of a collection
{: .mt}

A reposoitory is assigned to a collection as part of the collection configuration. See [Collection API Documentation]({{ site.baseurl }}/api/collections/#changing-a-collection-repository-implementation) for more info.