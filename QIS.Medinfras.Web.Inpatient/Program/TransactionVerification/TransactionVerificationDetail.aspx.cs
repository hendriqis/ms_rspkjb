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
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class TransactionVerificationDetail : BasePageTrx
    {
        String[] lstSelectedMember, lstUnselectedMember = null;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.TRANSACTION_VERIFICATION;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string cboDisplayValue = Request.Form[hdnCboDisplayValue.UniqueID] == null ? "" : Request.Form[hdnCboDisplayValue.UniqueID];
            /*if (cboDisplayValue != "")
            {
                BindGrid(cboDisplayValue, pageIndex);
            }*/
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                String visitID = Page.Request.QueryString["id"];
                hdnVisitID.Value = visitID;
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                ctlPatientBanner.InitializePatientBanner(entity);
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();

                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diverifikasi" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diverifikasi" });
                Methods.SetComboBoxField<Variable>(cboDisplay, lstVariable, "Value", "Code");
                cboDisplay.Value = "1";

                hdnDepartmentID.Value = Constant.Facility.INPATIENT;
                if (OnGetRowCount() > 0)
                    IsLoadFirstRecord = true;
                else
                {
                    IsLoadFirstRecord = false;
                    BindGrid();
                }

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }
        }

        #region Load Entity
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrid();
        }

        protected string OnGetTransactionNoFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND GCTransactionStatus IN ('{1}')", hdnVisitID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = OnGetTransactionNoFilterExpression();
            return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetTransactionNoFilterExpression();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetTransactionNoFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity);
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            txtTransactionNo.Text = entity.TransactionNo;
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            BindGrid();
        }

        private void BindGrid()
        {
            string filterExpression = "1 = 0";

            if (hdnTransactionHdID.Value != "" && hdnTransactionHdID.Value != "0")
            {
                string cboDisplayValue = cboDisplay.Value.ToString();
                filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value);
                if (cboDisplayValue == "1")
                    filterExpression += " AND IsVerified = 0";
                else if (cboDisplayValue == "2")
                    filterExpression += " AND IsVerified = 1";
            }


            List<vPatientChargesDt> lstEntityPatientDt = BusinessLayer.GetvPatientChargesDtList(filterExpression);

            List<vPatientChargesDt> lstService = lstEntityPatientDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            ctlService.BindGrid(lstService);

            List<vPatientChargesDt> lstDrugMS = lstEntityPatientDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ctlDrug.BindGrid(lstDrugMS);

            List<vPatientChargesDt> lstLogistic = lstEntityPatientDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ctlLogistic.BindGrid(lstLogistic);

            List<vPatientChargesDt> lstLaboratory = lstEntityPatientDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ctlLaboratory.BindGrid(lstLaboratory);

            List<vPatientChargesDt> lstImaging = lstEntityPatientDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ctlImaging.BindGrid(lstImaging);

            List<vPatientChargesDt> lstMedDiagnostic = lstEntityPatientDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ctlMedical.BindGrid(lstMedDiagnostic);

            txtTotalPayer.Text = lstEntityPatientDt.Sum(p => p.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPatient.Text = lstEntityPatientDt.Sum(p => p.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            //txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "process")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                lstSelectedMember = hdnSelectedMember.Value.Substring(0).Split(',');
                lstUnselectedMember = hdnUnselectedMember.Value.Substring(0).Split(',');

                try
                {
                    ctlService.VerifyProcess(ctx, lstSelectedMember, lstUnselectedMember, hdnTransactionHdID.Value);
                   
                    ctx.CommitTransaction();
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
            else
            {
                try
                {
                    List<PatientChargesHd> lstTransactionSelected = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND GCTransactionStatus = '{1}' AND TransactionID = {2}", Page.Request.QueryString["id"], Constant.TransactionStatus.WAIT_FOR_APPROVAL, hdnTransactionHdID.Value));
                    foreach(PatientChargesHd entity in lstTransactionSelected)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientChargesHd(entity);
                    }
                    
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }

}