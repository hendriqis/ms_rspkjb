<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="EDCMachineEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.EDCMachineEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:content id="Content2" contentplaceholderid="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:content>
<asp:content id="Content1" contentplaceholderid="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Bank
            $('#lblIBank.lblLink').click(function () {
                var filterExpression = "IsDeleted = 0";
                openSearchDialog('bank', filterExpression, function (value) {
                    $('#<%=txtBankCode.ClientID %>').val(value);
                    onTxtBankCodeChanged(value);
                });
            });

            $('#<%=txtBankCode.ClientID %>').change(function () {
                onTxtBankCodeChanged($(this).val());
            });

            function onTxtBankCodeChanged(value) {
                var filterExpression = "BankID = '" + value + "'";
                Methods.getObject('GetBankList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBankID.ClientID %>').val(result.BankID);
                        $('#<%=txtBankCode.ClientID %>').val(result.BankCode);
                        $('#<%=txtBankName.ClientID %>').val(result.BankName);
                    }
                    else {
                        $('#<%=hdnBankID.ClientID %>').val('');
                        $('#<%=txtBankCode.ClientID %>').val('');
                        $('#<%=txtBankName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>

    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingEdc" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Mesin EDC")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 20%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Mesin EDC")%></label>
                        </td>
                        <td>
                            <asp:textbox id="txtEDCMachineCode" width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Mesin EDC")%></label>
                        </td>
                        <td>
                            <asp:textbox id="txtEDCMachineName" width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Penyedia Kartu")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboCardProvider" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblIBank">
                                <%=GetLabel("Bank")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnBankID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:textbox id="txtBankCode" width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:textbox id="txtBankName" width="42%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:checkbox runat="server" id="chkIsChargeFeeToPatient" text='Fee dikenakan kepada Pasien' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:checkbox runat="server" id="chkIsUsingECR" text='Integration with EDC Device' />
                        </td>
                    </tr>
                     <tr runat="server" id="trEDCArea" style="display:none; ">
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Penyedia EDC")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboEdcVendor" Width="300px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:content>
