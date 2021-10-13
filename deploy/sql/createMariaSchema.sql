CREATE DATABASE IF NOT EXISTS `trivia`;

USE `trivia`;

CREATE TABLE IF NOT EXISTS `GameMetadata` (gameId int DEFAULT 0, 
		name varchar(255),
		currentRoundNum int DEFAULT 0,
		currentQuestionNum int DEFAULT 0,
		state int default 0,
		mc int default 0,
		seed int default 0);

CREATE TABLE IF NOT EXISTS `MultipleChoiceGameMetadata` (gameId int DEFAULT 0, 
		name varchar(255),
		currentQuestion int DEFAULT 0,
		state int default 0,
		seed int default 0);

CREATE TABLE IF NOT EXISTS `Questions` (gameId int DEFAULT 0,
		roundNum int DEFAULT 0,
		questionNum int DEFAULT 0,
		questionText varchar(1024) DEFAULT '',
		categoryText varchar(1024) DEFAULT '',
		answerText varchar(1024) DEFAULT '');

CREATE TABLE IF NOT EXISTS `MultipleChoiceQuestions` (gameId int DEFAULT 0,
		roundNum int DEFAULT 0,
		questionNum int DEFAULT 0,
		questionText varchar(1024) DEFAULT '',
		categoryText varchar(1024) DEFAULT '',
		possibleAnswers varchar(2048) DEFAULT '',
		answerId varchar(10) DEFAULT '');

CREATE TABLE IF NOT EXISTS `PlayerAnswers` (gameId int DEFAULT 0,
		playerName varchar(1024) DEFAULT '',
		id int DEFAULT 0,
		roundNum int DEFAULT 0,
		questionNum int DEFAULT 0,
		wager int DEFAULT 0,
		answerText varchar(1024) DEFAULT '',
		pointsAwarded int DEFAULT 0);

CREATE TABLE IF NOT EXISTS `MultipleChoiceAnswers` (gameId int DEFAULT 0,
		playerName varchar(1024) DEFAULT '',
		id int DEFAULT 0,
		roundNum int DEFAULT 0,
		questionNum int DEFAULT 0,
		answerId varchar(5) DEFAULT '');

CREATE TABLE IF NOT EXISTS `GameKeys` (gameId int NOT NULL, gameKey varchar (1024) DEFAULT '');
