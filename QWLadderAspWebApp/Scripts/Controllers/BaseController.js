var BaseController = function ($scope) {
    $scope.helloAgain = 'QWLadder Web Page';

    $scope.loggedIn = function () {
        return SessionService.token !== undefined;
    }

}
BaseController.$inject = ['$scope', 'SessionService'];