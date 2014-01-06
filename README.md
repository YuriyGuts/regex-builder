# RegexBuilder

Just another day at the office, you write a .NET Regex like a boss,
and suddenly realize that you need to declare, say, a non-capturing group.
It's `(?:pattern)`, right? Wait, or was it `(?=pattern)`? No no, `(?=pattern)`
must be a positive lookahead or something. But if `(?<=pattern)` is a positive lookbehind,
then maybe positive lookahead would be `(?>=pattern)`?

_"Aaargh! Now where's that Regex cheat sheet?.."_ And make sure to share it with your five colleagues who might be maintaining this code later.
Also, remember to use comments inside the Regex pattern, and maybe a few third-party tools to be sure what the expression does.

_"How did it come to this?"_

Inspired by the [Expression Trees](http://msdn.microsoft.com/en-us/library/bb397951.aspx) feature
in .NET, the RegexBuilder library provides a more verbose but more human-readable way of declaring regular
expressions, using a language friendly to the .NET world instead of two lines of cryptic mess.

When it might be useful:

* When the expressions are complex and might be frequently changed.
* When you can tolerate 20 lines of understandable code instead of 1 hardly understandable.
* If you can spare a bit of CPU time and memory for constructing the Regex object for the sake of readability.

## Example

Let's say you want to [make a simple HTML parser](http://stackoverflow.com/questions/1732348/regex-match-open-tags-except-xhtml-self-contained-tags)
and capture the value of every `href` attribute from hyperlinks, like shown in the [MSDN example](http://msdn.microsoft.com/en-us/library/t9e807fx.aspx).

The usual way:

```csharp
Regex hrefRegex = new Regex("href\\s*=\\s*(?:[\"'](?<Target>[^\"']*)[\"']|(?<Target>\\S+))", RegexOptions.IgnoreCase);
```

With RegexBuilder:

```csharp
const string quotationMark = "\"";
Regex hrefRegex = RegexBuilder.Build
(
    RegexOptions.IgnoreCase,
    // Regex structure declaration
    RegexBuilder.Literal("href"),
    RegexBuilder.MetaCharacter(RegexMetaChars.WhiteSpace, RegexQuantifier.ZeroOrMore),
    RegexBuilder.Literal("="),
    RegexBuilder.MetaCharacter(RegexMetaChars.WhiteSpace, RegexQuantifier.ZeroOrMore),
    RegexBuilder.Alternate
    (
        RegexBuilder.Concatenate
        (
            RegexBuilder.NonEscapedLiteral(quotationMark),
            RegexBuilder.Group
            (
                "Target",
                RegexBuilder.NegativeCharacterSet(quotationMark, RegexQuantifier.ZeroOrMore)
            ),
            RegexBuilder.NonEscapedLiteral(quotationMark)
        ),
        RegexBuilder.Group
        (
            "Target",
            RegexBuilder.MetaCharacter(RegexMetaChars.NonwhiteSpace, RegexQuantifier.OneOrMore)
        )
    )
);
```

See [CustomRegexTests.cs](/src/YuriyGuts.RegexBuilder.Tests/CustomRegexTests.cs) for more examples.

## Feature Support

RegexBuilder currently supports all regular expression language elements except [substitution/replacement patterns](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#substitutions).

The following elements are supported:

* [Quantifiers](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#quantifiers)
* [Character escapes](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#character_escapes)
* [Character classes](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#character_classes)
* [Anchors (atomic zero-width assertions)](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#atomic_zerowidth_assertions)
* [Grouping constructs](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#grouping_constructs)
* [Backreference constructs](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#backreference_constructs)
* [Alternation constructs](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#alternation_constructs)
* [Inline options and comments](http://msdn.microsoft.com/en-us/library/az24scfc.aspx#miscellaneous_constructs)

## How to Integrate RegexBuilder

Add a reference to YuriyGuts.RegexBuilder.dll in your project manually, or use NuGet Package Manager:
```
PM> Install-Package RegexBuilder 
```

## Usage Guide

There are 3 classes you'll need. They all expose their functionality via static members and work statelessly.

1. `RegexBuilder`: a factory class that produces and glues together different parts of a regular expression.
2. `RegexQuantifier`: produces quantifiers (`?`, `+` `{4,}`, etc.) for regex parts that support them.
3. `RegexMetaChars`: named constants for character classes (word boundary, whitespace, tab, etc.).

Start with `var regex = RegexBuilder.Build(...);` and replace `...` with the parts of your regular expression 
by calling the corresponding methods of `RegexBuilder`.

## Testing

RegexBuilder uses MSTest for unit testing. To run or add unit tests in Visual Studio, please see the `YuriyGuts.RegexBuilder.Tests` project.

The `YuriyGuts.RegexBuilder.TestApp` project is a console application that can be used as a temporary testing workbench.

## License

The source code is licensed under [The MIT License](http://opensource.org/licenses/MIT).
