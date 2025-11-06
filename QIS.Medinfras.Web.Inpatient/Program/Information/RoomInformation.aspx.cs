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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class RoomInformation : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.BED_INFORMATION;
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

                string filterSU = string.Format("DepartmentID = '{0}' AND IsUsingRegistration = 1 AND IsDeleted = 0 ORDER BY ServiceUnitName", Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstWard = BusinessLayer.GetvHealthcareServiceUnitList(filterSU);
                //lstWard.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWard, lstWard, "ServiceUnitName", "HealthcareServiceUnitID");
                cboBedPicksWard.SelectedIndex = 0;

                string filterCL = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC = BusinessLayer.GetClassCareList(filterCL);
                //lstCC.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboClassPicks, lstCC, "ClassName", "ClassID");
                cboClassPicks.SelectedIndex = 0;

                trClass.Attributes.Add("style", "display:none");

                BindGridView();

                string filterSU2 = string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstWard2 = BusinessLayer.GetvHealthcareServiceUnitList(filterSU2);
                lstWard2 = lstWard2.OrderBy(unit => unit.ServiceUnitName).ToList();
                lstWard2.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWardSum, lstWard2, "ServiceUnitName", "HealthcareServiceUnitID");
                cboBedPicksWardSum.SelectedIndex = 0;

                string filterCL2 = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC2 = BusinessLayer.GetClassCareList(filterCL2);
                lstCC2.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboClassPicksSum, lstCC2, "ClassName", "ClassID");
                cboClassPicksSum.SelectedIndex = 0;

                trClassSum.Attributes.Add("style", "display:none");

                BindingViewSummary();

                string filterSU4 = string.Format("DepartmentID = '{0}' and IsDeleted = 0", Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstWard4 = BusinessLayer.GetvHealthcareServiceUnitList(filterSU4);
                lstWard4 = lstWard4.OrderBy(unit => unit.ServiceUnitName).ToList();
                lstWard4.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWardBedBooking, lstWard4, "ServiceUnitName", "HealthcareServiceUnitID");
                cboBedPicksWardBedBooking.SelectedIndex = 0;

                string filterCL4 = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC4 = BusinessLayer.GetClassCareList(filterCL4);
                lstCC4.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboClassPicksBedBooking, lstCC4, "ClassName", "ClassID");
                cboClassPicksBedBooking.SelectedIndex = 0;

                trClassBedBooking.Attributes.Add("style", "display:none");

                BindingViewBedBooking();

                string filterSU3 = string.Format("DepartmentID = '{0}' and IsDeleted = 0", Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstWard3 = BusinessLayer.GetvHealthcareServiceUnitList(filterSU3);
                lstWard3 = lstWard3.OrderBy(unit => unit.ServiceUnitName).ToList();
                lstWard3.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWardPatientPlan, lstWard3, "ServiceUnitName", "HealthcareServiceUnitID");
                cboBedPicksWardPatientPlan.SelectedIndex = 0;

                string filterCL3 = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC3 = BusinessLayer.GetClassCareList(filterCL3);
                lstCC3.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboClassPicksPatientPlan, lstCC3, "ClassName", "ClassID");
                cboClassPicksPatientPlan.SelectedIndex = 0;

                trClassPlan.Attributes.Add("style", "display:none");

                BindingViewPatientPlan();

                string filterSU5 = string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstWard5 = BusinessLayer.GetvHealthcareServiceUnitList(filterSU5);
                lstWard5 = lstWard5.OrderBy(unit => unit.ServiceUnitName).ToList();
                lstWard5.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWardTitipan, lstWard5, "ServiceUnitName", "HealthcareServiceUnitID");
                cboBedPicksWardTitipan.SelectedIndex = 0;

                string filterCL5 = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC5 = BusinessLayer.GetClassCareList(filterCL5);
                lstCC5.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboClassPicksTitipan, lstCC5, "ClassName", "ClassID");
                cboClassPicksTitipan.SelectedIndex = 0;

                trClassTitipan.Attributes.Add("style", "display:none");

                BindingViewTitipan();

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
            string cboBedWard1 = "";
            string cboClass1 = "";
            cboBedWard1 = cboClassPicks.Value.ToString();
            cboClass1 = cboBedPicksWard.Value.ToString();
            if (rblFilter.SelectedValue == "filterClass")
            {
                lstRoomBed = BusinessLayer.GetBedList(string.Format(
                    "RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE ClassID = {0} AND DepartmentID = '{1}') AND GCBedStatus != '{1}' AND IsDeleted = 0",
                    cboClassPicks.Value, Constant.Facility.INPATIENT, Constant.BedStatus.CLOSED));

                filterParam = string.Format("ClassID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0 ORDER BY RoomCode", cboClassPicks.Value, Constant.Facility.INPATIENT);

                //if (cboClass1 != "0")
                //{
                //    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                //    "RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE ClassID = {0} AND DepartmentID = '{1}') AND GCBedStatus != '{1}' AND IsDeleted = 0",
                //    cboClassPicks.Value, Constant.Facility.INPATIENT, Constant.BedStatus.CLOSED));

                //    filterParam = string.Format("ClassID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0 ORDER BY RoomCode", cboClassPicks.Value, Constant.Facility.INPATIENT);
                //}
                //else
                //{
                //    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                //    "RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE DepartmentID = '{0}') AND GCBedStatus != '{1}' AND IsDeleted = 0",
                //    Constant.Facility.INPATIENT, Constant.BedStatus.CLOSED));

                //    filterParam = string.Format("DepartmentID = '{0}' AND IsDeleted = 0 ORDER BY RoomCode", Constant.Facility.INPATIENT);
                //}
            }
            else
            {
                lstRoomBed = BusinessLayer.GetBedList(string.Format(
                    "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE HealthcareServiceUnitID = {0}) AND GCBedStatus != '{1}' AND IsDeleted = 0",
                    cboBedPicksWard.Value, Constant.BedStatus.CLOSED));

                filterParam = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY RoomCode", cboBedPicksWard.Value);

                //if (cboBedWard1 != "0")
                //{
                //    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                //    "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE HealthcareServiceUnitID = {0}) AND GCBedStatus != '{1}' AND IsDeleted = 0",
                //    cboBedPicksWard.Value, Constant.BedStatus.CLOSED));

                //    filterParam = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY RoomCode", cboBedPicksWard.Value);
                //}
                //else
                //{
                //    lstRoomBed = BusinessLayer.GetBedList(string.Format(
                //    "RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE GCBedStatus != '{0}' AND IsDeleted = 0)",
                //    Constant.BedStatus.CLOSED));

                //    filterParam = string.Format("IsDeleted = 0 ORDER BY RoomCode");
                //}
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

        protected void cbpViewSum_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindingViewSummary();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingViewSummary()
        {
            string filterParam = "";
            string cboBedWard2 = "";
            string cboClass2 = "";
            cboBedWard2 = cboBedPicksWardSum.Value.ToString();
            cboClass2 = cboClassPicksSum.Value.ToString();
            if (rblFilterSum.SelectedValue == "filterClassSum")
            {
                if (cboClass2 != "0")
                {
                    filterParam = string.Format("ClassID = {0}", cboClassPicksSum.Value);
                }
                else
                {
                    filterParam = string.Format("");
                }
            }
            else
            {
                if (cboBedWard2 != "0")
                {
                    filterParam = string.Format("HealthcareServiceUnitID = {0}", cboBedPicksWardSum.Value);
                }
                else
                {
                    filterParam = string.Format("");
                }
            }

            List<vBedInformationSummary> lstSum = BusinessLayer.GetvBedInformationSummaryList(filterParam, Constant.GridViewPageSize.GRID_TEMP_MAX_FOR_ORDERING, 1, "RoomCode ASC");
            lvwView.DataSource = lstSum;
            lvwView.DataBind();
        }

        protected void cbpViewBedBooking_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindingViewBedBooking();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingViewBedBooking()
        {
            string filterParam = "";
            string cboBedWard4 = "";
            string cboClass4 = "";
            cboBedWard4 = cboBedPicksWardBedBooking.Value.ToString();
            cboClass4 = cboClassPicksBedBooking.Value.ToString();
            if (rblFilterBedBooking.SelectedValue == "filterClassBedBooking")
            {
                if (cboClass4 != "0")
                {
                    filterParam = string.Format("ClassID = {0}", cboClassPicksBedBooking.Value);
                }
                else
                {
                    filterParam = string.Format("");
                }
            }
            else
            {
                if (cboBedWard4 != "0")
                {
                    filterParam = string.Format("HealthcareServiceUnitID = {0}", cboBedPicksWardBedBooking.Value);
                }
                else
                {
                    filterParam = string.Format("");
                }
                
            }

            List<vBedInformationBooking> lstBedBooking = BusinessLayer.GetvBedInformationBookingList(filterParam, Constant.GridViewPageSize.GRID_TEMP_MAX_FOR_ORDERING, 1, "RoomCode, OrderBy, BedCode, QueueNo");
            lvwViewBedBooking.DataSource = lstBedBooking;
            lvwViewBedBooking.DataBind();
        }

        protected void cbpViewPatientPlan_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindingViewPatientPlan();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingViewPatientPlan()
        {
            string filterParam = "";
            string cboBedWard3 = "";
            string cboClass3 = "";
            cboBedWard3 = cboBedPicksWardPatientPlan.Value.ToString();
            cboClass3 = cboClassPicksPatientPlan.Value.ToString();
            filterParam += string.Format("GCVisitStatus NOT IN  ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.OPEN, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
            if (rblFilterPlan.SelectedValue == "filterClassPlan")
            {
                if (cboClass3 != "0")
                {
                    filterParam += string.Format(" AND PlanDischargeDate != '' AND ClassID = {0} ORDER BY RoomCode, BedCode", cboClassPicksPatientPlan.Value);
                }
                else
                {
                    filterParam += string.Format(" AND PlanDischargeDate != '' ORDER BY RoomCode, BedCode");
                }
            }
            else
            {
                if (cboBedWard3 != "0")
                {
                    filterParam += string.Format(" AND PlanDischargeDate != '' AND HealthcareServiceUnitID = {0} ORDER BY RoomCode, BedCode", cboBedPicksWardPatientPlan.Value);
                }
                else
                {
                    filterParam += string.Format(" AND PlanDischargeDate != '' ORDER BY RoomCode, BedCode");
                }
            }

            List<vBedInformationOccupied> lstPlan = BusinessLayer.GetvBedInformationOccupiedList(filterParam);
            lvwViewPatientPlan.DataSource = lstPlan;
            lvwViewPatientPlan.DataBind();
        }

        protected void cbpViewTitipan_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindingViewTitipan();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingViewTitipan()
        {
            string filterParam = "";
            string cboBedWard5 = "";
            string cboClass5 = "";
            cboBedWard5 = cboBedPicksWardTitipan.Value.ToString();
            cboClass5 = cboClassPicksTitipan.Value.ToString();
            filterParam += string.Format("ClassID != RequestClassID AND GCVisitStatus NOT IN  ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.OPEN, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);

            if (rblFilterTitipan.SelectedValue == "filterClassTitipan")
            {
                if (cboClass5 != "0")
                {
                    filterParam += string.Format(" AND RequestClassID IS NOT NULL AND ClassID = {0} ORDER BY RoomCode, BedCode", cboClassPicksTitipan.Value);
                }
                else
                {
                    filterParam += string.Format(" AND RequestClassID IS NOT NULL ORDER BY RoomCode, BedCode");
                }
            }
            else
            {
                if (cboBedWard5 != "0")
                {
                    filterParam += string.Format(" AND RequestClassID IS NOT NULL AND HealthcareServiceUnitID = {0} ORDER BY RoomCode, BedCode", cboBedPicksWardTitipan.Value);
                }
                else
                {
                    filterParam += string.Format(" AND RequestClassID IS NOT NULL ORDER BY RoomCode, BedCode");
                }
            }

            List<vConsultVisit7> lstTitipan = BusinessLayer.GetvConsultVisit7List(filterParam).OrderBy(lst => lst.RegistrationNo).ToList();
            lstViewTitipan.DataSource = lstTitipan;
            lstViewTitipan.DataBind();
        }

        //#region SummaryWard
        //protected void cbpViewSummary_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    int pageCount = 1;
        //    string result = "";
        //    if (e.Parameter != null && e.Parameter != "")
        //    {
        //        string[] param = e.Parameter.Split('|');
        //        if (param[0] == "changepage")
        //        {
        //            BindingRptSummaryBed();
        //            result = "changepage";
        //        }
        //        else // refresh
        //        {
        //            BindingRptSummaryBed();
        //            result = "refresh|" + pageCount;
        //        }
        //    }

        //    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        //    panel.JSProperties["cpResult"] = result;
        //}

        //private List<vRegistration> lstRegistration = null;
        //private void BindingRptSummaryBed() 
        //{
        //    List<vBed> lstBed = BusinessLayer.GetvBedList(String.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value));
        //    lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID IN (SELECT RegistrationID FROM vBed WHERE HealthcareServiceUnitID = {0} AND IsDeleted = 0)", hdnHealthcareServiceUnitID.Value));

        //    grdView.DataSource = lstBed;
        //    grdView.DataBind();
        //}

        //protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        vBed entity = e.Row.DataItem as vBed;
        //        if (entity.RegistrationID != 0) 
        //        {
        //            String filterExpression = String.Format("RegistrationID = {0}", entity.RegistrationID);
        //            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();

        //            HtmlGenericControl divAddress = e.Row.FindControl("divAddress") as HtmlGenericControl;
        //            divAddress.InnerHtml = entityRegistration.StreetName;
        //        }
        //    }
        //}
        //#endregion

    }
}