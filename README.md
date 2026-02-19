# RegexBuilder

[![NuGet](https://img.shields.io/nuget/v/RegexBuilder.svg)](https://www.nuget.org/packages/RegexBuilder/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

Just another day at the office, you write a .NET Regex like a boss,
and suddenly realize that you need to declare, say, a non-capturing group.
It's `(?:pattern)`, right? Wait, or was it `(?=pattern)`? No no, `(?=pattern)`
must be a positive lookahead or something. But if `(?<=pattern)` is a positive lookbehind,
then maybe positive lookahead would be `(?>=pattern)`?

_"Aaargh! Now where's that Regex cheat sheet?.."_ And make sure to share it with your five colleagues who might be maintaining this code later.
Also, remember to use comments inside the Regex pattern, and maybe a few third-party tools to be sure what the expression does.

_"How did it come to this?"_

Inspired by the [Expression Trees](https://learn.microsoft.com/dotnet/csharp/advanced-topics/expression-trees/) feature
in .NET, the RegexBuilder library provides a more verbose but more human-readable way of declaring regular
expressions, using a language friendly to the .NET world instead of two lines of cryptic mess.

When it might be useful:

* When the expressions are complex and might be frequently changed.
* When you can tolerate 20 lines of understandable code instead of 1 hardly understandable.
* If you can spare a bit of CPU time and memory for constructing the Regex object for the sake of readability.

## Compatibility

The library targets `netstandard2.0`, so it works with .NET Framework 4.6.1+, .NET Core 2.0+, and .NET 5+.
Tests are verified against .NET 8 and .NET 10 LTS.

## Example

Let's say you want to [make a simple HTML parser](http://stackoverflow.com/questions/1732348/regex-match-open-tags-except-xhtml-self-contained-tags)
and capture the value of every `href` attribute from hyperlinks, like shown in the [MSDN example](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-examples).

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

RegexBuilder currently supports all regular expression language elements except [substitution/replacement patterns](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#substitutions).

The following elements are supported:

* [Quantifiers](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#quantifiers)
* [Character escapes](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#character-escapes)
* [Character classes](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#character-classes)
* [Anchors (atomic zero-width assertions)](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#anchors)
* [Grouping constructs](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#grouping-constructs)
* [Backreference constructs](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#backreference-constructs)
* [Alternation constructs](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#alternation-constructs)
* [Inline options and comments](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference#miscellaneous-constructs)

## Installation

Install via the .NET CLI:

```shell
dotnet add package RegexBuilder
```

Or use the NuGet Package Manager Console:

```powershell
PM> Install-Package RegexBuilder
```

## Usage Guide

There are 3 classes you'll need. They all expose their functionality via static members and work statelessly.

1. `RegexBuilder`: a factory class that produces and glues together different parts of a regular expression.
2. `RegexQuantifier`: produces quantifiers (`?`, `+` `{4,}`, etc.) for regex parts that support them.
3. `RegexMetaChars`: named constants for character classes (word boundary, whitespace, tab, etc.).

Start with `var regex = RegexBuilder.Build(...);` and replace `...` with the parts of your regular expression
by calling the corresponding methods of `RegexBuilder`.

## Development

Build the solution:

```shell
dotnet build src/YuriyGuts.RegexBuilder.sln
```

Run the tests (runs on both `net8.0` and `net10.0`):

```shell
dotnet test --project src/YuriyGuts.RegexBuilder.Tests
```

Run the console test app:

```shell
dotnet run --project src/YuriyGuts.RegexBuilder.TestApp
```

The project has `GeneratePackageOnBuild=true`, so a `.nupkg` file is produced on every build.

## License

The source code is licensed under [The MIT License](LICENSE).
