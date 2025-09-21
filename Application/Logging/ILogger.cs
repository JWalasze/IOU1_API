namespace Application.Logging;

public interface ILogger //TODO Move to some Shared project, it doesn't fit in the Domain
{
    void Info(string message);
    void Warn(string message);
    void Error(string message);
    void Fatal(string message);
    void Debug(string message);
    void Fatal(Exception exception);
    void Error(Exception exception);
    void Fatal(Exception exception, string message);
    void Error(Exception exception, string message);
}
