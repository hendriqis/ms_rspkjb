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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingTransactionOutcomeTransferCtl : BaseUserControlCtl
    {
        private string[] lstNursingDiagnoseItemIndicatorID = null;
        private string[] lstScaleScore = null;
        private string[] lstRemarks = null;

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
            if (hdnID.Value != String.Empty)
                filterExpression += String.Format("NursingDiagnoseItemID = {0} ", hdnID.Value);
            else
                filterExpression += "1=0";
            return filterExpression;
        }

        private void BindGridView()
        {
            List<vNursingTransactionDt> lstEntity = BusinessLayer.GetvNursingTransactionDtList(String.Format("TransactionID = {0} AND IsUsingIndicator = 1 AND NursingDiagnoseID = {1}",MainPage.GetNursingTransactionID(),MainPage.GetNursingDiagnoseID()));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionDiagnoseItem();
            lstNursingDiagnoseItemIndicatorID = hdnListNursingDiagnoseItemIndicatorID.Value.Substring(1).Split('|');
            lstScaleScore = hdnListScaleScore.Value.Substring(1).Split('|');
            lstRemarks = hdnListRemarks.Value.Substring(1).Split('|');
            List<vNursingDiagnoseItemIndicator> lstEntity = BusinessLayer.GetvNursingDiagnoseItemIndicatorList(filterExpression);
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();

        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionDt entity = e.Row.DataItem as vNursingTransactionDt;
                
                Label lblTotalSelectedOutcomeTransfer = (Label)e.Row.FindControl("lblTotalSelectedOutcomeTransfer");
                int count = BusinessLayer.GetvNursingTransactionOutcomeDtList(String.Format("NursingDiagnoseItemID = {0} AND TransactionID = {1} AND NursingDiagnoseID = {2}", entity.NursingDiagnoseItemID, MainPage.GetNursingTransactionID(),MainPage.GetNursingDiagnoseID())).Count;
                lblTotalSelectedOutcomeTransfer.Text = count.ToString();
            }
        }

        protected void grdView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingDiagnoseItemIndicator entity = e.Row.DataItem as vNursingDiagnoseItemIndicator;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelectedOutcome");
                RadioButton rbtScale1 = (RadioButton)e.Row.FindControl("rbtScale1Text");
                RadioButton rbtScale2 = (RadioButton)e.Row.FindControl("rbtScale2Text");
                RadioButton rbtScale3 = (RadioButton)e.Row.FindControl("rbtScale3Text");
                RadioButton rbtScale4 = (RadioButton)e.Row.FindControl("rbtScale4Text");
                RadioButton rbtScale5 = (RadioButton)e.Row.FindControl("rbtScale5Text");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarksOutcome");
                if (lstNursingDiagnoseItemIndicatorID.Contains(entity.NursingDiagnoseItemIndicatorID.ToString()))
                {
                    int idx = Array.IndexOf(lstNursingDiagnoseItemIndicatorID, entity.NursingDiagnoseItemIndicatorID.ToString());
                    chkIsSelected.Checked = true;
                    switch (lstScaleScore[idx])
                    {
                        case "1" : rbtScale1.Checked = true; break;
                        case "2" : rbtScale2.Checked = true; break;
                        case "3" : rbtScale3.Checked = true; break;
                        case "4" : rbtScale4.Checked = true; break;
                        case "5" : rbtScale5.Checked = true; break;
                    }
                    txtRemarks.Text = lstRemarks[idx];
                }
            }
        }

        protected void cbpViewOutcome_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void cbpViewOutcome1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                        MainPage.SetChildrenControlHiddenField();
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
            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            NursingTransactionHd entity = BusinessLayer.GetNursingTransactionHdList(String.Format("TransactionID = {0}", transactionID),ctx).FirstOrDefault();
            entity.NOCInterval = Convert.ToInt32(txtNOCInterval.Text);
            entity.GCNOCIntervalPeriod = cboIntervalType.Value.ToString();
            entityHdDao.Update(entity);
            
            NursingTransactionOutcomeDtDao entityNursingTransactionDtDao = new NursingTransactionOutcomeDtDao(ctx);
            
            String[] paramNursingDiagnoseItemIndicatorID = hdnListNursingDiagnoseItemIndicatorID.Value.Substring(1).Split('|');
            String[] paramScaleScore = hdnListScaleScore.Value.Substring(1).Split('|');
            String[] paramRemarks = hdnListRemarks.Value.Substring(1).Split('|');


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
                    entityDt.ScaleScore = Convert.ToInt32(paramScaleScore[i]);
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
                        entityDt.NursingDiagnoseItemID = Convert.ToInt32(hdnOldID.Value);
                        entityDt.NursingDiagnoseItemIndicatorID = Convert.ToInt32(paramNursingDiagnoseItemIndicatorID[i]);
                        entityDt.ScaleScore = Convert.ToInt32(paramScaleScore[i]);
                        entityDt.Remarks = paramRemarks[i];
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityNursingTransactionDtDao.Insert(entityDt);
                    }
                    #endregion
                }
            }

            return MainPage.LoadNursingEvaluationFromNursingDiagnoseItem(ctx);
            
        }

        public void LoadGridViewByHeaderEntity(Int32 transactionID)
        {
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
                lstSelectedScaleScore.Add(lstSelectedItem[i].ScaleScore.ToString());
                lstSelectedRemarks.Add(lstSelectedItem[i].Remarks);
            }
            hdnListNursingDiagnoseItemIndicatorID.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemIndicatorID.ToArray());
            hdnListScaleScore.Value = "|" + string.Join("|", lstSelectedScaleScore.ToArray());
            hdnListRemarks.Value = "|" + string.Join("|", lstSelectedRemarks.ToArray());

            NursingTransactionHd entity = BusinessLayer.GetNursingTransactionHd(transactionID);
            if (entity != null)
            {
                txtNOCInterval.Text = entity.NOCInterval.ToString();
                cboIntervalType.Value = entity.GCNOCIntervalPeriod;
            }
            else
            {
                txtNOCInterval.Text = "0";
                cboIntervalType.Value = "X431^003";
            }

            BindGridView();
            BindGridViewDiagnoseItem();
        }
        #endregion
    }
}