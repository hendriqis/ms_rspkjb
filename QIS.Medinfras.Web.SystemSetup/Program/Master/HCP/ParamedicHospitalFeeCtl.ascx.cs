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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ParamedicHospitalFeeCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParamedicIDCtl.Value = param;

            ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicIDCtl.Value));
            txtParamedicName.Text = pm.FullName;

            SetControlEntrySetting();
            SetControlProperties();

            BindGridView();

        }

        private void SetControlProperties()
        {
            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void SetControlEntrySetting()
        {
            Helper.SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtAmount, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(chkIsPercentage, new ControlEntrySetting(true, true, false), "mpEntryPopup");
        }

        private void BindGridView()
        {
            string filter = string.Format("ParamedicID = {0} AND IsDeleted = 0", hdnParamedicIDCtl.Value);
            List<vParamedicHospitalFee> lst = BusinessLayer.GetvParamedicHospitalFeeList(filter);
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ParamedicHospitalFee entity)
        {
            entity.ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
            entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            entity.IsPercentage = chkIsPercentage.Checked;
            entity.Amount = Convert.ToDecimal(txtAmount.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ParamedicHospitalFee entity = new ParamedicHospitalFee();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertParamedicHospitalFee(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ParamedicHospitalFee entity = BusinessLayer.GetParamedicHospitalFee(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicHospitalFee(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                ParamedicHospitalFee entity = BusinessLayer.GetParamedicHospitalFee(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicHospitalFee(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}