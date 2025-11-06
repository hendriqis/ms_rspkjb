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
    public partial class NursingTransactionInterventionTransferCtl : BaseUserControlCtl
    {
        private string[] lstNursingInterventionItemID = null;
        private string[] lstNursingInterventionItemText = null;
        private string[] lstIsInterventionEditedByUser = null;

        private NursingProcessTransfer MainPage
        {
            get { return (NursingProcessTransfer)Page; }
        }

        public string NursingDiagnoseID { get; set; }

        public string GetNursingDiagnoseID()
        {
            return this.NursingDiagnoseID;
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
            return MainPage.GetInterventionFilterExpression();
        }

        private void BindGridView()
        {
            List<vNursingTransactionInterventionHd> lstEntity = BusinessLayer.GetvNursingTransactionInterventionHdList(String.Format("TransactionID = {0}",MainPage.GetNursingTransactionID()));
            grdViewIntervention.DataSource = lstEntity;
            grdViewIntervention.DataBind();
        }

        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionDiagnoseItem() + " Order By DisplayOrder";
            lstNursingInterventionItemID = hdnListNursingInterventionItemID.Value.Substring(1).Split('|');
            lstNursingInterventionItemText = hdnListNursingInterventionItemText.Value.Substring(1).Split('|');
            lstIsInterventionEditedByUser = hdnListIsInterventionEditedByUser.Value.Substring(1).Split('|');
            List<vNursingInterventionItem> lstEntity = BusinessLayer.GetvNursingInterventionItemList(filterExpression);
            grdViewIntervention1.DataSource = lstEntity;
            grdViewIntervention1.DataBind();
            
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

        protected void grdViewIntervention_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void grdViewIntervention1_RowDataBound(object sender, GridViewRowEventArgs e)
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
            hdnNursingDiagnoseID.Value = MainPage.GetNursingDiagnoseID();

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
                        hdnEvaluation = SaveNursingTransactionDt(ctx, transactionID, hdnNursingDiagnoseID.Value, ref errMessage);
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
        public string SaveNursingTransactionDt(IDbContext ctx,Int32 transactionID, string diagnoseID,ref string errMessage)
        {
            hdnTransactionID.Value = transactionID.ToString();
            hdnNursingDiagnoseID.Value = diagnoseID;

            NursingTransactionInterventionDtDao entityNursingTransactionInterventionDtDao = new NursingTransactionInterventionDtDao(ctx);

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
        public void LoadGridViewByHeaderEntity(Int32 transactionID, Int32 nursingDiagnoseID)
        {
            hdnTransactionID.Value = transactionID.ToString();
            hdnNursingDiagnoseID.Value = nursingDiagnoseID.ToString();     

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

        public void SetHiddenField(string transactionID, string diagnoseID)
        {
            hdnTransactionID.Value = transactionID;
            hdnNursingDiagnoseID.Value = diagnoseID;
        }
        #endregion

        protected void cbpFilterExpression_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            hdnTransactionID.Value = MainPage.GetNursingTransactionID();
            hdnNursingDiagnoseID.Value = MainPage.GetNursingDiagnoseID();
            hdnFilterExpression.Value = MainPage.GetInterventionFilterExpression();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = hdnFilterExpression.Value;
        }
    }
}