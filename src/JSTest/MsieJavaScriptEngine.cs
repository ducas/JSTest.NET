using JSTest.ScriptElements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine = MsieJavaScriptEngine.MsieJsEngine;

namespace JSTest
{
    internal class MsieJavaScriptEngine : IScriptEngine
    {
        public string Execute(string script)
        {
            var engine = new Engine(true, true);

            var stringifiedResult = engine.Evaluate(script) as string;
            var sampleResult = new { successful = false, result = "", error = "" };
            dynamic json = JsonConvert.DeserializeAnonymousType(stringifiedResult, sampleResult);
            if (!json.successful) throw new ScriptException(json.error);
            return json.result;
        }

        public string Convert(ScriptElement element)
        {
            return element.ToInlineScriptFragment();
        }
    }
}
