using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class ChangeBedStatusList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.CHANGE_EMERGENCY_BED_STATUS;
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

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.EMERGENCY, "IsUsingRegistration = 1");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                //hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";
                BindGridView();
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpBedStatus");
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
            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", cboServiceUnit.Value.ToString());
            if (hdnRoomID.Value != "")
            {
                filterExpression += string.Format(" AND RoomID = {0}", hdnRoomID.Value);
            }
            filterExpression += string.Format(" ORDER BY RoomID, BedCode");

            if (cboServiceUnit.Value.ToString() != "")
            {
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
                HtmlInputButton btnBedQuickPicks = e.Row.FindControl("btnBedQuickPicks") as HtmlInputButton;
                HtmlInputButton btnDisposisi = e.Row.FindControl("btnDisposisi") as HtmlInputButton;

                if (entity.GCBedStatus == Constant.BedStatus.OCCUPIED || entity.GCBedStatus == Constant.BedStatus.BOOKED || entity.GCBedStatus == Constant.BedStatus.WAIT_TO_BE_TRANSFERRED)
                {
                    btnSave.Style.Add("display", "none");
                    cboBedStatus.ClientVisible = false;
                }
                else
                {
                    btnBedQuickPicks.Style.Add("display", "none");
                    btnDisposisi.Style.Add("display", "none");
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
                if (param[0] == "transfer")
                {
                    int bedID = Convert.ToInt32(param[1]);
                    Bed entity = BusinessLayer.GetBed(bedID);

                    ProcessPatientTransfer(Convert.ToInt32(entity.RegistrationID));
                    result = "success";
                }
                else
                {
                    int bedID = Convert.ToInt32(param[0]);
                    String GCBedStatus = param[1];

                    Bed entity = BusinessLayer.GetBed(bedID);

                    if (entity.GCBedStatus == Constant.BedStatus.UNOCCUPIED || entity.GCBedStatus == Constant.BedStatus.HOUSEKEEPING || entity.GCBedStatus == Constant.BedStatus.CLOSED)
                    {
                        entity.GCBedStatus = GCBedStatus;
                        BusinessLayer.UpdateBed(entity);
                        result = "success";
                    }
                    else
                    {
                        result = string.Format("fail|{0}", "Bed Sudah Terisi/Dibooking, Harap Refresh Halaman Ini");
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ProcessPatientTransfer(int registrationID)
        {
            string url = string.Format("~/Program/PatientTransfer/PatientTransferEntry.aspx?id={0}", registrationID);
            Response.Redirect(url);
        }
    }
}