using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientMedicationList : BasePageTrx
    {
        protected int PageCount = 1;

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnPhysicianID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";//hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            String TransactionStatus = String.Format("'{0}','{1}'", Constant.TestOrderStatus.OPEN, Constant.TestOrderStatus.CANCELLED);
            filterExpression += string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);

            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvMedicationChartSummaryRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            //}

            List<EpisodeMedication> lstEntity = BusinessLayer.GetEpisodeMedicationList(AppSession.RegisteredPatient.VisitID.ToString());
            lstEntity = lstEntity.OrderBy(lst => lst.DrugName).ToList();
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
            pageCount = lstEntity.Count() > 0 ? pageCount : 0;
        }


        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                EpisodeMedication entity = e.Item.DataItem as EpisodeMedication;

                if (entity.IsHomeMedication)
                {
                    HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemName");
                    tr.Attributes.Add("class", "externalMedicationColor");
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            string menuCode = string.Empty;
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT:
                    menuCode = Constant.MenuCode.Inpatient.MEDICATION_LIST;
                    break;
                case Constant.Facility.PHARMACY:
                    menuCode = Constant.MenuCode.Pharmacy.UDD_MEDICATION_RECONCILIATION;
                    break;
                default:
                    break;
            }
            return menuCode;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            try
            {

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            finally
            {

            }
            return result;
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
    }
}