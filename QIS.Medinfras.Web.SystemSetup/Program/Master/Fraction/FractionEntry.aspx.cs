using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class FractionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.FRACTION;
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
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> ListStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.LABORATORY_UNIT, Constant.StandardCode.LABORATORY_RESULT_TYPE));
            string textField = "StandardCodeName";
            string valueField = "StandardCodeID";
            ListStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCMetricUnit, ListStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.LABORATORY_UNIT || sc.StandardCodeID == "").OrderBy(sc => sc.StandardCodeName).ToList<StandardCode>(), textField, valueField);
            Methods.SetComboBoxField<StandardCode>(cboGCInternationalUnit, ListStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.LABORATORY_UNIT || sc.StandardCodeID == "").OrderBy(sc => sc.StandardCodeName).ToList<StandardCode>(), textField, valueField);
            Methods.SetComboBoxField<StandardCode>(cboGCLabTestResultType, ListStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.LABORATORY_RESULT_TYPE).OrderBy(sc => sc.StandardCodeName).ToList<StandardCode>(), textField, valueField);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFractionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFractionName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFractionName2, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnSpecimenID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSpecimenCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSpecimenName, new ControlEntrySetting(false, false, true));
            
            SetControlEntrySetting(cboGCMetricUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCInternationalUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCLabTestResultType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtConversionFactor, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));
            
            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtChartMinValue, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtChartMaxValue, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtDisplayColor, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsDisplayInChart, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vFraction entity)
        {
            txtFractionCode.Text = entity.FractionCode;
            txtFractionName1.Text = entity.FractionName1;
            txtFractionName2.Text = entity.FractionName2;
            //hdnParentID.Value = entity.ParentID.ToString();
            hdnSpecimenID.Value = entity.SpecimenID.ToString();
            txtSpecimenCode.Text = entity.SpecimenCode;
            txtSpecimenName.Text = entity.SpecimenName;
            cboGCMetricUnit.Value = entity.GCMetricUnit;
            cboGCInternationalUnit.Value = entity.GCMetricUnit;
            cboGCLabTestResultType.Value = entity.GCLabTestResultType;
            txtConversionFactor.Text = entity.ConversionFactor.ToString();
            
            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
            txtDisplayColor.Text = txtDisplayColorPicker.Text = entity.DisplayColor;
            txtChartMinValue.Text = entity.ChartMinValue.ToString();
            txtChartMaxValue.Text = entity.ChartMaxValue.ToString();
            chkIsDisplayInChart.Checked = entity.IsDisplayInChart;
            //chkIsHeader.Checked = entity.IsHeader;
            
        }

        private void ControlToEntity(Fraction entity)
        {
            entity.FractionCode = txtFractionCode.Text;
            entity.FractionName1 = txtFractionName1.Text;
            entity.FractionName2 = txtFractionName2.Text;
            
            if (hdnParentID.Value == "" || hdnParentID.Value == "0")
                entity.ParentID = null;
            else
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);

            entity.SpecimenID = Convert.ToInt32(hdnSpecimenID.Value);

            if (cboGCMetricUnit.Value != null)
                entity.GCMetricUnit = cboGCMetricUnit.Value.ToString();
            else
                entity.GCMetricUnit = null;
            if (cboGCInternationalUnit.Value != null)
                entity.GCInternationalUnit = cboGCInternationalUnit.Value.ToString();
            else
                entity.GCInternationalUnit = null;
            entity.GCLabTestResultType = cboGCLabTestResultType.Value.ToString();
            
            entity.ConversionFactor = Convert.ToDecimal(txtConversionFactor.Text.ToString());

            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            entity.DisplayColor = txtDisplayColor.Text;
            entity.ChartMinValue = Convert.ToInt32(txtChartMinValue.Text);
            entity.ChartMaxValue = Convert.ToInt32(txtChartMaxValue.Text);
            entity.IsDisplayInChart = chkIsDisplayInChart.Checked;
            entity.IsHeader = chkIsHeader.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("FractionCode = '{0}'", txtFractionCode.Text);
            List<Fraction> lst = BusinessLayer.GetFractionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Fraction with code " + txtFractionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("FractionCode = '{0}' AND FractionID != {1}", txtFractionCode.Text, hdnFractionID.Value);
            List<Fraction> lst = BusinessLayer.GetFractionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Fraction with Code " + txtFractionCode.Text + " is already exist!";

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
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
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
                return false;
            }
        }
    }
}