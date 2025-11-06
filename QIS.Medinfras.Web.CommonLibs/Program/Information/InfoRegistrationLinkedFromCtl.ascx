<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoRegistrationLinkedFromCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoRegistrationLinkedFromCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 400px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnRegistrationIDCtl" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationNoCtl" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="width: 50%">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Linked To Registration")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLinkedToRegistrationNo" Width="100%" runat="server" ReadOnly="true"
                                Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div style="padding: 5px; max-height: 400px;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewInfoRegistrationLinkedFrom" runat="server" Width="100%"
                        ClientInstanceName="cbpViewInfoRegistrationLinkedFrom" ShowLoadingPanel="false"
                        OnCallback="cbpViewInfoRegistrationLinkedFrom_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { oncbpViewInfoRegistrationLinkedFromEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView ID="lvwView" runat="server">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdSelected notAllowSelect grdLinkedFromReg"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 200px">
                                                        <%= GetLabel("Registrasi")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%= GetLabel("SEP")%>
                                                    </th>
                                                    <th>
                                                        <%= GetLabel("Unit Pelayanan")%>
                                                    </th>
                                                    <th>
                                                        <%= GetLabel("Penjamin Bayar")%>
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("Tidak ada informasi registrasi asal dari registrasi ini") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdSelected notAllowSelect grdLinkedFromReg"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 200px">
                                                        <%= GetLabel("Registrasi")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%= GetLabel("SEP")%>
                                                    </th>
                                                    <th>
                                                        <%= GetLabel("Unit Pelayanan")%>
                                                    </th>
                                                    <th>
                                                        <%= GetLabel("Penjamin Bayar")%>
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trLinkedFromReg">
                                                <td>
                                                    <div style="font-weight: bold">
                                                        <%#: Eval("RegistrationNo")%>
                                                    </div>
                                                    <div style="font-size: small">
                                                        <%#: Eval("cfRegistrationDateInString")%><%=GetLabel(" ")%><%#: Eval("RegistrationTime")%>
                                                    </div>
                                                    <div style="font-size: small; font-style: italic">
                                                        <%#: Eval("RegistrationStatus")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="font-weight: bold">
                                                        <%#: Eval("NoSEP")%>
                                                    </div>
                                                    <div style="font-size: small">
                                                        <%#: Eval("cfTanggalSEPInString")%><%=GetLabel(" ")%><%#: Eval("JamSEP")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="font-size: small">
                                                        <%#: Eval("DepartmentID")%>
                                                    </div>
                                                    <div style="font-size: small; font-weight: bold">
                                                        <%#: Eval("ServiceUnitName")%>
                                                    </div>
                                                    <div style="font-size: small; font-style: italic">
                                                        <%#: Eval("ParamedicName")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="font-size: small">
                                                        <%#: Eval("BusinessPartnerName")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="font-size: small">
                                                        <%#: Eval("MedicalNo")%>
                                                    </div>
                                                    <div style="font-size: small; font-weight: bold">
                                                        <%#: Eval("PatientName")%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
