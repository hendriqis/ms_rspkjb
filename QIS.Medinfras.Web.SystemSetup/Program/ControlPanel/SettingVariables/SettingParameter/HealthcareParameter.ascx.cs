using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using System.Reflection;
using DevExpress.Web.ASPxEditors;
using System.Collections;

namespace QIS.Medinfras.Web.SystemSetup.Program.ControlPanel.SettingVariables.SettingParameter
{
    public partial class HealthcareParameter : BaseEntryPopupCtl
    {
        protected string SearchDialogFilterExpression = "1";
        protected string SearchDialogMethodName = "1";
        protected string SearchDialogIDField = "1";
        protected string SearchDialogCodeField = "1";
        protected string SearchDialogNameField = "1";
        protected string SearchDialogType = "1";

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            string[] temp = param.Split('|');
            hdnHealthcareID.Value = temp[0];
            hdnParameterCode.Value = temp[1];
            vSettingParameterDt entity = BusinessLayer.GetvSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'",hdnHealthcareID.Value,hdnParameterCode.Value)).ToList().FirstOrDefault();
            EntityToControl(entity);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtHealthcareName, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtParameterCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtParameterName, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboParameterValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParameterValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSDParameterCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSDParameterName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(vSettingParameterDt entity)
        {
            txtHealthcareName.Text = entity.HealthcareName;
            txtParameterCode.Text = entity.ParameterCode;
            txtParameterName.Text = entity.ParameterName;
            if (entity.GCParameterValueType == Constant.ControlType.TEXT_BOX)
            {
                trSDParameterValue.Style.Add("display", "none");
                trCboParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Remove("display");
                txtParameterValue.Text = entity.ParameterValue;
                hdnTempType.Value = "1";
            }
            else if (entity.GCParameterValueType == Constant.ControlType.COMBO_BOX)
            {
                trCboParameterValue.Style.Remove("display");
                trSDParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");
                cboParameterValue.Text = entity.ParameterValue;
                QIS.Medinfras.Data.Service.SettingParameter e = BusinessLayer.GetSettingParameter(entity.ParameterCode);

                string methodName = string.Format("Get{0}List", e.TableName);
                MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                object obj = method.Invoke(null, new string[] { e.FilterExpression });
                IList list = (IList)obj;

                cboParameterValue.DataSource = list;
                cboParameterValue.TextField = e.TextField;
                cboParameterValue.ValueField = e.ValueField;
                cboParameterValue.CallbackPageSize = 50;
                cboParameterValue.EnableCallbackMode = false;
                cboParameterValue.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                cboParameterValue.DropDownStyle = DropDownStyle.DropDownList;
                cboParameterValue.DataBind();
                hdnTempType.Value = "2";
            }
            else if (entity.GCParameterValueType == Constant.ControlType.SEARCH_DIALOG)
            {
                trSDParameterValue.Style.Remove("display");
                trCboParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");

                SearchDialogFilterExpression = entity.SearchDialogFilterExpression.Replace("@HealthcareID", hdnHealthcareID.Value);
                string filterExpression = SearchDialogFilterExpression;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("{0} = '{1}'", entity.SearchDialogIDField, entity.ParameterValue);
                MethodInfo method = typeof(BusinessLayer).GetMethod(entity.SearchDialogMethodName, new[] { typeof(string) });
                object tempObj = method.Invoke(null, new string[] { filterExpression });
                IList list = (IList)tempObj;
                if (list.Count > 0)
                {
                    object obj = list[0];
                    txtSDParameterCode.Text = obj.GetType().GetProperty(entity.SearchDialogCodeField).GetValue(obj, null).ToString();
                    txtSDParameterName.Text = obj.GetType().GetProperty(entity.SearchDialogNameField).GetValue(obj, null).ToString();
                }
                hdnSDParameterID.Value = entity.ParameterValue;

                hdnTempType.Value = "3";
                SearchDialogCodeField = entity.SearchDialogCodeField;
                SearchDialogNameField = entity.SearchDialogNameField;
                SearchDialogIDField = entity.SearchDialogIDField;
                SearchDialogMethodName = entity.SearchDialogMethodName;
                SearchDialogType = entity.SearchDialogType;
            }
        }

        private void ControlToEntity(SettingParameterDt entity)
        {
            if (hdnTempType.Value == "1")
            {
                entity.ParameterValue = txtParameterValue.Text;
            }
            else if(hdnTempType.Value == "2")
            {
                entity.ParameterValue = cboParameterValue.Value.ToString();
            }
            else if(hdnTempType.Value == "3")
            {
                entity.ParameterValue = hdnSDParameterID.Value;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                IDbContext ctx = DbFactory.Configure(true);
                SettingParameterDt entity = BusinessLayer.GetSettingParameterDt(hdnHealthcareID.Value,hdnParameterCode.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSettingParameterDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                return false;
            }
        }
    }
}