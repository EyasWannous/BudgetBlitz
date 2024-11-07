using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Application.DTO.Message;

public class IncomingMessageRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    [Required]
    public string DeviceToken { get; set; } = string.Empty;

    // Add more properties as needed based on your notification requirements
}