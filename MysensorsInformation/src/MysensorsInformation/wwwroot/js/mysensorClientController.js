"use strict";

app.controller("mysensorsClientController", [
    "$scope", "signalRHubProxy",
    function($scope, signalRHubProxy) {
        var nrf24HubProxy = signalRHubProxy(signalRHubProxy.defaultServer, "MysensorsHub");

        $scope.mysensorStructures = [];

        $scope.gridOptions = {
            enableSorting: false,
            enableFiltering: true,
            enableHorizontalScrollbar: 1,
            enableVerticalScrollbar: 0,
            showGridFooter: true,
            data: "mysensorStructures",
            columnDefs: [
                { field: "DateTime", name: "DateTime", headerTooltip: "DateTime", width: 110, cellFilter: "date:'HH:mm:ss.sss'" },
                { field: "NodeID", name: "NodeID", headerTooltip: "NodeID", width: 50 },
                { field: "ChildSensorID", name: "ChildSensorID", headerTooltip: "ChildSensorID", width: 50 },
                { field: "VeraDevice.Name", name: "Device", headerTooltip: "Device" },
                { field: "VeraDevice.VeraRoom.Name", name: "Room", headerTooltip: "Room" },
                { field: "MessageTypeString", name: "MessageType", headerTooltip: "MessageType" },
                { field: "Ack", name: "Ack", headerTooltip: "Ack", width: 50 },
                { field: "SubtypeString", name: "Subtype", headerTooltip: "Subtype" },
                { field: "Payload", name: "Payload", headerTooltip: "Payload" }
            ]
        };

        nrf24HubProxy.on("broadcastObject", function(mysensorStructure) {
            $scope.mysensorStructures.unshift(mysensorStructure);
            console.log(mysensorStructure);
        });

        $scope.getTableHeight = function ()
        {
            var headerHeight = 110;
            var val = ($scope.mysensorStructures.length * 30) + headerHeight;

            return {
                height: val + "px"
            };
        };
    }
]);