using Npgsql;

namespace DynamicTest
{
    public static class NpgsqlDynamicExtensions
    {
        public static dynamic CreateDynamicCommand(this NpgsqlConnection con) 
        {
            return new DynamicCommandProvider(con);
        }
    }
}
