using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http.Json;
using USPFinance.Models;
using USPSystem.ViewModels;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace USPSystem.Services
{
    public class StudentHoldService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentHoldService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri(_configuration["FinanceApi:BaseUrl"]);
        }

        public async Task<HoldStatusViewModel> CheckHoldStatus(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return new HoldStatusViewModel { IsOnHold = false };
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/StudentHold/check-hold/{studentId}");
                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<USPFinance.Models.StudentFinance>();
                    if (student != null)
                    {
                        return new HoldStatusViewModel
                        {
                            IsOnHold = student.IsOnHold,
                            HoldReason = student.HoldReason ?? string.Empty,
                            HoldStartDate = student.HoldStartDate,
                            HoldPlacedBy = student.HoldPlacedBy ?? string.Empty
                        };
                    }
                }
                return new HoldStatusViewModel { IsOnHold = false };
            }
            catch (Exception)
            {
                // Log the error if needed
                return new HoldStatusViewModel { IsOnHold = false };
            }
        }

        public async Task<List<USPFinance.Models.StudentFinance>> GetAllStudents()
        {
            var response = await _httpClient.GetAsync("api/StudentHold/all-students");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<USPFinance.Models.StudentFinance>>();
            }
            return new List<USPFinance.Models.StudentFinance>();
        }

        public async Task<bool> PlaceHold(string studentId, string reason)
        {
            var placedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
            
            var request = new
            {
                StudentId = studentId,
                Reason = reason,
                PlacedBy = placedBy
            };

            var response = await _httpClient.PostAsJsonAsync("api/StudentHold/place-hold", request);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error placing hold: {error}");
            }
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveHold(string studentId)
        {
            var response = await _httpClient.PostAsync($"api/StudentHold/remove-hold?studentId={studentId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckHold(string studentId)
        {
            var response = await _httpClient.GetAsync($"api/StudentHold/check-hold/{studentId}");
            return response.IsSuccessStatusCode;
        }
    }
} 