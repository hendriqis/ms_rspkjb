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
    public partial class PatientBirthEntryCtl : BaseEntryPopupCtl
    {
        protected string dateNow = "";
        protected string GetTodayDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        public override void InitializeDataControl(string param)
        {
            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param))[0];
            vPatientBirthRecord entityBirthRecord = BusinessLayer.GetvPatientBirthRecordList(string.Format("MotherMRN = '{0}' AND MotherVisitID = {1} AND IsDeleted = 0", entityRegistration.MRN, entityRegistration.VisitID)).FirstOrDefault();
            vPatient entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityRegistration.MRN)).FirstOrDefault();

            if (entityBirthRecord == null)
            {
                if (entityRegistration.IsParturition == true)
                {
                    #region Data Ibu
                    hdnMotherMRN.Value = Convert.ToString(entityRegistration.MRN);
                    hdnVisitIDMom.Value = Convert.ToString(entityRegistration.VisitID);
                    hdnVisitIDIbu.Value = Convert.ToString(entityRegistration.VisitID);
                    txtNoRegIbu.Text = entityRegistration.RegistrationNo;
                    txtMotherMRN.Text = entityPatient.MedicalNo;
                    cboSalutationMother.Value = entityPatient.GCSalutation;
                    cboTitleMother.Value = entityPatient.GCTitle;
                    txtFirstNameMother.Text = entityPatient.FirstName;
                    txtMiddleNameMother.Text = entityPatient.MiddleName;
                    txtLastNameMother.Text = entityPatient.LastName;
                    cboSuffixMother.Value = entityPatient.GCSuffix;
                    txtDOBIbu.Text = entityPatient.DateOfBirth.ToString("dd-MM-yyyy");
                    txtAgeYearMom.Text = Convert.ToString(entityPatient.AgeInYear);
                    txtAgeMonthMom.Text = Convert.ToString(entityPatient.AgeInMonth);
                    txtAgeDayMom.Text = Convert.ToString(entityPatient.AgeInDay);
                    cboGCIdentityNoType.Value = entityPatient.GCIdentityNoType;
                    txtIdentityNo.Text = entityPatient.SSN;
                    if (entityPatient.GCOccupation != "")
                    {
                        String[] patientJobCode = entityPatient.GCOccupation.Split('^');
                        txtPatientJobCode.Text = patientJobCode[1];
                        txtPatientJobName.Text = entityPatient.Occupation;
                    }

                    txtAddressMother.Text = entityPatient.StreetName;
                    hdnZipCodeMother.Value = entityPatient.ZipCode.ToString();
                    txtZipCodeMother.Text = entityPatient.ZipCode.ToString();
                    txtCountyMother.Text = entityPatient.County;
                    txtDistrictMother.Text = entityPatient.District;
                    txtCityMother.Text = entityPatient.City;
                    if (entityPatient.GCState != "")
                        txtProvinceCodeMother.Text = entityPatient.GCState.Substring(5);
                    else
                        txtProvinceCodeMother.Text = "";
                    txtProvinceNameMother.Text = entityPatient.State;
                    txtTelephoneNoMother.Text = entityPatient.PhoneNo1;
                    #endregion

                    #region Data Bayi
                    hdnBabyMRN.Value = Convert.ToString(entityRegistration.MRN);
                    hdnVisitID.Value = Convert.ToString(entityRegistration.VisitID);
                    hdnVisitIDBayi.Value = Convert.ToString(entityRegistration.VisitID);
                    txtNoRegBayi.Text = entityRegistration.RegistrationNo;
                    txtBabyMRN.Text = entityPatient.MedicalNo;
                    String namaBayi = "BAYI Ny. " + entityPatient.PatientName;
                    txtLastNameBaby.Text = namaBayi;
                    txtPreferredNameBaby.Text = namaBayi;
                    #endregion
                }
            }
            else
            {
                vPatientFamily entityFamily = BusinessLayer.GetvPatientFamilyList(string.Format("MRN = {0} AND GCFamilyRelation = '{1}'", entityBirthRecord.MRN, Constant.FamilyRelation.FATHER))[0];

                #region Data Ibu
                hdnMotherMRN.Value = Convert.ToString(entityRegistration.MRN);
                hdnVisitIDMom.Value = Convert.ToString(entityRegistration.VisitID);
                hdnVisitIDIbu.Value = Convert.ToString(entityRegistration.VisitID);
                txtNoRegIbu.Text = entityRegistration.RegistrationNo;
                txtMotherMRN.Text = entityPatient.MedicalNo;
                cboSalutationMother.Value = entityPatient.GCSalutation;
                cboTitleMother.Value = entityPatient.GCTitle;
                txtFirstNameMother.Text = entityPatient.FirstName;
                txtMiddleNameMother.Text = entityPatient.MiddleName;
                txtLastNameMother.Text = entityPatient.LastName;
                cboSuffixMother.Value = entityPatient.GCSuffix;
                txtDOBIbu.Text = entityPatient.DateOfBirth.ToString("dd-MM-yyyy");
                txtAgeYearMom.Text = Convert.ToString(entityPatient.AgeInYear);
                txtAgeMonthMom.Text = Convert.ToString(entityPatient.AgeInMonth);
                txtAgeDayMom.Text = Convert.ToString(entityPatient.AgeInDay);
                cboGCIdentityNoType.Value = entityPatient.GCIdentityNoType;
                txtIdentityNo.Text = entityPatient.SSN;
                if (entityPatient.GCOccupation != "")
                {
                    String[] patientJobCode = entityPatient.GCOccupation.Split('^');
                    txtPatientJobCode.Text = patientJobCode[1];
                    txtPatientJobName.Text = entityPatient.Occupation;
                }

                txtAddressMother.Text = entityPatient.StreetName;
                hdnZipCodeMother.Value = entityPatient.ZipCode.ToString();
                txtZipCodeMother.Text = entityPatient.ZipCode.ToString();
                txtCountyMother.Text = entityPatient.County;
                txtDistrictMother.Text = entityPatient.District;
                txtCityMother.Text = entityPatient.City;
                if (entityPatient.GCState != "")
                    txtProvinceCodeMother.Text = entityPatient.GCState.Substring(5);
                else
                    txtProvinceCodeMother.Text = "";
                txtProvinceNameMother.Text = entityPatient.State;
                txtTelephoneNoMother.Text = entityPatient.PhoneNo1;
                #endregion

                #region Data Bayi
                hdnBabyMRN.Value = Convert.ToString(entityBirthRecord.MRN);
                hdnVisitID.Value = Convert.ToString(entityBirthRecord.VisitID);
                hdnVisitIDBayi.Value = Convert.ToString(entityBirthRecord.VisitID);
                txtNoRegBayi.Text = entityRegistration.RegistrationNo;
                txtBabyMRN.Text = entityBirthRecord.MedicalNo;
                txtFirstNameBaby.Text = entityBirthRecord.FirstName;
                txtMiddleNameBaby.Text = entityBirthRecord.MiddleName;
                txtLastNameBaby.Text = entityBirthRecord.LastName;
                txtPreferredNameBaby.Text = entityBirthRecord.PreferredName;
                cboGenderBaby.Value = entityBirthRecord.Gender;
                txtDOBBayi.Text = entityBirthRecord.DateOfBirth.ToString("dd-MM-yyyy");

                txtAddressBaby.Text = entityBirthRecord.StreetName;
                hdnZipCodeBaby.Value = entityBirthRecord.ZipCode.ToString();
                txtZipCodeBaby.Text = entityBirthRecord.ZipCode.ToString();
                txtCountyBaby.Text = entityBirthRecord.County;
                txtDistrictBaby.Text = entityBirthRecord.District;
                txtCityBaby.Text = entityBirthRecord.City;
                if (entityBirthRecord.GCState != "")
                    txtProvinceCodeBaby.Text = entityBirthRecord.GCState.Substring(5);
                else
                    txtProvinceCodeBaby.Text = "";
                txtProvinceNameBaby.Text = entityBirthRecord.State;
                txtTelephoneNoBaby.Text = entityBirthRecord.PhoneNo1;
                #endregion

                #region Data Kelahiran Bayi
                txtChildNo.Text = Convert.ToString(entityBirthRecord.ChildNo);
                txtTimeOfBirth.Text = entityBirthRecord.TimeOfBirth;
                cboBornAt.Value = entityBirthRecord.GCBornAt;
                txtBirthPregnancyAge.Text = Convert.ToString(entityBirthRecord.BirthPregnancyAge);
                txtAPGARScore1.Text = Convert.ToString(entityBirthRecord.APGARScore1);
                txtAPGARScore2.Text = Convert.ToString(entityBirthRecord.APGARScore2);
                txtAPGARScore3.Text = Convert.ToString(entityBirthRecord.APGARScore3);
                txtLength.Text = Convert.ToString(entityBirthRecord.Length);
                txtWeight.Text = Convert.ToString(entityBirthRecord.Weight);
                txtHeadCircumference.Text = Convert.ToString(entityBirthRecord.HeadCircumference);
                txtChestCircumference.Text = Convert.ToString(entityBirthRecord.ChestCircumference);
                cboCaesarMethod.Value = entityBirthRecord.GCCaesarMethod;
                cboTwinSingle.Value = entityBirthRecord.TwinSingle;
                cboBornCondition.Value = entityBirthRecord.GCBornCondition;
                cboBirthMethod.Value = entityBirthRecord.GCBirthMethod;
                cboBirthComplication.Value = entityBirthRecord.GCBirthComplicationType;
                cboBirthCOD.Value = entityBirthRecord.GCBirthCOD;
                #endregion

                #region Data Ayah
                txtFatherMRN.Text = Convert.ToString(entityFamily.FamilyMRN);
                cboSalutationFather.Value = entityFamily.GCSalutation;
                cboTitleFather.Value = entityFamily.GCTitle;
                txtFirstNameFather.Text = entityFamily.FirstName;
                txtMiddleNameFather.Text = entityFamily.MiddleName;
                txtLastNameFather.Text = entityFamily.LastName;
                cboSuffixFather.Text = entityFamily.GCSuffix;

                txtAddressFather.Text = entityFamily.StreetName;
                hdnZipCodeFather.Value = entityFamily.ZipCode.ToString();
                txtZipCodeFather.Text = entityFamily.ZipCode.ToString();
                txtCountyFather.Text = entityFamily.County;
                txtDistrictFather.Text = entityFamily.District;
                txtCityFather.Text = entityFamily.City;
                if (entityFamily.GCState != "")
                    txtProvinceCodeFather.Text = entityFamily.GCState.Substring(5);
                else
                    txtProvinceCodeFather.Text = "";
                txtProvinceNameFather.Text = entityFamily.State;
                txtTelephoneNoFather.Text = entityFamily.PhoneNo1;
                #endregion
            }
            SetControlProperties();
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        protected string OnGetSCPatientJobFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.OCCUPATION);
        }
        #endregion

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
            Methods.SetComboBoxField<StandardCode>(cboGenderBaby, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsDeleted = 0", Constant.StandardCode.TITLE, Constant.StandardCode.SALUTATION, Constant.StandardCode.SUFFIX, Constant.StandardCode.IDENTITY_NUMBERY_TYPE, Constant.StandardCode.FAMILY_RELATION));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutationMother, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitleMother, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffixMother, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCIdentityNoType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.IDENTITY_NUMBERY_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboSalutationMother.SelectedIndex = 0;
            cboTitleMother.SelectedIndex = 0;
            cboSuffixMother.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboSalutationFather, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitleFather, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffixFather, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboSalutationFather.SelectedIndex = 0;
            cboTitleFather.SelectedIndex = 0;
            cboSuffixFather.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            #region Data Bayi
            SetControlEntrySetting(txtNoRegBayi, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBabyMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNoSKL, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtDOBBayi, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtFirstNameBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleNameBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLastNameBaby, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPreferredNameBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGenderBaby, new ControlEntrySetting(true, true, true));
            // Alamat Bayi
            SetControlEntrySetting(txtAddressBaby, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtZipCodeBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCountyBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrictBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCityBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCodeBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceNameBaby, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTelephoneNoBaby, new ControlEntrySetting(true, true, true));
            // Data Kelahiran Bayi
            SetControlEntrySetting(txtChildNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTimeOfBirth, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(cboBornAt, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBirthPregnancyAge, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAPGARScore1, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAPGARScore2, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAPGARScore3, new ControlEntrySetting(true, true, true, "0"));
            #endregion

            #region Data Ibu
            SetControlEntrySetting(txtNoRegIbu, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMotherMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDOBIbu, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeYearMom, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeMonthMom, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeDayMom, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboSalutationMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTitleMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFirstNameMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleNameMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLastNameMother, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSuffixMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCIdentityNoType, new ControlEntrySetting(true, true, false));
            // Alamat Ibu
            SetControlEntrySetting(txtAddressMother, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtZipCodeMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCountyMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrictMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCityMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCodeMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceNameMother, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTelephoneNoMother, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtIdentityNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPatientJobCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobName, new ControlEntrySetting(false, false, false));
            #endregion

            #region Data Ayah
            SetControlEntrySetting(chkIsFatherHasMRN, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtFatherMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboSalutationFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTitleFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFirstNameFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleNameFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLastNameFather, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSuffixFather, new ControlEntrySetting(true, true, false));
            // Alamat Ayah
            SetControlEntrySetting(txtAddressFather, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtZipCodeFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCountyFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrictFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCityFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCodeFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceNameFather, new ControlEntrySetting(false, true, false));
            SetControlEntrySetting(txtTelephoneNoFather, new ControlEntrySetting(true, true, true));
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
            txtNoRegBayi.Text = entity.RegistrationNo;
            hdnBabyMRN.Value = entity.MRN.ToString();
            txtBabyMRN.Text = entity.MedicalNo;
            txtNoSKL.Text = entity.ReferenceNo;
            txtDOBBayi.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtFirstNameBaby.Text = entity.FirstName;
            txtMiddleNameBaby.Text = entity.MiddleName;
            txtLastNameBaby.Text = entity.LastName;
            txtPreferredNameBaby.Text = entity.PreferredName;
            cboGenderBaby.Value = entity.Gender;

            txtChildNo.Text = entity.ChildNo.ToString();
            txtTimeOfBirth.Text = entity.TimeOfBirth.ToString();
            cboBornAt.Value = entity.GCBornAt;
            txtBirthPregnancyAge.Text = entity.BirthPregnancyAge.ToString();
            txtAPGARScore1.Text = entity.APGARScore1.ToString();
            txtAPGARScore2.Text = entity.APGARScore2.ToString();
            txtAPGARScore3.Text = entity.APGARScore3.ToString();

            txtAddressBaby.Text = entity.StreetName;
            hdnZipCodeBaby.Value = entity.ZipCode.ToString();
            txtZipCodeBaby.Text = entity.ZipCode.ToString();
            txtCountyBaby.Text = entity.County;
            txtDistrictBaby.Text = entity.District;
            txtCityBaby.Text = entity.City;
            if (entity.GCState != "")
                txtProvinceCodeBaby.Text = entity.GCState.Substring(5);
            else
                txtProvinceCodeBaby.Text = "";
            txtProvinceNameBaby.Text = entity.State;
            txtTelephoneNoBaby.Text = entity.PhoneNo1;

            txtBabyPhoto.Text = entity.PictureFileName;
            imgPreview.Src = entity.PictureFileName;
            #endregion

            #region Data Ibu
            hdnVisitIDIbu.Value = entity.MotherVisitID.ToString();
            txtNoRegIbu.Text = entity.MotherRegistrationNo;
            hdnMotherMRN.Value = entity.MotherMRN.ToString();
            txtMotherMRN.Text = entity.MotherMedicalNo;
            txtDOBIbu.Text = entity.MotherDateOfBirth.ToString("dd-MM-yyyy");
            txtAgeDayMom.Text = entity.AgeMotherInDay.ToString();
            txtAgeMonthMom.Text = entity.AgeMotherInMonth.ToString();
            txtAgeYearMom.Text = entity.AgeMotherInYear.ToString();

            cboSalutationMother.Value = entity.MotherGCSalutation;
            cboTitleMother.Value = entity.MotherGCTitle;
            txtFirstNameMother.Text = entity.MotherFirstName;
            txtMiddleNameMother.Text = entity.MotherMiddleName;
            txtLastNameMother.Text = entity.MotherLastName;
            cboSuffixMother.Value = entity.MotherGCSuffix;
            cboGCIdentityNoType.Value = entity.MotherGCIdentityNoType;
            txtIdentityNo.Text = entity.MotherIdentityNo;
            if (entity.MotherGCOccupation != "")
                txtPatientJobCode.Text = entity.MotherGCOccupation.Substring(5);
            else
                txtPatientJobCode.Text = "";
            txtPatientJobName.Text = entity.MotherOccupationName;
            txtAddressMother.Text = entity.MotherStreetName;
            hdnZipCodeMother.Value = entity.MotherZipCode.ToString();
            txtZipCodeMother.Text = entity.MotherZipCode.ToString();
            txtCountyMother.Text = entity.MotherCounty;
            txtDistrictMother.Text = entity.MotherDistrict;
            txtCityMother.Text = entity.MotherCity;
            if (entity.MotherGCState != "")
                txtProvinceCodeMother.Text = entity.MotherGCState.Substring(5);
            else
                txtProvinceCodeMother.Text = "";
            txtProvinceNameMother.Text = entity.MotherState;
            txtTelephoneNoMother.Text = entity.MotherPhoneNo1;
            #endregion

            #region Data Ayah
            vPatientFamily entityPatientFamilyFather = BusinessLayer.GetvPatientFamilyList(string.Format("MRN = {0} AND GCFamilyRelation = '{1}'", entity.MRN, Constant.FamilyRelation.FATHER)).FirstOrDefault();

            if (entityPatientFamilyFather != null)
            {
                if (entityPatientFamilyFather.FamilyMRN != null)
                {
                    chkIsFatherHasMRN.Checked = true;
                    txtFatherMRN.Text = entityPatientFamilyFather.FamilyMedicalNo;
                }
                else
                {
                    chkIsFatherHasMRN.Checked = false;
                    txtFatherMRN.Text = "";
                }
                cboSalutationFather.Value = entityPatientFamilyFather.GCSalutation;
                cboTitleFather.Value = entityPatientFamilyFather.GCTitle;
                txtFirstNameFather.Text = entityPatientFamilyFather.FirstName;
                txtMiddleNameFather.Text = entityPatientFamilyFather.MiddleName;
                txtLastNameFather.Text = entityPatientFamilyFather.LastName;
                cboSuffixFather.Value = entityPatientFamilyFather.GCSuffix;
                txtAddressFather.Text = entityPatientFamilyFather.StreetName;
                txtZipCodeFather.Text = entityPatientFamilyFather.ZipCode;
                txtCountyFather.Text = entityPatientFamilyFather.County;
                txtDistrictFather.Text = entityPatientFamilyFather.District;
                txtCityFather.Text = entityPatientFamilyFather.City;
                if (entity.GCState != "")
                    txtProvinceCodeFather.Text = entityPatientFamilyFather.GCState.Substring(5);
                else
                    txtProvinceCodeFather.Text = "";
                txtProvinceNameFather.Text = entityPatientFamilyFather.State;
                txtTelephoneNoFather.Text = entityPatientFamilyFather.PhoneNo1;
            }
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

        private void ControlToEntity(Patient entity, Address entityAddress)
        {
            #region Patient Data
            bool flagToUpper = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CAPITALIZE_PATIENT_NAME).ParameterValue == "1";
            entity.FirstName = txtFirstNameBaby.Text;
            entity.MiddleName = txtMiddleNameBaby.Text;
            entity.LastName = txtLastNameBaby.Text;
            entity.PreferredName = txtPreferredNameBaby.Text;
            if (flagToUpper)
            {
                entity.FirstName = entity.FirstName.ToUpper();
                entity.MiddleName = entity.MiddleName.ToUpper();
                entity.LastName = entity.LastName.ToUpper();
                entity.PreferredName = entity.PreferredName.ToUpper();
            }
            entity.GCSex = entity.GCGender = cboGenderBaby.Value.ToString();
            entity.DateOfBirth = Helper.GetDatePickerValue(txtDOBBayi);

            String title = "";
            String suffix = "";

            entity.FullName = Helper.GenerateFullName(Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName), title, suffix);

            #endregion

            #region Patient Address
            entityAddress.StreetName = txtAddressBaby.Text;
            entityAddress.County = txtCountyBaby.Text; // Desa
            entityAddress.District = txtDistrictBaby.Text; //Kabupaten
            entityAddress.City = txtCityBaby.Text;
            entityAddress.GCState = txtProvinceCodeBaby.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCodeBaby.Text);
            if (hdnZipCodeBaby.Value == "" || hdnZipCodeBaby.Value == "0")
            {
                entityAddress.ZipCode = null;
            }
            else
            {
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCodeBaby.Value);
            }
            #endregion
            entityAddress.PhoneNo1 = txtTelephoneNoBaby.Text;
            entity.GCPatientStatus = Constant.PatientStatus.ACTIVE;
        }

        private void ControlToEntity(Patient entity, Address entityAddress, PatientBirthRecord entityBirthRecord)
        {
            #region Data Bayi
            entityBirthRecord.MRN = Convert.ToInt32(hdnBabyMRN.Value);
            entityBirthRecord.VisitID = Convert.ToInt32(hdnVisitIDBayi.Value);
            entityBirthRecord.ReferenceNo = txtBabyMRN.Text;
            entityBirthRecord.ChildNo = Convert.ToInt16(txtChildNo.Text);
            entityBirthRecord.ReferenceNo = txtNoSKL.Text;
            entityBirthRecord.TimeOfBirth = txtTimeOfBirth.Text;
            entityBirthRecord.GCBornAt = cboBornAt.Value.ToString();
            entityBirthRecord.BornAtDescription = cboBornAt.Text;

            entityAddress.StreetName = txtAddressBaby.Text;
            entityAddress.County = txtCountyBaby.Text; // Desa
            entityAddress.District = txtDistrictBaby.Text; //Kabupaten
            entityAddress.City = txtCityBaby.Text;
            entityAddress.GCState = txtProvinceCodeBaby.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCodeBaby.Text);
            if (hdnZipCodeBaby.Value == "" || hdnZipCodeBaby.Value == "0")
            {
                entityAddress.ZipCode = null;
            }
            else
            {
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCodeBaby.Value);
            }
            entityAddress.PhoneNo1 = txtTelephoneNoBaby.Text;

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

            #region Data Ibu
            entityBirthRecord.MotherMRN = Convert.ToInt32(hdnMotherMRN.Value);
            entityBirthRecord.MotherVisitID = Convert.ToInt32(hdnVisitIDIbu.Value);
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

        private bool IsMedicalNoValid(string medicalNo, IDbContext ctx)
        {
            string filterExpression = string.Format("MedicalNo = '{0}'", medicalNo);
            Patient oPatient = BusinessLayer.GetPatientList(filterExpression, ctx).FirstOrDefault();
            return oPatient == null;
        }

        private void UploadItemPhoto(int MRN, string MedicalNo, string PatientName, ref string fileName)
        {
            if (hdnUploadedFile1.Value != "")
            {
                string imageData = hdnUploadedFile1.Value;
                if (imageData != "")
                {
                    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                    imageData = String.Join(",", parts);
                }

                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\", AppConfigManager.QISPatientImagePath.Replace('/', '\\'));

                path = path.Replace("#MRN", string.Format("{0}", MedicalNo));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                fileName = String.Format("{0}.png", MedicalNo);
                FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] data = Convert.FromBase64String(imageData);
                bw.Write(data);
                bw.Close();
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            string filename = "";
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityDao = new PatientDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            PatientBirthRecordDao entityBirthRecordDao = new PatientBirthRecordDao(ctx);
            PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
            PatientDao entityPatientMotherDao = new PatientDao(ctx);
            PatientDao entityPatientFatherDao = new PatientDao(ctx);
            AddressDao entityAddressMotherDao = new AddressDao(ctx);
            AddressDao entityAddressFatherDao = new AddressDao(ctx);
            try
            {
                Patient entity = new Patient();
                Address entityAddress = new Address();
                Address entityOfficeAddress = new Address();
                Address entityDomicileAddress = new Address();
                PatientBirthRecord entityBirthRecord = new PatientBirthRecord();

                List<PatientBirthRecord> lstPatientBirthRecord = BusinessLayer.GetPatientBirthRecordList(string.Format("VisitID = '{0}'", hdnVisitIDBayi.Value), ctx);

                if (lstPatientBirthRecord.Count > 0)
                {
                    //PatientBirthRecord entityBirthRecord = lstPatientBirthRecord.FirstOrDefault();
                    //ControlToEntity(entity, entityBirthRecord);
                    ControlToEntity(entity, entityAddress, entityBirthRecord);
                    entityBirthRecord.BirthRecordID = lstPatientBirthRecord.FirstOrDefault().BirthRecordID;
                    entityBirthRecord.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityBirthRecordDao.Update(entityBirthRecord);
                }
                else
                {
                    ControlToEntity(entity, entityAddress);
                    entity.IsAlive = true;
                    entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    UploadItemPhoto(entity.MRN, entity.MedicalNo, entity.FullName, ref filename);
                    entity.PictureFileName = filename;

                    entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityOfficeAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityDomicileAddress.CreatedBy = AppSession.UserLogin.UserID;

                    entity.HomeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                    //entity.HomeAddressID = BusinessLayer.GetAddressMaxID(ctx);
                    entity.OfficeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                    ////entity.OfficeAddressID = BusinessLayer.GetAddressMaxID(ctx);
                    entity.OtherAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                    ////entity.OtherAddressID = BusinessLayer.GetAddressMaxID(ctx);

                    entity.MRN = entityDao.InsertReturnPrimaryKeyID(entity);
                    string medicalNo = Helper.GenerateMRN(entity.MRN);

                    if (medicalNo != string.Empty)
                    {
                        if (IsMedicalNoValid(medicalNo, ctx))
                        {
                            entity.MedicalNo = medicalNo;
                            entity.PictureFileName = string.Format("{0}.jpg", entity.MedicalNo);
                            entityDao.Update(entity);
                        }
                        else
                        {
                            errMessage = "Terjadi kesalahan ketika proses pembuatan Nomor Rekam Medis (101)";
                            ctx.RollBackTransaction();
                            ctx.Close();
                            return false;
                        }
                    }
                    else
                    {
                        errMessage = "Terjadi kesalahan ketika proses pembuatan Nomor Rekam Medis";
                        ctx.RollBackTransaction();
                        ctx.Close();
                        return false;
                    }

                    hdnBabyMRN.Value = entity.MRN.ToString();

                    ControlToEntity(entity, entityAddress, entityBirthRecord);
                    entityBirthRecord.IsTemporary = true;
                    entityBirthRecord.CreatedBy = AppSession.UserLogin.UserID;
                    entityBirthRecordDao.Insert(entityBirthRecord);

                    #region Data Ibu

                    Patient entityPatientMotherNew = BusinessLayer.GetPatientList(string.Format("MRN = {0}", Convert.ToInt32(hdnMotherMRN.Value)), ctx)[0];
                    PatientFamily entityPatientFamilyMother = new PatientFamily();
                    Address entityAddressMother = null;

                    entityPatientFamilyMother.MRN = Convert.ToInt32(hdnBabyMRN.Value);
                    entityPatientFamilyMother.FamilyMRN = Convert.ToInt32(hdnMotherMRN.Value);

                    if (cboSalutationMother.Value != null)
                    {
                        entityPatientFamilyMother.GCSalutation = cboSalutationMother.Value.ToString();
                        entityPatientMotherNew.GCSalutation = cboSalutationMother.Value.ToString();
                    }
                    else
                    {
                        entityPatientFamilyMother.GCSalutation = "";
                        entityPatientMotherNew.GCSalutation = "";
                    }

                    if (cboTitleMother.Value != null)
                    {
                        entityPatientFamilyMother.GCTitle = cboTitleMother.Value.ToString();
                        entityPatientMotherNew.GCTitle = cboTitleMother.Value.ToString();
                    }
                    else
                    {
                        entityPatientFamilyMother.GCTitle = "";
                        entityPatientMotherNew.GCTitle = "";
                    }

                    if (cboSuffixMother.Value != null)
                    {
                        entityPatientFamilyMother.GCSuffix = cboSuffixMother.Value.ToString();
                        entityPatientMotherNew.GCSuffix = cboSuffixMother.Value.ToString();
                    }
                    else
                    {
                        entityPatientFamilyMother.GCSuffix = "";
                        entityPatientMotherNew.GCSuffix = "";
                    }

                    entityPatientFamilyMother.FirstName = txtFirstNameMother.Text;
                    entityPatientFamilyMother.MiddleName = txtMiddleNameMother.Text;
                    entityPatientFamilyMother.LastName = txtLastNameMother.Text;

                    entityPatientMotherNew.FirstName = txtFirstNameMother.Text;
                    entityPatientMotherNew.MiddleName = txtMiddleNameMother.Text;
                    entityPatientMotherNew.LastName = txtLastNameMother.Text;

                    string suffixMother = cboSuffixMother.Value == null ? "" : cboSuffixMother.Text;
                    string titleMother = cboTitleMother.Value == null ? "" : cboTitleMother.Text;
                    entityPatientFamilyMother.Name = Helper.GenerateName(entityPatientFamilyMother.LastName, entityPatientFamilyMother.MiddleName, entityPatientFamilyMother.FirstName);
                    entityPatientFamilyMother.FullName = Helper.GenerateFullName(entityPatientFamilyMother.Name, titleMother, suffixMother);
                    entityPatientMotherNew.Name = Helper.GenerateName(entityPatientFamilyMother.LastName, entityPatientFamilyMother.MiddleName, entityPatientFamilyMother.FirstName);
                    entityPatientMotherNew.FullName = Helper.GenerateFullName(entityPatientFamilyMother.Name, titleMother, suffixMother);

                    if (hdnAddressIDMother.Value != "" && hdnAddressIDMother.Value != "0")
                    {
                        entityAddressMother = entityAddressMotherDao.Get(Convert.ToInt32(hdnAddressIDMother.Value));
                    }
                    else
                    {
                        entityAddressMother = new Address();
                    }

                    entityAddressMother.StreetName = txtAddressMother.Text;

                    if (txtZipCodeMother.Text != "" && txtZipCodeMother.Text != "0")
                    {
                        entityAddressMother.ZipCode = Convert.ToInt32(txtZipCodeMother.Text);
                    }
                    else
                    {
                        entityAddressMother.ZipCode = null;
                    }

                    entityAddressMother.County = txtCountyMother.Text;
                    entityAddressMother.District = txtDistrictMother.Text;
                    entityAddressMother.City = txtCityMother.Text;
                    entityAddressMother.GCState = txtProvinceCodeMother.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCodeMother.Text);
                    entityAddressMother.PhoneNo1 = txtTelephoneNoMother.Text;

                    if (hdnAddressIDMother.Value == "")
                    {
                        entityAddressMother.CreatedBy = AppSession.UserLogin.UserID;
                        entityAddressMotherDao.Insert(entityAddressMother);
                        entityPatientFamilyMother.AddressID = BusinessLayer.GetAddressMaxID(ctx);
                        entityPatientMotherNew.HomeAddressID = BusinessLayer.GetAddressMaxID(ctx);
                    }
                    else
                    {
                        entityAddressMother.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityAddressMotherDao.Update(entityAddressMother);
                        entityPatientFamilyMother.AddressID = Convert.ToInt32(hdnAddressIDMother.Value);
                        entityPatientMotherNew.HomeAddressID = Convert.ToInt32(hdnAddressIDMother.Value);
                    }

                    entityPatientFamilyMother.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
                    entityPatientFamilyMother.IsPatient = true;
                    entityPatientFamilyMother.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Insert(entityPatientFamilyMother);

                    entityPatientMotherNew.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientMotherNew.SSN = txtIdentityNo.Text;
                    entityPatientMotherNew.GCOccupation = txtPatientJobCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.OCCUPATION, txtPatientJobCode.Text);

                    entityPatientMotherDao.Update(entityPatientMotherNew);

                    #endregion

                    #region Data Ayah

                    Patient entityPatientFatherNew = null;
                    PatientFamily entityPatientFamilyFather = new PatientFamily();
                    Address entityAddressFather = null;

                    entityPatientFamilyFather.MRN = Convert.ToInt32(hdnBabyMRN.Value);
                    if (Convert.ToInt32(hdnFatherMRN.Value) != 0)
                    {
                        entityPatientFatherNew = BusinessLayer.GetPatientList(string.Format("MRN = {0}", Convert.ToInt32(hdnFatherMRN.Value)), ctx)[0];
                        entityPatientFamilyFather.FamilyMRN = Convert.ToInt32(hdnFatherMRN.Value);
                    }
                    else
                    {
                        entityPatientFatherNew = new Patient();
                        entityPatientFamilyFather.FamilyMRN = null;
                    }

                    if (cboSalutationFather.Value != null)
                    {
                        entityPatientFamilyFather.GCSalutation = cboSalutationFather.Value.ToString();
                        entityPatientFatherNew.GCSalutation = cboSalutationFather.Value.ToString();
                    }
                    else
                    {
                        entityPatientFamilyFather.GCSalutation = "";
                        entityPatientFatherNew.GCSalutation = "";
                    }

                    if (cboTitleFather.Value != null)
                    {
                        entityPatientFamilyFather.GCTitle = cboTitleFather.Value.ToString();
                        entityPatientFatherNew.GCTitle = cboTitleFather.Value.ToString();
                    }
                    else
                    {
                        entityPatientFamilyFather.GCTitle = "";
                        entityPatientFatherNew.GCTitle = "";
                    }

                    if (cboSuffixFather.Value != null)
                    {
                        entityPatientFamilyFather.GCSuffix = cboSuffixFather.Value.ToString();
                        entityPatientFatherNew.GCSuffix = cboSuffixFather.Value.ToString();
                    }
                    else
                    {
                        entityPatientFamilyFather.GCSuffix = "";
                        entityPatientFatherNew.GCSuffix = "";
                    }

                    entityPatientFamilyFather.FirstName = txtFirstNameFather.Text;
                    entityPatientFamilyFather.MiddleName = txtMiddleNameFather.Text;
                    entityPatientFamilyFather.LastName = txtLastNameFather.Text;

                    entityPatientFatherNew.FirstName = txtFirstNameFather.Text;
                    entityPatientFatherNew.MiddleName = txtMiddleNameFather.Text;
                    entityPatientFatherNew.LastName = txtLastNameFather.Text;

                    string suffixFather = cboSuffixFather.Value == null ? "" : cboSuffixFather.Text;
                    string titleFather = cboTitleFather.Value == null ? "" : cboTitleFather.Text;
                    entityPatientFamilyFather.Name = Helper.GenerateName(entityPatientFamilyFather.LastName, entityPatientFamilyFather.MiddleName, entityPatientFamilyFather.FirstName);
                    entityPatientFamilyFather.FullName = Helper.GenerateFullName(entityPatientFamilyFather.Name, titleFather, suffixFather);
                    entityPatientFatherNew.Name = Helper.GenerateName(entityPatientFamilyFather.LastName, entityPatientFamilyFather.MiddleName, entityPatientFamilyFather.FirstName);
                    entityPatientFatherNew.FullName = Helper.GenerateFullName(entityPatientFamilyFather.Name, titleFather, suffixFather);

                    if (hdnAddressIDFather.Value != "" && hdnAddressIDFather.Value != "0")
                    {
                        entityAddressFather = entityAddressFatherDao.Get(Convert.ToInt32(hdnAddressIDFather.Value));
                    }
                    else
                    {
                        entityAddressFather = new Address();
                    }

                    entityAddressFather.StreetName = txtAddressFather.Text;

                    if (txtZipCodeFather.Text != "" && txtZipCodeFather.Text != "0")
                    {
                        entityAddressFather.ZipCode = Convert.ToInt32(txtZipCodeFather.Text);
                    }
                    else
                    {
                        entityAddressFather.ZipCode = null;
                    }

                    entityAddressFather.County = txtCountyFather.Text;
                    entityAddressFather.District = txtDistrictFather.Text;
                    entityAddressFather.City = txtCityFather.Text;
                    entityAddressFather.GCState = txtProvinceCodeFather.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCodeFather.Text);
                    entityAddressFather.PhoneNo1 = txtTelephoneNoFather.Text;

                    if (hdnAddressIDFather.Value == "")
                    {
                        entityAddressFather.CreatedBy = AppSession.UserLogin.UserID;
                        entityAddressFatherDao.Insert(entityAddressFather);
                        entityPatientFamilyFather.AddressID = BusinessLayer.GetAddressMaxID(ctx);
                        entityPatientFatherNew.HomeAddressID = BusinessLayer.GetAddressMaxID(ctx);
                    }
                    else
                    {
                        entityAddressFather.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityAddressFatherDao.Update(entityAddressFather);
                        entityPatientFamilyFather.AddressID = Convert.ToInt32(hdnAddressIDFather.Value);
                        entityPatientFatherNew.HomeAddressID = Convert.ToInt32(hdnAddressIDFather.Value);
                    }

                    if (Convert.ToInt32(hdnFatherMRN.Value) != 0)
                    {
                        entityPatientFamilyFather.IsPatient = true;
                        entityPatientFatherNew.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPatientFatherDao.Update(entityPatientFatherNew);
                    }

                    entityPatientFamilyFather.GCFamilyRelation = Constant.FamilyRelation.FATHER;
                    entityPatientFamilyFather.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Insert(entityPatientFamilyFather);

                    #endregion
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
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