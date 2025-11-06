using System;
using System.Collections;
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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class MatrixCtl : BaseViewPopupCtl
    {
        #region Billing Group Item
        private void InitializeBillingGroupItem(string queryString)
        {
            lblHeader.InnerText = "Kode Kelompok Tagihan";
            lblHeader2.InnerText = "Nama Kelompok Tagihan";

            BillingGroup billGroup = BusinessLayer.GetBillingGroupList(string.Format("BillingGroupID = {0}", queryString))[0];
            txtHeader.Text = billGroup.BillingGroupCode;
            txtHeader2.Text = billGroup.BillingGroupName1;

            //string GCItemType = string.Format("'{0}','{1}','{2}'",Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY);
            //string GCItemType = string.Format("'{0}'", Constant.ItemGroupMaster.LOGISTIC);
            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(string.Format("BillingGroupID IS NULL AND IsDeleted = 0 ORDER BY ItemName1 ASC"));
            //List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList("BillingGroupID IS NULL AND IsDeleted = 0 ORDER BY ItemName1 ASC");
            List<ItemMaster> ListSelectedItem = BusinessLayer.GetItemMasterList(string.Format("BillingGroupID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemCode }).ToList();
        }

        private bool SaveBillingGroupItem(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int BillingGroupID = Convert.ToInt32(queryString);
                ItemMasterDao entityDao = new ItemMasterDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    ItemMaster entity = BusinessLayer.GetItemMaster(Int32.Parse(row.ID));
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        entity.BillingGroupID = BillingGroupID;
                    }
                    else
                        entity.BillingGroupID = null;
                    entityDao.Update(entity);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Charges Template Item Service
        //private void InitializeChargesTemplateItemService(string queryString)
        //{
        //    lblHeader.InnerText = "Template Code";
        //    lblHeader2.InnerText = "Template Name";

        //    ChargesTemplateHd tth = BusinessLayer.GetChargesTemplateHd(Convert.ToInt32(queryString));
        //    txtHeader.Text = tth.ChargesTemplateCode;
        //    txtHeader2.Text = tth.ChargesTemplateName;

        //    vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", tth.HealthcareServiceUnitID)).FirstOrDefault();

        //    string filterExpression = string.Format("ItemID NOT IN (SELECT ItemID FROM ChargesTemplateDt WHERE ChargesTemplateID = {0}) AND ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = {1}) AND GCItemStatus != '{2}' AND IsDeleted = 0 ORDER BY ItemName1", queryString, tth.HealthcareServiceUnitID, Constant.ItemStatus.IN_ACTIVE);
        //    //filterExpression += string.Format(" OR ItemID IN (SELECT ItemID FROM vItemBalanceQuickPick WHERE LocationID = '{0}' AND IsDeleted = 0 AND GCItemStatus != '{1}' AND GCItemType IN ('{2}','{3}') AND IsDeleted = 0)) AND IsDeleted = 0 AND GCItemStatus != '{4}'", vsu.LocationID, Constant.ItemStatus.IN_ACTIVE, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemStatus.IN_ACTIVE);
        //    //filterExpression += string.Format(" OR ItemID IN (SELECT ItemID FROM vItemBalance WHERE LocationID = {0} AND GCItemType = '{1}' AND IsDeleted = 0 AND IsChargeToPatient = 1 AND QuantityEND > 0 AND GCItemStatus != '{2}') ORDER BY ItemName1", vsu.LogisticLocationID, Constant.ItemType.BARANG_UMUM, Constant.ItemStatus.IN_ACTIVE);
        //    List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(filterExpression);
        //    List<vChargesTemplateDt> ListSelectedItem = BusinessLayer.GetvChargesTemplateDtList(string.Format(
        //        "ChargesTemplateID = {0} AND ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = {1}) ORDER BY ItemName1 ASC",
        //        queryString, tth.HealthcareServiceUnitID));

        //    ListAvailable = (from p in ListAvailableItem
        //                     select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();

        //    ListSelected = (from p in ListSelectedItem
        //                    select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();
        //}

        //private bool SaveChargesTemplateItemService(string queryString, ref string errMessage)
        //{
        //    IDbContext ctx = DbFactory.Configure(true);
        //    bool result = false;
        //    try
        //    {
        //        int ChargesTemplateID = Convert.ToInt32(queryString);
        //        ChargesTemplateDtDao entityDao = new ChargesTemplateDtDao(ctx);
        //        foreach (ProceedEntity row in ListProceedEntity)
        //        {
        //            if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
        //            {
        //                ChargesTemplateDt entity = new ChargesTemplateDt();
        //                entity.ChargesTemplateID = ChargesTemplateID;
        //                entity.ItemID = Int32.Parse(row.ID);
        //                entityDao.Insert(entity);
        //            }
        //            else
        //            {
        //                entityDao.Delete(ChargesTemplateID, Int32.Parse(row.ID));
        //            }
        //        }
        //        ctx.CommitTransaction();
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ctx.RollBackTransaction();
        //        result = false;
        //        errMessage = ex.Message;
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //    return result;
        //}
        #endregion
        #region Charges Template Item Drug
        //private void InitializeChargesTemplateItemDrug(string queryString)
        //{
        //    lblHeader.InnerText = "Template Code";
        //    lblHeader2.InnerText = "Template Name";

        //    ChargesTemplateHd tth = BusinessLayer.GetChargesTemplateHd(Convert.ToInt32(queryString));
        //    txtHeader.Text = tth.ChargesTemplateCode;
        //    txtHeader2.Text = tth.ChargesTemplateName;

        //    vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", tth.HealthcareServiceUnitID)).FirstOrDefault();

        //    string filterExpression = string.Format("ItemID NOT IN (SELECT ItemID FROM ChargesTemplateDt WHERE ChargesTemplateID = {0}) AND ItemID IN (SELECT ItemID FROM vItemBalanceQuickPick WHERE LocationID = '{1}' AND IsDeleted = 0 AND GCItemStatus != '{2}' AND GCItemType IN ('{3}','{4}') AND IsDeleted = 0) AND IsDeleted = 0 AND GCItemStatus != '{2}' ORDER BY ItemName1", queryString, vsu.LocationID, Constant.ItemStatus.IN_ACTIVE, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
        //    List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(filterExpression);
        //    List<vChargesTemplateDt> ListSelectedItem = BusinessLayer.GetvChargesTemplateDtList(string.Format(
        //        "ChargesTemplateID = {0} AND DetailGCItemType IN ('{1}','{2}') ORDER BY ItemName1 ASC",
        //        queryString, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS));

        //    ListAvailable = (from p in ListAvailableItem
        //                     select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();

        //    ListSelected = (from p in ListSelectedItem
        //                    select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();
        //}

        //private bool SaveChargesTemplateItemDrug(string queryString, ref string errMessage)
        //{
        //    IDbContext ctx = DbFactory.Configure(true);
        //    bool result = false;
        //    try
        //    {
        //        int ChargesTemplateID = Convert.ToInt32(queryString);
        //        ChargesTemplateDtDao entityDao = new ChargesTemplateDtDao(ctx);
        //        foreach (ProceedEntity row in ListProceedEntity)
        //        {
        //            if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
        //            {
        //                ChargesTemplateDt entity = new ChargesTemplateDt();
        //                entity.ChargesTemplateID = ChargesTemplateID;
        //                entity.ItemID = Int32.Parse(row.ID);
        //                entityDao.Insert(entity);
        //            }
        //            else
        //            {
        //                entityDao.Delete(ChargesTemplateID, Int32.Parse(row.ID));
        //            }
        //        }
        //        ctx.CommitTransaction();
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ctx.RollBackTransaction();
        //        result = false;
        //        errMessage = ex.Message;
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //    return result;
        //}
        #endregion
        #region Charges Template Item Logistic
        //private void InitializeChargesTemplateItemLogistic(string queryString)
        //{
        //    lblHeader.InnerText = "Template Code";
        //    lblHeader2.InnerText = "Template Name";

        //    ChargesTemplateHd tth = BusinessLayer.GetChargesTemplateHd(Convert.ToInt32(queryString));
        //    txtHeader.Text = tth.ChargesTemplateCode;
        //    txtHeader2.Text = tth.ChargesTemplateName;

        //    vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", tth.HealthcareServiceUnitID)).FirstOrDefault();

        //    string filterExpression = string.Format("ItemID NOT IN (SELECT ItemID FROM ChargesTemplateDt WHERE ChargesTemplateID = {0}) AND ItemID IN (SELECT ItemID FROM vItemBalance WHERE LocationID = {2} AND GCItemType = '{3}' AND IsDeleted = 0 AND IsChargeToPatient = 1 AND QuantityEND > 0 AND GCItemStatus != '{4}') ORDER BY ItemName1", queryString, tth.HealthcareServiceUnitID, vsu.LogisticLocationID, Constant.ItemType.BARANG_UMUM, Constant.ItemStatus.IN_ACTIVE);
        //    List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(filterExpression);
        //    List<vChargesTemplateDt> ListSelectedItem = BusinessLayer.GetvChargesTemplateDtList(string.Format(
        //        "ChargesTemplateID = {0} AND DetailGCItemType IN ('{1}') ORDER BY ItemName1 ASC",
        //        queryString, Constant.ItemType.BARANG_UMUM));

        //    ListAvailable = (from p in ListAvailableItem
        //                     select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();

        //    ListSelected = (from p in ListSelectedItem
        //                    select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();
        //}

        //private bool SaveChargesTemplateItemLogistic(string queryString, ref string errMessage)
        //{
        //    IDbContext ctx = DbFactory.Configure(true);
        //    bool result = false;
        //    try
        //    {
        //        int ChargesTemplateID = Convert.ToInt32(queryString);
        //        ChargesTemplateDtDao entityDao = new ChargesTemplateDtDao(ctx);
        //        foreach (ProceedEntity row in ListProceedEntity)
        //        {
        //            if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
        //            {
        //                ChargesTemplateDt entity = new ChargesTemplateDt();
        //                entity.ChargesTemplateID = ChargesTemplateID;
        //                entity.ItemID = Int32.Parse(row.ID);
        //                entityDao.Insert(entity);
        //            }
        //            else
        //            {
        //                entityDao.Delete(ChargesTemplateID, Int32.Parse(row.ID));
        //            }
        //        }
        //        ctx.CommitTransaction();
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ctx.RollBackTransaction();
        //        result = false;
        //        errMessage = ex.Message;
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //    return result;
        //}
        #endregion
        #region Contract Coverage
        private void InitializeContractCoverage(string queryString)
        {
            lblHeader.InnerText = "Contract No";

            CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(queryString));
            txtHeader.Text = entity.ContractNo;

            List<CoverageType> ListAvailableCoverage = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeID NOT IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = {0}) AND IsDeleted = 0 ORDER BY CoverageTypeName ASC", queryString));
            List<vContractCoverage> ListSelectedCoverage = BusinessLayer.GetvContractCoverageList(string.Format("ContractID = {0} ORDER BY CoverageTypeName ASC", queryString));

            ListAvailable = (from p in ListAvailableCoverage.OrderBy(p => p.CoverageTypeName)
                             select new CMatrix { IsChecked = false, ID = p.CoverageTypeID.ToString(), Name = p.CoverageTypeName }).ToList();

            ListSelected = (from p in ListSelectedCoverage.OrderBy(p => p.CoverageTypeName)
                            select new CMatrix { IsChecked = false, ID = p.CoverageTypeID.ToString(), Name = p.CoverageTypeName }).ToList();
        }

        private bool SaveContractCoverage(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ContractID = Convert.ToInt32(queryString);
                ContractCoverageDao entityDao = new ContractCoverageDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ContractCoverage entity = new ContractCoverage();
                        entity.ContractID = ContractID;
                        entity.CoverageTypeID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ContractID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Contract Coverage Member
        private void InitializeContractCoverageMember(string queryString)
        {
            lblHeader.InnerText = "Contract No";
            lblHeader2.InnerText = "Coverage Type";

            string[] param = queryString.Split('|');

            CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(param[0]));
            CoverageType entityCoverageType = BusinessLayer.GetCoverageType(Convert.ToInt32(param[1]));
            txtHeader.Text = entity.ContractNo;
            txtHeader2.Text = entityCoverageType.CoverageTypeName;

            List<vPatient> ListAvailableMember = BusinessLayer.GetvPatientList(string.Format("MRN IN (SELECT MRN FROM CustomerMember WHERE BusinessPartnerID = {2}) AND MRN NOT IN (SELECT MRN FROM ContractCoverageMember WHERE ContractID = {0} AND CoverageTypeID = {1})", param[0], param[1], entity.BusinessPartnerID));
            List<vContractCoverageMember> ListSelectedMember = BusinessLayer.GetvContractCoverageMemberList(string.Format("ContractID = {0} AND CoverageTypeID = {1}", param[0], param[1]));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.PatientName)
                             select new CMatrix { IsChecked = false, ID = p.MRN.ToString(), Name = p.PatientName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.PatientName)
                            select new CMatrix { IsChecked = false, ID = p.MRN.ToString(), Name = p.PatientName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveContractCoverageMember(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                string[] param = queryString.Split('|');
                int ContractID = Convert.ToInt32(param[0]);
                int CoverageTypeID = Convert.ToInt32(param[1]);
                ContractCoverageMemberDao entityDao = new ContractCoverageMemberDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ContractCoverageMember entity = new ContractCoverageMember();
                        entity.ContractID = ContractID;
                        entity.CoverageTypeID = CoverageTypeID;
                        entity.MRN = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ContractID, CoverageTypeID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Customer Diagnose
        private void InitializeCustomersDiagnose(string queryString)
        {
            lblHeader.InnerText = "Diagnosa";

            CustomerDiagnoseHd chd = BusinessLayer.GetCustomerDiagnoseHd(Convert.ToInt32(queryString));
            txtHeader.Text = chd.CustomerDiagnoseName;

            List<Diagnose> ListAvailableItem = BusinessLayer.GetDiagnoseList(string.Format("DiagnoseID NOT IN (SELECT DiagnoseID FROM CustomerDiagnoseDt WHERE CustomerDiagnoseID = {0}) AND IsDeleted = 0 ORDER BY DiagnoseName ASC", queryString));
            List<vCustomerDiagnoseDt> ListSelectedItem = BusinessLayer.GetvCustomerDiagnoseDtList(string.Format("CustomerDiagnoseID = {0} AND IsDeleted = 0 ORDER BY DiagnoseName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.DiagnoseName)
                             select new CMatrix { IsChecked = false, ID = p.DiagnoseID.ToString(), Name = p.DiagnoseName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.DiagnoseName)
                            select new CMatrix { IsChecked = false, ID = p.DiagnoseID.ToString(), Name = p.DiagnoseName }).ToList();
        }

        private bool SaveCustomersDiagnose(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int CustomerDiagnoseID = Convert.ToInt32(queryString);
                CustomerDiagnoseDtDao entityDao = new CustomerDiagnoseDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {

                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        CustomerDiagnoseDt entity = new CustomerDiagnoseDt();
                        entity.CustomerDiagnoseID = CustomerDiagnoseID;
                        entity.DiagnoseID = row.ID;
                        entity.IsDeleted = false;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = "";
                        filter = string.Format("CustomerDiagnoseID = {0} AND DiagnoseID = '{1}' AND IsDeleted = 0", CustomerDiagnoseID, row.ID);
                        CustomerDiagnoseDt cdd = BusinessLayer.GetCustomerDiagnoseDtList(filter, ctx).FirstOrDefault();
                        if (cdd != null)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            CustomerDiagnoseDt cdt = entityDao.Get(cdd.ID);
                            cdt.IsDeleted = true;
                            cdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(cdt);
                        }
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Customer Member
        private void InitializeCustomerMember(string queryString)
        {
            lblHeader.InnerText = "Customer";

            BusinessPartners hsu = BusinessLayer.GetBusinessPartners(Convert.ToInt32(queryString));
            txtHeader.Text = string.Format("{0} - {1}", hsu.BusinessPartnerCode, hsu.BusinessPartnerName);

            List<vPatient> ListAvailableMember = BusinessLayer.GetvPatientList(string.Format("MRN NOT IN (SELECT MRN FROM CustomerMember WHERE BusinessPartnerID = {0})", queryString));
            List<vCustomerMember> ListSelectedMember = BusinessLayer.GetvCustomerMemberList(string.Format("BusinessPartnerID = {0}", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.PatientName)
                             select new CMatrix { IsChecked = false, ID = p.MRN.ToString(), Name = p.PatientName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.PatientName)
                            select new CMatrix { IsChecked = false, ID = p.MRN.ToString(), Name = p.PatientName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveCustomerMember(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int CustomerID = Convert.ToInt32(queryString);
                CustomerMemberDao entityDao = new CustomerMemberDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        CustomerMember entity = new CustomerMember();
                        entity.BusinessPartnerID = CustomerID;
                        entity.MRN = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(CustomerID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Diagnose Drugs
        private void InitializeDiagnoseDrugs(string queryString)
        {
            lblHeader.InnerText = "Diagnose";

            Diagnose diagnose = BusinessLayer.GetDiagnose(queryString);
            txtHeader.Text = string.Format("{0} - {1}", diagnose.DiagnoseID, diagnose.DiagnoseName);

            List<ItemMaster> ListAvailableItemDrugs = BusinessLayer.GetItemMasterList(string.Format("ItemID NOT IN (SELECT ItemID FROM DiagnoseItem WHERE DiagnoseID = '{0}') AND GCItemType = '{1}'", diagnose.DiagnoseID, Constant.ItemGroupMaster.DRUGS, queryString));
            List<ItemMaster> ListSelectedItemDrugs = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM DiagnoseItem WHERE DiagnoseID = '{0}') AND GCItemType = '{1}'", diagnose.DiagnoseID, Constant.ItemGroupMaster.DRUGS, queryString));

            ListAvailable = (from p in ListAvailableItemDrugs.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItemDrugs.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();
        }

        private bool SaveDiagnoseDrugs(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                string DiagnoseID = queryString;
                DiagnoseItemDao entityDao = new DiagnoseItemDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        DiagnoseItem entity = new DiagnoseItem();
                        entity.DiagnoseID = DiagnoseID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(DiagnoseID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Diagnose Test
        private void InitializeDiagnoseTest(string queryString)
        {
            lblHeader.InnerText = "Diagnose";

            Diagnose diagnose = BusinessLayer.GetDiagnose(queryString);
            txtHeader.Text = string.Format("{0} - {1}", diagnose.DiagnoseID, diagnose.DiagnoseName);

            List<ItemMaster> ListAvailableItemTest = BusinessLayer.GetItemMasterList(string.Format("ItemID NOT IN (SELECT ItemID FROM DiagnoseItem WHERE DiagnoseID = '{0}') AND GCItemType IN('{1}', '{2}','{3}')", diagnose.DiagnoseID, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, queryString));
            List<ItemMaster> ListSelectedItemTest = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM DiagnoseItem WHERE DiagnoseID = '{0}') AND GCItemType IN('{1}', '{2}','{3}')", diagnose.DiagnoseID, Constant.ItemGroupMaster.LABORATORY, Constant.ItemGroupMaster.RADIOLOGY, Constant.ItemGroupMaster.DIAGNOSTIC, queryString));

            ListAvailable = (from p in ListAvailableItemTest.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItemTest.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();
        }

        private bool SaveDiagnoseTest(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                string DiagnoseID = queryString;
                DiagnoseItemDao entityDao = new DiagnoseItemDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        DiagnoseItem entity = new DiagnoseItem();
                        entity.DiagnoseID = DiagnoseID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(DiagnoseID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL Account Payable
        private void InitializeGLAccountPayable(string queryString)
        {
            lblHeader.InnerText = "Jenis";
            lblHeader2.InnerText = "Tipe Item";

            vGLAccountPayable entity = BusinessLayer.GetvGLAccountPayableList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.AccountPayableType;
            txtHeader2.Text = entity.ItemType;

            List<BusinessPartners> ListAvailableMember = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID NOT IN (SELECT BusinessPartnerID FROM GLAccountPayableDt WHERE ID = {0}) AND GCBusinessPartnerType = '{1}' AND IsDeleted = 0", queryString, Constant.BusinessObjectType.SUPPLIER));
            List<BusinessPartners> ListSelectedMember = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM GLAccountPayableDt WHERE ID = {0}) AND IsDeleted = 0", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.BusinessPartnerName)
                             select new CMatrix { IsChecked = false, ID = p.BusinessPartnerID.ToString(), Name = p.BusinessPartnerName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.BusinessPartnerName)
                            select new CMatrix { IsChecked = false, ID = p.BusinessPartnerID.ToString(), Name = p.BusinessPartnerName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLAccountPayable(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLAccountPayableDtDao entityDao = new GLAccountPayableDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLAccountPayableDt entity = new GLAccountPayableDt();
                        entity.ID = ID;
                        entity.BusinessPartnerID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL Account Receivable
        private void InitializeGLAccountReceivable(string queryString)
        {
            lblHeader.InnerText = "Jenis";
            lblHeader2.InnerText = "Kelompok";

            vGLAccountReceivable entity = BusinessLayer.GetvGLAccountReceivableList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.AccountReceivableType;
            txtHeader2.Text = entity.ARTransactionGroup;

            List<BusinessPartners> ListAvailableMember = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID NOT IN (SELECT BusinessPartnerID FROM GLAccountReceivableDt WHERE ID = {0}) AND GCBusinessPartnerType = '{1}' AND IsDeleted = 0", queryString, Constant.BusinessObjectType.CUSTOMER));
            List<BusinessPartners> ListSelectedMember = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM GLAccountReceivableDt WHERE ID = {0}) AND IsDeleted = 0", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.BusinessPartnerName)
                             select new CMatrix { IsChecked = false, ID = p.BusinessPartnerID.ToString(), Name = p.BusinessPartnerName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.BusinessPartnerName)
                            select new CMatrix { IsChecked = false, ID = p.BusinessPartnerID.ToString(), Name = p.BusinessPartnerName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLAccountReceivable(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLAccountReceivableDtDao entityDao = new GLAccountReceivableDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLAccountReceivableDt entity = new GLAccountReceivableDt();
                        entity.ID = ID;
                        entity.BusinessPartnerID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL AP Revenue Sharing
        private void InitializeGLAPRevenueSharing(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLAPRevenueSharing entity = BusinessLayer.GetvGLAPRevenueSharingList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            List<vServiceUnitParamedic> ListAvailableMember = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID NOT IN (SELECT ParamedicID FROM GLAPRevenueSharingDt WHERE ID = {0}) AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));
            List<vServiceUnitParamedic> ListSelectedMember = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID IN (SELECT ParamedicID FROM GLAPRevenueSharingDt WHERE ID = {0}) AND IsDeleted = 0", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ParamedicName)
                             select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.ParamedicName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ParamedicName)
                            select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.ParamedicName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLAPRevenueSharing(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLAPRevenueSharingDtDao entityDao = new GLAPRevenueSharingDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLAPRevenueSharingDt entity = new GLAPRevenueSharingDt();
                        entity.ID = ID;
                        entity.ParamedicID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL Cash Flow Account Dt
        private void InitializeGLCashFlowAccountDt(string queryString)
        {
            lblHeader.InnerText = "Content Code";
            lblHeader2.InnerText = "Content Name";

            vGLCashFlowAccountHd entity = BusinessLayer.GetvGLCashFlowAccountHdList(string.Format("GLCashFlowAccountID = {0}", queryString))[0];
            txtHeader.Text = entity.ContentCode;
            txtHeader2.Text = entity.ContentName;

            List<ChartOfAccount> ListAvailableMember = BusinessLayer.GetChartOfAccountList(string.Format("GLAccountID NOT IN (SELECT GLAccountID FROM GLCashFlowAccountDt WHERE GLCashFlowAccountID = {0} AND IsDeleted = 0) AND IsDeleted = 0 ORDER BY GLAccountNo", queryString));
            List<vGLCashFlowAccountDt> ListSelectedMember = BusinessLayer.GetvGLCashFlowAccountDtList(string.Format("GLCashFlowAccountID = {0} AND IsDeleted = 0 ORDER BY GLAccountNo", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.GLAccountNo)
                             select new CMatrix { IsChecked = false, ID = p.GLAccountID.ToString(), Name = "(" + p.GLAccountNo + ") " + p.GLAccountName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.GLAccountNo)
                            select new CMatrix { IsChecked = false, ID = p.GLAccountID.ToString(), Name = "(" + p.GLAccountNo + ") " + p.GLAccountName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLCashFlowAccountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int GLCashFlowAccountID = Convert.ToInt32(queryString);
                GLCashFlowAccountDtDao entityDao = new GLCashFlowAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLCashFlowAccountDt entity = new GLCashFlowAccountDt();
                        entity.GLCashFlowAccountID = GLCashFlowAccountID;
                        entity.GLAccountID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = "";
                        filter = string.Format("GLCashFlowAccountID = {0} AND GLAccountID = '{1}' AND IsDeleted = 0", GLCashFlowAccountID, row.ID);
                        GLCashFlowAccountDt dt = BusinessLayer.GetGLCashFlowAccountDtList(filter, ctx).FirstOrDefault();
                        if (dt != null)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            GLCashFlowAccountDt cdt = entityDao.Get(dt.ID);
                            cdt.IsDeleted = true;
                            cdt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(cdt);
                        }
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL IP Final Discount Dt
        private void InitializeGLIPFinalDiscountDt(string queryString)
        {
            lblHeader.InnerText = "Department";
            lblHeader2.InnerText = "Catatan";

            vGLFinalDiscountAccount entity = BusinessLayer.GetvGLFinalDiscountAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.DepartmentName;
            txtHeader2.Text = entity.Remarks;

            List<vHealthcareServiceUnitClassCare> ListAvailableMember = BusinessLayer.GetvHealthcareServiceUnitClassCareList(string.Format("NOT EXISTS (SELECT 1 FROM GLFinalDiscountAccountDt fd WHERE fd.HealthcareServiceUnitID = vHealthcareServiceUnitClassCare.ClassID and fd.ClassID = vHealthcareServiceUnitClassCare.ClassID AND fd.ID = {0})", queryString));
            List<vGLFinalDiscountAccountDt> ListSelectedMember = BusinessLayer.GetvGLFinalDiscountAccountDtList(string.Format("ID = {0}", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ServiceUnitName)
                             select new CMatrix { IsChecked = false, ID = string.Format("{0}_{1}", p.HealthcareServiceUnitID, p.ClassID), Name = string.Format("{0} ({1})", p.ServiceUnitName, p.ClassName) }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ServiceUnitName)
                            select new CMatrix { IsChecked = false, ID = string.Format("{0}_{1}", p.HealthcareServiceUnitID, p.ClassID), Name = string.Format("{0} ({1})", p.ServiceUnitName, p.ClassName) }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLIPFinalDiscountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLFinalDiscountAccountDtDao entityDao = new GLFinalDiscountAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    string[] temp = row.ID.Split('_');
                    int healthcareServiceUnitID = Convert.ToInt32(temp[0]);
                    int classID = Convert.ToInt32(temp[1]);
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLFinalDiscountAccountDt entity = new GLFinalDiscountAccountDt();
                        entity.ID = ID;
                        entity.HealthcareServiceUnitID = healthcareServiceUnitID;
                        entity.ClassID = classID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, healthcareServiceUnitID, classID);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL IP Revenue Account Drug MS
        private void InitializeGLIPRevenueAccountDrugMS(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLIPRevenueAccount entity = BusinessLayer.GetvGLIPRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            int LocationID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID))[0]).LocationID;

            List<vItemBalance> ListAvailableMember = BusinessLayer.GetvItemBalanceList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLIPRevenueAccountDt WHERE ID = {0}) AND LocationID = {1} AND IsDeleted = 0", queryString, LocationID));
            List<ItemMaster> ListSelectedMember = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM GLIPRevenueAccountDt WHERE ID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0", queryString, Constant.ItemGroupMaster.DRUGS));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLIPRevenueAccountDrugMS(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLIPRevenueAccountDtDao entityDao = new GLIPRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLIPRevenueAccountDt entity = new GLIPRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL IP Revenue Account Logistic
        private void InitializeGLIPRevenueAccountLogistic(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLIPRevenueAccount entity = BusinessLayer.GetvGLIPRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            int LocationID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID))[0]).LogisticLocationID;

            List<vItemBalance> ListAvailableMember = BusinessLayer.GetvItemBalanceList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLIPRevenueAccountDt WHERE ID = {0}) AND LocationID = {1} AND IsDeleted = 0", queryString, LocationID));
            List<ItemMaster> ListSelectedMember = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM GLIPRevenueAccountDt WHERE ID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0", queryString, Constant.ItemGroupMaster.DRUGS));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLIPRevenueAccountLogistic(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLIPRevenueAccountDtDao entityDao = new GLIPRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLIPRevenueAccountDt entity = new GLIPRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL IP Revenue Account Service
        private void InitializeGLIPRevenueAccountService(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLIPRevenueAccount entity = BusinessLayer.GetvGLIPRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            List<vServiceUnitItem> ListAvailableMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLIPRevenueAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));
            List<vServiceUnitItem> ListSelectedMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID IN (SELECT ItemID FROM GLIPRevenueAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLIPRevenueAccountService(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLIPRevenueAccountDtDao entityDao = new GLIPRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLIPRevenueAccountDt entity = new GLIPRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL IP Revenue Sharing Account Dt
        private void InitializeGLIPRevenueSharingAccountDt(string queryString)
        {
            lblHeader.InnerText = "Unit Pelayanan";
            lblHeader2.InnerText = "Sharing Component";

            vGLIPRevenueSharingAccount entity = BusinessLayer.GetvGLIPRevenueSharingAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.ServiceUnitName;
            txtHeader2.Text = entity.SharingComponent;

            List<vServiceUnitItem> ListAvailableMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLIPRevenueSharingAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));
            List<vServiceUnitItem> ListSelectedMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID IN (SELECT ItemID FROM GLIPRevenueSharingAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLIPRevenueSharingAccountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLIPRevenueSharingAccountDtDao entityDao = new GLIPRevenueSharingAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLIPRevenueSharingAccountDt entity = new GLIPRevenueSharingAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL MD Revenue Account Drug MS
        private void InitializeGLMDRevenueAccountDrugMS(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLMDRevenueAccount entity = BusinessLayer.GetvGLMDRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            int LocationID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID))[0]).LocationID;

            List<vItemBalance> ListAvailableMember = BusinessLayer.GetvItemBalanceList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLMDRevenueAccountDt WHERE ID = {0}) AND LocationID = {1} AND IsDeleted = 0", queryString, LocationID));
            List<ItemMaster> ListSelectedMember = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM GLMDRevenueAccountDt WHERE ID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0", queryString, Constant.ItemGroupMaster.DRUGS));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLMDRevenueAccountDrugMS(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLMDRevenueAccountDtDao entityDao = new GLMDRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLMDRevenueAccountDt entity = new GLMDRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL MD Revenue Account Logistic
        private void InitializeGLMDRevenueAccountLogistic(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLMDRevenueAccount entity = BusinessLayer.GetvGLMDRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            int LocationID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID))[0]).LogisticLocationID;

            List<vItemBalance> ListAvailableMember = BusinessLayer.GetvItemBalanceList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLMDRevenueAccountDt WHERE ID = {0}) AND LocationID = {1} AND IsDeleted = 0", queryString, LocationID));
            List<ItemMaster> ListSelectedMember = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM GLMDRevenueAccountDt WHERE ID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0", queryString, Constant.ItemGroupMaster.DRUGS));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLMDRevenueAccountLogistic(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLMDRevenueAccountDtDao entityDao = new GLMDRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLMDRevenueAccountDt entity = new GLMDRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL MD Revenue Account Service
        private void InitializeGLMDRevenueAccountService(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLMDRevenueAccount entity = BusinessLayer.GetvGLMDRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            List<vServiceUnitItem> ListAvailableMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLMDRevenueAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));
            List<vServiceUnitItem> ListSelectedMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID IN (SELECT ItemID FROM GLMDRevenueAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLMDRevenueAccountService(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLMDRevenueAccountDtDao entityDao = new GLMDRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLMDRevenueAccountDt entity = new GLMDRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL MD Revenue Sharing Account Dt
        private void InitializeGLMDRevenueSharingAccountDt(string queryString)
        {
            lblHeader.InnerText = "Unit Pelayanan";
            lblHeader2.InnerText = "Sharing Component";

            vGLMDRevenueSharingAccount entity = BusinessLayer.GetvGLMDRevenueSharingAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.ServiceUnitName;
            txtHeader2.Text = entity.SharingComponent;

            List<vServiceUnitItem> ListAvailableMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLMDRevenueSharingAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));
            List<vServiceUnitItem> ListSelectedMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID IN (SELECT ItemID FROM GLMDRevenueSharingAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLMDRevenueSharingAccountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLMDRevenueSharingAccountDtDao entityDao = new GLMDRevenueSharingAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLMDRevenueSharingAccountDt entity = new GLMDRevenueSharingAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL OP Final Discount Dt
        private void InitializeGLOPFinalDiscountDt(string queryString)
        {
            lblHeader.InnerText = "Department";
            lblHeader2.InnerText = "Catatan";

            vGLFinalDiscountAccount entity = BusinessLayer.GetvGLFinalDiscountAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.DepartmentName;
            txtHeader2.Text = entity.Remarks;

            List<vHealthcareServiceUnit> ListAvailableMember = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID NOT IN (SELECT HealthcareServiceUnitID FROM GLOTCRevenueAccountDt WHERE ID = {0}) AND DepartmentID = '{1}' AND IsDeleted = 0", queryString, entity.DepartmentID));
            List<vGLFinalDiscountAccountDt> ListSelectedMember = BusinessLayer.GetvGLFinalDiscountAccountDtList(string.Format("ID = {0}", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ServiceUnitName)
                             select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ServiceUnitName)
                            select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLOPFinalDiscountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLFinalDiscountAccountDtDao entityDao = new GLFinalDiscountAccountDtDao(ctx);
                int classID = Convert.ToInt32(BusinessLayer.GetSettingParameter(Constant.SettingParameter.OUTPATIENT_CLASS).ParameterValue);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLFinalDiscountAccountDt entity = new GLFinalDiscountAccountDt();
                        entity.ID = ID;
                        entity.HealthcareServiceUnitID = Convert.ToInt32(row.ID);
                        entity.ClassID = classID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID), classID);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL OP Revenue Account Drug MS
        private void InitializeGLOPRevenueAccountDrugMS(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLOPRevenueAccount entity = BusinessLayer.GetvGLOPRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            int LocationID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID))[0]).LocationID;

            List<vItemBalance> ListAvailableMember = BusinessLayer.GetvItemBalanceList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLOPRevenueAccountDt WHERE ID = {0}) AND LocationID = {1} AND IsDeleted = 0", queryString, LocationID));
            List<ItemMaster> ListSelectedMember = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM GLOPRevenueAccountDt WHERE ID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0", queryString, Constant.ItemGroupMaster.DRUGS));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLOPRevenueAccountDrugMS(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLOPRevenueAccountDtDao entityDao = new GLOPRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLOPRevenueAccountDt entity = new GLOPRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL OP Revenue Account Logistic
        private void InitializeGLOPRevenueAccountLogistic(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLOPRevenueAccount entity = BusinessLayer.GetvGLOPRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            int LocationID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID))[0]).LogisticLocationID;

            List<vItemBalance> ListAvailableMember = BusinessLayer.GetvItemBalanceList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLOPRevenueAccountDt WHERE ID = {0}) AND LocationID = {1} AND IsDeleted = 0", queryString, LocationID));
            List<ItemMaster> ListSelectedMember = BusinessLayer.GetItemMasterList(string.Format("ItemID IN (SELECT ItemID FROM GLOPRevenueAccountDt WHERE ID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0", queryString, Constant.ItemGroupMaster.DRUGS));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLOPRevenueAccountLogistic(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLOPRevenueAccountDtDao entityDao = new GLOPRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLOPRevenueAccountDt entity = new GLOPRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL OP Revenue Account Service
        private void InitializeGLOPRevenueAccountService(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            lblHeader2.InnerText = "Unit Pelayanan";

            vGLOPRevenueAccount entity = BusinessLayer.GetvGLOPRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            txtHeader2.Text = entity.ServiceUnitName;

            List<vServiceUnitItem> ListAvailableMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLOPRevenueAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));
            List<vServiceUnitItem> ListSelectedMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID IN (SELECT ItemID FROM GLOPRevenueAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLOPRevenueAccountService(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLOPRevenueAccountDtDao entityDao = new GLOPRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLOPRevenueAccountDt entity = new GLOPRevenueAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL OP Revenue Sharing Account Dt
        private void InitializeGLOPRevenueSharingAccountDt(string queryString)
        {
            lblHeader.InnerText = "Unit Pelayanan";
            lblHeader2.InnerText = "Sharing Component";

            vGLOPRevenueSharingAccount entity = BusinessLayer.GetvGLOPRevenueSharingAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.ServiceUnitName;
            txtHeader2.Text = entity.SharingComponent;

            List<vServiceUnitItem> ListAvailableMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID NOT IN (SELECT ItemID FROM GLOPRevenueSharingAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));
            List<vServiceUnitItem> ListSelectedMember = BusinessLayer.GetvServiceUnitItemList(string.Format("ItemID IN (SELECT ItemID FROM GLOPRevenueSharingAccountDt WHERE ID = {0}) AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", queryString, entity.HealthcareServiceUnitID));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLOPRevenueSharingAccountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLOPRevenueSharingAccountDtDao entityDao = new GLOPRevenueSharingAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLOPRevenueSharingAccountDt entity = new GLOPRevenueSharingAccountDt();
                        entity.ID = ID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL OTC Revenue Account Dt
        private void InitializeGLOTCRevenueAccountDt(string queryString)
        {
            lblHeader.InnerText = "Revenue Transaction";
            //lblHeader2.InnerText = "Unit Pelayanan";

            vGLOTCRevenueAccount entity = BusinessLayer.GetvGLOTCRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLRevenueTransaction;
            //txtHeader2.Text = entity.GLRevenueTransaction;

            List<ProductLine> ListAvailableMember = BusinessLayer.GetProductLineList(string.Format("ProductLineID NOT IN (SELECT ProductLineID FROM GLOTCRevenueAccountDt WHERE ID = {0}) AND IsDeleted = 0", queryString));
            List<ProductLine> ListSelectedMember = BusinessLayer.GetProductLineList(string.Format("ProductLineID IN (SELECT ProductLineID FROM GLOTCRevenueAccountDt WHERE ID = {0}) AND IsDeleted = 0", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ProductLineName)
                             select new CMatrix { IsChecked = false, ID = p.ProductLineID.ToString(), Name = p.ProductLineName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ProductLineName)
                            select new CMatrix { IsChecked = false, ID = p.ProductLineID.ToString(), Name = p.ProductLineName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLOTCRevenueAccountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLOTCRevenueAccountDtDao entityDao = new GLOTCRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLOTCRevenueAccountDt entity = new GLOTCRevenueAccountDt();
                        entity.ID = ID;
                        entity.ProductLineID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL Payment Method
        private void InitializeGLPaymentMethod(string queryString)
        {
            lblHeader.InnerText = "Kelompok Transaksi";
            lblHeader2.InnerText = "Cara Pembayaran";

            vGLPaymentMethod entity = BusinessLayer.GetvGLPaymentMethodList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLTransactionGroup;
            txtHeader2.Text = entity.PaymentMethod;

            string DepartmentID = "";
            switch (entity.GCGLTransactionGroup)
            {
                case Constant.GLTransactionGroup.EMERGENCY: DepartmentID = Constant.Facility.EMERGENCY; break;
                case Constant.GLTransactionGroup.INPATIENT: DepartmentID = Constant.Facility.INPATIENT; break;
                case Constant.GLTransactionGroup.OUTPATIENT: DepartmentID = Constant.Facility.OUTPATIENT; break;
                case Constant.GLTransactionGroup.MEDICAL_DIAGNOSTIC: DepartmentID = Constant.Facility.DIAGNOSTIC; break;
                case Constant.GLTransactionGroup.MEDICAL_CHECKUP: DepartmentID = Constant.Facility.MEDICAL_CHECKUP; break;
                case Constant.GLTransactionGroup.PHARMACY: DepartmentID = Constant.Facility.PHARMACY; break;
            }
            List<vHealthcareServiceUnit> ListAvailableMember = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN (SELECT HealthcareServiceUnitID FROM GLPaymentMethodDt WHERE ID = {1}) AND IsDeleted = 0", DepartmentID, queryString));
            List<vHealthcareServiceUnit> ListSelectedMember = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM GLPaymentMethodDt WHERE ID = {0})", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ServiceUnitName)
                             select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ServiceUnitName)
                            select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLPaymentMethod(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLPaymentMethodDtDao entityDao = new GLPaymentMethodDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLPaymentMethodDt entity = new GLPaymentMethodDt();
                        entity.ID = ID;
                        entity.HealthcareServiceUnitID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL Payment Type Dt
        private void InitializeGLPaymentTypeDt(string queryString)
        {
            lblHeader.InnerText = "Department | Payment Type";
            lblHeader2.InnerText = "COA";

            vGLPaymentTypeHd entity = BusinessLayer.GetvGLPaymentTypeHdList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.DepartmentID + " | " + entity.PaymentType;
            txtHeader2.Text = entity.GLAccountNo + " - " + entity.GLAccountName;

            List<vHealthcareServiceUnit> ListAvailableMember = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN (SELECT HealthcareServiceUnitID FROM GLPaymentTypeDt WHERE ID = {1}) AND IsDeleted = 0", entity.DepartmentID, queryString));
            List<vHealthcareServiceUnit> ListSelectedMember = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM GLPaymentTypeDt WHERE ID = {0})", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ServiceUnitName)
                             select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ServiceUnitName)
                            select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLPaymentTypeDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLPaymentTypeDtDao entityDao = new GLPaymentTypeDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLPaymentTypeDt entity = new GLPaymentTypeDt();
                        entity.ID = ID;
                        entity.HealthcareServiceUnitID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL Revenue Account
        private void InitializeGLRevenueAccount(string queryString)
        {
            lblHeader.InnerText = "Transaction Group";
            lblHeader2.InnerText = "Revenue Transaction";

            vGLRevenueAccount entity = BusinessLayer.GetvGLRevenueAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.GLTransactionGroup;
            txtHeader2.Text = entity.GLRevenueTransaction;

            string DepartmentID = "";
            switch (entity.GCGLTransactionGroup)
            {
                case Constant.GLTransactionGroup.EMERGENCY: DepartmentID = Constant.Facility.EMERGENCY; break;
                case Constant.GLTransactionGroup.INPATIENT: DepartmentID = Constant.Facility.INPATIENT; break;
                case Constant.GLTransactionGroup.OUTPATIENT: DepartmentID = Constant.Facility.OUTPATIENT; break;
                case Constant.GLTransactionGroup.MEDICAL_DIAGNOSTIC: DepartmentID = Constant.Facility.DIAGNOSTIC; break;
                case Constant.GLTransactionGroup.MEDICAL_CHECKUP: DepartmentID = Constant.Facility.MEDICAL_CHECKUP; break;
            }
            List<vHealthcareServiceUnit> ListAvailableMember = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN (SELECT HealthcareServiceUnitID FROM GLRevenueAccountDt WHERE ID = {1}) AND IsDeleted = 0", DepartmentID, queryString));
            List<vHealthcareServiceUnit> ListSelectedMember = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM GLRevenueAccountDt WHERE ID = {0})", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.ServiceUnitName)
                             select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.ServiceUnitName)
                            select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLRevenueAccount(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLRevenueAccountDtDao entityDao = new GLRevenueAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLRevenueAccountDt entity = new GLRevenueAccountDt();
                        entity.ID = ID;
                        entity.HealthcareServiceUnitID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region GL Warehouse Product Line Account Dt
        private void InitializeGLWarehouseProductLineAccountDt(string queryString)
        {
            lblHeader.InnerText = "Tipe Item";
            lblHeader2.InnerText = "Product Line";

            vGLWarehouseProductLineAccount entity = BusinessLayer.GetvGLWarehouseProductLineAccountList(string.Format("ID = {0}", queryString))[0];
            txtHeader.Text = entity.ItemType;
            txtHeader2.Text = entity.ProductLineName;

            List<Location> ListAvailableMember = BusinessLayer.GetLocationList(string.Format("LocationID NOT IN (SELECT LocationID FROM GLWarehouseProductLineAccountDt WHERE ID = {0}) AND IsHeader = 0 AND IsDeleted = 0", queryString));
            List<Location> ListSelectedMember = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM GLWarehouseProductLineAccountDt WHERE ID = {0}) AND IsDeleted = 0", queryString));

            ListAvailable = (from p in ListAvailableMember.OrderBy(p => p.LocationName)
                             select new CMatrix { IsChecked = false, ID = p.LocationID.ToString(), Name = p.LocationName }).OrderBy(p => p.Name).ToList();

            ListSelected = (from p in ListSelectedMember.OrderBy(p => p.LocationName)
                            select new CMatrix { IsChecked = false, ID = p.LocationID.ToString(), Name = p.LocationName }).OrderBy(p => p.Name).ToList();
        }

        private bool SaveGLWarehouseProductLineAccountDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(queryString);
                GLWarehouseProductLineAccountDtDao entityDao = new GLWarehouseProductLineAccountDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        GLWarehouseProductLineAccountDt entity = new GLWarehouseProductLineAccountDt();
                        entity.ID = ID;
                        entity.LocationID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(ID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Meal Plan Dt
        private void InitializeMealPlanDt(string queryString)
        {
            lblHeader.InnerText = "Panel Menu Makanan";

            //ReportMaster
            MealPlan entity = BusinessLayer.GetMealPlan(Convert.ToInt32(queryString));
            txtHeader.Text = entity.MealPlanName;

            //grid sebelah kiri, mengambil semua mapping reportmaster yang ada di menu tsb kecuali yg sudah dipilih
            List<StandardCode> ListAvailableMenu = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND StandardCodeID NOT IN (SELECT GCMealTime FROM MealPlanDt WHERE MealPlanID = {1} AND IsDeleted = 0)", Constant.StandardCode.MEAL_TIME, queryString));
            //grid sebelah kanan, mengambil semua mapping reportmaster hanya yang sudah dimapping
            List<StandardCode> ListSelectedMenu = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND StandardCodeID IN (SELECT GCMealTime FROM MealPlanDt WHERE MealPlanID = {1} AND IsDeleted = 0)", Constant.StandardCode.MEAL_TIME, queryString));

            ListAvailable = (from p in ListAvailableMenu.OrderBy(p => p.StandardCodeID)
                             select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName }).ToList();

            ListSelected = (from p in ListSelectedMenu.OrderBy(p => p.StandardCodeID)
                            select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName }).ToList();
        }

        private bool SaveMealPlanDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                Int32 mealPlanID = Convert.ToInt32(queryString);
                MealPlanDtDao entityDao = new MealPlanDtDao(ctx);
                string lstID = String.Empty;
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Remove)
                    {
                        if (lstID != String.Empty)
                            lstID += ",";
                        lstID += string.Format("'{0}'", row.ID);
                    }
                }

                List<MealPlanDt> lstMealPlanDt = null;
                if (lstID != String.Empty)
                    lstMealPlanDt = BusinessLayer.GetMealPlanDtList(String.Format("MealPlanID = {0} AND GCMealTime IN ({1}) AND IsDeleted = 0", mealPlanID, lstID), ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        MealPlanDt entity = new MealPlanDt();
                        entity.MealPlanID = mealPlanID;
                        entity.GCMealTime = row.ID;
                        entity.IsDeleted = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        MealPlanDt entity = lstMealPlanDt.FirstOrDefault(p => p.GCMealTime == row.ID);
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Medical Record
        private void InitializeMedicalRecord(string queryString)
        {
            lblHeader.InnerText = "Medical Record";

            StandardCode std = BusinessLayer.GetStandardCode(queryString);
            txtHeader.Text = std.StandardCodeName;

            List<MedicalRecordForm> ListAvailableForm = BusinessLayer.GetMedicalRecordFormList(string.Format("FormID NOT IN (SELECT FormID FROM MedicalRecordFolder WHERE GCMedicalFolderType= '{0}') AND IsDeleted = 0", queryString));
            List<MedicalRecordForm> ListSelectedForm = BusinessLayer.GetMedicalRecordFormList(string.Format("FormID IN (SELECT FormID FROM MedicalRecordFolder WHERE GCMedicalFolderType = '{0}') AND IsDeleted = 0", queryString));

            ListAvailable = (from p in ListAvailableForm.OrderBy(p => p.FormName)
                             select new CMatrix { IsChecked = false, ID = p.FormID.ToString(), Name = p.FormName }).ToList();

            ListSelected = (from p in ListSelectedForm.OrderBy(p => p.FormName)
                            select new CMatrix { IsChecked = false, ID = p.FormID.ToString(), Name = p.FormName }).ToList();
        }

        private bool SaveMedicalRecord(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                string linkStd = queryString;
                MedicalRecordFolderDao entityDao = new MedicalRecordFolderDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        MedicalRecordFolder entity = new MedicalRecordFolder();
                        entity.GCMedicalFolderType = linkStd;
                        entity.FormID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(linkStd, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Module Menu
        private void InitializeModuleMenu(string queryString)
        {
            lblHeader.InnerText = "Module";

            Module module = BusinessLayer.GetModule(queryString);
            txtHeader.Text = string.Format("{0} - {1}", module.ModuleID, module.ModuleName);

            List<MenuMaster> ListAvailableMenu = BusinessLayer.GetMenuMasterList(string.Format("ModuleID != '{0}' OR ModuleID IS NULL ORDER BY MenuCaption ASC", queryString));
            List<MenuMaster> ListSelectedMenu = BusinessLayer.GetMenuMasterList(string.Format("ModuleID = '{0}' ORDER BY MenuCaption ASC", queryString));

            ListAvailable = (from p in ListAvailableMenu.OrderBy(p => p.MenuCode)
                             select new CMatrix { IsChecked = false, ID = p.MenuID.ToString(), Name = "(" + p.MenuCode + ") " + p.MenuCaption }).ToList();

            ListSelected = (from p in ListSelectedMenu.OrderBy(p => p.MenuCode)
                            select new CMatrix { IsChecked = false, ID = p.MenuID.ToString(), Name = "(" + p.MenuCode + ") " + p.MenuCaption }).ToList();
        }

        private bool SaveModuleMenu(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                string ModuleID = queryString;
                MenuMasterDao entityDao = new MenuMasterDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        Int32 MenuID = Convert.ToInt32(row.ID);
                        MenuMaster entity = entityDao.Get(MenuID);
                        entity.ModuleID = ModuleID;
                        entityDao.Update(entity);
                    }
                    else
                    {
                        Int32 MenuID = Convert.ToInt32(row.ID);
                        MenuMaster entity = entityDao.Get(MenuID);
                        entity.ModuleID = null;
                        entityDao.Update(entity);
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region NursingDiagnose - Intervention
        private void InitializeNursingDiagnoseIntervention(string queryString)
        {
            lblHeader.InnerText = "Diagnosa Keperawatan";
            NursingDiagnose sp = BusinessLayer.GetNursingDiagnose(Convert.ToInt32(queryString));
            txtHeader.Text = sp.NurseDiagnoseName;

            string filter = string.Format("NurseInterventionID NOT IN (SELECT NurseInterventionID FROM NursingDiagnoseIntervention WHERE NurseDiagnoseID = {0}) AND IsDeleted = 0", queryString);
            List<NursingIntervention> ListAvailableItem = BusinessLayer.GetNursingInterventionList(filter);
            List<vNursingDiagnoseIntervention> ListSelectedItem = BusinessLayer.GetvNursingDiagnoseInterventionList(string.Format("NurseDiagnoseID = {0}", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.NurseInterventionName)
                             select new CMatrix { IsChecked = false, ID = p.NurseInterventionID.ToString(), Name = p.NurseInterventionName, ToolTip = p.Description }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.NurseInterventionName)
                            select new CMatrix { IsChecked = false, ID = p.NurseInterventionID.ToString(), Name = p.NurseInterventionName, ToolTip = p.Description }).ToList();
        }

        private bool SaveNursingDiagnoseIntervention(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                String diagnoseID = queryString;
                NursingDiagnoseInterventionDao entityDao = new NursingDiagnoseInterventionDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        NursingDiagnoseIntervention entity = new NursingDiagnoseIntervention();
                        entity.NurseDiagnoseID = Convert.ToInt32(diagnoseID);
                        entity.NurseInterventionID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(Convert.ToInt32(diagnoseID), Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Nursing Problem Diagnosis
        private void InitializeNursingProblemDiagnosis(string queryString)
        {
            lblHeader.InnerText = "Masalah Keperawatan";
            NursingProblem entity = BusinessLayer.GetNursingProblem(Convert.ToInt32(queryString));
            txtHeader.Text = string.Format("{0} - {1}", entity.ProblemCode, entity.ProblemName);

            string filter = string.Format("NurseDiagnoseID NOT IN (SELECT NurseDiagnoseID FROM NursingProblemDiagnose WHERE ProblemID = {0}) AND IsDeleted = 0", queryString);
            List<NursingDiagnose> ListAvailableItem = BusinessLayer.GetNursingDiagnoseList(filter);
            List<NursingDiagnose> ListSelectedItem = BusinessLayer.GetNursingDiagnoseList(string.Format("NurseDiagnoseID IN (SELECT NurseDiagnoseID FROM NursingProblemDiagnose WHERE ProblemID = {0}) AND IsDeleted = 0", Convert.ToInt32(queryString)));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.NurseDiagnoseCode)
                             select new CMatrix { IsChecked = false, ID = p.NurseDiagnoseID.ToString(), Name = string.Format("{0} ({1})", p.NurseDiagnoseName, p.NurseDiagnoseCode), ToolTip = p.Description }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.NurseDiagnoseCode)
                            select new CMatrix { IsChecked = false, ID = p.NurseDiagnoseID.ToString(), Name = string.Format("{0} ({1})", p.NurseDiagnoseName, p.NurseDiagnoseCode), ToolTip = p.Description }).ToList();
        }

        private bool SaveNursingProblemDiagnosis(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                Int32 problemID = Convert.ToInt32(queryString);
                NursingProblemDiagnoseDao entityDao = new NursingProblemDiagnoseDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        NursingProblemDiagnose entity = new NursingProblemDiagnose();
                        entity.ProblemID = problemID;
                        entity.NurseDiagnoseID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(problemID, Convert.ToInt32(row.ID));
                    }

                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Nutrition Order Hd Diet Type
        private void InitializeNutritionOrderHdDietType(string queryString)
        {
            lblHeader.InnerText = "No. Order";

            if (queryString != "0")
            {
                NutritionOrderHd entity = BusinessLayer.GetNutritionOrderHd(Convert.ToInt32(queryString));
                txtHeader.Text = entity.NutritionOrderNo;
            }

            List<StandardCode> ListAvailableMenu = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND StandardCodeID NOT IN (SELECT GCDietType FROM NutritionOrderHdDietType WHERE NutritionOrderHdID = {1} AND IsDeleted = 0)", Constant.StandardCode.DIET_TYPE, queryString));
            List<StandardCode> ListSelectedMenu = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND StandardCodeID IN (SELECT GCDietType FROM NutritionOrderHdDietType WHERE NutritionOrderHdID = {1} AND IsDeleted = 0)", Constant.StandardCode.DIET_TYPE, queryString));

            ListAvailable = (from p in ListAvailableMenu.OrderBy(p => p.StandardCodeID)
                             select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName }).ToList();

            ListSelected = (from p in ListSelectedMenu.OrderBy(p => p.StandardCodeID)
                            select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName }).ToList();
        }

        private bool SaveNutritionOrderHdDietType(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                Int32 nutritionOrderHdID = Convert.ToInt32(queryString);
                NutritionOrderHdDietTypeDao entityDao = new NutritionOrderHdDietTypeDao(ctx);
                NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
                string lstID = String.Empty;

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Remove)
                    {
                        if (lstID != String.Empty)
                            lstID += ",";
                        lstID += string.Format("'{0}'", row.ID);
                    }
                }

                List<NutritionOrderHdDietType> lstNutritionOrderHdDietType = null;
                if (lstID != String.Empty)
                    lstNutritionOrderHdDietType = BusinessLayer.GetNutritionOrderHdDietTypeList(String.Format("NutritionOrderHdID = {0} AND GCDietType IN ({1})", nutritionOrderHdID, lstID), ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        NutritionOrderHdDietType entity = new NutritionOrderHdDietType();
                        entity.NutritionOrderHdID = nutritionOrderHdID;
                        entity.GCDietType = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        NutritionOrderHdDietType entity = lstNutritionOrderHdDietType.FirstOrDefault(p => p.GCDietType == row.ID);
                        entityDao.Delete(entity.NutritionOrderHdID, entity.GCDietType);
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Pharmacogenetic - Item
        private void InitializePharmacogeneticItem(string queryString)
        {
            lblHeader.InnerText = "Kode Panel";
            lblHeader2.InnerText = "Nama Panel";

            Pharmacogenetic obj = BusinessLayer.GetPharmacogeneticList(string.Format("PharmacogeneticID = {0}", queryString))[0];
            txtHeader.Text = obj.PharmacogeneticCode;
            txtHeader2.Text = obj.PharmacogeneticName;

            List<vDrugInfo> ListAvailableItem = BusinessLayer.GetvDrugInfoList(string.Format("GCItemType = 'X001^002' AND PharmacogeneticID IS NULL AND IsDeleted = 0 ORDER BY ItemName1 ASC"));
            List<vDrugInfo> ListSelectedItem = BusinessLayer.GetvDrugInfoList(string.Format("GCItemType = 'X001^002' AND PharmacogeneticID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemCode }).ToList();
        }

        private bool SavePharmacogeneticItem(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int recordID = Convert.ToInt32(queryString);
                DrugInfoDao entityDao = new DrugInfoDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    DrugInfo entity = BusinessLayer.GetDrugInfo(Int32.Parse(row.ID));
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        entity.PharmacogeneticID = recordID;
                    }
                    else
                        entity.PharmacogeneticID = null;
                    entityDao.Update(entity);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region PhysicianCompoundTemplate
        private void InitializePhysicianCompoundTemplate(string queryString)
        {
            lblHeader.InnerText = "Template Racikan";
            CompoundTemplateHd ct = BusinessLayer.GetCompoundTemplateHd(Int32.Parse(queryString));
            txtHeader.Text = ct.CompoundTemplateName;

            string filter = string.Format("ParamedicID NOT IN (SELECT ParamedicID FROM PhysicianCompoundTemplate WHERE CompoundTemplateID = {0}) AND IsDeleted = 0 AND GCParamedicMasterType = '{1}'", Int32.Parse(queryString), Constant.ParamedicType.Physician);
            List<ParamedicMaster> ListAvailableItem = BusinessLayer.GetParamedicMasterList(filter);
            List<vPhysicianCompoundTemplate> ListSelectedItem = BusinessLayer.GetvPhysicianCompoundTemplateList(string.Format("CompoundTemplateID = {0}", Int32.Parse(queryString)));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.FullName)
                             select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.FullName, ToolTip = p.FullName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.FullName)
                            select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.FullName, ToolTip = p.FullName }).ToList();
        }

        private bool SavePhysicianCompoundTemplate(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                Int32 compoundTemplateID = Int32.Parse(queryString);
                PhysicianCompoundTemplateDao entityDao = new PhysicianCompoundTemplateDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        PhysicianCompoundTemplate entity = new PhysicianCompoundTemplate();
                        entity.CompoundTemplateID = compoundTemplateID;
                        entity.ParamedicID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(compoundTemplateID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region PhysicianItemCharges
        private void InitializePhysicianItemCharges(string queryString)
        {
            lblHeader.InnerText = "Dokter / Tenaga Medis";

            ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(queryString));
            txtHeader.Text = pm.FullName;

            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(string.Format(
                                    "GCItemType IN ('{0}','{1}') AND ItemID NOT IN (SELECT ItemID FROM PhysicianItem WHERE ParamedicID = {2}) AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                                    Constant.ItemType.PELAYANAN, Constant.ItemType.PENUNJANG_MEDIS, queryString));
            List<vPhysicianItem> ListSelectedItem = BusinessLayer.GetvPhysicianItemList(string.Format(
                                    "ParamedicID = {0} AND GCItemType IN ('{1}','{2}') ORDER BY ItemName1 ASC", queryString, Constant.ItemType.PELAYANAN, Constant.ItemType.PENUNJANG_MEDIS));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();
        }

        private bool SavePhysicianItemCharges(string queryString, ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            PhysicianItemDao entityDao = new PhysicianItemDao(ctx);
            try
            {
                int ParamedicID = Convert.ToInt32(queryString);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        PhysicianItem entity = new PhysicianItem();
                        entity.ParamedicID = ParamedicID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(ParamedicID, Int32.Parse(row.ID));
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region PhysicianItemDrugs
        private void InitializePhysicianItemDrugs(string queryString)
        {
            lblHeader.InnerText = "ParamedicName";

            ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(queryString));
            txtHeader.Text = pm.FullName;

            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(string.Format(
                                    "GCItemType IN ('{0}','{1}') AND ItemID NOT IN (SELECT ItemID FROM PhysicianItem WHERE ParamedicID = {2}) AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                                    Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, queryString));
            List<vPhysicianItem> ListSelectedItem = BusinessLayer.GetvPhysicianItemList(string.Format(
                                    "ParamedicID = {0} AND GCItemType IN ('{1}','{2}') ORDER BY ItemName1 ASC",
                                    queryString, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();
        }

        private bool SavePhysicianItemDrugs(string queryString, ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            PhysicianItemDao entityDao = new PhysicianItemDao(ctx);
            try
            {
                int ParamedicID = Convert.ToInt32(queryString);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        PhysicianItem entity = new PhysicianItem();
                        entity.ParamedicID = ParamedicID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(ParamedicID, Int32.Parse(row.ID));
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region PhysicianItemLogistics
        private void InitializePhysicianItemLogistics(string queryString)
        {
            lblHeader.InnerText = "ParamedicName";

            ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(queryString));
            txtHeader.Text = pm.FullName;

            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(string.Format(
                                    "GCItemType = '{0}' AND ItemID NOT IN (SELECT ItemID FROM PhysicianItem WHERE ParamedicID = {1}) AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                                    Constant.ItemType.BARANG_UMUM, queryString));
            List<vPhysicianItem> ListSelectedItem = BusinessLayer.GetvPhysicianItemList(string.Format(
                                    "ParamedicID = {0} AND GCItemType = '{1}' ORDER BY ItemName1 ASC", queryString, Constant.ItemType.BARANG_UMUM));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();
        }

        private bool SavePhysicianItemLogistics(string queryString, ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            PhysicianItemDao entityDao = new PhysicianItemDao(ctx);
            try
            {
                int ParamedicID = Convert.ToInt32(queryString);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        PhysicianItem entity = new PhysicianItem();
                        entity.ParamedicID = ParamedicID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(ParamedicID, Int32.Parse(row.ID));
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region PhysicianTemplete
        private void InitializePhysicianTemplete(string queryString)
        {
            List<vParamedicMaster> ListAvailableTemplete = BusinessLayer.GetvParamedicMasterList(string.Format(
                                    "ParamedicID NOT IN (SELECT ParamedicID FROM PhysicianTemplateText WHERE TemplateID = {0}) AND IsDeleted = 0 AND GCParamedicMasterType = '{1}'", Int32.Parse(queryString), Constant.ParamedicType.Physician));
            List<vPhysicianTemplateText> ListSelectedTemplete = BusinessLayer.GetvPhysicianTemplateTextList(string.Format("TemplateID = {0}", Int32.Parse(queryString)));

            ListAvailable = (from p in ListAvailableTemplete.OrderBy(p => p.ParamedicName)
                             select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.ParamedicName, ToolTip = p.ParamedicName }).ToList();

            ListSelected = (from p in ListSelectedTemplete.OrderBy(p => p.ParamedicName)
                            select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.ParamedicName, ToolTip = p.ParamedicName }).ToList();
        }

        private bool SavePhysicianTemplete(string queryString, ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            PhysicianTemplateTextDao entityDao = new PhysicianTemplateTextDao(ctx);
            try
            {
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        PhysicianTemplateText entity = new PhysicianTemplateText();
                        entity.ParamedicID = Int32.Parse(row.ID);
                        entity.TemplateID = Int32.Parse(queryString);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(Int32.Parse(queryString), Int32.Parse(row.ID));
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Procedure Group Panel
        private void InitializeProcedureGroupPanel(string queryString)
        {
            lblHeader.InnerText = "Kelompok Tindakan";

            ProcedureGroup pg = BusinessLayer.GetProcedureGroupList(string.Format("ProcedureGroupID = {0}", queryString)).FirstOrDefault();
            txtHeader.Text = pg.ProcedureGroupCode + " - " + pg.ProcedureGroupName;

            List<ProcedurePanelHd> ListAvailableItem = BusinessLayer.GetProcedurePanelHdList(string.Format("ProcedurePanelID NOT IN (SELECT ProcedurePanelID FROM ProcedureGroupPanel WHERE ProcedureGroupID = {0}) AND IsDeleted = 0 ORDER BY ProcedurePanelName ASC", queryString));
            List<vProcedureGroupPanel> ListSelectedItem = BusinessLayer.GetvProcedureGroupPanelList(string.Format("ProcedureGroupID = {0} ORDER BY ProcedurePanelName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ProcedurePanelName)
                             select new CMatrix { IsChecked = false, ID = p.ProcedurePanelID.ToString(), Name = p.ProcedurePanelName, ToolTip = p.ProcedurePanelCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ProcedurePanelName)
                            select new CMatrix { IsChecked = false, ID = p.ProcedurePanelID.ToString(), Name = p.ProcedurePanelName, ToolTip = p.ProcedurePanelCode }).ToList();
        }

        private bool SaveProcedureGroupPanel(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ProcedureGroupID = Convert.ToInt32(queryString);
                ProcedureGroupPanelDao entityDao = new ProcedureGroupPanelDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ProcedureGroupPanel entity = new ProcedureGroupPanel();
                        entity.ProcedureGroupID = ProcedureGroupID;
                        entity.ProcedurePanelID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(ProcedureGroupID, Int32.Parse(row.ID));
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Procedure Item Service
        private void InitializeProcedureItemService(string queryString)
        {
            lblHeader.InnerText = "Prosedur Pelayanan";

            ItemMaster im = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", queryString)).FirstOrDefault();
            txtHeader.Text = im.ItemCode + " - " + im.ItemName1;

            List<Procedures> ListAvailableItem = BusinessLayer.GetProceduresList(string.Format("ProcedureID NOT IN (SELECT ProcedureID FROM ProcedureItemService WHERE ItemID = {0}) AND IsDeleted = 0 ORDER BY ProcedureName ASC", queryString));
            List<vProcedureItemService> ListSelectedItem = BusinessLayer.GetvProcedureItemServiceList(string.Format("ItemID = {0} ORDER BY ProcedureName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ProcedureName)
                             select new CMatrix { IsChecked = false, ID = p.ProcedureID.ToString(), Name = p.ProcedureName, ToolTip = p.ProcedureName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ProcedureName)
                            select new CMatrix { IsChecked = false, ID = p.ProcedureID.ToString(), Name = p.ProcedureName, ToolTip = p.ProcedureName }).ToList();
        }

        private bool SaveProcedureItemService(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int ItemID = Convert.ToInt32(queryString);
                ProcedureItemServiceDao entityDao = new ProcedureItemServiceDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ProcedureItemService entity = new ProcedureItemService();
                        entity.ItemID = ItemID;
                        entity.ProcedureID = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(ItemID, row.ID);
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Promotion Scheme - Healthcare
        private void InitializeHealthcarePromotionScheme(string queryString)
        {
            lblHeader.InnerText = "Skema Promo";

            PromotionScheme psc = BusinessLayer.GetPromotionSchemeList(string.Format("PromotionSchemeID = {0}", queryString))[0];
            txtHeader.Text = psc.PromotionSchemeName;

            List<Healthcare> ListAvailableEntity = BusinessLayer.GetHealthcareList(string.Format("HealthcareID NOT IN (SELECT HealthcareID FROM HealthcarePromotionScheme WHERE PromotionSchemeID = {0}) ORDER BY HealthcareID ASC", queryString));
            List<vHealthcarePromotionScheme> ListSelectedEntity = BusinessLayer.GetvHealthcarePromotionSchemeList(string.Format("PromotionSchemeID = {0} ORDER BY HealthcareID ASC", queryString));

            ListAvailable = (from p in ListAvailableEntity.OrderBy(p => p.HealthcareName)
                             select new CMatrix { IsChecked = false, ID = p.HealthcareID.ToString(), Name = string.Format("{0} ({1})", p.HealthcareName, p.HealthcareID), ToolTip = p.HealthcareName }).ToList();

            ListSelected = (from p in ListSelectedEntity.OrderBy(p => p.HealthcareName)
                            select new CMatrix { IsChecked = false, ID = p.HealthcareID.ToString(), Name = string.Format("{0} ({1})", p.HealthcareName, p.HealthcareID), ToolTip = p.HealthcareID }).ToList();
        }

        private bool SaveHealthcarePromotionScheme(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int PromotionSchemeID = Convert.ToInt32(queryString);
                HealthcarePromotionSchemeDao entityDao = new HealthcarePromotionSchemeDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        HealthcarePromotionScheme entity = new HealthcarePromotionScheme();
                        entity.PromotionSchemeID = PromotionSchemeID;
                        entity.HealthcareID = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(row.ID, PromotionSchemeID);
                }
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region Promotion Scheme - ItemGroupMapping
        private void InitializePromotionSchemeItemGroupMapping(string queryString)
        {
            lblHeader.InnerText = "Skema Promo";

            PromotionScheme psc = BusinessLayer.GetPromotionSchemeList(string.Format("PromotionSchemeID = {0}", queryString))[0];
            txtHeader.Text = psc.PromotionSchemeName;

            string filterPromo = "";
            if (psc.IsMinimumTransactionFromService)
            {
                filterPromo = string.Format("'{0}'", Constant.ItemType.PELAYANAN);
            }
            if (psc.IsMinimumTransactionFromDrug)
            {
                if (!String.IsNullOrEmpty(filterPromo))
                {
                    filterPromo += string.Format(",'{0}','{1}'", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
                }
                else
                {
                    filterPromo = string.Format("'{0}','{1}'", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
                }
            }
            if (psc.IsMinimumTransactionFromGeneralGoods)
            {
                if (!String.IsNullOrEmpty(filterPromo))
                {
                    filterPromo += string.Format(",'{0}'", Constant.ItemType.BARANG_UMUM);
                }
                else
                {
                    filterPromo += string.Format("'{0}'", Constant.ItemType.BARANG_UMUM);
                }
            }

            string filterAva = string.Format("ItemGroupID NOT IN (SELECT ItemGroupID FROM PromotionSchemeItemGroupMapping WHERE PromotionSchemeID = {0}) AND IsDeleted = 0 AND IsHeader = 0 ORDER BY ItemGroupID ASC", queryString);
            if (!String.IsNullOrEmpty(filterPromo))
            {
                filterAva = string.Format("ItemGroupID NOT IN (SELECT ItemGroupID FROM PromotionSchemeItemGroupMapping WHERE PromotionSchemeID = {0}) AND IsDeleted = 0 AND IsHeader = 0  AND GCItemType IN ({1}) ORDER BY ItemGroupID ASC", queryString, filterPromo);
            }
            List<ItemGroupMaster> ListAvailableEntity = BusinessLayer.GetItemGroupMasterList(filterAva);
            List<vPromotionSchemeItemGroupMapping> ListSelectedEntity = BusinessLayer.GetvPromotionSchemeItemGroupMappingList(string.Format("PromotionSchemeID = {0} ORDER BY ItemGroupID ASC", queryString));

            ListAvailable = (from p in ListAvailableEntity.OrderBy(p => p.ItemGroupName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemGroupID.ToString(), Name = string.Format("{0}", p.ItemGroupName1), ToolTip = p.ItemGroupName1 }).ToList();

            ListSelected = (from p in ListSelectedEntity.OrderBy(p => p.ItemGroupName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemGroupID.ToString(), Name = string.Format("{0})", p.ItemGroupName1), ToolTip = p.ItemGroupName1 }).ToList();
        }

        private bool SavePromotionSchemeItemGroupMapping(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int PromotionSchemeID = Convert.ToInt32(queryString);
                PromotionSchemeItemGroupMappingDao entityDao = new PromotionSchemeItemGroupMappingDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        PromotionSchemeItemGroupMapping entity = new PromotionSchemeItemGroupMapping();
                        entity.PromotionSchemeID = PromotionSchemeID;
                        entity.ItemGroupID = Convert.ToInt32(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(PromotionSchemeID, Convert.ToInt32(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
        #region ReportByMenuMaster
        private void InitializeReportByMenuMaster(string queryString)
        {
            lblHeader.InnerText = "Menu";
            //ReportMaster
            MenuMaster entity = BusinessLayer.GetMenuMaster(Convert.ToInt32(queryString));
            txtHeader.Text = string.Format("({0}) {1}", entity.MenuCode, entity.MenuCaption);

            //grid sebelah kiri, mengambil semua mapping reportmaster yang ada di menu tsb kecuali yg sudah dipilih
            List<ReportMaster> ListAvailableMenu = BusinessLayer.GetReportMasterList(string.Format("GCReportType='X140^002' AND IsDeleted = 0 AND ReportID NOT IN (SELECT ReportID FROM ReportByMenuMaster WHERE MenuID = {0} AND IsDeleted = 0)", queryString));

            //grid sebelah kanan, mengambil semua mapping reportmaster hanya yang sudah dimapping
            List<ReportMaster> ListSelectedMenu = BusinessLayer.GetReportMasterList(string.Format("GCReportType='X140^002' AND IsDeleted = 0 AND ReportID IN (SELECT ReportID FROM ReportByMenuMaster WHERE MenuID = {0} AND IsDeleted = 0)", queryString));

            ListAvailable = (from p in ListAvailableMenu.OrderBy(p => p.ReportID)
                             select new CMatrix { IsChecked = false, ID = p.ReportID.ToString(), Name = string.Format("({0}) {1}", p.ReportCode, p.ReportTitle1) }).ToList();

            ListSelected = (from p in ListSelectedMenu.OrderBy(p => p.ReportID)
                            select new CMatrix { IsChecked = false, ID = p.ReportID.ToString(), Name = string.Format("({0}) {1}", p.ReportCode, p.ReportTitle1) }).ToList();
        }

        private bool SaveReportByMenuMaster(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                Int32 MenuID = Convert.ToInt32(queryString);
                ReportByMenuMasterDao entityDao = new ReportByMenuMasterDao(ctx);
                string lstID = String.Empty;
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Remove)
                    {
                        if (lstID != String.Empty)
                            lstID += ",";
                        lstID += string.Format("'{0}'", row.ID);
                    }
                }

                List<ReportByMenuMaster> lstReportByMenuMaster = null;
                if (lstID != String.Empty)
                    lstReportByMenuMaster = BusinessLayer.GetReportByMenuMasterList(String.Format("MenuID = {0} AND ReportID IN ({1}) AND IsDeleted = 0", MenuID, lstID), ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ReportByMenuMaster entity = new ReportByMenuMaster();
                        entity.ReportID = Convert.ToInt16(row.ID);
                        entity.MenuID = MenuID;
                        entity.IsDeleted = false;
                        entity.IsAllowDownloadSummary = true;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ReportByMenuMaster entity = lstReportByMenuMaster.FirstOrDefault(p => p.MenuID == MenuID && p.ReportID == Convert.ToInt16(row.ID));
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }

                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Restriction Dt
        private void InitializeRestrictionDt(string queryString)
        {
            lblHeader.InnerText = "Restriction";

            RestrictionHd entity = BusinessLayer.GetRestrictionHd(Convert.ToInt32(queryString));
            txtHeader.Text = string.Format("{0} - {1}", entity.RestrictionCode, entity.RestrictionName);

            List<TransactionType> ListAvailableItem = BusinessLayer.GetTransactionTypeList(string.Format("TransactionCode NOT IN (SELECT TransactionCode FROM RestrictionDt WHERE RestrictionID = {0}) AND IsInventoryTransaction = 1", queryString));
            List<TransactionType> ListSelectedItem = BusinessLayer.GetTransactionTypeList(string.Format("TransactionCode IN (SELECT TransactionCode FROM RestrictionDt WHERE RestrictionID = {0})", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.TransactionName)
                             select new CMatrix { IsChecked = false, ID = p.TransactionCode, Name = p.TransactionName, ToolTip = p.TransactionCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.TransactionName)
                            select new CMatrix { IsChecked = false, ID = p.TransactionCode, Name = p.TransactionName, ToolTip = p.TransactionCode }).ToList();
        }

        private bool SaveRestrictionDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int RestrictionID = Convert.ToInt32(queryString);
                RestrictionDtDao entityDao = new RestrictionDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RestrictionDt entity = new RestrictionDt();
                        entity.RestrictionID = RestrictionID;
                        entity.TransactionCode = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(RestrictionID, row.ID);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region RevenueCostCenterDt
        private void InitializeRevenueCostCenterDt(string queryString)
        {
            lblHeader.InnerText = "Revenue Cost Center";
            lblHeader2.InnerText = "Healthcare";

            vRevenueCostCenter rcc = BusinessLayer.GetvRevenueCostCenterList(string.Format("RevenueCostCenterID = {0}", queryString))[0];
            txtHeader.Text = rcc.RevenueCostCenterName;
            txtHeader2.Text = AppSession.UserLogin.HealthcareName;

            List<vHealthcareServiceUnit> ListAvailableItem = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID NOT IN (SELECT HealthcareServiceUnitID FROM RevenueCostCenterDt) AND IsDeleted = 0 ORDER BY ServiceUnitName ASC", queryString));
            List<vRevenueCostCenterDt> ListSelectedItem = BusinessLayer.GetvRevenueCostCenterDtList(string.Format("RevenueCostCenterID = {0} ORDER BY ServiceUnitName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ServiceUnitName)
                             select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ServiceUnitName)
                            select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).ToList();
        }

        private bool SaveRevenueCostCenterDt(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int RevenueCostCenterID = Convert.ToInt32(queryString);
                RevenueCostCenterDtDao entityDao = new RevenueCostCenterDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueCostCenterDt entity = new RevenueCostCenterDt();
                        entity.RevenueCostCenterID = RevenueCostCenterID;
                        entity.HealthcareServiceUnitID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(RevenueCostCenterID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region RevenueCostCenterHealthcareUnit
        private void InitializeRevenueCostCenterHealthcareUnit(string queryString)
        {
            lblHeader.InnerText = "Revenue Cost Center";
            lblHeader2.InnerText = "Healthcare";

            vRevenueCostCenter rcc = BusinessLayer.GetvRevenueCostCenterList(string.Format("RevenueCostCenterID = {0}", queryString))[0];
            txtHeader.Text = rcc.RevenueCostCenterName;
            txtHeader2.Text = AppSession.UserLogin.HealthcareName;

            List<StandardCode> ListAvailableItem = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID NOT IN (SELECT GCHealthcareUnit FROM RevenueCostCenterHealthcareUnit) AND ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 ORDER BY StandardCodeName ASC", Constant.StandardCode.HEALTHCARE_UNIT));
            List<vRevenueCostCenterHealthcareUnit> ListSelectedItem = BusinessLayer.GetvRevenueCostCenterHealthcareUnitList(string.Format("RevenueCostCenterID = {0} AND IsDeleted = 0 ORDER BY HealthcareUnitName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.StandardCodeID)
                             select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.GCHealthcareUnit)
                            select new CMatrix { IsChecked = false, ID = p.GCHealthcareUnit.ToString(), Name = p.HealthcareUnitName }).ToList();
        }

        private bool SaveRevenueCostCenterHealthcareUnit(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int RevenueCostCenterID = Convert.ToInt32(queryString);
                RevenueCostCenterHealthcareUnitDao entityDao = new RevenueCostCenterHealthcareUnitDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueCostCenterHealthcareUnit entity = new RevenueCostCenterHealthcareUnit();
                        entity.RevenueCostCenterID = RevenueCostCenterID;
                        entity.GCHealthcareUnit = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(RevenueCostCenterID, row.ID);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region RevenueCostCenterFALocation
        private void InitializeRevenueCostCenterFALocation(string queryString)
        {
            lblHeader.InnerText = "Revenue Cost Center";
            lblHeader2.InnerText = "Healthcare";

            vRevenueCostCenter rcc = BusinessLayer.GetvRevenueCostCenterList(string.Format("RevenueCostCenterID = {0}", queryString))[0];
            txtHeader.Text = rcc.RevenueCostCenterName;
            txtHeader2.Text = AppSession.UserLogin.HealthcareName;

            List<FALocation> ListAvailableItem = BusinessLayer.GetFALocationList(string.Format("FALocationID NOT IN (SELECT FALocationID FROM RevenueCostCenterFALocation) AND IsDeleted = 0 ORDER BY FALocationName ASC"));
            List<vRevenueCostCenterFALocation> ListSelectedItem = BusinessLayer.GetvRevenueCostCenterFALocationList(string.Format("RevenueCostCenterID = {0} ORDER BY FALocationName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.FALocationName)
                             select new CMatrix { IsChecked = false, ID = p.FALocationID.ToString(), Name = p.FALocationName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.FALocationName)
                            select new CMatrix { IsChecked = false, ID = p.FALocationID.ToString(), Name = p.FALocationName }).ToList();
        }

        private bool SaveRevenueCostCenterFALocation(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int RevenueCostCenterID = Convert.ToInt32(queryString);
                RevenueCostCenterFALocationDao entityDao = new RevenueCostCenterFALocationDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueCostCenterFALocation entity = new RevenueCostCenterFALocation();
                        entity.RevenueCostCenterID = RevenueCostCenterID;
                        entity.FALocationID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(RevenueCostCenterID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region RevenueCostCenterLocation
        private void InitializeRevenueCostCenterLocation(string queryString)
        {
            lblHeader.InnerText = "Revenue Cost Center";
            lblHeader2.InnerText = "Healthcare";

            vRevenueCostCenter rcc = BusinessLayer.GetvRevenueCostCenterList(string.Format("RevenueCostCenterID = {0}", queryString))[0];
            txtHeader.Text = rcc.RevenueCostCenterName;
            txtHeader2.Text = AppSession.UserLogin.HealthcareName;

            List<vLocation> ListAvailableItem = BusinessLayer.GetvLocationList(string.Format("LocationID NOT IN (SELECT LocationID FROM RevenueCostCenterLocation) AND IsDeleted = 0 ORDER BY LocationName ASC"));
            List<vRevenueCostCenterLocation> ListSelectedItem = BusinessLayer.GetvRevenueCostCenterLocationList(string.Format("RevenueCostCenterID = {0} AND IsDeleted = 0 ORDER BY LocationName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.LocationName)
                             select new CMatrix { IsChecked = false, ID = p.LocationID.ToString(), Name = p.LocationName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.LocationName)
                            select new CMatrix { IsChecked = false, ID = p.LocationID.ToString(), Name = p.LocationName }).ToList();
        }

        private bool SaveRevenueCostCenterLocation(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int RevenueCostCenterID = Convert.ToInt32(queryString);
                RevenueCostCenterLocationDao entityDao = new RevenueCostCenterLocationDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueCostCenterLocation entity = new RevenueCostCenterLocation();
                        entity.RevenueCostCenterID = RevenueCostCenterID;
                        entity.LocationID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(RevenueCostCenterID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region RevenueSharingCustomerType
        private void InitializeRevenueSharingCustomerType(string queryString)
        {
            lblHeader.InnerText = "Kode Jasa Medis";
            lblHeader2.InnerText = "Nama Jasa Medis";

            RevenueSharingHd rsh = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(queryString));
            txtHeader.Text = rsh.RevenueSharingCode;
            txtHeader2.Text = rsh.RevenueSharingName;

            List<StandardCode> ListAvailableItem = BusinessLayer.GetStandardCodeList(string.Format(
                                    "ParentID = '{1}' AND StandardCodeID NOT IN (SELECT GCCustomerType FROM RevenueSharingCustomerType WHERE RevenueSharingID = {0} AND IsDeleted = 0) AND IsDeleted = 0 AND IsActive = 1 ORDER BY StandardCodeID ASC",
                                    queryString, Constant.StandardCode.CUSTOMER_TYPE));
            List<vRevenueSharingCustomerType> ListSelectedItem = BusinessLayer.GetvRevenueSharingCustomerTypeList(string.Format(
                                    "RevenueSharingID = {0} AND IsDeleted = 0 ORDER BY GCCustomerType ASC",
                                    queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.StandardCodeID)
                             select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.GCCustomerType)
                            select new CMatrix { IsChecked = false, ID = p.GCCustomerType.ToString(), Name = p.CustomerType }).ToList();
        }

        private bool SaveRevenueSharingCustomerType(string queryString, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int oRevenueSharingID = Convert.ToInt32(queryString);
                RevenueSharingCustomerTypeDao entityDao = new RevenueSharingCustomerTypeDao(ctx);

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueSharingCustomerType entity = new RevenueSharingCustomerType();
                        entity.RevenueSharingID = oRevenueSharingID;
                        entity.GCCustomerType = row.ID;
                        entity.IsDeleted = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = string.Format("RevenueSharingID = {0} AND GCCustomerType = '{1}' AND IsDeleted = 0", oRevenueSharingID, row.ID);
                        RevenueSharingCustomerType entity = BusinessLayer.GetRevenueSharingCustomerTypeList(filter, ctx).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
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
        #endregion
        #region RevenueSharingEmploymentStatus
        private void InitializeRevenueSharingEmploymentStatus(string queryString)
        {
            lblHeader.InnerText = "Kode Jasa Medis";
            lblHeader2.InnerText = "Nama Jasa Medis";

            RevenueSharingHd rsh = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(queryString));
            txtHeader.Text = rsh.RevenueSharingCode;
            txtHeader2.Text = rsh.RevenueSharingName;

            List<StandardCode> ListAvailableItem = BusinessLayer.GetStandardCodeList(string.Format(
                                    "ParentID = '{1}' AND StandardCodeID NOT IN (SELECT GCEmploymentStatus FROM RevenueSharingEmploymentStatus WHERE RevenueSharingID = {0} AND IsDeleted = 0) AND IsDeleted = 0 AND IsActive = 1 ORDER BY StandardCodeID ASC",
                                    queryString, Constant.StandardCode.EMPLOYMENT_STATUS));
            List<vRevenueSharingEmploymentStatus> ListSelectedItem = BusinessLayer.GetvRevenueSharingEmploymentStatusList(string.Format(
                                    "RevenueSharingID = {0} AND IsDeleted = 0 ORDER BY GCEmploymentStatus ASC",
                                    queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.StandardCodeID)
                             select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.GCEmploymentStatus)
                            select new CMatrix { IsChecked = false, ID = p.GCEmploymentStatus.ToString(), Name = p.EmploymentStatus }).ToList();
        }

        private bool SaveRevenueSharingEmploymentStatus(string queryString, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int oRevenueSharingID = Convert.ToInt32(queryString);
                RevenueSharingEmploymentStatusDao entityDao = new RevenueSharingEmploymentStatusDao(ctx);

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueSharingEmploymentStatus entity = new RevenueSharingEmploymentStatus();
                        entity.RevenueSharingID = oRevenueSharingID;
                        entity.GCEmploymentStatus = row.ID;
                        entity.IsDeleted = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = string.Format("RevenueSharingID = {0} AND GCEmploymentStatus = '{1}' AND IsDeleted = 0", oRevenueSharingID, row.ID);
                        RevenueSharingEmploymentStatus entity = BusinessLayer.GetRevenueSharingEmploymentStatusList(filter, ctx).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
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
        #endregion
        #region RevenueSharingHealthcareServiceUnit
        private void InitializeRevenueSharingHealthcareServiceUnit(string queryString)
        {
            lblHeader.InnerText = "Kode Jasa Medis";
            lblHeader2.InnerText = "Nama Jasa Medis";

            RevenueSharingHd rsh = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(queryString));
            txtHeader.Text = rsh.RevenueSharingCode;
            txtHeader2.Text = rsh.RevenueSharingName;

            List<vHealthcareServiceUnit> ListAvailableItem = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                                    "HealthcareServiceUnitID NOT IN (SELECT HealthcareServiceUnitID FROM RevenueSharingHealthcareServiceUnit WHERE RevenueSharingID = {0} AND IsDeleted = 0) AND IsDeleted = 0 AND IsUsingRegistration = 1 ORDER BY DepartmentID, ServiceUnitName ASC",
                                    queryString));
            List<vRevenueSharingHealthcareServiceUnit> ListSelectedItem = BusinessLayer.GetvRevenueSharingHealthcareServiceUnitList(string.Format(
                                    "RevenueSharingID = {0} AND IsDeleted = 0 ORDER BY DepartmentID, ServiceUnitName ASC",
                                    queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ServiceUnitName)
                             select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ServiceUnitName)
                            select new CMatrix { IsChecked = false, ID = p.HealthcareServiceUnitID.ToString(), Name = p.ServiceUnitName }).ToList();
        }

        private bool SaveRevenueSharingHealthcareServiceUnit(string queryString, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int oRevenueSharingID = Convert.ToInt32(queryString);
                RevenueSharingHealthcareServiceUnitDao entityDao = new RevenueSharingHealthcareServiceUnitDao(ctx);

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueSharingHealthcareServiceUnit entity = new RevenueSharingHealthcareServiceUnit();
                        entity.RevenueSharingID = oRevenueSharingID;
                        entity.HealthcareServiceUnitID = Convert.ToInt32(row.ID);
                        entity.IsDeleted = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = string.Format("RevenueSharingID = {0} AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", oRevenueSharingID, row.ID);
                        RevenueSharingHealthcareServiceUnit entity = BusinessLayer.GetRevenueSharingHealthcareServiceUnitList(filter, ctx).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
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
        #endregion
        #region RevenueSharingItemGroupMaster
        private void InitializeRevenueSharingItemGroupMaster(string queryString)
        {
            lblHeader.InnerText = "Kode Jasa Medis";
            lblHeader2.InnerText = "Nama Jasa Medis";

            RevenueSharingHd rsh = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(queryString));
            txtHeader.Text = rsh.RevenueSharingCode;
            txtHeader2.Text = rsh.RevenueSharingName;

            List<ItemGroupMaster> ListAvailableItem = BusinessLayer.GetItemGroupMasterList(string.Format(
                                    "ItemGroupID NOT IN (SELECT ItemGroupID FROM RevenueSharingItemGroupMaster WHERE RevenueSharingID = {0} AND IsDeleted = 0) AND IsDeleted = 0 ORDER BY ItemGroupName1 ASC",
                                    queryString));
            List<vRevenueSharingItemGroupMaster> ListSelectedItem = BusinessLayer.GetvRevenueSharingItemGroupMasterList(string.Format(
                                    "RevenueSharingID = {0} AND IsDeleted = 0 ORDER BY ItemGroupName1 ASC",
                                    queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemGroupName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemGroupID.ToString(), Name = p.ItemGroupName1 }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemGroupName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemGroupID.ToString(), Name = p.ItemGroupName1 }).ToList();
        }

        private bool SaveRevenueSharingItemGroupMaster(string queryString, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int oRevenueSharingID = Convert.ToInt32(queryString);
                RevenueSharingItemGroupMasterDao entityDao = new RevenueSharingItemGroupMasterDao(ctx);

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueSharingItemGroupMaster entity = new RevenueSharingItemGroupMaster();
                        entity.RevenueSharingID = oRevenueSharingID;
                        entity.ItemGroupID = Convert.ToInt32(row.ID);
                        entity.IsDeleted = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = string.Format("RevenueSharingID = {0} AND ItemGroupID = '{1}' AND IsDeleted = 0", oRevenueSharingID, row.ID);
                        RevenueSharingItemGroupMaster entity = BusinessLayer.GetRevenueSharingItemGroupMasterList(filter, ctx).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
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
        #endregion
        #region RevenueSharingItemMaster
        private void InitializeRevenueSharingItemMaster(string queryString)
        {
            lblHeader.InnerText = "Kode Jasa Medis";
            lblHeader2.InnerText = "Nama Jasa Medis";

            RevenueSharingHd rsh = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(queryString));
            txtHeader.Text = rsh.RevenueSharingCode;
            txtHeader2.Text = rsh.RevenueSharingName;

            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(string.Format(
                                    "ItemID NOT IN (SELECT ItemID FROM RevenueSharingItemMaster WHERE RevenueSharingID = {0} AND IsDeleted = 0) AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                                    queryString));
            List<vRevenueSharingItemMaster> ListSelectedItem = BusinessLayer.GetvRevenueSharingItemMasterList(string.Format(
                                    "RevenueSharingID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                                    queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();
        }

        private bool SaveRevenueSharingItemMaster(string queryString, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int oRevenueSharingID = Convert.ToInt32(queryString);
                RevenueSharingItemMasterDao entityDao = new RevenueSharingItemMasterDao(ctx);

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueSharingItemMaster entity = new RevenueSharingItemMaster();
                        entity.RevenueSharingID = oRevenueSharingID;
                        entity.ItemID = Convert.ToInt32(row.ID);
                        entity.IsDeleted = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = string.Format("RevenueSharingID = {0} AND ItemID = '{1}' AND IsDeleted = 0", oRevenueSharingID, row.ID);
                        RevenueSharingItemMaster entity = BusinessLayer.GetRevenueSharingItemMasterList(filter, ctx).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
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
        #endregion
        #region RevenueSharingParamedicMaster
        private void InitializeRevenueSharingParamedicMaster(string queryString)
        {
            lblHeader.InnerText = "Kode Jasa Medis";
            lblHeader2.InnerText = "Nama Jasa Medis";

            RevenueSharingHd rsh = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(queryString));
            txtHeader.Text = rsh.RevenueSharingCode;
            txtHeader2.Text = rsh.RevenueSharingName;

            List<ParamedicMaster> ListAvailableItem = BusinessLayer.GetParamedicMasterList(string.Format(
                                    "ParamedicID NOT IN (SELECT ParamedicID FROM RevenueSharingParamedicMaster WHERE RevenueSharingID = {0} AND IsDeleted = 0) AND GCParamedicMasterType = '{1}' AND IsDeleted = 0 ORDER BY FullName ASC",
                                    queryString, Constant.ParamedicType.Physician));
            List<vRevenueSharingParamedicMaster> ListSelectedItem = BusinessLayer.GetvRevenueSharingParamedicMasterList(string.Format(
                                    "RevenueSharingID = {0} AND IsDeleted = 0 ORDER BY ParamedicName ASC",
                                    queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.FullName)
                             select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.FullName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ParamedicName)
                            select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = p.ParamedicName }).ToList();
        }

        private bool SaveRevenueSharingParamedicMaster(string queryString, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int oRevenueSharingID = Convert.ToInt32(queryString);
                RevenueSharingParamedicMasterDao entityDao = new RevenueSharingParamedicMasterDao(ctx);

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        RevenueSharingParamedicMaster entity = new RevenueSharingParamedicMaster();
                        entity.RevenueSharingID = oRevenueSharingID;
                        entity.ParamedicID = Convert.ToInt32(row.ID);
                        entity.IsDeleted = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        String filter = string.Format("RevenueSharingID = {0} AND ParamedicID = '{1}' AND IsDeleted = 0", oRevenueSharingID, row.ID);
                        RevenueSharingParamedicMaster entity = BusinessLayer.GetRevenueSharingParamedicMasterList(filter, ctx).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
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
        #endregion
        #region Service Unit Form
        private void InitializeServiceUnitForm(string queryString)
        {
            lblHeader.InnerText = "Unit Pelayanan";
            lblHeader2.InnerText = "Rumah Sakit";

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", queryString))[0];
            txtHeader.Text = hsu.ServiceUnitName;
            txtHeader2.Text = hsu.HealthcareName;

            List<MedicalRecordForm> ListAvailableItem = BusinessLayer.GetMedicalRecordFormList(string.Format("FormID NOT IN (SELECT FormID FROM ServiceUnitForm WHERE HealthcareServiceUnitID = {0}) AND IsGeneratedForm = 1 AND IsDeleted = 0 ORDER BY FormCode ASC", queryString));
            List<vServiceUnitForm> ListSelectedItem = BusinessLayer.GetvServiceUnitFormList(string.Format("HealthcareServiceUnitID = {0} ORDER BY FormCode ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.FormName)
                             select new CMatrix { IsChecked = false, ID = p.FormID.ToString(), Name = p.FormName, ToolTip = p.FormCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.FormName)
                            select new CMatrix { IsChecked = false, ID = p.FormID.ToString(), Name = p.FormName, ToolTip = p.FormCode }).ToList();
        }

        private bool SaveServiceUnitForm(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int HealthcareServiceUnitID = Convert.ToInt32(queryString);
                ServiceUnitFormDao entityDao = new ServiceUnitFormDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ServiceUnitForm entity = new ServiceUnitForm();
                        entity.HealthcareServiceUnitID = HealthcareServiceUnitID;
                        entity.FormID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(HealthcareServiceUnitID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Service Unit Item
        private void InitializeServiceUnitItem(string queryString)
        {
            lblHeader.InnerText = "Unit Pelayanan";
            lblHeader2.InnerText = "Rumah Sakit";

            vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", queryString))[0];
            string healthcareID = hsu.HealthcareID;
            txtHeader.Text = hsu.ServiceUnitName;
            txtHeader2.Text = hsu.HealthcareName;

            string GCItemType = Constant.ItemGroupMaster.SERVICE;
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')", healthcareID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.RT0001));

            if (hsu.DepartmentID == Constant.Facility.MEDICAL_CHECKUP)
                GCItemType = string.Format("GCItemType IN ('{0}','{1}')", Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.MEDICAL_CHECKUP);
            else if (hsu.DepartmentID == Constant.Facility.DIAGNOSTIC)
            {
                if (hsu.ServiceUnitID == Convert.ToInt32(lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue))
                {
                    GCItemType = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.RADIOLOGY);
                }
                //else if (hsu.ServiceUnitID == Convert.ToInt32(lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue))
                else if (hsu.IsLaboratoryUnit)
                {
                    GCItemType = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.LABORATORY);
                }
                else if ((!string.IsNullOrEmpty(lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RT0001).ParameterValue)) && (hsu.HealthcareServiceUnitID == Convert.ToInt32(lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RT0001).ParameterValue)))
                {
                    GCItemType = string.Format("GCItemType = '{0}' AND GCSubItemType = 'X569^RT'", Constant.ItemGroupMaster.DIAGNOSTIC, Constant.SubItemType.RADIOTERAPI);
                }
                else
                {
                    GCItemType = string.Format("GCItemType IN ('{0}','{1}') AND GCSubItemType IS NULL", Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.DIAGNOSTIC);
                }
            }
            else
            {
                GCItemType = string.Format("GCItemType IN ('{0}','{1}') AND GCSubItemType IS NULL", Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.DIAGNOSTIC);
            }

            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(string.Format("ItemID NOT IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = {0}) AND {1} AND IsDeleted = 0 ORDER BY ItemName1 ASC", queryString, GCItemType));
            List<vServiceUnitItem> ListSelectedItem = BusinessLayer.GetvServiceUnitItemList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.cfItemName)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.cfItemName, ToolTip = p.ItemCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.cfItemName)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.cfItemName, ToolTip = p.ItemCode }).ToList();
        }

        private bool SaveServiceUnitItem(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int HealthcareServiceUnitID = Convert.ToInt32(queryString);
                ServiceUnitItemDao entityDao = new ServiceUnitItemDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ServiceUnitItem entity = new ServiceUnitItem();
                        entity.HealthcareServiceUnitID = HealthcareServiceUnitID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(HealthcareServiceUnitID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Service Unit Paramedic
        private void InitializeServiceUnitParamedic(string queryString)
        {
            lblHeader.InnerText = "Service Unit";
            lblHeader2.InnerText = "Healthcare";

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", queryString))[0];
            txtHeader.Text = hsu.ServiceUnitName;
            txtHeader2.Text = hsu.HealthcareName;

            List<vParamedicMaster> ListAvailableEntity = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID NOT IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0 ORDER BY LastName ASC", queryString));
            List<vServiceUnitParamedic> ListSelectedEntity = BusinessLayer.GetvServiceUnitParamedicList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY ParamedicLastName ASC", queryString));

            ListAvailable = (from p in ListAvailableEntity.OrderBy(p => p.ParamedicName)
                             select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = string.Format("{0} ({1})", p.ParamedicName, p.ParamedicCode), ToolTip = p.ParamedicCode }).ToList();

            ListSelected = (from p in ListSelectedEntity.OrderBy(p => p.ParamedicName)
                            select new CMatrix { IsChecked = false, ID = p.ParamedicID.ToString(), Name = string.Format("{0} ({1})", p.ParamedicName, p.ParamedicCode), ToolTip = p.ParamedicCode }).ToList();
        }

        private bool SaveServiceUnitParamedic(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int HealthcareServiceUnitID = Convert.ToInt32(queryString);
                ServiceUnitParamedicDao entityDao = new ServiceUnitParamedicDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ServiceUnitParamedic entity = new ServiceUnitParamedic();
                        entity.HealthcareServiceUnitID = HealthcareServiceUnitID;
                        entity.ParamedicID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(HealthcareServiceUnitID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Service Unit - Vital Sign
        private void InitializeServiceUnitVitalSign(string queryString)
        {
            lblHeader.InnerText = "Unit Pelayanan";
            lblHeader2.InnerText = "Rumah Sakit";

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", queryString))[0];
            txtHeader.Text = hsu.ServiceUnitName;
            txtHeader2.Text = hsu.HealthcareName;

            List<VitalSignType> ListAvailableItem = BusinessLayer.GetVitalSignTypeList(string.Format("VitalSignID NOT IN (SELECT VitalSignID FROM ServiceUnitVitalSign WHERE HealthcareServiceUnitID = {0}) AND IsAutoGenerated = 0 AND IsDeleted = 0 ORDER BY VitalSignName ASC", queryString));
            List<vServiceUnitVitalSign> ListSelectedItem = BusinessLayer.GetvServiceUnitVitalSignList(string.Format("HealthcareServiceUnitID = {0} ORDER BY VitalSignName ASC", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.VitalSignName)
                             select new CMatrix { IsChecked = false, ID = p.VitalSignID.ToString(), Name = p.VitalSignName, ToolTip = p.VitalSignCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.VitalSignName)
                            select new CMatrix { IsChecked = false, ID = p.VitalSignID.ToString(), Name = p.VitalSignName, ToolTip = p.VitalSignCode }).ToList();
        }

        private bool SaveServiceUnitVitalSign(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int HealthcareServiceUnitID = Convert.ToInt32(queryString);
                ServiceUnitVitalSignDao entityDao = new ServiceUnitVitalSignDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ServiceUnitVitalSign entity = new ServiceUnitVitalSign();
                        entity.HealthcareServiceUnitID = HealthcareServiceUnitID;
                        entity.VitalSignID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(HealthcareServiceUnitID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Specialty Body Diagram
        private void InitializeSpecialtyBodyDiagram(string queryString)
        {
            lblHeader.InnerText = "Specialty";
            Specialty sp = BusinessLayer.GetSpecialty(queryString);
            txtHeader.Text = sp.SpecialtyName;

            string filter = string.Format("DiagramID NOT IN (SELECT DiagramID FROM SpecialtyBodyDiagram WHERE SpecialtyID = '{0}') AND IsDeleted = 0", queryString);
            List<BodyDiagram> ListAvailableItem = BusinessLayer.GetBodyDiagramList(filter);
            List<vSpecialtyBodyDiagram> ListSelectedItem = BusinessLayer.GetvSpecialtyBodyDiagramList(string.Format("SpecialtyID = '{0}'", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.DiagramName)
                             select new CMatrix { IsChecked = false, ID = p.DiagramID.ToString(), Name = p.DiagramName, ToolTip = p.DiagramCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.DiagramName)
                            select new CMatrix { IsChecked = false, ID = p.DiagramID.ToString(), Name = p.DiagramName, ToolTip = p.DiagramCode }).ToList();
        }

        private bool SaveSpecialtyBodyDiagram(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                String specialtyID = queryString;
                SpecialtyBodyDiagramDao entityDao = new SpecialtyBodyDiagramDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        SpecialtyBodyDiagram entity = new SpecialtyBodyDiagram();
                        entity.SpecialtyID = specialtyID;
                        entity.DiagramID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(specialtyID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Specialty Procedure
        private void InitializeSpecialtyProcedure(string queryString)
        {
            lblHeader.InnerText = "Specialty";
            Specialty sp = BusinessLayer.GetSpecialty(queryString);
            txtHeader.Text = sp.SpecialtyName;

            string filter = string.Format("ProcedureID NOT IN (SELECT ProcedureID FROM SpecialtyProcedures WHERE SpecialtyID = '{0}') AND IsDeleted = 0", queryString);
            List<Procedures> ListAvailableItem = BusinessLayer.GetProceduresList(filter);
            List<vSpecialtyProcedures> ListSelectedItem = BusinessLayer.GetvSpecialtyProceduresList(string.Format("SpecialtyID = '{0}'", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ProcedureName)
                             select new CMatrix { IsChecked = false, ID = p.ProcedureID.ToString(), Name = p.ProcedureName, ToolTip = p.ProcedureName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ProcedureName)
                            select new CMatrix { IsChecked = false, ID = p.ProcedureID.ToString(), Name = p.ProcedureName, ToolTip = p.ProcedureName }).ToList();
        }

        private bool SaveSpecialtyProcedure(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                String specialtyID = queryString;
                SpecialtyProceduresDao entityDao = new SpecialtyProceduresDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        SpecialtyProcedures entity = new SpecialtyProcedures();
                        entity.SpecialtyID = specialtyID;
                        entity.ProcedureID = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(specialtyID, row.ID);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Specialty Review of System
        private void InitializeSpecialtyROS(string queryString)
        {
            lblHeader.InnerText = "Specialty";
            Specialty sp = BusinessLayer.GetSpecialty(queryString);
            txtHeader.Text = sp.SpecialtyName;

            string filter = string.Format("ParentID = '{0}' AND StandardCodeID NOT IN (SELECT GCROSystem FROM SpecialtyROS WHERE SpecialtyID = '{1}') AND IsDeleted = 0", Constant.StandardCode.REVIEW_OF_SYSTEM, queryString);
            List<StandardCode> ListAvailableItem = BusinessLayer.GetStandardCodeList(filter);
            List<vSpecialtyROS> ListSelectedItem = BusinessLayer.GetvSpecialtyROSList(string.Format("SpecialtyID = '{0}'", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.StandardCodeName)
                             select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName, ToolTip = p.StandardCodeName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.StandardCodeName)
                            select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName, ToolTip = p.StandardCodeName }).ToList();
        }

        private bool SaveSpecialtyROS(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                String specialtyID = queryString;
                SpecialtyROSDao entityDao = new SpecialtyROSDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        SpecialtyROS entity = new SpecialtyROS();
                        entity.SpecialtyID = specialtyID;
                        entity.GCROSystem = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(specialtyID, row.ID);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Specialty Vital Sign
        private void InitializeSpecialtyVitalSign(string queryString)
        {
            lblHeader.InnerText = "Specialty";
            Specialty sp = BusinessLayer.GetSpecialty(queryString);
            txtHeader.Text = sp.SpecialtyName;

            string filter = string.Format("VitalSignID NOT IN (SELECT VitalSignID FROM SpecialtyVitalSign WHERE SpecialtyID = '{0}') AND IsDeleted = 0", queryString);
            List<VitalSignType> ListAvailableItem = BusinessLayer.GetVitalSignTypeList(filter);
            List<vSpecialtyVitalSign> ListSelectedItem = BusinessLayer.GetvSpecialtyVitalSignList(string.Format("SpecialtyID = '{0}'", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.VitalSignName)
                             select new CMatrix { IsChecked = false, ID = p.VitalSignID.ToString(), Name = p.VitalSignName, ToolTip = p.VitalSignLabel }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.VitalSignName)
                            select new CMatrix { IsChecked = false, ID = p.VitalSignID.ToString(), Name = p.VitalSignName, ToolTip = p.VitalSignLabel }).ToList();
        }

        private bool SaveSpecialtyVitalSign(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                String specialtyID = queryString;
                SpecialtyVitalSignDao entityDao = new SpecialtyVitalSignDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        SpecialtyVitalSign entity = new SpecialtyVitalSign();
                        entity.SpecialtyID = specialtyID;
                        entity.VitalSignID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(specialtyID, Int32.Parse(row.ID));
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Symptom BodySystem
        private void InitializeSymptomBodySystem(string queryString)
        {
            lblHeader.InnerText = "Symptom";
            Symptom entity = BusinessLayer.GetSymptom(Convert.ToInt32(queryString));
            txtHeader.Text = entity.SymptomName;

            string filter = string.Format("StandardCodeID NOT IN (SELECT GCBodySystem FROM SystemSymptom WHERE SymptomID = {0}) AND ParentID = '{1}' AND IsDeleted = 0", queryString, Constant.StandardCode.BODY_PART_SYMPTOM_CHECKER);
            List<StandardCode> ListAvailableItem = BusinessLayer.GetStandardCodeList(filter);
            List<vSystemSymptom> ListSelectedItem = BusinessLayer.GetvSystemSymptomList(string.Format("SymptomID = '{0}'", queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.StandardCodeID)
                             select new CMatrix { IsChecked = false, ID = p.StandardCodeID.ToString(), Name = p.StandardCodeName, ToolTip = p.StandardCodeName }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.GCBodySystem)
                            select new CMatrix { IsChecked = false, ID = p.GCBodySystem.ToString(), Name = p.BodySystem, ToolTip = p.SymptomCode }).ToList();
        }

        private bool SaveSymptomBodySystem(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                Int32 SymptomID = Convert.ToInt32(queryString);
                SystemSymptomDao entityDao = new SystemSymptomDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        SystemSymptom entity = new SystemSymptom();
                        entity.SymptomID = SymptomID;
                        entity.GCBodySystem = row.ID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(SymptomID, row.ID);
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Symptom Diagnosis
        private void InitializeSymptomDiagnosis(string queryString)
        {
            lblHeader.InnerText = "Symptom";
            Symptom sp = BusinessLayer.GetSymptom(Convert.ToInt32(queryString));
            txtHeader.Text = sp.SymptomName;

            string filter = string.Format("DiagnoseID NOT IN (SELECT DiagnoseID FROM SymptomDiagnosis WHERE SymptomID = {0} AND IsDeleted = 0) AND IsDeleted = 0", queryString);
            List<Diagnose> ListAvailableItem = BusinessLayer.GetDiagnoseList(filter);
            List<vSymptomDiagnosis> ListSelectedItem = BusinessLayer.GetvSymptomDiagnosisList(string.Format("SymptomID = {0} AND IsDeleted = 0", Convert.ToInt32(queryString)));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.DiagnoseID)
                             select new CMatrix { IsChecked = false, ID = p.DiagnoseID.ToString(), Name = p.DiagnoseName, ToolTip = p.DiagnoseID }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.DiagnoseID)
                            select new CMatrix { IsChecked = false, ID = p.DiagnoseID.ToString(), Name = p.DiagnoseName, ToolTip = p.DiagnoseID }).ToList();
        }

        private bool SaveSymptomDiagnosis(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                Int32 SymptomID = Convert.ToInt32(queryString);
                SymptomDiagnosisDao entityDao = new SymptomDiagnosisDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        SymptomDiagnosis entity = new SymptomDiagnosis();
                        entity.SymptomID = SymptomID;
                        entity.DiagnoseID = row.ID;
                        entity.IsDeleted = false;

                        SymptomDiagnosis entitydel = entityDao.Get(SymptomID, row.ID);
                        if (entitydel == null)
                        {
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entity);
                        }
                        else
                        {
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }

                    }
                    else
                    {
                        SymptomDiagnosis entity = entityDao.Get(SymptomID, row.ID);
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                        //entityDao.Delete(SymptomID, row.ID);
                    }

                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region Test Template Item
        private void InitializeTestTemplateItem(string queryString)
        {
            lblHeader.InnerText = "Template Code";
            lblHeader2.InnerText = "Template Name";

            TestTemplateHd tth = BusinessLayer.GetTestTemplateHd(Convert.ToInt32(queryString));
            txtHeader.Text = tth.TestTemplateCode;
            txtHeader2.Text = tth.TestTemplateName;

            string filterAvailable = string.Format(
                "ItemID NOT IN (SELECT ItemID FROM TestTemplateDt WHERE TestTemplateID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0 AND GCItemStatus != '{2}' ORDER BY ItemName1",
                queryString, tth.GCItemType, Constant.ItemStatus.IN_ACTIVE);

            if (tth.HealthcareServiceUnitID != null && tth.HealthcareServiceUnitID != 0)
            {
                filterAvailable = string.Format(
                   "ItemID NOT IN (SELECT ItemID FROM TestTemplateDt WHERE TestTemplateID = {0}) AND GCItemType = '{1}' AND IsDeleted = 0 AND GCItemStatus != '{2}' AND ItemID IN (SELECT sui.ItemID FROM ServiceUnitItem sui WHERE sui.HealthcareServiceUnitID = {3}) ORDER BY ItemName1",
                   queryString, tth.GCItemType, Constant.ItemStatus.IN_ACTIVE, tth.HealthcareServiceUnitID);
            }

            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(filterAvailable);
            List<vTestTemplateDt> ListSelectedItem = BusinessLayer.GetvTestTemplateDtList(string.Format(
                "TestTemplateID = {0} ORDER BY ItemName1 ASC",
                queryString));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemCode }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemCode }).ToList();
        }

        private bool SaveTestTemplateItem(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int TestTemplateID = Convert.ToInt32(queryString);
                TestTemplateDtDao entityDao = new TestTemplateDtDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        TestTemplateDt entity = new TestTemplateDt();
                        entity.TestTemplateID = TestTemplateID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(TestTemplateID, Int32.Parse(row.ID));
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion
        #region UserRoleCOA
        private void InitializeUserRoleCOA(string queryString)
        {
            lblHeader.InnerText = GetLabel("UserRole");

            UserRole ur = BusinessLayer.GetUserRole(Convert.ToInt32(queryString));
            txtHeader.Text = ur.RoleName;

            string filterExpression = string.Format("IsDeleted = 0 AND GLAccountID NOT IN (SELECT GLAccountID FROM UserRoleCOA WHERE IsDeleted = 0 AND RoleID = {0} AND IsUsedAsTreasury = 0) ORDER BY GLAccountNo", ur.RoleID);
            List<ChartOfAccount> lstAllAvailable = BusinessLayer.GetChartOfAccountList(filterExpression);
            ListAvailable = (from p in lstAllAvailable.OrderBy(p => p.GLAccountNo)
                             select new CMatrix { IsChecked = false, ID = p.GLAccountID.ToString(), Name = string.Format("{0} ({1})", p.GLAccountName, p.GLAccountNo) }).ToList();

            filterExpression = string.Format("RoleID = {0} AND IsUsedAsTreasury = 0 AND IsDeleted = 0 ORDER BY GLAccountNo", queryString);
            List<vUserRoleCOA> lstSelected = BusinessLayer.GetvUserRoleCOAList(filterExpression);
            ListSelected = (from p in lstSelected.OrderBy(p => p.GLAccountNo)
                            select new CMatrix { IsChecked = false, ID = p.GLAccountID.ToString(), Name = string.Format("{0} ({1})", p.GLAccountName, p.GLAccountNo) }).ToList();
        }

        private bool SaveUserRoleCOA(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int oRoleID = Convert.ToInt32(queryString);
                UserRoleCOADao entityDao = new UserRoleCOADao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        UserRoleCOA entity = new UserRoleCOA();
                        entity.RoleID = oRoleID;
                        entity.GLAccountID = Convert.ToInt32(row.ID);
                        entity.IsUsedAsTreasury = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        string filterToDelete = string.Format("RoleID = {0} AND GLAccountID = {1} AND IsDeleted = 0", oRoleID, row.ID);
                        List<UserRoleCOA> entityList = BusinessLayer.GetUserRoleCOAList(filterToDelete);
                        if (entityList.Count() > 0)
                        {
                            UserRoleCOA entity = entityList.FirstOrDefault();
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }
                    }
                }
                ctx.CommitTransaction();
                result = true;
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

        #endregion
        #region UserRoleCOATreasury
        private void InitializeUserRoleCOATreasury(string queryString)
        {
            lblHeader.InnerText = GetLabel("UserRole");

            UserRole ur = BusinessLayer.GetUserRole(Convert.ToInt32(queryString));
            txtHeader.Text = ur.RoleName;

            string filterExpression = string.Format("IsDeleted = 0 AND IsUsedAsTreasury = 1 AND GLAccountID NOT IN (SELECT GLAccountID FROM UserRoleCOA WHERE IsDeleted = 0 AND RoleID = {0} AND IsUsedAsTreasury = 1) ORDER BY GLAccountNo", ur.RoleID);
            List<ChartOfAccount> lstAllAvailable = BusinessLayer.GetChartOfAccountList(filterExpression);
            ListAvailable = (from p in lstAllAvailable.OrderBy(p => p.GLAccountNo)
                             select new CMatrix { IsChecked = false, ID = p.GLAccountID.ToString(), Name = string.Format("{0} ({1})", p.GLAccountName, p.GLAccountNo) }).ToList();

            filterExpression = string.Format("RoleID = {0} AND IsUsedAsTreasury = 1 AND IsDeleted = 0 ORDER BY GLAccountNo", queryString);
            List<vUserRoleCOA> lstSelected = BusinessLayer.GetvUserRoleCOAList(filterExpression);
            ListSelected = (from p in lstSelected.OrderBy(p => p.GLAccountNo)
                            select new CMatrix { IsChecked = false, ID = p.GLAccountID.ToString(), Name = string.Format("{0} ({1})", p.GLAccountName, p.GLAccountNo) }).ToList();
        }

        private bool SaveUserRoleCOATreasury(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int oRoleID = Convert.ToInt32(queryString);
                UserRoleCOADao entityDao = new UserRoleCOADao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        UserRoleCOA entity = new UserRoleCOA();
                        entity.RoleID = oRoleID;
                        entity.GLAccountID = Convert.ToInt32(row.ID);
                        entity.IsUsedAsTreasury = true;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        string filterToDelete = string.Format("RoleID = {0} AND GLAccountID = {1} AND IsUsedAsTreasury = 1 AND IsDeleted = 0", oRoleID, row.ID);
                        List<UserRoleCOA> entityList = BusinessLayer.GetUserRoleCOAList(filterToDelete);
                        if (entityList.Count() > 0)
                        {
                            UserRoleCOA entity = entityList.FirstOrDefault();
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }
                    }
                }
                ctx.CommitTransaction();
                result = true;
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

        #endregion

        #region ExclusionPhysicianItem
        private void InitializeExclusionPhysicianItemCharges(string queryString)
        {
            lblHeader.InnerText = "Dokter / Tenaga Medis";

            ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(queryString));
            txtHeader.Text = pm.FullName;

            List<ItemMaster> ListAvailableItem = BusinessLayer.GetItemMasterList(string.Format(
                                    "GCItemType = '{0}' AND ItemID NOT IN (SELECT ItemID FROM ExclusionPhysicianItem WHERE ParamedicID = {1}) AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                                    Constant.ItemType.PELAYANAN, queryString));
            List<vExclusionPhysicianItem> ListSelectedItem = BusinessLayer.GetvExclusionPhysicianItemList(string.Format(
                                    "ParamedicID = {0} AND GCItemType = '{1}' ORDER BY ItemName1 ASC", queryString, Constant.ItemType.PELAYANAN));

            ListAvailable = (from p in ListAvailableItem.OrderBy(p => p.ItemName1)
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItem.OrderBy(p => p.ItemName1)
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1, ToolTip = p.ItemName1 }).ToList();
        }

        private bool SaveExclusionPhysicianItemCharges(string queryString, ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            ExclusionPhysicianItemDao entityDao = new ExclusionPhysicianItemDao(ctx);
            try
            {
                int ParamedicID = Convert.ToInt32(queryString);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        ExclusionPhysicianItem entity = new ExclusionPhysicianItem();
                        entity.ParamedicID = ParamedicID;
                        entity.ItemID = Int32.Parse(row.ID);
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        entityDao.Delete(ParamedicID, Int32.Parse(row.ID));
                    }
                }
                ctx.CommitTransaction();
                result = true;
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
        #endregion

        private void InitializeListMatrix(string type, string queryString)
        {
            switch (type)
            {
                case "BillingGroupItem": InitializeBillingGroupItem(queryString); break;
                case "ContractCoverage": InitializeContractCoverage(queryString); break;
                case "ContractCoverageMember": InitializeContractCoverageMember(queryString); break;
                case "CustomerDiagnose": InitializeCustomersDiagnose(queryString); break;
                case "CustomerMember": InitializeCustomerMember(queryString); break;
                //case "ChargesTemplateItemService": InitializeChargesTemplateItemService(queryString); break;
                //case "ChargesTemplateItemDrug": InitializeChargesTemplateItemDrug(queryString); break;
                //case "ChargesTemplateItemLogistic": InitializeChargesTemplateItemLogistic(queryString); break;
                case "DiagnoseDrugs": InitializeDiagnoseDrugs(queryString); break;
                case "DiagnoseTest": InitializeDiagnoseTest(queryString); break;
                case "GLAccountPayable": InitializeGLAccountPayable(queryString); break;
                case "GLAccountReceivable": InitializeGLAccountReceivable(queryString); break;
                case "GLAPRevenueSharing": InitializeGLAPRevenueSharing(queryString); break;
                case "GLCashFlowAccountDt": InitializeGLCashFlowAccountDt(queryString); break;
                case "GLIPFinalDiscountDt": InitializeGLIPFinalDiscountDt(queryString); break;
                case "GLIPRevenueAccountDrugMS": InitializeGLIPRevenueAccountDrugMS(queryString); break;
                case "GLIPRevenueAccountLogistic": InitializeGLIPRevenueAccountLogistic(queryString); break;
                case "GLIPRevenueAccountService": InitializeGLIPRevenueAccountService(queryString); break;
                case "GLIPRevenueSharingAccountDt": InitializeGLIPRevenueSharingAccountDt(queryString); break;
                case "GLMDRevenueAccountDrugMS": InitializeGLMDRevenueAccountDrugMS(queryString); break;
                case "GLMDRevenueAccountLogistic": InitializeGLMDRevenueAccountLogistic(queryString); break;
                case "GLMDRevenueAccountService": InitializeGLMDRevenueAccountService(queryString); break;
                case "GLMDRevenueSharingAccountDt": InitializeGLMDRevenueSharingAccountDt(queryString); break;
                case "GLOPFinalDiscountDt": InitializeGLOPFinalDiscountDt(queryString); break;
                case "GLOPRevenueAccountDrugMS": InitializeGLOPRevenueAccountDrugMS(queryString); break;
                case "GLOPRevenueAccountLogistic": InitializeGLOPRevenueAccountLogistic(queryString); break;
                case "GLOPRevenueAccountService": InitializeGLOPRevenueAccountService(queryString); break;
                case "GLOPRevenueSharingAccountDt": InitializeGLOPRevenueSharingAccountDt(queryString); break;
                case "GLOTCRevenueAccountDt": InitializeGLOTCRevenueAccountDt(queryString); break;
                case "GLPaymentMethod": InitializeGLPaymentMethod(queryString); break;
                case "GLPaymentTypeDt": InitializeGLPaymentTypeDt(queryString); break;
                case "GLRevenueAccount": InitializeGLRevenueAccount(queryString); break;
                case "GLWarehouseProductLineAccountDt": InitializeGLWarehouseProductLineAccountDt(queryString); break;
                case "HealthcarePromotionScheme": InitializeHealthcarePromotionScheme(queryString); break;
                case "MealPlanDt": InitializeMealPlanDt(queryString); break;
                case "MedicalRecord": InitializeMedicalRecord(queryString); break;
                case "ModuleMenu": InitializeModuleMenu(queryString); break;
                case "NursingProblemDiagnosis": InitializeNursingProblemDiagnosis(queryString); break;
                case "NutritionOrderHdDietType": InitializeNutritionOrderHdDietType(queryString); break;
                case "PhysicianCompoundTemplate": InitializePhysicianCompoundTemplate(queryString); break;
                case "PhysicianItemCharges": InitializePhysicianItemCharges(queryString); break;
                case "ExclusionPhysicianItem": InitializeExclusionPhysicianItemCharges(queryString); break;
                case "PhysicianItemDrugs": InitializePhysicianItemDrugs(queryString); break;
                case "PhysicianItemLogistics": InitializePhysicianItemLogistics(queryString); break;
                case "PhysicianTemplete": InitializePhysicianTemplete(queryString); break;
                case "ProcedureGroupPanel": InitializeProcedureGroupPanel(queryString); break;
                case "ProcedureItemService": InitializeProcedureItemService(queryString); break;
                case "PromotionSchemeItemGroupMapping": InitializePromotionSchemeItemGroupMapping(queryString); break;
                case "ReportByMenuMaster": InitializeReportByMenuMaster(queryString); break;
                case "RestrictionDt": InitializeRestrictionDt(queryString); break;
                case "RevenueCostCenterDt": InitializeRevenueCostCenterDt(queryString); break;
                case "RevenueCostCenterFALocation": InitializeRevenueCostCenterFALocation(queryString); break;
                case "RevenueCostCenterHealthcareUnit": InitializeRevenueCostCenterHealthcareUnit(queryString); break;
                case "RevenueCostCenterLocation": InitializeRevenueCostCenterLocation(queryString); break;
                case "RevenueSharingCustomerType": InitializeRevenueSharingCustomerType(queryString); break;
                case "RevenueSharingEmploymentStatus": InitializeRevenueSharingEmploymentStatus(queryString); break;
                case "RevenueSharingHealthcareServiceUnit": InitializeRevenueSharingHealthcareServiceUnit(queryString); break;
                case "RevenueSharingItemGroupMaster": InitializeRevenueSharingItemGroupMaster(queryString); break;
                case "RevenueSharingItemMaster": InitializeRevenueSharingItemMaster(queryString); break;
                case "RevenueSharingParamedicMaster": InitializeRevenueSharingParamedicMaster(queryString); break;
                case "ServiceUnitItem": InitializeServiceUnitItem(queryString); break;
                case "ServiceUnitForm": InitializeServiceUnitForm(queryString); break;
                case "ServiceUnitVitalSign": InitializeServiceUnitVitalSign(queryString); break;
                case "ServiceUnitParamedic": InitializeServiceUnitParamedic(queryString); break;
                case "SpecialtyBodyDiagram": InitializeSpecialtyBodyDiagram(queryString); break;
                case "SpecialtyProcedure": InitializeSpecialtyProcedure(queryString); break;
                case "SpecialtyROS": InitializeSpecialtyROS(queryString); break;
                case "SpecialtyVitalSign": InitializeSpecialtyVitalSign(queryString); break;
                case "SymptomBodySystem": InitializeSymptomBodySystem(queryString); break;
                case "SymptomDiagnosis": InitializeSymptomDiagnosis(queryString); break;
                case "TestTemplateItem": InitializeTestTemplateItem(queryString); break;
                case "NursingDiagnoseIntervention": InitializeNursingDiagnoseIntervention(queryString); break;
                case "PharmacogeneticItem": InitializePharmacogeneticItem(queryString); break;
                case "UserRoleCOA": InitializeUserRoleCOA(queryString); break;
                case "UserRoleCOATreasury": InitializeUserRoleCOATreasury(queryString); break;
            }
        }

        private bool SaveMatrix(string type, string queryString, ref string errMessage)
        {
            switch (type)
            {
                case "BillingGroupItem": return SaveBillingGroupItem(queryString, ref errMessage);
                case "ContractCoverage": return SaveContractCoverage(queryString, ref errMessage);
                case "ContractCoverageMember": return SaveContractCoverageMember(queryString, ref errMessage);
                case "CustomerDiagnose": return SaveCustomersDiagnose(queryString, ref errMessage);
                case "CustomerMember": return SaveCustomerMember(queryString, ref errMessage);
                //case "ChargesTemplateItemService": return SaveChargesTemplateItemService(queryString, ref errMessage);
                //case "ChargesTemplateItemDrug": return SaveChargesTemplateItemDrug(queryString, ref errMessage);
                //case "ChargesTemplateItemLogistic": return SaveChargesTemplateItemLogistic(queryString, ref errMessage);
                case "DiagnoseDrugs": return SaveDiagnoseDrugs(queryString, ref errMessage);
                case "DiagnoseTest": return SaveDiagnoseTest(queryString, ref errMessage);
                case "GLAccountPayable": return SaveGLAccountPayable(queryString, ref errMessage);
                case "GLAccountReceivable": return SaveGLAccountReceivable(queryString, ref errMessage);
                case "GLAPRevenueSharing": return SaveGLAPRevenueSharing(queryString, ref errMessage);
                case "GLCashFlowAccountDt": return SaveGLCashFlowAccountDt(queryString, ref errMessage);
                case "GLIPFinalDiscountDt": return SaveGLIPFinalDiscountDt(queryString, ref errMessage);
                case "GLIPRevenueAccountDrugMS": return SaveGLIPRevenueAccountDrugMS(queryString, ref errMessage);
                case "GLIPRevenueAccountLogistic": return SaveGLIPRevenueAccountLogistic(queryString, ref errMessage);
                case "GLIPRevenueAccountService": return SaveGLIPRevenueAccountService(queryString, ref errMessage);
                case "GLIPRevenueSharingAccountDt": return SaveGLIPRevenueSharingAccountDt(queryString, ref errMessage);
                case "GLMDRevenueAccountDrugMS": return SaveGLMDRevenueAccountDrugMS(queryString, ref errMessage);
                case "GLMDRevenueAccountLogistic": return SaveGLMDRevenueAccountLogistic(queryString, ref errMessage);
                case "GLMDRevenueAccountService": return SaveGLMDRevenueAccountService(queryString, ref errMessage);
                case "GLMDRevenueSharingAccountDt": return SaveGLMDRevenueSharingAccountDt(queryString, ref errMessage);
                case "GLOPFinalDiscountDt": return SaveGLOPFinalDiscountDt(queryString, ref errMessage);
                case "GLOPRevenueAccountDrugMS": return SaveGLOPRevenueAccountDrugMS(queryString, ref errMessage);
                case "GLOPRevenueAccountLogistic": return SaveGLOPRevenueAccountLogistic(queryString, ref errMessage);
                case "GLOPRevenueAccountService": return SaveGLOPRevenueAccountService(queryString, ref errMessage);
                case "GLOPRevenueSharingAccountDt": return SaveGLOPRevenueSharingAccountDt(queryString, ref errMessage);
                case "GLOTCRevenueAccountDt": return SaveGLOTCRevenueAccountDt(queryString, ref errMessage);
                case "GLPaymentMethod": return SaveGLPaymentMethod(queryString, ref errMessage);
                case "GLPaymentTypeDt": return SaveGLPaymentTypeDt(queryString, ref errMessage);
                case "GLRevenueAccount": return SaveGLRevenueAccount(queryString, ref errMessage);
                case "GLWarehouseProductLineAccountDt": return SaveGLWarehouseProductLineAccountDt(queryString, ref errMessage);
                case "HealthcarePromotionScheme": return SaveHealthcarePromotionScheme(queryString, ref errMessage);
                case "MealPlanDt": return SaveMealPlanDt(queryString, ref errMessage);
                case "MedicalRecord": return SaveMedicalRecord(queryString, ref errMessage);
                case "ModuleMenu": return SaveModuleMenu(queryString, ref errMessage);
                case "NursingProblemDiagnosis": return SaveNursingProblemDiagnosis(queryString, ref errMessage);
                case "NutritionOrderHdDietType": return SaveNutritionOrderHdDietType(queryString, ref errMessage);
                case "PhysicianCompoundTemplate": return SavePhysicianCompoundTemplate(queryString, ref errMessage);
                case "PhysicianItemCharges": return SavePhysicianItemCharges(queryString, ref errMessage);
                case "ExclusionPhysicianItem": return SaveExclusionPhysicianItemCharges(queryString, ref errMessage);
                case "PhysicianItemDrugs": return SavePhysicianItemDrugs(queryString, ref errMessage);
                case "PhysicianItemLogistics": return SavePhysicianItemLogistics(queryString, ref errMessage);
                case "PromotionSchemeItemGroupMapping": return SavePromotionSchemeItemGroupMapping(queryString, ref errMessage);
                case "ServiceUnitItem": return SaveServiceUnitItem(queryString, ref errMessage);
                case "ServiceUnitForm": return SaveServiceUnitForm(queryString, ref errMessage);
                case "ServiceUnitVitalSign": return SaveServiceUnitVitalSign(queryString, ref errMessage);
                case "RestrictionDt": return SaveRestrictionDt(queryString, ref errMessage);
                case "ReportByMenuMaster": return SaveReportByMenuMaster(queryString, ref errMessage);
                case "RevenueCostCenterDt": return SaveRevenueCostCenterDt(queryString, ref errMessage);
                case "RevenueCostCenterFALocation": return SaveRevenueCostCenterFALocation(queryString, ref errMessage);
                case "RevenueCostCenterHealthcareUnit": return SaveRevenueCostCenterHealthcareUnit(queryString, ref errMessage);
                case "RevenueCostCenterLocation": return SaveRevenueCostCenterLocation(queryString, ref errMessage);
                case "RevenueSharingEmploymentStatus": return SaveRevenueSharingEmploymentStatus(queryString, ref errMessage);
                case "RevenueSharingCustomerType": return SaveRevenueSharingCustomerType(queryString, ref errMessage);
                case "RevenueSharingHealthcareServiceUnit": return SaveRevenueSharingHealthcareServiceUnit(queryString, ref errMessage);
                case "RevenueSharingItemGroupMaster": return SaveRevenueSharingItemGroupMaster(queryString, ref errMessage);
                case "RevenueSharingItemMaster": return SaveRevenueSharingItemMaster(queryString, ref errMessage);
                case "RevenueSharingParamedicMaster": return SaveRevenueSharingParamedicMaster(queryString, ref errMessage);
                case "ProcedureGroupPanel": return SaveProcedureGroupPanel(queryString, ref errMessage);
                case "ProcedureItemService": return SaveProcedureItemService(queryString, ref errMessage);
                case "ServiceUnitParamedic": return SaveServiceUnitParamedic(queryString, ref errMessage);
                case "SpecialtyBodyDiagram": return SaveSpecialtyBodyDiagram(queryString, ref errMessage);
                case "SpecialtyProcedure": return SaveSpecialtyProcedure(queryString, ref errMessage);
                case "SpecialtyROS": return SaveSpecialtyROS(queryString, ref errMessage);
                case "SpecialtyVitalSign": return SaveSpecialtyVitalSign(queryString, ref errMessage);
                case "SymptomBodySystem": return SaveSymptomBodySystem(queryString, ref errMessage);
                case "SymptomDiagnosis": return SaveSymptomDiagnosis(queryString, ref errMessage);
                case "TestTemplateItem": return SaveTestTemplateItem(queryString, ref errMessage);
                case "PhysicianTemplete": return SavePhysicianTemplete(queryString, ref errMessage);
                case "NursingDiagnoseIntervention": return SaveNursingDiagnoseIntervention(queryString, ref errMessage);
                case "PharmacogeneticItem": return SavePharmacogeneticItem(queryString, ref errMessage);
                case "UserRoleCOA": return SaveUserRoleCOA(queryString, ref errMessage);
                case "UserRoleCOATreasury": return SaveUserRoleCOATreasury(queryString, ref errMessage);
            }
            return false;
        }


        protected int PageCountAvailable = 1;
        protected int PageCountSelected = 1;

        public override void InitializeDataControl(string param)
        {
            ListProceedEntity.Clear();
            hdnParam.Value = param;

            string type = param.Split('|')[0];
            string[] temp = param.Split('|').Skip(1).ToArray();
            string queryString = String.Join("|", temp);

            InitializeListMatrix(type, queryString);

            BindGridAvailable(1, true, ref PageCountAvailable);
            BindGridSelected(1, true, ref PageCountSelected);
        }

        #region Available
        private void BindGridAvailable(int pageIndex, bool isCountPageCount, ref int pageCount, List<string> listCheckedAvailable = null)
        {
            List<CMatrix> lstEntity = ListAvailable.Where(p => p.Name.ToUpper().Contains(hdnAvailableSearchText.Value.ToUpper())).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrix> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrix mtx in lst)
            {
                if (listCheckedAvailable != null && listCheckedAvailable.Contains(mtx.ID.ToString()))
                {
                    mtx.IsChecked = true;
                    listCheckedAvailable.Remove(mtx.ID.ToString());
                }
                else
                    mtx.IsChecked = false;
            }

            grdAvailable.DataSource = lst;
            grdAvailable.DataBind();
        }

        protected void cbpMatrixAvailable_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            List<string> listCheckedAvailable = hdnCheckedAvailable.Value.Split(';').ToList();
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    string[] newCheckedAvailable = param[2].Split(';');
                    foreach (string a in newCheckedAvailable)
                    {
                        if (a != "")
                            listCheckedAvailable.Add(a);
                    }

                    BindGridAvailable(Convert.ToInt32(param[1]), false, ref pageCount, listCheckedAvailable);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridAvailable(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpCheckedAvailable"] = string.Join(";", listCheckedAvailable.ToArray());
        }
        #endregion

        #region Selected
        private void BindGridSelected(int pageIndex, bool isCountPageCount, ref int pageCount, List<string> listCheckedSelected = null)
        {
            List<CMatrix> lstEntity = ListSelected.Where(p => p.Name.ToUpper().Contains(hdnSelectedSearchText.Value.ToUpper())).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrix> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrix mtx in lst)
            {
                if (listCheckedSelected != null && listCheckedSelected.Contains(mtx.ID.ToString()))
                {
                    mtx.IsChecked = true;
                    listCheckedSelected.Remove(mtx.ID.ToString());
                }
                else
                    mtx.IsChecked = false;
            }

            grdSelected.DataSource = lst;
            grdSelected.DataBind();
        }

        protected void cbpMatrixSelected_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            List<string> listCheckedSelected = hdnCheckedSelected.Value.Split(';').ToList();
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    string[] newCheckedSelected = param[2].Split(';');
                    foreach (string a in newCheckedSelected)
                    {
                        if (a != "")
                            listCheckedSelected.Add(a);
                    }

                    BindGridSelected(Convert.ToInt32(param[1]), false, ref pageCount, listCheckedSelected);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridSelected(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpCheckedSelected"] = string.Join(";", listCheckedSelected.ToArray());

        }
        #endregion


        protected void cbpMatrixProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            if (param[0] == "rightAll")
            {
                //                List<CMatrix> lst = ListAvailable.Where(p => p.Name.Contains(hdnAvailableSearchText.Value)).ToList();
                List<CMatrix> lst = ListAvailable.Where(p => p.Name.ToUpper().Contains(hdnAvailableSearchText.Value.ToUpper())).ToList();
                //                List<CMatrix> lst = ListAvailable.Where(p => p.Name.Contains(hdnAvailableSearchText.Value) && p.Name.EndsWith(hdnAvailableSearchText.Value)).ToList();


                foreach (CMatrix row in lst)
                {
                    ListSelected.Add(row);

                    ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == row.ID.ToString());
                    if (obj != null)
                        ListProceedEntity.Remove(obj);
                    else
                    {
                        ProceedEntity proceedEntity = new ProceedEntity();
                        proceedEntity.ID = row.ID.ToString();
                        proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Add;
                        ListProceedEntity.Add(proceedEntity);
                    }
                }
                ListSelected = ListSelected.ToList();
                ListAvailable.RemoveAll(x => x.Name.ToUpper().Contains(hdnAvailableSearchText.Value.ToUpper()));
            }
            else if (param[0] == "right")
            {
                List<string> listCheckedAvailable = hdnCheckedAvailable.Value.Split(';').ToList();
                string[] newCheckedAvailable = param[1].Split(';');
                foreach (string a in newCheckedAvailable)
                {
                    if (a != "")
                        listCheckedAvailable.Add(a);
                }

                foreach (string value in listCheckedAvailable)
                {
                    if (value != "")
                    {
                        ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == value.ToString());
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntity proceedEntity = new ProceedEntity();
                            proceedEntity.ID = value.ToString();
                            proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Add;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrix removeObj = ListAvailable.FirstOrDefault(p => p.ID.ToString() == value);
                        if (removeObj != null)
                        {
                            ListSelected.Add(removeObj);
                            ListAvailable.Remove(removeObj);
                        }
                    }
                }

                ListSelected = ListSelected.ToList();
            }
            else if (param[0] == "left")
            {
                List<string> listCheckedSelected = hdnCheckedSelected.Value.Split(';').ToList();
                string[] newCheckedSelected = param[1].Split(';');
                foreach (string a in newCheckedSelected)
                {
                    if (a != "")
                        listCheckedSelected.Add(a);
                }

                foreach (string value in listCheckedSelected)
                {
                    if (value != "")
                    {
                        ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == value.ToString());
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntity proceedEntity = new ProceedEntity();
                            proceedEntity.ID = value.ToString();
                            proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Remove;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrix removeObj = ListSelected.FirstOrDefault(p => p.ID.ToString() == value);
                        if (removeObj != null)
                        {
                            ListAvailable.Add(removeObj);
                            ListSelected.Remove(removeObj);
                        }
                    }
                }

                ListAvailable = ListAvailable.ToList();
            }
            else if (param[0] == "leftAll")
            {
                List<CMatrix> lst = ListSelected.Where(p => p.Name.Contains(hdnSelectedSearchText.Value)).ToList();
                foreach (CMatrix row in lst)
                {
                    ListAvailable.Add(row);

                    ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == row.ID.ToString());
                    if (obj != null)
                        ListProceedEntity.Remove(obj);
                    else
                    {
                        ProceedEntity proceedEntity = new ProceedEntity();
                        proceedEntity.ID = row.ID.ToString();
                        proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Remove;
                        ListProceedEntity.Add(proceedEntity);
                    }
                }
                ListAvailable = ListAvailable.ToList();
                ListSelected.RemoveAll(x => x.Name.Contains(hdnSelectedSearchText.Value));
            }
            else if (param[0] == "save")
            {
                string errMessage = "";
                string paramTemp = hdnParam.Value;

                string type = paramTemp.Split('|')[0];
                string[] temp = paramTemp.Split('|').Skip(1).ToArray();
                string queryString = String.Join("|", temp);

                if (SaveMatrix(type, queryString, ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private const string SESSION_NAME_SELECTED_ENTITY = "SelectedEntity";
        private const string SESSION_NAME_AVAILABLE_ENTITY = "AvailableEntity";
        private const string SESSION_PROCEED_ENTITY = "ProceedEntity";

        #region Matrix
        public static List<CMatrix> ListSelected
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] = new List<CMatrix>();
                return (List<CMatrix>)HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] = value;
            }
        }
        public static List<CMatrix> ListAvailable
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] = new List<CMatrix>();
                return (List<CMatrix>)HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] = value;
            }
        }

        private static List<ProceedEntity> ListProceedEntity
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_PROCEED_ENTITY] == null) HttpContext.Current.Session[SESSION_PROCEED_ENTITY] = new List<ProceedEntity>();
                return (List<ProceedEntity>)HttpContext.Current.Session[SESSION_PROCEED_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_PROCEED_ENTITY] = value;
            }
        }

        public static List<CMatrix> SelectListAvailableEntity()
        {
            return ListAvailable;
        }
        public static List<CMatrix> SelectListSelectedEntity()
        {
            return ListSelected;
        }
        #endregion
    }
}