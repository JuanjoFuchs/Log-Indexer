module LogIndexer.Analysis {
    "use strict";

    export class DataService {
        static $inject = ["$resource"];

        private logsOData;
        private _logs;
        private _search;

        constructor($resource: ng.resource.IResourceService) {
            this.logsOData = $resource("odata/logs", {}, { query: { method: "GET", isArray: false } });
            this._logs = $resource("api/logs/:id", { id: "@id" });
            this._search = $resource("api/logs/:id/search", { id: "@id" }, { search: { method: "GET" } });

        }

        get logs() {
            return {
                query: () => this._logs.query().$promise,

                load: id => this._logs.get({ id: id }).$promise
            };
        }

        get search() {
            return {
                query: (id, query?) => this._search.search({ id: id.replace("logs/", ""), query: query }).$promise
            };
        }
    }

    angular
        .module(Constants.app.module)
        .service(Constants.app.dataService, DataService);
}