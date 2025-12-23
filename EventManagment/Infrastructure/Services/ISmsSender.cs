namespace Infrastructure.Services;

public interface ISmsSender
{
    Task SendAsync(string phone, string message, CancellationToken ct = default);
}