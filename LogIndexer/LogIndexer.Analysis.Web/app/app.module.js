var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var AppModule = (function () {
            function AppModule() {
            }
            AppModule.moduleId = "app";
            return AppModule;
        })();
        Analysis.AppModule = AppModule;
        angular.module(AppModule.moduleId, [
            Analysis.CoreModule.moduleId
        ]);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
//# sourceMappingURL=app.module.js.map