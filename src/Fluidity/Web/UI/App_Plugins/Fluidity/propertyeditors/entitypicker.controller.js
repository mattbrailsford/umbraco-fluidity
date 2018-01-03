// <copyright file="entitypicker.controller.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function entityPickerController($scope, editorState, angularHelper, fluidityResource) {

        var aliases = $scope.model.config.collection.split(',');
        var sectionAlias = aliases.length >= 1 ? aliases[0] : "";
        var collectionAlias = aliases.length >= 2 ? aliases[1] : "";
        var dataView = aliases.length >= 3 ? aliases[2] : "";

        $scope.renderModel = [];
        $scope.dialogEditor = editorState && editorState.current && editorState.current.isDialogEditor;

        // Sortable options
        $scope.sortableOptions = {
            distance: 10,
            tolerance: "pointer",
            scroll: true,
            zIndex: 6000
        };

        // Dialog options
        var dialogOptions = {
            multiPicker: $scope.model.config.maxItems > 1,
            section: sectionAlias,
            collection: collectionAlias,
            dataView: dataView
        };

        $scope.openEntityPicker = function () {
            $scope.entityPickerOverlay = dialogOptions;
            $scope.entityPickerOverlay.view = "/app_plugins/fluidity/dialogs/entitypicker.html";
            $scope.entityPickerOverlay.show = true;

            $scope.entityPickerOverlay.submit = function (model) {

                if (angular.isArray(model.selection)) {
                    _.each(model.selection, function (item, i) {
                        $scope.add(item);
                    });
                }

                $scope.entityPickerOverlay.show = false;
                $scope.entityPickerOverlay = null;
            }

            $scope.entityPickerOverlay.close = function (oldModel) {
                $scope.entityPickerOverlay.show = false;
                $scope.entityPickerOverlay = null;
            }

        };

        $scope.add = function (item) {
            var currIds = _.map($scope.renderModel, function (i) {
                return i.id;
            });

            var itemId = item.id;

            if (currIds.indexOf(itemId) < 0) {
                $scope.renderModel.push({
                    "name": item.name,
                    "id": item.id,
                    "section": item.section,
                    "collection": item.collection,
                    "icon": item.icon,
                    "path": item.path,
                    "url": "",
                    "published": true
                    // only content supports published/unpublished content so we set everything else to published so the UI looks correct 
                });
            }
        };

        $scope.remove = function (index) {
            $scope.renderModel.splice(index, 1);
            angularHelper.getCurrentForm($scope).$setDirty();
        };

        $scope.clear = function () {
            $scope.renderModel = [];
        };

        function trim(str, chr) {
            var rgxtrim = (!chr) ? new RegExp('^\\s+|\\s+$', 'g') : new RegExp('^' + chr + '+|' + chr + '+$', 'g');
            return str.replace(rgxtrim, '');
        }

        function startWatch() {
            //We need to watch our renderModel so that we can update the underlying $scope.model.value properly, this is required
            // because the ui-sortable doesn't dispatch an event after the digest of the sort operation. Any of the events for UI sortable
            // occur after the DOM has updated but BEFORE the digest has occured so the model has NOT changed yet - it even states so in the docs.
            // In their source code there is no event so we need to just subscribe to our model changes here.
            //This also makes it easier to manage models, we update one and the rest will just work.
            $scope.$watch(function () {
                //return the joined Ids as a string to watch
                return _.map($scope.renderModel, function (i) {
                    return i.id;
                }).join();
            }, function (newVal) {
                $scope.model.value = trim(newVal, ",");

                //Validate!
                if ($scope.model.config && $scope.model.config.minItems && parseInt($scope.model.config.minItems) > $scope.renderModel.length) {
                    $scope.entityPickerForm.minCount.$setValidity("minCount", false);
                }
                else {
                    $scope.entityPickerForm.minCount.$setValidity("minCount", true);
                }

                if ($scope.model.config && $scope.model.config.maxItems && parseInt($scope.model.config.maxItems) < $scope.renderModel.length) {
                    $scope.entityPickerForm.maxCount.$setValidity("maxCount", false);
                }
                else {
                    $scope.entityPickerForm.maxCount.$setValidity("maxCount", true);
                }

                setSortingState($scope.renderModel);

                angularHelper.getCurrentForm($scope).$setDirty();

            });
        }

        function setSortingState(items) {
            // disable sorting if the list only consist of one item
            if (items.length > 1) {
                $scope.sortableOptions.disabled = false;
            } else {
                $scope.sortableOptions.disabled = true;
            }
        }

        // Update model on submit
        var unsubscribe = $scope.$on("formSubmitting", function (ev, args) {
            var currIds = _.map($scope.renderModel, function (i) {
                return i.id;
            });
            $scope.model.value = trim(currIds.join(), ",");
        });

        // When the scope is destroyed we need to unsubscribe
        $scope.$on('$destroy', function () {
            unsubscribe();
        });

        // Initialize
        var modelIds = $scope.model.value ? $scope.model.value.split(',') : [];

        // Load current data if anything selected
        if (modelIds.length > 0) {
            fluidityResource.getEntitiesByIds(sectionAlias, collectionAlias, modelIds).then(function (data) {

                // Add the entites to the renderModel making sure
                // they maintain their stored order
                _.each(modelIds, function (id, i) {
                    var itm = _.find(data, function(o) {
                        return o.id == id;
                    });
                    if (itm) {
                        $scope.add(itm);
                    }
                });

                // Everything is loaded, start the watch on the model
                startWatch();

            });
        }
        else
        {
            // Everything is loaded, start the watch on the model
            startWatch();
        }

    }

    angular.module("umbraco").controller("Fluidity.Controllers.EntityPickerController", entityPickerController);

})();