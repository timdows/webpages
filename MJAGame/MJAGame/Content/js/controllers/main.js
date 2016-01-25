mjaGame
    .controller("mjaGameClientController", function($rootScope, $scope, $http, $window, $timeout, growlService)
    {
        $scope.user = {};

        $scope.getUser = function(showGrowl)
        {
            $http.get("user/getuser")
                .success(function(result)
                {
                    $scope.user = result;

                    if (!showGrowl)
                    {
                        return;
                    }

                    if ($scope.user.ID === 0)
                    {
                        growlService.growl("Welcome new Games of God player", "inverse");
                    }
                    else
                    {
                        growlService.growl("Welcome back " + $scope.user.Name, "inverse");
                        if ($scope.user.Name === "root")
                        {
                            growlService.growl("Root is here", "inverse");
                            $rootScope.changeDashboard("scores");
                        }
                        else if ($scope.user.IsMarten)
                        {
                            $rootScope.changeDashboard("selectQuestion");
                        }
                        else
                        {
                            $rootScope.changeDashboard("currentQuestion");
                        }
                    }
                });
        };
        $scope.getUser(true);

        $scope.submitUser = function()
        {
            $http.post("user/newuser", $scope.user)
                .success(function(result)
                {
                    growlService.growl("Naam aangepast naar " + $scope.user.Name, "inverse");
                    $scope.getUser(false);
                    $window.location.reload();
                })
                .error(function(result)
                {
                    growlService.growl("Naam niet aangepast, mogelijk al in gebruik.", "inverse");
                });
        };

        $scope.sidebarToggle =
        {
            left: false
        };

        $scope.closeSidebars = function()
        {
            $rootScope.changeDashboard();
            $scope.sidebarToggle.left = false;
        };

        $scope.toggleSidebarLeft = function()
        {
            $scope.sidebarToggle.left = !$scope.sidebarToggle.left;
        };

        $scope.toggleSidebarRight = function()
        {
            $scope.sidebarToggle.right = !$scope.sidebarToggle.right;
        };

        $rootScope.dashboardType =
        {
            name: true,
            scores: false,
            currentQuestion: false,
            addQuestion: false,
            selectQuestion: false,
            selectWinningAnswer: false
        };

        $rootScope.changeDashboard = function(type)
        {
            // Always close the sidebar
            $scope.sidebarToggle.left = false;

            $rootScope.dashboardType.name = false;
            $rootScope.dashboardType.scores = false;
            $rootScope.dashboardType.currentQuestion = false;
            $rootScope.dashboardType.selectQuestion = false;
            $rootScope.dashboardType.selectWinningAnswer = false;
            $rootScope.dashboardType.addQuestion = false;

            switch (type)
            {
                case "name":
                    $rootScope.dashboardType.name = true;
                    break;
                case "scores":
                    $rootScope.dashboardType.scores = true;
                    break;
                case "currentQuestion":
                    $rootScope.dashboardType.currentQuestion = true;
                    break;
                case "addQuestion":
                    $rootScope.dashboardType.addQuestion = true;
                    break;
                case "selectQuestion":
                    $rootScope.dashboardType.selectQuestion = true;
                    break;
                case "selectWinningAnswer":
                    $rootScope.dashboardType.selectWinningAnswer = true;
                    break;
                default:
                    $rootScope.dashboardType.name = true;
                    break;
            }
        };

        // Detact Mobile Browser
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent))
        {
            angular.element("html").addClass("ismobile");
        }

        // Main refresh timer
        $scope.mainRefresh = function()
        {
            if ($rootScope.dashboardType.name)
            {
            }
            if ($rootScope.dashboardType.scores)
            {
                $rootScope.loadScores();
            }
            if ($rootScope.dashboardType.currentQuestion)
            {
                $rootScope.getCurrentQuestion();
            }
            if ($rootScope.dashboardType.addQuestion)
            {
            }
            if ($rootScope.dashboardType.selectQuestion)
            {
                $rootScope.getAllQuestions();
            }
            if ($rootScope.dashboardType.selectWinningAnswer)
            {
                $rootScope.getAllAnswers();
            }

            $timeout(function () { $scope.mainRefresh() }, 2500);
        };

        $timeout(function () { $scope.mainRefresh() }, 10000); // Start late so the $rootScope function will be defined when timer fires
    })
    .controller("sidebarLeftClientController", function($rootScope, $scope, $http)
    {
        $scope.selectOverview = function(overview)
        {
            $rootScope.changeDashboard(overview);
        };
    })
    .controller("nameClientController", function($rootScope, $scope, $http)
    {
    })
    .controller("scoresClientController", function($rootScope, $scope, $http)
    {
        $scope.userTotalScores = [];
        $rootScope.loadScores = function()
        {
            $http.post("score/getscores")
                .success(function(result)
                {
                    $scope.userTotalScores = result;
                });
        };

        $rootScope.loadScores();
    })
    .controller("currentQuestionClientController", function($rootScope, $scope, $http)
    {
        $scope.answerObject = {};
        $scope.answerObject.answer = ""; // Hack for bug that it cannot be againts the scope
        $scope.currentQuestion = {};
        $scope.userScores = {};

        $rootScope.getCurrentQuestion = function()
        {
            $http.get("question/getcurrent")
                .success(function(result)
                {
                    $scope.currentQuestion = result;

                    // Check if we should show the scores
                    if ($scope.currentQuestion.ShowingScores)
                    {
                        var postData =
                        {
                            questionID: $scope.currentQuestion.QuestionID
                        };
                        $http.post("question/getuserscores", postData)
                            .success(function(result)
                            {
                                $scope.userScores = result;
                            });
                    }
                });
        };

        $rootScope.getCurrentQuestion();

        $scope.submitAnswer = function()
        {
            var postParams =
            {
                questionID: $scope.currentQuestion.Question.ID,
                answerString: $scope.answerObject.answer
            };
            $http.post("answer/submitanswer", postParams)
                .success(function(result)
                {
                    $scope.answerObject.answer = "";
                    $rootScope.getCurrentQuestion();
                });
        };

        $scope.selectAnswer = function(lie)
        {
            var postParams =
            {
                answerID: lie.ID
            };
            $http.post("answer/selectanswer", postParams)
                .success(function(result)
                {
                    $rootScope.getCurrentQuestion();
                });
        };
    })
    .controller("addQuestionClientController", function ($rootScope, $scope, $http, growlService)
    {
        $scope.addQuestion = {};

        $scope.getEmptyResult = function()
        {
            $http.get("addquestion/getemptyresult")
               .success(function (result)
               {
                   $scope.addQuestion = result;
               });
        };

        $scope.getEmptyResult();
        
        $scope.submitQuestion = function()
        {
            // Check if fields are submitted
            if ($scope.addQuestion.newQuestion.Subject != null &&
                $scope.addQuestion.newQuestion.QuestionString !== "")
            {
                var postParams =
                {
                    question: $scope.addQuestion.newQuestion
                };
                $http.post("addquestion/submitquestion", postParams)
                    .success(function(result)
                    {
                        $scope.getEmptyResult();
                        growlService.growl("Question submitted.", "inverse");
                    });
            }
            else
            {
                growlService.growl("Subject or question missing.", "inverse");
            }
        };
    })
    .controller("selectQuestionClientController", function($rootScope, $scope, $http, growlService)
    {
        $scope.allQuestions = [];

        $rootScope.getAllQuestions = function()
        {
            $http.get("marten/getallquestions")
                .success(function(result)
                {
                    $scope.allQuestions = result;
                });
        };

        function roundIsRunning()
        {
            var running = false;
            angular.forEach($scope.allQuestions, function(question)
            {
                if (question.Status === 1 || question.Status === 2 || question.Status === 3)
                {
                    running = true;
                }
            });

            return running;
        }

        $rootScope.getAllQuestions();

        $scope.setQuestionCurrent = function(question)
        {
            if (roundIsRunning())
            {
                growlService.growl("Er is nog een ronde actief", "inverse");
                return;
            }

            if (question.Status !== 0)
            {
                growlService.growl("Deze vraag is al gespeeld", "inverse");
                return;
            }

            if (!question.IsCurrent)
            {
                var postParams =
                {
                    id: question.ID
                };
                $http.post("marten/setcurrent", postParams)
                    .success(function(result)
                    {
                        $rootScope.getAllQuestions();
                    });
            }
        };

        $scope.showScores = function(question)
        {
            var postParams =
            {
                id: question.ID
            };
            $http.post("marten/showscores", postParams)
                .success(function(result)
                {
                    $rootScope.getAllQuestions();
                });
        };

        $scope.selectWinningAnswer = function(question)
        {
            $rootScope.changeDashboard("selectWinningAnswer");
            $rootScope.questionForSelectWinningAnswer = question;
        };

        $scope.stopRound = function(question)
        {
            var postParams =
            {
                id: question.ID
            };
            $http.post("marten/endround", postParams)
                .success(function(result)
                {
                    $rootScope.getAllQuestions();
                });
        };
    })
    .controller("selectWinningAnswerClientController", function($rootScope, $scope, $http, growlService)
    {
        $scope.allAnswers = [];

        $rootScope.getAllAnswers = function()
        {
            var postParams =
            {
                questionID: $rootScope.questionForSelectWinningAnswer.ID
            };
            $http.post("marten/getallanswers", postParams)
                .success(function(result)
                {
                    $scope.allAnswers = result;
                });
        };

        $rootScope.getAllAnswers();

        $scope.setWinningAnswer = function(answer)
        {
            var postParams =
            {
                answerID: answer.ID,
                questionID: $scope.allAnswers.question.ID
            };
            $http.post("marten/setwinninganswer", postParams)
                .success(function(result)
                {
                    $rootScope.getAllAnswers();
                });
        };

        function hasLieSelected()
        {
            var selected = false;
            angular.forEach($scope.allAnswers.answers, function(answer)
            {
                if (answer.Correct)
                {
                    selected = true;
                }
            });

            return selected;
        }

        $scope.stopSubmittingAnswers = function()
        {
            // Do not allow going back before selecting answer
            if (!hasLieSelected())
            {
                growlService.growl("Nog geen winnaar gekozen", "inverse");
                return;
            }

            var postParams =
            {
                id: $scope.allAnswers.question.ID
            };
            $http.post("marten/selectlies", postParams)
                .success(function(result)
                {
                    $rootScope.changeDashboard("selectQuestion");
                });
        };
    });
