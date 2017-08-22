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

            getListViewEntities: function (section, collectionAlias, options) {
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getlistviewentities", 
                        method: "GET",
                        params: {
                            section: section,
                            collectionAlias: collectionAlias,
                            pageNumber: options.pageNumber,
                            orderBy: options.orderBy,
                            orderDirection: options.orderDirection,
                            query: options.filter,
                            dataView: options.dataView
                        }
                    }),
                    'Failed to get list view entities'
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