module LogIndexer.Analysis {
    "use strict";

    function RoutesConfig($routeProvider: ng.route.IRouteProvider) {
        var logs = Constants.app.logs;

        $routeProvider
            .when("/", {
                templateUrl: Constants.app.dashboard.templateUrl
            })
            .when("/logs", {
                title: logs.title,
                controller: logs.controller,
                controllerAs: logs.controllerAs,
                templateUrl: logs.templateUrl,
            })
            .otherwise({ redirectTo: "/" });
    }

    RoutesConfig.$inject = ["$routeProvider"];

    angular
        .module(Constants.app.core.module)
        .config(RoutesConfig);
}