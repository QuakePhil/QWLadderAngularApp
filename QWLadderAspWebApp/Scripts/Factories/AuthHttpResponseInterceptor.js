var AuthHttpResponseInterceptor = function ($q, $location, $injector) {
    return {
        response: function (response) {
            if (response.status === 401) {
                console.log("Response 401");
            }
            return response || $q.when(response);
        },
        responseError: function (rejection) {
            if (rejection.status === 401) {
                // todo: ideally, we need to inject UI Router's $state service.
                // However, due to a bug in this library, we can't inject this directly.
                // Instead we inject AngularJS' $injector service and use this to resolve
                // an instance of $state:                
                $injector.get('$state').go('loginRegister', { returnUrl: $location.path() });
            }
            return $q.reject(rejection);
        }
    }
}

AuthHttpResponseInterceptor.$inject = ['$q', '$location', '$injector'];