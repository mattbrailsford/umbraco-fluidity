// <copyright file="fluidity.bulkactions.service.js" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

(function () {

    'use strict';

    function fluidityDeleteBulkAction(fluidityResource) {

        return {
            
            performAction: function(section, collection, id) {
                return fluidityResource.deleteEntity(section, collection, id);
            },

            getConfirmMessage: function (count) {
                return "Are you sure you want to delete " + count + " items?";
            },

            getProgressMessage: function(count, total) {
                return count + " of " + total + " items deleted";
            },

            getCompleteMessage: function(total) {
                return total + " items successfully deleted";
            }

        }

    }

    angular.module("umbraco.services").factory("fluidityDeleteBulkActionService", fluidityDeleteBulkAction);

})();