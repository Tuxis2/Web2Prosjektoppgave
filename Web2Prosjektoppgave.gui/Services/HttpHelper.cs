using System.Net.Http.Headers;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;
using Web2Prosjektoppgave.shared.ViewModels.Blog;

namespace Web2Prosjektoppgave.gui.Services;

public class HttpHelper
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IJSRuntime _jsRuntime;

    public HttpHelper(IHttpClientFactory clientFactory, IJSRuntime jsRuntime)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public async Task<ResponseEmpty> PostAsync<T>(string uri, T formObject)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Add("Accept", "application/vnd.github.v3+json");
        request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
        request.Content = new StringContent(JsonSerializer.Serialize(formObject), Encoding.UTF8, "application/json");

        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return new ResponseEmpty()
            {
                IsSuccess = true
            };
        }

        return new ResponseEmpty()
        {
            IsSuccess = false
        };
    }

    public async Task<ResponseEmpty> PutAsync<T>(string uri, T formObject)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri);
        request.Headers.Add("Accept", "application/vnd.github.v3+json");
        request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
        request.Content = new StringContent(JsonSerializer.Serialize(formObject), Encoding.UTF8, "application/json");

        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return new ResponseEmpty()
            {
                IsSuccess = true
            };
        }

        return new ResponseEmpty()
        {
            IsSuccess = false
        };
    }

    public async Task<Response<TReturn>> GetAsync<TReturn>(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Add("Accept", "application/vnd.github.v3+json");
        request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
        //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetJWT());

        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);

        TReturn? blogResult = default;
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            using var responseStream = await response.Content.ReadAsStreamAsync();
            blogResult = await JsonSerializer.DeserializeAsync
                <TReturn>(responseStream, options);

            return new Response<TReturn>()
            {
                IsSuccess = true,
                Result = blogResult
            };
        }

        return new Response<TReturn>()
        {
            IsSuccess = false,
            Result = blogResult
        };
    }


    // Helpers
    private async Task<string> GetJWT()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken")
            .ConfigureAwait(false);
    }

    public class ResponseEmpty
    {
        public bool IsSuccess { get; set; }
    }
    
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T? Result { get; set; }
    }

}