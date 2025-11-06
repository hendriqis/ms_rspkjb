using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionChargesChecklistCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            string[] paramInfo = param.Split('|');
            hdnTransactionID.Value = paramInfo[0];
            hdnOrderID.Value = paramInfo[1];
            hdnIsReviewPrescriptionMandatoryForProposedTransactionCtl.Value = paramInfo[2];
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            vPatientChargesHd7 entity = BusinessLayer.GetvPatientChargesHd7List(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vPatientChargesHd7 entity)
        {
            if (entity != null)
            {
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                chkIsCorrectPatient.Checked = entity.IsCorrectPatient;
                chkIsCorrectMedication.Checked = entity.IsCorrectMedication;
                chkIsCorrectStrength.Checked = entity.IsCorrectStrength;
                chkIsCorrectFrequency.Checked = entity.IsCorrectFrequency;
                chkIsCorrectDosage.Checked = entity.IsCorrectDosage;
                chkIsCorrectRoute.Checked = entity.IsCorrectRoute;
                chkIsHasDrugInteraction.Checked = entity.IsHasDrugInteraction;
                chkIsHasDuplication.Checked = entity.IsHasDuplication;
                chkIsCorrectTimeOfGiving.Checked = entity.IsCorrectTimeOfGiving;
                chkIsADChecked.Checked = entity.IsADChecked;
                chkIsFARChecked.Checked = entity.IsFARChecked;
                chkIsKLNChecked.Checked = entity.IsKLNChecked;
                txtPrescriptionReviewText.Text = entity.PrescriptionReviewText;
                hdnOrderID.Value = entity.PrescriptionOrderID.ToString();
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdInfoDao transactionHdDao = new PatientChargesHdInfoDao(ctx);
            try
            {
                string referenceNo = string.Empty;
                bool isError = false;
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                PatientChargesHdInfo transactionHd = transactionHdDao.Get(transactionID);
                if (transactionHd != null)
                {
                    if (hdnIsReviewPrescriptionMandatoryForProposedTransactionCtl.Value == "1")
                    {
                        string filterCharges = string.Format("TransactionID = '{0}' AND GCTransactionStatus != '{1}'", transactionHd.TransactionID, Constant.TransactionStatus.VOID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHdList(filterCharges, ctx).FirstOrDefault();
                        if (chargesHd != null)
                        {
                            if (chargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                transactionHd.IsCorrectPatient = chkIsCorrectPatient.Checked;
                                transactionHd.IsCorrectMedication = chkIsCorrectMedication.Checked;
                                transactionHd.IsCorrectStrength = chkIsCorrectStrength.Checked;
                                transactionHd.IsCorrectFrequency = chkIsCorrectFrequency.Checked;
                                transactionHd.IsCorrectDosage = chkIsCorrectDosage.Checked;
                                transactionHd.IsCorrectRoute = chkIsCorrectRoute.Checked;
                                transactionHd.IsHasDrugInteraction = chkIsHasDrugInteraction.Checked;
                                transactionHd.IsHasDuplication = chkIsHasDuplication.Checked;
                                transactionHd.IsCorrectTimeOfGiving = chkIsCorrectTimeOfGiving.Checked;
                                transactionHd.IsADChecked = chkIsADChecked.Checked;
                                transactionHd.IsFARChecked = chkIsFARChecked.Checked;
                                transactionHd.IsKLNChecked = chkIsKLNChecked.Checked;
                                transactionHd.PrescriptionReviewText = txtPrescriptionReviewText.Text;
                                transactionHdDao.Update(transactionHd);
                            }
                            else
                            {
                                isError = true;
                                errMessage = string.Format("Resep ini sudah diproses.");
                            }
                        }
                        else
                        {
                            isError = true;
                            errMessage = string.Format("Resep ini sudah diproses.");
                        }
                    }
                    else
                    {
                        transactionHd.IsCorrectPatient = chkIsCorrectPatient.Checked;
                        transactionHd.IsCorrectMedication = chkIsCorrectMedication.Checked;
                        transactionHd.IsCorrectStrength = chkIsCorrectStrength.Checked;
                        transactionHd.IsCorrectFrequency = chkIsCorrectFrequency.Checked;
                        transactionHd.IsCorrectDosage = chkIsCorrectDosage.Checked;
                        transactionHd.IsCorrectRoute = chkIsCorrectRoute.Checked;
                        transactionHd.IsHasDrugInteraction = chkIsHasDrugInteraction.Checked;
                        transactionHd.IsHasDuplication = chkIsHasDuplication.Checked;
                        transactionHd.IsCorrectTimeOfGiving = chkIsCorrectTimeOfGiving.Checked;
                        transactionHd.IsADChecked = chkIsADChecked.Checked;
                        transactionHd.IsFARChecked = chkIsFARChecked.Checked;
                        transactionHd.IsKLNChecked = chkIsKLNChecked.Checked;
                        transactionHd.PrescriptionReviewText = txtPrescriptionReviewText.Text;
                        transactionHdDao.Update(transactionHd);
                    }
                }

                result = !isError;

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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