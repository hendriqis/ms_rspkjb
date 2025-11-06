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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryDiscountNewEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                SetControlProperties();
                hdnPatientBillingID.Value = param;
                hdnID.Value = null;

                vPatientBill entity = BusinessLayer.GetvPatientBillList(string.Format("PatientBillingID = {0} AND GCTransactionStatus != '{1}'",
                                                                                        hdnPatientBillingID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();

                txtPatientBillingNoCtl.Text = entity.PatientBillingNo;

                hdnTotalPatientAmount.Value = entity.TotalPatientAmount.ToString();
                txtTotalPatientAmount.Text = entity.TotalPatientAmount.ToString(Constant.FormatString.NUMERIC_2);
                txtPatientDiscountAmountCtl.Text = entity.PatientDiscountAmount.ToString(Constant.FormatString.NUMERIC_2);
                txtTotalPatient.Text = entity.TotalPatient.ToString(Constant.FormatString.NUMERIC_2);

                hdnTotalPayerAmount.Value = entity.TotalPayerAmount.ToString();
                txtTotalPayerAmount.Text = entity.TotalPayerAmount.ToString(Constant.FormatString.NUMERIC_2);
                txtPayerDiscountAmountCtl.Text = entity.PayerDiscountAmount.ToString(Constant.FormatString.NUMERIC_2);
                txtTotalPayer.Text = entity.TotalPayer.ToString(Constant.FormatString.NUMERIC_2);

                BindGrid();
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format(
                "ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DISCOUNT_REASON));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDiscountReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboDiscountReason.SelectedIndex = 0;

            string linkedVisit = "", allVisit = "";
            Registration mainReg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            if (mainReg.LinkedRegistrationID != null && mainReg.LinkedRegistrationID != 0)
            {
                List<ConsultVisit> linkedCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", mainReg.LinkedRegistrationID));
                foreach (ConsultVisit cv in linkedCV)
                {
                    linkedVisit += string.Format("{0},", cv.VisitID);
                }
            }
            if (linkedVisit != "")
            {
                linkedVisit = linkedVisit.Remove(linkedVisit.Length - 1);
                allVisit = string.Format("{0}, {1}", linkedVisit, AppSession.RegisteredPatient.VisitID);
            }
            else
            {
                allVisit = string.Format("{0}", AppSession.RegisteredPatient.VisitID);
            }

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                                "GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM PatientChargesDt WHERE IsDeleted = 0 AND TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID IN ({1}) AND GCTransactionStatus != '{2}'))",
                                                                Constant.ParamedicType.Physician, allVisit, Constant.TransactionStatus.VOID));
            lstParamedic.Insert(0, new vParamedicMaster { ParamedicID = 0, ParamedicName = "" });
            Methods.SetComboBoxField<vParamedicMaster>(cboDoctors, lstParamedic, "ParamedicName", "ParamedicID");
            cboDoctors.SelectedIndex = 0;

            hdnIsDiscountComp2ValidateTariffComp2.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_IS_PATIENTBILLDISCOUNT_DISCOUNTCOMP2_VALIDATE_TARIFFCOMP2).ParameterValue;
        }

        private void BindGrid()
        {
            string filterExpression = String.Format("PatientBillingID = {0} AND IsDeleted = 0", hdnPatientBillingID.Value);
            List<vPatientBillDiscount> lst = BusinessLayer.GetvPatientBillDiscountList(filterExpression);
            lvw.DataSource = lst;
            lvw.DataBind();
        }

        protected void cbp_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            int billingID = Convert.ToInt32(hdnPatientBillingID.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage, ref billingID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnDeleteRecord(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGrid();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpBillingID"] = billingID.ToString();
        }

        private void EntityToControl(vPatientBillDiscount entity)
        {
            cboDiscountReason.Value = entity.GCDiscountReason;
            txtReason.Text = entity.DiscountReason;
            cboDoctors.Value = entity.ParamedicID;
            txtPatientEntryDiscountAmount.Text = entity.PatientDiscountAmount.ToString("N2");
            txtPayerEntryDiscountAmount.Text = entity.PayerDiscountAmount.ToString("N2");
        }

        private void ControlToEntity(PatientBillDiscount pbDiscount)
        {
            pbDiscount.GCDiscountReason = cboDiscountReason.Value.ToString();

            if (pbDiscount.GCDiscountReason == Constant.DiscountReason.DOKTER)
            {
                pbDiscount.IsPhysicianDiscount = true;
                pbDiscount.ParamedicID = Convert.ToInt32(cboDoctors.Value);
            }
            else
            {
                pbDiscount.IsPhysicianDiscount = false;
            }

            pbDiscount.DiscountReason = txtReason.Text;
            pbDiscount.PatientDiscountAmount = Convert.ToDecimal(txtPatientEntryDiscountAmount.Text);
            pbDiscount.PayerDiscountAmount = Convert.ToDecimal(txtPayerEntryDiscountAmount.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage, ref int billingID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientBillDiscountDao entityDtDao = new PatientBillDiscountDao(ctx);
            try
            {
                bool allowDiscDoctor = true;
                decimal discDoctor = 0;
                decimal inputDisc = Convert.ToDecimal(txtPatientEntryDiscountAmount.Text) + Convert.ToDecimal(txtPayerEntryDiscountAmount.Text);

                if (cboDiscountReason.Value.ToString() == Constant.DiscountReason.DOKTER)
                {
                    if (hdnIsDiscountComp2ValidateTariffComp2.Value == "1")
                    {
                        string filterCharges = string.Format("ParamedicID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND TariffComp2 != 0 AND ItemID IN (SELECT ItemID FROM ItemMaster WHERE GCItemType NOT IN ('{2}','{3}','{4}'))",
                                                                cboDoctors.Value, Constant.TransactionStatus.VOID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                        filterCharges += string.Format(" AND TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE PatientBillingID = {0})", billingID);
                        List<PatientChargesDt> lstChDt = BusinessLayer.GetPatientChargesDtList(filterCharges, ctx);
                        discDoctor = lstChDt.Sum(a => (a.TariffComp2 * a.ChargedQuantity));

                        if (inputDisc > discDoctor)
                        {
                            allowDiscDoctor = false;
                        }
                    }
                }

                if (cboDiscountReason.Value.ToString() == Constant.DiscountReason.DOKTER && allowDiscDoctor == false)
                {
                    ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(cboDoctors.Value));

                    result = false;
                    errMessage = "Maaf, diskon <b>" + pm.FullName + "</b> tidak diperbolehkan melebihi <b>" + discDoctor.ToString(Constant.FormatString.NUMERIC_2) + "</b>";
                    ctx.RollBackTransaction();
                }
                else
                {
                    PatientBill entityHd = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID = {0}", billingID), ctx).FirstOrDefault();
                    if (entityHd.BillingDate.ToString(Constant.FormatString.DATE_FORMAT) == DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT))
                    {
                        PatientBillDiscount entityDt = new PatientBillDiscount();
                        ControlToEntity(entityDt);
                        entityDt.PatientBillingID = entityHd.PatientBillingID;
                        entityDt.IsDeleted = false;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, tagihan tidak dapat diubah karena tanggal pembuatan tagihan sudah lewat.";
                        ctx.RollBackTransaction();
                    }
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientBillDiscountDao entityDtDao = new PatientBillDiscountDao(ctx);
            try
            {
                bool allowDiscDoctor = true;
                decimal discDoctor = 0;
                decimal inputDisc = Convert.ToDecimal(txtPatientEntryDiscountAmount.Text) + Convert.ToDecimal(txtPayerEntryDiscountAmount.Text);

                if (cboDiscountReason.Value.ToString() == Constant.DiscountReason.DOKTER)
                {
                    if (hdnIsDiscountComp2ValidateTariffComp2.Value == "1")
                    {
                        string filterCharges = string.Format("ParamedicID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND TariffComp2 != 0 AND ItemID IN (SELECT ItemID FROM ItemMaster WHERE GCItemType NOT IN ('{2}','{3}','{4}'))",
                                                                cboDoctors.Value, Constant.TransactionStatus.VOID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                        filterCharges += string.Format(" AND TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE PatientBillingID = {0})", hdnPatientBillingID.Value);
                        List<PatientChargesDt> lstChDt = BusinessLayer.GetPatientChargesDtList(filterCharges, ctx);
                        discDoctor = lstChDt.Sum(a => a.TariffComp2);

                        if (inputDisc > discDoctor)
                        {
                            allowDiscDoctor = false;
                        }
                    }
                }

                if (cboDiscountReason.Value.ToString() == Constant.DiscountReason.DOKTER && allowDiscDoctor == false)
                {
                    ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(cboDoctors.Value));

                    result = false;
                    errMessage = "Maaf, diskon <b>" + pm.FullName + "</b> tidak diperbolehkan melebihi <b>" + discDoctor.ToString(Constant.FormatString.NUMERIC_2) + "</b>";
                    ctx.RollBackTransaction();
                }
                else
                {
                    PatientBill entityHd = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID = {0}", hdnPatientBillingID.Value), ctx).FirstOrDefault();
                    if (entityHd.BillingDate.ToString(Constant.FormatString.DATE_FORMAT) == DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT))
                    {
                        PatientBillDiscount entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                        ControlToEntity(entityDt);
                        entityDt.IsDeleted = false;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, tagihan tidak dapat diubah karena tanggal pembuatan tagihan sudah lewat.";
                        ctx.RollBackTransaction();
                    }
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

        private bool OnDeleteRecord(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientBillDiscountDao entityDtDao = new PatientBillDiscountDao(ctx);
            try
            {
                PatientBill entityHd = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID = {0}", hdnPatientBillingID.Value), ctx).FirstOrDefault();
                if (entityHd.BillingDate.ToString(Constant.FormatString.DATE_FORMAT) == DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT))
                {
                    PatientBillDiscount entity = entityDtDao.Get(ID);
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, tagihan tidak dapat diubah karena tanggal pembuatan tagihan sudah lewat.";
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