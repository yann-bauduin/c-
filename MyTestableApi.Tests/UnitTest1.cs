namespace MyTestableApi.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using System.Net;
using MyTestableApi.Api; 
using System.Text;       


public class UnitTest1
{
    [Fact]
    public async Task IsGetWeatherForcecastOK()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("WeatherForecast");
        string stringResponse = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    
    [Fact]
    public async Task GetCountryByName()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("Countries");
        string stringResponse = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    // erreur 404
    public async Task GetCountryByName_NotFound()
    {
        
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        var response = await client.GetAsync("api/Countries/NonExistentCountry");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    // erreur 500
    public async Task TriggerServerError_ReturnsInternalServerError()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        var response = await client.GetAsync("api/Countries/test-error");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    // pour l'add d'un pays
    [Fact]
    public async Task AddCountry_ReturnsCreatedAtAction_WithNewCountry()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        var newCountry = new Country { Name = "Espagne", Area = 505990.0 };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newCountry), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/Countries", content);
        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }


    // update d'un pays
    [Fact]
    public async Task UpdateCountry_ReturnsNoContent_WhenUpdated()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        var updatedCountry = new Country { Area = 600000.0 }; 
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedCountry), Encoding.UTF8, "application/json");
        var response = await client.PutAsync("api/Countries/France", content);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


    // delete d'un pays
    [Fact]
    public async Task DeleteCountry_ReturnsNoContent_WhenDeleted()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        var response = await client.DeleteAsync("api/Countries/Belgique");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    // voir tous les vins
    [Fact]
    public async Task GetAllWines_ReturnsAllWines()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        var response = await client.GetAsync("api/Wines");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var wines = System.Text.Json.JsonSerializer.Deserialize<List<Wine>>(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(wines);
        Assert.True(wines?.Count > 0); 
    }


    // recherche par nom
    [Fact]
    public async Task GetWineByName_ReturnsWine()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("api/Wines/Bordeaux");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var wine = System.Text.Json.JsonSerializer.Deserialize<Wine>(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(wine); 
        Assert.Equal("Bordeaux", wine?.Name);
    }

    // ajout d'un vin
    [Fact]
    public async Task AddWine_ReturnsCreatedAtAction_WithNewWine()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        
        var newWine = new Wine { Name = "Nouveau Vin", Region = "Nouvelle Région", Type = "Rouge" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newWine), Encoding.UTF8, "application/json");


        var response = await client.PostAsync("api/Wines", content);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }


    // update d'un vin
    [Fact]
    public async Task UpdateWine_ReturnsNoContent_WhenUpdated()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        
        var updatedWine = new Wine { Region = "Mise à jour Région", Type = "Blanc" }; 
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedWine), Encoding.UTF8, "application/json");

        var response = await client.PutAsync("api/Wines/NomDuVin", content);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


    //delete d'un vin
    [Fact]
    public async Task DeleteWine_ReturnsNoContent_WhenDeleted()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();
        var response = await client.DeleteAsync("api/Wines/NomDuVin");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }




}   