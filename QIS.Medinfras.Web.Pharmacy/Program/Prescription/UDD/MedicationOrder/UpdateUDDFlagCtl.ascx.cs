using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class UpdateUDDFlagCtl : BaseProcessPopupCtl
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
                BindGridView();
            }
        }

        private void EntityToControl(vPrescriptionOrderHd3 entity)
        {
            txtOrderDateTimeCtl.Text = string.Format("{0} {1}", entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), entity.PrescriptionTime);
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

        private void BindGridView()
        {
            string filterExpression = string.Format("PrescriptionOrderID = {0} AND GCItemType != '{1}' AND IsCompound = 0 AND IsAsRequired = 0 AND IsAllowUDD = 1 AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.ItemType.BARANG_MEDIS);
            List<vPrescriptionOrderDt1> lstDetail = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstDetail;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                int orderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                string referenceNo = string.Empty;

                string filterExpression = string.Format("PrescriptionOrderID = {0} AND PrescriptionOrderDetailID IN ({1})", hdnPrescriptionOrderID.Value, hdnSelectedID.Value);
                List<PrescriptionOrderDt> lstPrescriptionOrderDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);

                string[] lstIsUsingUDD = hdnSelectedIsUsingUDD.Value.Split(',');
                string[] lstSequenceTime1 = hdnSequence1Time.Value.Split(',');
                string[] lstSequenceTime2 = hdnSequence2Time.Value.Split(',');
                string[] lstSequenceTime3 = hdnSequence3Time.Value.Split(',');
                string[] lstSequenceTime4 = hdnSequence4Time.Value.Split(',');
                string[] lstSequenceTime5 = hdnSequence5Time.Value.Split(',');
                string[] lstSequenceTime6 = hdnSequence6Time.Value.Split(',');

                int index = 0;
                foreach (PrescriptionOrderDt item in lstPrescriptionOrderDt)
                {
                    item.IsUsingUDD = lstIsUsingUDD[index] == "1" ? true : false;
                    item.StartTime = lstSequenceTime1[index] == string.Empty ? "08:00" : lstSequenceTime1[index];
                    item.Sequence1Time = lstSequenceTime1[index] == string.Empty ? "08:00" : lstSequenceTime1[index];
                    item.Sequence2Time = lstSequenceTime2[index];
                    item.Sequence3Time = lstSequenceTime3[index];
                    item.Sequence4Time = lstSequenceTime4[index];
                    item.Sequence5Time = lstSequenceTime5[index];
                    item.Sequence6Time = lstSequenceTime6[index];

                    orderDtDao.Update(item);

                    index += 1; ;
                }

                PrescriptionOrderHd orderHd = orderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                if (orderHdDao != null)
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
                }

                ctx.CommitTransaction();
                retval = hdnPrescriptionOrderID.Value;
                result = true;
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