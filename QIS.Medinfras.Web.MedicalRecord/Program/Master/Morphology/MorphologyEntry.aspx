<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="MorphologyEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MorphologyEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
     <script type="text/javascript">
         function onLoad() {
             //#region Diagnose Code
             function getDiagnoseCodeExpression() {
                 var filterExpression = "IsDeleted = 0"; 
                 return filterExpression;
             }

             $('#lblDiagnoseCode.lblLink').click(function () {
                 openSearchDialog('diagnose', getDiagnoseCodeExpression(), function (value) {
                     $('#<%=txtDiagnoseID.ClientID %>').val(value);
                     onTxtDiagnoseCodeChanged(value);
                 });
             });

             $('#<%=txtDiagnoseID.ClientID %>').change(function () {
                 onTxtDiagnoseCodeChanged($(this).val());
             });

             function onTxtDiagnoseCodeChanged(value) {
                 var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
                 Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                     if (result != null)
                         $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                     else {
                         $('#<%=txtDiagnoseID.ClientID %>').val('');
                         $('#<%=txtDiagnoseName.ClientID %>').val('');
                     }
                 });
             }
             //#endregion
         }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Morfologi")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Morfologi")%></label></td>
                        <td><asp:TextBox ID="txtMorphologyID" Width="20%" runat="server" /></td>
                    </tr>
                     <tr>
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblDiagnoseCode"><%=GetLabel("Kode Diagnosa")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtDiagnoseName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Morfologi")%></label></td>
                        <td><asp:TextBox ID="txtMorphologyName" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
