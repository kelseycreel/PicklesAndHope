using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using PickleAndHope.Models;

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
            //pickle.Id = _pickles.Max(x => x.Id) + 1;
            //_pickles.Add(pickle);

            var query = @"insert into Pickle(NumberInStock,Price,size,Type)               
                            output inserted.*
                            values(@NumberInStock,@Price,@Size,@Type)";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = query;

                command.Parameters.AddWithValue("NumberInStock", pickle.NumberInStock);
                command.Parameters.AddWithValue("Price", pickle.Price);
                command.Parameters.AddWithValue("Size", pickle.Size);
                command.Parameters.AddWithValue("Type", pickle.Type);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {

                    var newPickle = MapReaderToPickle(reader);
                    return newPickle;
                }

                return null;
            }
        }

        public void Remove(string type)
        {

        }

        public Pickle UpdateNumInStock(Pickle pickle)
        {
            //var pickleToUpdate = _pickles.First(p => p.Type == pickle.Type);
            //pickleToUpdate.NumberInStock += pickle.NumberInStock;

            var query = @"update Pickle
                            set NumberInStock = NumberInStock + @NewStock
                            output inserted.*
                            where Id = @Id";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = query;

                command.Parameters.AddWithValue("NewStock", pickle.NumberInStock);
                command.Parameters.AddWithValue("Id", pickle.Id);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    //created a method at the bottom!!!!!

                    var pickleToUpdate = MapReaderToPickle(reader);
                    return pickleToUpdate;
                }

                return null;
            }
        }

        public Pickle GetByType(string type)
        {
            //return _pickles.FirstOrDefault(p => p.Type == type);

            //using statement and brackets removes the need for "closing statements" because the final bracket automatically calls the 'dispose' methods
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {

                sqlConnection.Open();

                var command = sqlConnection.CreateCommand();
                var query = $@"select *
                             from Pickle
                             where Type = @type";
                command.CommandText = query;
                command.Parameters.AddWithValue("Type", type);

                var reader = command.ExecuteReader();
                //ExecuteReader (care about the results) or ExecuteScaler (only care about first cell of the results) or Execute??? (don't care about the results)

                if (reader.Read())
                {
                    //created a method at the bottom!!!!!

                    var pickle = MapReaderToPickle(reader);
                    return pickle;
                }

                return null;
            }
        }

        public List<Pickle> GetAllPickles()
        {
            //return _pickles;

            //Connection string
            //var connectionString = "Server=localhost;Database=PickleAndHope;Trusted_Connection=True;";

            //Sql connection
            //Note: right click on dependencies and manage NuGet packages and search for Microsoft.Data.SqlClient and install, if issues
            var sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            //Sql command
            var command = sqlConnection.CreateCommand();
            command.CommandText = "select * from Pickle";

            //Run the query
            var reader = command.ExecuteReader();
            var pickles = new List<Pickle>();

            // map results to c#
            while (reader.Read())
            {
                //var id = (int)reader["Id"]; 
                // parantheses thing is called explicit or direct casting - taking one value and saying it IS another type of value
                //var type = (string)reader["Type"];

                //var pickle = new Pickle
                //{
                //    Id = (int)reader["Id"],
                //    Type = (string)reader["Type"],
                //    Price = (decimal)reader["Price"],
                //    NumberInStock = (int)reader["NumberInStock"],
                //    Size = (string)reader["Size"]
                //};

                //created a method at the bottom instead of the above!!!!!
                var pickle = MapReaderToPickle(reader);

                pickles.Add(pickle);
            }
            sqlConnection.Close();
            return pickles;
        }

        public Pickle GetById(int id)
        {
            //return _pickles.FirstOrDefault(p => p.Id == id);

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                var query =  @"select * 
                                    from Pickle 
                                    where id = @id";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return MapReaderToPickle(reader);
                }
                return null;
            }
        }

        Pickle MapReaderToPickle(SqlDataReader reader)
        {
            var pickle = new Pickle
            {
                Id = (int)reader["Id"],
                Type = (string)reader["Type"],
                Price = (decimal)reader["Price"],
                NumberInStock = (int)reader["NumberInStock"],
                Size = (string)reader["Size"]
            };

            return pickle;
        }
    }
}
