using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class AllergyList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            if (Page.Request.QueryString.Count > 0)
                return Constant.MenuCode.EMR.HEALTH_RECORD_ALLERGIES;
            return Constant.MenuCode.EMR.ALLERGIES;
        }

        #region List
        protected override void InitializeDataControl()
        {
            IsPromptDeleteReason = true;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
            string code = ddlAllergenTypeFilter.SelectedValue;
            if (code != "0")
            {
                String allergenType = "";
                switch (code)
                {
                    case "1":
                        allergenType = Constant.AllergenType.DRUG;
                        break;
                    case "2":
                        allergenType = Constant.AllergenType.ENVIRONMENTAL;
                        break;
                    case "3":
                        allergenType = Constant.AllergenType.POLLEN;
                        break;
                    case "4":
                        allergenType = Constant.AllergenType.FOOD;
                        break;
                    case "5":
                        allergenType = Constant.AllergenType.ANIMAL;
                        break;
                    case "6":
                        allergenType = Constant.AllergenType.MISCELLANEOUS;
                        break;
                }
                filterExpression += string.Format(" AND GCAllergenType = '{0}'", allergenType);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            bool result = true;
            return result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientAllergy entity = BusinessLayer.GetPatientAllergy(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePatientAllergy(entity);
                return true;
            }
            return false;
        }
        #endregion

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Program/PatientPage/Subjective/Allergy/AllergyEntryCtl.ascx");
            queryString = "";
            popupWidth = 700;
            popupHeight = 500;
            popupHeaderText = "Patient Allergy";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/PatientPage/Subjective/Allergy/AllergyEntryCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 700;
                popupHeight = 500;
                popupHeaderText = "Patient Allergy";
                return true;
            }
            return false;
        }

    }
}