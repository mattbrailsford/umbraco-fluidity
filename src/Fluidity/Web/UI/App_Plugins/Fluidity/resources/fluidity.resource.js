// <copyright file="fluidity.resource.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function fluidityResource($http, umbRequestHelper) {

        var api = {

            getSectionByAlias: function (section) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getsectionbyalias",
                        method: "GET",
                        params: {
                            section: section
                        }
                    }),
                    'Failed to get section by alias'
                );
            },

            getCollectionsLookup: function () {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getcollectionslookup",
                        method: "GET"
                    }),
                    'Failed to get collections lookup'
                );
            },

            getDashboardCollections: function (section) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getdashboardcollections",
                        method: "GET",
                        params: {
                            section: section
                        }
                    }),
                    'Failed to get dashboard collections'
                );
            },

            getCollectionByAlias: function (section, collectionAlias) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getcollectionbyalias",
                        method: "GET",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias
                        }
                    }),
                    'Failed to get collection by alias'
                );
            },

            getEntitiesByIds: function (section, collectionAlias, ids) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getentitiesbyids",
                        method: "GET",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias,
                            ids: ids.join(',')
                        }
                    }),
                    'Failed to get entities'
                );
            },

            getEntities: function (section, collectionAlias, options) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getentities", 
                        method: "GET",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias,
                            pageNumber: options.pageNumber,
                            pageSize: options.pageSize,
                            orderBy: options.orderBy,
                            orderDirection: options.orderDirection,
                            query: options.filter,
                            dataView: options.dataView
                        }
                    }),
                    'Failed to get entities'
                );
            },

            saveEntity: function(entity, isNew, files) {
                return umbRequestHelper.postSaveContent({
                    restApiUrl: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "saveentity",
                    content: entity,
                    action: "save",
                    files: files,
                    dataFormatter: function(c, a) {
                        return c;
                    }
                });
            },

            getEntityScaffold: function (section, collectionAlias) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getentityscaffold",
                        method: "GET",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias
                        }
                    }),
                    'Failed to get entity scaffold'
                );
            },

            getEntityById: function (section, collectionAlias, id) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getentitybyid",
                        method: "GET",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias,
                            id: id
                        }
                    }),
                    'Failed to get entity by id'
                );
            },

            deleteEntity: function (section, collectionAlias, id) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "deleteentity",
                        method: "DELETE",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias,
                            id: id
                        }
                    }),
                    'Failed to delete entity'
                );
            },

            getEntityTotalRecordCount: function (section, collectionAlias) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getentitytotalrecordcount",
                        method: "GET",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias
                        }
                    }),
                    'Failed to get entity total record count'
                );
            },

        } 

        return api;

    }

    angular.module("umbraco.resources").factory("fluidityResource", fluidityResource);

})();