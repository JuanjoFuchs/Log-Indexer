module LogIndexer.Analysis {
    "use strict";

    class LogsController {
        static $inject = [];

        logs = [
            { name: "A log" },
            { name: "A log" },
            { name: "A log" },
            { name: "A log" },
            { name: "A log" },
        ];

        goTo(log) {
            alert(log.name);
        }
    }

    angular
        .module(Constants.app.logs.module)
        .controller(Constants.app.logs.controller, LogsController);
}