using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using AlquileresSistemaPrincipal.Models;

public class PropiedadesController : Controller
{
    private readonly HttpClient _httpClient;

    public PropiedadesController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("https://localhost:7118/api/propiedades");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var propiedades = JsonConvert.DeserializeObject<List<PropiedadDto>>(json);
            return View("~/Views/Home/Index.cshtml", propiedades);
        }
        else
        {
            return View(new List<PropiedadDto>());
        }
    }


    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7118/");

            var response = await client.DeleteAsync($"api/Propiedades/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Propiedad eliminada correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "Error al eliminar la propiedad.";
            }
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Error al conectar con la API: {ex.Message}";
        }

        return RedirectToAction("Index");
    }




}


