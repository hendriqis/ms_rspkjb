using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxEditors;
using System.Reflection;
using System.Collections;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class SettingParameterEntryOld : BasePageEntry
    {
        protected string SearchDialogFilterExpression = "1";
        protected string SearchDialogMethodName = "1";
        protected string SearchDialogIDField = "1";
        protected string SearchDialogCodeField = "1";
        protected string SearchDialogNameField = "1";
        protected string SearchDialogType = "1";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.SYSTEM_PARAMETER;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SettingParameter entity = BusinessLayer.GetSettingParameter(ID);

                if (entity.GCParameterValueType == Constant.ControlType.COMBO_BOX)
                {
                    string methodName = string.Format("Get{0}List", entity.TableName);
                    MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                    object obj = method.Invoke(null, new string[] { entity.FilterExpression });
                    IList list = (IList)obj;

                    cboParameterValue.DataSource = list;
                    cboParameterValue.TextField = entity.TextField;
                    cboParameterValue.ValueField = entity.ValueField;
                    cboParameterValue.CallbackPageSize = 50;
                    cboParameterValue.EnableCallbackMode = false;
                    cboParameterValue.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                    cboParameterValue.DropDownStyle = DropDownStyle.DropDownList;
                    cboParameterValue.DataBind();
                }
                EntityToControl(entity);
            }
            txtParameterName.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtParameterCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtParameterName, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboParameterValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParameterValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtSDParameterCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSDParameterName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(SettingParameter entity)
        {
            txtParameterCode.Text = entity.ParameterCode;
            txtParameterName.Text = entity.ParameterName;
            if (entity.GCParameterValueType == Constant.ControlType.TEXT_BOX)
            {
                trSDParameterValue.Style.Add("display", "none");
                trCboParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Remove("display");
                txtParameterValue.Text = entity.ParameterValue;
            }
            else if (entity.GCParameterValueType == Constant.ControlType.COMBO_BOX)
            {
                trCboParameterValue.Style.Remove("display");
                trSDParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");
                cboParameterValue.Text = entity.ParameterValue;
            }
            else if (entity.GCParameterValueType == Constant.ControlType.SEARCH_DIALOG)
            {
                trSDParameterValue.Style.Remove("display");
                trCboParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");

                string filterExpression = entity.SearchDialogFilterExpression;
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

                SearchDialogCodeField = entity.SearchDialogCodeField;
                SearchDialogNameField = entity.SearchDialogNameField;
                SearchDialogIDField = entity.SearchDialogIDField;
                SearchDialogFilterExpression = entity.SearchDialogFilterExpression;
                SearchDialogMethodName = entity.SearchDialogMethodName;
                SearchDialogType = entity.SearchDialogType;
            }
            txtNotes.Text = entity.Notes;
        }

        private void ControlToEntity(SettingParameter entity)
        {
            entity.ParameterCode = txtParameterCode.Text;
            entity.ParameterName = txtParameterName.Text;
            if (entity.GCParameterValueType == Constant.ControlType.TEXT_BOX)
                entity.ParameterValue = txtParameterValue.Text;
            else if (entity.GCParameterValueType == Constant.ControlType.COMBO_BOX)
                entity.ParameterValue = cboParameterValue.Value.ToString();
            else if (entity.GCParameterValueType == Constant.ControlType.SEARCH_DIALOG)
                entity.ParameterValue = hdnSDParameterID.Value;
            entity.Notes = txtNotes.Text;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                SettingParameter entity = BusinessLayer.GetSettingParameter(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSettingParameter(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}