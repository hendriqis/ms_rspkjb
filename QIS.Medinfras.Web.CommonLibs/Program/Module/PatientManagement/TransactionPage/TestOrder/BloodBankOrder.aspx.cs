using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BloodBankOrderForm : BasePagePatientPageList
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
                    case Constant.Facility.INPATIENT: menuCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_BLOOD_BANK_ORDER; break;
                    case Constant.Facility.EMERGENCY: menuCode = Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_BLOOD_BANK_ORDER; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (hdnRegisterHSUIsLaboratoryUnit.Value == "1")
                            menuCode = Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_BLOOD_BANK_ORDER;
                        else menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_BLOOD_BANK_ORDER; break;
                        
                    default: menuCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_BLOOD_BANK_ORDER; break;
                }
            }
            else
            {
                switch (menuType)
                {
                    case "md": menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_DIAGNOSTIC_BLOOD_BANK_ORDER; break;
                    case "er": menuCode = Constant.MenuCode.EmergencyCare.PATIENT_DIAGNOSTIC_BLOOD_BANK_ORDER; break;
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
            hdnBloodBankUnitID.Value = AppSession.MD0018;

            hdnRegisterDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnRegisterHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            hdnRegisterRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnRegisterVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegisterParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            string filterHSU = string.Format("HealthcareServiceUnitID = {0}", hdnRegisterHealthcareServiceUnitID.Value);
            vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterHSU).FirstOrDefault();
            if (hsu != null)
            {
                hdnRegisterHSUIsLaboratoryUnit.Value = hsu.IsLaboratoryUnit ? "1" : "0";
            }

            BindGridView(1, true, ref PageCount);
            this.EnableViewState = true;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/DetailEntry/BloodBankOrderEntryCtl1.ascx");
            queryString = string.Format("{0}|{1}|{2}|{3}|{4}", "0", hdnRegisterRegistrationID.Value, hdnRegisterVisitID.Value, hdnRegisterParamedicID.Value, hdnBloodBankUnitID.Value);
            popupWidth = 900;
            popupHeight = 600;
            popupHeaderText = "Order Bank Darah";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            bool result = true;
            if (hdnTestOrderIDMP.Value != "")
            {
                Registration oRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegisterRegistrationID.Value));
                if (oRegistration != null)
                {
                    if (oRegistration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
                    {
                        errMessage = "Nomor Registrasi Pasien ini sudah dalam status TUTUP untuk dilakukan pengentrian transaksi.";
                        result = false;
                    }

                    if (oRegistration.IsLockDown != true)
                    {
                        if (hdnTransactionID.Value == "0"  || hdnTransactionID.Value == "")
                        {
                            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/DetailEntry/BloodBankOrderEntryCtl1.ascx");
                            queryString = string.Format("{0}|{1}|{2}|{3}|{4}", hdnTestOrderIDMP.Value, hdnRegisterRegistrationID.Value, hdnRegisterVisitID.Value, hdnRegisterParamedicID.Value, hdnBloodBankUnitID.Value);
                            popupWidth = 900;
                            popupHeight = 600;
                            popupHeaderText = "Order Bank Darah";
                            result = true;
                        }
                        else
                        {
                            errMessage = "Order sudah dikonfirmasi dan sudah memiliki nomor transaksi pasien " + hdnTransactionNo.Value;
                            result = false;
                        }
                    }
                    else
                    {
                        errMessage = "Saat ini nomor registrasi pasien ini sedang dalam proses penguncian transaksi untuk Pemrosesan Tagihan.";
                        result = false;
                    }
                }
            }
            return result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnTestOrderIDMP.Value != "")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
                TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
                try
                {
                    if (hdnTransactionID.Value == "0" || hdnTransactionID.Value == "")
                    {
                        TestOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTestOrderIDMP.Value));
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHd.VoidBy = AppSession.UserLogin.UserID;
                        entityHd.VoidDate = DateTime.Now;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entityHd);

                        List<TestOrderDt> lstDetail = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}",Convert.ToInt32(hdnTestOrderIDMP.Value)),ctx);
                        if (lstDetail.Count>0)
                        {
                            foreach (TestOrderDt item in lstDetail)
                            {
                                TestOrderDt orderDt = BusinessLayer.GetTestOrderDt(Convert.ToInt32(item.ID));
                                orderDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                orderDt.VoidReason = "Header Permintaan divoid";
                                orderDt.IsDeleted = true;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderDt.LastUpdatedDate = DateTime.Now;
                                entityDtDao.Update(orderDt);
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Order sudah dikonfirmasi dan sudah memiliki nomor transaksi pasien " + hdnTransactionNo.Value;
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }

        #region Test Order Hd
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            hdnIsOutstandingOrder.Value = "0";
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1}')", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID);

            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvBloodBankOrderRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            //}

            pageCount = 1;

            List<vBloodBankOrder1> lstEntity = BusinessLayer.GetvBloodBankOrder1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderDate DESC, TestOrderNo DESC");
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
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    vSurgeryTestOrderHd2 entity = e.Row.DataItem as vSurgeryTestOrderHd2;
            //    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            //    {
            //        hdnIsOutstandingOrder.Value = "1";
            //    }
            //}
        }
        #endregion

        #region Test Order Dt
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnTestOrderIDMP.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderDtBloodBank1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderDtBloodBank1> lstEntity = BusinessLayer.GetvTestOrderDtBloodBank1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemCode");

            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnTestOrderIDMP.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAssessment> lstEntity = BusinessLayer.GetvPatientAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentDate DESC, AssessmentTime DESC");
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
                if (type == "propose")
                {
                    TestOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnTestOrderIDMP.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        bool isValidToSendOrder = true;

                        isValidToSendOrder = IsValidToSendOrder(entity, ref errMessage);
                        if (isValidToSendOrder)
                        {
                            DateTime sendOrderDateTime = DateTime.Now;

                            TestOrderHd entityHd = entityOrderHdDao.Get(Convert.ToInt32(entity.TestOrderID));
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entityHd.SendOrderBy = AppSession.UserLogin.UserID;
                            entityHd.SendOrderDateTime = sendOrderDateTime;
                            entityOrderHdDao.Update(entityHd);

                            hdnIsOutstandingOrder.Value = "0";
                            result = true;
                        }
                        else
                        {
                            hdnIsOutstandingOrder.Value = "0";
                            result = false;
                        }
                    }
                    else
                    {
                        errMessage = "Status order sudah tidak open lagi sehingga proses tidak bisa dilanjutkan, mohon refresh halaman ini";
                        result = false;
                    }
                }
                else if (type == "ReOpen")
                {
                    TestOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnTestOrderIDMP.Value));

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
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

        private bool IsValidToSendOrder(TestOrderHd entity, ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            if (string.IsNullOrEmpty(entity.GCSourceType))
            {
                message.AppendLine("Sumber/Asal Darah harus dipilih|");
            }

            if (string.IsNullOrEmpty(entity.GCUsageType))
            {
                message.AppendLine("Cara Penyimpanan harus dipilih|");
            }

            if (entity.GCSourceType == "X533^001") //PMI
            {
                if (string.IsNullOrEmpty(entity.GCPaymentType))
                {
                    message.AppendLine("Cara Pembayaran di PMI harus dipilih|");
                }
            }

            if (string.IsNullOrEmpty(entity.Remarks))
            {
                message.AppendLine("Catatan Klinis / Diagnosa harus diisi|");
            }



            if (string.IsNullOrEmpty(entity.GCBloodType))
            {
                message.AppendLine("Jenis Golongan Darah harus diisi|");
            }

            if (string.IsNullOrEmpty(entity.BloodRhesus))
            {
                message.AppendLine("Rhesus Golongan Darah harus diisi|");
            }



            errMessage = message.ToString().Replace(@"|", "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        #endregion

        protected void cbpDeleteMonitoring_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteMonitoring(param);
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

        private string DeleteMonitoring(string recordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientAssessmentDao recordDao = new PatientAssessmentDao(ctx);
            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientAssessment obj = recordDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    recordDao.Update(obj);

                    ctx.CommitTransaction();

                    result = string.Format("1|{0}", string.Empty);
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
    }
}