---
layout: default
section: Reference
title: Known Issues
permalink: /reference/known-issues/index.html
---

Fluidity tries it's best to mimc the content pipeline as closely as possible whilst sticking to public and supported APIs. This is in order to be able to use the full data type suite for editing properties. Unfortunately there are some features in the Umbraco Core that are locked away in internal methods and so does mean some features may not be fully supported. Bellow are a list of known issues to date.

### Property Editors

#### Tags  

Whilst we have support for persisting the tags value, we don't currently have the ability to write these tags to the `cmsTags` DB table. Unfortunately this is all handled via a `tagsRepository` which is internal and so we currently can't save to it like core does.

### Multi Node Tree Picker

When using a Multi Node Tree Picker with an XPath filter, only filters starting with the `$root` placeholder will be valid as all other placeholders expect the property editor to be placed on a content node, with that node being used as context.
