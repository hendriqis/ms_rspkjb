using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using System.Net;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class BodyDiagramAddCtl : BaseViewPopupCtl
    {
        public int currRegistrationNumRows = 0;
        public override void InitializeDataControl(string param)
        {
            //IsAdd = true;
            GenerateThumbs();
            GenerateSymbol();

            string filterExpression = String.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);
            currRegistrationNumRows = BusinessLayer.GetPatientBodyDiagramHdList(filterExpression).Count;

            txtObservationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = DateTime.Now.ToString("HH:mm");

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = userLoginParamedic.ToString();
            }

        }

        #region Thumbnails

        private void GenerateThumbs()
        {
            string ImagePath = AppConfigManager.QISVirtualDirectory + AppConfigManager.QISBodyDiagramImagePath;
            //listGroup.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "All" });
            List<StandardCode> listGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.BODY_DIAGRAM_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboImageGroup, listGroup, "StandardCodeName", "StandardCodeID");

            List<BodyDiagram> listBodyDiagram = BusinessLayer.GetBodyDiagramList("IsDeleted = 0");

            Random random = new Random();
            foreach (StandardCode group in listGroup)
            {
                List<BodyDiagram> list = listBodyDiagram.Where(p => p.GCBodyDiagramGroup == group.StandardCodeID).ToList();
                foreach (BodyDiagram symbol in list)
                {
                    ulThumbs.Controls.Add(CreateThumbnails(random, group.StandardCodeID, symbol.DiagramID, ImagePath + symbol.ImageUrl));
                }
            }
        }

        private HtmlGenericControl CreateThumbnails(Random random, string groupID, int diagramID, string imgUrl)
        {
            imgUrl = string.Format("{0}?{1}", imgUrl, random.Next(1000000, 100000000));
            HtmlGenericControl li = new HtmlGenericControl("li");

            HtmlInputHidden inputGroupID = new HtmlInputHidden();
            inputGroupID.Value = groupID.ToString();
            inputGroupID.Attributes.Add("class", "imageGroupValue");

            HtmlInputHidden bodyDiagramID = new HtmlInputHidden();
            bodyDiagramID.Value = diagramID.ToString();
            bodyDiagramID.Attributes.Add("class", "bodyDiagramID");

            Image img = new Image();
            byte[] data;
            using (WebClient client = new WebClient())
                data = client.DownloadData(imgUrl);
            img.Attributes.Add("urlbase64", @"data:image/gif;base64," + Convert.ToBase64String(data));
            img.ImageUrl = imgUrl;
            img.Attributes.Add("class", "thumb");

            HtmlGenericControl anchor = new HtmlGenericControl("a");
            anchor.Attributes.Add("href", img.ImageUrl);
            anchor.Controls.Add(img);

            li.Controls.Add(anchor);
            li.Controls.Add(inputGroupID);
            li.Controls.Add(bodyDiagramID);
            return li;
        }
        #endregion

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
                row.Cells.Add(CreateSymbol(symbol.TagProperty, symbol.StandardCodeName));
            }

            return cell;
        }

        private HtmlTableCell CreateSymbol(string imgUrl, string tooltip)
        {
            HtmlGenericControl div = new HtmlGenericControl("DIV");
            div.Attributes.Add("class", "drag");

            HtmlInputHidden inputGroupID = new HtmlInputHidden();
            inputGroupID.Value = tooltip.Substring(0, 1);
            inputGroupID.Attributes.Add("class", "symbolGroupValue");

            HtmlTableCell cell = new HtmlTableCell();
            Image img = new Image();
            img.ImageUrl = imgUrl;
            img.ToolTip = tooltip;
            img.AlternateText = tooltip.Substring(0, 1);
            div.Controls.Add(img);
            div.Controls.Add(inputGroupID);
            cell.Controls.Add(div);
            return cell;
        }
        #endregion
    }
}