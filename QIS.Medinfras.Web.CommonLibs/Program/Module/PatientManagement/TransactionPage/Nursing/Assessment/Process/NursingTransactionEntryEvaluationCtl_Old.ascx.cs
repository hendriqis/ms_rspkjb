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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingTransactionEntryEvaluationCtl : BaseUserControlCtl
    {
        private string[] lstNursingDiagnoseItemID = null;
        private string[] lstNursingTransactionDtID = null;
        private string[] lstNursingDiagnoseItemText = null;
        private string[] lstIsEditedByUser = null;
        private int intSubTotalIndexNOC = 1;
        private int intSubTotalIndexNIC = 1;
        private string prevNOCHeader = String.Empty;
        private string prevNICHeader = String.Empty;

        private NursingProcessEntry1 MainPage
        {
            get { return (NursingProcessEntry1)Page; }
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    //BindGridView();
        //    //BindGridViewDiagnoseItem();
        //    //BindGridViewAssessment();
        //    base.OnLoad(e);
        //}

        protected string OnGetDiagnoseFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }

        protected string GetGCNursingEvaluationAssessment()
        {
            return Constant.NursingEvaluation.ASSESSMENT;
        }

        protected string GetGCNursingEvaluationPlanning()
        {
            return Constant.NursingEvaluation.PLANNING;
        }

        private string GetFilterExpressionDiagnoseItem()
        {
            string filterExpression = String.Empty;
            if (MainPage.GetNursingDiagnoseID() != String.Empty && MainPage.GetNursingTransactionID() != String.Empty)
                filterExpression = String.Format("((NursingDiagnoseID = {0} AND TransactionID = 0) OR (NursingDiagnoseID = 0 AND TransactionID = {1})) ", MainPage.GetNursingDiagnoseID(),MainPage.GetNursingTransactionID());
            if (hdnID.Value != String.Empty)
                filterExpression += String.Format(" AND GCNursingEvaluation = '{0}' ", hdnID.Value);
            else
                filterExpression += " AND 1=0 ";
            if(filterExpression == String.Empty)
                filterExpression = "1=0";
            filterExpression += " AND IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView()
        {
            List<vNursingTransactionDt> lstEntity = BusinessLayer.GetvNursingTransactionDtList(String.Format("TransactionID = {0} AND IsUsingIndicator = 1 AND NursingDiagnoseID = {1}", MainPage.GetNursingTransactionID(), MainPage.GetNursingDiagnoseID()));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionDiagnoseItem();
            lstNursingDiagnoseItemID = hdnListSaveNursingEvaluationDiagnoseItemID.Value.Substring(1).Split('|');
            lstNursingTransactionDtID = hdnListSaveNursingTransactionEvaluationID.Value.Substring(1).Split('|');
            lstNursingDiagnoseItemText = hdnListSaveNursingEvaluationDiagnoseItemText.Value.Substring(1).Split('|');
            lstIsEditedByUser = hdnListSaveIsEvaluationEditedByUser.Value.Substring(1).Split('|');
            List<vNursingDiagnoseItemForEvaluation> lstEntity = BusinessLayer.GetvNursingDiagnoseItemForEvaluationList(filterExpression);
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();

        }

        private void BindGridViewAssessment()
        {
            string filterExpression = String.Format("TransactionID = {0} ORDER BY NursingDiagnoseItemID", MainPage.GetNursingTransactionID());
            List<vNursingTransactionOutcomeDt> lstEntity = BusinessLayer.GetvNursingTransactionOutcomeDtList(filterExpression);
            grdViewAssessment.DataSource = lstEntity;
            grdViewAssessment.DataBind();

        }

        private void BindGridViewPlanning()
        {
            string filterExpression = String.Format("TransactionID = {0} ORDER BY NursingInterventionID", MainPage.GetNursingTransactionID());
            List<vNursingTransactionInterventionDt> lstEntity = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);
            grdViewPlanning.DataSource = lstEntity;
            grdViewPlanning.DataBind();

        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    GetNursingItemGroupSubGroupListByDiagnose entity = e.Row.DataItem as GetNursingItemGroupSubGroupListByDiagnose;
            //    if (!entity.IsHeader)
            //    {
            //        Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
            //        int count = BusinessLayer.GetvNursingTransactionDtList(String.Format("NursingItemGroupSubGroupID = {0} AND TransactionID = {1} AND NursingDiagnoseID = {2}",entity.NursingItemGroupSubGroupID,MainPage.GetNursingTransactionID(),MainPage.GetNursingDiagnoseID())).Count;
            //        lblTotalSelected.Text = count.ToString();
            //    }
            //}
        }

        protected void grdView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingDiagnoseItemForEvaluation entity = e.Row.DataItem as vNursingDiagnoseItemForEvaluation;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelectedEvaluation");
                TextBox txtNursingItemText = (TextBox)e.Row.FindControl("txtNursingItemText");
                CheckBox chkIsEditedByUser = (CheckBox)e.Row.FindControl("chkIsEvaluationEditedByUser");
                HtmlGenericControl divTxtInterventionItemText = (HtmlGenericControl)e.Row.FindControl("divTxtNursingItemText");
                HtmlGenericControl divLblInterventionItemText = (HtmlGenericControl)e.Row.FindControl("divLblNursingItemText");

                if (lstNursingDiagnoseItemID.Contains(entity.NursingDiagnoseItemID.ToString()))
                {
                    int idx = 0;
                    if (entity.NursingDiagnoseItemID != 0)
                        idx = Array.IndexOf(lstNursingDiagnoseItemID, entity.NursingDiagnoseItemID.ToString());
                    else
                        idx = Array.IndexOf(lstNursingTransactionDtID, entity.NursingTransactionEvaluationDt.ToString());

                    chkIsSelected.Checked = true;
                    txtNursingItemText.Text = lstNursingDiagnoseItemText[idx];
                    chkIsEditedByUser.Checked = Convert.ToBoolean(lstIsEditedByUser[idx]);
                    divTxtInterventionItemText.Style.Remove("display");
                    divLblInterventionItemText.Style.Remove("display");
                    if (chkIsEditedByUser.Checked)
                        divLblInterventionItemText.Style.Add("display", "none");
                    else
                        divTxtInterventionItemText.Style.Add("display", "none");
                }
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

        protected void cbpViewEvaluation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void cbpViewEvaluation1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(MainPage.GetNursingTransactionID());
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    MainPage.SaveHeaderFromUserControl();

                    IDbContext ctx = DbFactory.Configure(true);
                    try
                    {
                        SaveNursingTransactionDt(ctx, transactionID, ref errMessage);
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
        }

        protected void cbpViewEvaluation2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindGridViewAssessment();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = MainPage.GetNursingTransactionID();
        }

        protected void cbpViewEvaluation3_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindGridViewPlanning();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = MainPage.GetNursingTransactionID();
        }

        #region Public Function
        public void SaveNursingTransactionDt(IDbContext ctx, Int32 transactionID, ref string errMessage)
        {
            SaveNursingTransactionEvaluationDt(ctx,transactionID,errMessage);
            SaveNOCEvaluationScore(ctx, transactionID, errMessage);
            SaveNICContinuation(ctx, transactionID, errMessage);

            SetHiddenFieldWithinTransaction(Convert.ToInt32(MainPage.GetNursingTransactionID()), ctx);
        }


        private void SaveNursingTransactionEvaluationDt(IDbContext ctx, int transactionID, string errMessage)
        {
            NursingTransactionEvaluationDtDao entityNursingTransactionDtDao = new NursingTransactionEvaluationDtDao(ctx);

            int nursingTransactionDtID = 0;
            #region Save New Nursing Diagnose Item Quick Entry
            if (txtNewDiagnoseItem.Text.Length > 0)
            {
                NursingTransactionEvaluationDt entityEvalDt = new NursingTransactionEvaluationDt();
                entityEvalDt.TransactionID = Convert.ToInt32(MainPage.GetNursingTransactionID());
                entityEvalDt.NursingDiagnoseItemID = 0;
                entityEvalDt.NursingTransactionDtID = 0;
                entityEvalDt.GCNursingEvaluation = hdnOldID.Value;
                entityEvalDt.IsEditedByUser = true;
                entityEvalDt.NursingItemText = txtNewDiagnoseItem.Text;
                entityEvalDt.CreatedBy = AppSession.UserLogin.UserID;
                entityNursingTransactionDtDao.Insert(entityEvalDt);
                nursingTransactionDtID = BusinessLayer.GetNursingTransactionEvaluationDtMaxID(ctx);
                entityEvalDt = entityNursingTransactionDtDao.Get(nursingTransactionDtID);

                txtNewDiagnoseItem.Text = String.Empty;

                hdnListSaveNursingEvaluationDiagnoseItemID.Value = String.Format("{0}|{1}", hdnListSaveNursingEvaluationDiagnoseItemID.Value, entityEvalDt.NursingDiagnoseItemID);
                hdnListSaveNursingTransactionEvaluationID.Value = String.Format("{0}|{1}", hdnListSaveNursingTransactionEvaluationID.Value, entityEvalDt.ID); 
                hdnListSaveNursingEvaluationDiagnoseItemText.Value = String.Format("{0}|{1}", hdnListSaveNursingEvaluationDiagnoseItemText.Value, entityEvalDt.NursingItemText);
                hdnListSaveIsEvaluationEditedByUser.Value = String.Format("{0}|{1}", hdnListSaveIsEvaluationEditedByUser.Value, entityEvalDt.IsEditedByUser);
            }
            #endregion

            String[] paramNursingDiagnoseItemID = null;
            String[] paramNursingTransactionID = null;
            String[] paramNursingDiagnoseItemText = null;
            String[] paramIsEditedByUser = null;

            if (hdnListSaveNursingEvaluationDiagnoseItemID.Value == "|")
            {                

                //hdnListSaveNursingEvaluationDiagnoseItemID.Value = hdnListNursingEvaluationDiagnoseItemID.Value;
                //hdnListSaveNursingTransactionEvaluationID.Value = hdnListNursingTransactionEvaluationID.Value;
                //hdnListSaveNursingEvaluationDiagnoseItemText.Value = hdnListNursingEvaluationDiagnoseItemText.Value;
                //hdnListSaveIsEvaluationEditedByUser.Value = hdnListIsEvaluationEditedByUser.Value;
            }
            

            paramNursingDiagnoseItemID = hdnListSaveNursingEvaluationDiagnoseItemID.Value.Substring(1).Split('|');
            paramNursingTransactionID = hdnListSaveNursingTransactionEvaluationID.Value.Substring(1).Split('|');
            paramNursingDiagnoseItemText = hdnListSaveNursingEvaluationDiagnoseItemText.Value.Substring(1).Split('|');
            paramIsEditedByUser = hdnListSaveIsEvaluationEditedByUser.Value.Substring(1).Split('|');


            List<NursingTransactionEvaluationDt> lstEntityNursingTransDtBeforeSave = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("TransactionID = {0} AND NursingDiagnoseItemID <> 0", transactionID),ctx);
            List<NursingTransactionEvaluationDt> lstEntityNursingTransDtToDelete = lstEntityNursingTransDtBeforeSave.AsEnumerable().Where(p => !paramNursingDiagnoseItemID.Contains(p.NursingDiagnoseItemID.ToString())).ToList();

            List<NursingTransactionEvaluationDt> lstEntityDirectNursingTransDt = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("TransactionID = {0} AND NursingDiagnoseItemID = 0", transactionID), ctx);
            foreach (NursingTransactionEvaluationDt entity in lstEntityDirectNursingTransDt)
            {
                if(!paramNursingTransactionID.Contains(entity.ID.ToString()))
                {
                    lstEntityNursingTransDtToDelete.Add(entity);
                }
            }

            #region Delete
            foreach (NursingTransactionEvaluationDt entityDt in lstEntityNursingTransDtToDelete)
            {
                    entityNursingTransactionDtDao.Delete(entityDt.ID);
                    lstEntityNursingTransDtBeforeSave.Remove(entityDt);
            }
            #endregion

            for (int i = 0; i < paramNursingDiagnoseItemID.Count(); i++)
            {
                if (paramNursingDiagnoseItemID[i] != "0")
                {
                    NursingTransactionEvaluationDt entityDt = lstEntityNursingTransDtBeforeSave.FirstOrDefault(p => p.NursingDiagnoseItemID.ToString() == paramNursingDiagnoseItemID[i]);
                    if (entityDt != null)
                    {
                        #region Edit
                        entityDt.IsEditedByUser = (paramIsEditedByUser[i] == "true");
                        if (paramIsEditedByUser[i] == "true")
                            entityDt.NursingItemText = paramNursingDiagnoseItemText[i];
                        else
                            entityDt.NursingItemText = String.Empty;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityNursingTransactionDtDao.Update(entityDt);
                        #endregion
                    }
                    else
                    {
                        #region Add
                        if (paramNursingDiagnoseItemID[i] != "")
                        {
                            entityDt = new NursingTransactionEvaluationDt();
                            entityDt.TransactionID = transactionID;
                            entityDt.NursingDiagnoseItemID = Convert.ToInt32(paramNursingDiagnoseItemID[i]);
                            entityDt.NursingTransactionDtID = 0;
                            entityDt.IsEditedByUser = (paramIsEditedByUser[i] == "true");
                            if (paramIsEditedByUser[i] == "true")
                                entityDt.NursingItemText = paramNursingDiagnoseItemText[i];
                            else
                                entityDt.NursingItemText = String.Empty;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityNursingTransactionDtDao.Insert(entityDt);
                        }
                        #endregion
                    }
                }
                else
                {
                    NursingTransactionEvaluationDt entityDt = lstEntityNursingTransDtBeforeSave.FirstOrDefault(p => p.ID.ToString() == paramNursingTransactionID[i]);
                    if (entityDt != null)
                    {
                        #region Edit
                        entityDt.IsEditedByUser = (paramIsEditedByUser[i] == "true");
                        entityDt.NursingItemText = paramNursingDiagnoseItemText[i];
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityNursingTransactionDtDao.Update(entityDt);
                        #endregion
                    }
                }
            }
        }

        private void SaveNOCEvaluationScore(IDbContext ctx, int transactionID, string errMessage)
        {
            NursingTransactionOutcomeDtDao entityNursingTransactionDtDao = new NursingTransactionOutcomeDtDao(ctx);

            String[] paramNursingOutcomeDtID = hdnListOutcomeDtID.Value.Substring(1).Split('|');
            String[] paramScaleScoreEvaluation = hdnListScaleScoreEvaluation.Value.Substring(1).Split('|');


            List<NursingTransactionOutcomeDt> lstEntityNursingTransDtBeforeSave = BusinessLayer.GetNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}", transactionID), ctx);

            for (int i = 0; i < paramNursingOutcomeDtID.Count(); i++)
            {
                NursingTransactionOutcomeDt entityDt = lstEntityNursingTransDtBeforeSave.FirstOrDefault(p => p.ID.ToString() == paramNursingOutcomeDtID[i]);
                if (entityDt != null)
                {
                    #region Edit
                    entityDt.ScaleScoreEvaluation = Convert.ToInt32(paramScaleScoreEvaluation[i]);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityNursingTransactionDtDao.Update(entityDt);
                    #endregion
                }
            }
        }

        private void SaveNICContinuation(IDbContext ctx, int transactionID, string errMessage)
        {
            NursingTransactionInterventionDtDao entityNursingTransactionDtDao = new NursingTransactionInterventionDtDao(ctx);

            String[] paramNursingInterventionDtID = hdnListNursingInterventionEvaluation.Value.Substring(1).Split('|');
            String[] paramScaleScoreEvaluation = hdnListIsContinued.Value.Substring(1).Split('|');


            List<vNursingTransactionInterventionDt> lstEntityNursingTransDtBeforeSave = BusinessLayer.GetvNursingTransactionInterventionDtList(String.Format("TransactionID = {0}", transactionID), ctx);

            for (int i = 0; i < paramNursingInterventionDtID.Count(); i++)
            {
                int tempID = 0;
                if (paramNursingInterventionDtID[i] != "")
                    tempID = Convert.ToInt32(paramNursingInterventionDtID[i]);
                NursingTransactionInterventionDt entityDt = BusinessLayer.GetNursingTransactionInterventionDtList(String.Format("ID = {0}",tempID),ctx).FirstOrDefault();
                if (entityDt != null)
                {
                    #region Edit
                    entityDt.IsContinued = (paramScaleScoreEvaluation[i] == "1");
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityNursingTransactionDtDao.Update(entityDt);
                    #endregion
                }
            }
        }

        public void LoadGridViewByHeaderEntity(Int32 transactionID)
        {
            List<NursingTransactionEvaluationDt> lstSelectedItem = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("TransactionID = {0}", MainPage.GetNursingTransactionID()));
            List<string> lstSelectedNursingDiagnoseItemID = new List<string>();
            List<string> lstSelectedNursingTransactionDtID = new List<string>();
            List<string> lstSelectedNursingDiagnoseItemText = new List<string>();
            List<string> lstSelectedIsEditedByUser = new List<string>();
            for (int i = 0; i < lstSelectedItem.Count; i++)
            {
                lstSelectedNursingDiagnoseItemID.Add(lstSelectedItem[i].NursingDiagnoseItemID.ToString());
                if(lstSelectedItem[i].NursingDiagnoseItemID > 0)
                    lstSelectedNursingTransactionDtID.Add("0");
                else
                    lstSelectedNursingTransactionDtID.Add(lstSelectedItem[i].ID.ToString());
                lstSelectedNursingDiagnoseItemText.Add(lstSelectedItem[i].NursingItemText);
                lstSelectedIsEditedByUser.Add(lstSelectedItem[i].IsEditedByUser.ToString());
            }
            //hdnListNursingEvaluationDiagnoseItemID.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemID.ToArray());
            //hdnListNursingTransactionEvaluationID.Value = "|" + string.Join("|", lstSelectedNursingTransactionDtID.ToArray());
            //hdnListNursingEvaluationDiagnoseItemText.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemText.ToArray());
            //hdnListIsEvaluationEditedByUser.Value = "|" + string.Join("|", lstSelectedIsEditedByUser.ToArray());
            //hdnListSaveNursingEvaluationDiagnoseItemID.Value = hdnListNursingEvaluationDiagnoseItemID.Value;
            //hdnListSaveNursingTransactionEvaluationID.Value = hdnListNursingTransactionEvaluationID.Value;
            //hdnListSaveNursingEvaluationDiagnoseItemText.Value = hdnListNursingEvaluationDiagnoseItemText.Value;
            //hdnListSaveIsEvaluationEditedByUser.Value = hdnListIsEvaluationEditedByUser.Value;

            List<NursingTransactionOutcomeDt> lstSelectedOutcome = BusinessLayer.GetNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}",MainPage.GetNursingTransactionID()));
            List<string> lstSelectedNursingOutcomeID = new List<string>();
            List<string> lstSelectedScaleScoreEvaluation = new List<string>();
            for (int i = 0; i < lstSelectedOutcome.Count; i++)
            {
                lstSelectedNursingOutcomeID.Add(lstSelectedOutcome[i].ID.ToString());
                lstSelectedScaleScoreEvaluation.Add(lstSelectedOutcome[i].ScaleScoreEvaluation.ToString());
            }
            hdnListOutcomeDtID.Value = "|" + string.Join("|", lstSelectedNursingOutcomeID.ToArray());
            hdnListScaleScoreEvaluation.Value = "|" + string.Join("|", lstSelectedScaleScoreEvaluation.ToArray());

            string filterExpressionIntervention = String.Format("NursingTransactionInterventionHdID IN (SELECT ID FROM NursingTransactionInterventionHd WHERE TransactionID = {0})",MainPage.GetNursingTransactionID());
            List<NursingTransactionInterventionDt> lstSelectedIntervention = BusinessLayer.GetNursingTransactionInterventionDtList(filterExpressionIntervention);
            List<string> lstSelectedNursingInterventionDtID = new List<string>();
            List<string> lstSelectedIsContinue = new List<string>();
            for (int i = 0; i < lstSelectedIntervention.Count; i++)
            {
                lstSelectedNursingInterventionDtID.Add(lstSelectedIntervention[i].ID.ToString());
                lstSelectedIsContinue.Add(lstSelectedIntervention[i].IsContinued ? "1" : "0");
            }
            hdnListNursingInterventionEvaluation.Value = "|" + string.Join("|", lstSelectedNursingInterventionDtID.ToArray());
            hdnListIsContinued.Value = "|" + string.Join("|", lstSelectedIsContinue.ToArray());            

            BindGridView();
            BindGridViewDiagnoseItem();
            BindGridViewAssessment();
            BindGridViewPlanning();
        }

        public string SetHiddenFieldWithinTransaction(Int32 transactionID,IDbContext ctx)
        {
            List<NursingTransactionEvaluationDt> lstSelectedItem = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("TransactionID = {0}", MainPage.GetNursingTransactionID()),ctx);
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
            //hdnListNursingEvaluationDiagnoseItemID.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemID.ToArray());
            //hdnListNursingTransactionEvaluationID.Value = "|" + string.Join("|", lstSelectedNursingTransactionDtID.ToArray());
            //hdnListNursingEvaluationDiagnoseItemText.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemText.ToArray());
            //hdnListIsEvaluationEditedByUser.Value = "|" + string.Join("|", lstSelectedIsEditedByUser.ToArray());

            List<NursingTransactionOutcomeDt> lstSelectedOutcome = BusinessLayer.GetNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}", MainPage.GetNursingTransactionID()),ctx);
            List<string> lstSelectedNursingOutcomeID = new List<string>();
            List<string> lstSelectedScaleScoreEvaluation = new List<string>();
            for (int i = 0; i < lstSelectedOutcome.Count; i++)
            {
                lstSelectedNursingOutcomeID.Add(lstSelectedOutcome[i].ID.ToString());
                lstSelectedScaleScoreEvaluation.Add(lstSelectedOutcome[i].ScaleScoreEvaluation.ToString());
            }
            hdnListOutcomeDtID.Value = "|" + string.Join("|", lstSelectedNursingOutcomeID.ToArray());
            hdnListScaleScoreEvaluation.Value = "|" + string.Join("|", lstSelectedScaleScoreEvaluation.ToArray());

            string filterExpressionIntervention = String.Format("NursingTransactionInterventionHdID IN (SELECT ID FROM NursingTransactionInterventionHd WHERE TransactionID = {0})", MainPage.GetNursingTransactionID());
            List<NursingTransactionInterventionDt> lstSelectedIntervention = BusinessLayer.GetNursingTransactionInterventionDtList(filterExpressionIntervention,ctx);
            List<string> lstSelectedNursingInterventionDtID = new List<string>();
            List<string> lstSelectedIsContinue = new List<string>();
            for (int i = 0; i < lstSelectedIntervention.Count; i++)
            {
                lstSelectedNursingInterventionDtID.Add(lstSelectedIntervention[i].ID.ToString());
                lstSelectedIsContinue.Add(lstSelectedIntervention[i].IsContinued ? "1" : "0");
            }
            hdnListNursingInterventionEvaluation.Value = "|" + string.Join("|", lstSelectedNursingInterventionDtID.ToArray());
            hdnListIsContinued.Value = "|" + string.Join("|", lstSelectedIsContinue.ToArray());

            string retval = "";
            //retval += hdnListNursingEvaluationDiagnoseItemID.Value + "#";
            //retval += hdnListNursingTransactionEvaluationID.Value + "#";
            //retval += hdnListNursingEvaluationDiagnoseItemText.Value + "#";
            //retval += hdnListIsEvaluationEditedByUser.Value + "#";
            retval += hdnListOutcomeDtID.Value + "#";
            retval += hdnListScaleScoreEvaluation.Value + "#";
            retval += hdnListNursingInterventionEvaluation.Value + "#";
            retval += hdnListIsContinued.Value + "#";
            return retval;
        }

        #endregion
    }
}