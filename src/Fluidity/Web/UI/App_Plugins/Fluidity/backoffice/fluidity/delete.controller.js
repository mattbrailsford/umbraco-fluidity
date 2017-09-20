// <copyright file="delete.controller.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function deleteController($scope, fluidityResource, treeService, navigationService, editorState, $location, dialogService, notificationsService) {

        $scope.performDelete = function() {

            // Show the loading animation
            $scope.currentNode.loading = true;

            // Perform the delete
            fluidityResource.deleteEntity($scope.currentNode.section,
                $scope.currentNode.parentId,
                $scope.currentNode.metaData.entityId).then(function () {

                // Remove the current node from the tree
                treeService.removeNode($scope.currentNode);

                // If the current edited item is the same one as we're deleting, we need to navigate elsewhere
                if (editorState.current && editorState.current.id == $scope.currentNode.id) {

                    // Just navigate to the section dashboard
                    $location.path("/" + $scope.currentNode.metaData.application);

                }

                navigationService.hideMenu();

            }, function (err) {

                // Hide the loading animation
                $scope.currentNode.loading = false;

                // If error was a YSOD show it
                if (err.status && err.status >= 500) {
                    dialogService.ysodDialog(err);
                }

                // Show any error notifications
                if (err.data && angular.isArray(err.data.notifications)) {
                    for (var i = 0; i < err.data.notifications.length; i++) {
                        notificationsService.showNotification(err.data.notifications[i]);
                    }
                }

            });

        };

        $scope.cancel = function() {
            navigationService.hideDialog();
        };
    }

    angular.module("umbraco").controller("Fluidity.Controllers.DeleteController", deleteController);

})();