module LogIndexer.Analysis {
    "use strict";

    export class DataService {
        static $inject = ["$resource"];

        private logsOData;
        private logsRest;

        constructor($resource: ng.resource.IResourceService) {
            this.logsOData = $resource("odata/logs", {}, { oDataQuery: { method: "GET" } });
            this.logsRest = $resource("api/logs/:id", { id: "@id" });
        }

        get logs() {
            return {
                query: (filter?) => Rx.Observable
                    .fromPromise<any>(this.logsOData.oDataQuery(filter).$promise)
                    .map(response => {
                        return {
                            count: response["@odata.count"],
                            entities: response.value
                        }
                    }),
                load: id => Rx.Observable.fromPromise<any>(this.logsRest.get({ id: id }).$promise)
            };
        }
    }

    angular
        .module(Constants.app.module)
        .service(Constants.app.dataService, DataService);
}