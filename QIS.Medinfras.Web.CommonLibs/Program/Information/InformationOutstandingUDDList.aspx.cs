using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.ReportDesktop;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationOutstandingUDDList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.INFORMATION_OUTSTANDING_UDD_LIST;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            txtRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstSequence = new List<Variable>();
            lstSequence.Add(new Variable() { Code = "1", Value = "1" });
            lstSequence.Add(new Variable() { Code = "2", Value = "2" });
            lstSequence.Add(new Variable() { Code = "3", Value = "3" });
            lstSequence.Add(new Variable() { Code = "4", Value = "4" });
            lstSequence.Add(new Variable() { Code = "5", Value = "5" });
            lstSequence.Add(new Variable() { Code = "6", Value = "6" });
            Methods.SetComboBoxField<Variable>(cboSequence, lstSequence, "Value", "Code");
            cboSequence.SelectedIndex = 0;

            string filterExpression = string.Format("IsInpatientDispensary = 1");
            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.PHARMACY, filterExpression);
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh|";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string regDate = Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
            List<GetOutstandingUDDRegistrationList> lstEntity = BusinessLayer.GetOutstandingUDDRegistrationList(Convert.ToInt32(hdnServiceUnitID.Value), regDate, Convert.ToInt32(cboSequence.Value.ToString()));
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetOutstandingUDDRegistrationList entity = e.Item.DataItem as GetOutstandingUDDRegistrationList;
                Repeater rptItemName = e.Item.FindControl("rptItemName") as Repeater;

                string[] lstItem = entity.ItemList.Split('~');
                List<TempItemClass> lstItemObj = new List<TempItemClass>();
                for (int i = 0; i < lstItem.Length; i++)
                {
                    TempItemClass obj = new TempItemClass();
                    obj.NamaItem = lstItem[i];
                    lstItemObj.Add(obj);
                }

                rptItemName.DataSource = lstItemObj;
                rptItemName.DataBind();
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        public class TempItemClass
        {
            public String NamaItem { get; set; }
        }
    }
}