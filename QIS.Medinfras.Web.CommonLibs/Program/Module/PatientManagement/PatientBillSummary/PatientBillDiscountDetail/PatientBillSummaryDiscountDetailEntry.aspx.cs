using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class PatientBillSummaryDiscountDetailEntry : BasePageTrx
    {
        private string deptID = "";
        private string suID = "";
        private string filterDept = "IsActive = 1 AND IsHasRegistration = 1";
        private string filterSer = "IsDeleted = 0 AND IsUsingRegistration = 1";

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_DISCOUNT_DETAIL;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_DISCOUNT_DETAIL;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_DISCOUNT_DETAIL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_DISCOUNT_DETAIL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_DISCOUNT_DETAIL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_DISCOUNT_DETAIL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_DISCOUNT_DETAIL;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_DISCOUNT_DETAIL;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_DISCOUNT_DETAIL;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID))[0];
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //2
                                                        Constant.SettingParameter.FN_IS_ALLOW_DISCOUNT_IN_DETAIL_WHEN_ALREADY_HAS_PRESCRIPTION_RETURN //3
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            hdnIsAllowDiscountInDetailWhenAlreadyHasPrescriptionReturn.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_DISCOUNT_IN_DETAIL_WHEN_ALREADY_HAS_PRESCRIPTION_RETURN).FirstOrDefault().ParameterValue;

            string roleID = "";

            List<UserInRole> lst = BusinessLayer.GetUserInRoleList(string.Format(
                    "HealthcareID = '{0}' AND UserID = {1}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            foreach (UserInRole uir in lst)
            {
                roleID += "," + uir.RoleID;
            }
            roleID = roleID.Substring(1, roleID.Length - 1);

            if (roleID != "")
            {
                List<vServiceUnitUserRole> lstServiceUnit = BusinessLayer.GetvServiceUnitUserRoleList(string.Format("HealthcareID = '{0}' AND RoleID IN ({1})",
                        AppSession.UserLogin.HealthcareID, roleID));
                foreach (vServiceUnitUserRole sur in lstServiceUnit)
                {
                    deptID += "," + "'" + sur.DepartmentID + "'";
                    suID += "," + "'" + sur.ServiceUnitID + "'";
                }

                if (lstServiceUnit.Count > 0)
                {
                    deptID = deptID.Substring(1, deptID.Length - 1);
                    filterDept += string.Format(" AND DepartmentID IN ({0})", deptID);

                    suID = suID.Substring(1, suID.Length - 1);
                    filterSer += string.Format(" AND ServiceUnitID IN ({0})", suID);
                }

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
                lstDepartment.Add(new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                hdnFilterChargesHealthcareServiceUnitID.Value = filterSer;
            }
            else
            {
                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
                lstDepartment.Add(new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                hdnFilterChargesHealthcareServiceUnitID.Value = filterSer;
            }

            cboDepartment.Value = "";
            hdnCboChargesDepartmentID.Value = "";

            string filterMasterStdCode = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE, Constant.StandardCode.DISCOUNT_REASON_CHARGESDT);
            List<StandardCode> lstMasterStdCode = BusinessLayer.GetStandardCodeList(filterMasterStdCode);

            List<StandardCode> lstItemType = lstMasterStdCode.Where(t => t.ParentID == Constant.StandardCode.ITEM_TYPE).ToList();
            lstItemType.Add(new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCItemType, lstItemType, "StandardCodeName", "StandardCodeID");
            cboGCItemType.Value = "";
            hdnCboGCItemType.Value = "";

            List<StandardCode> lstSC = lstMasterStdCode.Where(t => t.ParentID == Constant.StandardCode.DISCOUNT_REASON_CHARGESDT).ToList();
            lstSC.Add(new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboGCDiscountReasonChargesDt, lstSC.OrderBy(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filter = string.Format("((RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1)) AND GCTransactionStatus IN ('{1}','{2}') AND ISNULL(GCTransactionDetailStatus,'') IN ('{1}','{2}') AND IsDeleted = 0)",
                                                hdnRegistrationID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            if (hdnCboChargesDepartmentID.Value != "" && hdnCboChargesDepartmentID.Value != "0")
            {
                filter += " AND ChargesDepartmentID = '" + cboDepartment.Value + "'";
            }

            if (txtChargesServiceUnitCode.Text != "")
            {
                filter += " AND ChargesServiceUnitCode = '" + txtChargesServiceUnitCode.Text + "'";
            }

            if (txtChargesParamedicCode.Text != "")
            {
                filter += " AND ParamedicCode = '" + txtChargesParamedicCode.Text + "'";
            }

            if (hdnCboGCItemType.Value != null && hdnCboGCItemType.Value != "")
            {
                filter += string.Format(" AND GCItemType = '{0}'", hdnCboGCItemType.Value);
            }

            if (chkIsHasRevenueSharing.Checked)
            {
                filter += " AND IsAllowRevenueSharing = 1";
            }

            filter += " ORDER BY TransactionDate, TransactionID, ID";

            List<vPatientChargesDt16> lst = BusinessLayer.GetvPatientChargesDt16List(filter);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientChargesDt16 entity = e.Item.DataItem as vPatientChargesDt16;

                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                TextBox txtDiscountAmount = (TextBox)e.Item.FindControl("txtDiscountAmount");
                TextBox txtLVDiscComp1 = (TextBox)e.Item.FindControl("txtLVDiscComp1");
                TextBox txtLVDiscComp2 = (TextBox)e.Item.FindControl("txtLVDiscComp2");
                TextBox txtLVDiscComp3 = (TextBox)e.Item.FindControl("txtLVDiscComp3");
                HtmlInputHidden hdnGCDiscountReasonChargesDt = (HtmlInputHidden)e.Item.FindControl("hdnGCDiscountReasonChargesDt");
                HtmlGenericControl lblGCDiscountReasonChargesDt = (HtmlGenericControl)e.Item.FindControl("lblGCDiscountReasonChargesDt");
                TextBox txtDiscountReasonChargesDt = (TextBox)e.Item.FindControl("txtDiscountReasonChargesDt");
                TextBox txtPayerAmount = (TextBox)e.Item.FindControl("txtPayerAmount");
                TextBox txtPatientAmount = (TextBox)e.Item.FindControl("txtPatientAmount");
                TextBox txtLineAmount = (TextBox)e.Item.FindControl("txtLineAmount");

                //chkIsSelected.Checked = true;

                txtDiscountAmount.Text = entity.DiscountAmount.ToString();
                txtLVDiscComp1.Text = entity.DiscountComp1.ToString();
                txtLVDiscComp2.Text = entity.DiscountComp2.ToString();
                txtLVDiscComp3.Text = entity.DiscountComp3.ToString();

                lblGCDiscountReasonChargesDt.Attributes.Remove("class");
                lblGCDiscountReasonChargesDt.Attributes.Add("class", "lblGCDiscountReasonChargesDt");
                hdnGCDiscountReasonChargesDt.Value = entity.GCDiscountReason;
                if (entity.GCDiscountReason != null && entity.GCDiscountReason != "")
                    lblGCDiscountReasonChargesDt.InnerHtml = entity.DiscountReasonName;
                else
                    lblGCDiscountReasonChargesDt.InnerHtml = "Pilih Alasan Diskon";
                txtDiscountReasonChargesDt.Text = entity.DiscountReason;

                txtPayerAmount.Text = entity.PayerAmount.ToString();
                txtPatientAmount.Text = entity.PatientAmount.ToString();
                txtLineAmount.Text = entity.LineAmount.ToString();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            BindGridDetail();
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "editdiscount")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
                ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
                ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
                try
                {
                    if (hdnSelectedTransactionDtID.Value != "")
                    {
                        List<String> lstChargesDtID = hdnSelectedTransactionDtID.Value.Split(',').ToList();

                        List<String> lstDiscAmount = hdnSelectedDiscountAmount.Value.Split(',').ToList();
                        List<String> lstDiscComp1 = hdnSelectedDiscountComp1.Value.Split(',').ToList();
                        List<String> lstDiscComp2 = hdnSelectedDiscountComp2.Value.Split(',').ToList();
                        List<String> lstDiscComp3 = hdnSelectedDiscountComp3.Value.Split(',').ToList();
                        List<String> lstGCDiscountReason = hdnSelectedGCDiscountReason.Value.Split(',').ToList();
                        List<String> lstDiscountReason = hdnSelectedDiscountReason.Value.Split(',').ToList();

                        List<String> lstPayerAmount = hdnSelectedPayerAmount.Value.Split(',').ToList();
                        List<String> lstPatientAmount = hdnSelectedPatientAmount.Value.Split(',').ToList();
                        List<String> lstLineAmount = hdnSelectedLineAmount.Value.Split(',').ToList();

                        lstChargesDtID.RemoveAt(0);
                        lstDiscAmount.RemoveAt(0);
                        lstDiscComp1.RemoveAt(0);
                        lstDiscComp2.RemoveAt(0);
                        lstDiscComp3.RemoveAt(0);
                        lstGCDiscountReason.RemoveAt(0);
                        lstDiscountReason.RemoveAt(0);
                        lstPayerAmount.RemoveAt(0);
                        lstPatientAmount.RemoveAt(0);
                        lstLineAmount.RemoveAt(0);

                        for (int i = 0; i < lstChargesDtID.Count(); i++)
                        {
                            PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(lstChargesDtID[i]));
                            PatientChargesHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                            ItemService its = itemServiceDao.Get(entityDt.ItemID);
                            ItemMaster ims = itemMasterDao.Get(entityDt.ItemID);
                            decimal discBeforeUpdate = entityDt.DiscountAmount;

                            bool isBlockedValidatePrescriptionReturn = false;

                            //jika setvar FN0185 = 0 akan baca validasi sudah punya retur resep
                            if (hdnIsAllowDiscountInDetailWhenAlreadyHasPrescriptionReturn.Value == "0")
                            {
                                string filterReturn = string.Format("PatientChargesDtID = {0}", entityDt.ID);
                                List<vPrescriptionReturnOrderDtCountPerChargesPharmacy> returnOrderList = BusinessLayer.GetvPrescriptionReturnOrderDtCountPerChargesPharmacyList(filterReturn, ctx);
                                if (returnOrderList.Count() != 0)
                                {
                                    isBlockedValidatePrescriptionReturn = true;
                                    result = false;
                                    errMessage = string.Format("Maaf, tidak dapat ubah diskon detail transaksi dengan nomor {0} karena transaksi ini sudah memiliki retur resep", entityHd.TransactionNo);
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    break;
                                }
                            }

                            if (!isBlockedValidatePrescriptionReturn)
                            {
                                entityDt.IsDiscountInPercentageComp1 = false;
                                entityDt.DiscountPercentageComp1 = 0;
                                entityDt.DiscountComp1 = Convert.ToDecimal(lstDiscComp1[i]);
                                entityDt.IsDiscountInPercentageComp2 = false;
                                entityDt.DiscountPercentageComp2 = 0;
                                entityDt.DiscountComp2 = Convert.ToDecimal(lstDiscComp2[i]);
                                entityDt.IsDiscountInPercentageComp3 = false;
                                entityDt.DiscountPercentageComp3 = 0;
                                entityDt.DiscountComp3 = Convert.ToDecimal(lstDiscComp3[i]);

                                entityDt.DiscountAmount = (entityDt.DiscountComp1 + entityDt.DiscountComp2 + entityDt.DiscountComp3) * entityDt.ChargedQuantity;

                                entityDt.IsDiscount = entityDt.DiscountAmount != 0 ? true : false;

                                entityDt.PayerAmount = Convert.ToDecimal(lstPayerAmount[i]);
                                entityDt.PatientAmount = Convert.ToDecimal(lstPatientAmount[i]);
                                entityDt.LineAmount = Convert.ToDecimal(lstLineAmount[i]);

                                if (entityDt.IsDiscount)
                                {
                                    entityDt.GCDiscountReason = lstGCDiscountReason[i];
                                    if (entityDt.GCDiscountReason == Constant.DiscountReasonChargesDt.LAIN_LAIN)
                                    {
                                        entityDt.DiscountReason = lstDiscountReason[i];
                                    }
                                    else
                                    {
                                        entityDt.DiscountReason = null;
                                    }
                                }
                                else
                                {
                                    entityDt.GCDiscountReason = null;
                                    entityDt.DiscountReason = null;
                                }

                                decimal oPatientAmount = entityDt.PatientAmount;
                                decimal oPayerAmount = entityDt.PayerAmount;
                                decimal oLineAmount = entityDt.LineAmount;

                                if (hdnIsEndingAmountRoundingTo1.Value == "1")
                                {
                                    decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                                    decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                                    if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                    {
                                        oPatientAmount = Math.Floor(oPatientAmount);
                                    }
                                    else
                                    {
                                        oPatientAmount = Math.Ceiling(oPatientAmount);
                                    }

                                    decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                                    decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                                    if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                                    {
                                        oPayerAmount = Math.Floor(oPayerAmount);
                                    }
                                    else
                                    {
                                        oPayerAmount = Math.Ceiling(oPayerAmount);
                                    }

                                    oLineAmount = oPatientAmount + oPayerAmount;
                                }

                                if (ims.GCItemType == Constant.ItemType.OBAT_OBATAN || ims.GCItemType == Constant.ItemType.BARANG_MEDIS || ims.GCItemType == Constant.ItemType.BARANG_UMUM || ims.GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                                {
                                    if (hdnIsEndingAmountRoundingTo100.Value == "1")
                                    {
                                        oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                        oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                        oLineAmount = oPatientAmount + oPayerAmount;
                                    }
                                }

                                entityDt.PatientAmount = oPatientAmount;
                                entityDt.PayerAmount = oPayerAmount;
                                entityDt.LineAmount = oLineAmount;

                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtDao.Update(entityDt);

                                List<PatientChargesDtPackage> lstDtPackageCheck = new List<PatientChargesDtPackage>();
                                if (chkIsProcessDetailPackageOnly.Checked)
                                {
                                    string filterPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", entityDt.ID);
                                    List<PatientChargesDtPackage> lstPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                                    //jika paket item dan akumulasi (centangan  Proses diskon juga untuk Transaksi Detail Paket ?  di centang)
                                    if (its != null)
                                    {
                                        if (its.IsUsingAccumulatedPrice && its.IsPackageItem)
                                        {
                                            foreach (PatientChargesDtPackage entityDtPackage in lstPackage)
                                            {
                                                if (entityDtPackage.TariffComp1 != 0)
                                                {
                                                    if (chkIsDiscountInPercentage.Checked)
                                                    {
                                                        entityDtPackage.DiscountComp1 = entityDtPackage.TariffComp1 * Convert.ToDecimal(txtDiscComp1.Text) / 100;
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDecimal(txtDiscComp1.Text) > entityDtPackage.TariffComp1)
                                                        {
                                                            entityDtPackage.DiscountComp1 = entityDtPackage.TariffComp1;
                                                        }
                                                        else
                                                        {
                                                            entityDtPackage.DiscountComp1 = Convert.ToDecimal(txtDiscComp1.Text);
                                                        }
                                                    }
                                                }
                                                if (entityDtPackage.TariffComp2 != 0)
                                                {
                                                    if (chkIsDiscountInPercentage.Checked)
                                                    {
                                                        entityDtPackage.DiscountComp2 = entityDtPackage.TariffComp2 * Convert.ToDecimal(txtDiscComp2.Text) / 100;
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDecimal(txtDiscComp2.Text) > entityDtPackage.TariffComp2)
                                                        {
                                                            entityDtPackage.DiscountComp2 = entityDtPackage.TariffComp2;
                                                        }
                                                        else
                                                        {
                                                            entityDtPackage.DiscountComp2 = Convert.ToDecimal(txtDiscComp2.Text);
                                                        }
                                                    }
                                                }
                                                if (entityDtPackage.TariffComp3 != 0)
                                                {
                                                    if (chkIsDiscountInPercentage.Checked)
                                                    {
                                                        entityDtPackage.DiscountComp3 = entityDtPackage.TariffComp3 * Convert.ToDecimal(txtDiscComp3.Text) / 100;
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDecimal(txtDiscComp3.Text) > entityDtPackage.TariffComp3)
                                                        {
                                                            entityDtPackage.DiscountComp3 = entityDtPackage.TariffComp3;
                                                        }
                                                        else
                                                        {
                                                            entityDtPackage.DiscountComp3 = Convert.ToDecimal(txtDiscComp3.Text);
                                                        }
                                                    }
                                                }
                                                entityDtPackage.DiscountAmount = (entityDtPackage.DiscountComp1 + entityDtPackage.DiscountComp2 + entityDtPackage.DiscountComp3) * entityDtPackage.ChargedQuantity;
                                                entityDtPackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityDtPackageDao.Update(entityDtPackage);

                                                lstDtPackageCheck.Add(entityDtPackage);
                                            }
                                        }
                                        //jika paket item dan tidak akumulasi (centangan  Proses diskon juga untuk Transaksi Detail Paket ?  di centang)
                                        else if (!its.IsUsingAccumulatedPrice && its.IsPackageItem)
                                        {
                                            foreach (PatientChargesDtPackage e in lstPackage)
                                            {
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                PatientChargesDtPackage entityDtPackage = entityDtPackageDao.Get(e.ID);

                                                if (entityDt.DiscountComp1 != 0)
                                                {
                                                    entityDtPackage.DiscountComp1 = ((entityDtPackage.Tariff / entityDt.Tariff) * entityDt.DiscountComp1);
                                                }
                                                else
                                                {
                                                    entityDtPackage.DiscountComp1 = 0;
                                                }

                                                if (entityDt.DiscountComp2 != 0)
                                                {
                                                    entityDtPackage.DiscountComp2 = ((entityDtPackage.Tariff / entityDt.Tariff) * entityDt.DiscountComp2);
                                                }
                                                else
                                                {
                                                    entityDtPackage.DiscountComp2 = 0;
                                                }

                                                if (entityDt.DiscountComp3 != 0)
                                                {
                                                    entityDtPackage.DiscountComp3 = ((entityDtPackage.Tariff / entityDt.Tariff) * entityDt.DiscountComp3);
                                                }
                                                else
                                                {
                                                    entityDtPackage.DiscountComp3 = 0;
                                                }
                                                entityDtPackage.DiscountAmount = (entityDtPackage.DiscountComp1 + entityDtPackage.DiscountComp2 + entityDtPackage.DiscountComp3) * entityDtPackage.ChargedQuantity;
                                                entityDtPackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityDtPackageDao.Update(entityDtPackage);

                                                lstDtPackageCheck.Add(entityDtPackage);
                                            }
                                        }
                                    }
                                }

                                //jika paket item dan akumulasi (kalkulasi ke chargesDt)
                                if (its != null)
                                {
                                    if (its.IsUsingAccumulatedPrice && its.IsPackageItem)
                                    {
                                        PatientChargesDt pcdt = entityDt;

                                        decimal BaseTariff = 0;
                                        decimal BaseComp1 = 0;
                                        decimal BaseComp2 = 0;
                                        decimal BaseComp3 = 0;
                                        decimal Tariff = 0;
                                        decimal TariffComp1 = 0;
                                        decimal TariffComp2 = 0;
                                        decimal TariffComp3 = 0;
                                        decimal DiscountAmount = 0;
                                        decimal DiscountComp1 = 0;
                                        decimal DiscountComp2 = 0;
                                        decimal DiscountComp3 = 0;
                                        foreach (PatientChargesDtPackage e in lstDtPackageCheck)
                                        {
                                            BaseTariff += e.BaseTariff * e.ChargedQuantity;
                                            BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                                            BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                                            BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                                            Tariff += e.Tariff * e.ChargedQuantity;
                                            TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                                            TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                                            TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                                            DiscountAmount += e.DiscountAmount;
                                            DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                                            DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                                            DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;
                                        }

                                        pcdt.BaseTariff = BaseTariff / entityDt.ChargedQuantity;
                                        pcdt.BaseComp1 = BaseComp1 / entityDt.ChargedQuantity;
                                        pcdt.BaseComp2 = BaseComp2 / entityDt.ChargedQuantity;
                                        pcdt.BaseComp3 = BaseComp3 / entityDt.ChargedQuantity;
                                        pcdt.Tariff = Tariff / entityDt.ChargedQuantity;
                                        pcdt.TariffComp1 = TariffComp1 / entityDt.ChargedQuantity;
                                        pcdt.TariffComp2 = TariffComp2 / entityDt.ChargedQuantity;
                                        pcdt.TariffComp3 = TariffComp3 / entityDt.ChargedQuantity;
                                        pcdt.DiscountAmount = DiscountAmount;
                                        pcdt.DiscountComp1 = DiscountComp1 / entityDt.ChargedQuantity;
                                        pcdt.DiscountComp2 = DiscountComp2 / entityDt.ChargedQuantity;
                                        pcdt.DiscountComp3 = DiscountComp3 / entityDt.ChargedQuantity;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), pcdt.ChargeClassID, pcdt.ItemID, 1, pcdt.CreatedDate, ctx);

                                        decimal coverageAmount = 0;
                                        bool isCoverageInPercentage = false;
                                        if (list.Count > 0)
                                        {
                                            GetCurrentItemTariff obj = list[0];
                                            coverageAmount = obj.CoverageAmount;
                                            isCoverageInPercentage = obj.IsCoverageInPercentage;
                                        }

                                        decimal grossLineAmount = (pcdt.Tariff * pcdt.ChargedQuantity) + (pcdt.CITOAmount - pcdt.CITODiscount);
                                        decimal totalDiscountAmount = pcdt.DiscountAmount;
                                        if (grossLineAmount > 0)
                                        {
                                            if (totalDiscountAmount > grossLineAmount)
                                            {
                                                totalDiscountAmount = grossLineAmount;
                                            }
                                        }

                                        decimal total = grossLineAmount - totalDiscountAmount;
                                        decimal totalPayer = 0;
                                        if (isCoverageInPercentage)
                                        {
                                            totalPayer = total * coverageAmount / 100;
                                        }
                                        else
                                        {
                                            totalPayer = coverageAmount * pcdt.ChargedQuantity;
                                        }

                                        if (total == 0)
                                        {
                                            totalPayer = total;
                                        }
                                        else
                                        {
                                            if (totalPayer < 0 && totalPayer < total)
                                            {
                                                totalPayer = total;
                                            }
                                            else if (totalPayer > 0 & totalPayer > total)
                                            {
                                                totalPayer = total;
                                            }
                                        }

                                        oPatientAmount = total - totalPayer;
                                        oPayerAmount = totalPayer;
                                        oLineAmount = total;

                                        if (hdnIsEndingAmountRoundingTo1.Value == "1")
                                        {
                                            decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                                            decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                                            if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                            {
                                                oPatientAmount = Math.Floor(oPatientAmount);
                                            }
                                            else
                                            {
                                                oPatientAmount = Math.Ceiling(oPatientAmount);
                                            }

                                            decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                                            decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                                            if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                                            {
                                                oPayerAmount = Math.Floor(oPayerAmount);
                                            }
                                            else
                                            {
                                                oPayerAmount = Math.Ceiling(oPayerAmount);
                                            }

                                            oLineAmount = oPatientAmount + oPayerAmount;
                                        }

                                        if (ims.GCItemType == Constant.ItemType.OBAT_OBATAN || ims.GCItemType == Constant.ItemType.BARANG_MEDIS || ims.GCItemType == Constant.ItemType.BARANG_UMUM || ims.GCItemType == Constant.ItemType.BAHAN_MAKANAN)
                                        {
                                            if (hdnIsEndingAmountRoundingTo100.Value == "1")
                                            {
                                                oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                                oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                                oLineAmount = oPatientAmount + oPayerAmount;
                                            }
                                        }

                                        pcdt.PatientAmount = oPatientAmount;
                                        pcdt.PayerAmount = oPayerAmount;
                                        pcdt.LineAmount = oLineAmount;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityDtDao.Update(pcdt);
                                    }
                                }
                            }
                        }

                        if (result)
                        {
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = GetErrorMsgSelectTransactionFirst();
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
            return true;
        }
    }
}