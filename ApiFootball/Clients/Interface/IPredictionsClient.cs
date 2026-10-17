namespace ApiFootball.Clients.Interface;

public interface IPredictionsClient
{
    Task<PagedResponse<PredictionResponse>> GetPredictions(int fixture, CancellationToken cancellationToken = default);
}