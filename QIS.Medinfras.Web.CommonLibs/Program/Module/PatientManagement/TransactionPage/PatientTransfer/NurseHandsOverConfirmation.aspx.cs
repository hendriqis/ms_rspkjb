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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NurseHandsOverConfirmation : BasePageTrx
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.IMAGING:
                        return Constant.MenuCode.Imaging.PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Facility.LABORATORY:
                        return Constant.MenuCode.Laboratory.PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_PATIENT_HANDOVER_CONFIRMATION;
                    default:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region List
        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                    deptType = param[0];
                }

                Registration reg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (reg.LinkedRegistrationID != null && reg.LinkedRegistrationID != 0)
                {
                    string filterVisit = string.Format("RegistrationID = {0}", reg.LinkedRegistrationID);
                    ConsultVisit cv = BusinessLayer.GetConsultVisitList(filterVisit).FirstOrDefault();
                    hdnVisitIDFrom.Value = cv.VisitID.ToString();
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<Registration> dataRegID = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} OR LinkedToRegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
            string lstReg = "";
            if (dataRegID != null)
            {
                foreach (Registration reg in dataRegID)
                {
                    if (lstReg != "")
                    {
                        lstReg += ",";
                    }
                    lstReg += reg.RegistrationID;
                }
            }

            string filterExpression = hdnFilterExpression.Value;
            if (!String.IsNullOrEmpty(hdnVisitIDFrom.Value))
            {
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("RegistrationID IN ({0}) AND IsConfirmed = 0 AND ToNurseID = {1} AND IsDeleted = 0", lstReg, AppSession.UserLogin.ParamedicID);
            }
            else
            {
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("VisitID = {0} AND IsConfirmed = 0 AND ToNurseID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientNurseTransferRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientNurseTransfer> lstEntity = BusinessLayer.GetvPatientNurseTransferList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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

            if (param[0] == "complete")
            {
                result = ProcessCheckedItem(param[0], lstRecordID);
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string ProcessCheckedItem(string type, string lstRecordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientNurseTransferDao entityDao = new PatientNurseTransferDao(ctx);
            string filterExpression = string.Format("ID IN ({0})", lstRecordID);

            try
            {
                if (AppSession.UserLogin.ParamedicID != null)
                {
                    //Confirm
                    List<PatientNurseTransfer> oList = BusinessLayer.GetPatientNurseTransferList(filterExpression, ctx);
                    foreach (PatientNurseTransfer item in oList)
                    {
                        if (!item.IsConfirmed && AppSession.UserLogin.ParamedicID != null)
                        {
                            item.IsConfirmed = true;
                            item.ConfirmationDate = DateTime.Now;
                            entityDao.Update(item);
                        }
                    }
                    ctx.CommitTransaction();
                    result = string.Format("process|1|{0}", string.Empty);
                }
                else
                {
                    result = string.Format("process|0|{0}", "Invalid Paramedic / Nurse ID");
                    ctx.RollBackTransaction();
                }
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

        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
        }

        private void ControlToEntity(PatientNurseTransfer entity)
        {
        }
        #endregion
    }
}