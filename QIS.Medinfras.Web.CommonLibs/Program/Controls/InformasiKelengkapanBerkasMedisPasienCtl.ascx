<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformasiKelengkapanBerkasMedisPasienCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InformasiKelengkapanBerkasMedisPasienCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitctl">
    function onCboInfoMedicalFileValueChanged(s) {
        onRefreshGridViewPopup();
    }

    function onCboMedicalFileChanged() {
        onRefreshGridViewPopup();
    }

    function onRefreshGridViewPopup() {
        if (IsValid(null, 'fsPatientListPopup', 'mpPatientList'))
            cbpView.PerformCallback('refresh');
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingInfoMedicalFileView"), pageCount, function (page) {
            cbpInfoMedicalFileView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#pagingInfoMedicalFileView"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion
</script>
<input type="hidden" id="hdnVisitID" runat="server" />
<div class="pageTitle">
    <%=GetLabel("Informasi Kelengkapan Berkas Medis Pasien")%></div>
<fieldset id="fsPatientListPopup">
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
        </colgroup>
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtRegistrationNo" Width="200px" ReadOnly="true" />
            </td>
        </tr>
    </table>
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
        </colgroup>
        <tr id="trMedicalFile" runat="server">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Jenis Analisa Berkas")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboMedicalFile" ClientInstanceName="cboMedicalFile" runat="server"
                    Width="200px">
                    <ClientSideEvents ValueChanged="function(s,e) { onCboMedicalFileChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
</fieldset>
<div style="position: relative;">
    <tr>
        <td colspan="2">
            <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="FormID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="FormCode" HeaderText="Kode Berkas" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="FormName" HeaderStyle-CssClass="formName" ItemStyle-CssClass="formName"
                                            HeaderText="Nama Berkas" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderText="Ada">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsExist" CssClass="chkIsExist" runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderText="Tanggal Berkas">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFormDate" Width="120px" runat="server" CssClass="txtFormDate datepicker"
                                                    ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderText="Jam Berkas">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFormTime" runat="server" Width="60px" CssClass="txtFormTime"
                                                    Style="text-align: center" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderText="Lengkap">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsCompleted" CssClass="chkIsCompleted" runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderText="Jenis Catatan">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="cboMRStatusNotes" CssClass="cboNotesType" Width="100%"
                                                    ReadOnly="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderText="Catatan Tambahan">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" CssClass="txtRemarks" Width="100%" runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingInfoMedicalFileView">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</div>
