"use strict";

app.factory("signalRHubProxy", [
    "$rootScope", "$timeout", "signalRServer",
    function ($rootScope, $timeout, signalRServer)
    {
        function signalRHubProxyFactory(serverUrl, hubName, startOptions)
        {
            var connection = $.hubConnection(signalRServer);
            var proxy = connection.createHubProxy(hubName);
            connection.start(startOptions).done(function () { });

            return {
                on: function (eventName, callback)
                {
                    proxy.on(eventName, function (result)
                    {
                        $timeout(function ()
                        {
                            if (callback)
                            {
                                callback(result);
                            }
                        });
                    });
                },
                off: function (eventName, callback)
                {
                    proxy.off(eventName, function (result)
                    {
                        $timeout(function ()
                        {
                            if (callback)
                            {
                                callback(result);
                            }
                        });
                    });
                },
                invoke: function (methodName, callback)
                {
                    proxy.invoke(methodName)
                        .done(function (result)
                        {
                            $timeout(function ()
                            {
                                if (callback)
                                {
                                    callback(result);
                                }
                            });
                        });
                },
                connection: connection
            };
        };

        return signalRHubProxyFactory;
    }
]);