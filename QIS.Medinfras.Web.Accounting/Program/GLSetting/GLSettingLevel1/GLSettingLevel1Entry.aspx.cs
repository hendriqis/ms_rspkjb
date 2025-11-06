using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLSettingLevel1Entry : BasePageEntry
    {
        protected string SearchDialogFilterExpression = "1";
        protected string SearchDialogMethodName = "1";
        protected string SearchDialogIDField = "1";
        protected string SearchDialogCodeField = "1";
        protected string SearchDialogNameField = "1";
        protected string SearchDialogType = "1";
        List<StandardCode> lstStandardCode = new List<StandardCode>();

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_SETTING_LEVEL1;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String[] param = Request.QueryString["id"].Split('|');
                String ID = param[0]; 
                String Healthcare = param[1];
                hdnID.Value = ID;
                HealthcareParameter entity = BusinessLayer.GetHealthcareParameter(Healthcare, ID);
                ASPxListBox cbmParameterValueListItem = ((ASPxListBox)cbmParameterValue.FindControl("cbmParameterValueListItem"));

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
                else if (entity.GCParameterValueType == Constant.ControlType.MULTI_SELECT_COMBO_BOX)
                {
                    if (string.IsNullOrEmpty(entity.ListValue) && string.IsNullOrEmpty(entity.ListText))
                    {
                        string methodName = string.Format("Get{0}List", entity.TableName);
                        MethodInfo method = typeof(BusinessLayer).GetMethod(methodName, new[] { typeof(string) });
                        object obj = method.Invoke(null, new string[] { entity.FilterExpression });
                        IList list = (IList)obj;

                        cbmParameterValueListItem.DataSource = list;
                        cbmParameterValueListItem.TextField = entity.TextField;
                        cbmParameterValueListItem.ValueField = entity.ValueField;
                        cbmParameterValueListItem.DataBind();
                    }
                    else
                    {
                        hdnListText.Value = entity.ListText;
                        hdnListValue.Value = entity.ListValue;
                        AssignListTextValue(entity, lstStandardCode);

                        cbmParameterValueListItem.DataSource = lstStandardCode;
                        cbmParameterValueListItem.TextField = "StandardCodeName";
                        cbmParameterValueListItem.ValueField = "StandardCodeID";
                        cbmParameterValueListItem.DataBind();
                    }
                }
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtParameterName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void AssignListTextValue(HealthcareParameter entity, List<StandardCode> lstStandardCode)
        {
            string[] lstText = entity.ListText.Split('|');
            string[] lstValue = entity.ListValue.Split('|');
            for (int i = 0; i < lstText.Length; i++)
            {
                StandardCode entitySC = new StandardCode();
                entitySC.StandardCodeID = lstValue[i];
                entitySC.StandardCodeName = lstText[i];
                lstStandardCode.Add(entitySC);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtParameterCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtParameterName, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboParameterValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cbmParameterValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParameterValue, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtSDParameterCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSDParameterName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(HealthcareParameter entity)
        {
            txtParameterCode.Text = entity.ParameterCode;
            txtParameterName.Text = entity.ParameterName;
            if (entity.GCParameterValueType == Constant.ControlType.TEXT_BOX)
            {
                trSDParameterValue.Style.Add("display", "none");
                trCboParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Remove("display");
                trCbmParameterValue.Style.Add("display", "none");
                txtParameterValue.Text = entity.ParameterValue;
            }
            else if (entity.GCParameterValueType == Constant.ControlType.COMBO_BOX)
            {
                trCboParameterValue.Style.Remove("display");
                trSDParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");
                trCbmParameterValue.Style.Add("display", "none");
                cboParameterValue.Text = entity.ParameterValue;
            }
            else if (entity.GCParameterValueType == Constant.ControlType.MULTI_SELECT_COMBO_BOX)
            {
                trCboParameterValue.Style.Add("display", "none");
                trSDParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");
                trCbmParameterValue.Style.Remove("display");

                if (!string.IsNullOrEmpty(entity.ListValue) && !string.IsNullOrEmpty(entity.ListText))
                {
                    if (!string.IsNullOrEmpty(entity.ParameterValue))
                    {
                        string[] key = entity.ParameterValue.Split('|');
                        string selected = string.Empty;
                        foreach (string a in key)
                        {
                            string keyName = lstStandardCode.Where(w => w.StandardCodeID == a).FirstOrDefault().StandardCodeName;
                            selected += keyName + "|";
                        }
                        selected.Remove(selected.Length - 1, 1);

                        cbmParameterValue.Text = selected;
                        hdnCbmParameterText.Value = selected;
                    }
                }
                else
                {
                    vHealthcareParameter entitySetvar = BusinessLayer.GetvHealthcareParameterList(string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, entity.ParameterCode)).FirstOrDefault();
                    cbmParameterValue.Text = entitySetvar.cfParameterValueCustom;
                    hdnCbmParameterText.Value = entitySetvar.cfParameterValueCustom;
                }
            }
            else if (entity.GCParameterValueType == Constant.ControlType.SEARCH_DIALOG)
            {
                trSDParameterValue.Style.Remove("display");
                trCboParameterValue.Style.Add("display", "none");
                trTxtParameterValue.Style.Add("display", "none");
                trCbmParameterValue.Style.Add("display", "none");

                string filterExpression = entity.SearchDialogFilterExpression.Replace("@HealthcareID", AppSession.UserLogin.HealthcareID);
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

        private void ControlToEntity(HealthcareParameter entity)
        {
            entity.ParameterCode = txtParameterCode.Text;
            entity.ParameterName = txtParameterName.Text;
            if (entity.GCParameterValueType == Constant.ControlType.TEXT_BOX)
                entity.ParameterValue = txtParameterValue.Text;
            else if (entity.GCParameterValueType == Constant.ControlType.COMBO_BOX)
                entity.ParameterValue = cboParameterValue.Value.ToString();
            else if (entity.GCParameterValueType == Constant.ControlType.MULTI_SELECT_COMBO_BOX)
                entity.ParameterValue = hdnCbmParameterValue.Value.ToString().Replace(',', '|');
            else if (entity.GCParameterValueType == Constant.ControlType.SEARCH_DIALOG)
                entity.ParameterValue = hdnSDParameterID.Value;
            entity.Notes = txtNotes.Text;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            HealthcareParameterDao entityDao = new HealthcareParameterDao(ctx);
            try
            {
                HealthcareParameter entity = entityDao.Get(AppSession.UserLogin.HealthcareID, hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}