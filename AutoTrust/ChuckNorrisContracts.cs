using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace AutoTrust
{
    public class ChuckNorrisJoke
    {

        public List<string>? Categories { get; set; }

        public string? Created_at { get; set; }

        public string? Icon_url { get; set; }

        public string? Id { get; set; }

        public string? Url { get; set; }

        public string? Value { get; set; }

    }

    public class ChuckNorrisJokes
    {
        public int? Total { get; set; }
        public List<ChuckNorrisJoke>? Result { get; set; }

    }
}