using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcessMultiVisitScheduleOrder : BasePageTrx
    {
        protected int PageCount = 1;
        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalDiagnostic.PROCESS_MULTI_VISIT_SCHEDULE_ORDER;
        }

        protected override void InitializeDataControl()
        {
            if (lstOrderMultiVisit != null)
            {
                lstOrderMultiVisit.Clear();
            }
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN));
            hdnPatientSearchDialogType.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN).FirstOrDefault().ParameterValue;

            txtFromOrderDate.Text = DateTime.Now.AddMonths(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToOrderDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND IsDeleted = 0 AND IsAllowMultiVisitSchedule = 1", Constant.Facility.DIAGNOSTIC));
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string periodeDate = string.Format("{0};{1}", Helper.GetDatePickerValue(txtFromOrderDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            lstOrderMultiVisit = BusinessLayer.GetListMultiVisitScheduleOrder(periodeDate, Convert.ToInt32(cboServiceUnit.Value.ToString()));
            if (!string.IsNullOrEmpty(hdnMRN.Value))
            {
                grdView.DataSource = lstOrderMultiVisit.Where(w => w.MRN == Convert.ToInt32(hdnMRN.Value)).GroupBy(g => g.FromTestOrderID).Select(s => s.FirstOrDefault()).OrderBy(o => o.PatientName).OrderByDescending(o => o.FromTestOrderID).ToList();
            }
            else
            {
                grdView.DataSource = lstOrderMultiVisit.GroupBy(g => g.FromTestOrderID).Select(s => s.FirstOrDefault()).OrderBy(o => o.PatientName).ToList();
            }
            grdView.DataBind();
            pageCount = lstOrderMultiVisit.OrderBy(lst => lst.ItemName1).ToList().Count() > 0 ? pageCount : 0;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetListMultiVisitScheduleOrder entity = e.Row.DataItem as GetListMultiVisitScheduleOrder;

                //if (!entity.IsInternalMedication)
                //{
                //    HtmlTableRow tr = (HtmlTableRow)e.Row.FindControl("trItemName");
                //    tr.Attributes.Add("class", "externalMedicationColor");
                //}
            }
        }

        protected void lvwViewDt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                MultiVisitSchedule obj = (MultiVisitSchedule)e.Item.DataItem;
                Repeater rptMedicationTimeDetail = (Repeater)e.Item.FindControl("rptMedicationTimeDetail");
                rptMedicationTimeDetail.DataSource = obj.LstOrderSchedule;
                rptMedicationTimeDetail.DataBind();
            }
        }

        #region Order Schedule
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<MultiVisitSchedule> lstPartial = new List<MultiVisitSchedule>();
            int count = lstOrderMultiVisit.Where(w => w.MRN == Convert.ToInt32(hdnMRN.Value) && w.FromTestOrderID == Convert.ToInt32(hdnTestOrderID.Value)).GroupBy(g => g.SequenceNo).ToList().Count();
            for (int i = 0; i < count; i++)
            {
                //set column
                List<String> lstItemName = lstOrderMultiVisit.Where(w => w.MRN == Convert.ToInt32(hdnMRN.Value) && w.FromTestOrderID == Convert.ToInt32(hdnTestOrderID.Value)).Select(o => o.ItemName1).Distinct().ToList();

                #region Set Header
                List<ScheduleHeader> lstHeader = new List<ScheduleHeader>();
                foreach (String String in lstItemName)
                {
                    lstHeader.Add(new ScheduleHeader() { ItemName = String });
                }

                rptItemNameHeader.DataSource = lstHeader;
                rptItemNameHeader.DataBind();
                #endregion

                MultiVisitSchedule entityPartial = new MultiVisitSchedule();
                List<GetListMultiVisitScheduleOrder> lstTemp = new List<GetListMultiVisitScheduleOrder>();
                foreach (string name in lstItemName)
                {
                    GetListMultiVisitScheduleOrder oSchedule = lstOrderMultiVisit.Where(w => w.MRN == Convert.ToInt32(hdnMRN.Value) && w.FromTestOrderID == Convert.ToInt32(hdnTestOrderID.Value) && w.ItemName1 == name && w.SequenceNo == (i + 1).ToString()).FirstOrDefault();
                    if (oSchedule != null)
                    {
                        lstTemp.Add(oSchedule);
                    }
                    else
                    {
                        GetListMultiVisitScheduleOrder empty = new GetListMultiVisitScheduleOrder()
                        {
                            ID = 0,
                            FromTestOrderID = 0,
                            FromTestOrderNo = "",
                            MRN = 0,
                            MedicalNo = "",
                            PatientName = "",
                            SequenceNo = "",
                            ItemID = 0,
                            ItemCode = "",
                            ItemName1 = "",
                            ScheduledDate = DateTime.Now,
                            GCDiagnosticScheduleStatus = "",
                            DiagnosticScheduleStatus = "EMPTY",
                            AppointmentID = 0,
                            AppointmentNo = "",
                            AppointmentStartDate = DateTime.Now,
                            RealDate = DateTime.Now,
                            IsDeleted = false,
                            CreatedBy = 0,
                            LastUpdatedBy = 0,
                            LastUpdatedDate = DateTime.Now
                        };
                        lstTemp.Add(empty);
                    }
                }

                entityPartial.SequenceNo = i + 1;
                entityPartial.LstOrderSchedule = lstTemp;
                lstPartial.Add(entityPartial);
            }
            lvwViewDt.DataSource = lstPartial;
            lvwViewDt.DataBind();
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "updateResult")
                {
                    int id = Convert.ToInt32(param[1]);
                    string gcDiagnosticStatus = param[2];
                    DateTime scheduleDate = Helper.GetDatePickerValue(param[3]);
                    int apmID = Convert.ToInt32(param[4]);
                    GetListMultiVisitScheduleOrder updateData = lstOrderMultiVisit.Where(w => w.ID == Convert.ToInt32(id)).FirstOrDefault();
                    updateData.GCDiagnosticScheduleStatus = gcDiagnosticStatus;
                    switch (gcDiagnosticStatus)
                    {
                        case Constant.DiagnosticVisitScheduleStatus.OPEN:
                            updateData.DiagnosticScheduleStatus = "OPEN";
                            break;
                        case Constant.DiagnosticVisitScheduleStatus.STARTED:
                            updateData.DiagnosticScheduleStatus = "STARTED";
                            break;
                        default :
                            updateData.DiagnosticScheduleStatus = "COMPLETED";
                            break;
                    }
                    
                    updateData.ScheduledDate = scheduleDate;
                    updateData.AppointmentID = apmID;
                    lstOrderMultiVisit.Remove(lstOrderMultiVisit.Where(w => w.ID == Convert.ToInt32(id)).FirstOrDefault());
                    lstOrderMultiVisit.Add(updateData);
                    BindGridViewDt(1, false, ref pageCount);
                }
                else if (param[0] == "print")
                {
                    vDiagnosticVisitSchedule dvs = BusinessLayer.GetvDiagnosticVisitScheduleList(string.Format("ID = {0}", param[1])).FirstOrDefault();
                    if (dvs != null)
                    {
                        vAppointment apm = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", dvs.AppointmentID)).FirstOrDefault();
                        if (apm != null)
                        {
                            try
                            {
                                Healthcare h = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                                if (h != null)
                                {
                                    if (h.Initial == "RSSY")
                                    {
                                        //Get Printer Address
                                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                                        string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                                            ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN_PERJANJIAN);
                                        PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                                        if (oPrinter != null)
                                        {
                                            ZebraPrinting.PrintBuktiPerjanjianRSSY(apm, oPrinter.PrinterName);
                                        }
                                        else
                                        {
                                            errMessage = "No printing configuration for IP Address " + ipAddress;
                                        }
                                    }
                                    else
                                    {
                                        //Get Printer Address
                                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                                        string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                                            ipAddress, Constant.DirectPrintType.BUKTI_PERJANJIAN_MULTI_KUNJUNGAN);
                                        PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                                        if (oPrinter != null)
                                        {
                                            ZebraPrinting.PrintBuktiPerjanjianMultiKunjungan(dvs, apm, oPrinter.PrinterName);
                                        }
                                        else
                                        {
                                            errMessage = "No printing configuration for IP Address " + ipAddress;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Helper.InsertErrorLog(ex);
                                errMessage = ex.Message;
                            }
                        }
                    }
                    BindGridViewDt(1, true, ref pageCount);
                }
                else // refresh
                {
                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            try
            {

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            finally
            {

            }
            return result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        public class ScheduleHeader
        {
            public string ItemName { get; set; }
        }

        public class MultiVisitSchedule
        {
            private int _SequenceNo;

            public int SequenceNo
            {
                get { return _SequenceNo; }
                set { _SequenceNo = value; }
            }

            private int _ItemName;

            public int ItemName
            {
                get { return _ItemName; }
                set { _ItemName = value; }
            }

            private List<GetListMultiVisitScheduleOrder> _LstOrderSchedule;

            public List<GetListMultiVisitScheduleOrder> LstOrderSchedule
            {
                get { return _LstOrderSchedule; }
                set { _LstOrderSchedule = value; }
            }
        }

        public List<GetListMultiVisitScheduleOrder> lstOrderMultiVisit
        {
            get
            {
                if (Session["__lstOrderMultiVisitDetail"] == null)
                    Session["__lstOrderMultiVisitDetail"] = new List<GetListMultiVisitScheduleOrder>();

                return (List<GetListMultiVisitScheduleOrder>)Session["__lstOrderMultiVisitDetail"];
            }
            set { Session["__lstOrderMultiVisitDetail"] = value; }
        }
    }
}