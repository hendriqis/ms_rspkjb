using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web;
using System.IO;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GFRCalculatorCtl1 : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnTransactionID.Value = param;
            GetGFRLaboratoriumData obj = BusinessLayer.GetGFRLaboratoriumData(Convert.ToInt32(param)).FirstOrDefault();
            SetEntityToControls(obj);
            Double par = Math.Pow(Convert.ToDouble(0.9938), Convert.ToDouble(obj.cfPatientAgeInYear));
            hdnPar.Value = par.ToString();
            GetSettingParameter();
        }

        private void SetEntityToControls(GetGFRLaboratoriumData entity)
        {
            #region Patient Information
            txtTransactionNo.Text = entity.TransactionNo;
            txtItemName.Text = entity.ItemName1;
            txtSex.Text = entity.Gender;
            txtAge.Text = entity.cfPatientAgeInYear;

            hdnItemID.Value = entity.ItemID.ToString();
            hdnReferenceDtID.Value = entity.ReferenceDtID.ToString();

            lblPatientName.InnerHtml = entity.cfPatientNameSalutation;
            lblMRN.InnerHtml = entity.MedicalNo;
            imgPatientProfilePicture.Src = HttpUtility.HtmlEncode(entity.cfPatientImageUrl);

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

            words = ((BasePage)Page).GetWords();
            if (entity.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                lblDOB.InnerHtml = entity.cfDateOfBirthInString;
                lblPatientAge.InnerHtml = string.Format("{0} - {1}", Helper.GetPatientAge(words, entity.DateOfBirth), entity.Religion);
            }
            else
            {
                lblDOB.InnerHtml = "";
                lblPatientAge.InnerHtml = string.Format("{0} - {1}", "", entity.Religion);
            }

            lblGender.InnerHtml = HttpUtility.HtmlEncode(entity.Gender);
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

            if (entity.PatientAllergy.Length > 20)
                lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.PatientAllergy.Substring(0, 20).ToUpper()) + "...";
            else
                lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.PatientAllergy.ToUpper());

            lblReferralNo.InnerText = "-";
            if (entity.ReferralNo != null && entity.ReferralNo != "")
            {
                lblReferralNo.InnerText = entity.ReferralNo;
            }

            lblPayer.InnerHtml = HttpUtility.HtmlEncode(entity.cfBusinessPartner);
            #endregion
            #endregion

            #region parameter
            txtParameter1.Text = entity.ResultValueKreatininDarah.ToString();
            if (entity.GCGender == Constant.Gender.MALE)
            {
                txtParameter2.Text = "0.9";
                hdnEndFct.Value = "1.000";
            }
            else if (entity.GCGender == Constant.Gender.FEMALE)
            {
                txtParameter2.Text = "0.7";
                hdnEndFct.Value = "1.012";
            }

            if (entity.GCGender == Constant.Gender.MALE)
            {
                txtParameter3.Text = "-0.302";
            }
            else if (entity.GCGender == Constant.Gender.FEMALE)
            {
                txtParameter3.Text = "-0.241";
            }

            Decimal param4 = Convert.ToDecimal(entity.ResultValueKreatininDarah) / Convert.ToDecimal(txtParameter2.Text);
            if (param4 < 1)
            {
                param4 = 1;
            }

            if (param4 != 1)
            {
                txtParameter4.Text = param4.ToString("N4");
            }
            else
            {
                txtParameter4.Text = param4.ToString();
            }

            Decimal param5 = Convert.ToDecimal(entity.ResultValueKreatininDarah) / Convert.ToDecimal(txtParameter2.Text);
            if (param5 > 1)
            {
                param5 = 1;
            }

            if (param5 != 1)
            {
                txtParameter5.Text = param5.ToString("N4");
            }
            else
            {
                txtParameter5.Text = param5.ToString();
            }
            #endregion
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_ITEM_ID_KREATININ_SERUM, Constant.SettingParameter.LB_FRACTION_ID_KREATININ_SERUM_DARAH, Constant.SettingParameter.LB_FRACTION_ID_eGFR));

            hdnItemIDKreatininSerum.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.LB_ITEM_ID_KREATININ_SERUM).FirstOrDefault().ParameterValue;
            hdnFractionIDKreatininSerumDarah.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.LB_FRACTION_ID_KREATININ_SERUM_DARAH).FirstOrDefault().ParameterValue;
            hdnFractionIDeGFR.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.LB_FRACTION_ID_eGFR).FirstOrDefault().ParameterValue;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            LaboratoryResultHdDao labResultHdDao = new LaboratoryResultHdDao(ctx);
            LaboratoryResultDtDao labResultDtDao = new LaboratoryResultDtDao(ctx);
            try
            {
                #region check Fraction eGRF
                string filterExpression = string.Format("(RecursiveID = {0} OR ItemID = {0}) AND FractionID = {1} ORDER BY DisplayOrder", hdnItemID.Value, hdnFractionIDeGFR.Value);
                vRecursiveItemLaboratoryFraction itemFraction = BusinessLayer.GetvRecursiveItemLaboratoryFractionList(filterExpression).FirstOrDefault();
                #endregion

                if (itemFraction != null)
                {
                    PatientChargesHd chargesHd = chargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));

                    string filterChargesDt = string.Format("TransactionID = '{0}' AND GCTransactionDetailStatus != '{1}' AND IsDeleted = 0 AND ItemID = '{2}'", chargesHd.TransactionID, Constant.TransactionStatus.VOID, hdnItemIDKreatininSerum.Value);
                    PatientChargesDt chargesDt = BusinessLayer.GetPatientChargesDtList(filterChargesDt, ctx).FirstOrDefault();

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    string filterHd = string.Format("ChargeTransactionID = {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}'", hdnTransactionID.Value, Constant.TransactionStatus.VOID);
                    LaboratoryResultHd resultHd = BusinessLayer.GetLaboratoryResultHdList(filterHd, ctx).FirstOrDefault();
                    if (resultHd != null)
                    {
                        string filterDt = string.Format("ID = {0} AND IsDeleted = 0 AND FractionID = {1}", resultHd.ID, hdnFractionIDeGFR.Value);
                        LaboratoryResultDt resultDt = BusinessLayer.GetLaboratoryResultDtList(filterDt).FirstOrDefault();
                        if (resultDt != null)
                        {
                            resultDt.MetricResultValue = Convert.ToDecimal(txtResult.Text);
                            resultDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            labResultDtDao.Update(resultDt);
                        }
                        else
                        {
                            LaboratoryResultDt entityDt = new LaboratoryResultDt();
                            entityDt.ID = resultHd.ID;
                            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
                            entityDt.FractionID = Convert.ToInt32(hdnFractionIDeGFR.Value);

                            if (itemFraction.GCMetricUnit != null)
                            {
                                entityDt.GCMetricUnit = itemFraction.GCMetricUnit;
                            }
                            else
                            {
                                entityDt.GCMetricUnit = "";
                            }

                            if (!string.IsNullOrEmpty(itemFraction.MetricUnitMax))
                            {
                                entityDt.MaxMetricNormalValue = Convert.ToDecimal(itemFraction.MetricUnitMax);
                            }
                            else
                            {
                                entityDt.MaxMetricNormalValue = 0;
                            }

                            if (!String.IsNullOrEmpty(itemFraction.MetricUnitMin))
                            {
                                entityDt.MinMetricNormalValue = Convert.ToDecimal(itemFraction.MetricUnitMin);
                            }
                            else
                            {
                                entityDt.MinMetricNormalValue = 0;
                            }

                            //if (txtMetricValue.Text == "" || txtMetricValue.Text == null) txtMetricValue.Text = "0";

                            entityDt.MetricResultValue = Convert.ToDecimal(txtResult.Text);
                            //entityDt.TextValue = HttpUtility.HtmlDecode(hdnTextResult.Value);

                            #region International Value
                            decimal internationalValue = entityDt.MetricResultValue;
                            entityDt.ConversionFactor = itemFraction.ConversionFactor;

                            if (entityDt.ConversionFactor > 1)
                            {
                                internationalValue = entityDt.MetricResultValue * entityDt.ConversionFactor;
                            }
                            entityDt.InternationalResultValue = internationalValue;

                            if (!string.IsNullOrEmpty(itemFraction.GCInternationalUnit))
                            {
                                entityDt.GCInternationalUnit = itemFraction.GCInternationalUnit;
                            }
                            else
                            {
                                entityDt.GCInternationalUnit = "";
                            }

                            if (!string.IsNullOrEmpty(itemFraction.InternationalUnitMax))
                            {
                                entityDt.MaxInternationalNormalValue = Convert.ToDecimal(itemFraction.InternationalUnitMax);
                            }
                            else
                            {
                                entityDt.MaxInternationalNormalValue = 0;
                            }

                            if (!string.IsNullOrEmpty(itemFraction.InternationalUnitMin))
                            {
                                entityDt.MinInternationalNormalValue = Convert.ToDecimal(itemFraction.InternationalUnitMin);
                            }
                            else
                            {
                                entityDt.MinInternationalNormalValue = 0;
                            }
                            #endregion

                            //entityDt.ResultFlag = cboResultFlag.SelectedValue;
                            //if (entityDt.TextValue != "" && entityDt.TextValue != null)
                            //{
                            //    entityDt.IsNormal = cboResultFlag.SelectedValue == "N";
                            //}
                            //else
                            //{
                            //    if (cboResultFlag.SelectedValue == "N")
                            //        entityDt.IsNormal = true;
                            //    else
                            //        entityDt.IsNormal = false;
                            //}

                            entityDt.IsNormal = true;
                            entityDt.IsNumeric = itemFraction.IsNumericResult;
                            //entityDt.IsPendingResult = chkIsPendingResult.Checked;
                            //entityDt.Remarks = hdnRemarks.Value;
                            entityDt.ReferenceDtID = Convert.ToInt32(hdnReferenceDtID.Value);
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            labResultDtDao.Insert(entityDt);

                            chargesDt.IsHasTestResult = true;
                            chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            chargesDtDao.Update(chargesDt);
                        }
                    }
                    else
                    {
                        LaboratoryResultHd entityHd = new LaboratoryResultHd();
                        entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        entityHd.ResultDate = DateTime.Now;
                        entityHd.ResultTime = string.Format("{0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
                        entityHd.ChargeTransactionID = Convert.ToInt32(hdnTransactionID.Value);
                        entityHd.TestOrderID = chargesHd.TestOrderID;
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        int labResultID = labResultHdDao.InsertReturnPrimaryKeyID(entityHd);

                        LaboratoryResultDt entityDt = new LaboratoryResultDt();
                        entityDt.ID = labResultID;
                        entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
                        entityDt.FractionID = Convert.ToInt32(hdnFractionIDeGFR.Value);

                        if (itemFraction.GCMetricUnit != null)
                        {
                            entityDt.GCMetricUnit = itemFraction.GCMetricUnit;
                        }
                        else
                        {
                            entityDt.GCMetricUnit = "";
                        }

                        if (!string.IsNullOrEmpty(itemFraction.MetricUnitMax))
                        {
                            entityDt.MaxMetricNormalValue = Convert.ToDecimal(itemFraction.MetricUnitMax);
                        }
                        else
                        {
                            entityDt.MaxMetricNormalValue = 0;
                        }

                        if (!String.IsNullOrEmpty(itemFraction.MetricUnitMin))
                        {
                            entityDt.MinMetricNormalValue = Convert.ToDecimal(itemFraction.MetricUnitMin);
                        }
                        else
                        {
                            entityDt.MinMetricNormalValue = 0;
                        }

                        //if (txtMetricValue.Text == "" || txtMetricValue.Text == null) txtMetricValue.Text = "0";

                        entityDt.MetricResultValue = Convert.ToDecimal(txtResult.Text);
                        //entityDt.TextValue = HttpUtility.HtmlDecode(hdnTextResult.Value);

                        #region International Value
                        decimal internationalValue = entityDt.MetricResultValue;
                        entityDt.ConversionFactor = itemFraction.ConversionFactor;

                        if (entityDt.ConversionFactor > 1)
                        {
                            internationalValue = entityDt.MetricResultValue * entityDt.ConversionFactor;
                        }
                        entityDt.InternationalResultValue = internationalValue;

                        if (!string.IsNullOrEmpty(itemFraction.GCInternationalUnit))
                        {
                            entityDt.GCInternationalUnit = itemFraction.GCInternationalUnit;
                        }
                        else
                        {
                            entityDt.GCInternationalUnit = "";
                        }

                        if (!string.IsNullOrEmpty(itemFraction.InternationalUnitMax))
                        {
                            entityDt.MaxInternationalNormalValue = Convert.ToDecimal(itemFraction.InternationalUnitMax);
                        }
                        else
                        {
                            entityDt.MaxInternationalNormalValue = 0;
                        }

                        if (!string.IsNullOrEmpty(itemFraction.InternationalUnitMin))
                        {
                            entityDt.MinInternationalNormalValue = Convert.ToDecimal(itemFraction.InternationalUnitMin);
                        }
                        else
                        {
                            entityDt.MinInternationalNormalValue = 0;
                        }
                        #endregion

                        //entityDt.ResultFlag = cboResultFlag.SelectedValue;
                        //if (entityDt.TextValue != "" && entityDt.TextValue != null)
                        //{
                        //    entityDt.IsNormal = cboResultFlag.SelectedValue == "N";
                        //}
                        //else
                        //{
                        //    if (cboResultFlag.SelectedValue == "N")
                        //        entityDt.IsNormal = true;
                        //    else
                        //        entityDt.IsNormal = false;
                        //}

                        entityDt.IsNormal = true;
                        entityDt.IsNumeric = itemFraction.IsNumericResult;
                        //entityDt.IsPendingResult = chkIsPendingResult.Checked;
                        //entityDt.Remarks = hdnRemarks.Value;
                        entityDt.ReferenceDtID = Convert.ToInt32(hdnReferenceDtID.Value);
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        labResultDtDao.Insert(entityDt);

                        chargesDt.IsHasTestResult = true;
                        chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        chargesDtDao.Update(chargesDt);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Pemeriksaan ini tidak memiliki artikel pemeriksaan eGRF");
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