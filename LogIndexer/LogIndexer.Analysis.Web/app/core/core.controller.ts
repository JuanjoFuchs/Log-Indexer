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
                console.log(event, current, previous);
                //console.log($route);
                //console.log(current.breadcrumbs);
                //console.log(current.originalPath);

                this.breadcrumbs = current.breadcrumbs.split(".").map(breadcrumb => {
                    var result = Object.keys($route.routes)
                        .map(route => $route.routes[route])
                        .filter(route => route.id === breadcrumb)
                        .map(route => {
                            return {
                                title: route.title,
                                href: route.originalPath
                            }
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