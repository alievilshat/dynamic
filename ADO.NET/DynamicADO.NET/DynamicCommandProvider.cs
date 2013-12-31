using System.Data;
using System.Dynamic;
using Npgsql;

namespace DynamicTest
{
    public class DynamicCommandProvider : DynamicObject
    {
        private NpgsqlConnection _con;

        public DynamicCommandProvider(NpgsqlConnection con)
        {
            this._con = con;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var cmd = _con.CreateCommand();
            cmd.CommandText = binder.Name;
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < binder.CallInfo.ArgumentCount; i++)
            {
                cmd.Parameters.AddWithValue(binder.CallInfo.ArgumentNames[i], args[i]);
            }
            result = new DynamicCommand(cmd);
            return true;
        }
    }
}
