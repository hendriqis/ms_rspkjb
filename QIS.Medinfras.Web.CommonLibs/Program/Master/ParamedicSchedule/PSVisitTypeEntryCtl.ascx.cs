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
using QIS.Data.Core.Dal;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PSVisitTypeEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnParamedicID.Value = par[0];
            hdnHealthcareServiceUnitID.Value = par[1];

            vServiceUnitParamedic entity = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value))[0];
            txtParamedicName.Text = entity.ParamedicName;
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnit.Text = entity.ServiceUnitName;

            SetControlProperties();

            BindGridView();

            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtVisitDuration, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        private void SetControlProperties()
        {
            List<vServiceUnitVisitType> lstVisitType = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
            Methods.SetComboBoxField<vServiceUnitVisitType>(cboVisitType, lstVisitType, "VisitTypeName", "VisitTypeID");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvParamedicVisitTypeList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} ORDER BY VisitTypeName ASC", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value));
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
                if (hdnVisitTypeID.Value.ToString() != "")
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

        private void ControlToEntity(ParamedicVisitType entity)
        {
            entity.VisitDuration = Convert.ToInt16(txtVisitDuration.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ParamedicVisitType entity = new ParamedicVisitType();
                ControlToEntity(entity);
                entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entity.VisitTypeID = Convert.ToInt32(cboVisitType.Value);
                BusinessLayer.InsertParamedicVisitType(entity);
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
                ParamedicVisitType entity = BusinessLayer.GetParamedicVisitType(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnVisitTypeID.Value));
                ControlToEntity(entity);
                BusinessLayer.UpdateParamedicVisitType(entity);
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
                BusinessLayer.DeleteParamedicVisitType(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnVisitTypeID.Value));
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