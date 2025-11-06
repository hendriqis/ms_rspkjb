using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AntenatalRecordFormEntry : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnFormType.Value = paramInfo[0];

            if (paramInfo.Length > 1)
            {
                IsAdd = false;
                hdnID.Value = paramInfo[1];

                if (paramInfo.Length > 2)
                {
                    hdnIsPartograf.Value = paramInfo[2];
                }
                else
                {
                    hdnIsPartograf.Value = "0";
                }
            }
            else
            {
                IsAdd = true;
            }

            SetControlProperties();

            if (IsAdd)
            {
                PopulateFormContent();
            }
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\Antenatal\{1}.html", filePath, hdnFormType.Value.Replace('^', '_'));
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            hdnFormLayout.Value = innerHtml.ToString();
            divFormContent.InnerHtml = innerHtml.ToString();
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.TIPE_PENYAKIT_INFEKSI,
        Constant.StandardCode.LMP_PERIOD,
        Constant.StandardCode.RESIKO_KEHAMILAN));
            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_PENYAKIT_INFEKSI).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCInfectiousDisease, lstCode1, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.LMP_PERIOD).ToList();
            Methods.SetComboBoxField<StandardCode>(cboLMPPeriod, lstCode2, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RESIKO_KEHAMILAN).ToList();
            Methods.SetComboBoxField<StandardCode>(cboPregnancyRisk, lstCode3, "StandardCodeName", "StandardCodeID");

            if (hdnIsPartograf.Value == "0")
            {
                trPartografInfo1.Style.Add("display", "none");
                trPartografInfo2.Style.Add("display", "none");
            }
            else
            {
                trPartografInfo1.Style.Add("display", "table-row");
                trPartografInfo2.Style.Add("display", "table-row");
            }

            if (!IsAdd)
            {
                string filter = string.Format("ID = '{0}'", hdnID.Value);
                vAntenatalRecord entity = BusinessLayer.GetvAntenatalRecordList(filter).FirstOrDefault();
                EntityToControl(entity);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPregnancyNo, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(txtMenarche, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLMP, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtLMPDays, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEDB, new ControlEntrySetting(true, true, true, DateTime.Now.AddDays(280).ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtGravida, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPara, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAbortion, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtLife, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtMenstrualHistory, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMedicalHistory, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtMembraneDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtMembraneTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtColicDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtColicTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(Antenatal oAntenatal)
        {
            oAntenatal.PregnancyNo = Convert.ToInt16(txtPregnancyNo.Text);
            oAntenatal.LMP = Helper.GetDatePickerValue(txtLMP);
            oAntenatal.LMPDays = Convert.ToInt32(txtLMPDays.Text);
            oAntenatal.GCLMPPeriod = cboLMPPeriod.Value.ToString();
            oAntenatal.IsTrueIbuNifas = chkIsIbuNifas.Checked;
            oAntenatal.IsTrueKortikosteroid = chkIsKortikosteroid.Checked;
            oAntenatal.IsHasInfectiousDisease = rblIsHasInfectiousDisease.SelectedValue == "1" ? true : false;
            if (oAntenatal.IsHasInfectiousDisease)
            {
                if (cboGCInfectiousDisease.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCInfectiousDisease.Value.ToString()))
                    {
                        oAntenatal.GCInfectiousDisease = cboGCInfectiousDisease.Value.ToString();
                        if (cboGCInfectiousDisease.Value.ToString() == "X522^999")
                            oAntenatal.OtherInfectiousDIsease = Page.Request.Form[txtOtherInfectiousDisease.UniqueID].ToString();
                        else
                            oAntenatal.OtherInfectiousDIsease = null;
                    }
                }
            }
            else
            {
                oAntenatal.GCInfectiousDisease = null;
                oAntenatal.OtherInfectiousDIsease = null;
            }
            oAntenatal.EDB = Helper.GetDatePickerValue(txtEDB);
            oAntenatal.Gravida = Convert.ToInt16(txtGravida.Text);
            oAntenatal.Para = Convert.ToInt16(txtPara.Text);
            oAntenatal.Abortion = Convert.ToInt16(txtAbortion.Text);
            oAntenatal.Life = Convert.ToInt16(txtLife.Text);
            oAntenatal.MenstrualHistory = txtMenstrualHistory.Text;
            oAntenatal.MedicalHistory = txtMedicalHistory.Text;

            oAntenatal.PregnancyRiskLayout = hdnFormLayout.Value;
            oAntenatal.PregnancyRiskValue = hdnFormValues.Value;
            if (cboPregnancyRisk.Value != null)
            {
                oAntenatal.GCPregnancyRiskType = cboPregnancyRisk.Value.ToString();
            }

            if (!string.IsNullOrEmpty(txtMembraneTime.Text))
            {
                oAntenatal.RuptureOfMembranesDate = Helper.GetDatePickerValue(txtMembraneDate);
                oAntenatal.RuptureOfMembranesTime = txtMembraneTime.Text;
            }

            if (!string.IsNullOrEmpty(txtColicTime.Text))
            {
                oAntenatal.ColicDate = Helper.GetDatePickerValue(txtColicDate);
                oAntenatal.ColicTime = txtColicTime.Text;
            }
        }

        private void EntityToControl(vAntenatalRecord oAntenatal)
        {
            txtPregnancyNo.Text = oAntenatal.PregnancyNo.ToString();
            txtMenarche.Text = oAntenatal.Menarche.ToString();
            txtLMP.Text = oAntenatal.LMP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLMPDays.Text = oAntenatal.LMPDays.ToString();
            cboLMPPeriod.Value = oAntenatal.GCLMPPeriod;
            rblIsHasInfectiousDisease.SelectedValue = oAntenatal.IsHasInfectiousDisease ? "1" : "0";
            if (oAntenatal.IsHasInfectiousDisease)
            {
                cboGCInfectiousDisease.Value = oAntenatal.GCInfectiousDisease;
                txtOtherInfectiousDisease.Text = oAntenatal.OtherInfectiousDIsease;
                trInfectiousInfo.Style.Add("display", "table-row");
                if (cboGCInfectiousDisease.Value != null)
                {
                    if (cboGCInfectiousDisease.Value.ToString() == Constant.StandardCode.InfectiousDisease.OTHERS)
                        txtOtherInfectiousDisease.ReadOnly = false;
                }
            }
            else
            {
                trInfectiousInfo.Style.Add("display", "none");
            }
            txtEDB.Text = oAntenatal.EDB.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtGravida.Text = oAntenatal.Gravida.ToString();
            txtPara.Text = oAntenatal.Para.ToString();
            txtAbortion.Text = oAntenatal.Abortion.ToString();
            txtLife.Text = oAntenatal.Life.ToString();
            txtMenstrualHistory.Text = oAntenatal.MenstrualHistory;
            txtMedicalHistory.Text = oAntenatal.MedicalHistory;
            chkIsIbuNifas.Checked = oAntenatal.IsTrueIbuNifas;
            chkIsKortikosteroid.Checked = oAntenatal.IsTrueKortikosteroid;

            cboPregnancyRisk.Value = oAntenatal.GCPregnancyRiskType;

            if (!string.IsNullOrEmpty(oAntenatal.PregnancyRiskLayout))
            {
                hdnFormLayout.Value = oAntenatal.PregnancyRiskLayout;
                hdnFormValues.Value = oAntenatal.PregnancyRiskValue;
                divFormContent.InnerHtml = hdnFormLayout.Value;
            }
            else
            {
                PopulateFormContent();
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AntenatalDao antenatalDao = new AntenatalDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            FetusDataDao fetusDataDao = new FetusDataDao(ctx);

            try
            {
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filterAntenatal = string.Format("MRN = '{0}' AND IsBorn = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
                List<Antenatal> lstAntenatal = BusinessLayer.GetAntenatalList(filterAntenatal, ctx);
                if (lstAntenatal.Count <= 0)
                {
                    int antenatalRecordID = 0;

                    Antenatal entity = new Antenatal();
                    ControlToEntity(entity);
                    entity.MRN = AppSession.RegisteredPatient.MRN;
                    entity.FirstVisitID = AppSession.RegisteredPatient.VisitID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    antenatalRecordID = antenatalDao.InsertReturnPrimaryKeyID(entity);

                    #region Patient
                    if (!string.IsNullOrEmpty(txtMenarche.Text) && Methods.IsNumeric(txtMenarche.Text))
                    {
                        //Update Patient Menarche Information
                        Patient oPatient = patientDao.Get(entity.MRN);
                        if (oPatient != null)
                        {
                            oPatient.Menarche = Convert.ToInt16(txtMenarche.Text);
                            patientDao.Update(oPatient);
                        }
                    }
                    #endregion

                    #region Fetus Information
                    FetusData oFetusData = new FetusData();

                    oFetusData.AntenatalRecordID = antenatalRecordID;
                    oFetusData.FetusNo = 1;
                    oFetusData.CreatedBy = AppSession.UserLogin.UserID;
                    fetusDataDao.Insert(oFetusData);

                    #endregion


                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Proses penambahan Antenatal Record tidak dapat dilakukan karena masih ada record yang aktif";
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            bool isError = false;
            IDbContext ctx = DbFactory.Configure(true);
            AntenatalDao antenatalDao = new AntenatalDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            if (!IsValidToProceed(ref errMessage))
            {
                isError = true;
                result = false;
            }

            if (!isError)
            {
                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    Antenatal entity = antenatalDao.Get(Convert.ToInt32(hdnID.Value));
                    if (entity != null)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        antenatalDao.Update(entity);

                        if (!string.IsNullOrEmpty(txtMenarche.Text) && Methods.IsNumeric(txtMenarche.Text))
                        {
                            //Update Patient Menarche Information
                            Patient oPatient = patientDao.Get(entity.MRN);
                            if (oPatient != null)
                            {
                                oPatient.Menarche = Convert.ToInt16(txtMenarche.Text);
                                patientDao.Update(oPatient);
                            }
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ditemukan antenatal record yang dilakukan perubahan";
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
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        private bool IsValidToProceed(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            DateTime date;
            string format = Constant.FormatString.DATE_PICKER_FORMAT;

            try
            {
                date = DateTime.ParseExact(txtLMP.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine(string.Format("Format Tanggal Terakhir Menstruasi (HPHT) tidak sesuai dengan format", txtMembraneDate.Text));
            }
            ;
            if (DateTime.Compare(Helper.GetDatePickerValue(txtLMP.Text), DateTime.Now.Date) > 0)
            {
                errMsg.AppendLine("Tanggal Terakhir Menstruasi harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            try
            {
                date = DateTime.ParseExact(txtMembraneDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine(string.Format("Format Tanggal Ketuban mulai pecah {0} tidak sesuai dengan format", txtMembraneDate.Text));
            }

            if (DateTime.Compare(Helper.GetDatePickerValue(txtMembraneDate.Text), DateTime.Now.Date) > 0)
            {
                errMsg.AppendLine("Tanggal Ketuban mulai pecah harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            try
            {
                date = DateTime.ParseExact(txtColicDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine(string.Format("Format Tanggal mulai mulas perut {0} tidak sesuai dengan format", txtColicDate.Text));
            }

            if (DateTime.Compare(Helper.GetDatePickerValue(txtColicDate.Text), DateTime.Now.Date) > 0)
            {
                errMsg.AppendLine("Tanggal mulai mulas perut harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            if (!string.IsNullOrEmpty(txtMembraneTime.Text) || txtMembraneTime.Text != "")
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtMembraneTime.Text))
                {
                    errMsg.AppendLine("Jam ketuban mulai pecah harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
            }

            if (!string.IsNullOrEmpty(txtColicTime.Text) || txtColicTime.Text != "")
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtColicTime.Text))
                {
                    errMsg.AppendLine("Jam mulai mulas perut harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
            }

            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }
    }
}