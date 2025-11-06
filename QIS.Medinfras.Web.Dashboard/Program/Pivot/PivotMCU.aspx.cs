using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class PivotMCU : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.PIVOT_MCU;
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void InitializeDataControl()
        {
            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            string filterExpression = string.Format("ID IN (SELECT ID FROM MCUResultFormTagField WHERE IsDeleted = 0) AND RegistrationDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            List<vMCUResultForm1> lstEntityJson = BusinessLayer.GetvMCUResultForm1List(filterExpression);
            if (lstEntityJson.Count > 0)
            {
                List<vMCUResultForm1> lstEntity = lstEntityJson.GroupBy(g => g.ResultType).Select(s => s.FirstOrDefault()).ToList();
                Methods.SetComboBoxField<vMCUResultForm1>(cboPivot, lstEntity, "ResultType", "GCResultType");
                cboPivot.SelectedIndex = 0;
                lstEntityJson = lstEntityJson.Where(w => w.GCResultType == cboPivot.Value.ToString()).ToList();
                hdnID.Value = cboPivot.Value.ToString();


                OnSetPivotValue(lstEntityJson);
            }
           
        }

        private string OnSetPivotValue(List<vMCUResultForm1> lstEntityJson)
        {
            string result = "";
            if (lstEntityJson.Count > 0)
            {
                string lstJson = string.Empty;

                foreach (vMCUResultForm1 json in lstEntityJson)
                {
                    Dictionary<String, dynamic> obj = new Dictionary<string, dynamic>();
                    obj["RegistrationNo"] = json.RegistrationNo;
                    obj["PatientName"] = json.PatientName;
                    obj["BusinessPartnerName"] = json.BusinessPartnerName;
                    obj["MedicalNo"] = json.MedicalNo;
                    obj["CorporateAccountNo"] = json.CorporateAccountNo;
                    obj["CorporateAccountName"] = json.CorporateAccountName;
                    obj["CorporateAccountDepartment"] = json.CorporateAccountDepartment;
                    obj["ParamedicCode"] = json.ParamedicCode;
                    obj["ParamedicName"] = json.ParamedicName;
                    obj[json.TagField1QuestionName] = json.TagField1QuestionValue;
                    obj[json.TagField2QuestionName] = json.TagField2QuestionValue;
                    obj[json.TagField3QuestionName] = json.TagField3QuestionValue;
                    obj[json.TagField4QuestionName] = json.TagField4QuestionValue;
                    obj[json.TagField5QuestionName] = json.TagField5QuestionValue;
                    obj[json.TagField6QuestionName] = json.TagField6QuestionValue;
                    obj[json.TagField7QuestionName] = json.TagField7QuestionValue;
                    obj[json.TagField8QuestionName] = json.TagField8QuestionValue;
                    obj[json.TagField9QuestionName] = json.TagField9QuestionValue;
                    obj[json.TagField10QuestionName] = json.TagField10QuestionValue;
                    obj[json.TagField11QuestionName] = json.TagField11QuestionValue;
                    obj[json.TagField12QuestionName] = json.TagField12QuestionValue;
                    obj[json.TagField13QuestionName] = json.TagField13QuestionValue;
                    obj[json.TagField14QuestionName] = json.TagField14QuestionValue;
                    obj[json.TagField15QuestionName] = json.TagField15QuestionValue;
                    obj[json.TagField16QuestionName] = json.TagField16QuestionValue;
                    obj[json.TagField17QuestionName] = json.TagField17QuestionValue;
                    obj[json.TagField18QuestionName] = json.TagField18QuestionValue;
                    obj[json.TagField19QuestionName] = json.TagField19QuestionValue;
                    obj[json.TagField20QuestionName] = json.TagField20QuestionValue;
                    obj.Remove("");
                    lstJson += JsonConvert.SerializeObject(obj) + ",";
                }
                Pivot1.Value = ("[" + lstJson.Remove(lstJson.Length - 1, 1) + "]").Replace(" ", string.Empty).Trim();
                result = Pivot1.Value;
            }

            return result;
        }

        protected void cboPivot_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = string.Format("ID IN (SELECT ID FROM MCUResultFormTagField WHERE IsDeleted = 0) AND GCResultType = '{0}' AND RegistrationDate = '{1}'", e.Parameter, Helper.GetDatePickerValue(txtDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            List<vMCUResultForm1> lstEntityJson = BusinessLayer.GetvMCUResultForm1List(filterExpression);
            //cboPivot.SelectedIndex = e.Parameter;
            string result = OnSetPivotValue(lstEntityJson);

            cboPivot.JSProperties["cpSelected"] = e.Parameter;
            cboPivot.JSProperties["cpResult"] = result;
        }

    }

    public class FormTagField
    {
        //public string QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string QuestionValue { get; set; }
        //public string SortID { get; set; }
        public string PatientName { get; set; }
    }
}