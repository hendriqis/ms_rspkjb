using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BodyDiagramSOAPEdit1Ctl : BaseViewPopupCtl
    {
        public int currRegistrationNumRows = 0;
        public int patientBodyDiagramID;

        public int ctrA = 0;
        public int ctrB = 0;
        public int ctrC = 0;
        public int ctrD = 0;
        public int ctrE = 0;
        public int ctrF = 0;
        public int ctrG = 0;
        public int ctrH = 0;
        public int ctrI = 0;
        public int ctrJ = 0;
        public int ctrK = 0;
        public int ctrL = 0;
        public int ctrM = 0;
        public int ctrN = 0;
        public int ctrO = 0;
        public int ctrP = 0;
        public int ctrQ = 0;
        public int ctrR = 0;
        public int ctrS = 0;
        public int ctrT = 0;
        public int ctrU = 0;
        public int ctrV = 0;
        public int ctrW = 0;
        public int ctrX = 0;
        public int ctrY = 0;
        public int ctrZ = 0;
        
          public override void InitializeDataControl(string queryString)
        {
            hdnPatientBodyDiagramID.Value = queryString;
            patientBodyDiagramID = Convert.ToInt32(queryString);

            PatientBodyDiagramHd bodyDiagramHd = BusinessLayer.GetPatientBodyDiagramHd(patientBodyDiagramID);
            if (bodyDiagramHd.DiagramID != null)
            {
                BodyDiagram bodyDiagram = BusinessLayer.GetBodyDiagram((int)bodyDiagramHd.DiagramID);

                string ImagePath = AppConfigManager.QISVirtualDirectory + AppConfigManager.QISBodyDiagramImagePath;
                string imageUrl = ImagePath + bodyDiagram.ImageUrl;
                byte[] data;
                using (WebClient client = new WebClient())
                    data = client.DownloadData(imageUrl);

                imgPreview.Src = @"data:image/gif;base64," + Convert.ToBase64String(data);
            }
            else
            {
                string ImagePath = AppConfigManager.QISVirtualDirectory + AppConfigManager.QISBodyDiagramImagePath + "Custom/";
                string imageUrl = ImagePath + bodyDiagramHd.CustomFileName;
                byte[] data;
                using (WebClient client = new WebClient())
                    data = client.DownloadData(imageUrl);

                imgPreview.Src = @"data:image/gif;base64," + Convert.ToBase64String(data);

            }
            GenerateSymbol();

            string filterExpression = String.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);
            currRegistrationNumRows = BusinessLayer.GetPatientBodyDiagramHdList(filterExpression).Count;

            int hour = Int32.Parse(bodyDiagramHd.ObservationTime.Substring(0, 2));
            int minute = Int32.Parse(bodyDiagramHd.ObservationTime.Substring(3, 2));
            txtObservationDate.Text = bodyDiagramHd.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = bodyDiagramHd.ObservationTime;

            List<vPatientBodyDiagramDt> list = BusinessLayer.GetvPatientBodyDiagramDtList(String.Format("ID = {0} AND IsDeleted = 0",  patientBodyDiagramID));
            foreach (vPatientBodyDiagramDt ptBodyDiagramDt in list)
            {
                ptBodyDiagramDt.TopPositionImage = ptBodyDiagramDt.TopMargin * (300 / 100);
                ptBodyDiagramDt.LeftPositionImage = ptBodyDiagramDt.LeftMargin * (300 / 100);
            }
            ctrA = getCounter(list, "A");
            ctrB = getCounter(list, "B");
            ctrC = getCounter(list, "C");
            ctrD = getCounter(list, "D");
            ctrE = getCounter(list, "E");
            ctrF = getCounter(list, "F");
            ctrG = getCounter(list, "G");
            ctrH = getCounter(list, "H");
            ctrI = getCounter(list, "I");
            ctrJ = getCounter(list, "J");
            ctrK = getCounter(list, "K");
            ctrL = getCounter(list, "L");
            ctrM = getCounter(list, "M");
            ctrN = getCounter(list, "N");
            ctrO = getCounter(list, "O");
            ctrP = getCounter(list, "P");
            ctrQ = getCounter(list, "Q");
            ctrR = getCounter(list, "R");
            ctrS = getCounter(list, "S");
            ctrT = getCounter(list, "T");
            ctrU = getCounter(list, "U");
            ctrV = getCounter(list, "V");
            ctrW = getCounter(list, "W");
            ctrX = getCounter(list, "X");
            ctrY = getCounter(list, "Y");
            ctrZ = getCounter(list, "Z");

            rptRemarks.DataSource = list;
            rptRemarks.DataBind();

            rptPatientBodyDiagramSymbol.DataSource = list;
            rptPatientBodyDiagramSymbol.DataBind();

        }

        private int getCounter(List<vPatientBodyDiagramDt> list, String symbolCode)
        {
            int result = 0;
            IEnumerable<vPatientBodyDiagramDt> tmpList = list.Where(p => p.SymbolCode.StartsWith(symbolCode)).OrderByDescending(v => int.Parse(v.SymbolCode.Substring(1)));
            if (tmpList.Count() > 0)
            {
                result = Int32.Parse(tmpList.First().SymbolCode.Substring(1));
            }
            return result;
        }

        #region Symbol

        private void GenerateSymbol()
        {
            tblContainerSymbol.Rows.Clear();
            HtmlTableRow row = new HtmlTableRow();
            tblContainerSymbol.Rows.Add(row);
            row.Cells.Add(CreateGroupSymbol());
        }

        private HtmlTableCell CreateGroupSymbol()
        {
            string filterExpression = String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.BODY_DIAGRAM_SYMBOL);
            List<StandardCode> listSymbol = BusinessLayer.GetStandardCodeList(filterExpression);

            HtmlTableCell cell = new HtmlTableCell();
            cell.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");

            HtmlTable tbl = new HtmlTable();
            cell.Controls.Add(tbl);

            foreach (StandardCode symbol in listSymbol)
            {
                HtmlTableRow row = new HtmlTableRow();
                tbl.Rows.Add(row);
                row.Cells.Add(CreateSymbol(symbol.TagProperty, symbol.StandardCodeName, symbol.StandardCodeID));
            }

            return cell;
        }

        private HtmlTableCell CreateSymbol(string imgUrl, string StandardCodeName, string StandardCodeID)
        {
            HtmlGenericControl div = new HtmlGenericControl("DIV");
            div.Attributes.Add("class", "drag");

            HtmlInputHidden inputGroupID = new HtmlInputHidden();
            if (StandardCodeID == "X115^Z")
            {
                inputGroupID.Value = "Z";
            }
            else
            {
                inputGroupID.Value = StandardCodeID.Substring(5, 1);
            }
            inputGroupID.Attributes.Add("class", "symbolGroupValue");

            HtmlTableCell cell = new HtmlTableCell();
            Image img = new Image();
            img.ImageUrl = imgUrl;
            img.ToolTip = StandardCodeName;
            img.AlternateText = StandardCodeID.Substring(5, 1);
            div.Controls.Add(img);
            div.Controls.Add(inputGroupID);
            cell.Controls.Add(div);
            return cell;
        }
        #endregion
    }
}