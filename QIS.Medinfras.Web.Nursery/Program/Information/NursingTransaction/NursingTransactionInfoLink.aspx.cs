using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Program;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingTransactionInfoLink : BasePageTrx
    {
        protected int PageCount = 1;
        protected bool IsEditable = true;
        protected int diagnoseID;
        private int intSubTotalIndexNOC = 1;
        private int intSubTotalIndexNIC = 1;
        private string prevNOCHeader = String.Empty;
        private string prevNICHeader = String.Empty;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.INFO_NURSING_TRANSACTION;
        }

        protected override void InitializeDataControl()
        {
        }

        protected string GetGCNursingEvaluationAssessment()
        {
            return Constant.NursingEvaluation.ASSESSMENT;
        }

        protected string GetGCNursingEvaluationPlanning()
        {
            return Constant.NursingEvaluation.PLANNING;
        }

        #region Bind All Gridview
        private void BindGridView()
        {   
            List<GetNursingItemGroupSubGroupListByDiagnose> lstAllDiagnoseItem = BusinessLayer.GetNursingItemGroupSubGroupListByDiagnose(Convert.ToInt32(hdnDiagnoseID.Value));
            List<vNursingTransactionDt> lstSelectedDiagnoseItem = BusinessLayer.GetvNursingTransactionDtList(String.Format("TransactionID = {0}",hdnTransactionID.Value));
            List<GetNursingItemGroupSubGroupListByDiagnose> lstEntity = lstAllDiagnoseItem.Where(p => lstSelectedDiagnoseItem.Any(p1 => (p1.NursingItemGroupSubGroupID == p.NursingItemGroupSubGroupID) || (p1.NursingItemGroupSubGroupParentID == p.NursingItemGroupSubGroupID))).ToList();
            grdView.DataSource = lstEntity;
            grdView.DataBind();    
        }

        private void BindDiagnoseItemGridView()
        {
            List<vNursingTransactionDt> lstEntity = BusinessLayer.GetvNursingTransactionDtList(String.Format("TransactionID = {0} AND NursingItemGroupSubGroupID = {1}", hdnTransactionID.Value, hdnNursingItemGroupID.Value));
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();
        }

        private void BindOutcomeGridView()
        {
            List<vNursingTransactionDt> lstEntityOutcome = BusinessLayer.GetvNursingTransactionDtList(String.Format("TransactionID = {0} AND IsUsingIndicator = 1 AND NursingDiagnoseID = {1}", hdnTransactionID.Value, hdnDiagnoseID.Value));
            List<vNursingTransactionOutcomeDt> lstSelectedOutcomeDt = BusinessLayer.GetvNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}", hdnTransactionID.Value));
            List<vNursingTransactionDt> lstEntity = lstEntityOutcome.Where(p => lstSelectedOutcomeDt.Any(p1 => p1.NursingDiagnoseItemID == p.NursingDiagnoseItemID)).ToList();
            grdViewOutcome.DataSource = lstEntity;
            grdViewOutcome.DataBind();
        }

        private void BindOutcomeDtGridView()
        {
            string filterExpression = String.Empty;
            if (hdnNursingDiagnoseItemID.Value != String.Empty)
                filterExpression += String.Format("NursingDiagnoseItemID = {0} AND TransactionID = {1} AND NursingDiagnoseID = {2}", hdnNursingDiagnoseItemID.Value,hdnTransactionID.Value,hdnDiagnoseID.Value);
            else
                filterExpression += "1=0";
            List<vNursingTransactionOutcomeDt> lstEntity = BusinessLayer.GetvNursingTransactionOutcomeDtList(filterExpression);
            grdViewOutcome1.DataSource = lstEntity;
            grdViewOutcome1.DataBind();
        }

        private void BindInterventionGridView()
        {
            List<vNursingTransactionInterventionHd> lstEntity = BusinessLayer.GetvNursingTransactionInterventionHdList(String.Format("TransactionID = {0}", hdnTransactionID.Value));
            grdViewIntervention.DataSource = lstEntity;
            grdViewIntervention.DataBind();
        }

        private void BindInterventionDtGridView()
        {
            string filterExpression = String.Format("NursingTransactionInterventionHdID = {0}",hdnInterventionID.Value);
            List<vNursingTransactionInterventionDt> lstEntity = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);
            grdViewIntervention1.DataSource = lstEntity;
            grdViewIntervention1.DataBind();

        }

        private void BindImplementationGridView()
        {
            string filterExpression = String.Empty;
            if (hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
                filterExpression = String.Format("TransactionID = {0}", hdnTransactionID.Value);
            else
                filterExpression = "1 = 0";
            List<vNursingTransactionInterventionDt> lstEntity = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);
            grdViewImplementation.DataSource = lstEntity;
            grdViewImplementation.DataBind();
        }

        private void BindImplementation1GridView()
        {
            string filterExpression = String.Empty;
            if (hdnInterventionDtID.Value != "")
                filterExpression = String.Format("NursingTransactionInterventionDtID = {0}", hdnInterventionDtID.Value);
            else
                filterExpression = "1 = 0";
            List<vNursingJournal> lstEntity = BusinessLayer.GetvNursingJournalList(filterExpression);
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();
        }

        private void BindEvaluationGridView()
        {
            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}'", Constant.StandardCode.NURSING_EVALUATION));
            grdViewEvaluation.DataSource = lstEntity;
            grdViewEvaluation.DataBind();
        }

        private void BindSubjectiveObjectiveGridView()
        {
            string filterExpression = String.Empty;
            if (hdnTransactionID.Value != String.Empty)
                filterExpression = String.Format("TransactionID = {0}", hdnTransactionID.Value);
            else
                filterExpression = "1=0";
            filterExpression += String.Format(" AND GCNursingEvaluation = '{0}'",hdnGCEvaluationType.Value);
            List<vNursingTransactionEvaluationDt> lstEntity = BusinessLayer.GetvNursingTransactionEvaluationDtList(filterExpression);
            grdViewSubjectiveObjective.DataSource = lstEntity;
            grdViewSubjectiveObjective.DataBind();
        }

        private void BindGridViewAssessment()
        {
            string filterExpression = String.Format("TransactionID = {0} ORDER BY NursingDiagnoseItemID", hdnTransactionID.Value);
            List<vNursingTransactionOutcomeDt> lstEntity = BusinessLayer.GetvNursingTransactionOutcomeDtList(filterExpression);
            grdViewAssessment.DataSource = lstEntity;
            grdViewAssessment.DataBind();

        }

        private void BindGridViewPlanning()
        {
            string filterExpression = String.Format("TransactionID = {0} ORDER BY NursingInterventionID", hdnTransactionID.Value);
            List<vNursingTransactionInterventionDt> lstEntity = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);
            grdViewPlanning.DataSource = lstEntity;
            grdViewPlanning.DataBind();

        }

        #endregion

        #region Gridview Row Data Bound
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetNursingItemGroupSubGroupListByDiagnose entity = e.Row.DataItem as GetNursingItemGroupSubGroupListByDiagnose;
                if (!entity.IsHeader)
                {
                    Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
                    int count = BusinessLayer.GetvNursingTransactionDtList(String.Format("NursingItemGroupSubGroupID = {0} AND TransactionID = {1} AND NursingDiagnoseID = {2}", entity.NursingItemGroupSubGroupID, hdnTransactionID.Value, hdnDiagnoseID.Value)).Count;
                    lblTotalSelected.Text = count.ToString();
                }
            }
        }

        protected void grdViewOutcome_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionDt entity = e.Row.DataItem as vNursingTransactionDt;

                Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
                int count = BusinessLayer.GetvNursingTransactionOutcomeDtList(String.Format("NursingDiagnoseItemID = {0} AND TransactionID = {1} AND NursingDiagnoseID = {2}", entity.NursingDiagnoseItemID, hdnTransactionID.Value, hdnDiagnoseID.Value)).Count;
                lblTotalSelected.Text = count.ToString();
            }
        }

        protected void grdViewOutcome1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionOutcomeDt entity = e.Row.DataItem as vNursingTransactionOutcomeDt;
                RadioButton rbtScale1 = (RadioButton)e.Row.FindControl("rbtScale1Text");
                RadioButton rbtScale2 = (RadioButton)e.Row.FindControl("rbtScale2Text");
                RadioButton rbtScale3 = (RadioButton)e.Row.FindControl("rbtScale3Text");
                RadioButton rbtScale4 = (RadioButton)e.Row.FindControl("rbtScale4Text");
                RadioButton rbtScale5 = (RadioButton)e.Row.FindControl("rbtScale5Text");
                switch (entity.ScaleScore)
                {
                    case 1: rbtScale1.Checked = true; break;
                    case 2: rbtScale2.Checked = true; break;
                    case 3: rbtScale3.Checked = true; break;
                    case 4: rbtScale4.Checked = true; break;
                    case 5: rbtScale5.Checked = true; break;
                }
            }
        }

        protected void grdViewIntervention_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionInterventionHd entity = e.Row.DataItem as vNursingTransactionInterventionHd;

                Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
                string filterExpression = String.Format("NursingInterventionID = {0} AND TransactionID = {1}", entity.NursingInterventionID, hdnTransactionID.Value);
                int count = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression).Count;
                lblTotalSelected.Text = count.ToString();

            }
        }

        protected void grdViewImplementation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionInterventionDt entity = e.Row.DataItem as vNursingTransactionInterventionDt;

                Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
                string filterExpression = String.Empty;
                if (entity.ID != 0)
                    filterExpression = String.Format("NursingTransactionInterventionDtID = {0} AND IsDeleted = 0", entity.ID);
                else
                    filterExpression = "1 = 0";
                int count = BusinessLayer.GetNursingJournalList(filterExpression).Count;
                lblTotalSelected.Text = count.ToString();
            }
        }

        protected void grdViewAssessment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionOutcomeDt entity = e.Row.DataItem as vNursingTransactionOutcomeDt;
                RadioButton rbtScale1 = (RadioButton)e.Row.FindControl("rbtScale1Text");
                RadioButton rbtScale2 = (RadioButton)e.Row.FindControl("rbtScale2Text");
                RadioButton rbtScale3 = (RadioButton)e.Row.FindControl("rbtScale3Text");
                RadioButton rbtScale4 = (RadioButton)e.Row.FindControl("rbtScale4Text");
                RadioButton rbtScale5 = (RadioButton)e.Row.FindControl("rbtScale5Text");

                prevNOCHeader = entity.NursingDiagnoseItemText;

                switch (entity.ScaleScoreEvaluation)
                {
                    case 1: rbtScale1.Checked = true; break;
                    case 2: rbtScale2.Checked = true; break;
                    case 3: rbtScale3.Checked = true; break;
                    case 4: rbtScale4.Checked = true; break;
                    case 5: rbtScale5.Checked = true; break;
                }
            }
        }

        protected void grdViewAssessment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string temp = DataBinder.Eval(e.Row.DataItem, "NursingDiagnoseItemText").ToString();
                if ((prevNOCHeader == string.Empty || prevNOCHeader != temp))
                {
                    GridView grdViewOrders = (GridView)sender;
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    TableCell cell = new TableCell();
                    cell.Text = DataBinder.Eval(e.Row.DataItem, "NursingDiagnoseItemText").ToString();
                    cell.ColumnSpan = 3;
                    cell.Attributes.Add("style", "background-color: #4682B4;color: #ffffff;font-weight: bold;");
                    row.Cells.Add(cell);
                    grdViewOrders.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndexNOC, row);
                    intSubTotalIndexNOC++;
                }
            }
        }

        protected void grdViewPlanning_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionInterventionDt entity = e.Row.DataItem as vNursingTransactionInterventionDt;
                RadioButton rbtContinue = (RadioButton)e.Row.FindControl("rbtContinue");
                RadioButton rbtNotContinue = (RadioButton)e.Row.FindControl("rbtNotContinue");

                prevNICHeader = entity.NurseInterventionName;

                if (entity.IsContinued)
                    rbtContinue.Checked = true;
                else
                    rbtNotContinue.Checked = true;
            }
        }

        protected void grdViewPlanning_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string temp = DataBinder.Eval(e.Row.DataItem, "NurseInterventionName").ToString();
                if ((prevNICHeader == string.Empty || prevNICHeader != temp))
                {
                    GridView grdViewOrders = (GridView)sender;
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    TableCell cell = new TableCell();
                    cell.Text = DataBinder.Eval(e.Row.DataItem, "NurseInterventionName").ToString();
                    cell.ColumnSpan = 3;
                    cell.Attributes.Add("style", "background-color: #4682B4;color: #ffffff;font-weight: bold;");
                    row.Cells.Add(cell);
                    grdViewOrders.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndexNIC, row);
                    intSubTotalIndexNIC++;
                }
            }
        }

        #endregion

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTransactionDate,new ControlEntrySetting(false,false));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(false, false));
            SetControlEntrySetting(txtDiagnoseName,new ControlEntrySetting(false,false));
            SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(false, false));
        }

        #region Callback
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpHeader_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                LoadHeader();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            
        }

        protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindDiagnoseItemGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewOutcome_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindOutcomeGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewOutcome1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindOutcomeDtGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewIntervention_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                if (param[0] == "refresh")
                {
                    BindInterventionGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewIntervention1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindInterventionDtGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewImplementation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                BindImplementationGridView();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewImplementation1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindImplementation1GridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewEvaluation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindEvaluationGridView();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        protected void cbpViewEvaluation1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindSubjectiveObjectiveGridView();
                    BindGridViewAssessment();
                    BindGridViewPlanning();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #endregion

        private void LoadHeader()
        {
            vInpatientPatientListLink entity = BusinessLayer.GetvInpatientPatientListLinkList(String.Format("RegistrationNo = '{0}'", txtRegistrationNo.Text)).FirstOrDefault();
            if (entity != null)
                ctlPatientBanner.InitializePatientBanner(entity);
            else
                ctlPatientBanner.InitializeEmptyInpatientPatientBanner();
        }
    }
}