using System;

namespace Phenix.Core.Mapper.Schema
{
    /// <summary>
    /// ���
    /// </summary>
    [Serializable]
    public sealed class ForeignKey
    {
        [Newtonsoft.Json.JsonConstructor]
        private ForeignKey(string name, string tableName, string columnName, string primaryKeyTableName, string primaryKeyColumnName)
        {
            _name = name;
            _tableName = tableName;
            _columnName = columnName;
            _primaryKeyTableName = primaryKeyTableName;
            _primaryKeyColumnName = primaryKeyColumnName;
        }

        internal ForeignKey(MetaData owner, string name, string tableName, string columnName, string primaryKeyTableName, string primaryKeyColumnName)
            : this(name, tableName, columnName, primaryKeyTableName, primaryKeyColumnName)
        {
            _owner = owner;
        }

        #region ����

        [NonSerialized]
        private MetaData _owner;

        /// <summary>
        /// �������ݿ�ܹ�
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public MetaData Owner
        {
            get { return _owner; }
            internal set { _owner = value; }
        }

        private readonly string _name;

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private readonly string _tableName;

        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }

        private readonly string _columnName;

        /// <summary>
        /// ColumnName
        /// </summary>
        public string ColumnName
        {
            get { return _columnName; }
        }

        [NonSerialized]
        private Table _table;

        /// <summary>
        /// Table
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Table Table
        {
            get { return _table ??= Owner.FindTable(TableName, true); }
        }

        private readonly string _primaryKeyTableName;

        /// <summary>
        /// ����TableName
        /// </summary>
        public string PrimaryKeyTableName
        {
            get { return _primaryKeyTableName; }
        }

        private readonly string _primaryKeyColumnName;

        /// <summary>
        /// ����ColumnName
        /// </summary>
        public string PrimaryKeyColumnName
        {
            get { return _primaryKeyColumnName; }
        }

        [NonSerialized]
        private Column _primaryKeyColumn;

        /// <summary>
        /// ����Column
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Column PrimaryKeyColumn
        {
            get { return _primaryKeyColumn ??= Owner.FindTable(PrimaryKeyTableName, true).Columns[PrimaryKeyColumnName]; }
        }

        #endregion
    }
}