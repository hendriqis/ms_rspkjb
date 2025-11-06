using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class CencusInformation : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.CENCUS_PER_DAY;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtLogDate.Text = DateTime.Now.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_ATD_STATUS_GROUP));
            lstStandardCode.Insert(6, new StandardCode { StandardCodeName = "Pasien Sisa", StandardCodeID = "0" });
            Methods.SetComboBoxField(cboGCATDStatus,lstStandardCode,"StandardCodeName","StandardCodeID");
            cboGCATDStatus.SelectedIndex = 0;

            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "");
            lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 1;

            BindGridView(1, true, ref PageCount);
            BindGridView2();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";

            if (cboGCATDStatus.Value.ToString() != "0")
            {
                if (txtLogDate.Text != "") filterExpression = String.Format("LogDate = '{0}'", Helper.GetDatePickerValue(txtLogDate.Text));
                if (cboGCATDStatus.Value != null && cboGCATDStatus.Value.ToString() != "")
                {
                    switch (cboGCATDStatus.Value.ToString())
                    {
                        case Constant.ATD_STATUS_GROUP.PATIENT_IN:
                            filterExpression += String.Format(" AND GCATDStatus = '{0}'", Constant.ATD_STATUS.ADMISSION);
                            break;
                        case Constant.ATD_STATUS_GROUP.PATIENT_TRANSFER_IN:
                            filterExpression += String.Format(" AND GCATDStatus = '{0}'", Constant.ATD_STATUS.TRANSFER_IN);
                            break;
                        case Constant.ATD_STATUS_GROUP.PATIENT_TRANSFER_OUT:
                            filterExpression += String.Format(" AND GCATDStatus = '{0}'", Constant.ATD_STATUS.TRANSFER_OUT);
                            break;
                        case Constant.ATD_STATUS_GROUP.PATIENT_OUT:
                            filterExpression += String.Format(" AND GCATDStatus IN ('{0}','{1}','{2}','{3}')", Constant.ATD_STATUS.DISCHARGED_WITH_AGREEMENT, Constant.ATD_STATUS.DISCHARGED_TO_OTHER_FACILITY, Constant.ATD_STATUS.DISCHARGED_TO_ANOTHER_HOSPITAL, Constant.ATD_STATUS.DISCHARGED_FORCED_OUT);
                            break;
                        case Constant.ATD_STATUS_GROUP.DIED_BEFORE_48_HR:
                            filterExpression += String.Format(" AND GCATDStatus = '{0}'", Constant.ATD_STATUS.DIED_BEFORE_48_HR);
                            break;
                        case Constant.ATD_STATUS_GROUP.DIED_AFTER_48_HR:
                            filterExpression += String.Format(" AND GCATDStatus = '{0}'", Constant.ATD_STATUS.DIED_AFTER_48_HR);
                            break;
                        default:
                            break;
                    }
                }
                if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "" && cboServiceUnit.Value.ToString() != "0")
                {
                    filterExpression += String.Format(" AND HealthcareServiceUnitID = {0} ", cboServiceUnit.Value);
                }
                else
                {
                    filterExpression += String.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = 'INPATIENT' AND IsDeleted = 0)");
                }

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientATDLogRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vPatientATDLog> lstEntity = BusinessLayer.GetvPatientATDLogList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RoomCode,RegistrationNo");
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
            else
            {
                DateTime logDate = Helper.GetDatePickerValue(txtLogDate.Text);
                Int32 healthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);

                List<GetCencusInformationPasienSisa> lstEntity = BusinessLayer.GetCencusInformationPasienSisaList(logDate, healthcareServiceUnitID);
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }

        protected void cbpView2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView2();
        }

        private void BindGridView2()
        {
            List<PatientCencusInformation> lstEntity = BusinessLayer.GetCencusInformationList(Helper.GetDatePickerValue(txtLogDate.Text), Convert.ToInt32(cboServiceUnit.Value));
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            if (lstEntity.Count > 0)
            {
                decimal totalPatientBegin = lstEntity.Sum(p => p.PatientBegin);
                decimal totalTransferIN = lstEntity.Sum(p => p.TransferIN);
                decimal totalAdmission = lstEntity.Sum(p => p.Admission);
                decimal totalTransferOUT = lstEntity.Sum(p => p.TransferOUT);
                decimal totalDischarged = lstEntity.Sum(p => p.Discharged);
                decimal totalDied1 = lstEntity.Sum(p => p.Died1);
                decimal totalDied2 = lstEntity.Sum(p => p.Died2);
                decimal totalStay = lstEntity.Sum(p => p.Stay);

                ((HtmlTableCell)lvwView.FindControl("tdTotalPatientBegin")).InnerHtml = totalPatientBegin > 0 ? totalPatientBegin.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalTransferIN")).InnerHtml = totalTransferIN > 0 ? totalTransferIN.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalAdmission")).InnerHtml = totalAdmission > 0 ? totalAdmission.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalTransferOUT")).InnerHtml = totalTransferOUT > 0 ? totalTransferOUT.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDischarged")).InnerHtml = totalDischarged > 0 ? totalDischarged.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDied1")).InnerHtml = totalDied1 > 0 ? totalDied1.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDied2")).InnerHtml = totalDied2 > 0 ? totalDied2.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalStay")).InnerHtml = totalStay > 0 ? totalStay.ToString() : "-";
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}