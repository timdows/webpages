"use strict";

app.controller("homeClientController", [
    "$scope", "$http",  "growl",
    function($scope, $http, growl) {
        $scope.records = [];

        $scope.gridOptions = {
            enableSorting: true,
            enableHorizontalScrollbar: 0,
            enableVerticalScrollbar: 0,
            data: "records",
            columnDefs: [
                { field: "Group", enableCellEdit: false, width: 150 },
                { field: "Name", enableCellEdit: false, width: 200 },
                {
                    field: "Value", width: 150,
                    // Make sure only some rows are editable
                    cellEditableCondition: function ($scope)
                    {
                        return $scope.row.entity.Editable
                    }
                },
                { field: "Description", enableCellEdit: false },
                { field: "Editable", visible: false }
            ]
        };


        // http://ui-grid.info/docs/#/tutorial/201_editable
        $scope.gridOptions.onRegisterApi = function (gridApi)
        {
            //set gridApi on scope
            $scope.gridApi = gridApi;
            gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue)
            {
                $http.post("home/postsettings", rowEntity).then(function (response)
                {
                    if (response.data)
                    {
                        growl.success("Changed <strong>" + rowEntity.Name + "</strong> to value <strong>" + rowEntity.Value + "</strong>", {ttl: 2000});
                    }
                    else
                    {
                        growl.error("Could not change setting <strong>" + rowEntity.Name + "</strong>");
                    }
                },function (response)
                {
                    growl.error("Could not change setting <strong>" + rowEntity.Name + "</strong>");
                    $scope.getSettings();
                });
                $scope.$apply();
            });
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