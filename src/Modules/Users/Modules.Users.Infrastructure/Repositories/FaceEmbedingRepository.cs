using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Repositories;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace Modules.Users.Infrastructure.Repositories;

public class FaceEmbedingRepository(QdrantClient qdrantClient) : IFaceEmbedingRepository
{
    public async Task AddEmbeding(FaceEmbeding faceEmbeding)
    {
        var operationInfo = await qdrantClient.UpsertAsync(collectionName: "user_embedings", points: new List<PointStruct>
        {
            new PointStruct (){
                Vectors = faceEmbeding.Embeding.ToArray(),
                Payload = {
                    ["user_id"] = faceEmbeding.UserId.ToString()
                }
            }
        });
    }

    public async Task<(float? score, Guid? userId)> GetMatching(ICollection<float> embeding)
    {
        IReadOnlyList<ScoredPoint> searchResults = await qdrantClient.QueryAsync(
            collectionName: "user_embedings",
            query: embeding.ToArray(),
            limit: 1);
        ScoredPoint? searchResult = searchResults.FirstOrDefault();
        if (searchResult == null)
            return (null, null);
        return (searchResult.Score, new Guid(searchResult.Payload.GetValueOrDefault("user_id")!.StringValue));
    }
}
