// <copyright file="FluidityEntityEditModel.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Umbraco.Web.Models.ContentEditing;

namespace Fluidity.Web.Models
{   
    [DataContract(Name = "entity", Namespace = "")]
    public class FluidityEntityEditModel : FluidityEntityBasicModel
    {
        public FluidityEntityEditModel()
        {
            Tabs = new List<Tab<ContentPropertyDisplay>>();
            Errors = new Dictionary<string, object>();
        }

        [DataMember(Name = "section", IsRequired = true)]
        public string Section { get; set; }

        [DataMember(Name = "tree", IsRequired = true)]
        public string Tree { get; set; }

        [DataMember(Name = "collectionIsEditable")]
        public bool CollectionIsEditable { get; set; }

        [DataMember(Name = "collectionNameSingular")]
        public string CollectionNameSingular { get; set; }

        [DataMember(Name = "collectionNamePlural")]
        public string CollectionNamePlural { get; set; }

        [DataMember(Name = "collectionIconSingular")]
        public string CollectionIconSingular { get; set; }

        [DataMember(Name = "collectionIconPlural")]
        public string CollectionIconPlural { get; set; }

        [DataMember(Name = "hasNameProperty")]
        public bool HasNameProperty { get; set; }

        [DataMember(Name = "isChildOfListView")]
        public bool IsChildOfListView { get; set; }

        [DataMember(Name = "isChildOfTreeView")]
        public bool IsChildOfTreeView { get; set; }

        [DataMember(Name = "treeNodeUrl")]
        public string TreeNodeUrl { get; set; }

        [DataMember(Name = "tabs")]
        public IEnumerable<Tab<ContentPropertyDisplay>> Tabs { get; set; }

        [DataMember(Name = "createDate")]
        public DateTime CreateDate { get; set; }

        [DataMember(Name = "updateDate")]
        public DateTime UpdateDate { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        /// <summary>
        /// This is used to add custom localized messages/strings to the response for the app to use for localized UI purposes.
        /// </summary>
        //[DataMember(Name = "notifications")]
        //[ReadOnly(true)]
        //public List<Notification> Notifications { get; private set; }

        /// <summary>
        /// This is used for validation of an entity.
        /// </summary>
        /// <remarks>
        /// NOTE: The ProperCase is important because when we return ModeState normally it will always be proper case.
        /// </remarks>
        [DataMember(Name = "ModelState")]
        [ReadOnly(true)]
        public IDictionary<string, object> Errors { get; set; }
    }
}
