using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using System.Net;
using System.Text;
using System.Text.Json;

namespace GerenciadorDeEmpresaWEB.Services
{
    public class UsuarioServices : IUsuarioServices
    {

        private const string apiEndpoint = "/api/Usuario/";

        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        private UsuarioViewModel usuarioVM;
        private IEnumerable<UsuarioViewModel> usuariosVM;

        public UsuarioServices(IHttpClientFactory clientFactory)
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetUsuarios()
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetAllUsuarios";  // Ajuste aqui para usar o endpoint correto

            try
            {
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        usuariosVM = await JsonSerializer
                                       .DeserializeAsync<IEnumerable<UsuarioViewModel>>
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

            return usuariosVM;
        }

        public async Task<UsuarioViewModel> GetUsuariosPorId(int id)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetUsuario/" + id;  // Ajuste aqui para usar o endpoint correto

            using (var response = await client.GetAsync(endpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuarioVM = await JsonSerializer
                                   .DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }

            return usuarioVM;
        }
        public async Task<IEnumerable<UsuarioViewModel>> GetUsuariosPorEmpresa(int empresaId)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetUsuariosByEmpresa/" + empresaId;  // Endpoint correto

            using (var response = await client.GetAsync(endpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var usuariosVM = await JsonSerializer
                        .DeserializeAsync<IEnumerable<UsuarioViewModel>>(apiResponse, _options);  // Deserialize uma lista de usuários

                    return usuariosVM ?? Enumerable.Empty<UsuarioViewModel>();  // Retorna uma lista vazia se o resultado for nulo
                }
                else
                {
                    // Tratar erros da API adequadamente
                    // Você pode logar o erro aqui ou lançar uma exceção customizada
                    return Enumerable.Empty<UsuarioViewModel>();  // Retorna uma lista vazia em caso de erro
                }
            }
        }


        public async Task<IEnumerable<UsuarioViewModel>> GetUsuariosPorPerfil(int perfilId)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var endpoint = apiEndpoint + "GetUsuariosByPerfil/" + perfilId;  // Endpoint correto

            using (var response = await client.GetAsync(endpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var usuariosVM = await JsonSerializer
                        .DeserializeAsync<IEnumerable<UsuarioViewModel>>(apiResponse, _options);  // Deserialize uma lista de usuários
                    return usuariosVM;
                }
                else
                {
                    return null;  // Trate erros de API adequadamente
                }
            }
        }
        public async Task<UsuarioViewModel> CriarUsuarios(UsuarioViewModel usuario)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var usuarioJson = JsonSerializer.Serialize(usuario);
            StringContent content = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndpoint + "AddUsuario", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Se a resposta for sucesso, deserializa e retorna o usuário criado
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    // Captura a resposta de conflito (CPF duplicado) e lança uma exceção
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro: {errorResponse}");
                }
                else
                {
                    // Captura outros erros da API e lança exceção com a mensagem de erro
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro na chamada de API: {errorResponse}");
                }
            }
        }

        public async Task<bool> AtualizarUsuarios(int id, UsuarioViewModel usuario)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var usuarioJson = JsonSerializer.Serialize(usuario);
            StringContent content = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            using (var response = await client.PutAsync(apiEndpoint + "UpdateUsuario/" + id, content))
            {
                return response.IsSuccessStatusCode;
            }
        }


        public async Task<bool> DeletarUsuarios(int id)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> CPFExistsAsync(string cpf, int empresaId)
        {
            var client = _clientFactory.CreateClient("APIGerenciadorDeEmpresas");
            var requestUrl = $"{apiEndpoint}CPFExists?cpf={cpf}&empresaId={empresaId}";

            try
            {
                using (var response = await client.GetAsync(requestUrl))
                {
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        // CPF já existe
                        return true;
                    }
                    else if (response.IsSuccessStatusCode)
                    {
                        // CPF não existe, retorna false
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<bool>(responseContent);
                    }

                    // Para qualquer outro status, considera que o CPF não existe
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Tratar ou logar o erro
                return false;
            }
        }

    }
}
