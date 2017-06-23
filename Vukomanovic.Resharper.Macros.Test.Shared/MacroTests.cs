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
        [InlineData("Hello", "_", "_", "l", "_hello_")]
        [InlineData("HelloBirdy", "_", "_", "l", "_helloBirdy_")]
        [InlineData("HelloBirdy", "_", "", "l", "_helloBirdy")]
        [InlineData("hello", "_", "_", "l", "_hello_")]
        [InlineData("helloBirdy", "_", "_", "l", "_helloBirdy_")]
        [InlineData("helloBirdy", "_", "", "l", "_helloBirdy")]
        [InlineData("Hello", "_", "_", "u", "_Hello_")]
        [InlineData("HelloBirdy", "_", "_", "u", "_HelloBirdy_")]
        [InlineData("HelloBirdy", "_", "", "u", "_HelloBirdy")]
        [InlineData("hello", "Pre", "_", "u", "PreHello_")]
        [InlineData("helloBirdy", "Pre", "_", "u", "PreHelloBirdy_")]
        [InlineData("helloBirdy", "Pre", "", "u", "PreHelloBirdy")]
        public void AddPreSuffixVariable(string initial, string prefix, string suffix, string transform, string expected)
        {
            var c = new MacroParameterValueCollection
            {
                new DelegateMacroParameter(() => initial),
                new ConstantMacroParameterValue(prefix),
                new ConstantMacroParameterValue(suffix),
                new DelegateMacroParameter(() => transform),
            };


            IMacroImplementation i = new AddPreSufFixVariableMacroImplementation(c);
            var value = i.EvaluateQuickResult(null);

            Xunit.Assert.Equal(expected, value);
        }


        [Theory]
        [InlineData("_preSomething_post", "_pre", "_post", "l", "something")]
        [InlineData("_preSomething_post", "_pre", "_post", "u", "Something")]
        [InlineData("_preSomething_post", "_pre", "_post", "L", "something")]
        [InlineData("_preSomething_post", "_pre", "_post", "U", "SOMETHING")]
        [InlineData("_preSomething_post", "_pre", "_post", "", "Something")]
        [InlineData("O", "_pre", "_post", "", "O")]
        [InlineData("_post", "_pre", "_post", "", "")]
        [InlineData("_pre", "_pre", "_post", "", "")]
        [InlineData("_pre_post", "_pre", "_post", "", "")]
        [InlineData("_pRE_poSt", "_pre", "_post", "", "")]
        [InlineData("_pre_pre", "_pre", "_post", "", "_pre")]
        [InlineData("_post_post", "_pre", "_post", "", "_post")]
        [InlineData("_prepost", "_pre", "_post", "", "post")]
        [InlineData("pre_post", "_pre", "_post", "", "pre")]
        [InlineData("_pre_pre", "_pre", "_post", "", "_pre")]
        [InlineData("_ppre_post", "_pre", "_post", "", "_ppre")]
        [InlineData("_pre_postp", "_pre", "_post", "", "_postp")]
        [InlineData("_ppre_pre_postp", "_pre", "_post", "", "_ppre_pre_postp")]
        public void RemovePreSuffixVariable(string initial, string pre, string post, string transform, string expected)
        {
            var c = new MacroParameterValueCollection
                {
                    new DelegateMacroParameter(() => initial),
                    new ConstantMacroParameterValue(pre),
                    new ConstantMacroParameterValue(post),
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

