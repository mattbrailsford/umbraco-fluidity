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
                bulkActions: [],
                dataViews: [],
                layouts: [],
                properties: []
            },
            getItems: function(id, options) {
                return fluidityResource.getListViewEntities($scope.collection.section, $scope.collection.alias, options);
            }
        }

        function init(collection) {
            $scope.page.name = collection.namePlural;
            $scope.listView.options.bulkActions = collection.listView.bulkActions;
            $scope.listView.options.dataViews   = collection.listView.dataViews;
            $scope.listView.options.layouts     = collection.listView.layouts;
            $scope.listView.options.properties  = collection.listView.properties;
            $scope.listView.options.isSearchable = collection.listView.isSearchable;
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