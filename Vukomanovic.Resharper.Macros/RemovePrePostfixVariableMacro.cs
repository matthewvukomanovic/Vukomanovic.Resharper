using System;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Vukomanovic.Resharper.Macros
{
    [MacroDefinition("removePreAndPostFixVariable.Macro",
        LongDescription = "Value of a variable removing user defined prefix and affix, use l|u to make the variable's first character lower|upper and L|U to make the entire thing lower|upper",
        ShortDescription = "Value of {#0:another variable} with prefix `{#1:prefix}` removed and affix `{#2:append}` removed and transformed with `{#3:transform}`"
        )]
    public class RemovePrePostfixVariableMacroDefinition : SimpleMacroDefinition
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

    [MacroImplementation(Definition = typeof(RemovePrePostfixVariableMacroDefinition))]
    public class RemovePrePostfixVariableMacroImplementation : SimpleMacroImplementation
    {
        private readonly IMacroParameterValueNew _variableParameter;
        private readonly string _prefix;
        private readonly string _postfix;
        private readonly Transform _transform;

        public RemovePrePostfixVariableMacroImplementation([Optional] MacroParameterValueCollection arguments)
        {
            if (arguments == null || arguments.Count != 4)
            {
                return;
            }

            _variableParameter = arguments[0];
            _prefix = ProcessHelper.GetRealValueOrNull(arguments[1]);
            _postfix = ProcessHelper.GetRealValueOrNull(arguments[2]);
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

            if (_prefix != null && _prefix.Length <= value.Length)
            {
                if (value.StartsWith(_prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (_prefix.Length < value.Length)
                    {
                        value = value.Substring(_prefix.Length);    
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            if (_postfix != null && _postfix.Length <= value.Length)
            {
                if (value.EndsWith(_postfix, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (_postfix.Length < value.Length)
                    {
                        value = value.Substring(0, value.Length - _postfix.Length);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            return ProcessHelper.ProcessValue(_transform, value);
        }
    }
}