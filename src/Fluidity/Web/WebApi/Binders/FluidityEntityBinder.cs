// <copyright file="FluidityEntityBinder.cs" company="Umbraco, Matt Brailsford">
// Original work Copyright (c) 2013 Umbraco and contributors.
// Original work licensed under the MIT License (https://github.com/umbraco/Umbraco-CMS/blob/release-7.6.0/LICENSE.md).
// Modified work Copyright (c) 2019 Matt Brailsford and contributors.
// Modified work licensed under the Apache License, Version 2.0.
// </copyright>
// <remarks>
// Based on https://github.com/umbraco/Umbraco-CMS/blob/release-7.6.0/src/Umbraco.Web/WebApi/Binders/ContentItemBinder.cs
// </remarks>

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Fluidity.Web.Models;
using Newtonsoft.Json;
using Umbraco.Core.IO;
using Umbraco.Web.Models.ContentEditing;

namespace Fluidity.Web.WebApi.Binders
{
    // Based heavily on the ContentItemBinder in core as we need to mimic the 
    // core pipeline as closely as possible to ensure things get executed 
    // as expected
    internal class FluidityEntityBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            // Validate content type
            if (actionContext.Request.Content.IsMimeMultipartContent() == false)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Setup temp folder for uploaded files
            var fileUploadsDir = IOHelper.MapPath("~/App_Data/TEMP/FileUploads");

            // Ensure the folder exists
            Directory.CreateDirectory(fileUploadsDir);

            // Create a multi part stream handler
            var dataProvider = new MultipartFormDataStreamProvider(fileUploadsDir);

            // Extract the model from the request
            var task = Task.Run(() => ParseModelAsync(actionContext, bindingContext, dataProvider))
                .ContinueWith(x =>
                {
                    if (x.IsFaulted && x.Exception != null)
                    {
                        throw x.Exception;
                    }

                    // Umbraco does the model validating here, but it seems like the wrong
                    // place to me, so in fluidity, we do it in the api controller

                    bindingContext.Model = x.Result;
                });

            task.Wait();

            return bindingContext.Model != null;
        }

        protected async Task<object> ParseModelAsync(HttpActionContext actionContext, ModelBindingContext bindingContext, MultipartFormDataStreamProvider provider)
        {
            // Validate the request
            var request = actionContext.Request;
            var content = request.Content;
            var result = await content.ReadAsMultipartAsync(provider);

            if (result.FormData["contentItem"] == null)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = "The request was not formatted correctly and is missing the 'contentItem' parameter";
                throw new HttpResponseException(response);
            }

            // Get the json post data from the request and deserialize it
            var contentItem = result.FormData["contentItem"];
            var model = JsonConvert.DeserializeObject<FluidityEntityPostModel>(contentItem);

            // Get the default body validator and validate the object
            var bodyValidator = actionContext.ControllerContext.Configuration.Services.GetBodyModelValidator();
            var metadataProvider = actionContext.ControllerContext.Configuration.Services.GetModelMetadataProvider();
            bodyValidator.Validate(model, typeof(FluidityEntityPostModel), metadataProvider, actionContext, "");

            // Get the files
            foreach (var file in result.FileData)
            {
                // The name that has been assigned in JS has 2 parts and the second part indicates the property id 
                // for which the file belongs.
                var parts = file.Headers.ContentDisposition.Name.Trim('\"').Split('_');
                if (parts.Length != 2)
                {
                    var response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                    response.ReasonPhrase = "The request was not formatted correctly the file name's must be underscore delimited";
                    throw new HttpResponseException(response);
                }
                var propAlias = parts[1];

                var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');

                model.UploadedFiles.Add(new ContentItemFile
                {
                    TempFilePath = file.LocalFileName,
                    PropertyAlias = propAlias,
                    FileName = fileName
                });
            }

            // Trim any rogue spaces from the name variable
            model.Name = model.Name.Trim();

            // Return the processed post model
            return model;
        }
    }
}
