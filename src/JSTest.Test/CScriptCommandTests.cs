using System;
using System.IO;
using Xunit;
using Xunit.Extensions;

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

namespace JSTest.Test
{
    public class WhenUsingCScriptCommand
    {
        [Theory, InlineData(0), InlineData(100), InlineData(Int16.MaxValue)]
        public void TimeoutAcceptedIfSecondsBetweenZeroAnd32768(Int32 timeout)
        {
            Assert.DoesNotThrow(() => new CScriptEngine(TimeSpan.FromSeconds(timeout)));
        }

        [Theory, InlineData(-1), InlineData(Int16.MaxValue + 1)]
        public void TimeoutRejectedIfSecondsNotBetweenZeroAnd32767(Int32 timeout)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CScriptEngine(TimeSpan.FromSeconds(timeout)));
        }

        [Fact]
        public void RunReturnsStdOutText()
        {
            Assert.Equal("Sample Text", new CScriptEngine().Execute(@"<script language='JavaScript'>WScript.Echo('Sample Text');</script>"));
        }

        [Fact]
        public void RunThrowsInvalidProgramExceptionOnBadInputFile()
        {
            var ex = Assert.Throws<ScriptException>(() => new CScriptEngine().Execute(@"<script language='JavaScript'>function ) { return 'Missing Opening ('; }</script>"));

            Assert.Contains("Microsoft JScript compilation error: Expected '('", ex.Message);
        }

        [Fact]
        public void RunTimesOutOnLongRunningScript()
        {
            var ex = Assert.Throws<ScriptException>(() => new CScriptEngine(TimeSpan.FromMilliseconds(100)).Execute(@"<script language='JavaScript'>while(true) { }</script>"));

            Assert.Contains("Script execution time was exceeded on script", ex.Message);
        }
    }
}
