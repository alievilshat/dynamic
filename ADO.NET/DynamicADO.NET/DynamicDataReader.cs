using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using Npgsql;

namespace DynamicTest
{
    class DynamicDataReader : DynamicObject, IEnumerable, IEnumerator, IDisposable
    {
        private NpgsqlDataReader _reader;
        private NpgsqlTransaction _transaction;

        public DynamicDataReader(NpgsqlDataReader reader)
        {
            this._reader = reader;
        }

        public DynamicDataReader(NpgsqlDataReader reader, NpgsqlTransaction transaction)
        {
            this._reader = reader;
            this._transaction = transaction;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (base.TryGetMember(binder, out result))
                return true;

            result = _reader[binder.Name];
            return true;
        }

        public DataTable LoadToDataTable()
        {
            var res = new DataTable();
            res.Load(_reader);
            return res;
        }

        #region IEnumerable and IEnumerator Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public object Current
        {
            get { return this; }
        }

        public bool MoveNext()
        {
            return _reader.Read();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
        }

        #endregion
    }
}
