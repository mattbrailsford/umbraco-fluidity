// <copyright file="minilistview.directive.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function fluidityMiniListView($routeParams, $injector, $timeout, listViewHelper) {

        function link($scope, el, attr, ctrl) {

            $scope.entityId = $routeParams.id;

            $scope.pagination = [];
            $scope.actionInProgress = false;
            $scope.selection = [];
            $scope.listViewResultSet = {
                totalPages: 1,
                items: []
            };

            $scope.options = angular.extend({}, {
                pageNumber: 1,
                pageSize: 10,
                orderBy: "name",
                orderDirection: "desc",
                filter: '', // Variable has to be named "filter" to work with list view properly
                dataView: $scope.opts.dataView,
                isSearchable: $scope.opts.isSearchable
            });

            var searchListView = _.debounce(function () {
                $scope.$apply(function () {
                    makeSearch();
                });
            }, 500);

            function makeSearch() {
                if ($scope.options.filter !== null && $scope.options.filter !== undefined) {
                    $scope.options.pageNumber = 1;
                    $scope.reloadView($scope.entityId);
                }
            }
            
            $scope.forceSearch = function (ev) {
                //13: enter
                switch (ev.keyCode) {
                    case 13:
                        makeSearch();
                        break;
                }
            };

            $scope.enterSearch = function () {
                $scope.viewLoaded = false;
                searchListView();
            };

            $scope.selectNode = function (item) {
                if ($scope.onSelectItem) {
                    $scope.onSelectItem(item);
                }
            };

            $scope.nextPage = function (pageNumber) {
                $scope.options.pageNumber = pageNumber;
                $scope.reloadView($scope.entityId);
            };

            $scope.goToPage = function (pageNumber) {
                $scope.options.pageNumber = pageNumber;
                $scope.reloadView($scope.entityId);
            };

            $scope.prevPage = function (pageNumber) {
                $scope.options.pageNumber = pageNumber;
                $scope.reloadView($scope.entityId);
            };

            $scope.reloadView = function (id) {

                $scope.viewLoaded = false;

                if ($scope.onGetItems) {

                    listViewHelper.clearSelection($scope.listViewResultSet.items, $scope.folders, $scope.selection);

                    $scope.onGetItems(id, $scope.options).then(function (data) {

                        $scope.listViewResultSet = data;

                        // Clear out items collection if there aren't any items
                        if ($scope.listViewResultSet.totalItems === 0) {
                            $scope.listViewResultSet.items = false;
                        }

                        $scope.viewLoaded = true;

                        // NOTE: This might occur if we are requesting a higher page number than what is actually available, for example
                        // if you have more than one page and you delete all items on the last page. In this case, we need to reset to the last
                        // available page and then re-load again
                        if ($scope.listViewResultSet.totalPages > 0 && $scope.options.pageNumber > $scope.listViewResultSet.totalPages) {

                            // Reset pagnumber to max pages
                            $scope.options.pageNumber = $scope.listViewResultSet.totalPages;

                            // Reload!
                            $scope.reloadView(id);
                        }

                    });
                }
            }

            $scope.getContent = function () {
                $scope.reloadView($scope.entityId);
            }

            $scope.getContent();
        }

        var directive = {
            restrict: 'E',
            replace: true,
            templateUrl: '/app_plugins/fluidity/directives/minilistview.html',
            scope: {
                opts: '=options',
                onGetItems: '=',
                onSelectItem: '=',
            },
            link: link
        };

        return directive;

    }

    angular.module("umbraco.directives").directive("fluidityMiniListView", fluidityMiniListView);

})();