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

        public static string ToLower(string existing)
        {
            return existing == null ? null : existing.ToLowerInvariant();
        }

        public static string ToUpper(string existing)
        {
            return existing == null ? null : existing.ToUpperInvariant();
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
            string value = null;
            if (arg != null)
            {
                value = arg.GetValue();
            }

            return GetTransformValueFromValue(value);
        }

        public static Func<string, string> GetTransformFuncFromValue(string value)
        {
            return GetTransformFuncFromTransform(GetTransformValueFromValue(value));
        }

        public static Func<string, string> GetTransformFuncFromTransform(Transform transform)
        {
            switch (transform)
            {
                case Transform.None:
                    return null;
                case Transform.FirstLower:
                    return FirstToLower;
                case Transform.FirstUpper:
                    return FirstToUpper;
                case Transform.AllLower:
                    return ToLower;
                case Transform.AllUpper:
                    return ToUpper;
                default:
                    return null;
            }
        }

        public static Transform GetTransformValueFromValue(string value)
        {
            var transformValue = Transform.None;
            if (!string.IsNullOrEmpty(value))
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

        public static string RemoveSuffix(string value, string suffix)
        {
            if (suffix != null && value != null && suffix.Length <= value.Length)
            {
                if (value.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (suffix.Length < value.Length)
                    {
                        return value.Substring(0, value.Length - suffix.Length);
                    }
                    return string.Empty;
                }
            }
            return value;
        }

        public static string RemovePrefix(string value, string prefix)
        {
            if (prefix != null && value != null && prefix.Length <= value.Length)
            {
                if (value.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (prefix.Length < value.Length)
                    {
                        return value.Substring(prefix.Length);
                    }
                    return string.Empty;
                }
            }
            return value;
        }

        public static string AddPrefix(string value, string prefix)
        {
            if (prefix != null)
            {
                return prefix + value;
            }
            return value;
        }

        public static string AddSuffix(string value, string suffix)
        {
            if (suffix != null)
            {
                return value + suffix;
            }
            return value;
        }
    }
}