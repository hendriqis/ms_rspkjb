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
    public partial class RevenueSharingAdjustmentDownloadUpload : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_ADJUSTMENT_DOWNLOAD_UPLOAD_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetAdjustmentGroupPlus()
        {
            return Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_FILE_NAME_DOCUMENT_REVENUE_SHARING_ADJUSTMENT_DOWNLOAD_UPLOAD).ParameterValue;

            hdnAutoApprovedRevenueSharingAdj.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_AUTO_APPROVE_REVENUE_SHARING_ADJ_FROM_UPLOAD).ParameterValue;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                                                                        Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP,
                                                                                        Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE));

            Methods.SetRadioButtonListField(rblAdjustment, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField(cboAdjustmentTypeAdd, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboAdjustmentTypeMin, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "2").ToList(), "StandardCodeName", "StandardCodeID");

            rblAdjustment.SelectedValue = OnGetAdjustmentGroupPlus();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingAdjustmentHdDao entityHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                if (type == "download")
                {
                    #region Download Document

                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("ParamedicCode");
                    sbResult.Append(",");
                    sbResult.Append("Remarks");
                    sbResult.Append(",");
                    sbResult.Append("RevenueSharingCode");
                    sbResult.Append(",");
                    sbResult.Append("RegistrationNo");
                    sbResult.Append(",");
                    sbResult.Append("RegistrationDate[yyyyMMdd]");
                    sbResult.Append(",");
                    sbResult.Append("DischargeDate[yyyyMMdd]");
                    sbResult.Append(",");
                    sbResult.Append("ReceiptNo");
                    sbResult.Append(",");
                    sbResult.Append("InvoiceNo");
                    sbResult.Append(",");
                    sbResult.Append("ReferenceNo");
                    sbResult.Append(",");
                    sbResult.Append("BusinessPartnerName");
                    sbResult.Append(",");
                    sbResult.Append("MedicalNo[xx-xx-xx-xx]");
                    sbResult.Append(",");
                    sbResult.Append("PatientName");
                    sbResult.Append(",");
                    sbResult.Append("TransactionNo");
                    sbResult.Append(",");
                    sbResult.Append("TransactionDate[yyyyMMdd]");
                    sbResult.Append(",");
                    sbResult.Append("ItemName1");
                    sbResult.Append(",");
                    sbResult.Append("ChargedQty");
                    sbResult.Append(",");
                    sbResult.Append("AdjustmentAmountBRUTO");
                    sbResult.Append(",");
                    sbResult.Append("IsTaxed");
                    sbResult.Append(",");
                    sbResult.Append("AdjustmentAmount");
                    sbResult.Append(",");
                    sbResult.Append("GCRSAdjustmentGroup");
                    sbResult.Append(",");
                    sbResult.Append("GCRSAdjustmentType");
                    sbResult.Append(",");

                    retval = sbResult.ToString();

                    ctx.RollBackTransaction();

                    #endregion
                }
                else if (type == "upload")
                {
                    #region Upload Document

                    string fileUpload = hdnRevenueSharingUploadedFile.Value;
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
                    }

                    FileStream fs = new FileStream(string.Format("{0}{1}", path, hdnFileName.Value), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    byte[] data = Convert.FromBase64String(fileUpload);
                    bw.Write(data);
                    bw.Close();

                    string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, hdnFileName.Value));

                    int rowCount = 0;
                    string adjNo = "";
                    string adjNoLast = "";

                    foreach (string temp in lstTemp)
                    {
                        if (rowCount != 0)
                        {
                            if (temp.Contains(','))
                            {
                                List<String> fieldTemp = temp.Split(',').ToList();
                                if (fieldTemp[0] != null && fieldTemp[0] != "")
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    string ParamedicText = fieldTemp[0].Trim();
                                    string filterPM = string.Format("ParamedicCode = '{0}'", fieldTemp[0].Trim());
                                    bool isTaging = ParamedicText.Contains("|");
                                    if (isTaging)
                                    {
                                        string[] paramedicField = fieldTemp[0].Trim().Split('|');
                                        filterPM = string.Format("ParamedicID= '{0}'", paramedicField[0]);
                                    }

                                    List<ParamedicMaster> pmList = BusinessLayer.GetParamedicMasterList(filterPM, ctx);
                                    if (pmList.Count() > 0)
                                    {
                                        int oParamedicID = pmList.FirstOrDefault().ParamedicID;

                                        int adjHdID = 0;
                                        string filterAdjHd = string.Format("ParamedicID = {0} AND GCTransactionStatus = '{1}' AND IsFromUpload = 1", oParamedicID, Constant.TransactionStatus.OPEN);
                                        List<TransRevenueSharingAdjustmentHd> adjHdList = BusinessLayer.GetTransRevenueSharingAdjustmentHdList(filterAdjHd, ctx);
                                        if (adjHdList.Count() == 0)
                                        {
                                            TransRevenueSharingAdjustmentHd entityHd = new TransRevenueSharingAdjustmentHd();
                                            entityHd.RSAdjustmentDate = DateTime.Now;
                                            entityHd.RSAdjustmentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.TRANS_REVENUE_SHARING_ADJUSTMENT_ENTRY, entityHd.RSAdjustmentDate, ctx);
                                            entityHd.ParamedicID = oParamedicID;
                                            if (hdnAutoApprovedRevenueSharingAdj.Value == "1")
                                            {
                                                entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                                                entityHd.ApprovedDate = DateTime.Now;
                                            }
                                            else
                                            {
                                                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                            }
                                            entityHd.CreatedBy = AppSession.UserLogin.UserID;
                                            entityHd.IsFromUpload = true;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            adjHdID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                                            if (adjNo != "")
                                            {
                                                adjNo += ", ";
                                            }
                                            adjNo += entityHd.RSAdjustmentNo;
                                            adjNoLast = entityHd.RSAdjustmentNo;
                                        }
                                        else
                                        {
                                            string adjNoNow = adjHdList.FirstOrDefault().RSAdjustmentNo;

                                            adjHdID = adjHdList.FirstOrDefault().RSAdjustmentID;

                                            if (adjNoNow != adjNoLast)
                                            {
                                                if (adjNo != "")
                                                {
                                                    adjNo += ", ";
                                                }
                                                adjNo += adjNoNow;
                                            }

                                            adjNoLast = adjNoNow;
                                        }

                                        TransRevenueSharingAdjustmentDt entityDt = new TransRevenueSharingAdjustmentDt();
                                        entityDt.RSAdjustmentID = adjHdID;
                                        entityDt.Remarks = fieldTemp[1];

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        string RevenueSharingText = fieldTemp[2].Trim();
                                        string filterRS = string.Format("RevenueSharingCode = '{0}' AND IsDeleted = 0", fieldTemp[2]);
                                        bool isRevenueTaging = RevenueSharingText.Contains("|");
                                        if (isRevenueTaging)
                                        {
                                            string[] RevenueSharingTextField = fieldTemp[2].Trim().Split('|');
                                            filterRS = string.Format("RevenueSharingID = '{0}' AND IsDeleted = 0", RevenueSharingTextField[0]);
                                        }

                                        List<RevenueSharingHd> rs = BusinessLayer.GetRevenueSharingHdList(filterRS, ctx);
                                        if (adjHdList.Count() > 0)
                                        {
                                            entityDt.RevenueSharingID = rs.FirstOrDefault().RevenueSharingID;
                                        }
                                        entityDt.RegistrationNo = fieldTemp[3];
                                        entityDt.RegistrationDate = fieldTemp[4];
                                        entityDt.DischargeDate = fieldTemp[5];
                                        entityDt.ReceiptNo = fieldTemp[6];
                                        entityDt.InvoiceNo = fieldTemp[7];
                                        entityDt.ReferenceNo = fieldTemp[8];
                                        entityDt.BusinessPartnerName = fieldTemp[9];
                                        entityDt.MedicalNo = fieldTemp[10];
                                        entityDt.PatientName = fieldTemp[11];
                                        entityDt.TransactionNo = fieldTemp[12];
                                        entityDt.TransactionDate = Helper.YYYYMMDDToDate(fieldTemp[13]);
                                        entityDt.ItemName1 = fieldTemp[14];
                                        entityDt.ChargedQty = Convert.ToDecimal(fieldTemp[15]);
                                        entityDt.AdjustmentAmount = Convert.ToDecimal(fieldTemp[18]);
                                        entityDt.AdjustmentAmountBRUTO = Convert.ToDecimal(fieldTemp[16]);
                                        entityDt.IsTaxed = fieldTemp[17] == "1" ? true : false;
                                        if (fieldTemp[19] != null && fieldTemp[19] != "")
                                        {
                                            entityDt.GCRSAdjustmentGroup = fieldTemp[19];
                                        }
                                        else
                                        {
                                            entityDt.GCRSAdjustmentGroup = rblAdjustment.SelectedValue;
                                        }
                                        if (fieldTemp[20] != null && fieldTemp[20] != "")
                                        {
                                            entityDt.GCRSAdjustmentType = fieldTemp[20];
                                        }
                                        else
                                        {
                                            if (rblAdjustment.SelectedValue == OnGetAdjustmentGroupPlus())
                                            {
                                                entityDt.GCRSAdjustmentType = cboAdjustmentTypeAdd.Value.ToString();
                                            }
                                            else
                                            {
                                                entityDt.GCRSAdjustmentType = cboAdjustmentTypeMin.Value.ToString();
                                            }
                                        }
                                        entityDt.IsFromUpload = true;
                                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityDtDao.Insert(entityDt);
                                    }
                                    else
                                    {
                                        errMessage = "Ada data Kode Dokter (ParamedicCode = " + fieldTemp[0] + ") yang tidak ditemukan, silahkan cek ulang dan coba upload lagi.";
                                        break;
                                    }
                                }
                                else
                                {
                                    errMessage = "Ada data Kode Dokter (ParamedicCode = " + fieldTemp[0] + ") yang tidak ditemukan, silahkan cek ulang dan coba upload lagi.";
                                    break;
                                }
                            }
                        }
                        rowCount += 1;
                    }

                    if (errMessage != "")
                    {
                        retval = "failed|" + errMessage;
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        retval = "success|" + adjNo;
                        ctx.CommitTransaction();
                    }

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