<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="IntegratedNotesSummaryList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.IntegratedNotesSummaryList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        .needConfirmation {background-color: Silver !important;}
    </style>
    <script type="text/javascript" id="dxss_integrationnotessummarylist">
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnVisitLinkedID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table style="margin-top:10px; margin-bottom:10px">
        <tr>
            <td class="tdLabel" style="width:150px">
                <label>
                    <%=GetLabel("Display Option")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                    Width="300px">
                    <ClientSideEvents ValueChanged="function() { cbpView.PerformCallback('refresh'); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ $('#containerImgLoadingView').hide(); }"/>
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage" Style="height: 450px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
                            <Columns>
                                <asp:BoundField DataField="NoteDate" HeaderText="Date" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Dokter" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
                                    <ItemTemplate>
                                        <div style="width:100%; overflow-x:auto">
                                            <%# Eval("PhysicianNote") != null ? Eval("PhysicianNote").ToString().Replace("\n","<br />") : ""%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Perawat" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
                                    <ItemTemplate>
                                        <div style="width:100%; overflow-x:auto" id= "divNurseNote" runat="server">
                                            <%# Eval("NursingNote") != null ? Eval("NursingNote").ToString().Replace("\n","<br />") : ""%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PPA Lainnya" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left"  >
                                    <ItemTemplate>
                                        <div style="width:100%; overflow-x:auto" id= "divOtherNote" runat="server">
                                            <%# Eval("OtherNote") != null ? Eval("OtherNote").ToString().Replace("\n", "<br />") : ""%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan terintegrasi untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>
