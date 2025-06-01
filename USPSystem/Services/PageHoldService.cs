using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using USPFinance.Models;
using Microsoft.Extensions.Logging;

namespace USPSystem.Services
{
    public class PageHoldService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PageHoldService> _logger;

        public PageHoldService(
            HttpClient httpClient, 
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<PageHoldService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            var baseUrl = _configuration["FinanceApi:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("FinanceApi:BaseUrl is not configured");
            }
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<(bool IsOnHold, string PageName, string Description)> CheckPageHold(string pagePath)
        {
            try
            {
                _logger.LogInformation($"Checking page hold for path: {pagePath}");

                // Normalize the path before sending to API
                var normalizedPath = NormalizePath(pagePath);
                _logger.LogInformation($"Normalized path: {normalizedPath}");

                var response = await _httpClient.GetAsync($"api/PageHold/check/{Uri.EscapeDataString(normalizedPath)}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API response: {content}");

                    var result = JsonSerializer.Deserialize<PageHoldResponse>(content);
                    if (result != null)
                    {
                        _logger.LogInformation($"Page hold result: IsOnHold={result.IsOnHold}, PageName={result.PageName}");
                        return (result.IsOnHold, result.PageName ?? string.Empty, result.Description ?? string.Empty);
                    }
                }

                _logger.LogWarning($"API request failed with status code: {response.StatusCode}");
                return (false, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking page hold");
                return (false, string.Empty, string.Empty);
            }
        }

        public async Task<List<PageHold>> GetAllPageHolds()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/PageHold");
                if (response.IsSuccessStatusCode)
                {
                    var holds = await response.Content.ReadFromJsonAsync<List<PageHold>>();
                    return holds ?? new List<PageHold>();
                }
                _logger.LogWarning($"Failed to get page holds. Status code: {response.StatusCode}");
                return new List<PageHold>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting page holds");
                return new List<PageHold>();
            }
        }

        public async Task<bool> TogglePageHold(int id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/PageHold/toggle/{id}", null);
                var success = response.IsSuccessStatusCode;
                _logger.LogInformation($"Toggle page hold for ID {id}: {(success ? "Success" : "Failed")}");
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling page hold");
                return false;
            }
        }

        public async Task<bool> UpdatePageHold(int id, string description)
        {
            try
            {
                var updateData = new { Description = description };
                var response = await _httpClient.PostAsJsonAsync($"api/PageHold/update/{id}", updateData);
                var success = response.IsSuccessStatusCode;
                _logger.LogInformation($"Update page hold for ID {id}: {(success ? "Success" : "Failed")}");
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating page hold");
                return false;
            }
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Remove leading and trailing slashes
            path = path.Trim('/');

            // Remove query string if present
            var queryIndex = path.IndexOf('?');
            if (queryIndex >= 0)
                path = path.Substring(0, queryIndex);

            // Convert to lowercase for case-insensitive comparison
            return path.ToLowerInvariant();
        }

        private class PageHoldResponse
        {
            public bool IsOnHold { get; set; }
            public string? PageName { get; set; }
            public string? Description { get; set; }
        }
    }
} 