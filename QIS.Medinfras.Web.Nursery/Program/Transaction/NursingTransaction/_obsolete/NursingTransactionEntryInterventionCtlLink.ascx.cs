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

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingTransactionEntryInterventionCtlLink : BaseUserControlCtl
    {
        private string[] lstNursingInterventionItemID = null;
        private string[] lstNursingInterventionItemText = null;
        private string[] lstIsInterventionEditedByUser = null;

        private NursingTransactionEntryLink MainPage
        {
            get { return (NursingTransactionEntryLink)Page; }
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    //BindGridView();
        //    //BindGridViewDiagnoseItem();
        //    base.OnLoad(e);
        //}

        protected string OnGetDiagnoseFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }

        private string GetFilterExpressionDiagnoseItem()
        {
            string filterExpression = String.Empty;
            if (hdnID.Value != String.Empty)
            {
                NursingTransactionInterventionHd entity = BusinessLayer.GetNursingTransactionInterventionHd(Convert.ToInt32(hdnID.Value));
                filterExpression = String.Format("NursingInterventionID = {0} ", entity.NursingInterventionID);
            }
            else
                filterExpression = "1=0 ";
            filterExpression += " AND IsDeleted = 0";
            return filterExpression;
        }

        protected string OnGetInterventionFilterExpression()
        {
            return String.Format("NurseInterventionID NOT IN (SELECT NursingInterventionID FROM NursingTransactionInterventionHD WHERE TransactionID = {0}) AND IsDeleted = 0",MainPage.GetNursingTransactionID());
        }

        private void BindGridView()
        {
            List<vNursingTransactionInterventionHd> lstEntity = BusinessLayer.GetvNursingTransactionInterventionHdList(String.Format("TransactionID = {0}",MainPage.GetNursingTransactionID()));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionDiagnoseItem();
            lstNursingInterventionItemID = hdnListNursingInterventionItemID.Value.Substring(1).Split('|');
            lstNursingInterventionItemText = hdnListNursingInterventionItemText.Value.Substring(1).Split('|');
            lstIsInterventionEditedByUser = hdnListIsInterventionEditedByUser.Value.Substring(1).Split('|');
            List<vNursingInterventionItem> lstEntity = BusinessLayer.GetvNursingInterventionItemList(filterExpression);
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();
            
        }

        private void OnAddNursingIntervention()
        {
            NursingTransactionInterventionHd entity = new NursingTransactionInterventionHd();
            entity.TransactionID = Convert.ToInt32(MainPage.GetNursingTransactionID());
            entity.NursingInterventionID = Convert.ToInt32(hdnInterventionID.Value);
            entity.CreatedBy = AppSession.UserLogin.UserID;
            BusinessLayer.InsertNursingTransactionInterventionHd(entity);
        }

        private void OnDeleteNursingIntervention()
        {
            BusinessLayer.DeleteNursingTransactionInterventionHd(Convert.ToInt32(hdnID.Value));
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionInterventionHd entity = e.Row.DataItem as vNursingTransactionInterventionHd;
                
                Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
                HtmlImage imgDelete = (HtmlImage)e.Row.FindControl("imgDelete");
                string filterExpression = String.Format("NursingInterventionID = {0} AND TransactionID = {1}", entity.NursingInterventionID, MainPage.GetNursingTransactionID());
                int count = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression).Count;
                lblTotalSelected.Text = count.ToString();
                if (count > 0)
                    imgDelete.Visible = false;
                
            }
        }

        protected void grdView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingInterventionItem entity = e.Row.DataItem as vNursingInterventionItem;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelectedIntervention");
                TextBox txtNursingItemText = (TextBox)e.Row.FindControl("txtNursingItemText");
                CheckBox chkIsEditedByUser = (CheckBox)e.Row.FindControl("chkIsInterventionEditedByUser");
                TextBox txtInterventionImplementation = (TextBox)e.Row.FindControl("txtInterventoinImplementation");
                HtmlGenericControl  divTxtInterventionItemText = (HtmlGenericControl )e.Row.FindControl("divTxtInterventionItemText");
                HtmlGenericControl  divLblInterventionItemText = (HtmlGenericControl )e.Row.FindControl("divLblInterventionItemText");
                if (lstNursingInterventionItemID.Contains(entity.NursingInterventionItemID.ToString()))
                {
                    int idx = Array.IndexOf(lstNursingInterventionItemID, entity.NursingInterventionItemID.ToString());
                    chkIsSelected.Checked = true;
                    txtNursingItemText.Text = lstNursingInterventionItemText[idx];
                    chkIsEditedByUser.Checked = Convert.ToBoolean(lstIsInterventionEditedByUser[idx]);
                    divTxtInterventionItemText.Style.Remove("display");
                    divLblInterventionItemText.Style.Remove("display");
                    if (chkIsEditedByUser.Checked)
                        divLblInterventionItemText.Style.Add("display", "none");               
                    else
                        divTxtInterventionItemText.Style.Add("display", "none");               
                }
            }
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
                    BindGridView();
                    result = "refresh|" + pageCount;
                }
                else if (param[0] == "save")
                {
                    OnAddNursingIntervention();
                    BindGridView();
                }
                else if (param[0] == "delete")
                {
                    OnDeleteNursingIntervention();
                    hdnID.Value = String.Empty;
                    BindGridView();
                    result = "delete";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewIntervention1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        public string SaveNursingTransactionDt(IDbContext ctx,Int32 transactionID,ref string errMessage)
        {
            hdnTransactionID.Value = transactionID.ToString();
            NursingTransactionInterventionDtDao entityNursingTransactionInterventionDtDao = new NursingTransactionInterventionDtDao(ctx);
            //int nursingTransactionDtID = 0;
            //#region Save New Nursing Diagnose Item Quick Entry
            //if (txtNewDiagnoseItem.Text.Length > 0)
            //{
            //    NursingItemDao entityItemDao = new NursingItemDao(ctx);
            //    NursingItem entityItem = new NursingItem();
            //    entityItem.NursingItemText = txtNewDiagnoseItem.Text;
            //    entityItem.IsDeleted = false;
            //    entityItem.CreatedBy = AppSession.UserLogin.UserID;
            //    entityItemDao.Insert(entityItem);
            //    int nursingItemID = BusinessLayer.GetNursingItemMaxID(ctx);

            //    NursingDiagnoseItemDao entityDiagItemDao = new NursingDiagnoseItemDao(ctx);
            //    NursingDiagnoseItem entityDiagItem = new NursingDiagnoseItem();
            //    entityDiagItem.NursingDiagnoseID = Convert.ToInt32(MainPage.GetNursingDiagnoseID());
            //    entityDiagItem.NursingItemGroupSubGroupID = Convert.ToInt32(hdnOldID.Value);
            //    entityDiagItem.NursingItemID = nursingItemID;
            //    entityDiagItem.Scale1Text = String.Empty;
            //    entityDiagItem.Scale2Text = String.Empty;
            //    entityDiagItem.Scale3Text = String.Empty;
            //    entityDiagItem.Scale4Text = String.Empty;
            //    entityDiagItem.Scale5Text = String.Empty;
            //    entityDiagItem.IsEditableByUser = true;
            //    entityDiagItem.IsUsingIndicator = false;
            //    entityDiagItem.IsDeleted = false;
            //    entityDiagItem.CreatedBy = AppSession.UserLogin.UserID;
            //    entityDiagItemDao.Insert(entityDiagItem);
            //    int nursingDiagnoseItemID = BusinessLayer.GetNursingDiagnoseItemMaxID(ctx);

            //    NursingTransactionDt entityDt = new NursingTransactionDt();
            //    entityDt.TransactionID = transactionID;
            //    entityDt.NursingDiagnoseItemID = nursingDiagnoseItemID;
            //    entityDt.IsEditedByUser = false;
            //    entityDt.NursingItemText = String.Empty;
            //    entityDt.CreatedBy = AppSession.UserLogin.UserID;
            //    entityNursingTransactionDtDao.Insert(entityDt);
            //    nursingTransactionDtID = BusinessLayer.GetNursingTransactionDtMaxID(ctx);
            //    entityDt = entityNursingTransactionDtDao.Get(nursingTransactionDtID);

            //    txtNewDiagnoseItem.Text = String.Empty;

            //    hdnListNursingDiagnoseItemID.Value = String.Format("{0}|{1}", hdnListNursingDiagnoseItemID.Value, entityDt.NursingDiagnoseItemID);
            //    hdnListNursingDiagnoseItemText.Value = String.Format("{0}|{1}", hdnListNursingDiagnoseItemText.Value, entityDt.NursingItemText);
            //    hdnListIsEditedByUser.Value = String.Format("{0}|{1}", hdnListIsEditedByUser.Value, entityDt.IsEditedByUser);
            //}
            //#endregion

            String[] paramNursingInterventionItemID = hdnListNursingInterventionItemID.Value.Substring(1).Split('|');
            String[] paramNursingInterventionItemText = hdnListNursingInterventionItemText.Value.Substring(1).Split('|');
            String[] paramIsEditedByUser = hdnListIsInterventionEditedByUser.Value.Substring(1).Split('|');

            List<vNursingTransactionInterventionDt> lstEntityNursingTransDtBeforeSave = BusinessLayer.GetvNursingTransactionInterventionDtList(String.Format("TransactionID = {0}", transactionID), ctx);
            List<vNursingTransactionInterventionDt> lstEntityNursingTransDtToDelete = lstEntityNursingTransDtBeforeSave.AsEnumerable().Where(p => !paramNursingInterventionItemID.Contains(p.NursingInterventionItemID.ToString())).ToList();

            #region Delete
            foreach (vNursingTransactionInterventionDt entityDt in lstEntityNursingTransDtToDelete)
            {
                entityNursingTransactionInterventionDtDao.Delete(entityDt.ID);
                lstEntityNursingTransDtBeforeSave.Remove(entityDt);
            }
            #endregion

            for (int i = 0; i < paramNursingInterventionItemID.Count(); i++)
            {
                vNursingTransactionInterventionDt vEntityDt = lstEntityNursingTransDtBeforeSave.FirstOrDefault(p => p.NursingInterventionItemID.ToString() == paramNursingInterventionItemID[i]);
                if (vEntityDt != null)
                {
                    #region Edit
                    NursingTransactionInterventionDt entityDt = BusinessLayer.GetNursingTransactionInterventionDt(vEntityDt.ID);
                    entityDt.IsEditedByUser = (paramIsEditedByUser[i] == "true");
                    if (paramIsEditedByUser[i] == "true")
                        entityDt.NursingItemText = paramNursingInterventionItemText[i];
                    else
                        entityDt.NursingItemText = String.Empty;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityNursingTransactionInterventionDtDao.Update(entityDt);
                    #endregion
                }
                else
                {
                    #region Add
                    if (paramNursingInterventionItemID[i] != "")
                    {
                        NursingTransactionInterventionDt entityDt = new NursingTransactionInterventionDt();
                        entityDt.NursingTransactionInterventionHdID = Convert.ToInt32(hdnOldID.Value);
                        entityDt.NursingInterventionItemID = Convert.ToInt32(paramNursingInterventionItemID[i]);
                        entityDt.IsEditedByUser = (paramIsEditedByUser[i] == "true");
                        if (paramIsEditedByUser[i] == "true")
                            entityDt.NursingItemText = paramNursingInterventionItemText[i];
                        else
                            entityDt.NursingItemText = String.Empty;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityNursingTransactionInterventionDtDao.Insert(entityDt);
                    }
                    #endregion
                }
            }

            return MainPage.LoadNursingEvaluationFromNursingDiagnoseItem(ctx);
        }
        public void LoadGridViewByHeaderEntity(Int32 transactionID)
        {
            hdnTransactionID.Value = transactionID.ToString();

            List<vNursingTransactionInterventionDt> lstSelectedItem = BusinessLayer.GetvNursingTransactionInterventionDtList(String.Format("TransactionID = {0}", transactionID));
            List<string> lstSelectedNursingInterventionItemID = new List<string>();
            List<string> lstSelectedNursingInterventionItemText = new List<string>();
            List<string> lstSelectedIsInterventionEditedByUser = new List<string>();
            for (int i=0; i< lstSelectedItem.Count;i++)
            {
                lstSelectedNursingInterventionItemID.Add(lstSelectedItem[i].NursingInterventionItemID.ToString());
                lstSelectedNursingInterventionItemText.Add(lstSelectedItem[i].NursingItemText);
                lstSelectedIsInterventionEditedByUser.Add(lstSelectedItem[i].IsEditedByUser.ToString());
            }
            hdnListNursingInterventionItemID.Value = "|" + string.Join("|", lstSelectedNursingInterventionItemID.ToArray());
            hdnListNursingInterventionItemText.Value = "|" + string.Join("|", lstSelectedNursingInterventionItemText.ToArray());
            hdnListIsInterventionEditedByUser.Value = "|" + string.Join("|", lstSelectedIsInterventionEditedByUser.ToArray());             

            BindGridView();
            BindGridViewDiagnoseItem();
        }
        #endregion
    }
}