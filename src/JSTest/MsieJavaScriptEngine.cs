using JSTest.ScriptElements;
using MsieJavaScriptEngine.ActiveScript;
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
        bool _useEcmaScript5Polyfill;
        bool _useJson2Library;

        public MsieJavaScriptEngine(bool useEcmaScript5Polyfill = true, bool useJson2Library = true)
        {
            _useEcmaScript5Polyfill = useEcmaScript5Polyfill;
            _useJson2Library = useJson2Library;
        }

        public string Execute(string script)
        {
            try
            {
                var engine = new Engine(_useEcmaScript5Polyfill, _useJson2Library);

                var stringifiedResult = engine.Evaluate(script) as string;
                var sampleResult = new { successful = false, result = "", error = "" };
                dynamic json = JsonConvert.DeserializeAnonymousType(stringifiedResult, sampleResult);
                if (!json.successful) throw new ScriptException(json.error);
                return json.result;
            }
            catch (ActiveScriptException ex)
            {
                throw new ScriptException(ex.Message, ex);
            }
        }

        public string Convert(ScriptElement element)
        {
            return element.ToInlineScriptFragment();
        }
    }
}
