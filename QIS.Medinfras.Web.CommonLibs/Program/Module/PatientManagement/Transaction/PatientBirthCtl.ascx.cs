using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBirthCtl : BaseEntryPopupCtl
    {
        protected string dateNow = "";
        protected string GetTodayDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        public override void InitializeDataControl(string param)
        {
            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param))[0];
            vPatientBirthRecord entityBirthRecord = BusinessLayer.GetvPatientBirthRecordList(string.Format("VisitID = '{0}' AND IsDeleted = 0", entityRegistration.VisitID)).FirstOrDefault();
            vPatient entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityRegistration.MRN)).FirstOrDefault();

            if (entityBirthRecord == null)
            {
                    #region Data Bayi
                    hdnBabyMRN.Value = Convert.ToString(entityRegistration.MRN);
                    hdnVisitID.Value = Convert.ToString(entityRegistration.VisitID);
                    hdnVisitIDBayi.Value = Convert.ToString(entityRegistration.VisitID);

                    #endregion
            }

            SetControlProperties();
        }



        private void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') AND IsActive = 1 AND IsDeleted = 0",
                                        Constant.StandardCode.BORN_CONDITION, Constant.StandardCode.BIRTH_METHOD, Constant.StandardCode.BIRTH_COMPLICATION_TYPE,
                                        Constant.StandardCode.BIRTH_COD, Constant.StandardCode.CAESAR_METHOD, Constant.StandardCode.TWIN_SINGLE, Constant.StandardCode.BORN_AT, Constant.StandardCode.GENDER);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboBornCondition, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BORN_CONDITION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBirthMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBirthComplication, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_COMPLICATION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBirthCOD, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_COD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCaesarMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CAESAR_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTwinSingle, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TWIN_SINGLE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBornAt, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BORN_AT).ToList(), "StandardCodeName", "StandardCodeID");
    
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsDeleted = 0", Constant.StandardCode.TITLE, Constant.StandardCode.SALUTATION, Constant.StandardCode.SUFFIX, Constant.StandardCode.IDENTITY_NUMBERY_TYPE, Constant.StandardCode.FAMILY_RELATION));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

        }

        protected override void OnControlEntrySetting()
        {
            #region Data Bayi
            // Data Kelahiran Bayi
            SetControlEntrySetting(txtChildNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTimeOfBirth, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(cboBornAt, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBirthPregnancyAge, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAPGARScore1, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAPGARScore2, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAPGARScore3, new ControlEntrySetting(true, true, true, "0"));
            #endregion

            #region Data Kuantitatif
            SetControlEntrySetting(txtLength, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtWeight, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtHeadCircumference, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtChestCircumference, new ControlEntrySetting(true, true, false));
            #endregion

            #region Data Kualitatif
            SetControlEntrySetting(cboCaesarMethod, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTwinSingle, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBornCondition, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBirthMethod, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBirthComplication, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBirthCOD, new ControlEntrySetting(true, true, false));
            #endregion
        }

        private void EntityToControl(vPatientBirthRecord entity)
        {
            #region Data Bayi
            hdnBirthRecordID.Value = entity.BirthRecordID.ToString();
            hdnVisitIDBayi.Value = entity.VisitID.ToString();        
            hdnBabyMRN.Value = entity.MRN.ToString();
            txtChildNo.Text = entity.ChildNo.ToString();
            txtTimeOfBirth.Text = entity.TimeOfBirth.ToString();
            cboBornAt.Value = entity.GCBornAt;
            txtBirthPregnancyAge.Text = entity.BirthPregnancyAge.ToString();
            txtAPGARScore1.Text = entity.APGARScore1.ToString();
            txtAPGARScore2.Text = entity.APGARScore2.ToString();
            txtAPGARScore3.Text = entity.APGARScore3.ToString();
            #endregion

            #region Data Kuantitatif
            txtLength.Text = entity.Length.ToString();
            txtWeight.Text = entity.Weight.ToString();
            txtHeadCircumference.Text = entity.HeadCircumference.ToString();
            txtChestCircumference.Text = entity.ChestCircumference.ToString();
            #endregion

            #region Data Kualitatif
            cboCaesarMethod.Value = entity.GCCaesarMethod;
            cboTwinSingle.Value = entity.GCTwinSingle;
            cboBornCondition.Value = entity.GCBornCondition;
            cboBirthMethod.Value = entity.GCBirthMethod;
            cboBirthComplication.Value = entity.GCBirthComplicationType;
            cboBirthCOD.Value = entity.GCBirthCOD;
            #endregion
        }



        private void ControlToEntity(PatientBirthRecord entityBirthRecord)
        {
            #region Data Bayi
            entityBirthRecord.MRN = Convert.ToInt32(hdnBabyMRN.Value);
            entityBirthRecord.VisitID = Convert.ToInt32(hdnVisitIDBayi.Value);

            entityBirthRecord.ChildNo = Convert.ToInt16(txtChildNo.Text);

            entityBirthRecord.TimeOfBirth = txtTimeOfBirth.Text;
            entityBirthRecord.GCBornAt = cboBornAt.Value.ToString();
            entityBirthRecord.BornAtDescription = cboBornAt.Text;

            if (txtBirthPregnancyAge.Text != "")
                entityBirthRecord.BirthPregnancyAge = Convert.ToInt16(txtBirthPregnancyAge.Text);
            else
                entityBirthRecord.BirthPregnancyAge = 0;
            if (txtAPGARScore1.Text != "")
                entityBirthRecord.APGARScore1 = Convert.ToDecimal(txtAPGARScore1.Text);
            else
                entityBirthRecord.APGARScore1 = 0;
            if (txtAPGARScore2.Text != "")
                entityBirthRecord.APGARScore2 = Convert.ToDecimal(txtAPGARScore2.Text);
            else
                entityBirthRecord.APGARScore2 = 0;
            if (txtAPGARScore3.Text != "")
                entityBirthRecord.APGARScore3 = Convert.ToDecimal(txtAPGARScore3.Text);
            else
                entityBirthRecord.APGARScore3 = 0;
            #endregion

            #region Data Kuantitatif
            entityBirthRecord.Length = Convert.ToDecimal(txtLength.Text);
            entityBirthRecord.Weight = Convert.ToDecimal(txtWeight.Text);
            if (txtHeadCircumference.Text != null && txtHeadCircumference.Text != "")
                entityBirthRecord.HeadCircumference = Convert.ToDecimal(txtHeadCircumference.Text);
            else
                entityBirthRecord.HeadCircumference = 0;
            if (txtChestCircumference.Text != null && txtChestCircumference.Text != "")
                entityBirthRecord.ChestCircumference = Convert.ToDecimal(txtChestCircumference.Text);
            else
                entityBirthRecord.ChestCircumference = 0;
            #endregion

            #region Data Kualitatif
            if (cboCaesarMethod.Value != null && cboCaesarMethod.Value.ToString() != "")
                entityBirthRecord.GCCaesarMethod = cboCaesarMethod.Value.ToString();
            else
                entityBirthRecord.GCCaesarMethod = null;
            entityBirthRecord.GCTwinSingle = cboTwinSingle.Value.ToString();
            entityBirthRecord.GCBornCondition = cboBornCondition.Value.ToString();
            entityBirthRecord.GCBirthMethod = cboBirthMethod.Value.ToString();
            entityBirthRecord.GCBirthComplicationType = cboBirthComplication.Value.ToString();
            if (cboBirthCOD.Value != null && cboBirthCOD.Value.ToString() != "")
                entityBirthRecord.GCBirthCOD = cboBirthCOD.Value.ToString();
            else
                entityBirthRecord.GCBirthCOD = null;
            #endregion

        }



        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientBirthRecordDao entityBirthRecordDao = new PatientBirthRecordDao(ctx);

            try
            {
                PatientBirthRecord entityBirthRecord = new PatientBirthRecord();

                    ControlToEntity(entityBirthRecord);
                    entityBirthRecord.IsTemporary = false;
                    entityBirthRecord.CreatedBy = AppSession.UserLogin.UserID;
                    entityBirthRecordDao.Insert(entityBirthRecord);

                
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string url, ref string errMessage)
        {
            return false;
        }
    }
}