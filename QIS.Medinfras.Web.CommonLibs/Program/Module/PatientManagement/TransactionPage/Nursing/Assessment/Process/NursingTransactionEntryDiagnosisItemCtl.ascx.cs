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
    public partial class NursingTransactionEntryDiagnosisItemCtl : BaseUserControlCtl
    {
        private string[] lstNursingDiagnoseItemID = null;
        private string[] lstNursingDiagnoseItemText = null;
        private string[] lstIsEditedByUser = null;

        private NursingProcessEntry1 MainPage
        {
            get { return (NursingProcessEntry1)Page; }
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
            if (MainPage.GetNursingDiagnoseID() != String.Empty)
                filterExpression = String.Format("NursingDiagnoseID = {0} ", MainPage.GetNursingDiagnoseID());
            else
                filterExpression = "NursingDiagnoseID = 0 ";
            
            if (hdnID.Value != String.Empty)
                filterExpression += String.Format(" AND NursingItemGroupSubGroupID = {0} ", hdnID.Value);
            else
                filterExpression += " AND 1 = 0 ";

            //if (txtNewDiagnoseItem.Text != "")
            //    filterExpression += String.Format(" AND NursingItemText LIKE '%{0}%'",txtNewDiagnoseItem.Text);
            
            filterExpression += " AND IsDeleted = 0";

            return filterExpression;
        }

        private void BindGridView()
        {
            List<GetNursingItemGroupSubGroupListByDiagnose> lstEntity = BusinessLayer.GetNursingItemGroupSubGroupListByDiagnose(Convert.ToInt32(MainPage.GetNursingDiagnoseID()));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionDiagnoseItem();
            lstNursingDiagnoseItemID = hdnListNursingDiagnoseItemID.Value.Substring(1).Split('|');
            lstNursingDiagnoseItemText = hdnListNursingDiagnoseItemText.Value.Substring(1).Split('|');
            lstIsEditedByUser = hdnListIsEditedByUser.Value.Substring(1).Split('|');
            List<vNursingDiagnoseItem> lstEntity = BusinessLayer.GetvNursingDiagnoseItemList(filterExpression);
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();

        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetNursingItemGroupSubGroupListByDiagnose entity = e.Row.DataItem as GetNursingItemGroupSubGroupListByDiagnose;
                if (!entity.IsHeader)
                {
                    Label lblTotalSelectedDiagnosisItem = (Label)e.Row.FindControl("lblTotalSelectedDiagnosisItem");
                    int count = BusinessLayer.GetvNursingTransactionDtList(String.Format("NursingItemGroupSubGroupID = {0} AND TransactionID = {1} AND NursingDiagnoseID = {2}",entity.NursingItemGroupSubGroupID,MainPage.GetNursingTransactionID(),MainPage.GetNursingDiagnoseID())).Count;
                    lblTotalSelectedDiagnosisItem.Text = count.ToString();
                }
            }
        }

        protected void grdView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingDiagnoseItem entity = e.Row.DataItem as vNursingDiagnoseItem;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");
                TextBox txtNursingItemText = (TextBox)e.Row.FindControl("txtNursingItemText");
                CheckBox chkIsEditedByUser = (CheckBox)e.Row.FindControl("chkIsEditedByUser");
                HtmlGenericControl divTxtInterventionItemText = (HtmlGenericControl)e.Row.FindControl("divTxtNursingItemText");
                HtmlGenericControl divLblInterventionItemText = (HtmlGenericControl)e.Row.FindControl("divLblNursingItemText");
                if (lstNursingDiagnoseItemID.Contains(entity.NursingDiagnoseItemID.ToString()))
                {
                    int idx = Array.IndexOf(lstNursingDiagnoseItemID, entity.NursingDiagnoseItemID.ToString());
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

        protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                        //MainPage.SaveEvaluationFromUserControl(ctx);
                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        errMessage = ex.Message;
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
            NursingTransactionDtDao entityNursingTransactionDtDao = new NursingTransactionDtDao(ctx);
            NursingTransactionEvaluationDtDao entityNursingTransactionEvaluationDtDao = new NursingTransactionEvaluationDtDao(ctx);
            NursingDiagnoseItemDao entityDiagnoseItemDao = new NursingDiagnoseItemDao(ctx);
            NursingItemGroupSubGroupDao entityGroupDao = new NursingItemGroupSubGroupDao(ctx);

            int nursingTransactionDtID = 0;
            #region Save New Nursing Diagnose Item Quick Entry
            if (txtNewDiagnoseItem.Text.Length > 0)
            {
                NursingItemDao entityItemDao = new NursingItemDao(ctx);
                NursingItem entityItem = new NursingItem();
                entityItem.NursingItemText = txtNewDiagnoseItem.Text;
                entityItem.IsDeleted = false;
                entityItem.CreatedBy = AppSession.UserLogin.UserID;
                int nursingItemID = entityItemDao.InsertReturnPrimaryKeyID(entityItem);

                NursingDiagnoseItemDao entityDiagItemDao = new NursingDiagnoseItemDao(ctx);
                NursingDiagnoseItem entityDiagItem = new NursingDiagnoseItem();
                entityDiagItem.NursingDiagnoseID = Convert.ToInt32(MainPage.GetNursingDiagnoseID());
                entityDiagItem.NursingItemGroupSubGroupID = Convert.ToInt32(hdnOldID.Value);
                entityDiagItem.NursingItemID = nursingItemID;
                entityDiagItem.Scale1Text = String.Empty;
                entityDiagItem.Scale2Text = String.Empty;
                entityDiagItem.Scale3Text = String.Empty;
                entityDiagItem.Scale4Text = String.Empty;
                entityDiagItem.Scale5Text = String.Empty;
                entityDiagItem.IsEditableByUser = true;
                entityDiagItem.IsUsingIndicator = false;
                entityDiagItem.IsDeleted = false;
                entityDiagItem.CreatedBy = AppSession.UserLogin.UserID;
                int nursingDiagnoseItemID = entityDiagItemDao.InsertReturnPrimaryKeyID(entityDiagItem);

                NursingTransactionDt entityDt = new NursingTransactionDt();
                entityDt.TransactionID = transactionID;
                entityDt.NursingDiagnoseItemID = nursingDiagnoseItemID;
                entityDt.IsEditedByUser = false;
                entityDt.NursingItemText = String.Empty;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                nursingTransactionDtID = entityNursingTransactionDtDao.InsertReturnPrimaryKeyID(entityDt);
                //nursingTransactionDtID = BusinessLayer.GetNursingTransactionDtMaxID(ctx);
                //entityDt = entityNursingTransactionDtDao.Get(nursingTransactionDtID);

                NursingDiagnoseItem entityDiagnoseItem = entityDiagnoseItemDao.Get(entityDt.NursingDiagnoseItemID);
                string gcNursingEvaluation = Constant.NursingEvaluation.SUBJECTIVE;
                if (entityDiagnoseItem.NursingItemGroupSubGroupID != null)
                {
                    NursingItemGroupSubGroup entityGroup = entityGroupDao.Get(entityDiagnoseItem.NursingItemGroupSubGroupID);
                    gcNursingEvaluation = entityGroup.GCNursingEvaluation;
                }

                if ((gcNursingEvaluation == Constant.NursingEvaluation.SUBJECTIVE || gcNursingEvaluation == Constant.NursingEvaluation.OBJECTIVE))
                {
                    NursingTransactionEvaluationDt entityEvalDt = new NursingTransactionEvaluationDt();
                    entityEvalDt.TransactionID = Convert.ToInt32(MainPage.GetNursingTransactionID());
                    entityEvalDt.NursingTransactionDtID = nursingTransactionDtID;
                    entityEvalDt.NursingDiagnoseItemID = entityDt.NursingDiagnoseItemID;
                    entityEvalDt.GCNursingEvaluation = gcNursingEvaluation;
                    entityEvalDt.IsEditedByUser = false;
                    entityEvalDt.NursingItemText = String.Empty;
                    entityEvalDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityNursingTransactionEvaluationDtDao.Insert(entityEvalDt);
                }

                txtNewDiagnoseItem.Text = String.Empty;

                hdnListNursingDiagnoseItemID.Value = String.Format("{0}|{1}", hdnListNursingDiagnoseItemID.Value,entityDt.NursingDiagnoseItemID);
                hdnListNursingDiagnoseItemText.Value = String.Format("{0}|{1}", hdnListNursingDiagnoseItemText.Value, entityDt.NursingItemText);
                hdnListIsEditedByUser.Value = String.Format("{0}|{1}", hdnListIsEditedByUser.Value, entityDt.IsEditedByUser);
            }
            #endregion
            
            String[] paramNursingDiagnoseItemID = hdnListNursingDiagnoseItemID.Value.Substring(1).Split('|');
            String[] paramNursingDiagnoseItemText = hdnListNursingDiagnoseItemText.Value.Substring(1).Split('|');
            String[] paramIsEditedByUser = hdnListIsEditedByUser.Value.Substring(1).Split('|');

            
            List<NursingTransactionDt> lstEntityNursingTransDtBeforeSave = BusinessLayer.GetNursingTransactionDtList(String.Format("TransactionID = {0}", transactionID), ctx);
            List<NursingTransactionDt> lstEntityNursingTransDtToDelete = lstEntityNursingTransDtBeforeSave.AsEnumerable().Where(p => !paramNursingDiagnoseItemID.Contains(p.NursingDiagnoseItemID.ToString())).ToList();

            #region Delete
            foreach (NursingTransactionDt entityDt in lstEntityNursingTransDtToDelete)
            {
                entityNursingTransactionDtDao.Delete(entityDt.ID);
                lstEntityNursingTransDtBeforeSave.Remove(entityDt);

                NursingTransactionEvaluationDt entityEval = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("NursingTransactionDtID = {0}",entityDt.ID),ctx).FirstOrDefault();
                if(entityEval != null)
                    entityNursingTransactionEvaluationDtDao.Delete(entityEval.ID);
            }
            #endregion

            for (int i = 0; i < paramNursingDiagnoseItemID.Count(); i++)
            {
                NursingTransactionDt entityDt = lstEntityNursingTransDtBeforeSave.FirstOrDefault(p => p.NursingDiagnoseItemID.ToString() == paramNursingDiagnoseItemID[i]);
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

                   
                    NursingDiagnoseItem entityDiagnoseItem = entityDiagnoseItemDao.Get(entityDt.NursingDiagnoseItemID);
                    string gcNursingEvaluation = Constant.NursingEvaluation.SUBJECTIVE;
                    if (entityDiagnoseItem != null)
                    {
                        NursingItemGroupSubGroup entityGroup = entityGroupDao.Get(entityDiagnoseItem.NursingItemGroupSubGroupID);
                        if (entityGroup != null)
                        {
                            gcNursingEvaluation = entityGroup.GCNursingEvaluation;
                        }
                    }

                    NursingTransactionEvaluationDt entityEval = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("NursingTransactionDtID = {0}",entityDt.ID),ctx).FirstOrDefault();
                    if (entityEval == null && (gcNursingEvaluation == Constant.NursingEvaluation.SUBJECTIVE || gcNursingEvaluation == Constant.NursingEvaluation.OBJECTIVE)) 
                    {
                        NursingTransactionEvaluationDt entityEvalDt = new NursingTransactionEvaluationDt();
                        entityEvalDt.TransactionID = Convert.ToInt32(MainPage.GetNursingTransactionID());
                        entityEvalDt.NursingTransactionDtID = entityDt.ID;
                        entityEvalDt.NursingDiagnoseItemID = entityDt.NursingDiagnoseItemID;
                        entityEvalDt.GCNursingEvaluation = gcNursingEvaluation;
                        entityEvalDt.IsEditedByUser = false;
                        entityDt.NursingItemText = String.Empty;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityNursingTransactionEvaluationDtDao.Insert(entityEvalDt);
                    }

                    #endregion
                }
                else
                {
                    #region Add
                    if (paramNursingDiagnoseItemID[i] != "")
                    {
                        entityDt = new NursingTransactionDt();
                        entityDt.TransactionID = transactionID;
                        entityDt.NursingDiagnoseItemID = Convert.ToInt32(paramNursingDiagnoseItemID[i]);
                        entityDt.IsEditedByUser = (paramIsEditedByUser[i] == "true");
                        if (paramIsEditedByUser[i] == "true")
                            entityDt.NursingItemText = paramNursingDiagnoseItemText[i];
                        else
                            entityDt.NursingItemText = String.Empty;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityNursingTransactionDtDao.Insert(entityDt);
                        nursingTransactionDtID = BusinessLayer.GetNursingTransactionDtMaxID(ctx);

                        NursingDiagnoseItem entityDiagnoseItem = entityDiagnoseItemDao.Get(entityDt.NursingDiagnoseItemID);
                        string gcNursingEvaluation = Constant.NursingEvaluation.SUBJECTIVE;
                        if (entityDiagnoseItem != null)
                        {
                            NursingItemGroupSubGroup entityGroup = entityGroupDao.Get(entityDiagnoseItem.NursingItemGroupSubGroupID);
                            gcNursingEvaluation = entityGroup.GCNursingEvaluation;
                        }

                        if ((gcNursingEvaluation == Constant.NursingEvaluation.SUBJECTIVE || gcNursingEvaluation == Constant.NursingEvaluation.OBJECTIVE))
                        {
                            NursingTransactionEvaluationDt entityEvalDt = new NursingTransactionEvaluationDt();
                            entityEvalDt.TransactionID = Convert.ToInt32(MainPage.GetNursingTransactionID());
                            entityEvalDt.NursingTransactionDtID = nursingTransactionDtID;
                            entityEvalDt.NursingDiagnoseItemID = entityDt.NursingDiagnoseItemID;
                            entityEvalDt.GCNursingEvaluation = gcNursingEvaluation;
                            entityEvalDt.IsEditedByUser = false;
                            entityDt.NursingItemText = String.Empty;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityNursingTransactionEvaluationDtDao.Insert(entityEvalDt);
                        }
                    }
                    #endregion
                }
            }

            return MainPage.LoadNursingEvaluationFromNursingDiagnoseItem(ctx);
        }

        public void LoadGridViewByHeaderEntity(Int32 transactionID)
        {
            List<NursingTransactionDt> lstSelectedItem = BusinessLayer.GetNursingTransactionDtList(String.Format("TransactionID = {0}", transactionID));
            List<string> lstSelectedNursingDiagnoseItemID = new List<string>();
            List<string> lstSelectedNursingDiagnoseItemText = new List<string>();
            List<string> lstSelectedIsEditedByUser = new List<string>();
            for (int i = 0; i < lstSelectedItem.Count; i++)
            {
                lstSelectedNursingDiagnoseItemID.Add(lstSelectedItem[i].NursingDiagnoseItemID.ToString());
                lstSelectedNursingDiagnoseItemText.Add(lstSelectedItem[i].NursingItemText);
                lstSelectedIsEditedByUser.Add(lstSelectedItem[i].IsEditedByUser.ToString()); 
            }
            hdnListNursingDiagnoseItemID.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemID.ToArray());
            hdnListNursingDiagnoseItemText.Value = "|" + string.Join("|", lstSelectedNursingDiagnoseItemText.ToArray());
            hdnListIsEditedByUser.Value = "|" + string.Join("|", lstSelectedIsEditedByUser.ToArray());

            BindGridView();
            BindGridViewDiagnoseItem();
        }
        #endregion
    }
}