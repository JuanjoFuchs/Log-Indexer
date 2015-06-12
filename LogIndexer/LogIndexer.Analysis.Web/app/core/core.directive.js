var LogIndexer;
(function (LogIndexer) {
    var Analysis;
    (function (Analysis) {
        "use strict";
        var CoreDirective = (function () {
            function CoreDirective() {
                this.restrict = "E";
                this.controller = Analysis.CoreController.controllerId;
                this.controllerAs = Analysis.CoreController.controllerAs;
                this.templateUrl = "/app/core/core.html";
            }
            CoreDirective.create = function () {
                return new CoreDirective();
            };
            CoreDirective.directiveId = "liCore";
            return CoreDirective;
        })();
        Analysis.CoreDirective = CoreDirective;
        CoreDirective.create.$inject = [];
        angular.module(Analysis.CoreModule.moduleId).directive(CoreDirective);
    })(Analysis = LogIndexer.Analysis || (LogIndexer.Analysis = {}));
})(LogIndexer || (LogIndexer = {}));
//# sourceMappingURL=core.directive.js.map