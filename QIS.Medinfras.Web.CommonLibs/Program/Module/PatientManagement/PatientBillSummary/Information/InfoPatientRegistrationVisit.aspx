<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="InfoPatientRegistrationVisit.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientRegistrationVisit" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 25px">
                                                            <%= GetLabel("Informasi Pasien")%>
                                                        </th>
                                                        <th style="width: 25px">
                                                            <%= GetLabel("Informasi Dokter / Paramedis")%>
                                                        </th>
                                                        <th style="width: 25px">
                                                            <%=GetLabel("Kunjungan")%>
                                                        </th>
                                                        <th style="width: 25px">
                                                            <%=GetLabel("Penjamin Bayar")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="5">
                                                            <%=GetLabel("Tidak ada informasi pendaftaran") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 24%">
                                                            <%= GetLabel("Informasi Pasien")%>
                                                        </th>
                                                        <th style="width: 24%">
                                                            <%= GetLabel("Informasi Dokter / Paramedis")%>
                                                        </th>
                                                        <th style="width: 26%">
                                                            <%=GetLabel("Kunjungan")%>
                                                        </th>
                                                        <th style="width: 26%">
                                                            <%=GetLabel("Penjamin Bayar")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trMutationDetail">
                                                    <td>
                                                        <div style="float: left">
                                                            <b><%#: Eval("RegistrationNo")%></b><br>
                                                            <%#: Eval("PatientName")%> (<i><%#: Eval("MedicalNo")%></i>) <br />
                                                            <br />
                                                            Jenis Peserta :<%#: Eval("JenisPeserta")%><br />
                                                            Kelas Tanggungan : <%#: Eval("KelasTanggungan")%><br />
                                                            Nama PPK : <%#: Eval("NamaPPK")%><br />
                                                            No. Peserta BPJS : <%#: Eval("NoPesertaBPJS")%> <br />
                                                            No. SEP          : <%#: Eval("NoSEP")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="float: left">
                                                            <%#: Eval("ParamedicName")%><br />
                                                            Spesialisasi : <%#: Eval("SpecialtyName")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="float: left">
                                                            Unit Pelayanan   : <%#: Eval("ServiceUnitName")%><br />
                                                            Ruang Perawatan  : <%#: Eval("RoomName")%> <br />
                                                            Tempat Tidur     : <%#: Eval("BedCode")%> <br />
                                                            Tipe Kunjungan   : <%#: Eval("VisitTypeName") %> <br />
                                                            Kelas Permintaan (Pasien Titipan) : <%#: Eval("RequestClassName") %> <br /><br />
                                                            <%#: Eval("cfTextDischargeDateTimeOrPlanDischargeDateTime") %> <br />
                                                            <%#: Eval("cfDischargeConditionText") %> <br />
                                                            <%#: Eval("cfDischargeMethodText") %> <br />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div align="left">
                                                             Nama Instansi : <%#: Eval("BusinessPartnerName")%><br />
                                                             No. Kontrak   : <%#: Eval("ContractNo")%><br />
                                                             Tipe Coverage : <%#: Eval("CoverageTypeName")%><br />
                                                             No. Peserta   : <%#: Eval("NoPesertaInstansi")%><br />
                                                            </div>
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
</asp:Content>
