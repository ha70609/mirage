using Creationline.Mirage.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;

namespace Creationline.Mirage.Application.UnitTests.Common.Exceptions;

public class 検証機能
{
    [Fact]
    public void デフォルトコンストラクタで空のエラーディクショナリを作成される()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Fact]
    public void 単一のバリデーションエラーが発生した場合単一の要素からなるエラーディクショナリが作成される()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Age", "must be over 18"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Age" });
        actual["Age"].Should().BeEquivalentTo(new string[] { "must be over 18" });
    }

    [Fact]
    public void 複数のプロパティに対する複数のバリデーションエラーが発生した場合にそれぞれのエラー辞書が複数の値を持つ()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Age", "must be 18 or older"),
                new ValidationFailure("Age", "must be 25 or younger"),
                new ValidationFailure("Password", "must contain at least 8 characters"),
                new ValidationFailure("Password", "must contain a digit"),
                new ValidationFailure("Password", "must contain upper case letter"),
                new ValidationFailure("Password", "must contain lower case letter"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Password", "Age" });

        actual["Age"].Should().BeEquivalentTo(new string[]
        {
                "must be 25 or younger",
                "must be 18 or older",
        });

        actual["Password"].Should().BeEquivalentTo(new string[]
        {
                "must contain lower case letter",
                "must contain upper case letter",
                "must contain at least 8 characters",
                "must contain a digit",
        });
    }
}
