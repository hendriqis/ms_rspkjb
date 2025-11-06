using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class EREvaluationNotes : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.PHYSICAL_EXAMINATION;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(4);

            Helper.SetControlEntrySetting(txtEvaluationNotes, new ControlEntrySetting(true, true, true), "mpEvaluationNotes");

             vPatientVisitNote entity = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EVALUATION_NOTES)).FirstOrDefault();
             if (entity != null)
                 txtEvaluationNotes.Text = entity.NoteText;
             else
                 txtEvaluationNotes.Text = "";
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.GCPatientNoteType = Constant.PatientVisitNotes.EVALUATION_NOTES;
            entity.NoteText = txtEvaluationNotes.Text;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if(type == "save")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao entityDao = new PatientVisitNoteDao(ctx);
                try
                {
                    List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EVALUATION_NOTES));
                    if (lstPatientVisitNote.Count > 0)
                    {
                        PatientVisitNote entity = lstPatientVisitNote.FirstOrDefault();
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                    }
                    else
                    {
                        PatientVisitNote entity = new PatientVisitNote();

                        ControlToEntity(entity);
                        DateTime date = new DateTime();
                        date = DateTime.Now;
                        string tgl = string.Format("{0:yyyy-MM-dd}", date);
                        string jam = string.Format("{0:hh:mm}", date);
                        DateTime noteDate = Convert.ToDateTime(tgl);
                        entity.NoteDate = noteDate;
                        entity.NoteTime = jam;
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
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
            return false;
        }
    }
}