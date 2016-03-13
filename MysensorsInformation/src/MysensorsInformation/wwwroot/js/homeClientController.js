"use strict";

app.controller("homeClientController", [
    "$scope", "$http", "signalRHubProxy", "growl",
    function ($scope, $http, signalRHubProxy, growl)
    {
        var nrf24HubProxy = signalRHubProxy(signalRHubProxy.defaultServer, "NRF24Hub");
        $scope.records = [];
        $scope.uploadRequested = false;

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
                $scope.records = response.data.records;
                $scope.uploadRequested = response.data.uploadRequested;
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

        $scope.requestUpload = function ()
        {
            $http.get("home/requestupload").then(function (response)
            {
                if (response.data)
                {
                    growl.success("Upload of configuration requested", { ttl: 2000 });
                    $scope.uploadRequested = true;
                }
                else
                {
                    growl.error("Something went wrong requesting the upload");
                }
            });
        };

        nrf24HubProxy.on("broadcastUploadConfigurationDone", function ()
        {
            growl.success("Configuration uploaded", { ttl: 2000 });
            $scope.uploadRequested = false;
        });
    }
]);