module LogIndexer.Analysis {
    "use strict";

    class CoreController {
        static $inject = [
            "$location",
            "$route",
            "$mdSidenav",
            "$rootScope"
        ];

        breadcrumbs = [];

        constructor(
            private $location: ng.ILocationService,
            $route: ng.route.IRouteService,
            private $mdSidenav,
            $rootScope) {
            $rootScope.$on("$routeChangeSuccess", (event, current, previous) => {
                this.breadcrumbs = current.breadcrumbs.split(".").map(breadcrumb => {
                    return Object.keys($route.routes)
                        .map(route => $route.routes[route])
                        .filter(route => route.id === breadcrumb)
                        .map(route => {
                            return {
                                title: route.title,
                                href: route.originalPath
                            }
                        })[0] || { title: current.locals.log.name };
                }) || [];
            });
        }

        toggleNav() {
            this.$mdSidenav("left").toggle();
        }

        goToLogs() {
            this.$mdSidenav("left").close();
            this.$location.path("/logs");
        }
    }

    angular
        .module(Constants.app.core.module)
        .controller(Constants.app.core.controller, CoreController);
}