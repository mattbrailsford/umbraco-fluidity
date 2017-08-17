(function () {

    'use strict';

    function fluidityOverrides($provide) {

        $provide.decorator('$controller', function ($delegate, $location) {

            return function (constructor, locals) {
                var ctrl = $delegate(constructor, locals);

                // Check for an ngController attribute
                if (locals.$attrs && locals.$attrs.ngController) {

                    // Override Umbraco.PropertyEditors.ListView.ListLayoutController.clickItem method
                    if (locals.$attrs.ngController.match(/^Umbraco\.PropertyEditors\.ListView\.ListLayoutController\b/i)) {
                        var baseClickItem = ctrl.clickItem;
                        ctrl.clickItem = function (item) {
                            if (item.editPath) {
                                $location.path(item.editPath);
                            } else {
                                baseClickItem(item);
                            }
                        };
                    }

                    // Override Umbraco.PropertyEditors.ListView.GridLayoutController.goToItem method
                    if (locals.$attrs.ngController.match(/^Umbraco\.PropertyEditors\.ListView\.GridLayoutController\b/i)) {
                        var baseGoToItem = ctrl.goToItem;
                        ctrl.goToItem = function(item) {
                            if (item.editPath) {
                                $location.path(item.editPath);
                            } else {
                                baseGoToItem(item);
                            }
                        };
                    }

                }

                return ctrl;
            }

        });

    }

    angular.module("umbraco").config(['$provide', fluidityOverrides]);

})();