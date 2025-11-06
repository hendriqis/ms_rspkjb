using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inpatient.Controls
{
    public partial class BedQuickPicksCtl : BaseContentPopupCtl
    {
        protected int pageC = 1;
        protected string GCBedStatusUnoccupied = "";

        public override void InitializeControl(string param)
        {
            hdnRegistrationID.Value = param;

            GCBedStatusUnoccupied = Constant.BedStatus.UNOCCUPIED;

            string filterSU = string.Format("DepartmentID = '{0}' AND IsUsingRegistration = 1 AND IsDeleted = 0 ORDER BY ServiceUnitCode", Constant.Facility.INPATIENT);
            List<vHealthcareServiceUnit> lstWard = BusinessLayer.GetvHealthcareServiceUnitList(filterSU);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWard, lstWard, "ServiceUnitName", "HealthcareServiceUnitID");
            cboBedPicksWard.SelectedIndex = 0;

            string filterCL = string.Format("IsDeleted = 0");
            List<ClassCare> lstCC = BusinessLayer.GetClassCareList(filterCL);
            Methods.SetComboBoxField<ClassCare>(cboClassPicks, lstCC, "ClassName", "ClassID");
            cboClassPicks.SelectedIndex = 0;

            List<StandardCode> lstGender = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.GENDER, Constant.Gender.UNSPECIFIED));
            lstGender.Add(new StandardCode() { StandardCodeID = "0", StandardCodeName = "All" });
            Methods.SetComboBoxField<StandardCode>(cboGenderPicks, lstGender.OrderBy(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");
            cboGenderPicks.SelectedIndex = 0;

            List<StandardCode> lstBedStatus = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.BED_STATUS, Constant.BedStatus.CLOSED, Constant.BedStatus.OCCUPIED));
            lstBedStatus.Add(new StandardCode() { StandardCodeID = Constant.StandardCode.BED_STATUS + "^" + "OM", StandardCodeName = "TERISI (Laki-laki)" });
            lstBedStatus.Add(new StandardCode() { StandardCodeID = Constant.StandardCode.BED_STATUS + "^" + "OF", StandardCodeName = "TERISI (Perempuan)" });
            rptFooter.DataSource = lstBedStatus;
            rptFooter.DataBind();

            trClass.Attributes.Add("style", "display:none");

            BindGridView(1, true, ref pageC);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            BindingRptRoom();
        }

        private void BindGridViewBed(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            BindingRptBed();
        }

        protected void cbpBedPicksView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpBedPicksViewBed_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewBed(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewBed(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingRptRoom()
        {
            string filterParam = "";
            string genderPicks = cboGenderPicks.Value.ToString();

            if (rblFilter.SelectedValue == "filterClass")
            {
                //lstRoomBed = BusinessLayer.GetBedList(string.Format(
                //    "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE ClassID = {0}) AND GCBedStatus != '{1}' AND IsDeleted = 0",
                //    cboClassPicks.Value, Constant.BedStatus.CLOSED));

                //filterParam = string.Format("ClassID = {0} AND IsDeleted = 0 ORDER BY RoomCode", cboClassPicks.Value);

                if (genderPicks == "0")
                {
                    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                        "RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE ClassID = {0} AND DepartmentID = '{1}') AND GCBedStatus != '{2}' AND IsDeleted = 0",
                        cboClassPicks.Value, Constant.Facility.INPATIENT, Constant.BedStatus.CLOSED));

                    filterParam = string.Format("ClassID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0 ORDER BY RoomCode", cboClassPicks.Value, Constant.Facility.INPATIENT);
                }
                else if (genderPicks == Constant.Gender.MALE)
                {
                    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                        "RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE ClassID = {0} AND DepartmentID = '{3}' AND IsDeleted = 0 AND RoomID IN (SELECT b.RoomID FROM Bed b WHERE (b.RegistrationID IS NULL OR b.RegistrationID IN (SELECT x.RegistrationID FROM (SELECT r.RegistrationID, Gender = CASE WHEN(r.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Registration r LEFT JOIN Guest g ON r.GuestID = g.GuestID LEFT JOIN Patient p ON p.MRN = r.MRN WHERE r.RegistrationID = b.RegistrationID) x WHERE x.Gender = '{1}')))) AND GCBedStatus != '{2}' AND IsDeleted = 0",
                        cboClassPicks.Value, Constant.Gender.MALE, Constant.BedStatus.CLOSED, Constant.Facility.INPATIENT));

                    filterParam = string.Format("ClassID = {0} AND DepartmentID = '{3}' AND IsDeleted = 0 AND RoomID IN (SELECT re.RoomID FROM ServiceUnitRoom re WHERE re.IsDeleted = 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{1}' OR x.Gender IS NULL) >= 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{2}') = 0) ORDER BY RoomCode", cboClassPicks.Value, Constant.Gender.MALE, Constant.Gender.FEMALE, Constant.Facility.INPATIENT);
                }
                else
                {
                    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                        "RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE ClassID = {0} AND DepartmentID = '{3}' AND IsDeleted = 0 AND RoomID IN (SELECT b.RoomID FROM Bed b WHERE (b.RegistrationID IS NULL OR b.RegistrationID IN (SELECT x.RegistrationID FROM (SELECT r.RegistrationID, Gender = CASE WHEN(r.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Registration r LEFT JOIN Guest g ON r.GuestID = g.GuestID LEFT JOIN Patient p ON p.MRN = r.MRN WHERE r.RegistrationID = b.RegistrationID) x WHERE x.Gender = '{1}')))) AND GCBedStatus != '{2}' AND IsDeleted = 0",
                        cboClassPicks.Value, Constant.Gender.FEMALE, Constant.BedStatus.CLOSED, Constant.Facility.INPATIENT));

                    filterParam = string.Format("ClassID = {0} AND DepartmentID = '{3}' AND IsDeleted = 0 AND RoomID IN (SELECT re.RoomID FROM ServiceUnitRoom re WHERE re.IsDeleted = 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{1}' OR x.Gender IS NULL) >= 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{2}') = 0) ORDER BY RoomCode", cboClassPicks.Value, Constant.Gender.FEMALE, Constant.Gender.MALE, Constant.Facility.INPATIENT);
                }
            }
            else
            {
                if (genderPicks == "0")
                {
                    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                        "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE HealthcareServiceUnitID = {0}) AND GCBedStatus != '{1}' AND IsDeleted = 0",
                        cboBedPicksWard.Value, Constant.BedStatus.CLOSED));

                    filterParam = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY RoomCode", cboBedPicksWard.Value);
                }
                else if (genderPicks == Constant.Gender.MALE)
                {
                    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                        "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND RoomID IN (SELECT b.RoomID FROM Bed b WHERE (b.RegistrationID IS NULL OR b.RegistrationID IN (SELECT x.RegistrationID FROM (SELECT r.RegistrationID, Gender = CASE WHEN(r.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Registration r LEFT JOIN Guest g ON r.GuestID = g.GuestID LEFT JOIN Patient p ON p.MRN = r.MRN WHERE r.RegistrationID = b.RegistrationID) x WHERE x.Gender = '{1}')))) AND GCBedStatus != '{2}' AND IsDeleted = 0",
                        cboBedPicksWard.Value, Constant.Gender.MALE, Constant.BedStatus.CLOSED));

                    filterParam = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND RoomID IN (SELECT re.RoomID FROM ServiceUnitRoom re WHERE re.IsDeleted = 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{1}' OR x.Gender IS NULL) >= 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{2}') = 0) ORDER BY RoomCode", cboBedPicksWard.Value, Constant.Gender.MALE, Constant.Gender.FEMALE);
                }
                else
                {
                    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                        "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND RoomID IN (SELECT b.RoomID FROM Bed b WHERE (b.RegistrationID IS NULL OR b.RegistrationID IN (SELECT x.RegistrationID FROM (SELECT r.RegistrationID, Gender = CASE WHEN(r.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Registration r LEFT JOIN Guest g ON r.GuestID = g.GuestID LEFT JOIN Patient p ON p.MRN = r.MRN WHERE r.RegistrationID = b.RegistrationID) x WHERE x.Gender = '{1}')))) AND GCBedStatus != '{2}' AND IsDeleted = 0",
                        cboBedPicksWard.Value, Constant.Gender.FEMALE, Constant.BedStatus.CLOSED));

                    filterParam = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND RoomID IN (SELECT re.RoomID FROM ServiceUnitRoom re WHERE re.IsDeleted = 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{1}' OR x.Gender IS NULL) >= 0 AND (SELECT COUNT(x.BedID) FROM (SELECT b.BedID, Gender = CASE WHEN(p.MRN IS NULL) THEN g.GCGender ELSE p.GCGender END FROM Bed b LEFT JOIN Registration r ON b.RegistrationID = r.RegistrationID LEFT JOIN Patient p ON p.MRN = r.MRN LEFT JOIN Guest g ON g.GuestID = r.GuestID WHERE b.RoomID = re.RoomID) x WHERE x.Gender = '{2}') = 0) ORDER BY RoomCode", cboBedPicksWard.Value, Constant.Gender.FEMALE, Constant.Gender.MALE);
                }
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
                HtmlGenericControl spnRoomStatus = (HtmlGenericControl)e.Item.FindControl("spnRoomRemarks");
                List<Bed> lstBed = lstRoomBed.Where(p => p.RoomID == room.RoomID).ToList();
                int totalBed = lstBed.Count();
                int emptyBed = lstBed.Where(p => p.GCBedStatus == Constant.BedStatus.UNOCCUPIED).Count();
                string roomStatus = "";
                if (emptyBed == 0)
                {
                    roomStatus = "PENUH";
                    spnRoomStatus.Style.Add("color", "red");
                }
                else
                {
                    roomStatus = "TERSEDIA";
                    spnRoomStatus.Style.Add("color", "blue");
                }
                spnRoomInformation.InnerHtml = string.Format("{0} / {1}", Convert.ToString(emptyBed), Convert.ToInt32(totalBed));
                spnRoomStatus.InnerHtml = roomStatus;
            }
        }

        private void BindingRptBed()
        {
            List<vBed> lstBed = BusinessLayer.GetvBedList(string.Format("RoomID = {0} AND GCBedStatus != '{1}' AND IsDeleted = 0 ORDER BY BedCode", hdnRoomID.Value, Constant.BedStatus.CLOSED));
            rptBed.DataSource = lstBed;
            rptBed.DataBind();
        }
    }
}