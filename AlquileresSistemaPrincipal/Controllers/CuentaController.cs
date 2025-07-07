// Controllers/CuentaController.cs
using AlquileresSistemaPrincipal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

public class CuentaController : Controller
{
    private readonly HttpClient _httpClient;

    public CuentaController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7118/"); 
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var response = await _httpClient.PostAsJsonAsync("api/usuarios/login", model);

        if (response.IsSuccessStatusCode)
        {
            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
       
            TempData["Usuario"] = usuario.Nombre;
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
        return View(model);
    }
}

