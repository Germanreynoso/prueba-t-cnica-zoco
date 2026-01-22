using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using BackendApi.Models;

namespace BackendApi.Services;

public class SessionService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public void LogLogin(int userId)
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        
        var sessionLog = new SessionLog
        {
            UserId = userId,
            Action = "Login",
            LoginTime = DateTime.UtcNow,
            IpAddress = ipAddress ?? "Unknown"
        };
        
        _context.SessionLogs.Add(sessionLog);
        _context.SaveChanges();
    }

    public void LogLogout(int userId)
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        
        var sessionLog = _context.SessionLogs
            .Where(s => s.UserId == userId && s.LogoutTime == null)
            .OrderByDescending(s => s.LoginTime)
            .FirstOrDefault();
        
        if (sessionLog != null)
        {
            sessionLog.LogoutTime = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }

    public void UpdateLogoutTime(int sessionLogId)
    {
        var sessionLog = _context.SessionLogs.Find(sessionLogId);
        if (sessionLog != null)
        {
            sessionLog.LogoutTime = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }
}