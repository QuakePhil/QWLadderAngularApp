//var QWLadderAngularApp = angular.module('QWLadderAngularApp', ['ui.router', 'ui.bootstrap']);
var QWLadderAngularApp = angular.module('QWLadderAngularApp', ['ngRoute', 'ngCookies']);

QWLadderAngularApp.controller('BaseController', BaseController);
QWLadderAngularApp.controller('LoginController', LoginController);
QWLadderAngularApp.controller('RegisterController', RegisterController);

//QWLadderAngularApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
QWLadderAngularApp.factory('LoginFactory', LoginFactory);
QWLadderAngularApp.factory('RegisterFactory', RegisterFactory);

QWLadderAngularApp.service('SessionService', SessionService);

var configFunction = function ($routeProvider, $locationProvider) {

    //$locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider
     .when('/#/login', {
         templateUrl: '/Account/Login',
         controller: 'LoginController'
     })
    .when('/#/register', {
        templateUrl: '/Account/Register',
        controller: 'RegisterController'
    });
};
configFunction.$inject = ['$routeProvider','$locationProvider'];

/*
var configFunction = function ($stateProvider, $httpProvider, $locationProvider) {

    $locationProvider.hashPrefix('!').html5Mode(true);

    $stateProvider
        .state('stateOne', {
            url: '/stateOne?thatfred',
            views: {
                "containerOne": {
                    templateUrl: '/routesDemo/one'
                },
                "containerTwo": {
                    templateUrl: function (params) { return '/routesDemo/two?fred=' + params.thatfred; }
                },
                "nestedView@stateOne": {
                    templateUrl: '/routesDemo/four'
                }
            }
        })
        .state('stateTwo', {
            url: '/stateTwo',
            views: {
                "containerOne": {
                    templateUrl: '/routesDemo/one'
                },
                "containerTwo": {
                    templateUrl: '/routesDemo/three'
                }
            }
        })
        .state('stateThree', {
            url: '/stateThree?thisfred',
            views: {
                "containerOne": {
                    templateUrl: function (params) { return '/routesDemo/two?fred=' + params.thisfred; }
                },
                "containerTwo": {
                    templateUrl: '/routesDemo/three'
                }
            }
        })
        .state('login', {
            url: '/login?returnUrl',
            views: {
                "containerOne": {
                    templateUrl: 'views/login.html',
                    controller: LoginController
                },
                "containerTwo": {
                    templateUrl: 'views/register.html',
                    controller: RegisterController
                }
            }
        });
    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}
configFunction.$inject = ['$stateProvider', '$httpProvider', '$locationProvider'];
*/
QWLadderAngularApp.config(configFunction);