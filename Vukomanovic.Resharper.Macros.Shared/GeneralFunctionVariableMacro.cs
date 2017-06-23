using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Vukomanovic.Resharper.Macros
{
    [MacroDefinition("generalFunctionVariable.Macro",
        LongDescription = "Value of a variable with any number of functions applied where the functions are separated by `;`'s and operations are separated from their values by `:`'s. - ap:add prefix; - as:add suffix; - rp:remove prefix; - rs:remove suffix; -t:transform; -d:default",
        ShortDescription = "Value of {#0:another variable} with multiple functions applied  `{#1:functions}`"
        )]
    public class GeneralFunctionVariableMacroDefinition : SimpleMacroDefinition
    {
        public override ParameterInfo[] Parameters
        {
            get
            {
                return new[]
                {
                    new ParameterInfo(ParameterType.VariableReference),
                    new ParameterInfo(ParameterType.String),
                };
            }
        }
    }

    [MacroImplementation(Definition = typeof(GeneralFunctionVariableMacroDefinition))]
    public class GeneralFunctionVariableMacroImplementation : SimpleMacroImplementation
    {
        private readonly IMacroParameterValueNew _variableParameter;
        private readonly Func<string, string> _functionToRun;

        public GeneralFunctionVariableMacroImplementation([Optional] MacroParameterValueCollection arguments)
        {
            if (arguments == null || arguments.Count != 2)
            {
                _functionToRun = NoTransform;
                return;
            }

            _variableParameter = arguments[0];

            _functionToRun = CreateFunctionBasedOn(ProcessHelper.GetRealValueOrNull(arguments[1]));
        }

        private interface IValueTransform
        {
            string Transform(string value);
        }

        private static string NoTransform(string value)
        {
            return value;
        }

        private class MultipleTransform : IValueTransform
        {
            private readonly Func<string, string>[] _transforms;
            public MultipleTransform(List<Func<string, string>> transforms)
            {
                _transforms = transforms.ToArray();
            }

            public string Transform(string value)
            {
                foreach (var transform in _transforms)
                {
                    value = transform(value);
                }
                return value;
            }

            public static Func<string, string> GetTransform(List<Func<string, string>> transforms)
            {
                MultipleTransform transform = new MultipleTransform(transforms);
                return transform.Transform;
            }
        }

        private class AddPrefixTransform : IValueTransform
        {
            private readonly string _prefix;

            public AddPrefixTransform(string prefix)
            {
                if (string.IsNullOrEmpty(prefix))
                {
                    throw new ArgumentNullException("prefix", "Shouldn't be able to get a null or empty prefix in here");
                }
                _prefix = prefix;
            }

            public string Transform(string value)
            {
                return _prefix + value;
            }

            public static Func<string, string> GetTransform(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return new AddPrefixTransform(value).Transform;
            }
        }

        private class AddSuffixTransform : IValueTransform
        {
            private readonly string _suffix;

            public AddSuffixTransform(string suffix)
            {
                if (string.IsNullOrEmpty(suffix))
                {
                    throw new ArgumentNullException("suffix", "Shouldn't be able to get a null or empty suffix in here");
                }
                _suffix = suffix;
            }

            public string Transform(string value)
            {
                return _suffix + value;
            }

            public static Func<string, string> GetTransform(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return new AddSuffixTransform(value).Transform;
            }
        }

        private class RemovePrefixTransform : IValueTransform
        {
            private readonly string _prefix;

            public RemovePrefixTransform(string prefix)
            {
                if (string.IsNullOrEmpty(prefix))
                {
                    throw new ArgumentNullException("prefix", "Shouldn't be able to get a null or empty prefix in here");
                }
                _prefix = prefix;
            }

            public string Transform(string value)
            {
                return ProcessHelper.RemovePrefix(value, _prefix);
            }

            public static Func<string, string> GetTransform(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return new RemovePrefixTransform(value).Transform;
            }
        }

        private class RemoveSuffixTransform : IValueTransform
        {
            private readonly string _suffix;

            public RemoveSuffixTransform(string suffix)
            {
                if (string.IsNullOrEmpty(suffix))
                {
                    throw new ArgumentNullException("suffix", "Shouldn't be able to get a null or empty suffix in here");
                }
                _suffix = suffix;
            }

            public string Transform(string value)
            {
                return ProcessHelper.RemoveSuffix(value, _suffix);
            }

            public static Func<string, string> GetTransform(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return new RemoveSuffixTransform(value).Transform;
            }
        }

        private class DefaultTransform : IValueTransform
        {
            private readonly string _default;

            public DefaultTransform(string defaultValue)
            {
                if (string.IsNullOrEmpty(defaultValue))
                {
                    throw new ArgumentNullException("defaultValue", "Shouldn't be able to get a null or empty defaultValue in here");
                }
                _default = defaultValue;
            }

            public string Transform(string value)
            {
                return string.IsNullOrEmpty(value) ? _default : value;
            }

            public static Func<string, string> GetTransform(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return new DefaultTransform(value).Transform;
            }
        }

        private static Func<string,string> CreateFunctionBasedOn(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return NoTransform;
            }
            var splitParameters = parameter.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            if (splitParameters.Length == 0)
            {
                return NoTransform;
            }

            var allFuncs = new List<Func<string, string>>(splitParameters.Length);

            foreach (var potentialParameter in splitParameters)
            {
                var function = CreateIndividualTransform(potentialParameter);
                if (function != null)
                {
                    allFuncs.Add(function);
                }
            }

            if (allFuncs.Count == 0)
            {
                return NoTransform;
            }
            else if( allFuncs.Count == 1)
            {
                return allFuncs[0];
            }
            return MultipleTransform.GetTransform(allFuncs);
        }

        private static Func<string, string> CreateIndividualTransform(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return null;
            }

            var firstColonIndex = parameter.IndexOf(':');
            string possibleFunction;
            if (firstColonIndex == -1)
            {
                possibleFunction = parameter;
                parameter = null;
            }
            else
            {
                possibleFunction = parameter.Substring(0, firstColonIndex);
                parameter = parameter.Length > firstColonIndex + 1 ? parameter.Substring(firstColonIndex + 1) : null;
            }

            possibleFunction = possibleFunction.Trim();
            switch (possibleFunction.ToLowerInvariant())
            {
                case "ap":
                    return AddPrefixTransform.GetTransform(parameter);
                case "as":
                    return AddSuffixTransform.GetTransform(parameter);
                case "rp":
                    return RemovePrefixTransform.GetTransform(parameter);
                case "rs":
                    return RemoveSuffixTransform.GetTransform(parameter);
                case "t":
                    return ProcessHelper.GetTransformFuncFromValue(parameter);
                case "d":
                    return DefaultTransform.GetTransform(parameter);
                default:
                    switch (possibleFunction)
                    {
                        case "tl":
                        case "Tl":
                        case "l":
                            return ProcessHelper.GetTransformFuncFromTransform(Transform.FirstLower);
                        case "tL":
                        case "TL":
                        case "L":
                            return ProcessHelper.GetTransformFuncFromTransform(Transform.AllLower);
                        case "tu":
                        case "Tu":
                        case "u":
                            return ProcessHelper.GetTransformFuncFromTransform(Transform.FirstUpper);
                        case "tU":
                        case "TU":
                        case "U":
                            return ProcessHelper.GetTransformFuncFromTransform(Transform.AllUpper);
                    }
                    break;
            }
            return null;
        }

        public override string EvaluateQuickResult(IHotspotContext context)
        {
            if (_variableParameter == null) return null;

            var value = _variableParameter.GetValue();

            return _functionToRun(value);
        }
    }
}