﻿using System;
using JSTest.ScriptLibraries;
using NUnit.Framework;

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

namespace JSTest.Examples.Nunit.Style3
{
    [TestFixture]
    public class WhenSettingCookies : JavaScriptTestBase
    {
        [SetUp]
        public void Setup()
        {
            Script = new TestScript { IncludeDefaultBreakpoint = false };

            // Append required JavaScript libraries.
            Script.AppendBlock(new JsAssertLibrary());

            // Append required JavaScript Files.
            Script.AppendFile(@"..\..\dateExtensions.js");
            Script.AppendFile(@"..\..\cookieContainer.js");
            Script.AppendFile(@"..\..\whenSettingCookies.js");

            // Setup JavaScript Context
            Script.AppendBlock(@"
                                 var document = {};
                                 var cookieContainer = new CookieContainer(document);
                               ");
        }

        [Test]
        public void CookieDocumentSet()
        {
            var result = RunTest(@"
                                   cookieContainer.setCookie('MyCookie', 'Chocolate Chip');

                                   return document.cookie;
                                 ");

            Assert.AreEqual("MyCookie=Chocolate%20Chip;path=/", result);
        }

        [Test]
        public void CookieExpirySetIfDaysSpecified()
        {
            const String dateFormat = "ddd, d MMM yyyy HH:mm:ss UTC";

            var now = DateTime.UtcNow;
            var result = RunTest(String.Format(@"
                                                 var now = new Date('{0}');

                                                 cookieContainer.setCookie('MyCookie', 'Chocolate Chip', 1, now);

                                                 return document.cookie;
                                               ", now.ToString(dateFormat)));

            Assert.AreEqual(String.Format("MyCookie=Chocolate%20Chip;expires={0};path=/", now.AddDays(1).ToString(dateFormat)), result);
        }
    }
}
