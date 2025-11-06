using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class DownloadUploadTariffBook : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.DOWNLOAD_UPLOAD_TARIFF;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lstTariffScheme = BusinessLayer.GetStandardCodeList(string.Format("IsActive = 1 AND IsDeleted = 0 AND ParentID = '{0}'", Constant.StandardCode.TARIFF_SCHEME));
            lstTariffScheme.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCTariffScheme, lstTariffScheme, "StandardCodeName", "StandardCodeID");
            cboGCTariffScheme.Value = "";

            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_FILE_NAME_DOCUMENT_UPLOAD_TARIFF).ParameterValue;

            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("IsDeleted = 0 AND NumberOfItems = 0 AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.OPEN);

            if (cboGCTariffScheme.Value != null)
            {
                filterExpression += string.Format(" AND GCTariffScheme = '{0}'", cboGCTariffScheme.Value);
            }

            List<vTariffBookHd> lst = BusinessLayer.GetvTariffBookHdList(filterExpression, int.MaxValue, 1, "BookID");
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TariffBookDtTempDao bookDtTempDao = new TariffBookDtTempDao(ctx);

            try
            {
                if (type == "download")
                {
                   
                    #region Download Document

                    StringBuilder sbResult = new StringBuilder();

                    List<dynamic> lstDynamic = null;
                    List<Variable> lstVariable = new List<Variable>();


                    string oBookID = "0";
                    if (hdnBookID.Value != null && hdnBookID.Value != "")
                    {
                        oBookID = hdnBookID.Value;
                    }

                    lstVariable.Add(new Variable { Code = "BookID", Value = oBookID });
                    lstVariable.Add(new Variable { Code = "UserID", Value = AppSession.UserLogin.UserID.ToString() });

                    lstDynamic = BusinessLayer.GetDataReport("GetTariffBookDtForUpload", lstVariable);

                    dynamic fields = lstDynamic[0];

                    foreach (var prop in fields.GetType().GetProperties())
                    {
                        sbResult.Append(prop.Name);
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");

                    for (int i = 0; i < lstDynamic.Count; ++i)
                    {
                        dynamic entity = lstDynamic[i];

                        foreach (var prop in entity.GetType().GetProperties())
                        {
                            sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', '_'));
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");
                    }

                    retval = sbResult.ToString();

                    ctx.RollBackTransaction();

                    #endregion

                }
                else if (type == "upload")
                {
                    #region Upload Document

                    string fileUpload = hdnTariffUploadedFile.Value;
                    if (fileUpload != "")
                    {
                        string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                        fileUpload = String.Join(",", parts);
                    }

                    string path = AppConfigManager.QISPhysicalDirectory;
                    path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        Directory.Delete(path, true);
                        Directory.CreateDirectory(path);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<TariffBookDtTemp> dtTempList = BusinessLayer.GetTariffBookDtTempList(string.Format("BookID = {0}", hdnBookID.Value), ctx);
                        foreach (TariffBookDtTemp dtTemp in dtTempList)
                        {
                            bookDtTempDao.Delete(dtTemp.BookID, dtTemp.ItemID, dtTemp.ClassID);
                        }
                    }

                    FileStream fs = new FileStream(string.Format("{0}{1}", path, hdnFileName.Value), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    byte[] data = Convert.FromBase64String(fileUpload);
                    bw.Write(data);
                    bw.Close();

                    string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, hdnFileName.Value));

                    int rowCount = 0;
                    foreach (string temp in lstTemp)
                    {
                        if (rowCount != 0)
                        {
                            if (temp.Contains(','))
                            {
                                List<String> fieldTemp = temp.Split(',').ToList();

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                if (Convert.ToDecimal(fieldTemp[15]) != 0)
                                {
                                    TariffBookDtTemp oData = new TariffBookDtTemp();
                                    oData.BookID = Convert.ToInt32(fieldTemp[0]);
                                    oData.DocumentNo = fieldTemp[1];
                                    oData.DocumentDate = fieldTemp[2];
                                    oData.StartingDate = fieldTemp[3];
                                    oData.GCItemType = fieldTemp[4];
                                    oData.ItemType = fieldTemp[5];
                                    oData.ItemID = Convert.ToInt32(fieldTemp[6]);
                                    oData.ItemCode = fieldTemp[7];
                                    oData.ItemName1 = fieldTemp[8];
                                    oData.ClassID = Convert.ToInt32(fieldTemp[9]);
                                    oData.ClassCode = fieldTemp[10];
                                    oData.ClassName = fieldTemp[11];
                                    oData.SuggestedTariff = Convert.ToDecimal(fieldTemp[12]);
                                    oData.BaseTariff = Convert.ToDecimal(fieldTemp[13]);
                                    oData.ApprovedBaseTariff = Convert.ToDecimal(fieldTemp[14]);
                                    oData.ProposedTariff = Convert.ToDecimal(fieldTemp[15]);
                                    oData.ProposedTariffComp1 = Convert.ToDecimal(fieldTemp[16]);
                                    oData.ProposedTariffComp2 = Convert.ToDecimal(fieldTemp[17]);
                                    oData.ProposedTariffComp3 = Convert.ToDecimal(fieldTemp[18]);
                                    oData.Notes = fieldTemp[19];
                                    oData.CreatedBy = Convert.ToInt32(fieldTemp[20]);
                                    oData.CreatedByUserName = fieldTemp[21];
                                    oData.CreatedByFullName = fieldTemp[22];
                                    oData.CreatedDate = DateTime.Now;
                                    bookDtTempDao.Insert(oData);
                                }
                            }
                        }
                        rowCount += 1;
                    }

                    ctx.CommitTransaction();

                    #endregion
                }

                else if (type == "uploadnew")
                {
                    #region Upload Document

                    string fileUpload = hdnTariffUploadedFile.Value;
                    if (fileUpload != "")
                    {
                        string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                        fileUpload = String.Join(",", parts);
                    }

                    string path = AppConfigManager.QISPhysicalDirectory;
                    path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        Directory.Delete(path, true);
                        Directory.CreateDirectory(path);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<TariffBookDtTemp> dtTempList = BusinessLayer.GetTariffBookDtTempList(string.Format("BookID = {0}", hdnBookID.Value), ctx);
                        foreach (TariffBookDtTemp dtTemp in dtTempList)
                        {
                            bookDtTempDao.Delete(dtTemp.BookID, dtTemp.ItemID, dtTemp.ClassID);
                        }
                    }

                    FileStream fs = new FileStream(string.Format("{0}{1}", path, hdnFileName.Value), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    byte[] data = Convert.FromBase64String(fileUpload);
                    bw.Write(data);
                    bw.Close();

                    string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, hdnFileName.Value));
                    //string
                    TariffBookHd oTarifHD = BusinessLayer.GetTariffBookHd( Convert.ToInt16(hdnBookID.Value));
                    String DocumentNo = string.Empty;
                    String DocumentDate = string.Empty;
                    String StartingDate = string.Empty;

                    if (oTarifHD != null) {
                        DocumentNo = oTarifHD.DocumentNo;
                        DocumentDate = oTarifHD.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                        StartingDate = oTarifHD.StartingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                    }
                    int rowCount = 0;
                    foreach (string temp in lstTemp)
                    {
                        if (rowCount != 0)
                        {
                            if (temp.Contains(';'))
                            {
                                List<String> fieldTemp = temp.Split(';').ToList();

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                if (Convert.ToDecimal(fieldTemp[2]) != 0)
                                {
                                    TariffBookDtTemp oData = new TariffBookDtTemp();
                                    oData.BookID = Convert.ToInt32(hdnBookID.Value);
                                    oData.DocumentNo = DocumentNo;
                                    oData.DocumentDate = DocumentDate;
                                    oData.StartingDate = StartingDate;
                                    oData.GCItemType = fieldTemp[0];
                                    oData.ItemType = fieldTemp[1];
                                    oData.ItemID = Convert.ToInt32(fieldTemp[2]);
                                    oData.ItemCode = fieldTemp[3];
                                    oData.ItemName1 = fieldTemp[4];
                                    oData.ClassID = Convert.ToInt32(fieldTemp[5]);
                                    oData.ClassCode = fieldTemp[6];
                                    oData.ClassName = fieldTemp[7];
                                    //oData.SuggestedTariff = Convert.ToDecimal(fieldTemp[12]);
                                    //oData.BaseTariff = Convert.ToDecimal(fieldTemp[13]);
                                    //oData.ApprovedBaseTariff = Convert.ToDecimal(fieldTemp[14]);
                                    //oData.ProposedTariff = Convert.ToDecimal(fieldTemp[15]);
                                    oData.ProposedTariffComp1 = Convert.ToDecimal(fieldTemp[8]);
                                    oData.ProposedTariffComp2 = Convert.ToDecimal(fieldTemp[9]);
                                    oData.ProposedTariffComp3 = Convert.ToDecimal(fieldTemp[10]);
                                    oData.BaseTariff = oData.ProposedTariffComp1 + oData.ProposedTariffComp2 + oData.ProposedTariffComp3;
                                    oData.CreatedBy = AppSession.UserLogin.UserID;
                                    oData.CreatedByUserName = AppSession.UserLogin.UserName;
                                    oData.CreatedByFullName = AppSession.UserLogin.UserFullName;
                                    oData.CreatedDate = DateTime.Now;
                                    bookDtTempDao.Insert(oData);
                                }
                            }
                        }
                        rowCount += 1;
                    }

                    ctx.CommitTransaction();

                    #endregion
                }
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