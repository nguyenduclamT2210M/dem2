using dem2.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace dem2.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error while calling API");
            }
        }
        public async Task<string> LoginAsync(LoginModel model)
        {
            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("api/auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error during login");
            }
        }
        public async Task<string> RegisterAsync(RegisterModel model)
        {
            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error during registration: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi kết nối hoặc lỗi yêu cầu HTTP
                throw new Exception("A network error occurred during registration.", ex);
            }
        }
        public string GetFullNameFromToken(string token)
        {
            // Giả sử token là chuỗi JSON
            var tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(token);
            return tokenData.TryGetValue("full_name", out var fullName) ? fullName : null;
        }

    }
}
