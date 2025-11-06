using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class VitalSignNormalValueEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnVitalSignID.Value = param;
            VitalSignType entity = BusinessLayer.GetVitalSignType(Convert.ToInt32(hdnVitalSignID.Value));
            txtVitalSignCode.Text = entity.VitalSignCode;
            txtVitalSignName.Text = entity.VitalSignName;

            BindGridView(1, true, ref PageCount);

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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = string.Format("VitalSignID = {0} AND IsDeleted = 0", hdnVitalSignID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignNormalValueRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            grdView.DataSource = BusinessLayer.GetvVitalSignNormalValueList(filterExpression, 8, pageIndex, "ID ASC");
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

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView(1, true, ref pageCount);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }

            result += "|" + pageCount;
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

            if (entity.GCAgeUnit == "X008^001") { // hari
                entity.FromAgeInDay = entity.FromAge * 1;
                entity.ToAgeInDay = entity.ToAge * 1; 
            }
            else if (entity.GCAgeUnit == "X008^002") // minggu
            {
                entity.FromAgeInDay = entity.FromAge * 7;
                entity.ToAgeInDay = entity.ToAge * 7; 
            }
            else if (entity.GCAgeUnit == "X008^003") { //bulan 
                entity.FromAgeInDay = entity.FromAge * 30;
                entity.ToAgeInDay = entity.ToAge * 30; 
            }
            else //tahun
            {
                entity.FromAgeInDay = entity.FromAge * 365;
                entity.ToAgeInDay = entity.ToAge * 365; 
            }
            
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