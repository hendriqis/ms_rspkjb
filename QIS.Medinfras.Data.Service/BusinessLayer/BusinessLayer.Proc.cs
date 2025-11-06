using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Data.Service
{
    public static partial class BusinessLayer
    {
        #region InsertUpdatePrimaryRegistrationPayer
        public static void InsertUpdatePrimaryRegistrationPayer(RegistrationPayer entity, Int32 RegistrationID, string param, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "InsertUpdatePrimaryRegistrationPayer";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RegistrationPayerID", entity.ID));
            ctx.Command.Parameters.Add(new SqlParameter("@RegistrationID", entity.RegistrationID));
            ctx.Command.Parameters.Add(new SqlParameter("@BusinessPartnerID", entity.BusinessPartnerID));
            ctx.Command.Parameters.Add(new SqlParameter("@GCCustomerType", entity.GCCustomerType));
            ctx.Command.Parameters.Add(new SqlParameter("@ContractID", entity.ContractID));
            ctx.Command.Parameters.Add(new SqlParameter("@CoverageTypeID", entity.CoverageTypeID));
            ctx.Command.Parameters.Add(new SqlParameter("@CoverageLimitAmount", entity.CoverageLimitAmount));
            ctx.Command.Parameters.Add(new SqlParameter("@CorporateAccountNo", entity.CorporateAccountNo));
            ctx.Command.Parameters.Add(new SqlParameter("@CorporateAccountName", entity.CorporateAccountName));
            ctx.Command.Parameters.Add(new SqlParameter("@IsCoverageLimitPerDay", entity.IsCoverageLimitPerDay));
            ctx.Command.Parameters.Add(new SqlParameter("@ControlClassID", entity.ControlClassID));
            ctx.Command.Parameters.Add(new SqlParameter("@IsPrimaryPayer", entity.IsPrimaryPayer));
            ctx.Command.Parameters.Add(new SqlParameter("@LastUpdatedBy", entity.LastUpdatedBy));
            ctx.Command.Parameters.Add(new SqlParameter("@param", param));
            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

        }
        #endregion

        #region FillStockTakingDt
        public static void FillStockTakingDt(Int32 StockTakingID, Int32 LocationID, String ABCClass, String ItemType, Int32 ProductLineID, DateTime Date, String Time, int UserID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "FillStockTakingDt";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@StockTakingID", StockTakingID));
            ctx.Command.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            ctx.Command.Parameters.Add(new SqlParameter("@ABCClass", ABCClass));
            ctx.Command.Parameters.Add(new SqlParameter("@ItemType", ItemType));
            ctx.Command.Parameters.Add(new SqlParameter("@ProductLineID", ProductLineID));
            ctx.Command.Parameters.Add(new SqlParameter("@Date", Date));
            ctx.Command.Parameters.Add(new SqlParameter("@Time", Time));
            ctx.Command.Parameters.Add(new SqlParameter("@UserID", UserID));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
        }
        #endregion
        #region FillStockTakingDt1
        public static void FillStockTakingDt1(Int32 StockTakingID, Int32 LocationID, String ABCClass, String ItemType, Int32 ProductLineID, DateTime Date, String Time, int UserID, int binLocationID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "FillStockTakingDt1";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@StockTakingID", StockTakingID));
            ctx.Command.Parameters.Add(new SqlParameter("@LocationID", LocationID));
            ctx.Command.Parameters.Add(new SqlParameter("@ABCClass", ABCClass));
            ctx.Command.Parameters.Add(new SqlParameter("@ItemType", ItemType));
            ctx.Command.Parameters.Add(new SqlParameter("@ProductLineID", ProductLineID));
            ctx.Command.Parameters.Add(new SqlParameter("@Date", Date));
            ctx.Command.Parameters.Add(new SqlParameter("@Time", Time));
            ctx.Command.Parameters.Add(new SqlParameter("@UserID", UserID));
            ctx.Command.Parameters.Add(new SqlParameter("@BinLocationID", binLocationID));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
        }
        #endregion
        #region InsertDataToStaging
        public static void InsertDataToStaging(DateTime FromDate, DateTime ToDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "InsertDataToStaging";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@FromDate", FromDate));
            ctx.Command.Parameters.Add(new SqlParameter("@ToDate", ToDate));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
        }
        #endregion
        #region GenerateFADepreciation
        public static void GenerateFADepreciation(int FixedAssetID, int CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateFADepreciation";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@FixedAssetID", FixedAssetID));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
        }
        #endregion
        #region GenerateParamedicRevenueSharing
        public static string GenerateParamedicRevenueSharing(DateTime RevenueSharingDate, int ParamedicID, string DepartmentID, string GCReduction, string GCPaymentMethod, string GCClinicGroup, string GCPeriodeType, DateTime PeriodeDateStart, DateTime PeriodeDateEnd, string lstTransactionDtID, string lstTransactionDtItemID, string lstRemarksDt, int CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateParamedicRevenueSharing";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RevenueSharingDate", RevenueSharingDate));
            ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", ParamedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
            ctx.Command.Parameters.Add(new SqlParameter("@GCReduction", GCReduction));
            ctx.Command.Parameters.Add(new SqlParameter("@GCPaymentMethod", GCPaymentMethod));
            ctx.Command.Parameters.Add(new SqlParameter("@GCClinicGroup", GCClinicGroup));
            ctx.Command.Parameters.Add(new SqlParameter("@GCPeriodeType", GCPeriodeType));
            ctx.Command.Parameters.Add(new SqlParameter("@PeriodeDateStart", PeriodeDateStart));
            ctx.Command.Parameters.Add(new SqlParameter("@PeriodeDateEnd", PeriodeDateEnd));
            ctx.Command.Parameters.Add(new SqlParameter("@lstTransactionDtID", lstTransactionDtID));
            ctx.Command.Parameters.Add(new SqlParameter("@lstTransactionDtItemID", lstTransactionDtItemID));
            ctx.Command.Parameters.Add(new SqlParameter("@lstRemarksDt", lstRemarksDt));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@RevenueSharingNo";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 20;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion
        #region GenerateParamedicRevenueSharingFromUpload
        public static string GenerateParamedicRevenueSharingFromUpload(int ParamUpload, string GCReduction, string GCPaymentMethod, int CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateParamedicRevenueSharingFromUpload";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@ParamUpload", ParamUpload));
            ctx.Command.Parameters.Add(new SqlParameter("@GCReduction", GCReduction));
            ctx.Command.Parameters.Add(new SqlParameter("@GCPaymentMethod", GCPaymentMethod));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@RevenueSharingNo";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = int.MaxValue;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion
        #region GenerateParamedicRevenueSharingPerRegistration
        public static string GenerateParamedicRevenueSharingPerRegistration(DateTime RevenueSharingDate, string DepartmentID, string GCReduction, string GCPaymentMethod, string GCClinicGroup, string lstMemberID, int CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateParamedicRevenueSharingPerRegistration";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RevenueSharingDate", RevenueSharingDate));
            ctx.Command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
            ctx.Command.Parameters.Add(new SqlParameter("@GCReduction", GCReduction));
            ctx.Command.Parameters.Add(new SqlParameter("@GCPaymentMethod", GCPaymentMethod));
            ctx.Command.Parameters.Add(new SqlParameter("@GCClinicGroup", GCClinicGroup));
            ctx.Command.Parameters.Add(new SqlParameter("@lstMemberID", lstMemberID));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@ResultOutput";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 20;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion
        #region GenerateReferenceQueueNo
        public static string GenerateReferenceQueueNo(DateTime queueDate, Int32 healthcareServiceUnitID, Int32 paramedicID, String gcCustomerType)
        {
            return GenerateReferenceQueueNo(queueDate, healthcareServiceUnitID, paramedicID, gcCustomerType, null);
        }
        public static string GenerateReferenceQueueNo(DateTime queueDate, Int32 healthcareServiceUnitID, Int32 paramedicID, String gcCustomerType, IDbContext ctx = null, Int32 IsGoshow = 0)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateReferenceQueueNo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@QueueDate", queueDate));
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", healthcareServiceUnitID));
            ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", paramedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@GCCustomerType", gcCustomerType));
            ctx.Command.Parameters.Add(new SqlParameter("@IsGoshow", IsGoshow));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion
        #region GenerateRevenueSharingAdjustment
        public static string GenerateRevenueSharingAdjustment(int RSTransactionID, int CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateRevenueSharingAdjustment";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RSTransactionID", RSTransactionID));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@RevenueSharingNo";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 20;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion
        #region GenerateRevenueSharingSummaryAdjustment
        public static string GenerateRevenueSharingSummaryAdjustment(int RSSummaryID, int NewParamedicID, int CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateRevenueSharingSummaryAdjustment";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RSSummaryID", RSSummaryID));
            ctx.Command.Parameters.Add(new SqlParameter("@NewParamedicID", NewParamedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@RSSummaryNo";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 20;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion
        #region GenerateTransactionNo
        public static string GenerateTransactionNo(string transactionCode, DateTime transactionDate)
        {
            return GenerateTransactionNo(transactionCode, transactionDate, "", null);
        }
        public static string GenerateTransactionNo(string transactionCode, DateTime transactionDate, String transactionInitial = "", IDbContext ctx = null)
        {
            return GenerateTransactionNo(transactionCode, transactionDate, ctx, transactionInitial);
        }
        public static string GenerateTransactionNo(string transactionCode, DateTime transactionDate, IDbContext ctx = null, String transactionInitial = "")
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateTransactionNo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionCode", transactionCode));
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionDate", transactionDate));
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionInitial", transactionInitial));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion
        #region GetAttachmentMinutesPerShift
        public static List<GetAttachmentMinutesPerShift> GetAttachmentMinutesPerShiftList(string PaymentDate, string CreatedBy, string GCShift)
        {
            List<GetAttachmentMinutesPerShift> result = new List<GetAttachmentMinutesPerShift>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetAttachmentMinutesPerShift));
                ctx.CommandText = "GetAttachmentMinutesPerShift";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentDate", PaymentDate);
                ctx.Add("CreatedBy", CreatedBy);
                ctx.Add("GCShift", GCShift);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetAttachmentMinutesPerShift)helper.IDataReaderToObject(reader, new GetAttachmentMinutesPerShift()));
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
        public static List<GetAttachmentMinutesPerShift> GetAttachmentMinutesPerShiftList(string filterExpression)
        {
            List<GetAttachmentMinutesPerShift> result = new List<GetAttachmentMinutesPerShift>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetAttachmentMinutesPerShift));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetAttachmentMinutesPerShift)helper.IDataReaderToObject(reader, new GetAttachmentMinutesPerShift()));
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
        #region GetActiveMedicationPerMRN
        public static List<GetActiveMedicationPerMRN> GetActiveMedicationPerMRNList(int MRN, int PrescriptionOrderID)
        {
            List<GetActiveMedicationPerMRN> result = new List<GetActiveMedicationPerMRN>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetActiveMedicationPerMRN));
                ctx.CommandText = "GetActiveMedicationPerMRN";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@MRN", MRN);
                ctx.Add("@PrescriptionOrderID", PrescriptionOrderID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetActiveMedicationPerMRN)helper.IDataReaderToObject(reader, new GetActiveMedicationPerMRN()));
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
        #region GetApprovalMultiVisitScheduleOrder
        public static List<GetApprovalMultiVisitScheduleOrder> GetApprovalMultiVisitScheduleOrder(string PeriodeDate, int HealthcareServiceUnitID)
        {
            List<GetApprovalMultiVisitScheduleOrder> result = new List<GetApprovalMultiVisitScheduleOrder>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetApprovalMultiVisitScheduleOrder));
                ctx.CommandText = "GetApprovalMultiVisitScheduleOrder";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@PeriodDate", PeriodeDate);
                ctx.Add("@HealthcareServiceUnitID", HealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetApprovalMultiVisitScheduleOrder)helper.IDataReaderToObject(reader, new GetApprovalMultiVisitScheduleOrder()));
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
        #region GetARInvoiceDtMain
        public static List<GetARInvoiceDtMain> GetARInvoiceDtMainList(int ARInvoiceID)
        {
            List<GetARInvoiceDtMain> result = new List<GetARInvoiceDtMain>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetARInvoiceDtMain));
                ctx.CommandText = "GetARInvoiceDtMain";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ARInvoiceID", ARInvoiceID);
                ctx.Command.CommandTimeout = 300;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetARInvoiceDtMain)helper.IDataReaderToObject(reader, new GetARInvoiceDtMain()));
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
        #region GetListMultiVisitScheduleOrder
        public static List<GetListMultiVisitScheduleOrder> GetListMultiVisitScheduleOrder(string periodeDate, int healthcareServiceUnitID)
        {
            List<GetListMultiVisitScheduleOrder> result = new List<GetListMultiVisitScheduleOrder>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetListMultiVisitScheduleOrder));
                ctx.CommandText = "GetListMultiVisitScheduleOrder";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@PeriodDate", periodeDate);
                ctx.Add("@HealthcareServiceUnitID", healthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetListMultiVisitScheduleOrder)helper.IDataReaderToObject(reader, new GetListMultiVisitScheduleOrder()));
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
        #region GetARInvoicedtClaim
        public static List<GetARInvoicedtClaim> GetARInvoicedtClaimList(int ARInvoiceID)
        {
            List<GetARInvoicedtClaim> result = new List<GetARInvoicedtClaim>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetARInvoicedtClaim));
                ctx.CommandText = "GetARInvoicedtClaim";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ARInvoiceID", ARInvoiceID);
                ctx.Command.CommandTimeout = 300;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetARInvoicedtClaim)helper.IDataReaderToObject(reader, new GetARInvoicedtClaim()));
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
        #region GetARInvoiceDtPerChargesPharmacy
        public static List<GetARInvoiceDtPerChargesPharmacy> GetARInvoiceDtPerChargesPharmacyList(int ARInvoiceID)
        {
            List<GetARInvoiceDtPerChargesPharmacy> result = new List<GetARInvoiceDtPerChargesPharmacy>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetARInvoiceDtPerChargesPharmacy));
                ctx.CommandText = "GetARInvoicedtClaim";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ARInvoiceID", ARInvoiceID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetARInvoiceDtPerChargesPharmacy)helper.IDataReaderToObject(reader, new GetARInvoiceDtPerChargesPharmacy()));
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
        #region GetARInvoiceDetailTransactionRSSBBInformation
        public static List<GetARInvoiceDetailTransactionRSSBBInformation> GetARInvoiceDetailTransactionRSSBBInformation(String ARInvoiceDate, String ARInvoiceNo)
        {
            List<GetARInvoiceDetailTransactionRSSBBInformation> result = new List<GetARInvoiceDetailTransactionRSSBBInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetARInvoiceDetailTransactionRSSBBInformation));
                ctx.CommandText = "GetARInvoiceDetailTransactionRSSBBInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ARInvoiceDate", ARInvoiceDate);
                ctx.Add("ARInvoiceNo", ARInvoiceNo);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetARInvoiceDetailTransactionRSSBBInformation)helper.IDataReaderToObject(reader, new GetARInvoiceDetailTransactionRSSBBInformation()));
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
        #region GetBedHistoryRegistrationEklaim
        public static List<GetBedHistoryRegistrationEklaim> GetBedHistoryRegistrationEklaim(Int32 RegistrationID)
        {
            List<GetBedHistoryRegistrationEklaim> result = new List<GetBedHistoryRegistrationEklaim>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetBedHistoryRegistrationEklaim));
                ctx.CommandText = "GetBedHistoryRegistrationEklaim";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetBedHistoryRegistrationEklaim)helper.IDataReaderToObject(reader, new GetBedHistoryRegistrationEklaim()));
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
        #region GetCountDashboardEMR
        public static List<GetCountDashboardEMR> GetCountDashboardEMR(Int32 ParamedicID, String DepartmentID)
        {
            List<GetCountDashboardEMR> result = new List<GetCountDashboardEMR>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCountDashboardEMR));
                ctx.CommandText = "GetCountDashboardEMR";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("DepartmentID", DepartmentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCountDashboardEMR)helper.IDataReaderToObject(reader, new GetCountDashboardEMR()));
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
        #region GetCencusInformation
        public static List<PatientCencusInformation> GetCencusInformationList(DateTime CensusDate, int HealthcareServiceUnitID, IDbContext ctx = null)
        {
            List<PatientCencusInformation> result = new List<PatientCencusInformation>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientCencusInformation));
                ctx.CommandText = "GetCencusInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientCencusInformation)helper.IDataReaderToObject(reader, new PatientCencusInformation()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetCencusInformationPasienSisa
        public static List<GetCencusInformationPasienSisa> GetCencusInformationPasienSisaList(DateTime CensusDate, int HealthcareServiceUnitID)
        {
            List<GetCencusInformationPasienSisa> result = new List<GetCencusInformationPasienSisa>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCencusInformationPasienSisa));
                ctx.CommandText = "GetCencusInformationPasienSisa";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@CensusDate", CensusDate);
                ctx.Add("@HealthcareServiceUnitID", HealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCencusInformationPasienSisa)helper.IDataReaderToObject(reader, new GetCencusInformationPasienSisa()));
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
        public static List<GetCencusInformationPasienSisa> GetCencusInformationPasienSisaList(DateTime CensusDate, int HealthcareServiceUnitID, IDbContext ctx = null)
        {
            List<GetCencusInformationPasienSisa> result = new List<GetCencusInformationPasienSisa>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCencusInformationPasienSisa));
                ctx.CommandText = "GetCencusInformationPasienSisa";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCencusInformationPasienSisa)helper.IDataReaderToObject(reader, new GetCencusInformationPasienSisa()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }

        public static Int32 GetCencusInformationPasienSisaRowCount(DateTime CensusDate, int HealthcareServiceUnitID, IDbContext ctx = null)
        {
            List<GetCencusInformationPasienSisa> result = new List<GetCencusInformationPasienSisa>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCencusInformationPasienSisa));
                ctx.CommandText = "GetCencusInformationPasienSisa";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetCencusInformationPasienSisaRowCountList(DateTime CensusDate, int HealthcareServiceUnitID)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetCencusInformationPasienSisaRowCount(CensusDate, HealthcareServiceUnitID, ctx);
        }
        #endregion
        #region GetChangeRegistrationPayerList
        public static List<GetChangeRegistrationPayerList> GetChangeRegistrationPayerList(string oDepartmentID, int oHealthcareServiceUnitID, string oVisitDate, string oQuickSearch)
        {
            List<GetChangeRegistrationPayerList> result = new List<GetChangeRegistrationPayerList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetChangeRegistrationPayerList));
                ctx.CommandText = "GetChangeRegistrationPayerList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@DepartmentID", oDepartmentID);
                ctx.Add("@HealthcareServiceUnitID", oHealthcareServiceUnitID);
                ctx.Add("@VisitDate", oVisitDate);
                ctx.Add("@QuickSearch", oQuickSearch);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetChangeRegistrationPayerList)helper.IDataReaderToObject(reader, new GetChangeRegistrationPayerList()));
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
        #region GetCurrentDraftItemTariff
        public static List<GetCurrentDraftItemTariff> GetCurrentDraftItemTariff(Int32 appointmentID, int classID, int itemID, int itemType, DateTime transactionDate, IDbContext ctx = null, int testPartnerID = 0, Int32 transactionID = 0)
        {
            List<GetCurrentDraftItemTariff> result = new List<GetCurrentDraftItemTariff>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCurrentDraftItemTariff));
                ctx.CommandText = "GetCurrentDraftItemTariff";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("AppointmentID", appointmentID);
                ctx.Add("ClassID", classID);
                ctx.Add("ItemID", itemID);
                ctx.Add("ItemType", itemType);
                ctx.Add("TransactionDate", transactionDate);
                ctx.Add("TestPartnerID", testPartnerID);
                ctx.Add("TransactionID", transactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCurrentDraftItemTariff)helper.IDataReaderToObject(reader, new GetCurrentDraftItemTariff()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetCurrentItemCost
        public static List<GetCurrentItemCost> GetCurrentItemCost(String healthcareID, int itemID)
        {
            List<GetCurrentItemCost> result = new List<GetCurrentItemCost>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCurrentItemCost));
                ctx.CommandText = "GetCurrentItemCost";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", healthcareID);
                ctx.Add("ItemID", itemID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCurrentItemCost)helper.IDataReaderToObject(reader, new GetCurrentItemCost()));
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
        #region GetCurrentItemTariff
        public static List<GetCurrentItemTariff> GetCurrentItemTariff(Int32 registrationID, Int32 visitID, int classID, int itemID, int itemType, DateTime transactionDate, IDbContext ctx = null, int testPartnerID = 0, Int32 transactionID = 0)
        {
            List<GetCurrentItemTariff> result = new List<GetCurrentItemTariff>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCurrentItemTariff));
                ctx.CommandText = "GetCurrentItemTariff";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", registrationID);
                ctx.Add("VisitID", visitID);
                ctx.Add("ClassID", classID);
                ctx.Add("ItemID", itemID);
                ctx.Add("ItemType", itemType);
                ctx.Add("TransactionDate", transactionDate);
                ctx.Add("TestPartnerID", testPartnerID);
                ctx.Add("TransactionID", transactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCurrentItemTariff)helper.IDataReaderToObject(reader, new GetCurrentItemTariff()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetCurrentItemTariff2
        public static List<GetCurrentItemTariff2> GetCurrentItemTariff2(Int32 registrationID, Int32 visitID, int classID, int itemID, int itemType, DateTime transactionDate, IDbContext ctx = null, int testPartnerID = 0, Int32 transactionID = 0, Decimal costAmountBefore = 0)
        {
            List<GetCurrentItemTariff2> result = new List<GetCurrentItemTariff2>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCurrentItemTariff2));
                ctx.CommandText = "GetCurrentItemTariff2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", registrationID);
                ctx.Add("VisitID", visitID);
                ctx.Add("ClassID", classID);
                ctx.Add("ItemID", itemID);
                ctx.Add("ItemType", itemType);
                ctx.Add("TransactionDate", transactionDate);
                ctx.Add("TestPartnerID", testPartnerID);
                ctx.Add("TransactionID", transactionID);
                ctx.Add("CostAmountBefore", costAmountBefore);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCurrentItemTariff2)helper.IDataReaderToObject(reader, new GetCurrentItemTariff2()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetCurrentItemTariffAIO
        public static List<GetCurrentItemTariffAIO> GetCurrentItemTariffAIO(Int32 registrationID, Int32 visitID, int classID, int itemID, int itemType, DateTime transactionDate, IDbContext ctx = null, int testPartnerID = 0, Int32 transactionID = 0)
        {
            List<GetCurrentItemTariffAIO> result = new List<GetCurrentItemTariffAIO>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCurrentItemTariffAIO));
                ctx.CommandText = "GetCurrentItemTariffAIO";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", registrationID);
                ctx.Add("VisitID", visitID);
                ctx.Add("ClassID", classID);
                ctx.Add("ItemID", itemID);
                ctx.Add("ItemType", itemType);
                ctx.Add("TransactionDate", transactionDate);
                ctx.Add("TestPartnerID", testPartnerID);
                ctx.Add("TransactionID", transactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCurrentItemTariffAIO)helper.IDataReaderToObject(reader, new GetCurrentItemTariffAIO()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetConsultVisitDiagnose
        public static List<GetConsultVisitDiagnose> GetConsultVisitDiagnoseList(DateTime RegistrationDate, String DepartmentID, Int32 ServiceUnitID, Int32 ParamedicID, Int32 IsNeedCodification)
        {
            List<GetConsultVisitDiagnose> result = new List<GetConsultVisitDiagnose>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetConsultVisitDiagnose));
                ctx.CommandText = "GetConsultVisitDiagnose";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationDate", RegistrationDate);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("ServiceUnitID", ServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("IsNeedCodification", IsNeedCodification);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetConsultVisitDiagnose)helper.IDataReaderToObject(reader, new GetConsultVisitDiagnose()));
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
        #region GetConsultVisitOutpatient
        public static List<GetConsultVisitOutpatient> GetConsultVisitOutpatientList(DateTime ActualVisitDate, DateTime ActualVisitTime, String GCVisitStatus, String GCCustomerType, Int32 IsNeedCodification)
        {
            List<GetConsultVisitOutpatient> result = new List<GetConsultVisitOutpatient>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetConsultVisitOutpatient));
                ctx.CommandText = "GetConsultVisitOutpatient";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ActualVisitDate", ActualVisitDate);
                ctx.Add("ActualVisitTime", ActualVisitTime);
                ctx.Add("GCVisitStatus", GCVisitStatus);
                ctx.Add("GCCustomerType", GCCustomerType);
                ctx.Add("IsneedCodification", IsNeedCodification);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetConsultVisitOutpatient)helper.IDataReaderToObject(reader, new GetConsultVisitOutpatient()));
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
        #region GetRegistrationNewOrOldPatient
        public static List<GetRegistrationNewOrOldPatient> GetRegistrationNewOrOldPatientList(DateTime ActualVisitDate, String DepartmentID, Int32 IsNeedCodification)
        {
            List<GetRegistrationNewOrOldPatient> result = new List<GetRegistrationNewOrOldPatient>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationNewOrOldPatient));
                ctx.CommandText = "GetRegistrationNewOrOldPatient";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ActualVisitDate", ActualVisitDate);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("IsNeedCodification", IsNeedCodification);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationNewOrOldPatient)helper.IDataReaderToObject(reader, new GetRegistrationNewOrOldPatient()));
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
        #region GetRL3_5_2025
        public static List<GetRL3_5_2025> GetGetRL3_5_2025List(string filterExpression)
        {
            List<GetRL3_5_2025> result = new List<GetRL3_5_2025>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRL3_5_2025));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRL3_5_2025)helper.IDataReaderToObject(reader, new GetRL3_5_2025()));
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
        #region GetRL3_6_2025
        public static List<GetRL3_6_2025> GetGetRL3_6_2025List(string filterExpression)
        {
            List<GetRL3_6_2025> result = new List<GetRL3_6_2025>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRL3_6_2025));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRL3_6_2025)helper.IDataReaderToObject(reader, new GetRL3_6_2025()));
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
        #region GetRL3_7_2025
        public static List<GetRL3_7_2025> GetRL3_7_2025List(string filterExpression)
        {
            List<GetRL3_7_2025> result = new List<GetRL3_7_2025>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRL3_7_2025));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRL3_7_2025)helper.IDataReaderToObject(reader, new GetRL3_7_2025()));
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
        #region GetCountVisitDashboard
        public static List<GetCountVisitDashboard> GetCountVisitDashboard(Int32 Year, Int32 Month, Int32 ParamedicID)
        {
            List<GetCountVisitDashboard> result = new List<GetCountVisitDashboard>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCountVisitDashboard));
                ctx.CommandText = "GetCountVisitDashboard";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                ctx.Add("ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCountVisitDashboard)helper.IDataReaderToObject(reader, new GetCountVisitDashboard()));
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
        #region GetCountVisitPerDepartment
        public static List<GetCountVisitPerDepartment> GetCountVisitPerDepartment(String RegistrationDate, String DepartmentID, IDbContext ctx = null)
        {
            List<GetCountVisitPerDepartment> result = new List<GetCountVisitPerDepartment>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCountVisitPerDepartment));
                ctx.CommandText = "GetCountVisitPerDepartment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationDate", RegistrationDate);
                ctx.Add("DepartmentID", DepartmentID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCountVisitPerDepartment)helper.IDataReaderToObject(reader, new GetCountVisitPerDepartment()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetCountVisitPerDepartmentDashboard
        public static List<GetCountVisitPerDepartmentDashboard> GetCountVisitPerDepartmentDashboard(Int32 Year, Int32 Month, Int32 ParamedicID, String DepartmentID, IDbContext ctx = null)
        {
            List<GetCountVisitPerDepartmentDashboard> result = new List<GetCountVisitPerDepartmentDashboard>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetCountVisitPerDepartmentDashboard));
                ctx.CommandText = "GetCountVisitPerDepartmentDashboard";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("DepartmentID", DepartmentID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetCountVisitPerDepartmentDashboard)helper.IDataReaderToObject(reader, new GetCountVisitPerDepartmentDashboard()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetDateDiffPOPORPerSupplier
        public static List<GetDateDiffPOPORPerSupplier> GetDateDiffPOPORPerSupplier(int SupplierID)
        {
            List<GetDateDiffPOPORPerSupplier> result = new List<GetDateDiffPOPORPerSupplier>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDateDiffPOPORPerSupplier));
                ctx.CommandText = "GetDateDiffPOPORPerSupplier";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("SupplierID", SupplierID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDateDiffPOPORPerSupplier)helper.IDataReaderToObject(reader, new GetDateDiffPOPORPerSupplier()));
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
        #region GetDateGLTransactionAuditedLastPosting
        public static List<GetDateGLTransactionAuditedLastPosting> GetDateGLTransactionAuditedLastPosting()
        {
            List<GetDateGLTransactionAuditedLastPosting> result = new List<GetDateGLTransactionAuditedLastPosting>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDateGLTransactionAuditedLastPosting));
                ctx.CommandText = "GetDateGLTransactionAuditedLastPosting";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                /// ctx.Add("SupplierID", SupplierID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDateGLTransactionAuditedLastPosting)helper.IDataReaderToObject(reader, new GetDateGLTransactionAuditedLastPosting()));
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
        #region GetDetailInformationDashboardEMR
        public static List<GetDetailInformationDashboardEMR> GetDetailInformationDashboardEMR(Int32 ParamedicID, Int32 HealthcareServiceUnitID, Int32 Total)
        {
            List<GetDetailInformationDashboardEMR> result = new List<GetDetailInformationDashboardEMR>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDetailInformationDashboardEMR));
                ctx.CommandText = "GetDetailInformationDashboardEMR";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("Total", Total);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDetailInformationDashboardEMR)helper.IDataReaderToObject(reader, new GetDetailInformationDashboardEMR()));
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

        public static Int32 GetDetailInformationDashboardEMRRowCount(Int32 ParamedicID, Int32 HealthcareServiceUnitID, Int32 Total, IDbContext ctx)
        {
            List<GetDetailInformationDashboardEMR> result = new List<GetDetailInformationDashboardEMR>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDetailInformationDashboardEMR));
                ctx.CommandText = "GetDetailInformationDashboardEMR";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("Total", Total);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetDetailInformationDashboardEMRRowCount(Int32 ParamedicID, Int32 HealthcareServiceUnitID, Int32 Total)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetDetailInformationDashboardEMRRowCount(ParamedicID, HealthcareServiceUnitID, Total, ctx);
        }

        public static List<GetDetailInformationDashboardEMR> GetDetailInformationDashboardEMR(string filterExpression, int numRows, int pageIndex, string orderByExpression = "")
        {
            List<GetDetailInformationDashboardEMR> result = new List<GetDetailInformationDashboardEMR>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDetailInformationDashboardEMR));
                ctx.CommandText = helper.Select(filterExpression, numRows, pageIndex, orderByExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDetailInformationDashboardEMR)helper.IDataReaderToObject(reader, new GetDetailInformationDashboardEMR()));
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
        #region GetDetailInformationAppointmentDashboardEMR
        public static List<GetDetailInformationAppointmentDashboardEMR> GetDetailInformationAppointmentDashboardEMR(Int32 ParamedicID, Int32 HealthcareServiceUnitID, Int32 Total)
        {
            List<GetDetailInformationAppointmentDashboardEMR> result = new List<GetDetailInformationAppointmentDashboardEMR>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDetailInformationAppointmentDashboardEMR));
                ctx.CommandText = "GetDetailInformationAppointmentDashboardEMR";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("Total", Total);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDetailInformationAppointmentDashboardEMR)helper.IDataReaderToObject(reader, new GetDetailInformationAppointmentDashboardEMR()));
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

        public static Int32 GetDetailInformationAppointmentDashboardEMRRowCount(Int32 ParamedicID, Int32 HealthcareServiceUnitID, Int32 Total, IDbContext ctx)
        {
            List<GetDetailInformationAppointmentDashboardEMR> result = new List<GetDetailInformationAppointmentDashboardEMR>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDetailInformationAppointmentDashboardEMR));
                ctx.CommandText = "GetDetailInformationAppointmentDashboardEMR";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("Total", Total);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetDetailInformationAppointmentDashboardEMRRowCount(Int32 ParamedicID, Int32 HealthcareServiceUnitID, Int32 Total)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetDetailInformationAppointmentDashboardEMRRowCount(ParamedicID, HealthcareServiceUnitID, Total, ctx);
        }

        public static List<GetDetailInformationAppointmentDashboardEMR> GetDetailInformationAppointmentDashboardEMR(string filterExpression, int numRows, int pageIndex, string orderByExpression = "")
        {
            List<GetDetailInformationAppointmentDashboardEMR> result = new List<GetDetailInformationAppointmentDashboardEMR>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDetailInformationAppointmentDashboardEMR));
                ctx.CommandText = helper.Select(filterExpression, numRows, pageIndex, orderByExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDetailInformationAppointmentDashboardEMR)helper.IDataReaderToObject(reader, new GetDetailInformationAppointmentDashboardEMR()));
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
        #region GetDistinctFraction
        public static List<GetDistinctFraction> GetDistinctFraction(String StartDate, String EndDate, Int32 MRN, Int32 ItemID, Int32 RegistrationID)
        {
            List<GetDistinctFraction> result = new List<GetDistinctFraction>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDistinctFraction));
                ctx.CommandText = "GetDistinctFraction";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("StartDate", StartDate);
                ctx.Add("EndDate", EndDate);
                ctx.Add("MRN", MRN);
                ctx.Add("ItemID", ItemID);
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDistinctFraction)helper.IDataReaderToObject(reader, new GetDistinctFraction()));
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
        #region GetDistinctFractionPerDetail
        public static List<GetDistinctFractionPerDetail> GetDistinctFractionPerDetail(String StartDate, String EndDate, Int32 MRN, Int32 ItemID, Int32 FractionID, Int32 RegistrationID)
        {
            List<GetDistinctFractionPerDetail> result = new List<GetDistinctFractionPerDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDistinctFractionPerDetail));
                ctx.CommandText = "GetDistinctFractionPerDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("StartDate", StartDate);
                ctx.Add("EndDate", EndDate);
                ctx.Add("MRN", MRN);
                ctx.Add("ItemID", ItemID);
                ctx.Add("FractionID", FractionID);
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDistinctFractionPerDetail)helper.IDataReaderToObject(reader, new GetDistinctFractionPerDetail()));
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
        #region GetPatientDepositIn
        public static List<GetPatientDepositIn> GetPatientDepositIn(Int32 MRN, Int32 RegistrationID, IDbContext ctx = null)
        {
            List<GetPatientDepositIn> result = new List<GetPatientDepositIn>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientDepositIn));
                ctx.CommandText = "GetPatientDepositIn";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MRN", MRN);
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientDepositIn)helper.IDataReaderToObject(reader, new GetPatientDepositIn()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientPhysiotherapy
        public static List<GetPatientPhysiotherapy> GetPatientPhysiotherapyList(Int32 Year, Int32 FromMonth, Int32 ToMonth)
        {
            List<GetPatientPhysiotherapy> result = new List<GetPatientPhysiotherapy>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientPhysiotherapy));
                ctx.CommandText = "GetPatientPhysiotherapy";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("FromMonth", FromMonth);
                ctx.Add("ToMonth", ToMonth);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientPhysiotherapy)helper.IDataReaderToObject(reader, new GetPatientPhysiotherapy()));
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
        #region GetPrecursorDrugUsage
        public static List<GetPrecursorDrugUsage> GetPrecursorDrugUsageList(string filterExpression)
        {
            List<GetPrecursorDrugUsage> result = new List<GetPrecursorDrugUsage>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrecursorDrugUsage));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrecursorDrugUsage)helper.IDataReaderToObject(reader, new GetPrecursorDrugUsage()));
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
        #region GetPrescriptionPrice
        public static List<GetPrescriptionPrice> GetPrescriptionPrice(Int32 transactionID, Int32 prescriptionOrderID, IDbContext ctx = null)
        {
            List<GetPrescriptionPrice> result = new List<GetPrescriptionPrice>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionPrice));
                ctx.CommandText = "GetPrescriptionPrice";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", transactionID);
                ctx.Add("PrescriptionOrderID", prescriptionOrderID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionPrice)helper.IDataReaderToObject(reader, new GetPrescriptionPrice()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }

        public static List<GetPrescriptionPrice> GetPrescriptionPrice(Int32 transactionID, Int32 prescriptionOrderID)
        {
            List<GetPrescriptionPrice> result = new List<GetPrescriptionPrice>();
            IDbContext ctx = DbFactory.Configure();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionPrice));
                ctx.CommandText = "GetPrescriptionPrice";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", transactionID);
                ctx.Add("PrescriptionOrderID", prescriptionOrderID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionPrice)helper.IDataReaderToObject(reader, new GetPrescriptionPrice()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPrescriptionReturnDetail
        public static List<GetPrescriptionReturnDetail> GetPrescriptionReturnDetailList(string filterExpression)
        {
            List<GetPrescriptionReturnDetail> result = new List<GetPrescriptionReturnDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionReturnDetail));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionReturnDetail)helper.IDataReaderToObject(reader, new GetPrescriptionReturnDetail()));
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
        #region GetDataReport
        public static List<dynamic> GetDataReport(string procedureName, List<Variable> lstVariable)
        {
            var result = new List<dynamic>();
            IDbContext ctx = DbFactory.Configure();
            string typeName = string.Format("QIS.Medinfras.Data.Service.{0}", procedureName);
            try
            {
                DbHelper helper = new DbHelper(Type.GetType(typeName));
                ctx.CommandText = procedureName;
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Command.CommandTimeout = 1000;
                ctx.Clear();
                //Add Parameter
                foreach (Variable variable in lstVariable)
                {
                    ctx.Add(variable.Code, variable.Value);
                }
                //Get DataReader
                //result = DaoBase.GetDataTable(ctx);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add(helper.IDataReaderToObject(reader, Activator.CreateInstance(Type.GetType(typeName))));
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
        #region GetEvaluationMealPatient
        public static List<GetEvaluationMealPatient> GetEvaluationMealPatientList(String Periode, Int32 HealthcareServiceUnitID, String GCMealTime)
        {
            List<GetEvaluationMealPatient> result = new List<GetEvaluationMealPatient>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetEvaluationMealPatient));
                ctx.CommandText = "GetEvaluationMealPatient";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Periode", Periode);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("GCMealTime", GCMealTime);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetEvaluationMealPatient)helper.IDataReaderToObject(reader, new GetEvaluationMealPatient()));
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
        #region GetGenerateGuestToMedicalRecord
        public static List<GetGenerateGuestToMedicalRecord> GetGenerateGuestToMedicalRecord(string filterExpression)
        {
            List<GetGenerateGuestToMedicalRecord> result = new List<GetGenerateGuestToMedicalRecord>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGenerateGuestToMedicalRecord));
                ctx.CommandText = "GetGenerateGuestToMedicalRecord";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("FilterExpression", filterExpression);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGenerateGuestToMedicalRecord)helper.IDataReaderToObject(reader, new GetGenerateGuestToMedicalRecord()));
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
        #region GetGFRLaboratoriumData
        public static List<GetGFRLaboratoriumData> GetGFRLaboratoriumData(Int32 TransactionID)
        {
            List<GetGFRLaboratoriumData> result = new List<GetGFRLaboratoriumData>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGFRLaboratoriumData));
                ctx.CommandText = "GetGFRLaboratoriumData";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGFRLaboratoriumData)helper.IDataReaderToObject(reader, new GetGFRLaboratoriumData()));
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
        #region GetGLAccountForTreasuryCopyPaymentDetailRecon
        public static List<GetGLAccountForTreasuryCopyPaymentDetailRecon> GetGLAccountForTreasuryCopyPaymentDetailRecon(String GCPaymentMethod, Int32 EDCMachineID, Int32 BankID, String GCCashierGroup, Int32 HealthcareServiceUnitID)
        {
            List<GetGLAccountForTreasuryCopyPaymentDetailRecon> result = new List<GetGLAccountForTreasuryCopyPaymentDetailRecon>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLAccountForTreasuryCopyPaymentDetailRecon));
                ctx.CommandText = "GetGLAccountForTreasuryCopyPaymentDetailRecon";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GCPaymentMethod", GCPaymentMethod);
                ctx.Add("EDCMachineID", EDCMachineID);
                ctx.Add("BankID", BankID);
                ctx.Add("GCCashierGroup", GCCashierGroup);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLAccountForTreasuryCopyPaymentDetailRecon)helper.IDataReaderToObject(reader, new GetGLAccountForTreasuryCopyPaymentDetailRecon()));
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
        #region GetGLBalancePerGLAccount
        public static List<GetGLBalancePerGLAccount> GetGLBalancePerGLAccountList(Int32 GLAccountID, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows, IDbContext ctx)
        {
            List<GetGLBalancePerGLAccount> result = new List<GetGLBalancePerGLAccount>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerGLAccount));
                ctx.CommandText = "GetGLBalancePerGLAccount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalancePerGLAccount)helper.IDataReaderToObject(reader, new GetGLBalancePerGLAccount()));
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
        public static List<GetGLBalancePerGLAccount> GetGLBalancePerGLAccountList(Int32 GLAccountID, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerGLAccountList(GLAccountID, year, month, PageIndex, NumRows, ctx);
        }
        public static Int32 GetGLBalancePerGLAccountRowCount(Int32 GLAccountID, Int32 year, Int32 month, IDbContext ctx)
        {
            List<GetGLBalancePerGLAccount> result = new List<GetGLBalancePerGLAccount>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerGLAccount));
                ctx.CommandText = "GetGLBalancePerGLAccountRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetGLBalancePerGLAccountRowCount(Int32 GLAccountID, Int32 year, Int32 month)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerGLAccountRowCount(GLAccountID, year, month, ctx);
        }
        #endregion
        #region GetGLBalancePerGLAccountPerDate
        public static List<GetGLBalancePerGLAccountPerDate> GetGLBalancePerGLAccountPerDateList(Int32 GLAccountIDFrom, Int32 GLAccountIDTo, String JournalDate, IDbContext ctx)
        {
            List<GetGLBalancePerGLAccountPerDate> result = new List<GetGLBalancePerGLAccountPerDate>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerGLAccountPerDate));
                ctx.CommandText = "GetGLBalancePerGLAccountPerDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountIDFrom", GLAccountIDFrom);
                ctx.Add("GLAccountIDTo", GLAccountIDTo);
                ctx.Add("JournalDate", JournalDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalancePerGLAccountPerDate)helper.IDataReaderToObject(reader, new GetGLBalancePerGLAccountPerDate()));
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
        public static List<GetGLBalancePerGLAccountPerDate> GetGLBalancePerGLAccountPerDateList(Int32 GLAccountIDFrom, Int32 GLAccountIDTo, String JournalDate)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerGLAccountPerDateList(GLAccountIDFrom, GLAccountIDTo, JournalDate, ctx);
        }
        public static Int32 GetGLBalancePerGLAccountPerDateRowCount(Int32 GLAccountIDFrom, Int32 GLAccountIDTo, String JournalDate, IDbContext ctx)
        {
            List<GetGLBalancePerGLAccountPerDate> result = new List<GetGLBalancePerGLAccountPerDate>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerGLAccountPerDate));
                ctx.CommandText = "GetGLBalancePerGLAccountPerDateRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountIDFrom", GLAccountIDFrom);
                ctx.Add("GLAccountIDTo", GLAccountIDTo);
                ctx.Add("JournalDate", JournalDate);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetGLBalancePerGLAccountPerDateRowCount(Int32 GLAccountIDFrom, Int32 GLAccountIDTo, String JournalDate)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerGLAccountPerDateRowCount(GLAccountIDFrom, GLAccountIDTo, JournalDate, ctx);
        }
        #endregion
        #region GetGLBalancePerGLAccountGRANOSTIC
        public static List<GetGLBalancePerGLAccountGRANOSTIC> GetGLBalancePerGLAccountGRANOSTICList(Int32 GLAccountID, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows, IDbContext ctx)
        {
            List<GetGLBalancePerGLAccountGRANOSTIC> result = new List<GetGLBalancePerGLAccountGRANOSTIC>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerGLAccountGRANOSTIC));
                ctx.CommandText = "GetGLBalancePerGLAccountGRANOSTIC";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalancePerGLAccountGRANOSTIC)helper.IDataReaderToObject(reader, new GetGLBalancePerGLAccountGRANOSTIC()));
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
        public static List<GetGLBalancePerGLAccountGRANOSTIC> GetGLBalancePerGLAccountGRANOSTICList(Int32 GLAccountID, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerGLAccountGRANOSTICList(GLAccountID, year, month, PageIndex, NumRows, ctx);
        }
        public static Int32 GetGLBalancePerGLAccountGRANOSTICRowCount(Int32 GLAccountID, Int32 year, Int32 month, IDbContext ctx)
        {
            List<GetGLBalancePerGLAccountGRANOSTIC> result = new List<GetGLBalancePerGLAccountGRANOSTIC>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerGLAccountGRANOSTIC));
                ctx.CommandText = "GetGLBalancePerGLAccountGRANOSTICRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetGLBalancePerGLAccountGRANOSTICRowCount(Int32 GLAccountID, Int32 year, Int32 month)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerGLAccountGRANOSTICRowCount(GLAccountID, year, month, ctx);
        }
        #endregion
        #region GetGeographyVisit
        public static List<GetGeographyVisit> GetGeographyVisitList(DateTime RegistrationDate, String GCState, Boolean IsNeedCodification)
        {
            List<GetGeographyVisit> result = new List<GetGeographyVisit>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGeographyVisit));
                ctx.CommandText = "GetGeographyVisit";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("registrationDate", RegistrationDate);
                ctx.Add("GCState", GCState);
                ctx.Add("IsNeedCodification", IsNeedCodification);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGeographyVisit)helper.IDataReaderToObject(reader, new GetGeographyVisit()));
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
        #region GetGLBalanceDtPerSubLedger
        public static List<GetGLBalanceDtPerSubLedger> GetGLBalanceDtPerSubLedgerList(Int32 GLAccountID, Int32 SubLedger, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows)
        {
            List<GetGLBalanceDtPerSubLedger> result = new List<GetGLBalanceDtPerSubLedger>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalanceDtPerSubLedger));
                ctx.CommandText = "GetGLBalanceDtPerSubLedger";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("SubLedger", SubLedger);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalanceDtPerSubLedger)helper.IDataReaderToObject(reader, new GetGLBalanceDtPerSubLedger()));
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
        public static List<GetGLBalanceDtPerSubLedger> GetGLBalanceDtPerSubLedgerList(Int32 GLAccountID, Int32 SubLedger, Int32 year, Int32 month)
        {
            return GetGLBalanceDtPerSubLedgerList(GLAccountID, SubLedger, year, month, 1, 1000);
        }
        public static Int32 GetGLBalanceDtPerSubLedgerRowCount(Int32 GLAccountID, Int32 SubLedger, Int32 year, Int32 month, IDbContext ctx)
        {
            List<GetGLBalanceDtPerSubLedger> result = new List<GetGLBalanceDtPerSubLedger>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalanceDtPerSubLedger));
                ctx.CommandText = "GetGLBalanceDtPerSubLedgerRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("SubLedger", SubLedger);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetGLBalanceDtPerSubLedgerRowCount(Int32 GLAccountID, Int32 SubLedger, Int32 year, Int32 month)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalanceDtPerSubLedgerRowCount(GLAccountID, SubLedger, year, month, ctx);
        }
        #endregion
        #region GetGLBalanceDtInformation
        public static List<GetGLBalanceDtInformation> GetGLBalanceDtInformationList(Int32 GLAccountID, String SubLedger, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows)
        {
            List<GetGLBalanceDtInformation> result = new List<GetGLBalanceDtInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalanceDtInformation));
                ctx.CommandText = "GetGLBalanceDtInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("SubLedger", SubLedger);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalanceDtInformation)helper.IDataReaderToObject(reader, new GetGLBalanceDtInformation()));
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
        public static List<GetGLBalanceDtInformation> GetGLBalanceDtInformationList(Int32 GLAccountID, String SubLedger, Int32 year, Int32 month)
        {
            return GetGLBalanceDtInformationList(GLAccountID, SubLedger, year, month, 1, 1000);
        }
        public static Int32 GetGLBalanceDtInformationRowCount(Int32 GLAccountID, String SubLedger, Int32 year, Int32 month, IDbContext ctx)
        {
            List<GetGLBalanceDtInformation> result = new List<GetGLBalanceDtInformation>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalanceDtInformation));
                ctx.CommandText = "GetGLBalanceDtInformationRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("SubLedger", SubLedger);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetGLBalanceDtInformationRowCount(Int32 GLAccountID, String SubLedger, Int32 year, Int32 month)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalanceDtInformationRowCount(GLAccountID, SubLedger, year, month, ctx);
        }
        #endregion
        #region GetGLBalanceDtInformationPerPeriode
        public static List<GetGLBalanceDtInformationPerPeriode> GetGLBalanceDtInformationPerPeriodeList(
                                        Int32 GLAccountID, Int32 SubLedgerID, String HealthcareID, String DepartmentID, Int32 ServiceUnitID, Int32 BusinessPartnerID,
                                        Int32 year, Int32 month, String DataSource, Int32 IsDetailOnly)
        {
            List<GetGLBalanceDtInformationPerPeriode> result = new List<GetGLBalanceDtInformationPerPeriode>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalanceDtInformationPerPeriode));
                ctx.CommandText = "GetGLBalanceDtInformationPerPeriode";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("SubLedgerID", SubLedgerID);
                ctx.Add("HealthcareID", HealthcareID);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("ServiceUnitID", ServiceUnitID);
                ctx.Add("BusinessPartnerID", BusinessPartnerID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("DataSource", DataSource);
                ctx.Add("IsDetailOnly", IsDetailOnly);

                ctx.Command.CommandTimeout = 300;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalanceDtInformationPerPeriode)helper.IDataReaderToObject(reader, new GetGLBalanceDtInformationPerPeriode()));
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
        #region GetGLBalanceDtPerPeriod
        public static Int32 GetGLBalanceDtPerPeriodRowCount(Int32 GLAccountID, Int32 year, Int32 month, IDbContext ctx)
        {
            List<GetGLBalanceDtPerPeriod> result = new List<GetGLBalanceDtPerPeriod>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalanceDtPerPeriod));
                ctx.CommandText = "GetGLBalanceDtPerPeriodRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static List<GetGLBalanceDtPerPeriod> GetGLBalanceDtPerPeriodList(Int32 GLAccountID, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows, IDbContext ctx)
        {
            List<GetGLBalanceDtPerPeriod> result = new List<GetGLBalanceDtPerPeriod>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalanceDtPerPeriod));
                ctx.CommandText = "GetGLBalanceDtPerPeriod";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("GLAccountID", GLAccountID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalanceDtPerPeriod)helper.IDataReaderToObject(reader, new GetGLBalanceDtPerPeriod()));
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
        public static List<GetGLBalanceDtPerPeriod> GetGLBalanceDtPerPeriodList(Int32 GLAccountID, Int32 year, Int32 month, Int32 PageIndex, Int32 NumRows)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalanceDtPerPeriodList(GLAccountID, year, month, PageIndex, NumRows, ctx);
        }
        public static Int32 GetGLBalanceDtPerPeriodRowCount(Int32 GLAccountID, Int32 year, Int32 month)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalanceDtPerPeriodRowCount(GLAccountID, year, month, ctx);
        }
        #endregion
        #region GetGLBalancePerPeriod
        public static Int32 GetGLBalancePerPeriodRowCount(string healthCareID, Int32 year, Int32 month, Boolean IsDetailOnly, Int32 coa, IDbContext ctx)
        {
            List<GetGLBalancePerPeriod> result = new List<GetGLBalancePerPeriod>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerPeriod));
                ctx.CommandText = "GetGLBalancePerPeriodRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", healthCareID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("IsDetailOnly", IsDetailOnly);
                ctx.Add("coa", coa);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static List<GetGLBalancePerPeriod> GetGLBalancePerPeriodList(String healthCareID, Int32 year, Int32 month, Boolean IsDetailOnly, Int32 PageIndex, Int32 NumRows, Int32 coa, IDbContext ctx)
        {
            List<GetGLBalancePerPeriod> result = new List<GetGLBalancePerPeriod>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerPeriod));
                ctx.CommandText = "GetGLBalancePerPeriod";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", healthCareID);
                ctx.Add("JournalYear", year);
                ctx.Add("JournalMonth", month);
                ctx.Add("IsDetailOnly", IsDetailOnly);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);
                ctx.Add("coa", coa);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalancePerPeriod)helper.IDataReaderToObject(reader, new GetGLBalancePerPeriod()));
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
        public static List<GetGLBalancePerPeriod> GetGLBalancePerPeriodList(string healthCareID, Int32 year, Int32 month, Boolean IsDetailOnly, Int32 PageIndex, Int32 NumRows, Int32 coa)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerPeriodList(healthCareID, year, month, IsDetailOnly, PageIndex, NumRows, coa, ctx);
        }
        public static Int32 GetGLBalancePerPeriodRowCount(string healthCareID, Int32 year, Int32 month, Boolean IsDetailOnly, Int32 coa)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetGLBalancePerPeriodRowCount(healthCareID, year, month, IsDetailOnly, coa, ctx);
        }
        #endregion
        #region GetGLBalancePerPeriodPerLevel
        public static List<GetGLBalancePerPeriodPerLevel> GetGLBalancePerPeriodPerLevelList(String HealthcareID, int JournalYear, int JournalMonth, int AccountLevel, int AccountType)
        {
            List<GetGLBalancePerPeriodPerLevel> result = new List<GetGLBalancePerPeriodPerLevel>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerPeriodPerLevel));
                ctx.CommandText = "GetGLBalancePerPeriodPerLevel";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", HealthcareID);
                ctx.Add("JournalYear", JournalYear);
                ctx.Add("JournalMonth", JournalMonth);
                ctx.Add("AccountLevel", AccountLevel);
                ctx.Add("AccountType", AccountType);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalancePerPeriodPerLevel)helper.IDataReaderToObject(reader, new GetGLBalancePerPeriodPerLevel()));
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
        #region GetGLBalancePerLevelCompare
        public static List<GetGLBalancePerLevelCompare> GetGLBalancePerLevelCompareList(String HealthcareID, int JournalYear, int JournalMonth, int JournalYear2, int JournalMonth2, int AccountLevel)
        {
            List<GetGLBalancePerLevelCompare> result = new List<GetGLBalancePerLevelCompare>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetGLBalancePerLevelCompare));
                ctx.CommandText = "GetGLBalancePerLevelCompare";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", HealthcareID);
                ctx.Add("JournalYear", JournalYear);
                ctx.Add("JournalMonth", JournalMonth);
                ctx.Add("JournalYear2", JournalYear2);
                ctx.Add("JournalMonth2", JournalMonth2);
                ctx.Add("AccountLevel", AccountLevel);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetGLBalancePerLevelCompare)helper.IDataReaderToObject(reader, new GetGLBalancePerLevelCompare()));
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
        #region GetBudgetingRealizationPerMonthInformation
        public static List<GetBudgetingRealizationPerMonthInformation> GetBudgetingRealizationPerMonthInformationList(String Year, Int32 Month, String Display, Int32 PageIndex, Int32 NumRows, IDbContext ctx)
        {
            List<GetBudgetingRealizationPerMonthInformation> result = new List<GetBudgetingRealizationPerMonthInformation>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetBudgetingRealizationPerMonthInformation));
                ctx.CommandText = "GetBudgetingRealizationPerMonthInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                ctx.Add("Display", Display);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetBudgetingRealizationPerMonthInformation)helper.IDataReaderToObject(reader, new GetBudgetingRealizationPerMonthInformation()));
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
        public static List<GetBudgetingRealizationPerMonthInformation> GetBudgetingRealizationPerMonthInformationList(String Year, Int32 Month, String Display, Int32 PageIndex, Int32 NumRows)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetBudgetingRealizationPerMonthInformationList(Year, Month, Display, PageIndex, NumRows, ctx);
        }

        public static Int32 GetBudgetingRealizationPerMonthInformationRowCount(String Year, Int32 Month, String Display, IDbContext ctx)
        {
            List<GetBudgetingRealizationPerMonthInformation> result = new List<GetBudgetingRealizationPerMonthInformation>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetBudgetingRealizationPerMonthInformation));
                ctx.CommandText = "GetBudgetingRealizationPerMonthInformationRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                ctx.Add("Display", Display);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetBudgetingRealizationPerMonthInformationRowCount(String Year, Int32 Month, String Display)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetBudgetingRealizationPerMonthInformationRowCount(Year, Month, Display, ctx);
        }
        #endregion
        #region GetDisplayNewMedicationOrder1
        public static List<GetDisplayNewMedicationOrder1> GetDisplayNewMedicationOrder1()
        {
            List<GetDisplayNewMedicationOrder1> result = new List<GetDisplayNewMedicationOrder1>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDisplayNewMedicationOrder1));
                ctx.CommandText = "GetDisplayNewMedicationOrder1";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDisplayNewMedicationOrder1)helper.IDataReaderToObject(reader, new GetDisplayNewMedicationOrder1()));
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
        #region GetHealthcareServiceUnitUserList
        public static List<GetHealthcareServiceUnitUserList> GetHealthcareServiceUnitUserList(String healthcareID, int userID, String departmentID, String filterExpression)
        {
            List<GetHealthcareServiceUnitUserList> result = new List<GetHealthcareServiceUnitUserList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetHealthcareServiceUnitUserList));
                ctx.CommandText = "GetServiceUnitUserList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareID", healthcareID);
                ctx.Add("p_UserID", userID);
                ctx.Add("p_DepartmentID", departmentID);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetHealthcareServiceUnitUserList)helper.IDataReaderToObject(reader, new GetHealthcareServiceUnitUserList()));
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
        #region GetIndicatorHospitalClassCarePerMonthPerYear
        public static List<GetIndicatorHospitalClassCarePerMonthPerYear> GetIndicatorHospitalClassCarePerMonthPerYear(Int32 Year, Int32 Month)
        {
            List<GetIndicatorHospitalClassCarePerMonthPerYear> result = new List<GetIndicatorHospitalClassCarePerMonthPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetIndicatorHospitalClassCarePerMonthPerYear));
                ctx.CommandText = "GetIndicatorHospitalClassCarePerMonthPerYear";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetIndicatorHospitalClassCarePerMonthPerYear)helper.IDataReaderToObject(reader, new GetIndicatorHospitalClassCarePerMonthPerYear()));
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
        #region GetIndicatorHospitalServiceUnitPerMonthPerYear
        public static List<GetIndicatorHospitalServiceUnitPerMonthPerYear> GetIndicatorHospitalServiceUnitPerMonthPerYear(Int32 Year, Int32 Month)
        {
            List<GetIndicatorHospitalServiceUnitPerMonthPerYear> result = new List<GetIndicatorHospitalServiceUnitPerMonthPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetIndicatorHospitalServiceUnitPerMonthPerYear));
                ctx.CommandText = "GetIndicatorHospitalServiceUnitPerMonthPerYear";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetIndicatorHospitalServiceUnitPerMonthPerYear)helper.IDataReaderToObject(reader, new GetIndicatorHospitalServiceUnitPerMonthPerYear()));
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
        #region GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear
        public static List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Int32 Year, Int32 Month)
        {
            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> result = new List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear));
                ctx.CommandText = "GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear)helper.IDataReaderToObject(reader, new GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear()));
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
        #region GetIndicatorHospitalServicePerMonthPerYear
        public static List<GetIndicatorHospitalServicePerMonthPerYear> GetIndicatorHospitalServicePerMonthPerYear(Int32 CensusYear, Int32 IsNeedCodification)
        {
            List<GetIndicatorHospitalServicePerMonthPerYear> result = new List<GetIndicatorHospitalServicePerMonthPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetIndicatorHospitalServicePerMonthPerYear));
                ctx.CommandText = "GetIndicatorHospitalServicePerMonthPerYear";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("CensusYear", CensusYear);
                ctx.Add("IsNeedCodification", IsNeedCodification);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetIndicatorHospitalServicePerMonthPerYear)helper.IDataReaderToObject(reader, new GetIndicatorHospitalServicePerMonthPerYear()));
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
        #region GetInformasiPasienDirawat
        public static List<GetInformasiPasienDirawat> GetInformasiPasienDirawatList(string filterExpression)
        {
            List<GetInformasiPasienDirawat> result = new List<GetInformasiPasienDirawat>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetInformasiPasienDirawat));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetInformasiPasienDirawat)helper.IDataReaderToObject(reader, new GetInformasiPasienDirawat()));
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
        #region GetInpatientRegistrationPerMonth
        public static List<GetInpatientRegistrationPerMonth> GetGetInpatientRegistrationPerMonth(int RegistrationYear, int RegistrationMonth)
        {
            List<GetInpatientRegistrationPerMonth> result = new List<GetInpatientRegistrationPerMonth>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetInpatientRegistrationPerMonth));
                ctx.CommandText = "GetInpatientRegistrationPerMonth";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationYear", RegistrationYear);
                ctx.Add("RegistrationMonth", RegistrationMonth);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetInpatientRegistrationPerMonth)helper.IDataReaderToObject(reader, new GetInpatientRegistrationPerMonth()));
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
        #region GetInterfaceJournalOtomatisStatusInformation
        public static List<GetInterfaceJournalOtomatisStatusInformation> GetInterfaceJournalOtomatisStatusInformationList(String ParamYear, String ParamMonth)
        {
            List<GetInterfaceJournalOtomatisStatusInformation> result = new List<GetInterfaceJournalOtomatisStatusInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetInterfaceJournalOtomatisStatusInformation));
                ctx.CommandText = "GetInterfaceJournalOtomatisStatusInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamYear", ParamYear);
                ctx.Add("ParamMonth", ParamMonth);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetInterfaceJournalOtomatisStatusInformation)helper.IDataReaderToObject(reader, new GetInterfaceJournalOtomatisStatusInformation()));
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
        #region GetInterfaceJournalStatusInformation
        public static List<GetInterfaceJournalStatusInformation> GetInterfaceJournalStatusInformationList(String ParamYear, String ParamMonth)
        {
            List<GetInterfaceJournalStatusInformation> result = new List<GetInterfaceJournalStatusInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetInterfaceJournalStatusInformation));
                ctx.CommandText = "GetInterfaceJournalStatusInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamYear", ParamYear);
                ctx.Add("ParamMonth", ParamMonth);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetInterfaceJournalStatusInformation)helper.IDataReaderToObject(reader, new GetInterfaceJournalStatusInformation()));
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
        #region GetInternInpatientPerMonthPerYear
        public static List<GetInternInpatientPerMonthPerYear> GetInternInpatientPerMonthPerYear(int Month, int Year)
        {
            List<GetInternInpatientPerMonthPerYear> result = new List<GetInternInpatientPerMonthPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetInternInpatientPerMonthPerYear));
                ctx.CommandText = "GetInternInpatientPerMonthPerYear";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Month", Month);
                ctx.Add("Year", Year);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetInternInpatientPerMonthPerYear)helper.IDataReaderToObject(reader, new GetInternInpatientPerMonthPerYear()));
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
        #region GetInventoryValue
        public static List<GetInventoryValue> GetGetInventoryValue(String movementDate)
        {
            List<GetInventoryValue> result = new List<GetInventoryValue>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetInventoryValue));
                ctx.CommandText = "GetInventoryValue";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MovementDate", movementDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetInventoryValue)helper.IDataReaderToObject(reader, new GetInventoryValue()));
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
        #region GetItemDistributionDtReport
        public static List<GetItemDistributionDtReport> GetItemDistributionDtReport(int locationID, string distributionType)
        {
            List<GetItemDistributionDtReport> result = new List<GetItemDistributionDtReport>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemDistributionDtReport));
                ctx.CommandText = "GetItemDistributionDtReport";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("LocationID", locationID);
                ctx.Add("DistributionType", distributionType);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemDistributionDtReport)helper.IDataReaderToObject(reader, new GetItemDistributionDtReport()));
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
        #region GetItemDistributionYearItemMonth
        public static List<GetItemDistributionYearItemMonth> GetItemDistributionYearItemMonth(string filterExpression)
        {
            List<GetItemDistributionYearItemMonth> result = new List<GetItemDistributionYearItemMonth>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemDistributionYearItemMonth));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemDistributionYearItemMonth)helper.IDataReaderToObject(reader, new GetItemDistributionYearItemMonth()));
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
        #region GetItemLastBaseTariff
        public static List<GetItemLastBaseTariff> GetItemLastBaseTariff(String healthcareID, int bookID, int itemID)
        {
            List<GetItemLastBaseTariff> result = new List<GetItemLastBaseTariff>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemLastBaseTariff));
                ctx.CommandText = "GetItemLastBaseTariff";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", healthcareID);
                ctx.Add("BookID", bookID);
                ctx.Add("ItemID", itemID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemLastBaseTariff)helper.IDataReaderToObject(reader, new GetItemLastBaseTariff()));
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
        #region GetItemNonMovement
        public static List<GetItemNonMovement> GetItemNonMovement(String MovementDate, String LocationID)
        {
            List<GetItemNonMovement> result = new List<GetItemNonMovement>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemNonMovement));
                ctx.CommandText = "GetItemNonMovement";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@MovementDate", MovementDate);
                ctx.Add("@LocationID", LocationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemNonMovement)helper.IDataReaderToObject(reader, new GetItemNonMovement()));
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
        public static Int32 GetItemNonMovementRowCount(String MovementDate, String LocationID)
        {
            List<GetItemNonMovement> result = new List<GetItemNonMovement>();
            SqlParameter param = new SqlParameter();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemNonMovement));
                ctx.CommandText = "GetItemNonMovement";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@MovementDate", MovementDate);
                ctx.Add("@LocationID", LocationID);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        //public static Int32 GetItemNonMovementRowCount(String MovementDate, String LocationID)
        //{
        //    IDbContext ctx = DbFactory.Configure();
        //    return GetItemNonMovementRowCount(MovementDate, LocationID);
        //}
        #endregion
        #region GetItemTariffListByBook
        public static List<ItemTariffListByBook> GetItemTariffListByBook(String healthcareID, int bookID, int itemID)
        {
            List<ItemTariffListByBook> result = new List<ItemTariffListByBook>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemLastBaseTariff));
                ctx.CommandText = "GetItemTariffListByBook";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", healthcareID);
                ctx.Add("BookID", bookID);
                ctx.Add("ItemID", itemID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((ItemTariffListByBook)helper.IDataReaderToObject(reader, new ItemTariffListByBook()));
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
        #region GetItemLastStocking
        public static List<GetItemLastStocking> GetItemLastStocking(DateTime Date)
        {
            List<GetItemLastStocking> result = new List<GetItemLastStocking>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemLastStocking));
                ctx.CommandText = "GetItemLastBaseTariff";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", Date);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemLastStocking)helper.IDataReaderToObject(reader, new GetItemLastStocking()));
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
        #region GetItemMasterPurchase
        public static List<GetItemMasterPurchase> GetItemMasterPurchaseList(string healthCareID, int itemID, int businessPartnerID, IDbContext ctx)
        {
            List<GetItemMasterPurchase> result = new List<GetItemMasterPurchase>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemMasterPurchase));
                ctx.CommandText = "GetItemMasterPurchase";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareID", healthCareID);
                ctx.Add("p_ItemID", itemID);
                ctx.Add("p_BusinessPartnerID", businessPartnerID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemMasterPurchase)helper.IDataReaderToObject(reader, new GetItemMasterPurchase()));
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
        public static List<GetItemMasterPurchase> GetItemMasterPurchaseList(string healthCareID, int itemID, int businessPartnerID)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetItemMasterPurchaseList(healthCareID, itemID, businessPartnerID, ctx);
        }
        #endregion
        #region GetItemMasterPurchaseWithDate
        public static List<GetItemMasterPurchaseWithDate> GetItemMasterPurchaseWithDateList(string healthCareID, int itemID, int businessPartnerID, string effectiveDate, IDbContext ctx)
        {
            List<GetItemMasterPurchaseWithDate> result = new List<GetItemMasterPurchaseWithDate>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemMasterPurchaseWithDate));
                ctx.CommandText = "GetItemMasterPurchaseWithDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareID", healthCareID);
                ctx.Add("p_ItemID", itemID);
                ctx.Add("p_BusinessPartnerID", businessPartnerID);
                ctx.Add("p_EffectiveDate", effectiveDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemMasterPurchaseWithDate)helper.IDataReaderToObject(reader, new GetItemMasterPurchaseWithDate()));
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
        public static List<GetItemMasterPurchaseWithDate> GetItemMasterPurchaseWithDateList(string healthCareID, int itemID, int businessPartnerID, string effectiveDate)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetItemMasterPurchaseWithDateList(healthCareID, itemID, businessPartnerID, effectiveDate, ctx);
        }
        #endregion
        #region GetItemMovementCustom
        public static List<GetItemMovementCustom> GetItemMovementCustom(string movementDate)
        {
            List<GetItemMovementCustom> result = new List<GetItemMovementCustom>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemMovementCustom));
                ctx.CommandText = "GetItemMovementCustom";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MovementDate", movementDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemMovementCustom)helper.IDataReaderToObject(reader, new GetItemMovementCustom()));
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
        #region GetItemMovementPerPeriodeDetail
        public static List<GetItemMovementPerPeriodeDetail> GetItemMovementPerPeriodeDetail(string movementDate, int locationID, string itemName, Int32 PageIndex, Int32 NumRows)
        {
            List<GetItemMovementPerPeriodeDetail> result = new List<GetItemMovementPerPeriodeDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemMovementPerPeriodeDetail));
                ctx.CommandText = "GetItemMovementPerPeriodeDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MovementDate", movementDate);
                ctx.Add("LocationID", locationID);
                ctx.Add("ItemName", itemName);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemMovementPerPeriodeDetail)helper.IDataReaderToObject(reader, new GetItemMovementPerPeriodeDetail()));
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
        #region GetItemProductExpiredDate
        public static List<GetItemProductExpiredDate> GetItemProductExpiredDate(String ExpiredDate, int LocationID)
        {
            List<GetItemProductExpiredDate> result = new List<GetItemProductExpiredDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemProductExpiredDate));
                ctx.CommandText = "GetItemProductExpiredDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ExpiredDate", ExpiredDate);
                ctx.Add("LocationID", ExpiredDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemProductExpiredDate)helper.IDataReaderToObject(reader, new GetItemProductExpiredDate()));
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
        #region GetItemQtyOnOrder
        public static List<GetItemQtyOnOrder> GetItemQtyOnOrder(int itemID, int locationID, int type)
        {
            List<GetItemQtyOnOrder> result = new List<GetItemQtyOnOrder>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemQtyOnOrder));
                ctx.CommandText = "GetItemQtyOnOrder";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ItemID", itemID);
                ctx.Add("LocationID", locationID);
                ctx.Add("Type", type);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemQtyOnOrder)helper.IDataReaderToObject(reader, new GetItemQtyOnOrder()));
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
        #region GetItemRevenueSharing
        public static List<GetItemRevenueSharing> GetItemRevenueSharing(string ItemCode, int ParamedicID, int ClassID, string GCParamedicRole, int VisitID, int ChargesHealthcareServiceUnitID, DateTime TransactionDate, string TransactionTime)
        {
            List<GetItemRevenueSharing> result = new List<GetItemRevenueSharing>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemRevenueSharing));
                ctx.CommandText = "GetItemRevenueSharing";
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Command.CommandTimeout = 200;
                //Add Parameter
                ctx.Add("p_ItemCode", ItemCode);
                ctx.Add("p_ParamedicID", ParamedicID);
                ctx.Add("p_ClassID", ClassID);
                ctx.Add("p_GCParamedicRole", GCParamedicRole);
                ctx.Add("p_VisitID", VisitID);
                ctx.Add("p_ChargesHealthcareServiceUnitID", ChargesHealthcareServiceUnitID);
                ctx.Add("p_TransactionDate", TransactionDate);
                ctx.Add("p_TransactionTime", TransactionTime);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemRevenueSharing)helper.IDataReaderToObject(reader, new GetItemRevenueSharing()));
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
        public static List<GetItemRevenueSharing> GetItemRevenueSharing(string ItemCode, int ParamedicID, int ClassID, string GCParamedicRole, int VisitID, int ChargesHealthcareServiceUnitID, DateTime TransactionDate, string TransactionTime, IDbContext ctx)
        {
            List<GetItemRevenueSharing> result = new List<GetItemRevenueSharing>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemRevenueSharing));
                ctx.CommandText = "GetItemRevenueSharing";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_ItemCode", ItemCode);
                ctx.Add("p_ParamedicID", ParamedicID);
                ctx.Add("p_ClassID", ClassID);
                ctx.Add("p_GCParamedicRole", GCParamedicRole);
                ctx.Add("p_VisitID", VisitID);
                ctx.Add("p_ChargesHealthcareServiceUnitID", ChargesHealthcareServiceUnitID);
                ctx.Add("p_TransactionDate", TransactionDate);
                ctx.Add("p_TransactionTime", TransactionTime);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemRevenueSharing)helper.IDataReaderToObject(reader, new GetItemRevenueSharing()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }
        #endregion
        #region GetItemUsagePerItem
        public static List<GetItemUsagePerItem> GetItemUsagePerItem(DateTime TransactionDate, string TransactionTime, int HealthcareServiceUnitID)
        {
            List<GetItemUsagePerItem> result = new List<GetItemUsagePerItem>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetItemUsagePerItem));
                ctx.CommandText = "GetItemUsagePerItem";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionDate", TransactionDate);
                ctx.Add("TransactionTime", TransactionTime);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetItemUsagePerItem)helper.IDataReaderToObject(reader, new GetItemUsagePerItem()));
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
        #region GetJournalInformationbyReferenceNumber
        public static List<GetJournalInformationbyReferenceNumber> GetJournalInformationbyReferenceNumber(String ReferenceNo, String GCTransactionStatus)
        {
            List<GetJournalInformationbyReferenceNumber> result = new List<GetJournalInformationbyReferenceNumber>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetJournalInformationbyReferenceNumber));
                ctx.CommandText = "GetJournalInformationbyReferenceNumber";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ReferenceNo", ReferenceNo);
                ctx.Add("GCTransactionStatus", GCTransactionStatus);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetJournalInformationbyReferenceNumber)helper.IDataReaderToObject(reader, new GetJournalInformationbyReferenceNumber()));
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

        public static Int32 GetJournalInformationbyReferenceNumberRowCount(String ReferenceNo, IDbContext ctx)
        {
            List<GetJournalInformationbyReferenceNumber> result = new List<GetJournalInformationbyReferenceNumber>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetJournalInformationbyReferenceNumber));
                ctx.CommandText = "GetJournalInformationbyReferenceNumberRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ReferenceNo", ReferenceNo);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        #endregion
        #region GetJournalProcessLogCountSummary
        public static List<GetJournalProcessLogCountSummary> GetJournalProcessLogCountSummary(string paramDate)
        {
            List<GetJournalProcessLogCountSummary> result = new List<GetJournalProcessLogCountSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetJournalProcessLogCountSummary));
                ctx.CommandText = "GetJournalProcessLogCountSummary";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ProcessDate", paramDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetJournalProcessLogCountSummary)helper.IDataReaderToObject(reader, new GetJournalProcessLogCountSummary()));
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
        #region GetKwitansiHonorDokter
        public static List<GetKwitansiHonorDokter> GetKwitansiHonorDokter(Int32 PaymentID)
        {
            List<GetKwitansiHonorDokter> result = new List<GetKwitansiHonorDokter>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetKwitansiHonorDokter));
                ctx.CommandText = "GetKwitansiHonorDokter";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentID", PaymentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetKwitansiHonorDokter)helper.IDataReaderToObject(reader, new GetKwitansiHonorDokter()));
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
        #region GetLabaRugiRekap
        public static List<GetLabaRugiRekap> GetLabaRugiRekap(Int32 JournalYear, Int32 JournalMonth)
        {
            List<GetLabaRugiRekap> result = new List<GetLabaRugiRekap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetLabaRugiRekap));
                ctx.CommandText = "GetLabaRugiRekap";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("JournalYear", JournalYear);
                ctx.Add("JournalMonth", JournalMonth);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetLabaRugiRekap)helper.IDataReaderToObject(reader, new GetLabaRugiRekap()));
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
        #region GetLocationAllUserList
        public static List<GetLocationAllUserList> GetLocationAllUserList(string healthCareID, int userID, string transactionCode, string filterExpression)
        {
            List<GetLocationAllUserList> result = new List<GetLocationAllUserList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetLocationAllUserList));
                ctx.CommandText = "GetLocationAllUserList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareID", healthCareID);
                ctx.Add("p_UserID", userID);
                ctx.Add("p_TransactionCode", transactionCode);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetLocationAllUserList)helper.IDataReaderToObject(reader, new GetLocationAllUserList()));
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
        #region GetLocationUserList
        public static List<GetLocationUserList> GetLocationUserList(string healthCareID, int userID, string transactionCode, string filterExpression)
        {
            List<GetLocationUserList> result = new List<GetLocationUserList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetLocationUserList));
                ctx.CommandText = "GetLocationUserList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareID", healthCareID);
                ctx.Add("p_UserID", userID);
                ctx.Add("p_TransactionCode", transactionCode);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetLocationUserList)helper.IDataReaderToObject(reader, new GetLocationUserList()));
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
        #region GetMCUDetailOriginalPackageTariff
        public static List<GetMCUDetailOriginalPackageTariff> GetMCUDetailOriginalPackageTariffList(Int32 RegistrationID)
        {
            List<GetMCUDetailOriginalPackageTariff> result = new List<GetMCUDetailOriginalPackageTariff>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMCUDetailOriginalPackageTariff));
                ctx.CommandText = "GetMCUDetailOriginalPackageTariff";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMCUDetailOriginalPackageTariff)helper.IDataReaderToObject(reader, new GetMCUDetailOriginalPackageTariff()));
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
        #region GetMealPlanPatientRSSEBK
        public static List<GetMealPlanPatientRSSEBK> GetMealPlanPatientRSSEBK(DateTime Date, Int32 HealthcareServiceUnitID, String GCMealTime)
        {
            List<GetMealPlanPatientRSSEBK> result = new List<GetMealPlanPatientRSSEBK>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMealPlanPatientRSSEBK));
                ctx.CommandText = "GetMealPlanPatientRSSEBK";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", Date);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("GCMealTime", GCMealTime);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMealPlanPatientRSSEBK)helper.IDataReaderToObject(reader, new GetMealPlanPatientRSSEBK()));
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
        #region GetMedicineDeadStockRSSES
        public static List<GetMedicineDeadStockRSSES> GetMedicineDeadStockRSSESList(string date, string numOfDaysStayed, string locationID, string gCItemType)
        {
            List<GetMedicineDeadStockRSSES> result = new List<GetMedicineDeadStockRSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMedicineDeadStockRSSES));
                ctx.CommandText = "GetMedicineDeadStockRSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@Date", date));
                ctx.Command.Parameters.Add(new SqlParameter("@NumOfDaysStayed", numOfDaysStayed));
                ctx.Command.Parameters.Add(new SqlParameter("@LocationID", locationID));
                ctx.Command.Parameters.Add(new SqlParameter("@GCItemType", gCItemType));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMedicineDeadStockRSSES)helper.IDataReaderToObject(reader, new GetMedicineDeadStockRSSES()));
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
        #region GetMedicalDiagnosticPerMonth
        public static List<GetMedicalDiagnosticPerMonth> GetMedicalDiagnosticPerMonth(int Month, int Year, int HealthcareServiceUnitID)
        {
            List<GetMedicalDiagnosticPerMonth> result = new List<GetMedicalDiagnosticPerMonth>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMedicalDiagnosticPerMonth));
                ctx.CommandText = "GetLocationUserList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Month", Month);
                ctx.Add("Year", Year);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMedicalDiagnosticPerMonth)helper.IDataReaderToObject(reader, new GetMedicalDiagnosticPerMonth()));
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
        #region GetMedicationChartItemPerDate
        public static List<GetMedicationChartItemPerDate> GetMedicationChartItemPerDateList(string visitID, string displayMode, string isUsingUDD, string medicationDate, IDbContext ctx = null)
        {
            List<GetMedicationChartItemPerDate> result = new List<GetMedicationChartItemPerDate>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetMedicationChartItemPerDate";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@VisitID", visitID));
            ctx.Command.Parameters.Add(new SqlParameter("@DisplayMode", displayMode));
            ctx.Command.Parameters.Add(new SqlParameter("@IsUsingUDD", isUsingUDD));
            ctx.Command.Parameters.Add(new SqlParameter("@MedicationDate", medicationDate));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(GetMedicationChartItemPerDate));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMedicationChartItemPerDate)helper.IDataReaderToObject(reader, new GetMedicationChartItemPerDate()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetMedicationRekapGenerikNonGenerik
        public static List<GetMedicationRekapGenerikNonGenerik> GetMedicationRekapGenerikNonGenerik(Int32 Year, Int32 Month)
        {
            List<GetMedicationRekapGenerikNonGenerik> result = new List<GetMedicationRekapGenerikNonGenerik>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMedicationRekapGenerikNonGenerik));
                ctx.CommandText = "GetMedicationRekapGenerikNonGenerik";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMedicationRekapGenerikNonGenerik)helper.IDataReaderToObject(reader, new GetMedicationRekapGenerikNonGenerik()));
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
        #region GetMSChiefComplaint
        public static List<GetMSChiefComplaint> GetMSChiefComplaintList(int VisitID)
        {
            List<GetMSChiefComplaint> result = new List<GetMSChiefComplaint>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMSChiefComplaint));
                ctx.CommandText = "GetMSChiefComplaint";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", VisitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMSChiefComplaint)helper.IDataReaderToObject(reader, new GetMSChiefComplaint()));
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
        #region GetMSConsultVisit
        public static List<GetMSConsultVisit> GetMSConsultVisitList(int VisitID)
        {
            List<GetMSConsultVisit> result = new List<GetMSConsultVisit>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMSConsultVisit));
                ctx.CommandText = "GetMSConsultVisit";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", VisitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMSConsultVisit)helper.IDataReaderToObject(reader, new GetMSConsultVisit()));
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
        #region GetMSMSTAssessment
        public static List<GetMSMSTAssessment> GetMSMSTAssessmentList(int VisitID)
        {
            List<GetMSMSTAssessment> result = new List<GetMSMSTAssessment>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMSMSTAssessment));
                ctx.CommandText = "GetMSMSTAssessment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", VisitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMSMSTAssessment)helper.IDataReaderToObject(reader, new GetMSMSTAssessment()));
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
        #region GetMSNurseChiefComplaint
        public static List<GetMSNurseChiefComplaint> GetMSNurseChiefComplaintList(int VisitID)
        {
            List<GetMSNurseChiefComplaint> result = new List<GetMSNurseChiefComplaint>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMSNurseChiefComplaint));
                ctx.CommandText = "GetMSNurseChiefComplaint";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", VisitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMSNurseChiefComplaint)helper.IDataReaderToObject(reader, new GetMSNurseChiefComplaint()));
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
        #region GetMSNursingJournal
        public static List<GetMSNursingJournal> GetMSNursingJournalList(String ListVisitID, Int32 ParamedicID)
        {
            List<GetMSNursingJournal> result = new List<GetMSNursingJournal>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMSNursingJournal));
                ctx.CommandText = "GetMSNursingJournal";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ListVisitID", ListVisitID);
                ctx.Add("@ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMSNursingJournal)helper.IDataReaderToObject(reader, new GetMSNursingJournal()));
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
        #region GetMSPatientNurseTransfer
        public static List<GetMSPatientNurseTransfer> GetMSPatientNurseTransferList(int RegistrationID)
        {
            List<GetMSPatientNurseTransfer> result = new List<GetMSPatientNurseTransfer>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMSPatientNurseTransfer));
                ctx.CommandText = "GetMSPatientNurseTransfer";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMSPatientNurseTransfer)helper.IDataReaderToObject(reader, new GetMSPatientNurseTransfer()));
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
        #region GetMSPatientVisitNote
        public static List<GetMSPatientVisitNote> GetMSPatientVisitNoteList(String RegistrationID, String StartDate, String EndDate)
        {
            List<GetMSPatientVisitNote> result = new List<GetMSPatientVisitNote>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetMSPatientVisitNote));
                ctx.CommandText = "GetMSPatientVisitNote";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RegistrationID", RegistrationID);
                ctx.Add("@StartDate", StartDate);
                ctx.Add("@EndDate", EndDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMSPatientVisitNote)helper.IDataReaderToObject(reader, new GetMSPatientVisitNote()));
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
        #region GetMutasiHutang
        public static List<GetMutasiHutang> GetMutasiHutangList(string Periode)
        {
            List<GetMutasiHutang> result = new List<GetMutasiHutang>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetLocationUserList));
                ctx.CommandText = "GetMutasiHutang";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Periode", Periode);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetMutasiHutang)helper.IDataReaderToObject(reader, new GetMutasiHutang()));
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
        #region GetNewsAttachmentPerShift
        public static List<GetNewsAttachmentPerShift> GetNewsAttachmentPerShiftList(string PaymentDate, string CreatedBy, string GCShift)
        {
            List<GetNewsAttachmentPerShift> result = new List<GetNewsAttachmentPerShift>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetNewsAttachmentPerShift));
                ctx.CommandText = "GetNewsAttachmentPerShift";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentDate", PaymentDate);
                ctx.Add("CreatedBy", CreatedBy);
                ctx.Add("GCShift", GCShift);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetNewsAttachmentPerShift)helper.IDataReaderToObject(reader, new GetNewsAttachmentPerShift()));
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
        public static List<GetNewsAttachmentPerShift> GetNewsAttachmentPerShiftList(string filterExpression)
        {
            List<GetNewsAttachmentPerShift> result = new List<GetNewsAttachmentPerShift>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetNewsAttachmentPerShift));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetNewsAttachmentPerShift)helper.IDataReaderToObject(reader, new GetNewsAttachmentPerShift()));
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
        #region GetNursingItemGroupSubGroupListByDiagnose
        public static List<GetNursingItemGroupSubGroupListByDiagnose> GetNursingItemGroupSubGroupListByDiagnose(int nursingDiagnoseID)
        {
            List<GetNursingItemGroupSubGroupListByDiagnose> result = new List<GetNursingItemGroupSubGroupListByDiagnose>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetNursingItemGroupSubGroupListByDiagnose));
                ctx.CommandText = "GetNursingItemGroupSubGroupListByDiagnose";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_NursingDiagnoseID", nursingDiagnoseID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetNursingItemGroupSubGroupListByDiagnose)helper.IDataReaderToObject(reader, new GetNursingItemGroupSubGroupListByDiagnose()));
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

        public static List<GetNursingItemGroupSubGroupListByDiagnose> GetNursingItemGroupSubGroupListByDiagnose(int nursingDiagnoseID, IDbContext ctx)
        {
            List<GetNursingItemGroupSubGroupListByDiagnose> result = new List<GetNursingItemGroupSubGroupListByDiagnose>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetNursingItemGroupSubGroupListByDiagnose));
                ctx.CommandText = "GetNursingItemGroupSubGroupListByDiagnose";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_NursingDiagnoseID", nursingDiagnoseID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetNursingItemGroupSubGroupListByDiagnose)helper.IDataReaderToObject(reader, new GetNursingItemGroupSubGroupListByDiagnose()));
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
        #region GetNursingJournal
        public static List<GetNursingJournal> GetNursingJournal(String TransactionDate)
        {
            List<GetNursingJournal> result = new List<GetNursingJournal>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetNursingJournal));
                ctx.CommandText = "GetNursingJournal";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionDate", TransactionDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetNursingJournal)helper.IDataReaderToObject(reader, new GetNursingJournal()));
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
        #region GetNursingJournalRSSEBK
        public static List<GetNursingJournalRSSEBK> GetNursingJournalRSSEBK(String TransactionDate, string VisitDepartmentID, Int32 HealthcareServiceUnitID, Int32 ParamedicID)
        {
            List<GetNursingJournalRSSEBK> result = new List<GetNursingJournalRSSEBK>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetNursingJournalRSSEBK));
                ctx.CommandText = "GetNursingJournalRSSEBK";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionDate", TransactionDate);
                ctx.Add("VisitDepartmentID", VisitDepartmentID);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetNursingJournalRSSEBK)helper.IDataReaderToObject(reader, new GetNursingJournalRSSEBK()));
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
        #region GetNursingPatientNameDead
        public static List<NursingPatientNameDead> GetNursingPatientNameDead(DateTime CensusDate, int HealthcareServiceUnitID, IDbContext ctx = null)
        {
            List<NursingPatientNameDead> result = new List<NursingPatientNameDead>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(NursingPatientNameDead));
                ctx.CommandText = "GetNursingPatientNameDead";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((NursingPatientNameDead)helper.IDataReaderToObject(reader, new NursingPatientNameDead()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetNursingPatientNameDischarged
        public static List<NursingPatientNameDischarged> GetNursingPatientNameDischarged(DateTime CensusDate, int HealthcareServiceUnitID, IDbContext ctx = null)
        {
            List<NursingPatientNameDischarged> result = new List<NursingPatientNameDischarged>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(NursingPatientNameDischarged));
                ctx.CommandText = "GetNursingPatientNameDischarged";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((NursingPatientNameDischarged)helper.IDataReaderToObject(reader, new NursingPatientNameDischarged()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetOccupiedRoomInpatient
        public static List<OccupiedRoomInpatient> GetOccupiedRoomInpatient(DateTime CensusDate, int HealthcareServiceUnitID, IDbContext ctx = null)
        {
            List<OccupiedRoomInpatient> result = new List<OccupiedRoomInpatient>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(OccupiedRoomInpatient));
                ctx.CommandText = "GetOccupiedRoomInpatient";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((OccupiedRoomInpatient)helper.IDataReaderToObject(reader, new OccupiedRoomInpatient()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetInpatientPatientDiagnoseSurveilans
        public static List<GetInpatientPatientDiagnoseSurveilans> GetInpatientPatientDiagnoseSurveilans(String RegistrationDate, Int32 IsInfectious)
        {
            List<GetInpatientPatientDiagnoseSurveilans> result = new List<GetInpatientPatientDiagnoseSurveilans>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetInpatientPatientDiagnoseSurveilans));
                ctx.CommandText = "GetInpatientPatientDiagnoseSurveilans";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationDate", RegistrationDate);
                ctx.Add("IsInfectious", IsInfectious);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetInpatientPatientDiagnoseSurveilans)helper.IDataReaderToObject(reader, new GetInpatientPatientDiagnoseSurveilans()));
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
        #region GetOutpatientPatientDiagnoseSurveilans
        public static List<GetOutpatientPatientDiagnoseSurveilans> GetOutpatientPatientDiagnoseSurveilans(String RegistrationDate, Int32 IsInfectious)
        {
            List<GetOutpatientPatientDiagnoseSurveilans> result = new List<GetOutpatientPatientDiagnoseSurveilans>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetOutpatientPatientDiagnoseSurveilans));
                ctx.CommandText = "GetOutpatientPatientDiagnoseSurveilans";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationDate", RegistrationDate);
                ctx.Add("IsInfectious", IsInfectious);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetOutpatientPatientDiagnoseSurveilans)helper.IDataReaderToObject(reader, new GetOutpatientPatientDiagnoseSurveilans()));
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
        #region GetOutstandingUDDRegistrationList
        public static List<GetOutstandingUDDRegistrationList> GetOutstandingUDDRegistrationList(Int32 HealthcareServiceUnitID, String RegistrationDate, Int32 SequenceNo)
        {
            List<GetOutstandingUDDRegistrationList> result = new List<GetOutstandingUDDRegistrationList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetOutstandingUDDRegistrationList));
                ctx.CommandText = "GetOutstandingUDDRegistrationList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("RegistrationDate", RegistrationDate);
                ctx.Add("SequenceNo", SequenceNo);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetOutstandingUDDRegistrationList)helper.IDataReaderToObject(reader, new GetOutstandingUDDRegistrationList()));
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
        #region GetParamedicScheduleByDate
        public static List<GetParamedicScheduleByDate> GetParamedicScheduleByDateList(string date)
        {
            List<GetParamedicScheduleByDate> result = new List<GetParamedicScheduleByDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicScheduleByDate));
                ctx.CommandText = "GetParamedicScheduleByDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@Date", date));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicScheduleByDate)helper.IDataReaderToObject(reader, new GetParamedicScheduleByDate()));
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
        #region GetParamedicLeaveScheduleCompare
        public static List<GetParamedicLeaveScheduleCompare> GetParamedicLeaveScheduleCompareList(String Date, Int32 ParamedicID)
        {
            List<GetParamedicLeaveScheduleCompare> result = new List<GetParamedicLeaveScheduleCompare>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicLeaveScheduleCompare));
                ctx.CommandText = "GetParamedicLeaveScheduleCompare";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", Date);
                ctx.Add("ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicLeaveScheduleCompare)helper.IDataReaderToObject(reader, new GetParamedicLeaveScheduleCompare()));
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
        #region GetParamedicLeaveScheduleDate
        public static List<GetParamedicLeaveScheduleDate> GetParamedicLeaveScheduleDateList(Int32 ParamedicID)
        {
            List<GetParamedicLeaveScheduleDate> result = new List<GetParamedicLeaveScheduleDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicLeaveScheduleDate));
                ctx.CommandText = "GetParamedicLeaveScheduleDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicLeaveScheduleDate)helper.IDataReaderToObject(reader, new GetParamedicLeaveScheduleDate()));
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
        public static List<GetParamedicLeaveScheduleDate> GetParamedicLeaveScheduleDateList(Int32 ParamedicID, IDbContext ctx)
        {
            List<GetParamedicLeaveScheduleDate> result = new List<GetParamedicLeaveScheduleDate>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicLeaveScheduleDate));
                ctx.CommandText = "GetParamedicLeaveScheduleDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicLeaveScheduleDate)helper.IDataReaderToObject(reader, new GetParamedicLeaveScheduleDate()));
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
        #region GetParamedicMasterRevenueSharing
        public static List<GetParamedicMasterRevenueSharing> GetParamedicMasterRevenueSharing(int ParamedicID, string ParamDate, int IsExcludeParamDate, int RevenueSharingID, int IsExcludeChargesFilter)
        {
            List<GetParamedicMasterRevenueSharing> result = new List<GetParamedicMasterRevenueSharing>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicMasterRevenueSharing));
                ctx.CommandText = "GetParamedicMasterRevenueSharing";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ParamedicID", ParamedicID);
                ctx.Add("@ParamDate", ParamDate);
                ctx.Add("@IsExcludeParamDate", IsExcludeParamDate);
                ctx.Add("@RevenueSharingID", RevenueSharingID);
                ctx.Add("@IsExcludeChargesFilter", IsExcludeChargesFilter);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicMasterRevenueSharing)helper.IDataReaderToObject(reader, new GetParamedicMasterRevenueSharing()));
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
        #region GetParamedicNoteNotification
        public static List<GetParamedicNoteNotification> GetParamedicNoteNotification()
        {
            List<GetParamedicNoteNotification> result = new List<GetParamedicNoteNotification>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicNoteNotification));
                ctx.CommandText = "GetParamedicNoteNotification";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicNoteNotification)helper.IDataReaderToObject(reader, new GetParamedicNoteNotification()));
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
        #region GetParamedicScheduleClinicStatus
        public static List<GetParamedicScheduleClinicStatus> GetParamedicScheduleClinicStatusList(Int32 ParamNumOfWeek, String ParamDate, Int32 HealthcareServiceUnitID)
        {
            List<GetParamedicScheduleClinicStatus> result = new List<GetParamedicScheduleClinicStatus>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicScheduleClinicStatus));
                ctx.CommandText = "GetParamedicScheduleClinicStatus";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ParamNumOfWeek", ParamNumOfWeek);
                ctx.Add("@ParamDate", ParamDate);
                ctx.Add("@HealthcareServiceUnitID", HealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicScheduleClinicStatus)helper.IDataReaderToObject(reader, new GetParamedicScheduleClinicStatus()));
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
        #region GetParamedicScheduleDateInfo
        public static List<GetParamedicScheduleDateInfo> GetParamedicScheduleDateInfoList(Int32 ParamNumOfWeek, String ParamDate, Int32 HealthcareServiceUnitID)
        {
            List<GetParamedicScheduleDateInfo> result = new List<GetParamedicScheduleDateInfo>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicScheduleDateInfo));
                ctx.CommandText = "GetParamedicScheduleDateInfo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ParamNumOfWeek", ParamNumOfWeek);
                ctx.Add("@ParamDate", ParamDate);
                ctx.Add("@HealthcareServiceUnitID", HealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicScheduleDateInfo)helper.IDataReaderToObject(reader, new GetParamedicScheduleDateInfo()));
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
        #region GetParamedicScheduleSummaryByDate
        public static List<GetParamedicScheduleSummaryByDate> GetParamedicScheduleSummaryByDate(string date)
        {
            List<GetParamedicScheduleSummaryByDate> result = new List<GetParamedicScheduleSummaryByDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicScheduleSummaryByDate));
                ctx.CommandText = "GetParamedicScheduleSummaryByDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", date);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicScheduleSummaryByDate)helper.IDataReaderToObject(reader, new GetParamedicScheduleSummaryByDate()));
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
        #region GetParamedicScheduleSummaryByServiceUnitByDate
        public static List<GetParamedicScheduleSummaryByServiceUnitByDate> GetParamedicScheduleSummaryByServiceUnitByDate(string date, string serviceUnitCode)
        {
            List<GetParamedicScheduleSummaryByServiceUnitByDate> result = new List<GetParamedicScheduleSummaryByServiceUnitByDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicScheduleSummaryByServiceUnitByDate));
                ctx.CommandText = "GetParamedicScheduleSummaryByServiceUnitByDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", date);
                ctx.Add("ServiceUnitCode", serviceUnitCode);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicScheduleSummaryByServiceUnitByDate)helper.IDataReaderToObject(reader, new GetParamedicScheduleSummaryByServiceUnitByDate()));
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
        #region GetParamedicVisitTypeList
        public static List<GetParamedicVisitTypeList> GetParamedicVisitTypeList(int healthcareServiceUnitID, int paramedicID, string filterExpression)
        {
            List<GetParamedicVisitTypeList> result = new List<GetParamedicVisitTypeList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicVisitTypeList));
                ctx.CommandText = "GetParamedicVisitTypeList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareServiceUnitID", healthcareServiceUnitID);
                ctx.Add("p_ParamedicID", paramedicID);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicVisitTypeList)helper.IDataReaderToObject(reader, new GetParamedicVisitTypeList()));
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
        public static List<GetParamedicVisitTypeList> GetParamedicVisitTypeList(int healthcareServiceUnitID, int paramedicID, string filterExpression, IDbContext ctx)
        {
            List<GetParamedicVisitTypeList> result = new List<GetParamedicVisitTypeList>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetParamedicVisitTypeList));
                ctx.CommandText = "GetParamedicVisitTypeList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareServiceUnitID", healthcareServiceUnitID);
                ctx.Add("p_ParamedicID", paramedicID);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetParamedicVisitTypeList)helper.IDataReaderToObject(reader, new GetParamedicVisitTypeList()));
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
        #region GetPartusAndNewBornPerMonthPerYear
        public static List<GetPartusAndNewBornPerMonthPerYear> GetPartusAndNewBornPerMonthPerYear(Int32 Year, Int32 Month)
        {
            List<GetPartusAndNewBornPerMonthPerYear> result = new List<GetPartusAndNewBornPerMonthPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPartusAndNewBornPerMonthPerYear));
                ctx.CommandText = "GetPartusAndNewBornPerMonthPerYear";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPartusAndNewBornPerMonthPerYear)helper.IDataReaderToObject(reader, new GetPartusAndNewBornPerMonthPerYear()));
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
        #region GetPatientATDLogSummary
        public static List<GetPatientATDLogSummary> GetPatientATDLogSummary(DateTime CensusDate, int HealthcareServiceUnitID, IDbContext ctx = null)
        {
            List<GetPatientATDLogSummary> result = new List<GetPatientATDLogSummary>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientATDLogSummary));
                ctx.CommandText = "GetPatientATDLogSummary";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientATDLogSummary)helper.IDataReaderToObject(reader, new GetPatientATDLogSummary()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientCensusPerClassNHS
        public static List<GetPatientCensusPerClassNHS> GetPatientCensusPerClassNHS(int CensusYear, int CensusMonth, IDbContext ctx = null)
        {
            List<GetPatientCensusPerClassNHS> result = new List<GetPatientCensusPerClassNHS>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientCensusPerClassNHS));
                ctx.CommandText = "GetPatientCensusPerClassNHS";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@CensusYear", CensusYear));
                ctx.Command.Parameters.Add(new SqlParameter("@CensusMonth", CensusMonth));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientCensusPerClassNHS)helper.IDataReaderToObject(reader, new GetPatientCensusPerClassNHS()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientChargesDetailKomponen
        public static List<GetPatientChargesDetailKomponen> GetPatientChargesDetailKomponenList(Int32 RegistrationID)
        {
            List<GetPatientChargesDetailKomponen> result = new List<GetPatientChargesDetailKomponen>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesDetailKomponen));
                ctx.CommandText = "GetPatientChargesDetailKomponen";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesDetailKomponen)helper.IDataReaderToObject(reader, new GetPatientChargesDetailKomponen()));
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
        #region GetPatientChargesDetailKomponenMCU
        public static List<GetPatientChargesDetailKomponenMCU> GetPatientChargesDetailKomponenMCUList(Int32 RegistrationID)
        {
            List<GetPatientChargesDetailKomponenMCU> result = new List<GetPatientChargesDetailKomponenMCU>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesDetailKomponenMCU));
                ctx.CommandText = "GetPatientChargesDetailKomponenMCU";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesDetailKomponenMCU)helper.IDataReaderToObject(reader, new GetPatientChargesDetailKomponenMCU()));
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
        #region GetPatientChargesDtPerBillingGroup
        public static List<GetPatientChargesDtPerBillingGroup> GetPatientChargesDtPerBillingGroupList(Int32 RegistrationID, Int32 LinkedRegistratioID)
        {
            List<GetPatientChargesDtPerBillingGroup> result = new List<GetPatientChargesDtPerBillingGroup>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesDtPerBillingGroup));
                ctx.CommandText = "GetPatientChargesDtPerBillingGroup";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("LinkedRegistratioID", LinkedRegistratioID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesDtPerBillingGroup)helper.IDataReaderToObject(reader, new GetPatientChargesDtPerBillingGroup()));
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
        #region GetPatientChargesDtPerBillingGroupReport
        public static List<GetPatientChargesDtPerBillingGroupReport> GetPatientChargesDtPerBillingGroupReport(Int32 RegistrationID)
        {
            List<GetPatientChargesDtPerBillingGroupReport> result = new List<GetPatientChargesDtPerBillingGroupReport>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesDtPerBillingGroupReport));
                ctx.CommandText = "GetPatientChargesDtPerBillingGroupReport";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesDtPerBillingGroupReport)helper.IDataReaderToObject(reader, new GetPatientChargesDtPerBillingGroupReport()));
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
        #region GetPatientChargesDtPerDetail
        public static List<GetPatientChargesDtPerDetail> GetPatientChargesDtPerDetail(Int32 RegistrationID)
        {
            List<GetPatientChargesDtPerDetail> result = new List<GetPatientChargesDtPerDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesDtPerDetail));
                ctx.CommandText = "GetPatientChargesDtPerDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesDtPerDetail)helper.IDataReaderToObject(reader, new GetPatientChargesDtPerDetail()));
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
        #region GetPatientChargesHdChargeClass
        public static List<GetPatientChargesHdChargeClass> GetPatientChargesHdChargeClassList(Int32 RegistrationID, Int32 HealthcareServiceUnitID, String TransactionDateFrom, String TransactionDateTo)
        {
            List<GetPatientChargesHdChargeClass> result = new List<GetPatientChargesHdChargeClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdChargeClass));
                ctx.CommandText = "GetPatientChargesHdChargeClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RegistrationID", RegistrationID);
                ctx.Add("@HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("@TransactionDateFrom", TransactionDateFrom);
                ctx.Add("@TransactionDateTo", TransactionDateTo);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdChargeClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdChargeClass()));
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
        #region GetPatientChargesHdDtALL
        public static List<GetPatientChargesHdDtALL> GetPatientChargesHdDtALLList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALL> result = new List<GetPatientChargesHdDtALL>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALL));
                ctx.CommandText = "GetPatientChargesHdDtALL";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALL)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALL()));
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
        #region GetPatientChargesHdDtALLEng
        public static List<GetPatientChargesHdDtALLEng> GetPatientChargesHdDtALLEngList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLEng> result = new List<GetPatientChargesHdDtALLEng>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLEng));
                ctx.CommandText = "GetPatientChargesHdDtALLEng";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLEng)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLEng()));
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
        #region GetPatientChargesHdDtALLWithOutDP
        public static List<GetPatientChargesHdDtChargesClass> GetPatientChargesHdDtChargesClassList(Int32 RegistrationID, Int32 ClassID)
        {
            List<GetPatientChargesHdDtChargesClass> result = new List<GetPatientChargesHdDtChargesClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtChargesClass));
                ctx.CommandText = "GetPatientChargesHdDtChargesClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("ClassID", ClassID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtChargesClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtChargesClass()));
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
        #region GetPatientChargesHdDtALLWithOutDPIng
        public static List<GetPatientChargesHdDtALLWithOutDPIng> GetPatientChargesHdDtALLWithOutDPIngList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLWithOutDPIng> result = new List<GetPatientChargesHdDtALLWithOutDPIng>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLWithOutDPIng));
                ctx.CommandText = "GetPatientChargesHdDtALLWithOutDPIng";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLWithOutDPIng)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLWithOutDPIng()));
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
        #region GetPatientChargesHdDtALLDiscount
        public static List<GetPatientChargesHdDtALLDiscount> GetPatientChargesHdDtALLDiscountList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLDiscount> result = new List<GetPatientChargesHdDtALLDiscount>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLDiscount));
                ctx.CommandText = "GetPatientChargesHdDtALLDiscount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLDiscount)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLDiscount()));
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
        #region GetPatientChargesHdDtALLNHS
        public static List<GetPatientChargesHdDtALLNHS> GetPatientChargesHdDtALLNHSList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLNHS> result = new List<GetPatientChargesHdDtALLNHS>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLNHS));
                ctx.CommandText = "GetPatientChargesHdDtALLNHS";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLNHS)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLNHS()));
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
        #region GetPatientChargesHdDtAllPerBillingByPaymentReceiptID
        public static List<GetPatientChargesHdDtAllPerBillingByPaymentReceiptID> GetPatientChargesHdDtAllPerBillingByPaymentReceiptIDList(Int32 RegistrationID, Int32 PaymentReceiptID)
        {
            List<GetPatientChargesHdDtAllPerBillingByPaymentReceiptID> result = new List<GetPatientChargesHdDtAllPerBillingByPaymentReceiptID>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerBillingByPaymentReceiptID));
                ctx.CommandText = "GetPatientChargesHdDtAllPerBillingByPaymentReceiptID";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerBillingByPaymentReceiptID)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerBillingByPaymentReceiptID()));
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
        #region GetPatientChargesHdDtWithDetailPackage
        public static List<GetPatientChargesHdDtWithDetailPackage> GetPatientChargesHdDtWithDetailPackageList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtWithDetailPackage> result = new List<GetPatientChargesHdDtWithDetailPackage>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtWithDetailPackage));
                ctx.CommandText = "GetPatientChargesHdDtWithDetailPackage";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtWithDetailPackage)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtWithDetailPackage()));
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
        #region GetPatientChargesHdDtALLWithOutDP
        public static List<GetPatientChargesHdDtALLWithOutDP> GetPatientChargesHdDtALLWithOutDPList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLWithOutDP> result = new List<GetPatientChargesHdDtALLWithOutDP>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLWithOutDP));
                ctx.CommandText = "GetPatientChargesHdDtALLWithOutDP";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLWithOutDP)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLWithOutDP()));
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
        #region GetPatientChargesHdDtALLWithOutDPRSRT
        public static List<GetPatientChargesHdDtALLWithOutDPRSRT> GetPatientChargesHdDtALLWithOutDPRSRTList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLWithOutDPRSRT> result = new List<GetPatientChargesHdDtALLWithOutDPRSRT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLWithOutDPRSRT));
                ctx.CommandText = "GetPatientChargesHdDtALLWithOutDPRSRT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLWithOutDPRSRT)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLWithOutDPRSRT()));
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
        #region GetPatientChargesHdDtALLWithOutDPAllBill
        public static List<GetPatientChargesHdDtALLWithOutDPAllBill> GetPatientChargesHdDtALLWithOutDPAllBillList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLWithOutDPAllBill> result = new List<GetPatientChargesHdDtALLWithOutDPAllBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLWithOutDPAllBill));
                ctx.CommandText = "GetPatientChargesHdDtALLWithOutDPAllBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLWithOutDPAllBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLWithOutDPAllBill()));
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
        #region GetPatientChargesHdDtALLWithOutDPAllBillRSRT
        public static List<GetPatientChargesHdDtALLWithOutDPAllBillRSRT> GetPatientChargesHdDtALLWithOutDPAllBillRSRTList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLWithOutDPAllBillRSRT> result = new List<GetPatientChargesHdDtALLWithOutDPAllBillRSRT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLWithOutDPAllBillRSRT));
                ctx.CommandText = "GetPatientChargesHdDtALLWithOutDPAllBillRSRT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLWithOutDPAllBillRSRT)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLWithOutDPAllBillRSRT()));
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
        #region GetPatientChargesHdDtAll2
        public static List<GetPatientChargesHdDtAll2> GetPatientChargesHdDtAll2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll2> result = new List<GetPatientChargesHdDtAll2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll2));
                ctx.CommandText = "GetPatientChargesHdDtAll2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll2()));
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
        #region GetPatientChargesHdDtAll2BeforeBillChargesClass
        public static List<GetPatientChargesHdDtAll2BeforeBillChargesClass> GetPatientChargesHdDtAll2BeforeBillChargesClassList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll2BeforeBillChargesClass> result = new List<GetPatientChargesHdDtAll2BeforeBillChargesClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll2BeforeBillChargesClass));
                ctx.CommandText = "GetPatientChargesHdDtAll2BeforeBillChargesClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll2BeforeBillChargesClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll2BeforeBillChargesClass()));
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
        #region GetPatientChargesHdDtAll2ChargesClass
        public static List<GetPatientChargesHdDtAll2ChargesClass> GetPatientChargesHdDtAll2ChargesClassList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll2ChargesClass> result = new List<GetPatientChargesHdDtAll2ChargesClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll2ChargesClass));
                ctx.CommandText = "GetPatientChargesHdDtAll2ChargesClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll2ChargesClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll2ChargesClass()));
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
        #region GetPatientChargesHdDtAll2Comp2
        public static List<GetPatientChargesHdDtAll2Comp2> GetPatientChargesHdDtAll2Comp2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll2Comp2> result = new List<GetPatientChargesHdDtAll2Comp2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll2Comp2));
                ctx.CommandText = "GetPatientChargesHdDtAll2Comp2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll2Comp2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll2Comp2()));
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
        #region GetPatientChargesHdDtAll2Comp2ChargesClass
        public static List<GetPatientChargesHdDtAll2Comp2ChargesClass> GetPatientChargesHdDtAll2Comp2ChargesClassList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll2Comp2ChargesClass> result = new List<GetPatientChargesHdDtAll2Comp2ChargesClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll2Comp2ChargesClass));
                ctx.CommandText = "GetPatientChargesHdDtAll2Comp2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll2Comp2ChargesClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll2Comp2ChargesClass()));
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
        #region GetPatientChargesHdDtALL3
        public static List<GetPatientChargesHdDtALL3> GetPatientChargesHdDtALL3List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALL3> result = new List<GetPatientChargesHdDtALL3>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALL3));
                ctx.CommandText = "GetPatientChargesHdDtALL3";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALL3)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALL3()));
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
        #region GetPatientChargesHdDtALLRSPKSB
        public static List<GetPatientChargesHdDtALLRSPKSB> GetPatientChargesHdDtALLRSPKSBList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLRSPKSB> result = new List<GetPatientChargesHdDtALLRSPKSB>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLRSPKSB));
                ctx.CommandText = "GetPatientChargesHdDtALLRSPKSB";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLRSPKSB)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLRSPKSB()));
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
        #region GetPatientChargesHdDtAll3ChargesClass
        public static List<GetPatientChargesHdDtAll3ChargesClass> GetPatientChargesHdDtAll3ChargesClassList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll3ChargesClass> result = new List<GetPatientChargesHdDtAll3ChargesClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll3ChargesClass));
                ctx.CommandText = "GetPatientChargesHdDtAll3ChargesClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll3ChargesClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll3ChargesClass()));
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
        #region GetPatientChargesHdDtAll3Comp2
        public static List<GetPatientChargesHdDtAll3Comp2> GetPatientChargesHdDtAll3Comp2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll3Comp2> result = new List<GetPatientChargesHdDtAll3Comp2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll3Comp2));
                ctx.CommandText = "GetPatientChargesHdDtAll3Comp2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll3Comp2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll3Comp2()));
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
        #region GetPatientChargesHdDtAll4
        public static List<GetPatientChargesHdDtAll4> GetPatientChargesHdDtAll4List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll4> result = new List<GetPatientChargesHdDtAll4>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll4));
                ctx.CommandText = "GetPatientChargesHdDtAll4";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll4)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll4()));
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
        #region GetPatientChargesHdDtAll5
        public static List<GetPatientChargesHdDtAll5> GetPatientChargesHdDtAll5List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll5> result = new List<GetPatientChargesHdDtAll5>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll5));
                ctx.CommandText = "GetPatientChargesHdDtAll5";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll5)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll5()));
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
        #region GetPatientChargesHdDtALL6
        public static List<GetPatientChargesHdDtALL6> GetPatientChargesHdDtALL6List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALL6> result = new List<GetPatientChargesHdDtALL6>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALL6));
                ctx.CommandText = "GetPatientChargesHdDtALL6";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALL6)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALL6()));
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
        #region GetPatientChargesHdDtAll7
        public static List<GetPatientChargesHdDtAll7> GetPatientChargesHdDtAll7List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll7> result = new List<GetPatientChargesHdDtAll7>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll7));
                ctx.CommandText = "GetPatientChargesHdDtAll7";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll7)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll7()));
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
        #region GetPatientChargesHdDtAll8
        public static List<GetPatientChargesHdDtAll8> GetPatientChargesHdDtAll8List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll8> result = new List<GetPatientChargesHdDtAll8>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll8));
                ctx.CommandText = "GetPatientChargesHdDtAll8";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll8)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll8()));
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
        #region GetPatientChargesHdDtAll9
        public static List<GetPatientChargesHdDtAll9> GetPatientChargesHdDtAll9List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll9> result = new List<GetPatientChargesHdDtAll9>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll9));
                ctx.CommandText = "GetPatientChargesHdDtAll9";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll9)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll9()));
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
        #region GetPatientChargesHdDtAll10
        public static List<GetPatientChargesHdDtAll10> GetPatientChargesHdDtAll10List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll10> result = new List<GetPatientChargesHdDtAll10>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll10));
                ctx.CommandText = "GetPatientChargesHdDtAll10";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll10)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll10()));
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
        #region GetPatientChargesHdDtALL11
        public static List<GetPatientChargesHdDtALL11> GetPatientChargesHdDtALL11List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALL11> result = new List<GetPatientChargesHdDtALL11>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALL11));
                ctx.CommandText = "GetPatientChargesHdDtALL11";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALL11)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALL11()));
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
        #region GetPatientChargesHdDtALL12
        public static List<GetPatientChargesHdDtALL12> GetPatientChargesHdDtALL12List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALL12> result = new List<GetPatientChargesHdDtALL12>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALL12));
                ctx.CommandText = "GetPatientChargesHdDtALL12";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALL12)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALL12()));
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
        #region GetPatientChargesHdDtAll13
        public static List<GetPatientChargesHdDtAll13> GetPatientChargesHdDtAll13List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll13> result = new List<GetPatientChargesHdDtAll13>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll13));
                ctx.CommandText = "GetPatientChargesHdDtAll13";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll13)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll13()));
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
        #region GetPatientChargesHdDtAll7BeforeBill
        public static List<GetPatientChargesHdDtAll7BeforeBill> GetPatientChargesHdDtAll7BeforeBillList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll7BeforeBill> result = new List<GetPatientChargesHdDtAll7BeforeBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll7BeforeBill));
                ctx.CommandText = "GetPatientChargesHdDtAll7BeforeBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll7BeforeBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll7BeforeBill()));
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
        #region GetPatientChargesHdDtAll9BeforeBill
        public static List<GetPatientChargesHdDtAll9BeforeBill> GetPatientChargesHdDtAll9BeforeBillList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll9BeforeBill> result = new List<GetPatientChargesHdDtAll9BeforeBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll9BeforeBill));
                ctx.CommandText = "GetPatientChargesHdDtAll9BeforeBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll9BeforeBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll9BeforeBill()));
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
        #region GetPatientChargesHdDtAll10BeforeBill
        public static List<GetPatientChargesHdDtAll10BeforeBill> GetPatientChargesHdDtAll10BeforeBillList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll10BeforeBill> result = new List<GetPatientChargesHdDtAll10BeforeBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll10BeforeBill));
                ctx.CommandText = "GetPatientChargesHdDtAll10BeforeBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll10BeforeBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll10BeforeBill()));
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
        #region GetPatientChargesHdDtAll5Comp2
        public static List<GetPatientChargesHdDtAll5Comp2> GetPatientChargesHdDtAll5Comp2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll5Comp2> result = new List<GetPatientChargesHdDtAll5Comp2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll5Comp2));
                ctx.CommandText = "GetPatientChargesHdDtAll5Comp2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll5Comp2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll5Comp2()));
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
        #region GetPatientChargesHdDtAll6Comp2
        public static List<GetPatientChargesHdDtAll6Comp2> GetPatientChargesHdDtAll6Comp2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll6Comp2> result = new List<GetPatientChargesHdDtAll6Comp2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll6Comp2));
                ctx.CommandText = "GetPatientChargesHdDtAll6Comp2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll6Comp2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll6Comp2()));
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
        #region GetPatientChargesHdDtAll7Comp2
        public static List<GetPatientChargesHdDtAll7Comp2> GetPatientChargesHdDtAll7Comp2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAll7Comp2> result = new List<GetPatientChargesHdDtAll7Comp2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAll7Comp2));
                ctx.CommandText = "GetPatientChargesHdDtAll7Comp2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAll7Comp2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAll7Comp2()));
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
        #region GetPatientChargesHdDtAllBeforeBill
        public static List<GetPatientChargesHdDtAllBeforeBill> GetPatientChargesHdDtAllBeforeBillList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBeforeBill> result = new List<GetPatientChargesHdDtAllBeforeBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBeforeBill));
                ctx.CommandText = "GetPatientChargesHdDtAllBeforeBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBeforeBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBeforeBill()));
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
        #region GetPatientChargesHdDtAllBeforeBillChargesClass
        public static List<GetPatientChargesHdDtAllBeforeBillChargesClass> GetPatientChargesHdDtAllBeforeBillChargesClassList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBeforeBillChargesClass> result = new List<GetPatientChargesHdDtAllBeforeBillChargesClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBeforeBillChargesClass));
                ctx.CommandText = "GetPatientChargesHdDtAllBeforeBillChargesClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBeforeBillChargesClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBeforeBillChargesClass()));
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
        #region GetPatientChargesHdDtAllBeforeBillEng
        public static List<GetPatientChargesHdDtAllBeforeBillEng> GetPatientChargesHdDtAllBeforeBillEngList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBeforeBillEng> result = new List<GetPatientChargesHdDtAllBeforeBillEng>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBeforeBillEng));
                ctx.CommandText = "GetPatientChargesHdDtAllBeforeBillEng";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBeforeBillEng)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBeforeBillEng()));
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
        #region GetPatientChargesHdDtAllBeforeBillPerDate
        public static List<GetPatientChargesHdDtAllBeforeBillPerDate> GetPatientChargesHdDtAllBeforeBillPerDateList(Int32 RegistrationID, String TransactionDate)
        {
            List<GetPatientChargesHdDtAllBeforeBillPerDate> result = new List<GetPatientChargesHdDtAllBeforeBillPerDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBeforeBillPerDate));
                ctx.CommandText = "GetPatientChargesHdDtAllBeforeBillPerDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("TransactionDate", TransactionDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBeforeBillPerDate)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBeforeBillPerDate()));
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
        #region GetPatientChargesHdDtAllBeforeBillPerDate2
        public static List<GetPatientChargesHdDtAllBeforeBillPerDate2> GetPatientChargesHdDtAllBeforeBillPerDate2List(Int32 RegistrationID, String TransactionDate)
        {
            List<GetPatientChargesHdDtAllBeforeBillPerDate2> result = new List<GetPatientChargesHdDtAllBeforeBillPerDate2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBeforeBillPerDate2));
                ctx.CommandText = "GetPatientChargesHdDtAllBeforeBillPerDate2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("TransactionDate", TransactionDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBeforeBillPerDate2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBeforeBillPerDate2()));
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
        #region GetPatientChargesHdDtAllBeforeBillPerServiceUnit
        public static List<GetPatientChargesHdDtAllBeforeBillPerServiceUnit> GetPatientChargesHdDtAllBeforeBillPerServiceUnitList(Int32 RegistrationID, Int32 ItemHealthcareServiceUnitID)
        {
            List<GetPatientChargesHdDtAllBeforeBillPerServiceUnit> result = new List<GetPatientChargesHdDtAllBeforeBillPerServiceUnit>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBeforeBillPerServiceUnit));
                ctx.CommandText = "GetPatientChargesHdDtAllBeforeBillPerServiceUnit";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("ItemHealthcareServiceUnitID", ItemHealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBeforeBillPerServiceUnit)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBeforeBillPerServiceUnit()));
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
        #region GetPatientChargesHdDtAllBill
        public static List<GetPatientChargesHdDtAllBill> GetPatientChargesHdDtAllBillList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBill> result = new List<GetPatientChargesHdDtAllBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBill));
                ctx.CommandText = "GetPatientChargesHdDtAllBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBill()));
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
        #region GetPatientChargesHdDtAllBill2
        public static List<GetPatientChargesHdDtAllBill2> GetPatientChargesHdDtAllBill2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBill2> result = new List<GetPatientChargesHdDtAllBill2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBill2));
                ctx.CommandText = "GetPatientChargesHdDtAllBill2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBill2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBill2()));
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
        #region GetPatientChargesHdDtAllBill2Summary
        public static List<GetPatientChargesHdDtAllBill2Summary> GetPatientChargesHdDtAllBill2SummaryList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBill2Summary> result = new List<GetPatientChargesHdDtAllBill2Summary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBill2Summary));
                ctx.CommandText = "GetPatientChargesHdDtAllBill2Summary";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBill2Summary)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBill2Summary()));
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
        #region GetPatientChargesHdDtAllBillChargesClass
        public static List<GetPatientChargesHdDtAllBillChargesClass> GetPatientChargesHdDtAllBillChargesClassList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBillChargesClass> result = new List<GetPatientChargesHdDtAllBillChargesClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBillChargesClass));
                ctx.CommandText = "GetPatientChargesHdDtAllBillChargesClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBillChargesClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBillChargesClass()));
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
        #region GetPatientChargesHdDtAllBillComp2
        public static List<GetPatientChargesHdDtAllBillComp2> GetPatientChargesHdDtAllBillComp2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBillComp2> result = new List<GetPatientChargesHdDtAllBillComp2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBillComp2));
                ctx.CommandText = "GetPatientChargesHdDtAllBillComp2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBillComp2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBillComp2()));
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
        #region GetPatientChargesHdDtAllBillRSSES
        public static List<GetPatientChargesHdDtAllBillRSSES> GetPatientChargesHdDtAllBillRSSESList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBillRSSES> result = new List<GetPatientChargesHdDtAllBillRSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBillRSSES));
                ctx.CommandText = "GetPatientChargesHdDtAllBillRSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBillRSSES)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBillRSSES()));
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
        #region GetPatientChargesHdDtAllBillRSP
        public static List<GetPatientChargesHdDtAllBillRSP> GetPatientChargesHdDtAllBillRSPList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBillRSP> result = new List<GetPatientChargesHdDtAllBillRSP>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBillRSP));
                ctx.CommandText = "GetPatientChargesHdDtAllBillRSP";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBillRSP)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBillRSP()));
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
        #region GetPatientChargesHdDtAllBillRSPM
        public static List<GetPatientChargesHdDtAllBillRSPM> GetPatientChargesHdDtAllBillRSPMList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBillRSPM> result = new List<GetPatientChargesHdDtAllBillRSPM>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBillRSPM));
                ctx.CommandText = "GetPatientChargesHdDtAllBillRSPM";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBillRSPM)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBillRSPM()));
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
        #region GetPatientChargesHdDtAllBillRSRT
        public static List<GetPatientChargesHdDtAllBillRSRT> GetPatientChargesHdDtAllBillRSRTList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllBillRSRT> result = new List<GetPatientChargesHdDtAllBillRSRT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllBillRSRT));
                ctx.CommandText = "GetPatientChargesHdDtAllBillRSRT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllBillRSRT)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllBillRSRT()));
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
        #region GetPatientChargesHdDtALLChargeClass
        public static List<GetPatientChargesHdDtALLChargeClass> GetPatientChargesHdDtALLChargeClassList(Int32 RegistrationID, Int32 PatientBillingID)
        {
            List<GetPatientChargesHdDtALLChargeClass> result = new List<GetPatientChargesHdDtALLChargeClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLChargeClass));
                ctx.CommandText = "GetPatientChargesHdDtALLChargeClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PatientBillingID", PatientBillingID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLChargeClass)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLChargeClass()));
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
        #region GetPatientChargesHdDtAllMCU
        public static List<GetPatientChargesHdDtAllMCU> GetPatientChargesHdDtAllMCUList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllMCU> result = new List<GetPatientChargesHdDtAllMCU>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllMCU));
                ctx.CommandText = "GetPatientChargesHdDtAllMCU";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllMCU)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllMCU()));
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
        #region GetPatientChargesHdDtAllMCU2
        public static List<GetPatientChargesHdDtAllMCU2> GetPatientChargesHdDtAllMCU2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllMCU2> result = new List<GetPatientChargesHdDtAllMCU2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllMCU2));
                ctx.CommandText = "GetPatientChargesHdDtAllMCU2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllMCU2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllMCU2()));
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
        #region GetPatientChargesHdDtAllMCUReceipt
        public static List<GetPatientChargesHdDtAllMCUReceipt> GetPatientChargesHdDtAllMCUReceipt(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllMCUReceipt> result = new List<GetPatientChargesHdDtAllMCUReceipt>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllMCUReceipt));
                ctx.CommandText = "GetPatientChargesHdDtAllMCUReceipt";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllMCUReceipt)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllMCUReceipt()));
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
        #region GetPatientChargesHdDtAllMCURSSES
        public static List<GetPatientChargesHdDtAllMCURSSES> GetPatientChargesHdDtAllMCURSSESList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllMCURSSES> result = new List<GetPatientChargesHdDtAllMCURSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllMCURSSES));
                ctx.CommandText = "GetPatientChargesHdDtAllMCURSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllMCURSSES)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllMCURSSES()));
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
        #region GetPatientChargesHdDtAllPaymentRSSES
        public static List<GetPatientChargesHdDtAllPaymentRSSES> GetPatientChargesHdDtAllPaymentRSSESList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllPaymentRSSES> result = new List<GetPatientChargesHdDtAllPaymentRSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPaymentRSSES));
                ctx.CommandText = "GetPatientChargesHdDtAllPaymentRSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPaymentRSSES)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPaymentRSSES()));
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
        #region GetPatientChargesHdDtAllPerBilling
        public static List<GetPatientChargesHdDtAllPerBilling> GetPatientChargesHdDtAllPerBillingList(Int32 RegistrationID, Int32 PaymentID)
        {
            List<GetPatientChargesHdDtAllPerBilling> result = new List<GetPatientChargesHdDtAllPerBilling>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerBilling));
                ctx.CommandText = "GetPatientChargesHdDtAllPerBilling";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PaymentID", PaymentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerBilling)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerBilling()));
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
        #region GetPatientChargesHdDtAllPerBillingMCU
        public static List<GetPatientChargesHdDtAllPerBillingMCU> GetPatientChargesHdDtAllPerBillingMCUList(Int32 RegistrationID, Int32 PaymentID)
        {
            List<GetPatientChargesHdDtAllPerBillingMCU> result = new List<GetPatientChargesHdDtAllPerBillingMCU>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerBillingMCU));
                ctx.CommandText = "GetPatientChargesHdDtAllPerBillingMCU";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PaymentID", PaymentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerBillingMCU)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerBillingMCU()));
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
        #region GetPatientChargesHdDtAllPerBillingMCURSSES
        public static List<GetPatientChargesHdDtAllPerBillingMCURSSES> GetPatientChargesHdDtAllPerBillingMCURSSESList(Int32 RegistrationID, Int32 PaymentID)
        {
            List<GetPatientChargesHdDtAllPerBillingMCURSSES> result = new List<GetPatientChargesHdDtAllPerBillingMCURSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerBillingMCURSSES));
                ctx.CommandText = "GetPatientChargesHdDtAllPerBillingMCURSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PaymentID", PaymentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerBillingMCURSSES)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerBillingMCURSSES()));
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
        #region GetPatientChargesHdDtALLPerPayment
        public static List<GetPatientChargesHdDtALLPerPayment> GetPatientChargesHdDtALLPerPaymentList(Int32 PaymentReceiptID, Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLPerPayment> result = new List<GetPatientChargesHdDtALLPerPayment>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLPerPayment));
                ctx.CommandText = "GetPatientChargesHdDtALLPerPayment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@PaymentReceiptID", PaymentReceiptID);
                ctx.Add("@RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLPerPayment)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLPerPayment()));
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
        #region GetPatientChargesHdDtALLPerPayment2
        public static List<GetPatientChargesHdDtALLPerPayment2> GetPatientChargesHdDtALLPerPayment2List(Int32 PaymentReceiptID, Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLPerPayment2> result = new List<GetPatientChargesHdDtALLPerPayment2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLPerPayment2));
                ctx.CommandText = "GetPatientChargesHdDtALLPerPayment2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@PaymentReceiptID", PaymentReceiptID);
                ctx.Add("@RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLPerPayment2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLPerPayment2()));
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
        #region GetPatientChargesHdDtALLPerPayment3
        public static List<GetPatientChargesHdDtALLPerPayment3> GetPatientChargesHdDtALLPerPayment3List(Int32 PaymentReceiptID, Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtALLPerPayment3> result = new List<GetPatientChargesHdDtALLPerPayment3>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtALLPerPayment3));
                ctx.CommandText = "GetPatientChargesHdDtALLPerPayment3";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@PaymentReceiptID", PaymentReceiptID);
                ctx.Add("@RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtALLPerPayment3)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtALLPerPayment3()));
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
        #region GetPatientChargesHdDtAllPerServiceUnit
        public static List<GetPatientChargesHdDtAllPerServiceUnit> GetPatientChargesHdDtAllPerServiceUnitList(Int32 RegistrationID, Int32 ItemHealthcareServiceUnitID)
        {
            List<GetPatientChargesHdDtAllPerServiceUnit> result = new List<GetPatientChargesHdDtAllPerServiceUnit>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerServiceUnit));
                ctx.CommandText = "GetPatientChargesHdDtAllPerServiceUnit";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("ItemHealthcareServiceUnitID", ItemHealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerServiceUnit)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerServiceUnit()));
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
        #region GetPatientChargesHdDtAllPerServiceUnit1
        public static List<GetPatientChargesHdDtAllPerServiceUnit1> GetPatientChargesHdDtAllPerServiceUnit1List(Int32 RegistrationID, Int32 ItemHealthcareServiceUnitID)
        {
            List<GetPatientChargesHdDtAllPerServiceUnit1> result = new List<GetPatientChargesHdDtAllPerServiceUnit1>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerServiceUnit1));
                ctx.CommandText = "GetPatientChargesHdDtAllPerServiceUnit1";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("ItemHealthcareServiceUnitID", ItemHealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerServiceUnit1)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerServiceUnit1()));
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
        #region GetPatientChargesHdDtAllPerServiceUnitRSMD
        public static List<GetPatientChargesHdDtAllPerServiceUnitRSMD> GetPatientChargesHdDtAllPerServiceUnitRSMDList(Int32 RegistrationID, Int32 ItemHealthcareServiceUnitID)
        {
            List<GetPatientChargesHdDtAllPerServiceUnitRSMD> result = new List<GetPatientChargesHdDtAllPerServiceUnitRSMD>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerServiceUnitRSMD));
                ctx.CommandText = "GetPatientChargesHdDtAllPerServiceUnitRSMD";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("ItemHealthcareServiceUnitID", ItemHealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerServiceUnitRSMD)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerServiceUnitRSMD()));
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
        #region GetPatientChargesHdDtAllPerServiceUnitBeforeBill
        public static List<GetPatientChargesHdDtAllPerServiceUnitBeforeBill> GetPatientChargesHdDtAllPerServiceUnitBeforeBillList(Int32 RegistrationID, Int32 ItemHealthcareServiceUnitID)
        {
            List<GetPatientChargesHdDtAllPerServiceUnitBeforeBill> result = new List<GetPatientChargesHdDtAllPerServiceUnitBeforeBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllPerServiceUnitBeforeBill));
                ctx.CommandText = "GetPatientChargesHdDtAllPerServiceUnitBeforeBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("ItemHealthcareServiceUnitID", ItemHealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllPerServiceUnitBeforeBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllPerServiceUnitBeforeBill()));
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
        #region GetPatientChargesHdDtAllRSSES
        public static List<GetPatientChargesHdDtAllRSSES> GetPatientChargesHdDtAllRSSESList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllRSSES> result = new List<GetPatientChargesHdDtAllRSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllRSSES));
                ctx.CommandText = "GetPatientChargesHdDtAllRSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllRSSES)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllRSSES()));
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
        #region GetPatientChargesHdDtAllRSRT2
        public static List<GetPatientChargesHdDtAllRSRT2> GetPatientChargesHdDtAllRSRT2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllRSRT2> result = new List<GetPatientChargesHdDtAllRSRT2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllRSRT2));
                ctx.CommandText = "GetPatientChargesHdDtAll2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllRSRT2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllRSRT2()));
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
        #region GetPatientChargesHdDtAllRSP2
        public static List<GetPatientChargesHdDtAllRSP2> GetPatientChargesHdDtAllRSP2List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllRSP2> result = new List<GetPatientChargesHdDtAllRSP2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllRSP2));
                ctx.CommandText = "GetPatientChargesHdDtAllRSP2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllRSP2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllRSP2()));
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
        #region GetPatientChargesHdDtAllRSPM
        public static List<GetPatientChargesHdDtAllRSPM> GetPatientChargesHdDtAllRSPMList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllRSPM> result = new List<GetPatientChargesHdDtAllRSPM>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllRSPM));
                ctx.CommandText = "GetPatientChargesHdDtAllRSPM";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllRSPM)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllRSPM()));
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
        #region GetPatientChargesHdDtAllRSPW
        public static List<GetPatientChargesHdDtAllRSPW> GetPatientChargesHdDtAllRSPWList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtAllRSPW> result = new List<GetPatientChargesHdDtAllRSPW>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtAllRSPW));
                ctx.CommandText = "GetPatientChargesHdDtAllRSPW";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtAllRSPW)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtAllRSPW()));
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
        #region GetPatientChargesHdDtBillingGroupDiscountALL
        public static List<GetPatientChargesHdDtBillingGroupDiscountALL> GetPatientChargesHdDtBillingGroupDiscountALLList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtBillingGroupDiscountALL> result = new List<GetPatientChargesHdDtBillingGroupDiscountALL>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtBillingGroupDiscountALL));
                ctx.CommandText = "GetPatientChargesHdDtBillingGroupDiscountALL";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtBillingGroupDiscountALL)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtBillingGroupDiscountALL()));
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
        #region GetPatientChargesHdDtEKlaimParameter
        public static List<GetPatientChargesHdDtEKlaimParameter> GetPatientChargesHdDtEKlaimParameterList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtEKlaimParameter> result = new List<GetPatientChargesHdDtEKlaimParameter>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtEKlaimParameter));
                ctx.CommandText = "GetPatientChargesHdDtEKlaimParameter";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtEKlaimParameter)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtEKlaimParameter()));
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
        #region GetPatientChargesHdDtEKlaimParameter1
        public static List<GetPatientChargesHdDtEKlaimParameter1> GetPatientChargesHdDtEKlaimParameter1List(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtEKlaimParameter1> result = new List<GetPatientChargesHdDtEKlaimParameter1>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtEKlaimParameter1));
                ctx.CommandText = "GetPatientChargesHdDtEKlaimParameter1";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtEKlaimParameter1)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtEKlaimParameter1()));
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
        #region GetPatientChargesHdDtEKlaimParameterDetail
        public static List<GetPatientChargesHdDtEKlaimParameterDetail> GetPatientChargesHdDtEKlaimParameterDetailList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtEKlaimParameterDetail> result = new List<GetPatientChargesHdDtEKlaimParameterDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtEKlaimParameterDetail));
                ctx.CommandText = "GetPatientChargesHdDtEKlaimParameterDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtEKlaimParameterDetail)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtEKlaimParameterDetail()));
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
        #region GetPatientChargesHdDtEKlaimParameterDetailPerBilling
        public static List<GetPatientChargesHdDtEKlaimParameterDetailPerBilling> GetPatientChargesHdDtEKlaimParameterDetailPerBillingList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtEKlaimParameterDetailPerBilling> result = new List<GetPatientChargesHdDtEKlaimParameterDetailPerBilling>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtEKlaimParameterDetailPerBilling));
                ctx.CommandText = "GetPatientChargesHdDtEKlaimParameterDetailPerBilling";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtEKlaimParameterDetailPerBilling)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtEKlaimParameterDetailPerBilling()));
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
        #region GetPatientChargesHdDtEKlaimParameterPerBilling
        public static List<GetPatientChargesHdDtEKlaimParameterPerBilling> GetPatientChargesHdDtEKlaimParameterPerBillingList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtEKlaimParameterPerBilling> result = new List<GetPatientChargesHdDtEKlaimParameterPerBilling>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtEKlaimParameterPerBilling));
                ctx.CommandText = "GetPatientChargesHdDtEKlaimParameterPerBilling";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtEKlaimParameterPerBilling)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtEKlaimParameterPerBilling()));
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
        #region GetPatientChargesHdDtForRevenueSharingPerRegistrationList
        public static List<GetPatientChargesHdDtForRevenueSharingPerRegistration> GetPatientChargesHdDtForRevenueSharingPerRegistrationList(int RegistrationID, IDbContext ctx = null)
        {
            List<GetPatientChargesHdDtForRevenueSharingPerRegistration> result = new List<GetPatientChargesHdDtForRevenueSharingPerRegistration>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtForRevenueSharingPerRegistration));
                ctx.CommandText = "GetPatientChargesHdDtForRevenueSharingPerRegistration";
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Command.CommandTimeout = 200;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@RegistrationID", RegistrationID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtForRevenueSharingPerRegistration)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtForRevenueSharingPerRegistration()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientChargesHdDtParamedic
        public static List<GetPatientChargesHdDtParamedic> GetPatientChargesHdDtParamedicList(Int32 TransactionID)
        {
            List<GetPatientChargesHdDtParamedic> result = new List<GetPatientChargesHdDtParamedic>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtParamedic));
                ctx.CommandText = "GetPatientChargesHdDtParamedic";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtParamedic)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtParamedic()));
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
        #region GetPatientChargesHdDtParamedicInfo
        public static List<GetPatientChargesHdDtParamedicInfo> GetPatientChargesHdDtParamedicInfoList(Int32 TransactionID)
        {
            List<GetPatientChargesHdDtParamedicInfo> result = new List<GetPatientChargesHdDtParamedicInfo>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtParamedicInfo));
                ctx.CommandText = "GetPatientChargesHdDtParamedic";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtParamedicInfo)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtParamedicInfo()));
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
        #region GetPatientChargesHdDtParamedicRSRT
        public static List<GetPatientChargesHdDtParamedicRSRT> GetPatientChargesHdDtParamedicRSRTList(Int32 TransactionID)
        {
            List<GetPatientChargesHdDtParamedicRSRT> result = new List<GetPatientChargesHdDtParamedicRSRT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtParamedicRSRT));
                ctx.CommandText = "GetPatientChargesHdDtParamedicRSRT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtParamedicRSRT)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtParamedicRSRT()));
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
        #region GetPatientChargesHdDtPatientBill
        public static List<GetPatientChargesHdDtPatientBill> GetPatientChargesHdDtPatientBillList(Int32 RegistrationID, Int32 PatientBillingID)
        {
            List<GetPatientChargesHdDtPatientBill> result = new List<GetPatientChargesHdDtPatientBill>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtPatientBill));
                ctx.CommandText = "GetPatientChargesHdDtPatientBill";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PatientBillingID", PatientBillingID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtPatientBill)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtPatientBill()));
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
        #region GetPatientChargesHdDtPatientBill2
        public static List<GetPatientChargesHdDtPatientBill2> GetPatientChargesHdDtPatientBill2List(Int32 RegistrationID, Int32 PatientBillingID)
        {
            List<GetPatientChargesHdDtPatientBill2> result = new List<GetPatientChargesHdDtPatientBill2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtPatientBill2));
                ctx.CommandText = "GetPatientChargesHdDtPatientBill2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PatientBillingID", PatientBillingID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtPatientBill2)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtPatientBill2()));
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
        #region GetPatientChargesHdDtPatientBill5
        public static List<GetPatientChargesHdDtPatientBill5> GetPatientChargesHdDtPatientBill5List(Int32 RegistrationID, Int32 PatientBillingID)
        {
            List<GetPatientChargesHdDtPatientBill5> result = new List<GetPatientChargesHdDtPatientBill5>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtPatientBill5));
                ctx.CommandText = "GetPatientChargesHdDtPatientBill5";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PatientBillingID", PatientBillingID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtPatientBill5)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtPatientBill5()));
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
        #region GetPatientChargesHdDtPerBillingRSSES
        public static List<GetPatientChargesHdDtPerBillingRSSES> GetPatientChargesHdDtPerBillingRSSESList(Int32 RegistrationID, Int32 PatientBillingID)
        {
            List<GetPatientChargesHdDtPerBillingRSSES> result = new List<GetPatientChargesHdDtPerBillingRSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtPerBillingRSSES));
                ctx.CommandText = "GetPatientChargesHdDtPerBillingRSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PatientBillingID", PatientBillingID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtPerBillingRSSES)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtPerBillingRSSES()));
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
        #region GetPatientChargesHdDtPerBillingRSPM
        public static List<GetPatientChargesHdDtPerBillingRSPM> GetPatientChargesHdDtPerBillingRSPMList(Int32 RegistrationID, Int32 PatientBillingID)
        {
            List<GetPatientChargesHdDtPerBillingRSPM> result = new List<GetPatientChargesHdDtPerBillingRSPM>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtPerBillingRSPM));
                ctx.CommandText = "GetPatientChargesHdDtPerBillingRSPM";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("PatientBillingID", PatientBillingID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtPerBillingRSPM)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtPerBillingRSPM()));
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
        #region GetPatientChargesHdDtPerPaymentRSSES
        public static List<GetPatientChargesHdDtPerPaymentRSSES> GetPatientChargesHdDtPerPaymentRSSESList(Int32 RegistrationID, Int32 PaymentID)
        {
            List<GetPatientChargesHdDtPerPaymentRSSES> result = new List<GetPatientChargesHdDtPerPaymentRSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtPerPaymentRSSES));
                ctx.CommandText = "GetPatientChargesHdDtPerPaymentRSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RegistrationID", RegistrationID);
                ctx.Add("@PaymentID", PaymentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtPerPaymentRSSES)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtPerPaymentRSSES()));
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
        #region GetPatientChargesHdDtRecalculateBillingGroup
        public static List<GetPatientChargesHdDtRecalculateBillingGroup> GetPatientChargesHdDtRecalculateBillingGroupList(Int32 RegistrationID)
        {
            List<GetPatientChargesHdDtRecalculateBillingGroup> result = new List<GetPatientChargesHdDtRecalculateBillingGroup>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdDtRecalculateBillingGroup));
                ctx.CommandText = "GetPatientChargesHdDtRecalculateBillingGroup";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdDtRecalculateBillingGroup)helper.IDataReaderToObject(reader, new GetPatientChargesHdDtRecalculateBillingGroup()));
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
        #region GetPatientChargesHdPerRegistration
        public static List<GetPatientChargesHdPerRegistration> GetPatientChargesHdPerRegistration(Int32 registrationID, IDbContext ctx = null)
        {
            List<GetPatientChargesHdPerRegistration> result = new List<GetPatientChargesHdPerRegistration>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdPerRegistration));
                ctx.CommandText = "GetPatientChargesHdPerRegistration";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RegistrationID", registrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdPerRegistration)helper.IDataReaderToObject(reader, new GetPatientChargesHdPerRegistration()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientChargesHdPerRegistrationPerRequestIDRowCount
        public static Int32 GetPatientChargesHdPerRegistrationPerRequestIDRowCount(Int32 RegistrationID, String RequestID, Int32 RegistrationHealthcareServiceUnitID, IDbContext ctx)
        {
            List<GetPatientChargesHdPerRegistration> result = new List<GetPatientChargesHdPerRegistration>();
            SqlParameter param = new SqlParameter();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdPerRegistration));
                ctx.CommandText = "GetPatientChargesHdPerRegistrationPerRequestIDRowCount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RegistrationID", RegistrationID);
                ctx.Add("@RequestID", RequestID);
                ctx.Add("@RegistrationHealthcareServiceUnitID", RegistrationHealthcareServiceUnitID);

                param.ParameterName = "@Result";
                param.SqlDbType = SqlDbType.Int;
                param.Size = 20;
                param.Direction = ParameterDirection.Output;

                ctx.Command.Parameters.Add(param);
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            return (Int32)param.Value;
        }
        public static Int32 GetPatientChargesHdPerRegistrationPerRequestIDRowCount(Int32 RegistrationID, String RequestID, Int32 RegistrationHealthcareServiceUnitID)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetPatientChargesHdPerRegistrationPerRequestIDRowCount(RegistrationID, RequestID, RegistrationHealthcareServiceUnitID, ctx);
        }
        #endregion
        #region GetPatientChargesHdRevenueSharingList
        public static List<GetPatientChargesHdRevenueSharing> GetPatientChargesHdRevenueSharingList(int ParamedicID, String DepartmentID, int CustomerID, int ExCustomerID, DateTime FromDate, DateTime ToDate, IDbContext ctx = null)
        {
            List<GetPatientChargesHdRevenueSharing> result = new List<GetPatientChargesHdRevenueSharing>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdRevenueSharing));
                ctx.CommandText = "GetPatientChargesHdRevenueSharingList";
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Command.CommandTimeout = 200;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", ParamedicID));
                ctx.Command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
                ctx.Command.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
                ctx.Command.Parameters.Add(new SqlParameter("@ExCustomerID", ExCustomerID));
                ctx.Command.Parameters.Add(new SqlParameter("@FromDate", FromDate));
                ctx.Command.Parameters.Add(new SqlParameter("@ToDate", ToDate));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdRevenueSharing)helper.IDataReaderToObject(reader, new GetPatientChargesHdRevenueSharing()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientChargesHdRevenueSharing1List
        public static List<GetPatientChargesHdRevenueSharing1> GetPatientChargesHdRevenueSharing1List(int ParamedicID, String DepartmentID, int CustomerID, int ExCustomerID, String PaidType, String ClinicGroup, String PeriodeType, DateTime FromDate, DateTime ToDate, String FromTime, String ToTime, Int32 RevenueSharingID, Int32 RegistrationStatus, Int32 BPJSStatus, Int32 PageIndex, Int32 NumRows, String SortBy, IDbContext ctx = null)
        {
            List<GetPatientChargesHdRevenueSharing1> result = new List<GetPatientChargesHdRevenueSharing1>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdRevenueSharing1));
                ctx.CommandText = "GetPatientChargesHdRevenueSharing1";
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Command.CommandTimeout = 200;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", ParamedicID));
                ctx.Command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
                ctx.Command.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
                ctx.Command.Parameters.Add(new SqlParameter("@ExCustomerID", ExCustomerID));
                ctx.Command.Parameters.Add(new SqlParameter("@PaidType", PaidType));
                ctx.Command.Parameters.Add(new SqlParameter("@ClinicGroup", ClinicGroup));
                ctx.Command.Parameters.Add(new SqlParameter("@PeriodeType", PeriodeType));
                ctx.Command.Parameters.Add(new SqlParameter("@FromDate", FromDate));
                ctx.Command.Parameters.Add(new SqlParameter("@ToDate", ToDate));
                ctx.Command.Parameters.Add(new SqlParameter("@FromTime", FromTime));
                ctx.Command.Parameters.Add(new SqlParameter("@ToTime", ToTime));
                ctx.Command.Parameters.Add(new SqlParameter("@RevenueSharingID", RevenueSharingID));
                ctx.Command.Parameters.Add(new SqlParameter("@RegistrationStatus", RegistrationStatus));
                ctx.Command.Parameters.Add(new SqlParameter("@BPJSStatus", BPJSStatus));
                ctx.Command.Parameters.Add(new SqlParameter("@PageIndex", PageIndex));
                ctx.Command.Parameters.Add(new SqlParameter("@NumRows", NumRows));
                ctx.Command.Parameters.Add(new SqlParameter("@SortBy", SortBy));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdRevenueSharing1)helper.IDataReaderToObject(reader, new GetPatientChargesHdRevenueSharing1()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientChargesHdRevenueSharing2List
        public static List<GetPatientChargesHdRevenueSharing2> GetPatientChargesHdRevenueSharing2List(int ParamedicID, String DepartmentID, int CustomerID, int ExCustomerID, String PaidType, String ClinicGroup, String PeriodeType, DateTime FromDate, DateTime ToDate, String FromTime, String ToTime, int RevenueSharingID, Int32 PageIndex, Int32 NumRows, String SortBy, IDbContext ctx = null)
        {
            List<GetPatientChargesHdRevenueSharing2> result = new List<GetPatientChargesHdRevenueSharing2>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesHdRevenueSharing2));
                ctx.CommandText = "GetPatientChargesHdRevenueSharing2";
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Command.CommandTimeout = 200;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", ParamedicID));
                ctx.Command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
                ctx.Command.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
                ctx.Command.Parameters.Add(new SqlParameter("@ExCustomerID", ExCustomerID));
                ctx.Command.Parameters.Add(new SqlParameter("@PaidType", PaidType));
                ctx.Command.Parameters.Add(new SqlParameter("@ClinicGroup", ClinicGroup));
                ctx.Command.Parameters.Add(new SqlParameter("@PeriodeType", PeriodeType));
                ctx.Command.Parameters.Add(new SqlParameter("@FromDate", FromDate));
                ctx.Command.Parameters.Add(new SqlParameter("@ToDate", ToDate));
                ctx.Command.Parameters.Add(new SqlParameter("@FromTime", FromTime));
                ctx.Command.Parameters.Add(new SqlParameter("@ToTime", ToTime));
                ctx.Command.Parameters.Add(new SqlParameter("@RevenueSharingID", RevenueSharingID));
                ctx.Command.Parameters.Add(new SqlParameter("@PageIndex", PageIndex));
                ctx.Command.Parameters.Add(new SqlParameter("@NumRows", NumRows));
                ctx.Command.Parameters.Add(new SqlParameter("@SortBy", SortBy));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesHdRevenueSharing2)helper.IDataReaderToObject(reader, new GetPatientChargesHdRevenueSharing2()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientChargesDtRevenueSharingList
        public static List<GetPatientChargesDtRevenueSharing> GetPatientChargesDtRevenueSharingList(int TransactionDtID, int ParamedicID, IDbContext ctx = null)
        {
            List<GetPatientChargesDtRevenueSharing> result = new List<GetPatientChargesDtRevenueSharing>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesDtRevenueSharing));
                ctx.CommandText = "GetPatientChargesDtRevenueSharingList";
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Command.CommandTimeout = 200;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@TransactionDtID", TransactionDtID));
                ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", ParamedicID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesDtRevenueSharing)helper.IDataReaderToObject(reader, new GetPatientChargesDtRevenueSharing()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientChargesValidationDoubleInput
        public static List<GetPatientChargesValidationDoubleInput> GetPatientChargesValidationDoubleInputList(int visitID, int transactionID, string transactionDate, IDbContext ctx)
        {
            List<GetPatientChargesValidationDoubleInput> result = new List<GetPatientChargesValidationDoubleInput>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientChargesValidationDoubleInput));
                ctx.CommandText = "GetPatientChargesValidationDoubleInput";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", visitID);
                ctx.Add("TransactionID", transactionID);
                ctx.Add("TransactionDate", transactionDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientChargesValidationDoubleInput)helper.IDataReaderToObject(reader, new GetPatientChargesValidationDoubleInput()));
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
        public static List<GetPatientChargesValidationDoubleInput> GetPatientChargesValidationDoubleInputList(int visitID, int transactionID, string transactionDate)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetPatientChargesValidationDoubleInputList(visitID, transactionID, transactionDate, ctx);
        }
        #endregion
        #region GetPatientDischargePerAgePerClass
        public static List<GetPatientDischargePerAgePerClass> GetPatientDischargePerAgePerClassList(Int32 Year, Int32 Month)
        {
            List<GetPatientDischargePerAgePerClass> result = new List<GetPatientDischargePerAgePerClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientDischargePerAgePerClass));
                ctx.CommandText = "GetPatientDischargePerAgePerClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientDischargePerAgePerClass)helper.IDataReaderToObject(reader, new GetPatientDischargePerAgePerClass()));
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
        #region GetPatientDischargePerAgePerFloor
        public static List<GetPatientDischargePerAgePerFloor> GetPatientDischargePerAgePerFloorList(Int32 Year, Int32 Month)
        {
            List<GetPatientDischargePerAgePerFloor> result = new List<GetPatientDischargePerAgePerFloor>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientDischargePerAgePerFloor));
                ctx.CommandText = "GetPatientDischargePerAgePerFloor";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientDischargePerAgePerFloor)helper.IDataReaderToObject(reader, new GetPatientDischargePerAgePerFloor()));
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
        #region GetPatientDischargePerAgePerSMF
        public static List<GetPatientDischargePerAgePerSMF> GetPatientDischargePerAgePerSMFList(Int32 Year, Int32 Month)
        {
            List<GetPatientDischargePerAgePerSMF> result = new List<GetPatientDischargePerAgePerSMF>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientDischargePerAgePerSMF));
                ctx.CommandText = "GetPatientDischargePerAgePerSMF";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientDischargePerAgePerSMF)helper.IDataReaderToObject(reader, new GetPatientDischargePerAgePerSMF()));
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
        #region GetPatientDoctorListSummary
        public static List<GetPatientDoctorListSummary> GetPatientDoctorListSummary(Int32 ParamedicID, String DepartmentID, String VisitDate, Int32 IncludeClosedRegistration, Int32 HealthcareServiceUnitID)
        {
            List<GetPatientDoctorListSummary> result = new List<GetPatientDoctorListSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientDoctorListSummary));
                ctx.CommandText = "GetPatientDoctorListSummary";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("IncludeClosedRegistration", IncludeClosedRegistration);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientDoctorListSummary)helper.IDataReaderToObject(reader, new GetPatientDoctorListSummary()));
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
        #region GetPatientDoctorListSummaryDetail
        public static List<GetPatientDoctorListSummaryDetail> GetPatientDoctorListSummaryDetail(Int32 ParamedicID, String DepartmentID, String VisitDate, Int32 IncludeClosedRegistration, Int32 HealthcareServiceUnitID, String PatientName)
        {
            List<GetPatientDoctorListSummaryDetail> result = new List<GetPatientDoctorListSummaryDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientDoctorListSummaryDetail));
                ctx.CommandText = "GetPatientDoctorListSummaryDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("IncludeClosedRegistration", IncludeClosedRegistration);
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("PatientName", PatientName);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientDoctorListSummaryDetail)helper.IDataReaderToObject(reader, new GetPatientDoctorListSummaryDetail()));
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
        #region GetPatientForArchived
        public static List<GetPatientForArchived> GetPatientForArchivedList(string LastVisitDate, bool IsIgnoreDate)
        {
            List<GetPatientForArchived> result = new List<GetPatientForArchived>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientForArchived));
                ctx.CommandText = "GetPatientForArchived";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@LastVisitDate", LastVisitDate);
                ctx.Add("@IsIgnoreDate", IsIgnoreDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientForArchived)helper.IDataReaderToObject(reader, new GetPatientForArchived()));
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
        #region GetPatientForRetention
        public static List<GetPatientForRetention> GetPatientForRetentionList(string LastVisitDate, bool IsIgnoreDate)
        {
            List<GetPatientForRetention> result = new List<GetPatientForRetention>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientForRetention));
                ctx.CommandText = "GetPatientForRetention";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@LastVisitDate", LastVisitDate);
                ctx.Add("@IsIgnoreDate", IsIgnoreDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientForRetention)helper.IDataReaderToObject(reader, new GetPatientForRetention()));
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
        #region GetPatientInfoByDiagnoseID
        public static List<GetPatientInfoByDiagnoseID> GetPatientInfoByDiagnoseID(string diagnoseFrom, string diagnoseTo, string startDate, string endDate)
        {
            List<GetPatientInfoByDiagnoseID> result = new List<GetPatientInfoByDiagnoseID>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientInfoByDiagnoseID));
                ctx.CommandText = "GetPatientInfoByDiagnoseID";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("DiagnoseFrom", diagnoseFrom);
                ctx.Add("DiagnoseTo", diagnoseTo);
                ctx.Add("StartDate", startDate);
                ctx.Add("EndDate", endDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientInfoByDiagnoseID)helper.IDataReaderToObject(reader, new GetPatientInfoByDiagnoseID()));
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
        #region GetPatientInformation
        public static List<GetPatientInformation> GetPatientInformation(string year, string month)
        {
            List<GetPatientInformation> result = new List<GetPatientInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientInformation));
                ctx.CommandText = "GetPatientInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("year", year);
                ctx.Add("month", month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientInformation)helper.IDataReaderToObject(reader, new GetPatientInformation()));
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
        #region GetPatientNewVisit
        public static List<GetPatientNewVisit> GetGetPatientNewVisitList(string filterExpression)
        {
            List<GetPatientNewVisit> result = new List<GetPatientNewVisit>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientNewVisit));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientNewVisit)helper.IDataReaderToObject(reader, new GetPatientNewVisit()));
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
        #region GetPatientPaymentDtARPatient
        public static List<GetPatientPaymentDtARPatient> GetPatientPaymentDtARPatientList(string MRN, string Date, string DepartmentID, string IsExclusion, string RegistrationStatus, string FilterBy, string SortBy)
        {
            List<GetPatientPaymentDtARPatient> result = new List<GetPatientPaymentDtARPatient>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientPaymentDtARPatient));
                ctx.CommandText = "GetPatientPaymentDtARPatient";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@MRN", MRN);
                ctx.Add("@Date", Date);
                ctx.Add("@DepartmentID", DepartmentID);
                ctx.Add("@IsExclusion", IsExclusion);
                ctx.Add("@RegistrationStatus", RegistrationStatus);
                ctx.Add("@Filter", FilterBy);
                ctx.Add("@Sort", SortBy);
                ctx.Command.CommandTimeout = 300;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientPaymentDtARPatient)helper.IDataReaderToObject(reader, new GetPatientPaymentDtARPatient()));
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
        #region GetPatientPaymentDtARPayer
        public static List<GetPatientPaymentDtARPayer> GetPatientPaymentDtARPayerList(int BusinessPartnerID, string Date, string DepartmentID, string IsExclusion, string RegistrationStatus, string FilterBy, string SortBy)
        {
            List<GetPatientPaymentDtARPayer> result = new List<GetPatientPaymentDtARPayer>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientPaymentDtARPayer));
                ctx.CommandText = "GetPatientPaymentDtARPayer";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@BusinessPartnerID", BusinessPartnerID);
                ctx.Add("@Date", Date);
                ctx.Add("@DepartmentID", DepartmentID);
                ctx.Add("@IsExclusion", IsExclusion);
                ctx.Add("@RegistrationStatus", RegistrationStatus);
                ctx.Add("@Filter", FilterBy);
                ctx.Add("@Sort", SortBy);
                ctx.Command.CommandTimeout = 300;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientPaymentDtARPayer)helper.IDataReaderToObject(reader, new GetPatientPaymentDtARPayer()));
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
        #region GetPatientPaymentDtRekapReportRSRT
        public static List<GetPatientPaymentDtRekapReportRSRT> GetPatientPaymentDtRekapReportRSRT(string Date, string DepartmentID, string GCShift, string StatusTransaksi, int CreatedBy)
        {
            List<GetPatientPaymentDtRekapReportRSRT> result = new List<GetPatientPaymentDtRekapReportRSRT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientPaymentDtRekapReportRSRT));
                ctx.CommandText = "GetPatientPaymentDtRekapReportRSRT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@Date", Date);
                ctx.Add("@DepartmentID", DepartmentID);
                ctx.Add("@GCShift", GCShift);
                ctx.Add("@StatusTransaksi", StatusTransaksi);
                ctx.Add("@CreatedBy", CreatedBy);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientPaymentDtRekapReportRSRT)helper.IDataReaderToObject(reader, new GetPatientPaymentDtRekapReportRSRT()));
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
        #region GetPatientPaymentDtRekapReportRSSY
        public static List<GetPatientPaymentDtRekapReportRSSY> GetPatientPaymentDtRekapReportRSSY(string Date, string DepartmentID, string GCShift, int CreatedBy)
        {
            List<GetPatientPaymentDtRekapReportRSSY> result = new List<GetPatientPaymentDtRekapReportRSSY>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientPaymentDtRekapReportRSSY));
                ctx.CommandText = "GetPatientPaymentDtRekapReportRSSY";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@Date", Date);
                ctx.Add("@DepartmentID", DepartmentID);
                ctx.Add("@GCShift", GCShift);
                //ctx.Add("@StatusTransaksi", StatusTransaksi);
                ctx.Add("@CreatedBy", CreatedBy);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientPaymentDtRekapReportRSSY)helper.IDataReaderToObject(reader, new GetPatientPaymentDtRekapReportRSSY()));
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
        #region GetPatientPaymentReportByDate
        public static List<GetPatientPaymentReportByDate> GetPatientPaymentReportByDateList(string paramDate, string paramTime)
        {
            List<GetPatientPaymentReportByDate> result = new List<GetPatientPaymentReportByDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientPaymentReportByDate));
                ctx.CommandText = "GetPatientPaymentReportByDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", paramDate);
                ctx.Add("Jam", paramTime);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientPaymentReportByDate)helper.IDataReaderToObject(reader, new GetPatientPaymentReportByDate()));
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
        #region GetPatientRecapitulationPerMonthPerDepartment
        public static List<GetPatientRecapitulationPerMonthPerDepartment> GetPatientRecapitulationPerMonthPerDepartment(String VisitDate, String DepartmentID)
        {
            List<GetPatientRecapitulationPerMonthPerDepartment> result = new List<GetPatientRecapitulationPerMonthPerDepartment>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientRecapitulationPerMonthPerDepartment));
                ctx.CommandText = "GetPatientRecapitulationPerMonthPerDepartment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("DepartmentID", DepartmentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientRecapitulationPerMonthPerDepartment)helper.IDataReaderToObject(reader, new GetPatientRecapitulationPerMonthPerDepartment()));
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
        #region GetPatientVisitPerAgePerClass
        public static List<GetPatientVisitPerAgePerClass> GetPatientVisitPerAgePerClassList(Int32 Year, Int32 Month)
        {
            List<GetPatientVisitPerAgePerClass> result = new List<GetPatientVisitPerAgePerClass>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientVisitPerAgePerClass));
                ctx.CommandText = "GetPatientVisitPerAgePerClass";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientVisitPerAgePerClass)helper.IDataReaderToObject(reader, new GetPatientVisitPerAgePerClass()));
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
        #region GetPatientVisitPerAgePerFloor
        public static List<GetPatientVisitPerAgePerFloor> GetPatientVisitPerAgePerFloorList(Int32 Year, Int32 Month)
        {
            List<GetPatientVisitPerAgePerFloor> result = new List<GetPatientVisitPerAgePerFloor>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientVisitPerAgePerFloor));
                ctx.CommandText = "GetPatientVisitPerAgePerFloor";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientVisitPerAgePerFloor)helper.IDataReaderToObject(reader, new GetPatientVisitPerAgePerFloor()));
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
        #region GetPatientVisitPerAgePerSMF
        public static List<GetPatientVisitPerAgePerSMF> GetPatientVisitPerAgePerSMFList(Int32 Year, Int32 Month)
        {
            List<GetPatientVisitPerAgePerSMF> result = new List<GetPatientVisitPerAgePerSMF>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientVisitPerAgePerSMF));
                ctx.CommandText = "GetPatientVisitPerAgePerSMF";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientVisitPerAgePerSMF)helper.IDataReaderToObject(reader, new GetPatientVisitPerAgePerSMF()));
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
        #region GetPatientVisitPerMonthPerYear
        public static List<GetPatientVisitPerMonthPerYear> GetPatientVisitPerMonthPerYearList(int Year, int Month, int StatusKunjungan)
        {
            List<GetPatientVisitPerMonthPerYear> result = new List<GetPatientVisitPerMonthPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientVisitPerMonthPerYear));
                ctx.CommandText = "GetParamedicVisitTypeList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                ctx.Add("StatusKunjungan", StatusKunjungan);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientVisitPerMonthPerYear)helper.IDataReaderToObject(reader, new GetPatientVisitPerMonthPerYear()));
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
        #region GetPaymentReceiptCustom2
        public static List<GetPaymentReceiptCustom2> GetGetPaymentReceiptCustom2List(Int32 PaymentReceiptID)
        {
            List<GetPaymentReceiptCustom2> result = new List<GetPaymentReceiptCustom2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustom2));
                ctx.CommandText = "GetPaymentReceiptCustom2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustom2)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustom2()));
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
        #region GetPaymentReceiptCustomDetailTransaction
        public static List<GetPaymentReceiptCustomDetailTransaction> GetPaymentReceiptCustomDetailTransactionList(Int32 PaymentReceiptID)
        {
            List<GetPaymentReceiptCustomDetailTransaction> result = new List<GetPaymentReceiptCustomDetailTransaction>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomDetailTransaction));
                ctx.CommandText = "GetPaymentReceiptCustomDetailTransaction";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomDetailTransaction)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomDetailTransaction()));
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
        #region GetPaymentReceiptCustomDetailTransaction1
        public static List<GetPaymentReceiptCustomDetailTransaction1> GetPaymentReceiptCustomDetailTransaction1List(Int32 PaymentReceiptID)
        {
            List<GetPaymentReceiptCustomDetailTransaction1> result = new List<GetPaymentReceiptCustomDetailTransaction1>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomDetailTransaction1));
                ctx.CommandText = "GetPaymentReceiptCustomDetailTransaction1";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomDetailTransaction1)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomDetailTransaction1()));
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
        #region GetPaymentReceiptCustomDetailTransaction2
        public static List<GetPaymentReceiptCustomDetailTransaction2> GetPaymentReceiptCustomDetailTransaction2List(Int32 PaymentReceiptID)
        {
            List<GetPaymentReceiptCustomDetailTransaction2> result = new List<GetPaymentReceiptCustomDetailTransaction2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomDetailTransaction2));
                ctx.CommandText = "GetPaymentReceiptCustomDetailTransaction2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomDetailTransaction2)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomDetailTransaction2()));
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
        #region GetPaymentReceiptCustomDetailTransactionDiscount
        public static List<GetPaymentReceiptCustomDetailTransactionDiscount> GetPaymentReceiptCustomDetailTransactionDiscountList(Int32 PaymentReceiptID)
        {
            List<GetPaymentReceiptCustomDetailTransactionDiscount> result = new List<GetPaymentReceiptCustomDetailTransactionDiscount>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomDetailTransactionDiscount));
                ctx.CommandText = "GetPaymentReceiptCustomDetailTransactionDiscount";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomDetailTransactionDiscount)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomDetailTransactionDiscount()));
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
        #region GetPaymentReceiptCustomDetailTransactionGRANOSTIC
        public static List<GetPaymentReceiptCustomDetailTransactionGRANOSTIC> GetPaymentReceiptCustomDetailTransactionGRANOSTICList(Int32 PaymentReceiptID)
        {
            List<GetPaymentReceiptCustomDetailTransactionGRANOSTIC> result = new List<GetPaymentReceiptCustomDetailTransactionGRANOSTIC>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomDetailTransactionGRANOSTIC));
                ctx.CommandText = "GetPaymentReceiptCustomDetailTransactionGRANOSTIC";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomDetailTransactionGRANOSTIC)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomDetailTransactionGRANOSTIC()));
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
        #region GetPaymentReceiptCustomDetailTransactionMCU
        public static List<GetPaymentReceiptCustomDetailTransactionMCU> GetPaymentReceiptCustomDetailTransactionMCUList(Int32 PaymentReceiptID)
        {
            List<GetPaymentReceiptCustomDetailTransactionMCU> result = new List<GetPaymentReceiptCustomDetailTransactionMCU>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomDetailTransactionMCU));
                ctx.CommandText = "GetPaymentReceiptCustomDetailTransactionMCU";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentReceiptID", PaymentReceiptID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomDetailTransactionMCU)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomDetailTransactionMCU()));
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
        #region GetPembelianObatPerPrinciple
        public static List<GetPembelianObatPerPrinciple> GetPembelianObatPerPrincipleList(string filterExpression)
        {
            List<GetPembelianObatPerPrinciple> result = new List<GetPembelianObatPerPrinciple>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPembelianObatPerPrinciple));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPembelianObatPerPrinciple)helper.IDataReaderToObject(reader, new GetPembelianObatPerPrinciple()));
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
        #region GetPembelianObatPerPrincipleRekap
        public static List<GetPembelianObatPerPrincipleRekap> GetPembelianObatPerPrincipleRekapList(string filterExpression)
        {
            List<GetPembelianObatPerPrincipleRekap> result = new List<GetPembelianObatPerPrincipleRekap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPembelianObatPerPrincipleRekap));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPembelianObatPerPrincipleRekap)helper.IDataReaderToObject(reader, new GetPembelianObatPerPrincipleRekap()));
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
        #region GetPendapatanRajal
        public static List<GetPendapatanRajal> GetPendapatanRajal(string TransactionDate)
        {
            List<GetPendapatanRajal> result = new List<GetPendapatanRajal>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPendapatanRajal));
                ctx.CommandText = "GetPendapatanRajal";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_TransactionDate", TransactionDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPendapatanRajal)helper.IDataReaderToObject(reader, new GetPendapatanRajal()));
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
        #region GetPendapatanRanap
        public static List<GetPendapatanRanap> GetPendapatanRanap(string TransactionDate)
        {
            List<GetPendapatanRanap> result = new List<GetPendapatanRanap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPendapatanRanap));
                ctx.CommandText = "GetPendapatanRajal";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_TransactionDate", TransactionDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPendapatanRanap)helper.IDataReaderToObject(reader, new GetPendapatanRanap()));
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
        #region GetPenjualanBahanOperasiBahanOrthopediPMIBDRS
        public static List<GetPenjualanBahanOperasiBahanOrthopediPMIBDRS> GetPenjualanBahanOperasiBahanOrthopediPMIBDRSList(string filterExpression)
        {
            List<GetPenjualanBahanOperasiBahanOrthopediPMIBDRS> result = new List<GetPenjualanBahanOperasiBahanOrthopediPMIBDRS>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPenjualanBahanOperasiBahanOrthopediPMIBDRS));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPenjualanBahanOperasiBahanOrthopediPMIBDRS)helper.IDataReaderToObject(reader, new GetPenjualanBahanOperasiBahanOrthopediPMIBDRS()));
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
        #region GetPermintaanDanRealisasiKasbonSubReport
        public static List<GetPermintaanDanRealisasiKasbonSubReport> GetPermintaanDanRealisasiKasbonSubReportList(String JournalDate)
        {
            List<GetPermintaanDanRealisasiKasbonSubReport> result = new List<GetPermintaanDanRealisasiKasbonSubReport>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPermintaanDanRealisasiKasbonSubReport));
                ctx.CommandText = "GetPermintaanDanRealisasiKasbonSubReport";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("JournalDate", JournalDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPermintaanDanRealisasiKasbonSubReport)helper.IDataReaderToObject(reader, new GetPermintaanDanRealisasiKasbonSubReport()));
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
        #region GetPharmacyChargesBPJSTransactionType
        public static List<GetPharmacyChargesBPJSTransactionType> GetPharmacyChargesBPJSTransactionTypeList(Int32 RegistrationID)
        {
            List<GetPharmacyChargesBPJSTransactionType> result = new List<GetPharmacyChargesBPJSTransactionType>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPharmacyChargesBPJSTransactionType));
                ctx.CommandText = "GetPharmacyChargesBPJSTransactionType";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPharmacyChargesBPJSTransactionType)helper.IDataReaderToObject(reader, new GetPharmacyChargesBPJSTransactionType()));
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
        #region GetPaymentReceiptCustom
        public static List<GetPaymentReceiptCustom> GetPaymentReceiptCustomList(int PaymentReceiptID)
        {
            List<GetPaymentReceiptCustom> result = new List<GetPaymentReceiptCustom>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustom));
                ctx.CommandText = "GetPaymentReceiptCustom";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@PaymentReceiptID", PaymentReceiptID));
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustom)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustom()));
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

        public static List<GetPaymentReceiptCustom> GetPaymentReceiptCustomList(string filterExpression)
        {
            List<GetPaymentReceiptCustom> result = new List<GetPaymentReceiptCustom>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustom));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustom)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustom()));
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
        #region GetPaymentReceiptCustomBROS
        public static List<GetPaymentReceiptCustomBROS> GetPaymentReceiptCustomBROSList(int RegistrationID)
        {
            List<GetPaymentReceiptCustomBROS> result = new List<GetPaymentReceiptCustomBROS>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomBROS));
                ctx.CommandText = "GetPaymentReceiptCustomBROS";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@RegistrationID", RegistrationID));
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomBROS)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomBROS()));
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

        public static List<GetPaymentReceiptCustomBROS> GetPaymentReceiptCustomBROSList(string filterExpression)
        {
            List<GetPaymentReceiptCustomBROS> result = new List<GetPaymentReceiptCustomBROS>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomBROS));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomBROS)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomBROS()));
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
        #region GetPaymentReceiptCustomMCURSUKRIDA
        public static List<GetPaymentReceiptCustomMCURSUKRIDA> GetPaymentReceiptCustomMCURSUKRIDAList(int PaymentReceiptID)
        {
            List<GetPaymentReceiptCustomMCURSUKRIDA> result = new List<GetPaymentReceiptCustomMCURSUKRIDA>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomMCURSUKRIDA));
                ctx.CommandText = "GetPaymentReceiptCustomMCURSUKRIDA";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@PaymentReceiptID", PaymentReceiptID));
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomMCURSUKRIDA)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomMCURSUKRIDA()));
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

        public static List<GetPaymentReceiptCustomMCURSUKRIDA> GetPaymentReceiptCustomMCURSUKRIDAList(string filterExpression)
        {
            List<GetPaymentReceiptCustomMCURSUKRIDA> result = new List<GetPaymentReceiptCustomMCURSUKRIDA>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPaymentReceiptCustomMCURSUKRIDA));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPaymentReceiptCustomMCURSUKRIDA)helper.IDataReaderToObject(reader, new GetPaymentReceiptCustomMCURSUKRIDA()));
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
        #region GetPivotItemBalancePerPeriodeList
        public static List<GetPivotItemBalancePerPeriode> GetPivotItemBalancePerPeriodeList(String HealthcareID, String FromDate, String ToDate, String AdditionalFilterExpression)
        {
            List<GetPivotItemBalancePerPeriode> result = new List<GetPivotItemBalancePerPeriode>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPivotItemBalancePerPeriode));
                ctx.CommandText = "GetPivotItemBalancePerPeriode";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@HealthcareID", HealthcareID));
                ctx.Command.Parameters.Add(new SqlParameter("@FromDate", FromDate));
                ctx.Command.Parameters.Add(new SqlParameter("@ToDate", ToDate));
                if (AdditionalFilterExpression != null)
                    ctx.Command.Parameters.Add(new SqlParameter("@AdditionalFilterExpression", AdditionalFilterExpression));
                else
                    ctx.Command.Parameters.Add(new SqlParameter("@AdditionalFilterExpression", ""));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPivotItemBalancePerPeriode)helper.IDataReaderToObject(reader, new GetPivotItemBalancePerPeriode()));
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
        #region GetPortionRecapitulationPerMonth
        public static List<GetPortionRecapitulationPerMonth> GetGetPortionRecapitulationPerMonthList(string filterExpression)
        {
            List<GetPortionRecapitulationPerMonth> result = new List<GetPortionRecapitulationPerMonth>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPortionRecapitulationPerMonth));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPortionRecapitulationPerMonth)helper.IDataReaderToObject(reader, new GetPortionRecapitulationPerMonth()));
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
        #region GetPrescriptionOrderDtCustom
        public static List<GetPrescriptionOrderDtCustom> GetPrescriptionOrderDtCustomList(Int32 TransactionID)
        {
            List<GetPrescriptionOrderDtCustom> result = new List<GetPrescriptionOrderDtCustom>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom));
                ctx.CommandText = "GetPrescriptionOrderDtCustom";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom()));
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
        #region GetPrescriptionOrderDtCustom1
        public static List<GetPrescriptionOrderDtCustom1> GetPrescriptionOrderDtCustom1List(Int32 TransactionID)
        {
            List<GetPrescriptionOrderDtCustom1> result = new List<GetPrescriptionOrderDtCustom1>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom1));
                ctx.CommandText = "GetPrescriptionOrderDtCustom1";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom1)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom1()));
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
        #region GetPrescriptionOrderDtCustom2
        public static List<GetPrescriptionOrderDtCustom2> GetPrescriptionOrderDtCustom2List(Int32 TransactionID)
        {
            List<GetPrescriptionOrderDtCustom2> result = new List<GetPrescriptionOrderDtCustom2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom2));
                ctx.CommandText = "GetPrescriptionOrderDtCustom2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom2)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom2()));
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
        #region GetPrescriptionOrderDtCustom3
        public static List<GetPrescriptionOrderDtCustom3> GetPrescriptionOrderDtCustom3List(Int32 PrescriptionOrderID)
        {
            List<GetPrescriptionOrderDtCustom3> result = new List<GetPrescriptionOrderDtCustom3>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom3));
                ctx.CommandText = "GetPrescriptionOrderDtCustom3";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PrescriptionOrderID", PrescriptionOrderID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom3)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom3()));
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
        #region GetPrescriptionOrderDtCustom4
        public static List<GetPrescriptionOrderDtCustom4> GetPrescriptionOrderDtCustom4List(Int32 TransactionID)
        {
            List<GetPrescriptionOrderDtCustom4> result = new List<GetPrescriptionOrderDtCustom4>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom4));
                ctx.CommandText = "GetPrescriptionOrderDtCustom4";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom4)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom4()));
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
        #region GetPrescriptionOrderDtCustom5
        public static List<GetPrescriptionOrderDtCustom5> GetPrescriptionOrderDtCustom5List(Int32 PrescriptionOrderID)
        {
            List<GetPrescriptionOrderDtCustom5> result = new List<GetPrescriptionOrderDtCustom5>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom5));
                ctx.CommandText = "GetPrescriptionOrderDtCustom5";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PrescriptionOrderID", PrescriptionOrderID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom5)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom5()));
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
        #region GetPrescriptionOrderDtCustom7
        public static List<GetPrescriptionOrderDtCustom7> GetPrescriptionOrderDtCustom7List(Int32 VisitID)
        {
            List<GetPrescriptionOrderDtCustom7> result = new List<GetPrescriptionOrderDtCustom7>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom7));
                ctx.CommandText = "GetPrescriptionOrderDtCustom7";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", VisitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom7)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom7()));
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
        #region GetPrescriptionOrderDtCustom8
        public static List<GetPrescriptionOrderDtCustom8> GetPrescriptionOrderDtCustom8List(Int32 PrescriptionOrderID)
        {
            List<GetPrescriptionOrderDtCustom8> result = new List<GetPrescriptionOrderDtCustom8>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom8));
                ctx.CommandText = "GetPrescriptionOrderDtCustom8";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PrescriptionOrderID", PrescriptionOrderID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom8)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom8()));
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
        #region GetPrescriptionOrderDtCustom9
        public static List<GetPrescriptionOrderDtCustom9> GetPrescriptionOrderDtCustom9List(String TransactionID)
        {
            List<GetPrescriptionOrderDtCustom9> result = new List<GetPrescriptionOrderDtCustom9>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtCustom9));
                ctx.CommandText = "GetPrescriptionOrderDtCustom9";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtCustom9)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtCustom9()));
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
        #region GetPrescriptionOrderDtMedicalPrescription
        public static List<GetPrescriptionOrderDtMedicalPrescription> GetPrescriptionOrderDtMedicalPrescriptionList(Int32 TransactionID)
        {
            List<GetPrescriptionOrderDtMedicalPrescription> result = new List<GetPrescriptionOrderDtMedicalPrescription>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtMedicalPrescription));
                ctx.CommandText = "GetPrescriptionOrderDtMedicalPrescription";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtMedicalPrescription)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtMedicalPrescription()));
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
        #region GetPrescriptionOrderDtMedicalPrescriptionUDD
        public static List<GetPrescriptionOrderDtMedicalPrescriptionUDD> GetPrescriptionOrderDtMedicalPrescriptionUDDList(Int32 TransactionID)
        {
            List<GetPrescriptionOrderDtMedicalPrescriptionUDD> result = new List<GetPrescriptionOrderDtMedicalPrescriptionUDD>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderDtMedicalPrescriptionUDD));
                ctx.CommandText = "GetPrescriptionOrderDtMedicalPrescriptionUDD";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionID", TransactionID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderDtMedicalPrescriptionUDD)helper.IDataReaderToObject(reader, new GetPrescriptionOrderDtMedicalPrescriptionUDD()));
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
        #region GetPrescriptionOrderHdInfoDisplay
        public static List<GetPrescriptionOrderHdInfoDisplay> GetPrescriptionOrderHdInfoDisplay(string transactionDate)
        {
            List<GetPrescriptionOrderHdInfoDisplay> result = new List<GetPrescriptionOrderHdInfoDisplay>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionOrderHdInfoDisplay));
                ctx.CommandText = "GetPrescriptionOrderHdInfoDisplay";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionDate", transactionDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionOrderHdInfoDisplay)helper.IDataReaderToObject(reader, new GetPrescriptionOrderHdInfoDisplay()));
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
        #region GetPrescriptionPerDokter
        public static List<GetPrescriptionPerDokter> GetGetPrescriptionPerDokterList(string filterExpression)
        {
            List<GetPrescriptionPerDokter> result = new List<GetPrescriptionPerDokter>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionPerDokter));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionPerDokter)helper.IDataReaderToObject(reader, new GetPrescriptionPerDokter()));
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
        #region GetPrescriptionPerDokterRekap
        public static List<GetPrescriptionPerDokterRekap> GetPrescriptionPerDokterRekapList(string filterExpression)
        {
            List<GetPrescriptionPerDokterRekap> result = new List<GetPrescriptionPerDokterRekap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionPerDokterRekap));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionPerDokterRekap)helper.IDataReaderToObject(reader, new GetPrescriptionPerDokterRekap()));
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
        public static List<GetPrescriptionPerDokterRekap> GetPrescriptionPerDokterRekap2List(string month, string year, string departementID)
        {
            List<GetPrescriptionPerDokterRekap> result = new List<GetPrescriptionPerDokterRekap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionPerDokterRekap));
                ctx.CommandText = "GetPrescriptionPerDokterRekap";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Command.Parameters.Add(new SqlParameter("@Month", month));
                ctx.Command.Parameters.Add(new SqlParameter("@Year", year));
                ctx.Command.Parameters.Add(new SqlParameter("@DepartmentID", departementID));

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionPerDokterRekap)helper.IDataReaderToObject(reader, new GetPrescriptionPerDokterRekap()));
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
        #region GetPrescriptionReturnOrderRemainingQty
        public static List<GetPrescriptionReturnOrderRemainingQty> GetPrescriptionReturnOrderRemainingQtyList(int RegistrationID, int LocationID)
        {
            List<GetPrescriptionReturnOrderRemainingQty> result = new List<GetPrescriptionReturnOrderRemainingQty>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionReturnOrderRemainingQty));
                ctx.CommandText = "GetPrescriptionReturnOrderRemainingQty";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("LocationID", LocationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionReturnOrderRemainingQty)helper.IDataReaderToObject(reader, new GetPrescriptionReturnOrderRemainingQty()));
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
        #region GetPrescriptionReturnOrderRemainingQtyPerItem
        public static List<GetPrescriptionReturnOrderRemainingQtyPerItem> GetPrescriptionReturnOrderRemainingQtyPerItemList(int RegistrationID, int LocationID, int ItemID, int PatientChargesDtID, IDbContext ctx = null)
        {
            List<GetPrescriptionReturnOrderRemainingQtyPerItem> result = new List<GetPrescriptionReturnOrderRemainingQtyPerItem>();
            if (ctx == null)
            {
                ctx = DbFactory.Configure(true);
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPrescriptionReturnOrderRemainingQtyPerItem));
                ctx.CommandText = "GetPrescriptionReturnOrderRemainingQtyPerItem";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("LocationID", LocationID);
                ctx.Add("ItemID", ItemID);
                ctx.Add("PatientChargesDtID", PatientChargesDtID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPrescriptionReturnOrderRemainingQtyPerItem)helper.IDataReaderToObject(reader, new GetPrescriptionReturnOrderRemainingQtyPerItem()));
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
        #region GetProcessFeeRevenueSharing
        public static List<GetProcessFeeRevenueSharing> GetProcessFeeRevenueSharing(string StartDate, string EndDate, int ParamedicID, string Status)
        {
            List<GetProcessFeeRevenueSharing> result = new List<GetProcessFeeRevenueSharing>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetProcessFeeRevenueSharing));
                ctx.CommandText = "GetProcessFeeRevenueSharing";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("StartDate", StartDate);
                ctx.Add("EndDate", EndDate);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("Status", Status);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetProcessFeeRevenueSharing)helper.IDataReaderToObject(reader, new GetProcessFeeRevenueSharing()));
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
        #region GetProcessFeeRevenueSharingParamedic
        public static List<GetProcessFeeRevenueSharingParamedic> GetProcessFeeRevenueSharingParamedic(string StartDate, string EndDate, string Paramedic, string CreatedBy, string Status)
        {
            List<GetProcessFeeRevenueSharingParamedic> result = new List<GetProcessFeeRevenueSharingParamedic>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetProcessFeeRevenueSharingParamedic));
                ctx.CommandText = "GetProcessFeeRevenueSharingParamedic";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("StartDate", StartDate);
                ctx.Add("EndDate", EndDate);
                ctx.Add("Paramedic", Paramedic);
                ctx.Add("CreatedBy", CreatedBy);
                ctx.Add("Status", Status);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetProcessFeeRevenueSharingParamedic)helper.IDataReaderToObject(reader, new GetProcessFeeRevenueSharingParamedic()));
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
        #region GetPRSCheckCountDataDouble
        public static List<GetPRSCheckCountDataDouble> GetPRSCheckCountDataDoubleList(String RSTransactionIDList)
        {
            List<GetPRSCheckCountDataDouble> result = new List<GetPRSCheckCountDataDouble>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPRSCheckCountDataDouble));
                ctx.CommandText = "GetPRSCheckCountDataDouble";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSTransactionIDList", RSTransactionIDList);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPRSCheckCountDataDouble)helper.IDataReaderToObject(reader, new GetPRSCheckCountDataDouble()));

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
        #region GetPRSCheckCountDataDoubleDetail
        public static List<GetPRSCheckCountDataDoubleDetail> GetPRSCheckCountDataDoubleDetailList(Int32 ParamedicID)
        {
            List<GetPRSCheckCountDataDoubleDetail> result = new List<GetPRSCheckCountDataDoubleDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPRSCheckCountDataDoubleDetail));
                ctx.CommandText = "GetPRSCheckCountDataDoubleDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPRSCheckCountDataDoubleDetail)helper.IDataReaderToObject(reader, new GetPRSCheckCountDataDoubleDetail()));

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
        #region GetPurchaseInvoiceDt
        public static List<GetPurchaseInvoiceDt> GetPurchaseInvoiceDt(string TransactionDate, int SourceDateAging)
        {
            List<GetPurchaseInvoiceDt> result = new List<GetPurchaseInvoiceDt>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseInvoiceDt));
                ctx.CommandText = "GetPurchaseInvoiceDt";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_TransactionDate", TransactionDate);
                ctx.Add("p_SourceDateAging", SourceDateAging);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseInvoiceDt)helper.IDataReaderToObject(reader, new GetPurchaseInvoiceDt()));
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
        #region GetPurchaseInvoicePending
        public static List<GetPurchaseInvoicePending> GetPurchaseInvoicePending(string Date)
        {
            List<GetPurchaseInvoicePending> result = new List<GetPurchaseInvoicePending>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseInvoicePending));
                ctx.CommandText = "GetPurchaseInvoicePending";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", Date);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseInvoicePending)helper.IDataReaderToObject(reader, new GetPurchaseInvoicePending()));
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
        #region GetPurchaseInvoiceInformation
        public static List<GetPurchaseInvoiceInformation> GetPurchaseInvoiceInformationList(String MovementDate, Int32 BusinessPartnerID, Int32 PageIndex, Int32 NumRows)
        {
            List<GetPurchaseInvoiceInformation> result = new List<GetPurchaseInvoiceInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseInvoiceInformation));
                ctx.CommandText = "GetPurchaseInvoiceInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MovementDate", MovementDate);
                ctx.Add("BusinessPartnerID", BusinessPartnerID);
                ctx.Add("PageIndex", PageIndex);
                ctx.Add("NumRows", NumRows);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseInvoiceInformation)helper.IDataReaderToObject(reader, new GetPurchaseInvoiceInformation()));
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
        #region GetPurchaseInvoiceInformationDt
        public static List<GetPurchaseInvoiceInformationDt> GetPurchaseInvoiceInformationDtList(String MovementDate, Int32 SupplierID, Int32 Start, Int32 End)
        {
            List<GetPurchaseInvoiceInformationDt> result = new List<GetPurchaseInvoiceInformationDt>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseInvoiceInformationDt));
                ctx.CommandText = "GetPurchaseInvoiceInformationDt";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MovementDate", MovementDate);
                ctx.Add("SupplierID", SupplierID);
                ctx.Add("Start", Start);
                ctx.Add("End", End);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseInvoiceInformationDt)helper.IDataReaderToObject(reader, new GetPurchaseInvoiceInformationDt()));
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
        #region GetPurchaseOrderReceiveConsignment
        public static List<GetPurchaseOrderReceiveConsignment> GetPurchaseOrderReceiveConsignment(Int32 SupplierID, Int32 FilterOrderDate, String OrderDate, Int32 ProductLineID)
        {
            List<GetPurchaseOrderReceiveConsignment> result = new List<GetPurchaseOrderReceiveConsignment>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseOrderReceiveConsignment));
                ctx.CommandText = "GetPurchaseOrderReceiveConsignment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("SupplierID", SupplierID);
                ctx.Add("FilterOrderDate", FilterOrderDate);
                ctx.Add("OrderDate", OrderDate);
                ctx.Add("ProductLineID", ProductLineID);
                ctx.Command.CommandTimeout = 90;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseOrderReceiveConsignment)helper.IDataReaderToObject(reader, new GetPurchaseOrderReceiveConsignment()));
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
        #region GetPurchaseReceiveDtExpired
        public static List<GetPurchaseReceiveDtExpired> GetPurchaseReceiveDtExpiredList(Int32 count, Int32 itemID, Int32 locationID, IDbContext ctx)
        {
            List<GetPurchaseReceiveDtExpired> result = new List<GetPurchaseReceiveDtExpired>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseReceiveDtExpired));
                ctx.CommandText = "GetPurchaseReceiveDtExpired";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Count", count);
                ctx.Add("ItemID", itemID);
                ctx.Add("LocationID", locationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseReceiveDtExpired)helper.IDataReaderToObject(reader, new GetPurchaseReceiveDtExpired()));
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
        public static List<GetPurchaseReceiveDtExpired> GetPurchaseReceiveDtExpiredList(Int32 count, Int32 itemID, Int32 locationID)
        {
            IDbContext ctx = DbFactory.Configure();
            return GetPurchaseReceiveDtExpiredList(count, itemID, locationID, ctx);
        }
        #endregion
        #region GetPurchaseReceiveDtFixedAsset
        public static List<GetPurchaseReceiveDtFixedAsset> GetPurchaseReceiveDtFixedAssetList(String ReceivedDate, Int32 BusinessPartnerID, Int32 ItemID, Int32 ProductLineID)
        {
            List<GetPurchaseReceiveDtFixedAsset> result = new List<GetPurchaseReceiveDtFixedAsset>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseReceiveDtFixedAsset));
                ctx.CommandText = "GetPurchaseReceiveDtFixedAsset";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@ReceivedDate", ReceivedDate);
                ctx.Add("@BusinessPartnerID", BusinessPartnerID);
                ctx.Add("@ItemID", ItemID);
                ctx.Add("@ProductLineID", ProductLineID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseReceiveDtFixedAsset)helper.IDataReaderToObject(reader, new GetPurchaseReceiveDtFixedAsset()));
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
        #region GetPurchaseRequestByID
        public static List<GetPurchaseRequestByID> GetPurchaseRequestByID(string filterExpression)
        {
            List<GetPurchaseRequestByID> result = new List<GetPurchaseRequestByID>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseRequestByID));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseRequestByID)helper.IDataReaderToObject(reader, new GetPurchaseRequestByID()));
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
        #region GetPurchaseRequestOrderReceivePerRequestDate
        public static List<GetPurchaseRequestOrderReceivePerRequestDate> GetPurchaseRequestOrderReceivePerRequestDateList(String TransactionDate, String GCItemType, Int32 ItemID)
        {
            List<GetPurchaseRequestOrderReceivePerRequestDate> result = new List<GetPurchaseRequestOrderReceivePerRequestDate>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseRequestOrderReceivePerRequestDate));
                ctx.CommandText = "GetPurchaseRequestOrderReceivePerRequestDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("TransactionDate", TransactionDate);
                ctx.Add("GCItemType", GCItemType);
                ctx.Add("ItemID", ItemID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseRequestOrderReceivePerRequestDate)helper.IDataReaderToObject(reader, new GetPurchaseRequestOrderReceivePerRequestDate()));
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
        #region GetPurchaseOrderType
        public static List<GetPurchaseOrderType> GetPurchaseOrderTypeList(Int32 PurchaseReceiveID)
        {
            List<GetPurchaseOrderType> result = new List<GetPurchaseOrderType>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseOrderType));
                ctx.CommandText = "GetPurchaseOrderType";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PurchaseReceiveID", PurchaseReceiveID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPurchaseOrderType)helper.IDataReaderToObject(reader, new GetPurchaseOrderType()));
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
        #region GetMaxQueueNo
        public static int GetMaxQueueNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, Int32 Session, Int32 IsFromAppointment)
        {
            int result = 0;
            IDbContext ctx = DbFactory.Configure();
            try
            {

                ctx.CommandText = "GetMaxQueueNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                //ctx.Add("VisitTime", VisitTime);
                ctx.Add("Session", Session);
                ctx.Add("IsFromAppointment", IsFromAppointment);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["MaxQueue"];
                    result = Convert.ToInt32(value);
                }
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
        public static int GetMaxQueueNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, Int32 Session, Int32 IsFromAppointment, IDbContext ctx)
        {
            int result = 0;
            try
            {

                ctx.CommandText = "GetMaxQueueNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                //ctx.Add("VisitTime", VisitTime);
                ctx.Add("Session", Session);
                ctx.Add("IsFromAppointment", IsFromAppointment);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["MaxQueue"];
                    result = Convert.ToInt32(value);
                }
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
        #region GetMaxQueueNoAppointment
        public static int GetMaxQueueNoAppointment(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, String StartTime, String EndTime, Int32 IsWaitingList)
        {
            int result = 0;
            IDbContext ctx = DbFactory.Configure();
            try
            {

                ctx.CommandText = "GetMaxQueueNoAppointment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("StartTime", StartTime);
                ctx.Add("EndTime", EndTime);
                ctx.Add("IsWaitingList", IsWaitingList);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["MaxQueue"];
                    result = Convert.ToInt32(value);
                }
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
        public static int GetMaxQueueNoAppointment(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, String StartTime, String EndTime, Int32 IsWaitingList, IDbContext ctx)
        {
            int result = 0;
            try
            {

                ctx.CommandText = "GetMaxQueueNoAppointment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("StartTime", StartTime);
                ctx.Add("EndTime", EndTime);
                ctx.Add("IsWaitingList", IsWaitingList);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["MaxQueue"];
                    result = Convert.ToInt32(value);
                }
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
        #region GetQueueNo
        public static int GetQueueNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, Int32 Session, bool isFromOnlineApps, bool isBPJS, Int32 IsFromAppointment, IDbContext ctx, Int32 IsGoshow = 0)
        {
            int result = 0;
            try
            {
                ctx.CommandText = "GetQueueNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("Session", Session);
                ctx.Add("IsFromOnlineApps", isFromOnlineApps);
                ctx.Add("IsBPJS", isBPJS);
                ctx.Add("IsFromAppointment", IsFromAppointment);
                ctx.Add("IsGoshow", IsGoshow);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["MaxQueue"];
                    result = Convert.ToInt32(value);
                }
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
        public static int GetQueueNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, Int32 Session, bool isFromOnlineApps, bool isBPJS, Int32 IsFromAppointment, Int32 IsGoshow = 0, IDbContext ctx = null)
        {
            int result = 0;
            if (ctx == null)
            {
                ctx = DbFactory.Configure(true);
            }
            try
            {
                ctx.CommandText = "GetQueueNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("Session", Session);
                ctx.Add("IsFromOnlineApps", isFromOnlineApps);
                ctx.Add("IsBPJS", isBPJS);
                ctx.Add("IsFromAppointment", IsFromAppointment);
                ctx.Add("IsGoshow", IsGoshow);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["MaxQueue"];
                    result = Convert.ToInt32(value);
                }
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
        public static int GetQueueNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, Int32 Session, bool isFromOnlineApps, bool isBPJS, Int32 IsFromAppointment)
        {
            int result = 0;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                ctx.CommandText = "GetQueueNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("Session", Session);
                ctx.Add("IsFromOnlineApps", isFromOnlineApps);
                ctx.Add("IsBPJS", isBPJS);
                ctx.Add("IsFromAppointment", IsFromAppointment);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["MaxQueue"];
                    result = Convert.ToInt32(value);
                }
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
        #region GetQueueLastNo
        public static int GetQueueLastNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, Int32 Session, Int32 Queue)
        {
            int result = 0;
            IDbContext ctx = DbFactory.Configure();
            try
            {

                ctx.CommandText = "GetLastQueueNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("Session", Session);
                ctx.Add("QueueNo", Queue);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["LastQueue"];
                    result = Convert.ToInt32(value);
                }
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
        public static int GetQueueLastNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, Int32 Session, Int32 Queue, IDbContext ctx)
        {
            int result = 0;
            try
            {

                ctx.CommandText = "GetLastQueueNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("Session", Session);
                ctx.Add("QueueNo", Queue);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["LastQueue"];
                    result = Convert.ToInt32(value);
                }
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
        #region GetQueueLastNo
        public static int GetLastQueueRequestNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, string VisitDate)
        {
            int result = 0;
            IDbContext ctx = DbFactory.Configure();
            try
            {

                ctx.CommandText = "GetLastQueueRequestNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["LastQueue"];
                    result = Convert.ToInt32(value);
                }
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
        public static int GetLastQueueRequestNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, String VisitDate, IDbContext ctx)
        {
            int result = 0;
            try
            {

                ctx.CommandText = "GetLastQueueRequestNo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["LastQueue"];
                    result = Convert.ToInt32(value);
                }
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
        #region GetRekapPaseinRawatJalanPerHari
        public static List<GetRekapPaseinRawatJalanPerHari> GetRekapPaseinRawatJalanPerHariList(DateTime Date)
        {
            List<GetRekapPaseinRawatJalanPerHari> result = new List<GetRekapPaseinRawatJalanPerHari>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPurchaseInvoiceInformationDt));
                ctx.CommandText = "GetPurchaseInvoiceInformationDt";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Date", Date);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRekapPaseinRawatJalanPerHari)helper.IDataReaderToObject(reader, new GetRekapPaseinRawatJalanPerHari()));
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
        #region GetRekapPasienDirawatdenganKomorbidSiranap
        public static List<GetRekapPasienDirawatdenganKomorbidSiranap> GetRekapPasienDirawatdenganKomorbidSiranapList(String ObservationDate)
        {
            List<GetRekapPasienDirawatdenganKomorbidSiranap> result = new List<GetRekapPasienDirawatdenganKomorbidSiranap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRekapPasienDirawatdenganKomorbidSiranap));
                ctx.CommandText = "GetRekapPasienDirawatdenganKomorbidSiranap";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ObservationDate", ObservationDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRekapPasienDirawatdenganKomorbidSiranap)helper.IDataReaderToObject(reader, new GetRekapPasienDirawatdenganKomorbidSiranap()));
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
        #region GetRekapPasienDirawatTanpaKomorbidSiranap
        public static List<GetRekapPasienDirawatTanpaKomorbidSiranap> GetGetRekapPasienDirawatTanpaKomorbidSiranapList(String ObservationDate)
        {
            List<GetRekapPasienDirawatTanpaKomorbidSiranap> result = new List<GetRekapPasienDirawatTanpaKomorbidSiranap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRekapPasienDirawatTanpaKomorbidSiranap));
                ctx.CommandText = "GetRekapPasienDirawatTanpaKomorbidSiranap";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ObservationDate", ObservationDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRekapPasienDirawatTanpaKomorbidSiranap)helper.IDataReaderToObject(reader, new GetRekapPasienDirawatTanpaKomorbidSiranap()));
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
        #region GetRekapPasienMasukSiranap
        public static List<GetRekapPasienMasukSiranap> GetRekapPasienMasukSiranapList(String ObservationDate)
        {
            List<GetRekapPasienMasukSiranap> result = new List<GetRekapPasienMasukSiranap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRekapPasienMasukSiranap));
                ctx.CommandText = "GetRekapPasienMasukSiranap";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ObservationDate", ObservationDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRekapPasienMasukSiranap)helper.IDataReaderToObject(reader, new GetRekapPasienMasukSiranap()));
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
        #region GetRekapPasienKeluar
        public static List<GetRekapPasienKeluar> GetRekapPasienKeluarList(String ObservationDate)
        {
            List<GetRekapPasienKeluar> result = new List<GetRekapPasienKeluar>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRekapPasienKeluar));
                ctx.CommandText = "GetRekapPasienKeluar";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ObservationDate", ObservationDate);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRekapPasienKeluar)helper.IDataReaderToObject(reader, new GetRekapPasienKeluar()));
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
        #region GetRegistrationCancel
        public static List<GetRegistrationCancel> GetRegistrationCancelList(DateTime ActualVisitDate, String DepartmentID, Boolean IsNeedCodification)
        {
            List<GetRegistrationCancel> result = new List<GetRegistrationCancel>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationCancel));
                ctx.CommandText = "GetRegistrationCancel";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ActualVisitDate", ActualVisitDate);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("IsNeedCodification", IsNeedCodification);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationCancel)helper.IDataReaderToObject(reader, new GetRegistrationCancel()));
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
        #region GetRegistrationDiagnosticResult
        public static List<GetRegistrationDiagnosticResult> GetRegistrationDiagnosticResultList(Int32 RegistrationID)
        {
            List<GetRegistrationDiagnosticResult> result = new List<GetRegistrationDiagnosticResult>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationDiagnosticResult));
                ctx.CommandText = "GetRegistrationDiagnosticResult";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationDiagnosticResult)helper.IDataReaderToObject(reader, new GetRegistrationDiagnosticResult()));
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
        #region GetRegistrationHasLabResult
        public static List<GetRegistrationHasLabResult> GetRegistrationHasLabResult(string StartDate,string EndDate,string DepartmentID, Int32 ServiceUnitID,Int32 ParamedicID)
        {
            List<GetRegistrationHasLabResult> result = new List<GetRegistrationHasLabResult>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationHasLabResult));
                ctx.CommandText = "GetRegistrationHasLabResult";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("StartDate", StartDate);
                ctx.Add("EndDate", EndDate);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("ServiceUnitID", ServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationHasLabResult)helper.IDataReaderToObject(reader, new GetRegistrationHasLabResult()));
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
        #region GetRegistrationPatient
        public static List<GetRegistrationPatient> GetRegistrationPatientList(string filterExpression)
        {
            List<GetRegistrationPatient> result = new List<GetRegistrationPatient>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationPatient));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationPatient)helper.IDataReaderToObject(reader, new GetRegistrationPatient()));
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
        #region GetRegistrationRealizationInfo
        public static List<GetRegistrationRealizationInfo> GetRegistrationRealizationInfoList(String FromDepartmentID)
        {
            List<GetRegistrationRealizationInfo> result = new List<GetRegistrationRealizationInfo>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationRealizationInfo));
                ctx.CommandText = "GetRegistrationRealizationInfo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("FromDepartmentID", FromDepartmentID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationRealizationInfo)helper.IDataReaderToObject(reader, new GetRegistrationRealizationInfo()));
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
        #region GetRegistrationRealizationInfoRekap
        public static List<GetRegistrationRealizationInfoRekap> GetRegistrationRealizationInfoRekapList(String FromDepartmentID, Int32 HSUID)
        {
            List<GetRegistrationRealizationInfoRekap> result = new List<GetRegistrationRealizationInfoRekap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationRealizationInfoRekap));
                ctx.CommandText = "GetRegistrationRealizationInfoRekap";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("FromDepartmentID", FromDepartmentID);
                ctx.Add("HSUID", HSUID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationRealizationInfoRekap)helper.IDataReaderToObject(reader, new GetRegistrationRealizationInfoRekap()));
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
        #region GetRegistrationSession
        public static int GetRegistrationSession(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, String VisitTime)
        {
            int result = 0;
            IDbContext ctx = DbFactory.Configure();
            try
            {

                ctx.CommandText = "GetRegistrationSession";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("VisitTime", VisitTime);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["Session"];
                    if (reader["Session"] != System.DBNull.Value)
                    {
                        result = Convert.ToInt32(value);
                    }
                    else
                    {
                        result = 0;
                    }
                }
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
        public static int GetRegistrationSession(Int32 HealthcareServiceUnitID, Int32 ParamedicID, DateTime VisitDate, String VisitTime, IDbContext ctx, int IsFromAppointment = 1)
        {
            int result = 0;
            try
            {
                ctx.CommandText = "GetRegistrationSession";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareServiceUnitID", HealthcareServiceUnitID);
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VisitDate", VisitDate);
                ctx.Add("VisitTime", VisitTime);
                ctx.Add("IsFromAppointment", IsFromAppointment);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["Session"];
                    if (reader["Session"] != System.DBNull.Value)
                    {
                        result = Convert.ToInt32(value);
                    }
                    else
                    {
                        result = 0;
                    }
                }
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
        #region GetReorderItemRequest
        public static List<GetReorderItemRequest> GetReorderItemRequestList(int oIsFilterLocationFrom, int oLocationIDFrom, int oLocationIDTo, int oProductLineID, int oIsFilterQtyOnHand, int oIsROPDynamic, int oDisplayMinimum, int oDisplayOption)
        {
            List<GetReorderItemRequest> result = new List<GetReorderItemRequest>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetReorderItemRequest));
                ctx.CommandText = "GetReorderItemRequest";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@oIsFilterLocationFrom", oIsFilterLocationFrom);
                ctx.Add("@oLocationIDFrom", oLocationIDFrom);
                ctx.Add("@oLocationIDTo", oLocationIDTo);
                ctx.Add("@oProductLineID", oProductLineID);
                ctx.Add("@oIsFilterQtyOnHand", oIsFilterQtyOnHand);
                ctx.Add("@oIsROPDynamic", oIsROPDynamic);
                ctx.Add("@oDisplayMinimum", oDisplayMinimum);
                ctx.Add("@oDisplayOption", oDisplayOption);
                ctx.Command.CommandTimeout = 300;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetReorderItemRequest)helper.IDataReaderToObject(reader, new GetReorderItemRequest()));
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
        #region GetRevenueSharingPaymentPreparationAllDoctor
        public static int GetRevenueSharingPaymentPreparationAllDoctor(Int32 ParamedicID, String VerificationDate, Int32 FromItemID, Int32 ToItemID)
        {
            int result = 0;
            IDbContext ctx = DbFactory.Configure();
            try
            {

                ctx.CommandText = "GetRevenueSharingPaymentPreparationAllDoctor";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", ParamedicID);
                ctx.Add("VerificationDate", VerificationDate);
                ctx.Add("FromItemID", FromItemID);
                ctx.Add("ToItemID", ToItemID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                {
                    reader.Read();
                    object value = reader["Session"];
                    if (reader["Session"] != System.DBNull.Value)
                    {
                        result = Convert.ToInt32(value);
                    }
                    else
                    {
                        result = 0;
                    }
                }
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
        #region GetSamplingPasienRawatInap
        public static List<GetSamplingPasienRawatInap> GetSamplingPasienRawatInapList(string filterExpression)
        {
            List<GetSamplingPasienRawatInap> result = new List<GetSamplingPasienRawatInap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetSamplingPasienRawatInap));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetSamplingPasienRawatInap)helper.IDataReaderToObject(reader, new GetSamplingPasienRawatInap()));
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
        #region GetSatuSehatRegistrationInformation
        public static List<GetSatuSehatRegistrationInformation> GetSatuSehatRegistrationInformation(int registrationID)
        {
            List<GetSatuSehatRegistrationInformation> result = new List<GetSatuSehatRegistrationInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetSatuSehatRegistrationInformation));
                ctx.CommandText = "GetSatuSehatRegistrationInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", registrationID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetSatuSehatRegistrationInformation)helper.IDataReaderToObject(reader, new GetSatuSehatRegistrationInformation()));
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
        #region GetServiceUnitParamedicList
        public static List<GetServiceUnitParamedicList> GetServiceUnitParamedicList(int healthcareServiceUnitID, string filterExpression)
        {
            List<GetServiceUnitParamedicList> result = new List<GetServiceUnitParamedicList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetServiceUnitParamedicList));
                ctx.CommandText = "GetServiceUnitParamedicList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareServiceUnitID", healthcareServiceUnitID);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetServiceUnitParamedicList)helper.IDataReaderToObject(reader, new GetServiceUnitParamedicList()));
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
        #region GetServiceUnitUserList
        public static List<GetServiceUnitUserList> GetServiceUnitUserList(String healthcareID, int userID, String departmentID, String filterExpression)
        {
            List<GetServiceUnitUserList> result = new List<GetServiceUnitUserList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetServiceUnitUserList));
                ctx.CommandText = "GetServiceUnitUserList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareID", healthcareID);
                ctx.Add("p_UserID", userID);
                ctx.Add("p_DepartmentID", departmentID);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetServiceUnitUserList)helper.IDataReaderToObject(reader, new GetServiceUnitUserList()));
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
        #region GetServiceUnitUserRoleList
        public static List<GetServiceUnitUserRoleList> GetServiceUnitUserRoleList(String healthcareID, int userID, String departmentID, String filterExpression)
        {
            List<GetServiceUnitUserRoleList> result = new List<GetServiceUnitUserRoleList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetServiceUnitUserRoleList));
                ctx.CommandText = "GetServiceUnitUserRoleList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_HealthcareID", healthcareID);
                ctx.Add("p_UserID", userID);
                ctx.Add("p_DepartmentID", departmentID);
                ctx.Add("p_AdditionalFilterExpression", filterExpression);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetServiceUnitUserRoleList)helper.IDataReaderToObject(reader, new GetServiceUnitUserRoleList()));
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
        #region GetSRSCheckCountDataDouble1
        public static List<GetSRSCheckCountDataDouble1> GetSRSCheckCountDataDouble1List(String RSSummaryIDList)
        {
            List<GetSRSCheckCountDataDouble1> result = new List<GetSRSCheckCountDataDouble1>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetSRSCheckCountDataDouble1));
                ctx.CommandText = "GetSRSCheckCountDataDouble1";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryIDList", RSSummaryIDList);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetSRSCheckCountDataDouble1)helper.IDataReaderToObject(reader, new GetSRSCheckCountDataDouble1()));

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
        #region GetSRSCheckCountDataDouble2
        public static List<GetSRSCheckCountDataDouble2> GetSRSCheckCountDataDouble2List(String RSSummaryIDList)
        {
            List<GetSRSCheckCountDataDouble2> result = new List<GetSRSCheckCountDataDouble2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetSRSCheckCountDataDouble2));
                ctx.CommandText = "GetSRSCheckCountDataDouble2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryIDList", RSSummaryIDList);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetSRSCheckCountDataDouble2)helper.IDataReaderToObject(reader, new GetSRSCheckCountDataDouble2()));

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
        #region GetSupplierForReorderPO
        public static List<GetSupplierForReorderPO> GetGetSupplierForReorderPOList(int itemID, string filterExpression)
        {
            List<GetSupplierForReorderPO> result = new List<GetSupplierForReorderPO>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetServiceUnitUserList));
                ctx.CommandText = "GetSupplierForReorderPO";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_ItemID", itemID);
                ctx.Add("p_FilterExpression", filterExpression);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetSupplierForReorderPO)helper.IDataReaderToObject(reader, new GetSupplierForReorderPO()));
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
        #region GetSupplierPaymentDate
        public static List<GetSupplierPaymentDate> GetGetSupplierPaymentDateList(string paymentDate, IDbContext ctx = null)
        {
            List<GetSupplierPaymentDate> result = new List<GetSupplierPaymentDate>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetSupplierPaymentDate));
                ctx.CommandText = "GetSupplierPaymentDate";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentDate", paymentDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetSupplierPaymentDate)helper.IDataReaderToObject(reader, new GetSupplierPaymentDate()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetTariffBookDtForUpload
        public static List<GetTariffBookDtForUpload> GetTariffBookDtForUpload(int BookID, int UserID, IDbContext ctx = null)
        {
            List<GetTariffBookDtForUpload> result = new List<GetTariffBookDtForUpload>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTariffBookDtForUpload));
                ctx.CommandText = "GetTariffBookDtForUpload";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("BookID", BookID);
                ctx.Add("UserID", UserID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTariffBookDtForUpload)helper.IDataReaderToObject(reader, new GetTariffBookDtForUpload()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        public static List<GetTariffBookDtForUpload> GetGetTariffBookDtForUploadList(string filterExpression)
        {
            List<GetTariffBookDtForUpload> result = new List<GetTariffBookDtForUpload>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTariffBookDtForUpload));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTariffBookDtForUpload)helper.IDataReaderToObject(reader, new GetTariffBookDtForUpload()));
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
        #region GetTariffBookDtForUpload1
        public static List<GetTariffBookDtForUpload1> GetTariffBookDtForUpload(int BookID, int UserID, String TypeItem, IDbContext ctx = null)
        {
            List<GetTariffBookDtForUpload1> result = new List<GetTariffBookDtForUpload1>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTariffBookDtForUpload1));
                ctx.CommandText = "GetTariffBookDtForUpload1";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("BookID", BookID);
                ctx.Add("UserID", UserID);
                ctx.Add("TypeItem", TypeItem);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTariffBookDtForUpload1)helper.IDataReaderToObject(reader, new GetTariffBookDtForUpload1()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetTariffEstimation
        public static List<GetTariffEstimation> GetTariffEstimation(String healthcareID, Int32 businessPartnerID, int classID, int coverageTypeID, Int32 itemID, int itemType, String transactionDate, String departmentID, IDbContext ctx = null)
        {
            List<GetTariffEstimation> result = new List<GetTariffEstimation>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTariffEstimation));
                ctx.CommandText = "GetTariffEstimation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", healthcareID);
                ctx.Add("BusinessPartnerID", businessPartnerID);
                ctx.Add("ClassID", classID);
                ctx.Add("CoverageTypeID", coverageTypeID);
                ctx.Add("ItemID", itemID);
                ctx.Add("ItemType", itemType);
                ctx.Add("TransactionDate", transactionDate);
                ctx.Add("DepartmentID", departmentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTariffEstimation)helper.IDataReaderToObject(reader, new GetTariffEstimation()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetTariffEstimationCustom
        public static List<GetTariffEstimationCustom> GetTariffEstimationCustom(String healthcareID, Int32 businessPartnerID, int classID, int coverageTypeID, Int32 itemID, int itemType, String transactionDate, String departmentID, IDbContext ctx = null)
        {
            List<GetTariffEstimationCustom> result = new List<GetTariffEstimationCustom>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTariffEstimationCustom));
                ctx.CommandText = "GetTariffEstimationCustom";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("HealthcareID", healthcareID);
                ctx.Add("BusinessPartnerID", businessPartnerID);
                ctx.Add("ClassID", classID);
                ctx.Add("CoverageTypeID", coverageTypeID);
                ctx.Add("ItemID", itemID);
                ctx.Add("ItemType", itemType);
                ctx.Add("TransactionDate", transactionDate);
                ctx.Add("DepartmentID", departmentID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTariffEstimationCustom)helper.IDataReaderToObject(reader, new GetTariffEstimationCustom()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetTimeUseInstallationType
        public static List<GetTimeUseInstallationType> GetTimeUseInstallationTypeList(string VisitID, string GCDeviceTypeID)
        {
            List<GetTimeUseInstallationType> result = new List<GetTimeUseInstallationType>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTimeUseInstallationType));
                ctx.CommandText = "GetTimeUseInstallationType";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", Convert.ToInt32(VisitID) );
                ctx.Add("GCDeviceTypeID", GCDeviceTypeID);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTimeUseInstallationType)helper.IDataReaderToObject(reader, new GetTimeUseInstallationType()));
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
		#region GetTotalAppointmentGranostic
        public static List<GetTotalAppointmentGranostic> GetTotalAppointmentGranosticList(string StartDate)
        {
            List<GetTotalAppointmentGranostic> result = new List<GetTotalAppointmentGranostic>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalAppointmentGranostic));
                ctx.CommandText = "GetTotalAppointmentGranostic";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("StartDate", StartDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalAppointmentGranostic)helper.IDataReaderToObject(reader, new GetTotalAppointmentGranostic()));
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
        public static List<GetTotalAppointmentGranostic> GetTotalAppointmentGranosticFilterList(string filterExpression)
        {
            List<GetTotalAppointmentGranostic> result = new List<GetTotalAppointmentGranostic>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalAppointmentGranostic));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalAppointmentGranostic)helper.IDataReaderToObject(reader, new GetTotalAppointmentGranostic()));
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
        #region GetTotalVisit
        public static List<GetTotalVisit> GetTotalVisitList(string RegistrationDate)
        {
            List<GetTotalVisit> result = new List<GetTotalVisit>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalVisit));
                ctx.CommandText = "GetTotalVisit";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationDate", RegistrationDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalVisit)helper.IDataReaderToObject(reader, new GetTotalVisit()));
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
        public static List<GetTotalVisit> GetTotalVisitFilterList(string filterExpression)
        {
            List<GetTotalVisit> result = new List<GetTotalVisit>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalVisit));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalVisit)helper.IDataReaderToObject(reader, new GetTotalVisit()));
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
        #region GetTotalLaboratoryTransactionPerMonth
        public static List<GetTotalLaboratoryTransactionPerMonth> GetTotalLaboratoryTransactionPerMonth(Int32 Year, Int32 Month)
        {
            List<GetTotalLaboratoryTransactionPerMonth> result = new List<GetTotalLaboratoryTransactionPerMonth>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalLaboratoryTransactionPerMonth));
                ctx.CommandText = "GetTotalLaboratoryTransactionPerMonth";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalLaboratoryTransactionPerMonth)helper.IDataReaderToObject(reader, new GetTotalLaboratoryTransactionPerMonth()));
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
        #region GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap
        public static List<GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap> GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap(Int32 Year, Int32 Month, Int32 ItemGroupID)
        {
            List<GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap> result = new List<GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap));
                ctx.CommandText = "GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                ctx.Add("ItemGroupID", ItemGroupID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap)helper.IDataReaderToObject(reader, new GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap()));
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
        #region GetTotalRadiologiTransactionPerMonth
        public static List<GetTotalRadiologiTransactionPerMonth> GetTotalRadiologiTransactionPerMonth(Int32 Year, Int32 Month)
        {
            List<GetTotalRadiologiTransactionPerMonth> result = new List<GetTotalRadiologiTransactionPerMonth>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalRadiologiTransactionPerMonth));
                ctx.CommandText = "GetTotalLaboratoryTransactionPerMonth";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalRadiologiTransactionPerMonth)helper.IDataReaderToObject(reader, new GetTotalRadiologiTransactionPerMonth()));
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
        #region GetTotalVisitInpatientAndOutpatientPerYear
        public static List<GetTotalVisitInpatientAndOutpatientPerYear> GetTotalVisitInpatientAndOutpatientPerYear(Int32 Year)
        {
            List<GetTotalVisitInpatientAndOutpatientPerYear> result = new List<GetTotalVisitInpatientAndOutpatientPerYear>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTotalVisitInpatientAndOutpatientPerYear));
                ctx.CommandText = "GetTotalVisitInpatientAndOutpatientPerYear";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTotalVisitInpatientAndOutpatientPerYear)helper.IDataReaderToObject(reader, new GetTotalVisitInpatientAndOutpatientPerYear()));
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
        #region GetTransferAccountBank
        public static List<GetTransferAccountBank> GetTransferAccountBankList(Int32 ARInvoiceID)
        {
            List<GetTransferAccountBank> result = new List<GetTransferAccountBank>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransferAccountBank));
                ctx.CommandText = "GetTransferAccountBank";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ARInvoiceID", ARInvoiceID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransferAccountBank)helper.IDataReaderToObject(reader, new GetTransferAccountBank()));
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
        #region GetTransferAccountBankRSRT
        public static List<GetTransferAccountBankRSRT> GetTransferAccountBankRSRTList(Int32 ARInvoiceID)
        {
            List<GetTransferAccountBankRSRT> result = new List<GetTransferAccountBankRSRT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransferAccountBankRSRT));
                ctx.CommandText = "GetTransferAccountBankRSRT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ARInvoiceID", ARInvoiceID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransferAccountBankRSRT)helper.IDataReaderToObject(reader, new GetTransferAccountBankRSRT()));
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
        #region GetTransferAccountBankRSSC
        public static List<GetTransferAccountBankRSSC> GetTransferAccountBankRSSCList(Int32 ARInvoiceID)
        {
            List<GetTransferAccountBankRSSC> result = new List<GetTransferAccountBankRSSC>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransferAccountBankRSSC));
                ctx.CommandText = "GetTransferAccountBankRSSC";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ARInvoiceID", ARInvoiceID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransferAccountBankRSSC)helper.IDataReaderToObject(reader, new GetTransferAccountBankRSSC()));
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
        #region GetTransRevenueSharingDateRSPBT
        public static List<GetTransRevenueSharingDateRSPBT> GetTransRevenueSharingDateRSPBTList(string filterExpression)
        {
            List<GetTransRevenueSharingDateRSPBT> result = new List<GetTransRevenueSharingDateRSPBT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingDateRSPBT));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingDateRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingDateRSPBT()));
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
        public static List<GetTransRevenueSharingDateRSPBT> GetTransRevenueSharingDateRSPBTList(string filterExpression, IDbContext ctx)
        {
            List<GetTransRevenueSharingDateRSPBT> result = new List<GetTransRevenueSharingDateRSPBT>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingDateRSPBT));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingDateRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingDateRSPBT()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }
        public static List<GetTransRevenueSharingDateRSPBT> GetTransRevenueSharingDateRSPBTList(string filterExpression, int numRows, int pageIndex, string orderByExpression = "")
        {
            List<GetTransRevenueSharingDateRSPBT> result = new List<GetTransRevenueSharingDateRSPBT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingDateRSPBT));
                ctx.CommandText = helper.Select(filterExpression, numRows, pageIndex, orderByExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingDateRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingDateRSPBT()));
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

        public static GetTransRevenueSharingDateRSPBT GetTransRevenueSharingDateRSPBT(string filterExpression, int pageIndex, string orderByExpression = "")
        {
            List<GetTransRevenueSharingDateRSPBT> result = new List<GetTransRevenueSharingDateRSPBT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingDateRSPBT));
                ctx.CommandText = helper.SelectByPageIndex(filterExpression, pageIndex, orderByExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingDateRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingDateRSPBT()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            if (result.Count > 0)
                return result[0];
            return null;
        }
        #endregion
        #region GetTransRevenueSharingPaymentPerPeriodePerParamedic
        public static List<GetTransRevenueSharingPaymentPerPeriodePerParamedic> GetTransRevenueSharingPaymentPerPeriodePerParamedic(string ReportParameter)
        {
            List<GetTransRevenueSharingPaymentPerPeriodePerParamedic> result = new List<GetTransRevenueSharingPaymentPerPeriodePerParamedic>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingPaymentPerPeriodePerParamedic));
                ctx.CommandText = "GetTransRevenueSharingPaymentPerPeriodePerParamedic";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ReportParameter", ReportParameter);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingPaymentPerPeriodePerParamedic)helper.IDataReaderToObject(reader, new GetTransRevenueSharingPaymentPerPeriodePerParamedic()));
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
        #region GetTransRevenueSharingRSPBT
        public static List<GetTransRevenueSharingRSPBT> GetTransRevenueSharingRSPBTList(string filterExpression)
        {
            List<GetTransRevenueSharingRSPBT> result = new List<GetTransRevenueSharingRSPBT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingRSPBT));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingRSPBT()));
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
        public static List<GetTransRevenueSharingRSPBT> GetTransRevenueSharingRSPBTList(string filterExpression, IDbContext ctx)
        {
            List<GetTransRevenueSharingRSPBT> result = new List<GetTransRevenueSharingRSPBT>();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingRSPBT));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingRSPBT()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }
        public static List<GetTransRevenueSharingRSPBT> GetTransRevenueSharingRSPBTList(string filterExpression, int numRows, int pageIndex, string orderByExpression = "")
        {
            List<GetTransRevenueSharingRSPBT> result = new List<GetTransRevenueSharingRSPBT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingRSPBT));
                ctx.CommandText = helper.Select(filterExpression, numRows, pageIndex, orderByExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingRSPBT()));
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

        public static GetTransRevenueSharingRSPBT GetTransRevenueSharingRSPBT(string filterExpression, int pageIndex, string orderByExpression = "")
        {
            List<GetTransRevenueSharingRSPBT> result = new List<GetTransRevenueSharingRSPBT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingRSPBT));
                ctx.CommandText = helper.SelectByPageIndex(filterExpression, pageIndex, orderByExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingRSPBT)helper.IDataReaderToObject(reader, new GetTransRevenueSharingRSPBT()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ctx.Close();
            }
            if (result.Count > 0)
                return result[0];
            return null;
        }
        #endregion
        #region GetTransRevenueSharingSummaryReportRSDOSOBA
        public static List<GetTransRevenueSharingSummaryReportRSDOSOBA> GetTransRevenueSharingSummaryReportRSDOSOBAList(Int32 RSSummaryID)
        {
            List<GetTransRevenueSharingSummaryReportRSDOSOBA> result = new List<GetTransRevenueSharingSummaryReportRSDOSOBA>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTransRevenueSharingSummaryReportRSDOSOBA));
                ctx.CommandText = "GetTransRevenueSharingSummaryReportRSDOSOBA";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTransRevenueSharingSummaryReportRSDOSOBA)helper.IDataReaderToObject(reader, new GetTransRevenueSharingSummaryReportRSDOSOBA()));

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
        #region GetTreasuryActivityHdDetail
        public static List<GetTreasuryActivityHdDetail> GetTreasuryActivityHdDetailList(
                                        Int32 Year, Int32 Month, Int32 GLAccountTreasuryID, Int32 TransactionStatus)
        {
            List<GetTreasuryActivityHdDetail> result = new List<GetTreasuryActivityHdDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTreasuryActivityHdDetail));
                ctx.CommandText = "GetTreasuryActivityHdDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", Year);
                ctx.Add("Month", Month);
                ctx.Add("GLAccountTreasuryID", GLAccountTreasuryID);
                ctx.Add("TransactionStatus", TransactionStatus);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTreasuryActivityHdDetail)helper.IDataReaderToObject(reader, new GetTreasuryActivityHdDetail()));

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
        #region GetTRSSummaryDtParamedicTaxBalance
        public static List<GetTRSSummaryDtParamedicTaxBalance> GetTRSSummaryDtParamedicTaxBalanceList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtParamedicTaxBalance> result = new List<GetTRSSummaryDtParamedicTaxBalance>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtParamedicTaxBalance));
                ctx.CommandText = "GetTRSSummaryDtParamedicTaxBalance";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtParamedicTaxBalance)helper.IDataReaderToObject(reader, new GetTRSSummaryDtParamedicTaxBalance()));

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
        #region GetTRSSummaryDtParamedicTaxBalanceRSAJ
        public static List<GetTRSSummaryDtParamedicTaxBalanceRSAJ> GetTRSSummaryDtParamedicTaxBalanceRSAJList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtParamedicTaxBalanceRSAJ> result = new List<GetTRSSummaryDtParamedicTaxBalanceRSAJ>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtParamedicTaxBalanceRSAJ));
                ctx.CommandText = "GetTRSSummaryDtParamedicTaxBalanceRSAJ";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtParamedicTaxBalanceRSAJ)helper.IDataReaderToObject(reader, new GetTRSSummaryDtParamedicTaxBalanceRSAJ()));

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
        #region GetTRSSummaryDtSummaryTotal
        public static List<GetTRSSummaryDtSummaryTotal> GetTRSSummaryDtSummaryTotalList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtSummaryTotal> result = new List<GetTRSSummaryDtSummaryTotal>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtSummaryTotal));
                ctx.CommandText = "GetTRSSummaryDtSummaryTotal";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtSummaryTotal)helper.IDataReaderToObject(reader, new GetTRSSummaryDtSummaryTotal()));
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
        #region GetTRSSummaryDtTransRegistrationDetail
        public static List<GetTRSSummaryDtTransRegistrationDetail> GetTRSSummaryDtTransRegistrationDetailList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetail> result = new List<GetTRSSummaryDtTransRegistrationDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetail));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetail)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetail()));

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
        #region GetTRSSummaryDtTransRegistrationDetail2
        public static List<GetTRSSummaryDtTransRegistrationDetail2> GetTRSSummaryDtTransRegistrationDetail2List(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetail2> result = new List<GetTRSSummaryDtTransRegistrationDetail2>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetail2));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetail2";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetail2)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetail2()));

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
        #region GetTRSSummaryDtTransRegistrationDetail3
        public static List<GetTRSSummaryDtTransRegistrationDetail3> GetTRSSummaryDtTransRegistrationDetail3List(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetail3> result = new List<GetTRSSummaryDtTransRegistrationDetail3>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetail3));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetail3";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetail3)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetail3()));

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
        #region GetTRSSummaryDtTransRegistrationDetailPHS
        public static List<GetTRSSummaryDtTransRegistrationDetailPHS> GetTRSSummaryDtTransRegistrationDetailPHSList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailPHS> result = new List<GetTRSSummaryDtTransRegistrationDetailPHS>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailPHS));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailPHS";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailPHS)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailPHS()));

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
        #region GetTRSSummaryDtTransRegistrationDetailRSAJ
        public static List<GetTRSSummaryDtTransRegistrationDetailRSAJ> GetTRSSummaryDtTransRegistrationDetailRSAJList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailRSAJ> result = new List<GetTRSSummaryDtTransRegistrationDetailRSAJ>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailRSAJ));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailRSAJ";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailRSAJ)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailRSAJ()));

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
        #region GetTRSSummaryDtTransRegistrationDetailRSASIH
        public static List<GetTRSSummaryDtTransRegistrationDetailRSASIH> GetTRSSummaryDtTransRegistrationDetailRSASIHList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailRSASIH> result = new List<GetTRSSummaryDtTransRegistrationDetailRSASIH>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailRSASIH));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailRSASIH";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailRSASIH)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailRSASIH()));

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
        #region GetTRSSummaryDtTransRegistrationDetailRSDO
        public static List<GetTRSSummaryDtTransRegistrationDetailRSDO> GetTRSSummaryDtTransRegistrationDetailRSDOList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailRSDO> result = new List<GetTRSSummaryDtTransRegistrationDetailRSDO>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailRSDO));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailRSDO";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailRSDO)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailRSDO()));

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
        #region GetTRSSummaryDtTransRegistrationDetailRSPBT
        public static List<GetTRSSummaryDtTransRegistrationDetailRSPBT> GetTRSSummaryDtTransRegistrationDetailRSPBTList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailRSPBT> result = new List<GetTRSSummaryDtTransRegistrationDetailRSPBT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailRSPBT));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailRSPBT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailRSPBT)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailRSPBT()));

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
        #region GetTRSSummaryDtTransRegistrationDetailRSPW
        public static List<GetTRSSummaryDtTransRegistrationDetailRSPW> GetTRSSummaryDtTransRegistrationDetailRSPWList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailRSPW> result = new List<GetTRSSummaryDtTransRegistrationDetailRSPW>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailRSPW));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailRSPW";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailRSPW)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailRSPW()));

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
        #region GetTRSSummaryDtTransRegistrationDetailRSSES
        public static List<GetTRSSummaryDtTransRegistrationDetailRSSES> GetTRSSummaryDtTransRegistrationDetailRSSESList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailRSSES> result = new List<GetTRSSummaryDtTransRegistrationDetailRSSES>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailRSSES));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailRSSES";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailRSSES)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailRSSES()));

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
        #region GetTRSSummaryTransRegistrationPerRevenue
        public static List<GetTRSSummaryTransRegistrationPerRevenue> GetGetTRSSummaryTransRegistrationPerRevenueList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryTransRegistrationPerRevenue> result = new List<GetTRSSummaryTransRegistrationPerRevenue>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryTransRegistrationPerRevenue));
                ctx.CommandText = "GetTRSSummaryTransRegistrationPerRevenue";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryTransRegistrationPerRevenue)helper.IDataReaderToObject(reader, new GetTRSSummaryTransRegistrationPerRevenue()));

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
        #region GetTRSSummaryTransRegistrationPerRevenueRSPW
        public static List<GetTRSSummaryTransRegistrationPerRevenueRSPW> GetTRSSummaryTransRegistrationPerRevenueRSPWList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryTransRegistrationPerRevenueRSPW> result = new List<GetTRSSummaryTransRegistrationPerRevenueRSPW>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryTransRegistrationPerRevenueRSPW));
                ctx.CommandText = "GetTRSSummaryTransRegistrationPerRevenueRSPW";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryTransRegistrationPerRevenueRSPW)helper.IDataReaderToObject(reader, new GetTRSSummaryTransRegistrationPerRevenueRSPW()));

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
        #region GetTRSSummaryDtTransRegistrationDetailRSRT
        public static List<GetTRSSummaryDtTransRegistrationDetailRSRT> GetTRSSummaryDtTransRegistrationDetailRSRTList(Int32 RSSummaryID)
        {
            List<GetTRSSummaryDtTransRegistrationDetailRSRT> result = new List<GetTRSSummaryDtTransRegistrationDetailRSRT>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetTRSSummaryDtTransRegistrationDetailRSRT));
                ctx.CommandText = "GetTRSSummaryDtTransRegistrationDetailRSRT";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RSSummaryID", RSSummaryID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetTRSSummaryDtTransRegistrationDetailRSRT)helper.IDataReaderToObject(reader, new GetTRSSummaryDtTransRegistrationDetailRSRT()));

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
        #region GetUserMenuAccess
        public static List<GetUserMenuAccess> GetUserMenuAccess(String moduleID, String healthcareID, int userID, string additionalFilterExpression)
        {
            List<GetUserMenuAccess> result = new List<GetUserMenuAccess>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetUserMenuAccess));
                ctx.CommandText = "GetUserMenuAccess";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_ModuleID", moduleID);
                ctx.Add("p_HealthcareID", healthcareID);
                ctx.Add("p_UserID", userID);
                ctx.Add("p_AdditionalFilterExpression", additionalFilterExpression);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetUserMenuAccess)helper.IDataReaderToObject(reader, new GetUserMenuAccess()));
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
        #region GetUserMenuList
        public static List<GetUserMenuList> GetUserMenuList(String moduleID, String healthcareID, Int32 userID, String loginHealthcareID, Int32 loginUserID)
        {
            List<GetUserMenuList> result = new List<GetUserMenuList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetUserMenuList));
                ctx.CommandText = "GetUserMenuList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_ModuleID", moduleID);
                ctx.Add("p_HealthcareID", healthcareID);
                ctx.Add("p_UserID", userID);
                ctx.Add("p_LoginHealthcareID", loginHealthcareID);
                ctx.Add("p_LoginUserID", loginUserID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetUserMenuList)helper.IDataReaderToObject(reader, new GetUserMenuList()));
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
        #region GetUserRoleMenuList
        public static List<GetUserRoleMenuList> GetUserRoleMenuList(String moduleID, String healthcareID, Int32 roleID, String loginHealthcareID, Int32 loginUserID)
        {
            List<GetUserRoleMenuList> result = new List<GetUserRoleMenuList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetUserRoleMenuList));
                ctx.CommandText = "GetUserRoleMenuList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_ModuleID", moduleID);
                ctx.Add("p_HealthcareID", healthcareID);
                ctx.Add("p_RoleID", roleID);
                ctx.Add("p_LoginHealthcareID", loginHealthcareID);
                ctx.Add("p_LoginUserID", loginUserID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetUserRoleMenuList)helper.IDataReaderToObject(reader, new GetUserRoleMenuList()));
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
        #region GetUserRoleMenuTabList
        public static List<GetUserRoleTabMenuList> GetUserRoleMenuTabList(String moduleID, String healthcareID, String lstRoleID, String loginHealthcareID, Int32 loginUserID, String ParentMenuCode)
        {
            List<GetUserRoleTabMenuList> result = new List<GetUserRoleTabMenuList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetUserRoleTabMenuList));
                ctx.CommandText = "GetUserRoleMenuTabList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("p_ModuleID", moduleID);
                ctx.Add("p_HealthcareID", healthcareID);
                ctx.Add("p_RoleID", lstRoleID);
                ctx.Add("p_LoginHealthcareID", loginHealthcareID);
                ctx.Add("p_LoginUserID", loginUserID);
                ctx.Add("p_MenuParentCode", ParentMenuCode);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetUserRoleTabMenuList)helper.IDataReaderToObject(reader, new GetUserRoleTabMenuList()));
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
     
        #region GetVisiteWithHonor
        public static List<GetVisiteWithHonor> GetVisiteWithHonorList(int RegistrationID, DateTime RegistrationDate, DateTime ParamEndDate, int ParamedicID)
        {
            List<GetVisiteWithHonor> result = new List<GetVisiteWithHonor>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetVisiteWithHonor));
                ctx.CommandText = "GetVisiteWithHonor";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                ctx.Add("RegistrationDate", RegistrationDate);
                //ctx.Add("ParamEndDate", ParamEndDate);
                ctx.Add("Date", ParamEndDate);
                ctx.Add("ParamedicID", ParamedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetVisiteWithHonor)helper.IDataReaderToObject(reader, new GetVisiteWithHonor()));
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
        #region GetVitalSignRSSBB
        public static List<GetVitalSignRSSBB> GetVitalSignRSSBBList(Int32 RegistrationID)
        {
            List<GetVitalSignRSSBB> result = new List<GetVitalSignRSSBB>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetVitalSignRSSBB));
                ctx.CommandText = "GetVitalSignRSSBB";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetVitalSignRSSBB)helper.IDataReaderToObject(reader, new GetVitalSignRSSBB()));
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
        #region GetWabahMingguanRawatJalan
        public static List<GetWabahMingguanRawatJalan> GetWabahMingguanRawatJalanList(string filterExpression)
        {
            List<GetWabahMingguanRawatJalan> result = new List<GetWabahMingguanRawatJalan>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetWabahMingguanRawatJalan));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetWabahMingguanRawatJalan)helper.IDataReaderToObject(reader, new GetWabahMingguanRawatJalan()));
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

        #region Journal
        #region PostingJournal v2
        public static bool PostingJournalv2(String HealthcareID, String PeriodNo, Boolean IsAuditedJournal, Int32 CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "PostingJournalv2";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareID", HealthcareID));
            ctx.Command.Parameters.Add(new SqlParameter("@PeriodNo", PeriodNo));
            ctx.Command.Parameters.Add(new SqlParameter("@IsAuditedJournal", IsAuditedJournal));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
            ctx.Command.CommandTimeout = 300;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #region ReopenJournal
        public static bool ReopenJournal(Int32 GLTransactionID, Int32 ReopenBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ReopenJournal";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@GLTransactionID", GLTransactionID));
            ctx.Command.Parameters.Add(new SqlParameter("@ReopenBy", ReopenBy));
            ctx.Command.CommandTimeout = 200;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #region ClosingJournal
        public static bool ClosingJournal(String HealthcareID, String PeriodNo, Int32 CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ClosingJournal";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareID", HealthcareID));
            ctx.Command.Parameters.Add(new SqlParameter("@PeriodNo", PeriodNo));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
            ctx.Command.CommandTimeout = 200;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #region PostingJournal
        public static bool PostingJournal(String HealthcareID, String PeriodNo, Int32 CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "PostingJournal";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareID", HealthcareID));
            ctx.Command.Parameters.Add(new SqlParameter("@PeriodNo", PeriodNo));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
            ctx.Command.CommandTimeout = 200;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #region RecalculationJournal
        public static bool RecalculationJournal(String PeriodNo, Int32 CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "RecalculationJournal";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@PeriodNo", PeriodNo));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
            ctx.Command.CommandTimeout = 200;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #region UnpostingJournal
        public static bool UnpostingJournal(String HealthcareID, String JournalNo, Int32 CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "UnpostingJournal";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareID", HealthcareID));
            ctx.Command.Parameters.Add(new SqlParameter("@JournalNo", JournalNo));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
            ctx.Command.CommandTimeout = 200;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #region UnpostingJournal V2
        public static bool UnpostingJournalv2(String HealthcareID, String JournalNo, Int32 CreatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "UnpostingJournalv2";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareID", HealthcareID));
            ctx.Command.Parameters.Add(new SqlParameter("@JournalNo", JournalNo));
            ctx.Command.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
            ctx.Command.CommandTimeout = 300;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #region ProcessInterfaceJournal
        public static string ProcessInterfaceJournal(String HealthcareID, String JournalDate, String TransactionCode, int UserID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ProcessInterfaceJournal";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareID", HealthcareID));
            ctx.Command.Parameters.Add(new SqlParameter("@JournalDate", JournalDate));
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionCode", TransactionCode));
            ctx.Command.Parameters.Add(new SqlParameter("@UserID", UserID));
            ctx.Command.CommandTimeout = 200;

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);
            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (string)param.Value;
        }
        #endregion

        #endregion
      
        #region ProcessPatientCensus
        public static bool ProcessPatientCensus(DateTime CensusDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ProcessPatientCensus";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@CensusDate", CensusDate));
            //SqlParameter param = new SqlParameter();
            //param.ParameterName = "@Result";
            //param.SqlDbType = SqlDbType.Bit;
            //param.Size = 1;
            //param.Direction = ParameterDirection.Output;
            //ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            //return (bool)param.Value;
            return true;
        }
        #endregion
        #region ProcessRevenueSharingEditItem
        public static bool ProcessRevenueSharingEditItem(Int32 ParamedicID, Int32 ItemID, Int32 ClassID, DateTime StartDate, DateTime EndDate, String GCParamedicRole, Int32 RevenueSharingID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ProcessRevenueSharingEditItem";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", ParamedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@ItemID", ItemID));
            ctx.Command.Parameters.Add(new SqlParameter("@ClassID", ClassID));
            ctx.Command.Parameters.Add(new SqlParameter("@StartDate", StartDate));
            ctx.Command.Parameters.Add(new SqlParameter("@EndDate", EndDate));
            ctx.Command.Parameters.Add(new SqlParameter("@GCParamedicRole", GCParamedicRole));
            ctx.Command.Parameters.Add(new SqlParameter("@RevenueSharingID", RevenueSharingID));

            //SqlParameter param = new SqlParameter();
            //param.ParameterName = "@Result";
            //param.SqlDbType = SqlDbType.Bit;
            //param.Size = 1;
            //param.Direction = ParameterDirection.Output;
            //ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            //return (bool)param.Value;
            return true;
        }
        #endregion
        #region ProcessARProportional
        public static bool ProcessARProportional(int ARInvoiceID, int UserID, IDbContext ctx = null)
        {
            bool result = true;
            bool IsCtxNull = false;

            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ProcessARProportional";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@ARInvoiceID", ARInvoiceID));
            ctx.Command.Parameters.Add(new SqlParameter("@UserID", UserID));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return result;
        }
        #endregion
        #region ProcessBPJSProportionalFinalClaim
        public static bool ProcessBPJSProportionalFinalClaim(int RegistrationID, bool IsBPJSFinal, decimal GrouperAmountFinal, int LastUpdatedBy, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ProcessBPJSProportionalFinalClaim";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RegistrationID", RegistrationID));
            ctx.Command.Parameters.Add(new SqlParameter("@IsBPJSFinal", IsBPJSFinal));
            ctx.Command.Parameters.Add(new SqlParameter("@GrouperAmountFinal", GrouperAmountFinal));
            ctx.Command.Parameters.Add(new SqlParameter("@LastUpdatedBy", LastUpdatedBy));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return true;
        }
        #endregion
        #region ProcessItemPlanningChangedInsertItemPriceHistory
        public static bool ProcessItemPlanningChangedInsertItemPriceHistory(string FromRegionMenu, int ItemPlanningID, decimal OldAveragePrice, decimal OldUnitPrice, decimal OldPurchasePrice, bool OldIsPriceLastUpdatedBySystem, bool OldIsDeleted, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "ProcessItemPlanningChangedInsertItemPriceHistory";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@FromRegionMenu", FromRegionMenu));
            ctx.Command.Parameters.Add(new SqlParameter("@ItemPlanningID", ItemPlanningID));
            ctx.Command.Parameters.Add(new SqlParameter("@OldAveragePrice", OldAveragePrice));
            ctx.Command.Parameters.Add(new SqlParameter("@OldUnitPrice", OldUnitPrice));
            ctx.Command.Parameters.Add(new SqlParameter("@OldPurchasePrice", OldPurchasePrice));
            ctx.Command.Parameters.Add(new SqlParameter("@OldIsPriceLastUpdatedBySystem", OldIsPriceLastUpdatedBySystem));
            ctx.Command.Parameters.Add(new SqlParameter("@OldIsDeleted", OldIsDeleted));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return true;
        }
        #endregion
        #region GetINACBGTariff
        public static List<GetINACBGTariff> GetINACBGGrouperTariff(string jnsrawat, string klsrawat, string diagnosisCode, string procedureCode, IDbContext ctx = null)
        {
            List<GetINACBGTariff> result = new List<GetINACBGTariff>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetINACBGTariff));
                ctx.CommandText = "GetINACBGGrouperTariff";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("JnsRawat", jnsrawat);
                ctx.Add("Klsrawat", klsrawat);
                ctx.Add("DiagnosisCode", diagnosisCode);
                ctx.Add("ProcedureCode", procedureCode);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetINACBGTariff)helper.IDataReaderToObject(reader, new GetINACBGTariff()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetImportTransactionJournalList
        public static List<GetExportTransactionJournal> GetExportTransactionJournalList(string journalDate, IDbContext ctx = null)
        {
            List<GetExportTransactionJournal> result = new List<GetExportTransactionJournal>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(GetExportTransactionJournal));
                ctx.CommandText = "GetExportTransactionJournal";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("JournalDate", journalDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetExportTransactionJournal)helper.IDataReaderToObject(reader, new GetExportTransactionJournal()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region GetPatientPaymentDTOList
        public static List<PatientPaymentEntryDTO> GetPatientPaymentDTOList(string paymentDate, IDbContext ctx = null)
        {
            List<PatientPaymentEntryDTO> result = new List<PatientPaymentEntryDTO>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientPaymentEntryDTO));
                ctx.CommandText = "GetExportTransactionPatientPayment";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PaymentDate", paymentDate);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientPaymentEntryDTO)helper.IDataReaderToObject(reader, new PatientPaymentEntryDTO()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion
        #region Maspion
        #region  InsertPaymentIDToMaspionDbLink
        public static bool InsertPaymentIDToMaspionDbLink(String PaymentID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "InsertPaymentIDToMaspionDbLink";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@PaymentID", PaymentID));
            ctx.Command.CommandTimeout = 200;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.Bit;
            param.Size = 1;
            param.Direction = ParameterDirection.Output;
            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return (bool)param.Value;
        }
        #endregion
        #endregion
        #region MergeMedicalNumber
        public static void MergeMedicalNumber(int mrn1, int mrn2, bool isReused, int userID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "MergeMedicalNumber";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@MRN1", mrn1));
            ctx.Command.Parameters.Add(new SqlParameter("@MRN2", mrn2));
            ctx.Command.Parameters.Add(new SqlParameter("@IsReused", isReused));
            ctx.Command.Parameters.Add(new SqlParameter("@UserID", userID));
            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

        }
        #endregion
        #region MoveGuestToMedicalNumber
        public static void MoveGuestToMedicalNumber(int guestID, int mrn2, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "MoveGuestToMedicalNumber";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@GuestID", guestID));
            ctx.Command.Parameters.Add(new SqlParameter("@MRN2", mrn2));
            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

        }
        #endregion

        #region SendToLISInterfaceDB
        public static void SendToLISInterfaceDB(int transactionID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "SendToLISInterfaceDB";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionID", transactionID));
            //ctx.Command.Parameters.Add(new SqlParameter("@IsDeleted", isDeleted ? 1 : 0));
            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

        }
        #endregion

        #region SendToLISInterfaceDB_GRACIA
        public static void SendToLISInterfaceDB_GRACIA(int transactionID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "SendToLISInterfaceDB_GRACIA";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionID", transactionID));
            //ctx.Command.Parameters.Add(new SqlParameter("@IsDeleted", isDeleted ? 1 : 0));
            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

        }
        #endregion

        #region OnInsertBPJSTaskLog
        public static void OnInsertBPJSTaskLog(Int32 RegistrationID, Int32 TaskID, Int32 UserID, DateTime LogDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "OnInsertBPJSTaskLog";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RegistrationID", RegistrationID));
            ctx.Command.Parameters.Add(new SqlParameter("@TaskID", TaskID));
            ctx.Command.Parameters.Add(new SqlParameter("@UserID", UserID));
            ctx.Command.Parameters.Add(new SqlParameter("@LogDate", LogDate));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
        }
        #endregion
        #region OnUpdateQueueLastNo
        public static void OnUpdateQueueLastNo(Int32 HealthcareServiceUnitID, Int32 ParamedicID, Int32 Session, Int32 QueueNo, DateTime StartDate, Int32 OldHealthcareServiceUnitID, Int32 OldParamedicID, Int32 OldSession, DateTime OldStartDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "OnUpdateQueueLastNo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@HealthcareServiceUnitID", HealthcareServiceUnitID));
            ctx.Command.Parameters.Add(new SqlParameter("@ParamedicID", ParamedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@Session", Session));
            ctx.Command.Parameters.Add(new SqlParameter("@QueueNo", QueueNo));
            ctx.Command.Parameters.Add(new SqlParameter("@StartDate", StartDate));
            ctx.Command.Parameters.Add(new SqlParameter("@OldHealthcareServiceUnitID", OldHealthcareServiceUnitID));
            ctx.Command.Parameters.Add(new SqlParameter("@OldParamedicID", OldParamedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@OldSession", OldSession));
            ctx.Command.Parameters.Add(new SqlParameter("@OldStartDate", OldStartDate));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
        }
        #endregion

        #region TransferPatientToPhysician
        public static void TransferPatientToPhysician(int visitID, int fromParamedicID, int toParamedicID, bool isChangeMedicalRecord, bool isChangeTransaction, string gcTransferReason, string otherReason, int userID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "TransferPatientToPhysician";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@VisitID", visitID));
            ctx.Command.Parameters.Add(new SqlParameter("@FromParamedicID", fromParamedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@ToParamedicID", toParamedicID));
            ctx.Command.Parameters.Add(new SqlParameter("@IsChangeMedicalRecord", isChangeMedicalRecord));
            ctx.Command.Parameters.Add(new SqlParameter("@IsChangeTransaction", isChangeTransaction));
            ctx.Command.Parameters.Add(new SqlParameter("@GCTransferReason", gcTransferReason));
            ctx.Command.Parameters.Add(new SqlParameter("@OtherReason", otherReason));
            ctx.Command.Parameters.Add(new SqlParameter("@UserID", userID));
            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

        }
        #endregion

        #region GetLaboratoryResultFromLIS
        public static List<GetLaboratoryResultFromLIS> GetLaboratoryResultFromLISList(int transactionID, IDbContext ctx = null)
        {
            List<GetLaboratoryResultFromLIS> result = new List<GetLaboratoryResultFromLIS>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetLaboratoryResultFromLIS";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionID", transactionID));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(GetLaboratoryResultFromLIS));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetLaboratoryResultFromLIS)helper.IDataReaderToObject(reader, new GetLaboratoryResultFromLIS()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region GetPatientLaboratoryHistory
        public static List<PatientLaboratoryHistory> GetPatientLaboratoryHistoryList(int mrn, IDbContext ctx = null)
        {
            List<PatientLaboratoryHistory> result = new List<PatientLaboratoryHistory>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetPatientLaboratoryHistory";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@MRN", mrn));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(PatientLaboratoryHistory));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientLaboratoryHistory)helper.IDataReaderToObject(reader, new PatientLaboratoryHistory()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region MedicationChartItem
        public static List<MedicationChartItem> GetMedicationChartItemList(string visitID, string displayMode, string isUsingUDD = "0", IDbContext ctx = null)
        {
            List<MedicationChartItem> result = new List<MedicationChartItem>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetMedicationChartItem";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@VisitID", visitID));
            ctx.Command.Parameters.Add(new SqlParameter("@DisplayMode", displayMode));
            ctx.Command.Parameters.Add(new SqlParameter("@IsUsingUDD", isUsingUDD));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(MedicationChartItem));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((MedicationChartItem)helper.IDataReaderToObject(reader, new MedicationChartItem()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region PRNMedicationItem
        public static List<PRNMedicationItem> GetPRNMedicationItemList(string visitID, string displayMode, IDbContext ctx = null)
        {
            List<PRNMedicationItem> result = new List<PRNMedicationItem>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetPRNMedicationItem";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@VisitID", visitID));
            ctx.Command.Parameters.Add(new SqlParameter("@DisplayMode", displayMode));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(PRNMedicationItem));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PRNMedicationItem)helper.IDataReaderToObject(reader, new PRNMedicationItem()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region EpisodeMedication
        public static List<EpisodeMedication> GetEpisodeMedicationList(string visitID, IDbContext ctx = null)
        {
            List<EpisodeMedication> result = new List<EpisodeMedication>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetEpisodeMedicationList";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@VisitID", visitID));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(EpisodeMedication));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((EpisodeMedication)helper.IDataReaderToObject(reader, new EpisodeMedication()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region BPJSPatientChronicMedication
        public static List<BPJSPatientChronicMedication> GetBPJSPatientChronicMedicationList(int mrn, int itemID, int duration, IDbContext ctx = null)
        {
            List<BPJSPatientChronicMedication> result = new List<BPJSPatientChronicMedication>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetBPJSPatientChronicMedicationList";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@MRN", mrn));
            ctx.Command.Parameters.Add(new SqlParameter("@ItemID", itemID));
            ctx.Command.Parameters.Add(new SqlParameter("@Duration", duration));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(EpisodeMedication));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((BPJSPatientChronicMedication)helper.IDataReaderToObject(reader, new BPJSPatientChronicMedication()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region GetPatientBannerVitalSignInfo
        public static List<PatientBannerVitalSignInfo> GetPatientBannerVitalSignInfo(int registrationID, IDbContext ctx = null)
        {
            List<PatientBannerVitalSignInfo> result = new List<PatientBannerVitalSignInfo>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetPatientBannerVitalSignInfo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RegistrationID", registrationID));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(PatientBannerVitalSignInfo));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientBannerVitalSignInfo)helper.IDataReaderToObject(reader, new PatientBannerVitalSignInfo()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region GenerateBPJSNoSuratKontrol
        public static string GenerateNoSuratKontrolBPJS(DateTime transactionDate)
        {
            return GenerateNoSuratKontrolBPJS(transactionDate, null);
        }
        public static string GenerateNoSuratKontrolBPJS(DateTime transactionDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateNoSuratKontrolBPJS";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionDate", transactionDate));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion

        #region GeneratePrescriptionReferenceNo
        public static string GeneratePrescriptionReferenceNo(int dispensaryUnitID, DateTime transactionDate)
        {
            return GeneratePrescriptionReferenceNo(dispensaryUnitID, transactionDate, null);
        }
        public static string GeneratePrescriptionReferenceNo(int dispensaryUnitID, DateTime transactionDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GeneratePrescriptionReferenceNo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@DispensaryUnitID", dispensaryUnitID));
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionDate", transactionDate));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion

        #region GenerateChargesQueueNoLabel
        public static string GenerateChargesQueueNoLabel(int serviceUnitID, DateTime transactionDate, bool isSpecialTrx)
        {
            return GenerateChargesQueueNoLabel(serviceUnitID, transactionDate, isSpecialTrx, null);
        }
        public static string GenerateChargesQueueNoLabel(int serviceUnitID, DateTime transactionDate, bool isSpecialTrx, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateChargesQueueNoLabel";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@ServiceUnitID", serviceUnitID));
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionDate", transactionDate));
            ctx.Command.Parameters.Add(new SqlParameter("@IsSpecialTrx", isSpecialTrx));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion

        #region GenerateGuestNo
        public static string GenerateGuestNo(DateTime RegistrationDate)
        {
            return GenerateGuestNo(RegistrationDate, null);
        }
        public static string GenerateGuestNo(DateTime RegistrationDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateGuestNo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@RegistrationDate", RegistrationDate));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion

        #region GenerateQueueDisplayPrescriptionReferenceNo
        public static string GenerateQueueDisplayPrescriptionReferenceNo(DateTime transactionDate, int IsCompound)
        {
            return GenerateQueueDisplayPrescriptionReferenceNo(transactionDate,IsCompound,  null);
        }
        public static string GenerateQueueDisplayPrescriptionReferenceNo(DateTime transactionDate, int  IsCompound,  IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateQueueDisplayPrescriptionReferenceNo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionDate", transactionDate));
            ctx.Command.Parameters.Add(new SqlParameter("@IsCompound", IsCompound));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
      
        #endregion

        #region GetPatientMedicationSummary
        public static List<PatientMedicationSummary> GetPatientMedicationSummaryList(int registrationID, string displayMode, string statusMode)
        {
            List<PatientMedicationSummary> result = new List<PatientMedicationSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientMedicationSummary));
                ctx.CommandText = "GetPatientMedicationSummaryList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", registrationID);
                ctx.Add("DisplayMode", displayMode);
                ctx.Add("StatusMode", statusMode);
                ctx.Command.CommandTimeout = 90;
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientMedicationSummary)helper.IDataReaderToObject(reader, new PatientMedicationSummary()));
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

        #region GetPatientResidualMedicationSummary
        public static List<PatientResidualMedicationSummary> GetPatientResidualMedicationSummaryList(int registrationID, string displayMode, string statusMode)
        {
            List<PatientResidualMedicationSummary> result = new List<PatientResidualMedicationSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientResidualMedicationSummary));
                ctx.CommandText = "GetPatientResidualMedicationSummaryList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("RegistrationID", registrationID);
                ctx.Add("DisplayMode", displayMode);
                ctx.Add("StatusMode", statusMode);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientResidualMedicationSummary)helper.IDataReaderToObject(reader, new PatientResidualMedicationSummary()));
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

        #region GetPhysicianPatientList
        public static List<PhysicianPatientList> GetPhysicianPatientList(int paramedicID, string DepartmentID, string Date)
        {
            List<PhysicianPatientList> result = new List<PhysicianPatientList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PhysicianPatientList));
                ctx.CommandText = "GetPhysicianPatientList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", paramedicID);
                ctx.Add("DepartmentID", DepartmentID);
                ctx.Add("Date", Date);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PhysicianPatientList)helper.IDataReaderToObject(reader, new PhysicianPatientList()));
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

        #region GetPhysicianPatientListIP
        public static List<PhysicianPatientListIP> GetPhysicianPatientListIP(int paramedicID)
        {
            List<PhysicianPatientListIP> result = new List<PhysicianPatientListIP>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PhysicianPatientListIP));
                ctx.CommandText = "GetPhysicianPatientListIP";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("ParamedicID", paramedicID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PhysicianPatientListIP)helper.IDataReaderToObject(reader, new PhysicianPatientListIP()));
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

        #region GetPatientDiagnosisSummaryList
        public static List<PatientDiagnosisSummary> GetPatientDiagnosisSummaryList(int mrn)
        {
            List<PatientDiagnosisSummary> result = new List<PatientDiagnosisSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientDiagnosisSummary));
                ctx.CommandText = "GetPatientDiagnosisSummaryList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MRN", mrn);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientDiagnosisSummary)helper.IDataReaderToObject(reader, new PatientDiagnosisSummary()));
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
        #region GetPatientFinalDiagnosisSummaryList
        public static List<PatientFinalDiagnosisSummary> GetPatientFinalDiagnosisSummaryList(int mrn)
        {
            List<PatientFinalDiagnosisSummary> result = new List<PatientFinalDiagnosisSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientFinalDiagnosisSummary));
                ctx.CommandText = "GetPatientFinalDiagnosisSummaryList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MRN", mrn);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientFinalDiagnosisSummary)helper.IDataReaderToObject(reader, new PatientFinalDiagnosisSummary()));
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
        #region GetPatientClaimDiagnosisSummaryList
        public static List<PatientClaimDiagnosisSummary> GetPatientClaimDiagnosisSummaryList(int mrn)
        {
            List<PatientClaimDiagnosisSummary> result = new List<PatientClaimDiagnosisSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientClaimDiagnosisSummary));
                ctx.CommandText = "GetPatientClaimDiagnosisSummaryList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MRN", mrn);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientClaimDiagnosisSummary)helper.IDataReaderToObject(reader, new PatientClaimDiagnosisSummary()));
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

        #region GetPatientDiagnosisInpatientTotalDetail
        public static List<GetPatientDiagnosisInpatientTotalDetail> GetPatientDiagnosisInpatientTotalDetailList(string filterExpression)
        {
            List<GetPatientDiagnosisInpatientTotalDetail> result = new List<GetPatientDiagnosisInpatientTotalDetail>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPatientDiagnosisInpatientTotalDetail));
                ctx.CommandText = helper.Select(filterExpression);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPatientDiagnosisInpatientTotalDetail)helper.IDataReaderToObject(reader, new GetPatientDiagnosisInpatientTotalDetail()));
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

        #region GetNursingDiagnoseOutcomeItem
        public static List<GetNursingDiagnoseOutcomeItem> GetNursingDiagnoseOutcomeItem(int nursingDiagnoseID)
        {
            List<GetNursingDiagnoseOutcomeItem> result = new List<GetNursingDiagnoseOutcomeItem>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetNursingDiagnoseOutcomeItem));
                ctx.CommandText = "GetNursingDiagnoseOutcomeItem";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_NursingDiagnoseID", nursingDiagnoseID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetNursingDiagnoseOutcomeItem)helper.IDataReaderToObject(reader, new GetNursingDiagnoseOutcomeItem()));
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

        #region PopulateHSUVitalSignIndicatorList
        public static List<PopulateHSUVitalSignIndicator> PopulateHSUVitalSignIndicatorList(int healthcareServiceUnitID, string specialtyID)
        {
            List<PopulateHSUVitalSignIndicator> result = new List<PopulateHSUVitalSignIndicator>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PopulateHSUVitalSignIndicator));
                ctx.CommandText = "PopulateHSUVitalSignIndicatorList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@HealthcareServiceUnitID", healthcareServiceUnitID);
                ctx.Add("@SpecialtyID", specialtyID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PopulateHSUVitalSignIndicator)helper.IDataReaderToObject(reader, new PopulateHSUVitalSignIndicator()));
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

        #region GetVisitVitalSignSummary
        public static List<VisitVitalSignSummary> GetVisitVitalSignSummaryList(int visitID)
        {
            List<VisitVitalSignSummary> result = new List<VisitVitalSignSummary>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PopulateHSUVitalSignIndicator));
                ctx.CommandText = "GetVisitVitalSignSummary";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@VisitID", visitID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((VisitVitalSignSummary)helper.IDataReaderToObject(reader, new VisitVitalSignSummary()));
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

        #region GetEpisodeDiagnosticSupportTestItem
        public static List<EpisodeDiagnosticSupportTestItem> GetEpisodeDiagnosticSupportTestItemList(int visitID, string transactionCode)
        {
            List<EpisodeDiagnosticSupportTestItem> result = new List<EpisodeDiagnosticSupportTestItem>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(EpisodeDiagnosticSupportTestItem));
                ctx.CommandText = "GetEpisodeDiagnosticSupportTestItem";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_VisitID", visitID);
                ctx.Add("@p_TransactionCode", transactionCode);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((EpisodeDiagnosticSupportTestItem)helper.IDataReaderToObject(reader, new EpisodeDiagnosticSupportTestItem()));
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

        #region GetEpisodeSurgeryProcedureList
        public static List<EpisodeSurgeryProcedure> GetEpisodeSurgeryProcedureList(int visitID)
        {
            List<EpisodeSurgeryProcedure> result = new List<EpisodeSurgeryProcedure>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(EpisodeSurgeryProcedure));
                ctx.CommandText = "GetEpisodeSurgeryProcedureList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_VisitID", visitID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((EpisodeSurgeryProcedure)helper.IDataReaderToObject(reader, new EpisodeSurgeryProcedure()));
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

        #region PopulateMonitoringVitalSignList
        public static List<MonitoringVitalSignIndicator> PopulateMonitoringVitalSignIndicatorList(string gcMonitoringType)
        {
            List<MonitoringVitalSignIndicator> result = new List<MonitoringVitalSignIndicator>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(MonitoringVitalSignIndicator));
                ctx.CommandText = "PopulateMonitoringVitalSignList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@GCMonitoringType", gcMonitoringType);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((MonitoringVitalSignIndicator)helper.IDataReaderToObject(reader, new MonitoringVitalSignIndicator()));
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

        #region MCU MedinLink
        #region GetRegistrationListFromMCULink
        public static List<GetRegistrationListFromMCULink> GetRegistrationListFromMCULinkList(string RegistrationList)
        {
            List<GetRegistrationListFromMCULink> result = new List<GetRegistrationListFromMCULink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetRegistrationListFromMCULink));
                ctx.CommandText = "GetRegistrationListFromMCULink";
                ctx.CommandType = CommandType.StoredProcedure;

                ctx.Add("@RegistrationList ", RegistrationList);

                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetRegistrationListFromMCULink)helper.IDataReaderToObject(reader, new GetRegistrationListFromMCULink()));
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
        #region MCULink_GetRegistration
        public static List<MCULink_GetRegistration> MCULink_GetRegistrationList(string  DateRegistration, string GCCustomerType, int BussinesPartnerID)
        {
            List<MCULink_GetRegistration> result = new List<MCULink_GetRegistration>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(MCULink_GetRegistration));
                ctx.CommandText = "MCULink_GetRegistration";
                ctx.CommandType = CommandType.StoredProcedure;

                ctx.Add("@DateRegistration", DateRegistration);
                ctx.Add("@GCCustomerType",GCCustomerType);
                ctx.Add("@BussinesPartnerID", BussinesPartnerID);
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((MCULink_GetRegistration)helper.IDataReaderToObject(reader, new MCULink_GetRegistration()));
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
        #region MCULink_GetMCUResult
        public static List<MCULink_GetMCUResult> MCULink_GetMCUResultList(int VisitID)
        {
            List<MCULink_GetMCUResult> result = new List<MCULink_GetMCUResult>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(MCULink_GetMCUResult));
                ctx.CommandText = "MCULink_GetMCUResult";
                ctx.CommandType = CommandType.StoredProcedure;

                ctx.Add("@VisitID", VisitID);

                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((MCULink_GetMCUResult)helper.IDataReaderToObject(reader, new MCULink_GetMCUResult()));
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
        #region UpdateRegistrationDBLinkMCULink
        public static List<UpdateRegistrationDBLinkMCULink> UpdateRegistrationDBLinkMCULinkData(string RegistrationList)
        {
           
            List<UpdateRegistrationDBLinkMCULink> result = new List<UpdateRegistrationDBLinkMCULink>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(UpdateRegistrationDBLinkMCULink));
                ctx.CommandText = "UpdateRegistrationDBLinkMCULink";
                ctx.CommandType = CommandType.StoredProcedure;

                ctx.Add("@RegistrationList", RegistrationList);

                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((UpdateRegistrationDBLinkMCULink)helper.IDataReaderToObject(reader, new UpdateRegistrationDBLinkMCULink()));
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
        #region MCULink_GetRegistrationOnsite
        public static List<MCULink_GetRegistrationOnsite> MCULink_GetRegistrationOnsite(string RegistrationListID)
        {
            List<MCULink_GetRegistrationOnsite> result = new List<MCULink_GetRegistrationOnsite>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(MCULink_GetRegistrationOnsite));
                ctx.CommandText = "MCULink_GetRegistrationOnsite";
                ctx.CommandType = CommandType.StoredProcedure;
                ctx.Add("@RegistrationListID", RegistrationListID);

                   using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((MCULink_GetRegistrationOnsite)helper.IDataReaderToObject(reader, new MCULink_GetRegistrationOnsite()));
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

        #region GetDeviceMonitoringLogList
        public static List<GetDeviceMonitoringLog> GetDeviceMonitoringLogList(int recordID)
        {
            List<GetDeviceMonitoringLog> result = new List<GetDeviceMonitoringLog>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDeviceMonitoringLog));
                ctx.CommandText = "GetDeviceMonitoringLog";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RecordID", recordID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDeviceMonitoringLog)helper.IDataReaderToObject(reader, new GetDeviceMonitoringLog()));
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

        #region GetDeviceMonitoringLogRSSEBList
        public static List<GetDeviceMonitoringLogRSSEB> GetDeviceMonitoringLogRSSEBList(int recordID)
        {
            List<GetDeviceMonitoringLogRSSEB> result = new List<GetDeviceMonitoringLogRSSEB>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetDeviceMonitoringLogRSSEB));
                ctx.CommandText = "GetDeviceMonitoringLogRSSEB";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@RecordID", recordID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetDeviceMonitoringLogRSSEB)helper.IDataReaderToObject(reader, new GetDeviceMonitoringLogRSSEB()));
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

        #region GeneratePALaboratoryResultReferenceNo
        public static string GeneratePALaboratoryResultReferenceNo(string prefix, DateTime transactionDate)
        {
            return GeneratePALaboratoryResultReferenceNo(prefix, transactionDate, null);
        }
        public static string GeneratePALaboratoryResultReferenceNo(string prefix, DateTime transactionDate, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GeneratePALaboratoryResultReferenceNo";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@Prefix", prefix));
            ctx.Command.Parameters.Add(new SqlParameter("@TransactionDate", transactionDate));
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Result";
            param.SqlDbType = SqlDbType.VarChar;
            param.Size = 30;
            param.Direction = ParameterDirection.Output;

            ctx.Command.Parameters.Add(param);

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return (string)param.Value;
        }
        #endregion

        #region GetOperatingRoomScheduleInfo
        public static List<OperatingRoomScheduleInfo> GetGetOperatingRoomScheduleInfoList(string healthcareServiceUnitID, string fromDate, string toDate, string roomCode)
        {
            List<OperatingRoomScheduleInfo> result = new List<OperatingRoomScheduleInfo>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(OperatingRoomScheduleInfo));
                ctx.CommandText = "GetOperatingRoomScheduleInfo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_HealthcareServiceUnitID", healthcareServiceUnitID);
                ctx.Add("@p_FromDate", fromDate);
                ctx.Add("@p_ToDate", toDate);
                ctx.Add("@p_RoomCode", roomCode);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((OperatingRoomScheduleInfo)helper.IDataReaderToObject(reader, new OperatingRoomScheduleInfo()));
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

        #region GetPatientProcedureChargesList
        public static List<PatientProcedureCharges> GetPatientProcedureChargesList(int visitID, int linkedVisitID)
        {
            List<PatientProcedureCharges> result = new List<PatientProcedureCharges>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientProcedureCharges));
                ctx.CommandText = "GetPatientProcedureChargesList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_VisitID", visitID);
                ctx.Add("@p_LinkedVisitID", linkedVisitID);

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientProcedureCharges)helper.IDataReaderToObject(reader, new PatientProcedureCharges()));
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

        #region GetPatientSurgeryInformation
        public static List<PatientSurgeryInformation> GetPatientSurgeryInformation(string testOrderID)
        {
            List<PatientSurgeryInformation> result = new List<PatientSurgeryInformation>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientProcedureCharges));
                ctx.CommandText = "GetPatientSurgeryInformation";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_TestOrderID", testOrderID.ToString());

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientSurgeryInformation)helper.IDataReaderToObject(reader, new PatientSurgeryInformation()));
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

        #region GetGrowthChartPoint
        public static List<GrowthChartPointData> GetGrowthChartPointDataList(int mrn, int vitalsignID)
        {
            List<GrowthChartPointData> result = new List<GrowthChartPointData>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GrowthChartPointData));
                ctx.CommandText = "GetGrowthChartPoint";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@MRN", mrn.ToString());
                ctx.Add("@VitalSignID", vitalsignID.ToString());

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GrowthChartPointData)helper.IDataReaderToObject(reader, new GrowthChartPointData()));
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

        #region GetPhysicianInitialAssessmentDetail
        public static List<PhysicianInitialAssessmentInfo> GetPhysicianInitialAssessmentDetailInfo(int visitNoteID)
        {
            List<PhysicianInitialAssessmentInfo> result = new List<PhysicianInitialAssessmentInfo>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PhysicianInitialAssessmentInfo));
                ctx.CommandText = "GetPhysicianInitialAssessmentDetail";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("@p_PatientVisitNoteID", visitNoteID.ToString());

                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PhysicianInitialAssessmentInfo)helper.IDataReaderToObject(reader, new PhysicianInitialAssessmentInfo()));
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

        #region GetOutpatientWaitingTime
        public static List<GetOutpatientWaitingTime> GetOutpatientWaitingTime(int year, int month)
        {
            List<GetOutpatientWaitingTime> result = new List<GetOutpatientWaitingTime>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetOutpatientWaitingTime));
                ctx.CommandText = "GetOutpatientWaitingTime";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("Year", year);
                ctx.Add("Month", month);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetOutpatientWaitingTime)helper.IDataReaderToObject(reader, new GetOutpatientWaitingTime()));
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

        #region GetPrescriptionUDDListByDispense
        public static List<PrescriptionUDDListByDispense> GetPrescriptionUDDListByDispense(string visitID, string displayMode, string isUsingUDD = "0", IDbContext ctx = null)
        {
            List<PrescriptionUDDListByDispense> result = new List<PrescriptionUDDListByDispense>();
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }

            ctx.CommandText = "GetPrescriptionUDDListByDispense";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@VisitID", visitID));
            ctx.Command.Parameters.Add(new SqlParameter("@DisplayMode", displayMode));
            ctx.Command.Parameters.Add(new SqlParameter("@IsUsingUDD", isUsingUDD));
            try
            {
                //Get DataReader
                DbHelper helper = new DbHelper(typeof(PrescriptionUDDListByDispense));
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PrescriptionUDDListByDispense)helper.IDataReaderToObject(reader, new PrescriptionUDDListByDispense()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }
            return result;
        }
        #endregion

        #region GetPatientLaboratoryResult
        public static List<PatientLaboratoryResult> GetPatientLaboratoryResult(String StartDate, String EndDate, Int32 MRN, Int32 FractionID, Int32 RegistrationID)
        {
            List<PatientLaboratoryResult> result = new List<PatientLaboratoryResult>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(PatientLaboratoryResult));
                ctx.CommandText = "GetPatientLaboratoryResult";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("StartDate", StartDate);
                ctx.Add("EndDate", EndDate);
                ctx.Add("MRN", MRN);
                ctx.Add("FractionID", FractionID);
                ctx.Add("RegistrationID", RegistrationID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PatientLaboratoryResult)helper.IDataReaderToObject(reader, new PatientLaboratoryResult()));
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

        #region GetPregancyAge
        public static List<GetPregancyAge> GetPregancyAge(Int32 visitID)
        {
            List<GetPregancyAge> result = new List<GetPregancyAge>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetPregancyAge));
                ctx.CommandText = "GetPregancyAge";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", visitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetPregancyAge)helper.IDataReaderToObject(reader, new GetPregancyAge()));
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
        #region GetAntenatalHistoryList
        public static List<GetAntenatalHistoryList> GetAntenatalHistoryList(Int32 mrn)
        {
            List<GetAntenatalHistoryList> result = new List<GetAntenatalHistoryList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetAntenatalHistoryList));
                ctx.CommandText = "GetAntenatalHistoryList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MRN", mrn);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetAntenatalHistoryList)helper.IDataReaderToObject(reader, new GetAntenatalHistoryList()));
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
        #region GetListItemForPatientObsygnHistoryList
        public static List<GetListItemForPatientObsygnHistoryList> GetListItemForPatientObsygnHistoryList(Int32 visitID)
        {
            List<GetListItemForPatientObsygnHistoryList> result = new List<GetListItemForPatientObsygnHistoryList>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetListItemForPatientObsygnHistoryList));
                ctx.CommandText = "GetListItemForPatientObsygnHistoryList";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", visitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetListItemForPatientObsygnHistoryList)helper.IDataReaderToObject(reader, new GetListItemForPatientObsygnHistoryList()));
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
        #region GetVisitHistoryListByMRN
        public static List<GetVisitHistoryListByMRN> GetVisitHistoryListByMRN(Int32 mrn, Int32 currentVisitID)
        {
            List<GetVisitHistoryListByMRN> result = new List<GetVisitHistoryListByMRN>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetVisitHistoryListByMRN));
                ctx.CommandText = "GetVisitHistoryListByMRN";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("MRN", mrn);
                ctx.Add("CurrentVisitID", currentVisitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((GetVisitHistoryListByMRN)helper.IDataReaderToObject(reader, new GetVisitHistoryListByMRN()));
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

        #region GetPrescriptionOrderPPRAInfo
        public static List<PrescriptionOrderPPRAInfo> GetPrescriptionOrderPPRAInfo(Int32 prescriptionOrderID)
        {
            List<PrescriptionOrderPPRAInfo> result = new List<PrescriptionOrderPPRAInfo>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(GetVisitHistoryListByMRN));
                ctx.CommandText = "GetPrescriptionOrderPPRAInfo";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("PrescriptionOrderID", prescriptionOrderID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((PrescriptionOrderPPRAInfo)helper.IDataReaderToObject(reader, new PrescriptionOrderPPRAInfo()));
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

        #region GenerateARInvoiceReceivingDtPerAlokasiHd
        public static string GenerateARInvoiceReceivingDtPerAlokasiHd(Int32 ARInvoiceReceivingID, IDbContext ctx = null)
        {
            bool IsCtxNull = false;
            if (ctx == null)
            {
                IsCtxNull = true;
                ctx = DbFactory.Configure();
            }
            ctx.CommandText = "GenerateARInvoiceReceivingDtPerAlokasiHd";
            ctx.CommandType = CommandType.StoredProcedure;
            ctx.Command.Parameters.Add(new SqlParameter("@ARInvoiceReceivingID", ARInvoiceReceivingID));

            try
            {
                DaoBase.ExecuteNonQuery(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (IsCtxNull)
                    ctx.Close();
            }

            return "";
        }
        #endregion

        #region GetVisitInfoForRadiotherapyProgram
        public static List<VisitInfoForRadiotherapyProgram> GetVisitInfoForRadiotherapyProgram(Int32 visitID)
        {
            List<VisitInfoForRadiotherapyProgram> result = new List<VisitInfoForRadiotherapyProgram>();
            IDbContext ctx = DbFactory.Configure();
            try
            {
                DbHelper helper = new DbHelper(typeof(VisitInfoForRadiotherapyProgram));
                ctx.CommandText = "GetVisitInfoForRadiotherapyProgram";
                ctx.CommandType = CommandType.StoredProcedure;
                //Add Parameter
                ctx.Add("VisitID", visitID);
                //Get DataReader
                using (IDataReader reader = DaoBase.GetDataReader(ctx))
                    while (reader.Read())
                        result.Add((VisitInfoForRadiotherapyProgram)helper.IDataReaderToObject(reader, new VisitInfoForRadiotherapyProgram()));
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

    }
}