// <copyright file="list.controller.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function listViewController($scope, $routeParams, $http, $q, editorState, navigationService, umbRequestHelper, contentEditingHelper, localizationService, fluidityResource) {

        // Parse the id route param
        var collectionAlias = $routeParams.id;

        $scope.page = {};
        $scope.page.loading = false;
        $scope.page.menu = {};
        $scope.page.menu.currentNode = null;
        $scope.page.menu.currentSection = $scope.currentSection;

        $scope.listView = {
            options: {
                isSearchable: true,
                pageSize: 10,
                defaultOrderBy: "Name",
                defaultOrderDirection: "desc",
                bulkActions: [],
                dataViews: [],
                layouts: [],
                properties: []
            },
            getItems: function(id, options) {
                return fluidityResource.getEntities($scope.collection.section, $scope.collection.alias, options);
            }
        }

        function init(collection) {
            $scope.page.name = collection.namePlural;
            $scope.listView.options.isSearchable          = collection.isSearchable;
            $scope.listView.options.pageSize              = collection.listView.pageSize;
            $scope.listView.options.defaultOrderBy        = collection.listView.defaultOrderBy;
            $scope.listView.options.defaultOrderDirection = collection.listView.defaultOrderDirection;
            $scope.listView.options.bulkActions           = collection.listView.bulkActions;
            $scope.listView.options.dataViews             = collection.listView.dataViews;
            $scope.listView.options.layouts               = collection.listView.layouts;
            $scope.listView.options.properties            = collection.listView.properties;
        }

        function syncTree(entity, path, initialLoad) {
            navigationService.syncTree({ tree: entity.tree, path: path.split(","), forceReload: initialLoad !== true }).then(function (syncArgs) {
                $scope.page.menu.currentNode = syncArgs.node;
            });
        }

        $scope.page.loading = true;

        // Get the collection data
        fluidityResource.getCollectionByAlias($scope.currentSection, collectionAlias).then(function (data) {
            $scope.collection = data;
            init($scope.collection);
            syncTree($scope.collection, $scope.collection.path, true);
            $scope.page.loading = false;
        });

    }

    angular.module("umbraco").controller("Fluidity.Controllers.ListViewController", listViewController);

})();