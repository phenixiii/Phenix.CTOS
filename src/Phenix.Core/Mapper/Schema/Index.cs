using System;

namespace Phenix.Core.Mapper.Schema
{
    /// <summary>
    /// ����
    /// </summary>
    [Serializable]
    public sealed class Index
    {
        [Newtonsoft.Json.JsonConstructor]
        private Index(string name, bool unique, string[] columnNames)
        {
            _name = name;
            _unique = unique;
            _columnNames = columnNames;
        }

        internal Index(Table owner, string name, bool unique)
            : this(name, unique, null)
        {
            _owner = owner;
        }

        #region ����

        [NonSerialized]
        private Table _owner;

        /// <summary>
        /// ������
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Table Owner
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

        private readonly bool _unique;

        /// <summary>
        /// ��Ψһ��
        /// </summary>
        public bool Unique
        {
            get { return _unique; }
        }

        private string[] _columnNames;

        /// <summary>
        /// �ֶ����嵥
        /// </summary>
        public string[] ColumnNames
        {
            get { return _columnNames; }
            internal set { _columnNames = value; }
        }

        #endregion
    }
}