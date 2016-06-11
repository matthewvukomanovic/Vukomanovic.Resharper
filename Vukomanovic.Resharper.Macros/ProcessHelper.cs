using System;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.Util;

namespace Vukomanovic.Resharper.Macros
{
    internal class ProcessHelper
    {
        public static string FirstToLower(string existing)
        {
            return ChangeFirstCharacter(existing, CharEx.ToLowerFast);
        }

        public static string FirstToUpper(string existing)
        {
            return ChangeFirstCharacter(existing, CharEx.ToUpperFast);
        }

        public static string ChangeFirstCharacter(string existing, Func<char, char> function)
        {
            if (existing.Length > 0)
            {
                var start = function(existing[0]);

                if (existing.Length > 1)
                {
                    existing = start + existing.Substring(1);
                }
                else
                {
                    existing = start.ToString();
                }
            }
            return existing;
        }

        public static string GetRealValueOrNull(IMacroParameterValueNew argument)
        {
            var value = argument.GetValue();
            return value.IsEmpty() ? null : value;
        }

        public static Transform GetTransformValueFromArgument(IMacroParameterValueNew arg)
        {
            var transformValue = Transform.None;
            string value = null;
            if (arg != null)
            {
                value = arg.GetValue();
            }

            if (!String.IsNullOrEmpty(value))
            {
                value = value.Trim();

                switch (value)
                {
                    case "l":
                        transformValue = Transform.FirstLower;
                        break;
                    case "u":
                        transformValue = Transform.FirstUpper;
                        break;
                    case "L":
                        transformValue = Transform.AllLower;
                        break;
                    case "U":
                        transformValue = Transform.AllUpper;
                        break;
                }
            }
            return transformValue;
        }

        public static string ProcessVariable(IMacroParameterValueNew variableParameter, Transform transform)
        {
            var value = variableParameter.GetValue();
            return ProcessValue(transform, value);
        }

        public static string ProcessValue(Transform transform, string value)
        {
            switch (transform)
            {
                case Transform.None:
                    return value;
                case Transform.FirstLower:
                    return ProcessHelper.FirstToLower(value);
                case Transform.FirstUpper:
                    return ProcessHelper.FirstToUpper(value);
                case Transform.AllLower:
                    return value.ToLowerInvariant();
                case Transform.AllUpper:
                    return value.ToUpperInvariant();
            }
            return value;
        }
    }
}