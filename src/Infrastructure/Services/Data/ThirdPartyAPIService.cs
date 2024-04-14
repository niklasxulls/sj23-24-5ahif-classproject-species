using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Application.Interfaces.Services.Data;
using DiveSpecies.Infrastructure.Util;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using DiveSpecies.Infrastructure.Models;

namespace DiveSpecies.Infrastructure.Services.Data;

public class ThirdPartyAPIService : IThirdPartyAPIService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ThirdPartyAPIService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<RapidApiFishSchema>?> RequestAllFishData()
    {
        var httpClient = _httpClientFactory.CreateClient(HttpClientUtil.HTTP_CLIENT_RAPIDAPI_CONFIG);
        var httpResponseMessage = await httpClient.GetAsync("/fish_api/fishes");

        if(httpResponseMessage.IsSuccessStatusCode)
        {
            using var content = await httpResponseMessage.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<RapidApiFishSchema>>(content);
            
            return result;
        }

        return null;
    }
}
