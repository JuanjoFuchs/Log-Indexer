var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var Constants = (function () {
            function Constants() {
            }
            Constants.app = {
                module: "app",
                title: "LogIndexer",
                core: {
                    module: "app.core",
                    directive: "liCore",
                    controller: "CoreController",
                    controllerAs: "core",
                    templateUrl: "app/core/core.html"
                },
                dataService: "DataService",
                dashboard: {
                    templateUrl: "app/dashboard/dashboard.html"
                },
                logs: {
                    module: "app.logs",
                    title: "Logs",
                    controller: "LogsController",
                    controllerAs: "logList",
                    templateUrl: "app/logs/logs.html",
                    search: {
                        title: "Search",
                        controller: "SearchController",
                        controllerAs: "logSearch",
                        templateUrl: "app/logs/search.html",
                    },
                }
            };
            return Constants;
        })();
        Analysis.Constants = Constants;
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        angular.module(Analysis.Constants.app.core.module, [
            "ngAnimate",
            "ngRoute",
            "ngResource",
            "ngMaterial",
        ]).config(function ($mdIconProvider) {
            // Configure URLs for icons specified by [set:]id.
            $mdIconProvider.iconSet("action", "/Content/angular-material-icons/action.svg").iconSet("alert", "/Content/angular-material-icons/alert.svg").iconSet("av", "/Content/angular-material-icons/av.svg").iconSet("communication", "/Content/angular-material-icons/communication.svg").iconSet("content", "/Content/angular-material-icons/content.svg").iconSet("device", "/Content/angular-material-icons/device.svg").iconSet("editor", "/Content/angular-material-icons/editor.svg").iconSet("file", "/Content/angular-material-icons/file.svg").iconSet("hardware", "/Content/angular-material-icons/hardware.svg").iconSet("image", "/Content/angular-material-icons/image.svg").iconSet("maps", "/Content/angular-material-icons/maps.svg").iconSet("navigation", "/Content/angular-material-icons/navigation.svg").iconSet("notification", "/Content/angular-material-icons/notification.svg").iconSet("soclal", "/Content/angular-material-icons/soclal.svg").iconSet("toggle", "/Content/angular-material-icons/toggle.svg");
        });
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        angular.module(Analysis.Constants.app.logs.module, []);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var app = Analysis.Constants.app;
        angular.module(app.module, [
            app.core.module,
            app.logs.module
        ]);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
/// <reference path="app/constants.ts" />
/// <reference path="app/core/core.module.ts" />
/// <reference path="app/logs/logs.module.ts" />
/// <reference path="app/app.module.ts" />
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var CoreController = (function () {
            function CoreController($location, $route, $mdSidenav, $rootScope) {
                var _this = this;
                this.$location = $location;
                this.$mdSidenav = $mdSidenav;
                this.breadcrumbs = [];
                $rootScope.$on("$routeChangeSuccess", function (event, current, previous) {
                    console.log(event, current, previous);
                    //console.log($route);
                    //console.log(current.breadcrumbs);
                    //console.log(current.originalPath);
                    _this.breadcrumbs = current.breadcrumbs.split(".").map(function (breadcrumb) {
                        var result = Object.keys($route.routes).map(function (route) { return $route.routes[route]; }).filter(function (route) { return route.id === breadcrumb; }).map(function (route) {
                            return {
                                title: route.title,
                                href: route.originalPath
                            };
                        })[0] || { title: current.locals.log.name };
                        switch (breadcrumb) {
                            case "logs":
                                break;
                            default:
                        }
                        return result;
                    }) || [];
                });
            }
            CoreController.prototype.toggleNav = function () {
                this.$mdSidenav("left").toggle();
            };
            CoreController.prototype.goToLogs = function () {
                this.$mdSidenav("left").close();
                this.$location.path("/logs");
            };
            CoreController.$inject = [
                "$location",
                "$route",
                "$mdSidenav",
                "$rootScope"
            ];
            return CoreController;
        })();
        angular.module(Analysis.Constants.app.core.module).controller(Analysis.Constants.app.core.controller, CoreController);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var CoreDirective = (function () {
            function CoreDirective() {
                this.restrict = "E";
                this.controller = Analysis.Constants.app.core.controller;
                this.controllerAs = Analysis.Constants.app.core.controllerAs;
                this.templateUrl = Analysis.Constants.app.core.templateUrl;
            }
            CoreDirective.create = function () {
                return new CoreDirective();
            };
            return CoreDirective;
        })();
        CoreDirective.create.$inject = [];
        angular.module(Analysis.Constants.app.core.module).directive(Analysis.Constants.app.core.directive, CoreDirective.create);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        function RoutesConfig($routeProvider) {
            var logs = Analysis.Constants.app.logs;
            $routeProvider.when("/", {
                title: Analysis.Constants.app.title,
                id: "home",
                breadcrumbs: "home",
                templateUrl: Analysis.Constants.app.dashboard.templateUrl
            }).when("/logs", {
                title: logs.title,
                id: "logs",
                breadcrumbs: "home.logs",
                controller: logs.controller,
                controllerAs: logs.controllerAs,
                templateUrl: logs.templateUrl,
                resolve: Analysis.LogsController.resolve
            }).when("/logs/:id/search", {
                title: logs.search.title,
                id: "search",
                breadcrumbs: "home.logs.log.search",
                controller: logs.search.controller,
                controllerAs: logs.search.controllerAs,
                templateUrl: logs.search.templateUrl,
                resolve: Analysis.SearchController.resolve
            }).otherwise({ redirectTo: "/" });
        }
        RoutesConfig.$inject = ["$routeProvider"];
        angular.module(Analysis.Constants.app.core.module).config(RoutesConfig);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var DataService = (function () {
            function DataService($resource) {
                this.logsOData = $resource("odata/logs", {}, { oDataQuery: { method: "GET" } });
                this.logsRest = $resource("api/logs/:id", { id: "@id" });
                this._search = $resource("api/search", {}, { search: { method: "GET" } });
            }
            Object.defineProperty(DataService.prototype, "logs", {
                get: function () {
                    var _this = this;
                    return {
                        query: function (filter) { return _this.logsOData.oDataQuery(filter).$promise.then(function (response) {
                            return {
                                count: response["@odata.count"],
                                entities: response.value
                            };
                        }); },
                        load: function (id) { return _this.logsRest.get({ id: id }).$promise; }
                    };
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(DataService.prototype, "search", {
                get: function () {
                    var _this = this;
                    return {
                        query: function (query) { return _this._search.search({ query: query }).$promise; }
                    };
                },
                enumerable: true,
                configurable: true
            });
            DataService.$inject = ["$resource"];
            return DataService;
        })();
        Analysis.DataService = DataService;
        angular.module(Analysis.Constants.app.module).service(Analysis.Constants.app.dataService, DataService);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var logs = Analysis.Constants.app.logs;
        var LogsController = (function () {
            function LogsController($location, dataService, logs) {
                this.$location = $location;
                this.logs = logs;
            }
            LogsController.prototype.goTo = function (log) {
                this.$location.path("/logs/" + log.id + "/search");
            };
            LogsController.$inject = ["$location", Analysis.Constants.app.dataService, "logs"];
            LogsController.resolve = {
                logs: function (dataService) { return dataService.logs.query(); }
            };
            return LogsController;
        })();
        Analysis.LogsController = LogsController;
        LogsController.resolve.logs.$inject = [Analysis.Constants.app.dataService];
        angular.module(logs.module).controller(logs.controller, LogsController);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var logs = Analysis.Constants.app.logs;
        var SearchController = (function () {
            function SearchController(locationService, dataService, log) {
                this.locationService = locationService;
                this.dataService = dataService;
            }
            SearchController.prototype.search = function () {
                var _this = this;
                this.dataService.search.query(this.query).then(function (results) { return _this.results = results; });
            };
            SearchController.$inject = ["$location", Analysis.Constants.app.dataService, "log"];
            SearchController.resolve = {
                log: function ($route, dataService) { return dataService.logs.load($route.current.params.id); }
            };
            return SearchController;
        })();
        Analysis.SearchController = SearchController;
        SearchController.resolve.log.$inject = ["$route", Analysis.Constants.app.dataService];
        angular.module(logs.module).controller(logs.search.controller, SearchController);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
//# sourceMappingURL=generated.js.map