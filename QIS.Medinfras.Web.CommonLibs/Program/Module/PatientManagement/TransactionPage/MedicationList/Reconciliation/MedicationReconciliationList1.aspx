<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" CodeBehind="MedicationReconciliationList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationReconciliationList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomLeftButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientallergylist">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnItemID.ClientID %>').val($(this).find('.itemID').html());
                $('#<%=hdnItemName.ClientID %>').val($(this).find('.drugName').html());
                $('#<%=hdnSignaInfo.ClientID %>').val($(this).find('.signaInfo').html());
                $('#<%=hdnStartDate.ClientID %>').val($(this).find('.logDate').html());
                $('#<%=hdnDuration.ClientID %>').val($(this).find('.dosingDuration').html());
                $('#<%=hdnIsContinueToInpatient.ClientID %>').val($(this).find('.isContinueToInpatient').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
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

        function onValidateBeforeLoadRightPanelContent(code, errMessage) {
            if (code == 'medicationSchedule') {
                if ($('#<%:hdnIsContinueToInpatient.ClientID %>').val() == "False") {
                    errMessage.text = "Proses Jadwal Pemberian Obat hanya berlaku untuk obat yang diteruskan di Rawat Inap.";
                    return false;
                }
                return true;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'medicationSchedule') {
                if ($('#<%:hdnIsContinueToInpatient.ClientID %>').val() == "True") {
                    var param = $('#<%:hdnID.ClientID %>').val() + '|' + $('#<%:hdnMedicalNo.ClientID %>').val() + '|' + $('#<%:hdnPatientName.ClientID %>').val() + '|' + $('#<%=hdnItemID.ClientID %>').val() + '|' + $('#<%=hdnItemName.ClientID %>').val() + '|' + $('#<%=hdnSignaInfo.ClientID %>').val() + '|' + $('#<%=hdnStartDate.ClientID %>').val() + '|' + +$('#<%=hdnDuration.ClientID %>').val();
                    return param;
                }
            }
        }
    </script>

    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPatientName" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnItemName" runat="server" />
    <input type="hidden" value="" id="hdnSignaInfo" runat="server" />
    <input type="hidden" value="" id="hdnStartDate" runat="server" />
    <input type="hidden" value="" id="hdnDuration" runat="server" />
    <input type="hidden" value="" id="hdnIsContinueToInpatient" runat="server" />

    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="120px" HeaderStyle-CssClass="logDate" ItemStyle-CssClass="logDate" />
                                <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="itemID hiddenColumn" ItemStyle-CssClass="itemID hiddenColumn" />
                                <asp:BoundField DataField="cfSignaRule" HeaderStyle-CssClass="signaInfo hiddenColumn" ItemStyle-CssClass="signaInfo hiddenColumn" />
                                <asp:BoundField DataField="DosingDuration" HeaderStyle-CssClass="dosingDuration hiddenColumn" ItemStyle-CssClass="dosingDuration hiddenColumn" />
                                <asp:BoundField DataField="IsContinueInpatientMedication" HeaderStyle-CssClass="isContinueToInpatient hiddenColumn" ItemStyle-CssClass="isContinueToInpatient hiddenColumn" />
                                <asp:BoundField DataField="DrugName" HeaderStyle-CssClass="drugName" ItemStyle-CssClass="drugName" HeaderText="Nama Obat" HeaderStyle-Width="230px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="GenericName" HeaderText="Nama Generik" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        Aturan Pemberian
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#Eval("cfSignaRule")%>
                                        <br />
                                        <%#Eval("MedicationAdministration")%>                                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                    <HeaderTemplate>
                                        Pemberian Terakhir
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#Eval("cfLastTakenDate")%>, <%#Eval("LastTakenTime")%>                                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfIsContinueInpatientMedication" HeaderText="Dilanjutkan di Rawat Inap" HeaderStyle-Width="120px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        Keterangan
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="height: auto; max-height:150px; overflow-y: auto;">
                                            <%#Eval("Remarks").ToString().Replace("\n","<br />")%><br />
                                        </div>                                                
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan rekonsiliasi obat untuk pasien ini")%>
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
</asp:Content>
