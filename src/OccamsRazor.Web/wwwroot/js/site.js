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
        $http({
            method: "GET",
            url: "/api/Host/GetQuestions",
            params: { "id": $scope.selectedGame.gameId }
        }).then(function mySuccess(response) {
            $scope.game = response.data;
            $scope.state = "menu";
            $scope.updateView($scope.state, $scope.game);
        }, function myError(response) {
            console.log("error");
        });
    }

    $scope.updateView = function (state, game) {
        $scope.$broadcast('Joined', { state: state, game: game });
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
        /*$interval(function () {
            $scope.reload($scope.game);
        }, 1000);*/
    })

    $scope.reload = function (game) {
        $http({
            method: "GET",
            url: "/api/Host/GetPlayerAnswers",
            params: { "id": game.metadata.gameId },
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
        });
    }

    $scope.playerScoreChanged = function (playerAnswer, score) {
        if (score == "Correct") {
            playerAnswer.pointsAwarded = playerAnswer.wager;
        }
        else if (score == "Incorrect") {
            playerAnswer.pointsAwarded = 0;
        }
    }

    $scope.updateScores = function (playerAnswers) {
        $http({
            method: "POST",
            url: "/api/Host/updatePlayerScores",
            data: playerAnswers
        }).then(function mySuccess(response) {

        });
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
            $scope.$parent.$broadcast('showMessage', {text: "Questions have been saved."})
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
    $scope.selectedGame = "";
    $scope.player = { name: "" };

}]);

playApp.controller('setupController', ['$scope', '$http', function ($scope, $http) {
    $scope.player = { name: "" };
    $http({
        method: "GET",
        url: "/api/Play/LoadGames"
    }).then(function mySuccess(response) {
        $scope.games = response.data;
    }, function myError(response) {
        console.log("error");
    });

    $scope.join = function (game, player) {
        $scope.$parent.selectedGame = game;
        $scope.$parent.player = player;
        $scope.$parent.state = "Answer";
        $scope.$parent.$broadcast('joined', { game: game * 1, player: player });
    }



}]);


playApp.controller('questionController', ['$scope', '$http', '$interval', function ($scope, $http, $interval) {
    $scope.answer = {
        player:
        {
            name: ""
        },
        answerText: "",
        wager: 0,
        questionNumber: 0,
        round: 0,
        gameId: 0
    };

    $scope.$on('joined', function (event, args) {


        $scope.answer.gameId = args.game;
        $scope.answer.player = args.player;
        $scope.getCurrentQuestion();
        $interval(function () {
            $scope.getCurrentQuestion();
        }, 1000);
    });

    $scope.getCurrentQuestion = function () {
        $http({
            method: "GET",
            url: "/api/Play/GetCurrentQuestion",
            params: { "gameId": $scope.answer.gameId }
        }).then(function mySuccess(response) {
            $scope.question = response.data;
        }, function myError(response) {
            console.log("error");
        });
    }

    $scope.submitAnswer = function (answer) {
        //answer.gameId = $scope.$parent.selectedGame.gameId;
        //answer.player.name = $scope.$parent.player.name;
        answer.questionNumber = $scope.question.number;
        answer.round = $scope.question.round;
        $http({
            method: "POST",
            url: "/api/Play/SubmitAnswer",
            data: JSON.stringify(answer)
        }).then(function mySuccess(response) {
            $scope.saved = response.data;

            $scope.$parent.$broadcast('showMessage', { text: "Your answer was received: " + answer.answerText + " for " + answer.wager + " points", error: false })
        }, function myError(response) {
            console.log("error");
        });
    }
}]);

playApp.controller('modal', ['$scope', '$http', function ($scope, $http) {
    $scope.show = false;
    $scope.$on('showMessage', function (event, args) {
        $scope.text = args.text;
        $scope.show = true;
    });

}])

playApp.controller('playerResults', ['$scope', '$http', function ($scope, $http) {
    $scope.show = false;

    $scope.$on('joined', function(event, args)
    {
        alert(JSON.stringify(args));
        $http({
            method: "GET",
            url: "/api/Play/GetScoredResponses",
            params: { "gameId": args.game, "name": args.player.name }
        }).then(function mySuccess(response) {
            $scope.scores = response.data;
        }, function myError(response) {
            console.log("error");
        });
    });

}])