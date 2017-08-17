(function () {

    'use strict';

    function fluidityResource($http, umbRequestHelper) {

        var api = {

            getCollectionByAlias: function (section, collectionAlias) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getcollectionbyalias?section=" + section + "&collectionAlias=" + collectionAlias),
                    'Failed to get collection by alias'
                );
            },

            getListViewEntities: function (section, collectionAlias, options) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getlistviewentities?section=" + section + "&collectionAlias=" + collectionAlias + "&options=" + options),
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
                    $http.get(Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getentityscaffold?section=" + section + "&collectionAlias=" + collectionAlias),
                    'Failed to get entity scaffold'
                );
            },

            getEntityById: function (section, collectionAlias, id) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "getentitybyid?section=" + section + "&collectionAlias=" + collectionAlias + "&id=" + id),
                    'Failed to get entity by id'
                );
            },

            deleteEntity: function (section, collectionAlias, id) {
                return api.deleteEntities(section, collectionAlias, [id]);
            },

            deleteEntities: function (section, collectionAlias, ids) {
                return umbRequestHelper.resourcePromise(
                    $http.delete(Umbraco.Sys.ServerVariables.fluidity.apiBaseUrl + "delete?section=" + section + "&collectionAlias=" + collectionAlias + "&ids=" + ids.join(',')),
                    'Failed to delete entity'
                );
            },

        } 

        return api;

    }

    angular.module("umbraco.resources").factory("fluidityResource", fluidityResource);

})();