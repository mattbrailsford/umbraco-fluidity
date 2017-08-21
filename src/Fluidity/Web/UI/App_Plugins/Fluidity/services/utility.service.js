/**
 * Compares two software version numbers (e.g. "1.7.1" or "1.2b").
 *
 *
 * @param {string} v1 The first version to be compared.
 * @param {string} v2 The second version to be compared.
 * @param {object} [options] Optional flags that affect comparison behavior:
 * <ul>
 *     <li>
 *         <tt>lexicographical: true</tt> compares each part of the version strings lexicographically instead of
 *         naturally; this allows suffixes such as "b" or "dev" but will cause "1.10" to be considered smaller than
 *         "1.2".
 *     </li>
 *     <li>
 *         <tt>zeroExtend: true</tt> changes the result if one version string has less parts than the other. In
 *         this case the shorter string will be padded with "zero" parts instead of being considered smaller.
 *     </li>
 * </ul>
 * @returns {number|NaN}
 * <ul>
 *    <li>0 if the versions are equal</li>
 *    <li>a negative integer iff v1 < v2</li>
 *    <li>a positive integer iff v1 > v2</li>
 *    <li>NaN if either version string is in the wrong format</li>
 * </ul>
 */
(function () {
    'use strict';

    function fluidityUtilityService(localStorageService) {

        var dataViewLocalStorageKey = "fluidityDataViewes";

        function saveDataViewInLocalStorage(nodeId, selectedDataViewAlias) {
            var dataViewFound = false;
            var storedDataViews = [];

            if (localStorageService.get(dataViewLocalStorageKey)) {
                storedDataViews = localStorageService.get(dataViewLocalStorageKey);
            }

            if (storedDataViews.length > 0) {
                for (var i = 0; storedDataViews.length > i; i++) {
                    var entry = storedDataViews[i];
                    if (entry.nodeId === nodeId) {
                        entry.alias = selectedDataViewAlias;
                        dataViewFound = true;
                    }
                }
            }

            if (!dataViewFound) {
                var storageObject = {
                    "nodeId": nodeId,
                    "alias": selectedDataViewAlias
                };
                storedDataViews.push(storageObject);
            }

            localStorageService.set(dataViewLocalStorageKey, storedDataViews);

        }

        var service = {

            rememberDataView: function (nodeId, selectedDataViewAlias, availableDataViews) {
                
                var activeDataView = {};
                var dataViewFound = false;

                for (var i = 0; availableDataViews.length > i; i++) {
                    var dataView = availableDataViews[i];
                    if (dataView.alias === selectedDataViewAlias) {
                        activeDataView = dataView;
                        dataViewFound = true;
                    }
                }

                if (!dataViewFound && availableDataViews.length > 0) {
                    activeDataView = availableDataViews[0];
                }

                saveDataViewInLocalStorage(nodeId, activeDataView.alias);

                return activeDataView.alias;

            },

            recallDataView: function (nodeId, availableDataViews) {
                
                var storedDataViews = [];

                if (localStorageService.get(dataViewLocalStorageKey)) {
                    storedDataViews = localStorageService.get(dataViewLocalStorageKey);
                }

                if (storedDataViews && storedDataViews.length > 0) {
                    for (var i = 0; storedDataViews.length > i; i++) {
                        var entry = storedDataViews[i];
                        if (entry.nodeId === nodeId) {
                            return service.rememberDataView(nodeId, entry.alias, availableDataViews);
                        }
                    }

                }

                return availableDataViews[0].alias;

            },

            compareCurrentUmbracoVersion: function compareCurrentUmbracoVersion(v, options) {
                return this.compareVersions(Umbraco.Sys.ServerVariables.application.version, v, options);
            },

            compareVersions: function compareVersions(v1, v2, options) {

                var lexicographical = options && options.lexicographical,
                    zeroExtend = options && options.zeroExtend,
                    v1parts = v1.split('.'),
                    v2parts = v2.split('.');

                function isValidPart(x) {
                    return (lexicographical ? /^\d+[A-Za-z]*$/ : /^\d+$/).test(x);
                }

                if (!v1parts.every(isValidPart) || !v2parts.every(isValidPart)) {
                    return NaN;
                }

                if (zeroExtend) {
                    while (v1parts.length < v2parts.length) {
                        v1parts.push("0");
                    }
                    while (v2parts.length < v1parts.length) {
                        v2parts.push("0");
                    }
                }

                if (!lexicographical) {
                    v1parts = v1parts.map(Number);
                    v2parts = v2parts.map(Number);
                }

                for (var i = 0; i < v1parts.length; ++i) {
                    if (v2parts.length === i) {
                        return 1;
                    }

                    if (v1parts[i] === v2parts[i]) {
                        continue;
                    } else if (v1parts[i] > v2parts[i]) {
                        return 1;
                    } else {
                        return -1;
                    }
                }

                if (v1parts.length !== v2parts.length) {
                    return -1;
                }

                return 0;
            }
        };

        return service;
    }

    angular.module('umbraco.services').factory('fluidityUtilityService', fluidityUtilityService);

})();