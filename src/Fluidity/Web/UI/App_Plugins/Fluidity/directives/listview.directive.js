(function () {

    'use strict';

    function fluidityListView($routeParams, listViewHelper, fluidityUtilityService) {

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
                pageNumber: ($routeParams.page && Number($routeParams.page) != NaN && Number($routeParams.page) > 0) ? $routeParams.page : 1,
                orderBy: "name",
                orderDirection: "desc",
                filter: '', // Variable has to be named "filter" to work with list view properly
                dataView: fluidityUtilityService.recallDataView($routeParams.id, $scope.opts.dataViews), // TODO: Remember the dataview like how the layout persists
                dataViews: $scope.opts.dataViews,
                orderBySystemField: true,
                includeProperties: $scope.opts.properties,
                layout: {
                    layouts: $scope.opts.layouts,
                    activeLayout: listViewHelper.getLayout($routeParams.id, $scope.opts.layouts)
                },
                bulkActionsAllowed: true // TODO: Check for any actions?
            });

            var searchListView = _.debounce(function () {
                $scope.$apply(function () {
                    makeSearch();
                });
            }, 500);

            function makeSearch() {
                if ($scope.options.filter !== null && $scope.options.filter !== undefined) {
                    $scope.options.pageNumber = 1;
                    $scope.reloadView($scope.contentId);
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


            $scope.changeDataView = function () {
                $scope.options.pageNumber = 1;
                fluidityUtilityService.rememberDataView($routeParams.id, $scope.options.dataView, $scope.options.dataViews);
                $scope.reloadView($scope.contentId);
            };

            $scope.isAnythingSelected = function() {
                return false;
            }

            $scope.selectLayout = function (selectedLayout) {
                $scope.options.layout.activeLayout = listViewHelper.setLayout($routeParams.id, selectedLayout, $scope.options.layout.layouts);
            }

            $scope.reloadView = function (id) {

                $scope.viewLoaded = false;

                if ($scope.onGetItems) {

                    listViewHelper.clearSelection($scope.listViewResultSet.items, $scope.folders, $scope.selection);

                    $scope.onGetItems(id, $scope.options).then(function (data) {

                        $scope.actionInProgress = false;
                        $scope.listViewResultSet = data;

                        // Clear out items collection if there aren't any items
                        if ($scope.listViewResultSet.totalItems === 0) {
                            $scope.listViewResultSet.items = false;
                        }

                        // Copy all property values to be object level variables
                        if ($scope.listViewResultSet.items) {
                            _.each($scope.listViewResultSet.items, function (e, index) {
                                _.each(e.properties, function (p, index) {
                                    e[p.alias] = p.value;
                                });
                            });
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
            templateUrl: '/app_plugins/fluidity/directives/listview.html',
            scope: {
                collection: '=',
                opts: '=options',
                onGetItems: '=',
            },
            link: link
        };

        return directive;

    }

    angular.module("umbraco.directives").directive("fluidityListView", fluidityListView);

})();