using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DynamicTest
{
    class Program
    {
        const string CONNECTION_STRING = "User ID=postgres;Password=;Host=;Port=5432;Database=";

        static void Main(string[] args)
        {
            using (var con = new NpgsqlConnection(CONNECTION_STRING))
            {
                con.Open();
                getdata(con);
                Console.WriteLine("------------------------------------------------------------");
                getdatadyn(con);
            }
        }

        private static void getdatadyn(NpgsqlConnection con)
        {
            dynamic cmd = con.CreateDynamicCommand();
            var res = cmd.ordermanager_getorderdetailsbyid(_orderid: 186, _cousevalue: 1).ExecuteReader();

            foreach (var i in res)
            {
                Console.WriteLine(i.name);
            }
        }

        private static void getdata(NpgsqlConnection con)
        {
            var transaction = con.BeginTransaction();
            var cmd = con.CreateCommand();
            cmd.CommandText = "ordermanager_getorderdetailsbyid";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("_orderid", 186);
            cmd.Parameters.AddWithValue("_coursevalue", 1);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader["name"]);
            }
            transaction.Commit();
        }
    }
}
