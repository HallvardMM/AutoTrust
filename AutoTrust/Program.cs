using AutoTrust;
using System;
using System.Net.Http.Json;

Console.WriteLine("Ready for Chuck Norris jokes?");

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.chucknorris.io/")
};


var query = args.AsQueryable().FirstOrDefault();

Start:
if (query == null)
{
    Console.Write("Query: ");
    query = Console.ReadLine()!.Trim();
    if (query == "exit" || query == "exit()")
    {
        return;
    }
}

var jokes = await httpClient.GetFromJsonAsync<ChuckNorrisJokes>($"jokes/search?query={query}");
if(jokes == null || jokes.Total == 0)
{
    Console.WriteLine($"Jokes not found with query '{query}'!");
    query = null;
    goto Start;
}
Console.WriteLine("Here are some jokes! Enjoy!");

if (jokes.Result == null)
{
    Console.WriteLine("No jokes found!");
    query = null;
    goto Start;
}

foreach (ChuckNorrisJoke joke in jokes.Result)
{
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine(joke.Value);
}
query = null;
goto Start;
