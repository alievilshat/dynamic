using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using Npgsql;

namespace DynamicTest
{
    class DynamicCommand : DynamicObject
    {
        private NpgsqlCommand _cmd;

        public DynamicCommand(Npgsql.NpgsqlCommand cmd)
        {
            this._cmd = cmd;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            switch (binder.Name)
            {
                case "ExecuteScalar":
                    result = _cmd.ExecuteScalar();
                    return true;

                case "ExecuteReader":
                    var transaction = _cmd.Connection.BeginTransaction();
                    var reader = _cmd.ExecuteReader();
                    result = new DynamicDataReader(reader, transaction);
                    return true;

                case "ExecuteNonQuery":
                    _cmd.ExecuteNonQuery();
                    result = null;
                    return true;

                default:
                    return base.TryInvokeMember(binder, args, out result);
            }

        }
    }
}
