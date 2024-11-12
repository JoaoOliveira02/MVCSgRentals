using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace GerenciadorDeEmpresaWEB.Services;

public class PerfilUsuarioServices : IPerfilUsuarioServices
{
    private const string apiEndpoint = "/api/PerfilUsuario/";

    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _clientFactory;

    private PerfilUsuarioViewModel perfilUsuarioVM;
    private IEnumerable<PerfilUsuarioViewModel> perfilUsuariosVM;

    public PerfilUsuarioServices(IHttpClientFactory clientFactory)
    {
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _clientFactory = clientFactory;
    }

    public async Task<IEnumerable<PerfilUsuarioViewModel>> GetPerfilUsuario()
    {
        var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
        var endpoint = apiEndpoint + "GetAllTipoPerfilusuario";  // Ajuste aqui para usar o endpoint correto

        try
        {
            using (var response = await client.GetAsync(endpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    perfilUsuariosVM = await JsonSerializer
                                   .DeserializeAsync<IEnumerable<PerfilUsuarioViewModel>>
                                   (apiResponse, _options);
                }
                else
                {
                    Console.WriteLine($"Erro ao chamar a API: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            return null;
        }

        return perfilUsuariosVM;
    }
    public async Task<PerfilUsuarioViewModel> GetPerfilUsuarioPorId(int id)
    {
        var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
        var endpoint = apiEndpoint + "GetPerfilUsuario/" + id;  // Ajuste aqui para usar o endpoint correto

        using (var response = await client.GetAsync(endpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                perfilUsuarioVM = await JsonSerializer
                               .DeserializeAsync<PerfilUsuarioViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }

        return perfilUsuarioVM;
    }

    public async Task<PerfilUsuarioViewModel> CriarPerfilUsuario(PerfilUsuarioViewModel perfilUsuario)
    {
        var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
        var perfilUsuarioJson = JsonSerializer.Serialize(perfilUsuario);
        StringContent content = new StringContent(perfilUsuarioJson, Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint + "AddPerfilUsuario", content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PerfilUsuarioViewModel>(apiResponse, _options);
            }
        }

        return null;
    }

    public async Task<bool> AtualizarPerfilUsuario(int id, PerfilUsuarioViewModel perfilUsuario)
    {
        var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
        var perfilUsuarioJson = JsonSerializer.Serialize(perfilUsuario);
        StringContent content = new StringContent(perfilUsuarioJson, Encoding.UTF8, "application/json");

        using (var response = await client.PutAsync(apiEndpoint + "UpdatePerfilUsuario/" + id, content))
        {
            return response.IsSuccessStatusCode;
        }
    }

    public async Task<bool> DeletarPerfilUsuario(int id)
    {
        var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");

        using (var response = await client.DeleteAsync(apiEndpoint + "DeletePerfilUsuario/" + id))
        {
            return response.IsSuccessStatusCode;
        }
    }


}
