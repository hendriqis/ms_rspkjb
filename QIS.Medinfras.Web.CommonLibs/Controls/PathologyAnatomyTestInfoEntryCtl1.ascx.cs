using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class PathologyAnatomyTestInfoEntryCtl1 : BaseEntryPopupCtl3
    {
        protected int gridSpecimenPageCount = 1;

        protected static string _visitID = "0";
        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            IsAdd = false;
            hdnVisitID.Value = "0";
            OnControlEntrySettingLocal();
            ReInitControl();
            hdnVisitID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            hdnTransactionID.Value = paramInfo[4];
            _orderID = hdnTestOrderID.Value;
            _visitID = hdnVisitID.Value;
            PatientChargesHdInfo entityHd = BusinessLayer.GetPatientChargesHdInfoList(string.Format("TransactionID = {0}", Convert.ToInt32(hdnTransactionID.Value))).FirstOrDefault();
            if (entityHd != null)
            {
                txtNoPA.Text = entityHd.PAReferenceNo;
                txtMacroscopicRemarks.Text = entityHd.MacroscopicRemarks;
                txtSpecimenLocation.Text = entityHd.SpecimenLocation;
                chkIsPATest.Checked = entityHd.IsPathologicalAnatomyTest;
            }
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlProperties();
        }

        private void ControlToEntity(PatientChargesHdInfo entityHd)
        {
            entityHd.LastUpdatedPAInfoBy = Convert.ToInt32(AppSession.UserLogin.UserID);
            entityHd.LastUpdatedPAInfoDate = DateTime.Now;
            entityHd.MacroscopicRemarks = txtMacroscopicRemarks.Text;
            entityHd.SpecimenLocation = txtSpecimenLocation.Text;
            entityHd.IsPathologicalAnatomyTest = chkIsPATest.Checked;
            entityHd.PAReferenceNo = txtNoPA.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            bool result = true;
            StringBuilder errMsg = new StringBuilder();

            #region Remarks
            if (string.IsNullOrEmpty(txtMacroscopicRemarks.Text))
            {
                errMsg.AppendLine("Catatan Makroskopic harus diisi");
            }
            if (string.IsNullOrEmpty(txtSpecimenLocation.Text))
            {
                errMsg.AppendLine("Lokasi Jaringan harus diisi");
            }
            #endregion

            errMessage = errMsg.ToString();

            result = string.IsNullOrEmpty(errMessage.ToString());

            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            try
            {
                if (IsValid(ref errMessage))
                {
                    PatientChargesHdInfo entityUpdate = BusinessLayer.GetPatientChargesHdInfo(Convert.ToInt32(hdnTransactionID.Value));
                    ControlToEntity(entityUpdate);
                    BusinessLayer.UpdatePatientChargesHdInfo(entityUpdate);

                    retVal = entityUpdate.TransactionID.ToString();
                }
                else
                {
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected void cbpProcessPA_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            if (param == "generateNo")
            {
                if (AppSession.LB0034 == "1" && chkIsPATest.Checked == true)
                {
                    string noPA = BusinessLayer.GeneratePALaboratoryResultReferenceNo(AppSession.LB0035, DateTime.Now);
                    hdnNoPA.Value = noPA;
                }
            }
            
        }
    }
}