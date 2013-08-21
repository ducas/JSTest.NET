﻿using System;
using System.Text;
using JSTest.ScriptElements;
using JSTest.ScriptLibraries;

/* Copyright (c) 2011 CBaxter
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
 * IN THE SOFTWARE. 
 */

namespace JSTest
{
    public class TestScript
    {
        private const String NoAction = "";
        private const String Breakpoint = "debugger;";
        private readonly StringBuilder _script = new StringBuilder();
        private readonly IScriptEngine _scriptCommand;

        public Boolean IncludeDefaultBreakpoint { get; set; }

        public TestScript()
            : this(TimeSpan.FromSeconds(10))
        { }

        public TestScript(TimeSpan timeout)
            : this(new CScriptEngine(timeout))
        { }

        internal TestScript(IScriptEngine scriptCommand)
        {
            IncludeDefaultBreakpoint = true;

            _scriptCommand = scriptCommand;
        }

        public void AppendBlock(String scriptBlock)
        {
            AppendBlock(new ScriptBlock(scriptBlock));
        }

        public void AppendBlock(ScriptBlock scriptBlock)
        {
            Verify.NotNull(scriptBlock, "scriptBlock");

            AppendLine(scriptBlock);
        }

        public void AppendFile(String fileName)
        {
            AppendLine(new ScriptInclude(fileName));
        }

        public void AppendFile(ScriptInclude scriptInclude)
        {
            Verify.NotNull(scriptInclude, "scriptInclude");

            AppendLine(scriptInclude);
        }

        private void AppendLine(ScriptElement element)
        {
            _script.AppendLine(_scriptCommand.Convert(element));
        }

        public String RunTest(TestCase testCase)
        {
            return RunTest(testCase, NoAction, NoAction);
        }

        public String RunTest(TestCase testCase, String setup, String teardown)
        {
            Verify.NotNull(testCase, "testCase");

            return RunTest(testCase.ToScriptFragment(), setup, teardown);
        }

        public String RunTest(String testScript)
        {
            return RunTest(testScript, NoAction, NoAction);
        }

        public String RunTest(String testScript, String setup, String teardown)
        {
            var finalScript = new StringBuilder(_script.ToString());
            finalScript.AppendLine(_scriptCommand.Convert(new JsonLibrary()));
            finalScript.AppendLine(_scriptCommand.Convert(CreateExecutor(setup, testScript, teardown)));

            return _scriptCommand.Execute(finalScript.ToString());
        }

        private TestExecutor CreateExecutor(String setup, String testScript, String teardown)
        {
            setup = (setup ?? NoAction).Trim();
            teardown = (teardown ?? NoAction).Trim();

            if (IncludeDefaultBreakpoint)
                setup = Breakpoint + (String.IsNullOrEmpty(setup) ? NoAction : Environment.NewLine + setup);

            return new TestExecutor(setup, testScript, teardown);
        }

        public override string ToString()
        {
            return _script.ToString();
        }
    }
}
