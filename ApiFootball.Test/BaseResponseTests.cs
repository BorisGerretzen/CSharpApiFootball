using ApiFootball.Models;
using ApiFootball.Models.Responses;

namespace ApiFootball.Test;

public class BaseResponseTests
{
    [Test]
    public void IsSuccess_WhenNoErrors_True()
    {
        var response = new BaseResponse<string>
        {
            Get = "test",
            Parameters = new Dictionary<string, string>(),
            Errors = new Dictionary<string, string>(),
            Results = 0,
            Response = new List<string>(),
            Paging = new Paging()
        };

        Assert.That(response.IsSuccess, Is.True);
    }

    [Test]
    public void IsSuccess_WhenErrors_False()
    {
        var response = new BaseResponse<string>
        {
            Get = "test",
            Parameters = new Dictionary<string, string>(),
            Errors = new Dictionary<string, string> { { "error", "test" } },
            Results = 0,
            Response = new List<string>(),
            Paging = new Paging()
        };

        Assert.That(response.IsSuccess, Is.False);
    }

    [Test]
    public void EnsureSuccess_WhenNoErrors_DoesNotThrow()
    {
        var response = new BaseResponse<string>
        {
            Get = "test",
            Parameters = new Dictionary<string, string>(),
            Errors = new Dictionary<string, string>(),
            Results = 0,
            Response = new List<string>(),
            Paging = new Paging()
        };

        Assert.That(() => response.EnsureSuccess(), Throws.Nothing);
    }

    [Test]
    public void EnsureSuccess_WhenErrors_ThrowsApiFootballException()
    {
        const string message = "MESSAGE";
        const string code = "CODE";

        var response = new BaseResponse<string>
        {
            Get = "test",
            Parameters = new Dictionary<string, string>(),
            Errors = new Dictionary<string, string> { [code] = message },
            Results = 0,
            Response = new List<string>(),
            Paging = new Paging()
        };

        var ex = Assert.Throws<ApiFootballException>(() => response.EnsureSuccess());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.Not.Empty);
            Assert.That(ex.Message, Does.Contain(code));
            Assert.That(ex.Message, Does.Contain(message));
            Assert.That(ex.Errors, Is.Not.Null);
            Assert.That(ex.Errors, Has.Count.EqualTo(1));
            Assert.That(ex.Errors[0].Code, Is.EqualTo(code));
            Assert.That(ex.Errors[0].Message, Is.EqualTo(message));
        }
    }
}