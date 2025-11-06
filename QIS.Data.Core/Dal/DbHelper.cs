using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;

namespace QIS.Data.Core.Dal
{
    public class DbHelper
    {
        #region Private Field

        private readonly List<ColumnAttribute> _listSchema;
        private readonly string _prefixParameter;
        private readonly string _tableName;

        #endregion

        #region Constructor

        public DbHelper(Type type)
            : this(type, DbConst.MAIN_CN_SETTING)
        {
        }

        public DbHelper(Type type, string cnsconfig)
        {
            _tableName = GetTableName(type);
            _listSchema = new List<ColumnAttribute>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                foreach (Attribute attrib in prop.GetCustomAttributes(true))
                {
                    ColumnAttribute schema = attrib as ColumnAttribute;
                    if (schema != null)
                    {
                        _listSchema.Add(schema);
                    }
                }
            }

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[cnsconfig];
            switch (settings.ProviderName)
            {
                case DbConst.SQL_SERVER_CODENAME:
                    _prefixParameter = "@p_";
                    break;
                case DbConst.OLEDB_CODENAME:
                    _prefixParameter = "@p_";
                    break;
                case DbConst.ORACLE_CODENAME:
                    _prefixParameter = ":p_";
                    break;
            }
        }

        private static string GetTableName(MemberInfo type)
        {
            TableAttribute tableInfo = (TableAttribute) Attribute.GetCustomAttribute(type, typeof (TableAttribute));
            if (tableInfo == null || tableInfo.Name.Equals(""))
            {
                return type.Name;
            }
            else
            {
                return tableInfo.Name;
            }
        }

        #endregion

        #region Public SqlBuilder

        private string GetInsertAuditLogScript(object obj)
        {
            StringBuilder auditLogScript = new StringBuilder();
            string auditLogHeaderScript = "";
            if (obj != null && obj is DbDataModel)
            {
                Type type = obj.GetType();
                Hashtable hash = ((DbDataModel) obj).OriginalValue;

                string primaryKeyData = "";
                foreach (DbFieldValue dbValue in GetKeyValues(obj))
                {
                    primaryKeyData +=
                        string.Format(" AND [{0}] = ''{1}''", dbValue.FieldName, dbValue.Current);
                }
                if (primaryKeyData != string.Empty)
                    primaryKeyData = primaryKeyData.Substring(5);


                foreach (PropertyInfo prop in type.GetProperties())
                {
                    foreach (Attribute attrib in prop.GetCustomAttributes(false))
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        object newValue = prop.GetValue(obj, null);

                        if (schema.DataType != "Byte[]")
                            auditLogScript.AppendLine(
                                @"INSERT INTO AuditLogData (AuditLogTransactionID,ColumnName,OldValueVarchar,NewValueVarchar,DataType) VALUES (@l_AuditLogTransactionID," +
                                string.Format("'{0}','{1}','{2}','{3}')", schema.Name, "", newValue == null ? newValue:newValue.ToString().Replace("'", "''"), "A"));
                    }
                }
            }

            string auditLogBody = auditLogScript.ToString();
            string retScript;
            if (auditLogBody != "")
                retScript = auditLogHeaderScript + auditLogBody +
                            @"	-- Restore @@IDENTITY VALUE DECLARE @l_maxprec AS VARCHAR(2) SET @l_maxprec = CAST(@@MAX_PRECISION as VARCHAR(2))
                            EXEC('SELECT IDENTITY(DECIMAL('+@l_maxprec+',0),'+@l_IdentitySave+',1) id INTO #tmp')";
            else
                retScript = "";

            return retScript;
        }

        public IDbContext Insert(IDbContext ctx, object obj, bool isAuditLog)
        {
            string sqlInsert;
            string fieldName = "";
            string parameter = "";

            if (obj is DbDataModel)
            {
                Type type = obj.GetType();
                PropertyInfo[] propInfs = type.GetProperties();
                foreach (PropertyInfo prop in propInfs)
                {
                    object[] custAttr = prop.GetCustomAttributes(false);
                    foreach (Attribute attrib in custAttr)
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        if (schema != null && !schema.IsComputed
                            && !schema.IsIdentity
                            && !schema.IsTimeStamp)
                        {
                            object fieldValue = prop.GetValue(obj, null);
                            if (!schema.IsNullable)
                                fieldValue = CheckIsNull(fieldValue, prop.PropertyType);

                            //if (fieldValue != null &&
                            //    !(schema.IsNullable && IsFieldNullAbleNotAssignValue(fieldValue, prop.PropertyType)))
                            //{
                            //    ctx.Add(string.Format("{0}{1}", _prefixParameter, schema.Name), fieldValue);
                            //    fieldName += string.Format(", {0}", schema.Name);
                            //    parameter += string.Format(", {0}{1}", _prefixParameter, schema.Name);

                            //    //Simpan informasi primary key di session untuk informasi error handling bila terjadi duplicate key
                            //    if (schema.IsPrimaryKey)
                            //    {
                            //        HttpContext.Current.Session["_LastSqlProcessPKField"] += ";" + schema.Name;
                            //        HttpContext.Current.Session["_LastSqlProcessPKValue"] += ";" + fieldValue;
                            //    }
                            //}

                            fieldName += string.Format(", {0}", schema.Name);
                            if (fieldValue != null &&
                               !(schema.IsNullable && IsFieldNullAbleNotAssignValue(fieldValue, prop.PropertyType)))
                            {
                                parameter += string.Format(", {0}{1}", _prefixParameter, schema.Name);

                                if (fieldValue.GetType().FullName.Contains("DateTime"))
                                {
                                    if (fieldValue is DBNull || Convert.ToDateTime(fieldValue).Year < 1900)
                                        fieldValue = new DateTime(1900, 1, 1);
                                }
                                ctx.Add(string.Format("{0}{1}", _prefixParameter, schema.Name), fieldValue);

                            }
                            else
                            {
                                parameter += ", NULL";
                            }

                        }
                    }
                }
            }

            fieldName = (fieldName.Length > 2) ? fieldName.Substring(2) : fieldName;
            parameter = (parameter.Length > 2) ? parameter.Substring(2) : parameter;


            sqlInsert = string.Format("INSERT INTO {0} ", _tableName);
            sqlInsert += string.Format("( {0} ) ", fieldName);
            sqlInsert += string.Format(" {0} ", "VALUES");
            sqlInsert += string.Format("( {0} )", parameter);

            if (isAuditLog)
                ctx.CommandText = GetInsertAuditLogScript(obj) + "; " + sqlInsert;
            else
                ctx.CommandText = sqlInsert;


            //if (!_tableName.ToLower().Contains("systranslation"))
            //{
            //    SiAuto.Main.LogSql("Insert " + _tableName, ctx.CommandText);
            //    SiAuto.Main.LogObject(_tableName, obj);
            //}

            return ctx;
        }

        public IDbContext InsertReturnPrimaryKeyID(IDbContext ctx, object obj, bool isAuditLog)
        {
            string sqlInsert;
            string fieldName = "";
            string parameter = "";
            string primaryColumn = "ID";

            if (obj is DbDataModel)
            {
                Type type = obj.GetType();
                PropertyInfo[] propInfs = type.GetProperties();
                foreach (PropertyInfo prop in propInfs)
                {
                    object[] custAttr = prop.GetCustomAttributes(false);
                    foreach (Attribute attrib in custAttr)
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        if (schema != null && !schema.IsComputed
                            && !schema.IsIdentity
                            && !schema.IsTimeStamp)
                        {
                            object fieldValue = prop.GetValue(obj, null);
                            if (!schema.IsNullable)
                                fieldValue = CheckIsNull(fieldValue, prop.PropertyType);

                            fieldName += string.Format(", {0}", schema.Name);
                            if (fieldValue != null &&
                               !(schema.IsNullable && IsFieldNullAbleNotAssignValue(fieldValue, prop.PropertyType)))
                            {
                                parameter += string.Format(", {0}{1}", _prefixParameter, schema.Name);

                                if (fieldValue.GetType().FullName.Contains("DateTime"))
                                {
                                    if (fieldValue is DBNull || Convert.ToDateTime(fieldValue).Year < 1900)
                                        fieldValue = new DateTime(1900, 1, 1);
                                }
                                ctx.Add(string.Format("{0}{1}", _prefixParameter, schema.Name), fieldValue);

                            }
                            else
                            {
                                parameter += ", NULL";
                            }
                        }

                        if (schema.IsPrimaryKey)
                        {
                            primaryColumn = schema.Name;
                        }
                    }
                }
            }

            fieldName = (fieldName.Length > 2) ? fieldName.Substring(2) : fieldName;
            parameter = (parameter.Length > 2) ? parameter.Substring(2) : parameter;
            sqlInsert = string.Format("INSERT INTO {0} ", _tableName);
            sqlInsert += string.Format("( {0} ) ", fieldName);
            sqlInsert += string.Format(" {0} ", "VALUES");
            sqlInsert += string.Format("( {0} ); SELECT SCOPE_IDENTITY()", parameter);

            if (isAuditLog)
                ctx.CommandText = GetInsertAuditLogScript(obj) + "; " + sqlInsert;
            else
                ctx.CommandText = sqlInsert;

            return ctx;
        }

        private string GetUpdateAuditLogScript(object obj)
        {
            StringBuilder auditLogScript = new StringBuilder();
            string auditLogHeaderScript = "";
            if (obj != null && obj is DbDataModel)
            {
                Type type = obj.GetType();
                Hashtable hash = ((DbDataModel) obj).OriginalValue;
                string primaryKeyData = "";
                foreach (DbFieldValue dbValue in GetKeyValues(obj))
                {
                    primaryKeyData +=
                        string.Format(" AND [{0}] = ''{1}''", dbValue.FieldName, dbValue.Current);
                }
                primaryKeyData = primaryKeyData.Substring(5);

                foreach (PropertyInfo prop in type.GetProperties())
                {
                    foreach (Attribute attrib in prop.GetCustomAttributes(false))
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        if (schema != null && !schema.IsPrimaryKey)
                        {
                            object oriValue = hash != null ? hash[schema.Name] : null;
                            object newValue = prop.GetValue(obj, null);
                            if (schema.DataType != "Byte[]")
                                if (schema.IsNullable && IsFieldNullAbleNotAssignValue(newValue, prop.PropertyType))
                                    //(newValue == null && schema.IsNullable && oriValue != null)
                                {
                                    //Update with null
                                    auditLogScript.AppendLine(
                                        @"INSERT INTO AuditLogData (AuditLogTransactionID,ColumnName,OldValueVarchar,NewValueVarchar,DataType) VALUES (@l_AuditLogTransactionID," +
                                        string.Format("'{0}','{1}','NULL','{2}')", schema.Name, oriValue == null ? oriValue : oriValue.ToString().Replace("'", "''"), "A"));
                                }
                                else if (newValue != null && !newValue.Equals(oriValue))
                                {
                                    auditLogScript.AppendLine(
                                        @"INSERT INTO AuditLogData (AuditLogTransactionID,ColumnName,OldValueVarchar,NewValueVarchar,DataType) VALUES (@l_AuditLogTransactionID," +
                                        string.Format("'{0}','{1}','{2}','{3}')", schema.Name, oriValue == null ? oriValue : oriValue.ToString().Replace("'", "''"), newValue == null ? newValue : newValue.ToString().Replace("'", "''"), "A"));
                                }
                        }
                    }
                }
            }

            string auditLogBody = auditLogScript.ToString();
            string retScript;
            if (auditLogBody != "")
                retScript = auditLogHeaderScript + auditLogBody +
                            @"	-- Restore @@IDENTITY VALUE DECLARE @l_maxprec AS VARCHAR(2) SET @l_maxprec = CAST(@@MAX_PRECISION AS VARCHAR(2))
                            EXEC('SELECT IDENTITY(DECIMAL('+@l_maxprec+',0),'+@l_IdentitySave+',1) id INTO #tmp')";
            else
                retScript = "";

            return retScript;
        }

        public IDbContext Update(IDbContext ctx, object obj, bool isAuditLog)
        {
            ctx.CommandText = "";
            if (obj != null && obj is DbDataModel)
            {
                Type type = obj.GetType();
                Hashtable hash = ((DbDataModel) obj).OriginalValue;

                string fields = "";

                foreach (PropertyInfo prop in type.GetProperties())
                {
                    foreach (Attribute attrib in prop.GetCustomAttributes(false))
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        if (schema != null && !schema.IsPrimaryKey)
                        {
                            object oriValue = hash != null ? hash[schema.Name] : null;
                            object newValue = prop.GetValue(obj, null);
                            if (schema.IsNullable && IsFieldNullAbleNotAssignValue(newValue, prop.PropertyType))
                            //(newValue == null && schema.IsNullable && oriValue != null)
                            {
                                //Sql Script Update
                                fields += string.Format(", {0} = null", schema.Name);
                            }
                            else if (newValue != null && !newValue.Equals(oriValue))
                            {
                                //Sql Script Update
                                fields += string.Format(", {0} = {1}{0}", schema.Name, _prefixParameter);
                                //Parameter
                                ctx.Add(string.Format("{0}{1}", _prefixParameter, schema.Name), newValue);
                            }

                            //newValue = CheckIsNull(newValue, prop.PropertyType);
                            //if (newValue.GetType().FullName.Contains("Int64"))
                            //    if (Convert.ToInt64(newValue) == 0)
                            //        newValue = null;
                            //if (newValue == null && schema.IsNullable)
                            //{
                            //    //Sql Script Update
                            //    fields += string.Format(", {0} = null", schema.Name);
                            //}
                            //else if (!newValue.Equals(oriValue))
                            //{
                            //    ////if (newValue.GetType().FullName.Contains("DateTime"))
                            //    ////    if (Convert.ToDateTime(newValue).Year < 1900)
                            //    ////        newValue = new DateTime(1900, 1, 1);
                            //    newValue = CheckIsNull(newValue, prop.PropertyType);
                            //    //Sql Script Update
                            //    fields += string.Format(", {0} = {1}{0}", schema.Name, _prefixParameter);
                            //    //Parameter
                            //    ctx.Add(string.Format("{0}{1}", _prefixParameter, schema.Name), newValue);
                            //}
                        }
                    }
                }
                if (!fields.Equals(string.Empty))
                {
                    string keys = "";
                    foreach (DbFieldValue dbValue in GetKeyValues(obj))
                    {
                        keys += string.Format(" AND {0} = {1}{0}", dbValue.FieldName, _prefixParameter);
                        ctx.Add(string.Format("{0}{1}", _prefixParameter, dbValue.FieldName), dbValue.Current);
                    }

                    string query;
                    query = string.Format("UPDATE {0} SET ", _tableName);
                    query += string.Format("{0} ", fields.Substring(2));
                    query += string.Format("WHERE {0} ", keys.Substring(5));

                    if (isAuditLog)
                        ctx.CommandText = GetUpdateAuditLogScript(obj) + "; " + query;
                    else
                        ctx.CommandText = query;

                    //if (!_tableName.ToLower().Contains("systranslation"))
                    //{
                    //    SiAuto.Main.LogSql("Update " + _tableName, ctx.CommandText);
                    //    SiAuto.Main.LogObject(_tableName, obj);
                    //}
                }
            }


            return ctx;
        }

        //public string Delete()
        //{
        //    string result;
        //    result = string.Format("DELETE {0} ", _tableName);
        //    result += string.Format("{0} ", WhereParameter);

        //    SiAuto.Main.LogSql("Delete " + _tableName, result);
        //    return result;
        //}

        private string GetDeleteAuditLogScript(object obj)
        {
            StringBuilder auditLogScript = new StringBuilder();
            string auditLogHeaderScript = "";
            if (obj != null && obj is DbDataModel)
            {
                Type type = obj.GetType();
                Hashtable hash = ((DbDataModel) obj).OriginalValue;

                string primaryKeyData = "";
                foreach (DbFieldValue dbValue in GetKeyValues(obj))
                {
                    primaryKeyData +=
                        string.Format(" AND [{0}] = ''{1}''", dbValue.FieldName, dbValue.Current);
                }
                primaryKeyData = primaryKeyData.Substring(5);

                foreach (PropertyInfo prop in type.GetProperties())
                {
                    foreach (Attribute attrib in prop.GetCustomAttributes(false))
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        object newValue = prop.GetValue(obj, null);
                        auditLogScript.AppendLine(
                            @"INSERT INTO AuditLogData (AuditLogTransactionID,ColumnName,OldValueVarchar,NewValueVarchar,DataType) VALUES ( @l_AuditLogTransactionID," +
                            string.Format("'{0}','{1}','{2}','{3}')", schema.Name, "", newValue == null ? newValue : newValue.ToString().Replace("'", "''"), "A"));
                    }
                }
            }

            string auditLogBody = auditLogScript.ToString();
            string retScript;
            if (auditLogBody != "")
                retScript = auditLogHeaderScript + auditLogBody +
                            @"	-- Restore @@IDENTITY VALUES DECLARE @l_maxprec AS VARCHAR(2) SET @l_maxprec = CAST(@@MAX_PRECISION AS VARCHAR(2))
                            EXEC('SELECT IDENTITY(decimal('+@l_maxprec+',0),'+@l_IdentitySave+',1) id INTO #tmp')";
            else
                retScript = "";

            return retScript;
        }

        public IDbContext Delete(IDbContext ctx, object obj, bool isAuditLog)
        {
            ctx.CommandText = "";
            if (obj != null && obj is DbDataModel)
            {
                string keys = "";
                foreach (DbFieldValue dbValue in GetKeyValues(obj))
                {
                    keys += string.Format(" AND {0} = {1}{0}", dbValue.FieldName, _prefixParameter);
                    ctx.Add(string.Format("{0}{1}", _prefixParameter, dbValue.FieldName), dbValue.Current);
                }
                if (!keys.Equals(string.Empty))
                {
                    string query;
                    query = string.Format("DELETE {0} ", _tableName);
                    query += string.Format("WHERE {0} ", keys.Substring(5));

                    if (isAuditLog)
                        ctx.CommandText = GetDeleteAuditLogScript(obj) + "; " + query;
                    else
                        ctx.CommandText = query;

                    //if (!_tableName.ToLower().Contains("systranslation"))
                    //{
                    //    SiAuto.Main.LogSql("Delete " + _tableName, ctx.CommandText);
                    //    SiAuto.Main.LogObject(_tableName, obj);
                    //}
                }
            }
            return ctx;
        }

        public IDbContext DeleteAll(IDbContext ctx, object obj, bool isAuditLog)
        {
            ctx.CommandText = "";
            if (obj != null && obj is DbDataModel)
            {
                string keys = "";
                foreach (DbFieldValue dbValue in GetKeyValues(obj))
                {
                    keys += string.Format(" AND {0} = {1}{0}", dbValue.FieldName, _prefixParameter);
                    ctx.Add(string.Format("{0}{1}", _prefixParameter, dbValue.FieldName), dbValue.Current);
                }

                string query;
                query = string.Format("DELETE {0} ", _tableName);

                if (isAuditLog)
                    ctx.CommandText = GetDeleteAuditLogScript(obj) + "; " + query;
                else
                    ctx.CommandText = query;
            }
            return ctx;
        }

        public string GetRowIndex(string filterExpression, string keyField, string keyValue, string orderByExpression)
        {
            //string result = string.Format("SELECT al.RowIndex FROM (
            //SELECT ROW_NUMBER() OVER (ORDER BY RegistrationID DESC) AS RowIndex, RegistrationNo
            //FROM Registration) al WHERE RegistrationNo = 'REG/20130419/00002'", _tableName);
            
            if (filterExpression != "")
                filterExpression = " WHERE " + filterExpression;
            if (orderByExpression == null || orderByExpression == "")
                orderByExpression = "(SELECT 0)";

            string mainQry = string.Format("SELECT a.row FROM (SELECT ROW_NUMBER() OVER (ORDER BY {0}) - 1 AS ROW, {2} FROM {1}{4}) a WHERE {2} = '{3}'", orderByExpression, _tableName, keyField, keyValue, filterExpression);
            string fixQry = string.Format("IF EXISTS ({0}) BEGIN {0} END ELSE BEGIN SELECT 0 END", mainQry);

            return fixQry;
        }

        public string GetRowCount(string filterExpression, params object[] args)
        {
            string result = string.Format("SELECT COUNT(*) FROM {0} ", _tableName);
            if (filterExpression != null && filterExpression.Trim().Length > 0)
            {
                result += string.Format("WHERE {0}", string.Format(filterExpression, args));
            }
            return result;
        }

        public string GetRowCountByFieldName(string filterExpression, string fieldName, params object[] args)
        {
            string result = string.Format("SELECT COUNT({1}) FROM {0} ", _tableName, fieldName);
            if (filterExpression != null && filterExpression.Trim().Length > 0)
            {
                result += string.Format("WHERE {0}", string.Format(filterExpression, args));
            }
            return result;
        }

        public string Select()
        {
            return string.Format("SELECT * FROM {0} ", _tableName);
        }

        public string Select(string filterExpression, int numRows, int pageIndex, string orderByExpression)
        {
            if (filterExpression != "")
                filterExpression = " WHERE " + filterExpression;
            if (pageIndex == 1)
            {
                if (orderByExpression != "")
                {
                    if (filterExpression == "")
                        filterExpression = " WHERE 1 = 1";
                    filterExpression += " ORDER BY " + orderByExpression;
                }
                return string.Format("SELECT TOP({0})* FROM {1}{2}", numRows, _tableName, filterExpression);
            }
            int startIndex = (pageIndex - 1) * numRows;
            if (orderByExpression == null || orderByExpression == "")
                orderByExpression = "(SELECT 0)";

            startIndex++;
            int endIndex = startIndex + numRows - 1;
            //return string.Format("SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) - 1 as row FROM {1}{4}) a WHERE a.row >= {2} and a.row < {3}", orderByExpression, _tableName, startIndex, endIndex, filterExpression);
            return string.Format("WITH mytable AS (SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) AS 'RowNumber' FROM {1}{4}) SELECT * FROM myTable WHERE RowNumber BETWEEN {2} AND {3}", orderByExpression, _tableName, startIndex, endIndex, filterExpression);
            //return string.Format("SELECT * FROM {0} ", _tableName);
        }

        public string SelectByPageIndex(string filterExpression, int pageIndex, string orderByColumn)
        {
            if (filterExpression != "")
                filterExpression = " WHERE " + filterExpression;
            if (orderByColumn == null || orderByColumn == "")
                orderByColumn = "(SELECT 0)";
            pageIndex++;
            //return string.Format("SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) - 1 as row FROM {1}{3}) a WHERE a.row = {2}", orderByColumn, _tableName, pageIndex, filterExpression);
            return string.Format("WITH mytable AS (SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) AS 'RowNumber' FROM {1}{3}) SELECT * FROM myTable WHERE RowNumber = {2}", orderByColumn, _tableName, pageIndex, filterExpression);
            //return string.Format("SELECT * FROM {0} ", _tableName);
        }

        public string SelectColumn(string columnName, string filterExpression, params object[] args)
        {
            string result = string.Format("SELECT {0} FROM {1} ", columnName, _tableName);
            if (filterExpression != null && filterExpression.Trim().Length > 0)
            {
                result += string.Format(" WHERE {0}", string.Format(filterExpression, args));
            }
            return result;
        }

        public string Select(string filterExpression, params object[] args)
        {
            string result = string.Format("SELECT * FROM {0} ", _tableName);
            if (filterExpression != null && filterExpression.Trim().Length > 0)
            {
                result += string.Format(" WHERE {0}", string.Format(filterExpression, args));
            }
            return result;
        }

        public string SelectMaxColumn(string columnName, string filterExpression = "")
        {
            string whereStmt = "";
            if (filterExpression != null && filterExpression.Trim().Length > 0)
                whereStmt = string.Format(" WHERE {0} ", filterExpression);
            string result = string.Format("SELECT TOP(1) {0} FROM {1} {2} ORDER BY {0} DESC", columnName, _tableName, whereStmt);

            return result;
        }

        public string SelectTop(string filterExpression, int maxRow, params object[] args)
        {
            string result = string.Format("SELECT TOP {0} * FROM {1} ", maxRow, _tableName);
            if (filterExpression != null && filterExpression.Trim().Length > 0)
            {
                result += string.Format(" WHERE {0}", string.Format(filterExpression, args));
            }
            return result;
        }

        public string GetRecord()
        {
            string result;
            result = string.Format("SELECT * FROM {0} ", _tableName);
            result += string.Format("{0} ", WhereParameter);
            return result;
        }

        #endregion

        #region Public Method

        public string TableName
        {
            get { return _tableName; }
        }

        public object CheckIsNull(object obj)
        {
            if (obj == null) return null;
            Type type = obj.GetType();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                foreach (Attribute attrib in prop.GetCustomAttributes(true))
                {
                    ColumnAttribute schema = attrib as ColumnAttribute;
                    if (schema != null && !schema.IsNullable)
                    {
                        prop.SetValue(obj, CheckIsNull(prop.GetValue(obj, null), prop.PropertyType), null);
                    }
                }
            }
            return obj;
        }

        public object DataRowToObject(DataRow row, object obj)
        {
            Hashtable hash = new Hashtable();
            if (obj == null) return null;
            Type type = obj.GetType();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                foreach (Attribute attrib in prop.GetCustomAttributes(true))
                {
                    ColumnAttribute schema = attrib as ColumnAttribute;
                    if (schema != null)
                    {
                        object value = CheckIsNull(row[schema.Name], prop.PropertyType);
                        prop.SetValue(obj, value, null);
                        hash.Add(schema.Name, value);
                    }
                }
            }

            if (obj is DbDataModel)
            {
                ((DbDataModel) obj).OriginalValue = hash;
            }

            return obj;
        }

        public object IDataReaderToObject(IDataReader reader, object obj)
        {
            Hashtable hash = new Hashtable();
            if (obj == null) return null;
            Type type = obj.GetType();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                foreach (Attribute attrib in prop.GetCustomAttributes(false))
                {
                    ColumnAttribute schema = attrib as ColumnAttribute;
                    if (schema != null)
                    {
                        object value = CheckIsNull(reader[schema.Name], prop.PropertyType);
                        prop.SetValue(obj, value, null);
                        hash.Add(schema.Name, value);
                    }
                }
            }

            if (obj is DbDataModel)
            {
                ((DbDataModel) obj).OriginalValue = hash;
            }
            return obj;
        }


        public List<DbFieldValue> GetModifValues(object obj)
        {
            List<DbFieldValue> list = new List<DbFieldValue>();
            if (obj != null && obj is DbDataModel)
            {
                Type type = obj.GetType();
                Hashtable hash = ((DbDataModel) obj).OriginalValue;


                foreach (PropertyInfo prop in type.GetProperties())
                {
                    foreach (Attribute attrib in prop.GetCustomAttributes(false))
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        if (schema != null && !schema.IsPrimaryKey)
                        {
                            //object oriValue = hash != null ? hash[schema.Name] : null;
                            //object newValue = prop.GetValue(obj, null);
                            //if ((newValue == null && schema.IsNullable && oriValue != null) ||
                            //    (newValue != null && !newValue.Equals(oriValue)))
                            //{
                            //    list.Add(new DbFieldValue(schema.Name, oriValue, newValue));
                            //}

                            object oriValue = hash != null ? hash[schema.Name] : null;
                            object newValue = prop.GetValue(obj, null);
                            if (!newValue.Equals(oriValue))
                            {
                                list.Add(new DbFieldValue(schema.Name, oriValue, newValue));
                            }
                        }
                    }
                }
            }
            return list;
        }

        public List<DbFieldValue> GetKeyValues(object obj)
        {
            List<DbFieldValue> list = new List<DbFieldValue>();
            if (obj != null && obj is DbDataModel)
            {
                Type type = obj.GetType();
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    foreach (Attribute attrib in prop.GetCustomAttributes(true))
                    {
                        ColumnAttribute schema = attrib as ColumnAttribute;
                        if (schema != null && schema.IsPrimaryKey)
                        {
                            object newValue = prop.GetValue(obj, null);
                            list.Add(new DbFieldValue(schema.Name, newValue, true));
                        }
                    }
                }
            }
            return list;
        }

        #endregion

        #region Private Method and Properties

        private string UpdateFieldParameter
        {
            get
            {
                string fields = "";
                foreach (ColumnAttribute schema in _listSchema)
                {
                    if (!schema.IsPrimaryKey
                        || !schema.IsComputed
                        || !schema.IsIdentity
                        || !schema.IsTimeStamp)
                    {
                        fields += string.Format(", {0} = {1}{0}", schema.Name, _prefixParameter);
                    }
                }
                return (fields.Length > 2 ? fields.Substring(2) : "");
            }
        }

        private string WhereParameter
        {
            get
            {
                string fields = "";
                foreach (ColumnAttribute schema in _listSchema)
                {
                    if (schema.IsPrimaryKey)
                    {
                        fields += string.Format(" AND {0} = {1}{0}", schema.Name, _prefixParameter);
                    }
                }
                return (fields.Length > 5 ? string.Format(" WHERE {0}", fields.Substring(5)) : "");
            }
        }

        private static bool IsFieldNullAbleNotAssignValue(object value, Type type)
        {
            //Mengecek field type tertentu tidak diisi
            if (value != null)
            {
                switch (type.Name)
                {
                    case "String":
                        return value.ToString().Trim().Equals(string.Empty);
                    //case "Int16":
                    //    return value.Equals((Int16) 0);
                    //case "Int32":
                    //    return value.Equals(0);
                    //case "Int64":
                    //    return value.Equals((Int64) 0);
                    //case "Double":
                    //    return value.Equals((Double) 0);
                    //case "Decimal":
                    //    return value.Equals((Decimal) 0);
                    case "DateTime":
                        return Convert.ToDateTime(value).Year < 1900;
                }
                return false;
            }
            else
                return true;
        }

        private static object CheckIsNull(object obj, Type type)
        {
            //if (type.FullName.Contains("DateTime"))
            //{
            //    if (obj is DBNull || obj == null)
            //        return Convert.ToDateTime("1900-01-01");
            //    if (Convert.ToDateTime(obj).Year < 1900)
            //        return Convert.ToDateTime("1900-01-01");
            //}
            //else if (obj is DBNull || obj == null)
            //{
            //    if (type.FullName.Contains("String")) return string.Empty;
            //    if (type.FullName.Contains("Int16")) return 0;
            //    if (type.FullName.Contains("Int32")) return 0;
            //    if (type.FullName.Contains("Int64")) return 0;
            //    if (type.FullName.Contains("Boolean")) return false;
            //    if (type.FullName.Contains("Double")) return Double.NaN;
            //    if (type.FullName.Contains("Decimal")) return Decimal.Zero;
            //    if (type.FullName.Contains("DateTime")) return Convert.ToDateTime("1900-01-01");
            //    if (type.FullName.Contains("Byte")) return Byte.MinValue;
            //    if (type.FullName.Contains("Byte[]")) return new Byte[] { };
            //}

            if (type.FullName.Contains("DateTime"))
            {
                if (obj is DBNull || obj == null)
                    return Convert.ToDateTime("1900-01-01");
                if (Convert.ToDateTime(obj).Year < 1900)
                    return Convert.ToDateTime("1900-01-01");
            }
            else if (obj is DBNull || obj == null)
            {
                if (type.FullName.Contains("String")) return string.Empty;
                //if (type.FullName.Contains("Int64")) return 0;
                if (type.FullName.Contains("Boolean")) return false;
                return null;
            }
            return obj;
        }

        #endregion
    }
}