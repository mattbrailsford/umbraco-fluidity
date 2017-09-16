---
layout: default
section: API
title: Events
permalink: /api/events/index.html
---

Fluidity fires a number of standard .NET events during regular operation to allow for extending of the default behavour.

### Repository events

#### Saving(object sender, SavingEntityEventArgs args)
{: .signature}

Raised when the repository `Save` method is called and before the entity has been saved. The `args` param contains an `Entity` property with `Before` and `After` inner properties providing access to a copy of the currently persisted entity (or null if a new entity) and the updated entity about to be saved. Changes can be made to the `After` entity and they will be persisted as part of the save opperation. If the `Cancel` property of `args` is set to `true` then the save operation will be canceled and no changes will be saved.

````csharp
// Example
Fluidity.Saving += (sender, args) => {
    var person = args.Entity.After as Person;
    if (person != null){
        ...
    }
};
````

---


#### Saved(object sender, SavedEntityEventArgs args)
{: .signature}

Raised when the repository `Save` method is called and after the entity has been saved. The `args` param contains an `Entity` property with `Before` and `After` inner properties providing access to a copy of the previously persisted entity (or null if a new entity) and the updated entity about just saved. 

````csharp
// Example
Fluidity.Saved += (sender, args) => {
    var person = args.Entity.After as Person;
    if (person != null){
        ...
    }
};
````

---


#### Deleting(object sender, DeletingEntityEventArgs args)
{: .signature}

Raised when the repository `Delete` method is called and before the entity is deleted. The `args` param contains an `Entity` property providing access to a copy of the entity about to be deleted. If the `Cancel` property of `args` is set to `true` then the delete operation will be canceled and entity won't be deleted.

````csharp
// Example
Fluidity.Deleting += (sender, args) => {
    var person = args.Entity as Person;
    if (person != null){
        ...
    }
};
````

---

#### Deleted(object sender, DeletedEntityEventArgs args)
{: .signature}

Raised when the repository `Delete` method is called and after the entity is deleted. The `args` param contains an `Entity` property providing access to a copy of the entity just deleted. 

````csharp
// Example
Fluidity.Deleted += (sender, args) => {
    var person = args.Entity as Person;
    if (person != null){
        ...
    }
};
````