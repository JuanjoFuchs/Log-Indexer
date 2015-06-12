module LogIndexer.Analysis {
    "use strict";

    export class DataService {
        static $inject = ["$resource"];

        private logsOData;
        private logsRest;
        private _search;

        constructor($resource: ng.resource.IResourceService) {
            this.logsOData = $resource("odata/logs", {}, { oDataQuery: { method: "GET" } });
            this.logsRest = $resource("api/logs/:id", { id: "@id" });
            this._search = $resource("api/search", {}, { search: { method: "GET" } });
            
        }

        get logs() {
            return {
                query: (filter?) => this.logsOData.oDataQuery(filter).$promise
                    .then(response => {
                        return {
                            count: response["@odata.count"],
                            entities: response.value
                        };
                    }),

                load: id => this.logsRest.get({ id: id }).$promise
            };
        }

        get search() {
            return {
                query: (query?) => this._search.search({ query: query }).$promise
            };
        }
    }

    angular
        .module(Constants.app.module)
        .service(Constants.app.dataService, DataService);
}