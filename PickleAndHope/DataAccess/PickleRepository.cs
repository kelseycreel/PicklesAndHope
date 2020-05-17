using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using PickleAndHope.Models;
using Dapper;

namespace PickleAndHope.DataAccess
{
    public class PickleRepository
    {
        //static List<Pickle> _pickles = new List<Pickle>
        //{
        //    new Pickle
        //    {
        //        Type = "bread and butter",
        //        NumberInStock = 3,
        //        Id = 1
        //    }
        //};

        const string ConnectionString = "Server=localhost;Database=PickleAndHope;Trusted_Connection=True;";


        public Pickle Add(Pickle pickle)
        {
            // if you wanted to use the static list, this is an example of adding a pickle
            // pickle.Id = _pickles.Max(x => x.Id) + 1;
            // _pickles.Add(pickle);

            // if you wanted to use ADO.NET this is example of adding a pickle
            //var query = @"insert into Pickle(NumberInStock,Price,size,Type)               
            //                output inserted.*
            //                values(@NumberInStock,@Price,@Size,@Type)";

            //Connection string
            //var connectionString = "Server=localhost;Database=PickleAndHope;Trusted_Connection=True;";

            //Sql connection
            //Note: right click on dependencies and manage NuGet packages and search for Microsoft.Data.SqlClient and install, if issues

            //using statement and brackets removes the need for "closing statements" because the final bracket automatically calls the 'dispose' methods

            //using (var connection = new SqlConnection(ConnectionString))
            //{
            //    connection.Open();
            //    var command = connection.CreateCommand();
            //    command.CommandText = query;

            //    command.Parameters.AddWithValue("NumberInStock", pickle.NumberInStock);
            //    command.Parameters.AddWithValue("Price", pickle.Price);
            //    command.Parameters.AddWithValue("Size", pickle.Size);
            //    command.Parameters.AddWithValue("Type", pickle.Type);

            //    var reader = command.ExecuteReader();

            //    if (reader.Read())
            //    {

            //        var newPickle = MapReaderToPickle(reader);
            //        return newPickle;
            //    }

            //    return null;
            //}

            //using dapper to add a pickle
            var sql = @"insert into Pickle(NumberInStock,Price,size,Type)               
                            output inserted.*
                            values(@NumberInStock,@Price,@Size,@Type)";

            using (var db = new SqlConnection(ConnectionString))
            {
                var result = db.QueryFirstOrDefault<Pickle>(sql, pickle);
                return result;
            }
        }

        public void Remove(string type)
        {

        }

        public Pickle UpdateNumInStock(Pickle pickle)
        {
            var sql = @"update Pickle
                            set NumberInStock = NumberInStock + @NewStock
                            output inserted.*
                            where Id = @Id";

            using (var db = new SqlConnection(ConnectionString))
            {
                var result = db.QueryFirst<Pickle>(sql, pickle);
                return result;
            }
        }

        public Pickle GetByType(string type)
        {
            using (var db = new SqlConnection(ConnectionString))
            {

                var sql = $@"select *
                             from Pickle
                             where Type = @type";

                var result = db.QueryFirstOrDefault<Pickle>(sql, new { Type = type });
                return result;
            }
        }

        public List<Pickle> GetAllPickles()
        {
            var sql = "select * from Pickle";
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<Pickle>(sql).ToList();
            }
        }

        public Pickle GetById(int id)
        {
            var sql = @"select * 
                        from Pickle 
                        where id = @id";
            using (var db = new SqlConnection(ConnectionString))
            {
                var result = db.QueryFirstOrDefault<Pickle>(sql, new { Id = id });
                return result;
            }
        }

        // for ADO.NET
        //Pickle MapReaderToPickle(SqlDataReader reader)
        //{
        //    var pickle = new Pickle
        //    {
        //        Id = (int)reader["Id"],
        //        Type = (string)reader["Type"],
        //        Price = (decimal)reader["Price"],
        //        NumberInStock = (int)reader["NumberInStock"],
        //        Size = (string)reader["Size"]
        //    };

        //    return pickle;
        //}
    }
}
