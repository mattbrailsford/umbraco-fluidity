(function () {

    'use strict';

    function collectionPickerController($scope, fluidityResource) {

        fluidityResource.getCollectionsLookup().then(function (data) {
            $scope.collections = data;
        });

    }

    angular.module("umbraco").controller("Fluidity.Controllers.CollectionPickerController", collectionPickerController);

})();