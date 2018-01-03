// <copyright file="edit.controller.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function editController($scope, $routeParams, $http, $q, editorState, navigationService, umbRequestHelper, contentEditingHelper, localizationService, fluidityResource) {

        // Parse the id route param
        var idParts = $routeParams.id.split("!");
        var collectionAlias = idParts[0];
        var id = idParts.length > 1 ? idParts[1] : "0";
        var isNew = id == "0";

        // Fake the create route param as some of the umbraco
        // helpers expect it to be there
        $routeParams.create = isNew;

        //TODO: Handle returning the an entity with a sub list view on it
        //TODO: Handle entity menu items
        var localizeSaving = localizationService.localize("general_saving");

        // Setup the page (mimicing how the content editor works)
        $scope.page = {};
        $scope.page.loading = false;
        $scope.page.menu = {};
        $scope.page.menu.currentNode = null;
        $scope.page.menu.currentSection = $scope.currentSection;
        $scope.page.listViewPath = null;
        $scope.page.isNew = isNew;
        $scope.page.saveButtonState = "init";

        function syncTree(entity, path, initialLoad) {
            if (entity.isChildOfTreeView) {
                navigationService.syncTree({ tree: entity.tree, path: path.split(","), forceReload: initialLoad !== true }).then(function (syncArgs) {
                    $scope.page.menu.currentNode = syncArgs.node;
                });
            }
            else if (initialLoad === true) {

                // it's a child item, just sync the ui node to the parent
                navigationService.syncTree({ tree: entity.tree, path: path.substring(0, path.lastIndexOf(",")).split(","), forceReload: initialLoad !== true });

                // if this is a child of a list view and it's the initial load of the editor, we need to get the tree node 
                // from the server so that we can load in the actions menu.
                umbRequestHelper.resourcePromise($http.get(entity.treeNodeUrl),
                    'Failed to retrieve data for child node ' + entity.id).then(function (node) {
                        $scope.page.menu.currentNode = node;
                    });
            }
        }

        // Load the editor configuration
        if (isNew) {

            $scope.page.loading = true;

            // Create an empty item
            fluidityResource.getEntityScaffold($scope.currentSection, collectionAlias).then(function(data) {
                $scope.content = data;
                editorState.set($scope.content);
                $scope.page.loading = false;
            });

        } else {
            
            $scope.page.loading = true;

            // Create an empty item
            fluidityResource.getEntityById($scope.currentSection, collectionAlias, id).then(function (data) {
                $scope.content = data;
                if (data.isChildOfListView) {
                    $scope.page.listViewPath = ($routeParams.page)
                        ? "/" + $scope.currentSection + "/fluidity/list/" + $scope.content.collection + "?page=" + $routeParams.page
                        : "/" + $scope.currentSection + "/fluidity/list/" + $scope.content.collection;
                }
                editorState.set($scope.content);
                syncTree($scope.content, $scope.content.path, true);
                $scope.page.loading = false;
            });

        }

        $scope.save = function() {

            $scope.page.saveButtonState = "busy";

            contentEditingHelper.contentEditorPerformSave({
                statusMessage: localizeSaving,
                saveMethod: function (entity, isNew, files) {

                    var postModel = {
                        section: entity.section,
                        collection: entity.collection,
                        id: entity.id,
                        name: entity.name,
                        properties: []
                    };

                    _.each(entity.tabs, function (tab) {
                        _.each(tab.properties, function (prop) {
                            postModel.properties.push({
                                id: prop.id,
                                alias: prop.alias,
                                value: prop.value
                            });
                        });
                    });

                    return fluidityResource.saveEntity(postModel, isNew, files).then(function (savedEntity) {
                        
                        // Stash a composite in scope as the contentEditorPerformSave will
                        // perform a redirect to the edit screen if an entity is new but 
                        // fluidity uses a componsite ID and not just the entity id
                        $scope.redirectId = savedEntity.collection + "!" + savedEntity.id;

                        return savedEntity;

                    });
                    
                },
                scope: $scope,
                content: $scope.content,
                // We do not redirect on failure for entities - this is because it is not possible to actually save the entity
                // when server side validation fails - as opposed to umbraco content where we are capable of saving the content
                // item if server side validation fails
                redirectOnFailure: false
            }).then(function (data) {
                // success
                editorState.set($scope.content);
                syncTree($scope.content, data.path);
                $scope.page.saveButtonState = "success";
            }, function (err) {
                // error
                if (err) {
                    editorState.set($scope.content);
                }
                $scope.page.saveButtonState = "error";
            });
        }

    }

    angular.module("umbraco").controller("Fluidity.Controllers.EditController", editController);

})();