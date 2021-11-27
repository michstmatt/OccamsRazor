using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Persistence.Service
{
    public class GameDataService : IGameDataService
    {
        private readonly IGameDataRepository gameDataRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IMultipleChoiceRepository multipleChoiceRepository;
        public GameDataService(IGameDataRepository gdr, IQuestionRepository qr, IMultipleChoiceRepository mcqr)
        {
            gameDataRepository = gdr;
            questionRepository = qr;
            multipleChoiceRepository = mcqr;
        }

        private void InitBlankQuestions(Game game)
        {
            foreach (var pair in game.Format)
            {
                var round = pair.Key;
                for (int i = 0; i < pair.Value; i++)
                {
                    game.Questions.Add(new Question { Round = pair.Key, Number = i + 1, Text = "", AnswerText = "", Category = "" });
                }
            }
        }

        public async Task<GameMetadata> CreateGameAsync(Game game)
        {
            var response = await gameDataRepository.InsertGameMetadataAsync(game.Metadata);

            if (game.Metadata.IsMultipleChoice == false)
            {
                InitBlankQuestions(game);
                var typedQuestions = game.Questions.Select(q => (Question)q).ToList();
                await questionRepository.InsertQuestionsAsync(game.Metadata.GameId, typedQuestions);
            }
            return response;

        }
        public async Task<GameMetadata> SaveGameAsync(Game game)
        {
            GameMetadata response;
            bool exists = (await gameDataRepository.GetGameMetadataAsync(game.Metadata.GameId)) != null;

            if (exists)
            {
                response = await gameDataRepository.UpdateExistingGameMetadataAsync(game.Metadata);

                if (game.Metadata.IsMultipleChoice == false)
                {
                    var typedQuestions = game.Questions.Select(q => (Question)q).ToList();
                    await questionRepository.UpdateExistingQuestionsAsync(game.Metadata.GameId, typedQuestions);
                }
            }
            else
                throw new NullReferenceException();

            return response;
        }
        public async Task<AbstractGame> LoadGameAsync(int gameId)
        {
            AbstractGame abstractGame;
            var md = await gameDataRepository.GetGameMetadataAsync(gameId);
            if (md.IsMultipleChoice)
            {
                var game = new MultipleChoiceGame();
                game.Questions = (await multipleChoiceRepository.LoadQuestionsAsync(md.Seed, 12)).ToList();
                abstractGame = game;
            }
            else
            {
                var game = new Game();
                game.Questions = (await questionRepository.LoadQuestionsAsync(md.GameId)).ToList();
                abstractGame = game;
            }

            abstractGame.Metadata = md;
            return abstractGame;
        }

        public async Task<IEnumerable<GameMetadata>> LoadGamesAsync() => await gameDataRepository.GetExistingGamesAsync();

        public async Task<AbstractQuestion> GetCurrentQuestionAsync(int gameId)
        {
            var metadata = await gameDataRepository.GetGameMetadataAsync(gameId);
            if (metadata.IsMultipleChoice)
                return await multipleChoiceRepository.GetQuestionAsync(metadata.Seed, metadata.CurrentQuestion);
            else
                return await questionRepository.GetQuestionAsync(metadata.GameId, metadata.CurrentRound, metadata.CurrentQuestion);
        }

        public async Task<AbstractQuestion> SetCurrentQuestionAsync(GameMetadata game)
        {
            await gameDataRepository.UpdateExistingGameMetadataAsync(game);
            return await GetCurrentQuestionAsync(game.GameId);
        }

        public async Task<GameMetadata> SetGameStateAsync(GameMetadata game)
        {
            return await gameDataRepository.UpdateExistingGameMetadataAsync(game);
        }

        public async Task<GameMetadata> GetGameStateAsync(int gameId)
        {
            return await gameDataRepository.GetGameMetadataAsync(gameId);
        }

        public async Task<bool> DeleteGameAsync(int gameId)
        {
            var existing = await gameDataRepository.GetGameMetadataAsync(gameId);
            if (existing == null)
                throw new NullReferenceException();
            await gameDataRepository.DeleteGameMetadataAsync(gameId);

            if (existing.IsMultipleChoice == false)
                await questionRepository.DeleteQuestionsAsync(gameId);

            return true;
        }

    }
}