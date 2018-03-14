// <copyright file="listview.directive.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function fluidityListView($routeParams, $injector, $timeout, listViewHelper, notificationsService, localizationService, fluidityUtilityService) {

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
                pageSize: $scope.opts.pageSize,
                orderBy: $scope.opts.defaultOrderBy || "name",
                orderDirection: $scope.opts.defaultOrderDirection || "desc",
                filter: '', // Variable has to be named "filter" to work with list view properly
                dataView: fluidityUtilityService.recallDataView($routeParams.id, $scope.opts.dataViews), // TODO: Remember the dataview like how the layout persists
                dataViews: $scope.opts.dataViews,
                bulkActions: $scope.opts.bulkActions,
                bulkActionsAllowed: $scope.opts.bulkActions && $scope.opts.bulkActions.length > 0,
                isSearchable: $scope.opts.isSearchable,
                orderBySystemField: true,
                includeProperties: $scope.opts.properties,
                layout: {
                    layouts: $scope.opts.layouts,
                    activeLayout: listViewHelper.getLayout($routeParams.id, $scope.opts.layouts)
                }
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

            function showNotificationsAndReset(err, reload, successMsg) {

                //check if response is ysod
                if (err.status && err.status >= 500) {

                    // Open ysod overlay
                    $scope.ysodOverlay = {
                        view: "ysod",
                        error: err,
                        show: true
                    };
                }

                $timeout(function () {
                    $scope.bulkStatus = "";
                    $scope.actionInProgress = false;
                },
                    500);

                if (reload === true) {
                    $scope.reloadView($scope.entityId);
                }

                if (err.data && angular.isArray(err.data.notifications)) {
                    for (var i = 0; i < err.data.notifications.length; i++) {
                        notificationsService.showNotification(err.data.notifications[i]);
                    }
                } else if (successMsg) {
                    localizationService.localize("bulk_done")
                        .then(function (v) {
                            notificationsService.success(v, successMsg);
                        });
                }
            }

            function serial(selected, fn, getProgressMsg, index) {
                return fn(selected, index).then(function (content) {
                    index++;
                    $scope.bulkStatus = getProgressMsg(index, selected.length);
                    return index < selected.length ? serial(selected, fn, getProgressMsg, index) : content;
                }, function (err) {
                    var reload = index > 0;
                    showNotificationsAndReset(err, reload);
                    return err;
                });
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
                $scope.reloadView($scope.entityId);
            };

            $scope.isAnythingSelected = function() {
                return $scope.selection.length !== 0;
            }

            $scope.clearSelection = function () {
                listViewHelper.clearSelection($scope.listViewResultSet.items, false, $scope.selection);
            };

            $scope.performBulkAction = function (bulkAction) {

                var selected = $scope.selection;
                if (selected.length === 0)
                    return;

                if (!bulkAction.angularServiceName)
                    return; // TODO: Error

                var bulkActionService = $injector.get(bulkAction.angularServiceName);
                if (!bulkActionService || !bulkActionService.performAction)
                    return; // TODO: Error


                if (bulkActionService.getConfirmMessage && !confirm(bulkActionService.getConfirmMessage(selected.length)))
                    return;

                var getProgressMsg = bulkActionService.getProgressMessage || function(count, total) {
                    return count + " of " + total + " items processed";
                }

                var getCompleteMsg = bulkActionService.getCompleteMessage || function (total) {
                    return total + " items successfully processed";
                }

                $scope.actionInProgress = true;
                $scope.bulkStatus = getProgressMsg(0, selected.length);

                return serial(selected, function (selected, index) {
                    return bulkActionService.performAction($scope.collection.section, $scope.collection.alias, selected[index].id);
                }, getProgressMsg, 0).then(function (result) {
                    // executes once the whole selection has been processed
                    // in case of an error (caught by serial), result will be the error
                    if (!(result.data && angular.isArray(result.data.notifications)))
                        showNotificationsAndReset(result, true, getCompleteMsg(selected.length));
                });
            }

            $scope.selectLayout = function (selectedLayout) {
                $scope.options.layout.activeLayout = listViewHelper.setLayout($routeParams.id, selectedLayout, $scope.options.layout.layouts);
            }

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