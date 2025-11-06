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
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class TransferPrescriptionOrderList : BasePageCheckRegisteredPatient
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PH_TRANSFER_PRESCRIPTION_ORDER;
        }
        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
        public override bool IsShowRightPanel()
        {
            return false;
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "10";

        protected override void InitializeDataControl()
        {
            string paramFilter = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL, Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT);

            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(paramFilter);
            if (lstParam.Count > 0)
            {
                hdnIsUsingUDD.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT).FirstOrDefault().ParameterValue;
                refreshGridInterval = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
            }

            //string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE);
            //List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            //Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            //cboPrescriptionType.SelectedIndex = 0;

            string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1 ", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);
            //if (hdnIsUsingUDD.Value == "1")
            //    filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsInpatientDispensary = 0 AND IsUsingRegistration = 1 ", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);

            List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitOrder, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboToServiceUnitOrder, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnitOrder.SelectedIndex = 1;

            string deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY);
            if (hdnIsUsingUDD.Value == "1")
                deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID NOT IN ('{0}','{1}')", Constant.Facility.PHARMACY, Constant.Facility.INPATIENT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
            List<Department> lstDepartmentOrder = BusinessLayer.GetDepartmentList(deptFilter);
            lstDepartmentOrder.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<Department>(cboDepartmentOrder, lstDepartmentOrder, "DepartmentName", "DepartmentID");
            cboDepartmentOrder.SelectedIndex = 0;

            txtTestOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SettingControlProperties();
            grdOrderPatient.InitializeControl();

            Helper.SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, true, false), "mpPatientListOrder");
            Helper.SetControlEntrySetting(cboDepartmentOrder, new ControlEntrySetting(true, true, false), "mpPatientListOrder");

            hdnPageTitle.Value = BusinessLayer.GetvMenuList(string.Format("MenuCode = '{0}'", this.OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        private void SettingControlProperties()
        {
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("PrescriptionDate = '{0}' AND GCVisitStatus NOT IN ('{2}','{3}','{4}') AND GCTransactionStatus IN ('{5}','{6}')",
                Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                cboServiceUnitOrder.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.APPROVED);

            if (cboServiceUnitOrder.Value.ToString() != "")
                filterExpression += string.Format(" AND DispensaryServiceUnitID = {0}", cboServiceUnitOrder.Value);
            if (cboDepartmentOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartmentOrder.Value);
            if (hdnServiceUnitIDOrder.Value != "0" && hdnServiceUnitIDOrder.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = '{0}'", hdnServiceUnitIDOrder.Value);
            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);
            return filterExpression;
            //return "1 = 0";
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "transfer")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderChangesLogDao changesLogDao = new PrescriptionOrderChangesLogDao(ctx);
                try
                {
                    List<vPrescriptionOrderHd4> lstOrder = BusinessLayer.GetvPrescriptionOrderHd4List(string.Format("PrescriptionOrderID IN ({0})", hdnParam.Value), ctx);

                    DateTime logDate = DateTime.Now.Date;
                    string logTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);


                    Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboToServiceUnitOrder.Value),ctx).FirstOrDefault();
                    int locationID = 0;
                    if (location != null)
                    {
                        locationID = location.LocationID;
                    }

                    if (lstOrder.FirstOrDefault().GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        foreach (vPrescriptionOrderHd4 order in lstOrder)
                        {
                            PrescriptionOrderChangesLog log = new PrescriptionOrderChangesLog();
                            log.VisitID = order.VisitID;
                            log.PrescriptionOrderID = order.PrescriptionOrderID;
                            log.GCPrescriptionOrderChangeType = Constant.PrescriptionOrderChangesType.DISPENSARY;
                            log.HealthcareServiceUnitID = order.DispensaryServiceUnitID;
                            log.LogDate = logDate;
                            log.LogTime = logTime;
                            log.NoteText = string.Format("Perubahan Lokasi Farmasi dari {0} ke {1} (Log dibuat oleh system)", cboServiceUnitOrder.Text, cboToServiceUnitOrder.Text);
                            log.IsNeedConfirmation = false;
                            log.IsAutoGeneratedBySystem = true;
                            log.CreatedBy = AppSession.UserLogin.UserID;

                            changesLogDao.Insert(log);

                            PrescriptionOrderHd oHeader = orderHdDao.Get(order.PrescriptionOrderID);

                            oHeader.DispensaryServiceUnitID = Convert.ToInt32(cboToServiceUnitOrder.Value.ToString());
                            oHeader.LocationID = locationID;
                            oHeader.LastUpdatedBy = AppSession.UserLogin.UserID;
                            orderHdDao.Update(oHeader);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Transfer order farmasi tidak dapat dilakukan karena order udah direalisasi/dibatalkan. Harap refresh halaman ini.";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }
    }
}