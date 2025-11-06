using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ServiceUnitHealthcareEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnServiceUnitID.Value = param;
            ServiceUnitMaster entity = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(hdnServiceUnitID.Value));
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            if (entity.DepartmentID == Constant.Facility.EMERGENCY)
            {
                trChargeClass.Attributes.Remove("style");
            }

            if (entity.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                trIsAutoCloseRegistration.Attributes.Remove("style");
                trChargeClass.Attributes.Remove("style");
            }
            
            if (entity.DepartmentID == Constant.Facility.INPATIENT)
            {
                trItemBedCharges.Attributes.Remove("style");
                trChargeClass.Attributes.Remove("style");
                trNursingServiceItem.Attributes.Remove("style");
                trIsChargeClassEditableForNonInpatient.Attributes.Add("style", "display:none");
            }

            if (entity.DepartmentID == Constant.Facility.DIAGNOSTIC)
            {
                trChargeClass.Attributes.Remove("style");
                trIsAutomaticPrintTracer.Attributes.Remove("style");
            }

            if (entity.DepartmentID == Constant.Facility.PHARMACY)
            {
                trChargeClass.Attributes.Remove("style");
                tblInpatientDispensary.Attributes.Remove("style");
                trIsAutomaticPrintTracer.Attributes.Remove("style");
            }

            BindGridView();

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.CLINIC_GROUP));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboClinicGroup, lstSc, "StandardCodeName", "StandardCodeID");
            cboClinicGroup.SelectedIndex = 0;

            List<ClassCare> listChargeClassID = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0 AND IsUsedInChargeClass = 1"));
            listChargeClassID.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
            Methods.SetComboBoxField<ClassCare>(cboChargeClassID, listChargeClassID, "ClassName", "ClassID");
            cboChargeClassID.SelectedIndex = 0;

            txtHealthcareCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtLocationCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtLogisticLocationCode.Attributes.Add("validationgroup", "mpEntryPopup");
            if (entity.DepartmentID != Constant.Facility.PHARMACY)
            {
                txtDispensaryServiceUnitCode.Attributes.Add("validationgroup", "mpEntryPopup");
                lblDispensaryServiceUnit.Attributes.Add("class", "lblLink lblMandatory");
            }
            else
            {
                lblDispensaryServiceUnit.Attributes.Add("class", "lblLink lblNormal");
            }
            txtServiceInterval.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemBedChargesCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtNursingServiceItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        protected string OnGetItemBedChargesFilterExpression()
        {
            return string.Format("GCItemType = '{0}' AND IsDeleted = 0 AND GCItemStatus != '{1}'", Constant.ItemGroupMaster.SERVICE, Constant.ItemStatus.IN_ACTIVE);
        }

        protected string onGetNursingServiceItemFilterExpression()
        {
            return string.Format("GCItemType = '{0}' AND IsDeleted = 0 AND GCItemStatus != '{1}'", Constant.ItemGroupMaster.SERVICE, Constant.ItemStatus.IN_ACTIVE);
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("ServiceUnitID = '{0}' AND IsDeleted = 0 ORDER BY HealthcareID ASC", hdnServiceUnitID.Value));
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnHealthcareServiceUnitID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(HealthcareServiceUnit entity)
        {
            entity.HealthcareID = txtHealthcareCode.Text;

            if (cboChargeClassID.Value != null)
            {
                if (cboChargeClassID.Value.ToString() != "0")
                {
                    entity.ChargeClassID = Convert.ToInt32(cboChargeClassID.Value.ToString());
                }
                else
                {
                    entity.ChargeClassID = null;
                }
            }

            if (cboClinicGroup.Value != null)
            {
                entity.GCClinicGroup = cboClinicGroup.Value.ToString();
            }
            else
            {
                entity.GCClinicGroup = null;
            }

            entity.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entity.LogisticLocationID = Convert.ToInt32(hdnLogisticLocationID.Value);

            if (hdnDispensaryServiceUnitID.Value != "" && hdnDispensaryServiceUnitID.Value != "0")
            {
                entity.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
            }
            else
            {
                entity.DispensaryServiceUnitID = null;
            }

            if (hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
            else
            {
                entity.RevenueSharingID = null;
            }

            if (hdnDefaultParamedicID.Value != "" && hdnDefaultParamedicID.Value != "0")
            {
                entity.DefaultParamedicID = Convert.ToInt32(hdnDefaultParamedicID.Value);
            }
            else
            {
                entity.DefaultParamedicID = null;
            }

            if (hdnItemBedChargesID.Value != "" && hdnItemBedChargesID.Value != "0")
            {
                entity.ItemID = Convert.ToInt32(hdnItemBedChargesID.Value);
            }
            else
            {
                entity.ItemID = null;
            }

            if (hdnNursingServiceItemID.Value != "" && hdnNursingServiceItemID.Value != "0")
            {
                entity.NursingServiceItemID = Convert.ToInt32(hdnNursingServiceItemID.Value);
            }
            else
            {
                entity.NursingServiceItemID = null;
            }

            if (txtServiceInterval.Text != "")
            {
                entity.ServiceInterval = Convert.ToByte(txtServiceInterval.Text);
            }

            entity.ServiceUnitOfficer = txtServiceUnitOfficer.Text;

            entity.IsAutoCloseRegistration = chkIsAutoCloseRegistration.Checked;
            entity.IsInpatientDispensary = chkIsInpatientDispensary.Checked;
            entity.IsUseDiagnosisCodingProcess = chkIsUseDiagnosisCodingProcess.Checked;
            entity.IsChargeClassEditableForNonInpatient = chkIsChargeClassEditableForNonInpatient.Checked;
            entity.IsAutomaticPrintTracer = chkIsAutomaticPrintTracer.Checked;

            if (hdnSubLedgerID.Value.ToString() != "" && hdnSubLedgerID.Value != "0")
            {
                entity.SubLedger = Convert.ToInt32(hdnSubLedgerID.Value.ToString());
            }
            else
            {
                entity.SubLedger = null;
            }

            entity.Printer1Url = txtPrinter1Url.Text;

            if (IsValidIPAddress(txtIPAddress.Text))
            {
                entity.IPAddress = txtIPAddress.Text;
            }
            else
            {
                entity.IPAddress = null;
            }
        }

        private bool IsValidIPAddress(string ipAddress)
        {
            //create our match pattern
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (ipAddress == "")
            {
                //no address provided   so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(ipAddress, 0);
            }
            //return the results
            return valid;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                HealthcareServiceUnit entity = new HealthcareServiceUnit();
                ControlToEntity(entity);
                entity.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertHealthcareServiceUnit(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                HealthcareServiceUnit entity = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateHealthcareServiceUnit(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                HealthcareServiceUnit entity = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateHealthcareServiceUnit(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}