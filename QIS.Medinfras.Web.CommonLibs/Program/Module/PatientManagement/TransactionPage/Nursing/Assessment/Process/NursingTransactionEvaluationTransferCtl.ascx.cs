using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingTransactionEvaluationTransferCtl : BaseUserControlCtl
    {
        private string[] lstNursingDiagnoseItemIndicatorID1 = null;
        private string[] lstScaleScore1 = null;
        private string[] lstRemarks1 = null;

        private NursingProcessTransfer MainPage
        {
            get { return (NursingProcessTransfer)Page; }
        }


        protected string OnGetDiagnoseFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }

        private string GetFilterExpressionDiagnoseItem()
        {
            string filterExpression = String.Empty;
            if (hdnID1.Value != String.Empty)
                filterExpression += String.Format("NursingDiagnoseItemID = {0} AND NursingDiagnoseItemIndicatorID IN (SELECT NursingDiagnoseItemIndicatorID FROM NursingTransactionOutcomeDt WHERE TransactionID = {1} AND NursingDiagnoseItemID = {0})", hdnID1.Value, MainPage.GetNursingTransactionID());
            else
                filterExpression += "1=0";

            return filterExpression;
        }

        private void BindGridView()
        {
            String filter = string.Format("TransactionID = {0} AND IsUsingIndicator = 1 AND NursingDiagnoseID = {1}", MainPage.GetNursingTransactionID(), MainPage.GetNursingDiagnoseID());
            List<vNursingTransactionDt> lstEntity = BusinessLayer.GetvNursingTransactionDtList(filter);
            grdViewOutcomeEvaluation.DataSource = lstEntity;
            grdViewOutcomeEvaluation.DataBind();
        }

        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionDiagnoseItem();
            lstNursingDiagnoseItemIndicatorID1 = hdnListNursingDiagnoseItemIndicatorID1.Value.Substring(1).Split('|');
            lstScaleScore1 = hdnListScaleScore1.Value.Substring(1).Split('|');
            lstRemarks1 = hdnListRemarks1.Value.Substring(1).Split('|');
            List<vNursingDiagnoseItemIndicator> lstEntity = BusinessLayer.GetvNursingDiagnoseItemIndicatorList(filterExpression);
            grdViewOutcomeEvaluation1.DataSource = lstEntity;
            grdViewOutcomeEvaluation1.DataBind();

        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionDt entity = e.Row.DataItem as vNursingTransactionDt;
                
                Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
                int count = BusinessLayer.GetvNursingTransactionOutcomeDtList(String.Format("NursingDiagnoseItemID = {0} AND TransactionID = {1} AND NursingDiagnoseID = {2}", entity.NursingDiagnoseItemID, MainPage.GetNursingTransactionID(),MainPage.GetNursingDiagnoseID())).Count;
                lblTotalSelected.Text = count.ToString();
            }
        }

        protected void grdView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingDiagnoseItemIndicator entity = e.Row.DataItem as vNursingDiagnoseItemIndicator;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelectedEvaluationOutcome");
                RadioButton rbtScaleEvalution1 = (RadioButton)e.Row.FindControl("rbtScaleEvalution1Text");
                RadioButton rbtScaleEvalution2 = (RadioButton)e.Row.FindControl("rbtScaleEvalution2Text");
                RadioButton rbtScaleEvalution3 = (RadioButton)e.Row.FindControl("rbtScaleEvalution3Text");
                RadioButton rbtScaleEvalution4 = (RadioButton)e.Row.FindControl("rbtScaleEvalution4Text");
                RadioButton rbtScaleEvalution5 = (RadioButton)e.Row.FindControl("rbtScaleEvalution5Text");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarksEvaluationOutcome");
                if (lstNursingDiagnoseItemIndicatorID1.Contains(entity.NursingDiagnoseItemIndicatorID.ToString()))
                {
                    int idx = Array.IndexOf(lstNursingDiagnoseItemIndicatorID1, entity.NursingDiagnoseItemIndicatorID.ToString());
                    chkIsSelected.Checked = true;
                    switch (lstScaleScore1[idx])
                    {
                        case "1" : rbtScaleEvalution1.Checked = true; break;
                        case "2" : rbtScaleEvalution2.Checked = true; break;
                        case "3" : rbtScaleEvalution3.Checked = true; break;
                        case "4" : rbtScaleEvalution4.Checked = true; break;
                        case "5" : rbtScaleEvalution5.Checked = true; break;
                    }
                    txtRemarks.Text = lstRemarks1[idx];
                }
            }
        }

        protected void cbpViewOutcomeEvaluation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void cbpViewOutcomeEvaluation1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(MainPage.GetNursingTransactionID());
            string hdnEvaluation = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    MainPage.SaveHeaderFromUserControl();

                    IDbContext ctx = DbFactory.Configure(true);
                    try
                    {
                        hdnEvaluation = SaveNursingTransactionDt(ctx, transactionID, ref errMessage);
                        ctx.CommitTransaction();
                    }
                    catch
                    {
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }

                    BindGridViewDiagnoseItem();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = MainPage.GetNursingTransactionID();
            panel.JSProperties["cpEvaluation"] = hdnEvaluation;
        }

        #region Public Function
        public string SaveNursingTransactionDt(IDbContext ctx, Int32 transactionID, ref string errMessage)
        {           
            NursingTransactionOutcomeDtDao entityNursingTransactionDtDao = new NursingTransactionOutcomeDtDao(ctx);
            
            String[] paramNursingDiagnoseItemIndicatorID = hdnListNursingDiagnoseItemIndicatorID1.Value.Substring(1).Split('|');
            String[] paramScaleScore = hdnListScaleScore1.Value.Substring(1).Split('|');
            String[] paramRemarks = hdnListRemarks1.Value.Substring(1).Split('|');


            List<NursingTransactionOutcomeDt> lstEntityNursingTransDtBeforeSave = BusinessLayer.GetNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}", transactionID), ctx);
            List<NursingTransactionOutcomeDt> lstEntityNursingTransDtToDelete = lstEntityNursingTransDtBeforeSave.AsEnumerable().Where(p => !paramNursingDiagnoseItemIndicatorID.Contains(p.NursingDiagnoseItemIndicatorID.ToString())).ToList();

             #region Delete
            foreach (NursingTransactionOutcomeDt entityDt in lstEntityNursingTransDtToDelete)
            {
                entityNursingTransactionDtDao.Delete(entityDt.ID);
                lstEntityNursingTransDtBeforeSave.Remove(entityDt);
            }
            #endregion

            for (int i = 0; i < paramNursingDiagnoseItemIndicatorID.Count(); i++)
            {
                NursingTransactionOutcomeDt entityDt = lstEntityNursingTransDtBeforeSave.FirstOrDefault(p => p.NursingDiagnoseItemIndicatorID.ToString() == paramNursingDiagnoseItemIndicatorID[i]);
                if (entityDt != null)
                {
                    #region Edit
                    entityDt.ScaleScoreEvaluation = Convert.ToInt32(paramScaleScore[i]);
                    entityDt.Remarks = paramRemarks[i];
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityNursingTransactionDtDao.Update(entityDt);
                    #endregion
                }
                else
                {
                    #region Add
                    if (paramNursingDiagnoseItemIndicatorID[i] != "")
                    {

                        entityDt = new NursingTransactionOutcomeDt();
                        entityDt.TransactionID = transactionID;
                        entityDt.NursingDiagnoseItemID = Convert.ToInt32(hdnOldID1.Value);
                        entityDt.NursingDiagnoseItemIndicatorID = Convert.ToInt32(paramNursingDiagnoseItemIndicatorID[i]);
                        entityDt.ScaleScoreEvaluation = Convert.ToInt32(paramScaleScore[i]);
                        entityDt.Remarks = paramRemarks[i];
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityNursingTransactionDtDao.Insert(entityDt);
                    }
                    #endregion
                }
            }

            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            NursingTransactionHd entity = BusinessLayer.GetNursingTransactionHdList(String.Format("TransactionID = {0}", transactionID), ctx).FirstOrDefault();
            entity.NOCInterval = Convert.ToInt32(txtNOCInterval.Text);
            entity.GCNOCIntervalPeriod = cboIntervalType.Value.ToString();
            entity.SubjectiveText = hdnProblemName.Value;
            entity.IsProblemSolved = rblProblemStatus.SelectedValue == "1" ? true : false;
            entityHdDao.Update(entity);

            return MainPage.LoadNursingEvaluationFromNursingDiagnoseItem(ctx);
            
        }

        public void LoadGridViewByHeaderEntity(Int32 transactionID)
        {
            hdnTransactionID1.Value = transactionID.ToString();

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.NOC_Interval_Period);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboIntervalType, lstSc.Where(p => p.ParentID == Constant.StandardCode.NOC_Interval_Period).ToList(), "StandardCodeName", "StandardCodeID");

            List<NursingTransactionOutcomeDt> lstSelectedItem = BusinessLayer.GetNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}", transactionID));
            List<string> lstSelectedNursingDiagnoseItemIndicatorID = new List<string>();
            List<string> lstSelectedScaleScore = new List<string>();
            List<string> lstSelectedRemarks = new List<string>();
            for (int i = 0; i < lstSelectedItem.Count; i++)
            {
                lstSelectedNursingDiagnoseItemIndicatorID.Add(lstSelectedItem[i].NursingDiagnoseItemIndicatorID.ToString());
                if (lstSelectedItem[i].ScaleScoreEvaluation.ToString() == "0")
                    lstSelectedScaleScore.Add(lstSelectedItem[i].ScaleScore.ToString());
                else
                    lstSelectedScaleScore.Add(lstSelectedItem[i].ScaleScoreEvaluation.ToString());

                lstSelectedRemarks.Add(lstSelectedItem[i].Remarks);
            }
            hdnListNursingDiagnoseItemIndicatorID1.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemIndicatorID.ToArray());
            hdnListScaleScore1.Value = "|" + string.Join("|", lstSelectedScaleScore.ToArray());
            hdnListRemarks1.Value = "|" + string.Join("|", lstSelectedRemarks.ToArray());

            vNursingTransactionHd entity = BusinessLayer.GetvNursingTransactionHd(string.Format("TransactionID = {0}",transactionID),0);
            if (entity != null)
            {
                hdnProblemName.Value = entity.ProblemName;
                txtNOCInterval.Text = entity.NOCInterval.ToString();
                cboIntervalType.Value = entity.GCNOCIntervalPeriod;
                rblProblemStatus.SelectedValue = entity.IsProblemSolved ? "1" : "0";
            }
            else
            {
                hdnProblemName.Value = "";
                txtNOCInterval.Text = "0"; 
                cboIntervalType.Value = "X431^003";
                rblProblemStatus.SelectedValue = "0";
            }


            BindGridView();
            BindGridViewDiagnoseItem();
        }

        public string SetHiddenFieldWithinTransaction(Int32 transactionID, IDbContext ctx)
        {
            List<NursingTransactionEvaluationDt> lstSelectedItem = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("TransactionID = {0}", MainPage.GetNursingTransactionID()), ctx);
            List<string> lstSelectedNursingDiagnoseItemID = new List<string>();
            List<string> lstSelectedNursingTransactionDtID = new List<string>();
            List<string> lstSelectedNursingDiagnoseItemText = new List<string>();
            List<string> lstSelectedIsEditedByUser = new List<string>();
            for (int i = 0; i < lstSelectedItem.Count; i++)
            {
                lstSelectedNursingDiagnoseItemID.Add(lstSelectedItem[i].NursingDiagnoseItemID.ToString());
                if (lstSelectedItem[i].NursingDiagnoseItemID > 0)
                    lstSelectedNursingTransactionDtID.Add("0");
                else
                    lstSelectedNursingTransactionDtID.Add(lstSelectedItem[i].ID.ToString());
                lstSelectedNursingDiagnoseItemText.Add(lstSelectedItem[i].NursingItemText);
                lstSelectedIsEditedByUser.Add(lstSelectedItem[i].IsEditedByUser.ToString());
            }
            hdnListNursingEvaluationDiagnoseItemID1.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemID.ToArray());
            hdnListNursingTransactionEvaluationID1.Value = "|" + string.Join("|", lstSelectedNursingTransactionDtID.ToArray());
            hdnListNursingEvaluationDiagnoseItemText1.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemText.ToArray());
            hdnListIsEvaluationEditedByUser1.Value = "|" + string.Join("|", lstSelectedIsEditedByUser.ToArray());

            List<NursingTransactionOutcomeDt> lstSelectedOutcome = BusinessLayer.GetNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}", MainPage.GetNursingTransactionID()), ctx);
            List<string> lstSelectedNursingOutcomeID = new List<string>();
            List<string> lstSelectedScaleScoreEvaluation = new List<string>();
            for (int i = 0; i < lstSelectedOutcome.Count; i++)
            {
                lstSelectedNursingOutcomeID.Add(lstSelectedOutcome[i].ID.ToString());
                lstSelectedScaleScoreEvaluation.Add(lstSelectedOutcome[i].ScaleScoreEvaluation.ToString());
            }
            hdnListOutcomeDtID1.Value = "|" + string.Join("|", lstSelectedNursingOutcomeID.ToArray());
            hdnListScaleScoreEvaluation1.Value = "|" + string.Join("|", lstSelectedScaleScoreEvaluation.ToArray());

            string filterExpressionIntervention = String.Format("NursingTransactionInterventionHdID IN (SELECT ID FROM NursingTransactionInterventionHd WHERE TransactionID = {0})", MainPage.GetNursingTransactionID());
            List<NursingTransactionInterventionDt> lstSelectedIntervention = BusinessLayer.GetNursingTransactionInterventionDtList(filterExpressionIntervention, ctx);
            List<string> lstSelectedNursingInterventionDtID = new List<string>();
            List<string> lstSelectedIsContinue = new List<string>();
            for (int i = 0; i < lstSelectedIntervention.Count; i++)
            {
                lstSelectedNursingInterventionDtID.Add(lstSelectedIntervention[i].ID.ToString());
                lstSelectedIsContinue.Add(lstSelectedIntervention[i].IsContinued ? "1" : "0");
            }
            hdnListNursingInterventionEvaluation1.Value = "|" + string.Join("|", lstSelectedNursingInterventionDtID.ToArray());
            hdnListIsContinued1.Value = "|" + string.Join("|", lstSelectedIsContinue.ToArray());

            string retval = "";
            retval += hdnListNursingEvaluationDiagnoseItemID1.Value + "#";
            retval += hdnListNursingTransactionEvaluationID1.Value + "#";
            retval += hdnListNursingEvaluationDiagnoseItemText1.Value + "#";
            retval += hdnListIsEvaluationEditedByUser1.Value + "#";
            retval += hdnListOutcomeDtID1.Value + "#";
            retval += hdnListScaleScoreEvaluation1.Value + "#";
            retval += hdnListNursingInterventionEvaluation1.Value + "#";
            retval += hdnListIsContinued1.Value + "#";
            return retval;
        }
        #endregion
    }
}