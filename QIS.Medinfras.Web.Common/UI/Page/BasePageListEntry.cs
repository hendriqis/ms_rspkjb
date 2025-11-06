using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BasePageListEntry : BasePageContent
    {
        #region Session & View State
        public Hashtable ControlEntryList
        {
            get
            {
                if (Session["__ControlEntryList"] == null)
                    Session["__ControlEntryList"] = new Hashtable();

                return (Hashtable)Session["__ControlEntryList"];
            }
            set { Session["__ControlEntryList"] = value; }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsCallback)
            {
                ControlEntryList.Clear();
                OnControlEntrySetting();
                InitializeDataControl();
                SetControlProperties();
            }
        }

        #region Utility Function
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
                ctl.ValidationSettings.ValidationGroup = "mpEntryMPPage";
            }
            else if (ctrl is WebControl)
            {
                if (setting.IsRequired)
                {
                    if (ctrl is RadioButtonList)
                    {
                        RadioButtonList rbl = ctrl as RadioButtonList;
                        if (rbl.Items.Count > 0)
                            rbl.Items[0].Attributes.Add("class", "required");
                    }
                    else
                        Helper.AddCssClass(((WebControl)ctrl), "required");
                }
                ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryMPPage");
                if (setting.IsEditAbleInEditMode)
                    ((WebControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "1");
                else
                    ((WebControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "0");
            }
            else if (ctrl is HtmlGenericControl)
            {
                if (setting.IsEditAbleInEditMode)
                    ((HtmlGenericControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "1");
                else
                    ((HtmlGenericControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "0");
            }
        }
        #endregion

        #region Button Event
        public void OnBtnDeleteClick(ref string result)
        {
            result = "delete|";
            string errMessage = "";
            if (OnBeforeDeleteRecord(ref errMessage))
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
                result += string.Format("fail|{0}", errMessage);
        }

        public void OnBtnSaveClick(ref string result, bool isAdd, ref string retval)
        {
            string errMessage = "";
            if (isAdd)
            {
                result = "save|";
                if (OnBeforeSaveAddRecord(ref errMessage))
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                    {
                        if (errMessage == "")
                        {
                            if (OnSaveAddRecord(ref errMessage, ref retval))
                                result += "success";
                            else
                                result += string.Format("fail|{0}", errMessage);
                        }
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                result = "save|";
                if (OnBeforeSaveEditRecord(ref errMessage))
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                    {
                        if (errMessage == "")
                        {
                            if (OnSaveEditRecord(ref errMessage, ref retval))
                                result += "success";
                            else
                                result += string.Format("fail|{0}", errMessage);
                        }
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
        }

        public void OnQuickEntryClick(ref string result, string quickEntryText, ref string retval)
        {
            string errMessage = "";
            result = "savequickentry|";
            if (OnSaveQuickEntryRecord(quickEntryText, ref errMessage))
                result += "success";
            else
            {
                if (errMessage == "")
                {
                    if (OnSaveQuickEntryRecord(quickEntryText, ref errMessage, ref retval))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
        }
        #endregion

        #region Virtual Function
        protected virtual void InitializeDataControl()
        {
        }

        protected virtual void SetControlProperties()
        {
        }

        protected virtual bool OnBeforeDeleteRecord(ref string errMessage)
        {
            return true;
        }

        protected virtual bool OnDeleteRecord(ref string errMessage)
        {
            return false;
        }

        protected virtual void OnControlEntrySetting()
        {
        }

        protected virtual bool OnSaveQuickEntryRecord(string quickEntryText, ref string errMessage)
        {
            return false;
        }

        protected virtual bool OnSaveQuickEntryRecord(string quickEntryText, ref string errMessage, ref string retval)
        {
            return false;
        }

        protected virtual bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            return true;
        }
        protected virtual bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            return true;
        }

        protected virtual bool OnSaveAddRecord(ref string errMessage)
        {
            return false;
        }
        protected virtual bool OnSaveEditRecord(ref string errMessage)
        {
            return false;
        }

        protected virtual bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            return false;
        }
        protected virtual bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            return false;
        }

        public virtual string OnGetCustomMenuCaption()
        {
            return "";
        }
        public virtual string OnGetCustomBreadcrumbs(string breadCrumbs)
        {
            return "";
        }

        public virtual void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = true;
        }
        #endregion
    }
}
