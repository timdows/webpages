"use strict";

var app = angular.module("mysensorsInformationApp", ["ui.grid", "ui.grid.autoResize", "ui.grid.edit", "angular-growl"]);

// Specify SignalR server URL here for supporting CORS (in this case it is locally)
app.value("signalRServer", "");