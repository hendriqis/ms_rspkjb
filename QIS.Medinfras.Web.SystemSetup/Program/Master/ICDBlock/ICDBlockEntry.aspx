<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ICDBlockEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ICDBlockEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region From ICD
            $('#lblFromICD.lblLink').click(function () {
                openSearchDialog('diagnose', '', function (value) {
                    $('#<%=txtFromICDNo.ClientID %>').val(value);
                    onTxtFromICDNoChanged(value);
                });
            });

            $('#<%=txtFromICDNo.ClientID %>').change(function () {
                onTxtFromICDNoChanged($(this).val());
            });

            function onTxtFromICDNoChanged(value) {
                var filterExpression = "DiagnoseID = '" + value + "'";
                Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtFromICDName.ClientID %>').val(result.DiagnoseName);
                    else {
                        $('#<%=txtFromICDNo.ClientID %>').val('');
                        $('#<%=txtFromICDName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("ICD Block")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("ICD Block ID")%></label></td>
                        <td><asp:TextBox ID="txtICDBlockID" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("ICD Block Name")%></label></td>
                        <td><asp:TextBox ID="txtICDBlockName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblFromICD"><%=GetLabel("From ICD")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtFromICDNo" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtFromICDName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblToICD"><%=GetLabel("To ICD")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtToICDNo" Width="100%" runat="server" /></td>
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
