using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Text;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemDrugDetailCtl : BaseEntryPopupCtl
    {
        protected int columnCount = 0;
        public override void InitializeDataControl(string queryString)
        {
            string[] param = queryString.Split('|');
            hdnItemID.Value = param[0];
            hdnDrugDetailType.Value = param[1];
            IsAdd = true;

            if (param[1] == "10")
            {
                List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}) AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
                Methods.SetComboBoxField<StandardCode>(cboAlternateUnit, lstSc, "StandardCodeName", "StandardCodeID");
            }

            InitGrdDrugDetail();

        }
        private string[] getColumn()
        {
            int drugDetailType = Convert.ToInt32(hdnDrugDetailType.Value);
            switch (drugDetailType)
            {
                case 1: return new string[] { "Keyword", "Description" };
                case 2: return new string[] { "Keyword", "Indication #1", "Indication #2" };
                case 3: return new string[] { "Keyword", "Dosage #1", "Dosage #2" };
                case 4: return new string[] { "Administration #1", "Administration #2" };
                case 5: return new string[] { "Contra Indication #1", "Contra Indication #2" };
                case 6: return new string[] { "Special Precaution #1", "Special Precaution #2" };
                case 7: return new string[] { "Adverse Reaction #1", "Adverse Reaction #2" };
                case 10:
                    string baseUnit = BusinessLayer.GetvItemProductList(string.Format("ItemID = {0}", hdnItemID.Value))[0].ItemUnit;
                    return new string[] { "Alternate Unit", "Conversion x Unit of Measure ( " + baseUnit + " )" };
                default: return null;
            }
        }

        private void InitGrdDrugDetail()
        {
            string[] columns = getColumn();
            columnCount = columns.Length;
            int width = 400 / columnCount;

            List<CItemDrugDetailTable> lst = new List<CItemDrugDetailTable>();
            int drugDetailType = Convert.ToInt32(hdnDrugDetailType.Value);
            if (drugDetailType == 10)
            {
                lst.Add(new CItemDrugDetailTable { Caption = columns[0], Width = 120 });
                lst.Add(new CItemDrugDetailTable { Caption = columns[1], Width = 280 });
            }
            else
            {
                foreach (string col in columns)
                    lst.Add(new CItemDrugDetailTable { Caption = col, Width = width });
            }
            rptTblDrugDetail.DataSource = lst;
            rptTblDrugDetail.DataBind();
        }

        public class CItemDrugDetailTable
        {
            public string Caption { get; set; }
            public int Width { get; set; }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');

            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int drugDetailType = Convert.ToInt32(hdnDrugDetailType.Value);
                int itemID = Convert.ToInt32(hdnItemID.Value);

                #region Drug Content
                if (drugDetailType == 1)
                {
                    DrugContentDao entityDao = new DrugContentDao(ctx);

                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    if (listID.ToString() != "")
                    {
                        string filterExpression = string.Format("ID NOT IN ({0})", listID.ToString());
                        List<DrugContent> lstDeletedEntity = BusinessLayer.GetDrugContentList(filterExpression, ctx);
                        foreach (DrugContent deletedEntity in lstDeletedEntity)
                        {
                            entityDao.Delete(deletedEntity.ID);
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                DrugContent entity = entityDao.Get(ID);
                                entity.Keyword = data[2];
                                entity.ContentText = data[3];
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                DrugContent entity = new DrugContent();
                                entity.ItemID = itemID;
                                entity.Keyword = data[2];
                                entity.ContentText = data[3];
                                entityDao.Insert(entity);
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region Drug Indication
                else if (drugDetailType == 2)
                {
                    DrugIndicationDao entityDao = new DrugIndicationDao(ctx);

                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    if (listID.ToString() != "")
                    {
                        string filterExpression = string.Format("ID NOT IN ({0})", listID.ToString());
                        List<DrugIndication> lstDeletedEntity = BusinessLayer.GetDrugIndicationList(filterExpression, ctx);
                        foreach (DrugIndication deletedEntity in lstDeletedEntity)
                        {
                            entityDao.Delete(deletedEntity.ID);
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                DrugIndication entity = entityDao.Get(ID);
                                entity.Keyword = data[2];
                                entity.IndicationText1 = data[3];
                                entity.IndicationText2 = data[4];
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                DrugIndication entity = new DrugIndication();
                                entity.ItemID = itemID;
                                entity.Keyword = data[2];
                                entity.IndicationText1 = data[3];
                                entity.IndicationText2 = data[4];
                                entityDao.Insert(entity);
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region Drug Dosage
                else if (drugDetailType == 3)
                {
                    DrugDosageDao entityDao = new DrugDosageDao(ctx);

                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    if (listID.ToString() != "")
                    {
                        string filterExpression = string.Format("ID NOT IN ({0})", listID.ToString());
                        List<DrugDosage> lstDeletedEntity = BusinessLayer.GetDrugDosageList(filterExpression, ctx);
                        foreach (DrugDosage deletedEntity in lstDeletedEntity)
                        {
                            entityDao.Delete(deletedEntity.ID);
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                DrugDosage entity = entityDao.Get(ID);
                                entity.Keyword = data[2];
                                entity.DosageText1 = data[3];
                                entity.DosageText2 = data[4];
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                DrugDosage entity = new DrugDosage();
                                entity.ItemID = itemID;
                                entity.Keyword = data[2];
                                entity.DosageText1 = data[3];
                                entity.DosageText2 = data[4];
                                entityDao.Insert(entity);
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region Drug Administration
                else if (drugDetailType == 4)
                {
                    DrugAdministrationDao entityDao = new DrugAdministrationDao(ctx);

                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    if (listID.ToString() != "")
                    {
                        string filterExpression = string.Format("ID NOT IN ({0})", listID.ToString());
                        List<DrugAdministration> lstDeletedEntity = BusinessLayer.GetDrugAdministrationList(filterExpression, ctx);
                        foreach (DrugAdministration deletedEntity in lstDeletedEntity)
                        {
                            entityDao.Delete(deletedEntity.ID);
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                DrugAdministration entity = entityDao.Get(ID);
                                entity.AdministrationText1 = data[2];
                                entity.AdministrationText2 = data[3];
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                DrugAdministration entity = new DrugAdministration();
                                entity.ItemID = itemID;
                                entity.AdministrationText1 = data[2];
                                entity.AdministrationText2 = data[3];
                                entityDao.Insert(entity);
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region Drug Contra Indication
                else if (drugDetailType == 5)
                {
                    DrugContraIndicationDao entityDao = new DrugContraIndicationDao(ctx);

                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    if (listID.ToString() != "")
                    {
                        string filterExpression = string.Format("ID NOT IN ({0})", listID.ToString());
                        List<DrugContraIndication> lstDeletedEntity = BusinessLayer.GetDrugContraIndicationList(filterExpression, ctx);
                        foreach (DrugContraIndication deletedEntity in lstDeletedEntity)
                        {
                            entityDao.Delete(deletedEntity.ID);
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                DrugContraIndication entity = entityDao.Get(ID);
                                entity.ContraIndicationText1 = data[2];
                                entity.ContraIndicationText2 = data[3];
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                DrugContraIndication entity = new DrugContraIndication();
                                entity.ItemID = itemID;
                                entity.ContraIndicationText1 = data[2];
                                entity.ContraIndicationText2 = data[3];
                                entityDao.Insert(entity);
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region Drug Precaution
                else if (drugDetailType == 6)
                {
                    DrugPrecautionDao entityDao = new DrugPrecautionDao(ctx);

                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    if (listID.ToString() != "")
                    {
                        string filterExpression = string.Format("ID NOT IN ({0})", listID.ToString());
                        List<DrugPrecaution> lstDeletedEntity = BusinessLayer.GetDrugPrecautionList(filterExpression, ctx);
                        foreach (DrugPrecaution deletedEntity in lstDeletedEntity)
                        {
                            entityDao.Delete(deletedEntity.ID);
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                DrugPrecaution entity = entityDao.Get(ID);
                                entity.PrecautionText1 = data[2];
                                entity.PrecautionText2 = data[3];
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                DrugPrecaution entity = new DrugPrecaution();
                                entity.ItemID = itemID;
                                entity.PrecautionText1 = data[2];
                                entity.PrecautionText2 = data[3];
                                entityDao.Insert(entity);
                            }
                            #endregion
                        }
                    }
                }

                #endregion
                #region Drug Reaction
                else if (drugDetailType == 7)
                {
                    DrugReactionDao entityDao = new DrugReactionDao(ctx);

                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    if (listID.ToString() != "")
                    {
                        string filterExpression = string.Format("ID NOT IN ({0})", listID.ToString());
                        List<DrugReaction> lstDeletedEntity = BusinessLayer.GetDrugReactionList(filterExpression, ctx);
                        foreach (DrugReaction deletedEntity in lstDeletedEntity)
                        {
                            entityDao.Delete(deletedEntity.ID);
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                DrugReaction entity = entityDao.Get(ID);
                                entity.AdverseReactionText1 = data[2];
                                entity.AdverseReactionText2 = data[3];
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                DrugReaction entity = new DrugReaction();
                                entity.ItemID = itemID;
                                entity.AdverseReactionText1 = data[2];
                                entity.AdverseReactionText2 = data[3];
                                entityDao.Insert(entity);
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region Item Alternate
                else if (drugDetailType == 10)
                {
                    ItemAlternateUnitDao entityDao = new ItemAlternateUnitDao(ctx);
                    #region Delete
                    StringBuilder listID = new StringBuilder();
                    foreach (String param in listParam)
                    {
                        int ID = Convert.ToInt32(param.Split(';')[1]);
                        if (ID > 0)
                        {
                            if (listID.ToString() != "")
                                listID.Append(", ");
                            listID.Append(ID);
                        }
                    }
                    string filterExpression = "";
                    if (listID.ToString() != "")
                        filterExpression = string.Format("ItemID = {0} AND ID NOT IN ({1}) AND IsDeleted = 0", itemID, listID.ToString());
                    else
                        filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", itemID);
                    
                    List<ItemAlternateUnit> lstDeletedEntity = BusinessLayer.GetItemAlternateUnitList(filterExpression);

                    ItemPlanning iplanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", itemID)).FirstOrDefault();
                    foreach (ItemAlternateUnit deletedEntity in lstDeletedEntity)
                    {
                        if (iplanning.GCPurchaseUnit != deletedEntity.GCAlternateUnit)
                        {
                            deletedEntity.IsDeleted = true;
                            deletedEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(deletedEntity);
                        }
                        else
                        {
                            result = false;
                            errMessage = "Maaf satuan tidak dapat dihapus karena sudah digunakan dalam perencanaan pembelian.";
                            ctx.RollBackTransaction();
                        }
                    }
                    #endregion

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');

                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged)
                        {
                            #region Update
                            if (ID > 0)
                            {
                                ItemAlternateUnit entity = entityDao.Get(ID);
                                entity.GCAlternateUnit = data[2];
                                entity.ConversionFactor = Convert.ToDecimal(data[3]);
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entity);
                            }
                            #endregion
                            #region Insert
                            else
                            {
                                ItemAlternateUnit entity = new ItemAlternateUnit();
                                entity.ItemID = itemID;
                                entity.GCAlternateUnit = data[2];
                                entity.ConversionFactor = Convert.ToDecimal(data[3]);
                                entity.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entity);
                            }
                        }
                            #endregion
                    }
                }
                #endregion
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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