using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class HCPTeamEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParamedicParentID.Value = param;

            ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicParentID.Value));
            txtParamedicParentName.Text = pm.FullName;

            SetControlEntrySetting();
            SetControlProperties();

            BindGridView();

        }

        private void SetControlProperties()
        {
            List<StandardCode> lstRole = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PARAMEDIC_ROLE));
            Methods.SetComboBoxField<StandardCode>(cboParamedicRole, lstRole, "StandardCodeName", "StandardCodeID");
            cboParamedicRole.SelectedIndex = 1;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void SetControlEntrySetting()
        {
            Helper.SetControlEntrySetting(hdnParamedicTeamID, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicTeamCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicTeamName, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboParamedicRole, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        private void BindGridView()
        {
            string filterParamedicTeam = string.Format("ParamedicParentID = {0} AND IsDeleted = 0", hdnParamedicParentID.Value);
            grdView.DataSource = BusinessLayer.GetvParamedicMasterTeamList(filterParamedicTeam);
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

        private void ControlToEntity(ParamedicMasterTeam entity)
        {
            entity.ParamedicParentID = Convert.ToInt32(hdnParamedicParentID.Value);
            entity.ParamedicID = Convert.ToInt32(hdnParamedicTeamID.Value);
            entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            entity.GCParamedicRole = cboParamedicRole.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ParamedicMasterTeam entity = new ParamedicMasterTeam();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertParamedicMasterTeam(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ParamedicMasterTeam entity = BusinessLayer.GetParamedicMasterTeam(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicMasterTeam(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                ParamedicMasterTeam entity = BusinessLayer.GetParamedicMasterTeam(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicMasterTeam(entity);
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