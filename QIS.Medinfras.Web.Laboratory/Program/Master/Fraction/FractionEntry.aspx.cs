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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class FractionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.FRACTION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnFractionID.Value = ID;
                vFraction entity = BusinessLayer.GetvFractionList(string.Format("FractionID = {0}", hdnFractionID.Value)).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
                //if (entity.ParentID != null && entity.ParentID > 0)
                //{
                //    Fraction entityParent = BusinessLayer.GetFraction((int)entity.ParentID);
                //    txtParentCode.Text = entityParent.FractionCode;
                //    txtParentName.Text = entityParent.FractionName1;
                //}
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.LABORATORY_RESULT_TYPE, Constant.StandardCode.LABORATORY_UNIT));
            //lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboMetricUnit, lstSC.Where(p => p.ParentID == Constant.StandardCode.LABORATORY_UNIT).OrderBy(sc => sc.StandardCodeName).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
            Methods.SetComboBoxField<StandardCode>(cboInternationalUnit, lstSC.Where(p => p.ParentID == Constant.StandardCode.LABORATORY_UNIT).OrderBy(sc => sc.StandardCodeName).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
            Methods.SetComboBoxField<StandardCode>(cboLabTestResultType, lstSC.Where(p => p.ParentID == Constant.StandardCode.LABORATORY_RESULT_TYPE).OrderBy(sc => sc.StandardCodeName).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            #region RightSide
            SetControlEntrySetting(cboLabTestResultType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDecmalDigits, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtChartMaxValue, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtChartMinValue, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtDisplayColor, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDisplayColorPicker, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(chkIsDisplay, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCommCode, new ControlEntrySetting(true, true, false, string.Empty));
            SetControlEntrySetting(txtTestMethod, new ControlEntrySetting(true, true, false, string.Empty));
            SetControlEntrySetting(chkIsConfidential, new ControlEntrySetting(true, true, false));
            #endregion

            #region LeftSide
            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnFractionGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtFractionGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFractionGroupName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnSpecimenID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSpecimenCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSpecimenName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtConversion, new ControlEntrySetting(true, true, true, 0));

            SetControlEntrySetting(txtFractionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFractionName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFractionName2, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(cboInternationalUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboMetricUnit, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(chkIsUsingFooterNote, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFooterNote, new ControlEntrySetting(true, true, false));
            #endregion
        }

        private void EntityToControl(vFraction entity)
        {
            #region RightSide
            cboLabTestResultType.Value = entity.GCLabTestResultType;
            txtDecmalDigits.Text = entity.DecimalDigits.ToString("G29");
            txtChartMaxValue.Text = entity.ChartMaxValue.ToString("G29");
            txtChartMinValue.Text = entity.ChartMinValue.ToString("G29");
            txtDisplayColor.Text = txtDisplayColorPicker.Text = entity.DisplayColor;
            txtDisplayOrder.Text = entity.DisplayOrder.ToString("G29");
            chkIsDisplay.Checked = entity.IsDisplayInChart;
            txtCommCode.Text = entity.CommCode;
            txtTestMethod.Text = entity.TestMethod;
            chkIsConfidential.Checked = entity.IsConfidential;
            #endregion

            #region LeftSide

            //hdnParentID.Value = entity.ParentID.ToString();

            hdnFractionGroupID.Value = entity.FractionGroupID.ToString();
            txtFractionGroupCode.Text = entity.FractionGroupCode;
            txtFractionGroupName.Text = entity.FractionGroupName;

            hdnSpecimenID.Value = entity.SpecimenID.ToString();
            txtSpecimenCode.Text = entity.SpecimenCode;
            txtSpecimenName.Text = entity.SpecimenName;

            txtConversion.Text = entity.ConversionFactor.ToString("G29");

            txtRemarks.Text = entity.Remarks.ToString();

            txtFractionCode.Text = entity.FractionCode;
            txtFractionName1.Text = entity.FractionName1;
            txtFractionName2.Text = entity.FractionName2;

            //chkIsHeader.Checked = entity.IsHeader;

            cboInternationalUnit.Value = entity.GCInternationalUnit;
            cboMetricUnit.Value = entity.GCMetricUnit;

            chkIsUsingFooterNote.Checked = entity.IsUsingFooterNote;
            txtFooterNote.Text = entity.FooterNote;

            #endregion
        }

        private void ControlToEntity(Fraction entity)
        {
            #region RightSide
            entity.GCLabTestResultType = cboLabTestResultType.Value.ToString();
            entity.DecimalDigits = Convert.ToInt16(txtDecmalDigits.Text);
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            entity.DisplayColor = txtDisplayColor.Text;
            entity.ChartMinValue = Convert.ToInt32(txtChartMinValue.Text);
            entity.ChartMaxValue = Convert.ToInt32(txtChartMaxValue.Text);
            entity.IsDisplayInChart = chkIsDisplay.Checked;

            if (!string.IsNullOrEmpty(txtCommCode.Text))
                entity.CommCode = txtCommCode.Text;
            if (!string.IsNullOrEmpty(txtTestMethod.Text))
                entity.TestMethod = txtTestMethod.Text;

            entity.IsConfidential = chkIsConfidential.Checked;

            #endregion

            #region LeftSide

            entity.FractionCode = txtFractionCode.Text;
            entity.FractionName1 = txtFractionName1.Text;
            entity.FractionName2 = txtFractionName2.Text;

            if (hdnFractionGroupID.Value != "" && hdnFractionGroupID.Value != "0")
                entity.FractionGroupID = Convert.ToInt32(hdnFractionGroupID.Value);
            else
                entity.FractionGroupID = null;

            entity.SpecimenID = Convert.ToInt32(hdnSpecimenID.Value);

            if (cboMetricUnit.Value != null)
                entity.GCMetricUnit = cboMetricUnit.Value.ToString();
            else
                entity.GCMetricUnit = null;
            if (cboInternationalUnit.Value != null)
                entity.GCInternationalUnit = cboInternationalUnit.Value.ToString();
            else
                entity.GCInternationalUnit = null;


            entity.ConversionFactor = Convert.ToDecimal(txtConversion.Text);

            entity.Remarks = txtRemarks.Text;

            entity.IsUsingFooterNote = chkIsUsingFooterNote.Checked;
            entity.FooterNote = txtFooterNote.Text;

            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("FractionCode = '{0}' AND IsDeleted = 0", txtFractionCode.Text);
            List<Fraction> lst = BusinessLayer.GetFractionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Fraction Code is already exist";

            return (errMessage == string.Empty);
        }


        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            FractionDao entityDao = new FractionDao(ctx);
            bool result = false;
            try
            {
                Fraction entity = new Fraction();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetFractionMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
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

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnFractionID.Value);
            string FilterExpression = string.Format("FractionCode = '{0}' AND FractionID != {1} AND IsDeleted = 0", txtFractionCode.Text, ID);
            List<Fraction> lst = BusinessLayer.GetFractionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Fraction with Code " + txtFractionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Fraction entity = BusinessLayer.GetFraction(Convert.ToInt32(hdnFractionID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFraction(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}