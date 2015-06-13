module LogIndexer.Analysis {
    "use strict";

    var logs = Constants.app.logs;

    export class LogsController {
        static $inject = ["$location", Constants.app.dataService, "logs"];

        constructor(private $location: ng.ILocationService, dataService: DataService, public logs) {
        }

        static resolve: any = {
            logs: dataService => dataService.logs.query()
        };

        goTo(log) {
            this.$location.path(`/${log.id}/search`);
        }

        map(list, property, separator = ", ") {
            return list.map(x => x[property]).join(separator);
        }
    }

    LogsController.resolve.logs.$inject = [Constants.app.dataService];

    angular
        .module(logs.module)
        .controller(logs.controller, LogsController);
}