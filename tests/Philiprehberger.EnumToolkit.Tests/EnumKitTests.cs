using Xunit;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Philiprehberger.EnumToolkit.Tests;

public enum Color
{
    [Display(Name = "Red Color")]
    [Description("The color red")]
    Red,

    [Display(Name = "Green Color")]
    Green,

    Blue
}

public enum Priority
{
    Low = 0,
    Medium = 1,
    High = 2
}

public class EnumKitTests
{
    [Fact]
    public void GetValues_ReturnsAllEnumValues()
    {
        var values = EnumKit.GetValues<Color>();

        Assert.Equal(3, values.Count);
        Assert.Contains(Color.Red, values);
        Assert.Contains(Color.Green, values);
        Assert.Contains(Color.Blue, values);
    }

    [Fact]
    public void GetDisplayName_WithDisplayAttribute_ReturnsDisplayName()
    {
        var result = EnumKit.GetDisplayName(Color.Red);

        Assert.Equal("Red Color", result);
    }

    [Fact]
    public void GetDisplayName_WithoutDisplayAttribute_ReturnsMemberName()
    {
        var result = EnumKit.GetDisplayName(Color.Blue);

        Assert.Equal("Blue", result);
    }

    [Fact]
    public void GetDescription_WithDescriptionAttribute_ReturnsDescription()
    {
        var result = EnumKit.GetDescription(Color.Red);

        Assert.Equal("The color red", result);
    }

    [Fact]
    public void GetDescription_WithoutDescriptionAttribute_ReturnsNull()
    {
        var result = EnumKit.GetDescription(Color.Blue);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("Red", Color.Red)]
    [InlineData("red", Color.Red)]
    [InlineData("RED", Color.Red)]
    [InlineData("Green", Color.Green)]
    public void ParseOr_ValidValue_ReturnsParsedEnum(string input, Color expected)
    {
        var result = EnumKit.ParseOr(input, Color.Blue);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseOr_InvalidValue_ReturnsFallback()
    {
        var result = EnumKit.ParseOr("Invalid", Color.Blue);

        Assert.Equal(Color.Blue, result);
    }

    [Fact]
    public void TryParse_ValidValue_ReturnsTrueAndSetsResult()
    {
        var success = EnumKit.TryParse<Color>("red", out var result);

        Assert.True(success);
        Assert.Equal(Color.Red, result);
    }

    [Fact]
    public void TryParse_InvalidValue_ReturnsFalse()
    {
        var success = EnumKit.TryParse<Color>("invalid", out _);

        Assert.False(success);
    }

    [Fact]
    public void ToDictionary_ReturnsIntToDisplayNameMapping()
    {
        var dict = EnumKit.ToDictionary<Priority>();

        Assert.Equal(3, dict.Count);
        Assert.Equal("Low", dict[0]);
        Assert.Equal("Medium", dict[1]);
        Assert.Equal("High", dict[2]);
    }

    [Fact]
    public void ToDictionary_WithDisplayAttributes_UsesDisplayNames()
    {
        var dict = EnumKit.ToDictionary<Color>();

        Assert.Equal("Red Color", dict[0]);
        Assert.Equal("Green Color", dict[1]);
        Assert.Equal("Blue", dict[2]);
    }

    [Fact]
    public void GetInfo_ReturnsMetadataForAllValues()
    {
        var info = EnumKit.GetInfo<Color>();

        Assert.Equal(3, info.Count);

        var redInfo = info.First(i => i.Value.Equals(Color.Red));
        Assert.Equal("Red", redInfo.Name);
        Assert.Equal("Red Color", redInfo.DisplayName);
        Assert.Equal("The color red", redInfo.Description);

        var blueInfo = info.First(i => i.Value.Equals(Color.Blue));
        Assert.Equal("Blue", blueInfo.Name);
        Assert.Equal("Blue", blueInfo.DisplayName);
        Assert.Null(blueInfo.Description);
    }

    [Theory]
    [InlineData(Color.Red, true)]
    [InlineData(Color.Green, true)]
    [InlineData((Color)99, false)]
    public void IsDefined_ReturnsExpectedResult(Color value, bool expected)
    {
        var result = EnumKit.IsDefined(value);

        Assert.Equal(expected, result);
    }
}
