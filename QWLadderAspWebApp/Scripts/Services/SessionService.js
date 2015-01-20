var SessionService = function ($cookies) {
    this.token = undefined;

    this.getToken = function () {
        if (!$cookies.QWLadderAngularAppToken) {
            if (!this.token) {
                return undefined;
            }
            this.setToken(this.token);
        }
        return $cookies.QWLadderAngularAppToken;
    }

    this.setToken = function (token) {
        this.token = token;
        $cookies.QWLadderAngularAppToken = token;
    }

    this.apiUrl = 'http://localhost:5586';
}

SessionService.$inject = ['$cookies'];