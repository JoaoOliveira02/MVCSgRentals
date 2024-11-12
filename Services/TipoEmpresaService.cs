using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace GerenciadorDeEmpresaWEB.Services
{
    public class TipoEmpresaService : ITipoEmpresa
    {
        private const string apiEndpoint = "/api/TipoEmpresa/";

        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        private TipoEmpresaViewModel tipoEmpresaVM;
        private IEnumerable<TipoEmpresaViewModel> tipoEmpresasVM;

        public TipoEmpresaService(IHttpClientFactory clientFactory)
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<TipoEmpresaViewModel>> GetTipoEmpresas()
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetAllTipoEmpresas";  // Ajuste aqui para usar o endpoint correto

            try
            {
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        tipoEmpresasVM = await JsonSerializer
                                       .DeserializeAsync<IEnumerable<TipoEmpresaViewModel>>
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

            return tipoEmpresasVM;
        }



        public async Task<TipoEmpresaViewModel> GetTipoEmpresasPorId(int id)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetTipoEmpresa/" + id;  // Ajuste aqui para usar o endpoint correto

            using (var response = await client.GetAsync(endpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tipoEmpresaVM = await JsonSerializer
                                   .DeserializeAsync<TipoEmpresaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }

            return tipoEmpresaVM;
        }

        public async Task<TipoEmpresaViewModel> CriarTipoEmpresas(TipoEmpresaViewModel tipoEmpresa)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var tipoEmpresaJson = JsonSerializer.Serialize(tipoEmpresa);
            StringContent content = new StringContent(tipoEmpresaJson, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndpoint + "AddTipoEmpresa", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<TipoEmpresaViewModel>(apiResponse, _options);
                }
            }

            return null;
        }

         public async Task<bool> AtualizarTipoEmpresas(int id, TipoEmpresaViewModel tipoEmpresa)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var tipoEmpresaJson = JsonSerializer.Serialize(tipoEmpresa);
            StringContent content = new StringContent(tipoEmpresaJson, Encoding.UTF8, "application/json");

            using (var response = await client.PutAsync(apiEndpoint + "UpdateTipoEmpresa/" + id, content))
            {
                return response.IsSuccessStatusCode;
            }
        }
     
        public async Task<bool> DeletarTipoEmpresas(int id)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");

            using (var response = await client.DeleteAsync(apiEndpoint + "DeleteTipoEmpresa/" + id))
            {
                return response.IsSuccessStatusCode;
            }
        }
    }
}
