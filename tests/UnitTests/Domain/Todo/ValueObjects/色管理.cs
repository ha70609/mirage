using FluentAssertions;
using Creationline.Mirage.Domain.Todo.ValueObjects;
using Creationline.Mirage.Domain.Todo.Exceptions;

namespace Creationline.Mirage.UnitTests.Domain;

public class 色管理
{
    [Theory]
    [InlineData("#FFFFFF")]
    [InlineData("#FF5733")]
    public void ColourコードからColourオブジェクトを取得できる(string code)
    {
        var colour = Colour.From(code);

        colour.Code.Should().Be(code);
    }

    [Fact]
    public void Colourコードを文字列に変換できる()
    {
        var colour = Colour.White;

        colour.ToString().Should().Be(colour.Code);
    }

    [Fact]
    public void Colourコードを文字列に暗黙の変換ができる()
    {
        string code = Colour.White;

        code.Should().Be("#FFFFFF");
    }

    [Fact]
    public void サポートされているColourコードが指定された場合明示的な変換ができる()
    {
        var colour = (Colour)"#FFFFFF";

        colour.Should().Be(Colour.White);
    }

    [Fact]
    public void サポートされていないColourコードが指定された場合UnsupportedColourExceptionが発生する()
    {
        FluentActions.Invoking(() => Colour.From("##FF33CC"))
            .Should().Throw<UnsupportedColourException>();
    }
}