// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



var hostApp = angular.module('hostApp', []);

hostApp.controller('indexController', ['$scope', '$http', function ($scope, $http) {
    $scope.state = "home";
    $http({
        method: "GET",
        url: "/api/Play/LoadGames",
        responseType: 'json'
    }).then(function mySuccess(response) {
        $scope.games = response.data;
    }, function myError(response) {
        console.log("error");
    });

    $scope.createGame = function () {

        if ($scope.selectedGame == null || $scope.selectedGame == 0) {
            $scope.$parent.$broadcast('showMessage', {
                text: "Please set all items before continuing",
                error: true
            })
            return;
        }
        $http({
            method: "POST",
            url: "/api/Host/CreateGame",
            data: $scope.selectedGame
        }).then(function mySuccess(response) {
            $scope.game = response.data;
            $scope.state = "menu";
            $scope.updateView($scope.state, $scope.game);
        });
    }

    $scope.loadGame = function () {
        if ($scope.selectedGame.gameId == null) {
            $scope.$parent.$broadcast('showMessage', {
                text: "Please set all items before continuing",
                error: true
            })
            return;
        }
        $http({
            method: "GET",
            url: "/api/Host/GetQuestions",
            params: {
                "id": $scope.selectedGame.gameId
            }
        }).then(function mySuccess(response) {
            $scope.game = response.data;
            $scope.state = "menu";
            $scope.updateView($scope.state, $scope.game);
        }, function myError(response) {
            console.log("error");
        });
    }

    $scope.updateView = function (state, game) {
        $scope.$broadcast('Joined', {
            state: state,
            game: game
        });
    }
    $scope.roundNames = {
        "1": "One",
        "2": "Two",
        "3": "Three",
        "4": "Half Time",
        "5": "Four",
        "6": "Five",
        "7": "Six",
        "8": "Final"
    };

}]);

hostApp.controller('scoreController', ['$scope', '$http', '$interval', function ($scope, $http, $interval) {
    $scope.game = {};


    $scope.$on("Joined", function (events, args) {
        $scope.game = args.game;
        $scope.reload(args.game);
        $scope.getCurrentQuestion();
    })

    $scope.reload = function (game) {
        $http({
            method: "GET",
            url: "/api/Host/GetPlayerAnswers",
            params: {
                "id": game.metadata.gameId
            },
        }).then(function mySuccess(response) {
            $scope.playerAnswers = response.data;
        }, function myError(response) {
            console.log("error");
        });

    }

    //$scope.selectedRound = $scope.$parent.rounds[0];


    $scope.setCurrentQuestion = function (questionStr) {
        var game = $scope.$parent.game.metadata;
        var question = JSON.parse(questionStr);
        game.currentRound = question.round;
        game.currentQuestion = question.number;
        $http({
            method: "POST",
            url: "/api/Host/SetCurrentQuestion",
            data: game
        }).then(function mySuccess(response) {
            $scope.saved = true;
            $scope.currentQuestion = response.data;
        });
    }

    $scope.getCurrentQuestion = function () {
        $http({
            method: "GET",
            url: "/api/Play/GetCurrentQuestion",
            params: {
                "gameId": $scope.game.metadata.gameId,
                "host": $scope.game.metadata.gameId
            }
        }).then(function mySuccess(response) {
            $scope.currentQuestion = response.data;
        }, function myError(response) {
            console.log("error");
        });
    }

    $scope.playerScoreChanged = function (playerAnswer, score) {
        if (score == "Correct") {
            playerAnswer.pointsAwarded = playerAnswer.wager;
        } else if (score == "Incorrect") {
            playerAnswer.pointsAwarded = 0;
        }
    }

}]);


hostApp.controller('questionController', ['$scope', '$http', function ($scope, $http) {
    $scope.Saved = false;

    $scope.submit = function (game) {
        $scope.saved = false;
        $http({
            method: "POST",
            url: "/api/Host/SaveQuestions",
            data: game
        }).then(function mySuccess(response) {
            $scope.$parent.$broadcast('showMessage', {
                text: "Questions have been saved."
            })
        });
    }


}]);

hostApp.controller('resultsController', ['$scope', '$http', function ($scope, $http) {

    $scope.game = {};
    $scope.$on('Joined', function (event, args) {
        $scope.game = args.game;
        $scope.showResults = $scope.game.metadata.showResults;
        $scope.getResults();
    });

    $scope.getResults = function () {
        $http({
            method: "GET",
            url: "/api/Host/GetScoredResponses",
            params: {
                "gameId": $scope.game.metadata.gameId,
            }
        }).then(function mySuccess(response) {
            $scope.show = true;
            $scope.results = response.data;

        }, function myError(response) {
            console.log("error");
        });
    }

    $scope.submitShowHideResults = function (show) {
        $scope.game.metadata.showResults = show;
        $http({
            method: "POST",
            url: "/api/Host/ShowResults",
            data: $scope.game.metadata

        }).then(function mySuccess(response) {
            $scope.game.metadata = response.data;

        }, function myError(response) {
            console.log("error");
        });
    }
}]);

hostApp.controller('modal', ['$scope', '$http', function ($scope, $http) {
    $scope.show = false;
    $scope.$on('showMessage', function (event, args) {
        $scope.text = args.text;
        $scope.show = true;
    });

}])











var playApp = angular.module('playApp', []);

playApp.controller('myCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.state = "Join";

    $scope.roundNames = {
        "1": "One",
        "2": "Two",
        "3": "Three",
        "4": "Half Time",
        "5": "Four",
        "6": "Five",
        "7": "Six",
        "8": "Final"
    };

    $scope.shortRoundNames = {
        "1": "1",
        "2": "2",
        "3": "3",
        "4": "HT",
        "5": "4",
        "6": "5",
        "7": "6",
        "8": "F"
    };

}]);




playApp.controller('questionController', ['$scope', '$http', '$interval', function ($scope, $http, $interval) {
    $scope.answer = {
        player: {
            name: ""
        },
        answerText: "",
        wager: 0,
        questionNumber: 0,
        round: 0,
        gameId: 0
    };
    $scope.question = {
        round: 1,
        number: 1,
        text: "",
        category: ""
    }
    $scope.getCurrentQuestion = function () {
        $http({
            method: "GET",
            url: "/api/Play/GetCurrentQuestion",
            params: {
                "gameId": $scope.answer.gameId
            }
        }).then(function mySuccess(response) {
            $scope.question = response.data;
        }, function myError(response) {
            console.log("error");
        });
    }

    $scope.$on('Joined', function (event, args) {

        $scope.answer.gameId = args.game;
        $scope.answer.player = args.player;
        $scope.getCurrentQuestion();
        $interval(function () {
            $scope.getCurrentQuestion();
        }, 1000);
    });



    $scope.submitAnswer = function (answer) {
        //answer.gameId = $scope.$parent.selectedGame.gameId;
        //answer.player.name = $scope.$parent.player.name;
        answer.questionNumber = $scope.question.number;
        answer.round = $scope.question.round;
        answer.wager = answer.wager * 1;
        if (answer.wager == 0 || answer.answerText == "") {
            $scope.$parent.$broadcast('showMessage', {
                text: "Answer or Wager not set!",
                error: true
            })
            return;
        }

        $http({
            method: "POST",
            url: "/api/Play/SubmitAnswer",
            data: JSON.stringify(answer)
        }).then(function mySuccess(response) {
            $scope.saved = response.data;

            $scope.$parent.$broadcast('showMessage', {
                text: "Your answer was received: " + answer.answerText + " for " + answer.wager + " points",
                error: false
            })
        }, function myError(response) {
            console.log("error");
        });
    }
}]);

playApp.controller('setupController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
    var name = localStorage.getItem('player');
    var gameId = localStorage.getItem('gameId');
    var gameName = localStorage.getItem('gameName');

    $scope.selectedGame = {
        name: gameName,
        gameId: gameId
    };
    $scope.player = {
        name: name
    };


    $http({
        method: "GET",
        url: "/api/Play/LoadGames"
    }).then(function mySuccess(response) {
        $scope.games = response.data;
    }, function myError(response) {
        console.log("error");
    });

    $scope.join = function (game, player) {
        $scope.selectedGame = JSON.parse(game);

        if (game == null || player.name == "") {
            $scope.$parent.$broadcast('showMessage', {
                text: "Please set all items before continuing",
                error: true
            })
            return;
        }


        localStorage.setItem('player', player.name);
        localStorage.setItem('gameId', $scope.selectedGame.gameId);
        localStorage.setItem('gameName', $scope.selectedGame.name);
        $scope.setupComplete($scope.selectedGame, player);
    }

    $scope.setupComplete = function(game, player)
    {
        $scope.$parent.player = player;
        $scope.$parent.selectedGame = $scope.selectedGame;
        $scope.$parent.state = 'Answer';
        $scope.$parent.$broadcast('Joined', {
            game: game.gameId,
            player: player
        });
    }

    if (name != null && gameId != null && gameName !=null) {
        $timeout(function () {
            $scope.setupComplete($scope.selectedGame, $scope.player);
        }, 10);
    }

}]);

playApp.controller('modal', ['$scope', '$http', function ($scope, $http) {
    $scope.show = false;
    $scope.$on('showMessage', function (event, args) {
        $scope.text = args.text;
        $scope.show = true;
    });

}])

playApp.controller('playerResults', ['$scope', '$http', '$interval', function ($scope, $http, $interval) {
    $scope.show = false;
    $scope.score = 0;
    $scope.gameId = 0;
    $scope.player = {
        name: ""
    };

    $scope.$on('Joined', function (event, args) {
        $scope.gameId = args.game;
        $scope.player = args.player;
        $interval(function () {
            $http({
                method: "GET",
                url: "/api/Play/GetScoredResponsesForPlayer",
                params: {
                    "gameId": $scope.gameId,
                    "name": $scope.player.name
                }
            }).then(function mySuccess(response) {
                $scope.show = true;
                $scope.scores = response.data;
                $scope.score = 0;
                $scope.scores.forEach(score => {
                    $scope.score += score.pointsAwarded;
                });

            }, function myError(response) {
                console.log("error");
            });
        }, 5000);
    });



}])


playApp.controller('resultsController', ['$scope', '$http', '$interval', function ($scope, $http, $interval) {
    $scope.show = false;
    $scope.gameId = 0;
    $scope.$on('Joined', function (event, args) {
        $scope.gameId = args.game;
        $interval(function () {
            $scope.getResults();
        }, 5000);
    });

    $scope.getResults = function () {
        $http({
            method: "GET",
            url: "/api/Play/GetScoredResponses",
            params: {
                "gameId": $scope.gameId
            }
        }).then(function mySuccess(response) {
            $scope.results = response.data;
            if ($scope.results.length > 0) {
                $scope.show = true;
                $scope.$parent.state = "Results";
            } else {
                $scope.show = false;
                $scope.$parent.state = "Answer";
            }

        }, function myError(response) {
            console.log("error");
        });
    }
}]);