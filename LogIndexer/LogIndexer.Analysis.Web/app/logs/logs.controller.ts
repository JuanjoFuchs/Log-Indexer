module LogIndexer.Analysis {
    "use strict";

    var logs = Constants.app.logs;

    class LogsController {
        static $inject = ["$location", Constants.app.dataService];

        logs = [];
        logCount = 0;

        constructor(private locationService: ng.ILocationService, dataService: DataService) {
            dataService.logs
                .query()
                .take(1)
                .subscribe(this.activate.bind(this))
        }

        activate(logs) {
            this.logCount = logs.count;
            this.logs = logs.entities;
        }

        goTo(log) {
            var path = `/#/logs/${log.id}/search`;
            this.locationService.path(path);
            console.log(path);
        }
    }

    angular
        .module(logs.module)
        .controller(logs.controller, LogsController);
}