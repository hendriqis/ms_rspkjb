<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="OutpatientIntegrationNotes.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.OutpatientIntegrationNotes" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region Detail Tab
            $('#ulDetail li').click(function () {
                $('#ulDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            //#endregion
        });

        //region tab detail
        function onCbpViewDetailEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=grdViewDetail.ClientID %> tr:eq(1)').click();
            }
            else
                $('#<%=grdViewDetail.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <div style="position: relative; padding-top: 10px">
        <table style="width: 100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div class="containerUlTabPage" style="margin-bottom: 3px;">
                        <ul class="ulTabPage" id="ulDetail">
                            <li class="selected" contentid="panCppt">
                                <%=GetLabel("Catatan Terintegarasi")%></li>
                            <li contentid="panVisitHistory">
                                <%=GetLabel("Catatan Kunjungan Rawat Jalan")%></li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr style="height: 550px">
                <td valign="top">
                    <div class="containerOrderDt" id="panCppt" style="position: relative;">
                        <table style="width: 100%; padding-bottom: 10px" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Unit Pelayanan ") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                                        Width="400px">
                                        <ClientSideEvents ValueChanged="function() { cbpView.PerformCallback('refresh'); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                EndCallback="function(s,e){ $('#containerImgLoadingView').hide(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage" Style="height: 550px">
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
                                                <asp:TemplateField HeaderText="Catatan Per Unit" HeaderStyle-HorizontalAlign="Left"
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
                    </div>
                    <div class="containerOrderDt" id="panVisitHistory" style='display: none'>
                        <table style="width: 100%; padding-bottom: 10px" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Unit Pelayanan ") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDisplayHistory" ClientInstanceName="cboDisplayHistory" runat="server"
                                        Width="400px">
                                        <ClientSideEvents ValueChanged="function() { cbpViewDetail.PerformCallback('refresh'); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                        <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                            ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                EndCallback="function(s,e){ onCbpViewDetailEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 510px">
                                        <asp:GridView ID="grdViewDetail" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdViewDetail_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 60%" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <%=GetLabel("Tanggal Kunjungan") %></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div>
                                                                        <%#: Eval("cfVisitDate")%><br>
                                                                        <%#: Eval("RegistrationNo")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 60%" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <%=GetLabel("Diagnosa") %></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div style="font-style: italic">
                                                                        <asp:Repeater ID="rptDiagnose" runat="server">
                                                                            <ItemTemplate>
                                                                                <div style="padding-left: 5px;">
                                                                                    <b>Dokter :
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "ParamedicName") %></b><br>
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "DiagnoseType") %>
                                                                                    :
                                                                                    <b><%#: DataBinder.Eval(Container.DataItem, "DiagnosisText") %></b>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <br style="clear: both" />
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 60%" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <%=GetLabel("Pemeriksaan Fisik") %></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div>
                                                                        <asp:Repeater ID="rptVitalSignHd" OnItemDataBound="rptVitalSignHd_ItemDataBound"
                                                                            runat="server">
                                                                            <HeaderTemplate>
                                                                                <ul>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <li><span>
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "ObservationDateInString") %>
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "ObservationTime") %>
                                                                                    <br />
                                                                                    <b><%#: Eval("ParamedicName")%></b></span>
                                                                                    <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                                        <ItemTemplate>
                                                                                            <div style="padding-left: 5px; font-style: italic">
                                                                                                <strong>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %>
                                                                                                    : </strong>&nbsp;
                                                                                                <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <br style="clear: both" />
                                                                                        </FooterTemplate>
                                                                                    </asp:Repeater>
                                                                                </li>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </ul>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ChiefComplaintText" HeaderText="Data Pemeriksaan Klinis"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 60%" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <%=GetLabel("Data Pemeriksaan Penunjang") %></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div style="font-style: italic">
                                                                        <asp:Repeater ID="rptTestOrderDt" runat="server">
                                                                            <ItemTemplate>
                                                                                <div style="padding-left: 5px;">
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <br style="clear: both" />
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 60%" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <%=GetLabel("Terapi") %></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div style="font-style: italic">
                                                                        <asp:Repeater ID="rptPrescription" runat="server">
                                                                            <ItemTemplate>
                                                                                <div style="padding-left: 5px;">
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <br style="clear: both" />
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div>
                                                    <div>
                                                        <%=GetLabel("Belum ada catatan kunjungan") %></div>
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
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
