using Xunit;
namespace Philiprehberger.EnumToolkit.Tests;

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public class EnumInfoTests
{
    [Fact]
    public void EnumInfo_StoresAllProperties()
    {
        var info = new EnumInfo<Season>(Season.Spring, "Spring", "Spring Season", "The spring season");

        Assert.Equal(Season.Spring, info.Value);
        Assert.Equal("Spring", info.Name);
        Assert.Equal("Spring Season", info.DisplayName);
        Assert.Equal("The spring season", info.Description);
    }

    [Fact]
    public void EnumInfo_NullDescription_IsAllowed()
    {
        var info = new EnumInfo<Season>(Season.Winter, "Winter", "Winter", null);

        Assert.Null(info.Description);
    }

    [Fact]
    public void EnumInfo_EqualityByValue_Works()
    {
        var info1 = new EnumInfo<Season>(Season.Summer, "Summer", "Summer", null);
        var info2 = new EnumInfo<Season>(Season.Summer, "Summer", "Summer", null);

        Assert.Equal(info1, info2);
    }

    [Fact]
    public void EnumInfo_DifferentValues_AreNotEqual()
    {
        var info1 = new EnumInfo<Season>(Season.Spring, "Spring", "Spring", null);
        var info2 = new EnumInfo<Season>(Season.Autumn, "Autumn", "Autumn", null);

        Assert.NotEqual(info1, info2);
    }
}
