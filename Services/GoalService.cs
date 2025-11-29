using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using EduPlanWPF.Models;
using Newtonsoft.Json;

namespace EduPlanWPF.Services
{
    public class GoalService
    {
        private readonly HttpClient _client;

        public GoalService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("apikey", SupabaseConfig.SupabaseAnonKey);
        }

        public async Task<List<GoalModel>> GetGoals(string token, string userId)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.GetAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/goals?user_id=eq.{userId}");

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GoalModel>>(json);
        }

        public async Task<bool> AddGoal(string token, GoalModel goal)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var json = JsonConvert.SerializeObject(goal);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/goals",
                content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateGoal(string token, Guid goalId, object update)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var json = JsonConvert.SerializeObject(update);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/goals?id=eq.{goalId}",
                content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteGoal(string token, Guid goalId)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.DeleteAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/goals?id=eq.{goalId}");

            return response.IsSuccessStatusCode;
        }
    }
}

