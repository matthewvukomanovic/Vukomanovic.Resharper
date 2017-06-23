using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Vukomanovic.Resharper.Macros.Test
{
    public class MacroTests
    {
        private readonly ITestOutputHelper _output;
        public MacroTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("_preSomething_post", "l", "something")]
        [InlineData("_preSomething_post", "u", "Something")]
        [InlineData("_preSomething_post", "L", "something")]
        [InlineData("_preSomething_post", "U", "SOMETHING")]
        [InlineData("_preSomething_post", "", "Something")]
        [InlineData("O", "", "O")]
        [InlineData("_post", "", "")]
        [InlineData("_pre", "", "")]
        [InlineData("_pre_post", "", "")]
        [InlineData("_pRE_poSt", "", "")]
        [InlineData("_pre_pre", "", "_pre")]
        [InlineData("_post_post", "", "_post")]
        [InlineData("_prepost", "", "post")]
        [InlineData("pre_post", "", "pre")]
        [InlineData("_pre_pre", "", "_pre")]
        [InlineData("_ppre_post", "", "_ppre")]
        [InlineData("_pre_postp", "", "_postp")]
        [InlineData("_ppre_pre_postp", "", "_ppre_pre_postp")]
        public void RemovePreSuffixVariable(string initial, string transform, string expected)
        {
            var c = new MacroParameterValueCollection
                {
                    new DelegateMacroParameter(() => initial),
                    new ConstantMacroParameterValue("_pre"),
                    new ConstantMacroParameterValue("_post"),
                    new DelegateMacroParameter(() => transform),
                };


            IMacroImplementation i = new RemovePreSufFixVariableMacroImplementation(c);
            var value = i.EvaluateQuickResult(null);

            Xunit.Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData("_lock_", "rp:_;rs:_;u;d:Lock;ap:got", "gotLock")]
        [InlineData("_lock_", "rp:_;rs:_;u;d:Lock;ap:got", "gotLock")]
        [InlineData("_secondaryLock_", "rp:_;rs:_;u;d:Lock;ap:got", "gotSecondaryLock")]
        public void GeneralFunctionVariable(string initial, string transform, string expected)
        {

            var c = new MacroParameterValueCollection
                {
                    new DelegateMacroParameter(()=>initial),
                    new DelegateMacroParameter(()=>transform),
                };

            IMacroImplementation i = new GeneralFunctionVariableMacroImplementation(c);
            var value = i.EvaluateQuickResult(null);
            //_output.WriteLine("Initial: {0}\r\nProcessed: {1}", initial, value);
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData("Hello", "_hello")]
        [InlineData("HelloBirdy", "_helloBirdy")]
        [InlineData("Hello_Birdy", "_hello_Birdy")]
        [InlineData("Hello_birdy", "_hello_birdy")]
        [InlineData("_Hello_birdy", "__Hello_birdy")]
        public void PrivateVariableTest(string initial, string expected)
        {

            var c = new MacroParameterValueCollection
                {
                    new DelegateMacroParameter(()=>initial),
                };

            IMacroImplementation i = new PrivateVariableMacroImplementation(c);
            var value = i.EvaluateQuickResult(null);
            //_output.WriteLine("Initial: {0}\r\nProcessed: {1}", initial, value);
            Assert.Equal(expected, value);
        }
    }
}

