<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="MorphologyEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.MorphologyEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Diagnose
            $('#lblDiagnose.lblLink').click(function () {
                openSearchDialog('diagnose', '', function (value) {
                    $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                    onTxtDiagnoseCodeChanged(value);
                });
            });

            $('#<%=txtDiagnoseCode.ClientID %>').change(function () {
                onTxtDiagnoseCodeChanged($(this).val());
            });

            function onTxtDiagnoseCodeChanged(value) {
                var filterExpression = "DiagnoseID = '" + value + "'";
                Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    else {
                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Morphology")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Morphology Code")%></label></td>
                        <td><asp:TextBox ID="txtMorphologyID" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblDiagnose"><%=GetLabel("Diagnose Code")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtDiagnoseCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtDiagnoseName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Morphology Name")%></label></td>
                        <td><asp:TextBox ID="txtMorphologyName" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
