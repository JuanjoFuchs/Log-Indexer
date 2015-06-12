module LogIndexer.Analysis {
    "use strict";

    class CoreDirective {
        restrict = "E";
        controller = Constants.app.core.controller;
        controllerAs = Constants.app.core.controllerAs;
        templateUrl = Constants.app.core.templateUrl;

        static create() {
            return new CoreDirective();
        }
    }

    CoreDirective.create.$inject = [];

    angular
        .module(Constants.app.core.module)
        .directive(Constants.app.core.directive, CoreDirective.create);
}