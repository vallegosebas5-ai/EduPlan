using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using EduPlanWPF.Models;

namespace EduPlanWPF.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;

        public AuthService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("apikey", SupabaseConfig.SupabaseAnonKey);
        }

        public async Task<bool> Register(string email, string password)
        {
            var payload = new
            {
                email = email,
                password = password
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(
                $"{SupabaseConfig.SupabaseUrl}/auth/v1/signup",
                content);

            return response.IsSuccessStatusCode;
        }

        public async Task<string> Login(string email, string password)
        {
            var payload = new
            {
                email = email,
                password = password
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(
                $"{SupabaseConfig.SupabaseUrl}/auth/v1/token?grant_type=password",
                content);

            var body = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(body);

            return data.access_token; // devuelve token JWT para futuras peticiones
        }
    }
}
