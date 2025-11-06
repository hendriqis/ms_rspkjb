using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientBirth : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PATIENT_BIRTH;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected override void InitializeDataControl()
        {
            MPTrx3 master = (MPTrx3)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnURLPictureDirectory.Value = string.Format("{0}Patient/", AppConfigManager.QISVirtualDirectory);

            SetControlProperties();
        }

        protected string OnGetBabyRegistrationNoFilterExpression()
        {
            return string.Format("DepartmentID = '{0}' AND GCRegistrationStatus NOT IN ('{1}','{2}','{3}') AND IsNewPatient = 1",
                    Constant.Facility.INPATIENT, Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
        }

        protected string OnGetMotherRegistrationNoFilterExpression()
        {
            return string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}') AND GCGender = '{4}'",
                    Constant.Facility.INPATIENT, Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.Gender.FEMALE);
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

        protected string OnGetSCPatientGenderFilterExpression()
        {
            return string.Format("GCGender = '{0}' AND IsDeleted = 0", Constant.Gender.MALE);
        }
        #endregion

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}') AND IsActive = 1 AND IsDeleted = 0",
                                                            Constant.StandardCode.BORN_CONDITION, //0
                                                            Constant.StandardCode.BIRTH_METHOD, //1
                                                            Constant.StandardCode.BIRTH_COMPLICATION_TYPE, //2
                                                            Constant.StandardCode.BIRTH_COD, //3
                                                            Constant.StandardCode.CAESAR_METHOD, //4
                                                            Constant.StandardCode.TWIN_SINGLE, //5
                                                            Constant.StandardCode.BORN_AT, //6
                                                            Constant.StandardCode.TITLE, //7
                                                            Constant.StandardCode.SALUTATION, //8
                                                            Constant.StandardCode.SUFFIX, //9
                                                            Constant.StandardCode.IDENTITY_NUMBERY_TYPE, //10
                                                            Constant.StandardCode.FAMILY_RELATION, //11
                                                            Constant.StandardCode.OCCUPATION, //12
                                                            Constant.StandardCode.BIRTH_FROM_HIV_MOTHER, //13
                                                            Constant.StandardCode.BIRTH_FROM_SYPHILIS_MOTHER, //14
                                                            Constant.StandardCode.BIRTH_FROM_HEPATITIS_MOTHER, //15
                                                            Constant.StandardCode.PARTUM_DEATH_TYPE, //16
                                                            Constant.StandardCode.NEONATAL_PERINATAL_DEATH_TYPE //17
                                                        );
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboBornCondition, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BORN_CONDITION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBirthMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBirthComplication, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_COMPLICATION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBirthCOD, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_COD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCaesarMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CAESAR_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTwinSingle, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TWIN_SINGLE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBornAt, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BORN_AT).ToList(), "StandardCodeName", "StandardCodeID");

            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutationBaby, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSalutationMother, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitleMother, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffixMother, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCIdentityNoType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.IDENTITY_NUMBERY_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCIdentityNoTypeFather, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.IDENTITY_NUMBERY_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboOccupationMother, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.OCCUPATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboGCBirthFromHIVMother, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_FROM_HIV_MOTHER || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCBirthFromSyphilisMother, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_FROM_SYPHILIS_MOTHER || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCBirthFromHepatitisMother, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_FROM_HEPATITIS_MOTHER || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPartumDeathType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PARTUM_DEATH_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboNeonatalPerinatalDeathType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.NEONATAL_PERINATAL_DEATH_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboSalutationFather, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitleFather, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffixFather, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboOccupationFather, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.OCCUPATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboSalutationMother.SelectedIndex = 0;
            cboTitleMother.SelectedIndex = 0;
            cboSuffixMother.SelectedIndex = 0;
            cboGCIdentityNoType.SelectedIndex = 0;
            cboOccupationMother.SelectedIndex = 0;

            txtNoRegBayi.Focus();

            cboSalutationFather.SelectedIndex = 0;
            cboTitleFather.SelectedIndex = 0;
            cboSuffixFather.SelectedIndex = 0;
            cboOccupationFather.SelectedIndex = 0;

            txtFatherDOB.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnURLPictureDirectory.Value = string.Format("{0}Patient/", AppConfigManager.QISVirtualDirectory);

        }

        protected override void OnControlEntrySetting()
        {
            #region Data Bayi
            SetControlEntrySetting(txtNoRegBayi, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBabyMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNoSKL, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboSalutationBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGenderBaby, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDOBBayi, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFirstNameBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleNameBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLastNameBaby, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPreferredNameBaby, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBabyPhoto, new ControlEntrySetting(false, false, false));

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
            SetControlEntrySetting(txtBirthNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtChildNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTimeOfBirth, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(cboBornAt, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBirthPregnancyAge, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAPGARScore1, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAPGARScore2, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAPGARScore3, new ControlEntrySetting(true, true, true, "0"));

            SetControlEntrySetting(chkIsNewBornGivenKangarooMethod, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsNewBornGivenEarlyInitiationBreastfeeding, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsNewBornGivenCongenitalHyperthyroidismScreening, new ControlEntrySetting(true, true, false, false));

            //SetControlEntrySetting(txtParamedicCode1, new ControlEntrySetting(true, true, true));
            //SetControlEntrySetting(txtParamedicName1, new ControlEntrySetting(false, false, true));
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

            // Riwayat Penyakit Ibu
            SetControlEntrySetting(cboGCBirthFromHIVMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCBirthFromSyphilisMother, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCBirthFromHepatitisMother, new ControlEntrySetting(true, true, false));

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
            SetControlEntrySetting(cboOccupationMother, new ControlEntrySetting(true, true, false));
            #endregion

            #region Data Ayah
            SetControlEntrySetting(chkIsFatherHasMRN, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtFatherMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboSalutationFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTitleFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFirstNameFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleNameFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLastNameFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboSuffixFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFatherDOB, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboOccupationFather, new ControlEntrySetting(true, true, false));

            // Alamat Ayah
            SetControlEntrySetting(txtAddressFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtZipCodeFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCountyFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrictFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCityFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCodeFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceNameFather, new ControlEntrySetting(false, true, false));
            SetControlEntrySetting(txtTelephoneNoFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCIdentityNoTypeFather, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtIdentityNoFather, new ControlEntrySetting(true, true, false));
            #endregion

            #region Data Kuantitatif
            SetControlEntrySetting(txtLength, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtWeight, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtWeightGram, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtHeadCircumference, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtChestCircumference, new ControlEntrySetting(true, true, false));
            #endregion

            #region Data Kualitatif
            SetControlEntrySetting(cboCaesarMethod, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTwinSingle, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBornCondition, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPartumDeathType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboNeonatalPerinatalDeathType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCaesarMethod, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBirthMethod, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBirthComplication, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBirthCOD, new ControlEntrySetting(true, true, false));
            #endregion
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            string filterExpression = "IsDeleted = 0";
            return BusinessLayer.GetvPatientBirthRecordRowCount(filterExpression);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = "";
            if (!String.IsNullOrEmpty(keyValue))
            {
                filterExpression = string.Format("BirthRecordID = {0}", keyValue);
            }

            vPatientBirthRecord entity = BusinessLayer.GetvPatientBirthRecordList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = "IsDeleted = 0";
            vPatientBirthRecord entity = BusinessLayer.GetvPatientBirthRecord(filterExpression, PageIndex, "RegistrationNo ASC");
            EntityToControl(entity);
        }

        private void EntityToControl(vPatientBirthRecord entity)
        {
            Address entityAddressNew = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", entity.MotherAddressID)).FirstOrDefault();
            ZipCodes entityZipCodesNew = null;
            if (entityAddressNew.ZipCode != null)
            {
                entityZipCodesNew = BusinessLayer.GetZipCodesList(string.Format("ID = {0}", entityAddressNew.ZipCode)).FirstOrDefault();
            }

            #region Data Bayi
            hdnBirthRecordID.Value = entity.BirthRecordID.ToString();
            hdnVisitIDBayi.Value = entity.VisitID.ToString();
            txtNoRegBayi.Text = entity.RegistrationNo;
            hdnBabyMRN.Value = entity.MRN.ToString();
            txtBabyMRN.Text = entity.MedicalNo;
            txtNoSKL.Text = entity.ReferenceNo;
            txtDOBBayi.Text = entity.DateOfBirth.ToString("dd-MM-yyyy");
            //txtAgeDayBaby.Text = entity.AgeInDay.ToString();
            //txtAgeMonthBaby.Text = entity.AgeInMonth.ToString();
            //txtAgeYearBaby.Text = entity.AgeInYear.ToString();
            cboSalutationBaby.Value = entity.GCSalutation;
            txtFirstNameBaby.Text = entity.FirstName;
            txtMiddleNameBaby.Text = entity.MiddleName;
            txtLastNameBaby.Text = entity.LastName;
            txtPreferredNameBaby.Text = entity.PreferredName;
            txtGenderBaby.Text = entity.Gender;

            hdnParamedicID1.Value = entity.ParamedicID1.ToString();
            txtParamedicCode1.Text = entity.ParamedicCode1;
            txtParamedicName1.Text = entity.ParamedicName1;

            hdnParamedicID2.Value = entity.ParamedicID2.ToString();
            txtParamedicCode2.Text = entity.ParamedicCode2;
            txtParamedicName2.Text = entity.ParamedicName2;

            hdnParamedicID3.Value = entity.ParamedicID3.ToString();
            txtParamedicCode3.Text = entity.ParamedicCode3;
            txtParamedicName3.Text = entity.ParamedicName3;

            txtBirthNo.Text = entity.BirthNo.ToString();
            txtChildNo.Text = entity.ChildNo.ToString();
            txtTimeOfBirth.Text = entity.TimeOfBirth.ToString();
            cboBornAt.Value = entity.GCBornAt;
            txtBirthPregnancyAge.Text = entity.BirthPregnancyAge.ToString();
            txtAPGARScore1.Text = entity.APGARScore1.ToString();
            txtAPGARScore2.Text = entity.APGARScore2.ToString();
            txtAPGARScore3.Text = entity.APGARScore3.ToString();

            chkIsNewBornGivenKangarooMethod.Checked = entity.IsNewBornGivenKangarooMethod;
            chkIsNewBornGivenEarlyInitiationBreastfeeding.Checked = entity.IsNewBornGivenEarlyInitiationBreastfeeding;
            chkIsNewBornGivenCongenitalHyperthyroidismScreening.Checked = entity.IsNewBornGivenCongenitalHyperthyroidismScreening;

            hdnAddressIDBaby.Value = entity.BabyAddressID.ToString();
            txtAddressBaby.Text = entity.StreetName;
            if (entityZipCodesNew != null)
            {
                hdnZipCodeBaby.Value = entityZipCodesNew.ID.ToString();
                txtZipCodeBaby.Text = entityZipCodesNew.ZipCode.ToString();
            }
            else
            {
                hdnZipCodeBaby.Value = null;
                txtZipCodeBaby.Text = null;
            }
            txtCountyBaby.Text = entity.County;
            txtDistrictBaby.Text = entity.District;
            txtCityBaby.Text = entity.City;
            if (entity.GCState != "")
                txtProvinceCodeBaby.Text = entity.GCState.Substring(5);
            else
                txtProvinceCodeBaby.Text = "";
            txtProvinceNameBaby.Text = entity.State;
            txtTelephoneNoBaby.Text = entity.PhoneNo1;

            #region UploadFoto
            txtBabyPhoto.Text = entity.PictureFileName;
            string ItemImagePath = string.Format("Patient/{0}/", entity.MedicalNo);
            imgPreview.Src = string.Format("{0}{1}{2}.png", AppConfigManager.QISVirtualDirectory, ItemImagePath, entity.MedicalNo);
            #endregion UploadFoto

            //#region Item Photo
            //txtFileName.Text = entity.PictureFileName;
            //string ItemImagePath = string.Format("ItemMaster/{0}/", entity.ItemCode);
            //imgPreview.Src = string.Format("{0}{1}Item{2}.png", AppConfigManager.QISVirtualDirectory, ItemImagePath, entity.ItemCode);
            //#endregion

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
            cboOccupationMother.Value = entity.MotherGCOccupation;

            cboGCBirthFromHIVMother.Value = entity.GCBirthFromHIVMother;
            cboGCBirthFromSyphilisMother.Value = entity.GCBirthFromSyphilisMother;
            cboGCBirthFromHepatitisMother.Value = entity.GCBirthFromHepatitisMother;

            hdnAddressIDMother.Value = entity.MotherAddressID.ToString();
            txtAddressMother.Text = entity.MotherStreetName;
            if (entityZipCodesNew != null)
            {
                hdnZipCodeMother.Value = entityZipCodesNew.ID.ToString();
                txtZipCodeMother.Text = entityZipCodesNew.ZipCode.ToString();
            }
            else
            {
                hdnZipCodeMother.Value = null;
                txtZipCodeMother.Text = null;
            }
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
            vPatientFamily entityPatientFamilyFather = BusinessLayer.GetvPatientFamilyList(string.Format("MRN = {0} AND GCFamilyRelation = '{1}' AND IsDeleted = 0", entity.MRN, Constant.FamilyRelation.FATHER)).FirstOrDefault();
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

                cboGCIdentityNoTypeFather.Value = entityPatientFamilyFather.GCIdentityNoType;
                txtIdentityNoFather.Text = entityPatientFamilyFather.IdentityNo;
                cboSalutationFather.Value = entityPatientFamilyFather.GCSalutation;
                cboTitleFather.Value = entityPatientFamilyFather.GCTitle;
                txtFirstNameFather.Text = entityPatientFamilyFather.FirstName;
                txtMiddleNameFather.Text = entityPatientFamilyFather.MiddleName;
                txtLastNameFather.Text = entityPatientFamilyFather.LastName;
                cboSuffixFather.Value = entityPatientFamilyFather.GCSuffix;
                txtFatherDOB.Text = entityPatientFamilyFather.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                cboOccupationFather.Value = entityPatientFamilyFather.GCOccupation;
                hdnAddressIDFather.Value = entityPatientFamilyFather.AddressID.ToString();
                txtAddressFather.Text = entityPatientFamilyFather.StreetName;
                txtZipCodeFather.Text = entityPatientFamilyFather.ZipCode;
                txtCountyFather.Text = entityPatientFamilyFather.County;
                txtDistrictFather.Text = entityPatientFamilyFather.District;
                txtCityFather.Text = entityPatientFamilyFather.City;
                if (entityPatientFamilyFather.GCState != "")
                    txtProvinceCodeFather.Text = entityPatientFamilyFather.GCState.Substring(5);
                else
                    txtProvinceCodeFather.Text = "";
                txtProvinceNameFather.Text = entityPatientFamilyFather.State;
                txtTelephoneNoFather.Text = entityPatientFamilyFather.PhoneNo1;
            }
            #endregion

            #region Data Kuantitatif
            decimal weightGram = System.Math.Round(entity.WeightGram, 0, MidpointRounding.ToEven);
            txtLength.Text = entity.Length.ToString();
            txtWeight.Text = entity.Weight.ToString();
            txtWeightGram.Text = weightGram.ToString();
            txtHeadCircumference.Text = entity.HeadCircumference.ToString();
            txtChestCircumference.Text = entity.ChestCircumference.ToString();
            txtHerediateryDefectRemarks.Text = entity.HerediateryDefectRemarks;
            #endregion

            #region Data Kualitatif
            cboCaesarMethod.Value = entity.GCCaesarMethod;
            cboTwinSingle.Value = entity.GCTwinSingle;
            cboBornCondition.Value = entity.GCBornCondition;
            cboPartumDeathType.Value = entity.GCPartumDeathType;
            cboNeonatalPerinatalDeathType.Value = entity.GCNeonatalPerinatalDeathType;
            cboBirthMethod.Value = entity.GCBirthMethod;
            cboBirthComplication.Value = entity.GCBirthComplicationType;
            cboBirthCOD.Value = entity.GCBirthCOD;
            #endregion
        }

        #endregion

        #region Save Entity

        protected string OnGetRegistrationNoFilterExpression()
        {
            string filterExpression = "";
            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                    filterExpression = string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                else
                    filterExpression = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2})", hdnDepartmentID.Value, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
            }
            else
                filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
            return filterExpression;
        }

        private void ControlToEntity(PatientBirthRecord entity)
        {
            #region Data Bayi
            entity.MRN = Convert.ToInt32(hdnBabyMRN.Value);
            entity.VisitID = Convert.ToInt32(hdnVisitIDBayi.Value);
            entity.BirthNo = Convert.ToInt16(txtBirthNo.Text);
            entity.ChildNo = Convert.ToInt16(txtChildNo.Text);
            entity.ReferenceNo = txtNoSKL.Text;
            entity.TimeOfBirth = txtTimeOfBirth.Text;
            entity.GCBornAt = cboBornAt.Value.ToString();
            entity.BornAtDescription = cboBornAt.Text;
            if (txtBirthPregnancyAge.Text != "")
                entity.BirthPregnancyAge = Convert.ToInt16(txtBirthPregnancyAge.Text);
            else
                entity.BirthPregnancyAge = 0;
            if (txtAPGARScore1.Text != "")
                entity.APGARScore1 = Convert.ToDecimal(txtAPGARScore1.Text);
            else
                entity.APGARScore1 = 0;
            if (txtAPGARScore2.Text != "")
                entity.APGARScore2 = Convert.ToDecimal(txtAPGARScore2.Text);
            else
                entity.APGARScore2 = 0;
            if (txtAPGARScore3.Text != "")
                entity.APGARScore3 = Convert.ToDecimal(txtAPGARScore3.Text);
            else
                entity.APGARScore3 = 0;

            entity.IsNewBornGivenKangarooMethod = chkIsNewBornGivenKangarooMethod.Checked;
            entity.IsNewBornGivenEarlyInitiationBreastfeeding = chkIsNewBornGivenEarlyInitiationBreastfeeding.Checked;
            entity.IsNewBornGivenCongenitalHyperthyroidismScreening = chkIsNewBornGivenCongenitalHyperthyroidismScreening.Checked;

            if (hdnParamedicID1.Value != "0" && hdnParamedicID1.Value != null && hdnParamedicID1.Value != "")
                entity.ParamedicID1 = Convert.ToInt32(hdnParamedicID1.Value);
            else
                entity.ParamedicID1 = null;

            if (hdnParamedicID2.Value != "0" && hdnParamedicID2.Value != null && hdnParamedicID2.Value != "")
                entity.ParamedicID2 = Convert.ToInt32(hdnParamedicID2.Value);
            else
                entity.ParamedicID2 = null;

            if (hdnParamedicID3.Value != "0" && hdnParamedicID3.Value != null && hdnParamedicID3.Value != "")
                entity.ParamedicID3 = Convert.ToInt32(hdnParamedicID3.Value);
            else
                entity.ParamedicID3 = null;
            #endregion

            #region Data Ibu

            entity.MotherMRN = Convert.ToInt32(hdnMotherMRN.Value);
            entity.MotherVisitID = Convert.ToInt32(hdnVisitIDIbu.Value);

            if (cboGCBirthFromHIVMother.Value != null)
            {
                if (cboGCBirthFromHIVMother.Value.ToString() != "")
                {
                    entity.GCBirthFromHIVMother = cboGCBirthFromHIVMother.Value.ToString();
                }
                else
                {
                    entity.GCBirthFromHIVMother = null;
                }
            }
            else
            {
                entity.GCBirthFromHIVMother = null;
            }

            if (cboGCBirthFromSyphilisMother.Value != null)
            {
                if (cboGCBirthFromSyphilisMother.Value.ToString() != "")
                {
                    entity.GCBirthFromSyphilisMother = cboGCBirthFromSyphilisMother.Value.ToString();
                }
                else
                {
                    entity.GCBirthFromSyphilisMother = null;
                }
            }
            else
            {
                entity.GCBirthFromSyphilisMother = null;
            }

            if (cboGCBirthFromHepatitisMother.Value != null)
            {
                if (cboGCBirthFromHepatitisMother.Value.ToString() != "")
                {
                    entity.GCBirthFromHepatitisMother = cboGCBirthFromHepatitisMother.Value.ToString();
                }
                else
                {
                    entity.GCBirthFromHepatitisMother = null;
                }
            }
            else
            {
                entity.GCBirthFromHepatitisMother = null;
            }

            #endregion

            #region Data Kuantitatif
            entity.Length = Convert.ToDecimal(txtLength.Text);
            entity.Weight = Convert.ToDecimal(txtWeight.Text);
            if (txtHeadCircumference.Text != null && txtHeadCircumference.Text != "")
                entity.HeadCircumference = Convert.ToDecimal(txtHeadCircumference.Text);
            else
                entity.HeadCircumference = 0;
            if (txtChestCircumference.Text != null && txtChestCircumference.Text != "")
                entity.ChestCircumference = Convert.ToDecimal(txtChestCircumference.Text);
            else
                entity.ChestCircumference = 0;
            entity.HerediateryDefectRemarks = txtHerediateryDefectRemarks.Text;
            #endregion

            #region Data Kualitatif
            if (cboCaesarMethod.Value != null && cboCaesarMethod.Value.ToString() != "")
                entity.GCCaesarMethod = cboCaesarMethod.Value.ToString();
            else
                entity.GCCaesarMethod = null;
            entity.GCTwinSingle = cboTwinSingle.Value.ToString();
            entity.GCBornCondition = cboBornCondition.Value.ToString();
            entity.GCBirthMethod = cboBirthMethod.Value.ToString();
            entity.GCBirthComplicationType = cboBirthComplication.Value.ToString();
            if (cboBirthCOD.Value != null && cboBirthCOD.Value.ToString() != "")
                entity.GCBirthCOD = cboBirthCOD.Value.ToString();
            else
                entity.GCBirthCOD = null;

            if (cboPartumDeathType.Value != null)
            {
                if (cboPartumDeathType.Value.ToString() != "")
                {
                    entity.GCPartumDeathType = cboPartumDeathType.Value.ToString();
                }
                else
                {
                    entity.GCPartumDeathType = null;
                }
            }
            else
            {
                entity.GCPartumDeathType = null;
            }

            if (cboNeonatalPerinatalDeathType.Value != null)
            {
                if (cboNeonatalPerinatalDeathType.Value.ToString() != "")
                {
                    entity.GCNeonatalPerinatalDeathType = cboNeonatalPerinatalDeathType.Value.ToString();
                }
                else
                {
                    entity.GCNeonatalPerinatalDeathType = null;
                }
            }
            else
            {
                entity.GCNeonatalPerinatalDeathType = null;
            }

            #endregion
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
            PatientBirthRecordDao entityDao = new PatientBirthRecordDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
            PatientDao entityPatientBabyDao = new PatientDao(ctx);
            PatientDao entityPatientMotherDao = new PatientDao(ctx);
            PatientDao entityPatientFatherDao = new PatientDao(ctx);
            AddressDao entityAddressBabyDao = new AddressDao(ctx);
            AddressDao entityAddressMotherDao = new AddressDao(ctx);
            AddressDao entityAddressFatherDao = new AddressDao(ctx);
            AntenatalDao entityAntenatalDao = new AntenatalDao(ctx);
            ObstetricHistoryDao entityObstetricHistoryDao = new ObstetricHistoryDao(ctx);

            try
            {
                List<PatientBirthRecord> lstPatientBirthRecord = BusinessLayer.GetPatientBirthRecordList(string.Format("VisitID = '{0}'", hdnVisitIDBayi.Value), ctx);
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("MRN IN ({0},{1})", hdnMotherMRN.Value, hdnBabyMRN.Value), ctx);
                Registration MotherRegistration = lstRegistration.Where(x => x.MRN == Convert.ToInt32(hdnMotherMRN.Value)).FirstOrDefault();
                Registration BabyRegistration = lstRegistration.Where(x => x.MRN == Convert.ToInt32(hdnBabyMRN.Value)).FirstOrDefault();
                PatientBirthRecord entity = null;
                if (lstPatientBirthRecord.Count > 0)
                {
                    entity = lstPatientBirthRecord.FirstOrDefault();
                }
                else
                {
                    entity = new PatientBirthRecord();
                }
                ControlToEntity(entity);
                if (lstPatientBirthRecord.Count > 0)
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.IsTemporary = false;
                    entityDao.Update(entity);

                    #region update Partus And New Born
                    if (!BabyRegistration.IsNewBorn)
                    {
                        BabyRegistration.IsNewBorn = true;
                        entityRegistrationDao.Update(BabyRegistration);
                    }

                    if (!MotherRegistration.IsParturition)
                    {
                        MotherRegistration.IsParturition = true;
                        entityRegistrationDao.Update(MotherRegistration);
                    }
                    #endregion

                    retval = entity.BirthRecordID.ToString();
                }
                else
                {
                    entity.IsTemporary = true;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    #region update Partus And New Born
                    if (!BabyRegistration.IsNewBorn)
                    {
                        BabyRegistration.IsNewBorn = true;
                        entityRegistrationDao.Update(BabyRegistration);
                    }

                    if (!MotherRegistration.IsParturition)
                    {
                        MotherRegistration.IsParturition = true;
                        entityRegistrationDao.Update(MotherRegistration);
                    }
                    #endregion

                    retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

                }

                #region Data Bayi

                Patient entityPatientBabyNew = BusinessLayer.GetPatientList(string.Format("MRN = {0}", Convert.ToInt32(hdnBabyMRN.Value)), ctx)[0];
                Address entityAddressBaby = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", entityPatientBabyNew.HomeAddressID), ctx)[0];

                if (cboSalutationBaby.Value != null)
                {
                    entityPatientBabyNew.GCSalutation = cboSalutationBaby.Value.ToString();
                }
                else
                {
                    entityPatientBabyNew.GCSalutation = null;
                }
                entityPatientBabyNew.FirstName = txtFirstNameBaby.Text;
                entityPatientBabyNew.MiddleName = txtMiddleNameBaby.Text;
                entityPatientBabyNew.LastName = txtLastNameBaby.Text;

                entityPatientBabyNew.Name = Helper.GenerateName(entityPatientBabyNew.LastName, entityPatientBabyNew.MiddleName, entityPatientBabyNew.FirstName);
                entityPatientBabyNew.FullName = Helper.GenerateFullName(entityPatientBabyNew.Name, "", "");
                entityPatientBabyNew.PreferredName = txtPreferredNameBaby.Text;

                if (hdnAddressIDBaby.Value != "" && hdnAddressIDBaby.Value != "0")
                {
                    entityAddressBaby = entityAddressBabyDao.Get(Convert.ToInt32(hdnAddressIDBaby.Value));
                }
                else
                {
                    entityAddressBaby = new Address();
                }

                entityAddressBaby.StreetName = txtAddressBaby.Text;

                if (txtZipCodeBaby.Text != "" && txtZipCodeBaby.Text != "0")
                {
                    entityAddressBaby.ZipCode = Convert.ToInt32(hdnZipCodeBaby.Value);
                }
                else
                {
                    entityAddressBaby.ZipCode = null;
                }

                entityAddressBaby.County = txtCountyBaby.Text;
                entityAddressBaby.District = txtDistrictBaby.Text;
                entityAddressBaby.City = txtCityBaby.Text;
                entityAddressBaby.GCState = txtProvinceCodeBaby.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCodeBaby.Text);
                entityAddressBaby.PhoneNo1 = txtTelephoneNoBaby.Text;

                if (hdnAddressIDBaby.Value == "")
                {
                    entityAddressBaby.CreatedBy = AppSession.UserLogin.UserID;
                    entityAddressBabyDao.Insert(entityAddressBaby);
                    entityPatientBabyNew.HomeAddressID = BusinessLayer.GetAddressMaxID(ctx);
                }
                else
                {
                    entityAddressBaby.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressBabyDao.Update(entityAddressBaby);
                    entityPatientBabyNew.HomeAddressID = Convert.ToInt32(hdnAddressIDBaby.Value);
                }

                UploadItemPhoto(entityPatientBabyNew.MRN, entityPatientBabyNew.MedicalNo, entityPatientBabyNew.FullName, ref filename);
                if (!String.IsNullOrEmpty(filename))
                {
                    entityPatientBabyNew.PictureFileName = filename;
                }
                else
                {
                    entityPatientBabyNew.PictureFileName = string.Format("{0}.jpg", entityPatientBabyNew.MedicalNo);
                }

                #endregion

                #region Data Ibu

                List<PatientFamily> lstPatientMother = BusinessLayer.GetPatientFamilyList(string.Format(" MRN = '{0}' AND GCFamilyRelation = '{1}'", hdnBabyMRN.Value, Constant.FamilyRelation.MOTHER), ctx);
                Patient entityPatientMotherNew = BusinessLayer.GetPatientList(string.Format("MRN = {0}", Convert.ToInt32(hdnMotherMRN.Value)), ctx)[0];
                PatientFamily entityPatientFamilyMother = null;
                Address entityAddressMother = null;

                if (lstPatientMother.Count > 0)
                {
                    entityPatientFamilyMother = lstPatientMother.FirstOrDefault();
                }
                else
                {
                    entityPatientFamilyMother = new PatientFamily();
                }

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
                    entityAddressMother.ZipCode = Convert.ToInt32(hdnZipCodeMother.Value);
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

                entityPatientMotherNew.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPatientMotherNew.SSN = txtIdentityNo.Text;

                if (cboOccupationMother.Value != null)
                {
                    entityPatientMotherNew.GCOccupation = cboOccupationMother.Value.ToString();
                    entityPatientFamilyMother.GCOccupation = cboOccupationMother.Value.ToString();
                }
                else
                {
                    entityPatientMotherNew.GCOccupation = null;
                    entityPatientFamilyMother.GCOccupation = null;
                }

                if (lstPatientMother.Count > 0)
                {
                    entityPatientFamilyMother.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
                    entityPatientFamilyMother.IsPatient = true;
                    entityPatientFamilyMother.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Update(entityPatientFamilyMother);
                }
                else
                {
                    entityPatientFamilyMother.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
                    entityPatientFamilyMother.IsPatient = true;
                    entityPatientFamilyMother.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Insert(entityPatientFamilyMother);
                }

                entityPatientMotherNew.IsPregnant = false;
                entityPatientMotherDao.Update(entityPatientMotherNew);

                #endregion

                #region Data Ayah

                List<PatientFamily> lstPatientFather = BusinessLayer.GetPatientFamilyList(string.Format(" MRN = '{0}' AND GCFamilyRelation = '{1}' AND IsDeleted = 0", hdnBabyMRN.Value, Constant.FamilyRelation.FATHER), ctx);
                Patient entityPatientFatherNew = null;
                PatientFamily entityPatientFamilyFather = null;
                Address entityAddressFather = null;

                if (lstPatientFather.Count > 0)
                {
                    entityPatientFamilyFather = lstPatientFather.FirstOrDefault();
                }
                else
                {
                    entityPatientFamilyFather = new PatientFamily();
                }

                entityPatientFamilyFather.MRN = Convert.ToInt32(hdnBabyMRN.Value);
                if (Convert.ToInt32(hdnFatherMRN.Value) != 0)
                {
                    entityPatientFatherNew = BusinessLayer.GetPatientList(string.Format("MRN = '{0}'", Convert.ToInt32(hdnFatherMRN.Value)), ctx)[0];
                    entityPatientFamilyFather.FamilyMRN = Convert.ToInt32(hdnFatherMRN.Value);
                }
                else
                {
                    entityPatientFatherNew = new Patient();
                    entityPatientFamilyFather.FamilyMRN = null;
                    if (cboGCIdentityNoTypeFather.Value != null)
                    {
                        entityPatientFamilyFather.GCIdentityNoType = cboGCIdentityNoTypeFather.Value.ToString();
                    }
                    entityPatientFamilyFather.SSN = txtIdentityNoFather.Text;
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

                if (txtFatherDOB.Text != "" && Helper.GetDatePickerValue(txtFatherDOB).ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    entityPatientFamilyFather.DateOfBirth = Helper.GetDatePickerValue(txtFatherDOB.Text);
                    entityPatientFatherNew.DateOfBirth = Helper.GetDatePickerValue(txtFatherDOB.Text);
                }
                else
                {
                    entityPatientFamilyFather.DateOfBirth = null;
                }

                if (cboOccupationFather.Value != null)
                {
                    entityPatientFamilyFather.GCOccupation = cboOccupationFather.Value.ToString();
                    entityPatientFatherNew.GCOccupation = cboOccupationFather.Value.ToString();
                }
                else
                {
                    entityPatientFamilyFather.GCOccupation = null;
                    entityPatientFatherNew.GCOccupation = null;
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
                    entityPatientFamilyFather.AddressID = entityPatientFatherNew.HomeAddressID = entityAddressFatherDao.InsertReturnPrimaryKeyID(entityAddressFather);
                    //entityPatientFamilyFather.AddressID = BusinessLayer.GetAddressMaxID(ctx);
                    //entityPatientFatherNew.HomeAddressID = BusinessLayer.GetAddressMaxID(ctx);
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

                if (lstPatientFather.Count > 0)
                {
                    entityPatientFamilyFather.GCFamilyRelation = Constant.FamilyRelation.FATHER;
                    entityPatientFamilyFather.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Update(entityPatientFamilyFather);
                }
                else
                {
                    entityPatientFamilyFather.GCFamilyRelation = Constant.FamilyRelation.FATHER;
                    entityPatientFamilyFather.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Insert(entityPatientFamilyFather);
                }

                #endregion

                #region Update Data Patient Baby

                entityPatientBabyNew.MotherName = Helper.GenerateFullName(entityPatientFamilyMother.Name, titleMother, suffixMother);
                entityPatientBabyNew.FatherName = Helper.GenerateFullName(entityPatientFamilyFather.Name, titleFather, suffixFather);
                entityPatientBabyNew.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPatientBabyDao.Update(entityPatientBabyNew);

                #endregion

                #region Update Antenatal
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                string filterAntenatal = string.Format("MRN = '{0}' AND IsDeleted = 0 AND IsBorn = 0", entity.MotherMRN);
                Antenatal antenatal = BusinessLayer.GetAntenatalList(filterAntenatal, ctx).FirstOrDefault();
                if (antenatal != null)
                {                   
                    antenatal.DeliveryVisitID = entity.MotherVisitID;
                    antenatal.FinalEDB = entityPatientBabyNew.DateOfBirth;
                    antenatal.IsBorn = true;
                    antenatal.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAntenatalDao.Update(antenatal);
                }
                #endregion

                #region Insert or Update Obstetric History
                string filterHistory = string.Format("MRN = {0} AND RegistrationID = {1} AND IsDeleted = 0", entity.MotherMRN, MotherRegistration.RegistrationID);
                ObstetricHistory oHistory = BusinessLayer.GetObstetricHistoryList(filterHistory, ctx).FirstOrDefault();

                bool isNewRecord = false;

                if (oHistory == null)
                {
                    oHistory = new ObstetricHistory();
                    isNewRecord = true;
                }                

                oHistory.MRN = Convert.ToInt32(entity.MotherMRN);
                oHistory.RegistrationID = MotherRegistration.RegistrationID;
                oHistory.PregnancyNo = antenatal != null ? antenatal.PregnancyNo : entity.ChildNo;
                oHistory.ChildMRN = entity.MRN;
                oHistory.GCSex = entityPatientBabyNew.GCSex;
                oHistory.DateOfBirth = entityPatientBabyNew.DateOfBirth;
                oHistory.PregnancyDuration = entity.BirthPregnancyAge;
                
                if (hdnParamedicID1.Value != "0" && hdnParamedicID1.Value != null && hdnParamedicID1.Value != "")
                    oHistory.GCParamedicType = Constant.ParamedicType.Physician;
                else if (hdnParamedicID3.Value != "0" && hdnParamedicID3.Value != null && hdnParamedicID3.Value != "")
                    oHistory.GCParamedicType = Constant.ParamedicType.Bidan;

                oHistory.GCBirthMethod = entity.GCBirthMethod;
                oHistory.GCCaesarMethod = entity.GCCaesarMethod;
                oHistory.GCBornCondition = entity.GCBornCondition;
                oHistory.Length = entity.Length;
                oHistory.Weight = entity.Weight*1000;
                oHistory.APGARScore1 = entity.APGARScore1;
                oHistory.APGARScore2 = entity.APGARScore2;
                oHistory.APGARScore3 = entity.APGARScore3;
                oHistory.HeadCircumference = entity.HeadCircumference;
                oHistory.Remarks = entity.Remarks;

                if (isNewRecord)
                {
                    entityObstetricHistoryDao.Insert(oHistory);
                }
                else
                {
                    entityObstetricHistoryDao.Update(oHistory);
                }
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
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            string filename = "";
            IDbContext ctx = DbFactory.Configure(true);
            PatientBirthRecordDao entityDao = new PatientBirthRecordDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
            PatientDao entityPatientBabyDao = new PatientDao(ctx);
            PatientDao entityPatientMotherDao = new PatientDao(ctx);
            PatientDao entityPatientFatherDao = new PatientDao(ctx);
            AddressDao entityAddressBabyDao = new AddressDao(ctx);
            AddressDao entityAddressMotherDao = new AddressDao(ctx);
            AddressDao entityAddressFatherDao = new AddressDao(ctx);
            ObstetricHistoryDao entityObstetricHistoryDao = new ObstetricHistoryDao(ctx);

            try
            {
                List<PatientBirthRecord> lstPatientBirthRecord = BusinessLayer.GetPatientBirthRecordList(string.Format("VisitID = '{0}'", hdnVisitIDBayi.Value), ctx);
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("MRN IN ({0},{1})", hdnMotherMRN.Value, hdnBabyMRN.Value), ctx);
                Registration MotherRegistration = lstRegistration.Where(x => x.MRN == Convert.ToInt32(hdnMotherMRN.Value)).FirstOrDefault();
                Registration BabyRegistration = lstRegistration.Where(x => x.MRN == Convert.ToInt32(hdnBabyMRN.Value)).FirstOrDefault();
                PatientBirthRecord entity = null;
                if (lstPatientBirthRecord.Count > 0)
                {
                    entity = lstPatientBirthRecord.FirstOrDefault();
                }
                else
                {
                    entity = new PatientBirthRecord();
                }
                ControlToEntity(entity);
                if (lstPatientBirthRecord.Count > 0)
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.IsTemporary = false;
                    entityDao.Update(entity);

                    #region update Partus And New Born
                    if (!BabyRegistration.IsNewBorn)
                    {
                        BabyRegistration.IsNewBorn = true;
                        entityRegistrationDao.Update(BabyRegistration);
                    }

                    if (!MotherRegistration.IsParturition)
                    {
                        MotherRegistration.IsParturition = true;
                        entityRegistrationDao.Update(MotherRegistration);
                    }
                    #endregion

                    retval = entity.BirthRecordID.ToString();
                }
                else
                {
                    entity.IsTemporary = true;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    #region update Partus And New Born
                    if (!BabyRegistration.IsNewBorn)
                    {
                        BabyRegistration.IsNewBorn = true;
                        entityRegistrationDao.Update(BabyRegistration);
                    }

                    if (!MotherRegistration.IsParturition)
                    {
                        MotherRegistration.IsParturition = true;
                        entityRegistrationDao.Update(MotherRegistration);
                    }
                    #endregion

                    retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

                }

                #region Data Bayi

                Patient entityPatientBabyNew = BusinessLayer.GetPatientList(string.Format("MRN = {0}", Convert.ToInt32(hdnBabyMRN.Value)), ctx)[0];
                Address entityAddressBaby = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", entityPatientBabyNew.HomeAddressID), ctx)[0];

                if (cboSalutationBaby.Value != null)
                {
                    entityPatientBabyNew.GCSalutation = cboSalutationBaby.Value.ToString();
                }
                else
                {
                    entityPatientBabyNew.GCSalutation = null;
                }
                entityPatientBabyNew.FirstName = txtFirstNameBaby.Text;
                entityPatientBabyNew.MiddleName = txtMiddleNameBaby.Text;
                entityPatientBabyNew.LastName = txtLastNameBaby.Text;

                entityPatientBabyNew.Name = Helper.GenerateName(entityPatientBabyNew.LastName, entityPatientBabyNew.MiddleName, entityPatientBabyNew.FirstName);
                entityPatientBabyNew.FullName = Helper.GenerateFullName(entityPatientBabyNew.Name, "", "");
                entityPatientBabyNew.PreferredName = txtPreferredNameBaby.Text;

                if (hdnAddressIDBaby.Value != "" && hdnAddressIDBaby.Value != "0")
                {
                    entityAddressBaby = entityAddressBabyDao.Get(Convert.ToInt32(hdnAddressIDBaby.Value));
                }
                else
                {
                    entityAddressBaby = new Address();
                }

                entityAddressBaby.StreetName = txtAddressBaby.Text;

                if (txtZipCodeBaby.Text != "" && txtZipCodeBaby.Text != "0")
                {
                    entityAddressBaby.ZipCode = Convert.ToInt32(hdnZipCodeBaby.Value);
                }
                else
                {
                    entityAddressBaby.ZipCode = null;
                }

                entityAddressBaby.County = txtCountyBaby.Text;
                entityAddressBaby.District = txtDistrictBaby.Text;
                entityAddressBaby.City = txtCityBaby.Text;
                entityAddressBaby.GCState = txtProvinceCodeBaby.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCodeBaby.Text);
                entityAddressBaby.PhoneNo1 = txtTelephoneNoBaby.Text;

                if (hdnAddressIDBaby.Value == "")
                {
                    entityAddressBaby.CreatedBy = AppSession.UserLogin.UserID;
                    entityAddressBabyDao.Insert(entityAddressBaby);
                    entityPatientBabyNew.HomeAddressID = BusinessLayer.GetAddressMaxID(ctx);
                }
                else
                {
                    entityAddressBaby.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressBabyDao.Update(entityAddressBaby);
                    entityPatientBabyNew.HomeAddressID = Convert.ToInt32(hdnAddressIDBaby.Value);
                }

                UploadItemPhoto(entityPatientBabyNew.MRN, entityPatientBabyNew.MedicalNo, entityPatientBabyNew.FullName, ref filename);
                if (!String.IsNullOrEmpty(filename))
                {
                    entityPatientBabyNew.PictureFileName = filename;
                }
                else
                {
                    entityPatientBabyNew.PictureFileName = string.Format("{0}.jpg", entityPatientBabyNew.MedicalNo);
                }

                #endregion

                #region Data Ibu

                List<PatientFamily> lstPatientMother = BusinessLayer.GetPatientFamilyList(string.Format(" MRN = '{0}' AND GCFamilyRelation = '{1}'", hdnBabyMRN.Value, Constant.FamilyRelation.MOTHER), ctx);
                Patient entityPatientMotherNew = BusinessLayer.GetPatientList(string.Format("MRN = {0}", Convert.ToInt32(hdnMotherMRN.Value)), ctx)[0];
                PatientFamily entityPatientFamilyMother = null;
                Address entityAddressMother = null;

                if (lstPatientMother.Count > 0)
                {
                    entityPatientFamilyMother = lstPatientMother.FirstOrDefault();
                }
                else
                {
                    entityPatientFamilyMother = new PatientFamily();
                }

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
                    entityAddressMother.ZipCode = Convert.ToInt32(hdnZipCodeMother.Value);
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

                entityPatientMotherNew.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPatientMotherNew.SSN = txtIdentityNo.Text;

                if (cboOccupationMother.Value != null)
                {
                    entityPatientMotherNew.GCOccupation = cboOccupationMother.Value.ToString();
                    entityPatientFamilyMother.GCOccupation = cboOccupationMother.Value.ToString();
                }
                else
                {
                    entityPatientMotherNew.GCOccupation = null;
                    entityPatientFamilyMother.GCOccupation = null;
                }

                if (lstPatientMother.Count > 0)
                {
                    entityPatientFamilyMother.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
                    entityPatientFamilyMother.IsPatient = true;
                    entityPatientFamilyMother.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Update(entityPatientFamilyMother);
                }
                else
                {
                    entityPatientFamilyMother.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
                    entityPatientFamilyMother.IsPatient = true;
                    entityPatientFamilyMother.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Insert(entityPatientFamilyMother);
                }

                entityPatientMotherDao.Update(entityPatientMotherNew);

                #endregion

                #region Data Ayah

                List<PatientFamily> lstPatientFather = BusinessLayer.GetPatientFamilyList(string.Format(" MRN = '{0}' AND GCFamilyRelation = '{1}'", hdnBabyMRN.Value, Constant.FamilyRelation.FATHER), ctx);
                Patient entityPatientFatherNew = null;
                PatientFamily entityPatientFamilyFather = null;
                Address entityAddressFather = null;

                if (lstPatientFather.Count > 0)
                {
                    entityPatientFamilyFather = lstPatientFather.FirstOrDefault();
                }
                else
                {
                    entityPatientFamilyFather = new PatientFamily();
                }

                entityPatientFamilyFather.MRN = Convert.ToInt32(hdnBabyMRN.Value);
                if (Convert.ToInt32(hdnFatherMRN.Value) != 0)
                {
                    entityPatientFatherNew = BusinessLayer.GetPatientList(string.Format("MRN = '{0}'", Convert.ToInt32(hdnFatherMRN.Value)), ctx)[0];
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

                if (txtFatherDOB.Text != "" && Helper.GetDatePickerValue(txtFatherDOB).ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    entityPatientFamilyFather.DateOfBirth = Helper.GetDatePickerValue(txtFatherDOB.Text);
                    entityPatientFatherNew.DateOfBirth = Helper.GetDatePickerValue(txtFatherDOB.Text);
                }
                else
                {
                    entityPatientFamilyFather.DateOfBirth = null;
                }

                if (cboOccupationFather.Value != null)
                {
                    entityPatientFamilyFather.GCOccupation = cboOccupationFather.Value.ToString();
                    entityPatientFatherNew.GCOccupation = cboOccupationFather.Value.ToString();
                }
                else
                {
                    entityPatientFamilyFather.GCOccupation = null;
                    entityPatientFatherNew.GCOccupation = null;
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

                if (lstPatientFather.Count > 0)
                {
                    entityPatientFamilyFather.GCFamilyRelation = Constant.FamilyRelation.FATHER;
                    entityPatientFamilyFather.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Update(entityPatientFamilyFather);
                }
                else
                {
                    entityPatientFamilyFather.GCFamilyRelation = Constant.FamilyRelation.FATHER;
                    entityPatientFamilyFather.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientFamilyDao.Insert(entityPatientFamilyFather);
                }

                #endregion

                #region Update Data Patient Baby

                entityPatientBabyNew.MotherName = Helper.GenerateFullName(entityPatientFamilyMother.Name, titleMother, suffixMother);
                entityPatientBabyNew.FatherName = Helper.GenerateFullName(entityPatientFamilyFather.Name, titleFather, suffixFather);
                entityPatientBabyNew.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPatientBabyDao.Update(entityPatientBabyNew);

                #endregion

                #region Insert or Update Obstetric History
                string filterHistory = string.Format("MRN = {0} AND RegistrationID = {1} AND IsDeleted = 0", entity.MotherMRN, MotherRegistration.RegistrationID);
                ObstetricHistory oHistory = BusinessLayer.GetObstetricHistoryList(filterHistory, ctx).FirstOrDefault();

                bool isNewRecord = false;

                if (oHistory == null)
                {
                    oHistory = new ObstetricHistory();
                    isNewRecord = true;
                }

                oHistory.MRN = Convert.ToInt32(entity.MotherMRN);
                oHistory.RegistrationID = MotherRegistration.RegistrationID;
                oHistory.PregnancyNo = entity.ChildNo;
                oHistory.ChildMRN = entity.MRN;
                oHistory.GCSex = entityPatientBabyNew.GCSex;
                oHistory.DateOfBirth = entityPatientBabyNew.DateOfBirth;
                oHistory.PregnancyDuration = entity.BirthPregnancyAge;

                if (hdnParamedicID1.Value != "0" && hdnParamedicID1.Value != null && hdnParamedicID1.Value != "")
                    oHistory.GCParamedicType = Constant.ParamedicType.Physician;
                else if (hdnParamedicID3.Value != "0" && hdnParamedicID3.Value != null && hdnParamedicID3.Value != "")
                    oHistory.GCParamedicType = Constant.ParamedicType.Bidan;

                oHistory.GCBirthMethod = entity.GCBirthMethod;
                oHistory.GCCaesarMethod = entity.GCCaesarMethod;
                oHistory.GCBornCondition = entity.GCBornCondition;
                oHistory.Length = entity.Length*1000;
                oHistory.Weight = entity.Weight;
                oHistory.APGARScore1 = entity.APGARScore1;
                oHistory.APGARScore2 = entity.APGARScore2;
                oHistory.APGARScore3 = entity.APGARScore3;
                oHistory.HeadCircumference = entity.HeadCircumference;
                oHistory.Remarks = entity.Remarks;

                if (isNewRecord)
                {
                    oHistory.CreatedBy = AppSession.UserLogin.UserID;
                    entityObstetricHistoryDao.Insert(oHistory);
                }
                else
                {
                    oHistory.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityObstetricHistoryDao.Update(oHistory);
                }
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
            return result;
        }
        #endregion

    }
}