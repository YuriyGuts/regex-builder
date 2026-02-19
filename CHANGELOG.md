# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.2] - 2026-02-19

No public API changes. Library continues to target `netstandard2.0`.

### Added

- Static code analysis (+ fixed the current warnings).
- `Directory.Build.props` for shared build configuration.
- `global.json` with MSTest SDK pin and `Microsoft.Testing.Platform` runner config.

### Changed

- Updated test and demo projects to target .NET 8 and .NET 10 LTS (replacing EOL .NET 6).
- Migrated tests to MSTest SDK 4.1.0 and updated the test syntax.

## [1.0.1] - 2026-02-19

### Changed

- Migrated to SDK-style project format targeting `netstandard2.0` (previously .NET Framework).

## [1.0.0] - 2014-01-06

### Added

- Initial release of RegexBuilder.
- Support for quantifiers, character escapes, character classes, anchors, grouping constructs, backreference constructs, alternation constructs, inline options, and comments.
- NuGet package.

[1.0.2]: https://github.com/YuriyGuts/regex-builder/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/YuriyGuts/regex-builder/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/YuriyGuts/regex-builder/releases/tag/v1.0.0
