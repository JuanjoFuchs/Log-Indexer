module LogIndexer.Analysis {
    "use strict";

    class CoreController {
        static $inject = [];
    }

    angular
        .module(Constants.app.core.module)
        .controller(Constants.app.core.controller, CoreController);
}