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
    public partial class NursingInterventionItemEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override void InitializeDataControl(string param)
        {
            hdnNursingInterventionID.Value = param;
            NursingIntervention entity = BusinessLayer.GetNursingIntervention(Convert.ToInt32(hdnNursingInterventionID.Value));
            txtNursingDiagnoseName.Text = entity.NurseInterventionName;

            List<StandardCode> lstStdCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.NURSING_INTERVENTION_ITEM_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboGCNursingItemType, lstStdCode.Where(lst => lst.ParentID == Constant.StandardCode.NURSING_INTERVENTION_ITEM_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            BindGridViewDiagnoseItem();

            txtNursingItemEntry.Attributes.Add("validationgroup", "mpEntryPopup");
            txtDisplayOrder.Attributes.Add("validationgroup", "mpEntryPopup");
        }



        private string GetFilterExpressionNursingInterventionItem()
        {
            string filterExpression = String.Format("NursingInterventionID = {0}", hdnNursingInterventionID.Value); //hdnFilterExpression.Value;
            filterExpression += " AND IsDeleted = 0";
            return filterExpression;
        }

        protected string OnGetNursingItemFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }


        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionNursingInterventionItem();

            List<vNursingInterventionItem> lstEntity = BusinessLayer.GetvNursingInterventionItemList(filterExpression);
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

        private void ControlToEntity(NursingInterventionItem entity)
        {
            if (cboGCNursingItemType.Value == null || cboGCNursingItemType.Value == "")
            {
                entity.GCNursingItemType = null;
            }
            else
            {
                entity.GCNursingItemType = cboGCNursingItemType.Value.ToString();
            }
            entity.IsEditableByUser = chkIsEditableByUser.Checked;
            entity.DisplayOrder = txtDisplayOrder.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingInterventionItemDao entityDao = new NursingInterventionItemDao(ctx);
            try
            {
                NursingInterventionItem entity = new NursingInterventionItem();
                entity.NursingInterventionID = Convert.ToInt32(hdnNursingInterventionID.Value);
                entity.NursingItemID = InsertNursingItem(ctx);
                ControlToEntity(entity);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingInterventionItemDao entityDao = new NursingInterventionItemDao(ctx);
            try
            {
                NursingInterventionItem entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.NursingItemID = InsertNursingItem(ctx);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                NursingInterventionItem entity = BusinessLayer.GetNursingInterventionItem(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingInterventionItem(entity);
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