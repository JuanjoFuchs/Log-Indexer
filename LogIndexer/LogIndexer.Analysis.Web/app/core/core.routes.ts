module LogIndexer.Analysis {
    "use strict";

    function RoutesConfig($routeProvider: ng.route.IRouteProvider) {
        var logs = Constants.app.logs;

        $routeProvider
            .when("/", {
                title: Constants.app.title,
                id: "home",
                breadcrumbs: "home",
                templateUrl: Constants.app.dashboard.templateUrl
            })
            .when("/logs", {
                title: logs.title,
                id: "logs",
                breadcrumbs: "home.logs",
                controller: logs.controller,
                controllerAs: logs.controllerAs,
                templateUrl: logs.templateUrl,
                resolve: LogsController.resolve
            })
            .when("/logs/:id/search", {
                title: logs.search.title,
                id: "search",
                breadcrumbs: "home.logs.log.search",
                controller: logs.search.controller,
                controllerAs: logs.search.controllerAs,
                templateUrl: logs.search.templateUrl,
                resolve: SearchController.resolve
            })
            .otherwise({ redirectTo: "/" });
    }

    RoutesConfig.$inject = ["$routeProvider"];

    angular
        .module(Constants.app.core.module)
        .config(RoutesConfig);
}