using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Application.Interfaces.Services.Data;
using DiveSpecies.Infrastructure.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DiveSpecies.Infrastructure.Services.Data;
public class MediaService : IMediaService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MediaService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<string>> UploadMedia(IFormFileCollection formFiles)
    {
        if(!formFiles.Any()) return new List<string>();

        var httpClient = _httpClientFactory.CreateClient(HttpClientUtil.HTTP_CLIENT_MEDIA_CONFIG);
        var formData = new MultipartFormDataContent();

        foreach (var formFile in formFiles)
        {
            formData.Add(CreateFileContent(formFile), "file", formFile.FileName);
        }

        var response = await httpClient.PostAsync("/api/uploads", formData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(result)!;
        }

        return new List<string>();
    }

    private ByteArrayContent CreateFileContent(IFormFile file)
    {
        var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);

        var fileContent = new ByteArrayContent(memoryStream.ToArray());
        
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        return fileContent;
    }

}
