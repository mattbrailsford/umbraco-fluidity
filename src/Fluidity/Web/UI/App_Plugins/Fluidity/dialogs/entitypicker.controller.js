// <copyright file="entitypicker.controller.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function entityPickerDialogController($scope, editorState, angularHelper, fluidityResource) {

        var dialogOptions = $scope.model;

        $scope.model.selection = [];

        $scope.listView = {
            options: {
                dataView: dialogOptions.dataView,
                isSearchable: false
            },
            getItems: function (id, options) {
                return fluidityResource.getEntities(dialogOptions.section, dialogOptions.collection, options).then(function (data) {

                    // Repopulate selection
                    _.each(data.items, function(itm) {
                        var selectedItem = _.find($scope.model.selection, function(o) {
                            return o.id === itm.id;
                        });
                        if (selectedItem) {
                            itm.selected = true;
                        }
                    });

                    return data;
                });
            },
            selectItem: function(itm) {
                itm.selected = itm.selected === true ? false : true;
                if (itm.selected) {
                    $scope.model.selection.push(itm);
                    if (!$scope.model.multiPicker) {
                        $scope.model.submit($scope.model);
                    }
                } else {
                    $scope.model.selection = _.reject($scope.model.selection, function(o) {
                        return o.id === itm.id;
                    });
                }
            }
        }

        function init(collection) {
            $scope.listView.options.isSearchable = collection.isSearchable;
            $scope.collection = collection;
        }

        // Get the collection data
        fluidityResource.getCollectionByAlias(dialogOptions.section, dialogOptions.collection).then(function (data) {
            init(data);
        });

    }

    angular.module("umbraco").controller("Fluidity.Controllers.EntityPickerDialogController", entityPickerDialogController);

})();