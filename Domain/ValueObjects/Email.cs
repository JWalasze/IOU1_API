using Domain.Base;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public record Email : ValueObject
{
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public string EmailAddress { get; }

    public Email(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            throw new ArgumentException("Email address cannot be empty.");
        }

        var regex = new Regex(EmailPattern);
        if (!regex.IsMatch(emailAddress))
        {
            throw new ArgumentException("Invalid email address format.");
        }

        EmailAddress = emailAddress;
    }
}
