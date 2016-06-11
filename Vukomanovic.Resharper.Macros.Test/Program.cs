using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Vukomanovic.Resharper.Macros.Test
{
    class Program
    {

        static void Main(string[] args)
        {
            var initial = "_preSomething_post";
            var transform = "l";
            var c = new MacroParameterValueCollection
            {
                new DelegateMacroParameter(()=>initial),
                new ConstantMacroParameterValue("_pre"),
                new ConstantMacroParameterValue("_post"),
                new DelegateMacroParameter(()=>transform),
            };


            var i = new RemovePrePostfixVariableMacroImplementation(c);
            var value = i.EvaluateQuickResult(null);
            transform = "u"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            transform = "L"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            transform = "U"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            transform = "O"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "O"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_post"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pre"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pre_post"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pRE_poSt"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pre_pre"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_post_post"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_prepost"; i = new RemovePrePostfixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
        }
    }
}
