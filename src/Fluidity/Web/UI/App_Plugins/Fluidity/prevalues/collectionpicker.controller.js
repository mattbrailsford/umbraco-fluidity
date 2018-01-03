// <copyright file="collectionpicker.controller.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function collectionPickerController($scope, fluidityResource) {

        $scope.properties = [
            {
                alias: "section",
                label: "Section",
                description: "Select the section the collection is located in",
                value: ""
            },
            {
                alias: "collection",
                label: "Collection",
                description: "Select the collection to pick items from",
                value: ""
            },
            {
                alias: "dataView",
                label: "Data view",
                description: "Select an optional data view to filter the collection with",
                value: ""
            }
        ];

        $scope.clearCollection = function() {
            $scope.properties[1].value = $scope.properties[2].value = '';
        }

        $scope.clearDataView = function () {
            $scope.properties[2].value = '';
        }

        fluidityResource.getCollectionsLookup().then(function (data) {
            $scope.data = data;

            if ($scope.model.value) {
                var aliases = $scope.model.value.split(',');

                if (aliases.length >= 1) {
                    var section = $scope.properties[0].value = _.find($scope.data, function(s) {
                        return s.alias == aliases[0];
                    });

                    if (aliases.length >= 2) {
                        var collection = $scope.properties[1].value = _.find(section.collections, function (c) {
                            return c.alias == aliases[1];
                        });

                        if (aliases.length >= 3) {
                            var dataView = $scope.properties[2].value = _.find(collection.dataViews, function (dv) {
                                return dv.alias == aliases[2];
                            });
                        }
                    }
                }
            }
        });

        var unsubscribe = $scope.$on("formSubmitting", function (ev, args) {
            var aliases = [];
            if ($scope.properties[0].value) {
                aliases.push($scope.properties[0].value.alias);
                if ($scope.properties[1].value) {
                    aliases.push($scope.properties[1].value.alias);
                    if ($scope.properties[2].value) {
                        aliases.push($scope.properties[2].value.alias);
                    }
                }
            }
            $scope.model.value = aliases.join();
        });

        //when the scope is destroyed we need to unsubscribe
        $scope.$on('$destroy', function () {
            unsubscribe();
        });

    }

    angular.module("umbraco").controller("Fluidity.Controllers.CollectionPickerController", collectionPickerController);

})();