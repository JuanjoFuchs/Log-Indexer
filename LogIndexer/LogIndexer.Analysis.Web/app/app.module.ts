module LogIndexer.Analysis {
    "use strict";

    var app = Constants.app;

    angular
        .module(app.module, [
            app.core.module,
            app.logs.module
        ]);
}

