using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Repositories;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using static Qdrant.Client.Grpc.Conditions;

namespace Modules.Users.Infrastructure.Repositories;

public class FaceEmbedingRepository(QdrantClient qdrantClient) : IFaceEmbedingRepository
{
    public async Task AddEmbeding(FaceEmbeding faceEmbeding)
    {
        var operationInfo = await qdrantClient.UpsertAsync(collectionName: "user_embedings", points: new List<PointStruct>
        {
            new PointStruct (){
                Id = Guid.NewGuid(),
                Vectors = faceEmbeding.Embeding.ToArray(),
                Payload = {
                    ["user_id"] = faceEmbeding.UserId.ToString()
                }
            }
        });
    }

    public async Task<(float? score, Guid? userId)> GetMatching(ICollection<float> embeding)
    {
        IReadOnlyList<ScoredPoint> searchResults = await qdrantClient.SearchAsync(
            collectionName: "user_embedings",
            vector: embeding.ToArray(),
            limit: 1,
            payloadSelector: true
        );
        ScoredPoint? searchResult = searchResults.FirstOrDefault();
        if (searchResult == null)
            return (null, null);
        float? score = searchResult.Score;
        Guid? guid = Guid.Parse(searchResult.Payload.GetValueOrDefault("user_id")!.StringValue);
        return (score, guid);
    }
}
