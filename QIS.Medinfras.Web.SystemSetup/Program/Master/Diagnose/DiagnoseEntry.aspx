<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="DiagnoseEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.DiagnoseEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region DTD
            $('#lblDTDNo.lblLink').click(function () {
                openSearchDialog('dtd', '', function (value) {
                    $('#<%=txtDTDNo.ClientID %>').val(value);
                    onTxtDTDNoChanged(value);
                });
            });

            $('#<%=txtDTDNo.ClientID %>').change(function () {
                onTxtDTDNoChanged($(this).val());
            });

            function onTxtDTDNoChanged(value) {
                var filterExpression = "DTDNo = '" + value + "'";
                Methods.getObject('GetDTDList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtDTDName.ClientID %>').val(result.DTDName);
                    else {
                        $('#<%=txtDTDNo.ClientID %>').val('');
                        $('#<%=txtDTDName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Diagnose")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Diagnose ID")%></label></td>
                        <td><asp:TextBox ID="txtDiagnoseID" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Diagnose Name")%></label></td>
                        <td><asp:TextBox ID="txtDiagnoseName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblDTDNo"><%=GetLabel("DTD No")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtDTDNo" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtDTDName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsDisease" runat="server" /><%=GetLabel("Disease")%></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsChronicDisease" runat="server" /><%=GetLabel("Chronic Disease")%></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
