# Vukomanovic.Resharper.Macros
I wanted to make templates with private variable a bit easier to make so included a macro to do that, then made two extra ones for other common stuff that I need.

ReSharper Gallery Link: https://resharper-plugins.jetbrains.com/packages/Vukomanovic.Resharper.Macros
 
## Macros
* Value of another variable with prefix `_` added and first character made lower case;
* Value of another variable transformed with a prefix added a suffix added; and
* Value of another variable with a prefix removed a suffix removed and remaining text transformed.
* Value of another variable with multiple functions applied.

### All the transforms are defined as:
* `l` for the first character being made lowercase;
* `u` for the first character being made uppercase;
* `L` for all the characters being made lowercase (in the original variable, not the prefix or suffix);
* `U` for all the characters being made uppercase (in the original variable, not the prefix or suffix); and
* Anything else for no transform.

### Value of another variable with multiple functions applied
This macro allows chaining different functions together for more flexibilty

The second parameter needs to be separated by `;`'s and the functions should be followed by `:`'s

The functions are:
* `ap` - Add prefix
* `as` - Add suffix
* `rp` - Remove prefix
* `rs` - Remove suffix
* `t` - Perform case transform (see above for transforms)
* `d` - Replace an empty string with the defined default
* `tx` or `x` where `x` is any of the transforms by themselves with or without a `:`

_Example: if you had `_lockSuf` as your variable input you could run with the function parameter as `rp:_;rs:Suf;u;d:Lock;ap:got;as:YAY` which would give you the output `gotLockYAY`_

Note that any function which you define which wouldn't do anything is skipped so that it is quicker.
_Example: if you again had `_lockSuf` as your variable input and had `ap:;as:;rp;rs:;t;d:` as the function parameter. Then the macro compiles this down to just returning the original value._



## Example of usage
I have include a file [instanceExample.DotSettings](instanceExample.DotSettings) which creates a thread safe Instance property in a class and also a Lock object for that Instance.
The good thing about having the entire variable encapsulated in the resharper variable is that if you change it, it can have a nice flow on effect through the rest of the tempalte as well.
