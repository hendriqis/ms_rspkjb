using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace QIS.Data.Core.Dal
{
    public static class DaoBase
    {

        public static int ExecuteNonQuery(IDbContext ctx, bool checkQuery)
        {
            if (checkQuery && ctx.CommandText.Trim().Equals(string.Empty))
            {
                return 0;
            }
            return ExecuteNonQuery(ctx);
        }

        public static int ExecuteNonQuery(IDbContext ctx)
        {
            bool isNullTransaction=false;
            // Selalu pakai transaction untuk mencegah record ditable lain di update bila ada proses yg gagal
            if (ctx.Transaction == null)
            {
                isNullTransaction = true;
                ctx.Command.Transaction = ctx.Command.Connection.BeginTransaction();
            }
            int result;



            try
            {
                result = ctx.Command.ExecuteNonQuery();
                if (isNullTransaction)
                    ctx.Command.Transaction.Commit();
            }
            catch (SqlException ex)
            {
                if (isNullTransaction)
                    ctx.Command.Transaction.Rollback();
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (isNullTransaction)
                    ctx.Command.Transaction.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (isNullTransaction)
                    ctx.Command.Transaction = null;
                ctx.Close();
            }
            return result;
        }

        public static DataSet GetDataSet(IDbContext ctx)
        {
            DataSet ds = new DataSet();
            IDataAdapter da = ctx.DataAdapter;

            try
            {
                da.Fill(ds);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return ds;
        }

        public static DataTable GetDataTable(IDbContext ctx)
        {
            return GetDataSet(ctx).Tables[0];
        }

        public static DataRow GetDataRow(IDbContext ctx)
        {
            DataTable dt = GetDataTable(ctx);
            return (dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        public static IDataReader GetDataReader(IDbContext ctx)
        {
            IDataReader idr;
            try
            {
                idr = ctx.Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Clear();
            }
            return idr;
        }

        //public static object ExecuteScalar(IDbContext ctx)
        //{
        //    object result;
        //    try
        //    {
        //        result = ctx.Command.ExecuteScalar();
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //    return result;
        //}

        public static int ExecuteScalar(IDbContext ctx)
        {
            bool isNullTransaction = false;
            // Selalu pakai transaction untuk mencegah record ditable lain di update bila ada proses yg gagal
            if (ctx.Transaction == null)
            {
                isNullTransaction = true;
                ctx.Command.Transaction = ctx.Command.Connection.BeginTransaction();
            }

            int result;

            try
            {
                result = Convert.ToInt32(ctx.Command.ExecuteScalar());
                if (isNullTransaction)
                    ctx.Command.Transaction.Commit();
            }
            catch (SqlException ex)
            {
                if (isNullTransaction)
                    ctx.Command.Transaction.Rollback();
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (isNullTransaction)
                    ctx.Command.Transaction.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (isNullTransaction)
                    ctx.Command.Transaction = null;
                ctx.Close();
            }
            return result;
        }
    }
}