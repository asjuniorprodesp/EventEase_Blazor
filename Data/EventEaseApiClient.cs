using System.Net.Http.Json;
using EventEase_Blazor.Models;

namespace EventEase_Blazor.Data;

public class EventEaseApiClient(HttpClient httpClient)
{
    public Uri ApiBaseUri => httpClient.BaseAddress ?? throw new InvalidOperationException("BaseAddress da API nao configurado.");

    public async Task<IReadOnlyList<EventItem>> GetEventsAsync()
    {
        var items = await httpClient.GetFromJsonAsync<List<EventItem>>("api/events");
        return items ?? [];
    }

    public Task<EventItem?> GetEventByIdAsync(int eventId)
    {
        return httpClient.GetFromJsonAsync<EventItem>($"api/events/{eventId}");
    }

    public async Task<EventItem> CreateEventAsync(EventCreateRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/events", new
        {
            request.EventName,
            request.EventDescription,
            EventDate = request.EventDate,
            request.EventLocation
        });

        if (!response.IsSuccessStatusCode)
        {
            var details = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Falha ao criar evento: {details}");
        }

        var created = await response.Content.ReadFromJsonAsync<EventItem>();
        if (created is null)
        {
            throw new InvalidOperationException("A API retornou um evento vazio.");
        }

        return created;
    }

    public async Task UpdateEventAsync(EventItem eventItem)
    {
        var response = await httpClient.PutAsJsonAsync($"api/events/{eventItem.EventId}", eventItem);
        if (!response.IsSuccessStatusCode)
        {
            var details = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Falha ao atualizar evento: {details}");
        }
    }

    public async Task<IReadOnlyList<UserItem>> GetUsersAsync()
    {
        var items = await httpClient.GetFromJsonAsync<List<UserItem>>("api/users");
        return items ?? [];
    }

    public async Task UpdateUserAsync(UserItem user)
    {
        var response = await httpClient.PutAsJsonAsync($"api/users/{user.UserId}", user);
        if (!response.IsSuccessStatusCode)
        {
            var details = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Falha ao atualizar usuario: {details}");
        }
    }

    public async Task<IReadOnlyList<EventRegistrationItem>> GetEventRegistrationsByEventAsync(int eventId)
    {
        var items = await httpClient.GetFromJsonAsync<List<EventRegistrationItem>>("api/eventregistrations") ?? [];
        return items.Where(r => r.EventId == eventId).ToList();
    }

    public async Task RegisterAsync(int eventId, string fullName, string email)
    {
        var createdUserResponse = await httpClient.PostAsJsonAsync("api/users", new UserItem
        {
            FullName = fullName,
            Email = email
        });

        UserItem? user;

        if (createdUserResponse.IsSuccessStatusCode)
        {
            user = await createdUserResponse.Content.ReadFromJsonAsync<UserItem>();
        }
        else
        {
            var users = await httpClient.GetFromJsonAsync<List<UserItem>>("api/users") ?? [];
            user = users.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            if (user is null)
            {
                var details = await createdUserResponse.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Falha ao criar usuario: {details}");
            }
        }

        var registrationResponse = await httpClient.PostAsJsonAsync("api/eventregistrations", new EventRegistrationCreateRequest
        {
            UserId = user!.UserId,
            EventId = eventId
        });

        if (!registrationResponse.IsSuccessStatusCode)
        {
            var details = await registrationResponse.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Falha ao registrar inscricao: {details}");
        }
    }

    public async Task<IReadOnlyList<AttendanceItem>> GetAttendanceByEventAsync(int eventId)
    {
        var items = await httpClient.GetFromJsonAsync<List<AttendanceItem>>("api/attendance") ?? [];
        return items.Where(a => a.EventId == eventId).ToList();
    }

    public async Task<AttendanceItem> CheckInAsync(int eventId, int userId)
    {
        var existing = await GetAttendanceByEventAsync(eventId);
        var alreadyCheckedIn = existing.FirstOrDefault(a => a.UserId == userId);
        if (alreadyCheckedIn is not null)
        {
            return alreadyCheckedIn;
        }

        var response = await httpClient.PostAsJsonAsync("api/attendance", new AttendanceItem
        {
            UserId = userId,
            EventId = eventId,
            CheckInTime = DateTime.Now
        });

        if (!response.IsSuccessStatusCode)
        {
            var details = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Falha ao registrar check-in: {details}");
        }

        var created = await response.Content.ReadFromJsonAsync<AttendanceItem>();
        if (created is null)
        {
            throw new InvalidOperationException("A API retornou um check-in vazio.");
        }

        return created;
    }
}
