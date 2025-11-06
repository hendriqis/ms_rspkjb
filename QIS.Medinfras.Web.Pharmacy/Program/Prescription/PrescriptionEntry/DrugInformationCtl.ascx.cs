using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DrugInformationCtl : BaseViewPopupCtl
    {

        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.COENAM_RULE));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboCoenamRuleCtl, lstStandardCode, "StandardCodeName", "StandardCodeID");
        }

        protected string OnGetItemGroupFilterExpression()
        {
            string filterExpression = string.Format("GCItemType = '{0}' AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS);
            return filterExpression;
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpressionQuickSearch.Value;
            if (filterExpression != "")
            {
                filterExpression += string.Format(" AND GCItemType = '{0}' AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS);
            }
            if (hdnItemGroupID.Value != "" && filterExpression != "")
            {
                filterExpression += string.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%')", hdnItemGroupID.Value);
            }
            else if (hdnItemGroupID.Value != "" && filterExpression == "") {
                filterExpression = string.Format("ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%')", hdnItemGroupID.Value);           
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (!string.IsNullOrEmpty(filterExpression))
            {
                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvDrugInfoRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, 8);
                }
                List<vDrugInfo> lstEntity = BusinessLayer.GetvDrugInfoList(filterExpression, 8, pageIndex, "ItemCode ASC");
                grdView.DataSource = lstEntity;
                grdView.DataBind(); 
            }
        }

        protected void cbpViewCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnIDCtl.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(DrugInfo entityDrug)
        {
            #region Drug Information
            if (entityDrug != null)
            {
                if (cboCoenamRuleCtl.Value != null && cboCoenamRuleCtl.Value.ToString() != "")
                {
                    entityDrug.GCCoenamRule = cboCoenamRuleCtl.Value.ToString();
                }
                else
                {
                    entityDrug.GCCoenamRule = null;
                }

                entityDrug.MedicationPurpose = Request.Form[txtPurposeOfMedication.UniqueID];
                entityDrug.MedicationAdministration = Request.Form[txtMedicationAdministration.UniqueID];
            }
            #endregion
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DrugInfoDao entityDao = new DrugInfoDao(ctx);
            try
            {
                int itemID = Convert.ToInt32(hdnIDCtl.Value);
                DrugInfo entity = entityDao.Get(itemID);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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
    }
}