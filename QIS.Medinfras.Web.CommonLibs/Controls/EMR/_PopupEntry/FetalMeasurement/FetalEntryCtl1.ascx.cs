using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class FetalEntryCtl1 : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnID.Value = paramInfo[0];
            hdnAntenatalRecordID.Value = paramInfo[4];

            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                SetControlProperties();
                FetusData entity = BusinessLayer.GetFetusData(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.GENDER));
            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.GENDER).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGender, lstCode1, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFetusNo, new ControlEntrySetting(true, false, true, "1"));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(FetusData oFetusData)
        {
            oFetusData.AntenatalRecordID = Convert.ToInt32(hdnAntenatalRecordID.Value);
            oFetusData.FetusNo = Convert.ToInt16(txtFetusNo.Text);
            if (cboGender.Value != null)
            {
                oFetusData.GCSex = cboGender.Value.ToString();
            }
            oFetusData.Remarks = txtRemarks.Text;
        }

        private void EntityToControl(FetusData oFetusData)
        {
            txtFetusNo.Text = oFetusData.FetusNo.ToString();
            if (oFetusData.GCSex != null)
            {
                cboGender.Value = oFetusData.GCSex.ToString();
            }
            txtRemarks.Text = oFetusData.Remarks;
        }

        private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            if (string.IsNullOrEmpty(hdnAntenatalRecordID.Value) || hdnAntenatalRecordID.Value == "0")
            {
                errMsg.AppendLine("Informasi Janin harus melekat pada Antenatal Record");
            }

            if (!string.IsNullOrEmpty(txtFetusNo.Text) || txtFetusNo.Text.TrimEnd() != "0")
            {
                if (!Methods.IsNumeric(txtFetusNo.Text))
                {
                    errMsg.AppendLine(string.Format("Kode Janin harus dalam bentuk numerik/angka (1,2,3, dstnya)", txtFetusNo.Text));
                }
                else
                {
                    if (IsAdd)
                    {
                        List<FetusData> lstFetusData = BusinessLayer.GetFetusDataList(string.Format("AntenatalRecordID = {0} AND FetusNo = {1} AND IsDeleted = 0", hdnAntenatalRecordID.Value, txtFetusNo.Text.TrimEnd()));
                        if (lstFetusData.Count > 0)
                        {
                            errMsg.AppendLine(string.Format("Sudah ada informasi janin ke-{0}", txtFetusNo.Text));
                        }
                    }
                }
            }

            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            bool isError = false;

            if (!IsValidToSave(ref errMessage))
            {
                isError = true;
                result = false;
            }

            if (!isError)
            {
                IDbContext ctx = DbFactory.Configure(true);
                FetusDataDao fetusDataDao = new FetusDataDao(ctx);

                try
                {
                    #region Fetus Information
                    FetusData oFetusData = new FetusData();

                    ControlToEntity(oFetusData);
                    oFetusData.CreatedBy = AppSession.UserLogin.UserID;
                    fetusDataDao.Insert(oFetusData);
                    #endregion
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
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FetusDataDao fetusDataDao = new FetusDataDao(ctx);

            try
            {
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                FetusData entity = fetusDataDao.Get(Convert.ToInt32(hdnID.Value));
                if (entity != null)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    fetusDataDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Tidak ditemukan data janin yang dilakukan perubahan";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}