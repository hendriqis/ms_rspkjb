using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QIS.Medinfras.Web.CommonLibs.Tools
{
    public partial class CSToJSEntityToControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public static string GetControlID(string s)
        {
            int l = s.IndexOf(".");
            if (l > 0)
                return s.Substring(0, l);
            return "";
        }

        public static string GetControlValue(string s)
        {
            int l = s.IndexOf("=");
            if (l > 0)
                return s.Substring(l + 1).Trim();
            return "";
        }

        public static string GetControlIDFromDdl(string s)
        {
            int l = s.IndexOf("(");
            if (l > 0)
            {
                string result = s.Substring(l + 1);
                int l2 = result.IndexOf(",");
                return result.Substring(0, l2); 
            }
            return "";
        }

        public static string GetControlValueFromDdl(string s)
        {
            int l = s.IndexOf(",");
            if (l > 0)
            {
                string result = s.Substring(l + 1);
                int l2 = result.IndexOf(")");
                return result.Substring(0, l2);
            }
            return "";
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string[] par = txtCS.Text.Split('\n');
            string result = "";
            foreach (string s in par)
            {
                string temp = s.Trim().Replace(".Trim()", "");
                if (s.Contains("region"))
                {
                    result += string.Format("//{0}\n", temp);
                }
                else if (s.Contains(".ToString(Constant.FormatString.DATE_PICKER_FORMAT)"))
                {
                    temp = temp.Replace(".ToString(Constant.FormatString.DATE_PICKER_FORMAT);", "");
                    result += string.Format("$('#<%={0}.ClientID %>').val(Methods.getJSONDateValue({1}));\n", GetControlID(temp), GetControlValue(temp));
                }
                else if (s.Contains("SetDropDownListValue"))
                {
                    temp = temp.Replace(";", "");
                    result += string.Format("$('#<%={0}.ClientID %>').val({1});\n", GetControlIDFromDdl(temp), GetControlValueFromDdl(temp));
                }
                else if (temp == "")
                {
                    result += "\n";
                }
                else
                {
                    temp = temp.Replace(";", "");
                    temp = temp.Replace(".ToString()", "");
                    result += string.Format("$('#<%={0}.ClientID %>').val({1});\n", GetControlID(temp), GetControlValue(temp));
                }
            }
            txtJS.Text = result;
        }
    }
}