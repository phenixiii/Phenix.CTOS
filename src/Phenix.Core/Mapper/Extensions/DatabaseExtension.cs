using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Phenix.Core.Data.Common;

namespace Phenix.Core.Data
{
    /// <summary>
    /// ���ݿ������չ
    /// </summary>
    public static class DatabaseExtension
    {
        /// <summary>
        /// ��ȡʵ���������
        /// </summary>
        public static IDictionary<TKey, T> ReadEntityData<TKey, T>(this Database database, Expression<Func<T, TKey>> keyLambda, string sql, params ParamValue[] paramValues)
            where T : class
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            using (DataReader dataReader = database.CreateDataReader(sql, paramValues))
            {
                return dataReader.ReadEntity(keyLambda);
            }
        }

        /// <summary>
        /// ��ȡʵ���������
        /// </summary>
        public static IList<T> ReadEntityData<T>(this Database database, string sql, params ParamValue[] paramValues)
            where T : class
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            return ReadEntityData<T>(database, sql, CommandBehavior.Default, paramValues);
        }

        /// <summary>
        /// ��ȡʵ���������
        /// </summary>
        public static IList<T> ReadEntityData<T>(this Database database, string sql, CommandBehavior behavior, params ParamValue[] paramValues)
            where T : class
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            using (DataReader dataReader = database.CreateDataReader(sql, behavior, paramValues))
            {
                return dataReader.ReadEntity<T>(behavior == CommandBehavior.SingleRow);
            }
        }
    }
}