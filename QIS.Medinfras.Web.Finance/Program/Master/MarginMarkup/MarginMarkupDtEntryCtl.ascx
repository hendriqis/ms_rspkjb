<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarginMarkupDtEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.MarginMarkupDtEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_marginmarkupdtentry">
    $(function () {
        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });
    });

    $('#<%=txtStartingValue.ClientID %>').live('change', function () {
        var startValue = parseFloat($('#<%=txtStartingValue.ClientID %>').val());
        var endingValue = parseFloat($('#<%=hdnEndingValueCtl.ClientID %>').val());

        if (endingValue < startValue) {
            alert('Maaf, nilai rentang awal harus lebih kecil dari rentang akhir.');
            $('#<%=txtStartingValue.ClientID %>').val(endingValue).trigger('changeValue');
        }

        $('#<%=hdnStartingValueCtl.ClientID %>').val(endingValue);
    });

    $('#<%=txtEndingValue.ClientID %>').live('change', function () {
        var startValue = parseFloat($('#<%=hdnStartingValueCtl.ClientID %>').val());
        var endingValue = parseFloat($('#<%=txtEndingValue.ClientID %>').val());

        if (endingValue < startValue) {
            alert('Maaf, nilai rentang akhir harus lebih besar dari rentang awal.');
            $('#<%=txtEndingValue.ClientID %>').val(startValue).trigger('changeValue');
        }

        $('#<%=hdnEndingValueCtl.ClientID %>').val(startValue);
    });
</script>
<div style="height: 300px; overflow-y: auto">
    <input type="hidden" id="hdnMarkupIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnSequenceNoCtl" value="" runat="server" />
    <input type="hidden" id="hdnStartingValueCtl" value="" runat="server" />
    <input type="hidden" id="hdnEndingValueCtl" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <table class="tblEntryContent" cellpadding="0" cellspacing="1">
                        <colgroup>
                            <col style="width: 160px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Markup")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtMarkupCodeCtl" Width="100px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Markup")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtMarkupNameCtl" Width="250px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sequence")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtSequenceNoCtl" Width="100px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Rentang Awal")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartingValue" CssClass="txtCurrency" Width="150px"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Rentang Akhir")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndingValue" CssClass="txtCurrency" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Margin Default")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtMarkupAmount" CssClass="txtCurrency" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Margin Rawat Inap")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtMarkupAmount2" CssClass="txtCurrency" Width="150px"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
