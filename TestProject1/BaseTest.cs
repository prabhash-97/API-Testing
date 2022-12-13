using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace TestProject
{
    public class BaseTest
    {
        protected ILogger logger;


        public static IConfiguration GetCommonConfiguration()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "AppSettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonFile(configPath, true, true)
                .AddEnvironmentVariables()
                .Build();
            return config;
        }


        [SetUp]
        public void Setup()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders().AddConfiguration(GetCommonConfiguration().GetSection("Logging"));
                builder.AddConsole();

            });

            this.logger = loggerFactory.CreateLogger<BaseTest>();
            logger.LogInformation($"Use TestUrl");
        }

        public void WriteToFile(string fileType, string description)
        {

            var foldrPath = GetCommonConfiguration().GetSection("TestSettings").GetChildren();
            var projectDir = Directory.GetDirectories("C:\\kash\\TestProject2\\Logs");
            var fileName = "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            var filePath = Path.Combine(projectDir.FirstOrDefault(), fileName);
            if (!File.Exists(filePath))
            {

                var fileCreated = File.Create(filePath);
                fileCreated.Close();
                File.WriteAllText(filePath, DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + " " +
                    fileType + " : " + description);
                fileCreated.Close();
            }
            else
            {
                File.AppendAllText(filePath, DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + " " +
                    fileType + " : " + description);
            }


        }
    }
}
