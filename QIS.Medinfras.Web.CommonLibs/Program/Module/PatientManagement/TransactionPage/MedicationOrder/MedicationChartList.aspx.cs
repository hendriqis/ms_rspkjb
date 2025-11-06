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
    public partial class MedicationChartList : BasePageTrx
    {
        protected int PageCount = 1;
        private string refreshGridInterval = "10";

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();

            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "All Medication", Value = "0" }
                , new Variable() { Code = "Current Medication", Value = "1" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "1";

            string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsInpatientDispensary = 1 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);
            List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            if (lstHServiceUnit.Count > 0)
            {
                hdnDispensaryServiceUnitID.Value = Convert.ToString(lstHServiceUnit[0].HealthcareServiceUnitID);
            }

            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL, Constant.SettingParameter.PH_SEQUENCE_FOR_MEDICATION_LIST, Constant.SettingParameter.NR0001));
            refreshGridInterval = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
            hdnSequenceForMedicationList.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_SEQUENCE_FOR_MEDICATION_LIST).ParameterValue;
            hdnParamNR0001.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NR0001).ParameterValue;

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            return filterExpression;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";//hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            String TransactionStatus = String.Format("'{0}','{1}'", Constant.TestOrderStatus.OPEN, Constant.TestOrderStatus.CANCELLED);
            filterExpression += string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);

            string isUsingUDD = chkIsUsingUDD.Checked ? "1" : "0";

            List<MedicationChartItem> lstEntity = BusinessLayer.GetMedicationChartItemList(AppSession.RegisteredPatient.VisitID.ToString(), cboDisplay.Value.ToString(), isUsingUDD);
            lstEntity = lstEntity.OrderBy(lst => lst.DrugName).ToList();           
            grdView.DataSource = lstEntity;
            grdView.DataBind();
            pageCount = lstEntity.Count() > 0 ? pageCount : 0;
        }


        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MedicationChartItem entity = e.Row.DataItem as MedicationChartItem;

                if (!entity.IsInternalMedication)
                {
                    HtmlTableRow tr = (HtmlTableRow)e.Row.FindControl("trItemName");
                    tr.Attributes.Add("class", "externalMedicationColor");
                }
            }
        }

        protected void lvwViewDt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PartialMedicationSchedule obj = (PartialMedicationSchedule)e.Item.DataItem;
                Repeater rptMedicationTimeDetail = (Repeater)e.Item.FindControl("rptMedicationTimeDetail");
                rptMedicationTimeDetail.DataSource = obj.LstMedicationSchedule;
                rptMedicationTimeDetail.DataBind();
            }
        }

        #region Medication Schedule
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderDtID.Value != "" && hdnPrescriptionOrderDtID.Value != "0")
            {
                filterExpression = string.Format("VisitID = {0} AND PrescriptionOrderDetailID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnPrescriptionOrderDtID.Value);
            }
            else if (hdnPastMedicationID.Value != "" && hdnPastMedicationID.Value != "0")
            {
                filterExpression = string.Format("VisitID = {0} AND PastMedicationID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnPastMedicationID.Value);
            }

            List<vMedicationSchedule> lstEntity = BusinessLayer.GetvMedicationScheduleList(filterExpression, Constant.GridViewPageSize.GRID_TEMP_MAX_5000, pageIndex, "MedicationDate ASC");
            List<DateTime> lstDate = lstEntity.Select(o => o.MedicationDate).Distinct().ToList();

            if (hdnSequenceForMedicationList.Value != "0")
            {
                List<DateTime> lstFinal = new List<DateTime>();

                Int32 duration = Convert.ToInt32(hdnSequenceForMedicationList.Value) + 1;
                List<DateTime> recentDateBefore = lstDate.Where(t => t.Date <= DateTime.Now).ToList();
                recentDateBefore = recentDateBefore.OrderByDescending(x => x.Date).Take(duration).ToList();

                List<DateTime> recentDateAfter = lstDate.Where(t => t.Date > DateTime.Now).ToList();
                recentDateAfter = recentDateAfter.OrderBy(x => x.Date).Take(Convert.ToInt32(hdnSequenceForMedicationList.Value)).ToList();

                lstFinal = recentDateBefore;
                lstFinal.AddRange(recentDateAfter);

                lstFinal = lstFinal.OrderBy(x => x.Date).ToList();
                lstDate = lstFinal;
            }

            List<ScheduleHeader> lstHeader = new List<ScheduleHeader>();
            foreach (DateTime date in lstDate)
            {
                lstHeader.Add(new ScheduleHeader() { MedicationDate = date.ToString("dd-MMM") });
            }

            rptMedicationDateHeader.DataSource = lstHeader;
            rptMedicationDateHeader.DataBind();


            List<PartialMedicationSchedule> lstPartial = new List<PartialMedicationSchedule>();
            for (int i = 1; i <= 6; i++)
            {
                PartialMedicationSchedule entityPartial = new PartialMedicationSchedule();
                List<TempMedicationSchedule> lstTemp = new List<TempMedicationSchedule>();
                foreach (DateTime scheduleDate in lstDate)
                {
                    vMedicationSchedule oSchedule = lstEntity.Where(lst => lst.MedicationDate == scheduleDate && lst.SequenceNo == i.ToString()).FirstOrDefault();
                    TempMedicationSchedule newTemp = new TempMedicationSchedule();
                    if (oSchedule != null)
                    {
                        newTemp.SequenceNo = oSchedule.SequenceNo;
                        newTemp.MedicationDate = oSchedule.MedicationDate;
                        newTemp.MedicationTime = oSchedule.cfMedicationTime;
                        newTemp.PrescriptionOrderDetailID = oSchedule.PrescriptionOrderDetailID;
                        newTemp.ID = oSchedule.ID;
                        newTemp.ReferenceNo = oSchedule.ReferenceNo;
                        newTemp.GCMedicationStatus = oSchedule.GCMedicationStatus;
                        newTemp.IsEditable = oSchedule.MedicationDate <= DateTime.Now.Date ? true : false;
                        newTemp.IsHomeMedication = oSchedule.PrescriptionOrderDetailID == 0 || oSchedule.PrescriptionOrderDetailID == null;
                        newTemp.MedicationDisplayStatus = "OPEN";
                    }
                    else
                    {
                        newTemp.SequenceNo = i.ToString();
                        newTemp.MedicationDate = DateTime.MinValue;
                        newTemp.MedicationTime = "-";
                        newTemp.PrescriptionOrderDetailID = 0;
                        newTemp.ID = 0;
                        newTemp.ReferenceNo = string.Empty;
                        newTemp.GCMedicationStatus = Constant.MedicationStatus.EMPTY;
                        newTemp.IsEditable = false;
                        newTemp.IsHomeMedication = false;
                        newTemp.MedicationDisplayStatus = "OPEN";
                    }

                    switch (newTemp.GCMedicationStatus)
                    {
                        case Constant.MedicationStatus.OPEN:
                            newTemp.MedicationDisplayStatus = "OPEN";
                            break;
                        case Constant.MedicationStatus.DIPROSES_FARMASI:
                            if (newTemp.MedicationDate == DateTime.Now.Date)
                            {
                                string medicationTime = "00:00";
                                if (newTemp.MedicationTime != "-")
                                {
                                    medicationTime = newTemp.MedicationTime;
                                }
                                var t1 = TimeSpan.Parse(medicationTime);
                                var t2 = TimeSpan.Parse(DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

                                if (t2 > t1)
                                    newTemp.MedicationDisplayStatus = "OVERDUE";
                                else
                                    newTemp.MedicationDisplayStatus = "SCHEDULE";
                            }
                            else if (newTemp.MedicationDate < DateTime.Now.Date)
                            {
                                newTemp.MedicationDisplayStatus = "OVERDUE";
                            }
                            else
                            {
                                newTemp.MedicationDisplayStatus = "SCHEDULE";
                            }
                            break;
                        case Constant.MedicationStatus.DISCONTINUE:
                            newTemp.MedicationDisplayStatus = "DISCONTINUE";
                            break;
                        case Constant.MedicationStatus.TELAH_DIBERIKAN:
                            newTemp.MedicationDisplayStatus = "TAKEN";
                            break;
                        case Constant.MedicationStatus.PASIEN_MENOLAK:
                            newTemp.MedicationDisplayStatus = "REFUSED";
                            break;
                        case Constant.MedicationStatus.PASIEN_ABSEN:
                            newTemp.MedicationDisplayStatus = "PENDING";
                            break;
                        case Constant.MedicationStatus.PASIEN_PUASA:
                            newTemp.MedicationDisplayStatus = "PENDING";
                            break;
                        case Constant.MedicationStatus.DI_TUNDA:
                            newTemp.MedicationDisplayStatus = "PENDING";
                            break;
                        default:
                            newTemp.MedicationDisplayStatus = "EMPTY";
                            break;
                    }

                    lstTemp.Add(newTemp);
                }

                entityPartial.SequenceNo = i;
                entityPartial.LstMedicationSchedule = lstTemp;
                lstPartial.Add(entityPartial);
            }
            lvwViewDt.DataSource = lstPartial;
            lvwViewDt.DataBind();
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.MEDICATION_CHART_LIST;
        }

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
            public string MedicationDate { get; set; }
        }

        public class PartialMedicationSchedule
        {
            private int _SequenceNo;

            public int SequenceNo
            {
                get { return _SequenceNo; }
                set { _SequenceNo = value; }
            }

            private List<TempMedicationSchedule> _LstMedicationSchedule;

            public List<TempMedicationSchedule> LstMedicationSchedule
            {
                get { return _LstMedicationSchedule; }
                set { _LstMedicationSchedule = value; }
            }
        }

        public partial class TempMedicationSchedule
        {
            public Int32 ID { get; set; }
            public Int32 PrescriptionOrderDetailID { get; set; }
            public DateTime MedicationDate { get; set; }
            public string MedicationTime { get; set; }
            public string SequenceNo { get; set; }
            public string ReferenceNo { get; set; }
            public string GCMedicationStatus { get; set; }
            public string MedicationDisplayStatus { get; set; }
            public Boolean IsHomeMedication { get; set; }

            private Boolean _IsEditable;
            public Boolean IsEditable
            {
                get { return _IsEditable; }
                set { _IsEditable = value; }
            }

            public String cfMedicationDate1
            {
                get
                {
                    return MedicationDate.ToString("dd-MMM");
                }
            }

            public Boolean cfIsNeedToReturn
            {
                get
                {
                    if (GCMedicationStatus == Constant.MedicationStatus.DISCONTINUE && !String.IsNullOrEmpty(ReferenceNo))
                        return true;
                    else
                        return false;
                }
            }

        }
    }
}