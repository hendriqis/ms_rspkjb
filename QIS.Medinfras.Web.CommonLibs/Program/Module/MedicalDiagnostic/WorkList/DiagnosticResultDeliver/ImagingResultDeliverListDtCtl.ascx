<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagingResultDeliverListDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ImagingResultDeliverListDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ImagingResultDeliverListDtCtl">
    $(function () {
        setDatePicker('<%=txtResultDeliveredDate.ClientID %>');

        $('#<%=txtResultDeliveredDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });

</script>
<input type="hidden" id="hdnImagingResultIDCtl" value="" runat="server" />
<div style="height: 400px; overflow-y: auto;">
    <table class="tblContentArea">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                        <col style="width: 80px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="150px" ReadOnly="true" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Tanggal-Jam Registrasi") %></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtRegistrationDate" Width="120px" ReadOnly="true" runat="server"
                                            Style="text-align: center" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationTime" Width="80px" ReadOnly="true" runat="server"
                                            Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientInfo" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Tanggal-Jam Hasil") %></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtResultDate" Width="120px" ReadOnly="true" runat="server" Style="text-align: center" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultTime" Width="80px" ReadOnly="true" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal-Jam Order") %>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtOrderDate" Width="120px" ReadOnly="true" runat="server" Style="text-align: center" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrderTime" Width="80px" ReadOnly="true" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px">
                            <%=GetLabel("Catatan Hasil")%>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2"
                                ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table>
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                        <col style="width: 80px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal-Jam Terima Hasil ")%></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtResultDeliveredDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultDeliveredTime" Width="80px" CssClass="time" runat="server"
                                            Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Hasil Diterima Oleh")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResultDeliveredToName" Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Diproses Terima Pada")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResultDeliveredDateTime" Width="250px" runat="server" Style="text-align: center"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Diproses Terima Oleh")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResultDeliveredByName" Width="250px" runat="server" Style="text-align: left"
                                ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
