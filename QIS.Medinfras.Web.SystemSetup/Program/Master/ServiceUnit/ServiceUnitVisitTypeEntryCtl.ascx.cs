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
    public partial class ServiceUnitVisitTypeEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnHealthcareServiceUnitID.Value = param;

            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value))[0];
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnit.Text = entity.ServiceUnitName;

            SetControlProperties();

            BindGridView();

            ddlVisitType.Attributes.Add("validationgroup", "mpEntryPopup");
            txtVisitDuration.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void SetControlProperties()
        {
            List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList("IsDeleted = 0");
            Methods.SetComboBoxField<VisitType>(ddlVisitType, lstVisitType, "VisitTypeName", "VisitTypeID");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0} ORDER BY VisitTypeName ASC", hdnHealthcareServiceUnitID.Value));
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

        private void ControlToEntity(ServiceUnitVisitType entity)
        {
            entity.VisitDuration = Convert.ToInt16(txtVisitDuration.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ServiceUnitVisitType entity = new ServiceUnitVisitType();
                ControlToEntity(entity);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entity.VisitTypeID = Convert.ToInt32(Request.Form[ddlVisitType.UniqueID]);
                BusinessLayer.InsertServiceUnitVisitType(entity);
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
                ServiceUnitVisitType entity = BusinessLayer.GetServiceUnitVisitType(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnVisitTypeID.Value));
                ControlToEntity(entity);
                BusinessLayer.UpdateServiceUnitVisitType(entity);
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
                BusinessLayer.DeleteServiceUnitVisitType(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnVisitTypeID.Value));
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