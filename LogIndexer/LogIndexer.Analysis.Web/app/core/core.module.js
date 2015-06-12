var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var CoreModule = (function () {
            function CoreModule() {
            }
            CoreModule.moduleId = "" + Analysis.AppModule.moduleId + ".core";
            return CoreModule;
        })();
        Analysis.CoreModule = CoreModule;
        angular.module(CoreModule.moduleId, [
            "ngAnimate",
            "ngRoute",
            "ngResource",
            "ngMaterial",
        ]);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
//# sourceMappingURL=core.module.js.map