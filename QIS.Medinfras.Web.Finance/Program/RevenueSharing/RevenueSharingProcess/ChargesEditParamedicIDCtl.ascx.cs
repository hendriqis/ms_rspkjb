using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChargesEditParamedicIDCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramList = param.Split('|');

            int oParamedicID = Convert.ToInt32(paramList[1]);
            string chargesDtIDSource = paramList[2];
            int chargesDtDtID = Convert.ToInt32(paramList[3]);

            hdnChargesDtIDSourceCtl.Value = chargesDtIDSource;
            hdnChargesDtIDDtIDCtl.Value = chargesDtDtID.ToString();

            PatientChargesDt chargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(paramList[0]));
            hdnChargesDtIDCtl.Value = chargesDt.ID.ToString();
            if (chargesDt.ParamedicID != null && chargesDt.ParamedicID != 0)
            {
                ParamedicMaster entity = BusinessLayer.GetParamedicMaster(Convert.ToInt32(chargesDt.ParamedicID));
                hdnParamedicIDCtl.Value = entity.ParamedicID.ToString();
                txtParamedicCodeCtl.Text = entity.ParamedicCode;
                txtParamedicNameCtl.Text = entity.FullName;

                hdnIsUpdate.Value = "0";
            }

            PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(chargesDt.TransactionID);
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtTransactionDate.Text = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);

            string filterDtPackage = string.Format("PatientChargesDtID = {0} AND ParamedicID = {1} AND IsDeleted = 0", chargesDt.ID, oParamedicID);
            if (chargesDtIDSource == "ChargesDtPackage")
            {
                filterDtPackage = string.Format("PatientChargesDtID = {0} AND ParamedicID = {1} AND IsDeleted = 0 AND ID = {2}", chargesDt.ID, oParamedicID, chargesDtDtID);
            }
            List<PatientChargesDtPackage> chargesDtPackageLst = BusinessLayer.GetPatientChargesDtPackageList(filterDtPackage);
            if (chargesDtIDSource == "ChargesDtPackage" && chargesDtPackageLst.Count() > 0)
            {
                PatientChargesDtPackage dtPackage = chargesDtPackageLst.FirstOrDefault();

                if (dtPackage.ParamedicID != null && dtPackage.ParamedicID != 0)
                {
                    ParamedicMaster entity = BusinessLayer.GetParamedicMaster(Convert.ToInt32(dtPackage.ParamedicID));
                    hdnParamedicIDCtl.Value = entity.ParamedicID.ToString();
                    txtParamedicCodeCtl.Text = entity.ParamedicCode;
                    txtParamedicNameCtl.Text = entity.FullName;

                    hdnIsUpdate.Value = "2";
                }

                ItemMaster itemMaster = BusinessLayer.GetItemMaster(dtPackage.ItemID);
                txtItemNameCode.Text = itemMaster.OldItemCode != "" ? string.Format("{0} ({1} / {2}*)", itemMaster.ItemName1, itemMaster.ItemCode, itemMaster.OldItemCode) : string.Format("{0} ({1})", itemMaster.ItemName1, itemMaster.ItemCode);
            }
            else
            {
                ItemMaster itemMaster = BusinessLayer.GetItemMaster(chargesDt.ItemID);
                txtItemNameCode.Text = itemMaster.OldItemCode != "" ? string.Format("{0} ({1} / {2}*)", itemMaster.ItemName1, itemMaster.ItemCode, itemMaster.OldItemCode) : string.Format("{0} ({1})", itemMaster.ItemName1, itemMaster.ItemCode);
            }

            string filterDtParamedic = string.Format("ID = {0} AND ParamedicID = {1}", chargesDt.ID, oParamedicID);
            List<PatientChargesDtParamedic> chargesDtParamedicLst = BusinessLayer.GetPatientChargesDtParamedicList(filterDtParamedic);
            if (chargesDtParamedicLst.Count() > 0)
            {
                PatientChargesDtParamedic dtParamedic = chargesDtParamedicLst.FirstOrDefault();
                if (dtParamedic.ParamedicID != null && dtParamedic.ParamedicID != 0)
                {
                    ParamedicMaster entity = BusinessLayer.GetParamedicMaster(Convert.ToInt32(dtParamedic.ParamedicID));
                    hdnParamedicIDCtl.Value = entity.ParamedicID.ToString();
                    txtParamedicCodeCtl.Text = entity.ParamedicCode;
                    txtParamedicNameCtl.Text = entity.FullName;

                    hdnIsUpdate.Value = "1";
                }
            }

            InitializeControlProperties();
        }

        private void InitializeControlProperties()
        {
            IsAdd = false;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnParamedicIDCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParamedicCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParamedicNameCtl, new ControlEntrySetting(false, false, false));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao chargesDtPackageDao = new PatientChargesDtPackageDao(ctx);
            PatientChargesDtParamedicDao chargesDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            ItemServiceDao isrvDao = new ItemServiceDao(ctx);
            try
            {
                PatientChargesDt chargesDt = chargesDtDao.Get(Convert.ToInt32(hdnChargesDtIDCtl.Value));
                if (hdnParamedicIDCtl.Value != "" && hdnParamedicIDCtl.Value != "0")
                {
                    if (hdnChargesDtIDSourceCtl.Value == "ChargesDtPackage")
                    {
                        string filterDtPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0 AND ID = {1}",
                                                                chargesDt.ID, hdnChargesDtIDDtIDCtl.Value);
                        List<PatientChargesDtPackage> lstChargesDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterDtPackage, ctx);
                        foreach (PatientChargesDtPackage chargesDtPackage in lstChargesDtPackage)
                        {
                            ItemService itemSrvc = isrvDao.Get(chargesDtPackage.ItemID);
                            if (itemSrvc.IsAllowRevenueSharing)
                            {
                                chargesDtPackage.ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
                                chargesDtPackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                                chargesDtPackage.LastUpdatedDate = DateTime.Now;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                chargesDtPackageDao.Update(chargesDtPackage);
                            }
                        }
                    }

                    string filterDtParamedic = string.Format("ID = {0} AND ParamedicID = {1}", chargesDt.ID, chargesDt.ParamedicID);
                    List<PatientChargesDtParamedic> lstChargesDtParamedic = BusinessLayer.GetPatientChargesDtParamedicList(filterDtParamedic, ctx);
                    if (lstChargesDtParamedic.Count > 0)
                    {
                        foreach (PatientChargesDtParamedic chargesDtParamedic in lstChargesDtParamedic)
                        {
                            chargesDtParamedic.ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            chargesDtParamedicDao.Update(chargesDtParamedic);
                        }
                    }

                    chargesDt.ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
                    chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    chargesDt.LastUpdatedDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    chargesDtDao.Update(chargesDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pilih terlebih dahulu untuk jenis jasa medis nya.";
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

    }
}