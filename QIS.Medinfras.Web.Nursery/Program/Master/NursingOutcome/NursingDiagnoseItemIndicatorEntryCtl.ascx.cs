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
    public partial class NursingDiagnoseItemIndicatorEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override void InitializeDataControl(string param)
        {
            hdnNursingDiagnoseItemID.Value = param;
            vNursingDiagnoseItem entity = BusinessLayer.GetvNursingDiagnoseItemList(String.Format("NursingDiagnoseItemID = {0}",Convert.ToInt32(hdnNursingDiagnoseItemID.Value))).FirstOrDefault();
            txtNursingDiagnoseName.Text = entity.NurseDiagnoseName;
            txtDiagnosisItem.Text = entity.NursingItemText;

            vNursingItemGroupSubGroup entityGroup = BusinessLayer.GetvNursingItemGroupSubGroupList(String.Format("NursingItemGroupSubGroupID = {0}", entity.NursingItemGroupSubGroupID)).FirstOrDefault();
            txtGroup.Text = String.Format("{0} >> {1}",entityGroup.ParentText,entityGroup.NursingItemGroupSubGroupText);

            BindGridViewDiagnoseItem();

            txtNursingIndicatorCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtNursingIndicatorText.Attributes.Add("validationgroup", "mpEntryPopup");
            
        }



        private string GetFilterExpressionNursingInterventionItem()
        {
            string filterExpression = String.Format("NursingDiagnoseItemID = {0}", hdnNursingDiagnoseItemID.Value); //hdnFilterExpression.Value;
            return filterExpression;
        }

        protected string OnGetNursingIndicatorFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }


        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = GetFilterExpressionNursingInterventionItem();

            List<vNursingDiagnoseItemIndicator> lstEntity = BusinessLayer.GetvNursingDiagnoseItemIndicatorList(filterExpression);
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

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingDiagnoseItemIndicatorDao entityDao = new NursingDiagnoseItemIndicatorDao(ctx);
            try
            {
                NursingDiagnoseItemIndicator entity = new NursingDiagnoseItemIndicator();
                entity.NursingDiagnoseItemID = Convert.ToInt32(hdnNursingDiagnoseItemID.Value);
                entity.NursingIndicatorID = InsertNursingIndicator(ctx);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
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
            try
            {
                InsertNursingIndicator(ctx);
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
                BusinessLayer.DeleteNursingDiagnoseItemIndicator(Convert.ToInt32(hdnID.Value));
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void ControlToEntityNursingIndicator(NursingIndicator entity)
        {
            entity.NursingIndicatorText = txtNursingIndicatorText.Text;
            entity.Scale1Text = txtScale1Text.Text;
            entity.Scale2Text = txtScale2Text.Text;
            entity.Scale3Text = txtScale3Text.Text;
            entity.Scale4Text = txtScale4Text.Text;
            entity.Scale5Text = txtScale5Text.Text;
        }

        private int InsertNursingIndicator(IDbContext ctx)
        {
            NursingIndicatorDao entityDao = new NursingIndicatorDao(ctx);
            int result = 0;
            try
            {
                NursingIndicator entity = BusinessLayer.GetNursingIndicatorList(String.Format("LOWER(NursingIndicatorCode) = '{0}'", txtNursingIndicatorCode.Text.ToLower()),ctx).FirstOrDefault();
                if (entity != null)
                {
                    ControlToEntityNursingIndicator(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                    result = entity.NursingIndicatorID;
                }
                else
                {
                    entity = new NursingIndicator();
                    entity.NursingIndicatorCode = txtNursingIndicatorCode.Text;
                    ControlToEntityNursingIndicator(entity);
                    entity.IsDeleted = false;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    result = BusinessLayer.GetNursingIndicatorMaxID(ctx);

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