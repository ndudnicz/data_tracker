using dotnet_api.Data.Entities;
using Microsoft.Data.Sqlite;

namespace dotnet_api.Repositories;

public class DataRepository(string connectionString): IDataRepository
{
    private readonly string tableName = "data";
    public IEnumerable<DataObj> Get()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {tableName}";

            List<DataObj> data = new List<DataObj>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(new DataObj()
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetDouble(1),
                        CreatedAt = reader.GetDateTime(2),
                    });

                }
            }

            return data;
        }
    }

    public DataObj Get(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {tableName} WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            DataObj data = new DataObj();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    data.Id = reader.GetInt32(0);
                    data.Value = reader.GetDouble(1);
                    data.CreatedAt = reader.GetDateTime(2);
                }
            }

            return data.Id > 0 ? data : null;
        }
    }
    
    public DataObj Create(DataObj element)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {tableName} (value, created_at) VALUES ($value, $created_at) RETURNING id";
            command.Parameters.AddWithValue("$value", element.Value);
            command.Parameters.AddWithValue("$created_at", element.CreatedAt);

            var id = (long)command.ExecuteScalar();
            element.Id = (int)id;
        }

        return element;
    }

    public int Delete(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {tableName} WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            return command.ExecuteNonQuery();
        }
    }
}