using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingDiagnoseItemEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnNursingDiagnoseID.Value = paramInfo[0];
            hdnNursingDiagnosisType.Value = paramInfo[1];
            NursingDiagnose entity = BusinessLayer.GetNursingDiagnose(Convert.ToInt32(hdnNursingDiagnoseID.Value));
            txtNursingDiagnoseName.Text = entity.NurseDiagnoseName;

            BindGridView(CurrPage, true, ref PageCount);
            BindGridViewDiagnoseItem();

            txtNursingItemEntry.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("GCNursingDiagnosisType = '{0}' AND IsDeleted = 0", hdnNursingDiagnosisType.Value);
            return filterExpression;
        }

        private string GetFilterExpressionDiagnoseItem()
        {
            string filterExpression = String.Format("NursingDiagnoseID = {0}",hdnNursingDiagnoseID.Value); //hdnFilterExpression.Value;
            if (hdnItemGroupID.Value != "")
                filterExpression += String.Format(" AND NursingItemGroupSubGroupID = {0} ",hdnItemGroupID.Value);
            filterExpression += " AND IsDeleted = 0";
            return filterExpression;
        }

        protected string OnGetNursingItemFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetNursingItemGroupSubGroupRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<NursingItemGroupSubGroup> lstEntity = BusinessLayer.GetNursingItemGroupSubGroupList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex,"NursingItemGroupSubGroupCode");
            grdEntryPopupGrdView.DataSource = lstEntity;
            grdEntryPopupGrdView.DataBind();
        }


        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionDiagnoseItem();

            List<vNursingDiagnoseItem> lstEntity = BusinessLayer.GetvNursingDiagnoseItemList(filterExpression);
            grdSaved.DataSource = lstEntity;
            grdSaved.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            int pageCount = 1;
            string result = param[0] + "|";
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

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpEntryPopupView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";
            if (param[0] == "save")
            {
                if (hdnIsAdd.Value.ToString() == "0")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridViewDiagnoseItem();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(NursingDiagnoseItem entity)
        {
            if (hdnNursingOutcomeID.Value != "" && hdnNursingOutcomeID.Value != "0")
                entity.NursingOutcomeID = Convert.ToInt32(hdnNursingOutcomeID.Value);
            if (hdnIsSubjectiveObjectiveData.Value == "1")
            {
                if (rblIsSubjectiveObjectiveData.SelectedValue == "1")
                {
                    entity.IsMajorData = true;
                    entity.IsMinorData = false;
                }
                else
                {
                    entity.IsMajorData = false;
                    entity.IsMinorData = true;
                }
            }
            entity.IsUsingIndicator = hdnIsNursingOutcome.Value == "True" ? true : chkIsUsingIndicator.Checked;
            entity.NursingOutcomeResult = txtOutcomeResult.Text;
            entity.IsEditableByUser = chkIsEditableByUser.Checked;            
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingDiagnoseItemDao entityDao = new NursingDiagnoseItemDao(ctx);
            try
            {
                NursingDiagnoseItem entity = new NursingDiagnoseItem();
                entity.NursingDiagnoseID = Convert.ToInt32(hdnNursingDiagnoseID.Value);
                entity.NursingItemGroupSubGroupID = Convert.ToInt32(hdnItemGroupID.Value);
                entity.NursingItemID = InsertNursingItem(ctx);
                ControlToEntity(entity);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNursingDiagnoseItem(entity);
                result = true;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingDiagnoseItemDao entityDao = new NursingDiagnoseItemDao(ctx);
            try
            {
                NursingDiagnoseItem entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.NursingItemID = InsertNursingItem(ctx);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingDiagnoseItem(entity);
                result =  true;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result =  false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                NursingDiagnoseItem entity = BusinessLayer.GetNursingDiagnoseItem(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingDiagnoseItem(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private int InsertNursingItem(IDbContext ctx)
        {
            NursingItemDao entityDao = new NursingItemDao(ctx);
            int result = 0;
            try
            {
                NursingItem entity = BusinessLayer.GetNursingItemList(String.Format("LOWER(NursingItemText) = '{0}'", txtNursingItemEntry.Text.ToLower()),ctx).FirstOrDefault();
                if (entity != null)
                {
                    entity.NursingItemText = txtNursingItemEntry.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                    result = entity.NursingItemID;
                }
                else
                {
                    entity = new NursingItem();
                    entity.NursingItemText = txtNursingItemEntry.Text;
                    entity.IsDeleted = false;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    result = BusinessLayer.GetNursingItemMaxID(ctx);
                }
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
            
        }
       
    }
}