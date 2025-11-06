using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class PatientBannerCtl : BaseUserControlCtl
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Status/";
        private const string TOOLBAR_IMAGE_PATH = "~/libs/Images/Toolbar/";

        private class cBanner
        {
            private String _Header;

            public String Header
            {
                get { return _Header; }
                set { _Header = value; }
            }

            private String _Detail;

            public String Detail
            {
                get { return _Detail; }
                set { _Detail = value; }
            }

            private Boolean _IsNormal;

            public Boolean IsNormal
            {
                get { return _IsNormal; }
                set { _IsNormal = value; }
            }
        }

        protected void rptGridInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                cBanner obj = (cBanner)e.Item.DataItem;
                HtmlTableCell tdDetail = (HtmlTableCell)e.Item.FindControl("tdDetail");

                if (obj.IsNormal)
                {
                    tdDetail.Style.Add("color", "Black");
                }
                else
                {
                    tdDetail.Style.Add("color", "Red");
                    HtmlControl control = e.Item.FindControl("lblVitalSignValue") as HtmlControl;
                    if (control != null)
                    {
                        control.Attributes.Add("class", "blink-alert");
                    }
                }
            }
        }

        public void InitializePatientBanner(vRegistration3 entity)
        {

            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                        AppSession.UserLogin.HealthcareID,
                                                                        Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS,
                                                                        Constant.SettingParameter.RM_ICON_IS_PATIENT_INFECTIOUS_IS_ALLOW_DISPLAY_IN_PATIENT_BANNER,
                                                                        Constant.SettingParameter.SA_IS_VIEW_PHYSICIAN_LICENSE_NO_AT_PATIENT_BANNER
                                                                    ));
            bool isViewPhysicianLicenseNoAtPatientBanner = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.SA_IS_VIEW_PHYSICIAN_LICENSE_NO_AT_PATIENT_BANNER).FirstOrDefault().ParameterValue == "1" ? true : false;

            imgPhysician.Src = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicPictureFileName);
            imgPatientProfilePicture.Src = HttpUtility.HtmlEncode(entity.cfPatientImageUrl);
            hdnPatientGender.Value = HttpUtility.HtmlEncode(entity.GCSex);
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            lblRegistrationNo.InnerHtml = HttpUtility.HtmlEncode(entity.RegistrationNo);
            hdnRegistrationNo.Value = entity.RegistrationNo;
            hdnMRN.Value = entity.MRN.ToString();
            hdnMedicalNo.Value = entity.MedicalNo;
            hdnPatientName.Value = entity.PatientName;

            if (entity.BusinessPartnerCode != "PERSONAL")
            {
                tdPayerInfobanner.Style.Remove("display");
            }

            hdnbannerContractID.Value = entity.ContractID.ToString();
            hdnbannerPayerID.Value = entity.BusinessPartnerID.ToString();

            string filterRegFrom = string.Format("LinkedToRegistrationID = {0} AND GCRegistrationStatus <> '{1}'", entity.RegistrationID, Constant.VisitStatus.CANCELLED);
            List<Registration> lstRegFrom = BusinessLayer.GetRegistrationList(filterRegFrom);
            if (lstRegFrom.Count() > 0)
            {
                string regNoList = "";
                foreach (Registration regFrom in lstRegFrom)
                {
                    if (regNoList != "")
                    {
                        regNoList += ", ";
                    }
                    regNoList += regFrom.RegistrationNo;
                }

                lblFromRegistrationNo.InnerHtml = HttpUtility.HtmlEncode(string.Format("({0})", regNoList));
            }
            else
            {
                lblFromRegistrationNo.InnerHtml = string.Empty;
            }

            lblPatientName.InnerHtml = HttpUtility.HtmlEncode(entity.cfPatientNameSalutation);

            if (entity.OldMedicalNo != null)
            {
                lblMRN.InnerHtml = HttpUtility.HtmlEncode(entity.MedicalNo);
                spnOldMedicalNo.Style.Add("display", "block");
                lblOldMRN.InnerHtml = HttpUtility.HtmlEncode("/" + entity.OldMedicalNo);
            }
            else
            {
                lblMRN.InnerHtml = HttpUtility.HtmlEncode(entity.MedicalNo);
                spnOldMedicalNo.Style.Add("display", "none");
                lblOldMRN.InnerHtml = "";
            }

            if (entity.MRN != null && entity.MRN != 0)
            {
                List<Registration> lstOtherActiveReg = BusinessLayer.GetRegistrationList(string.Format("MRN = {0} AND GCRegistrationStatus NOT IN ('{1}','{2}') ORDER BY RegistrationDate, RegistrationID", entity.MRN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED));

                if (lstOtherActiveReg.Count > 0)
                {
                    imgIsHasOthersRegActive.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "reg_active.png");
                }
                else
                {
                    imgIsHasOthersRegActive.Style.Add("display", "none");
                }
            }
            else
            {
                imgIsHasOthersRegActive.Style.Add("display", "none");
            }

            words = ((BasePage)Page).GetWords();
            if (entity.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                lblDOB.InnerHtml = entity.cfDateOfBirthInString;

                if (entity.DischargeDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                {
                    lblPatientAge.InnerHtml = string.Format("{0} - {1}", Helper.GetPatientAgeOnDischarge(words, entity.DateOfBirth, entity.DischargeDate), entity.Religion);
                }
                else
                {
                    lblPatientAge.InnerHtml = string.Format("{0} - {1}", Helper.GetPatientAge(words, entity.DateOfBirth), entity.Religion);
                }
            }
            else
            {
                lblDOB.InnerHtml = "";
                lblPatientAge.InnerHtml = string.Format("{0} - {1}", "", entity.Religion);
            }
            if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                lblDateHour.InnerHtml = string.Format("{0} - {1}", entity.cfRegistrationDateInString, entity.RegistrationTime);
            else
                lblDateHour.InnerHtml = string.Format("{0} - {1} s/d {2} - {3}", entity.cfRegistrationDateInString, entity.RegistrationTime, entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), entity.DischargeTime);
            lblPayer.InnerHtml = HttpUtility.HtmlEncode(entity.cfBusinessPartner);
            if (entity.isHasGuaranteeLetterExists != 0)
            {
                lblPayerInfo.InnerHtml = HttpUtility.HtmlEncode(string.Format("Memiliki Kontrol Surat Jaminan"));
            }
            lblGender.InnerHtml = HttpUtility.HtmlEncode(entity.Sex);
            lblPatientCategory.InnerHtml = HttpUtility.HtmlEncode(entity.PatientCategory);

            if (entity.GCTriage != "")
                divPatientBannerImgInfo.Style.Add("background-color", entity.TriageColor);
            else
                divPatientBannerImgInfo.Style.Add("display", "none");

            #region Patient Status
            if (entity.IsHasAllergy)
            {
                divPatientStatusAllergy.Style.Add("background-color", "red");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("A");
            }
            else
            {
                divPatientStatusAllergy.Style.Add("background-color", "white");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("");
            }
            if (entity.IsFallRisk)
            {
                divPatientStatusFallRisk.Style.Add("background-color", "yellow");
                divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("F");
            }
            else
            {
                divPatientStatusFallRisk.Style.Add("background-color", "white");
                divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("");
            }
            if (entity.IsDNR)
            {
                divPatientStatusDNR.Style.Add("background-color", "purple");
                divPatientStatusDNR.InnerHtml = HttpUtility.HtmlEncode("D");
            }
            else
            {
                divPatientStatusDNR.Style.Add("background-color", "white");
                divPatientStatusDNR.InnerHtml = HttpUtility.HtmlEncode("");
            }
            if (entity.IsGeriatricPatient)
            {
                divIsGeriatricPatient.Style.Add("background-color", "Blue");
                divIsGeriatricPatient.InnerHtml = HttpUtility.HtmlEncode("G");
            }
            else
            {
                divIsGeriatricPatient.Style.Add("background-color", "white");
                divIsGeriatricPatient.InnerHtml = HttpUtility.HtmlEncode("");
            }
            if (entity.MRN != 0 && entity.MRN != null)
            {
                Patient opatient = BusinessLayer.GetPatient(entity.MRN);
                if (opatient.IsHasInfectious)
                {
                    if (hdnIsInfectiousIconIsAllowDisplay.Value == "1")
                    {
                        imgIsHasInfectious.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "infectious.PNG");
                    }
                    else
                    {
                        imgIsHasInfectious.Style.Add("display", "none");
                    }
                }
                else
                {
                    imgIsHasInfectious.Style.Add("display", "none");
                }

                if (opatient.IsVIP)
                {
                    imgIsVIP.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "VIP.PNG");

                    if (opatient.GCVIPGroup != Constant.VIPPatientGroup.VIP_OTHER)
                    {
                        StandardCode sc = BusinessLayer.GetStandardCode(opatient.GCVIPGroup);
                        imgIsVIP.Attributes.Add("title", String.Format("VIP Group : {0}", sc.StandardCodeName));
                    }
                    else
                    {
                        imgIsVIP.Attributes.Add("title", String.Format("VIP Group : {0}", opatient.OtherVIPGroup));
                    }
                }

                if (opatient.IsHasCommunicationRestriction)
                {
                    imgIsHasCommunicationRestriction.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "CommunicationRestriction.PNG");
                }

                if (opatient.IsHasPhysicalLimitation)
                {
                    imgIsHasPhysicalLimitation.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "PhysicalLimitation.PNG");
                }

                if (opatient.IsBlackList)
                {
                    imgIsBlacklist.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "blacklist.png");
                    imgIsBlacklist.Style.Add("class", "blink-alert");
                    imgIsBlacklist.Attributes.Add("class", "blink-alert");
                }
                else
                {
                    imgIsBlacklist.Attributes.Remove("class");
                }

                if (!opatient.IsAlive)
                {
                    imgIsAlive.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "RIP.PNG");

                    if (opatient.DateOfDeath.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                    {
                        string info = String.Format("died on {0}", opatient.DateOfDeath.ToString("dd-MM-yyyy"));
                        imgIsAlive.Attributes.Add("title", info);
                    }
                }
            }

            if (entity.IsPregnant)
            {
                imgIsPregnant.Src = string.Format("{0}{1}", TOOLBAR_IMAGE_PATH, "obsgyn_history.png");
            }
            #endregion

            lblPhysicianName.InnerHtml = HttpUtility.HtmlEncode(entity.ParamedicName);
            if (isViewPhysicianLicenseNoAtPatientBanner)
            {
                if (entity.LicenseNo != null && entity.LicenseNo != "")
                {
                    lblParamedicLicenseNo.InnerHtml = HttpUtility.HtmlEncode(entity.LicenseNo);
                }
            }
            lblParamedicType.InnerHtml = HttpUtility.HtmlEncode(entity.SpecialtyName);
            if (entity.cfSpecialistPhysicianGrading != null && entity.cfSpecialistPhysicianGrading != "")
            {
                lblGradingInfo.InnerHtml = HttpUtility.HtmlEncode("(" + entity.cfSpecialistPhysicianGrading + ")");
            }
            else
            {
                lblGradingInfo.InnerHtml = HttpUtility.HtmlEncode(entity.cfSpecialistPhysicianGrading);
            }

            if (entity.PatientAllergy.Length > 20)
                lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.PatientAllergy.Substring(0, 20).ToUpper()) + "...";
            else
                lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.PatientAllergy.ToUpper());

            AppSession.IsHasAllergy = entity.IsHasAllergy;
            AppSession.PatientAllergyInfo = entity.PatientAllergy.ToUpper();

            if (entity.GCCustomerType != Constant.CustomerType.PERSONAL)
            {
                string filterPaymentPersonal = string.Format("RegistrationID = '{0}' AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}' AND IsDeleted = 0",
                                                                entity.RegistrationID, Constant.PaymentType.AR_PATIENT, Constant.TransactionStatus.VOID);
                List<PatientPaymentHd> lstPaymentPersonalAR = BusinessLayer.GetPatientPaymentHdList(filterPaymentPersonal);

                decimal payment = entity.PaymentAmount;
                decimal totalLineAmount = (entity.ChargesAmount + entity.SourceAmount + entity.AdminAmount - entity.DiscountAmount + entity.RoundingAmount - entity.TransferAmount);
                decimal coverage = entity.CoverageLimitAmount;
                decimal payerCoverage = entity.ARInProcessAmount - lstPaymentPersonalAR.Sum(t => t.TotalPaymentAmount);
                decimal PatientARAmount = lstPaymentPersonalAR.Sum(t => t.TotalPaymentAmount);
                decimal patientCoverage = totalLineAmount - entity.ARInProcessAmount;
                decimal remaining = entity.PaymentAmount - totalLineAmount;
                //decimal remaining = totalLineAmount - (entity.ARInProcessAmount + (totalLineAmount - entity.ARInProcessAmount));

                if (payment < totalLineAmount)
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_warning.png");
                }
                else
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_ok.png");
                }

                imgCoverage.Attributes.Add("title", String.Format("Coverage Amount = {0}\nPayer Amount = {1}\nPatient AR Amount = {2}\nPatient Amount = {3}\nTotal Amount = {4}\nRemaining Amount = {5}",
                                                    coverage.ToString(Constant.FormatString.NUMERIC_2),
                                                    payerCoverage.ToString(Constant.FormatString.NUMERIC_2),
                                                    PatientARAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                    patientCoverage.ToString(Constant.FormatString.NUMERIC_2),
                                                    totalLineAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                    remaining.ToString(Constant.FormatString.NUMERIC_2)));
            }
            else
            {
                decimal payment = entity.PaymentAmount;
                decimal totalLineAmount = (entity.ChargesAmount + entity.SourceAmount + entity.AdminAmount - entity.DiscountAmount + entity.RoundingAmount - entity.TransferAmount);
                decimal remaining = entity.PaymentAmount - totalLineAmount;

                if (payment < totalLineAmount)
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_warning.png");
                }
                else
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_ok.png");
                }

                imgCoverage.Attributes.Add("title", String.Format("Payment Amount = {0}\nTotal Amount = {1}\nRemaining Amount = {2}",
                                                    payment.ToString(Constant.FormatString.NUMERIC_2),
                                                    totalLineAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                    remaining.ToString(Constant.FormatString.NUMERIC_2)));
            }

            string filterDeposit = string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN);
            PatientDepositBalance pDepositBalance = BusinessLayer.GetPatientDepositBalanceList(filterDeposit).LastOrDefault();
            if (pDepositBalance != null)
            {
                if (pDepositBalance.BalanceEND == 0)
                {
                    imgPatientWallet.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "patientwallet_empty.png");
                }
                else
                {
                    imgPatientWallet.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "patientwallet.png");
                }
                imgPatientWallet.Attributes.Add("title", String.Format("Saldo = {0}", pDepositBalance.BalanceEND.ToString(Constant.FormatString.NUMERIC_2)));
            }
            else
            {
                imgPatientWallet.Style.Add("display", "none");
            }

            lblSEPNo.InnerText = "-";
            if (entity.GCCustomerType == lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue)
            {
                tdLegal.Style.Remove("visibility");
                if (entity.NoSEP != null && entity.NoSEP != "")
                {
                    if (string.IsNullOrEmpty(entity.NoSEP.Trim()))
                    {
                        imgSEPInfo.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "no_sep.png");
                    }
                    else
                    {
                        imgSEPInfo.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "sep.png");

                        if (entity.IsManualSEP)
                        {
                            lblSEPNo.InnerText = string.Format("{0}*", entity.NoSEP);
                        }
                        else
                        {
                            lblSEPNo.InnerText = entity.NoSEP;
                        }
                    }
                }
                else
                {
                    imgSEPInfo.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "no_sep.png");
                }
            }

            lblSJPNo.InnerText = "-";
            if (entity.NoSJP != null && entity.NoSJP != "")
            {
                if (entity.IsManualSJP)
                {
                    lblSJPNo.InnerText = string.Format("{0}*", entity.NoSJP);
                }
                else
                {
                    lblSJPNo.InnerText = entity.NoSJP;
                }
            }

            lblReferralNo.InnerText = "-";
            if (entity.ReferralNo != null && entity.ReferralNo != "")
            {
                lblReferralNo.InnerText = entity.ReferralNo;
            }

            hdnDepartmentID.Value = entity.DepartmentID;
            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                divOutpatientInfoLine1.Style.Add("display", "none");
                divOutpatientInfoLine2.Style.Add("display", "none");

                lblInpatientClass.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0} | {1}", entity.ClassName, entity.ChargeClassName));

                if (entity.BusinessPartnerID != 1 && entity.IsControlClassCare)
                {
                    if (entity.ControlClassID != null && entity.ControlClassID != 0)
                    {
                        ClassCare oControlClass = BusinessLayer.GetClassCare(entity.ControlClassID);
                        lblInpatientControlClass.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0}", oControlClass.ClassName));
                    }
                    else
                    {
                        divInpatientInfoLine1b.Style.Add("display", "none");
                    }
                }
                else
                {
                    divInpatientInfoLine1b.Style.Add("display", "none");
                }

                if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);

                    decimal totalLineAmount = (entity.ChargesAmount + entity.SourceAmount + entity.AdminAmount - entity.DiscountAmount + entity.RoundingAmount);
                    decimal sisaSaldoINA = ((entity.INADitempati * entity.Pengali_INADitempati) - totalLineAmount);

                    imgSEPInfo.Attributes.Add("title", String.Format("INACBG Grouper = {0}\nINACBG Hak Pasien = {1}\nINACBG Ditempati = {2}\nPengali INACBG Ditempati = {3}\nSisa Saldo INACBG = {4}",
                                                            entityRegBPJS.GrouperCodeClaim, //0
                                                            entity.INAHakPasien.ToString(Constant.FormatString.NUMERIC_2), //1
                                                            entity.INADitempati.ToString(Constant.FormatString.NUMERIC_2), //2
                                                            entity.Pengali_INADitempati.ToString(Constant.FormatString.NUMERIC_2), //3
                                                            sisaSaldoINA.ToString(Constant.FormatString.NUMERIC_2) //4
                                                        )); 
                }

                string isTempBed = "";
                if (entity.BedID != 0 && entity.BedID != null)
                {
                    Bed entityBed = BusinessLayer.GetBed(entity.BedID);
                    if (entityBed != null)
                    {
                        if (entityBed.IsTemporary)
                        {
                            isTempBed = "*";
                        }
                    }
                }

                lblInpatientWard.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0} | {1} | {3}{4} ({2})", entity.ServiceUnitName, entity.RoomName, (DateTime.Now - entity.RegistrationDate).Days, entity.BedCode, isTempBed));

                DateTime date = DateTime.Now;
                if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                    date = entity.DischargeDate;
                lblInpatientLOS.InnerHtml = string.Format("{0} hari", (date - entity.RegistrationDate).Days);
            }
            else
            {
                divInpatientInfoLine1.Style.Add("display", "none");
                divInpatientInfoLine1b.Style.Add("display", "none");
                divInpatientInfoLine2.Style.Add("display", "none");

                lblServiceUnit.InnerHtml = HttpUtility.HtmlEncode(entity.ServiceUnitName);
                lblVisitType.InnerHtml = HttpUtility.HtmlEncode(entity.VisitTypeName);

                if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);
                    if (entityRegBPJS != null)
                    {
                        imgSEPInfo.Attributes.Add("title", String.Format("INACBG Grouper = {0}\nINACBG Hak Pasien = {1}\nINACBG Ditempati = {2}",
                                                                entityRegBPJS.GrouperCodeClaim, //0
                                                                entityRegBPJS.INAHakPasien.ToString(Constant.FormatString.NUMERIC_2), //1
                                                                entityRegBPJS.INADitempati.ToString(Constant.FormatString.NUMERIC_2) //2
                                                            ));
                    }
                }
            }

            if (entity.GCCustomerType == Constant.CustomerType.BPJS)
            {
                if (!string.IsNullOrEmpty(entity.NamaKelasTanggungan))
                {
                    lblInpatientHakKelas.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0}", entity.NamaKelasTanggungan));
                }
                else
                {
                    lblInpatientHakKelas.InnerHtml = "";
                    divHakKelasBPJS.Style.Add("display", "none");
                }
            }
            else
            {
                lblInpatientHakKelas.InnerHtml = "";
                divHakKelasBPJS.Style.Add("display", "none");
            }

            string filterCVIP = string.Format("RegistrationID = {0} AND IsDeleted = 0", entity.RegistrationID);
            List<vConsultVisitItemPackage1> listCVIP = BusinessLayer.GetvConsultVisitItemPackage1List(filterCVIP);

            if (listCVIP.Where(a => !a.IsPackageAllInOne).ToList().Count() > 0)
            {
                int cvipCount = 0;
                string infoCVIP = string.Format("PaketAIO = ");
                foreach (vConsultVisitItemPackage1 cvip in listCVIP)
                {
                    if (cvipCount != 0)
                    {
                        infoCVIP += ", ";
                    }

                    infoCVIP += string.Format("{0} ({1})", cvip.ItemName1, cvip.ItemCode);

                    cvipCount += 1;
                }
                imgRegHasPackageMCU.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "has_mcu_package.png");
                imgRegHasPackageMCU.Attributes.Add("title", infoCVIP);
            }
            else
            {
                imgRegHasPackageMCU.Style.Add("display", "none");
            }

            if (listCVIP.Where(a => a.IsPackageAllInOne).ToList().Count() > 0)
            {
                int cvipCount = 0;
                string infoCVIP = string.Format("PaketAIO = ");
                foreach (vConsultVisitItemPackage1 cvip in listCVIP)
                {
                    if (cvipCount != 0)
                    {
                        infoCVIP += ", ";
                    }

                    infoCVIP += string.Format("{0} ({1})", cvip.ItemName1, cvip.ItemCode);

                    cvipCount += 1;
                }
                imgRegHasPackageAIO.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "has_aio_package.png");
                imgRegHasPackageAIO.Attributes.Add("title", infoCVIP);
            }
            else
            {
                imgRegHasPackageAIO.Style.Add("display", "none");
            }

            string filterExpression = string.Format("RegistrationID = {0}", entity.RegistrationID);
            List<vRegistrationOutstandingInfo> lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression);
            int outstanding = (lstInfo.Sum(info => info.ServiceOrder + info.PrescriptionOrder + info.PrescriptionReturnOrder + info.LaboratoriumOrder + info.RadiologiOrder + info.OtherOrder) > 0) ? 1 : 0;

            if (outstanding == 1)
            {
                imgOutstandingOrder.Src = string.Format("{0}{1}", TOOLBAR_IMAGE_PATH, "outstanding_order.png");
            }
            else
            {
                imgOutstandingOrder.Style.Add("display", "none");
            }

            imgNeedTranslator.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "need_translator.png");
            if (entity.Penterjemah == "1")
            {
                imgNeedTranslator.Style.Remove("display");
            }
            else
            {
                imgNeedTranslator.Style.Add("display", "none");
            }

            #region Vital Sign Banner
            InitializePatientVitalSignBanner(entity.LastGlucoseLevel);
            #endregion
        }

        private void InitializePatientVitalSignBanner(string glucoseInfo)
        {

            List<PatientBannerVitalSignInfo> lstVitalSignInfo = BusinessLayer.GetPatientBannerVitalSignInfo(AppSession.RegisteredPatient.RegistrationID);
            if (lstVitalSignInfo.Count > 0)
            {
                List<cBanner> lst = new List<cBanner>();
                foreach (PatientBannerVitalSignInfo item in lstVitalSignInfo)
                {
                    string[] vitalSignInfo = item.VitalSignInfo.Split('|');
                    cBanner vitalScore = new cBanner() { Header = vitalSignInfo[0], Detail = vitalSignInfo[1], IsNormal = (vitalSignInfo[6] == "1" ? true : false) };
                    lst.Add(vitalScore);
                }

                //String xGlucose = "-";

                //if (glucoseInfo != null && glucoseInfo != "")
                //{
                //    xGlucose = glucoseInfo;
                //}

                //cBanner glucoseScore = new cBanner() { Header = "Glucose", Detail = xGlucose, IsNormal = true };
                //lst.Add(glucoseScore);

                rptGridInfo.DataSource = lst;
                rptGridInfo.DataBind();
            }
            else
            {
                rptGridInfo.Visible = false;
            }

        }

        private void SetLinkPatientStatus(int mrn)
        {
            Patient oPatient = BusinessLayer.GetPatient(mrn);
            if (oPatient != null)
            {
                if (oPatient.IsHasAllergy)
                {
                    divPatientStatusAllergy.Style.Add("background-color", "red");
                    divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("A");
                }
                else
                {
                    divPatientStatusAllergy.Style.Add("background-color", "white");
                    divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("");
                }
                divPatientStatusFallRisk.Style.Add("background-color", "white");
                divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("");
                divPatientStatusDNR.Style.Add("background-color", "white");
                divPatientStatusDNR.InnerHtml = HttpUtility.HtmlEncode("");
            }
        }

        protected string GetServiceUnitLabel()
        {
            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                return GetLabel("Klinik");
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                return GetLabel("Penunjang Medis");
            return "";
        }

        public void InitializePatientBanner(vConsultVisit2 entity)
        {
            string VisitID = Page.Request.QueryString["FNVisitID"]; //dari finanace >> pembayaran
            if (!string.IsNullOrEmpty(VisitID))
            {
                string p = "";
                if (AppSession.RegisteredPatient.VisitID != Convert.ToInt32(VisitID))
                {

                    vConsultVisit4 entityVisit = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID='{0}'", VisitID)).FirstOrDefault();
                    RegisteredPatient pt = new RegisteredPatient();
                    pt.MRN = entityVisit.MRN;
                    pt.MedicalNo = entityVisit.MedicalNo;
                    pt.PatientName = entityVisit.PatientName;
                    pt.GCGender = entityVisit.GCGender;
                    pt.GCSex = entityVisit.GCSex;
                    pt.DateOfBirth = entityVisit.DateOfBirth;
                    pt.RegistrationID = entityVisit.RegistrationID;
                    pt.RegistrationNo = entityVisit.RegistrationNo;
                    pt.RegistrationDate = entityVisit.RegistrationDate;
                    pt.RegistrationTime = entityVisit.RegistrationTime;
                    pt.VisitID = entityVisit.VisitID;
                    pt.VisitDate = entityVisit.VisitDate;
                    pt.VisitTime = entityVisit.VisitTime;
                    pt.StartServiceDate = entityVisit.StartServiceDate;
                    pt.StartServiceTime = entityVisit.StartServiceTime;
                    pt.DischargeDate = entityVisit.DischargeDate;
                    pt.DischargeTime = entityVisit.DischargeTime;
                    pt.GCCustomerType = entityVisit.GCCustomerType;
                    pt.BusinessPartnerID = entityVisit.BusinessPartnerID;
                    pt.ParamedicID = entityVisit.ParamedicID;
                    pt.ParamedicCode = entityVisit.ParamedicCode;
                    pt.ParamedicName = entityVisit.ParamedicName;
                    pt.SpecialtyID = entityVisit.SpecialtyID;
                    pt.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                    pt.DepartmentID = entityVisit.DepartmentID;
                    pt.ServiceUnitName = entityVisit.ServiceUnitName;
                    pt.RoomCode = entityVisit.RoomCode;
                    pt.BedCode = entityVisit.BedCode;
                    pt.DepartmentID = entityVisit.DepartmentID;
                    pt.ChargeClassID = entityVisit.ChargeClassID;
                    pt.ClassID = entityVisit.ClassID;
                    pt.GCRegistrationStatus = entityVisit.GCVisitStatus;
                    pt.IsLockDown = entityVisit.IsLockDown;
                    pt.IsBillingReopen = entityVisit.IsBillingReopen;
                    pt.LinkedRegistrationID = entityVisit.LinkedRegistrationID;
                    pt.LinkedToRegistrationID = entityVisit.LinkedToRegistrationID;
                    pt.LastAcuteInitialAssessmentDate = entityVisit.LastAcuteInitialAssessmentDate;
                    pt.LastChronicInitialAssessmentDate = entityVisit.LastChronicInitialAssessmentDate;
                    pt.IsNeedRenewalAcuteInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastAcuteInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
                    pt.IsNeedRenewalChronicInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastChronicInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
                    pt.OpenFromModuleID = "FN";
                    AppSession.RegisteredPatient = pt;
                    string url = Request.Url.AbsoluteUri;
                    Response.Redirect(url);
                }
            }
            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                        AppSession.UserLogin.HealthcareID,
                                                                        Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS,
                                                                        Constant.SettingParameter.RM_ICON_IS_PATIENT_INFECTIOUS_IS_ALLOW_DISPLAY_IN_PATIENT_BANNER,
                                                                        Constant.SettingParameter.SA_IS_VIEW_PHYSICIAN_LICENSE_NO_AT_PATIENT_BANNER
                                                                    ));
            bool isViewPhysicianLicenseNoAtPatientBanner = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.SA_IS_VIEW_PHYSICIAN_LICENSE_NO_AT_PATIENT_BANNER).FirstOrDefault().ParameterValue == "1" ? true : false;

            hdnIsInfectiousIconIsAllowDisplay.Value = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.RM_ICON_IS_PATIENT_INFECTIOUS_IS_ALLOW_DISPLAY_IN_PATIENT_BANNER).FirstOrDefault().ParameterValue;

            imgPhysician.Src = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicPictureFileName);
            imgPatientProfilePicture.Src = HttpUtility.HtmlEncode(entity.PatientImageUrl);
            hdnPatientGender.Value = HttpUtility.HtmlEncode(entity.GCSex);
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            lblRegistrationNo.InnerHtml = HttpUtility.HtmlEncode(entity.RegistrationNo);
            hdnRegistrationNo.Value = entity.RegistrationNo;
            hdnMRN.Value = entity.MRN.ToString();
            hdnGuestID.Value = entity.GuestID.ToString();
            hdnMedicalNo.Value = entity.MedicalNo;
            hdnPatientName.Value = entity.PatientName;

            if (entity.PayerCode != "PERSONAL")
            {
                tdPayerInfobanner.Style.Remove("display");
            }
            hdnbannerContractID.Value = entity.ContractID.ToString();
            hdnbannerPayerID.Value = entity.BusinessPartnerID.ToString();

            string filterRegFrom = string.Format("LinkedToRegistrationID = {0} AND GCRegistrationStatus <> '{1}'", entity.RegistrationID, Constant.VisitStatus.CANCELLED);
            List<Registration> lstRegFrom = BusinessLayer.GetRegistrationList(filterRegFrom);
            if (lstRegFrom.Count() > 0)
            {
                string regNoList = "";
                foreach (Registration regFrom in lstRegFrom)
                {
                    if (regNoList != "")
                    {
                        regNoList += ", ";
                    }
                    regNoList += regFrom.RegistrationNo;
                }

                lblFromRegistrationNo.InnerHtml = HttpUtility.HtmlEncode(string.Format("({0})", regNoList));
            }
            else
            {
                lblFromRegistrationNo.InnerHtml = string.Empty;
            }

            lblPatientName.InnerHtml = HttpUtility.HtmlEncode(entity.cfPatientNameSalutation);

            lblMRN.InnerHtml = HttpUtility.HtmlEncode(entity.MedicalNo);

            if (entity.MRN != null && entity.MRN != 0)
            {
                List<Registration> lstOtherActiveReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID != {0} AND MRN = {1} AND GCRegistrationStatus NOT IN ('{2}','{3}') ORDER BY RegistrationDate, RegistrationID", entity.RegistrationID, entity.MRN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED));
                if (lstOtherActiveReg.Count() > 0)
                {
                    imgIsHasOthersRegActive.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "reg_active.png");
                }
                else
                {
                    imgIsHasOthersRegActive.Style.Add("display", "none");
                }
            }
            else
            {
                imgIsHasOthersRegActive.Style.Add("display", "none");
            }

            words = ((BasePage)Page).GetWords();

            if (entity.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                lblDOB.InnerHtml = entity.DateOfBirthInString;

                if (entity.DischargeDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                {
                    lblPatientAge.InnerHtml = string.Format("{0} - {1}", Helper.GetPatientAgeOnDischarge(words, entity.DateOfBirth, entity.DischargeDate), entity.Religion);
                }
                else
                {
                    lblPatientAge.InnerHtml = string.Format("{0} - {1}", Helper.GetPatientAge(words, entity.DateOfBirth), entity.Religion);
                }
            }
            else
            {
                lblDOB.InnerHtml = "";
                lblPatientAge.InnerHtml = string.Format("{0} - {1}", "", entity.Religion);
            }
            if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                lblDateHour.InnerHtml = string.Format("{0} - {1}", entity.VisitDateInString, entity.VisitTime);
            else
                lblDateHour.InnerHtml = string.Format("{0} - {1} s/d {2} - {3}", entity.VisitDateInString, entity.VisitTime, entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), entity.DischargeTime);

            if (entity.ReferralPhysicianName != null && entity.ReferralPhysicianName != "")
            {
                lblReferral.InnerHtml = HttpUtility.HtmlEncode(entity.ReferralPhysicianName);
            }
            else
            {
                lblReferral.InnerHtml = HttpUtility.HtmlEncode(entity.ReferrerName);
            }
            if (entity.BusinessPartnerID != 1)
            {
                lblPayer.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0} ({1})", entity.BusinessPartner, entity.CoverageTypeName));
                if (entity.isHasGuaranteeLetterExists != 0)
                {
                    lblPayerInfo.InnerHtml = HttpUtility.HtmlEncode(string.Format("Memiliki Kontrol Surat Jaminan"));
                }
            }
            else
            {
                lblPayer.InnerHtml = HttpUtility.HtmlEncode(entity.BusinessPartner);
                if (entity.PromotionSchemeID != 0)
                {
                    lblPayer.Attributes.Add("class", "blink");
                    lblPayer.Attributes.Add("title", entity.PromotionSchemeName);
                }
            }

            lblGender.InnerHtml = HttpUtility.HtmlEncode(entity.Sex);

            if (entity.OldMedicalNo != null)
            {
                lblMRN.InnerHtml = HttpUtility.HtmlEncode(entity.MedicalNo);
                spnOldMedicalNo.Style.Add("Display", "block");
                lblOldMRN.InnerHtml = HttpUtility.HtmlEncode(entity.OldMedicalNo);
            }
            else
            {
                lblMRN.InnerHtml = HttpUtility.HtmlEncode(entity.MedicalNo);
                spnOldMedicalNo.Style.Add("Display", "none");
            }

            lblPhysicianName.InnerHtml = HttpUtility.HtmlEncode(entity.ParamedicName);
            if (isViewPhysicianLicenseNoAtPatientBanner)
            {
                if (entity.LicenseNo != null && entity.LicenseNo != "")
                {
                    lblParamedicLicenseNo.InnerHtml = HttpUtility.HtmlEncode(entity.LicenseNo);
                }
            }
            lblParamedicType.InnerHtml = HttpUtility.HtmlEncode(entity.SpecialtyName);
            if (entity.cfSpecialistPhysicianGrading != null && entity.cfSpecialistPhysicianGrading != "")
            {
                lblGradingInfo.InnerHtml = HttpUtility.HtmlEncode("(" + entity.cfSpecialistPhysicianGrading + ")");
            }
            else
            {
                lblGradingInfo.InnerHtml = HttpUtility.HtmlEncode(entity.cfSpecialistPhysicianGrading);
            }

            SetAllergyInfo(entity);

            lblPatientCategory.InnerHtml = HttpUtility.HtmlEncode(entity.PatientCategory);

            if (entity.GCCustomerType != Constant.CustomerType.PERSONAL)
            {
                string filterPaymentPersonal = string.Format("RegistrationID = '{0}' AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}' AND IsDeleted = 0",
                                                                entity.RegistrationID, Constant.PaymentType.AR_PATIENT, Constant.TransactionStatus.VOID);
                List<PatientPaymentHd> lstPaymentPersonalAR = BusinessLayer.GetPatientPaymentHdList(filterPaymentPersonal);

                decimal payment = entity.PaymentAmount;
                decimal totalLineAmount = (entity.ChargesAmount + entity.SourceAmount + entity.AdminAmount - entity.DiscountAmount + entity.RoundingAmount - entity.TransferAmount);
                decimal coverage = entity.CoverageLimitAmount;
                decimal payerCoverage = entity.ARInProcessAmount - lstPaymentPersonalAR.Sum(t => t.TotalPaymentAmount);
                decimal PatientARAmount = lstPaymentPersonalAR.Sum(t => t.TotalPaymentAmount);
                decimal patientCoverage = totalLineAmount - entity.ARInProcessAmount;
                decimal remaining = entity.PaymentAmount - totalLineAmount;
                //decimal remaining = totalLineAmount - (entity.ARInProcessAmount + (totalLineAmount - entity.ARInProcessAmount));

                if (payment < totalLineAmount)
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_warning.png");
                }
                else
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_ok.png");
                }

                imgCoverage.Attributes.Add("title", String.Format("Coverage Amount = {0}\nPayer Amount = {1}\nPatient AR Amount = {2}\nPatient Amount = {3}\nTotal Amount = {4}\nRemaining Amount = {5}",
                                                    coverage.ToString(Constant.FormatString.NUMERIC_2),
                                                    payerCoverage.ToString(Constant.FormatString.NUMERIC_2),
                                                    PatientARAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                    patientCoverage.ToString(Constant.FormatString.NUMERIC_2),
                                                    totalLineAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                    remaining.ToString(Constant.FormatString.NUMERIC_2)));
            }
            else
            {
                decimal payment = entity.PaymentAmount;
                decimal totalLineAmount = (entity.ChargesAmount + entity.SourceAmount + entity.AdminAmount - entity.DiscountAmount + entity.RoundingAmount - entity.TransferAmount);
                decimal remaining = entity.PaymentAmount - totalLineAmount;

                if (payment < totalLineAmount)
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_warning.png");
                }
                else
                {
                    imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_ok.png");
                }

                imgCoverage.Attributes.Add("title", String.Format("Payment Amount = {0}\nTotal Amount = {1}\nRemaining Amount = {2}",
                                                    payment.ToString(Constant.FormatString.NUMERIC_2),
                                                    totalLineAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                    remaining.ToString(Constant.FormatString.NUMERIC_2)));
            }

            string filterDeposit = string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN);
            PatientDepositBalance pDepositBalance = BusinessLayer.GetPatientDepositBalanceList(filterDeposit).LastOrDefault();
            if (pDepositBalance != null)
            {
                if (pDepositBalance.BalanceEND == 0)
                {
                    imgPatientWallet.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "patientwallet_empty.png");
                }
                else
                {
                    imgPatientWallet.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "patientwallet.png");
                }
                imgPatientWallet.Attributes.Add("title", String.Format("Saldo = {0}", pDepositBalance.BalanceEND.ToString(Constant.FormatString.NUMERIC_2)));
            }
            else
            {
                imgPatientWallet.Style.Add("display", "none");
            }

            if (entity.IsVisitorRestriction)
            {
                imgVisitorRestriction.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "visitor_restriction.png");
            }
            else
            {
                imgVisitorRestriction.Style.Add("display", "none");
            }

            lblSEPNo.InnerText = "-";
            if (entity.GCCustomerType == lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue)
            {
                tdLegal.Style.Remove("visibility");
                if (entity.NoSEP != null && entity.NoSEP != "")
                {
                    if (string.IsNullOrEmpty(entity.NoSEP.Trim()))
                    {
                        imgSEPInfo.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "no_sep.png");
                    }
                    else
                    {
                        imgSEPInfo.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "sep.png");

                        if (entity.IsManualSEP)
                        {
                            lblSEPNo.InnerText = string.Format("{0}*", entity.NoSEP);
                        }
                        else
                        {
                            lblSEPNo.InnerText = entity.NoSEP;
                        }
                    }
                }
                else
                {
                    imgSEPInfo.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "no_sep.png");
                }
            }

            lblSJPNo.InnerText = "-";
            if (entity.NoSJP != null && entity.NoSJP != "")
            {
                if (entity.IsManualSJP)
                {
                    lblSJPNo.InnerText = string.Format("{0}*", entity.NoSJP);
                }
                else
                {
                    lblSJPNo.InnerText = entity.NoSJP;
                }
            }

            lblReferralNo.InnerText = "-";
            if (entity.ReferralNo != null && entity.ReferralNo != "")
            {
                lblReferralNo.InnerText = entity.ReferralNo;
            }

            hdnDepartmentID.Value = entity.DepartmentID;

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                divOutpatientInfoLine1.Style.Add("display", "none");
                divOutpatientInfoLine2.Style.Add("display", "none");

                lblInpatientClass.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0} | {1}", entity.ClassName, entity.ChargeClassName));

                if (entity.BusinessPartnerID != 1 && entity.IsControlClassCare)
                {
                    if (entity.ControlClassID != null && entity.ControlClassID != 0)
                    {
                        ClassCare oControlClass = BusinessLayer.GetClassCare(entity.ControlClassID);
                        lblInpatientControlClass.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0}", oControlClass.ClassName));
                    }
                    else
                    {
                        divInpatientInfoLine1b.Style.Add("display", "none");
                    }
                }
                else
                {
                    divInpatientInfoLine1b.Style.Add("display", "none");
                }

                if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);

                    decimal totalLineAmount = (entity.ChargesAmount + entity.SourceAmount + entity.AdminAmount - entity.DiscountAmount + entity.RoundingAmount);
                    decimal sisaSaldoINA = ((entity.INADitempati * entity.Pengali_INADitempati) - totalLineAmount);

                    imgSEPInfo.Attributes.Add("title", String.Format("INACBG Grouper = {0}\nINACBG Hak Pasien = {1}\nINACBG Ditempati = {2}\nPengali INACBG Ditempati = {3}\nSisa Saldo INACBG = {4}",
                                                            entityRegBPJS.GrouperCodeClaim, //0
                                                            entity.INAHakPasien.ToString(Constant.FormatString.NUMERIC_2), //1
                                                            entity.INADitempati.ToString(Constant.FormatString.NUMERIC_2), //2
                                                            entity.Pengali_INADitempati.ToString(Constant.FormatString.NUMERIC_2), //3
                                                            sisaSaldoINA.ToString(Constant.FormatString.NUMERIC_2) //4
                                                        ));
                }

                string isTempBed = "";
                if (entity.BedID != 0 && entity.BedID != null)
                {
                    Bed entityBed = BusinessLayer.GetBed(entity.BedID);
                    if (entityBed != null)
                    {
                        if (entityBed.IsTemporary)
                        {
                            isTempBed = "*";
                        }
                    }
                }

                lblInpatientWard.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0} | {1} | {2}{3}", entity.ServiceUnitName, entity.RoomName, entity.BedCode, isTempBed));

                DateTime date = DateTime.Now;
                if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                    date = entity.DischargeDate;
                lblInpatientLOS.InnerHtml = string.Format("{0} hari", (date - entity.VisitDate).Days);
            }
            else
            {
                divInpatientInfoLine1.Style.Add("display", "none");
                divInpatientInfoLine1b.Style.Add("display", "none");
                divInpatientInfoLine2.Style.Add("display", "none");

                lblServiceUnit.InnerHtml = HttpUtility.HtmlEncode(entity.ServiceUnitName);
                lblVisitType.InnerHtml = HttpUtility.HtmlEncode(entity.VisitTypeName);

                if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);
                    if (entityRegBPJS != null)
                    {
                        imgSEPInfo.Attributes.Add("title", String.Format("INACBG Grouper = {0}\nINACBG Hak Pasien = {1}\nINACBG Ditempati = {2}",
                                                                entityRegBPJS.GrouperCodeClaim, //0
                                                                entityRegBPJS.INAHakPasien.ToString(Constant.FormatString.NUMERIC_2), //1
                                                                entityRegBPJS.INADitempati.ToString(Constant.FormatString.NUMERIC_2) //2
                                                            ));
                    }
                }
            }

            if (entity.GCCustomerType == Constant.CustomerType.BPJS)
            {
                if (!string.IsNullOrEmpty(entity.NamaKelasTanggungan))
                {
                    lblInpatientHakKelas.InnerHtml = HttpUtility.HtmlEncode(string.Format("{0}", entity.NamaKelasTanggungan));
                }
                else
                {
                    lblInpatientHakKelas.InnerHtml = "";
                    divHakKelasBPJS.Style.Add("display", "none");
                }
            }
            else
            {
                lblInpatientHakKelas.InnerHtml = "";
                divHakKelasBPJS.Style.Add("display", "none");
            }

            if (entity.GCAdmissionCondition != Constant.AdmissionCondition.DEATH_ON_ARRIVAL)
            {
                if (entity.GCTriage != "")
                    divPatientBannerImgInfo.Style.Add("background-color", entity.TriageColor);
                else
                    divPatientBannerImgInfo.Style.Add("display", "none");
            }
            else
            {
                divPatientBannerImgInfo.Style.Add("background-color", "black");
            }

            if (entity.IsGeriatricPatient)
            {
                divIsGeriatricPatient.Style.Add("background-color", "Blue");
                divIsGeriatricPatient.InnerHtml = HttpUtility.HtmlEncode("G");
            }
            else
            {
                divIsGeriatricPatient.Style.Add("background-color", "white");
                divIsGeriatricPatient.InnerHtml = HttpUtility.HtmlEncode("");
            }

            imgNeedTranslator.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "need_translator.png");
            if (entity.Penterjemah == "1")
            {
                imgNeedTranslator.Style.Remove("display");
            }
            else
            {
                imgNeedTranslator.Style.Add("display", "none");
            }

            #region Patient Status
            if (entity.IsParamedicTeam != 0)
            {
                imgParamedicTeam.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coopcare.PNG");
            }
            else
            {
                imgParamedicTeam.Style.Add("display", "none");
            }

            if (entity.MRN != 0 && entity.MRN != null)
            {
                string filterARInvoice = string.Format("MRN = {0} AND TransactionCode = '{1}' AND TotalPaymentAmount < TotalClaimedAmount ORDER BY ARInvoiceNo", AppSession.RegisteredPatient.MRN, Constant.TransactionCode.AR_INVOICE_PATIENT);
                List<vARInvoiceHd> lstOutstandingARInvoice = BusinessLayer.GetvARInvoiceHdList(filterARInvoice);
                if (lstOutstandingARInvoice.Count() > 0)
                {
                    imgARInvoicePatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "ar_invoice_patient.PNG");
                }
                else
                {
                    imgARInvoicePatient.Style.Add("display", "none");
                }
            }

            if (AppSession.SA0198)
            {
                if (!String.IsNullOrEmpty(AppSession.RegisteredPatient.MRN.ToString()))
                {
                    List<vVisitPackageBalanceHdChargesInfo> lstBalance = BusinessLayer.GetvVisitPackageBalanceHdChargesInfoList(string.Format("MRN = {0} AND HealthcareServiceUnitID = {1} AND Quantity > 0", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.HealthcareServiceUnitID));
                    if (lstBalance.Count > 0)
                    {
                        imgPackageItem.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "archieved.png");
                    }
                    else
                    {
                        imgPackageItem.Style.Add("display", "none");
                    }
                }
            }
            else
            {
                imgPackageItem.Style.Add("display", "none");
            }

            if (entity.IsCoB != 0)
            {
                imgCOB.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "cob.PNG");
            }
            else
            {
                imgCOB.Style.Add("display", "none");
            }

            if (entity.IsMultipleVisit != 0)
            {
                imgIsMultipleVisit.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "multivisit.PNG");
            }
            else
            {
                imgIsMultipleVisit.Style.Add("display", "none");
            }

            if (entity.cfIsHasAllergy)
            {
                divPatientStatusAllergy.Style.Add("background-color", "red");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("A");
            }
            else
            {
                divPatientStatusAllergy.Style.Add("background-color", "white");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("");
            }

            if (entity.IsHasPharmacogenomicProfile)
            {
                imgHasPGxTest.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "PGxTest.png");
                imgHasPGxTest.Style.Add("class", "blink-alert");
                imgHasPGxTest.Attributes.Add("class", "blink-alert");
            }
            else
            {
                imgHasPGxTest.Style.Add("display", "none");
                imgHasPGxTest.Attributes.Remove("class");
            }

            if (entity.RegistrationID != 0 && entity.RegistrationID != null)
            {
                string filterFallRisk = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY AssessmentID DESC", entity.RegistrationID);
                FallRiskAssessment entityFallRisk = BusinessLayer.GetFallRiskAssessmentList(filterFallRisk).FirstOrDefault();
                if (entityFallRisk != null)
                {
                    if (entityFallRisk.IsFallRisk)
                    {
                        divPatientStatusFallRisk.Style.Add("background-color", "yellow");
                        divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("F");
                    }
                    else
                    {
                        divPatientStatusFallRisk.Style.Add("background-color", "white");
                        divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("");
                    }
                }
                else
                {
                    divPatientStatusFallRisk.Style.Add("background-color", "white");
                    divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("");
                }
            }

            if (entity.IsDNR)
            {
                divPatientStatusDNR.Style.Add("background-color", "purple");
                divPatientStatusDNR.InnerHtml = HttpUtility.HtmlEncode("D");
            }
            else
            {
                divPatientStatusDNR.Style.Add("background-color", "white");
                divPatientStatusDNR.InnerHtml = HttpUtility.HtmlEncode("");
            }

            if (entity.MRN != 0 && entity.MRN != null)
            {
                if (!string.IsNullOrEmpty(entity.ProlanisPRB))
                {
                    imgPRB.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "prb.png");

                    if (!string.IsNullOrEmpty(entity.ProlanisPRB))
                    {
                        imgPRB.Attributes.Add("title", String.Format("{0}", entity.ProlanisPRB));
                    }
                }
                else
                {
                    imgPRB.Style.Add("display", "none");
                }

                if (entity.IsHasInfectious)
                {
                    if (hdnIsInfectiousIconIsAllowDisplay.Value == "1")
                    {
                        imgIsHasInfectious.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "infectious.PNG");
                    }
                    else
                    {
                        imgIsHasInfectious.Style.Add("display", "none");
                    }
                }
                else
                {
                    imgIsHasInfectious.Style.Add("display", "none");
                }

                if (entity.IsVIP)
                {
                    imgIsVIP.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "VIP.PNG");
                    string vipGroupName = string.IsNullOrEmpty(entity.OtherVIPGroup) ? entity.VIPGroupName : entity.OtherVIPGroup;
                    imgIsVIP.Attributes.Add("title", String.Format("{0}", vipGroupName));
                }
                else
                {
                    imgIsVIP.Style.Add("display", "none");
                }

                if (entity.IsBlackList)
                {
                    imgIsBlacklist.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "blacklist.png");
                    imgIsBlacklist.Style.Add("class", "blink-alert");
                    imgIsBlacklist.Attributes.Add("class", "blink-alert");
                    string blacklistReasonName = string.IsNullOrEmpty(entity.OtherBlacklistReason) ? entity.BlackListReason : entity.OtherBlacklistReason;
                    imgIsBlacklist.Attributes.Add("title", string.Format("{0}", blacklistReasonName));
                }
                else
                {
                    imgIsBlacklist.Style.Add("display", "none");
                    imgIsBlacklist.Attributes.Remove("class");
                }

                if (entity.IsTerminalPatient)
                {
                    imgIsTerminalPatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "terminalpatient.PNG");
                }
                else
                {
                    imgIsTerminalPatient.Style.Add("display", "none");
                }

                if (entity.IsFastTrack)
                {
                    imgIsFastTrack.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "FastTrack.PNG");
                }
                else
                {
                    imgIsFastTrack.Style.Add("display", "none");
                }

                if (entity.RegistrationID != 0 && entity.RegistrationID != null)
                {
                    string filterPainAssessment = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY AssessmentID DESC", entity.RegistrationID);
                    PainAssessment entityPain = BusinessLayer.GetPainAssessmentList(filterPainAssessment).FirstOrDefault();
                    if (entityPain != null)
                    {
                        if (entityPain.IsPain)
                        {
                            imgIsPain.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "PainAssessment.png");
                        }
                        else
                        {
                            imgIsPain.Style.Add("display", "none");
                        }
                    }
                }

                if (entity.IsFallRisk)
                {
                    imgIsFallRisk.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "fall_risk.png");
                }
                else
                {
                    imgIsFallRisk.Style.Add("display", "none");
                }

                if (entity.IsHasCommunicationRestriction)
                {
                    imgIsHasCommunicationRestriction.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "CommunicationRestriction.png");
                    string CommunicationRestriction = string.IsNullOrEmpty(entity.CommunicationRestriction) ? entity.CommunicationRestriction : entity.CommunicationRestriction;
                    imgIsHasCommunicationRestriction.Attributes.Add("title", string.Format("{0}", CommunicationRestriction));
                }
                else
                {
                    imgIsHasCommunicationRestriction.Style.Add("display", "none");
                }

                if (entity.IsHasPhysicalLimitation)
                {
                    imgIsHasPhysicalLimitation.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "PhysicalLimitation.png");
                    string PhysicalLimitationType = string.IsNullOrEmpty(entity.PhysicalLimitationType) ? entity.PhysicalLimitationType : entity.PhysicalLimitationType;
                    imgIsHasPhysicalLimitation.Attributes.Add("title", string.Format("{0}", PhysicalLimitationType));
                }
                else
                {
                    imgIsHasPhysicalLimitation.Style.Add("display", "none");
                }

                if (entity.IsRAPUH)
                {
                    imgIsRAPUH.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "geriatricpatient.PNG");
                }
                else
                {
                    imgIsRAPUH.Style.Add("display", "none");
                }

                if (!entity.IsAlive)
                {
                    imgIsAlive.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "RIP.PNG");

                    if (entity.DateOfDeath.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                    {
                        string info = String.Format("died on {0}", entity.DateOfDeath.ToString("dd-MM-yyyy"));
                        imgIsAlive.Attributes.Add("title", info);
                    }
                }
                else
                {
                    imgIsAlive.Style.Add("display", "none");
                }
            }

            string filterCVIP = string.Format("RegistrationID = {0} AND IsDeleted = 0", entity.RegistrationID);
            List<vConsultVisitItemPackage1> listCVIP = BusinessLayer.GetvConsultVisitItemPackage1List(filterCVIP);

            if (listCVIP.Where(a => !a.IsPackageAllInOne).ToList().Count() > 0)
            {
                int cvipCount = 0;
                string infoCVIP = string.Format("PaketMCU = ");
                foreach (vConsultVisitItemPackage1 cvip in listCVIP)
                {
                    if (cvipCount != 0)
                    {
                        infoCVIP += ", ";
                    }

                    infoCVIP += string.Format("{0} ({1})", cvip.ItemName1, cvip.ItemCode);

                    cvipCount += 1;
                }
                imgRegHasPackageMCU.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "has_mcu_package.png");
                imgRegHasPackageMCU.Attributes.Add("title", infoCVIP);
            }
            else
            {
                imgRegHasPackageMCU.Style.Add("display", "none");
            }

            if (listCVIP.Where(a => a.IsPackageAllInOne).ToList().Count() > 0)
            {
                int cvipCount = 0;
                string infoCVIP = string.Format("PaketAIO = ");
                foreach (vConsultVisitItemPackage1 cvip in listCVIP)
                {
                    if (cvipCount != 0)
                    {
                        infoCVIP += ", ";
                    }

                    infoCVIP += string.Format("{0} ({1})", cvip.ItemName1, cvip.ItemCode);

                    cvipCount += 1;
                }
                imgRegHasPackageAIO.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "has_aio_package.png");
                imgRegHasPackageAIO.Attributes.Add("title", infoCVIP);
            }
            else
            {
                imgRegHasPackageAIO.Style.Add("display", "none");
            }

            string filterExpression = string.Format("RegistrationID = {0}", entity.RegistrationID);
            List<vRegistrationOutstandingInfo> lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression);
            int outstanding = (lstInfo.Sum(info => info.ServiceOrder + info.PrescriptionOrder + info.PrescriptionReturnOrder + info.LaboratoriumOrder + info.RadiologiOrder + info.OtherOrder) > 0) ? 1 : 0;
            
            if (outstanding == 1)
            {
                imgOutstandingOrder.Src = string.Format("{0}{1}", TOOLBAR_IMAGE_PATH, "outstanding_order.png");
            }
            else
            {
                imgOutstandingOrder.Style.Add("display", "none");
            }

            if (entity.IsPregnant)
            {
                imgIsPregnant.Src = string.Format("{0}{1}", TOOLBAR_IMAGE_PATH, "obsgyn_history.png");
            }

            #endregion

            #region Vital Sign Banner
            InitializePatientVitalSignBanner(entity.LastGlucoseLevel);
            #endregion

            lblNurseParamedicName.InnerText = entity.NurseParamedicName;
        }

        private void SetAllergyInfo(vConsultVisit2 entity)
        {
            string allergyInfo = entity.cfPatientAllergyInfo;
            bool isHasAllergy = !string.IsNullOrEmpty(allergyInfo);
            if (entity.cfIsHasAllergy)
            {
                lblAllergy.Style.Add("color", "red");
                lblAllergy.Style.Add("font-weight", "bold");
                lblAllergy.Attributes.Add("class", "blink-alert");
                if (allergyInfo.Length > 20)
                {
                    lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.cfPatientAllergyInfo.Substring(0, 20).ToUpper()) + "...";
                }
                else
                {
                    lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.cfPatientAllergyInfo.ToUpper());
                }
                lblAllergy.Attributes.Add("title", entity.cfPatientAllergyInfo);

                divPatientStatusAllergy.Style.Add("background-color", "red");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("A");
            }
            else
            {
                lblAllergy.Style.Add("color", "black");
                lblAllergy.Style.Add("font-weight", "normal");
                lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.cfPatientAllergyInfo.ToUpper());
                lblAllergy.Attributes.Remove("class");

                divPatientStatusAllergy.Style.Add("background-color", "white");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("");

            }

            AppSession.IsHasAllergy = entity.cfIsHasAllergy;
            AppSession.PatientAllergyInfo = entity.cfPatientAllergyInfo.ToUpper();
        }

        public void InitializeEmptyInpatientPatientBanner()
        {
            imgPatientProfilePicture.Src = Function.GeneratePatientPictureFileName(String.Empty, String.Empty);
            hdnPatientGender.Value = String.Empty;

            lblRegistrationNo.InnerHtml = String.Empty;
            lblPatientName.InnerHtml = String.Empty;

            lblFromRegistrationNo.InnerHtml = String.Empty;

            lblMRN.InnerHtml = String.Empty;
            words = ((BasePage)Page).GetWords();


            lblDOB.InnerHtml = "";
            lblPatientAge.InnerHtml = "";


            lblDateHour.InnerHtml = String.Empty;
            lblPayer.InnerHtml = String.Empty;
            lblGender.InnerHtml = String.Empty;


            lblMRN.InnerHtml = String.Empty;
            spnOldMedicalNo.Style.Add("display", "none");

            lblPhysicianName.InnerHtml = String.Empty;
            lblParamedicLicenseNo.InnerHtml = String.Empty;
            lblParamedicType.InnerHtml = String.Empty;
            lblAllergy.InnerHtml = String.Empty;
            lblPatientCategory.InnerHtml = String.Empty;


            divOutpatientInfoLine1.Style.Add("display", "none");
            divOutpatientInfoLine2.Style.Add("display", "none");

            lblInpatientClass.InnerHtml = String.Empty;
            lblInpatientControlClass.InnerHtml = String.Empty;
            lblInpatientWard.InnerHtml = String.Empty;

            lblInpatientLOS.InnerHtml = String.Empty;

            divPatientBannerImgInfo.Style.Add("display", "none");
        }

        public void RefreshAllergyStatus(vConsultVisit2 entity)
        {
            SetAllergyInfo(entity);
        }
    }
}