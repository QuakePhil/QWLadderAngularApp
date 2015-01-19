var QWLadderAngularApp = angular.module('QWLadderAngularApp', ['ui.router', 'ui.bootstrap']);

QWLadderAngularApp.controller('LandingPageController', LandingPageController);
QWLadderAngularApp.controller('LoginController', LoginController);
QWLadderAngularApp.controller('RegisterController', RegisterController);

QWLadderAngularApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
QWLadderAngularApp.factory('LoginFactory', LoginFactory);
QWLadderAngularApp.factory('RegistrationFactory', RegistrationFactory);

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
        .state('loginRegister', {
            url: '/loginRegister?returnUrl',
            views: {
                "containerOne": {
                    templateUrl: '/Account/Login',
                    controller: LoginController
                },
                "containerTwo": {
                    templateUrl: '/Account/Register',
                    controller: RegisterController
                }
            }
        });

    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}
configFunction.$inject = ['$stateProvider', '$httpProvider', '$locationProvider'];

QWLadderAngularApp.config(configFunction);