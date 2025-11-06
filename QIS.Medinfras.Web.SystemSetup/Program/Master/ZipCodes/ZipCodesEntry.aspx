<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="ZipCodesEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ZipCodesEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 300px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Pos")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtZipCode" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Nama Jalan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStreetName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Kelurahan/Desa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCountyCodeReference" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kelurahan/Desa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCounty" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kecamatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDistrict" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kabupaten/Kota")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Masuk Region Kabupaten/Kota")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsInRegionDistrict" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Provinsi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCProvince" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Negara")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCNation" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Latitude")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLatitude" Width="150px" runat="server" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Longitude")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLongitude" Width="150px" runat="server" CssClass="number" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
