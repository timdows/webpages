mjaGame
    .config(function($stateProvider, $urlRouterProvider)
    {
        $urlRouterProvider.otherwise("/GameOfGods");

        $stateProvider.state("GameOfGods", {
            url: "/GameOfGods",
            templateUrl: "partials/dashboard.html"
        });
    });
