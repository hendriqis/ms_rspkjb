<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DraftPrescriptionOrderDtServiceViewCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DraftPrescriptionOrderDtServiceViewCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_draftprescriptionorderdtviewctl">
    $(function () {
        $val = $('#<%=hdnHideCheckbox.ClientID %>').val();
        if ($val == "1") {
            $('.chkSelectAll input').prop('style', 'display:none');
            $('.chkIsSelectedTestOrder input').each(function () {
                $(this).prop('style', 'display:none');
            });
        }
    });

</script>
<input type="hidden" value="0" id="hdnHideCheckbox" runat="server" />
<asp:ListView ID="lvwTestOrder" runat="server">
    <EmptyDataTemplate>
        <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
            <tr>
                <th style="width: 40px" align="center">
                    <div style="padding: 3px">
                        <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                    </div>
                </th>
                <th>
                    <div>
                        <%=GetLabel("generik")%>
                        -
                        <%=GetLabel("Nama Obat")%>
                        -
                        <%=GetLabel("Kadar")%>
                        -
                        <%=GetLabel("Bentuk")%></div>
                    <div>
                        <div style="color: Blue; width: 45px; float: left;">
                            <%=GetLabel("DOSIS")%></div>
                        <%=GetLabel("dosis")%>
                        -
                        <%=GetLabel("Rute")%>
                        -
                        <%=GetLabel("Frekuensi")%></div>
                </th>
                <th style="width: 100px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Dokter")%>
                    </div>
                </th>
                <th style="width: 150px">
                    <div style="text-align: left; padding-left: 3px">
                        <%=GetLabel("Instalasi Farmasi")%>
                    </div>
                </th>
                <th style="width: 80px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Lokasi Obat")%>
                    </div>
                </th>
                <th style="width: 100px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Jenis Resep")%>
                    </div>
                </th>
                <th>
                    <div style="width: 50px; text-align: center; padding-left: 3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>
            </tr>
            <tr class="trEmpty">
                <td colspan="7">
                    <%=GetLabel("No Data To Display") %>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
            <tr>
                <th style="width: 40px" align="center">
                    <div style="padding: 3px">
                        <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                    </div>
                </th>
                <th>
                    <div>
                        <%=GetLabel("generik")%>
                        -
                        <%=GetLabel("Nama Obat")%>
                        -
                        <%=GetLabel("Kadar")%>
                        -
                        <%=GetLabel("Bentuk")%></div>
                    <div>
                        <div style="color: Blue; width: 45px; float: left;">
                            <%=GetLabel("DOSIS")%></div>
                        <%=GetLabel("dosis")%>
                        -
                        <%=GetLabel("Rute")%>
                        -
                        <%=GetLabel("Frekuensi")%></div>
                </th>
                <th style="width: 100px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Dokter")%>
                    </div>
                </th>
                <th style="width: 150px">
                    <div style="text-align: left; padding-left: 3px">
                        <%=GetLabel("Instalasi Farmasi")%>
                    </div>
                </th>
                <th style="width: 80px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Lokasi Obat")%>
                    </div>
                </th>
                <th style="width: 100px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Jenis Resep")%>
                    </div>
                </th>
                <th>
                    <div style="width: 50px; text-align: center; padding-left: 3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>
            </tr>
            <tr runat="server" id="itemPlaceholder">
            </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td align="center">
                <div style="padding: 3px">
                    <asp:CheckBox ID="chkIsSelectedPrescriptionOrder" CssClass="chkIsSelectedPrescriptionOrder"
                        runat="server" />
                    <input type="hidden" class="hdnKeyField" value="<%#: Eval("DraftPrescriptionOrderDetailID")%>" />
                </div>
            </td>
            <td>
                <div>
                    <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                        min-width: 30px; float: left;' />
                    <%#: Eval("InformationLine1")%></div>
                <div>
                    <div style="color: Blue; width: 45px; float: left;">
                        <%=GetLabel("DOSIS")%></div>
                    <%#: Eval("NumberOfDosage")%>
                    <%#: Eval("DosingUnit")%>
                    -
                    <%#: Eval("Route")%>
                    -
                    <%#: Eval("SignaName1")%></div>
            </td>
            <td>
                <div style="padding: 3px;">
                    <div>
                        <%#: Eval("ParamedicName")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: left;">
                    <div>
                        <%#: Eval("ServiceUnitName")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: center;">
                    <div>
                        <%#: Eval("LocationName")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: left;">
                    <div>
                        <%#: Eval("PrescriptionType")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: left;">
                    <div>
                        <%#: Eval("CreatedByFullName")%></div>
                    <div>
                        <%#: Eval("cfCreatedDateInString")%></div>
                </div>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
