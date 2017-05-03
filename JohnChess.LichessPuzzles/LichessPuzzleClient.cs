using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;

using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JohnChess.LichessPuzzles
{
    public class LichessPuzzleClient
    {
        private readonly JsJsonParser jsJsonParser = new JsJsonParser();
        private readonly Regex LichessPuzzleRegex = new Regex("lichess\\.puzzle[\\s]?\\=[\\s]?");
        private const string PUZZLE_BASE = "https://en.lichess.org/training/{0}";



        public async Task<dynamic> GetJsonForPuzzleAsync(int puzzleNumber)
        {
            string url = string.Format(PUZZLE_BASE, puzzleNumber.ToString());
            string html = await GetHtmlContentAsync(url);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var scripts = htmlDoc.DocumentNode.Descendants("script");
            var validScripts = (from s in scripts
                                where LichessPuzzleRegex.IsMatch(s.InnerText)
                                select s.InnerText);

            string javaScriptCode = validScripts.First();
            string puzzleVal = jsJsonParser.GetJsonFromJsScriptVariable(javaScriptCode, "lichess");
            dynamic json = JObject.Parse(puzzleVal);
            return json;
        }
        private async Task<string> GetHtmlContentAsync(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            using (var response = await request.GetResponseAsync())
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
