// <copyright file="ServerVariableInjector.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Fluidity.Web.Api;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.UI.JavaScript;

namespace Fluidity.Web
{
    internal class ServerVariableInjector : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ServerVariablesParser.Parsing += (sender, objects) =>
            {
                if (HttpContext.Current == null)
                    return;

                var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));
                var variables = new Dictionary<string, object>
                {
                    { "apiBaseUrl", urlHelper.GetUmbracoApiServiceBaseUrl<FluidityApiController>(c => c.Index()) }
                };

                if (!objects.ContainsKey("fluidity"))
                {
                    objects.Add("fluidity", variables);
                }
            };
        }
    }
}
