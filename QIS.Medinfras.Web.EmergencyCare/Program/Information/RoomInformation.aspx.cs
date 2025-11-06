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

namespace QIS.Medinfras.Web.Emergency.Program
{
    public partial class RoomInformation : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.BED_INFORMATION;
        }

        private GetUserMenuAccess menu;

        public override bool IsShowRightPanel()
        {
            return false;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<StandardCode> lstBedStatus = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.BED_STATUS, Constant.BedStatus.CLOSED, Constant.BedStatus.OCCUPIED));
                lstBedStatus.Add(new StandardCode() { StandardCodeID = Constant.StandardCode.BED_STATUS + "^" + "OM", StandardCodeName = "TERISI (Laki-laki)" });
                lstBedStatus.Add(new StandardCode() { StandardCodeID = Constant.StandardCode.BED_STATUS + "^" + "OF", StandardCodeName = "TERISI (Perempuan)" });
                rptFooter.DataSource = lstBedStatus;
                rptFooter.DataBind();

                string filterSU = string.Format("DepartmentID = '{0}' AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.EMERGENCY);
                List<vHealthcareServiceUnit> lstWard = BusinessLayer.GetvHealthcareServiceUnitList(filterSU);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWard, lstWard, "ServiceUnitName", "HealthcareServiceUnitID");
                cboBedPicksWard.SelectedIndex = 0;

                string filterCL = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC = BusinessLayer.GetClassCareList(filterCL);
                Methods.SetComboBoxField<ClassCare>(cboClassPicks, lstCC, "ClassName", "ClassID");
                cboClassPicks.SelectedIndex = 0;

                trClass.Attributes.Add("style", "display:none");

                BindGridView();

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void BindGridView()
        {
            BindingRptRoom();
        }

        private void BindGridViewBed()
        {
            BindingRptBed();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewBed_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridViewBed();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingRptRoom()
        {
            string filterParam = "";
            if (rblFilter.SelectedValue == "filterClass")
            {
                lstRoomBed = BusinessLayer.GetBedList(string.Format(
                    "RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE ClassID = {0} AND DepartmentID = '{1}') AND GCBedStatus != '{2}' AND IsDeleted = 0",
                    cboClassPicks.Value, Constant.Facility.EMERGENCY, Constant.BedStatus.CLOSED));

                filterParam = string.Format("ClassID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0 ORDER BY RoomCode", cboClassPicks.Value, Constant.Facility.EMERGENCY);
            }
            else
            {
                lstRoomBed = BusinessLayer.GetBedList(string.Format(
                    "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE HealthcareServiceUnitID = {0}) AND GCBedStatus != '{1}' AND IsDeleted = 0",
                    cboBedPicksWard.Value, Constant.BedStatus.CLOSED));

                filterParam = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY RoomCode", cboBedPicksWard.Value);
            }

            List<vServiceUnitRoom> lstRoom = BusinessLayer.GetvServiceUnitRoomList(filterParam);
            rptRoom.DataSource = lstRoom;
            rptRoom.DataBind();
        }

        List<Bed> lstRoomBed = null;

        protected void rptRoom_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                vServiceUnitRoom room = e.Item.DataItem as vServiceUnitRoom;
                HtmlGenericControl spnRoomInformation = (HtmlGenericControl)e.Item.FindControl("spnRoomInformation");
                HtmlGenericControl spnRoomRemarks = (HtmlGenericControl)e.Item.FindControl("spnRoomRemarks");

                List<Bed> lstBed = lstRoomBed.Where(p => p.RoomID == room.RoomID).ToList();
                int totalBed = lstBed.Count();
                int emptyBed = lstBed.Where(p => p.GCBedStatus == Constant.BedStatus.UNOCCUPIED).Count();

                spnRoomInformation.InnerHtml = string.Format("{0} / {1}", Convert.ToString(emptyBed), Convert.ToInt32(totalBed));
                spnRoomRemarks.InnerHtml = emptyBed == 0 ? "PENUH" : string.Format("TERSEDIA ({0})", emptyBed.ToString());
                if (emptyBed <= 0)
                    spnRoomRemarks.Style.Add("color", "red");
                else
                    spnRoomRemarks.Style.Add("color", "blue");
            }
        }

        private void BindingRptBed()
        {
            List<vBed> lstBed = BusinessLayer.GetvBedList(string.Format("RoomID = {0} AND GCBedStatus != '{1}' AND IsDeleted = 0", hdnRoomID.Value, Constant.BedStatus.CLOSED));
            rptBed.DataSource = lstBed.Where(x => x.RoomID == Convert.ToInt32(hdnRoomID.Value) && x.GCBedStatus != Constant.BedStatus.CLOSED);
            rptBed.DataBind();
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            return "";
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }
    }
}