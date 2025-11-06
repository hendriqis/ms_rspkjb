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

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BasePagePatientPageList : BasePageContent
    {
        public bool IsPromptDeleteReason { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsCallback)
            {
                InitializeDataControl();
                SetControlProperties();
            }
        }

        #region Button Event
        public void OnBtnAddClick(ref string result, ref string url, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText, bool isEntryUsePopup)
        {
            result = "add|";
            string errMessage = "";
            if (OnBeforeAddRecord(ref errMessage))
            {
                if (isEntryUsePopup)
                {
                    if (OnAddRecord(ref url, ref errMessage, ref queryString, ref popupWidth, ref popupHeight, ref popupHeaderText))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnAddRecord(ref url, ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
                result += string.Format("fail|{0}", errMessage);
        }

        public void OnBtnEditClick(ref string result, ref string url, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText, bool isEntryUsePopup)
        {
            result = "edit|";
            string errMessage = "";
            if (OnBeforeEditRecord(ref errMessage))
            {
                if (isEntryUsePopup)
                {
                    if (OnEditRecord(ref url, ref errMessage, ref queryString, ref popupWidth, ref popupHeight, ref popupHeaderText))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnEditRecord(ref url, ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
                result += string.Format("fail|{0}", errMessage);
        }

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

        public void OnBtnCustomClick(ref string result, string type)
        {
            result = "customclick|";
            string message = "";
            if (OnCustomButtonClick(type, ref message))
                result += string.Format("success|{0}", message);
            else
                result += string.Format("fail|{0}", message);
        }
        #endregion

        #region Virtual Function
        protected virtual void InitializeDataControl()
        {
        }

        protected virtual void SetControlProperties()
        {
        }

        protected virtual bool OnBeforeAddRecord(ref string errMessage)
        {
            return true;
        }

        protected virtual bool OnBeforeEditRecord(ref string errMessage)
        {
            return true;
        }

        protected virtual bool OnBeforeDeleteRecord(ref string errMessage)
        {
            return true;
        }

        protected virtual bool OnAddRecord(ref string url, ref string errMessage)
        {
            return false;
        }

        protected virtual bool OnEditRecord(ref string url, ref string errMessage)
        {
            return false;
        }

        protected virtual bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            return false;
        }

        protected virtual bool OnAddRecordPopupCustomByType(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText, string type = "")
        {
            return false;
        }


        protected virtual bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            return false;
        }

        protected virtual bool OnDeleteRecord(ref string errMessage)
        {
            return false;
        }

        protected virtual bool OnCustomButtonClick(string type, ref string errMessage)
        {
            return true;
        }

        public virtual void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = null;
            fieldListValue = null;
        }
        #endregion
    }
}
