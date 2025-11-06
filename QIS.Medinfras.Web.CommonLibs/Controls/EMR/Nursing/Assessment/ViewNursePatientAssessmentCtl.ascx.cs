using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewNursePatientAssessmentCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                if (param.Contains("X401^113") || param.Contains("X401^003") || param.Contains("X401^004") || param.Contains("X401^013") || param.Contains("X401^017"))
                {
                    paramInfo = param.Split('@');
                }

                string isNutritionMenu = "0";
                if (paramInfo.Count() > 15)
                {
                    isNutritionMenu = paramInfo[15];
                }
                if (isNutritionMenu == "1")
                {
                    trIsNeedVerify.Attributes.Remove("style");
                    trToddlerNutritionProblem.Attributes.Remove("style");
                }
                else
                {
                    trIsNeedVerify.Attributes.Add("style", "display:none");
                    trToddlerNutritionProblem.Attributes.Add("style", "display:none");
                }

                SetControlProperties(paramInfo);

                PatientAssessment oEntity = BusinessLayer.GetPatientAssessment(Convert.ToInt32(hdnIDViewCtl.Value));

                string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1",
                                                        Constant.StandardCode.TODDLER_NUTRITION_PROBLEM //0
                                                    );
                List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(filterSC);
                lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                Methods.SetComboBoxField<StandardCode>(cboToddlerNutritionProblemViewCtl, lstSC.Where(sc => sc.ParentID == Constant.StandardCode.TODDLER_NUTRITION_PROBLEM || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

                if (oEntity.GCToddlerNutritionProblem != null)
                {
                    cboToddlerNutritionProblemViewCtl.Value = oEntity.GCToddlerNutritionProblem;
                }
                else
                {
                    cboToddlerNutritionProblemViewCtl.Value = "";
                }

            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormTypeViewCtl.Value = paramInfo[0];
            hdnIDViewCtl.Value = paramInfo[1];
            txtObservationDate.Text = paramInfo[2];
            txtObservationTime.Text = paramInfo[3];
            txtParamedicInfo.Text = paramInfo[4];
            chkIsInitialAssessment.Checked = paramInfo[5] == "True" ? true : false;
            hdnFormLayoutViewCtl.Value = paramInfo[6];
            divFormContent.InnerHtml = paramInfo[6];
            hdnFormValuesViewCtl.Value = paramInfo[7];
            txtMedicalNo.Text = paramInfo[10];
            txtPatientName.Text = paramInfo[11];
            txtDateOfBirth.Text = paramInfo[12];
            txtRegistrationNo.Text = paramInfo[13];
            chkIsNeedVerify.Checked = paramInfo[14] == "True" ? true : false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}