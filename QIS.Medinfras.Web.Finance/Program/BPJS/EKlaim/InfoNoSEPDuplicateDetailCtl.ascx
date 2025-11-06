<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoNoSEPDuplicateDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InfoNoSEPDuplicateDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_InfoNoSEPDuplicateDetailCtl">

</script>
<div id="containerPopup">
    <input type="hidden" id="hdnNoSEPCtl" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td align="left">
                <table>
                    <colgroup>
                        <col style="width: 140px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <%=GetLabel("SEP No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSEPNo" Width="170px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
                <div style="height: 350px; overflow-y: auto; overflow-x: hidden">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Tanggal SEP")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Tanggal Registrasi")%>
                                                    </th>
                                                    <th style="width: 170px">
                                                        <%=GetLabel("No Registrasi")%>
                                                    </th>
                                                    <th style="width: 300px">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Informasi Visit")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Tanggal SEP")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Tanggal Registrasi")%>
                                                    </th>
                                                    <th style="width: 170px">
                                                        <%=GetLabel("No Registrasi")%>
                                                    </th>
                                                    <th style="width: 300px">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Informasi Visit")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <label style="font-size: small;">
                                                        <%#: Eval("cfTanggalSEPInString")%></label>
                                                </td>
                                                <td align="center">
                                                    <label style="font-size: small;">
                                                        <%#: Eval("cfRegistrationDateInString")%></label>
                                                </td>
                                                <td>
                                                    <div>
                                                        <label style="font-size: small;">
                                                            <b>
                                                                <%#: Eval("RegistrationNo") %></b></label>
                                                    </div>
                                                    <div>
                                                        <label style="font-size: small;">
                                                            <i>
                                                                <%#: Eval("RegistrationStatus") %></i></label>
                                                    </div>
                                                    <div>
                                                        <hr style="margin: 0 0 0 0;" />
                                                    </div>
                                                    <div style='<%# Eval("LinkedRegistrationNo").ToString() == "" ? "display:none": "" %>'>
                                                        <label style="font-size: x-small;">
                                                            <%=GetLabel("Linked = ")%><%#: Eval("LinkedRegistrationNo") %></label>
                                                    </div>
                                                    <div style='<%# Eval("LinkedToRegistrationNo").ToString() == "" ? "display:none": "" %>'>
                                                        <label style="font-size: x-small;">
                                                            <%=GetLabel("LinkedTo = ")%><%#: Eval("LinkedToRegistrationNo") %></label>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <label style="font-size: x-small; font-style: italic">
                                                            <%=GetLabel("No.RM = ")%></label>
                                                        <label style="font-size: small">
                                                            <%#: Eval("MedicalNo") %></label>
                                                    </div>
                                                    <div>
                                                        <label style="font-size: small">
                                                            <b>
                                                                <%#: Eval("PatientName") %></b></label>
                                                    </div>
                                                    <div>
                                                        <hr style="margin: 0 0 0 0;" />
                                                    </div>
                                                    <div>
                                                        <label style="font-size: x-small; font-style: italic">
                                                            <%=GetLabel("No.Peserta = ")%></label>
                                                        <label style="font-size: small">
                                                            <%#: Eval("NoPeserta") %></label>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <label style="font-size: small">
                                                            <i>
                                                                <%#: Eval("DepartmentID") %></i></label>
                                                    </div>
                                                    <div>
                                                        <label style="font-size: small">
                                                            <b>
                                                                <%#: Eval("ServiceUnitName") %></b></label>
                                                    </div>
                                                    <div>
                                                        <label style="font-size: small">
                                                            <i>
                                                                <%#: Eval("ParamedicName") %></i></label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging" style="display: none">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
