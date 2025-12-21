using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Infrastructure.Services;

public class SessionUserService : ISessionUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string UserIdKey = "UserId";

    public SessionUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
            throw new InvalidOperationException("Session is not available");

        if (!session.TryGetValue(UserIdKey, out var userIdBytes))
        {
            // Generate new Guid for this session
            var userId = Guid.NewGuid();
            var userIdString = userId.ToString();
            session.Set(UserIdKey, Encoding.UTF8.GetBytes(userIdString));
            return userId;
        }

        // Retrieve existing Guid from session
        var userIdStringFromSession = Encoding.UTF8.GetString(userIdBytes);
        if (string.IsNullOrEmpty(userIdStringFromSession))
        {
            // Fallback: generate new Guid if string is empty
            var userId = Guid.NewGuid();
            var userIdString = userId.ToString();
            session.Set(UserIdKey, Encoding.UTF8.GetBytes(userIdString));
            return userId;
        }

        return Guid.Parse(userIdStringFromSession);
    }
}

