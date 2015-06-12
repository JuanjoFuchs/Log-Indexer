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
                core: {
                    module: "app.core",
                    directive: "liCore",
                    controller: "CoreController",
                    controllerAs: "core",
                    templateUrl: "app/core/core.html"
                },
                dashboard: {
                    templateUrl: "app/dashboard/dashboard.html"
                },
                logs: {
                    module: "app.logs",
                    title: "Logs",
                    controller: "LogsController",
                    controllerAs: "logList",
                    templateUrl: "app/logs/logs.html",
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
            function CoreController() {
            }
            CoreController.$inject = [];
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
                templateUrl: Analysis.Constants.app.dashboard.templateUrl
            }).when("/logs", {
                title: logs.title,
                controller: logs.controller,
                controllerAs: logs.controllerAs,
                templateUrl: logs.templateUrl,
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
        var LogsController = (function () {
            function LogsController() {
                this.logs = [
                    { name: "A log" },
                    { name: "A log" },
                    { name: "A log" },
                    { name: "A log" },
                    { name: "A log" },
                ];
            }
            LogsController.prototype.goTo = function (log) {
                alert(log.name);
            };
            LogsController.$inject = [];
            return LogsController;
        })();
        angular.module(Analysis.Constants.app.logs.module).controller(Analysis.Constants.app.logs.controller, LogsController);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
//# sourceMappingURL=generated.js.map