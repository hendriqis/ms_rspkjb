using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OperatingRoomOrder : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected int PageCountDt1 = 1;
        protected int PageCountDt2 = 1;
        protected int PageCountDt3 = 1;
        string menuType = string.Empty;


        public override string OnGetMenuCode()
        {
            string menuCode = string.Empty;
            if (string.IsNullOrEmpty(menuType))
            {
                switch (hdnRegisterDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_TEST_ORDER_OK;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_TEST_ORDER_OK;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_TEST_ORDER_OK;
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_TEST_ORDER_OK;
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_TEST_ORDER_OK;
                    default: return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_TEST_ORDER_OK;
                }
            }
            else
            {
                switch (menuType)
                {
                    case "md": menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_DIAGNOSTIC_TEST_ORDER_OK; break;
                    case "er": menuCode = Constant.MenuCode.EmergencyCare.PATIENT_DIAGNOSTIC_TEST_ORDER_OK; break;
                    default:
                        break;
                }
            }
            return menuCode;
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
            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE));
            hdnOperatingRoomID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).ParameterValue;

            hdnRegisterDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnRegisterRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnRegisterVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegisterParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            BindGridView(1, true, ref PageCount);
            //BindGridViewDt1(1, true, ref PageCountDt1);
            //BindGridViewDt2(1, true, ref PageCountDt2);
            this.EnableViewState = true;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/DetailEntry/OperatingRoomOrderEntryCtl1.ascx");
            queryString = string.Format("{0}|{1}|{2}|{3}", "0", hdnRegisterRegistrationID.Value, hdnRegisterVisitID.Value, hdnRegisterParamedicID.Value);
            popupWidth = 900;
            popupHeight = 600;
            popupHeaderText = "Order Jadwal Kamar Operasi";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            bool result = true;
            if (hdnID.Value != "")
            {
                Registration oRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegisterRegistrationID.Value));
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
                        url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/DetailEntry/OperatingRoomOrderEntryCtl1.ascx");
                        queryString = string.Format("{0}|{1}|{2}|{3}", hdnID.Value, hdnRegisterRegistrationID.Value, hdnRegisterVisitID.Value, hdnRegisterParamedicID.Value);
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
                        url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/DetailEntry/OperatingRoomOrderEntryCtl1.ascx");
                        queryString = string.Format("{0}|{1}|{2}|{3}", hdnID.Value, hdnRegisterRegistrationID.Value, hdnRegisterVisitID.Value, hdnRegisterParamedicID.Value);
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

        #region Test Order Hd
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            hdnIsOutstandingOrder.Value = "0";
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCOrderStatus NOT IN ('{2}') AND GCTransactionStatus NOT IN ('{3}')", AppSession.RegisteredPatient.VisitID, hdnOperatingRoomID.Value, Constant.OrderStatus.CANCELLED, Constant.TransactionStatus.VOID);

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
        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreProcedureChecklistRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreProcedureChecklist> lstEntity = BusinessLayer.GetvPreProcedureChecklistList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ChecklistDate DESC, ChecklistTime DESC");
            grdViewDt3.DataSource = lstEntity;
            grdViewDt3.DataBind();
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
        protected void cbpViewDt3_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt3(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt3(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityOrderHdDao = new TestOrderHdDao(ctx);

            try
            {
                TestOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnID.Value));

                if (type == "propose" || type == "proposeNextVisit")
                {
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
        #endregion

        protected void cbpDeleteChecklist_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteChecklist(param);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string DeleteChecklist(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            PreProcedureChecklistDao recordDao = new PreProcedureChecklistDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PreProcedureChecklist obj = recordDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    recordDao.Update(obj);

                    result = string.Format("1|{0}", string.Empty);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
    }
}