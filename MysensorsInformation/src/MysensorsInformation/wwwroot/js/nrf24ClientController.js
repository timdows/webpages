"use strict";

app.controller("nrf24ClientController", [
    "$scope", "signalRHubProxy",
    function($scope, signalRHubProxy) {
        var nrf24HubProxy = signalRHubProxy(signalRHubProxy.defaultServer, "NRF24Hub");

        $scope.nrf24Structures = [];

        $scope.gridOptions = {
            enableSorting: false,
            enableFiltering: true,
            enableHorizontalScrollbar: 1,
            enableVerticalScrollbar: 0,
            showGridFooter: true,
            data: "nrf24Structures",
            columnDefs: [
                { field: "DateTime", name: "DateTime", cellFilter: "date:'HH:mm:ss.sss'", width: 110 },
                //{ field: "TypeAndLength", name: "HType", width: 50 },

                //{ field: "NRF24Header.Timestamp", name: "Timestamp", width: 80 },
                //{ field: "NRF24Header.PacketsLost", name: "PacketsLost", width: 50 },
                //{ field: "NRF24Header.Address", name: "Address" },

                { field: "NRF24Data.NodeAddress", name: "NodeAddress", headerTooltip: "NodeAddress", width: 100 },
                { field: "NRF24Data.PayloadLengthNumber", name: "PayloadLength", headerTooltip: "PayloadLength", width: 50 },
                { field: "NRF24Data.PidNumber", name: "Pid", headerTooltip: "Pid", width: 50 },
                { field: "NRF24Data.NoAck", name: "NoAck", headerTooltip: "NoAck", width: 50 },
                //{ field: "NRF24Data.NRF24Mysensor.Last", name: "Last", headerTooltip: "Last", width: 50 },
                { field: "NRF24Data.NRF24Mysensor.Sender", name: "Sender", headerTooltip: "Sender", width: 50 },
                { field: "NRF24Data.NRF24Mysensor.SenderVeraDevice.Name", name: "SenderVeraDevice", headerTooltip: "SenderVeraDevice", width: 200 },
                { field: "NRF24Data.NRF24Mysensor.Destination", name: "Destination", headerTooltip: "Destination", width: 50 },
                { field: "NRF24Data.NRF24Mysensor.DestinationVeraDevice.Name", name: "DestinationVeraDevice", headerTooltip: "DestinationVeraDevice", width: 200 },
                { field: "NRF24Data.NRF24Mysensor.LengthNumber", name: "Length", headerTooltip: "Length", width: 50 },
                { field: "NRF24Data.NRF24Mysensor.VersionString", name: "Version", headerTooltip: "Version", width: 70 },
                { field: "NRF24Data.NRF24Mysensor.DataTypeString", name: "DataType", headerTooltip: "DataType", width: 70 },
                { field: "NRF24Data.NRF24Mysensor.IsAck", name: "IsAck", headerTooltip: "IsAck", width: 50 },
                { field: "NRF24Data.NRF24Mysensor.ReqAck", name: "ReqAck", headerTooltip: "ReqAck", width: 50 },
                { field: "NRF24Data.NRF24Mysensor.CommandTypeString", name: "CommandType", headerTooltip: "CommandType", width: 60 },
                { field: "NRF24Data.NRF24Mysensor.TypeString", name: "Type", headerTooltip: "Type", width: 110 },
                { field: "NRF24Data.NRF24Mysensor.Sensor", name: "Sensor", headerTooltip: "Sensor", width: 50 },
                { field: "NRF24Data.NRF24Mysensor.SensorVeraDevice.Name", name: "SensorVeraDevice", headerTooltip: "SensorVeraDevice", width: 200 },
                { field: "NRF24Data.NRF24Mysensor.Payload", name: "Payload", headerTooltip: "Payload", minWidth: 250 }
            ]
        };

        nrf24HubProxy.on("broadcastObject", function(nrf24Structure) {
            $scope.nrf24Structures.unshift(nrf24Structure);
            //console.log(nrf24Structure);
        });

        $scope.getTableHeight = function ()
        {
            var headerHeight = 110;
            var val = ($scope.nrf24Structures.length * 30) + headerHeight;

            return {
                height: val + "px"
            };
        };
    }
]);