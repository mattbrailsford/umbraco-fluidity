// <copyright file="dashboard.controller.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function dashboardController($scope, navigationService, fluidityResource) {

        // Sync tree to tree root
        fluidityResource.getSectionByAlias($scope.currentSection).then(function (data) {
            navigationService.syncTree({ tree: data.tree, path: ["-1"], forceReload: false });
        });

        // Get summary types
        fluidityResource.getDashboardCollections($scope.currentSection).then(function (data) {
            $scope.collections = data.map(function (itm) {
                itm.recordCount = "-";
                return itm;
            });

            if ($scope.collections.length > 0) {
                loadNextRecordCount(0);
            }
        });

        // Get total record counts
        function loadNextRecordCount(idx) {
            fluidityResource.getEntityTotalRecordCount($scope.currentSection, $scope.collections[idx].alias).then(function (count) {
                $scope.collections[idx].recordCount = count;
                if (idx < $scope.collections.length - 1) {
                    loadNextRecordCount(++idx);
                }
            });
        }

    }

    angular.module("umbraco").controller("Fluidity.Controllers.DashboardController", dashboardController);

})();