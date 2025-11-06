using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangeTransactionDetailRemarksCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnTransactionID.Value = param;
                vPatientChargesHd1 oHeader = BusinessLayer.GetvPatientChargesHd1List(string.Format("TransactionID = {0}", hdnTransactionID.Value)).FirstOrDefault();
                if (oHeader != null)
                {
                    txtTransactionNo.Text = oHeader.TransactionNo;
                    BindGridView();
                    SetControlProperties();
                }

                //vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
                //txtMRN.ReadOnly = true;
                //txtPatientName.ReadOnly = true;
                //txtMRN.Text = entity.MedicalNo;
                //txtPatientName.Text = entity.PatientName;
                //ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param)).FirstOrDefault();
                //hdnVisitID.Value = entityCV.VisitID.ToString();
                //BindGridView();
            } 
        }

        private void SetControlProperties()
        {
            txtItemCode.Attributes.Add("validationgroup", "mpTrxDetail");
            txtItemName.Attributes.Add("validationgroup", "mpTrxDetail");
            txtNoteText.Attributes.Add("validationgroup", "mpTrxDetail");
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value);
            List<vPatientChargesDt> lstDetail = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            grdView.DataSource = lstDetail;
            grdView.DataBind();
        }

        protected void cbpDetailInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientChargesDtInfo entity)
        {
            entity.Remarks = txtNoteText.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PatientChargesDtInfo entity = BusinessLayer.GetPatientChargesDtInfo(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientChargesDtInfo(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            return result;
        }
    }
}