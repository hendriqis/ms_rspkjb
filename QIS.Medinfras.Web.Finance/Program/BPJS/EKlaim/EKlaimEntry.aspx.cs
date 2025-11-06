using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class EKlaimEntry : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Status/";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_EKLAIM_ENTRY;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetDefaultDiagnosisType()
        {
            return Constant.DiagnoseType.MAIN_DIAGNOSIS;
        }

        protected string GetDefaultDifferentialDiagnosisStatus()
        {
            return Constant.DifferentialDiagnosisStatus.CONFIRMED;
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;
            hdnMenuID.Value = oMenu.MenuID.ToString();
            hdnRegistrationID.Value = Request.QueryString["id"];

            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "HealthcareID = '{0}' AND Parametercode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                    AppSession.UserLogin.HealthcareID, //0
                    Constant.SettingParameter.VITAL_SIGN_NBPs, //1
                    Constant.SettingParameter.VITAL_SIGN_NBPd, //2
                    Constant.SettingParameter.RM_EKLAIM_SEND_EKLAIM_MEDICALNO, //3
                    Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM, //4
                    Constant.SettingParameter.RM_SISTOLE_AND_DIASTOLE_FROM_LINKED_REGISTRATION_IN_EKLAIM_PROCESS, //5
                    Constant.SettingParameter.FN_IS_EKLAIM_NONINPATIENT_DISCHARGEDATE_FROM_SEPDATE, //6
                    Constant.SettingParameter.FN_IS_EKLAIM_NONINPATIENT_REGISTRATIONDATE_FROM_SEPDATE, //7
                    Constant.SettingParameter.FN_IS_EKLAIM_DOKTER_USING_DPJP_KONSULEN_VCLAIM, //8
                    Constant.SettingParameter.RM_PROSES_EKLAIM_MENAMPILKAN_DIAGNOSA_MASUK //9
            ));
            hdnSystole.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.VITAL_SIGN_NBPs).FirstOrDefault().ParameterValue;
            hdnDiastole.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.VITAL_SIGN_NBPd).FirstOrDefault().ParameterValue;
            hdnIsSendEKlaimMedicalNo.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.RM_EKLAIM_SEND_EKLAIM_MEDICALNO).FirstOrDefault().ParameterValue;
            hdnIsBridgingToEKlaim.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM).FirstOrDefault().ParameterValue;
            hdnIsSistoleAndDiastoleFromLinkedRegistration.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.RM_SISTOLE_AND_DIASTOLE_FROM_LINKED_REGISTRATION_IN_EKLAIM_PROCESS).FirstOrDefault().ParameterValue;
            hdnIsNonInpatientDischargeDateFromSEPDate.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.FN_IS_EKLAIM_NONINPATIENT_DISCHARGEDATE_FROM_SEPDATE).FirstOrDefault().ParameterValue;
            hdnIsNonInpatientRegistrationDateFromSEPDate.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.FN_IS_EKLAIM_NONINPATIENT_REGISTRATIONDATE_FROM_SEPDATE).FirstOrDefault().ParameterValue;
            hdnIsDokterUsingDokterKonsulenVClaim.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.FN_IS_EKLAIM_DOKTER_USING_DPJP_KONSULEN_VCLAIM).FirstOrDefault().ParameterValue;
            hdnMenampilkanDiagnosaMasuk.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.RM_PROSES_EKLAIM_MENAMPILKAN_DIAGNOSA_MASUK).FirstOrDefault().ParameterValue;

            string filterRegBPJS = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            vRegistrationBPJS1 entity = BusinessLayer.GetvRegistrationBPJS1List(filterRegBPJS).FirstOrDefault();
            hdnRegistrationNo.Value = entity.RegistrationNo;
            hdnMRN.Value = entity.MRN.ToString();
            EntityToControl(entity);

            if (entity.IsMultipleVisit != 0)
            {
                imgIsMultipleVisit.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "multivisit.PNG");
                imgIsMultipleVisit.Attributes.Add("class", "lnkIsMultipleVisit imgLink");
            }
            else
            {
                imgIsMultipleVisit.Style.Add("display", "none");
            }

            BindGridView();
            BindDiagnosaGridView();
            DiagnosaSetEntityToControl(entity);

            ProsedureSetEntityControl(entity);
            BindProcedureGridView();
        }

        private void DiagnosaSetEntityToControl(vRegistrationBPJS1 entity)
        {
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

            txtClaimDiagnosisDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtClaimDiagnosisTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnDefaultParamedicID.Value = entity.ParamedicID.ToString();
            ParamedicMaster pmEntity = BusinessLayer.GetParamedicMaster(entity.ParamedicID);
            hdnDefaultParamedicCode.Value = pmEntity.ParamedicCode;
            hdnDefaultParamedicName.Value = pmEntity.FullName;

            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.DIAGNOSIS_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboDiagnoseTypeClaim, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void ProsedureSetEntityControl(vRegistrationBPJS1 entity)
        {
            hdnProsedureHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnProsedureHealthcareServiceUnitID.Value));
            if (count > 0)
                hdnProsedureIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnProsedureIsHealthcareServiceUnitHasParamedic.Value = "0";

            txtClaimProcedureDate.Text = entity.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtClaimProcedureTime.Text = entity.VisitTime;

            hdnProsedureDefaultParamedicID.Value = entity.ParamedicID.ToString();
            ParamedicMaster pmEntity = BusinessLayer.GetParamedicMaster(entity.ParamedicID);
            hdnProsedureDefaultParamedicCode.Value = pmEntity.ParamedicCode;
            hdnDefaultParamedicName.Value = pmEntity.FullName;
        }

        private void EntityToControl(vRegistrationBPJS1 entity)
        {
            UserAttribute ua = BusinessLayer.GetUserAttributeList(string.Format("UserID = '{0}'", AppSession.UserLogin.UserID)).FirstOrDefault();
            if (ua != null)
            {
                hdnUsernameLogin.Value = ua.SSN;
            }

            if (entity.IsTransferredToInpatient == true)
            {
                imgIsTransferredToInpatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "transfer_to_inpatient.png");
                imgIsTransferredToInpatient.Attributes.Add("title", string.Format("Pengantar Rawat Inap"));
                imgIsTransferredToInpatient.Attributes.Add("class", string.Format("blink-icon"));
            }

            Healthcare ovHealthcare = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            hdnHealthcareInitial.Value = ovHealthcare.Initial;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                        "IsDeleted = 0 AND IsActive = 1 AND ParentID IN ('{0}', '{1}', '{2}', '{3}', '{4}')",
                                                       Constant.StandardCode.KDTARIF_INACBG, //0
                                                       Constant.StandardCode.CARA_KELUAR_EKLAIM, //1
                                                       Constant.StandardCode.CARA_MASUK_EKLAIM, //2
                                                       Constant.StandardCode.CARA_BAYAR_EKLAIM, //3
                                                       Constant.StandardCode.COB_EKLAIM //4
            ));

            hdnVisitID.Value = entity.VisitID.ToString();
            hdnParamedicID.Value = entity.ParamedicID.ToString();

            #region Header Left

            List<RegistrationPayer> lstRp = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY ID ASC", entity.RegistrationID));
            if (lstRp.Count > 0)
            {
                RegistrationPayer rp = lstRp.Where(p => p.IsPrimaryPayer == true).FirstOrDefault();
                RegistrationPayer rpNoPrimary = lstRp.Where(p => p.IsPrimaryPayer == false).FirstOrDefault();
                int bpNoPrimary = 0;
                int bpPrimary = 0;
                if (rp != null)
                {
                    bpPrimary = rp.BusinessPartnerID;
                }

                if (rpNoPrimary != null)
                {
                    bpNoPrimary = rpNoPrimary.BusinessPartnerID;
                }

                List<Customer> lstCustomer = BusinessLayer.GetCustomerList(string.Format("BusinessPartnerID IN('{0}','{1}')", bpPrimary, bpNoPrimary));
                if (lstCustomer.Count > 0)
                {
                    Customer oCsPrimary = lstCustomer.Where(p => p.EKlaimPayorID > 0 && p.EKlaimPayorID != null).FirstOrDefault();
                    if (oCsPrimary != null)
                    {
                        hdnpayor_id.Value = oCsPrimary.EKlaimPayorID.ToString();
                        hdnpayor_code.Value = oCsPrimary.EKlaimPayorCode.ToString();
                    }

                    Customer oCsCob = lstCustomer.Where(p => p.EKlaimCOBCode != null && p.EKlaimCOBCode != "").FirstOrDefault();
                    if (oCsCob != null)
                    {
                        hdncob_cd.Value = oCsCob.EKlaimCOBCode.ToString();
                    }
                }
            }

            List<StandardCode> lstCaraBayar = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.CARA_BAYAR_EKLAIM).ToList();
            Methods.SetComboBoxField<StandardCode>(cboCaraBayarEKlaim, lstCaraBayar, "StandardCodeName", "StandardCodeID");
            if (!string.IsNullOrEmpty(entity.EKlaimPayorCode))
            {
                if (lstCaraBayar.Where(a => a.TagProperty.Contains(entity.EKlaimPayorCode)) != null)
                {
                    cboCaraBayarEKlaim.Value = lstCaraBayar.Where(a => a.TagProperty.Contains(entity.EKlaimPayorCode)).FirstOrDefault().StandardCodeID;
                }
            }
            else if (!string.IsNullOrEmpty(hdnpayor_code.Value))
            {
                if (lstCaraBayar.Where(a => a.TagProperty.Contains(hdnpayor_code.Value)) != null)
                {
                    cboCaraBayarEKlaim.Value = lstCaraBayar.Where(a => a.TagProperty.Contains(hdnpayor_code.Value)).FirstOrDefault().StandardCodeID;
                }
            }

            if (cboCaraBayarEKlaim.Value != null)
            {
                if (cboCaraBayarEKlaim.Value.ToString() == Constant.EKlaimJaminanCaraBayar.JKN)
                {
                    trCOB.Attributes.Remove("style");
                }
                else
                {
                    trCOB.Attributes.Add("style", "display:none");
                }
            }

            List<StandardCode> lstCOB = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.COB_EKLAIM).ToList();
            Methods.SetComboBoxField<StandardCode>(cboCOBEKlaim, lstCOB, "StandardCodeName", "StandardCodeID");
            if (!string.IsNullOrEmpty(entity.EKlaimCOBCode))
            {
                if (lstCOB.Where(a => a.TagProperty.Contains(entity.EKlaimCOBCode)) != null)
                {
                    cboCOBEKlaim.Value = lstCOB.Where(a => a.TagProperty.Contains(entity.EKlaimCOBCode)).FirstOrDefault().StandardCodeID;
                }
            }
            else if (!string.IsNullOrEmpty(hdncob_cd.Value))
            {
                if (lstCOB.Where(a => a.TagProperty.Contains(hdncob_cd.Value)) != null)
                {
                    cboCaraBayarEKlaim.Value = lstCOB.Where(a => a.TagProperty.Contains(hdncob_cd.Value)).FirstOrDefault().StandardCodeID;
                }
            }

            hdnTanggalSEP.Value = entity.TanggalSEP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnJamSEP.Value = entity.JamSEP;
            txtTanggalSEP.Text = string.Format("{0} {1}", entity.cfTanggalSEPInString, entity.JamSEP);

            if (entity.DepartmentID == Constant.Facility.INPATIENT)
            {
                txtServiceUnitType.Text = "Rawat Inap";

                trExecutiveClassFlag.Attributes.Add("style", "display:none");
                trExecutiveClassAmount.Attributes.Add("style", "display:none");

                if (entity.EKlaimUpgradeClassIndex == 1)
                {
                    trUpgradeClassFlag.Attributes.Remove("style");
                    trUpgradeClass.Attributes.Remove("style");
                    trUpgradeClassLOS.Attributes.Remove("style");
                    trUpgradeClassPayor.Attributes.Remove("style");
                    trAddPaymentPct.Attributes.Add("style", "display:none");

                    chkIsUpgradeClass.Checked = true;
                    rblUpgradeClass.SelectedValue = entity.EKlaimUpgradeClassCode;
                    txtUpgradeClassLOS.Text = entity.EKlaimUpgradeClassLOS.ToString();
                    rblUpgradeClassPayor.SelectedValue = entity.EKlaimUpgradeClassPayor;
                    txtAddPaymentPct.Text = entity.EKlaimAddPaymentPct.ToString();
                }
                else
                {
                    trUpgradeClassFlag.Attributes.Remove("style");
                    trUpgradeClass.Attributes.Add("style", "display:none");
                    trUpgradeClassLOS.Attributes.Add("style", "display:none");
                    trUpgradeClassPayor.Attributes.Add("style", "display:none");
                    trAddPaymentPct.Attributes.Add("style", "display:none");

                    chkIsUpgradeClass.Checked = false;
                    rblUpgradeClass.SelectedValue = null;
                    txtUpgradeClassLOS.Text = "0";
                    rblUpgradeClassPayor.SelectedValue = null;
                    txtAddPaymentPct.Text = "0";
                }
            }
            else
            {
                txtServiceUnitType.Text = "Rawat Jalan";

                trUpgradeClassFlag.Attributes.Add("style", "display:none");
                trUpgradeClass.Attributes.Add("style", "display:none");
                trUpgradeClassLOS.Attributes.Add("style", "display:none");
                trUpgradeClassPayor.Attributes.Add("style", "display:none");
                trAddPaymentPct.Attributes.Add("style", "display:none");

                if (entity.EKlaimTariffPoliEksekutif != 0)
                {
                    trExecutiveClassFlag.Attributes.Remove("style");
                    trExecutiveClassAmount.Attributes.Remove("style");

                    chkIsExecutiveClass.Checked = true;
                    txtExecutiveClassAmount.Text = entity.EKlaimTariffPoliEksekutif.ToString();
                }
                else
                {
                    trExecutiveClassFlag.Attributes.Remove("style");
                    trExecutiveClassAmount.Attributes.Add("style", "display:none");

                    chkIsExecutiveClass.Checked = false;
                    txtExecutiveClassAmount.Text = "0";
                }
            }

            hdnDepartmentID.Value = entity.DepartmentID;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            txtParamedicName.Text = entity.ParamedicName;
            txtNamaDPJPKonsulan.Text = entity.NamaDPJPKonsulan;

            txtNoSEP.Text = entity.NoSEP;
            txtNoPeserta.Text = entity.NoPeserta;
            txtMedicalNo.Text = entity.MedicalNo;
            txtEKlaimMedicalNo.Text = entity.EKlaimMedicalNo;
            txtPatientName.Text = entity.PatientName;
            hdnDOB.Value = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDOB.Text = entity.cfDOB;
            hdnGender.Value = entity.GCGender;
            txtGender.Text = entity.Gender;

            string filterBirthRecord = string.Format("MRN = {0} AND IsDeleted=0", entity.MRN);
            PatientBirthRecord birthRecord = BusinessLayer.GetPatientBirthRecordList(filterBirthRecord).FirstOrDefault();
            if (birthRecord != null)
            {
                decimal Weight = 0;
                if (!string.IsNullOrEmpty(birthRecord.Weight.ToString()))
                {
                    Weight = birthRecord.Weight * 1000;
                    hdnBirthWeight.Value = Weight.ToString();
                    txtBirthWeight.Text = Weight.ToString();
                }
                else
                {
                    hdnBirthWeight.Value = birthRecord.Weight.ToString();
                    txtBirthWeight.Text = birthRecord.Weight.ToString();
                }
            }

            string sitbPatient = "";
            if (entity.EKlaimNoRegisterSITB != null && entity.EKlaimNoRegisterSITB != "")
            {
                sitbPatient = entity.EKlaimNoRegisterSITB;
            }
            else if (entity.SITBRegisterNo != null && entity.SITBRegisterNo != "")
            {
                sitbPatient = entity.SITBRegisterNo;
            }

            txtNoPasienTB.Text = sitbPatient;
            if (txtNoPasienTB.Text != null && txtNoPasienTB.Text != "")
            {
                chkPasienTB.Checked = true;
                tdtb1.Attributes.Remove("style");
                tdtb2.Attributes.Remove("style");
            }
            else
            {
                chkPasienTB.Checked = false;
                tdtb1.Attributes.Add("style", "display:none");
                tdtb2.Attributes.Add("style", "display:none");
            }

            #endregion

            #region Header Right

            txtRegistrationNo.Text = entity.RegistrationNo;

            hdnRegistrationDate.Value = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnRegistrationTime.Value = entity.RegistrationTime + ":00";
            txtRegistrationDate.Text = string.Format("{0} {1}", entity.cfRegistrationDateInString, entity.RegistrationTime);
            txtRegistrationStatus.Text = entity.RegistrationStatus;

            if (entity.DepartmentID != Constant.Facility.INPATIENT)
            {
                hdnDischargeDate.Value = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnDischargeTime.Value = entity.ActualVisitTime;
                txtDischargeDate.Text = string.Format("{0} {1}", entity.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT), entity.ActualVisitTime);
            }
            else
            {
                if (entity.TanggalPulang.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    hdnDischargeDate.Value = entity.TanggalPulang.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnDischargeTime.Value = entity.JamPulang;
                    txtDischargeDate.Text = string.Format("{0} {1}", entity.cfTanggalPulangInString, entity.JamPulang);
                }
                else if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    hdnDischargeDate.Value = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnDischargeTime.Value = entity.DischargeTime;
                    txtDischargeDate.Text = string.Format("{0} {1}", entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), entity.DischargeTime);
                }
                else
                {
                    if (entity.DepartmentID == Constant.Facility.INPATIENT)
                    {
                        txtDischargeDateSementara.Text = string.Format("{0} {1}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
                        hdnDischargeDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        hdnDischargeTime.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }

            //jika tanggal pulang belum ada kusus untuk rawat inap
            if (txtDischargeDate.Text == null || txtDischargeDate.Text == "")
            {
                if (entity.DepartmentID == Constant.Facility.INPATIENT)
                {
                    txtDischargeDateSementara.Text = string.Format("{0} {1}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
                    hdnDischargeDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnDischargeTime.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                }
            }

            hdnLOSinDay.Value = entity.LOSInDay.ToString();
            txtLOSinDay.Text = entity.LOSInDay.ToString("G29");
            txtLOSinHour.Text = entity.LOSInHour.ToString("G29");
            txtLOSinMinute.Text = entity.LOSInMinute.ToString("G29");

            if (entity.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (entity.EKlaimICUIndicator == 0)
                {
                    List<GetBedHistoryRegistrationEklaim> lstBedHistory = BusinessLayer.GetBedHistoryRegistrationEklaim(Convert.ToInt32(hdnRegistrationID.Value));
                    if (lstBedHistory.Count > 0)
                    {
                        double totalDay = 0;
                        foreach (GetBedHistoryRegistrationEklaim rowBed in lstBedHistory)
                        {
                            totalDay += rowBed.cfTotalTime;
                        }
                        hdnICULOS.Value = Math.Round(totalDay, 0).ToString();
                        txtICULOS.Text = Math.Round(totalDay, 0).ToString();

                        if (!string.IsNullOrEmpty(hdnICULOS.Value))
                        {
                            chkIsRawatIntensif.Checked = true;

                            trIsRawatIntensif.Attributes.Remove("style");
                            trValueRawatIntensif.Attributes.Remove("style");
                            trValueVentilatorRow1.Attributes.Remove("style");
                            trValueVentilatorRow2.Attributes.Remove("style");
                            trValueVentilatorRow3.Attributes.Remove("style");
                        }
                    }
                }
                else
                {
                    chkIsRawatIntensif.Checked = entity.EKlaimICUIndicator == 1 ? true : false;
                    hdnICULOS.Value = entity.EKlaimICULOS.ToString();
                    txtICULOS.Text = entity.EKlaimICULOS.ToString();

                    txtLamaJamVentilator.Text = entity.EKlaimVentilatorHour.ToString();
                    txtVentilatorStartDate.Text = entity.EKlaimVentilatorStartDatetime.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtVentilatorStartTime1.Text = entity.EKlaimVentilatorStartDatetime.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
                    txtVentilatorStartTime2.Text = entity.EKlaimVentilatorStartDatetime.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
                    txtVentilatorEndDate.Text = entity.EKlaimVentilatorStopDatetime.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtVentilatorEndTime1.Text = entity.EKlaimVentilatorStopDatetime.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
                    txtVentilatorEndTime2.Text = entity.EKlaimVentilatorStopDatetime.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);

                    if (chkIsRawatIntensif.Checked)
                    {
                        trIsRawatIntensif.Attributes.Remove("style");
                        trValueRawatIntensif.Attributes.Remove("style");
                        trValueVentilatorRow1.Attributes.Remove("style");
                        trValueVentilatorRow2.Attributes.Remove("style");
                        trValueVentilatorRow3.Attributes.Remove("style");
                    }
                    else
                    {
                        trIsRawatIntensif.Attributes.Remove("style");
                        trValueRawatIntensif.Attributes.Remove("style");
                        trValueVentilatorRow1.Attributes.Remove("style");
                        trValueVentilatorRow2.Attributes.Remove("style");
                        trValueVentilatorRow3.Attributes.Remove("style");
                    }
                }
            }
            else
            {
                trIsRawatIntensif.Attributes.Add("style", "display:none");
                trValueRawatIntensif.Attributes.Add("style", "display:none");
                trValueVentilatorRow1.Attributes.Add("style", "display:none");
                trValueVentilatorRow2.Attributes.Add("style", "display:none");
                trValueVentilatorRow3.Attributes.Add("style", "display:none");

                chkIsRawatIntensif.Checked = false;
                hdnICULOS.Value = null;
                txtICULOS.Text = "0";
            }

            txtKelasSEP.Text = entity.KelasSEP.ToString();
            txtKelasTanggungan.Text = entity.KelasTanggungan;

            txtNoRujukan.Text = entity.NoRujukan;
            txtTanggalRujukan.Text = entity.cfTanggalRujukanInString;

            List<StandardCode> lstCaraMasuk = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.CARA_MASUK_EKLAIM).ToList();
            Methods.SetComboBoxField<StandardCode>(cboEKlaimCaraMasuk, lstCaraMasuk, "StandardCodeName", "StandardCodeID");
            if (!string.IsNullOrEmpty(entity.GCEKlaimCaraMasuk))
            {
                cboEKlaimCaraMasuk.Value = entity.GCEKlaimCaraMasuk;
            }

            List<StandardCode> lstCaraPulang = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.CARA_KELUAR_EKLAIM).ToList();
            Methods.SetComboBoxField<StandardCode>(cboEKlaimCaraPulang, lstCaraPulang, "StandardCodeName", "StandardCodeID");
            if (!string.IsNullOrEmpty(entity.GCEKlaimDischargeStatus))
            {
                cboEKlaimCaraPulang.Value = entity.GCEKlaimDischargeStatus;
            }

            if (ovHealthcare.GCEKlaimTariffCategory != null)
            {
                string[] lstEKlaimTariffCat = ovHealthcare.GCEKlaimTariffCategory.Split('|');
                List<StandardCode> hcTarifList = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.KDTARIF_INACBG && (p.StandardCodeID == lstEKlaimTariffCat[0] || p.StandardCodeID == lstEKlaimTariffCat[1])).ToList();
                Methods.SetComboBoxField<StandardCode>(cboKdTarifINACBG, hcTarifList, "StandardCodeName", "StandardCodeID");
                cboKdTarifINACBG.Value = lstEKlaimTariffCat[0];
            }

            #endregion

            #region Diagnose & Procedure
            string filterPDiagnosis = string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", hdnVisitID.Value, Constant.DiagnoseType.MAIN_DIAGNOSIS);
            List<vPatientDiagnosisEklaim> lstPDiagnosis = BusinessLayer.GetvPatientDiagnosisEklaimList(filterPDiagnosis);
            if (lstPDiagnosis.Count() > 0)
            {
                vPatientDiagnosisEklaim pdiagnosis = lstPDiagnosis.FirstOrDefault();
                txtDiagnoseCode.Text = pdiagnosis.DiagnoseID;
                txtDiagnoseName.Text = pdiagnosis.DiagnoseName != "" && pdiagnosis.DiagnoseName != null ? pdiagnosis.DiagnoseName : pdiagnosis.DiagnosisText;
                if (pdiagnosis.DifferentialDateTimeInString != "")
                {
                    txtDiagnoseInfo.Text = string.Format("{0} | {1}", pdiagnosis.DifferentialDateTimeInString, pdiagnosis.DifferentialDiagnosisUserName);
                }
                txtFinalDiagnoseCode.Text = pdiagnosis.FinalDiagnosisID;
                txtFinalDiagnoseName.Text = pdiagnosis.FinalDiagnosisName != "" && pdiagnosis.FinalDiagnosisName != null ? pdiagnosis.FinalDiagnosisName : pdiagnosis.FinalDiagnosisText;
                if (pdiagnosis.cfFinalDateTimeInString != "")
                {
                    txtFinalDiagnoseInfo.Text = string.Format("{0} | {1}", pdiagnosis.cfFinalDateTimeInString, pdiagnosis.FinalDiagnosisUserName);
                }
                txtClaimDiagnoseCode.Text = pdiagnosis.ClaimDiagnosisID;
                txtClaimDiagnoseName.Text = pdiagnosis.ClaimDiagnosisName != "" && pdiagnosis.ClaimDiagnosisName != null ? pdiagnosis.ClaimDiagnosisName : pdiagnosis.ClaimDiagnosisText;
                if (pdiagnosis.cfClaimDiagnosisDateTimeInString != "")
                {
                    txtClaimDiagnoseInfo.Text = string.Format("{0} | {1}", pdiagnosis.cfClaimDiagnosisDateTimeInString, pdiagnosis.ClaimDiagnosisByUserName);
                }
            }

            string filterPProcedure = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
            List<vPatientProcedure> lstPProcedure = BusinessLayer.GetvPatientProcedureList(filterPProcedure);
            if (lstPProcedure.Count() > 0)
            {
                vPatientProcedure pProcedure = lstPProcedure.FirstOrDefault();
                //txtProcedureCode.Text = pProcedure.ProcedureID;
                //txtProcedureName.Text = pProcedure.ProcedureName != "" && pProcedure.ProcedureName != null ? pProcedure.ProcedureName : pProcedure.ProcedureText;
                //txtProcedureInfo.Text = string.Format("{0}", pProcedure.ProcedureDateTimeInString);
                //txtFinalProcedureCode.Text = pProcedure.FinalProcedureID;
                //txtFinalProcedureName.Text = pProcedure.FinalProcedureName != "" && pProcedure.FinalProcedureName != null ? pProcedure.ProcedureName : pProcedure.FinalProcedureText;
                //if (pProcedure.cfFinalProcedureDateTimeInString != "")
                //{
                //    txtFinalProcedureInfo.Text = string.Format("{0} | {1}", pProcedure.cfFinalProcedureDateTimeInString, pProcedure.FinalProcedureByName);
                //}
                //txtClaimProcedureCode.Text = pProcedure.ClaimProcedureID;
                //txtClaimProcedureName.Text = pProcedure.ClaimProcedureName != "" && pProcedure.ClaimProcedureName != null ? pProcedure.ClaimProcedureName : pProcedure.ClaimProcedureText;
                //if (pProcedure.cfClaimProcedureDateTimeInString != "")
                //{
                //    txtClaimProcedureInfo.Text = string.Format("{0} | {1}", pProcedure.cfClaimProcedureDateTimeInString, pProcedure.ClaimProcedureByName);
                //}
            }

            #endregion

            #region Jenazah

            rblPemulasaraanJenazah.SelectedValue = entity.EKlaimPemulasaraanJenazah ? "1" : "0";
            rblKantongJenazah.SelectedValue = entity.EKlaimKantongJenazah ? "1" : "0";
            rblPetiJenazah.SelectedValue = entity.EKlaimPetiJenazah ? "1" : "0";
            rblPlastikErat.SelectedValue = entity.EKlaimPlastikErat ? "1" : "0";
            rblDesinfektanJenazah.SelectedValue = entity.EKlaimDesinfektanJenazah ? "1" : "0";
            rblMobilJenazah.SelectedValue = entity.EKlaimMobilJenazah ? "1" : "0";
            rblDesinfektanMobilJenazah.SelectedValue = entity.EKlaimDesinfektanMobilJenazah ? "1" : "0";

            #endregion

            #region Sistole & Diastole
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();

            List<vVitalSignDt> lstTTV = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = '{0}' AND VitalSignID IN ('{1}','{2}') AND IsDeleted = 0 ORDER BY ID ASC", hdnVisitID.Value, hdnSystole.Value, hdnDiastole.Value));
            if (hdnIsSistoleAndDiastoleFromLinkedRegistration.Value == "1")
            {
                if (entityRegistration.LinkedRegistrationID == 0 || entityRegistration.LinkedRegistrationID == null)
                {
                    if (lstTTV.Count > 0)
                    {
                        vVitalSignDt Nbps = lstTTV.Where(p => p.VitalSignID == Convert.ToInt32(hdnSystole.Value)).FirstOrDefault();
                        if (Nbps != null)
                        {
                            txtSistole.Text = Nbps.VitalSignValue;
                        }

                        vVitalSignDt NBPd = lstTTV.Where(p => p.VitalSignID == Convert.ToInt32(hdnDiastole.Value)).FirstOrDefault();
                        if (NBPd != null)
                        {
                            txtDiastole.Text = NBPd.VitalSignValue;
                        }
                    }
                }
                else
                {
                    ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                    List<vVitalSignDt> lstTTVLinked = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = '{0}' AND VitalSignID IN ('{1}','{2}') AND IsDeleted = 0 ORDER BY ID ASC", entityLinkedentityVisit.VisitID, hdnSystole.Value, hdnDiastole.Value));

                    if (lstTTVLinked.Count > 0)
                    {
                        vVitalSignDt Nbps = lstTTVLinked.Where(p => p.VitalSignID == Convert.ToInt32(hdnSystole.Value)).FirstOrDefault();
                        if (Nbps != null)
                        {
                            txtSistole.Text = Nbps.VitalSignValue;
                        }

                        vVitalSignDt NBPd = lstTTVLinked.Where(p => p.VitalSignID == Convert.ToInt32(hdnDiastole.Value)).FirstOrDefault();
                        if (NBPd != null)
                        {
                            txtDiastole.Text = NBPd.VitalSignValue;
                        }
                    }
                    else
                    {
                        if (lstTTV.Count > 0)
                        {
                            vVitalSignDt Nbps = lstTTV.Where(p => p.VitalSignID == Convert.ToInt32(hdnSystole.Value)).FirstOrDefault();
                            if (Nbps != null)
                            {
                                txtSistole.Text = Nbps.VitalSignValue;
                            }

                            vVitalSignDt NBPd = lstTTV.Where(p => p.VitalSignID == Convert.ToInt32(hdnDiastole.Value)).FirstOrDefault();
                            if (NBPd != null)
                            {
                                txtDiastole.Text = NBPd.VitalSignValue;
                            }
                        }
                    }
                }

            }
            else
            {
                if (lstTTV.Count > 0)
                {
                    vVitalSignDt Nbps = lstTTV.Where(p => p.VitalSignID == Convert.ToInt32(hdnSystole.Value)).FirstOrDefault();
                    if (Nbps != null)
                    {
                        txtSistole.Text = Nbps.VitalSignValue;
                    }

                    vVitalSignDt NBPd = lstTTV.Where(p => p.VitalSignID == Convert.ToInt32(hdnDiastole.Value)).FirstOrDefault();
                    if (NBPd != null)
                    {
                        txtDiastole.Text = NBPd.VitalSignValue;
                    }
                }
            }
            #endregion

            #region APGAR Score

            txtEKlaimAPGARMenit1Appearance.Text = entity.EKlaimAPGARMenit1Appearance.ToString();
            txtEKlaimAPGARMenit1Pulse.Text = entity.EKlaimAPGARMenit1Pulse.ToString();
            txtEKlaimAPGARMenit1Grimace.Text = entity.EKlaimAPGARMenit1Grimace.ToString();
            txtEKlaimAPGARMenit1Activity.Text = entity.EKlaimAPGARMenit1Activity.ToString();
            txtEKlaimAPGARMenit1Respiration.Text = entity.EKlaimAPGARMenit1Respiration.ToString();

            txtEKlaimAPGARMenit5Appearance.Text = entity.EKlaimAPGARMenit5Appearance.ToString();
            txtEKlaimAPGARMenit5Pulse.Text = entity.EKlaimAPGARMenit5Pulse.ToString();
            txtEKlaimAPGARMenit5Grimace.Text = entity.EKlaimAPGARMenit5Grimace.ToString();
            txtEKlaimAPGARMenit5Activity.Text = entity.EKlaimAPGARMenit5Activity.ToString();
            txtEKlaimAPGARMenit5Respiration.Text = entity.EKlaimAPGARMenit5Respiration.ToString();

            #endregion

            #region Persalinan & Delivery

            txtEKlaimPersalinanUsiaKehamilan.Text = entity.EKlaimPersalinanUsiaKehamilan.ToString();
            txtEKlaimPersalinanGravida.Text = entity.EKlaimPersalinanGravida.ToString();
            txtEKlaimPersalinanPartus.Text = entity.EKlaimPersalinanPartus.ToString();
            txtEKlaimPersalinanAbortus.Text = entity.EKlaimPersalinanAbortus.ToString();
            rblOnSetKontraksi.SelectedValue = entity.EKlaimPersalinanOnsetKontraksi;

            txtEKlaimDeliverySequence.Text = entity.EKlaimDeliverySequence.ToString();
            rblDeliveryMethod.SelectedValue = entity.EKlaimDeliveryMethod;
            rblDeliveryLetakJanin.SelectedValue = entity.EKlaimDeliveryLetakJanin;
            rblDeliveryKondisi.SelectedValue = entity.EKlaimDeliveryKondisi;
            rblDeliveryUseManual.SelectedValue = entity.EKlaimDeliveryUseManual ? "1" : "0";
            rblDeliveryUseForcep.SelectedValue = entity.EKlaimDeliveryUseForcep ? "1" : "0";
            rblDeliveryUseVacuum.SelectedValue = entity.EKlaimDeliveryUseVacuum ? "1" : "0";
            rblDeliverySHKSpesienAmbil.SelectedValue = entity.EKlaimDeliverySHKSpesimenAmbil ? "1" : "0";
            rblDeliverySHKAlasan.SelectedValue = entity.EKlaimDeliverySHKAlasan;
            rblDeliverySHKLokasi.SelectedValue = entity.EKlaimDeliverySHKLokasi;
            if (entity.EKlaimDeliverySHKDatetime.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
            {
                txtDeliverySHKDate.Text = entity.EKlaimDeliverySHKDatetime.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDeliverySHKTime1.Text = entity.EKlaimDeliverySHKDatetime.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
                txtDeliverySHKTime2.Text = entity.EKlaimDeliverySHKDatetime.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
            }

            #endregion
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            List<GetPatientChargesHdDtEKlaimParameter1> lstEntity = BusinessLayer.GetPatientChargesHdDtEKlaimParameter1List(Convert.ToInt32(hdnRegistrationID.Value));
            tarif_rs otarif = new tarif_rs();
            string TarifData = string.Empty;
            Decimal lainlain = 0;
            if (lstEntity.Count > 0)
            {
                otarif.prosedur_non_bedah = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.ProsedurNonBedah).FirstOrDefault().PayerAmount.ToString();
                otarif.prosedur_bedah = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.ProsedurBedah).FirstOrDefault().PayerAmount.ToString();
                otarif.konsultasi = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Konsultasi).FirstOrDefault().PayerAmount.ToString();
                otarif.tenaga_ahli = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.TenagaAhli).FirstOrDefault().PayerAmount.ToString();
                otarif.keperawatan = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Keperawatan).FirstOrDefault().PayerAmount.ToString();
                otarif.penunjang = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Penunjang).FirstOrDefault().PayerAmount.ToString();
                otarif.radiologi = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Radiologi).FirstOrDefault().PayerAmount.ToString();
                otarif.laboratorium = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Laboratorium).FirstOrDefault().PayerAmount.ToString();
                otarif.pelayanan_darah = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.PelayananDarah).FirstOrDefault().PayerAmount.ToString();
                otarif.rehabilitasi = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Rehabilitasi).FirstOrDefault().PayerAmount.ToString();
                otarif.kamar = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.KamarAkomodasi).FirstOrDefault().PayerAmount.ToString();
                otarif.rawat_intensif = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.RawatIntensiff).FirstOrDefault().PayerAmount.ToString();
                otarif.obat = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Obat).FirstOrDefault().PayerAmount.ToString();
                otarif.obat_kronis = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.ObatKronis).FirstOrDefault().PayerAmount.ToString();
                otarif.obat_kemoterapi = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.ObatKemoterapi).FirstOrDefault().PayerAmount.ToString();
                otarif.alkes = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Alkes).FirstOrDefault().PayerAmount.ToString();
                otarif.bmhp = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.BMHP).FirstOrDefault().PayerAmount.ToString();
                otarif.sewa_alat = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.SewaAlat).FirstOrDefault().PayerAmount.ToString();
                lainlain = lstEntity.Where(p => p.EKlaimParameterCode == Constant.TypeTarifINACBG.Lainlain).FirstOrDefault().PayerAmount;
            }
            else
            {
                otarif.prosedur_non_bedah = "0";
                otarif.prosedur_bedah = "0";
                otarif.konsultasi = "0";
                otarif.tenaga_ahli = "0";
                otarif.keperawatan = "0";
                otarif.penunjang = "0";
                otarif.radiologi = "0";
                otarif.laboratorium = "0";
                otarif.pelayanan_darah = "0";
                otarif.rehabilitasi = "0";
                otarif.kamar = "0";
                otarif.rawat_intensif = "0";
                otarif.obat = "0";
                otarif.obat_kronis = "0";
                otarif.obat_kemoterapi = "0";
                otarif.alkes = "0";
                otarif.bmhp = "0";
                otarif.sewa_alat = "0";
                lainlain = 0;
            }
            setTarifEntity(otarif);
            if (lainlain > 0) // ada item yang belum di maaping untuk tarif inacbg
            {
                hdnIsMaapingTarifFailed.Value = "1";
            }
            else
            {
                hdnIsMaapingTarifFailed.Value = "0";
            }
            TarifData = JsonConvert.SerializeObject(otarif);
            hdnTarifKlaimJson.Value = TarifData;

            grdView.DataSource = lstEntity;
            grdView.DataBind();
            txtTotalBPJSAmount.Text = lstEntity.Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPatientAmount.Text = lstEntity.Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalAmount.Text = lstEntity.Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);
        }

        private void setTarifEntity(tarif_rs otarif)
        {
            hdnprosedur_non_bedah.Value = otarif.prosedur_non_bedah;
            hdnprosedur_bedah.Value = otarif.prosedur_bedah;
            hdnkonsultasi.Value = otarif.konsultasi;
            hdntenaga_ahli.Value = otarif.tenaga_ahli;
            hdnkeperawatan.Value = otarif.keperawatan;
            hdnpenunjang.Value = otarif.penunjang;
            hdnradiologi.Value = otarif.radiologi;
            hdnlaboratorium.Value = otarif.laboratorium;
            hdnpelayanan_darah.Value = otarif.pelayanan_darah;
            hdnrehabilitasi.Value = otarif.rehabilitasi;
            hdnkamar.Value = otarif.kamar;
            hdnrawat_intensif.Value = otarif.rawat_intensif;
            hdnobat.Value = otarif.obat;
            hdnobat_kronis.Value = otarif.obat_kronis;
            hdnobat_kemoterapi.Value = otarif.obat_kemoterapi;
            hdnalkes.Value = otarif.alkes;
            hdnbmhp.Value = otarif.bmhp;
            hdnsewa_alat.Value = otarif.sewa_alat;
        }

        private void SetSessionRegisteredPatient(vConsultVisit entity)
        {
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.DateOfBirth = entity.DateOfBirth;
            AppSession.RegisteredPatient = pt;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            string url = "";
            if (type == "upload")
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value))[0];
                SetSessionRegisteredPatient(entity);
                url = string.Format("~/Program/BPJS/TemporaryClaim/UploadDocument/BPJSDocument.aspx?id={0}|EK|", hdnRegistrationID.Value);

                retval = url;
            }
            return true;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string respEklaim = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "saveClaimDiagnose")
            {
                if (OnSaveEditClaimDiagnose(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "saveClaimProcedure")
            {
                if (OnSaveEditClaimProcedure(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "newClaim")
            {
                if (OnNewClaim(ref  errMessage, ref  respEklaim))
                {
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "updateClaim")
            {
                if (OnSetClaimData(ref errMessage, ref respEklaim))
                {
                    OnUpdateStatusLogBPJS("edit", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "grouperStage1")
            {
                if (OnGrouperStage1(ref errMessage, ref respEklaim))
                {
                    OnUpdateStatusLogBPJS("grouper1", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);

                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "grouperStage2")
            {
                if (OnGrouperStage2(ref errMessage, ref respEklaim))
                {
                    OnUpdateStatusLogBPJS("grouper2", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "finalClaim")
            {
                if (OnFinalClaimData(ref errMessage, ref respEklaim))
                {
                    OnUpdateStatusLogBPJS("final", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "getPrintClaim")
            {
                if (OnPrintClaimData(ref errMessage, ref respEklaim))
                {
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "getStatusClaim")
            {
                if (OnGetStatusKlaim(ref errMessage, ref respEklaim))
                {
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "getClaimData")
            {
                if (OnGetKlaimDetail(ref errMessage, ref respEklaim))
                {
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }

            }
            else if (param[0] == "reeditClaim")
            {
                if (OnEditClaimData(ref errMessage, ref respEklaim))
                {
                    OnUpdateStatusLogBPJS("reopen", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }

            }
            else if (param[0] == "sendClaimIndividual")
            {
                if (OnSendClaimIndividualData(ref errMessage, ref respEklaim))
                {
                    OnUpdateStatusLogBPJS("sendonline", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }

            }
            else if (param[0] == "deleteClaim")
            {

                if (OnDeletedClaimProcess(ref errMessage, ref respEklaim))
                {
                    OnUpdateStatusLogBPJS("delete", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "print_sep")
            {
                if (OnUpdatePrintNumber(ref  errMessage))
                {
                    result += string.Format("success|{0}", "");
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "sitb_validate")
            {
                if (OnValidateSITB(ref errMessage, ref respEklaim))
                {
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }

            }
            else if (param[0] == "sitb_invalidate")
            {
                if (OnInvalidateSITB(ref  errMessage, ref respEklaim))
                {
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }

            }
            else
            {
                bindData(ref errMessage, ref respEklaim);
                if (string.IsNullOrEmpty(errMessage))
                {
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Diagnose

        protected void cbpDiagnosaView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = string.Empty;

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "saveClaimDiagnose")
                {
                    if (OnSaveEditClaimDiagnose(ref errMessage))
                        result = "saveClaimDiagnose|1|success";
                    else
                        result = string.Format("saveClaimDiagnose|0|{0}", errMessage);
                }
                else if (param[0] == "deletedClaimDiagnose")
                {
                    if (OnDeletedClaimDiagnose(ref errMessage))
                        result = "deletedClaimDiagnose|1|success";
                    else
                        result = string.Format("deletedClaimDiagnose|0|{0}", errMessage);
                }
                else // refresh
                {
                    BindDiagnosaGridView();
                    result = "refresh|";
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindDiagnosaGridView()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

            if (hdnMenampilkanDiagnosaMasuk.Value != "1")
            {
                filterExpression += string.Format(" AND GCDiagnoseType != '{0}'", Constant.DiagnoseType.EARLY_DIAGNOSIS);
            }

            List<vPatientDiagnosisEklaim> lstEntity = BusinessLayer.GetvPatientDiagnosisEklaimList(filterExpression, int.MaxValue, 1, "GCDiagnoseType ASC, ID ASC");
            grdDiagnosaView.DataSource = lstEntity;
            grdDiagnosaView.DataBind();
        }

        private void ControlToEntity(PatientDiagnosis entity)
        {
            entity.ClaimDiagnosisDate = Helper.GetDatePickerValue(txtClaimDiagnosisDate);
            entity.ClaimDiagnosisTime = txtClaimDiagnosisTime.Text;
            entity.ClaimDiagnosisID = txtClaimDiagnosisCode.Text;
            entity.ClaimDiagnosisText = txtClaimDiagnosisText.Text;
            entity.ClaimINADiagnoseID = txtv6DiagnosaID.Text;
            entity.ClaimINADiagnoseText = txtv6DiagnosaName.Text;
            entity.ClaimDiagnosisBy = AppSession.UserLogin.UserID;
        }

        private bool IsValidToSave(ref string errMessage, bool IsAddMode)
        {
            string filterExpression = string.Format("VisitID = {0} AND GCDiagnoseTypeClaim = '{1}' AND IsDeleted = 0", hdnVisitID.Value, Constant.DiagnoseType.MAIN_DIAGNOSIS);
            if (!String.IsNullOrEmpty(hdnEntryID.Value))
            {
                filterExpression += string.Format(" AND ID != '{0}'", hdnEntryID.Value);
            }
            List<PatientDiagnosis> lstEntity = BusinessLayer.GetPatientDiagnosisList(filterExpression);
            if (lstEntity.Count > 0)
            {
                //Validate one episode should only have one main diagnose
                if (cboDiagnoseTypeClaim.Value.ToString() == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                {
                    errMessage = "Dalam satu episode keperawatan/kunjungan pasien hanya boleh ada 1 diagnosa utama.";
                    return false;
                }
            }
            return true;
        }

        private bool OnSaveEditClaimDiagnose(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDiagnosisDao oPatientDiagnosisDao = new PatientDiagnosisDao(ctx);
            try
            {
                if (!IsValidToSave(ref errMessage, true))
                {
                    result = false;
                }

                if (result)
                {
                    if (String.IsNullOrEmpty(hdnEntryID.Value))
                    {
                        PatientDiagnosis entity = new PatientDiagnosis();
                        entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        entity.ClaimDiagnosisDate = Helper.GetDatePickerValue(txtClaimDiagnosisDate);
                        entity.ClaimDiagnosisTime = txtClaimDiagnosisTime.Text;
                        entity.ClaimDiagnosisID = txtClaimDiagnosisCode.Text;
                        entity.ClaimDiagnosisText = txtClaimDiagnosisText.Text;
                        entity.ClaimINADiagnoseID = txtv6DiagnosaID.Text;
                        entity.ClaimINADiagnoseText = txtv6DiagnosaName.Text;
                        entity.ClaimDiagnosisBy = AppSession.UserLogin.UserID;
                        entity.GCDiagnoseTypeClaim = entity.GCDiagnoseType = cboDiagnoseTypeClaim.Value.ToString();
                        entity.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                        entity.IsIgnorePhysicianDiagnose = true;
                        entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        oPatientDiagnosisDao.Insert(entity);

                    }
                    else
                    {
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnEntryID.Value));
                        entity.GCDiagnoseTypeClaim = cboDiagnoseTypeClaim.Value.ToString();
                        entity.ClaimDiagnosisDate = Helper.GetDatePickerValue(txtClaimDiagnosisDate);
                        entity.ClaimDiagnosisTime = txtClaimDiagnosisTime.Text;
                        entity.ClaimDiagnosisID = txtClaimDiagnosisCode.Text;
                        entity.ClaimDiagnosisText = txtClaimDiagnosisText.Text;
                        entity.ClaimINADiagnoseID = txtv6DiagnosaID.Text;
                        entity.ClaimINADiagnoseText = txtv6DiagnosaName.Text;
                        entity.ClaimDiagnosisBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientDiagnosisDao.Update(entity);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
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

        private bool OnDeletedClaimDiagnose(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDiagnosisDao oPatientDiagnosisDao = new PatientDiagnosisDao(ctx);
            try
            {
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnEntryID.Value));
                if (entity != null)
                {
                    if (string.IsNullOrEmpty(entity.DiagnoseID) && string.IsNullOrEmpty(entity.FinalDiagnosisID) && string.IsNullOrEmpty(entity.DiagnosisText) && string.IsNullOrEmpty(entity.FinalDiagnosisText))
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientDiagnosisDao.Update(entity);
                    }
                    else
                    {
                        entity.GCDiagnoseTypeClaim = null;
                        entity.ClaimDiagnosisID = null;
                        entity.ClaimDiagnosisText = null;
                        entity.ClaimINADiagnoseID = null;
                        entity.ClaimINADiagnoseText = null;
                        entity.ClaimDiagnosisBy = null;
                        entity.ClaimDiagnosisDate = null;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientDiagnosisDao.Update(entity);
                    }


                }
                ctx.CommitTransaction();

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

        #endregion

        #region Procedure

        private void BindProcedureGridView()
        {
            string filterExpression = hdnFilterExpressionProcedure.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

            List<vPatientProcedure> lstEntity = BusinessLayer.GetvPatientProcedureList(filterExpression, int.MaxValue, 1, "ID ASC");
            grdProcedureView.DataSource = lstEntity;
            grdProcedureView.DataBind();
        }

        protected void cbpProcedureView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "saveClaimProcedure")
                {
                    if (OnSaveEditClaimProcedure(ref   errMessage))
                    {
                        result = "saveClaimProcedure|1|Success";
                    }
                    else
                    {
                        result = "saveClaimProcedure|0|" + errMessage;
                    }
                }
                if (param[0] == "deletedClaimProcedure")
                {
                    if (OnDeletedClaimProcedure(ref   errMessage))
                    {
                        result = "deletedClaimProcedure|1|Success";
                    }
                    else
                    {
                        result = "deletedClaimProcedure|0|" + errMessage;
                    }
                }
                else // refresh
                {
                    BindProcedureGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditClaimProcedure(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientProcedureDao oPatientProcedureDao = new PatientProcedureDao(ctx);

            try
            {
                if (string.IsNullOrEmpty(hdnEntryProsedureID.Value))
                {
                    PatientProcedure entity = new PatientProcedure();
                    entity.ClaimProcedureDate = Helper.GetDatePickerValue(txtClaimProcedureDate.Text);
                    entity.ClaimProcedureTime = txtClaimProcedureTime.Text;
                    entity.ClaimProcedureID = txtClaimProcedureCode.Text;
                    entity.ClaimProcedureText = txtClaimProcedureText.Text;
                    entity.ClaimProcedureBy = AppSession.UserLogin.UserID;
                    entity.ParamedicID = Convert.ToInt32(hdnProsedureDefaultParamedicID.Value);
                    entity.IsCreatedBySystem = false;
                    entity.ReferenceID = null;

                    entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    oPatientProcedureDao.Insert(entity);
                }
                else
                {
                    PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnEntryProsedureID.Value));
                    entity.ClaimProcedureDate = Helper.GetDatePickerValue(txtClaimProcedureDate.Text);
                    entity.ClaimProcedureTime = txtClaimProcedureTime.Text;
                    entity.ClaimProcedureID = txtClaimProcedureCode.Text;
                    entity.ClaimProcedureText = txtClaimProcedureText.Text;
                    entity.ClaimProcedureBy = AppSession.UserLogin.UserID;
                    entity.ParamedicID = Convert.ToInt32(hdnProsedureDefaultParamedicID.Value);
                    entity.IsCreatedBySystem = false;
                    entity.ReferenceID = null;

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    oPatientProcedureDao.Update(entity);
                }
                ctx.CommitTransaction();
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

        private bool OnDeletedClaimProcedure(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientProcedureDao oPatientProcedureDao = new PatientProcedureDao(ctx);

            try
            {
                PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnEntryProsedureID.Value));
                if (entity != null)
                {
                    if (string.IsNullOrEmpty(entity.ProcedureID) && string.IsNullOrEmpty(entity.FinalProcedureID) && string.IsNullOrEmpty(entity.ProcedureText) && string.IsNullOrEmpty(entity.FinalProcedureText))
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientProcedureDao.Update(entity);
                    }
                    else
                    {
                        entity.ClaimProcedureDate = null;
                        entity.ClaimProcedureBy = null;
                        entity.ClaimINAProcedureID = null;
                        entity.ClaimINAProcedureText = null;
                        entity.ClaimProcedureID = null;
                        entity.ClaimProcedureText = null;
                        entity.LastUpdatedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientProcedureDao.Update(entity);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Data prosedur/tindakan tidak ditemukan.";
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

        #endregion

        #region EKLAIM SERVICE

        private bool OnNewClaim(ref string errMessage, ref string respEklaim)
        {
            bool result = true;

            try
            {
                string filterRegBPJS = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                vRegistrationBPJS1 entity = BusinessLayer.GetvRegistrationBPJS1List(filterRegBPJS).FirstOrDefault();
                string gender = string.Empty;
                if (entity.GCGender == Constant.Gender.MALE)
                {
                    gender = "1";
                }
                else if (entity.GCGender == Constant.Gender.FEMALE)
                {
                    gender = "2";
                }

                string nama_pasien = entity.PatientName;
                string nomor_kartu = entity.NoPeserta;
                string nomor_sep = txtNoSEP.Text;
                string tgl_lahir = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);

                string nomor_rm = hdnIsSendEKlaimMedicalNo.Value == "1" ? (entity.EKlaimMedicalNo != null && entity.EKlaimMedicalNo != "" ? entity.EKlaimMedicalNo : entity.MedicalNo) : entity.MedicalNo;

                NewClaimMethod newClaim = new NewClaimMethod()
                {
                    metadata = new NewClaimMetadata()
                    {
                        method = "new_claim"
                    },
                    data = new NewClaimData()
                    {
                        gender = gender,
                        nama_pasien = nama_pasien,
                        nomor_kartu = nomor_kartu,
                        nomor_sep = nomor_sep,
                        tgl_lahir = tgl_lahir,
                        nomor_rm = nomor_rm
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(newClaim);
                EKlaimService eklaimService = new EKlaimService();

                string response = eklaimService.NewClaim(jsonRequest);
                respEklaim = response;
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnSetClaimData(ref string errMessage, ref string respEklaim)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao oRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            StandardCodeDao scDao = new StandardCodeDao(ctx);

            try
            {
                string filterRegBPJS = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                vRegistrationBPJS1 entity = BusinessLayer.GetvRegistrationBPJS1List(filterRegBPJS, ctx).FirstOrDefault();

                #region Definisi per Field

                var nomor_sep = entity.NoSEP;
                var nomor_kartu = entity.NoPeserta;

                var temp_tgl_masuk = "";
                var temp_jam_masuk = "";
                var temp_tgl_pulang = "";
                var temp_jam_pulang = "";

                if (hdnDepartmentID.Value != Constant.Facility.INPATIENT && hdnIsNonInpatientRegistrationDateFromSEPDate.Value == "1" && entity.TanggalSEP != null && entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                {
                    temp_tgl_masuk = hdnTanggalSEP.Value;
                    temp_jam_masuk = hdnJamSEP.Value;
                }
                else
                {
                    temp_tgl_masuk = hdnRegistrationDate.Value;
                    temp_jam_masuk = hdnRegistrationTime.Value;
                }

                if (hdnDepartmentID.Value != Constant.Facility.INPATIENT && hdnIsNonInpatientDischargeDateFromSEPDate.Value == "1" && entity.TanggalSEP != null && entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                {
                    temp_tgl_pulang = hdnTanggalSEP.Value;
                    temp_jam_pulang = hdnJamSEP.Value;
                }
                else
                {
                    temp_tgl_pulang = hdnDischargeDate.Value;
                    temp_jam_pulang = hdnDischargeTime.Value;
                }

                temp_jam_pulang += ":00";

                var tgl_masuk = string.Format("{0} {1}", changeFormatDate(temp_tgl_masuk), temp_jam_masuk);
                var tgl_pulang = string.Format("{0} {1}", changeFormatDate(temp_tgl_pulang), temp_jam_pulang);

                if (temp_tgl_pulang == Constant.ConstantDate.DEFAULT_NULL)
                {
                    tgl_pulang = "";
                }

                var cara_masuk = "";
                string caraMasuk = cboEKlaimCaraMasuk.Value.ToString();
                if (caraMasuk == Constant.EKlaimCaraMasuk.RUJUKAN_FKTP)
                {
                    cara_masuk = "gp";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.RUJUKAN_FKRTL)
                {
                    cara_masuk = "hosp-trans";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.RUJUKAN_DOKTER_SPESIALIS)
                {
                    cara_masuk = "mp";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.DARI_RAWAT_JALAN)
                {
                    cara_masuk = "outp";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.DARI_RAWAT_INAP)
                {
                    cara_masuk = "inp";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.DARI_RAWAT_DARURAT)
                {
                    cara_masuk = "emd";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.LAHIR_DI_RUMAH_SAKIT)
                {
                    cara_masuk = "born";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.RUJUKAN_DARI_PANTI_JOMPO)
                {
                    cara_masuk = "nursing";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.RUJUKAN_DARI_RUMAH_SAKIT_JIWA)
                {
                    cara_masuk = "psych";
                }
                else if (caraMasuk == Constant.EKlaimCaraMasuk.RUJUKAN_DARI_FASILITAS_REHABILITASI)
                {
                    cara_masuk = "rehab";
                }
                else
                {
                    cara_masuk = "other";
                }

                var jenis_rawat = "2"; /// 1 = rawat inap , 2 rawat jalan , 3 rawat igd
                var kelas_rawat = string.Empty;
                if (entity.DepartmentID == Constant.Facility.INPATIENT)
                {
                    jenis_rawat = "1";
                    kelas_rawat = entity.KelasTanggungan;
                }
                else if (entity.DepartmentID != Constant.Facility.INPATIENT)
                {
                    jenis_rawat = "2";
                    kelas_rawat = "3"; //1= eksekutif // 3 = reguler 
                    tgl_pulang = string.Format("{0} {1}:00", entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), entity.RegistrationTime);
                }

                var adl_sub_acute = "";
                var adl_chronic = "";

                var icu_indikator = "0";
                var icu_los = "";
                if (chkIsRawatIntensif.Checked)
                {
                    icu_indikator = "1";
                    icu_los = hdnICULOS.Value;
                }

                var ventilator_hour = txtLamaJamVentilator.Text;
                var ventilator_use_ind = txtLamaJamVentilator.Text != null && txtLamaJamVentilator.Text != "" && txtLamaJamVentilator.Text != "0" ? "1" : "0";
                var ventilator_start_dttm = "";
                if (txtVentilatorStartDate.Text != "")
                {
                    ventilator_start_dttm = Helper.GetDatePickerValue(txtVentilatorStartDate.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT2) + " " + txtVentilatorStartTime1.Text + ":" + txtVentilatorStartTime2.Text + ":00";
                }
                var ventilator_stop_dttm = "";
                if (txtVentilatorEndDate.Text != "")
                {
                    ventilator_stop_dttm = Helper.GetDatePickerValue(txtVentilatorEndDate.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT2) + " " + txtVentilatorEndTime1.Text + ":" + txtVentilatorEndTime2.Text + ":00";
                }

                var upgrade_class_ind = chkIsUpgradeClass.Checked ? "1" : "0";
                var upgrade_class_class = rblUpgradeClass.SelectedValue;
                var upgrade_class_los = txtUpgradeClassLOS.Text;
                var upgrade_class_payor = rblUpgradeClassPayor.SelectedValue;
                var add_payment_pct = txtAddPaymentPct.Text;

                var birth_weight = hdnBirthWeight.Value;

                var sistole = 0;
                if (!string.IsNullOrEmpty(txtSistole.Text))
                {
                    sistole = Convert.ToInt32(txtSistole.Text);
                }

                var diastole = 0;
                if (!string.IsNullOrEmpty(txtDiastole.Text))
                {
                    diastole = Convert.ToInt32(txtDiastole.Text);
                }

                var discharge_status = "";
                string caraPulang = cboEKlaimCaraPulang.Value.ToString();
                if (caraPulang == Constant.EKlaimCaraKeluar.ATAS_PERSETUJUAN)
                {
                    discharge_status = "1";
                }
                else if (caraPulang == Constant.EKlaimCaraKeluar.DIRUJUK)
                {
                    discharge_status = "2";
                }
                else if (caraPulang == Constant.EKlaimCaraKeluar.PERMINTAAN_SENDIRI)
                {
                    discharge_status = "3";
                }
                else if (caraPulang == Constant.EKlaimCaraKeluar.MENINGGAL)
                {
                    discharge_status = "4";
                }
                else if (caraPulang == Constant.EKlaimCaraKeluar.LAIN_LAIN)
                {
                    discharge_status = "5";
                }
                else
                {
                    discharge_status = "5";
                }

                #region Diagnose
                string filterExpressionDiagnosa = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                List<vPatientDiagnosisEklaim> lstDiagnosa = BusinessLayer.GetvPatientDiagnosisEklaimList(filterExpressionDiagnosa, ctx);
                string diagnosaValue = "#";
                string diagnosaINAValue = "#";
                if (lstDiagnosa.Count > 0)
                {
                    StringBuilder sbDiagnosaUtamav5 = new StringBuilder();
                    StringBuilder sbDiagnosaINAUtamav6 = new StringBuilder();
                    StringBuilder sbDiagnosa2v5 = new StringBuilder();
                    StringBuilder sbDiagnosaINA2v6 = new StringBuilder();
                    foreach (vPatientDiagnosisEklaim row in lstDiagnosa)
                    {
                        if (!string.IsNullOrEmpty(row.ClaimINACBGLabel) || !string.IsNullOrEmpty(row.INACBGINALabel))
                        {

                            if (row.GCDiagnoseTypeClaim == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                            {
                                sbDiagnosaUtamav5.Append(string.Format("{0}#", row.ClaimINACBGLabel));
                                sbDiagnosaINAUtamav6.Append(string.Format("{0}#", row.INACBGINALabel));
                            }

                        }
                        if (!string.IsNullOrEmpty(row.ClaimINACBGLabel) || !string.IsNullOrEmpty(row.INACBGINALabel))
                        {

                            if (row.GCDiagnoseTypeClaim != Constant.DiagnoseType.MAIN_DIAGNOSIS)
                            {
                                sbDiagnosa2v5.Append(string.Format("{0}#", row.ClaimINACBGLabel));
                                sbDiagnosaINA2v6.Append(string.Format("{0}#", row.INACBGINALabel));
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(sbDiagnosaUtamav5.ToString()) || !string.IsNullOrEmpty(sbDiagnosa2v5.ToString()))
                    {
                        diagnosaValue = string.Format("{0}{1}", sbDiagnosaUtamav5.ToString(), sbDiagnosa2v5.ToString());
                    }
                    if (!string.IsNullOrEmpty(sbDiagnosaINAUtamav6.ToString()) || !string.IsNullOrEmpty(sbDiagnosaINA2v6.ToString()))
                    {
                        diagnosaINAValue = string.Format("{0}{1}", sbDiagnosaINAUtamav6.ToString(), sbDiagnosaINA2v6.ToString());
                    }

                    if (!string.IsNullOrEmpty(diagnosaValue))
                    {
                        string[] diagnosValueChecker = diagnosaValue.Split('#');
                        if (diagnosaValue.Length > 1)
                        {
                            diagnosaValue = diagnosaValue.Remove(diagnosaValue.Length - 1);
                        }

                    }
                    else
                    {
                        result = false;
                        errMessage = "silahkan diisi diagnosa eklaim dan pastikan diagnosa tersebut sudah termaping";
                        return result;
                    }

                    if (!string.IsNullOrEmpty(diagnosaINAValue))
                    {
                        string[] diagnosINAChecker = diagnosaINAValue.Split('#');
                        if (diagnosINAChecker.Length > 1)
                        {
                            diagnosaINAValue = diagnosaINAValue.Remove(diagnosaINAValue.Length - 1);
                        }
                    }
                }
                var diagnosa = diagnosaValue;
                var diagnosa_inagrouper = diagnosaINAValue;
                #endregion

                #region Procedure
                string procedureValue = "#";
                string procedureINAValue = "#";
                string filterProcedure = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                List<vPatientProcedure> lstProcedure = BusinessLayer.GetvPatientProcedureList(filterProcedure, ctx);
                if (lstProcedure.Count > 0)
                {
                    StringBuilder sbProcedure = new StringBuilder();
                    StringBuilder sbProcedureINA = new StringBuilder();

                    foreach (vPatientProcedure row in lstProcedure)
                    {
                        if (!string.IsNullOrEmpty(row.ClaimINAProcedureID))
                        {
                            sbProcedure.Append(string.Format("{0}#", row.ClaimINAProcedureID));
                        }
                        if (!string.IsNullOrEmpty(row.ClaimINACBGINAProcedureID))
                        {
                            sbProcedureINA.Append(string.Format("{0}#", row.ClaimINACBGINAProcedureID));
                        }

                    }

                    if (!string.IsNullOrEmpty(sbProcedure.ToString()))
                    {
                        procedureValue = sbProcedure.ToString();
                    }
                    if (!string.IsNullOrEmpty(sbProcedureINA.ToString()))
                    {
                        procedureINAValue = sbProcedureINA.ToString();
                    }

                    if (!string.IsNullOrEmpty(procedureValue))
                    {
                        string[] checkProcedure = procedureValue.Split('#');
                        if (checkProcedure.Length > 1)
                        {
                            procedureValue = procedureValue.Remove(procedureValue.Length - 1);
                        }

                    }

                    if (!string.IsNullOrEmpty(procedureINAValue))
                    {
                        string[] checkProcedureIna = procedureINAValue.Split('#');
                        if (checkProcedureIna.Length > 1)
                        {
                            procedureINAValue = procedureINAValue.Remove(procedureINAValue.Length - 1);
                        }

                    }

                }
                var procedure = procedureValue;
                var procedure_inagrouper = procedureINAValue;
                #endregion

                #region TariffRS

                var prosedur_non_bedah = hdnprosedur_non_bedah.Value;
                var prosedur_bedah = hdnprosedur_bedah.Value;
                var konsultasi = hdnkonsultasi.Value;
                var tenaga_ahli = hdntenaga_ahli.Value;
                var keperawatan = hdnkeperawatan.Value;
                var penunjang = hdnpenunjang.Value;
                var radiologi = hdnradiologi.Value;
                var laboratorium = hdnlaboratorium.Value;
                var pelayanan_darah = hdnpelayanan_darah.Value;
                var rehabilitasi = hdnrehabilitasi.Value;
                var kamar = hdnkamar.Value;
                var rawat_intensif = hdnrawat_intensif.Value;
                var obat = hdnobat.Value;
                var obat_kronis = hdnobat_kronis.Value;
                var obat_kemoterapi = hdnobat_kemoterapi.Value;
                var alkes = hdnalkes.Value;
                var bmhp = hdnbmhp.Value;
                var sewa_alat = hdnsewa_alat.Value;
                var lainlain = "";

                #endregion

                var pemulasaraan_jenazah = rblPemulasaraanJenazah.SelectedValue != null ? rblPemulasaraanJenazah.SelectedValue : "";
                var kantong_jenazah = rblKantongJenazah.SelectedValue != null ? rblKantongJenazah.SelectedValue : "";
                var peti_jenazah = rblPetiJenazah.SelectedValue != null ? rblPetiJenazah.SelectedValue : "";
                var plastik_erat = rblPlastikErat.SelectedValue != null ? rblPlastikErat.SelectedValue : "";
                var desinfektan_jenazah = rblDesinfektanJenazah.SelectedValue != null ? rblDesinfektanJenazah.SelectedValue : "";
                var mobil_jenazah = rblMobilJenazah.SelectedValue != null ? rblMobilJenazah.SelectedValue : "";
                var desinfektan_mobil_jenazah = rblDesinfektanMobilJenazah.SelectedValue != null ? rblDesinfektanMobilJenazah.SelectedValue : "";

                var covid19_status_cd = "";

                var nomor_kartu_t = "";

                var episodes = "";
                var covid19_cc_ind = "";
                var covid19_rs_darurat_ind = "";
                var covid19_co_insidense_ind = "";

                #region Covid19PenunjangPengurang

                var lab_asam_laktat = "";
                var lab_procalcitonin = "";
                var lab_crp = "";
                var lab_kultur = "";
                var lab_d_dimer = "";
                var lab_pt = "";
                var lab_aptt = "";
                var lab_waktu_pendarahan = "";
                var lab_anti_hiv = "";
                var lab_analisa_gas = "";
                var lab_albumin = "";
                var rad_thorax_ap_pa = "";

                #endregion

                var terapi_konvalesen = "";
                var akses_naat = "";
                var isoman_ind = "";

                var bayi_lahir_status_cd = rblStatusBayiLahir.SelectedValue != null ? rblStatusBayiLahir.SelectedValue : "";

                var dializer_single_use = rblDializer.SelectedValue;
                var kantong_darah = txtEKlaimKantongDarah.Text != null && txtEKlaimKantongDarah.Text != "" ? Convert.ToInt32(txtEKlaimKantongDarah.Text) : 0;

                #region APGAR Score
                menit_1 menit_1 = new menit_1()
                {
                    appearance = txtEKlaimAPGARMenit1Appearance.Text != null && txtEKlaimAPGARMenit1Appearance.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit1Appearance.Text) : 0,
                    pulse = txtEKlaimAPGARMenit1Pulse.Text != null && txtEKlaimAPGARMenit1Pulse.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit1Pulse.Text) : 0,
                    grimace = txtEKlaimAPGARMenit1Grimace.Text != null && txtEKlaimAPGARMenit1Grimace.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit1Grimace.Text) : 0,
                    activity = txtEKlaimAPGARMenit1Activity.Text != null && txtEKlaimAPGARMenit1Activity.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit1Activity.Text) : 0,
                    respiration = txtEKlaimAPGARMenit1Respiration.Text != null && txtEKlaimAPGARMenit1Respiration.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit1Respiration.Text) : 0,
                };
                menit_5 menit_5 = new menit_5()
                {
                    appearance = txtEKlaimAPGARMenit5Appearance.Text != null && txtEKlaimAPGARMenit5Appearance.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit5Appearance.Text) : 0,
                    pulse = txtEKlaimAPGARMenit5Pulse.Text != null && txtEKlaimAPGARMenit5Pulse.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit5Pulse.Text) : 0,
                    grimace = txtEKlaimAPGARMenit5Grimace.Text != null && txtEKlaimAPGARMenit5Grimace.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit5Grimace.Text) : 0,
                    activity = txtEKlaimAPGARMenit5Activity.Text != null && txtEKlaimAPGARMenit5Activity.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit5Activity.Text) : 0,
                    respiration = txtEKlaimAPGARMenit5Respiration.Text != null && txtEKlaimAPGARMenit5Respiration.Text != "" ? Convert.ToInt32(txtEKlaimAPGARMenit5Respiration.Text) : 0,
                };
                apgar apgar = new apgar()
                {
                    menit_1 = menit_1,
                    menit_5 = menit_5,
                };
                #endregion

                #region Persalinan & Delivery
                List<delivery> lstDeliveryData = new List<delivery>();
                delivery deliverydt = new delivery()
                {
                    delivery_sequence = txtEKlaimDeliverySequence.Text != "" ? txtEKlaimDeliverySequence.Text : "0",
                    delivery_method = rblDeliveryMethod.SelectedValue != null ? rblDeliveryMethod.SelectedValue : "",
                    delivery_dttm = "",
                    letak_janin = rblDeliveryLetakJanin.SelectedValue != null ? rblDeliveryLetakJanin.SelectedValue : "",
                    kondisi = rblDeliveryKondisi.SelectedValue != null ? rblDeliveryKondisi.SelectedValue : "",
                    use_manual = rblDeliveryUseManual.SelectedValue != null ? rblDeliveryUseManual.SelectedValue : "",
                    use_forcep = rblDeliveryUseForcep.SelectedValue != null ? rblDeliveryUseForcep.SelectedValue : "",
                    use_vacuum = rblDeliveryUseVacuum.SelectedValue != null ? rblDeliveryUseVacuum.SelectedValue : "",
                    shk_spesimen_ambil = rblDeliverySHKSpesienAmbil.SelectedValue != null ? rblDeliverySHKSpesienAmbil.SelectedValue : "tidak",
                    shk_alasan = rblDeliverySHKAlasan.SelectedValue != null ? rblDeliverySHKAlasan.SelectedValue : "tidak-dapat",
                    shk_lokasi = rblDeliverySHKLokasi.SelectedValue != null ? rblDeliverySHKLokasi.SelectedValue : "",
                    shk_spesimen_dttm = ventilator_start_dttm = Helper.GetDatePickerValue(txtDeliverySHKDate.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT2) + " " + txtDeliverySHKTime1.Text + ":" + txtDeliverySHKTime2.Text + ":00"
                };
                lstDeliveryData.Add(deliverydt);

                persalinan persalinan = new persalinan()
                {
                    usia_kehamilan = txtEKlaimPersalinanUsiaKehamilan.Text != null && txtEKlaimPersalinanUsiaKehamilan.Text != "" ? Convert.ToInt32(txtEKlaimPersalinanUsiaKehamilan.Text) : 0,
                    gravida = txtEKlaimPersalinanGravida.Text != null && txtEKlaimPersalinanGravida.Text != "" ? Convert.ToInt32(txtEKlaimPersalinanGravida.Text) : 0,
                    partus = txtEKlaimPersalinanPartus.Text != null && txtEKlaimPersalinanPartus.Text != "" ? Convert.ToInt32(txtEKlaimPersalinanPartus.Text) : 0,
                    abortus = txtEKlaimPersalinanAbortus.Text != null && txtEKlaimPersalinanAbortus.Text != "" ? Convert.ToInt32(txtEKlaimPersalinanAbortus.Text) : 0,
                    onset_kontraksi = rblOnSetKontraksi.SelectedValue != null ? rblOnSetKontraksi.SelectedValue : "",
                    delivery = lstDeliveryData,
                };
                #endregion

                var tarif_poli_eks = txtTarifPoliEks.Text;

                var nama_dokter = txtParamedicName.Text;

                if (hdnIsDokterUsingDokterKonsulenVClaim.Value == "1")
                {
                    nama_dokter = txtNamaDPJPKonsulan.Text;
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                StandardCode scKdTariffINACBG = scDao.Get(cboKdTarifINACBG.Value.ToString());
                var kode_tarif = scKdTariffINACBG.TagProperty;

                var payor_id = "";
                var payor_cd = "";
                if (cboCaraBayarEKlaim.Value != null)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    StandardCode scCaraBayar = scDao.Get(cboCaraBayarEKlaim.Value.ToString());
                    string[] payorList = scCaraBayar.TagProperty.Split('|');
                    payor_id = payorList[0];
                    payor_cd = payorList[1];
                }

                var cob_cd = "";
                if (cboCOBEKlaim.Value != null)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    StandardCode scCOB = scDao.Get(cboCOBEKlaim.Value.ToString());
                    cob_cd = scCOB.TagProperty;
                }

                var coder_nik = hdnUsernameLogin.Value;

                #endregion

                #region SetClaimMethod

                SetClaimMethod set_claim_method = new SetClaimMethod()
                {
                    metadata = new SetClaimMetadata()
                    {
                        method = "set_claim_data",
                        nomor_sep = entity.NoSEP
                    },
                    data = new SetClaimData()
                    {
                        nomor_sep = nomor_sep,
                        nomor_kartu = nomor_kartu,
                        tgl_masuk = tgl_masuk,
                        tgl_pulang = tgl_pulang,
                        cara_masuk = cara_masuk,
                        jenis_rawat = jenis_rawat,
                        kelas_rawat = kelas_rawat,
                        adl_sub_acute = adl_sub_acute,
                        adl_chronic = adl_chronic,
                        icu_indikator = icu_indikator,
                        icu_los = icu_los,
                        ventilator_hour = ventilator_hour,
                        ventilator = new ventilator()
                        {
                            use_ind = ventilator_use_ind,
                            start_dttm = ventilator_start_dttm,
                            stop_dttm = ventilator_stop_dttm
                        },
                        upgrade_class_ind = upgrade_class_ind,
                        upgrade_class_class = upgrade_class_class,
                        upgrade_class_los = upgrade_class_los,
                        upgrade_class_payor = upgrade_class_payor,
                        add_payment_pct = add_payment_pct,
                        birth_weight = birth_weight,
                        diastole = diastole,
                        sistole = sistole,
                        discharge_status = discharge_status,
                        diagnosa = diagnosa,
                        procedure = procedure,
                        diagnosa_inagrouper = diagnosa_inagrouper,
                        procedure_inagrouper = procedure_inagrouper,
                        tarif_rs = new tarif_rs()
                        {
                            prosedur_non_bedah = prosedur_non_bedah,
                            prosedur_bedah = prosedur_bedah,
                            konsultasi = konsultasi,
                            tenaga_ahli = tenaga_ahli,
                            keperawatan = keperawatan,
                            penunjang = penunjang,
                            radiologi = radiologi,
                            laboratorium = laboratorium,
                            pelayanan_darah = pelayanan_darah,
                            rehabilitasi = rehabilitasi,
                            kamar = kamar,
                            rawat_intensif = rawat_intensif,
                            obat = obat,
                            obat_kronis = obat_kronis,
                            obat_kemoterapi = obat_kemoterapi,
                            alkes = alkes,
                            bmhp = bmhp,
                            sewa_alat = sewa_alat
                        },
                        pemulasaraan_jenazah = pemulasaraan_jenazah,
                        kantong_jenazah = kantong_jenazah,
                        peti_jenazah = peti_jenazah,
                        plastik_erat = plastik_erat,
                        desinfektan_jenazah = desinfektan_jenazah,
                        mobil_jenazah = mobil_jenazah,
                        desinfektan_mobil_jenazah = desinfektan_mobil_jenazah,
                        covid19_status_cd = covid19_status_cd,
                        nomor_kartu_t = nomor_kartu_t,
                        episodes = episodes,
                        covid19_cc_ind = covid19_cc_ind,
                        covid19_rs_darurat_ind = covid19_rs_darurat_ind,
                        covid19_co_insidense_ind = covid19_co_insidense_ind,
                        covid19_penunjang_pengurang = new covid19_penunjang_pengurang()
                        {
                            lab_asam_laktat = lab_asam_laktat,
                            lab_procalcitonin = lab_procalcitonin,
                            lab_crp = lab_crp,
                            lab_kultur = lab_kultur,
                            lab_d_dimer = lab_d_dimer,
                            lab_pt = lab_pt,
                            lab_aptt = lab_aptt,
                            lab_waktu_pendarahan = lab_waktu_pendarahan,
                            lab_anti_hiv = lab_anti_hiv,
                            lab_analisa_gas = lab_analisa_gas,
                            lab_albumin = lab_albumin,
                            rad_thorax_ap_pa = rad_thorax_ap_pa
                        },
                        terapi_konvalesen = terapi_konvalesen,
                        akses_naat = akses_naat,
                        isoman_ind = isoman_ind,
                        bayi_lahir_status_cd = bayi_lahir_status_cd,
                        dializer_single_use = dializer_single_use,
                        kantong_darah = kantong_darah,
                        apgar = apgar,
                        persalinan = persalinan,
                        tarif_poli_eks = tarif_poli_eks,
                        nama_dokter = nama_dokter,
                        kode_tarif = kode_tarif,
                        payor_id = payor_id,
                        payor_cd = payor_cd,
                        cob_cd = cob_cd,
                        coder_nik = coder_nik,
                    }
                };

                #endregion

                string paramRequest = JsonConvert.SerializeObject(set_claim_method);
                EKlaimService oEKlaimService = new EKlaimService();
                var respData = oEKlaimService.SetClaimData2(paramRequest);
                SetClaimResponse respInfo = JsonConvert.DeserializeObject<SetClaimResponse>(respData.ToString());
                respEklaim = respData.ToString();

                #region RegistrationBPJS

                RegistrationBPJS oRegBpjs = oRegistrationBPJSDao.Get(entity.RegistrationID);
                if (oRegBpjs != null)
                {
                    oRegBpjs.GCEKlaimCaraMasuk = cboEKlaimCaraMasuk.Value.ToString();
                    if (adl_sub_acute != null && adl_sub_acute != "")
                    {
                        oRegBpjs.EKlaimADLSubAcute = Convert.ToInt32(adl_sub_acute);
                    }
                    if (adl_chronic != null && adl_chronic != "")
                    {
                        oRegBpjs.EKlaimADLChronic = Convert.ToInt32(adl_chronic);
                    }
                    if (icu_indikator != null && icu_indikator != "")
                    {
                        oRegBpjs.EKlaimICUIndicator = Convert.ToInt32(icu_indikator);
                    }
                    if (icu_los != null && icu_los != "")
                    {
                        oRegBpjs.EKlaimICUIndicator = Convert.ToInt32(icu_los);
                    }
                    if (ventilator_hour != null && ventilator_hour != "")
                    {
                        oRegBpjs.EKlaimVentilatorHour = Convert.ToInt32(ventilator_hour);
                        oRegBpjs.EKlaimVentilatorUseIndex = Convert.ToInt32(ventilator_use_ind);
                        oRegBpjs.EKlaimVentilatorStartDatetime = Convert.ToDateTime(ventilator_start_dttm);
                        oRegBpjs.EKlaimVentilatorStopDatetime = Convert.ToDateTime(ventilator_stop_dttm);
                    }
                    if (upgrade_class_ind != null && upgrade_class_ind != "")
                    {
                        oRegBpjs.EKlaimUpgradeClassIndex = Convert.ToInt32(upgrade_class_ind);
                    }
                    oRegBpjs.EKlaimUpgradeClassCode = upgrade_class_class;
                    if (upgrade_class_los != null && upgrade_class_los != "")
                    {
                        oRegBpjs.EKlaimUpgradeClassLOS = Convert.ToInt32(upgrade_class_los);
                    }
                    oRegBpjs.EKlaimUpgradeClassPayor = upgrade_class_payor;
                    if (add_payment_pct != null && add_payment_pct != "")
                    {
                        oRegBpjs.EKlaimAddPaymentPct = Convert.ToInt32(add_payment_pct);
                    }
                    if (birth_weight != null && birth_weight != "")
                    {
                        oRegBpjs.EKlaimBirthWeight = Convert.ToDecimal(birth_weight);
                    }
                    oRegBpjs.EKlaimSistole = sistole;
                    oRegBpjs.EKlaimDiastole = diastole;
                    oRegBpjs.GCEKlaimDischargeStatus = cboEKlaimCaraPulang.Value.ToString();

                    oRegBpjs.EKlaimPemulasaraanJenazah = pemulasaraan_jenazah == "1" ? true : false;
                    oRegBpjs.EKlaimKantongJenazah = kantong_jenazah == "1" ? true : false;
                    oRegBpjs.EKlaimPetiJenazah = peti_jenazah == "1" ? true : false;
                    oRegBpjs.EKlaimPlastikErat = plastik_erat == "1" ? true : false;
                    oRegBpjs.EKlaimDesinfektanJenazah = desinfektan_jenazah == "1" ? true : false;
                    oRegBpjs.EKlaimMobilJenazah = mobil_jenazah == "1" ? true : false;
                    oRegBpjs.EKlaimDesinfektanMobilJenazah = desinfektan_mobil_jenazah == "1" ? true : false;

                    if (terapi_konvalesen != null && terapi_konvalesen != "")
                    {
                        oRegBpjs.EKlaimTerapiKonvalesen = Convert.ToInt32(terapi_konvalesen);
                    }
                    if (isoman_ind != null && isoman_ind != "")
                    {
                        oRegBpjs.EKlaimIsomanIndex = Convert.ToInt32(isoman_ind);
                    }
                    if (bayi_lahir_status_cd != null && bayi_lahir_status_cd != "")
                    {
                        oRegBpjs.EKlaimBayiLahirStatus = Convert.ToInt32(bayi_lahir_status_cd);
                    }

                    if (dializer_single_use != null && dializer_single_use != "")
                    {
                        oRegBpjs.EKlaimDializerSingleUse = Convert.ToInt32(dializer_single_use);
                    }
                    oRegBpjs.EKlaimKantongDarah = kantong_darah;

                    oRegBpjs.EKlaimAPGARMenit1Appearance = apgar.menit_1.appearance;
                    oRegBpjs.EKlaimAPGARMenit1Pulse = apgar.menit_1.pulse;
                    oRegBpjs.EKlaimAPGARMenit1Grimace = apgar.menit_1.grimace;
                    oRegBpjs.EKlaimAPGARMenit1Activity = apgar.menit_1.activity;
                    oRegBpjs.EKlaimAPGARMenit1Respiration = apgar.menit_1.respiration;
                    oRegBpjs.EKlaimAPGARMenit5Appearance = apgar.menit_5.appearance;
                    oRegBpjs.EKlaimAPGARMenit5Pulse = apgar.menit_5.pulse;
                    oRegBpjs.EKlaimAPGARMenit5Grimace = apgar.menit_5.grimace;
                    oRegBpjs.EKlaimAPGARMenit5Activity = apgar.menit_5.activity;
                    oRegBpjs.EKlaimAPGARMenit5Respiration = apgar.menit_5.respiration;
                    if (tarif_poli_eks != null && tarif_poli_eks != "")
                    {
                        oRegBpjs.EKlaimTariffPoliEksekutif = Convert.ToDecimal(tarif_poli_eks);
                    }

                    oRegBpjs.EKlaimPersalinanUsiaKehamilan = persalinan.usia_kehamilan;
                    oRegBpjs.EKlaimPersalinanGravida = persalinan.gravida;
                    oRegBpjs.EKlaimPersalinanPartus = persalinan.partus;
                    oRegBpjs.EKlaimPersalinanAbortus = persalinan.abortus;
                    oRegBpjs.EKlaimPersalinanOnsetKontraksi = persalinan.onset_kontraksi;

                    oRegBpjs.EKlaimDeliverySequence = lstDeliveryData.FirstOrDefault().delivery_sequence != null ? Convert.ToInt32(lstDeliveryData.FirstOrDefault().delivery_sequence) : 0;
                    oRegBpjs.EKlaimDeliveryMethod = lstDeliveryData.FirstOrDefault().delivery_method;
                    if (lstDeliveryData.FirstOrDefault().delivery_dttm != null && lstDeliveryData.FirstOrDefault().delivery_dttm != "")
                    {
                        oRegBpjs.EKlaimDeliveryDatetime = Convert.ToDateTime(lstDeliveryData.FirstOrDefault().delivery_dttm);
                    }
                    oRegBpjs.EKlaimDeliveryLetakJanin = lstDeliveryData.FirstOrDefault().letak_janin;
                    oRegBpjs.EKlaimDeliveryKondisi = lstDeliveryData.FirstOrDefault().kondisi;
                    oRegBpjs.EKlaimDeliveryUseManual = lstDeliveryData.FirstOrDefault().use_manual == "1" ? true : false;
                    oRegBpjs.EKlaimDeliveryUseForcep = lstDeliveryData.FirstOrDefault().use_forcep == "1" ? true : false;
                    oRegBpjs.EKlaimDeliveryUseVacuum = lstDeliveryData.FirstOrDefault().use_vacuum == "1" ? true : false;
                    oRegBpjs.EKlaimDeliverySHKSpesimenAmbil = lstDeliveryData.FirstOrDefault().shk_spesimen_ambil == "ya" ? true : false;
                    oRegBpjs.EKlaimDeliverySHKAlasan = lstDeliveryData.FirstOrDefault().shk_alasan;
                    oRegBpjs.EKlaimDeliverySHKLokasi = lstDeliveryData.FirstOrDefault().shk_lokasi;
                    if (lstDeliveryData.FirstOrDefault().shk_spesimen_dttm != null && lstDeliveryData.FirstOrDefault().shk_spesimen_dttm != "" && lstDeliveryData.FirstOrDefault().shk_spesimen_dttm != "1900-01-01 ::00")
                    {
                        oRegBpjs.EKlaimDeliverySHKDatetime = Convert.ToDateTime(lstDeliveryData.FirstOrDefault().shk_spesimen_dttm);
                    }

                    oRegBpjs.EKlaimNamaDokter = nama_dokter;
                    oRegBpjs.EKlaimKodeTariff = kode_tarif;
                    oRegBpjs.EKlaimPayorID = payor_id;
                    oRegBpjs.EKlaimPayorCode = payor_cd;
                    oRegBpjs.EKlaimCOBCode = cob_cd;
                    oRegBpjs.EKlaimCoderNIK = coder_nik;
                    oRegBpjs.LastUpdatedBy = AppSession.UserLogin.UserID;
                    oRegistrationBPJSDao.Update(oRegBpjs);
                }

                #endregion

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private bool OnGrouperStage1(ref string errMessage, ref string respEklaim)
        {
            bool result = true;

            try
            {
                GroupingStage1Method grouping_stage_1 = new GroupingStage1Method()
                {
                    metadata = new GroupingStage1Metadata()
                    {
                        method = "grouper",
                        stage = "1"
                    },
                    data = new GroupingStage1Data()
                    {
                        nomor_sep = txtNoSEP.Text
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(grouping_stage_1);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.GroupingStage1(jsonRequest);
                respEklaim = response.ToString();

                ///chek update amount 
                if (!string.IsNullOrEmpty(response))
                {
                    GroupingStage1Response respInfo = JsonConvert.DeserializeObject<GroupingStage1Response>(response);
                    if (respInfo.metadata.code == "200")
                    {
                        //check special 
                        if (respInfo.special_cmg_option != null)
                        {
                            if (respInfo.special_cmg_option.Count > 0)
                            {
                                hdnIsGrouperStage2.Value = "1";
                                List<GroupingStage1SpecialCMGOption> lstspecial_cmg_option = respInfo.special_cmg_option;

                                Methods.SetComboBoxField<GroupingStage1SpecialCMGOption>(cboSpecialProsthesis, lstspecial_cmg_option.Where(p => p.type == "Special Prosthesis").ToList(), "description", "code");
                                Methods.SetComboBoxField<GroupingStage1SpecialCMGOption>(cboSpecialProcedure, lstspecial_cmg_option.Where(p => p.type == "Special Procedure").ToList(), "description", "code");
                                Methods.SetComboBoxField<GroupingStage1SpecialCMGOption>(cboSpecialInvestigation, lstspecial_cmg_option.Where(p => p.type == "Special Investigation").ToList(), "description", "code");
                                Methods.SetComboBoxField<GroupingStage1SpecialCMGOption>(cboSpecialDrug, lstspecial_cmg_option.Where(p => p.type == "Special Drug").ToList(), "description", "code");


                                cboSpecialDrug.SelectedIndex = 0;
                                cboSpecialProcedure.SelectedIndex = 0;
                                cboSpecialInvestigation.SelectedIndex = 0;
                                cboSpecialDrug.SelectedIndex = 0;

                            }
                            else
                            {
                                hdnIsGrouperStage2.Value = "0";
                            }
                        }
                        else
                        {
                            hdnIsGrouperStage2.Value = "0";
                        }

                        UpdateStatusProcess();

                        if (hdnIsGrouperStage2.Value == "0")
                        {
                            string errMessage1 = "";
                            string resp = "";
                            bindData(ref   errMessage1, ref   resp);
                        }
                        else
                        {
                            grouperDescription.InnerText = respInfo.response.cbg.description;
                            grouperCode.InnerHtml = respInfo.response.cbg.code;
                            grouperValue1.Value = Convert.ToDecimal(respInfo.response.cbg.tariff).ToString(Constant.FormatString.NUMERIC_2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;

        }

        private bool OnGrouperStage2(ref string errMessage, ref string respEklaim)
        {
            bool result = true;

            try
            {
                string special_cmg = string.Empty;
                if (cboSpecialDrug.Value != null)
                {
                    special_cmg += string.Format("{0}#", cboSpecialDrug.Value.ToString());
                }

                if (cboSpecialProcedure.Value != null)
                {
                    special_cmg += string.Format("{0}#", cboSpecialProcedure.Value.ToString());
                }

                if (cboSpecialProsthesis.Value != null)
                {
                    special_cmg += string.Format("{0}#", cboSpecialProsthesis.Value.ToString());
                }

                if (cboSpecialInvestigation.Value != null)
                {
                    special_cmg += string.Format("{0}#", cboSpecialInvestigation.Value.ToString());
                }

                if (!string.IsNullOrEmpty(special_cmg))
                {
                    special_cmg = special_cmg.Remove(special_cmg.Length - 1);
                }


                GroupingStage2Method grouping_stage_2 = new GroupingStage2Method()
                {
                    metadata = new GroupingStage2Metadata()
                    {
                        method = "grouper",
                        stage = "2"
                    },
                    data = new GroupingStage2Data()
                    {
                        nomor_sep = txtNoSEP.Text,
                        special_cmg = special_cmg
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(grouping_stage_2);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.GroupingStage2(jsonRequest);
                respEklaim = response.ToString();

                ///chek update amount 
                if (!string.IsNullOrEmpty(response))
                {

                    GroupingStage2Response respInfo = JsonConvert.DeserializeObject<GroupingStage2Response>(response);
                    if (respInfo.metadata.code == "200")
                    {
                        UpdateStatusProcess();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnFinalClaimData(ref string errMessage, ref string respEklaim)
        {
            bool isExistingPayment = false;

            #region PatientPayment
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus <> '{1}' AND GCPaymentType = '{2}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER);
            List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(filterExpression);
            if (lstPaymentHd.Count > 0)
            {
                string paymentID = "";
                foreach (PatientPaymentHd row in lstPaymentHd)
                {
                    paymentID += string.Format("{0},", row.PaymentID);
                }

                if (!string.IsNullOrEmpty(paymentID))
                {
                    paymentID = paymentID.Remove(paymentID.Length - 1);
                    string paymentDtID = "";
                    string filterExpressionDt = string.Format("PaymentID IN ({0}) AND BusinessPartnerID IN (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{1}' AND BusinessPartnerID = PatientPaymentDt.BusinessPartnerID) AND IsDeleted = 0", paymentID, Constant.CustomerType.BPJS);
                    List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(filterExpressionDt);
                    if (lstPaymentDt.Count > 0)
                    {
                        foreach (PatientPaymentDt rowDt in lstPaymentDt)
                        {
                            paymentDtID += string.Format("{0},", rowDt.PaymentDetailID);
                        }

                        paymentDtID = paymentDtID.Remove(paymentDtID.Length - 1);
                        List<PatientPaymentDtInfo> lstPaymentDtInfo = BusinessLayer.GetPatientPaymentDtInfoList(string.Format("PaymentDetailID IN ({0}) ", paymentDtID));
                        if (lstPaymentDtInfo.Count > 0)
                        {
                            isExistingPayment = true;
                        }
                    }

                }

            }

            #endregion

            if (isExistingPayment == false)
            {
                errMessage = "Proses gagal karena belum memiliki nomor pembayaran yang akan diproses";
                return false;
            }
            else
            {
                try
                {
                    ClaimFinalMethod ClaimFinal = new ClaimFinalMethod()
                    {
                        metadata = new ClaimFinalMetadata()
                        {
                            method = "claim_final",
                        },
                        data = new ClaimFinalData()
                        {
                            coder_nik = hdnUsernameLogin.Value,
                            nomor_sep = txtNoSEP.Text,

                        }
                    };
                    string jsonRequest = JsonConvert.SerializeObject(ClaimFinal);
                    EKlaimService eklaimService = new EKlaimService();
                    string response = eklaimService.ClaimFinal(jsonRequest);
                    respEklaim = response.ToString();

                    #region UpdateStatusProcess
                    UpdateStatusProcess();
                    #endregion

                    return true;
                }
                catch (Exception er)
                {
                    errMessage = er.Message.ToString();
                    return false;
                }

            }

        }

        private bool OnPrintClaimData(ref string errMessage, ref string respEklaim)
        {
            bool result = true;
            try
            {
                ClaimPrintMethod getClaimDt = new ClaimPrintMethod()
                {
                    metadata = new ClaimPrintMetadata()
                    {
                        method = "claim_print"
                    },
                    data = new ClaimPrintData()
                    {
                        nomor_sep = txtNoSEP.Text

                    }
                };

                string jsonRequest = JsonConvert.SerializeObject(getClaimDt);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.ClaimPrint(jsonRequest);
                respEklaim = response;
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnGetStatusKlaim(ref string errMessage, ref string respEklaim)
        {
            bool result = true;
            try
            {
                GetClaimStatusMethod getClaimDt = new GetClaimStatusMethod()
                {
                    metadata = new GetClaimStatusMetadata()
                    {
                        method = "get_claim_status"
                    },
                    data = new GetClaimStatusData()
                    {
                        nomor_sep = txtNoSEP.Text

                    }
                };

                string jsonRequest = JsonConvert.SerializeObject(getClaimDt);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.GetClaimStatus(jsonRequest);
                respEklaim = response;
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnGetKlaimDetail(ref string errMessage, ref string respEklaim)
        {
            bool result = true;
            try
            {
                GetClaimMethod getClaimDt = new GetClaimMethod()
                {
                    metadata = new GetClaimMetadata()
                    {
                        method = "get_claim_data"
                    },
                    data = new GetClaimData()
                    {
                        nomor_sep = txtNoSEP.Text
                    }
                };

                string jsonRequest = JsonConvert.SerializeObject(getClaimDt);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.GetClaim(jsonRequest);
                respEklaim = response.ToString();

                if (!string.IsNullOrEmpty(response))
                {
                    GetClaimResponse respInfo = JsonConvert.DeserializeObject<GetClaimResponse>(response);

                    if (respInfo.metadata.code == "200")
                    {
                        if (respInfo.response.data.icu_indikator == "1")
                        {
                            chkIsRawatIntensif.Checked = true;
                            txtLamaJamVentilator.Text = respInfo.response.data.ventilator_hour;
                            txtICULOS.Text = respInfo.response.data.icu_los;
                            hdnICULOS.Value = respInfo.response.data.icu_los;
                        }

                        decimal totalAmount = 0;
                        decimal cbgTarif = 0;
                        if (respInfo.response.data.grouper.response != null)
                        {
                            cbgTarif = Convert.ToDecimal(respInfo.response.data.grouper.response.cbg.tariff);
                            if (respInfo.response.data.grouper.response.special_cmg != null)
                            {
                                if (respInfo.response.data.grouper.response.special_cmg.Count > 0)
                                {
                                    List<special_cmg> lstspecial_cmg_option = respInfo.response.data.grouper.response.special_cmg;
                                    if (lstspecial_cmg_option.Count > 0)
                                    {
                                        Methods.SetComboBoxField<special_cmg>(cboSpecialProsthesis, lstspecial_cmg_option.Where(p => p.type == "Special Prosthesis").ToList(), "description", "code");
                                        Methods.SetComboBoxField<special_cmg>(cboSpecialProcedure, lstspecial_cmg_option.Where(p => p.type == "Special Procedure").ToList(), "description", "code");
                                        Methods.SetComboBoxField<special_cmg>(cboSpecialInvestigation, lstspecial_cmg_option.Where(p => p.type == "Special Investigation").ToList(), "description", "code");
                                        Methods.SetComboBoxField<special_cmg>(cboSpecialDrug, lstspecial_cmg_option.Where(p => p.type == "Special Drug").ToList(), "description", "code");

                                        foreach (special_cmg row in lstspecial_cmg_option)
                                        {

                                            if (row.type == "Special Prosthesis")
                                            {
                                                cboSpecialProsthesis.Value = row.code;
                                                lblSpecialProsthesis.InnerText = row.code;
                                                txtSpecialProsthesisAmount.Value = row.tariff.ToString(Constant.FormatString.NUMERIC_2);
                                                totalAmount += row.tariff;
                                            }
                                            else if (row.type == "Special Procedure")
                                            {
                                                cboSpecialProcedure.Value = row.code;
                                                lblSpecialProcedure.InnerHtml = row.code;
                                                txtSpecialProcedureAmount.Value = row.tariff.ToString(Constant.FormatString.NUMERIC_2);
                                                totalAmount += row.tariff;
                                            }
                                            else if (row.type == "Special Investigation")
                                            {
                                                cboSpecialInvestigation.Value = row.code;
                                                lblSpecialInvestigation.InnerHtml = row.code;
                                                txtSpecialInvestigationAmount.Value = row.tariff.ToString(Constant.FormatString.NUMERIC_2);
                                                totalAmount += row.tariff;
                                            }
                                            else if (row.type == "Special Drug")
                                            {
                                                cboSpecialDrug.Value = row.code;
                                                lblSpecialDrugCode.InnerHtml = row.code;
                                                txtSpecialDrugAmount.Value = row.tariff.ToString(Constant.FormatString.NUMERIC_2);
                                                totalAmount += row.tariff;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (respInfo.response.data.grouper.response.cbg != null)
                                {
                                    grouperDescription.InnerText = respInfo.response.data.grouper.response.cbg.description;
                                    grouperCode.InnerText = respInfo.response.data.grouper.response.cbg.code;
                                }
                            }
                        }

                        totalAmount = cbgTarif;
                        txtTotalAmountCBG.Value = totalAmount.ToString(Constant.FormatString.NUMERIC_2);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message.ToString();
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnEditClaimData(ref string errMessage, ref string respEklaim)
        {

            string filterExpression = string.Format("RegistrationID='{0}' AND GCTransactionStatus <> '{1}' AND GCPaymentType='{2}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER);
            List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(filterExpression);
            if (lstPaymentHd.Count > 0)
            {
                string paymentID = "";
                foreach (PatientPaymentHd row in lstPaymentHd)
                {
                    paymentID += string.Format("{0},", row.PaymentID);
                }


                if (!string.IsNullOrEmpty(paymentID))
                {
                    paymentID = paymentID.Remove(paymentID.Length - 1);
                    string paymentDtID = "";
                    string filterExpressionDt = string.Format("PaymentID IN ({0}) AND BusinessPartnerID IN(SELECT BusinessPartnerID FROM Customer where GCCustomerType='{1}' AND  BusinessPartnerID=  PatientPaymentDt.BusinessPartnerID) AND  IsDeleted=0", paymentID, Constant.CustomerType.BPJS);
                    List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(filterExpressionDt);
                    if (lstPaymentDt.Count > 0)
                    {

                        foreach (PatientPaymentDt rowDt in lstPaymentDt)
                        {
                            paymentDtID += string.Format("{0},", rowDt.PaymentID);
                        }

                        paymentDtID = paymentDtID.Remove(paymentDtID.Length - 1);
                        List<PatientPaymentDtInfo> lstPaymentDtInfo = BusinessLayer.GetPatientPaymentDtInfoList(string.Format("PaymentDetailID IN ('{0}') ", paymentDtID));

                        List<PatientPaymentDtInfo> lstPaymentDtInfo1 = lstPaymentDtInfo.Where(p => p.GCFinalStatus == Constant.FinalStatus.APPROVED).ToList();
                        if (lstPaymentDtInfo1.Count > 1)
                        {
                            errMessage = "Maaf klaim tidak bisa dihapus karena sudah ada pengakuan hutang";
                            return false;
                        }
                    }
                }
            }
            if (OnEditUlangClaimData(ref errMessage, ref respEklaim))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool OnEditUlangClaimData(ref string errMessage, ref string respEklaim)
        {

            try
            {
                ReeditClaimMethod ClaimFinal = new ReeditClaimMethod()
                {
                    metadata = new ReeditClaimMetadata()
                    {
                        method = "reedit_claim",
                    },
                    data = new ReeditClaimData()
                    {
                        nomor_sep = txtNoSEP.Text,
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(ClaimFinal);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.ReeditClaim(jsonRequest);
                respEklaim = response.ToString();
                return true;
            }
            catch (Exception er)
            {
                errMessage = er.Message.ToString();
                return false;
            }

        }

        private bool OnSendClaimIndividualData(ref string errMessage, ref string respEklaim)
        {
            bool result = true;

            try
            {
                SendClaimIndividualMethod ClaimFinal = new SendClaimIndividualMethod()
                {
                    metadata = new SendClaimIndividualMetadata()
                    {
                        method = "send_claim_individual",
                    },
                    data = new SendClaimIndividualData()
                    {
                        nomor_sep = txtNoSEP.Text,
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(ClaimFinal);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.SendClaimIndividual(jsonRequest);
                respEklaim = response.ToString();
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;

        }

        //sementara untuk update payment masih blm fix
        private bool OnDeletedClaimProcess(ref string errMessage, ref string respEklaim)
        {
            bool result = false;
            try
            {
                string filterExpression = string.Format("RegistrationID='{0}' AND GCTransactionStatus <> '{1}' AND GCPaymentType='{2}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER);
                List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(filterExpression);
                if (lstPaymentHd.Count > 0)
                {
                    string paymentID = "";
                    foreach (PatientPaymentHd row in lstPaymentHd)
                    {
                        paymentID += string.Format("{0},", row.PaymentID);
                    }


                    if (!string.IsNullOrEmpty(paymentID))
                    {
                        paymentID = paymentID.Remove(paymentID.Length - 1);
                        string paymentDtID = "";
                        string filterExpressionDt = string.Format("PaymentID IN ({0}) AND BusinessPartnerID IN(SELECT BusinessPartnerID FROM Customer where GCCustomerType='{1}' AND  BusinessPartnerID=  PatientPaymentDt.BusinessPartnerID) AND  IsDeleted=0", paymentID, Constant.CustomerType.BPJS);
                        List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(filterExpressionDt);
                        if (lstPaymentDt.Count > 0)
                        {

                            foreach (PatientPaymentDt rowDt in lstPaymentDt)
                            {
                                paymentDtID += string.Format("{0},", rowDt.PaymentID);
                            }

                            paymentDtID = paymentDtID.Remove(paymentDtID.Length - 1);
                            List<PatientPaymentDtInfo> lstPaymentDtInfo = BusinessLayer.GetPatientPaymentDtInfoList(string.Format("PaymentDetailID IN ('{0}') ", paymentDtID));

                            List<PatientPaymentDtInfo> lstPaymentDtInfo1 = lstPaymentDtInfo.Where(p => p.GCFinalStatus == Constant.FinalStatus.APPROVED).ToList();
                            if (lstPaymentDtInfo1.Count > 1)
                            {
                                errMessage = "Maaf klaim tidak bisa dihapus karena sudah ada pengakuan hutang";
                                result = false;
                            }
                            else
                            {

                                if (OnDeleteClaim(ref   errMessage, ref   respEklaim))
                                {
                                    string respData = respEklaim;
                                    DeleteClaimResponse oResponse = JsonConvert.DeserializeObject<DeleteClaimResponse>(respData);
                                    if (oResponse.metadata.code == "200")
                                    {
                                        foreach (PatientPaymentDtInfo row in lstPaymentDtInfo)
                                        {
                                            row.GrouperCodeClaim = null;
                                            row.GrouperAmountClaim = 0;
                                            row.ClaimBy = null;
                                            row.GCClaimStatus = Constant.ClaimStatus.OPEN;
                                            BusinessLayer.UpdatePatientPaymentDtInfo(row);

                                        }

                                        //update registration Bpjs 
                                        string filterBpjs = string.Format("RegistrationID='{0}'", hdnRegistrationID.Value);
                                        RegistrationBPJS oBpjs = BusinessLayer.GetRegistrationBPJSList(filterBpjs).FirstOrDefault();
                                        if (oBpjs != null)
                                        {
                                            oBpjs.GrouperAmountClaim = 0;
                                            oBpjs.GrouperCode = null;
                                            oBpjs.GrouperCodeClaim = null;
                                            oBpjs.GCClaimStatus = Constant.ClaimStatus.OPEN;
                                            oBpjs.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            oBpjs.LastUpdatedDate = DateTime.Now;
                                            oBpjs.SpecialDrugType = null;
                                            oBpjs.SpecialDrugCode = null;
                                            oBpjs.SpecialDrugAmount = 0;
                                            oBpjs.SpecialInvestigationCode = null;
                                            oBpjs.SpecialInvestigationType = null;
                                            oBpjs.SpecialInvestigationAmount = 0;
                                            oBpjs.SpecialProcedureType = null;
                                            oBpjs.SpecialProcedureCode = null;
                                            oBpjs.SpecialProcedureAmount = 0;
                                            oBpjs.SpecialProsthesisCode = null;
                                            oBpjs.SpecialProsthesisType = null;
                                            oBpjs.SpecialProsthesisAmount = 0;
                                            oBpjs.SubAcuteAmount = 0;
                                            oBpjs.SubAcuteCode = null;
                                            oBpjs.SubAcuteType = null;
                                            oBpjs.ChronicType = null;
                                            oBpjs.ChronicCode = null;
                                            oBpjs.ChronicAmount = 0;

                                            oBpjs.GrouperTypeClaim = oBpjs.GrouperTypeFinal = null;
                                            oBpjs.GrouperCodeClaim = oBpjs.GrouperCodeFinal = null;
                                            oBpjs.GrouperAmountClaim = oBpjs.GrouperAmountFinal = 0;
                                            BusinessLayer.UpdateRegistrationBPJS(oBpjs);
                                        }
                                        result = true;
                                        respEklaim = respData;
                                    }
                                    else
                                    {
                                        respEklaim = respData;

                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        if (OnDeleteClaim(ref   errMessage, ref   respEklaim))
                        {
                            string respData = respEklaim;
                            DeleteClaimResponse oResponse = JsonConvert.DeserializeObject<DeleteClaimResponse>(respData);
                            if (oResponse.metadata.code == "200")
                            {
                                //update registration Bpjs 
                                string filterBpjs = string.Format("RegistrationID='{0}'", hdnRegistrationID.Value);
                                RegistrationBPJS oBpjs = BusinessLayer.GetRegistrationBPJSList(filterBpjs).FirstOrDefault();
                                if (oBpjs != null)
                                {
                                    oBpjs.GrouperAmountClaim = 0;
                                    oBpjs.GrouperCode = null;
                                    oBpjs.GrouperCodeClaim = null;
                                    oBpjs.GCClaimStatus = Constant.ClaimStatus.OPEN;
                                    oBpjs.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oBpjs.LastUpdatedDate = DateTime.Now;
                                    oBpjs.SpecialDrugType = null;
                                    oBpjs.SpecialDrugCode = null;
                                    oBpjs.SpecialDrugAmount = 0;
                                    oBpjs.SpecialInvestigationCode = null;
                                    oBpjs.SpecialInvestigationType = null;
                                    oBpjs.SpecialInvestigationAmount = 0;
                                    oBpjs.SpecialProcedureType = null;
                                    oBpjs.SpecialProcedureCode = null;
                                    oBpjs.SpecialProcedureAmount = 0;
                                    oBpjs.SpecialProsthesisCode = null;
                                    oBpjs.SpecialProsthesisType = null;
                                    oBpjs.SpecialProsthesisAmount = 0;
                                    oBpjs.SubAcuteAmount = 0;
                                    oBpjs.SubAcuteCode = null;
                                    oBpjs.SubAcuteType = null;
                                    oBpjs.ChronicType = null;
                                    oBpjs.ChronicCode = null;
                                    oBpjs.ChronicAmount = 0;

                                    oBpjs.GrouperTypeClaim = oBpjs.GrouperTypeFinal = null;
                                    oBpjs.GrouperCodeClaim = oBpjs.GrouperCodeFinal = null;
                                    oBpjs.GrouperAmountClaim = oBpjs.GrouperAmountFinal = 0;
                                    BusinessLayer.UpdateRegistrationBPJS(oBpjs);
                                }
                            }
                            respEklaim = respData;
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }

            return result;

        }

        private bool OnDeleteClaim(ref string errMessage, ref string respEklaim)
        {
            try
            {
                DeleteClaimMethod Claim = new DeleteClaimMethod()
                {
                    metadata = new DeleteClaimMetadata()
                    {
                        method = "delete_claim",
                    },
                    data = new DeleteClaimData()
                    {
                        coder_nik = hdnUsernameLogin.Value,
                        nomor_sep = txtNoSEP.Text,
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(Claim);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.DeleteClaim(jsonRequest);
                respEklaim = response.ToString();
                return true;
            }
            catch (Exception er)
            {
                errMessage = er.Message.ToString();
                return false;
            }
        }

        private bool OnUpdatePrintNumber(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDao = new RegistrationBPJSDao(ctx);
            try
            {
                RegistrationBPJS entity = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                entity.GCSEPStatus = Constant.SEP_Status.DICETAK;
                entity.PrintNumber += 1;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                ctx.CommitTransaction();
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

        private bool OnValidateSITB(ref string errMessage, ref string respEklaim)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao oRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            PatientDao oPatientDao = new PatientDao(ctx);

            try
            {
                SITBValidateMethod validate = new SITBValidateMethod()
                {
                    metadata = new SITBValidateMetadata()
                    {
                        method = "sitb_validate",
                    },
                    data = new SITBValidateData()
                    {
                        nomor_sep = txtNoSEP.Text,
                        nomor_register_sitb = txtNoPasienTB.Text
                    }
                };

                string jsonRequest = JsonConvert.SerializeObject(validate);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.SITBValidate(jsonRequest);
                respEklaim = response.ToString();

                if (!string.IsNullOrEmpty(response))
                {
                    SITBValidateResponse respInfo = JsonConvert.DeserializeObject<SITBValidateResponse>(response);
                    if (respInfo.response.status == "VALID")
                    {
                        RegistrationBPJS oRegBpjs = oRegistrationBPJSDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        if (oRegBpjs != null)
                        {
                            oRegBpjs.EKlaimIsValidateSITB = true;
                            oRegBpjs.EKlaimNoRegisterSITB = respInfo.response.validation.data.FirstOrDefault().id;
                            oRegBpjs.EKlaimNamaPasienSITB = respInfo.response.validation.data.FirstOrDefault().nama;
                            oRegBpjs.EKlaimNIKSITB = respInfo.response.validation.data.FirstOrDefault().nik;
                            oRegBpjs.EKlaimJenisKelaminIDSITB = respInfo.response.validation.data.FirstOrDefault().jenis_kelamin_id;
                            oRegBpjs.EKlaimLastValidateSITBBy = AppSession.UserLogin.UserID;
                            oRegBpjs.EKlaimLastValidateSITBDatetime = DateTime.Now;
                            oRegistrationBPJSDao.Update(oRegBpjs);
                        }

                        if (oRegBpjs.EKlaimNoRegisterSITB != null && oRegBpjs.EKlaimNoRegisterSITB != "")
                        {
                            Patient oPatient = oPatientDao.Get(AppSession.RegisteredPatient.MRN);
                            oPatient.SITBRegisterNo = oRegBpjs.EKlaimNoRegisterSITB;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oPatientDao.Update(oPatient);
                        }
                    }
                    else
                    {
                        result = false;
                        respEklaim = errMessage = respInfo.response.status + " || " + respInfo.response.detail;
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
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private bool OnInvalidateSITB(ref string errMessage, ref string respEklaim)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao oRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            PatientDao oPatientDao = new PatientDao(ctx);

            try
            {
                SITBInvalidateMethod validate = new SITBInvalidateMethod()
                {
                    metadata = new SITBInvalidateMetadata()
                    {
                        method = "sitb_invalidate",
                    },
                    data = new SITBInvalidateData()
                    {
                        nomor_sep = txtNoSEP.Text
                    }
                };

                string jsonRequest = JsonConvert.SerializeObject(validate);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.SITBInvalidate(jsonRequest);
                respEklaim = response.ToString();

                if (!string.IsNullOrEmpty(response))
                {
                    SITBInvalidateResponse respInfo = JsonConvert.DeserializeObject<SITBInvalidateResponse>(response);
                    if (respInfo.metadata.code == "200")
                    {
                        RegistrationBPJS oRegBpjs = oRegistrationBPJSDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        if (oRegBpjs != null)
                        {
                            oRegBpjs.EKlaimIsValidateSITB = false;
                            oRegBpjs.EKlaimLastInvalidateSITBBy = AppSession.UserLogin.UserID;
                            oRegBpjs.EKlaimLastInvalidateSITBDatetime = DateTime.Now;
                            oRegistrationBPJSDao.Update(oRegBpjs);
                        }

                        if (oRegBpjs.EKlaimNoRegisterSITB != null && oRegBpjs.EKlaimNoRegisterSITB != "")
                        {
                            Patient oPatient = oPatientDao.Get(AppSession.RegisteredPatient.MRN);
                            oPatient.SITBRegisterNo = null;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oPatientDao.Update(oPatient);
                        }
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        public void bindData(ref string errMessage, ref string respEklaim)
        {
            try
            {
                vRegistrationBPJS1 oData = BusinessLayer.GetvRegistrationBPJS1List(string.Format("RegistrationID='{0}'", hdnRegistrationID.Value)).FirstOrDefault();
                if (oData != null)
                {
                    List<special_cmg> lstspecial_cmg_option = new List<special_cmg>();
                    if (!string.IsNullOrEmpty(oData.SpecialProsthesisCode))
                    {

                        special_cmg oScmg = new special_cmg()
                        {
                            code = oData.SpecialProsthesisCode,
                            description = oData.SpecialProsthesisType,
                            tariff = Convert.ToDecimal(oData.SpecialProsthesisAmount),
                            type = "Special Prosthesis"
                        };
                        lstspecial_cmg_option.Add(oScmg);
                        lblSpecialProsthesis.InnerHtml = oData.SpecialProsthesisCode;
                        txtSpecialProsthesisAmount.Value = oData.SpecialProsthesisAmount.ToString(Constant.FormatString.NUMERIC_2);
                    }
                    if (!string.IsNullOrEmpty(oData.SpecialProcedureCode))
                    {

                        special_cmg oScmg = new special_cmg()
                        {
                            code = oData.SpecialProcedureCode,
                            description = oData.SpecialProcedureType,
                            tariff = Convert.ToDecimal(oData.SpecialProcedureAmount),
                            type = "Special Procedure"
                        };
                        lstspecial_cmg_option.Add(oScmg);
                        lblSpecialProcedure.InnerHtml = oData.SpecialProcedureCode;
                        txtSpecialProcedureAmount.Value = oData.SpecialProcedureAmount.ToString(Constant.FormatString.NUMERIC_2);
                    }
                    if (!string.IsNullOrEmpty(oData.SpecialInvestigationCode))
                    {
                        special_cmg oScmg = new special_cmg()
                        {
                            code = oData.SpecialInvestigationCode,
                            description = oData.SpecialInvestigationType,
                            tariff = Convert.ToDecimal(oData.SpecialInvestigationAmount),
                            type = "Special Investigation"
                        };
                        lstspecial_cmg_option.Add(oScmg);
                        lblSpecialInvestigation.InnerText = oData.SpecialInvestigationType;
                        txtSpecialInvestigationAmount.Value = oData.SpecialInvestigationAmount.ToString(Constant.FormatString.NUMERIC_2);
                    }
                    if (!string.IsNullOrEmpty(oData.SpecialDrugCode))
                    {

                        special_cmg oScmg = new special_cmg()
                        {
                            code = oData.SpecialDrugCode,
                            description = oData.SpecialDrugType,
                            tariff = Convert.ToDecimal(oData.SpecialDrugAmount),
                            type = "Special Drug"
                        };
                        lstspecial_cmg_option.Add(oScmg);
                        lblSpecialDrugCode.InnerHtml = oData.SpecialDrugCode;
                        txtSpecialDrugAmount.Value = oData.SpecialDrugAmount.ToString(Constant.FormatString.NUMERIC_2);
                    }

                    Methods.SetComboBoxField<special_cmg>(cboSpecialProsthesis, lstspecial_cmg_option.Where(p => p.type == "Special Prosthesis").ToList(), "description", "code");
                    Methods.SetComboBoxField<special_cmg>(cboSpecialProcedure, lstspecial_cmg_option.Where(p => p.type == "Special Procedure").ToList(), "description", "code");
                    Methods.SetComboBoxField<special_cmg>(cboSpecialInvestigation, lstspecial_cmg_option.Where(p => p.type == "Special Investigation").ToList(), "description", "code");
                    Methods.SetComboBoxField<special_cmg>(cboSpecialDrug, lstspecial_cmg_option.Where(p => p.type == "Special Drug").ToList(), "description", "code");
                    cboSpecialDrug.SelectedIndex = 0;
                    cboSpecialProcedure.SelectedIndex = 0;
                    cboSpecialInvestigation.SelectedIndex = 0;
                    cboSpecialDrug.SelectedIndex = 0;

                    //if (oData.ClaimDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    //{
                    //    txtLastGrouperDate.Text = "-";
                    //}
                    //else
                    //{
                    //    txtLastGrouperDate.Text = oData.ClaimDate.ToString(Constant.FormatString.DATE_TIME_FORMAT_2);
                    //}

                    //txtClaimby.Text = oData.ClaimByName;


                    #region Binding Grouper

                    JenisRawat.InnerText = txtServiceUnitType.Text;

                    grouperDescription.InnerText = oData.GrouperTypeClaim;
                    grouperCode.InnerHtml = oData.GrouperCodeClaim;

                    decimal SpecialDrugAmount = oData.SpecialDrugAmount;
                    decimal SpecialProcedureAmount = oData.SpecialProcedureAmount;
                    decimal SpecialProsthesisAmount = oData.SpecialProsthesisAmount;
                    decimal SpecialInvestigationAmount = oData.SpecialInvestigationAmount;

                    decimal totalFinal = oData.GrouperAmountClaim - (SpecialDrugAmount + SpecialProcedureAmount + SpecialProsthesisAmount + SpecialInvestigationAmount);
                    grouperValue1.Value = totalFinal.ToString(Constant.FormatString.NUMERIC_2);
                    txtTotalAmountCBG.Value = oData.GrouperAmountClaim.ToString(Constant.FormatString.NUMERIC_2);

                    if (oData.GrouperCodeClaim != null && oData.GrouperCodeClaim != "" && oData.GrouperAmountClaim != 0)
                    {
                        divGrouper.Style.Add("background", "#c6ff9b");
                    }
                    else
                    {
                        divGrouper.Style.Add("background", "#ffffff");
                    }
                    if (OnGetKlaimDetail(ref errMessage, ref respEklaim)) { }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
        }

        #endregion

        private string changeFormatDate(string Date)
        {
            string[] data = Date.Split('-');
            string dateNow = string.Empty;
            dateNow = string.Format("{0}-{1}-{2}", data[2], data[1], data[0]);
            return dateNow;
        }

        private void UpdateStatusProcess()
        {
            GetClaimMethod getClaimDt = new GetClaimMethod()
            {
                metadata = new GetClaimMetadata()
                {
                    method = "get_claim_data"
                },
                data = new GetClaimData()
                {
                    nomor_sep = txtNoSEP.Text
                }
            };

            string jsonRequest1 = JsonConvert.SerializeObject(getClaimDt);
            EKlaimService eklaimService = new EKlaimService();
            string responseData = eklaimService.GetClaim(jsonRequest1);
            GetClaimResponse respInfo = JsonConvert.DeserializeObject<GetClaimResponse>(responseData);
            if (respInfo.metadata.code == "200")
            {
                if (respInfo.response.data.grouper != null)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    RegistrationBPJSDao oRegBpjsDao = new RegistrationBPJSDao(ctx);
                    RegistrationClaimHistoryDao regClaimHistoryDao = new RegistrationClaimHistoryDao(ctx);
                    PatientPaymentDtInfoDao patientPaymentDtInfoDao = new PatientPaymentDtInfoDao(ctx);

                    try
                    {
                        RegistrationBPJS oData = BusinessLayer.GetRegistrationBPJSList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                        if (oData != null)
                        {
                            decimal cbgTarif = Convert.ToDecimal(respInfo.response.data.grouper.response.cbg.tariff);
                            string cbgCode = string.Format("{0}", respInfo.response.data.grouper.response.cbg.code);
                            string cbgDescription = respInfo.response.data.grouper.response.cbg.description;
                            decimal totalCoverageAmount = cbgTarif;
                            if (respInfo.response.data.grouper.response.special_cmg != null)
                            {
                                if (respInfo.response.data.grouper.response.special_cmg.Count > 0)
                                {
                                    List<special_cmg> lstspecial_cmg_option = respInfo.response.data.grouper.response.special_cmg;
                                    foreach (special_cmg row in lstspecial_cmg_option)
                                    {
                                        if (row.type == "Special Prosthesis")
                                        {
                                            oData.SpecialProsthesisType = row.description;
                                            oData.SpecialProsthesisCode = row.code;
                                            oData.SpecialProsthesisAmount = row.tariff;
                                        }
                                        else if (row.type == "Special Procedure")
                                        {
                                            oData.SpecialProcedureCode = row.code;
                                            oData.SpecialProcedureType = row.description;
                                            oData.SpecialProcedureAmount = row.tariff;
                                        }
                                        else if (row.type == "Special Investigation")
                                        {
                                            oData.SpecialInvestigationCode = row.code;
                                            oData.SpecialInvestigationType = row.description;
                                            oData.SpecialInvestigationAmount = row.tariff;
                                        }
                                        else if (row.type == "Special Drug")
                                        {
                                            oData.SpecialDrugCode = row.code;
                                            oData.SpecialDrugType = row.description;
                                            oData.SpecialDrugAmount = row.tariff;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                oData.SpecialProsthesisType = null;
                                oData.SpecialProsthesisCode = null;
                                oData.SpecialProsthesisAmount = 0;

                                oData.SpecialProcedureCode = null;
                                oData.SpecialProcedureType = null;
                                oData.SpecialProcedureAmount = 0;

                                oData.SpecialInvestigationCode = null;
                                oData.SpecialInvestigationType = null;
                                oData.SpecialInvestigationAmount = 0;

                                oData.SpecialDrugCode = null;
                                oData.SpecialDrugType = null;
                                oData.SpecialDrugAmount = 0;

                                oData.GrouperTypeClaim = oData.GrouperTypeFinal = null;
                                oData.GrouperCodeClaim = oData.GrouperCodeFinal = null;
                                oData.GrouperAmountClaim = oData.GrouperAmountFinal = 0;
                            }

                            oData.GrouperTypeClaim = oData.GrouperTypeFinal = cbgDescription;
                            oData.GrouperCodeClaim = oData.GrouperCodeFinal = cbgCode;
                            oData.GrouperAmountClaim = oData.GrouperAmountFinal = cbgTarif;


                            oData.INAHakPasien = Convert.ToDecimal(totalCoverageAmount);

                            string grouperCodeAll = string.Format("{0}|{1}|{2}|{3}|{4}", oData.GrouperCodeClaim, oData.SpecialProcedureCode, oData.SpecialProsthesisCode, oData.SpecialInvestigationCode, oData.SpecialDrugCode);

                            decimal PatientAmount = Convert.ToDecimal(txtTotalAmount.Text) - totalCoverageAmount;

                            oData.RealCostAmount = Convert.ToDecimal(txtTotalAmount.Text);
                            oData.INADitempati = oData.OccupiedAmount = Convert.ToDecimal(txtTotalAmount.Text);
                            oData.PatientAmount = Convert.ToDecimal("0");
                            oData.DifferenceAmount = Convert.ToDecimal(oData.RealCostAmount - totalCoverageAmount);
                            oData.LastUpdatedBy = AppSession.UserLogin.UserID;

                            RegistrationClaimHistory oregHistory = new RegistrationClaimHistory();
                            oregHistory.CodingBy = AppSession.UserLogin.UserID;
                            oregHistory.CodingDate = DateTime.Now;
                            oregHistory.PatientAmount = 0;
                            oregHistory.DifferenceAmount = oData.RealCostAmount - totalCoverageAmount;
                            oregHistory.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                            oregHistory.CreatedBy = AppSession.UserLogin.UserID;
                            regClaimHistoryDao.Insert(oregHistory);

                            string oIsFinalisasiKlaimAfterARInvoiceCtlAR = AppSession.IsClaimFinalAfterARInvoice ? "1" : "0";
                            string oIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR = AppSession.IsClaimFinalBeforeARInvoiceAndSkipProcessClaim ? "1" : "0";

                            if (oIsFinalisasiKlaimAfterARInvoiceCtlAR == "0" && oIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR == "1")
                            {
                                oData.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                oData.ClaimBy = AppSession.UserLogin.UserID;
                                oData.ClaimDate = DateTime.Now;
                                oData.GCFinalStatus = Constant.ClaimStatus.APPROVED;
                                oData.FinalBy = AppSession.UserLogin.UserID;
                                oData.FinalDate = DateTime.Now;
                            }
                            else if (oIsFinalisasiKlaimAfterARInvoiceCtlAR == "1" && oIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR == "1")
                            {
                                oData.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                oData.ClaimBy = AppSession.UserLogin.UserID;
                                oData.ClaimDate = DateTime.Now;
                            }
                            else
                            {
                                oData.GCClaimStatus = Constant.ClaimStatus.OPEN;
                                oData.ClaimBy = AppSession.UserLogin.UserID;
                                oData.ClaimDate = DateTime.Now;
                            }
                            oData.CodingBy = AppSession.UserLogin.UserID;
                            oData.CodingDate = DateTime.Now;
                            oRegBpjsDao.Update(oData);

                            #region PatientPayment

                            string filterExpression = string.Format("RegistrationID='{0}' AND GCTransactionStatus <> '{1}' AND GCPaymentType='{2}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER);
                            List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(filterExpression, ctx);
                            if (lstPaymentHd.Count > 0)
                            {
                                string paymentID = "";
                                foreach (PatientPaymentHd row in lstPaymentHd)
                                {
                                    paymentID += string.Format("{0},", row.PaymentID);
                                }

                                if (!string.IsNullOrEmpty(paymentID))
                                {
                                    paymentID = paymentID.Remove(paymentID.Length - 1);
                                    string paymentDtID = "";
                                    string filterExpressionDt = string.Format("PaymentID IN ({0}) AND BusinessPartnerID IN(SELECT BusinessPartnerID FROM Customer where GCCustomerType='{1}' AND  BusinessPartnerID=  PatientPaymentDt.BusinessPartnerID) AND  IsDeleted=0", paymentID, Constant.CustomerType.BPJS);
                                    List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(filterExpressionDt, ctx);
                                    if (lstPaymentDt.Count > 0)
                                    {
                                        foreach (PatientPaymentDt rowDt in lstPaymentDt)
                                        {
                                            paymentDtID += string.Format("{0},", rowDt.PaymentDetailID);
                                        }

                                        paymentDtID = paymentDtID.Remove(paymentDtID.Length - 1);
                                        List<PatientPaymentDtInfo> lstPaymentDtInfo = BusinessLayer.GetPatientPaymentDtInfoList(string.Format("PaymentDetailID IN ('{0}') ", paymentDtID), ctx);
                                        if (lstPaymentDtInfo.Count > 0)
                                        {
                                            foreach (PatientPaymentDtInfo row in lstPaymentDtInfo)
                                            {
                                                if (row.GCFinalStatus == Constant.FinalStatus.OPEN || string.IsNullOrEmpty(row.GCFinalStatus))
                                                {
                                                    if (oIsFinalisasiKlaimAfterARInvoiceCtlAR == "0" && oIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR == "1")
                                                    {
                                                        row.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                        row.ClaimBy = AppSession.UserLogin.UserID;
                                                        row.ClaimDate = DateTime.Now;

                                                        row.GCFinalStatus = Constant.FinalStatus.OPEN;
                                                        row.FinalBy = AppSession.UserLogin.UserID;
                                                        row.FinalDate = DateTime.Now;

                                                        row.GrouperCodeClaim = grouperCodeAll;
                                                        row.GrouperAmountClaim = totalCoverageAmount;
                                                        row.GrouperCodeFinal = grouperCodeAll;
                                                        row.GrouperAmountFinal = totalCoverageAmount;
                                                    }
                                                    else if (oIsFinalisasiKlaimAfterARInvoiceCtlAR == "1" && oIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR == "1")
                                                    {
                                                        row.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                        row.ClaimBy = AppSession.UserLogin.UserID;
                                                        row.ClaimDate = DateTime.Now;

                                                        row.GrouperCodeClaim = grouperCodeAll;
                                                        row.GrouperAmountClaim = totalCoverageAmount;
                                                        row.GrouperCodeFinal = grouperCodeAll;
                                                        row.GrouperAmountFinal = totalCoverageAmount;
                                                    }
                                                    else
                                                    {
                                                        row.GCClaimStatus = Constant.ClaimStatus.OPEN;
                                                        row.ClaimBy = AppSession.UserLogin.UserID;
                                                        row.ClaimDate = DateTime.Now;

                                                        row.GrouperCodeClaim = grouperCodeAll;
                                                        row.GrouperAmountClaim = totalCoverageAmount;
                                                    }

                                                    patientPaymentDtInfoDao.Update(row);
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            #endregion

                            ctx.CommitTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                    }
                }
            }
        }

        private bool OnUpdateStatusLogBPJS(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDao = new RegistrationBPJSDao(ctx);
            try
            {
                RegistrationBPJS entity = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                if (type == "final")
                {
                    entity.EKlaimFinalBy = AppSession.UserLogin.UserID;
                    entity.EKlaimFinalDateTime = DateTime.Now;
                }
                else if (type == "reopen")
                {
                    entity.EKlaimReopenBy = AppSession.UserLogin.UserID;
                    entity.EKlaimReopenDateTime = DateTime.Now;
                }
                else if (type == "edit")
                {
                    entity.EKlaimEditBy = AppSession.UserLogin.UserID;
                    entity.EKlaimEditDateTime = DateTime.Now;
                }
                else if (type == "delete")
                {
                    entity.EKlaimDeleteBy = AppSession.UserLogin.UserID;
                    entity.EKlaimDeleteDateTime = DateTime.Now;
                }
                else if (type == "sendclaim")
                {
                    entity.ClaimBy = AppSession.UserLogin.UserID;
                    entity.ClaimDate = DateTime.Now;
                }
                else if (type == "grouper1")
                {
                    entity.GrouperStage1By = AppSession.UserLogin.UserID;
                    entity.GrouperStage1DateTime = DateTime.Now;
                }
                else if (type == "grouper2")
                {
                    entity.GrouperStage2By = AppSession.UserLogin.UserID;
                    entity.GrouperStage2DateTime = DateTime.Now;
                }
                else if (type == "sendonline")
                {
                    entity.EKlaimSendOnlineBy = AppSession.UserLogin.UserID;
                    entity.EKlaimSendOnlineDateTime = DateTime.Now;
                }

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                entityDao.Update(entity);
                ctx.CommitTransaction();
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