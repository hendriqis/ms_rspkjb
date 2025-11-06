using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDiagnosisHistoryCtl1 : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPopupVisitID.Value = paramInfo[0];
            hdnPopupMRN.Value = paramInfo[1];
            hdnPopupVisitDate.Value = paramInfo[2];
            hdnPopupVisitTime.Value = paramInfo[3];

            BindGridView();
        }

        private void BindGridView()
        {
            List<PatientDiagnosisSummary> lstEntity = BusinessLayer.GetPatientDiagnosisSummaryList(Convert.ToInt32(hdnPopupMRN.Value));
            grdDiagnosisSummaryView.DataSource = lstEntity;
            grdDiagnosisSummaryView.DataBind();
        }

        protected void cbpLookupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

            if (!IsValidated(ref errMessage))
            {
                result = false;
                retval = "0";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PatientDiagnosisDao diagnosisDao = new PatientDiagnosisDao(ctx);
            try
            {
                string[] lstSelectedDiagnosisID = hdnSelectedDiagnosisID.Value.Split('|');
                string[] lstSelectedDiagnosisName = hdnSelectedDiagnosisName.Value.Split('|');
                string[] lstSelectedDiagnosisText = hdnSelectedDiagnosisText.Value.Split('|');
                string[] lstSelectedMainDiagnosis = hdnSelectedMainDiagnosis.Value.Split('|');


                int i = 0;
                foreach (string diagnoseID in lstSelectedDiagnosisID)
                {
                    PatientDiagnosis oDiagnosis = new PatientDiagnosis();
                    
                    oDiagnosis.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
                    oDiagnosis.DifferentialDate = Helper.GetDatePickerValue(hdnPopupVisitDate.Value);
                    oDiagnosis.DifferentialTime = hdnPopupVisitTime.Value;

                    oDiagnosis.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    if (lstSelectedMainDiagnosis[i] == "1")
                        oDiagnosis.GCDiagnoseType = Constant.DiagnoseType.MAIN_DIAGNOSIS;
                    else
                        oDiagnosis.GCDiagnoseType = Constant.DiagnoseType.COMPLICATION;

                    oDiagnosis.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;

                    if (!string.IsNullOrEmpty(diagnoseID) && diagnoseID != "&nbsp;")
                        oDiagnosis.DiagnoseID = diagnoseID;
                    else
                        oDiagnosis.DiagnoseID = null;

                    oDiagnosis.DiagnosisText = lstSelectedDiagnosisText[i];
                    oDiagnosis.MorphologyID = null;
                    oDiagnosis.IsChronicDisease = false;
                    oDiagnosis.IsFollowUpCase = false;
                    oDiagnosis.Remarks = string.Empty;
                    oDiagnosis.CreatedBy = AppSession.UserLogin.UserID;

                    diagnosisDao.Insert(oDiagnosis);

                    i++;
                }

                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private bool IsValidated(ref string errMessage)
        {
            StringBuilder tempMsg = new StringBuilder();

            string message = string.Empty;

            return message == string.Empty;
        }
    }
}