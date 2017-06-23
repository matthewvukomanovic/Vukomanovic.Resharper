using System;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Vukomanovic.Resharper.Macros
{
    [MacroDefinition("removePreAndPostFixVariable.Macro",
        LongDescription = "Value of a variable removing user defined prefix and suffix, use l|u to make the variable's first character lower|upper and L|U to make the entire thing lower|upper",
        ShortDescription = "Value of {#0:another variable} with prefix `{#1:prefix}` removed and affix `{#2:append}` removed and transformed with `{#3:transform}`"
        )]
    public class RemovePreSufFixVariableMacroDefinition : SimpleMacroDefinition
    {
        public override ParameterInfo[] Parameters
        {
            get
            {
                return new[]
                {
                    new ParameterInfo(ParameterType.VariableReference),
                    new ParameterInfo(ParameterType.String),
                    new ParameterInfo(ParameterType.String),
                    new ParameterInfo(ParameterType.String),
                };
            }
        }
    }

    [MacroImplementation(Definition = typeof(RemovePreSufFixVariableMacroDefinition))]
    public class RemovePreSufFixVariableMacroImplementation : SimpleMacroImplementation
    {
        private readonly IMacroParameterValueNew _variableParameter;
        private readonly string _prefix;
        private readonly string _suffix;
        private readonly Transform _transform;

        public RemovePreSufFixVariableMacroImplementation([Optional] MacroParameterValueCollection arguments)
        {
            if (arguments == null || arguments.Count != 4)
            {
                return;
            }

            _variableParameter = arguments[0];
            _prefix = ProcessHelper.GetRealValueOrNull(arguments[1]);
            _suffix = ProcessHelper.GetRealValueOrNull(arguments[2]);
            _transform = ProcessHelper.GetTransformValueFromArgument(arguments[3]);
        }


        public override string EvaluateQuickResult(IHotspotContext context)
        {
            if (_variableParameter == null) return null;

            var value = _variableParameter.GetValue();

            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = ProcessHelper.RemovePrefix(value, _prefix);
            value = ProcessHelper.RemoveSuffix(value, _suffix);

            return ProcessHelper.ProcessValue(_transform, value);
        }
    }
}