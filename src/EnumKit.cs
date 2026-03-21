using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Philiprehberger.EnumToolkit;

/// <summary>
/// Provides utility methods for working with enum types, including parsing, attribute reading,
/// and conversion to dictionaries and metadata lists.
/// </summary>
public static class EnumKit
{
    /// <summary>
    /// Returns all defined values of the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>A read-only list of all enum values.</returns>
    public static IReadOnlyList<T> GetValues<T>() where T : struct, Enum
    {
        return Array.AsReadOnly(Enum.GetValues<T>());
    }

    /// <summary>
    /// Gets the display name for an enum value. Reads the <see cref="DisplayAttribute.Name"/> property
    /// if present; otherwise falls back to the enum member name.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The display name or the enum member name.</returns>
    public static string GetDisplayName<T>(T value) where T : struct, Enum
    {
        var field = typeof(T).GetField(value.ToString());

        if (field is null)
        {
            return value.ToString();
        }

        var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? value.ToString();
    }

    /// <summary>
    /// Gets the description for an enum value by reading the <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The description string, or <c>null</c> if no <see cref="DescriptionAttribute"/> is present.</returns>
    public static string? GetDescription<T>(T value) where T : struct, Enum
    {
        var field = typeof(T).GetField(value.ToString());

        if (field is null)
        {
            return null;
        }

        var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute?.Description;
    }

    /// <summary>
    /// Parses a string to the specified enum type using case-insensitive matching.
    /// Returns the provided fallback value if parsing fails.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The string to parse.</param>
    /// <param name="fallback">The fallback value to return if parsing fails.</param>
    /// <returns>The parsed enum value, or <paramref name="fallback"/> if parsing fails.</returns>
    public static T ParseOr<T>(string value, T fallback) where T : struct, Enum
    {
        return Enum.TryParse<T>(value, ignoreCase: true, out var result) ? result : fallback;
    }

    /// <summary>
    /// Attempts to parse a string to the specified enum type using case-insensitive matching.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">When this method returns, contains the parsed enum value if successful.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryParse<T>(string value, out T result) where T : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase: true, out result);
    }

    /// <summary>
    /// Converts all values of the specified enum type to a dictionary mapping each integer value
    /// to its display name.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>A read-only dictionary mapping integer values to display names.</returns>
    public static IReadOnlyDictionary<int, string> ToDictionary<T>() where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        var dictionary = new Dictionary<int, string>(values.Length);

        foreach (var value in values)
        {
            var intValue = Convert.ToInt32(value);
            dictionary[intValue] = GetDisplayName(value);
        }

        return new ReadOnlyDictionary<int, string>(dictionary);
    }

    /// <summary>
    /// Returns metadata for all values of the specified enum type, including name, display name,
    /// and description.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>A read-only list of <see cref="EnumInfo{T}"/> records for each enum value.</returns>
    public static IReadOnlyList<EnumInfo<T>> GetInfo<T>() where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        var infoList = new List<EnumInfo<T>>(values.Length);

        foreach (var value in values)
        {
            infoList.Add(new EnumInfo<T>(
                Value: value,
                Name: value.ToString(),
                DisplayName: GetDisplayName(value),
                Description: GetDescription(value)
            ));
        }

        return infoList.AsReadOnly();
    }

    /// <summary>
    /// Determines whether the specified value is defined in the enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the value is defined; otherwise, <c>false</c>.</returns>
    public static bool IsDefined<T>(T value) where T : struct, Enum
    {
        return Enum.IsDefined(value);
    }
}
