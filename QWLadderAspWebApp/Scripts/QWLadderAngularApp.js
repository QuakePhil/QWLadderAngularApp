var QWLadderAngularApp = angular.module('QWLadderAngularApp', ['ngRoute']);

QWLadderAngularApp.controller('LandingPageController', LandingPageController);
QWLadderAngularApp.controller('LoginController', LoginController);
QWLadderAngularApp.controller('RegisterController', RegisterController);

QWLadderAngularApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
QWLadderAngularApp.factory('LoginFactory', LoginFactory);
QWLadderAngularApp.factory('RegistrationFactory', RegistrationFactory);

var configFunction = function ($routeProvider, $httpProvider) {
    $routeProvider.
        when('/routeOne', {
            templateUrl: 'routesDemo/one'
        })
        .when('/routeTwo/:thisfred', {
            templateUrl: function (params) { return '/routesDemo/two?fred=' + params.thisfred; }
        })
        .when('/routeThree', {
            templateUrl: 'routesDemo/three'
        })
        .when('/login', {
            templateUrl: 'Account/Login',
            controller: LoginController
        })
        .when('/register', {
            templateUrl: 'Account/Register',
            controller: RegisterController
        });

    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}
configFunction.$inject = ['$routeProvider', '$httpProvider'];

QWLadderAngularApp.config(configFunction);