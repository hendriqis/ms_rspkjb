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
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientFamilyCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;

                vPatient entity = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", param)).FirstOrDefault();
                txtMRN.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;

                hdnMRN.Value = param;

                vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                hdnGCState.Value = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
                hdnState.Value = healthcare.State;
                hdnPhoneArea.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;

                BindGridView();

                SetControlProperties();
                GetSettingParameter();
            }
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        private void SetControlProperties()
        {
            txtFamilyMRN.Attributes.Add("validationgroup", "mpPatientFamily");
            txtFirstName.Attributes.Add("validationgroup", "mpPatientFamily");
            txtMiddleName.Attributes.Add("validationgroup", "mpPatientFamily");
            txtLastName.Attributes.Add("validationgroup", "mpPatientFamily");
            txtTelephoneNo.Attributes.Add("validationgroup", "mpPatientFamily");

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0 AND IsActive = 1",
                                                                                        Constant.StandardCode.TITLE, //0
                                                                                        Constant.StandardCode.SALUTATION, //1
                                                                                        Constant.StandardCode.SUFFIX, //2
                                                                                        Constant.StandardCode.FAMILY_RELATION, //3
                                                                                        Constant.StandardCode.GENDER, //4
                                                                                        Constant.StandardCode.OCCUPATION //5
                                                                                    ));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboSalutation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.FAMILY_RELATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelationLink, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.FAMILY_RELATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGender, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboOccupation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.OCCUPATION).ToList(), "StandardCodeName", "StandardCodeID");

            cboSalutation.SelectedIndex = 0;
            cboTitle.SelectedIndex = 0;
            cboSuffix.SelectedIndex = 0;
            cboFamilyRelation.SelectedIndex = 0;
            cboFamilyRelationLink.SelectedIndex = 0;

            Helper.SetControlEntrySetting(cboFamilyRelation, new ControlEntrySetting(true, true, true), "mpPatientFamily");
            Helper.SetControlEntrySetting(cboFamilyRelationLink, new ControlEntrySetting(true, true, true), "mpRelationFamilyLink");
            Helper.SetControlEntrySetting(txtLastName, new ControlEntrySetting(true, true, true), "mpPatientFamily");
            Helper.SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true), "mpPatientFamily");
            Helper.SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, true), "mpPatientFamily");
            Helper.SetControlEntrySetting(txtBirthPlace, new ControlEntrySetting(true, true, true), "mpPatientFamily");
            Helper.SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, true, true), "mpPatientFamily");
            Helper.SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, true), "mpPatientFamily");
        }

        private void BindGridView()
        {
            List<vPatientFamily> lst = BusinessLayer.GetvPatientFamilyList(string.Format("MRN = {0} AND IsDeleted = 0", hdnMRN.Value));
            grdFamilyName.DataSource = lst;
            grdFamilyName.DataBind();
        }

        protected void cbpPatientFamily_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnFamilyID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientFamily entity, Address entityAddress, Patient entityPatient)
        {
            if (hdnFamilyMRN.Value != "" && hdnFamilyMRN.Value != "0" && hdnFamilyMRN.Value != null)
            {
                entity.FamilyMRN = Convert.ToInt32(hdnFamilyMRN.Value);
            }
            else
            {
                if (cboSalutation.Value != null)
                    entity.GCSalutation = cboSalutation.Value.ToString();
                else
                    entity.GCSalutation = "";
                if (cboTitle.Value != null)
                    entity.GCTitle = cboTitle.Value.ToString();
                else
                    entity.GCTitle = "";
                if (cboSuffix.Value != null)
                    entity.GCSuffix = cboSuffix.Value.ToString();
                else
                    entity.GCSuffix = "";
                entity.FirstName = txtFirstName.Text;
                entity.MiddleName = txtMiddleName.Text;
                entity.LastName = txtLastName.Text;

                string suffix = cboSuffix.Value == null ? "" : cboSuffix.Text;
                string title = cboTitle.Value == null ? "" : cboTitle.Text;
                entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
                entity.FullName = Helper.GenerateFullName(entity.Name, title, suffix);

                entity.DateOfBirth = Helper.GetDatePickerValue(txtDOB.Text);
                entity.CityOfBirth = txtBirthPlace.Text;
                if (cboGender.Value != null)
                {
                    entity.GCGender = cboGender.Value.ToString();
                }
                if (cboOccupation.Value != null)
                {
                    entity.GCOccupation = cboOccupation.Value.ToString();
                }
            }

            entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();

            #region Patient Address
            entityAddress.StreetName = txtAddress.Text;
            entityAddress.County = txtCounty.Text; // Desa
            entityAddress.District = txtDistrict.Text; //Kabupaten
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            entityAddress.PhoneNo1 = txtTelephoneNo.Text;
            entityAddress.Email = txtEmail.Text;
            #endregion

            #region PatientMotherFatherSpouse
            if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.FATHER))
            {
                entityPatient.FatherName = entity.FullName;
            }

            if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.MOTHER))
            {
                entityPatient.MotherName = entity.FullName;
            }

            if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.SPOUSE))
            {
                entityPatient.SpouseName = entity.FullName;
            }
            #endregion
        }

        private void ControlToEntityFamilyRelation(PatientFamily entity, Address entityAddress, Patient entityPatient)
        {
            entity.FamilyMRN = Convert.ToInt32(hdnMRN.Value);
            entity.GCFamilyRelation = cboFamilyRelationLink.Value.ToString();

            #region PatientMotherFatherSpouse
            if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.FATHER))
            {
                entityPatient.FatherName = entity.FullName;
            }

            if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.MOTHER))
            {
                entityPatient.MotherName = entity.FullName;
            }

            if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.SPOUSE))
            {
                entityPatient.SpouseName = entity.FullName;
            }
            #endregion
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            int patientAddressID = 0;
            string patientEmailAddress = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientFamilyDao entityDao = new PatientFamilyDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            PatientDao entityPatientDao = new PatientDao(ctx);
            try
            {
                PatientFamily entity = new PatientFamily();
                PatientFamily entityLink = new PatientFamily();
                Address entityAddress = null;
                Patient entityPatient = BusinessLayer.GetPatient(Convert.ToInt32(hdnMRN.Value));
                patientEmailAddress = entityPatient.EmailAddress;
                patientAddressID = Convert.ToInt32(entityPatient.HomeAddressID);

                if (hdnAddressID.Value != "")
                    entityAddress = entityAddressDao.Get(Convert.ToInt32(hdnAddressID.Value));
                else
                    entityAddress = new Address();

                ControlToEntity(entity, entityAddress, entityPatient);

                entity.MRN = Convert.ToInt32(hdnMRN.Value);
                if (hdnAddressID.Value != null && hdnAddressID.Value != "0" && hdnAddressID.Value != "")
                {
                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityAddress);
                    entity.AddressID = Convert.ToInt32(hdnAddressID.Value);
                }
                else
                {
                    entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entity.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.FamilyID = entityDao.InsertReturnPrimaryKeyID(entity);

                if (chkAutomaticallyLinkFamily.Checked)
                {
                    entityLink = new PatientFamily();
                    entityPatient = BusinessLayer.GetPatient(Convert.ToInt32(hdnFamilyMRN.Value));

                    if (entityPatient.HomeAddressID > 0)
                    {
                        entityAddress = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", entityPatient.HomeAddressID), ctx).FirstOrDefault();
                    }
                    else
                    {
                        entityAddress = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", patientAddressID), ctx).FirstOrDefault();
                    }

                    ControlToEntityFamilyRelation(entityLink, entityAddress, entityPatient);

                    entityLink.MRN = Convert.ToInt32(hdnFamilyMRN.Value);

                    entityLink.AddressID = entityAddress.AddressID;

                    //if (hdnAddressID.Value != null && hdnAddressID.Value != "0" && hdnAddressID.Value != "")
                    //{
                    //    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //    entityAddressDao.Update(entityAddress);
                    //    entityLink.AddressID = Convert.ToInt32(hdnAddressID.Value);
                    //}
                    //else
                    //{
                    //    entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                    //    entityLink.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                    //}


                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPatientDao.Update(entityPatient);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityLink.CreatedBy = AppSession.UserLogin.UserID;
                    entityLink.FamilyID = entityDao.InsertReturnPrimaryKeyID(entityLink);
                }

                ctx.CommitTransaction();

                BridgingToMedinfrasMobileApps(entity, txtEmail.Text, "001");
                if (chkAutomaticallyLinkFamily.Checked)
                {
                    BridgingToMedinfrasMobileApps(entityLink, patientEmailAddress, "001");
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientFamilyDao entityDao = new PatientFamilyDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            PatientDao entityPatientDao = new PatientDao(ctx);
            try
            {
                PatientFamily entity = entityDao.Get(Convert.ToInt32(hdnFamilyID.Value));
                Address entityAddress = entityAddressDao.Get((int)entity.AddressID);
                Patient entityPatient = BusinessLayer.GetPatient(Convert.ToInt32(hdnMRN.Value));
                if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.FATHER))
                {
                    entityPatient.FatherName = null;
                }
                else if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.MOTHER))
                {
                    entityPatient.MotherName = null;
                }
                else if (entity.GCFamilyRelation.Equals(Constant.FamilyRelation.SPOUSE))
                {
                    entityPatient.SpouseName = null;
                }
                ControlToEntity(entity, entityAddress, entityPatient);

                entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Update(entityAddress);

                entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPatientDao.Update(entityPatient);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();

                BridgingToMedinfrasMobileApps(entity, entityPatient.EmailAddress, "002");
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityPatientDao = new PatientDao(ctx);
            PatientFamilyDao entityFamilyDao = new PatientFamilyDao(ctx);
            try
            {
                PatientFamily entity = entityFamilyDao.Get(Convert.ToInt32(hdnFamilyID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityFamilyDao.Update(entity);

                Patient pt = entityPatientDao.Get(entity.MRN);
                if (pt != null)
                {
                    if (entity.GCFamilyRelation == Constant.FamilyRelation.MOTHER)
                    {
                        pt.MotherName = null;
                    }
                    if (entity.GCFamilyRelation == Constant.FamilyRelation.FATHER)
                    {
                        pt.FatherName = null;
                    }
                    if (entity.GCFamilyRelation == Constant.FamilyRelation.SPOUSE)
                    {
                        pt.SpouseName = null;
                    }
                    pt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPatientDao.Update(pt);
                }

                ctx.CommitTransaction();

                BridgingToMedinfrasMobileApps(entity, txtEmail.Text, "003");
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

        private void BridgingToMedinfrasMobileApps(PatientFamily oPatient, string email, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                if (oPatient != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    entityAPILog.IsSuccess = true;
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.Sender = "MEDINFRAS";
                    entityAPILog.Recipient = "MOBILE APPS";

                    string apiResult = oService.OnPatientFamilyMasterChanged(oPatient, email, eventType);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.ErrorMessage = apiResultInfo[2];
                        //entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[2]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[2];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}