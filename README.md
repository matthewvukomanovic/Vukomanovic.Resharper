# Vukomanovic.Resharper.Macros
I wanted to make templates with private variable a bit easier to make so included a macro to do that, then made two extra ones for other common stuff that I need.

ReSharper Gallery Link: https://resharper-plugins.jetbrains.com/packages/Vukomanovic.Resharper.Macros
 
## Macros
* Value of another variable with prefix `_` added and first character made lower case;
* Value of another variable transformed with a prefix added a suffix added; and
* Value of another variable with a prefix removed a suffix removed and remaining text transformed.

All the transforms are defined as:
* `l` for the first character being made lowercase;
* `u` for the first character being made uppercase;
* `L` for all the characters being made lowercase (in the original variable, not the prefix or suffix);
* `U` for all the characters being made uppercase (in the original variable, not the prefix or suffix); and
* Anything else for no transform.

## Example of usage
I have include a file [instanceExample.DotSettings](instanceExample.DotSettings) which creates a thread safe Instance property in a class and also a Lock object for that Instance.
The good thing about having the entire variable encapsulated in the resharper variable is that if you change it, it can have a nice flow on effect through the rest of the tempalte as well.
