namespace Philiprehberger.EnumToolkit;

/// <summary>
/// Represents metadata for a single enum value, including its name, display name, and description.
/// </summary>
/// <typeparam name="T">The enum type.</typeparam>
/// <param name="Value">The enum value.</param>
/// <param name="Name">The raw enum member name.</param>
/// <param name="DisplayName">The display name from <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/>, or the member name if not specified.</param>
/// <param name="Description">The description from <see cref="System.ComponentModel.DescriptionAttribute"/>, or <c>null</c> if not specified.</param>
public record EnumInfo<T>(T Value, string Name, string DisplayName, string? Description) where T : struct, Enum;
