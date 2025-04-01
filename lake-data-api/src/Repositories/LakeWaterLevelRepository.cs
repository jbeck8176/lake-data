using Dapper;
using lake_data_api.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

public interface ILakeWaterLevelRepository
{
    Task<LakeWaterLevel> Create(LakeWaterLevel lakeWaterLevel);
}

public class LakeWaterLevelRepository : ILakeWaterLevelRepository
{
    private readonly MySqlConnection _connection;

    public LakeWaterLevelRepository([FromServices] MySqlConnection connection)
    {
        _connection = connection;
    }
    

    public async Task<LakeWaterLevel> Create(LakeWaterLevel lakeWaterLevel)
    {
        lakeWaterLevel.Id = Guid.NewGuid().ToString();

        var sql = "INSERT INTO lakeWaterLevels (Id, LakeId, SurfaceElevation, WaterLevelTimeStamp) VALUES (@Id, @LakeId, @SurfaceElevation, @WaterLevelTimeStamp)";

        var count = await _connection.ExecuteAsync(sql, lakeWaterLevel);

        if (count == 0)
        {
            throw new Exception("Lake water level not created");
        }

        var returnLakeWaterLevel = await _connection.QueryAsync<LakeWaterLevel>("SELECT * FROM lakeWaterLevels WHERE Id = @id", new {Id= lakeWaterLevel.Id});
        if (returnLakeWaterLevel == null)
        {
            throw new Exception("Lake water level not found");
        }

        return returnLakeWaterLevel.FirstOrDefault()!;
    }
}