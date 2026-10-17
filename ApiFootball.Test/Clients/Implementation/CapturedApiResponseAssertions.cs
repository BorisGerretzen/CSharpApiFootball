using System.Text.Json;
using ApiFootball.Models;
using ApiFootball.Models.Responses;

namespace ApiFootball.Test.Clients.Implementation;

internal static class CapturedApiResponseAssertions
{
    public static void AssertPage<T>(PagedResponse<T> response, string route, int expectedPages = 1)
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(route));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Results, Is.EqualTo(response.Response.Count));
            Assert.That(response.Paging.Current, Is.EqualTo(1));
            Assert.That(response.Paging.Total, Is.EqualTo(expectedPages));
        });
    }

    public static void AssertParameter(BaseResponse response, string key, string value, int expectedParameterCount = 1)
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.Parameters, Has.Count.EqualTo(expectedParameterCount));
            Assert.That(response.Parameters, Contains.Key(key).WithValue(value));
        });
    }

    public static void AssertMinuteCounts(
        Dictionary<string, TeamStatisticsCardCount>? actual,
        params (string Period, int? Total, string? Percentage)[] expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Has.Count.EqualTo(expected.Length));
        if (actual is null) return;
        foreach (var (period, total, percentage) in expected)
        {
            Assert.That(actual, Contains.Key(period), $"Missing minute interval {period}");
            Assert.That(actual[period].Total, Is.EqualTo(total), $"Unexpected total for {period}");
            Assert.That(actual[period].Percentage, Is.EqualTo(percentage), $"Unexpected percentage for {period}");
        }
    }

    public static void AssertUnderOver(
        Dictionary<string, JsonElement>? actual,
        params (string Threshold, int Over, int Under)[] expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Has.Count.EqualTo(expected.Length));
        if (actual is null) return;
        foreach (var (threshold, over, under) in expected)
        {
            Assert.That(actual, Contains.Key(threshold), $"Missing under/over threshold {threshold}");
            Assert.That(actual[threshold].ValueKind, Is.EqualTo(JsonValueKind.Object));
            Assert.That(actual[threshold].GetProperty("over").GetInt32(), Is.EqualTo(over),
                $"Unexpected over value for {threshold}");
            Assert.That(actual[threshold].GetProperty("under").GetInt32(), Is.EqualTo(under),
                $"Unexpected under value for {threshold}");
        }
    }
}