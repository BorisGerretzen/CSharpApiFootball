using ApiFootball.Clients.Interface;
using ApiFootball.Models.Responses;
using Newtonsoft.Json;

namespace ApiFootball.Clients.Implementation;

public class TimezoneClient : BaseClient, ITimezoneClient {
    public TimezoneClient(IHttpClientFactory factory) : base(factory) { }
    protected override string Route => "timezone";

    /// <summary>
    ///     Get the list of available timezones to be used in the fixtures endpoint.
    /// </summary>
    /// <exception cref="NullReferenceException">If unable to deserialize response</exception>
    public async Task<BaseResponse<string>> GetTimezones() {
        var queryString = BuildQueryString();
        var response = await HttpClient.GetStringAsync(queryString);
        var responseObject = JsonConvert.DeserializeObject<BaseResponse<string>>(response, SerializerSettings);
        if (responseObject is null) throw new NullReferenceException("Could not deserialize response.");
        return responseObject;
    }
}