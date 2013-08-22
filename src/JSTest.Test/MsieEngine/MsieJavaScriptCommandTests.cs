using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JSTest.Tests.MsieEngine
{
    public class MsieJavaScriptCommandTests
    {
        [Fact]
        public void ShouldUseEcmaScript5PolyfillByDefault()
        {
            var script = new TestScript(new MsieJavaScriptEngine());

            var result = script.RunTest(@"
return [{ a: 1 }, { a: 2 }, { a: 3 }].filter(function (l) { return l.a >= 2; }).length;
            ");
            Assert.Equal("2", result);
        }

        [Fact]
        public void RunThrowsInvalidProgramExceptionOnBadInputFile()
        {
            var ex = Assert.Throws<ScriptException>(() => new MsieJavaScriptEngine().Execute(@"function ) { return 'Missing Opening ('; }"));

            Assert.Contains("Expected '('", ex.Message);
        }
    }
}
