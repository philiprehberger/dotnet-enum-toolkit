# Philiprehberger.EnumToolkit

[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.EnumToolkit)](https://www.nuget.org/packages/Philiprehberger.EnumToolkit)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![CI](https://github.com/philiprehberger/dotnet-enum-toolkit/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-enum-toolkit/actions/workflows/ci.yml)

Enum utilities — parse with fallback, get display names and descriptions, convert to dictionaries, and list all values.

## Installation

```bash
dotnet add package Philiprehberger.EnumToolkit
```

## Usage

### Parse with Fallback

```csharp
using Philiprehberger.EnumToolkit;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public enum Status
{
    [Display(Name = "Not Started")]
    [Description("The task has not been started yet.")]
    NotStarted,

    [Display(Name = "In Progress")]
    [Description("The task is currently being worked on.")]
    InProgress,

    [Display(Name = "Completed")]
    [Description("The task has been finished.")]
    Completed
}

var status = EnumKit.ParseOr("inprogress", Status.NotStarted);
// status == Status.InProgress

var unknown = EnumKit.ParseOr("invalid", Status.NotStarted);
// unknown == Status.NotStarted

if (EnumKit.TryParse<Status>("completed", out var parsed))
{
    // parsed == Status.Completed
}
```

### Display Names and Descriptions

```csharp
using Philiprehberger.EnumToolkit;

var displayName = EnumKit.GetDisplayName(Status.InProgress);
// "In Progress"

var description = EnumKit.GetDescription(Status.InProgress);
// "The task is currently being worked on."

var isDefined = EnumKit.IsDefined(Status.Completed);
// true
```

### List and Convert

```csharp
using Philiprehberger.EnumToolkit;

var allValues = EnumKit.GetValues<Status>();
// [Status.NotStarted, Status.InProgress, Status.Completed]

var dictionary = EnumKit.ToDictionary<Status>();
// { 0: "Not Started", 1: "In Progress", 2: "Completed" }

var infoList = EnumKit.GetInfo<Status>();
// [
//   EnumInfo { Value = NotStarted, Name = "NotStarted", DisplayName = "Not Started", Description = "The task has not been started yet." },
//   EnumInfo { Value = InProgress, Name = "InProgress", DisplayName = "In Progress", Description = "The task is currently being worked on." },
//   EnumInfo { Value = Completed, Name = "Completed", DisplayName = "Completed", Description = "The task has been finished." }
// ]
```

## API

| Method | Return Type | Description |
|--------|-------------|-------------|
| `EnumKit.GetValues<T>()` | `IReadOnlyList<T>` | Returns all defined values of the enum type. |
| `EnumKit.GetDisplayName<T>(T value)` | `string` | Gets the `[Display(Name)]` attribute value, or the member name. |
| `EnumKit.GetDescription<T>(T value)` | `string?` | Gets the `[Description]` attribute value, or `null`. |
| `EnumKit.ParseOr<T>(string value, T fallback)` | `T` | Case-insensitive parse with a fallback value. |
| `EnumKit.TryParse<T>(string value, out T result)` | `bool` | Case-insensitive parse returning success/failure. |
| `EnumKit.ToDictionary<T>()` | `IReadOnlyDictionary<int, string>` | Maps integer values to display names. |
| `EnumKit.GetInfo<T>()` | `IReadOnlyList<EnumInfo<T>>` | Returns metadata for all enum values. |
| `EnumKit.IsDefined<T>(T value)` | `bool` | Checks whether the value is defined in the enum. |

## Development

```bash
dotnet build src/Philiprehberger.EnumToolkit.csproj --configuration Release
```

## License

MIT
