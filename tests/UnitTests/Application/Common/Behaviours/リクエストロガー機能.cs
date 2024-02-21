using Creationline.Mirage.Application.Common.Behaviours;
using Creationline.Mirage.Application.Common.Interfaces;
using Creationline.Mirage.Application.TodoItems.Commands;
using Microsoft.Extensions.Logging;
using Moq;

namespace Creationline.Mirage.UnitTests.Application.Common.Behaviours;

public class リクエストロガー機能
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;


    public リクエストロガー機能()
    {
        _logger = new Mock<ILogger<CreateTodoItemCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Fact]
    public async Task 認証済みの場合GetUserNameAsyncが１度だけ実行される()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task 認証されていない場合GetUserNameAsyncは実行されない()
    {
        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
