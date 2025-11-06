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
    public partial class MedicalResumeRevisionNoteCtl1 : BaseProcessPopupCtl
    {
        protected string MRN = "";
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];
            hdnRecordID.Value = paramInfo[1];
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

                if (!string.IsNullOrEmpty(txtRemarks.Text))
                {
                    string retVal = ProcessRecord(hdnRecordID.Value);
                    string[] retValInfo = retVal.Split('|');
                    if (retValInfo[0] == "0")
                    {
                        isError = true;
                        errMessage = retValInfo[1];
                    }
                }
                else
                {
                    isError = true;
                    errMessage = "Catatan harus diisi";
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
            MedicalResumeDao resumeDao = new MedicalResumeDao(ctx);

            try
            {
                int id = Convert.ToInt32(recordID);
                MedicalResume oRecord = BusinessLayer.GetMedicalResumeList(string.Format("ID = {0}", id), ctx).FirstOrDefault();
                if (oRecord != null)
                {
                    //Create New Record from Old Record
                    MedicalResume oNewRecord = new MedicalResume();
                    oNewRecord = oRecord;
                    oNewRecord.MedicalResumeDate = DateTime.Now;
                    oNewRecord.MedicalResumeTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    oNewRecord.RevisionDate = DateTime.Now;
                    oNewRecord.RevisionRemarks = txtRemarks.Text;
                    oNewRecord.ParamedicID = oRecord.ParamedicID;
                    oNewRecord.RevisedByParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    oNewRecord.GCMedicalResumeStatus = Constant.MedicalResumeStatus.OPEN;
                    oNewRecord.IsCasemixRevision = chkIsCasemixRevision.Checked;
                    resumeDao.Insert(oNewRecord);

                    //Update Old Record
                    oRecord = BusinessLayer.GetMedicalResume(Convert.ToInt32(id));
                    oRecord.IsRevised = true;
                    oRecord.GCMedicalResumeStatus = Constant.MedicalResumeStatus.REVISED;
                    oRecord.RevisedByParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    oRecord.RevisionDate = DateTime.Now;
                    oRecord.RevisionRemarks = txtRemarks.Text;
                    oRecord.IsCasemixRevision = chkIsCasemixRevision.Checked;
                    oRecord.LastUpdatedBy = AppSession.UserLogin.UserID;
                    resumeDao.Update(oRecord);

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