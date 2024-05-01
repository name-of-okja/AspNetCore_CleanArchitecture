
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace CleanArchitecture.Infrastructure.Services;
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings?.Value;
        _logger = logger;
    }
    
    public async Task<bool> SendEmail(Email email)
    {
        #if DEBUG
        return true;
        #endif

        var client = new SendGridClient(_emailSettings.ApiKey);

        var subject = email.Subject;
        var to = new EmailAddress(email.To);
        var emailBody = email.Body;

        var from = new EmailAddress
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName,
        };

        var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
        var response = await client.SendEmailAsync(sendGridMessage);

        if (response.StatusCode is System.Net.HttpStatusCode.Accepted or System.Net.HttpStatusCode.OK)
        {
            return true;
        }

        _logger.LogError($"Email Send Failed :: {email.To} :: {emailBody}");
        return false;
    }
}
