using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Maui.Storage;
using SPApp.Dto;

namespace PixelPalApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> LoginAsync(string email, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                Console.WriteLine("========== LOGIN START ==========");
                Console.WriteLine($"POST: {_httpClient.BaseAddress}api/auth/login");
                Console.WriteLine($"Email: {email}");

                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Body: {content}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("LOGIN FAILED");
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result == null || string.IsNullOrWhiteSpace(result.Token))
                {
                    Console.WriteLine("TOKEN MISSING IN RESPONSE");
                    return null;
                }

                await SecureStorage.SetAsync("jwt_token", result.Token);
                await SecureStorage.SetAsync("user_role", result.Role);

                Console.WriteLine("LOGIN SUCCESS");
                Console.WriteLine($"Role: {result.Role}");
                Console.WriteLine($"Token start: {result.Token.Substring(0, 25)}...");
                Console.WriteLine("========== LOGIN END ==========");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("LOGIN EXCEPTION");
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            try
            {
                var token = await SecureStorage.GetAsync("jwt_token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("No JWT token found in SecureStorage.");
                    return null;
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine("========== GET CURRENT USER ==========");
                Console.WriteLine($"GET: {_httpClient.BaseAddress}api/user/me");
                Console.WriteLine($"Token start: {token.Substring(0, 25)}...");

                var response = await _httpClient.GetAsync("api/user/me");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Body: {content}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GET CURRENT USER EXCEPTION");
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<string> CreateUserAsync(CreateUserDto user)
        {
            try
            {
                Console.WriteLine("========== CREATE USER ==========");
                Console.WriteLine($"POST: {_httpClient.BaseAddress}api/user");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"FullName: {user.FullName}");

                var response = await _httpClient.PostAsJsonAsync("api/user", user);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Body: {content}");

                if (!response.IsSuccessStatusCode)
                {
                    return $"Fejl: {response.StatusCode} - {content}";
                }

                return "OK";
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("CREATE USER HTTP EXCEPTION");
                Console.WriteLine(ex.ToString());
                return $"Connection failure: {ex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine("CREATE USER EXCEPTION");
                Console.WriteLine(ex.ToString());
                return $"Ukendt fejl: {ex.Message}";
            }
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto user)
        {
            try
            {
                var token = await SecureStorage.GetAsync("jwt_token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("Update failed: no JWT token.");
                    return false;
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine($"PUT: {_httpClient.BaseAddress}api/user/{id}");

                var response = await _httpClient.PutAsJsonAsync($"api/user/{id}", user);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Body: {content}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("UPDATE USER EXCEPTION");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var token = await SecureStorage.GetAsync("jwt_token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("Delete failed: no JWT token.");
                    return false;
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine($"DELETE: {_httpClient.BaseAddress}api/user/{id}");

                var response = await _httpClient.DeleteAsync($"api/user/{id}");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Body: {content}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DELETE USER EXCEPTION");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}