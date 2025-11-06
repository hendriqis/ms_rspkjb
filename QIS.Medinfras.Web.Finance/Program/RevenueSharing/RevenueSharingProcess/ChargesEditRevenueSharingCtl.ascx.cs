using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChargesEditRevenueSharingCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramList = param.Split('|');

            int oChargesDtID = Convert.ToInt32(paramList[0]);
            int oParamedicID = Convert.ToInt32(paramList[1]);
            int oItemID = Convert.ToInt32(paramList[2]);
            string chargesDtIDSource = paramList[3];
            int chargesDtDtID = Convert.ToInt32(paramList[4]);

            hdnChargesDtIDSourceCtlRS.Value = chargesDtIDSource;
            hdnChargesDtIDDtIDCtlRS.Value = chargesDtDtID.ToString();

            hdnParamedicIDCtlRS.Value = oParamedicID.ToString();

            PatientChargesDt chargesDt = BusinessLayer.GetPatientChargesDt(oChargesDtID);
            hdnPatientChargesDtIDCtlRS.Value = chargesDt.ID.ToString();
            if (chargesDt.RevenueSharingID != null && chargesDt.RevenueSharingID != 0)
            {
                RevenueSharingHd revenueSharingHd = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(chargesDt.RevenueSharingID));
                hdnRevenueSharingIDCtl.Value = revenueSharingHd.RevenueSharingID.ToString();
                txtRevenueSharingCodeCtl.Text = revenueSharingHd.RevenueSharingCode;
                txtRevenueSharingNameCtl.Text = revenueSharingHd.RevenueSharingName;
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

                ItemMaster itemMaster = BusinessLayer.GetItemMaster(dtPackage.ItemID);
                txtItemNameCode.Text = itemMaster.OldItemCode != "" ? string.Format("{0} ({1} / {2}*)", itemMaster.ItemName1, itemMaster.ItemCode, itemMaster.OldItemCode) : string.Format("{0} ({1})", itemMaster.ItemName1, itemMaster.ItemCode);

                ParamedicMaster paramedicMaster = BusinessLayer.GetParamedicMaster(dtPackage.ParamedicID);
                txtParamedicNameCode.Text = string.Format("{0} ({1})", paramedicMaster.FullName, paramedicMaster.ParamedicCode);

                if (dtPackage.RevenueSharingID != null && dtPackage.RevenueSharingID != 0)
                {
                    RevenueSharingHd revenueSharingHd = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(dtPackage.RevenueSharingID));
                    hdnRevenueSharingIDCtl.Value = revenueSharingHd.RevenueSharingID.ToString();
                    txtRevenueSharingCodeCtl.Text = revenueSharingHd.RevenueSharingCode;
                    txtRevenueSharingNameCtl.Text = revenueSharingHd.RevenueSharingName;
                }
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

                ParamedicMaster paramedicMaster = BusinessLayer.GetParamedicMaster(dtParamedic.ParamedicID);
                txtParamedicNameCode.Text = string.Format("{0} ({1})", paramedicMaster.FullName, paramedicMaster.ParamedicCode);

                if (dtParamedic.RevenueSharingID != null && dtParamedic.RevenueSharingID != 0)
                {
                    RevenueSharingHd revenueSharingHd = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(dtParamedic.RevenueSharingID));
                    hdnRevenueSharingIDCtl.Value = revenueSharingHd.RevenueSharingID.ToString();
                    txtRevenueSharingCodeCtl.Text = revenueSharingHd.RevenueSharingCode;
                    txtRevenueSharingNameCtl.Text = revenueSharingHd.RevenueSharingName;
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
            SetControlEntrySetting(hdnRevenueSharingIDCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueSharingCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueSharingNameCtl, new ControlEntrySetting(false, false, false));
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
                PatientChargesDt chargesDt = chargesDtDao.Get(Convert.ToInt32(hdnPatientChargesDtIDCtlRS.Value));
                if (hdnRevenueSharingIDCtl.Value != "" && hdnRevenueSharingIDCtl.Value != "0")
                {
                    chargesDt.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingIDCtl.Value);
                    chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    chargesDt.LastUpdatedDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    chargesDtDao.Update(chargesDt);

                    if (hdnChargesDtIDSourceCtlRS.Value == "ChargesDtPackage")
                    {
                        string filterDtPackage = string.Format("PatientChargesDtID = {0} AND ParamedicID = {1} AND IsDeleted = 0 AND ID = {2}",
                                                                chargesDt.ID, hdnParamedicIDCtlRS.Value, hdnChargesDtIDDtIDCtlRS.Value);
                        List<PatientChargesDtPackage> lstChargesDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterDtPackage, ctx);
                        if (lstChargesDtPackage.Count > 0)
                        {
                            foreach (PatientChargesDtPackage chargesDtPackage in lstChargesDtPackage)
                            {
                                ItemService itemSrvc = isrvDao.Get(chargesDtPackage.ItemID);
                                if (itemSrvc.IsAllowRevenueSharing)
                                {
                                    chargesDtPackage.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingIDCtl.Value);
                                    chargesDtPackage.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    chargesDtPackage.LastUpdatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    chargesDtPackageDao.Update(chargesDtPackage);
                                }
                            }
                        }
                    }

                    string filterDtParamedic = string.Format("ID = {0} AND ParamedicID = {1}", chargesDt.ID, hdnParamedicIDCtlRS.Value);
                    List<PatientChargesDtParamedic> lstChargesDtParamedic = BusinessLayer.GetPatientChargesDtParamedicList(filterDtParamedic, ctx);
                    if (lstChargesDtParamedic.Count > 0)
                    {
                        foreach (PatientChargesDtParamedic chargesDtParamedic in lstChargesDtParamedic)
                        {
                            chargesDtParamedic.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingIDCtl.Value);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            chargesDtParamedicDao.Update(chargesDtParamedic);
                        }
                    }

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