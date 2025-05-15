using Dapper;
using lake_data_api.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

public interface ILakeWaterTempRepository
{
    Task<LakeWaterTemp> Create(LakeWaterTemp lakeWaterTemp);
}

public class LakeWaterTempRepository : ILakeWaterTempRepository
{
     private readonly MySqlConnection _connection;

    public LakeWaterTempRepository([FromServices] MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<LakeWaterTemp> Create(LakeWaterTemp lakeWaterTemp)
    {
        lakeWaterTemp.Id = Guid.NewGuid().ToString();

        var sql = "INSERT INTO lakeWaterTemps (Id, LakeId, TempInFahrenheit, TempInCelsius, WaterTempTimeStamp) VALUES (@Id, @LakeId, @TempInFahrenheit, @TempInCelsius, @WaterTempTimeStamp)";

        var count = await _connection.ExecuteAsync(sql, lakeWaterTemp);

        if (count == 0)
        {
            throw new Exception("Lake water temperature not created");
        }

        var returnLakeWaterTemp = await _connection.QueryAsync<LakeWaterTemp>("SELECT * FROM lakeWaterTemps WHERE Id = @id", new {Id= lakeWaterTemp.Id});
        if (returnLakeWaterTemp == null)
        {
            throw new Exception("Lake water temperature not found");
        }

        return returnLakeWaterTemp.FirstOrDefault()!;
    
    }
}