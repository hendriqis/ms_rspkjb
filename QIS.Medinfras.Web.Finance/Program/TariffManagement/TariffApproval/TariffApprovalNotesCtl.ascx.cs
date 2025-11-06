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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TariffApprovalNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnBookID.Value = param;

            BindGridView();

            txtNewNotes.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            int bookID = Convert.ToInt32(hdnBookID.Value);
            TariffBookHd entity = BusinessLayer.GetTariffBookHd(bookID);

            List<TariffNotes> ListTariffNotes = new List<TariffNotes>();
            if (entity.Notes != "")
            {
                ListTariffNotes = entity.Notes.Split(';').Select(n => new TariffNotes { FullText = n }).ToList();
                int ctr = 1;
                foreach (TariffNotes notes in ListTariffNotes)
                    notes.ID = ctr++;
            }
            grdNotes.DataSource = ListTariffNotes;
            grdNotes.DataBind();

        }

        public class TariffNotes
        {
            public string FullText;

            public int ID { get; set; }

            public String DateInString
            {
                get
                {
                    DateTime date = Helper.DateInStringToDateTime(FullText.Split('|')[0]);
                    return date.ToString(Constant.FormatString.DATE_FORMAT);
                }
            }
            public String Time
            {
                get { return FullText.Split('|')[1]; }
            }
            public String CreatedBy
            {
                get { return FullText.Split('|')[2]; }
            }
            public String Text
            {
                get { return FullText.Split('|')[3]; }
            }
        }

        protected void cbpSaveNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            try
            {
                int bookID = Convert.ToInt32(hdnBookID.Value);
                TariffBookHd entity = BusinessLayer.GetTariffBookHd(bookID);
                DateTime now = DateTime.Now;

                string newNotes = String.Format("{0}|{1}|{2}|{3}", now.ToString(Constant.FormatString.DATE_FORMAT_112), now.ToString(Constant.FormatString.TIME_FORMAT),
                    AppSession.UserLogin.UserName, txtNewNotes.Text);
                if (entity.Notes != "")
                    entity.Notes += ";";
                entity.Notes += newNotes;
                BusinessLayer.UpdateTariffBookHd(entity);
                result = "success";
            }
            catch (Exception ex)
            {
                result = "fail|" + ex.Message;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}