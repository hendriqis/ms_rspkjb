using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientManagementTransactionMedicationOrderCtl : BaseViewPopupCtl
    {
        protected bool IsEditable = true;

        protected string GetServiceUnitFilterFilterExpression()
        {
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != {2} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.RegisteredPatient.HealthcareServiceUnitID);
        }

        public override void InitializeDataControl(string param)
        {
            //hdnCurrentHealthcareServiceUnitID.Value = param;
            ControlEntryList.Clear();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            hdnDepartmentID.Value = entityCV.DepartmentID;
            hdnClassID.Value = entityCV.ClassID.ToString();
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            hdnDefaultDispensaryServiceUnitID.Value = lstHealthcareServiceUnit.FirstOrDefault().DispensaryServiceUnitID.ToString();
            OnControlEntrySetting();
            ReInitControl();
            SetControlProperties();
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        private void SetControlProperties()
        {
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryUnitCtl, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            if (cboDispensaryUnitCtl.Value == null && hdnDefaultDispensaryServiceUnitID.Value != "0")
                cboDispensaryUnitCtl.Value = hdnDefaultDispensaryServiceUnitID.Value; ;
            BindCboLocation();
        }

        protected void cboLocationCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (cboDispensaryUnitCtl.Value != null)
            {
                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensaryUnitCtl.Value)).FirstOrDefault();

                if (location != null)
                {
                    int locationID = location.LocationID;
                    Location loc = BusinessLayer.GetLocation(locationID);
                    List<Location> lstLocation = null;
                    if (loc.IsHeader)
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                    else
                    {
                        lstLocation = new List<Location>();
                        lstLocation.Add(loc);
                    }
                    Methods.SetComboBoxField<Location>(cboLocationCtl, lstLocation, "LocationName", "LocationID");
                    cboLocationCtl.SelectedIndex = 0;
                }

                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(cboDispensaryUnitCtl.Value));
                hdnIPAddress.Value = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;
            }
        }

        private void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnOrderIDCtl, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtOrderNoCtl, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtOrderDateCtl, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTimeCtl, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNotesCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtStatus, new ControlEntrySetting(false, false));
            SetControlEntrySetting(cboDispensaryUnitCtl, new ControlEntrySetting(true, true, false));
            if (cboDispensaryUnitCtl.Items.Count > 0)
                cboDispensaryUnitCtl.SelectedIndex = 0;
        }

        private void OnLoadEntity(string keyValue)
        {
            string filterExpression = string.Empty;
            if (keyValue.Equals(string.Empty))
            {
                filterExpression = string.Format("PrescriptionOrderNo = '{0}'", txtOrderNoCtl.Text);
            }
            else filterExpression = string.Format("PrescriptionOrderID = {0}", keyValue);
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        public void LoadPage(string OrderID)
        {
            SetControlEnabled(false);
            SetControlProperties();
            OnLoadEntity(OrderID);
        }

        protected void cbpMainPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string orderID = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "load")
            {
                LoadPage(string.Empty);
            }
            else if (param[0] == "loadaftersave")
            {
                LoadPage(hdnOrderIDCtl.Value);
            }
            else if (param[0] == "new")
            {
                ReInitControl();
                SetControlProperties();
            }
            else if (param[0] == "save")
            {
                if (OnSaveAddRecord(ref errMessage, ref orderID))
                {
                    result += "success";
                    LoadPage(orderID);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "proposed")
            {
                if (OnProposeRecord(ref errMessage))
                {
                    result += "success";
                    LoadPage(hdnOrderIDCtl.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = orderID;
        }

        #region Save Entity
        public void SavePrescriptionOrderHd(IDbContext ctx, ref int orderID)
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            if (hdnOrderIDCtl.Value == "0")
            {
                PrescriptionOrderHd entityHd = new PrescriptionOrderHd();

                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.PrescriptionDate = Helper.GetDatePickerValue(Request.Form[txtOrderDateCtl.UniqueID]);
                entityHd.PrescriptionTime = Request.Form[txtOrderTimeCtl.UniqueID];
                entityHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnitCtl.Value);
                entityHd.LocationID = Convert.ToInt32(cboLocationCtl.Value);
                entityHd.GCPrescriptionType = Constant.PrescriptionType.MEDICATION_ORDER;
                entityHd.Remarks = txtNotesCtl.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                orderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                orderID = Convert.ToInt32(hdnOrderIDCtl.Value);
            }
        }

        private bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int orderID = 0;
                SavePrescriptionOrderHd(ctx, ref orderID);
                hdnOrderIDCtl.Value = orderID.ToString();
                retval = orderID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                string x = retval;
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnOrderIDCtl.Value));
                entity.Remarks = txtNotesCtl.Text;
                BusinessLayer.UpdatePrescriptionOrderHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion



        private void EntityToControl(vPrescriptionOrderHd entity)
        {
            txtStatus.Text = entity.TransactionStatusWatermark;
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);
            hdnOrderIDCtl.Value = entity.PrescriptionOrderID.ToString();
            txtOrderNoCtl.Text = entity.PrescriptionOrderNo;
            txtOrderDateCtl.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTimeCtl.Text = entity.PrescriptionTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            cboDispensaryUnitCtl.Value = entity.DispensaryServiceUnitID.ToString();
            txtNotesCtl.Text = entity.Remarks;
        }

        #region Void Entity
        private bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                Int32 orderID = Convert.ToInt32(hdnOrderIDCtl.Value);
                PrescriptionOrderHd entity = entityHdDao.Get(orderID);
                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        #region Proposed Entity
        private bool OnProposeRecord(ref string errMessage)
        {
            try
            {
                Int32 orderID = Convert.ToInt32(hdnOrderIDCtl.Value);
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(orderID);
                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionOrderHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion


        #region Utility Function
        private void SetControlEnabled(bool isAdd)
        {
            foreach (DictionaryEntry entry in ControlEntryList)
            {
                Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
                ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
                bool isEnabled = (isAdd ? setting.IsEditAbleInAddMode : setting.IsEditAbleInEditMode);
                SetControlAttribute(ctrl, isEnabled);
            }
        }

        public void ReInitControl()
        {
            SetControlEnabled(true);
            LoadWords();
            foreach (DictionaryEntry entry in ControlEntryList)
            {
                Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
                if (ctrl is ASPxEdit || ctrl is WebControl || ctrl is HtmlInputHidden)
                {
                    ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
                    switch (setting.DefaultValue.ToString())
                    {
                        case Constant.DefaultValueEntry.DATE_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)); break;
                        case Constant.DefaultValueEntry.TIME_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)); break;
                        default: SetControlValue(ctrl, setting.DefaultValue); break;
                    }
                    if (ctrl is ASPxEdit)
                    {
                        ASPxEdit ctl = ctrl as ASPxEdit;
                        ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
                        ctl.ValidationSettings.RequiredField.ErrorText = "";
                        ctl.ValidationSettings.CausesValidation = true;
                        ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                        ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

                        //if (setting.IsRequired)
                        ctl.ValidationSettings.ValidationGroup = "mpEntryPopup";
                    }
                    else if (ctrl is WebControl)
                    {
                        if (setting.IsRequired)
                            Helper.AddCssClass(((WebControl)ctrl), "required");
                        ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryPopup");
                    }
                }
            }
        }

        protected void SetControlEntrySetting(Control ctrl, ControlEntrySetting setting)
        {
            ControlEntryList.Add(ctrl.ID, setting);
            if (ctrl is ASPxEdit)
            {
                ASPxEdit ctl = ctrl as ASPxEdit;
                ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
                ctl.ValidationSettings.RequiredField.ErrorText = "";
                ctl.ValidationSettings.CausesValidation = true;
                ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

                //if (setting.IsRequired)
                ctl.ValidationSettings.ValidationGroup = "mpEntryPopup";
            }
            else if (ctrl is WebControl)
            {
                if (setting.IsRequired)
                    Helper.AddCssClass(((WebControl)ctrl), "required");
                ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryPopup");
            }
        }

        private void SetControlAttribute(Control ctrl, bool isEnabled)
        {
            if (ctrl is ASPxEdit)
            {
                ((ASPxEdit)ctrl).ClientEnabled = isEnabled;
            }
            else if (ctrl is TextBox)
            {
                if (isEnabled)
                    ((TextBox)ctrl).ReadOnly = false;
                else
                    ((TextBox)ctrl).ReadOnly = true;
            }
            else if (ctrl is DropDownList)
            {
                ((DropDownList)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is HtmlGenericControl)
            {
                HtmlGenericControl lbl = ctrl as HtmlGenericControl;
                if (!isEnabled)
                    lbl.Attributes.Add("class", "lblDisabled");
            }
        }

        private void SetControlValue(Control ctrl, object value)
        {
            if (ctrl is ASPxEdit)
                ((ASPxEdit)ctrl).Value = value;
            else if (ctrl is TextBox)
                ((TextBox)ctrl).Text = value.ToString();
            else if (ctrl is DropDownList)
                Helper.SetDropDownListValue((DropDownList)ctrl, value.ToString());
            else if (ctrl is CheckBox)
            {
                if (value.ToString() == "")
                    ((CheckBox)ctrl).Checked = false;
                else
                    ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
            }

            else if (ctrl is HtmlInputHidden)
                ((HtmlInputHidden)ctrl).Value = value.ToString();
        }
        #endregion

        #region Session & View State
        public Hashtable ControlEntryList
        {
            get
            {
                if (Session["__PopupControlEntryList"] == null)
                    Session["__PopupControlEntryList"] = new Hashtable();

                return (Hashtable)Session["__PopupControlEntryList"];
            }
            set { Session["__PopupControlEntryList"] = value; }
        }
        #endregion
    }
}