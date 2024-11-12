using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace GerenciadorDeEmpresaWEB.Services
{
    public class EmpresasServices : IEmpresasServices
    {
        private const string apiEndpoint = "/api/Empresa/";

        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        private EmpresaViewModel empresaVM;
        private IEnumerable<EmpresaViewModel> empresasVM;

        public EmpresasServices(IHttpClientFactory clientFactory)
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<EmpresaViewModel>> GetEmpresas()
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetAllEmpresas";  // Ajuste aqui para usar o endpoint correto

            try
            {
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        empresasVM = await JsonSerializer
                                       .DeserializeAsync<IEnumerable<EmpresaViewModel>>
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

            return empresasVM;
        }



        public async Task<EmpresaViewModel> GetEmpresaPorId(int id)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetEmpresa/" + id;  // Ajuste aqui para usar o endpoint correto

            using (var response = await client.GetAsync(endpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    empresaVM = await JsonSerializer
                                   .DeserializeAsync<EmpresaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }

            return empresaVM;
        }

        public async Task<EmpresaViewModel> CriarEmpresa(EmpresaViewModel empresa)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var empresaJson = JsonSerializer.Serialize(empresa);
            StringContent content = new StringContent(empresaJson, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndpoint + "AddEmpresa", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<EmpresaViewModel>(apiResponse, _options);
                }
                else
                {
                    // Captura a resposta de erro da API
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro na chamada de API: {errorResponse}");
                }
            }
        }

         public async Task<bool> AtualizarEmpresa(int id, EmpresaViewModel empresa)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var empresaJson = JsonSerializer.Serialize(empresa);
            StringContent content = new StringContent(empresaJson, Encoding.UTF8, "application/json");

            using (var response = await client.PutAsync(apiEndpoint + "UpdateEmpresa/" + id, content))
            {
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeletarEmpresa(int id)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");

            using (var response = await client.DeleteAsync(apiEndpoint + "DeleteEmpresa/" + id))
            {
                return response.IsSuccessStatusCode;
            }
        }       

    }
}
