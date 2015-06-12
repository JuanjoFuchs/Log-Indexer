var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var CoreController = (function () {
            function CoreController() {
            }
            CoreController.controllerId = "CoreController";
            CoreController.controllerAs = "core";
            CoreController.$inject = [];
            return CoreController;
        })();
        Analysis.CoreController = CoreController;
        angular.module(Analysis.CoreModule.moduleId).controller(CoreController.controllerId);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
//# sourceMappingURL=core.controller.js.map