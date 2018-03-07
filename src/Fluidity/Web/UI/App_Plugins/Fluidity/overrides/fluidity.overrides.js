// <copyright file="fluidity.overrides.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    // Umbraco overrides
    function fluidityUmbracoOverrides($provide) {

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
                                $location.url(item.editPath);
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
                                $location.url(item.editPath);
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

    angular.module("umbraco").config(['$provide', fluidityUmbracoOverrides]);

    // Umbraco services overrides
    function fluidityUmbracoServicesOverrides($provide) {

        $provide.decorator('contentEditingHelper', function ($delegate) {

            // Override handleSuccessfulSave and look for redirectId in scope
            // instead of just args as args is not modifiable like scope is.
            // We need this as we use a non standard ID structure in urls so
            // need to provide an explicit redirect ID
            var oldHandleSuccessfulSave = $delegate.handleSuccessfulSave;
            $delegate.handleSuccessfulSave = function (args) {
                if (args.scope.redirectId) {
                    args.redirectId = args.scope.redirectId;
                }
                oldHandleSuccessfulSave.apply($delegate, arguments);
            };

            return $delegate;

        });

    }

    angular.module("umbraco.services").config(['$provide', fluidityUmbracoServicesOverrides]);

    // Umbraco resource overrides
    function fluidityUmbracoResourceOverrides($provide) {

        $provide.decorator('entityResource', function ($delegate) {

            // When using MNTP with an xpath filter with fluidity
            // a call to getByQuery is made using the $routeParams.id
            // as context. Unfortunately it expects that variable to
            // be an int, which in fluidity's case is not, so we 
            // intercept the call and just set it to the root id.
            var oldGetByQuery = $delegate.getByQuery;
            $delegate.getByQuery = function () {
                if (arguments.length >= 2 && arguments[1].toString().indexOf('!') >= 0) { // '!' signifies a fluidity composite id
                    arguments[1] = -1;
                }
                return oldGetByQuery.apply($delegate, arguments);
            };

            return $delegate;

        });

    }

    angular.module("umbraco.resources").config(['$provide', fluidityUmbracoResourceOverrides]);

})();