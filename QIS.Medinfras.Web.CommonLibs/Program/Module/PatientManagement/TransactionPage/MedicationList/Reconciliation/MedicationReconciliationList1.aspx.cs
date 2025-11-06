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
    public partial class MedicationReconciliationList1 : BasePagePatientPageList
    {
        string deptType = string.Empty;
        protected int PageCount = 1;

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
            }
            else
            {
                deptType = param[0];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnMedicalNo.Value = AppSession.RegisteredPatient.MedicalNo;
            hdnPatientName.Value = AppSession.RegisteredPatient.PatientName;
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            BindGridView(1, true, ref PageCount);
        }


        public override string OnGetMenuCode()
        {
            switch (deptType.ToUpper())
            {
                case Constant.Module.PHARMACY:
                    return Constant.MenuCode.Pharmacy.MEDICATION_RECONCILIATION;
                default:
                    return Constant.MenuCode.Pharmacy.MEDICATION_RECONCILIATION;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/MedicationList/Reconciliation/AddMedicationReconciliationCtl.ascx");
            queryString = "0" + "|";
            popupWidth = 700;
            popupHeight = 500;
            popupHeaderText = "Rekonsiliasi Obat";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "" && hdnID.Value != "0")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/MedicationList/Reconciliation/AddMedicationReconciliationCtl.ascx");
                queryString = hdnID.Value + "|";
                popupWidth = 700;
                popupHeight = 500;
                popupHeaderText = "Rekonsiliasi Obat";
                return true;
            }
            else
            {
                errMessage = "Tidak ada record yang dapat dilakukan perubahan.";
                return false;
            }
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "" && hdnID.Value != "0")
            {
                PastMedication entity = BusinessLayer.GetPastMedication(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePastMedication(entity);
                return true;
            }
            else
            {
                errMessage = "Tidak ada record yang dapat dihapus.";
                return false;
            }
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            if (hdnIsContinueToInpatient.Value == "True")
            {
                //Validate if Medication has already have schedule or not
                List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(string.Format("PastMedicationID = {0} AND IsDeleted = 0", hdnID.Value));
                if (lstSchedule.Count > 0)
                {
                    errMessage = "Sudah dilakukan penjadwalan pemberian obat di ruangan. Jadwal harus dihapus terlebih dahulu";
                    return false;
                } 
            }

            return true;
        }

        #region Grid Binding
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            int cvLinkedID = 0;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }
            }

            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND IsMedicationReconciliation = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPastMedicationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPastMedication> lstEntity = BusinessLayer.GetvPastMedicationList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (lstEntity.Count == 0)
                hdnID.Value = "0";
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
    }
}