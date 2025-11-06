using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class UploadTarifBookEntryCtl : BaseViewPopupCtl
    {
        public List<GetTariffBookDtForUpload1> lstUpdateData;
        public override void InitializeDataControl(string param)
        {
            TmpItemTariflist.Clear();
            hdnBookID.Value = param;
            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_FILE_NAME_DOCUMENT_UPLOAD_TARIFF).ParameterValue;
            BindGridView();
        }

        private void BindGridView()
        {
            List<TmpItemTarif> lstData = TmpItemTariflist.ToList();
            grdView.DataSource = lstData;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    for (int i = 0; i < e.Row.Cells.Count; i++)
            //        e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            //}            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string param = e.Parameter;
            string result = param + "|";
            string errMessage = "";
            if (param == "upload")
            {
                if (OnUploadRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param == "save")
            {
                if (OnSaveRecord(ref errMessage))
                {
                    result += "success";
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnUploadRecord(ref string errMessage)
        {
            bool finalresult = true;
            TmpItemTariflist.Clear();
            try
            {
                #region Upload Document
                string fileUpload = hdnTmpTariffUploadedFile.Value;
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
                TariffBookHd oTarifHD = BusinessLayer.GetTariffBookHd(Convert.ToInt16(hdnBookID.Value));
                String DocumentNo = string.Empty;
                String DocumentDate = string.Empty;
                String StartingDate = string.Empty;

                if (oTarifHD != null)
                {
                    DocumentNo = oTarifHD.DocumentNo;
                    DocumentDate = oTarifHD.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                    StartingDate = oTarifHD.StartingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                }
                int rowCount = 0;
                int keyID = 0;
                foreach (string temp in lstTemp)
                {
                    if (rowCount != 0)
                    {
                        if (temp.Contains(';'))
                        {
                            List<String> fieldTemp = temp.Split(';').ToList();
                            if (!string.IsNullOrEmpty(fieldTemp[2]))
                            {
                                if (Convert.ToDecimal(fieldTemp[2]) != 0)
                                {
                                    GetTariffBookDtForUpload1 oData = new GetTariffBookDtForUpload1();
                                    oData.GCItemType = fieldTemp[0];
                                    oData.ItemType = fieldTemp[1];
                                    oData.ItemID = Convert.ToInt32(fieldTemp[2]);
                                    oData.ItemCode = fieldTemp[3];
                                    oData.ItemName1 = fieldTemp[4];
                                    oData.ClassID = Convert.ToInt32(fieldTemp[5]);
                                    oData.ClassCode = fieldTemp[6];
                                    oData.ClassName = fieldTemp[7];
                                    oData.ProposedTariffComp1 = Convert.ToDecimal(fieldTemp[8]);
                                    oData.ProposedTariffComp2 = Convert.ToDecimal(fieldTemp[9]);
                                    oData.ProposedTariffComp3 = Convert.ToDecimal(fieldTemp[10]);
                                    keyID += 1;
                                    SaveTarifSessionState(oData, keyID);
                                }
                            }
                        }
                    }
                    rowCount += 1;
                }
                finalresult = true;
                #endregion
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                finalresult = false;
            }
            return finalresult;
        }

        private bool OnSaveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TariffBookDtTempDao bookDtTempDao = new TariffBookDtTempDao(ctx);
            try
            {
                string[] lstKeyID = hdnSelectedListKey.Value.Split(',');
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<TariffBookDtTemp> dtTempList = BusinessLayer.GetTariffBookDtTempList(string.Format("BookID = {0}", hdnBookID.Value), ctx);
                foreach (TariffBookDtTemp dtTemp in dtTempList)
                {
                    bookDtTempDao.Delete(dtTemp.BookID, dtTemp.ItemID, dtTemp.ClassID);
                }
                TariffBookHd oTarifHD = BusinessLayer.GetTariffBookHd(Convert.ToInt32(hdnBookID.Value));
                string DocumentNo = string.Empty;
                string DocumentDate = string.Empty;
                string StartingDate = string.Empty;
                if (oTarifHD != null)
                {
                    DocumentNo = oTarifHD.DocumentNo;
                    DocumentDate = oTarifHD.DocumentDate.ToString(Constant.FormatString.DATE_FORMAT_112); ;
                    StartingDate = oTarifHD.StartingDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                }
                List<TmpItemTarif> lstDataTmpSucces = new List<TmpItemTarif>();
                List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0 ORDER BY ClassCode");

                for (int i = 0; i < lstKeyID.Length; i++)
                {
                    TmpItemTarif oDataItemTarif = TmpItemTariflist.Where(p => p.KeyID == Convert.ToInt32(lstKeyID[i])).FirstOrDefault();
                    if (oDataItemTarif != null)
                    {
                        TariffBookDtTemp oData = new TariffBookDtTemp();
                        oData.BookID = Convert.ToInt32(hdnBookID.Value);
                        oData.DocumentNo = DocumentNo;
                        oData.DocumentDate = DocumentDate;
                        oData.StartingDate = StartingDate;
                        oData.GCItemType = oDataItemTarif.GCItemType;
                        oData.ItemType = oDataItemTarif.ItemType;
                        oData.ItemID = oDataItemTarif.ItemID;
                        oData.ItemCode = oDataItemTarif.ItemCode;
                        oData.ItemName1 = oDataItemTarif.ItemName1;
                        oData.ClassID = oDataItemTarif.ClassID;
                        oData.ClassCode = oDataItemTarif.ClassCode;
                        oData.ClassName = oDataItemTarif.ClassName;
                        oData.BaseTariff = oDataItemTarif.BaseTariff;
                        oData.ProposedTariffComp1 = oDataItemTarif.ProposedTariffComp1;
                        oData.ProposedTariffComp2 = oDataItemTarif.ProposedTariffComp2;
                        oData.ProposedTariffComp3 = oDataItemTarif.ProposedTariffComp3;
                        oData.ProposedTariff = oDataItemTarif.ProposedTariff;
                        oData.MarginPercentage = 0;
                        oData.SuggestedTariff = 0;
                        oData.RevisionNo = 0;
                        oData.IsRevised = false;
                        oData.CreatedBy = AppSession.UserLogin.UserID;
                        oData.CreatedByUserName = AppSession.UserLogin.UserName;
                        oData.CreatedByFullName = AppSession.UserLogin.UserFullName;
                        oData.CreatedDate = DateTime.Now;
                        bookDtTempDao.Insert(oData);
                        //tmp session
                        lstDataTmpSucces.Add(oDataItemTarif);

                        //insert 0 
                        lstClassCare.Where(p => p.ClassID != oDataItemTarif.ClassID);
                    }
                }

                if (lstDataTmpSucces.Count > 0)
                {
                    var data = lstDataTmpSucces.GroupBy(test => test.ItemID).Select(grp => grp.First()).ToList().OrderBy(x => x.ItemID);
                    foreach (TmpItemTarif k in data)
                    {
                        List<TmpItemTarif> lstData = lstDataTmpSucces.Where(t => t.ItemID == k.ItemID).ToList();
                        foreach (ClassCare e in lstClassCare)
                        {
                            if (lstData.Where(t => t.ClassID == e.ClassID).Count() <= 0)
                            {
                                TariffBookDtTemp oData = new TariffBookDtTemp();
                                oData.BookID = Convert.ToInt32(hdnBookID.Value);
                                oData.DocumentNo = DocumentNo;
                                oData.DocumentDate = DocumentDate;
                                oData.StartingDate = StartingDate;
                                oData.GCItemType = lstData.FirstOrDefault().GCItemType;
                                oData.ItemType = lstData.FirstOrDefault().ItemType;
                                oData.ItemID = lstData.FirstOrDefault().ItemID;
                                oData.ItemCode = lstData.FirstOrDefault().ItemCode;
                                oData.ItemName1 = lstData.FirstOrDefault().ItemName1;
                                oData.ClassID = e.ClassID;
                                oData.ClassCode = e.ClassCode;
                                oData.ClassName = e.ClassName;
                                oData.BaseTariff = 0;
                                oData.ProposedTariffComp1 = 0;
                                oData.ProposedTariffComp2 = 0;
                                oData.ProposedTariffComp3 = 0;
                                oData.ProposedTariff = 0;
                                oData.MarginPercentage = 0;
                                oData.SuggestedTariff = 0;
                                oData.RevisionNo = 0;
                                oData.IsRevised = false;
                                oData.CreatedBy = AppSession.UserLogin.UserID;
                                oData.CreatedByUserName = AppSession.UserLogin.UserName;
                                oData.CreatedByFullName = AppSession.UserLogin.UserFullName;
                                oData.CreatedDate = DateTime.Now;
                                bookDtTempDao.Insert(oData);
                            }
                        }
                    }
                }
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

        private void SaveTarifSessionState(GetTariffBookDtForUpload1 data, int KeyID)
        {
            TmpItemTarif row = new TmpItemTarif();
            row.KeyID = KeyID;
            row.GCItemType = data.GCItemType;
            row.ItemType = data.ItemType;
            row.ItemID = data.ItemID;
            row.ItemCode = data.ItemCode;
            row.ItemName1 = data.ItemName1;
            row.ClassID = data.ClassID;
            row.ClassCode = data.ClassCode;
            row.ClassName = data.ClassName;
            row.ProposedTariffComp1 = data.ProposedTariffComp1;
            row.ProposedTariffComp2 = data.ProposedTariffComp2;
            row.ProposedTariffComp3 = data.ProposedTariffComp3;
            row.BaseTariff = 0;
            row.ProposedTariff = row.ProposedTariffComp1 + row.ProposedTariffComp2 + row.ProposedTariffComp3;
            TmpItemTariflist.Add(row);
        }

        public class TmpItemTarif
        {
            private int _KeyID;
            private string _GCItemType;
            private String _ItemType;
            private Int32 _ItemID;
            private String _ItemCode;
            private String _ItemName1;
            private Int32 _ClassID;
            private String _ClassCode;
            private String _ClassName;
            private Decimal _ProposedTariffComp1;
            private Decimal _ProposedTariffComp2;
            private Decimal _ProposedTariffComp3;
            private Decimal _BaseTariff;
            private Decimal _ProposedTariff;

            public int KeyID
            {
                get { return _KeyID; }
                set { _KeyID = value; }
            }
            public String GCItemType
            {
                get { return _GCItemType; }
                set { _GCItemType = value; }
            }
            public String ItemType
            {
                get { return _ItemType; }
                set { _ItemType = value; }
            }
            public Int32 ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            public String ItemCode
            {
                get { return _ItemCode; }
                set { _ItemCode = value; }
            }
            public String ItemName1
            {
                get { return _ItemName1; }
                set { _ItemName1 = value; }
            }
            public Int32 ClassID
            {
                get { return _ClassID; }
                set { _ClassID = value; }
            }
            public String ClassCode
            {
                get { return _ClassCode; }
                set { _ClassCode = value; }
            }
            public String ClassName
            {
                get { return _ClassName; }
                set { _ClassName = value; }
            }
            public Decimal ProposedTariffComp1
            {
                get { return _ProposedTariffComp1; }
                set { _ProposedTariffComp1 = value; }
            }
            public Decimal ProposedTariffComp2
            {
                get { return _ProposedTariffComp2; }
                set { _ProposedTariffComp2 = value; }
            }
            public Decimal ProposedTariffComp3
            {
                get { return _ProposedTariffComp3; }
                set { _ProposedTariffComp3 = value; }
            }
            public Decimal BaseTariff
            {
                get { return _BaseTariff; }
                set { _BaseTariff = value; }
            }
            public Decimal ProposedTariff
            {
                get { return _ProposedTariff; }
                set { _ProposedTariff = value; }
            }
        }

        public List<TmpItemTarif> TmpItemTariflist
        {
            get
            {
                if (Session["__TmpItemTarifList"] == null)
                    Session["__TmpItemTarifList"] = new List<TmpItemTarif>();
                return (List<TmpItemTarif>)Session["__TmpItemTarifList"];
            }
            set { Session["__TmpItemTarifList"] = value; }
        }
    }
}