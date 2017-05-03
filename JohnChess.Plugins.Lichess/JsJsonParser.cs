using System;
using System.Collections.Generic;
using System.Text;

using Jint;

namespace JohnChess.Plugins.Lichess
{
    public class JsJsonParser
    {
        public string GetJsonFromJsScriptVariable(string javascript, string varName)
        {
            var engine = new Engine();
            var tempVal = engine.Execute($"var {varName} = {{}};");
            var jsObj = engine.Execute(javascript);
            engine.Execute($"var jsonStr = JSON.stringify({varName});");
            var finalStr = engine.GetValue("jsonStr").AsString();
            return finalStr;
        }
    }
}
