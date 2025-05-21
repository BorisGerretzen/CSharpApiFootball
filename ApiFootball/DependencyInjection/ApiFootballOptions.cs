namespace ApiFootball;

public class ApiFootballOptions
{
    /// <summary>
    ///     Your api-football api key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    ///     Base url of api-football, use the trailing slash.
    /// </summary>
    public string ApiUrl { get; set; } = ApiFootballGlobals.ApiUrl;

    /// <summary>
    ///     Whether to throw exceptions on errors.
    /// </summary>
    public bool ThrowOnError { get; set; } = false;
}