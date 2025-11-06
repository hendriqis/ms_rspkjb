using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BaseEntryPopupCtl : BaseContentPopupCtl
    {
        private BaseMPPopupEntry MasterControl = null;
        protected bool IsAdd = true;

        public override void LoadMasterControl()
        {
            MasterControl = (BaseMPPopupEntry)LoadControl("~/Libs/Controls/MPEntryPopupCtl.ascx");
            this.Parent.Controls.Add(MasterControl);
            MasterControl.GetPanelEntryPopup().Controls.Add(this);
        }

        public override void InitializeControl(string param)
        {
            base.InitializeControl(param);

            ControlEntryList.Clear();
            OnControlEntrySetting();
            ReInitControl();
            InitializeDataControl(param);
            SetControlEnabled(IsAdd);
            MasterControl.SetIsAdd(IsAdd);
        }

        public void OnBtnSaveClick(ref string result, ref string retval, bool isAdd)
        {
            string errMessage = "";
            if (isAdd)
            {
                result = "saveadd|";
                if (OnSaveAddRecord(ref errMessage, ref retval))
                    result += string.Format("success|{0}|{1}", errMessage,retval);
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                result = "saveedit|";
                if (OnSaveEditRecord(ref errMessage, ref retval))
                    result += string.Format("success|{0}|{1}", errMessage,retval);
                else
                    result += string.Format("fail|{0}", errMessage);
            }
        }

        public void OnRefreshCtlControl()
        {
            SetControlProperties();
            OnLoadCtlControl();
        }

        #region Virtual & Abstract Function

        protected virtual void SetControlProperties()
        {
        }

        protected virtual void OnLoadCtlControl()
        {
        }

        protected virtual void OnControlEntrySetting()
        {
        }

        protected virtual bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            return false;
        }
        protected virtual bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            return false;
        }

        public virtual void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = true;
        }

        public abstract void InitializeDataControl(string param);
        #endregion

        #region Utility Function
        private void SetControlEnabled(bool isAdd)
        {
            foreach (DictionaryEntry entry in ControlEntryList)
            {
                Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
                ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
                bool isEnabled = (isAdd ? setting.IsEditAbleInAddMode : setting.IsEditAbleInEditMode);
                SetControlProperties(ctrl, isEnabled);
            }
        }

        public void ReInitControl()
        {
            SetControlEnabled(true);
            LoadWords();
            foreach (DictionaryEntry entry in ControlEntryList)
            {
                Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
                if (ctrl is ASPxEdit || ctrl is WebControl || ctrl is HtmlInputHidden)
                {
                    ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
                    switch (setting.DefaultValue.ToString())
                    {
                        case Constant.DefaultValueEntry.DATE_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)); break;
                        case Constant.DefaultValueEntry.TIME_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)); break;
                        default: SetControlValue(ctrl, setting.DefaultValue); break;
                    }
                }
            }
        }

        protected void SetControlEntrySetting(Control ctrl, ControlEntrySetting setting)
        {
            ControlEntryList.Add(ctrl.ID, setting);
            if (ctrl is ASPxEdit)
            {
                ASPxEdit ctl = ctrl as ASPxEdit;
                ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
                ctl.ValidationSettings.RequiredField.ErrorText = "";
                ctl.ValidationSettings.CausesValidation = true;
                ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

                //if (setting.IsRequired)
                ctl.ValidationSettings.ValidationGroup = "mpEntryPopup";
            }
            else if (ctrl is WebControl)
            {
                if (setting.IsRequired)
                    Helper.AddCssClass(((WebControl)ctrl), "required");
                ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryPopup");
            }
        }

        private void SetControlProperties(Control ctrl, bool isEnabled)
        {
            if (ctrl is ASPxEdit)
            {
                ((ASPxEdit)ctrl).ClientEnabled = isEnabled;
            }
            else if (ctrl is TextBox)
            {
                if (isEnabled)
                    ((TextBox)ctrl).ReadOnly = false;
                else
                    ((TextBox)ctrl).ReadOnly = true;
            }
            else if (ctrl is DropDownList)
            {
                ((DropDownList)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is HtmlGenericControl)
            {
                HtmlGenericControl lbl = ctrl as HtmlGenericControl;
                if (!isEnabled)
                    lbl.Attributes.Add("class", "lblDisabled");
            }
        }

        private void SetControlValue(Control ctrl, object value)
        {
            if (ctrl is ASPxEdit)
                ((ASPxEdit)ctrl).Value = value;
            else if (ctrl is TextBox)
                ((TextBox)ctrl).Text = value.ToString();
            else if (ctrl is DropDownList)
                Helper.SetDropDownListValue((DropDownList)ctrl, value.ToString());
            else if (ctrl is CheckBox)
            {
                if (value.ToString() == "")
                    ((CheckBox)ctrl).Checked = false;
                else
                    ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
            }

            else if (ctrl is HtmlInputHidden)
                ((HtmlInputHidden)ctrl).Value = value.ToString();
        }
        #endregion

        #region Session & View State
        public Hashtable ControlEntryList
        {
            get
            {
                if (Session["__PopupControlEntryList"] == null)
                    Session["__PopupControlEntryList"] = new Hashtable();

                return (Hashtable)Session["__PopupControlEntryList"];
            }
            set { Session["__PopupControlEntryList"] = value; }
        }
        #endregion
    }
}
