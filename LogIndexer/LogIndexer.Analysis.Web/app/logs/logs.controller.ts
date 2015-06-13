module LogIndexer.Analysis {
    "use strict";

    var logs = Constants.app.logs;

    export class LogsController {
        static $inject = ["$filter", "$location", Constants.app.dataService, "data"];

        logs;
        private dataSourceTotals;

        constructor(private $filter, private $location: ng.ILocationService, dataService: DataService, data) {
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

        mapDataSources(dataSources) {
            var count = dataSource => this.dataSourceTotals
                .filter(x => x.dataSourceId === dataSource.id)
                .map(x => x.count)
                .reduce((previous, current, index, array) => previous + current);

            return dataSources
                .map(dataSource => `${dataSource.file}: ${this.$filter("number")(count(dataSource), 0) }`)
                .join(", ");
        }
    }

    LogsController.resolve.data.$inject = [Constants.app.dataService];

    angular
        .module(logs.module)
        .controller(logs.controller, LogsController);
}