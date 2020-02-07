using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SongInfo.Api;
using Xunit.Gherkin.Quick;

namespace SongInfo.Spec.Steps
{
    [FeatureFile("./Specifications/SearchingForArtists.feature")]
    public class SearchingForArtistsSteps : Feature
    {
        // This file is automation for the BDD specs in the file referenced above.

        // Keeping the workings of the console app really thin allows us to run our tests
        // directly through the ArtistCommandRunner, which speeds them up considerably without
        // much extra risk.
        private ArtistCommandRunner _unit;
        private List<string> _results;

        [Given("an artist search prompt")]
        public void GivenSearchPrompt() => _unit = new ArtistCommandRunner();

        [When(@"'([\w\d\s]+)' is entered")]
        public async Task WhenSearchEntered(string searchTerm) => _results = await _unit.RunCommand(searchTerm);

        [Then(@"the message '([\w\d\s:]+)' is returned")]
        [And(@"the message '([\w\d\s:]+)' is returned")]
        public void ThenMessageIsReturned(string expectedMessage) => _results
                .Any(m => m.Contains(expectedMessage))
                .Should()
                .BeTrue(
                    "because we need the expected message to be in at least one result message");
    }
}