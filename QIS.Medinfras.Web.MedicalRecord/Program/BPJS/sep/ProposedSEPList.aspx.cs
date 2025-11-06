using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Text;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class ProposedSEPList : BasePageCheckRegisteredPatient //BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BPJS_SEP_APPROVAL;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected override void InitializeDataControl()
        {
            hdnIsBridgingBPJSVClaimVersion.Value = AppSession.SA0167;
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration = 1", Constant.Facility.PHARMACY));

            lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
            cboPatientFrom.SelectedIndex = 0;

            Helper.SetControlEntrySetting(txtFromRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtToRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

            txtFromRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            grdRegisteredPatient.InitializeControl();

            hdnPageTitle.Value = BusinessLayer.GetvMenuList(string.Format("MenuCode = '{0}'", this.OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Empty;

            if (cboPatientFrom.Value == null)
                filterExpression = string.Format("DepartmentID != '{0}' AND (VisitDate BETWEEN '{1}' AND '{2}') AND GCVisitStatus != '{3}'", Constant.Facility.PHARMACY, Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            else
                filterExpression = string.Format("DepartmentID = '{0}'  AND (VisitDate BETWEEN '{1}' AND '{2}') AND GCVisitStatus != '{3}'", cboPatientFrom.Value, Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);

            filterExpression += string.Format(" AND GCSEPStatus = '{0}'", Constant.SEP_Status.PENGAJUAN);

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (!string.IsNullOrEmpty(hdnParam.Value))
            {
                string filterExpression = string.Format("VisitID IN ({0})", hdnParam.Value);
                bool isHasSuccess = false;
                StringBuilder message = new StringBuilder();
                List<vConsultVisitBPJS> lstVisit = BusinessLayer.GetvConsultVisitBPJSList(filterExpression);
                if (lstVisit.Count > 0)
                {
                    foreach (vConsultVisitBPJS oVisit in lstVisit)
                    {
                        string noPeserta = oVisit.NoPeserta;
                        DateTime tglSEP = oVisit.ActualVisitDate;
                        string jnsPelayanan = oVisit.DepartmentID == Constant.Facility.INPATIENT ? "1" : "2";
                        string keterangan = oVisit.Catatan;
                        string user = AppSession.UserLogin.UserName;

                        try
                        {
                            BPJSService oService = new BPJSService();

                            if (hdnIsBridgingBPJSVClaimVersion.Value == Constant.BPJS_Version_Release.v2_0)
                            {
                                string apiResult = oService.ApprovalSEP_MEDINFRASAPI(noPeserta, tglSEP, jnsPelayanan, keterangan).ToString();
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "1")
                                {
                                    //Update Registration BPJS
                                    RegistrationBPJS entity = BusinessLayer.GetRegistrationBPJS(oVisit.RegistrationID);
                                    entity.GCSEPStatus = Constant.SEP_Status.DISETUJUI;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    BusinessLayer.UpdateRegistrationBPJS(entity);

                                    isHasSuccess = true;
                                }
                                else
                                {
                                    message.AppendLine(apiResultInfo[2]);
                                }
                            }
                            else
                            {
                                string apiResult = oService.ApprovalSEP(noPeserta, tglSEP, jnsPelayanan, keterangan).ToString();
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "1")
                                {
                                    //Update Registration BPJS
                                    RegistrationBPJS entity = BusinessLayer.GetRegistrationBPJS(oVisit.RegistrationID);
                                    entity.GCSEPStatus = Constant.SEP_Status.DISETUJUI;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    BusinessLayer.UpdateRegistrationBPJS(entity);

                                    isHasSuccess = true;
                                }
                                else
                                {
                                    message.AppendLine(apiResultInfo[2]);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errMessage = ex.Message;
                            Helper.InsertErrorLog(ex);
                        }
                    }

                    errMessage = message.ToString();
                    return isHasSuccess;
                }
                else
                {
                    errMessage = "There is no record to be processed !";
                    return false; 
                }
            }
            else
            {
                errMessage = "There is no record to be processed !";
                return false;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}