using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web;
using System.IO;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class ReferralLetterFormCtl : BaseEntryPopupCtl4
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnConsentFormGroup.Value = paramInfo[0];

            StandardCode sc = BusinessLayer.GetStandardCode(hdnConsentFormGroup.Value);
            hdnExtraFieldCount.Value = sc.TagProperty;
            
            string fileName = string.Format(@"{0}\ReferralLetter\{1}.txt",AppConfigManager.QISPhysicalDirectory, sc.StandardCodeID.Replace('^', '_'));
            IEnumerable<string> lstCommand = File.ReadAllLines(fileName);
            StringBuilder commandText = new StringBuilder();
            foreach (string command in lstCommand)
            {
                commandText.AppendLine(command);
            }
            string[] CaptionData = commandText.ToString().Split(';');

            if (!string.IsNullOrEmpty(hdnExtraFieldCount.Value))
            {
                int jumlah = Convert.ToInt32(hdnExtraFieldCount.Value);
                if (jumlah == 1)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Add("display", "none");
                    trExtraField3.Style.Add("display", "none");
                    trExtraField4.Style.Add("display", "none");
                    trExtraField5.Style.Add("display", "none");

                    trExtraField6.Style.Add("display", "none");
                    trExtraField7.Style.Add("display", "none");
                    trExtraField8.Style.Add("display", "none");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 2)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Add("display", "none");
                    trExtraField4.Style.Add("display", "none");
                    trExtraField5.Style.Add("display", "none");

                    trExtraField6.Style.Add("display", "none");
                    trExtraField7.Style.Add("display", "none");
                    trExtraField8.Style.Add("display", "none");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 3)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Add("display", "none");
                    trExtraField5.Style.Add("display", "none");

                    trExtraField6.Style.Add("display", "none");
                    trExtraField7.Style.Add("display", "none");
                    trExtraField8.Style.Add("display", "none");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 4)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Add("display", "none");

                    trExtraField6.Style.Add("display", "none");
                    trExtraField7.Style.Add("display", "none");
                    trExtraField8.Style.Add("display", "none");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 5)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Add("display", "none");
                    trExtraField7.Style.Add("display", "none");
                    trExtraField8.Style.Add("display", "none");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 6)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Add("display", "none");
                    trExtraField8.Style.Add("display", "none");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 7)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Add("display", "none");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 8)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Add("display", "none");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 9)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Add("display", "none");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 10)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Add("display", "none");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 11)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Add("display", "none");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 12)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Add("display", "none");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 13)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Add("display", "none");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 14)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];
                    Label14.Text = CaptionData[13].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Remove("display");
                    trExtraField15.Style.Add("display", "none");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 15)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];
                    Label14.Text = CaptionData[13].Split('|')[0];
                    Label15.Text = CaptionData[14].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Remove("display");
                    trExtraField15.Style.Remove("display");

                    trExtraField16.Style.Add("display", "none");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 16)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];
                    Label14.Text = CaptionData[13].Split('|')[0];
                    Label15.Text = CaptionData[14].Split('|')[0];
                    Label16.Text = CaptionData[15].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Remove("display");
                    trExtraField15.Style.Remove("display");

                    trExtraField16.Style.Remove("display");
                    trExtraField17.Style.Add("display", "none");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 17)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];
                    Label14.Text = CaptionData[13].Split('|')[0];
                    Label15.Text = CaptionData[14].Split('|')[0];
                    Label16.Text = CaptionData[15].Split('|')[0];
                    Label17.Text = CaptionData[16].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Remove("display");
                    trExtraField15.Style.Remove("display");

                    trExtraField16.Style.Remove("display");
                    trExtraField17.Style.Remove("display");
                    trExtraField18.Style.Add("display", "none");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 18)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];
                    Label14.Text = CaptionData[13].Split('|')[0];
                    Label15.Text = CaptionData[14].Split('|')[0];
                    Label16.Text = CaptionData[15].Split('|')[0];
                    Label17.Text = CaptionData[16].Split('|')[0];
                    Label18.Text = CaptionData[17].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Remove("display");
                    trExtraField15.Style.Remove("display");

                    trExtraField16.Style.Remove("display");
                    trExtraField17.Style.Remove("display");
                    trExtraField18.Style.Remove("display");
                    trExtraField19.Style.Add("display", "none");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 19)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];
                    Label14.Text = CaptionData[13].Split('|')[0];
                    Label15.Text = CaptionData[14].Split('|')[0];
                    Label16.Text = CaptionData[15].Split('|')[0];
                    Label17.Text = CaptionData[16].Split('|')[0];
                    Label18.Text = CaptionData[17].Split('|')[0];
                    Label19.Text = CaptionData[18].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Remove("display");
                    trExtraField15.Style.Remove("display");

                    trExtraField16.Style.Remove("display");
                    trExtraField17.Style.Remove("display");
                    trExtraField18.Style.Remove("display");
                    trExtraField19.Style.Remove("display");
                    trExtraField20.Style.Add("display", "none");
                }
                else if (jumlah == 20)
                {
                    Label1.Text = CaptionData[0].Split('|')[0];
                    Label2.Text = CaptionData[1].Split('|')[0];
                    Label3.Text = CaptionData[2].Split('|')[0];
                    Label4.Text = CaptionData[3].Split('|')[0];
                    Label5.Text = CaptionData[4].Split('|')[0];
                    Label6.Text = CaptionData[5].Split('|')[0];
                    Label7.Text = CaptionData[6].Split('|')[0];
                    Label8.Text = CaptionData[7].Split('|')[0];
                    Label9.Text = CaptionData[8].Split('|')[0];
                    Label10.Text = CaptionData[9].Split('|')[0];
                    Label11.Text = CaptionData[10].Split('|')[0];
                    Label12.Text = CaptionData[11].Split('|')[0];
                    Label13.Text = CaptionData[12].Split('|')[0];
                    Label14.Text = CaptionData[13].Split('|')[0];
                    Label15.Text = CaptionData[14].Split('|')[0];
                    Label16.Text = CaptionData[15].Split('|')[0];
                    Label17.Text = CaptionData[16].Split('|')[0];
                    Label18.Text = CaptionData[17].Split('|')[0];
                    Label19.Text = CaptionData[18].Split('|')[0];
                    Label20.Text = CaptionData[19].Split('|')[0];

                    trExtraField1.Style.Remove("display");
                    trExtraField2.Style.Remove("display");
                    trExtraField3.Style.Remove("display");
                    trExtraField4.Style.Remove("display");
                    trExtraField5.Style.Remove("display");

                    trExtraField6.Style.Remove("display");
                    trExtraField7.Style.Remove("display");
                    trExtraField8.Style.Remove("display");
                    trExtraField9.Style.Remove("display");
                    trExtraField10.Style.Remove("display");

                    trExtraField11.Style.Remove("display");
                    trExtraField12.Style.Remove("display");
                    trExtraField13.Style.Remove("display");
                    trExtraField14.Style.Remove("display");
                    trExtraField15.Style.Remove("display");

                    trExtraField16.Style.Remove("display");
                    trExtraField17.Style.Remove("display");
                    trExtraField18.Style.Remove("display");
                    trExtraField19.Style.Remove("display");
                    trExtraField20.Style.Remove("display");
                }
            }

            if (paramInfo.Length > 2)
            {
                hdnExistingSignature.Value = paramInfo[2];
            }
            if (paramInfo[1] != "" && paramInfo[1] != "0")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[1];
                Helper.SetControlEntrySetting(cboGCConsentFormGroup, new ControlEntrySetting(true, false, true), "mpEntryPopup");
                RegistrationReferralLetter entity = BusinessLayer.GetRegistrationReferralLetter(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
            }

            cboGCConsentFormGroup.Value = hdnConsentFormGroup.Value;
            cboGCConsentFormGroup.Enabled = false;

            if (cboGCConsentFormGroup.Value != null)
            {
                hdnConsentFormGroup.Value = cboGCConsentFormGroup.Value.ToString();
            }
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    cboParamedicID.ClientEnabled = false;
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
            }

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REFERRAL_LETTER);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboGCConsentFormGroup, lstSc.Where(p => p.ParentID == Constant.StandardCode.REFERRAL_LETTER).ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(RegistrationReferralLetter entity)
        {
            txtObservationDate.Text = Convert.ToDateTime(entity.LetterDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.LetterTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();

            List<ExtraField> lstExtraField = JsonConvert.DeserializeObject<List<ExtraField>>(entity.ExtraField);
            foreach (ExtraField e in lstExtraField)
            {
                if (e.ExtraFieldNo == 1)
                {
                    txtField1.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 2)
                {
                    txtField2.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 3)
                {
                    txtField3.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 4)
                {
                    txtField4.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 5)
                {
                    txtField5.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 6)
                {
                    txtField6.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 7)
                {
                    txtField7.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 8)
                {
                    txtField8.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 9)
                {
                    txtField9.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 10)
                {
                    txtField10.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 11)
                {
                    txtField11.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 12)
                {
                    txtField12.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 13)
                {
                    txtField13.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 14)
                {
                    txtField14.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 15)
                {
                    txtField15.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 16)
                {
                    txtField16.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 17)
                {
                    txtField17.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 18)
                {
                    txtField18.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 19)
                {
                    txtField19.Text = e.Value;
                }
                else if (e.ExtraFieldNo == 20)
                {
                    txtField20.Text = e.Value;
                }
            }
        }

        private void ControlToEntity(RegistrationReferralLetter entity)
        {
            entity.LetterDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.LetterTime = txtObservationTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.StandardCodeID = hdnConsentFormGroup.Value;

            List<ExtraField> lstData = new List<ExtraField>();

            if (!string.IsNullOrEmpty(hdnExtraFieldCount.Value))
            {
                for (int i = 1; i <= Convert.ToInt32(hdnExtraFieldCount.Value); i++)
                {
                    ExtraField data = new ExtraField();
                    data.ExtraFieldNo = i;

                    if (i == 1)
                    {
                        data.Value = txtField1.Text;
                    }
                    else if (i == 2)
                    {
                        data.Value = txtField2.Text;
                    }
                    else if (i == 3)
                    {
                        data.Value = txtField3.Text;
                    }
                    else if (i == 4)
                    {
                        data.Value = txtField4.Text;
                    }
                    else if (i == 5)
                    {
                        data.Value = txtField5.Text;
                    }
                    else if (i == 6)
                    {
                        data.Value = txtField6.Text;
                    }
                    else if (i == 7)
                    {
                        data.Value = txtField7.Text;
                    }
                    else if (i == 8)
                    {
                        data.Value = txtField8.Text;
                    }
                    else if (i == 9)
                    {
                        data.Value = txtField9.Text;
                    }
                    else if (i == 10)
                    {
                        data.Value = txtField10.Text;
                    }
                    else if (i == 11)
                    {
                        data.Value = txtField11.Text;
                    }
                    else if (i == 12)
                    {
                        data.Value = txtField12.Text;
                    }
                    else if (i == 13)
                    {
                        data.Value = txtField13.Text;
                    }
                    else if (i == 14)
                    {
                        data.Value = txtField14.Text;
                    }
                    else if (i == 15)
                    {
                        data.Value = txtField15.Text;
                    }
                    else if (i == 16)
                    {
                        data.Value = txtField16.Text;
                    }
                    else if (i == 17)
                    {
                        data.Value = txtField17.Text;
                    }
                    else if (i == 18)
                    {
                        data.Value = txtField18.Text;
                    }
                    else if (i == 19)
                    {
                        data.Value = txtField19.Text;
                    }
                    else if (i == 20)
                    {
                        data.Value = txtField20.Text;
                    }
                    lstData.Add(data);
                }

                string json = JsonConvert.SerializeObject(lstData);
                entity.ExtraField = json;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (AppSession.UserLogin.ParamedicID == null)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", "Field Tenaga Medis tidak boleh dikosongkan", "User Login saat ini tidak terhubung dengan kode tenaga medis");
                result = false;
            }

            IDbContext ctx = DbFactory.Configure(true);
            RegistrationReferralLetterDao entityDao = new RegistrationReferralLetterDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    RegistrationReferralLetter entity = new RegistrationReferralLetter();

                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    int id = entityDao.InsertReturnPrimaryKeyID(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationReferralLetterDao entityDao = new RegistrationReferralLetterDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    RegistrationReferralLetter entity = entityDao.Get(Convert.ToInt32(hdnID.Value));

                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void cbpConsentFormContent_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

                string fileName = "";
                IEnumerable<string> lstText = File.ReadAllLines(fileName);
                StringBuilder innerHtml = new StringBuilder();
                foreach (string text in lstText)
                {
                    innerHtml.AppendLine(text);
                }

                result = innerHtml.ToString();

                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpFormLayout"] = result;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);

                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpFormLayout"] = string.Empty;
            }
        }

        private bool IsValid(ref string errMessage)
        {
            bool isValid = true;

            StringBuilder sbMessage = new StringBuilder();

            errMessage = sbMessage.ToString().Replace(Environment.NewLine, "<br />");
            isValid = string.IsNullOrEmpty(errMessage) ? true : false;

            return isValid;
        }

        public class ExtraField
        {
            public int ExtraFieldNo { get; set; }
            public string Value { get; set; }
        }
    }
}