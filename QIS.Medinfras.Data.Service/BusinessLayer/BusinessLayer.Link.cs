using System;
using System.Collections.Generic;
using System.Data;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Data.Service
{
    public static partial class BusinessLayer
    {
        #region Medinfrasv1.1 Views
        #region vOrderResep
        public static List<vOrderResep> GetvOrderResepList(string filterExpression)
        {
            List<vOrderResep> result = new List<vOrderResep>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vOrderResep));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vOrderResep)helper.IDataReaderToObject(reader, new vOrderResep()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vEmergencyPatientListLink
        public static List<vEmergencyPatientListLink> GetvEmergencyPatientListLinkList(string filterExpression)
        {
            List<vEmergencyPatientListLink> result = new List<vEmergencyPatientListLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vEmergencyPatientListLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vEmergencyPatientListLink)helper.IDataReaderToObject(reader, new vEmergencyPatientListLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vInpatientAllPatientLink
        public static List<vInpatientAllPatientLink> GetvInpatientAllPatientLinkList(string filterExpression)
        {
            List<vInpatientAllPatientLink> result = new List<vInpatientAllPatientLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vInpatientAllPatientLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vInpatientAllPatientLink)helper.IDataReaderToObject(reader, new vInpatientAllPatientLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vInpatientClassLink
        public static List<vInpatientClassLink> GetvInpatientClassLinkList(string filterExpression)
        {
            List<vInpatientClassLink> result = new List<vInpatientClassLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vInpatientClassLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vInpatientClassLink)helper.IDataReaderToObject(reader, new vInpatientClassLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vInpatientPatientListLink
        public static List<vInpatientPatientListLink> GetvInpatientPatientListLinkList(string filterExpression, IDbContext ctx)
        {
            List<vInpatientPatientListLink> result = new List<vInpatientPatientListLink>();
            try
            {
                DbHelper helper = new DbHelper(typeof(vInpatientPatientListLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vInpatientPatientListLink)helper.IDataReaderToObject(reader, new vInpatientPatientListLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }
        public static List<vInpatientPatientListLink> GetvInpatientPatientListLinkList(string filterExpression)
        {
            List<vInpatientPatientListLink> result = new List<vInpatientPatientListLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vInpatientPatientListLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vInpatientPatientListLink)helper.IDataReaderToObject(reader, new vInpatientPatientListLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        public static List<vInpatientPatientListLink> GetvInpatientPatientListLinkList(string filterExpression, int numRows, int pageIndex, string orderByExpression = "")
        {
            List<vInpatientPatientListLink> result = new List<vInpatientPatientListLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vInpatientPatientListLink));
                ctx.CommandText = helper.Select(filterExpression, numRows, pageIndex, orderByExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vInpatientPatientListLink)helper.IDataReaderToObject(reader, new vInpatientPatientListLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        public static Int32 GetvInpatientPatientListLinkRowCount(string filterExpression)
        {
            Int32 result = 0;
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vConsultVisit));
                ctx.CommandText = helper.GetRowCount(filterExpression);
                DataRow row = DaoBase.GetDataRow(ctx);
                result = Convert.ToInt32(row.ItemArray.GetValue(0));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vInpatientServiceUnitLink
        public static List<vInpatientServiceUnitLink> GetvInpatientServiceUnitLinkList(string filterExpression)
        {
            List<vInpatientServiceUnitLink> result = new List<vInpatientServiceUnitLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vInpatientServiceUnitLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vInpatientServiceUnitLink)helper.IDataReaderToObject(reader, new vInpatientServiceUnitLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vInpatientServiceUnitLinkPerUser
        public static List<vInpatientServiceUnitLinkPerUser> GetvInpatientServiceUnitLinkPerUserList(string filterExpression)
        {
            List<vInpatientServiceUnitLinkPerUser> result = new List<vInpatientServiceUnitLinkPerUser>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vInpatientServiceUnitLinkPerUser));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vInpatientServiceUnitLinkPerUser)helper.IDataReaderToObject(reader, new vInpatientServiceUnitLinkPerUser()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vOutPatientPatientListLink
        public static List<vOutPatientPatientListLink> GetvOutPatientPatientListLinkList(string filterExpression)
        {
            List<vOutPatientPatientListLink> result = new List<vOutPatientPatientListLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vOutPatientPatientListLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vOutPatientPatientListLink)helper.IDataReaderToObject(reader, new vOutPatientPatientListLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vOutPatientServiceUnitLinkPerUser
        public static List<vOutPatientServiceUnitLinkPerUser> GetvOutPatientServiceUnitLinkPerUserList(string filterExpression)
        {
            List<vOutPatientServiceUnitLinkPerUser> result = new List<vOutPatientServiceUnitLinkPerUser>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vOutPatientServiceUnitLinkPerUser));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vOutPatientServiceUnitLinkPerUser)helper.IDataReaderToObject(reader, new vOutPatientServiceUnitLinkPerUser()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vParamedicMasterLink
        public static List<vParamedicMasterLink> GetvParamedicMasterLinkList(string filterExpression)
        {
            List<vParamedicMasterLink> result = new List<vParamedicMasterLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vParamedicMasterLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vParamedicMasterLink)helper.IDataReaderToObject(reader, new vParamedicMasterLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vPatientListLink
        public static List<vPatientListLink> GetvPatientListLinkList(string filterExpression)
        {
            List<vPatientListLink> result = new List<vPatientListLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vPatientListLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vPatientListLink)helper.IDataReaderToObject(reader, new vPatientListLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vRegistrationListLink
        public static List<vRegistrationListLink> GetvRegistrationListLinkList(string filterExpression)
        {
            List<vRegistrationListLink> result = new List<vRegistrationListLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vRegistrationListLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vRegistrationListLink)helper.IDataReaderToObject(reader, new vRegistrationListLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vServiceUnitLink
        public static List<vServiceUnitLink> GetvServiceUnitLinkList(string filterExpression)
        {
            List<vServiceUnitLink> result = new List<vServiceUnitLink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vServiceUnitLink));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vServiceUnitLink)helper.IDataReaderToObject(reader, new vServiceUnitLink()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region vServiceUnitLinkPerUser
        [Obsolete("This method is obsolete, used a new method GetvlnkServiceUnitPerUserList", false)]
        public static List<vServiceUnitLinkPerUser> GetvServiceUnitLinkPerUserList(string filterExpression)
        {
            List<vServiceUnitLinkPerUser> result = new List<vServiceUnitLinkPerUser>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vServiceUnitLinkPerUser));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vServiceUnitLinkPerUser)helper.IDataReaderToObject(reader, new vServiceUnitLinkPerUser()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        #region vlnkServiceUnitPerUser
        public static List<vlnk_ServiceUnitPerUser> GetvlnkServiceUnitPerUserList(string filterExpression)
        {
            List<vlnk_ServiceUnitPerUser> result = new List<vlnk_ServiceUnitPerUser>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(vlnk_ServiceUnitPerUser));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((vlnk_ServiceUnitPerUser)helper.IDataReaderToObject(reader, new vlnk_ServiceUnitPerUser()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #endregion

        #region MedinfrasLink
        #region RegistrationBpjsReport
        public static RegistrationBpjsReport GetRegistrationBpjsReport(Int32 ID)
        {
            return new RegistrationBpjsReportDao().Get(ID);
        }
        public static int InsertRegistrationBpjsReport(RegistrationBpjsReport record)
        {
            return new RegistrationBpjsReportDao().Insert(record);
        }
        public static int UpdateRegistrationBpjsReport(RegistrationBpjsReport record)
        {
            return new RegistrationBpjsReportDao().Update(record);
        }
        public static int DeleteRegistrationBpjsReport(Int32 ID)
        {
            return new RegistrationBpjsReportDao().Delete(ID);
        }
        public static List<RegistrationBpjsReport> GetRegistrationBpjsReportList(string filterExpression)
        {
            List<RegistrationBpjsReport> result = new List<RegistrationBpjsReport>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(RegistrationBpjsReport));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((RegistrationBpjsReport)helper.IDataReaderToObject(reader, new RegistrationBpjsReport()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region ARInvoiceReportDt
        public static ARInvoiceReportDt GetARInvoiceReportDt(Int32 ID)
        {
            return new ARInvoiceReportDtDao().Get(ID);
        }
        public static int InsertARInvoiceReportDt(ARInvoiceReportDt record)
        {
            return new ARInvoiceReportDtDao().Insert(record);
        }
        public static int UpdateARInvoiceReportDt(ARInvoiceReportDt record)
        {
            return new ARInvoiceReportDtDao().Update(record);
        }
        public static int DeleteARInvoiceReportDt(Int32 ID)
        {
            return new ARInvoiceReportDtDao().Delete(ID);
        }
        public static List<ARInvoiceReportDt> GetARInvoiceReportDtList(string filterExpression)
        {
            List<ARInvoiceReportDt> result = new List<ARInvoiceReportDt>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(ARInvoiceReportDt));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((ARInvoiceReportDt)helper.IDataReaderToObject(reader, new ARInvoiceReportDt()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region ARInvoiceReportHD
        public static ARInvoiceReportHD GetARInvoiceReportHD(Int32 ID)
        {
            return new ARInvoiceReportHDDao().Get(ID);
        }
        public static int InsertARInvoiceReportHD(ARInvoiceReportHD record)
        {
            return new ARInvoiceReportHDDao().Insert(record);
        }
        public static int UpdateARInvoiceReportHD(ARInvoiceReportHD record)
        {
            return new ARInvoiceReportHDDao().Update(record);
        }
        public static int DeleteARInvoiceReportHD(Int32 ID)
        {
            return new ARInvoiceReportHDDao().Delete(ID);
        }
        public static List<ARInvoiceReportHD> GetARInvoiceReportHDList(string filterExpression)
        {
            List<ARInvoiceReportHD> result = new List<ARInvoiceReportHD>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(ARInvoiceReportHD));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((ARInvoiceReportHD)helper.IDataReaderToObject(reader, new ARInvoiceReportHD()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #endregion
    }
}