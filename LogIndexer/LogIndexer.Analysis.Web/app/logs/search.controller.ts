module LogIndexer.Analysis {
    "use strict";

    var logs = Constants.app.logs;

    export class SearchController {
        static $inject = ["$location", Constants.app.dataService, "log"];

        query;
        results;

        constructor(private locationService: ng.ILocationService, private dataService: DataService, log) {
        }

        static resolve: any = {
            log: ($route, dataService) => dataService.logs.load($route.current.params.id)
        }

        search() {
            this.dataService.search
                .query(this.query)
                .then(results => this.results = results);
        }
    }

    SearchController.resolve.log.$inject = ["$route", Constants.app.dataService];

    angular
        .module(logs.module)
        .controller(logs.search.controller, SearchController);
}