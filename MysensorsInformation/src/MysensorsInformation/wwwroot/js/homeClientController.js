"use strict";

app.controller("homeClientController", [
    "$scope", "$http",
    function($scope, $http) {
        $scope.records = [];

        $scope.gridOptions = {
            enableSorting: true,
            enableHorizontalScrollbar: 0,
            enableVerticalScrollbar: 0,
            data: "records",
            columnDefs: [
                { field: "group", width: 150 },
                { field: "name", width: 200 },
                { field: "value", width: 150 },
                { field: "description" }
            ]
        };

        $scope.getSettings = function() {
            $http.get("home/getsettings").then(function(response) {
                $scope.records = response.data;
            });
        };

        $scope.getSettings();

        $scope.getTableHeight = function ()
        {
            var headerHeight = 31;
            var val = ($scope.records.length * 30) + headerHeight;

            return {
                height: val + "px"
            };
        };
    }
]);