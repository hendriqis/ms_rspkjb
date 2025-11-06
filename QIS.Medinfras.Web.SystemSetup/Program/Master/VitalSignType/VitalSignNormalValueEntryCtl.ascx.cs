using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class VitalSignNormalValueEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVitalSignID.Value = param;
            VitalSignType entity = BusinessLayer.GetVitalSignType(Convert.ToInt32(hdnVitalSignID.Value));
            txtVitalSignCode.Text = entity.VitalSignCode;
            txtVitalSignName.Text = entity.VitalSignName;

            BindGridView();

            SetControlProperties();

            ddlSex.Attributes.Add("validationgroup", "mpEntryPopup");
            txtFromAge.Attributes.Add("validationgroup", "mpEntryPopup");
            txtToAge.Attributes.Add("validationgroup", "mpEntryPopup");
            ddlAgeUnit.Attributes.Add("validationgroup", "mpEntryPopup");
            txtMinValue.Attributes.Add("validationgroup", "mpEntryPopup");
            txtMaxValue.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void SetControlProperties()
        {
            ddlSex.Items.Add(new ListItem { Value = "M", Text = GetLabel("Male") });
            ddlSex.Items.Add(new ListItem { Value = "F", Text = GetLabel("Female") });

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.AGE_UNIT));
            Methods.SetComboBoxField<StandardCode>(ddlAgeUnit, lstSc, "StandardCodeName", "StandardCodeID");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvVitalSignNormalValueList(string.Format("VitalSignID = {0} AND IsDeleted = 0 ORDER BY ID ASC", hdnVitalSignID.Value));
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(VitalSignNormalValue entity)
        {
            entity.Sex = Request.Form[ddlSex.UniqueID];
            entity.FromAge = Convert.ToInt16(txtFromAge.Text);
            entity.ToAge = Convert.ToInt16(txtToAge.Text);
            entity.GCAgeUnit = Request.Form[ddlAgeUnit.UniqueID];
            entity.MinValue = Convert.ToDecimal(txtMinValue.Text);
            entity.MaxValue = Convert.ToDecimal(txtMaxValue.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                VitalSignNormalValue entity = new VitalSignNormalValue();
                ControlToEntity(entity);
                entity.VitalSignID = Convert.ToInt32(hdnVitalSignID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertVitalSignNormalValue(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                VitalSignNormalValue entity = BusinessLayer.GetVitalSignNormalValue(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignNormalValue(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                VitalSignNormalValue entity = BusinessLayer.GetVitalSignNormalValue(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignNormalValue(entity);
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