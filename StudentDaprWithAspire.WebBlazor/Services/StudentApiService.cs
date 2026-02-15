using StudentDaprWithAspire.WebBlazor.Models;
using System.Net.Http.Json;

namespace StudentDaprWithAspire.WebBlazor.Services;

public class StudentApiService
{
    private readonly HttpClient _httpClient;

    public StudentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Student>>("api/students") ?? new List<Student>();
        }
        catch
        {
            return new List<Student>();
        }
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Student>($"api/students/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<Student?> CreateStudentAsync(Student student)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/students", student);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Student>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<Student?> UpdateStudentAsync(int id, Student student)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/students/{id}", student);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Student>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/students/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
