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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class MedicationScheduleProcess : BasePageTrx
    {
        protected int PageCount = 1;
        private string pageTitle = string.Empty;

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            hdnDispensaryServiceUnitID.Value = param[2];

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT));
            hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";//hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            String TransactionStatus = String.Format("'{0}','{1}'", Constant.TestOrderStatus.OPEN, Constant.TestOrderStatus.CANCELLED);
            filterExpression += string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMedicationChartItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vMedicationChartItem> lstEntity = BusinessLayer.GetvMedicationChartItemList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DrugName");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            if (lstEntity.Count>0)
            {
                lvwView.SelectedIndex = 0; 
            }
        }


        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlTableCell tdIndicator = e.Item.FindControl("tdIndicator") as HtmlTableCell;
                vMedicationChartItem entity = e.Item.DataItem as vMedicationChartItem;
                if (entity.IsInternalMedication)
                {
                    tdIndicator.Style.Add("background-color", "#0066FF");
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
            if (hdnPrescriptionOrderDtID.Value != "")
            {
                filterExpression = string.Format("PrescriptionOrderDetailID = {0}", hdnPrescriptionOrderDtID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvMedicationScheduleRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vMedicationSchedule> lstEntity = BusinessLayer.GetvMedicationScheduleList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
            List<vMedicationSchedule> lstSequence1 = lstEntity.Where(t => t.SequenceNo == "1").OrderBy(t => t.MedicationDate).ToList();
            rptMedicationDateHeader.DataSource = lstSequence1;
            rptMedicationDateHeader.DataBind();

            List<PartialMedicationSchedule> lstPartial = new List<PartialMedicationSchedule>();
            for (int i = 1; i <= 6; i++)
            {
                PartialMedicationSchedule entityPartial = new PartialMedicationSchedule();
                List<vMedicationSchedule> lstEntityMedicationSchedule = lstEntity.Where(t => t.SequenceNo == i.ToString()).ToList();
                List<TempMedicationSchedule> lstTemp = new List<TempMedicationSchedule>();
                if (lstEntityMedicationSchedule.Count() == 0)
                {
                    for (int j = 0; j < lstSequence1.Count(); j++)
                    {
                        TempMedicationSchedule newTemp = new TempMedicationSchedule()
                        {
                            SequenceNo = j.ToString(),
                            MedicationDate = lstSequence1[j].MedicationDate,
                            MedicationTime = "__:__",
                            PrescriptionOrderDetailID = lstSequence1[j].PrescriptionOrderDetailID,
                            ID = 0,
                            IsEditable=false
                        };
                        lstTemp.Add(newTemp);
                    }
                }
                else
                {
                    for (int j = 0; j < lstEntityMedicationSchedule.Count(); j++)
                    {
                        TempMedicationSchedule newTemp = new TempMedicationSchedule()
                        {
                            SequenceNo = lstEntityMedicationSchedule[j].SequenceNo,
                            MedicationDate = lstEntityMedicationSchedule[j].MedicationDate,
                            MedicationTime = lstEntityMedicationSchedule[j].MedicationTime,
                            PrescriptionOrderDetailID = lstEntityMedicationSchedule[j].PrescriptionOrderDetailID,
                            ID = lstEntityMedicationSchedule[j].ID,
                            IsEditable = lstEntityMedicationSchedule[j].MedicationDate <= DateTime.Now.Date ? true : false
                        };
                        lstTemp.Add(newTemp);
                    }
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
            return pageTitle;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.MEDICATION_SCHEDULE_PROCESS;
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
            private Int32 _ID;
            private Int32 _PrescriptionOrderDetailID;
            private DateTime _MedicationDate;
            private String _MedicationTime;
            private String _SequenceNo;
            private Boolean _IsEditable;

            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            public Int32 PrescriptionOrderDetailID
            {
                get { return _PrescriptionOrderDetailID; }
                set { _PrescriptionOrderDetailID = value; }
            }
            public DateTime MedicationDate
            {
                get { return _MedicationDate; }
                set { _MedicationDate = value; }
            }
            public String MedicationTime
            {
                get { return _MedicationTime; }
                set { _MedicationTime = value; }
            }
            public String SequenceNo
            {
                get { return _SequenceNo; }
                set { _SequenceNo = value; }
            }

            public Boolean IsEditable
            {
                get { return _IsEditable; }
                set { _IsEditable = value; }
            }

            public String MedicationDateInString
            {
                get
                {
                    return _MedicationDate.ToString(Constant.FormatString.DATE_FORMAT);
                }
            }
        }
    }
}