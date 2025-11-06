using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class SurgeryOrderList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected int PageCountDt1 = 1;
        protected int PageCountDt2 = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.ORDER_JADWAL_KAMAR_OPERASI;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override bool IsEntryUsePopup()
        {
            return true;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE));
            hdnOperatingRoomID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).ParameterValue;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    hdnLinkedRegistrationVisitID.Value = entityLinkedRegistration.VisitID.ToString();
                }
            }

            BindGridView(1, true, ref PageCount);
            BindGridViewDt1(1, true, ref PageCountDt1);
            BindGridViewDt2(1, true, ref PageCountDt2);
            this.EnableViewState = true;
        }

        #region Test Order Hd
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            int cvLinkedID = AppSession.RegisteredPatient.VisitID;

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (!string.IsNullOrEmpty(hdnLinkedRegistrationVisitID.Value) && hdnLinkedRegistrationVisitID.Value != "0")
                {
                    cvLinkedID = Convert.ToInt32(hdnLinkedRegistrationVisitID.Value);
                }
            }

            hdnIsOutstandingOrder.Value = "0";
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{4}) AND HealthcareServiceUnitID = {1} AND GCOrderStatus NOT IN ('{2}') AND GCTransactionStatus NOT IN ('{3}')", hdnVisitID.Value, hdnOperatingRoomID.Value, Constant.OrderStatus.CANCELLED, Constant.TransactionStatus.VOID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryTestOrderHd2RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryTestOrderHd2> lstEntity = BusinessLayer.GetvSurgeryTestOrderHd2List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Test Order Dt
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderDtProcedureGroupRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderDtProcedureGroup> lstEntity = BusinessLayer.GetvTestOrderDtProcedureGroupList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ProcedureGroupCode");

            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }
        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderDtParamedicTeamRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderDtParamedicTeam> lstEntity = BusinessLayer.GetvTestOrderDtParamedicTeamList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdViewDt2.DataSource = lstEntity;
            grdViewDt2.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        protected void cbpViewDt2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt2(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt2(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCOrderStatus != Constant.OrderStatus.OPEN)
                {
                    errMessage = "Order Jadwal Kamar Operasi sudah dikirim atau dijadwalkan.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCOrderStatus != Constant.OrderStatus.OPEN)
                {
                    errMessage = "Order Jadwal Kamar Operasi sudah dikirim atau dijadwalkan.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Program/PatientPage/Planning/Surgery/SurgeryOrderEntryCtl1.ascx");
            queryString = "";
            popupWidth = 900;
            popupHeight = 600;
            popupHeaderText = "Order Jadwal Kamar Operasi";
            return true;

            //bool result = true;
            //Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            //if (oRegistration != null)
            //{
            //    if (oRegistration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
            //    {
            //        errMessage = "Nomor Registrasi Pasien ini sudah dalam status TUTUP untuk dilakukan pengentrian transaksi.";
            //        result = false;
            //    }

            //    if (oRegistration.IsLockDown != true)
            //    {
            //        url = ResolveUrl("~/Program/PatientPage/Planning/Surgery/SurgeryOrderEntry.aspx");
            //        result = true;
            //    }
            //    else
            //    {
            //        errMessage = "Saat ini nomor registrasi pasien ini sedang dalam proses penguncian transaksi untuk Pemrosesan Tagihan.";
            //        result = false;
            //    }
            //}
            //return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            bool result = true;
            if (hdnID.Value != "")
            {
                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                TestOrderHd oTestOrderHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnID.Value));
                if (oRegistration != null)
                {
                    if (oRegistration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
                    {
                        errMessage = "Nomor Registrasi Pasien ini sudah dalam status TUTUP untuk dilakukan pengentrian transaksi.";
                        result = false;
                    }

                    if (oRegistration.IsLockDown != true)
                    {
                        url = ResolveUrl("~/Program/PatientPage/Planning/Surgery/SurgeryOrderEntryCtl1.ascx");
                        queryString = hdnID.Value;
                        popupWidth = 900;
                        popupHeight = 600;
                        popupHeaderText = "Order Jadwal Kamar Operasi";
                        result = true;
                    }
                    else
                    {
                        errMessage = "Saat ini nomor registrasi pasien ini sedang dalam proses penguncian transaksi untuk Pemrosesan Tagihan.";
                        result = false;
                    }

                    if (oTestOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        url = ResolveUrl("~/Program/PatientPage/Planning/Surgery/SurgeryOrderEntryCtl1.ascx");
                        queryString = hdnID.Value;
                        popupWidth = 900;
                        popupHeight = 600;
                        popupHeaderText = "Order Jadwal Kamar Operasi";
                        result = true;
                    }
                    else if (oTestOrderHd.GCTransactionStatus == Constant.TransactionStatus.VOID)
                    {
                        errMessage = "Nomor Order ini sudah dibatalkan.";
                        result = false;
                    }
                    else
                    {
                        errMessage = "Nomor Order ini sedang dalam proses.";
                        result = false;
                    }
                }
            }
            return result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            PatientSurgeryDao entityPsDao = new PatientSurgeryDao(ctx);
            try
            {
                if (hdnID.Value != "")
                {
                    TestOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnID.Value));
                    List<PatientSurgery> lstPatientSurgery = BusinessLayer.GetPatientSurgeryList(string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, entityHd.TestOrderID), ctx);
                    if (lstPatientSurgery.Count > 0)
                    {
                        errMessage = "Nomor Order ini tidak dapat di hapus karena sudah ada Laporan Operasi.";
                        result = false;
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entityHd.VoidBy = AppSession.UserLogin.UserID;
                            entityHd.VoidDate = DateTime.Now;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entityHd);

                            List<TestOrderDt> lstEntityDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entityHd.TestOrderID), ctx);
                            foreach (TestOrderDt entityDt in lstEntityDt)
                            {
                                entityDt.IsDeleted = true;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }
                        }
                        else
                        {
                            errMessage = "Nomor Order ini tidak dapat di hapus karena sudah dilakukan send order";
                            result = false;
                        }
                    }
                }
                else
                {
                    errMessage = "Nomor Order tidak ditemukan.";
                    result = false;
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityOrderHdDao = new TestOrderHdDao(ctx);

            try
            {
                if (type == "propose" || type == "proposeNextVisit")
                {
                    TestOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        if (entity.GCToBePerformed == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                        }
                        else
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.GCOrderStatus = Constant.OrderStatus.OPEN;
                        }
                        entity.SendOrderBy = AppSession.UserLogin.UserID;
                        entity.SendOrderDateTime = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityOrderHdDao.Update(entity);

                        hdnIsOutstandingOrder.Value = "0";
                        result = true;
                    }
                    else
                    {
                        errMessage = "Status order sudah tidak open lagi sehingga proses tidak bisa dilanjutkan, mohon refresh halaman ini";
                        result = false;
                    }
                }
                else if (type == "ReOpen")
                {
                    TestOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnID.Value));

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityOrderHdDao.Update(entity);

                        hdnIsOutstandingOrder.Value = "1";
                        result = true;
                    }
                    else
                    {
                        errMessage = "Status Order sudah berubah, haraf refresh halaman ini";
                        result = false;
                    }
                }

                if (result)
                    ctx.CommitTransaction();
                else
                    ctx.RollBackTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void SendNotification(TestOrderHd order, string ipAddress)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("MRN : {0}", AppSession.RegisteredPatient.MedicalNo));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("Tx  :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), 6000);
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vSurgeryTestOrderHd2 entity = e.Row.DataItem as vSurgeryTestOrderHd2;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    hdnIsOutstandingOrder.Value = "1";
                }
            }
        }
    }
}