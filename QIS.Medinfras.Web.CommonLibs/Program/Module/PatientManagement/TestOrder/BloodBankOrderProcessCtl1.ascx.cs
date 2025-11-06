using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Globalization;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BloodBankOrderProcessCtl1 : BaseProcessPopupCtl
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTestOrderID.Value = paramInfo[0];
            //hdnParameterRegistrationID.Value = paramInfo[1];
            //hdnParameterVisitID.Value = paramInfo[2];
            //hdnParameterParamedicID.Value = paramInfo[3];
            //hdnHealthcareServiceUnitID.Value = paramInfo[4];

            if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
            {
                IsAdd = false;
                _orderID = hdnTestOrderID.Value;
                string filterExpression = string.Format("TestOrderID = {0}", hdnTestOrderID.Value);
                vBloodBankOrder1 entity = BusinessLayer.GetvBloodBankOrder1List(filterExpression).FirstOrDefault();
                OnControlEntrySettingLocal(entity);
                ReInitControl();
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                    Constant.StandardCode.BLOOD_TYPE,
                    Constant.StandardCode.BLOOD_BANK_TYPE));


            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BLOOD_TYPE).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BLOOD_BANK_TYPE).ToList();

            Methods.SetComboBoxField<StandardCode>(cboBloodType, lstCode1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBloodComponentType, lstCode2, "StandardCodeName", "StandardCodeID");

            List<Variable> lstBloodRhesus = new List<Variable>();
            lstBloodRhesus.Add(new Variable { Code = "+", Value = "+" });
            lstBloodRhesus.Add(new Variable { Code = "-", Value = "-" });
            Methods.SetComboBoxField<Variable>(cboRhesus, lstBloodRhesus, "Value", "Code");
        }

        private void OnControlEntrySettingLocal(vBloodBankOrder1 entity)
        {
            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType = '{0}' AND IsDeleted = 0",
                                                    Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = entity.ParamedicID;

            SetControlProperties();

            txtRealizationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRealizationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            cboParamedicID.Enabled = false;
            cboBloodType.Enabled = false;
            cboRhesus.Enabled = false;
            cboBloodComponentType.Enabled = false;
            txtQuantity.Enabled = false;
            txtRemarks.Enabled = false;
            txtMedicalHistory.Enabled = false;

        }

        private void EntityToControl(vBloodBankOrder1 entity)
        {
            hdnBloodBankOrderID.Value = entity.TestOrderID.ToString();

            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            cboBloodType.Value = entity.GCBloodType;
            cboRhesus.Value = entity.BloodRhesus;
            rblGCSourceType.SelectedValue = entity.GCSourceType;
            rblGCUsageType.SelectedValue = entity.GCUsageType;
            rblGCPaymentType.SelectedValue = entity.GCPaymentType;
            chkIsCITO.Checked = entity.IsCITO;
            txtRemarks.Text = entity.PurposeRemarks;
            txtMedicalHistory.Text = entity.TransfusionHistory;

            txtReferenceNo.Text = entity.PMIReferenceNo;
            if (entity.PMIPickupDate.Year != 1900)
                txtPMIPickupDate.Text = string.Empty;
            else
                txtPMIPickupDate.Text = entity.PMIPickupDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.PMIPickupDate.Year != 1900)
                txtPMIPickupTime.Text = string.Empty;
            else
                txtPMIPickupTime.Text = entity.PMIPickupTime;

            hdnTransactionID.Value = entity.TransactionID.ToString();
        }

        private void ControlToEntity(BloodBankOrder entity)
        {
            //entity.TransactionCode = Constant.TransactionCode.BLOOD_BANK_ORDER;
            //if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
            //    entity.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            //else
            //    entity.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.RegisteredPatient.HealthcareServiceUnitID);

            //entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.MD0018);
            //entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            //entity.VisitID = AppSession.RegisteredPatient.VisitID;
            //entity.OrderDate = Helper.GetDatePickerValue(txtOrderDate);
            //entity.OrderTime = txtOrderTime.Text;

            //entity.GCBloodType = cboBloodType.Value.ToString();
            //entity.BloodRhesus = cboRhesus.Value.ToString();
            //entity.GCBloodComponentType = cboBloodComponentType.Value.ToString();
            //entity.OrderQty = Convert.ToDecimal(txtQuantity.Text);
            //entity.GCSourceType = rblGCSourceType.SelectedValue;
            //entity.GCUsageType = rblGCUsageType.SelectedValue;
            //if (!string.IsNullOrEmpty(rblGCPaymentType.SelectedValue))
            //{
            //    entity.GCPaymentType = rblGCPaymentType.SelectedValue;
            //}

            //entity.IsCITO = chkIsCITO.Checked;
            //entity.PurposeRemarks = txtRemarks.Text;
            //entity.TransfusionHistory = txtMedicalHistory.Text;

            if (!string.IsNullOrEmpty(txtReferenceNo.Text))
                entity.PMIReferenceNo = txtReferenceNo.Text;
            if (!string.IsNullOrEmpty(txtPMIPickupDate.Text))
                entity.PMIPickupDate = Helper.GetDatePickerValue(txtPMIPickupDate.Text);
            if (!string.IsNullOrEmpty(txtPMIPickupTime.Text))
                entity.PMIPickupTime = txtPMIPickupTime.Text;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (!IsValidated(ref errMessage))
            {
                result = false;
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            BloodBankOrderDao bloodBankDao = new BloodBankOrderDao(ctx);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            try
            {
                BloodBankOrder bloodBankOrder = bloodBankDao.Get(Convert.ToInt32(hdnBloodBankOrderID.Value));
                TestOrderHd testOrderHd = orderHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                if (testOrderHd != null)
                {
                    ControlToEntity(bloodBankOrder);
                    bloodBankOrder.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                    bloodBankOrder.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    bloodBankOrder.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                    bloodBankOrder.LastUpdatedBy = AppSession.UserLogin.UserID;
                    bloodBankDao.Update(bloodBankOrder);

                    testOrderHd.ParamedicID = bloodBankOrder.ParamedicID;
                    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    orderHdDao.Update(testOrderHd);
                }
                if (hdnTransactionID.Value == "0" || string.IsNullOrEmpty(hdnTransactionID.Value))
                {
                    #region Patient Charges Hd
                    PatientChargesHd patientChargesHd = null;
                    if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                    {
                        patientChargesHd = new PatientChargesHd();
                        patientChargesHd.VisitID = bloodBankOrder.VisitID;
                        patientChargesHd.LinkedChargesID = null;
                        patientChargesHd.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                        patientChargesHd.HealthcareServiceUnitID = testOrderHd.HealthcareServiceUnitID;
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        else
                            patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        patientChargesHd.TransactionDate = Helper.GetDatePickerValue(txtRealizationDate.Text);
                        patientChargesHd.TransactionTime = txtRealizationTime.Text;
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = bloodBankOrder.OrderNo;
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TotalPatientAmount = 0;
                        patientChargesHd.TotalPayerAmount = 0;
                        patientChargesHd.TotalAmount = 0;
                        patientChargesHd.Remarks = txtRemarks.Text;
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        hdnTransactionID.Value = chargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd).ToString();
                    }

                    retVal = hdnTransactionID.Value;
                    #endregion
                }

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

        private void ControlToTestOrderHdEntity(TestOrderHd entityHd)
        {
            entityHd.HealthcareServiceUnitID = Convert.ToInt32(AppSession.MD0018);
            entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
            entityHd.TestOrderDate = Helper.GetDatePickerValue(txtOrderDate);
            entityHd.TestOrderTime = txtOrderTime.Text;
            entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
            entityHd.IsBloodBankOrder = true;

            entityHd.IsCITO = chkIsCITO.Checked;
            entityHd.Remarks = txtRemarks.Text;

            entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
        }

        private bool IsValidated(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();


            if (string.IsNullOrEmpty(txtOrderTime.Text))
            {
                message.AppendLine("Jam order harus diisi|");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtOrderTime.Text))
                    message.AppendLine("Format Jam Order tidak sesuai format (HH:MM)|");
                else
                {
                    DateTime startDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtOrderDate.Text, txtOrderTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (startDateTime.Date > DateTime.Now.Date)
                    {
                        message.AppendLine("Tanggal Order harus lebih kecil atau sama dengan tanggal hari ini.|");
                    }
                }
            }


            if (cboBloodType.Value == null)
            {
                message.AppendLine("Jenis golongan darah harus diisi|");
            }
            else
            {
                if (string.IsNullOrEmpty(cboBloodType.Value.ToString()))
                {
                    message.AppendLine("Jenis golongan darah harus diisi|");
                }
            }

            if (cboRhesus.Value == null)
            {
                message.AppendLine("Rhesus golongan darah harus diisi|");
            }
            else
            {
                if (string.IsNullOrEmpty(cboRhesus.Value.ToString()))
                {
                    message.AppendLine("Rhesus golongan darah harus diisi|");
                }
            }

            if (cboBloodComponentType.Value == null)
            {
                message.AppendLine("Jenis darah/komponen harus diisi|");
            }
            else
            {
                if (string.IsNullOrEmpty(cboBloodComponentType.Value.ToString()))
                {
                    message.AppendLine("Jenis darah/komponen harus diisi|");
                }
            }

            if (!Methods.IsNumeric(txtQuantity.Text) || txtQuantity.Text == "0")
            {
                message.AppendLine("Jumlah permintaan harus berupa angka dan lebih besar dari 0|");
            }

            if (string.IsNullOrEmpty(rblGCSourceType.SelectedValue))
            {
                message.AppendLine("Sumber/asal darah harus dipilih|");
            }

            if (string.IsNullOrEmpty(rblGCUsageType.SelectedValue))
            {
                message.AppendLine("Cara penyimpanan harus dipilih|");
            }

            if (!string.IsNullOrEmpty(txtReferenceNo.Text))
            {
                if (!Methods.ValidateTimeFormat(txtPMIPickupTime.Text))
                    message.AppendLine("Format Jam Pengambilan di PMI tidak sesuai format (HH:MM)|");
                else
                {
                    DateTime pmiDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtPMIPickupDate.Text, txtPMIPickupTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DateTime orderDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtOrderDate.Text, txtOrderTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (pmiDateTime.Date < orderDateTime)
                    {
                        message.AppendLine("Tanggal Pengambilan di PMI harus lebih besar dari tanggal dan jam Order|");
                    }
                }
            }

            errMessage = message.ToString().Replace(@"|", "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }
    }
}