<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="SupplierFinanceEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.SupplierFinanceEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            registerCollapseExpandHandler();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Umum")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Kode Pemasok")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierCode" Width="20%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Kode External")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExternalCode" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Nama Pemasok")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Singkat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtShortName" Width="200px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Contact Person (CP)")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama (CP)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPersonName" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Mobile Number (CP)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPersonPhoneNumber" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email (CP)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPersonEmail" CssClass="email" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Pemasok")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Rumah Sakit")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboHealthcare" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Tipe Pemasok")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSupplierType" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor PKP/NPWP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVATRegistrationNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Termin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTerm" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah Max PO")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaxPOAmount" Width="100%" CssClass="txtCurrency" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Waktu Tunggu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLeadTime" CssClass="number" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblSupplierLine">
                                    <%=GetLabel("Supplier Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnSupplierLineID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtSupplierLineCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtSupplierLineName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Item Supplier")%></label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsPharmacySupplier" Text="Farmasi" Width="100%" runat="server" />
                                <asp:CheckBox ID="chkIsLogisticSupplier" Text="Logistik" Width="100%" runat="server" />
                                <%--<asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Farmasi" Value="1" Selected="True" />
                                    <asp:ListItem Text="Logistik" Value="0" />
                                </asp:RadioButtonList>--%>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Akunt GL Segment")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGLAccountSegmentNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Bank")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cabang Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankBranch" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Account Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankAccountNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Account Virtual Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankVirtualAccountNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Account Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankAccountName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Sandi Kliring")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankClearingPassword" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 70%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: middle">
                                <label class="lblWarning" id="lblInfoTransactionOpen" runat="server" style="color: Red">
                                    <%=GetLabel("Informasi Bank hanya bisa di ubah di master yang ada di modul Keuangan") %></label>
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4 class="h4expanded">
                        <%=GetLabel("Atribut")%></h4>
                    <asp:Repeater ID="rptCustomAttribute" runat="server">
                        <HeaderTemplate>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                    </colgroup>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%#: Eval("Value") %></label>
                                </td>
                                <td>
                                    <input type="hidden" value='<%#: Eval("Code") %>' runat="server" id="hdnTagFieldCode" />
                                    <asp:TextBox ID="txtTagField" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
