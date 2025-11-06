using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.MCU.Program
{
    public partial class MCUResultFormEntryDetailCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {


                string filePath = HttpContext.Current.Server.MapPath("~/Libs/Scripts/McuForm/MCUFormRSRT.js");
                string text = File.ReadAllText(filePath);

                System.Web.UI.HtmlControls.HtmlGenericControl si = new System.Web.UI.HtmlControls.HtmlGenericControl();
                si.TagName = "script";
                si.Attributes.Add("type", @"text\javascript");
                si.InnerText = @"alert('I am in Head Element.')";
                this.Page.Header.Controls.Add(si);

                //validationForm.Text = text;  //untuk validasi form 
                string[] paramInfo = param.Split('|');
                IsAdd = paramInfo[0] == "1";
                SetControlProperties(paramInfo);
                PopulateFormContent();

            }
            else {
                hdnIDCtl.Value = "";
            }
            Healthcare oHc = BusinessLayer.GetHealthcareList(string.Format("HealthcareID='{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            if (oHc != null)
            {
                hdnHealthcareInitialID.Value = oHc.Initial;
            }
        }

        private void PopulateFormContent()
        {
            //string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string path = AppConfigManager.QISPhysicalDirectory;
            path += string.Format("{0}\\", AppConfigManager.QISMCUResultForm.Replace('/', '\\'));

            string fileName = string.Format(@"{0}\{1}.html", path, hdnGCResultTypeCtl.Value.Replace('^', '_'));
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent.InnerHtml = innerHtml.ToString();
            hdnDivHTMLCtl.Value = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnGCResultTypeCtl.Value = paramInfo[1];
            lblTitle.InnerText = paramInfo[2];
            hdnIDCtl.Value = paramInfo[3];

            if (!IsAdd)
            {
                hdnFormValueCtl.Value = paramInfo[4];
                txtRemarks.Text = paramInfo[5];
            }
        }

        protected override void OnControlEntrySetting()
        {
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MCUResultFormDao entityDao = new MCUResultFormDao(ctx);
            MCUResultFormTagFieldDao entiyTagDao = new MCUResultFormTagFieldDao(ctx);
            try
            {
                MCUResultForm entity = new MCUResultForm();
                entity.GCResultType = hdnGCResultTypeCtl.Value;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.FormLayout = hdnDivHTMLCtl.Value;
                entity.FormValue = hdnFormValueCtl.Value;
                entity.FormResult = hdnFormResultValueCtl.Value; 
                entity.Remarks = txtRemarks.Text;
                entity.CreatedBy = AppSession.UserLogin.UserID;

                //string strTagField = Helper.ParsingFormElektronikGetTagResult(hdnDivHTMLCtl.Value, hdnFormValueCtl.Value, string.Empty);
                //if (!string.IsNullOrEmpty(strTagField))
                //{
                //    entity.FormResult = strTagField;
                //}

               
                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

               
               /* string strTagField = Helper.ParsingFormElektronikGetTagField(hdnDivHTMLCtl.Value, hdnFormValueCtl.Value, string.Empty);
                if (entity.ID > 0)
                {
                    List<FormQuestionModel> lst = JsonConvert.DeserializeObject<List<FormQuestionModel>>(strTagField);
                    MCUResultFormTagField oData = new MCUResultFormTagField();
                    oData.ID = entity.ID;
                    oData.CreatedBy = AppSession.UserLogin.UserID;
                    oData.CreatedDate = DateTime.Now;

                    if (lst.Count > 0)
                    {
                      
                        foreach (FormQuestionModel row in lst)
                        {
                            string FormValue = JsonConvert.SerializeObject(row, Formatting.Indented);
                            if (row.SortID == "1")
                            {
                                oData.TagField01 = FormValue;
                            }
                            else if (row.SortID == "2") {
                                oData.TagField02 = FormValue;
                            }
                            else if (row.SortID == "3")
                            {
                                oData.TagField03 = FormValue;
                            }
                            else if (row.SortID == "4")
                            {
                                oData.TagField04 = FormValue;
                            }
                            else if (row.SortID == "5")
                            {
                                oData.TagField05 = FormValue;
                            }
                            else if (row.SortID == "6")
                            {
                                oData.TagField06 = FormValue;
                            }
                            else if (row.SortID == "7")
                            {
                                oData.TagField07 = FormValue;
                            }
                            else if (row.SortID == "8")
                            {
                                oData.TagField08 = FormValue;
                            }
                            else if (row.SortID == "9")
                            {
                                oData.TagField09 = FormValue;
                            }
                            else if (row.SortID == "10")
                            {
                                oData.TagField10 = FormValue;
                            }
                            else if (row.SortID == "11")
                            {
                                oData.TagField11 = FormValue;
                            }
                            else if (row.SortID == "12")
                            {
                                oData.TagField12 = FormValue;
                            }
                            else if (row.SortID == "13")
                            {
                                oData.TagField13 = FormValue;
                            }
                            else if (row.SortID == "14")
                            {
                                oData.TagField14 = FormValue;
                            }
                            else if (row.SortID == "15")
                            {
                                oData.TagField15 = FormValue;
                            }
                            else if (row.SortID == "16")
                            {
                                oData.TagField16 = FormValue;
                            }
                            else if (row.SortID == "17")
                            {
                                oData.TagField17 = FormValue;
                            }
                            else if (row.SortID == "18")
                            {
                                oData.TagField18 = FormValue;
                            }
                            else if (row.SortID == "19")
                            {
                                oData.TagField19 = FormValue;
                            }
                            else if (row.SortID == "20")
                            {
                                oData.TagField20 = FormValue;
                            }
                            else if (row.SortID == "21")
                            {
                                oData.TagField21 = FormValue;
                            }
                            else if (row.SortID == "22")
                            {
                                oData.TagField22 = FormValue;
                            }
                            else if (row.SortID == "23")
                            {
                                oData.TagField23 = FormValue;
                            }
                            else if (row.SortID == "24")
                            {
                                oData.TagField24 = FormValue;
                            }
                            else if (row.SortID == "25")
                            {
                                oData.TagField25 = FormValue;
                            }
                            else if (row.SortID == "26")
                            {
                                oData.TagField26 = FormValue;
                            }
                            else if (row.SortID == "27")
                            {
                                oData.TagField27 = FormValue;
                            }
                            else if (row.SortID == "28")
                            {
                                oData.TagField28 = FormValue;
                            }
                            else if (row.SortID == "29")
                            {
                                oData.TagField29 = FormValue;
                            }
                            else if (row.SortID == "30")
                            {
                                oData.TagField30 = FormValue;
                            }
                           
                            else if (row.SortID == "31")
                            {
                                oData.TagField31 = FormValue;
                            }
                            else if (row.SortID == "32")
                            {
                                oData.TagField32 = FormValue;
                            }
                            else if (row.SortID == "33")
                            {
                                oData.TagField33 = FormValue;
                            }
                            else if (row.SortID == "34")
                            {
                                oData.TagField34 = FormValue;
                            }
                            else if (row.SortID == "35")
                            {
                                oData.TagField35 = FormValue;
                            }
                            else if (row.SortID == "36")
                            {
                                oData.TagField36 = FormValue;
                            }
                            else if (row.SortID == "37")
                            {
                                oData.TagField37 = FormValue;
                            }
                            else if (row.SortID == "38")
                            {
                                oData.TagField38 = FormValue;
                            }
                            else if (row.SortID == "39")
                            {
                                oData.TagField39 = FormValue;
                            }
                            else if (row.SortID == "40")
                            {
                                oData.TagField40 = FormValue;
                            }
                            else if (row.SortID == "41")
                            {
                                oData.TagField41 = FormValue;
                            }
                            else if (row.SortID == "42")
                            {
                                oData.TagField42 = FormValue;
                            }
                            else if (row.SortID == "43")
                            {
                                oData.TagField43 = FormValue;
                            }
                            else if (row.SortID == "44")
                            {
                                oData.TagField44 = FormValue;
                            }
                            else if (row.SortID == "45")
                            {
                                oData.TagField45 = FormValue;
                            }
                            else if (row.SortID == "46")
                            {
                                oData.TagField46 = FormValue;
                            }
                            else if (row.SortID == "47")
                            {
                                oData.TagField47 = FormValue;
                            }
                            else if (row.SortID == "48")
                            {
                                oData.TagField48 = FormValue;
                            }
                            else if (row.SortID == "49")
                            {
                                oData.TagField49 = FormValue;
                            }
                            else if (row.SortID == "50")
                            {
                                oData.TagField50 = FormValue;
                            }

                        }
                        entiyTagDao.Insert(oData);
                    }
                }*/
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MCUResultFormDao entityDao = new MCUResultFormDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            MCUResultFormTagFieldDao entiyTagDao = new MCUResultFormTagFieldDao(ctx);
            try
            {
                int ID = Convert.ToInt32(hdnIDCtl.Value);
                MCUResultForm entity = entityDao.Get(ID);
                entity.GCResultType = hdnGCResultTypeCtl.Value;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.FormLayout = hdnDivHTMLCtl.Value;
                entity.FormValue = hdnFormValueCtl.Value;
                entity.Remarks = txtRemarks.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.FormResult = hdnFormResultValueCtl.Value; 
                //string strTagField = Helper.ParsingFormElektronikGetTagResult(hdnDivHTMLCtl.Value, hdnFormValueCtl.Value, string.Empty);
                //if (!string.IsNullOrEmpty(strTagField)) {
                //    entity.FormResult = strTagField; 
                //}
                entityDao.Update(entity);

                
                /* string strTagField = Helper.ParsingFormElektronikGetTagField(hdnDivHTMLCtl.Value, hdnFormValueCtl.Value, string.Empty);
                if (entity.ID > 0)
                {
                    List<FormQuestionModel> lst = JsonConvert.DeserializeObject<List<FormQuestionModel>>(strTagField);
                    MCUResultFormTagField oData = entiyTagDao.Get(ID);
                    oData.ID = entity.ID;
                    oData.CreatedBy = AppSession.UserLogin.UserID;
                    oData.CreatedDate = DateTime.Now;
                    if (lst.Count > 0)
                    {
                       
                        foreach (FormQuestionModel row in lst)
                        {
                            string FormValue = JsonConvert.SerializeObject(row, Formatting.Indented);
                            if (row.SortID == "1")
                            {
                                oData.TagField01 = FormValue;
                            }
                            else if (row.SortID == "2")
                            {
                                oData.TagField02 = FormValue;
                            }
                            else if (row.SortID == "3")
                            {
                                oData.TagField03 = FormValue;
                            }
                            else if (row.SortID == "4")
                            {
                                oData.TagField04 = FormValue;
                            }
                            else if (row.SortID == "5")
                            {
                                oData.TagField05 = FormValue;
                            }
                            else if (row.SortID == "6")
                            {
                                oData.TagField06 = FormValue;
                            }
                            else if (row.SortID == "7")
                            {
                                oData.TagField07 = FormValue;
                            }
                            else if (row.SortID == "8")
                            {
                                oData.TagField08 = FormValue;
                            }
                            else if (row.SortID == "9")
                            {
                                oData.TagField09 = FormValue;
                            }
                            else if (row.SortID == "10")
                            {
                                oData.TagField10 = FormValue;
                            }
                            else if (row.SortID == "11")
                            {
                                oData.TagField11 = FormValue;
                            }
                            else if (row.SortID == "12")
                            {
                                oData.TagField12 = FormValue;
                            }
                            else if (row.SortID == "13")
                            {
                                oData.TagField13 = FormValue;
                            }
                            else if (row.SortID == "14")
                            {
                                oData.TagField14 = FormValue;
                            }
                            else if (row.SortID == "15")
                            {
                                oData.TagField15 = FormValue;
                            }
                            else if (row.SortID == "16")
                            {
                                oData.TagField16 = FormValue;
                            }
                            else if (row.SortID == "17")
                            {
                                oData.TagField17 = FormValue;
                            }
                            else if (row.SortID == "18")
                            {
                                oData.TagField18 = FormValue;
                            }
                            else if (row.SortID == "19")
                            {
                                oData.TagField19 = FormValue;
                            }
                            else if (row.SortID == "20")
                            {
                                oData.TagField20 = FormValue;
                            }
                            else if (row.SortID == "21")
                            {
                                oData.TagField21 = FormValue;
                            }
                            else if (row.SortID == "22")
                            {
                                oData.TagField22 = FormValue;
                            }
                            else if (row.SortID == "23")
                            {
                                oData.TagField23 = FormValue;
                            }
                            else if (row.SortID == "24")
                            {
                                oData.TagField24 = FormValue;
                            }
                            else if (row.SortID == "25")
                            {
                                oData.TagField25 = FormValue;
                            }
                            else if (row.SortID == "26")
                            {
                                oData.TagField26 = FormValue;
                            }
                            else if (row.SortID == "27")
                            {
                                oData.TagField27 = FormValue;
                            }
                            else if (row.SortID == "28")
                            {
                                oData.TagField28 = FormValue;
                            }
                            else if (row.SortID == "29")
                            {
                                oData.TagField29 = FormValue;
                            }
                            else if (row.SortID == "30")
                            {
                                oData.TagField30 = FormValue;
                            }

                            else if (row.SortID == "31")
                            {
                                oData.TagField31 = FormValue;
                            }
                            else if (row.SortID == "32")
                            {
                                oData.TagField32 = FormValue;
                            }
                            else if (row.SortID == "33")
                            {
                                oData.TagField33 = FormValue;
                            }
                            else if (row.SortID == "34")
                            {
                                oData.TagField34 = FormValue;
                            }
                            else if (row.SortID == "35")
                            {
                                oData.TagField35 = FormValue;
                            }
                            else if (row.SortID == "36")
                            {
                                oData.TagField36 = FormValue;
                            }
                            else if (row.SortID == "37")
                            {
                                oData.TagField37 = FormValue;
                            }
                            else if (row.SortID == "38")
                            {
                                oData.TagField38 = FormValue;
                            }
                            else if (row.SortID == "39")
                            {
                                oData.TagField39 = FormValue;
                            }
                            else if (row.SortID == "40")
                            {
                                oData.TagField40 = FormValue;
                            }
                            else if (row.SortID == "41")
                            {
                                oData.TagField41 = FormValue;
                            }
                            else if (row.SortID == "42")
                            {
                                oData.TagField42 = FormValue;
                            }
                            else if (row.SortID == "43")
                            {
                                oData.TagField43 = FormValue;
                            }
                            else if (row.SortID == "44")
                            {
                                oData.TagField44 = FormValue;
                            }
                            else if (row.SortID == "45")
                            {
                                oData.TagField45 = FormValue;
                            }
                            else if (row.SortID == "46")
                            {
                                oData.TagField46 = FormValue;
                            }
                            else if (row.SortID == "47")
                            {
                                oData.TagField47 = FormValue;
                            }
                            else if (row.SortID == "48")
                            {
                                oData.TagField48 = FormValue;
                            }
                            else if (row.SortID == "49")
                            {
                                oData.TagField49 = FormValue;
                            }
                            else if (row.SortID == "50")
                            {
                                oData.TagField50 = FormValue;
                            }

                        }
                        entiyTagDao.Update(oData);
                    }
                }*/
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}