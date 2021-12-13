namespace OccamsRazor.Common.Configuration
{
    public class DbConfiguration
    {
        public string ConnectionString { get; set; }
        public string MariaDbVersion { get; set; }
        public string AnswerTable { get; set; }
        public string GameMetadataTable { get; set; }
        public string QuestionsTable { get; set; }
        public string McQuestionsTable { get; set; }
        public string KeyTable { get; set; }

    }

}
