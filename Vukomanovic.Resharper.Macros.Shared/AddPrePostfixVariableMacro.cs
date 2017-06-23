using System.Runtime.InteropServices;
using System.Text;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Vukomanovic.Resharper.Macros
{
    [MacroDefinition("addPreAndPostFixVariable.Macro",
        LongDescription = "Value of a variable adding a prefix and suffix from user defined strings, use l|u to make the variable's first character lower|upper and L|U to make the entire thing lower|upper",
        ShortDescription = "Value of {#0:another variable} adding prefix `{#1:prefix}` and suffix `{#2:append}` and transformed with `{#3:transform}`"
        )]
    public class AddPreSufFixVariableMacroDefinition : SimpleMacroDefinition
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

    [MacroImplementation(Definition = typeof(AddPreSufFixVariableMacroDefinition))]
    public class AddPreSufFixVariableMacroImplementation : SimpleMacroImplementation
    {
        private readonly IMacroParameterValueNew _variableParameter;
        private readonly string _prefix;
        private readonly string _suffix;
        private readonly Transform _transform;

        public AddPreSufFixVariableMacroImplementation([Optional] MacroParameterValueCollection arguments)
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

            var value = ProcessHelper.ProcessVariable(_variableParameter, _transform);
            var builder = new StringBuilder();
            if (_prefix != null)
            {
                builder.Append(_prefix);
            }
            builder.Append(value);
            if (_suffix != null)
            {
                builder.Append(_suffix);
            }
            return builder.ToString();
        }
    }
}