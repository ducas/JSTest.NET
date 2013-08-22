using JSTest.ScriptElements;
using System;

namespace JSTest
{
    public interface IScriptEngine
    {
        String Execute(String script);
        String Convert(ScriptElement element);
    }
}
