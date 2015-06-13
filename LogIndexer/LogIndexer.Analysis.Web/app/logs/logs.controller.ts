module LogIndexer.Analysis {
    "use strict";

    var logs = Constants.app.logs;

    export class LogsController {
        static $inject = ["$location", Constants.app.dataService, "data"];

        logs;
        private dataSourceTotals;

        constructor(private $location: ng.ILocationService, dataService: DataService, data) {
            console.log(data);
            this.logs = data.logs;
            this.dataSourceTotals = data.dataSourceTotals;
        }

        static resolve: any = {
            data: dataService => {
                return Q.when({
                    logs: dataService.logs.query(),
                    dataSourceTotals: dataService.records.totals.byDataSourceId()
                });
            },
        };

        goTo(log) {
            this.$location.path(`/${log.id}/search`);
        }

        serverNames(dataSources) {
            return dataSources.map(x => x.server.name);
        }

        recordsIndexed(dataSources) {
            return this.dataSourceTotals
                .filter(x => dataSources.some(y => y.id === x.dataSourceId))
                .map(x => x.count)
                .reduce((previous, current, index, array) => previous + current, 0);
        }

        fileNames(dataSources) {
            return dataSources.map(x => x.file);
        }
    }

    LogsController.resolve.data.$inject = [Constants.app.dataService];

    angular
        .module(logs.module)
        .controller(logs.controller, LogsController);
}