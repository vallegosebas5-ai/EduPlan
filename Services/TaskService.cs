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
    public class TaskService
    {
        private readonly HttpClient _client;

        public TaskService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("apikey", SupabaseConfig.SupabaseAnonKey);
        }

        public async Task<List<TaskModel>> GetTasks(string token, string userId)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.GetAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/tasks?user_id=eq.{userId}");

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TaskModel>>(json);
        }

        public async Task<bool> AddTask(string token, TaskModel task)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var json = JsonConvert.SerializeObject(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/tasks",
                content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTask(string token, Guid taskId, object update)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var json = JsonConvert.SerializeObject(update);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/tasks?id=eq.{taskId}",
                content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTask(string token, Guid taskId)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.DeleteAsync(
                $"{SupabaseConfig.SupabaseUrl}/rest/v1/tasks?id=eq.{taskId}");

            return response.IsSuccessStatusCode;
        }
    }
}

