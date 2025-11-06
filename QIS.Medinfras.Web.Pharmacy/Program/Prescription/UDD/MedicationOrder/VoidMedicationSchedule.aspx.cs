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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class VoidMedicationSchedule : BasePageTrx
    {
        string menuType = string.Empty;
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            string menuCode = Constant.MenuCode.Pharmacy.UDD_MEDICATION_SCHEDULE_VOID;
            return menuCode;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Length > 1)
                {
                    hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
                    hdnSubMenuType.Value = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
                }
            }

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionID IS NULL AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            filterExpression += string.Format(" ORDER BY DrugName ASC, MedicationDate ASC, SequenceNo ASC");

            List<vMedicationScheduleVoid> lstEntity = BusinessLayer.GetvMedicationScheduleVoidList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCustomProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            if (param[0] == "void")
            {
                result = CompleteVoidMedication(param[0], lstRecordID);
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string CompleteVoidMedication(string type, string lstRecordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            MedicationScheduleDao entityDao = new MedicationScheduleDao(ctx);
            try
            {
                string filterExpression = string.Format("ID IN ({0})", lstRecordID);
                List<MedicationSchedule> oList = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                foreach (MedicationSchedule entity in oList)
                {
                    entity.IsDeleted = true;
                    entity.GCDeleteReason = Constant.DeleteReason.OTHER;
                    entity.DeleteReason = "Deleted from Void Medication Schedule : " + txtDeleteReason.Text;
                    entity.DeleteBy = AppSession.UserLogin.UserID;
                    entity.DeleteDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                }

                ctx.CommitTransaction();

                result = string.Format("process|1|{0}", string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
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

    }
}