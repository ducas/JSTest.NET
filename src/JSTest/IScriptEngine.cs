using JSTest.ScriptElements;
using System;

namespace JSTest
{
    internal interface IScriptEngine
    {
        String Execute(String script);
        String Convert(ScriptElement element);
    }
}
