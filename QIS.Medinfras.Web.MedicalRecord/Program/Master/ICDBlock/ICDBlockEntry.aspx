<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="ICDBlockEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ICDBlockEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
     <script type="text/javascript">
         function onLoad() {
             //#region FromICD
             function getICDFromExpression() {
                 var filterExpression = "IsDeleted = 0";
                 return filterExpression;
             }

             $('#lblICDBlockCodeFrom.lblLink').click(function () {
                 openSearchDialog('diagnose', getICDFromExpression(), function (value) {
                     $('#<%=txtFromICDCode.ClientID %>').val(value);
                     onTxtFromICDCodeChanged(value);
                 });
             });

             $('#<%=txtFromICDCode.ClientID %>').change(function () {
                 onTxtFromICDCodeChanged($(this).val());
             });

             function onTxtFromICDCodeChanged(value) {
                 var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
                 Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                     if (result != null)
                         $('#<%=txtFromICDName.ClientID %>').val(result.DiagnoseName);
                     else {
                         $('#<%=txtFromICDCode.ClientID %>').val('');
                         $('#<%=txtFromICDName.ClientID %>').val('');
                     }
                 });
             }
             //#endregion
             //#region ToICD
             function getICDToExpression() {
                 var filterExpression = "IsDeleted = 0";
                 return filterExpression;
             }

             $('#lblICDBlockCodeTo.lblLink').click(function () {
                 openSearchDialog('diagnose', getICDToExpression(), function (value) {
                     $('#<%=txtToICDCode.ClientID %>').val(value);
                     onTxtToICDCodeChanged(value);
                 });
             });

             $('#<%=txtToICDCode.ClientID %>').change(function () {
                 onTxtToICDCodeChanged($(this).val());
             });

             function onTxtToICDCodeChanged(value) {
                 var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
                 Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                     if (result != null)
                         $('#<%=txtToICDName.ClientID %>').val(result.DiagnoseName);
                     else {
                         $('#<%=txtToICDCode.ClientID %>').val('');
                         $('#<%=txtToICDName.ClientID %>').val('');
                     }
                 });
             }
             //#endregion
         }
    </script>
    
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Blok ICD")%></div>--%>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("ID Blok ICD")%></label></td>
                        <td><asp:TextBox ID="txtICDCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Blok ICD")%></label></td>
                        <td><asp:TextBox ID="txtICDName" Width="400px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblICDBlockCodeFrom"><%=GetLabel("Dari Diagnosa")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtFromICDCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtFromICDName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblICDBlockCodeTo"><%=GetLabel("Ke Diagnosa")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtToICDCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtToICDName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
