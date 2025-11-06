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
    public partial class PrescriptionChecklistCtl : BaseEntryPopupCtl
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
            string filterExpression = string.Format("PrescriptionOrderID = {0}", hdnOrderID.Value);
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vPrescriptionOrderHd entity)
        {
            if (entity != null)
            {
                txtTransactionNo.Text = entity.PrescriptionOrderNo;
                txtTransactionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                string referenceNo = string.Empty;
                bool isError = false;
                int orderID = Convert.ToInt32(hdnOrderID.Value);
                PrescriptionOrderHd orderHd = orderHdDao.Get(orderID);
                if (orderHd != null)
                {
                    if (hdnIsReviewPrescriptionMandatoryForProposedTransactionCtl.Value == "1")
                    {
                        string filterCharges = string.Format("PrescriptionOrderID = '{0}' AND GCTransactionStatus != '{1}'", orderHd.PrescriptionOrderID, Constant.TransactionStatus.VOID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHdList(filterCharges, ctx).FirstOrDefault();
                        if (chargesHd != null)
                        {
                            if (chargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                orderHd.IsCorrectPatient = chkIsCorrectPatient.Checked;
                                orderHd.IsCorrectMedication = chkIsCorrectMedication.Checked;
                                orderHd.IsCorrectStrength = chkIsCorrectStrength.Checked;
                                orderHd.IsCorrectFrequency = chkIsCorrectFrequency.Checked;
                                orderHd.IsCorrectDosage = chkIsCorrectDosage.Checked;
                                orderHd.IsCorrectRoute = chkIsCorrectRoute.Checked;
                                orderHd.IsHasDrugInteraction = chkIsHasDrugInteraction.Checked;
                                orderHd.IsHasDuplication = chkIsHasDuplication.Checked;
                                orderHd.IsCorrectTimeOfGiving = chkIsCorrectTimeOfGiving.Checked;
                                orderHd.IsADChecked = chkIsADChecked.Checked;
                                orderHd.IsFARChecked = chkIsFARChecked.Checked;
                                orderHd.IsKLNChecked = chkIsKLNChecked.Checked;
                                orderHd.PrescriptionReviewText = txtPrescriptionReviewText.Text;
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderHdDao.Update(orderHd);
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
                        orderHd.IsCorrectPatient = chkIsCorrectPatient.Checked;
                        orderHd.IsCorrectMedication = chkIsCorrectMedication.Checked;
                        orderHd.IsCorrectStrength = chkIsCorrectStrength.Checked;
                        orderHd.IsCorrectFrequency = chkIsCorrectFrequency.Checked;
                        orderHd.IsCorrectDosage = chkIsCorrectDosage.Checked;
                        orderHd.IsCorrectRoute = chkIsCorrectRoute.Checked;
                        orderHd.IsHasDrugInteraction = chkIsHasDrugInteraction.Checked;
                        orderHd.IsHasDuplication = chkIsHasDuplication.Checked;
                        orderHd.IsCorrectTimeOfGiving = chkIsCorrectTimeOfGiving.Checked;
                        orderHd.IsADChecked = chkIsADChecked.Checked;
                        orderHd.IsFARChecked = chkIsFARChecked.Checked;
                        orderHd.IsKLNChecked = chkIsKLNChecked.Checked;
                        orderHd.PrescriptionReviewText = txtPrescriptionReviewText.Text;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderHdDao.Update(orderHd);
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