using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Presentation.DTO.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBlitz.Presentation.Controllers;

[Authorize]
public class NotificationController(IUnitOfWork unitOfWork, IFirebaseService firebaseService) : BaseContoller(unitOfWork)
{
    private readonly IFirebaseService _firebaseService = firebaseService;

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] IncomingMessageRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        //var message = new Message()
        //{
        //    Notification = new Notification
        //    {
        //        Title = request.Title,
        //        Body = request.Body,
        //    },
        //    Data = new Dictionary<string, string>()
        //    {
        //        ["FirstName"] = "John",
        //        ["LastName"] = "Doe"
        //    },
        //    Token = request.DeviceToken
        //};

        //var messaging = FirebaseMessaging.DefaultInstance;
        //var result = await messaging.SendAsync(message);

        var result = await _firebaseService.SendMessageAsync(request.Title, request.Body, request.DeviceToken, HttpContext.User);

        if (!string.IsNullOrWhiteSpace(result))
        {
            // Message was sent successfully
            return Ok("Message sent successfully!");
        }

        // There was an error sending the message
        throw new Exception("Error sending the message.");
    }

    [HttpGet("{deviceToken}")]
    public async Task<IActionResult> RegisterDeviceToken([FromRoute] string deviceToken)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(deviceToken))
            return BadRequest(ModelState);

        var result = await _firebaseService.RegisterDeviceTokenAsync(deviceToken, HttpContext.User);
        if(!result)
            return BadRequest();

        return Ok();
    }
}
