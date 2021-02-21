using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using OccamsRazor.Common.Context;
using OccamsRazor.Common.Models;
using OccamsRazor.DataLoader.Models;

namespace OccamsRazor.DataLoader
{
    class Program
    {
        private static string DATA_FILE = "DATA_FILE";
        private static string CONNECTION_STRING = "CONNECTION_STRING";
        private static string MC_QUESTIONS_TABLE = "MC_QUESTIONS_TABLE";
        static void Main(string[] args)
        {
            var dataFile = System.Environment.GetEnvironmentVariable(DATA_FILE);
            var connString = System.Environment.GetEnvironmentVariable(CONNECTION_STRING);
            var mcTable = System.Environment.GetEnvironmentVariable(MC_QUESTIONS_TABLE);
            if (dataFile == null || connString == null || mcTable == null) throw new ArgumentNullException();

            var options = new DbContextOptionsBuilder<OccamsRazorEfSqlContext>();
            options.UseSqlServer(connString);
            var context = new OccamsRazorEfSqlContext(options.Options);
            OccamsRazorEfSqlContext.MC_QUESTION_TABLE = mcTable;

            loadMcQuestionData(context);
        }

        static MultipleChoiceQuestion mapMcSourceToSink(SourceMultipleChoiceQuestion q, int index) =>
         new MultipleChoiceQuestion()
            {
                GameId = 0,
                Category = "",
                Number = index,
                Round = 0,
                Text = q.Question,
                PossibleAnswers = q.PossibleAnswers.ToDictionary(keyItem => keyItem.Choice, valueItem => valueItem.Text),
                AnswerId = q.PossibleAnswers.Where(x => x.Correct).Select(x=>x.Choice).FirstOrDefault()
            };

        static void storeMcQuestionData(OccamsRazorEfSqlContext context, string dataFile)
        {
            var text = File.ReadAllText("/home/ubuntu/projects/OccamsRazor/DB.json");
            var data = JsonSerializer.Deserialize<List<SourceMultipleChoiceQuestion>>(text);
            int index =0;
            var mapped = data.Select(x => mapMcSourceToSink(x, index++));
            context.McQuestions.AddRange(mapped);
            context.SaveChanges();
            Console.ReadLine();
        }

        static void loadMcQuestionData(OccamsRazorEfSqlContext context)
        {
            var data = context.McQuestions.ToArray();
            var rand = new Random(1);
            
            var selected = data.OrderBy(_ => rand.Next()).Take(12);
            Console.WriteLine(data[0].Text);
        }

    }
}
