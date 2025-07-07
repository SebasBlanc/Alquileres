using AlquileresSistemaPrincipal.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Index()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync("https://localhost:7118/api/Propiedades"); 

        if (!response.IsSuccessStatusCode)
        {
            // Puedes retornar una vista de error personalizada
            return View(new List<PropiedadDto>());
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var propiedades = JsonConvert.DeserializeObject<List<PropiedadDto>>(jsonData);

        return View(propiedades); 
    }

    // Mostrar formulario
    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }
    // Recibir datos del formulario
    [HttpPost]
    public async Task<IActionResult> Crear(PropiedadDto propiedad)
    {
        if (!ModelState.IsValid)
        {
            return View(propiedad);
        }

        var httpClient = _httpClientFactory.CreateClient();
        var json = JsonConvert.SerializeObject(propiedad);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://localhost:7118/api/Propiedades", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        // Si falla, volver a mostrar el formulario
        ModelState.AddModelError(string.Empty, "Error al crear la propiedad");
        return View(propiedad);
    }

    public async Task<IActionResult> Detalles(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7118/api/Propiedades/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var propiedad = JsonConvert.DeserializeObject<PropiedadDto>(json);

        // Llamada a la API del clima
        var clima = await ObtenerClimaActual(propiedad.Direccion);

        ViewBag.Clima = clima;

        return View(propiedad);
    }

    private async Task<ClimaDto> ObtenerClimaActual(string ciudad)
    {
        string apiKey = "fbfe37c41642d87caf70f39bd88cfb92"; 
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={apiKey}&lang=es";

        using var client = new HttpClient();
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        dynamic climaInfo = JsonConvert.DeserializeObject(json);

        return new ClimaDto
        {
            Main = climaInfo.weather[0].main,
            Description = climaInfo.weather[0].description,
            Temperature = (float)climaInfo.main.temp
        };
    }







}
