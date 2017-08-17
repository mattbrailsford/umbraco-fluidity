(function () {

    'use strict';

    function fluidityListView($routeParams, listViewHelper) {

        function link($scope, el, attr, ctrl) {

            $scope.entityId = $routeParams.id;

            $scope.pagination = [];
            $scope.actionInProgress = false;
            $scope.selection = [];
            $scope.listViewResultSet = {
                totalPages: 1,
                items: [
                {
                    id: "person!1",
                    icon: "icon-umb-users",
                    name: "Test",
                    FirstName: "Matt",
                    Age: "26",
                    DateOfBirth: "1980-03-19",
                    editPath: "database/fluidity/edit/person!2"
                }]
            };

            $scope.options = angular.extend({}, {
                pageSize: 10,
                pageNumber: ($routeParams.page && Number($routeParams.page) != NaN && Number($routeParams.page) > 0) ? $routeParams.page : 1,
                filter: '',
                orderBy: "Name",
                orderDirection: "desc",
                orderBySystemField: true,
                includeProperties: $scope.opts.properties,
                layout: {
                    layouts: $scope.opts.layouts,
                    activeLayout: listViewHelper.getLayout($routeParams.id, $scope.opts.layouts)
                },
                bulkActionsAllowed: true // TODO: Check for any actions?
            });

            $scope.isAnythingSelected = function() {
                return false;
            }

            $scope.selectLayout = function (selectedLayout) {
                $scope.options.layout.activeLayout = listViewHelper.setLayout($routeParams.id, selectedLayout, $scope.options.layout.layouts);
            }

            $scope.reloadView = function (id) {

                $scope.viewLoaded = false;

                console.log($scope.onGetItems);

                if ($scope.onGetItems) {

                    listViewHelper.clearSelection($scope.listViewResultSet.items, $scope.folders, $scope.selection);

                    $scope.onGetItems(id, $scope.options).then(function (data) {

                        $scope.actionInProgress = false;
                        $scope.listViewResultSet = data;

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
                        if ($scope.options.pageNumber > $scope.listViewResultSet.totalPages) {

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