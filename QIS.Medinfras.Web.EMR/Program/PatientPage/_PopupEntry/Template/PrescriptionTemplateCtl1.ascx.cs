using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionTemplateCtl1 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            IsAdd = true;
            hdnOrderID.Value = paramInfo[0];
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTemplateText, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionTemplateHdDao entityHdDao = new PrescriptionTemplateHdDao(ctx);
            PrescriptionTemplateDtDao entityDtDao = new PrescriptionTemplateDtDao(ctx);

            try
            {
                PrescriptionTemplateHd entity = new PrescriptionTemplateHd();
                entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                entity.PrescriptionTemplateCode = txtTemplateCode.Text;
                entity.PrescriptionTemplateName = txtTemplateText.Text;
                entity.CreatedBy = AppSession.UserLogin.UserID;

                int headerID = entityHdDao.InsertReturnPrimaryKeyID(entity);

                if (headerID != 0)
                {
                    List<PrescriptionOrderDt> lstDrugInfo = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0 ORDER BY PrescriptionOrderDetailID", hdnOrderID.Value));
                    List<PrescriptionTemplateDt> lstTemplateDt = new List<PrescriptionTemplateDt>();
                    int oldParentID = 0;
                    int parentID = 0;

                    foreach (PrescriptionOrderDt item in lstDrugInfo)
                    {
                        PrescriptionTemplateDt entityDt = new PrescriptionTemplateDt();

                        entityDt.PrescriptionTemplateID = headerID;
                        entityDt.IsRFlag = item.IsRFlag;
                        entityDt.IsCompound = item.IsCompound;

                        if (item.IsCompound)
                        {
                            if (item.IsRFlag)
                            {
                                oldParentID = Convert.ToInt32(item.PrescriptionOrderDetailID);
                                entityDt.ParentID = null;
                            }
                            else
                            {
                                entityDt.ParentID = parentID;
                            }
                        }

                        entityDt.ItemID = item.ItemID;
                        entityDt.GenericName = item.GenericName;
                        entityDt.DrugName = item.DrugName;
                        entityDt.CompoundDrugname = item.CompoundDrugname;
                        entityDt.GCDrugForm = item.GCDrugForm;
                        entityDt.Dose = Convert.ToInt32(item.Dose);
                        entityDt.GCDoseUnit = item.GCDoseUnit;
                        entityDt.Frequency = item.Frequency;
                        entityDt.GCDosingFrequency = item.GCDosingFrequency;
                        entityDt.NumberOfDosage = item.NumberOfDosage;
                        entityDt.NumberOfDosageInString = item.NumberOfDosageInString;
                        entityDt.CompoundQty = item.CompoundQty;
                        entityDt.CompoundQtyInString = item.CompoundQtyInString;
                        entityDt.GCCompoundUnit = item.GCCompoundUnit;
                        entityDt.GCDosingUnit = item.GCDosingUnit;
                        entityDt.DosingDuration = item.DosingDuration;
                        entityDt.SignaID = item.SignaID;
                        entityDt.GCRoute = item.GCRoute;
                        entityDt.GCCoenamRule = item.GCCoenamRule;
                        entityDt.MedicationAdministration = item.MedicationAdministration;
                        entityDt.MedicationPurpose =item.MedicationPurpose;
                        entityDt.DispenseQty = item.DispenseQty;
                        entityDt.TakenQty = item.TakenQty;
                        entityDt.ResultQty = item.ResultQty;
                        entityDt.ChargeQty = item.ChargeQty;
                        entityDt.ConversionFactor = item.ConversionFactor;
                        entityDt.EmbalaceID = item.EmbalaceID;
                        entityDt.EmbalaceQty = item.EmbalaceQty;
                        entityDt.IsUseSweetener = item.IsUseSweetener;
                        entityDt.ExpiredDate = item.ExpiredDate;
                        entityDt.IsAsRequired = item.IsAsRequired;
                        entityDt.IsMorning = item.IsMorning;
                        entityDt.IsNoon = item.IsNoon;
                        entityDt.IsEvening = item.IsEvening;
                        entityDt.IsNight = item.IsNight;
                        entityDt.IsUsingUDD = item.IsUsingUDD;
                        entityDt.IsDeleted = false;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDt.CreatedDate = DateTime.Now;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDt.LastUpdatedDate = DateTime.Now;

                        int id = entityDtDao.InsertReturnPrimaryKeyID(entityDt);
                        if (entityDt.IsRFlag)
                            parentID = id;               
                    }

                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = "Prescription Template with Code " + txtTemplateCode.Text + " is already exist!";
                result = false;
            }
            finally 
            {
                ctx.Close();
            }
            return result;
        }
    }
}