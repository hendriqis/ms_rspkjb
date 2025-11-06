using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class ImportLISResultCtl : BaseEntryPopupCtl
    {
        protected string GCTemplateGroup = "";

        private LaboratoryTestResultDetail DetailPage
        {
            get { return (LaboratoryTestResultDetail)Page; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (lvwView.Items.Count < 1)
                BindGridView();
        }
        public override void InitializeDataControl(string param)
        {
            GCTemplateGroup = Constant.TemplateGroup.LABORATORY;

            string[] par = param.Split('|');

            hdnVisitID.Value = par[0];
            hdnItemID.Value = par[1];
            hdnLabResultID.Value = par[2];
            hdnTransactionID.Value = par[3];

            BindGridView();
            List<LaboratoryResultDt> lstEntityDt = BusinessLayer.GetLaboratoryResultDtList(string.Format("ID = {0}", hdnLabResultID.Value));
            if (lstEntityDt.Count > 0)
            {
                IsAdd = false;
                EntityToControl(lstEntityDt);
            }
        }

        #region Bind Grid
        private void BindGridView()
        {
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            int ageInYear = Function.GetPatientAgeInYear(entity.DateOfBirth, DateTime.Now);
            int ageInMonth = Function.GetPatientAgeInMonth(entity.DateOfBirth, DateTime.Now);
            int ageInDay = Function.GetPatientAgeInDay(entity.DateOfBirth, DateTime.Now);
            int ageInTotalDays = (ageInYear * 365) + (ageInMonth * 30) + ageInDay;

            string itemID = hdnItemID.Value;
            if (itemID == "")
                itemID = Request.Form[hdnItemID.UniqueID];
            string filterExpression = string.Format("ItemID IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = {0} AND IsDeleted = 0) AND IsDeleted = 0", hdnTransactionID.Value);

            List<GetLaboratoryResultFromLIS> lstItemFraction = BusinessLayer.GetLaboratoryResultFromLISList(Convert.ToInt32(hdnTransactionID.Value));

            List<GetLaboratoryResultFromLIS> list = lstItemFraction;
            lvwView.DataSource = list;
            lvwView.DataBind();
        }
        #endregion

        #region Load
        private void EntityToControl(List<LaboratoryResultDt> lstEntityDt)
        {
            foreach (ListViewDataItem item in lvwView.Items)
            {
                CheckBox chkPatient = (CheckBox)item.FindControl("chkPatient");
                CheckBox chkSelectAllPatient = (CheckBox)item.FindControl("chkSelectAllPatient");
                HtmlInputHidden hdnFractionID = (HtmlInputHidden)item.FindControl("keyField");
                HtmlInputHidden hdnIsVerified = (HtmlInputHidden)item.FindControl("hdnIsVerified");
                TextBox txtMetricValue = (TextBox)item.FindControl("txtMetricValue");
                TextBox txtInternationalValue = (TextBox)item.FindControl("txtInternationalValue");
                TextBox txtTextResult = (TextBox)item.FindControl("txtTestResult");
                CheckBox chkIsDeleted = (CheckBox)item.FindControl("chkIsDeleted");
                LaboratoryResultDt entityDt = lstEntityDt.FirstOrDefault(p => p.ID == Convert.ToInt32(hdnLabResultID.Value) && p.FractionID == Convert.ToInt32(hdnFractionID.Value));
                if (entityDt != null)
                {
                    if (entityDt.IsVerified)
                    {
                        chkPatient.Enabled = false;
                        txtInternationalValue.Enabled = false;
                        txtMetricValue.Enabled = false;
                        contentResult.InnerHtml = entityDt.TextValue;
                        chkIsDeleted.Enabled = false;
                    }
                    else
                        chkPatient.Checked = true;
                    txtMetricValue.Text = entityDt.MetricResultValue.ToString("G29");
                    txtInternationalValue.Text = entityDt.InternationalResultValue.ToString("G29");
                    hdnIsVerified.Value = entityDt.IsVerified.ToString();
                }
            }
        }
        #endregion

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            GetLaboratoryResultFromLIS item = e.Item.DataItem as GetLaboratoryResultFromLIS;

            TextBox txtMetricValue = (TextBox)e.Item.FindControl("txtMetricValue");
            TextBox txtInternationalValue = (TextBox)e.Item.FindControl("txtInternationalValue");
            HtmlControl divResultText = (HtmlControl)e.Item.FindControl("divTextResult");
            if (item.GCLabTestResultType == Constant.LaboratoryResultValue.NUMERIC)
            {
            Helper.SetControlEntrySetting(txtMetricValue, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtInternationalValue, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            }
            divResultText.Visible = !item.IsNumericResult;
            txtMetricValue.Text = item.MetricResultValue.ToString("G29");
            txtMetricValue.Visible = item.IsNumericResult;
            txtInternationalValue.Text = (item.MetricResultValue * item.ConversionFactor).ToString("G29");
            txtInternationalValue.Visible = item.IsNumericResult;
        }

        #region Save Entity
        private void ControlToEntity(List<LaboratoryResultDt> lstEntityDt, int flag)
        {
            #region LaboratoryResultDt
            foreach (ListViewDataItem item in lvwView.Items)
            {
                CheckBox chkPatient = (CheckBox)item.FindControl("chkPatient");
                HtmlInputHidden hdnFractionID = (HtmlInputHidden)item.FindControl("keyField");
                HtmlInputHidden hdnDetailItemID = (HtmlInputHidden)item.FindControl("hdnDetailItemID");
                HtmlInputHidden hdnInternationalUnitMax = (HtmlInputHidden)item.FindControl("hdnInternationalUnitMax");
                HtmlInputHidden hdnInternationalUnitMin = (HtmlInputHidden)item.FindControl("hdnInternationalUnitMin");
                HtmlInputHidden hdnMetricUnitMin = (HtmlInputHidden)item.FindControl("hdnMetricUnitMin");
                HtmlInputHidden hdnMetricUnitMax = (HtmlInputHidden)item.FindControl("hdnMetricUnitMax");
                HtmlInputHidden hdnGCMetricUnit = (HtmlInputHidden)item.FindControl("hdnGCMetricUnit");
                HtmlInputHidden hdnGCInternationalUnit = (HtmlInputHidden)item.FindControl("hdnGCInternationalUnit");
                HtmlInputHidden hdnTextResult = (HtmlInputHidden)item.FindControl("hdnTextResult");
                HtmlInputHidden hdnResultFlag = (HtmlInputHidden)item.FindControl("hdnResultFlag");

                if (chkPatient.Checked)
                {
                    TextBox txtMetricValue = (TextBox)item.FindControl("txtMetricValue");
                    TextBox txtInternationalValue = (TextBox)item.FindControl("txtInternationalValue");
                    TextBox txtNormalValueText = (TextBox)item.FindControl("txtNormalValueText");
                    CheckBox chkIsDeleted = (CheckBox)item.FindControl("chkIsDeleted");
                    LaboratoryResultDt entityDt = new LaboratoryResultDt();
                    if (flag == 1)
                        entityDt.ID = Convert.ToInt32(hdnLabResultID.Value);
                    entityDt.ItemID = Convert.ToInt32(hdnDetailItemID.Value);
                    entityDt.FractionID = Convert.ToInt32(hdnFractionID.Value);

                    if (hdnGCMetricUnit.Value == "N/A")
                        entityDt.GCMetricUnit = "";
                    else
                        entityDt.GCMetricUnit = hdnGCMetricUnit.Value;
                    if (hdnGCInternationalUnit.Value == "N/A")
                        entityDt.GCInternationalUnit = "";
                    else
                        entityDt.GCInternationalUnit = hdnGCInternationalUnit.Value;
                    if (hdnInternationalUnitMax.Value == "N/A")
                        entityDt.MaxInternationalNormalValue = 0;
                    else
                        entityDt.MaxInternationalNormalValue = Convert.ToDecimal(hdnInternationalUnitMax.Value);
                    if (hdnInternationalUnitMin.Value == "N/A")
                        entityDt.MinInternationalNormalValue = 0;
                    else
                        entityDt.MinInternationalNormalValue = Convert.ToDecimal(hdnInternationalUnitMin.Value);
                    if (hdnMetricUnitMax.Value == "N/A")
                        entityDt.MaxMetricNormalValue = 0;
                    else
                        entityDt.MaxMetricNormalValue = Convert.ToDecimal(hdnMetricUnitMax.Value);
                    if (hdnMetricUnitMin.Value == "N/A")
                        entityDt.MinMetricNormalValue = 0;
                    else
                        entityDt.MinMetricNormalValue = Convert.ToDecimal(hdnMetricUnitMin.Value);

                    if (txtInternationalValue.Text == "" || txtInternationalValue.Text == null) txtInternationalValue.Text = "0";
                    if (txtMetricValue.Text == "" || txtMetricValue.Text == null) txtMetricValue.Text = "0";
                    entityDt.InternationalResultValue = Convert.ToDecimal(txtInternationalValue.Text);
                    entityDt.MetricResultValue = Convert.ToDecimal(txtMetricValue.Text);
                    entityDt.TextValue = HttpUtility.HtmlDecode(hdnTextResult.Value);
                    entityDt.ResultFlag = hdnResultFlag.Value;
                    if (entityDt.ResultFlag == "N")
                        entityDt.IsNormal = true;
                    else
                        entityDt.IsNormal = false;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
     
                     lstEntityDt.Add(entityDt);
                }
            }
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LaboratoryResultDtDao entityDtDao = new LaboratoryResultDtDao(ctx);
            PatientChargesDtDao entityPatientDtDao = new PatientChargesDtDao(ctx);
            int labResultID = Convert.ToInt32(hdnLabResultID.Value);
            try
            {
                DetailPage.SaveLaboratoryResultHd(ctx, ref labResultID);
                List<LaboratoryResultDt> lstEntityDt = new List<LaboratoryResultDt>();
                ControlToEntity(lstEntityDt, 0);

                foreach (LaboratoryResultDt entityDt in lstEntityDt)
                {
                    entityDt.ID = labResultID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    PatientChargesDt entityPatientDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND ItemID = {1}", hdnTransactionID.Value, entityDt.ItemID), ctx)[0];
                    entityDt.ReferenceDtID = entityPatientDt.ReferenceDtID;
                    entityPatientDt.IsHasTestResult = true;
                    entityPatientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientDtDao.Update(entityPatientDt);
                }
                retval = labResultID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LaboratoryResultDtDao entityDtDao = new LaboratoryResultDtDao(ctx);
            PatientChargesDtDao entityPatientDtDao = new PatientChargesDtDao(ctx);
            try
            {
                List<LaboratoryResultDt> lstEntityDt = BusinessLayer.GetLaboratoryResultDtList(string.Format("ID = {0}", hdnLabResultID.Value), ctx);
                List<LaboratoryResultDt> lstNewEntityDt = new List<LaboratoryResultDt>();

                ControlToEntity(lstNewEntityDt, 1);
                foreach (LaboratoryResultDt entityDt in lstNewEntityDt)
                {
                    //HtmlInputHidden hdnFractionID = (HtmlInputHidden)item.FindControl("keyField");
                    LaboratoryResultDt obj = lstEntityDt.FirstOrDefault(p => p.ID == entityDt.ID && p.FractionID == entityDt.FractionID);
                    if (obj == null)
                    {
                        entityDtDao.Insert(entityDt);
                    }
                    else
                    {
                        if (!entityDt.IsDeleted)
                            entityDtDao.Update(entityDt);
                        else
                            entityDtDao.Delete(entityDt.ID,entityDt.ItemID,entityDt.FractionID);
                    }
                }

                PatientChargesDt entityPatientDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND ItemID = {1}", hdnTransactionID.Value, hdnItemID.Value))[0];
                entityPatientDt.IsHasTestResult = true;
                entityPatientDtDao.Update(entityPatientDt);

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
        #endregion
    }
}