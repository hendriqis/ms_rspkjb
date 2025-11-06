using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RadiotheraphyProgramEntry : BaseEntryPopupCtl3
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnPopupMRN.Value = paramInfo[0];
            hdnPopupMedicalNo.Value = paramInfo[1];
            hdnPopupPatientName.Value = paramInfo[2];
            hdnPopupVisitID.Value = paramInfo[3];
            hdnFormType.Value = paramInfo[4];
            hdnID.Value = paramInfo[5];
            
            if (hdnID.Value == "0" || hdnID.Value == string.Empty)
            {
                IsAdd = true;
            }
            else
            {
                IsAdd = false;
            }

            SetControlProperties(); 

            //if (IsAdd)
            //{
            //    PopulateFormContent();
            //}
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\Radiotheraphy\{1}.html", filePath, hdnFormType.Value.Replace('^', '_'));
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
            txtMedicalNo.Text = hdnPopupMedicalNo.Value;
            txtPatientName.Text = hdnPopupPatientName.Value;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.BEAM_TECHNIQUE,
        Constant.StandardCode.RADIOTHERAPY_TYPE,
        Constant.StandardCode.APPLICATOR_TYPE,
        Constant.StandardCode.RADIOTHERAPY_PLAN,
        Constant.StandardCode.RADIOTHERAPY_VERIFICATION,
        Constant.StandardCode.RADIOTHERAPY_PURPOSE,
        Constant.StandardCode.BRACHYTHERAPY_TYPE));

            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_TECHNIQUE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCBeamTechnique, lstCode3, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_TECHNIQUE && lst.StandardCodeID != "X572^999").ToList();
            dlBeamTechnique.DataSource = lstCode4;
            dlBeamTechnique.DataBind();

            List<StandardCode> lstCode5 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCTherapyType, lstCode5, "StandardCodeName", "StandardCodeID");
            cboGCTherapyType.SelectedIndex = 0;

            List<StandardCode> lstCode6 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BRACHYTHERAPY_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCBrachyTherapyType, lstCode6, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode7 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.APPLICATOR_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorType, lstCode7, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode8 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_PLAN).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCTherapyPlan, lstCode8, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode9 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_VERIFICATION).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCVerificationType, lstCode9, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode10 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_PURPOSE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCTherapyPurpose, lstCode10, "StandardCodeName", "StandardCodeID");

            if (!IsAdd)
            {
                string filter = string.Format("ProgramID = '{0}'", hdnID.Value);
                vRadiotheraphyProgram entity = BusinessLayer.GetvRadiotheraphyProgramList(filter).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                txtProgramDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtProgramTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProgramDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
        }

        private void ControlToEntity(RadiotherapyProgram oRecord)
        {
            oRecord.ProgramDate = Helper.GetDatePickerValue(txtProgramDate);
            oRecord.ProgramTime = txtProgramTime.Text;
            oRecord.MRN = Convert.ToInt32(hdnPopupMRN.Value);
            oRecord.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            oRecord.GCTherapyType = cboGCTherapyType.Value.ToString();
            oRecord.GCTherapyPurpose = cboGCTherapyPurpose.Value.ToString();
            if (oRecord.GCTherapyType == Constant.RadiotherapyType.EXTERNAL)
            {
                oRecord.GCBeamTechnique = cboGCBeamTechnique.Value.ToString();
                if (oRecord.GCBeamTechnique == "X572^999")
                {
                    oRecord.CombinationBeamTechCode = hdnCombinationTechniqueCode.Value;
                    oRecord.CombinationBeamTech = hdnCombinationTechnique.Value;
                }

                oRecord.GCTherapyPlan = cboGCTherapyPlan.Value.ToString();
            }
            else if (oRecord.GCTherapyType == Constant.RadiotherapyType.EXTERNAL)
            {
                oRecord.GCBrachytherapyType = cboGCBrachyTherapyType.Value.ToString();
                oRecord.GCApplicatorType = cboGCApplicatorType.Value.ToString();
            }

            oRecord.NumberOfDosage1 = Convert.ToDecimal(txtNumberOfDosage1.Text);
            oRecord.NumberOfFields1 = Convert.ToDecimal(txtNumberOfFields1.Text);
            oRecord.TotalDosage1 = Convert.ToDecimal(Request.Form[txtTotalDosage1.UniqueID]);

            oRecord.IsHasDosage2 = chkIsHasSequence2.Checked;
            if (oRecord.IsHasDosage2)
            {
                oRecord.NumberOfDosage2 = Convert.ToDecimal(txtNumberOfDosage2.Text);
                oRecord.NumberOfFields2 = Convert.ToDecimal(txtNumberOfFields2.Text);
                oRecord.TotalDosage2 = Convert.ToDecimal(Request.Form[txtTotalDosage2.UniqueID]); 
            }

            oRecord.IsHasDosage3 = chkIsHasSequence3.Checked;
            if (oRecord.IsHasDosage3)
            {
                oRecord.NumberOfDosage3 = Convert.ToDecimal(txtNumberOfDosage3.Text);
                oRecord.NumberOfFields3 = Convert.ToDecimal(txtNumberOfFields3.Text);
                oRecord.TotalDosage3 = Convert.ToDecimal(Request.Form[txtTotalDosage3.UniqueID]);
            }

            oRecord.GCVerificationType = cboGCVerificationType.Value.ToString();

            if (!string.IsNullOrEmpty(txtRemarks.Text))
                oRecord.Remarks = txtRemarks.Text;
        }

        private void EntityToControl(vRadiotheraphyProgram oRecord)
        {
            txtProgramDate.Text = oRecord.ProgramDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProgramTime.Text = oRecord.ProgramTime;
            cboGCBeamTechnique.Value = oRecord.GCBeamTechnique;
            if (oRecord.GCBeamTechnique == "X572^999")
            {
                trCombination.Style.Add("display", "table-row");
            }
            else
            {
                trCombination.Style.Add("display", "none");
            }
            hdnCombinationTechniqueCode.Value = oRecord.CombinationBeamTechCode;
            hdnCombinationTechnique.Value = oRecord.CombinationBeamTech;
            txtRemarks.Text = oRecord.Remarks;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (IsValidToProceed(ref errMessage))
            {

                IDbContext ctx = DbFactory.Configure(true);
                RadiotherapyProgramDao objDao = new RadiotherapyProgramDao(ctx);

                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int recordID = 0;

                    RadiotherapyProgram entity = new RadiotherapyProgram();
                    ControlToEntity(entity);
                    entity.MRN = AppSession.RegisteredPatient.MRN;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    recordID = objDao.InsertReturnPrimaryKeyID(entity);

                    retVal = recordID.ToString();

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    retVal = "0";
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            bool isError = false;
            IDbContext ctx = DbFactory.Configure(true);
            RadiotherapyProgramDao objDao = new RadiotherapyProgramDao(ctx);

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
                    RadiotherapyProgram entity = objDao.Get(Convert.ToInt32(hdnID.Value));
                    if (entity != null)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        objDao.Update(entity);

                        retVal = hdnID.Value;

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ditemukan program radioterapi yang dilakukan perubahan";
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
                date = DateTime.ParseExact(txtProgramDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine(string.Format("Format Tanggal Program tidak sesuai dengan format", txtProgramDate.Text));
            }
;
            if (DateTime.Compare(Helper.GetDatePickerValue(txtProgramDate.Text), DateTime.Now.Date) > 0)
            {
                errMsg.AppendLine("Tanggal Program harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            if (!string.IsNullOrEmpty(txtProgramTime.Text) || txtProgramTime.Text != "")
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtProgramTime.Text))
                {
                    errMsg.AppendLine("Jam pengisian program harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
            }

            string therapyType = string.Empty;
            if (cboGCTherapyType.Value != null)
            {
                if (string.IsNullOrEmpty(cboGCTherapyType.Value.ToString()))
                {
                    errMsg.AppendLine("Jenis Radioterapi harus diisi atau tidak boleh kosong");
                }
                else
                {
                    therapyType = cboGCTherapyType.Value.ToString();
                    if (therapyType == Constant.RadiotherapyType.EXTERNAL)
                    {
                        if (cboGCBeamTechnique.Value != null)
                        {
                            if (string.IsNullOrEmpty(cboGCBeamTechnique.Value.ToString()))
                            {
                                errMsg.AppendLine("Teknik Penyinaran Radiasi External harus diisi atau tidak boleh kosong");
                            }
                        }
                        else
                        {
                            errMsg.AppendLine("Teknik Penyinaran Radiasi External harus diisi atau tidak boleh kosong");
                        }

                        if (cboGCTherapyPlan.Value != null)
                        {
                            if (string.IsNullOrEmpty(cboGCTherapyPlan.Value.ToString()))
                            {
                                errMsg.AppendLine("Rencana Pengobatan harus diisi atau tidak boleh kosong");
                            }
                        }
                        else
                        {
                            errMsg.AppendLine("Rencana Pengobatan harus diisi atau tidak boleh kosong");
                        }

                        if (cboGCVerificationType.Value != null)
                        {
                            if (string.IsNullOrEmpty(cboGCVerificationType.Value.ToString()))
                            {
                                errMsg.AppendLine("Tipe verifikasi harus diisi atau tidak boleh kosong");
                            }
                        }
                        else
                        {
                            errMsg.AppendLine("Tipe verifikasi harus diisi atau tidak boleh kosong");
                        }
                    }
                    else
                    {
                        if (cboGCBrachyTherapyType.Value != null)
                        {
                            if (string.IsNullOrEmpty(cboGCBrachyTherapyType.Value.ToString()))
                            {
                                errMsg.AppendLine("Jenis Brakiterapi harus diisi atau tidak boleh kosong");
                            }
                        }

                        if (cboGCApplicatorType.Value != null)
                        {
                            if (string.IsNullOrEmpty(cboGCApplicatorType.Value.ToString()))
                            {
                                errMsg.AppendLine("Aplikator Brakiterapi harus diisi atau tidak boleh kosong");
                            }
                        }
                    }
                }
            }
            else
            {
                errMsg.AppendLine("Jenis Radioterapi harus diisi atau tidak boleh kosong");
            }

            if (cboGCTherapyPurpose.Value != null)
            {
                if (string.IsNullOrEmpty(cboGCTherapyType.Value.ToString()))
                {
                    errMsg.AppendLine("Tujuan Radiasi harus diisi atau tidak boleh kosong");
                }
                else
                {
                    if (!string.IsNullOrEmpty(therapyType) && (therapyType == Constant.RadiotherapyType.EXTERNAL) && (cboGCTherapyPurpose.Value.ToString() == Constant.RadiotherapyPurpose.BOOSTER_EXTERNAL))
                    {
                        errMsg.AppendLine("Tujuan Radioterapi dengan Tipe Booster Pasca Radiasi Eksterna hanya untuk Tipe Radioterapi dengan Brakiterapi");
                    }
                }
            }
            else
            {
                errMsg.AppendLine("Tujuan Radiasi harus diisi atau tidak boleh kosong");
            }

            if (string.IsNullOrEmpty(txtNumberOfDosage1.Text))
            {
                errMsg.AppendLine("Jumlah Dosis 1 harus diisi atau tidak boleh kosong");
            }
            else
            {
                if (!Methods.IsNumeric(txtNumberOfDosage1.Text))
                {
                    errMsg.AppendLine("Jumlah Dosis 1 harus berupa angka dan lebih besar dari 0");
                }
                else
                {
                    if (Convert.ToInt32(txtNumberOfDosage1.Text) <= 0)
                    {
                        errMsg.AppendLine("Jumlah Dosis 1 harus berupa angka dan lebih besar dari 0");
                    }
                }
            }

            if (string.IsNullOrEmpty(txtNumberOfFields1.Text))
            {
                errMsg.AppendLine("Jumlah Fraksi 1 harus diisi atau tidak boleh kosong");
            }
            else
            {
                if (!Methods.IsNumeric(txtNumberOfFields1.Text))
                {
                    errMsg.AppendLine("Jumlah Fraksi 1 harus berupa angka dan lebih besar dari 0");
                }
                else
                {
                    if (Convert.ToInt32(txtNumberOfFields1.Text) <= 0)
                    {
                        errMsg.AppendLine("Jumlah Fraksi 1 harus berupa angka dan lebih besar dari 0");
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtNumberOfDosage2.Text))
            {
                if (!Methods.IsNumeric(txtNumberOfDosage2.Text))
                {
                    errMsg.AppendLine("Jumlah Dosis 2 harus berupa angka dan lebih besar dari 0");
                }
                else
                {
                    if (Convert.ToInt32(txtNumberOfDosage2.Text) <= 0)
                    {
                        errMsg.AppendLine("Jumlah Dosis 2 harus berupa angka dan lebih besar dari 0");
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtNumberOfFields2.Text))
            {
                if (!Methods.IsNumeric(txtNumberOfFields2.Text))
                {
                    errMsg.AppendLine("Jumlah Fraksi 2 harus berupa angka dan lebih besar dari 0");
                }
                else
                {
                    if (Convert.ToInt32(txtNumberOfFields2.Text) <= 0)
                    {
                        errMsg.AppendLine("Jumlah Fraksi 2 harus berupa angka dan lebih besar dari 0");
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtNumberOfDosage3.Text))
            {
                if (!Methods.IsNumeric(txtNumberOfDosage3.Text))
                {
                    errMsg.AppendLine("Jumlah Dosis 3 harus berupa angka dan lebih besar dari 0");
                }
                else
                {
                    if (Convert.ToInt32(txtNumberOfDosage3.Text) <= 0)
                    {
                        errMsg.AppendLine("Jumlah Dosis 3 harus berupa angka dan lebih besar dari 0");
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtNumberOfFields3.Text))
            {
                if (!Methods.IsNumeric(txtNumberOfFields3.Text))
                {
                    errMsg.AppendLine("Jumlah Fraksi 3 harus berupa angka dan lebih besar dari 0");
                }
                else
                {
                    if (Convert.ToInt32(txtNumberOfFields3.Text) <= 0)
                    {
                        errMsg.AppendLine("Jumlah Fraksi 3 harus berupa angka dan lebih besar dari 0");
                    }
                }
            }

            errMessage = errMsg.ToString().Replace(Environment.NewLine, "<br />");

            return (errMessage == string.Empty);
        }
    }
}