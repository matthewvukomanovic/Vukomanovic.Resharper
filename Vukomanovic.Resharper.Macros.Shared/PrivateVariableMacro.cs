using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Vukomanovic.Resharper.Macros
{
    [MacroDefinition("privateVariable.Macro",
        LongDescription = "Value of a variable prefixed with `_` and first letter made lower case",
        ShortDescription = "Value of {#0:other variable} prefixed with `_` and first letter made lower case"
        )]
    public class PrivateVariableMacroDefinition : SimpleMacroDefinition
    {
        public override ParameterInfo[] Parameters
        {
          get
          {
              return new[] { new ParameterInfo(ParameterType.VariableReference) };
          }
        }
    }

    [MacroImplementation(Definition = typeof(PrivateVariableMacroDefinition))]
    public class PrivateVariableMacroImplementation : SimpleMacroImplementation
    {
        private readonly IMacroParameterValueNew _variableParameter;

        public PrivateVariableMacroImplementation([Optional]MacroParameterValueCollection parameters)
        {
            _variableParameter = parameters.OptionalFirstOrDefault();
        }

        public override string EvaluateQuickResult(IHotspotContext context)
        {
            if (_variableParameter == null) return null;
            
            var existing = _variableParameter.GetValue();
            existing = ProcessHelper.FirstToLower(existing);
            return "_" + existing;
        }
    }
}

