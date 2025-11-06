<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" 
    CodeBehind="NurseHandsOverList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NurseHandsOverList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4 em">
        <%=HttpUtility.HtmlEncode(GetPageTitle()) %>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnIsConfirmed.ClientID %>').val($(this).find('.isConfirmed').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.isEditable').html());
                $('#<%=hdnFromNurseID.ClientID %>').val($(this).find('.fromNurseID').html());
                $('#<%=hdnFromHealthcareServiceUnitID.ClientID %>').val($(this).find('.fromHealthcareServiceUnitID').html());
                $('#<%=hdnToHealthcareServiceUnitID.ClientID %>').val($(this).find('.toHealthcareServiceUnitID').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Verify
            $('.btnPropose').live('click', function () {
                var $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                showToastConfirmation('Lanjutkan proses serah terima pasien ?', function (result) {
                    if (result) {
                        var selectedID = $('#<%=hdnID.ClientID %>').val();
                        cbpPropose.PerformCallback(selectedID);
                    }
                });

            });
            //#endregion

            $('.lnkView a').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();

                var param = id;
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientTransfer/ViewNurseHandsOverEntryCtl.ascx");
                openUserControlPopup(url, param, 'Patient Handover', 800, 600);
            });
        });

        function onAfterSaveRecordPatientPageEntry() {
            onRefreshControl();
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onBeforeEditRecord(entity, errMessage) {
            if (entity.IsConfirmed == true) {
                errMessage.text = 'Maaf Data tidak bisa diubah, Data Serah Terima Pasien sudah diproses untuk dikonfirmasi.';
                return false;
            }
            return true;
        }

        function onBeforeDeleteRecord(entity, errMessage) {
            if (entity.IsConfirmed == true) {
                errMessage.text = 'Maaf Data tidak bisa dihapus, Data Serah Terima Pasien sudah diproses untuk dikonfirmasi.';
                return false;
            }
            return true;
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var id = $('#<%:hdnID.ClientID %>').val();
            if (id == '') {
                errMessage.text = 'Belum ada record yang dipilih!';
                return false;
            }
            else {
                if (code == 'PM-00146' || code == 'PM-00147') {
                    filterExpression.text = "ID = "+ id;
                    return true;
                }
            }
        }

        function onCbpProposeEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] != 'success') {
                showToast('PROPOSE : FAILED', 'Error Message : ' + param[1]);
            }
            else {
                onRefreshControl();
            }
        }
    </script>
    
    <input type="hidden" id="hdnID" runat="server" value="" /> 
    <input type="hidden" id="hdnFromNurseID" runat="server" value = "0" />
    <input type="hidden" id="hdnIsConfirmed" runat="server" value="0" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="0" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnFromHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnToHealthcareServiceUnitID" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound" >
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="IsConfirmed" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="hiddenColumn isConfirmed" />
                                <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn isEditable" />
                                <asp:BoundField DataField="FromNurseID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fromNurseID" />
                                <asp:BoundField DataField="FromHealthcareServiceUnitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fromHealthcareServiceUnitID" />
                                <asp:BoundField DataField="ToHealthcareServiceUnitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn toHealthcareServiceUnitID" />
                                <asp:BoundField DataField="cfTransferDate" HeaderText="Tanggal" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:BoundField DataField="TransferTime" HeaderText="Jam" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PatientNurseTransferType" HeaderText="Jenis Transfer" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="FromNurseName" HeaderText="Dari Perawat" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ToNurseName" HeaderText="Ke Perawat" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfConfirmedDateTime" HeaderText="Tanggal Konfirmasi" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cfIsConfirmed" HeaderText="Dikonfirmasi" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <div <%# Eval("GCPatientNurseTransferStatus") != "X078^01" ? "Style='display:none'":"" %>>
                                            <input type="button" id="btnPropose" runat="server" class="btnPropose w3-btn w3-hover-blue" value="Propose"
                                                style="width: 100px; background-color: Red; color: White;" />
                                        </div>                                                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField HeaderText=" " Text="View" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkView" HeaderStyle-Width="60px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada riwayat serah terima pasien antar perawat")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPropose" runat="server" Width="100%" ClientInstanceName="cbpPropose"
        ShowLoadingPanel="false" OnCallback="cbpPropose_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProposeEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
