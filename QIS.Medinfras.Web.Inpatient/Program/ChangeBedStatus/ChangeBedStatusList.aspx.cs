using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class ChangeBedStatusList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.CHANGE_BED_STATUS;
        }
        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "IsUsingRegistration = 1");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";
                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID,
                    Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID,
                    Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID));
                hdnHealthcareServiceUnitICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                hdnHealthcareServiceUnitNICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                hdnHealthcareServiceUnitPICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                
                BindGridView();
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpBedStatus");
                Helper.SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, true), "mpBedStatus");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }


        private List<StandardCode> lstBedStatus = null;
        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (cboServiceUnit.Value.ToString() != "" && hdnRoomID.Value != "")
            {
                filterExpression = string.Format("RoomID = {0} AND IsDeleted = 0", hdnRoomID.Value);
                if (lstBedStatus == null)
                    lstBedStatus = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.BED_STATUS, Constant.BedStatus.BOOKED, Constant.BedStatus.OCCUPIED, Constant.BedStatus.WAIT_TO_BE_TRANSFERRED));
            }
            List<vBed> lstEntity = BusinessLayer.GetvBedList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vBed entity = e.Row.DataItem as vBed;
                ASPxComboBox cboBedStatus = e.Row.FindControl("cboBedStatus") as ASPxComboBox;
                HtmlInputButton btnSave = e.Row.FindControl("btnSave") as HtmlInputButton;

                if (entity.GCBedStatus == Constant.BedStatus.OCCUPIED || entity.GCBedStatus == Constant.BedStatus.BOOKED || entity.GCBedStatus == Constant.BedStatus.WAIT_TO_BE_TRANSFERRED)
                {
                    btnSave.Style.Add("display", "none");
                    cboBedStatus.ClientVisible = false;
                }
                else
                {
                    Methods.SetComboBoxField<StandardCode>(cboBedStatus, lstBedStatus, "StandardCodeName", "StandardCodeID");
                    cboBedStatus.Value = entity.GCBedStatus;
                }
            }
        }

        protected void cbpSaveBedStatus_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            try
            {
                string[] param = e.Parameter.Split('|');
                int bedID = Convert.ToInt32(param[0]);
                String GCBedStatus = param[1];

                Bed entity = BusinessLayer.GetBed(bedID);

                if (entity.GCBedStatus == Constant.BedStatus.UNOCCUPIED || entity.GCBedStatus == Constant.BedStatus.HOUSEKEEPING || entity.GCBedStatus == Constant.BedStatus.CLOSED)
                {
                    entity.GCBedStatus = GCBedStatus;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateBed(entity);
                    result = "success|" + entity.RoomID.ToString();
                }
                else
                {
                    result = string.Format("fail|{0}", "Bed Sudah Terisi/Dibooking, Harap Refresh Halaman Ini");
                }
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}