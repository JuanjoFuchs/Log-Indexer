module LogIndexer.Analysis {
    "use strict";

    export class DataService {
        static $inject = ["$resource"];

        private logsOData;
        private _logs;
        private _search;
        private _recordsTotals;

        constructor($resource: ng.resource.IResourceService) {
            this.logsOData = $resource("odata/logs", {}, { query: { method: "GET", isArray: false } });
            this._logs = $resource("api/logs/:id", { id: "@id" });
            this._search = $resource("api/logs/:id/search/:by/:model", { id: "@id", model: "@model" }, {
                byText: { method: "GET" , params:{by:"byText"}},
                byModel:{ method:"GET", params:{by:"byModel"}}
            });
            this._recordsTotals = $resource("api/records/totals/:by", {}, {
                 byDataSourceId: { method: "GET", params: { by: "byDataSourceId"}, isArray: true }
            });
        }

        get logs() {
            return {
                query: () => this._logs.query(),

                load: id => this._logs.get({ id: id }).$promise
            };
        }

        get records() {
            return {
                totals: {
                    byDataSourceId: () => this._recordsTotals.byDataSourceId()
                }
            };
        }

        get search() {
            return {
                byText: (id, query?) => this._search.byText({ id: id.replace("logs/", ""), query: query }).$promise,
                byModel: (id, model, query) => this._search.byModel({
                    id: id.replace("logs/", ""),
                    model: model,
                    query: query
                }).$promise,
            };
        }
    }

    angular
        .module(Constants.app.module)
        .service(Constants.app.dataService, DataService);
}