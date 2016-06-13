using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using System.Diagnostics;

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

            IMacroImplementation i = new RemovePreSufFixVariableMacroImplementation(c);
            var value = i.EvaluateQuickResult(null);
            transform = "u"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            transform = "L"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            transform = "U"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            transform = "O"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "O"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_post"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pre"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pre_post"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pRE_poSt"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_pre_pre"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_post_post"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);
            initial = "_prepost"; i = new RemovePreSufFixVariableMacroImplementation(c);
            value = i.EvaluateQuickResult(null);

            c = new MacroParameterValueCollection
            {
                new DelegateMacroParameter(()=>initial),
                new DelegateMacroParameter(()=>transform),
            };

            transform = "rp:_;rs:_;u;d:Lock;ap:got";
            i = new GeneralTransformPreSufFixVariableMacroImplementation(c);
            initial = "_lock_";
            value = i.EvaluateQuickResult(null); Debug.WriteLine("Initial: {0}\r\nProcessed: {1}", initial, value);
            initial = "_";
            value = i.EvaluateQuickResult(null); Debug.WriteLine("Initial: {0}\r\nProcessed: {1}", initial, value);
            initial = "_secondaryLock_";
            value = i.EvaluateQuickResult(null); Debug.WriteLine("Initial: {0}\r\nProcessed: {1}", initial, value);
        }
    }
}
