using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace GerenciadorDeEmpresaWEB.Services
{
    public class TipoEmpresaServices : ITipoEmpresaServices
    {
        private const string apiEndpoint = "/api/TipoEmpresa/";

        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        private TipoEmpresaViewModel tipoEmpresaVM;
        private IEnumerable<TipoEmpresaViewModel> tipoEmpresasVM;

        public TipoEmpresaServices(IHttpClientFactory clientFactory)
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<TipoEmpresaViewModel>> GetTipoEmpresas()
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetAllTipoEmpresas";  // Ajuste para o endpoint correto

            try
            {
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();

                        var tipoEmpresasVM = await JsonSerializer.DeserializeAsync<IEnumerable<TipoEmpresaViewModel>>(apiResponse, _options);

                        // Retorna uma lista vazia se a API retornar null
                        return tipoEmpresasVM ?? new List<TipoEmpresaViewModel>();
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao chamar a API: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar a solicitação: {ex.Message}");
            }

            // Retorna uma lista vazia em caso de erro
            return new List<TipoEmpresaViewModel>();
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
                else
                {
                    // Captura a resposta de erro da API
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro na chamada de API: {errorResponse}");
                }
            }
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
