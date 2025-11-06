<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderSupplierInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrderSupplierInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<table style="width: 100%">
    <colgroup>
        <col style="width: 50%" />
    </colgroup>
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 50%" />
                        <col style="width: 100%" />
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
                                            <%=GetLabel("Kode Pemasok")%></label>
                                        </td>
                                        <td>
                                            <asp:textbox id="txtSupplierCode" readOnly="true" width="20%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Nama Pemasok")%></label>
                                        </td>
                                        <td>
                                            <asp:textbox id="txtSupplierName" readOnly="true" width="100%" runat="server" />
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
                                            <asp:textbox id="txtContactPersonName" readOnly="true" width="200px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Mobile Number (CP)")%></label>
                                        </td>
                                        <td>
                                            <asp:textbox id="txtContactPersonPhoneNumber" readOnly="true" width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Email (CP)")%></label>
                                        </td>
                                        <td>
                                            <asp:textbox id="txtContactPersonEmail" readOnly="true" cssclass="email" width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="padding: 5px; vertical-align: top">
                        <h4 class="h4expanded">
                            <%=GetLabel("Alamat")%></h4>
                        <div class="containerTblEntryContent">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 30%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jalan")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtAddress" readOnly="true" width="100%" runat="server" textmode="MultiLine" rows="2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Kode Pos")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtZipCode" readOnly="true" width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Desa / Kelurahan")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtCounty" readOnly="true" width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Kecamatan")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtDistrict" readOnly="true" width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Kota")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtCity" readOnly="true" width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Provinsi")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:textbox id="txtProvinceCode" readOnly="true" width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:textbox id="txtProvinceName" readOnly="true" width="100%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Telepon")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtTelephoneNo" readOnly="true" width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No Fax")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtFaxNo" readOnly="true" width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Email")%></label>
                                    </td>
                                    <td>
                                        <asp:textbox id="txtEmail" readOnly="true" cssclass="email" width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>