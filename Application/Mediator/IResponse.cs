namespace Application.Mediator;

public interface IResponse
{
    public string? ErrorMessage { get; set; }

    public bool IsSuccess { get; set; }
}
