using System.Net;
using System.Net.Http.Json;
using Turbohesap.Shared.Common;

namespace Turbohesap.Web.Services;

/// <summary>API'den dönen hata. <see cref="ProblemDetail"/> gövdesinden okunan mesajı taşır.</summary>
public sealed class ApiException(int statusCode, string detail) : Exception(detail)
{
    public int StatusCode { get; } = statusCode;
    public bool IsUnauthorized => StatusCode == (int)HttpStatusCode.Unauthorized;
}

/// <summary>
/// HTTP servis fabrikası + token ekleme mekanizması (req 27). Her istekte adlandırılmış
/// "TurbohesapApi" istemcisi üretilir ve geçerli oturumun JWT'si Authorization başlığına
/// eklenir. Hatalı yanıtlar <see cref="ApiException"/>'a dönüştürülür ({"detail":...}).
/// </summary>
public sealed class ApiClient(IHttpClientFactory factory, TokenStore tokenStore)
{
    public const string ClientName = "TurbohesapApi";

    private HttpClient CreateClient()
    {
        var client = factory.CreateClient(ClientName);
        var session = tokenStore.Session;
        if (session is not null && !session.IsExpired)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.AccessToken);
        }
        return client;
    }

    public async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        var response = await CreateClient().GetAsync(url, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    public async Task<TResponse?> PostAsync<TResponse>(string url, object body, CancellationToken cancellationToken = default)
    {
        var response = await CreateClient().PostAsJsonAsync(url, body, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
    }

    public async Task<TResponse?> PutAsync<TResponse>(string url, object body, CancellationToken cancellationToken = default)
    {
        var response = await CreateClient().PutAsJsonAsync(url, body, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
    }

    public async Task DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        var response = await CreateClient().DeleteAsync(url, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        string detail = "İstek tamamlanamadı.";
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetail>(cancellationToken);
            if (!string.IsNullOrWhiteSpace(problem?.Detail))
            {
                detail = problem.Detail;
            }
        }
        catch
        {
            // Gövde ProblemDetail değilse varsayılan mesaj kullanılır.
        }

        throw new ApiException((int)response.StatusCode, detail);
    }
}
