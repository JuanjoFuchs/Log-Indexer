module LogIndexer.Analysis {
    "use strict";

    var logs = Constants.app.logs;

    export class SearchController {
        static $inject = ["$location", Constants.app.dataService, "log"];

        query;
        results;
        models = [
            { id: "WebLog", name: "WebLog" },
            { id: "WebLogError", name: "WebLogError" },
        ];
        model;
        modelResults;

        constructor(private locationService: ng.ILocationService, private dataService: DataService, private log) {
        }

        static resolve: any = {
            log: ($route, dataService) => dataService.logs.load($route.current.params.id)
        }

        search() {
            if (this.model)
                this.dataService.search
                    .byModel(this.log.id, this.model, this.query)
                    .then(modelResults => {
                        this.modelResults = modelResults;
                        this.results = null;
                    });
            else
                this.dataService.search
                    .byText(this.log.id, this.query)
                    .then(results => {
                        this.results = results;
                        this.modelResults = null;
                    });
        }
    }

    SearchController.resolve.log.$inject = ["$route", Constants.app.dataService];

    angular
        .module(logs.module)
        .controller(logs.search.controller, SearchController);
}