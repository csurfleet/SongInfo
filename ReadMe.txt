The solution I've provided comes in 2 parts - a .net CLI interface and a set of
BDD Specifications running in xunit. Instructions are provided below for running them,
followed by some notes on the design and things I could have added given more time.

System requirements
Any system with the .net core  3 framework for the CLI
Any system with the .net core 3 SDK for the tests

The CLI interface
This can be found in the CompiledProduct folder and can be interogated via powershell:
.\SongInfo search --artist "Arctic Monkeys"

There are more options available in the CLI, to take a look try:
.\SongInfo -h
or
.\SongInfo search -h

The BDD Specifications
This solution shows how I like to use BDD for integration tests. You can find the
business-readable Specifications as well as the automation for them in the SongInfo.Spec folder, but
if you simply want to run them, just call dotnet test in this folder.

Please note - if the lyrics.ovh service is taking too long to respond (I cap at 10s)
I count the song as having no words - this will cause one of the tests to fail! In a real
system I would have mocked out the webservice and this wouldn't be an issue in 99% of tests.

Things which are missing
If I had more time I would have continued to add:
- This only works for Artists with a unique Name - with a web UI we could allow clicking into
    the Artist of choice
- One of the calls can timeout when lyrics.ovh takes too long - exception handling around that could be better
- Dependency Injection. This would make the app easier to update, and would allow me to decouple the
    integration tests from the live webservice (if so desired)
- A web interface. A UI for those who hate the CLI.
- An additional set of BDD automations, showing how both the CLI and the web interface could be tested
    from the same set of BDD specifications
- Better configuration for the web services, they ideally would take an IHttpClientFactory to get their
    clients from
- Dockerization

If you would like me to expand on any of the above I'm happy to do so, thanks!