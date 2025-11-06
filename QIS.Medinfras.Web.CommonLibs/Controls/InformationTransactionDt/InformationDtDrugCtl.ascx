<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformationDtDrugCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InformationDtDrugCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_infodtdrug">

</script>
<input type="hidden" value="0" id="hdnHideCheckbox" runat="server" />
<asp:ListView ID="lvwDrug" runat="server">
    <EmptyDataTemplate>
        <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
            <tr>
                <th rowspan="2">
                    <div style="text-align: left; padding-left: 3px">
                        <%=GetLabel("Deskripsi")%>
                    </div>
                </th>
                <th rowspan="2" style="width: 70px">
                    <div style="text-align: center;">
                        <%=GetLabel("Kelas Tagihan")%>
                    </div>
                </th>
                <%--<th rowspan="2" style="width: 80px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Harga Satuan")%>
                    </div>
                </th>--%>
                <th rowspan="2" style="width: 50px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Jumlah")%>
                    </div>
                </th>
                <%--<th colspan="3" align="center">
                    <%=GetLabel("Harga")%>
                </th>--%>
                <th colspan="3" align="center">
                    <%=GetLabel("Total")%>
                </th>
                <th rowspan="2" style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>
                <th rowspan="2" style="width: 50px">
                    <div style="text-align: center;">
                        <%=GetLabel("Verified")%>
                    </div>
                </th>
                <th rowspan="2" style="width: 50px">
                    <div style="text-align: center;">
                        <%=GetLabel("Reviewed")%>
                    </div>
                </th>
            </tr>
            <tr>
                <%--<th style="width: 70px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Harga")%>
                    </div>
                </th>
                <th style="width: 70px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("CITO")%>
                    </div>
                </th>
                <th style="width: 70px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Diskon")%>
                    </div>
                </th>--%>
                <th style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Instansi")%>
                    </div>
                </th>
                <th style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Pasien")%>
                    </div>
                </th>
                <th style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Total")%>
                    </div>
                </th>
            </tr>
            <tr class="trEmpty">
                <td colspan="13">
                    <%=GetLabel("No Data To Display") %>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="tblView" runat="server" class="grdDrug grdNormal" cellspacing="0" rules="all">
            <tr>
                <th rowspan="2">
                    <div style="text-align: left; padding-left: 3px">
                        <%=GetLabel("Deskripsi")%>
                    </div>
                </th>
                <th rowspan="2" style="width: 70px">
                    <div style="text-align: center;">
                        <%=GetLabel("Kelas Tagihan")%>
                    </div>
                </th>
                <%--<th rowspan="2" style="width: 80px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Harga Satuan")%>
                    </div>
                </th>--%>
                <th rowspan="2" style="width: 50px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Jumlah")%>
                    </div>
                </th>
                <%--<th colspan="3" align="center">
                    <%=GetLabel("Harga")%>
                </th>--%>
                <th colspan="3" align="center">
                    <%=GetLabel("Total")%>
                </th>
                <th rowspan="2" style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>
                <th rowspan="2" style="width: 50px">
                    <div style="text-align: center;">
                        <%=GetLabel("Verified")%>
                    </div>
                </th>
                <th rowspan="2" style="width: 50px">
                    <div style="text-align: center;">
                        <%=GetLabel("Reviewed")%>
                    </div>
                </th>
            </tr>
            <tr>
                <%--<th style="width: 70px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Harga")%>
                    </div>
                </th>
                <th style="width: 70px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("CITO")%>
                    </div>
                </th>
                <th style="width: 70px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Diskon")%>
                    </div>
                </th>--%>
                <th style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Instansi")%>
                    </div>
                </th>
                <th style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Pasien")%>
                    </div>
                </th>
                <th style="width: 120px">
                    <div style="text-align: right; padding-right: 3px">
                        <%=GetLabel("Total")%>
                    </div>
                </th>
            </tr>
            <tr runat="server" id="itemPlaceholder">
            </tr>
            <tr id="Tr1" class="trFooter" runat="server">
                <td colspan="3" align="right" style="padding-right: 3px">
                    <%=GetLabel("Total") %>
                </td>
                <td align="right" style="padding-right: 3px" id="tdDrugTotalPayer" runat="server">
                </td>
                <td align="right" style="padding-right: 3px" id="tdDrugTotalPatient" runat="server">
                </td>
                <td align="right" style="padding-right: 3px" id="tdDrugTotal" runat="server">
                </td>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                <div style="padding: 3px">
                    <div>
                        <b>
                            <%#: Eval("ItemName1")%></b></div>
                    <div>
                        <%#: Eval("ParamedicName")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: center">
                    <div>
                        <%#: Eval("ChargeClassName")%></div>
                </div>
            </td>
            <%--<td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("Tariff", "{0:N}")%></div>
                </div>
            </td>--%>
            <td>
                <div style="padding: 3px; text-align: center;">
                    <div>
                        <%#: Eval("ChargedQuantity")%></div>
                </div>
            </td>
            <%--<td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("GrossLineAmount", "{0:N}")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("CITOAmount", "{0:N}")%></div>
                </div>
            </td>
            <td style="display: none">
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("ComplicationAmount", "{0:N}")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("DiscountAmount", "{0:N}")%></div>
                </div>
            </td>--%>
            <td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("PayerAmount", "{0:N}")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("PatientAmount", "{0:N}")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("LineAmount", "{0:N}")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: right;">
                    <div>
                        <%#: Eval("CreatedByFullName")%></div>
                    <div>
                        <%#: Eval("CreatedDateInString")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: center;">
                    <asp:CheckBox ID="chkIsVerified" runat="server" Checked='<%# Eval("IsVerified")%>'
                        Enabled="false" />
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: center;">
                    <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsReviewed")%>'
                        Enabled="false" />
                </div>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
