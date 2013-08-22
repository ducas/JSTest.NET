﻿using System;
using System.IO;
using JSTest.ScriptElements;
using Xunit;

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

namespace JSTest.Test.MsieEngine
{
    public class WhenUsingTestScript
    {
        [Fact]
        public void AlwaysAppendBlockToExistingScript()
        {
            var block1 = new ScriptBlock("var x = 0;");
            var block2 = new ScriptBlock("var y = 1;");
            var block3 = new ScriptBlock("var z = 2;");
            var script = new TestScript(new MsieJavaScriptEngine());

            script.AppendBlock(block1);
            script.AppendBlock(block2);
            script.AppendBlock(block3);

            Assert.Equal(block1.ToInlineScriptFragment() + Environment.NewLine + block2.ToInlineScriptFragment() + Environment.NewLine + block3.ToInlineScriptFragment() + Environment.NewLine, script.ToString());
        }

        [Fact]
        public void AlwaysAppendIncludeToExistingScript()
        {
            using (var tempFile1 = new TempFile())
            using (var tempFile2 = new TempFile())
            using (var tempFile3 = new TempFile())
            {
                File.WriteAllText(tempFile1.FileName, "var x = 1;");
                File.WriteAllText(tempFile2.FileName, "var y = 2;");
                File.WriteAllText(tempFile3.FileName, "var z = 3;");
                var script = new TestScript(new MsieJavaScriptEngine());

                script.AppendFile(tempFile1.FileName);
                script.AppendFile(tempFile2.FileName);
                script.AppendFile(tempFile3.FileName);

                Assert.Equal(
@"var x = 1;
var y = 2;
var z = 3;
",
                script.ToString());
            }
        }

        [Fact]
        public void RunTestReturnsNullWhenNoReturnSpecified()
        {
            var script = new TestScript(new MsieJavaScriptEngine());

            Assert.Equal("null", script.RunTest("var result = 'Success!';"));
        }

        [Fact]
        public void RunTestReturnsValueWhenExplicitReturnSpecified()
        {
            var script = new TestScript(new MsieJavaScriptEngine());

            Assert.Equal("\"Success!\"", script.RunTest("return 'Success!';"));
        }

        [Fact]
        public void RunTestThrowsScriptExceptionBackToCaller()
        {
            var script = new TestScript(new MsieJavaScriptEngine());

            var ex = Assert.Throws<ScriptException>(() => script.RunTest("throw { message: 'My Script Exception' };"));

            Assert.Equal(ex.Message, "{\"message\":\"My Script Exception\"}");
        }
    }
}
