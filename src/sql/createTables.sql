IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'GameMetadata')
		BEGIN
				CREATE TABLE GameMetadata(gameId int DEFAULT 0, 
						name varchar(255),
						currentRoundNum int DEFAULT 0,
						currentQuestionNum int DEFAULT 0,
						state int default 0,
						mc int default 0,
						seed int default 0);
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'MultipleChoiceGameMetadata')
		BEGIN
				CREATE TABLE MultipleChoiceGameMetadata(gameId int DEFAULT 0, 
						name varchar(255),
						currentQuestion int DEFAULT 0,
						state int default 0,
						seed int default 0);
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Questions')
		BEGIN
				CREATE TABLE Questions(gameId int DEFAULT 0,
						roundNum int DEFAULT 0,
						questionNum int DEFAULT 0,
						questionText varchar(1024) DEFAULT '',
						categoryText varchar(1024) DEFAULT '',
						answerText varchar(1024) DEFAULT '')
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'MultipleChoiceQuestions')
		BEGIN
				CREATE TABLE MultipleChoiceQuestions(gameId int DEFAULT 0,
						roundNum int DEFAULT 0,
						questionNum int DEFAULT 0,
						questionText varchar(1024) DEFAULT '',
						categoryText varchar(1024) DEFAULT '',
						possibleAnswers varchar(2048) DEFAULT '',
						answerId varchar (10) DEFAULT '');
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PlayerAnswers')
		BEGIN
				CREATE TABLE PlayerAnswers(gameId int DEFAULT 0,
						playerName varchar(1024) DEFAULT '',
						id int DEFAULT 0,
						roundNum int DEFAULT 0,
						questionNum int DEFAULT 0,
						wager int DEFAULT 0,
						answerText varchar(1024) DEFAULT '',
						pointsAwarded int DEFAULT 0);
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'MultipleChoiceAnswers')
		BEGIN
				CREATE TABLE MultipleChoiceAnswers(gameId int DEFAULT 0,
						playerName varchar(1024) DEFAULT '',
						id int DEFAULT 0,
						roundNum int DEFAULT 0,
						questionNum int DEFAULT 0,
						answerId varchar(5) DEFAULT '');
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'GameKeys')
		BEGIN
				CREATE TABLE GameKeys(gameId int NOT NULL, gameKey varchar (1024) DEFAULT '');
END
