using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransferPatientBillDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.TRANSFER_PATIENT_BILL;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();

                string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}')", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
                int count = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
                if (count < 1)
                {
                    filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}')", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                    count = BusinessLayer.GetPatientBillRowCount(filterExpression);
                }
                if (count < 1)
                    tblInfoOutstandingBill.Style.Add("display", "none");
                hdnOutstandingCount.Value = count.ToString();

                hdnDepartmentID.Value = entity.DepartmentID;
                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);

                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", entity.LinkedRegistrationID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID));
                BindGrid(lst);

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }   
        }

        

        private void BindGrid(List<vPatientChargesDt8> lst)
        {            
            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);
            ((TransactionDtServiceViewCtl)ctlService).HideCheckBox();

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);
            ((TransactionDtProductViewCtl)ctlDrugMS).HideCheckBox();

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);
            ((TransactionDtProductViewCtl)ctlDrugMS).HideCheckBox();

            List<vPatientChargesDt8> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);
            ((TransactionDtServiceViewCtl)ctlLaboratory).HideCheckBox();

            List<vPatientChargesDt8> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);
            ((TransactionDtServiceViewCtl)ctlImaging).HideCheckBox();

            List<vPatientChargesDt8> lstMedicalDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstMedicalDiagnostic);
            ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).HideCheckBox();

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N2");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N2");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N2");
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            //int count = Convert.ToInt32(hdnOutstandingCount.Value);
            //if (count < 1)
            //{
            //    if (OnProcessRecord(ref errMessage))
            //    {
            //        result += "success";
            //    }
            //    else
            //        result += string.Format("fail|{0}", errMessage);
            //}
            //else
            //    result = "fail|Masih Ada Bill Yang Belum Lunas / Order Yang Belum Direalisasi. Tagihan Tidak Bisa Ditransfer";

            if (OnProcessRecord(ref errMessage))
                result += "success";
            else
                result += string.Format("fail|{0}", errMessage);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao consVisitDao = new ConsultVisitDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            try
            {
                Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

                decimal patientAmount = Convert.ToDecimal(Request.Form[txtTotalPatient.UniqueID]);
                decimal payerAmount = Convert.ToDecimal(Request.Form[txtTotalPayer.UniqueID]);
                decimal tariff = Convert.ToDecimal(Request.Form[txtTotal.UniqueID]);
                
                string filterExpression = string.Format("VisitID IN (SELECT VisitID FROM vConsultVisit WHERE RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}'))", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
                List<TestOrderHd> lstTestOrderHd = BusinessLayer.GetTestOrderHdList(filterExpression, ctx);
                foreach (TestOrderHd testOrderHd in lstTestOrderHd)
                {
                    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    testOrderHd.VoidBy = AppSession.UserLogin.UserID;
                    testOrderHd.VoidDate = DateTime.Now;
                    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    testOrderHdDao.Update(testOrderHd);
                }

                filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterExpression, ctx);
                foreach (PatientBill patientBill in lstPatientBill)
                {
                    patientBill.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientBill.LastUpdatedDate = DateTime.Now;
                    patientBillDao.Update(patientBill);
                }

                Registration entityLinked = registrationDao.Get((int)entity.LinkedRegistrationID);
                entityLinked.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                entityLinked.IsChargesTransfered = true;
                entityLinked.LastUpdatedBy = AppSession.UserLogin.UserID;
                registrationDao.Update(entityLinked);   
                
                string filter = string.Format("RegistrationID = {0}", entityLinked.RegistrationID);
                ConsultVisit entConsult = BusinessLayer.GetConsultVisitList(filter, ctx)[0];
                entConsult.GCVisitStatus = Constant.VisitStatus.CLOSED;
                entConsult.LastUpdatedBy = AppSession.UserLogin.UserID;
                consVisitDao.Update(entConsult);

                filter = string.Format("TransactionID IN (SELECT TransactionID FROM vPatientChargesHd WHERE RegistrationID = {0}) AND GCTransactionStatus NOT IN ('{1}', '{2}')", entity.LinkedRegistrationID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(filter, ctx);
                foreach (PatientChargesHd enPch in lstPatientChargesHd)
                {
                    enPch.IsChargesTransfered = true;
                    enPch.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientChargesHdDao.Update(enPch);
                }

                ctx.CommitTransaction();
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
    }
}