using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class UpdateOrderStatusCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderID.Value = paramInfo[1];
            txtTransactionNoCtl.Text = paramInfo[2];
            if (!string.IsNullOrEmpty(hdnPrescriptionOrderID.Value))
            {
                String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PRESCRIPTION_TYPE, Constant.StandardCode.TRANSACTION_STATUS);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                Methods.SetComboBoxField<StandardCode>(cboTransactionStatusCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");

                if (!AppSession.IsUsedInpatientPrescriptionTypeFilter)
                {
                    Methods.SetComboBoxField<StandardCode>(cboPrescriptionTypeCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppSession.InpatientPrescriptionTypeFilter))
                    {
                        string[] prescriptionType = AppSession.InpatientPrescriptionTypeFilter.Split(',');
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionTypeCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).Where(x => !prescriptionType.Contains(x.StandardCodeID)).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                    else
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionTypeCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                }

                List<Variable> lstTransactionStatus = new List<Variable>();
                lstTransactionStatus.Add(new Variable() { Code = "0", Value = "Belum Dikonfirmasi" });
                lstTransactionStatus.Add(new Variable() { Code = "1", Value = "Sudah Dikonfirmasi" });
                Methods.SetComboBoxField<Variable>(cboTransactionStatusCtl, lstTransactionStatus, "Value", "Code");

                filterExpression = string.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value);
                vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3List(filterExpression).FirstOrDefault();
                if (entity != null)
                {
                    EntityToControl(entity);
                }
            }
        }

        private void EntityToControl(vPrescriptionOrderHd3 entity)
        {
            txtOrderDateTime.Text = string.Format("{0} {1}", entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), entity.PrescriptionTime);
            chkIsCorrectPatientCtl.Checked = entity.IsCorrectPatient;
            chkIsCorrectMedicationCtl.Checked = entity.IsCorrectMedication;
            chkIsCorrectStrengthCtl.Checked = entity.IsCorrectStrength;
            chkIsCorrectFrequencyCtl.Checked = entity.IsCorrectFrequency;
            chkIsCorrectDosageCtl.Checked = entity.IsCorrectDosage;
            chkIsCorrectRouteCtl.Checked = entity.IsCorrectRoute;
            chkIsHasDrugInteractionCtl.Checked = entity.IsHasDrugInteraction;
            chkIsHasDuplicationCtl.Checked = entity.IsHasDuplication;
            chkIsADCheckedCtl.Checked = entity.IsADChecked;
            chkIsFARCheckedCtl.Checked = entity.IsFARChecked;
            chkIsKLNCheckedCtl.Checked = entity.IsKLNChecked;
            cboPrescriptionTypeCtl.Value = entity.GCPrescriptionType;

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                cboTransactionStatusCtl.Value = "0";
            else
                cboTransactionStatusCtl.Value = "1";
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                PrescriptionOrderHd orderHd = orderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                if (orderHdDao != null)
                {
                    List<PatientChargesHd> lstChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PrescriptionOrderID = {0} AND GCTransactionStatus <> '{1}'", orderHd.PrescriptionOrderID, Constant.TransactionStatus.VOID), ctx);
                    if (lstChargesHd.Count() == 0)
                    {
                        //orderHd.GCPrescriptionType = cboPrescriptionTypeCtl.Value.ToString();
                        if (orderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL || orderHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            orderHd.GCTransactionStatus = cboTransactionStatusCtl.Value.ToString() == "1" ? Constant.TransactionStatus.APPROVED : Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        }
                        orderHd.IsCorrectPatient = chkIsCorrectPatientCtl.Checked;
                        orderHd.IsCorrectMedication = chkIsCorrectMedicationCtl.Checked;
                        orderHd.IsCorrectStrength = chkIsCorrectStrengthCtl.Checked;
                        orderHd.IsCorrectFrequency = chkIsCorrectFrequencyCtl.Checked;
                        orderHd.IsCorrectDosage = chkIsCorrectDosageCtl.Checked;
                        orderHd.IsCorrectRoute = chkIsCorrectRouteCtl.Checked;
                        orderHd.IsHasDrugInteraction = chkIsHasDrugInteractionCtl.Checked;
                        orderHd.IsHasDuplication = chkIsHasDuplicationCtl.Checked;
                        orderHd.IsADChecked = chkIsADCheckedCtl.Checked;
                        orderHd.IsFARChecked = chkIsFARCheckedCtl.Checked;
                        orderHd.IsKLNChecked = chkIsKLNCheckedCtl.Checked;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderHdDao.Update(orderHd);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Order resep {0} ini tidak dapat dilakukan perubahan karena masih ada Medication Charges yg masih valid", orderHd.PrescriptionOrderNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }

                retval = hdnPrescriptionOrderID.Value;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = "0|" + errMessage;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
    }
}