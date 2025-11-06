using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GeriatricScreeningEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            if (parameter[0] == "add")
            {
                IsAdd = true;
                hdnVisitID.Value = parameter[1];
                hdnID.Value = "";
                SetControlProperties();
            }
            else if (parameter[0] == "edit")
            {
                IsAdd = false;
                hdnVisitID.Value = parameter[1];
                hdnID.Value = parameter[2];
                SetControlProperties();
                MiniNutritionalAssessment entity = BusinessLayer.GetMiniNutritionalAssessment(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            txtAssessmentDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'",
                Constant.ParamedicType.Nutritionist, AppSession.UserLogin.ParamedicID));
            Methods.SetComboBoxField<ParamedicMaster>(cboParamedicID, lstParamedic, "FullName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;
            if (AppSession.UserLogin.ParamedicID != null)
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            else
                cboParamedicID.SelectedIndex = 0;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') AND IsDeleted = 0", 
                                    Constant.StandardCode.MNA_SCREENING_A,
                                    Constant.StandardCode.MNA_SCREENING_B,
                                    Constant.StandardCode.MNA_SCREENING_C,
                                    Constant.StandardCode.MNA_SCREENING_E,
                                    Constant.StandardCode.MNA_SCREENING_F,
                                    Constant.StandardCode.MNA_ASSESSMENT_J,
                                    Constant.StandardCode.MNA_ASSESSMENT_M,
                                    Constant.StandardCode.MNA_ASSESSMENT_N,
                                    Constant.StandardCode.MNA_ASSESSMENT_O,
                                    Constant.StandardCode.MNA_ASSESSMENT_P,
                                    Constant.StandardCode.MNA_ASSESSMENT_Q));
            Methods.SetComboBoxField<StandardCode>(cboScreeningA, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_SCREENING_A).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboScreeningB, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_SCREENING_B).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboScreeningC, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_SCREENING_C).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboScreeningE, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_SCREENING_E).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboScreeningF, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_SCREENING_F).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAssessmentJ, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_ASSESSMENT_J).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAssessmentM, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_ASSESSMENT_M).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAssessmentN, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_ASSESSMENT_N).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAssessmentO, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_ASSESSMENT_O).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAssessmentP, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_ASSESSMENT_P).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAssessmentQ, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MNA_ASSESSMENT_Q).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAssessmentDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAssessmentTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboScreeningA, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboScreeningB, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboScreeningC, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboScreeningE, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboScreeningF, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(MiniNutritionalAssessment entity)
        {
            int changeProductOnceVal = 0;
            int changeProductTwiceVal = 0;
            int meatProductVal = 0;
            int totalBMConsumption = 0;

            txtAssessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = entity.AssessmentTime;
            hdnParamedicID.Value = entity.NutritionistID.ToString();
            cboParamedicID.Value = entity.NutritionistID.ToString();
            cboScreeningA.Value = entity.GCNutritionScreeningIntake;
            cboScreeningB.Value = entity.GCNutritionScreeningWeight;
            cboScreeningC.Value = entity.GCNutritionScreeningMobility;
            cboScreeningE.Value = entity.GCNutritionScreeningPsychology;
            cboScreeningF.Value = entity.GCNutritionScreeningIMT;
            cboAssessmentJ.Value = entity.GCNutritionAssessmentFullMeal;
            cboAssessmentM.Value = entity.GCNutritionAssessmentMineral;
            cboAssessmentN.Value = entity.GCNutritionAssessmentEatMethod;
            cboAssessmentO.Value = entity.GCNutritionAssessmentInsight;
            cboAssessmentP.Value = entity.GCNutritionAssessmentHealthStatus;
            cboAssessmentQ.Value = entity.GCNutritionAssessmentUAC;

            rblIsPsychologicallyStress.SelectedValue = entity.IsPsychologicallyStress ? "0" : "2";
            if (entity.IsPsychologicallyStress)
            {
                txtIsPsychologicallyStress.Text = "0";
            }
            else
            {
                txtIsPsychologicallyStress.Text = "2";
            }

            rblIsIndependent.SelectedValue = entity.IsIndependent ? "1" : "0";
            if (entity.IsIndependent)
            {
                txtIsIndependent.Text = "0";
            }
            else
            {
                txtIsIndependent.Text = "1";
            }

            rblIsTakingMedicine.SelectedValue = entity.IsTakingMedicines ? "1" : "0";
            if (entity.IsTakingMedicines)
            {
                txtIsTakingMedicine.Text = "0";
            }
            else
            {
                txtIsTakingMedicine.Text = "1";
            }

            rblUlkusDekubitus.SelectedValue = entity.IsHaveUlkusDekubitus ? "1" : "0";
            if (entity.IsHaveUlkusDekubitus)
            {
                txtUlkusDekubitus.Text = "0";
            }
            else
            {
                txtUlkusDekubitus.Text = "1";
            }

            rblExchangeOnce.SelectedValue = entity.IsChangeProductOnce ? "1" : "0";
            if (entity.IsChangeProductOnce)
            {
                txtExchangeOnce.Text = "1";
                changeProductOnceVal = 1;
            }
            else
            {
                txtExchangeOnce.Text = "0";
                changeProductOnceVal = 0;
            }

            rblExchangeTwice.SelectedValue = entity.IsChangeProductTwice ? "1" : "0";
            if (entity.IsChangeProductTwice)
            {
                txtExchangeTwice.Text = "1";
                changeProductTwiceVal = 1;
            }
            else
            {
                txtExchangeTwice.Text = "0";
                changeProductTwiceVal = 0;
            }

            rblMeatProduct.SelectedValue = entity.IsMeatProduct ? "1" : "0";
            if (entity.IsMeatProduct)
            {
                txtMeatProduct.Text = "1";
                meatProductVal = 1;
            }
            else
            {
                txtMeatProduct.Text = "0";
                meatProductVal = 0;
            }

            totalBMConsumption = changeProductOnceVal + changeProductTwiceVal + meatProductVal;
            if (totalBMConsumption <= 1)
            {
                txtTotalIntake.Text = "0";
            }
            else if (totalBMConsumption == 2)
            {
                txtTotalIntake.Text = "0.5";
            }
            else
            {
                txtTotalIntake.Text = "1";
            }

            rblChangeVegetableProduct.SelectedValue = entity.IsChangeVegetableProduct ? "1" : "0";
            if (entity.IsChangeVegetableProduct)
            {
                txtChangeVegetableProduct.Text = "0";
            }
            else
            {
                txtChangeVegetableProduct.Text = "1";
            }

            rblCalfCircumference.SelectedValue = entity.CalfCircumference ? "1" : "0";
            if (entity.CalfCircumference)
            {
                txtCalfCircumference.Text = "1";
            }
            else
            {
                txtCalfCircumference.Text = "0";
            }

            txtScreeningTotal.Text = entity.ScreeningScoreTotal.ToString();
            txtAssessmentTotal.Text = entity.AssessmentScoreTotal.ToString();
            txtOverallTotal.Text = entity.OverallTotal.ToString();
        }

        private void ControlToEntity(MiniNutritionalAssessment entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.NutritionistID = Convert.ToInt16(cboParamedicID.Value.ToString());
            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;
            entity.IsPsychologicallyStress = rblIsPsychologicallyStress.SelectedValue == "1";
            entity.IsIndependent = rblIsIndependent.SelectedValue == "1";
            entity.IsTakingMedicines = rblIsTakingMedicine.SelectedValue == "1";
            entity.IsHaveUlkusDekubitus = rblUlkusDekubitus.SelectedValue == "1";
            entity.IsChangeProductOnce = rblExchangeOnce.SelectedValue == "1";
            entity.IsChangeProductTwice = rblExchangeTwice.SelectedValue == "1";
            entity.IsMeatProduct = rblMeatProduct.SelectedValue == "1";
            entity.IsChangeVegetableProduct = rblChangeVegetableProduct.SelectedValue == "1";
            entity.CalfCircumference = rblCalfCircumference.SelectedValue == "1";
            entity.ScreeningScoreTotal = Convert.ToDecimal(Request.Form[txtScreeningTotal.UniqueID]);
            entity.AssessmentScoreTotal = Convert.ToDecimal(Request.Form[txtAssessmentTotal.UniqueID]);
            entity.OverallTotal = Convert.ToDecimal(Request.Form[txtOverallTotal.UniqueID]);

            if (cboScreeningA.Value != null)
                entity.GCNutritionScreeningIntake = cboScreeningA.Value.ToString();
            if (cboScreeningB.Value != null)
                entity.GCNutritionScreeningWeight = cboScreeningB.Value.ToString();
            if (cboScreeningC.Value != null)
                entity.GCNutritionScreeningMobility = cboScreeningC.Value.ToString();
            if (cboScreeningE.Value != null)
                entity.GCNutritionScreeningPsychology = cboScreeningE.Value.ToString();
            if (cboScreeningF.Value != null)
                entity.GCNutritionScreeningIMT = cboScreeningF.Value.ToString();

            if (cboAssessmentJ.Value != null)
                entity.GCNutritionAssessmentFullMeal = cboAssessmentJ.Value.ToString();
            if (cboAssessmentM.Value != null)
                entity.GCNutritionAssessmentMineral = cboAssessmentM.Value.ToString();
            if (cboAssessmentN.Value != null)
                entity.GCNutritionAssessmentEatMethod = cboAssessmentN.Value.ToString();
            if (cboAssessmentO.Value != null)
                entity.GCNutritionAssessmentInsight = cboAssessmentO.Value.ToString();
            if (cboAssessmentP.Value != null)
                entity.GCNutritionAssessmentHealthStatus = cboAssessmentP.Value.ToString();
            if (cboAssessmentQ.Value != null)
                entity.GCNutritionAssessmentUAC = cboAssessmentQ.Value.ToString();
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                MiniNutritionalAssessment entity = new MiniNutritionalAssessment();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertMiniNutritionalAssessment(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                MiniNutritionalAssessment entityUpdate = BusinessLayer.GetMiniNutritionalAssessment(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMiniNutritionalAssessment(entityUpdate);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}