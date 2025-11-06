<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="IntegratedNotes.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.IntegratedNotes" %>

<%@ Register Src="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientSOAPToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_integrationnotes">
        function onBeforeRightPanelPrint(code, filter, errMessage) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var filterExpression = 'VisitID = ' + visitID + ' AND IsDeleted = 0';
            Methods.getObject('GetvPatientVisitNoteList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnID.ClientID %>').val('1');
                }
                else {
                    $('#<%=hdnID.ClientID %>').val('0');
                }
            });
            var id = $('#<%=hdnID.ClientID %>').val();
            if (id == '' || id == '0') {
                errMessage.text = 'Pasien tidak memiliki Catatan Integrasi';
                return false;
            }
            else {
                filter.text = 'VisitID = ' + visitID;
                return true;
            }
        }
    </script>
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table style="margin-top:15px;">
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Display")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                    Width="235px">
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
                                <asp:TemplateField HeaderText="Catatan Dokter" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
                                    <ItemTemplate>
                                        <div>
                                            <%# Eval("PhysicianNote") != null ? Eval("PhysicianNote").ToString().Replace("\n","<br />") : ""%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Catatan Perawat" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
                                    <ItemTemplate>
                                        <div>
                                            <%# Eval("NursingNote") != null ? Eval("NursingNote").ToString().Replace("\n","<br />") : ""%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Catatan Lain" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <%# Eval("OtherNote") != null ? Eval("OtherNote").ToString().Replace("\n", "<br />") : ""%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan perawat untuk pasien ini") %>
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
