using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplateInstructionList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case "EM": return Constant.MenuCode.EMR.MASTER_PHYSICIAN_INSTRUCTION_TEMPLATE;
                case "MR": return Constant.MenuCode.MedicalRecord.PATIENT_INSTRUCTION;
                default: return Constant.MenuCode.MedicalRecord.PATIENT_INSTRUCTION;
            }
            
          
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnDepartmentID.Value = Page.Request.QueryString["id"];
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvInstructionRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Template Instruction", "Template Code", "Template Group" };
            fieldListValue = new string[] { "TemplateInstruction", "TemplateCode", "TextTemplateGroup" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("CreatedBy = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vInstruction> lstEntity = BusinessLayer.GetvInstructionList(string.Format(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
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
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            url = ResolveUrl(string.Format("~/Libs/Program/Master/PhysicianText/TemplateInstructionEntry.aspx?id={0}", MenuID));
            return true;
        }
        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/PhysicianText/TemplateInstructionEntry.aspx?id={0}|{1}", MenuID, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                Instruction entity = BusinessLayer.GetInstruction(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateInstruction(entity);
                return true;
            }
            return false;
        }
    }
}