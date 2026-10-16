namespace ApiFootball.Clients.Interface;

public interface ITransfersClient
{
    Task<PagedResponse<TransferHistory>> GetTransfers(int? player = null, int? team = null, CancellationToken cancellationToken = default);
}
