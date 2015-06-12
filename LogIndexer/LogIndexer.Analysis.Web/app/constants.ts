module LogIndexer.Analysis {
    "use strict";

    export class Constants {
        static app = {
            module: "app",
            title: "LogIndexer",

            core: {
                module: "app.core",
                directive: "liCore",
                controller: "CoreController",
                controllerAs: "core",
                templateUrl: "app/core/core.html"
            },

            dataService: "DataService",

            dashboard: {
                templateUrl: "app/dashboard/dashboard.html"
            },

            logs: {
                module: "app.logs",
                title: "Logs",
                controller: "LogsController",
                controllerAs: "logList",
                templateUrl: "app/logs/logs.html",

                search: {
                    title: "Search",
                    controller: "SearchController",
                    controllerAs: "logSearch",
                    templateUrl: "app/logs/search.html",
                },
            }
        };
    }
}

