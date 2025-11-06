<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="SurveyDataEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.SurveyDataEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
   <script type="text/javascript" id="dxss_patiententryctl">
       function onLoad() {
       }

       $('#btnGetIHS').live('click', function () {
           var surveyCode = $('#<%=txtSurveyCode.ClientID %>').val();
           var surveyName = $('#<%=txtSurveyName.ClientID %>').val();
           var surveyNotes = $('#<%=txtSurveyNotes.ClientID %>').val();
           try {
               IHSService.createIHSLocationID(surveyCode, surveyQuestion, surveyNotes, function (result) {
                   GetIHSDataHandler(result);
               });
           }
           catch (err) {
               displayErrorMessageBox("Integrasi SATUSEHAT", err.Message);
           }
       });

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitICUID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitPICUID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitNICUID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyNotes" TextMode="MultiLine" Rows="10" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
