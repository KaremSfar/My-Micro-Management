namespace MicroManagement.Shared
{
    public class DatabaseSettings
    {
        public static string SectionName = "Database";

        public string DatabaseType { get; set; }

        public string ConnectionString { get; set; }
    }
}
