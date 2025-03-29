using Dapper;
using lake_data_api.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

public interface ILakeRepository
{
    Task<Lake> Find(string id);
    Task<Lake> Create(Lake lake);
    Task<IEnumerable<Lake>> List();
    Task<Lake> Update(Lake lake);
    Task Delete(string id);
}

public class LakeRepository : ILakeRepository
{
    private readonly MySqlConnection _connection;

    public LakeRepository([FromServices] MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Lake> Find(string id)
    {
        var sql = "SELECT * FROM lakes WHERE Id = @Id";
        var lake = await _connection.QueryAsync<Lake>(sql, new {Id= id});
        if (lake == null)
        {
            throw new Exception("Lake not found");
        }
        return lake.FirstOrDefault()!;
    }

    public async Task<IEnumerable<Lake>> List()
    {
        var sql = "SELECT * FROM lakes";
        var lakes = await _connection.QueryAsync<Lake>(sql);
        if (lakes == null)
        {
            throw new Exception("Lakes not found");
        }
        return lakes;
    }

    public async Task<Lake> Create(Lake lake)
    {
        var sql = "INSERT INTO lakes (Id, Name, USGSSiteId, WQDataSiteId, Latitude, Longitude) VALUES (@Id, @Name, @USGSSiteId, @WQDataSiteId, @Latitude, @Longitude)";

        var count = await _connection.ExecuteAsync(sql, lake);
        if (count == 0)
        {
            throw new Exception("Failed to create lake");
        }

        var returnLake = await _connection.QueryAsync<Lake>("SELECT * FROM lakes WHERE Id = @id", new {Id= lake.Id});
        if (returnLake == null)
        {
            throw new Exception("Lake not found");
        }
        
        return returnLake.FirstOrDefault()!;
    }

    public async Task<Lake> Update(Lake lake)
    {
        var sql = "UPDATE lakes SET Name = @Name, USGSSiteId = @USGSSiteId, WQDataSiteId = @WQDataSiteId, Latitude = @Latitude, Longitude = @Longitude WHERE Id = @Id";
        var count = await _connection.ExecuteAsync(sql, lake);
        if (count == 0)
        {
            throw new Exception("Failed to update lake");
        }
        var returnLake = await _connection.QueryAsync<Lake>("SELECT * FROM lakes WHERE Id = @id", new {Id= lake.Id});
        
        if (returnLake == null)
        {
            throw new Exception("Lake not found");
        }
        
        return returnLake.FirstOrDefault()!;
    }

    public async Task Delete(string id)
    {
        var sql = "DELETE FROM lakes WHERE Id = @Id";
        var count = await _connection.ExecuteAsync(sql, new {Id= id});
        if (count == 0)
        {
            throw new Exception("Failed to delete lake");
        }     
    }
}