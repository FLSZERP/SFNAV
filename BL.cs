using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ALAN.SQLServerDAL;
namespace SFAPP
{
    class BL
    {
        private int _PageSize = 10;
        private int _PageIndex = 1;
        private int _PageCount = 0;
        private int _TotalCount = 0;
        private string _TableName;//表名
        private string _QueryFieldName = "*";//表字段FieldStr
        private string _OrderStr = string.Empty; //排序_SortStr
        private string _QueryCondition = string.Empty;//查询的条件 RowFilter
        private string _PrimaryKey = string.Empty;//主键
        private bool _isQueryTotalCounts = true;//是否查询总的记录条数
        /// <summary>
        /// 是否查询总的记录条数
        /// </summary>
        public bool IsQueryTotalCounts
        {
            get { return _isQueryTotalCounts; }
            set { _isQueryTotalCounts = value; }
        }
        /// <summary>
        /// 显示页数
        /// </summary>
        public int PageSize
        {
            get
            {
                return _PageSize;

            }
            set
            {
                _PageSize = value;
            }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return _PageCount;
            }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return _TotalCount;
            }
        }
        /// <summary>
        /// 表名，包括视图
        /// </summary>
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
            }
        }
        /// <summary>
        /// 表字段FieldStr
        /// </summary>
        public string QueryFieldName
        {
            get
            {
                return _QueryFieldName;
            }
            set
            {
                _QueryFieldName = value;
            }
        }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderStr
        {
            get
            {
                return _OrderStr;
            }
            set
            {
                _OrderStr = value;
            }
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        public string QueryCondition
        {
            get
            {
                return _QueryCondition;
            }
            set
            {
                _QueryCondition = value;
            }
        }
        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey
        {
            get
            {
                return _PrimaryKey;
            }
            set
            {
                _PrimaryKey = value;
            }
        }
        public DataSet QueryDataTable()
        {

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 200),
				    new SqlParameter("@ID" , SqlDbType.NVarChar , 150),	
                    new SqlParameter("@FldSort", SqlDbType.NVarChar , 200 ),
                    new SqlParameter("@page", SqlDbType.Int),
					new SqlParameter("@pageSize", SqlDbType.Int),									
                    new SqlParameter("@fldName", SqlDbType.NVarChar, 2000),
					new SqlParameter("@strCondition", SqlDbType.NVarChar,1000),                   
                    new SqlParameter("@Dist" ,SqlDbType.Int)
                    //new SqlParameter("@pageCount)", SqlDbType.Int),
                    //new SqlParameter("@Counts)", SqlDbType.Int)
					};
            parameters[0].Value = _TableName;
            parameters[1].Value = _PrimaryKey;
            parameters[2].Value = _OrderStr;
            parameters[3].Value = PageIndex;
            parameters[4].Value = PageSize;
            parameters[5].Value = _QueryFieldName;
            parameters[6].Value = _QueryCondition;
            parameters[7].Value = "";
            //parameters[8].Value = pagecount;
            //parameters[9].Value = recordcount;
            DataSet ds = DbHelperSQL.RunProcedure("proc_ListPage", parameters, _TableName);
            if (_isQueryTotalCounts)
            {
                _TotalCount = GetTotalCount();
            }
            if (_TotalCount == 0)
            {
                _PageIndex = 0;
                _PageCount = 0;
            }
            else
            {
                _PageCount = _TotalCount % _PageSize == 0 ? _TotalCount / _PageSize : _TotalCount / _PageSize + 1;
                if (_PageIndex > _PageCount)
                {
                    _PageIndex = _PageCount;

                    parameters[4].Value = _PageSize;

                    ds = QueryDataTable();
                }
            }
            return ds;
        }
        public DataSet QueryDataTable(out int pageCount, out int Counts)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 2000),
				    new SqlParameter("@ID" , SqlDbType.VarChar , 150),	
                    new SqlParameter("@fldSort", SqlDbType.VarChar , 200 ),
                    new SqlParameter("@page", SqlDbType.Int),
					new SqlParameter("@pageSize", SqlDbType.Int),									
                    new SqlParameter("@fldName", SqlDbType.VarChar, 2000),
					new SqlParameter("@strCondition", SqlDbType.VarChar,1000), 
                    new SqlParameter("@Sort", SqlDbType.Bit), 
                    new SqlParameter("@Dist" ,SqlDbType.Int)
                    //new SqlParameter("@pageCount)", SqlDbType.Int),
                    //new SqlParameter("@Counts)", SqlDbType.Int)
					};
            parameters[0].Value = _TableName;
            parameters[1].Value = _PrimaryKey;
            parameters[2].Value = _OrderStr;
            parameters[3].Value = PageIndex;
            parameters[4].Value = PageSize;
            parameters[5].Value = _QueryFieldName;
            parameters[6].Value = _QueryCondition;
            //parameters[7].Value = "";
            //parameters[8].Value = "";

            DataSet ds = DbHelperSQL.RunProcedure("proc_ListPage", parameters, _TableName, out pageCount, out Counts);
           
            if (Counts == 0)
            {
                _PageIndex = 0;
                _PageCount = 0;
            }
            if (pageCount == 0)
            {
                _PageIndex = 0;
                _PageCount = 0;
            }
            else
            {
                //_PageCount = Counts % _PageSize == 0 ? Counts / _PageSize : Counts / _PageSize + 1;
                if (_PageIndex > pageCount)
                {
                    _PageIndex = pageCount;

                    parameters[4].Value = _PageSize;

                    ds = QueryDataTable();
                }
            }
            return ds;
        }
        public int GetTotalCount()
        {
            string strSql = " select count(1) from [" + _TableName + "]";
            if (_QueryCondition != string.Empty)
            {
                strSql += " where " + _QueryCondition;
            }
            return int.Parse(DbHelperSQL.GetSingle(strSql).ToString());
        }
    }
}
