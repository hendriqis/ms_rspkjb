using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GeneratePatientFromGuest : BasePageTrx
    {
        private GetUserMenuAccess menu;
        private string refreshGridInterval = "";
        private int setvarRetensi = 0;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.GENERATE_MEDICAL_NUMBER_FROM_GUEST;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected override void InitializeDataControl()
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Common.Constant.SettingParameter.RM_DEFAULT_MONTH_FOR_RETENTION);
                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExp);
                //txtLastVisitDate.Text = DateTime.Today.AddMonths(Convert.ToInt32(lstParam[0].ParameterValue) * -1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                setvarRetensi = Convert.ToInt32(lstParam[0].ParameterValue);
                menu = ((MPMain)((MPTrx)Master).Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                BindGridView();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            List<GetGenerateGuestToMedicalRecord> lstEntity = BusinessLayer.GetGenerateGuestToMedicalRecord(!string.IsNullOrEmpty(hdnFilterExpressionQuickSearch.Value) ? hdnFilterExpressionQuickSearch.Value : string.Empty);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }


        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetGenerateGuestToMedicalRecord entity = e.Item.DataItem as GetGenerateGuestToMedicalRecord;
                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                hdnKey.Value = entity.GuestID.ToString();
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityPatientDao = new PatientDao(ctx);
            try
            {
                List<Patient> lstEntityPatient = BusinessLayer.GetPatientList(string.Format("MRN IN ({0})", hdnSelectedMember.Value.Replace('|', ',')));
                if (type == "archive")
                {
                    foreach (Patient entity in lstEntityPatient)
                    {
                        entity.GCPatientStatus = Constant.PatientStatus.ARCHIVED;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPatientDao.Update(entity);
                    }
                }
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
    }
}