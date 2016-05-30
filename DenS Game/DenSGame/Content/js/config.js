densGame
    .config(function($stateProvider, $urlRouterProvider)
    {
        $urlRouterProvider.otherwise("/");

        $stateProvider.state("game", {
            url: "/",
            templateUrl: "partials/dashboard.html"
        });
    });
