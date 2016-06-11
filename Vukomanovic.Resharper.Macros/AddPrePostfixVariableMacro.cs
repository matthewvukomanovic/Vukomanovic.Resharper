using System.Runtime.InteropServices;
using System.Text;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Vukomanovic.Resharper.Macros
{
    [MacroDefinition("addPreAndPostFixVariable.Macro",
        LongDescription = "Value of a variable prefixed with a user defined string and appended with another user defined string, use l|u to make the variable's first character lower|upper and L|U to make the entire thing lower|upper",
        ShortDescription = "Value of {#0:another variable} prefixed with `{#1:prefix}` appended with `{#2:append}` and transformed with `{#3:transform}`"
        )]
    public class AddPrePostfixVariableMacroDefinition : SimpleMacroDefinition
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

    [MacroImplementation(Definition = typeof(AddPrePostfixVariableMacroDefinition))]
    public class AddPrePostfixVariableMacroImplementation : SimpleMacroImplementation
    {
        private readonly IMacroParameterValueNew _variableParameter;
        private readonly string _prefix;
        private readonly string _postfix;
        private readonly Transform _transform;

        public AddPrePostfixVariableMacroImplementation([Optional] MacroParameterValueCollection arguments)
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

            var value = ProcessHelper.ProcessVariable(_variableParameter, _transform);
            var builder = new StringBuilder();
            if (_prefix != null)
            {
                builder.Append(_prefix);
            }
            builder.Append(value);
            if (_postfix != null)
            {
                builder.Append(_postfix);
            }
            return builder.ToString();
        }
    }
}