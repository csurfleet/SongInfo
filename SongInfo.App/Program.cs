using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using SongInfo.Api;

namespace SongInfo.App
{
    class Program
    {
        const string helpOptions = "-h|--h|-?";
        static void Main(string[] args)
        {
            var app = new CommandLineApplication<Program>(throwOnUnexpectedArg: false);

            app.Command("search",
                (search) =>
                {
                    search.Description = "Search for musical information by a variety of options";
                    search.ShowInHelpText = true;

                    var searchOptions = new List<CommandOption> // TODO: Add further search options
                    {
                        search.Option("-a|--artist", "Search by artist", CommandOptionType.SingleValue)
                    };

                    foreach (var option in searchOptions)
                        option.ShowInHelpText = true;

                    search.HelpOption(helpOptions);

                    search.OnExecute(() => 
                    {
                        var results = Search(searchOptions).Result;

                        foreach (var result in results)
                            Console.WriteLine(result);
                    });
                });
            
            app.HelpOption(helpOptions);
	        app.Execute(args);

        }

        private static async Task<IEnumerable<string>> Search(List<CommandOption> searchOptions)
        {
            var optionCount = searchOptions.Count(o => o.HasValue());

            if (optionCount == 0)
                return new[] { "No search option specified, options are --artist" };

            if (optionCount > 1)
                return new [] { "Too many search options specified, please chooose only one" };
            
            var option = searchOptions.First(o => o.HasValue());

            switch (option.LongName)
            {
                case "artist":
                    var runner = new ArtistCommandRunner();
                    var results = await runner.RunCommand(option.Value());
                    results.Insert(0, "Searching artists...");
                    return results;
                default:
                    return new[] { $"Unknown search option {option.LongName}, system error" }; // In reality this would throw and enter our exception handling process
            }
        }
    }
}
