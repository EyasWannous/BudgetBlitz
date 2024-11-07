using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BudgetBlitz.Application.Services;

public class FirebaseService(UserManager<User> userManager, IUnitOfWork unitOfWork) : IFirebaseService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> RegisterDeviceTokenAsync(string deviceToken, ClaimsPrincipal principal)
    {
        var loggedINUser = await _userManager.GetUserAsync(principal);
        if (loggedINUser is null)
            return false;

        var isTokenExsits = await _unitOfWork.UserDevices.IsExistsAsync(deviceToken);
        if (isTokenExsits)
            return false;

        var userDevice = new UserDevice
        {
            UsertId = loggedINUser.Id,
            DeviceToken = deviceToken,
        };

        await _unitOfWork.UserDevices.AddAsync(userDevice);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<string> SendMessageAsync(string title, string body, string deviceToken, ClaimsPrincipal principal)
    {
        var loggedINUser = await _userManager.GetUserAsync(principal);
        //if(loggedINUser is null)

        var message = new Message()
        {
            Notification = new Notification
            {
                Title = title,
                Body = body,
            },
            Data = new Dictionary<string, string>()
            {
                ["UserName"] = loggedINUser?.UserName ?? "There Is No Name",
            },
            Token = deviceToken
        };

        //var messaging = FirebaseMessaging.DefaultInstance;
        //var result = await messaging.SendAsync(message);

        return await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
}
