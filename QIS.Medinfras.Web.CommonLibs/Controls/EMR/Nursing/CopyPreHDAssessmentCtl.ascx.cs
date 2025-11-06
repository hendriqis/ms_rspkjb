using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class CopyPreHDAssessmentCtl : BaseProcessPopupCtl
    {
        protected string MRN = "";
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnRecordID.Value = paramInfo[0];
            txtParamedicName.Text = AppSession.UserLogin.UserFullName;
            txtNoteDateTime.Text = string.Format("{0} - {1}", DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

            hdnUserParamedicName.Value = AppSession.UserLogin.UserFullName;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                int id = Convert.ToInt32(hdnRecordID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                string retVal = ProcessRecord(hdnRecordID.Value);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "0")
                {
                    isError = true;
                    errMessage = retValInfo[1];
                }

                result = !isError;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        private string ProcessRecord(string recordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PreHDAssessmentDao oAssessmentDao = new PreHDAssessmentDao(ctx);

            try
            {
                int id = Convert.ToInt32(recordID);
                PreHDAssessment oRecord = BusinessLayer.GetPreHDAssessmentList(string.Format("ID = {0}",id),ctx).FirstOrDefault();
                if (oRecord != null)
                {
                    //Create New Record from Old Record
                    PreHDAssessment oNewRecord = new PreHDAssessment();
                    oNewRecord = oRecord;
                    oNewRecord.ID = 0;
                    oNewRecord.VisitID = AppSession.RegisteredPatient.VisitID;
                    oNewRecord.AssessmentDate = DateTime.Now.Date;
                    oNewRecord.AssessmentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                    if (AppSession.UserLogin.ParamedicID != 0)
                        oNewRecord.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);

                    oNewRecord.StartDate = DateTime.Now.Date;
                    oNewRecord.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    oNewRecord.EndDate = DateTime.MinValue;
                    oNewRecord.EndTime = null;
                    oNewRecord.PostHDAnamnese = string.Empty;
                    oNewRecord.PostHDParamedicID = null;
                    oNewRecord.PrimingBalance = 0;
                    oNewRecord.WashOut = 0;
                    oNewRecord.TotalIntake = 0;
                    oNewRecord.TotalOutput = 0;
                    oNewRecord.FinalUFG = 0;
                    oNewRecord.TotalUF = 0;

                    oAssessmentDao.Insert(oNewRecord);

                    ctx.CommitTransaction();

                    result = string.Format("1|{0}", string.Empty);
                }

            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}