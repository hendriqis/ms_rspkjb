using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InhealthTempatTidurEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.INHEALTH_REFERENCE_TEMPAT_TIDUR;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                //vBedClassReference entity = BusinessLayer.GetvBedClassReference(Convert.ToInt32(ID));
                vBedClassReference entity = BusinessLayer.GetvBedClassReferenceList(string.Format("ID = {0}", ID)).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            //txtNutrientCode.Focus();
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtBedCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInhealthCode, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vBedClassReference entity)
        {
            hdnBedID.Value = entity.BedID.ToString();
            txtBedCode.Text = entity.BedCode;
            txtBedName.Text = entity.BedCode;
            hdnClassID.Value = entity.ClassID.ToString();
            txtClassCode.Text = entity.ClassCode;
            txtClassName.Text = entity.ClassName;
            txtInhealthCode.Text = entity.InhealthReferenceInfo;
            //txtNutrientName.Text = entity.NutrientName;
            //txtNutrientValue.Text = entity.PercentDailyValue.ToString();
            //cboNutrientUnit.Text = entity.GCNutrientUnit;
        }

        private void ControlToEntity(BedClassReference entity)
        {
            entity.BedID = Convert.ToInt32(hdnBedID.Value);
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);
            entity.InhealthReferenceInfo = txtInhealthCode.Text;
            entity.IsDeleted = false;
            //entity.NutrientName = txtNutrientName.Text;
            //entity.PercentDailyValue = Convert.ToDecimal(txtNutrientValue.Text);
            //entity.GCNutrientUnit = cboNutrientUnit.Value.ToString();
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("BedID = {0} AND ClassID = {1}", hdnBedID.Value, hdnClassID.Value);
            List<BedClassReference> lst = BusinessLayer.GetBedClassReferenceList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Bed With Code " + txtBedCode.Text + " and Class with code " + txtClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        //protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        //{
        //    errMessage = string.Empty;
        //    string FilterExpression = string.Format("NutrientCode = '{0}' AND NutrientID != {1}", txtNutrientCode.Text, hdnID.Value);
        //    List<Nutrient> lst = BusinessLayer.GetNutrientList(FilterExpression);

        //    if (lst.Count > 0)
        //        errMessage = " Nutrient With Code " + txtNutrientCode.Text + " is already exist!";

        //    return (errMessage == string.Empty);
        //}

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            BedClassReferenceDao entityDao = new BedClassReferenceDao(ctx);
            bool result = false;
            try
            {
                BedClassReference entity = new BedClassReference();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Today;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetBedClassReferenceMaxID(ctx).ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                BedClassReference entity = BusinessLayer.GetBedClassReference(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBedClassReference(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}