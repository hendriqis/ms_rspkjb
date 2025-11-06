using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VitalSign : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_VITAL_SIGN;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_VITAL_SIGN;
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_VITAL_SIGN;
                    case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_VITAL_SIGN;
                    default: return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_VITAL_SIGN;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_VITAL_SIGN;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_VITAL_SIGN;
                    default: return Constant.MenuCode.Outpatient.DATA_PATIENT_VITAL_SIGN;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.VITAL_SIGN_TRANSACTION;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.VITAL_SIGN_TRANSACTION;
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_VITAL_SIGN;
                    case Constant.Facility.DIAGNOSTIC:
                        if (menuType != null)
                        {
                            string unit = menuType;
                            if (unit == "nt")
                                return Constant.MenuCode.Nutrition.NUTRITION_VITAL_SIGN;
                            else
                                if (unit == "tr")
                                    return Constant.MenuCode.MedicalDiagnostic.VITAL_SIGN_TRANSACTION;
                                else
                                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_VITAL_SIGN;
                        }
                        else
                        {
                            return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_VITAL_SIGN;
                        }
                    case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.PATIENT_PAGE_VITAL_SIGN;
                    case Constant.Module.NURSING: return Constant.MenuCode.Nursing.NURSING_PATIENT_PAGE_VITAL_SIGN_MENU;
                    case Constant.Module.RADIOTHERAPHY: return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_VITAL_SIGN;
                    default: return Constant.MenuCode.Outpatient.PATIENT_PAGE_VITAL_SIGN;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();

            Registration entityR = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            hdnIsFallRisk.Value = entityR.IsFallRisk ? "1" : "0"; 

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);
        }

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

            filterExpression += string.Format("VisitID IN ({0},{3}) AND (ObservationDate BETWEEN '{1}' AND '{2}') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112), cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ObservationDate DESC, ObservationTime DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0},{1}) ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, cvLinkedID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VitalSign/VitalSignEntry.ascx");
            queryString = "";
            popupWidth = 700;
            popupHeight = 500;
            popupHeaderText = "Tanda Vital dan Indikator Lainnya";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VitalSign/VitalSignEntry.ascx");
                queryString = hdnID.Value;
                popupWidth = 700;
                popupHeight = 500;
                popupHeaderText = "Tanda Vital dan Indikator Lainnya";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            if (hdnID.Value != "")
            {
                IDbContext ctx = DbFactory.Configure(true);
                VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);
                RegistrationDao regDao = new RegistrationDao(ctx);
                try
                {
                    VitalSignHd entity = vitalSignHdDao.Get(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    vitalSignHdDao.Update(entity);

                    Registration oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                    if (oRegistration != null)
                    {
                        oRegistration.IsFallRisk = false;
                        regDao.Update(oRegistration);
                    }

                    if (entity.TestOrderID != null && entity.TestOrderID != 0)
	                {
                        List<PerioperativeNursing> lst1 = BusinessLayer.GetPerioperativeNursingList(string.Format("TestOrderID = {0} AND IsDeleted = 0",entity.TestOrderID), ctx);
                        if (lst1.Count>0)
	                    {
                           PerioperativeNursingDao perioperativeDao = new PerioperativeNursingDao(ctx);
                            foreach (PerioperativeNursing item in lst1)
	                        {
		                        if (item.PreOperativeVitalSignID == entity.ID)
		                            item.PreOperativeVitalSignID = null;
	                            if (item.PostOperativeVitalSignID == entity.ID)
                                    item.PostOperativeVitalSignID = null;
                                perioperativeDao.Update(item);
	                        }
	                    }
	                }

                    result = true;
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    result = false;
                    errMessage = ex.Message.ToString();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }
    }
}