using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OccamsRazor.Common.Models;
using OccamsRazor.Common.AuthenticationModels;

namespace OccamsRazor.Common.Context
{
    public class OccamsRazorEfSqlContext : DbContext
    {
        public static string GAMEMETADATA_TABLE;
        public static string QUESTION_TABLE;
        public static string MC_QUESTION_TABLE;
        public static string ANSWER_TABLE;
        public static string MC_ANSWER_TABLE;
        public static string KEY_TABLE;
        public DbSet<GameMetadata> GameMetadata { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoiceQuestion> McQuestions { get; set; }
        public DbSet<PlayerAnswer> Answers { get; set; }
        public DbSet<AuthenticationModel> Keys { get; set; }

        public OccamsRazorEfSqlContext(DbContextOptions<OccamsRazorEfSqlContext> options): base(options) {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GameMetadata>().ToTable(GAMEMETADATA_TABLE).HasKey(q => q.GameId);
            builder.Entity<GameMetadata>().Property(g => g.GameId).HasColumnName("GameId");
            builder.Entity<GameMetadata>().Property(g => g.Name).HasColumnName("Name");
            builder.Entity<GameMetadata>().Property(g => g.CurrentRound).HasColumnName("CurrentRoundNum");
            builder.Entity<GameMetadata>().Property(g => g.CurrentQuestion).HasColumnName("CurrentQuestionNum");
            builder.Entity<GameMetadata>().Property(g => g.State).HasColumnName("State");
            builder.Entity<GameMetadata>().Property(g => g.Seed).HasColumnName("Seed");
            builder.Entity<GameMetadata>().Property(g => g.isMc).HasColumnName("MC");
            builder.Entity<GameMetadata>().Ignore(g => g.IsMultipleChoice);

            builder.Entity<Question>().ToTable(QUESTION_TABLE).HasKey(q => new {q.GameId, q.Round, q.Number});
            builder.Entity<Question>().Property(q => q.GameId).HasColumnName("GameId");
            builder.Entity<Question>().Property(q => q.Round).HasColumnName("RoundNum");
            builder.Entity<Question>().Property(q => q.Number).HasColumnName("QuestionNum");
            builder.Entity<Question>().Property(q => q.Text).HasColumnName("QuestionText");
            builder.Entity<Question>().Property(q => q.Category).HasColumnName("CategoryText");
            builder.Entity<Question>().Property(q => q.AnswerText).HasColumnName("AnswerText");

            builder.Entity<MultipleChoiceQuestion>().ToTable(MC_QUESTION_TABLE).HasKey(q => new {q.GameId, q.Number});
            builder.Entity<MultipleChoiceQuestion>().Property(q => q.GameId).HasColumnName("GameId");
            builder.Entity<MultipleChoiceQuestion>().Property(q => q.Round).HasColumnName("RoundNum");
            builder.Entity<MultipleChoiceQuestion>().Property(q => q.Number).HasColumnName("QuestionNum");
            builder.Entity<MultipleChoiceQuestion>().Property(q => q.Text).HasColumnName("QuestionText");
            builder.Entity<MultipleChoiceQuestion>().Property(q => q.Category).HasColumnName("CategoryText");
            builder.Entity<MultipleChoiceQuestion>().Property(q => q.AnswerId).HasColumnName("AnswerId");
            builder.Entity<MultipleChoiceQuestion>().Property(q => q.possibleAnswers).HasColumnName("PossibleAnswers");
            builder.Entity<MultipleChoiceQuestion>().Ignore(q => q.PossibleAnswers);

            builder.Entity<PlayerAnswer>().ToTable(ANSWER_TABLE).HasKey(p => p.Id);
            builder.Entity<PlayerAnswer>().OwnsOne(a => a.Player).Property(p => p.Name).HasColumnName("PlayerName");
            builder.Entity<PlayerAnswer>().Property(a => a.Id).HasColumnName("Id");
            builder.Entity<PlayerAnswer>().Property(a => a.GameId).HasColumnName("GameId");
            builder.Entity<PlayerAnswer>().Property(a => a.Round).HasColumnName("RoundNum");
            builder.Entity<PlayerAnswer>().Property(a => a.QuestionNumber).HasColumnName("QuestionNum");
            builder.Entity<PlayerAnswer>().Property(a => a.AnswerText).HasColumnName("AnswerText");

            builder.Entity<AuthenticationModel>().ToTable(KEY_TABLE).HasKey(k => k.GameId);
            builder.Entity<AuthenticationModel>().Property(a => a.GameId).HasColumnName("GameId");
            builder.Entity<AuthenticationModel>().Property(a => a.GameKey).HasColumnName("GameKey");
        }
    }
}