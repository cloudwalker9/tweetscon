// See https://aka.ms/new-console-template for more information
using TweetStreamCon;
using TweetStreamCon.Database;
using TweetStreamCon.Publisher;
using TweetStreamCon.Twitter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using TweetStreamCon.Database.SQLLite;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

        services.AddSingleton<SQLLiteDBConfiguration>();
        services.AddSingleton<IRepositoryReader<string>, SQLLiteSelectStatementExecutor>();
        services.AddSingleton<IUserTweetsRepositoryReader, UserTweetsRepositoryReader>();
        services.AddSingleton<IRepositoryWriter<string, string>, UserTweetsDBRepositoryWriter>();
        services.AddSingleton<IPublisher<string>, ConsolePublisher>();
        services.AddSingleton<TwitterCountPublisher>();
        services.AddSingleton<TwiterAPIConfiguration>();
        services.AddHttpClient<TwitterHttpAuthHandler>();
        services.AddHttpClient<TwitterHttpProcessor>().AddHttpMessageHandler<TwitterHttpAuthHandler>();
        

        //Host Service
        services.AddHostedService<TimeTriggeredHostService>();
        services.AddHostedService<TwittConsumerHostService>();
        services.AddSingleton<HttpStreamReaderConfig>( (serviceProvider) => {
            var Config = serviceProvider.GetService<IConfiguration>();
            var streamURI = !string.IsNullOrWhiteSpace(Config?["TwitterStreamURI"])
                ? Config["TwitterStreamURI"]
                : "https://api.twitter.com/2/tweets/sample/stream";
            return new HttpStreamReaderConfig()
            {
                URL = streamURI
            };
        });
    })
    .Build();
await host.RunAsync();


