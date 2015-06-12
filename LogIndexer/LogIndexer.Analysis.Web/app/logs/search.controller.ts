module LogIndexer.Analysis {
    "use strict";

    var logs = Constants.app.logs;

    export class SearchController {
        static $inject = ["$location", Constants.app.dataService, "log"];

        constructor(private locationService: ng.ILocationService, dataService: DataService, log) {
        }

        static resolve: any = {
            log: ($route, dataService) => dataService.logs.load($route.current.params.id).toPromise
        }
    }

    SearchController.resolve.log.$inject = ["$route", Constants.app.dataService];

    angular
        .module(logs.module)
        .controller(logs.search.controller, SearchController);
}