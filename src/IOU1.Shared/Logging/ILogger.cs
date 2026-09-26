namespace IOU1.Shared.Logging;

public interface ILogger
{
    void Info(string message);

    void Warn(string message);

    void Debug(string message);

    void Fatal(Exception exception);

    void Fatal(Exception exception, string message);

    void Fatal(string message);

    void Error(Exception exception);

    void Error(Exception exception, string message);

    void Error(string message);
}
