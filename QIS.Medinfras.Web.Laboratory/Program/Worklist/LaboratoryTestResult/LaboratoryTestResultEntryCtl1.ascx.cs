using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryTestResultEntryCtl1 : BaseEntryPopupCtl
    {
        protected string GCTemplateGroup = "";

        private LaboratoryTestResultDetail1 DetailPage
        {
            get { return (LaboratoryTestResultDetail1)Page; }
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
            hdnParamedicIDResultDt.Value = par[4];

            BindGridView();

            PatientChargesHd entityHD = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));
            if (entityHD != null && entityHD.GCTransactionStatus != Constant.TransactionStatus.VOID)
            {
                String filterExpression = String.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", entityHD.TransactionID, Constant.TransactionStatus.VOID);
                List<PatientChargesDt> lstEntityDtCharges = BusinessLayer.GetPatientChargesDtList(filterExpression);

                List<LaboratoryResultDt> lstEntityDt = BusinessLayer.GetLaboratoryResultDtList(string.Format("ID = {0}", hdnLabResultID.Value));
                List<LaboratoryResultDt> lstEntityDtValid = new List<LaboratoryResultDt>();

                foreach (LaboratoryResultDt e in lstEntityDt)
                {
                    PatientChargesDt en = lstEntityDtCharges.Where(t => t.ItemID == e.ItemID).FirstOrDefault();
                    if (en != null)
                    {
                        lstEntityDtValid.Add(e);
                    }
                }

                if (lstEntityDtValid.Count > 0)
                {
                    IsAdd = false;
                    EntityToControl(lstEntityDtValid);
                }
            }
        }

        #region Bind Grid
        private void BindGridView()
        {
            vConsultVisit1 entity = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            int ageInYear = Function.GetPatientAgeInYear(entity.DateOfBirth, DateTime.Now);
            int ageInMonth = Function.GetPatientAgeInMonth(entity.DateOfBirth, DateTime.Now);
            int ageInDay = Function.GetPatientAgeInDay(entity.DateOfBirth, DateTime.Now);
            int ageInTotalDays = (ageInYear * 365) + (ageInMonth * 30) + ageInDay;

            string itemID = hdnItemID.Value;
            if (itemID == "")
                itemID = Request.Form[hdnItemID.UniqueID];
            string filterExpression = string.Format("(RecursiveID = {0} OR ItemID = {0})", itemID);

            string filterExpressionFraction = filterExpression;
            filterExpressionFraction += string.Format(" ORDER BY DisplayOrder");
            List<vRecursiveItemLaboratoryFraction> lstItemFraction = BusinessLayer.GetvRecursiveItemLaboratoryFractionList(filterExpressionFraction);
            foreach (var item in lstItemFraction)
            {
                string filterExpression2 = filterExpression;
                filterExpression2 += string.Format(
                    " AND FractionID = {0} AND (GCSex = '{1}' OR GCSex IS NULL) AND {2} BETWEEN FromAgeInDay AND ToAgeInDay AND IsDeleted = 0",
                    item.FractionID, entity.GCGender, ageInTotalDays);
                vItemFraction fractionInfo = BusinessLayer.GetvItemFractionList(filterExpression2).FirstOrDefault();
                LaboratoryResultDt labDt = BusinessLayer.GetLaboratoryResultDtList(string.Format(
                    "ID = {0} AND ItemID = {1} AND FractionID = {2} AND IsDeleted = 0",
                    hdnLabResultID.Value, itemID, item.FractionID)).FirstOrDefault();

                item.IsMetricNormalValueExists = fractionInfo != null;
                item.IsInternationalNormalValueExists = fractionInfo != null;
                UpdateItemFractionInfo(item, fractionInfo, labDt);
            }

            List<vRecursiveItemLaboratoryFraction> list = lstItemFraction;
            lvwView.DataSource = list;
            lvwView.DataBind();
        }

        private void UpdateItemFractionInfo(vRecursiveItemLaboratoryFraction item, vItemFraction fractionInfo, LaboratoryResultDt labDt)
        {
            item.MetricUnitMin = fractionInfo != null ? fractionInfo.MetricUnitMin.ToString("G29") : item.NotApplicableMetricNormalValueLabel;
            item.MetricUnitMax = fractionInfo != null ? fractionInfo.MetricUnitMax.ToString("G29") : item.NotApplicableMetricNormalValueLabel;
            item.MetricUnit = fractionInfo != null ? fractionInfo.MetricUnit : item.NotApplicableMetricNormalValueLabel;
            item.GCMetricUnit = fractionInfo != null ? fractionInfo.GCMetricUnit : item.NotApplicableMetricNormalValueLabel;
            item.InternationalUnitMin = fractionInfo != null ? fractionInfo.InternationalUnitMin.ToString("G29") : item.NotApplicableInternationalNormalValueLabel;
            item.InternationalUnitMax = fractionInfo != null ? fractionInfo.InternationalUnitMax.ToString("G29") : item.NotApplicableInternationalNormalValueLabel;
            item.InternationalUnit = fractionInfo != null ? fractionInfo.MetricUnit : item.NotApplicableInternationalNormalValueLabel;
            item.GCInternationalUnit = fractionInfo != null ? fractionInfo.GCInternationalUnit : item.NotApplicableInternationalNormalValueLabel;

            //item.MetricRangeLabel = fractionInfo != null ? fractionInfo.MetricUnitLabel : item.NotApplicableMetricNormalValueLabel;
            //item.InternationalRangeLabel = fractionInfo != null ? fractionInfo.InternationalUnitLabel : item.NotApplicableInternationalNormalValueLabel;

            if (labDt != null) 
            {
                item.MetricRangeLabel = String.Format("{0} - {1}", labDt.MinMetricNormalValue.ToString("G29"), labDt.MaxMetricNormalValue.ToString("G29"));
                item.InternationalRangeLabel = String.Format("{0} - {1}", labDt.MinInternationalNormalValue.ToString("G29"), labDt.MaxInternationalNormalValue.ToString("G29"));
            }
            else if (fractionInfo != null && labDt == null)
            {
                item.MetricRangeLabel = fractionInfo.MetricUnitLabel;
                item.InternationalRangeLabel = fractionInfo.InternationalUnitLabel;
            }
            else 
            {
                item.MetricRangeLabel = item.NotApplicableMetricNormalValueLabel;
                item.InternationalRangeLabel = item.NotApplicableInternationalNormalValueLabel;
            }
            
            item.PanicMetricRangeLabel = fractionInfo != null ? fractionInfo.PanicMetricUnitLabel : item.NotApplicableMetricNormalValueLabel;
            item.PanicInternationalRangeLabel = fractionInfo != null ? fractionInfo.PanicInternationalUnitLabel : item.NotApplicableInternationalNormalValueLabel;
            item.TextValue = labDt == null ? "" : labDt.TextValue;
            item.isNormal = labDt == null ? false : labDt.IsNormal;
            item.isVerified = labDt == null ? false : labDt.IsVerified;
            item.cfIsAllowDelete = labDt != null;
            item.Remarks = labDt == null ? string.Empty : labDt.Remarks;
            
        }
        #endregion

        #region Load
        private void EntityToControl(List<LaboratoryResultDt> lstEntityDt)
        {
            foreach (ListViewDataItem item in lvwView.Items)
            {
                CheckBox chkResultItem = (CheckBox)item.FindControl("chkResultItem");
                CheckBox chkSelectAllPatient = (CheckBox)item.FindControl("chkSelectAllPatient");
                HtmlInputHidden hdnFractionID = (HtmlInputHidden)item.FindControl("keyField");
                HtmlInputHidden hdnIsVerified = (HtmlInputHidden)item.FindControl("hdnIsVerified");
                TextBox txtMetricValue = (TextBox)item.FindControl("txtMetricValue");
                //TextBox txtInternationalValue = (TextBox)item.FindControl("txtInternationalValue");
                TextBox txtTextResult = (TextBox)item.FindControl("txtTestResult");
                CheckBox chkIsDeleted = (CheckBox)item.FindControl("chkIsDeleted");
                CheckBox chkIsPendingResult = (CheckBox)item.FindControl("chkIsPendingResult");
                DropDownList cboResultFlag = (DropDownList)item.FindControl("cboResultFlag");
                //CheckBox chkIsNormal = (CheckBox)item.FindControl("chkIsNormal");
                HtmlInputHidden hdnTextResult = (HtmlInputHidden)item.FindControl("hdnTextResult");
                HtmlInputHidden hdnIsResultInPDF = (HtmlInputHidden)item.FindControl("hdnIsResultInPDF");
                HtmlInputHidden hdnRemarks = (HtmlInputHidden)item.FindControl("hdnRemarks");

                LaboratoryResultDt entityDt = lstEntityDt.FirstOrDefault(p => p.ID == Convert.ToInt32(hdnLabResultID.Value) && p.FractionID == Convert.ToInt32(hdnFractionID.Value));
                if (entityDt != null)
                {
                    if (entityDt.IsVerified)
                    {
                        chkResultItem.Enabled = false;
                        //txtInternationalValue.Enabled = false;
                        txtMetricValue.Enabled = false;
                        contentResult.InnerHtml = entityDt.TextValue;
                        cboResultFlag.Enabled = false;
                        chkIsPendingResult.Enabled = false;
                        chkIsPendingResult.Enabled = false;
                        chkIsDeleted.Enabled = false;
                    }
                    else
                        chkResultItem.Checked = true;

                    txtMetricValue.Text = entityDt.MetricResultValue.ToString("G29");
                    //txtInternationalValue.Text = entityDt.InternationalResultValue.ToString("G29");
                    hdnTextResult.Value = HttpUtility.HtmlDecode(entityDt.TextValue);
                    hdnIsVerified.Value = entityDt.IsVerified.ToString();
                    hdnIsNormal.Value = entityDt.IsNormal.ToString();
                    cboResultFlag.SelectedValue = entityDt.ResultFlag;
                    hdnIsResultInPDF.Value = entityDt.TextNormalValue == "PDF" ? "True" : "False";
                    chkIsPendingResult.Checked = entityDt.IsPendingResult;
                    hdnRemarks.Value = entityDt.Remarks;
                }
            }
        }
        #endregion

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            vRecursiveItemLaboratoryFraction item = e.Item.DataItem as vRecursiveItemLaboratoryFraction;

            TextBox txtMetricValue = (TextBox)e.Item.FindControl("txtMetricValue");
            HtmlControl divResultText = (HtmlControl)e.Item.FindControl("divTextResult");
            HtmlControl divIsNormal = (HtmlControl)e.Item.FindControl("divIsNormal");
            CheckBox chkIsDeleted = (CheckBox)e.Item.FindControl("chkIsDeleted");
            Label lblTextResult = (Label)e.Item.FindControl("lblTextResult");
            //CheckBox chkIsNormal = (CheckBox)e.Item.FindControl("chkIsNormal");

            HtmlInputHidden hdnTextResult = (HtmlInputHidden)e.Item.FindControl("hdnTextResult");
            if (item.GCLabTestResultType == Constant.LaboratoryResultValue.NUMERIC)
            {
                Helper.SetControlEntrySetting(txtMetricValue, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            }
            divResultText.Visible = !item.IsNumericResult;
            //chkIsNormal.Visible = !item.IsNumericResult;
            txtMetricValue.Visible = item.IsNumericResult;
            hdnTextResult.Value = HttpUtility.HtmlDecode(item.TextValue);
            //result text 
            if (!item.IsNumericResult) {
                
                if (!string.IsNullOrEmpty(item.TextValue)) {
                    lblTextResult.Text = HttpUtility.HtmlDecode(item.TextValue);
                }
               
            }
            //Show/Hide Checkbox : IsDeleted
            chkIsDeleted.Visible = item.cfIsAllowDelete;

            DropDownList cboResultFlag = e.Item.FindControl("cboResultFlag") as DropDownList;
            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "", Value = "" });
            lstVariable.Add(new Variable { Code = "N", Value = "N" });
            lstVariable.Add(new Variable { Code = "L", Value = "L" });
            lstVariable.Add(new Variable { Code = "LL", Value = "LL" });
            lstVariable.Add(new Variable { Code = "H", Value = "H" });
            lstVariable.Add(new Variable { Code = "HH", Value = "HH" });
            cboResultFlag.DataValueField = "Code";
            cboResultFlag.DataTextField = "Value";
            cboResultFlag.DataSource = lstVariable;
            cboResultFlag.DataBind();
            cboResultFlag.SelectedValue = "N";
        }

        #region Save Entity
        private void ControlToEntity(List<LaboratoryResultDt> lstEntityDt, int flag)
        {
            #region LaboratoryResultDt
            foreach (ListViewDataItem item in lvwView.Items)
            {
                CheckBox chkResultItem = (CheckBox)item.FindControl("chkResultItem");
                HtmlInputHidden hdnFractionID = (HtmlInputHidden)item.FindControl("keyField");
                HtmlInputHidden hdnInternationalUnitMax = (HtmlInputHidden)item.FindControl("hdnInternationalUnitMax");
                HtmlInputHidden hdnInternationalUnitMin = (HtmlInputHidden)item.FindControl("hdnInternationalUnitMin");
                HtmlInputHidden hdnMetricUnitMin = (HtmlInputHidden)item.FindControl("hdnMetricUnitMin");
                HtmlInputHidden hdnMetricUnitMax = (HtmlInputHidden)item.FindControl("hdnMetricUnitMax");
                HtmlInputHidden hdnGCMetricUnit = (HtmlInputHidden)item.FindControl("hdnGCMetricUnit");
                HtmlInputHidden hdnGCInternationalUnit = (HtmlInputHidden)item.FindControl("hdnGCInternationalUnit");
                HtmlInputHidden hdnTextResult = (HtmlInputHidden)item.FindControl("hdnTextResult");
                HtmlInputHidden hdnIsNumeric = (HtmlInputHidden)item.FindControl("hdnIsNumeric");
                HtmlInputHidden hdnConversionFactor = (HtmlInputHidden)item.FindControl("hdnUnitConversion");
                HtmlInputHidden hdnRemarks = (HtmlInputHidden)item.FindControl("hdnRemarks");

                if (chkResultItem.Checked)
                {
                    TextBox txtMetricValue = (TextBox)item.FindControl("txtMetricValue");
                    //TextBox txtInternationalValue = (TextBox)item.FindControl("txtInternationalValue");
                    TextBox txtNormalValueText = (TextBox)item.FindControl("txtNormalValueText");
                    CheckBox chkIsDeleted = (CheckBox)item.FindControl("chkIsDeleted");
                    CheckBox chkIsPendingResult = (CheckBox)item.FindControl("chkIsPendingResult");
                    DropDownList cboResultFlag = (DropDownList)item.FindControl("cboResultFlag");
                    //CheckBox chkIsNormal = (CheckBox)item.FindControl("chkIsNormal");
                    LaboratoryResultDt entityDt = new LaboratoryResultDt();
                    if (flag == 1)
                        entityDt.ID = Convert.ToInt32(hdnLabResultID.Value);
                    entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
                    entityDt.FractionID = Convert.ToInt32(hdnFractionID.Value);

                    if (hdnGCMetricUnit.Value == "N/A" || hdnGCMetricUnit.Value == "")
                        entityDt.GCMetricUnit = "";
                    else
                        entityDt.GCMetricUnit = hdnGCMetricUnit.Value;


                    if (hdnMetricUnitMax.Value == "N/A" || hdnMetricUnitMax.Value == "")
                        entityDt.MaxMetricNormalValue = 0;
                    else
                        entityDt.MaxMetricNormalValue = Convert.ToDecimal(hdnMetricUnitMax.Value);

                    if (hdnMetricUnitMin.Value == "N/A" || hdnMetricUnitMin.Value == "")
                        entityDt.MinMetricNormalValue = 0;
                    else
                        entityDt.MinMetricNormalValue = Convert.ToDecimal(hdnMetricUnitMin.Value);

                    //if (txtInternationalValue.Text == "" || txtInternationalValue.Text == null) txtInternationalValue.Text = "0";
                    if (txtMetricValue.Text == "" || txtMetricValue.Text == null) txtMetricValue.Text = "0";

                    entityDt.MetricResultValue = Convert.ToDecimal(txtMetricValue.Text);
                    entityDt.TextValue = HttpUtility.HtmlDecode(hdnTextResult.Value);

                    #region International Value
                    decimal internationalValue = entityDt.MetricResultValue;
                    entityDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);

                    if (entityDt.ConversionFactor > 1)
                    {
                        internationalValue = entityDt.MetricResultValue * entityDt.ConversionFactor;
                    }
                    entityDt.InternationalResultValue = internationalValue;

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

                    #endregion
              
                    entityDt.IsDeleted = chkIsDeleted.Checked;
                    entityDt.ResultFlag = cboResultFlag.SelectedValue;
                    if (entityDt.TextValue != "" && entityDt.TextValue != null)
                    {
                        entityDt.IsNormal = cboResultFlag.SelectedValue == "N";
                    }
                    else
                    {
                        if (cboResultFlag.SelectedValue == "N")
                            entityDt.IsNormal = true;
                        else
                            entityDt.IsNormal = false;
                    }

                    entityDt.IsNumeric = Convert.ToBoolean(HttpUtility.HtmlDecode(hdnIsNumeric.Value));
                    entityDt.IsPendingResult = chkIsPendingResult.Checked;
                    entityDt.Remarks = hdnRemarks.Value;

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
            string referenceNo = string.Empty;
            try
            {
                DetailPage.SaveLaboratoryResultHd(ctx, ref labResultID, ref referenceNo);
                List<LaboratoryResultDt> lstEntityDt = new List<LaboratoryResultDt>();
                ControlToEntity(lstEntityDt, 0);

                foreach (LaboratoryResultDt entityDt in lstEntityDt)
                {
                    entityDt.ID = labResultID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    PatientChargesDt entityPatientDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND ItemID = {1} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{2}'", hdnTransactionID.Value, entityDt.ItemID, Constant.TransactionStatus.VOID), ctx)[0];
                    entityDt.ReferenceDtID = entityPatientDt.ReferenceDtID;
                    entityPatientDt.IsHasTestResult = true;
                    entityPatientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientDtDao.Update(entityPatientDt);
                }
                //List<PatientChargesDt> lstEntityPatientDt = BusinessLayer.GetPatientChargesDtList(string.Format("ChargeTransactionID = {0} AND ItemID = {1}", hdnTransactionID.Value, hdnItemID.Value));
                retval = labResultID.ToString() + "|" + referenceNo;
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
                            entityDtDao.Delete(entityDt.ID, entityDt.ItemID, entityDt.FractionID);
                    }
                }

                PatientChargesDt entityPatientDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND ItemID = {1} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{2}'", hdnTransactionID.Value, hdnItemID.Value, Constant.TransactionStatus.VOID))[0];
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