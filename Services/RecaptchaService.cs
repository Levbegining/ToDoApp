using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ToDoApp.Models;

namespace ToDoApp.Services;

public class RecaptchaService
{
    private ReCaptchaSettings settings;
    IHttpClientFactory httpClientFactory;

    public RecaptchaService(IOptions<ReCaptchaSettings> settings,IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
        this.settings = settings.Value;
    }
    public async Task<bool> VerifyAsync(string token)
    {
        //using var httpClient = new HttpClient(); // fixed
        var httpClient = httpClientFactory.CreateClient();

        var response = await httpClient.PostAsync(
            $"https://www.google.com/recaptcha/api/siteverify?secret={settings.SecretKey}&response={token}",
            null);

        if(!response.IsSuccessStatusCode)
        {
            return false;
        }

        var jsonString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString);
        

        return result != null && result.Success;
    }
}
class ReCaptchaResponse{
    public bool Success{get;set;}
}
