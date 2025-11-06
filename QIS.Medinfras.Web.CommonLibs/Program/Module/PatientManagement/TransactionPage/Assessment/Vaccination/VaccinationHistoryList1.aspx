<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" CodeBehind="VaccinationHistoryList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VaccinationHistoryList1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomLeftButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_vaccinHistoryList">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').live('click', function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=grdVaccinTypeList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVaccinTypeList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVaccinationTypeID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnVaccinationTypeName.ClientID %>').val($(this).find('.vaccinationTypeName').html());
                var isCovid19 = $(this).find('.isCovid19').html();
                if (isCovid19 == "False")
                    $('#<%=hdnIsCovid19.ClientID %>').val("0");
                else
                    $('#<%=hdnIsCovid19.ClientID %>').val("1");
                cbpView.PerformCallback('refresh');
            });
            $('#<%=grdVaccinTypeList.ClientID %> tr:eq(1)').click();
        });

        //#region Paging Header
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingHd"), pageCount, function (page) {
                cbpVaccinTypeList.PerformCallback('changepage|' + page);
            });
        });

        function oncbpVaccinTypeListEndCallback(s) {
            $('#containerHdImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdVaccinTypeList.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingHd"), pageCount, function (page) {
                    cbpVaccinTypeList.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdVaccinTypeList.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging
        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

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

        //#region Detail Grid Button
        $('.imgAdd.imgLink').die('click');
        $('.imgAdd.imgLink').live('click', function (evt) {
            addRecord();
        });

        $('#lblAddVaccination').die('click');
        $('#lblAddVaccination').live('click', function () {
            addRecord();
        });

        $('.imgEditRecord.imgLink').die('click');
        $('.imgEditRecord.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var mrn = $('#<%=hdnMRN.ClientID %>').val();
            var type = $('#<%=hdnVaccinationTypeID.ClientID %>').val();
            var name = $('#<%=hdnVaccinationTypeName.ClientID %>').val();
            var isCovid19 = $('#<%=hdnIsCovid19.ClientID %>').val();
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/Vaccination/VaccinationHistoryEntryCtl1.ascx");
            var param = recordID + "|" + type + "|" + name + "|" + isCovid19 + "|" + visitID + "|" + mrn;
            openUserControlPopup(url, param, "Data Vaksinasi", 700, 500);
        });
        $('.imgDeleteRecord.imgLink').die('click');
        $('.imgDeleteRecord.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var name = $(this).attr('name');
            var sequenceNo = $(this).attr('sequenceNo');
            var message = "Hapus riwayat vaksinasi " + name + " ke-" + sequenceNo + " untuk pasien ini ?";
            displayConfirmationMessageBox("Riwayat Vaksinasi", message, function (result) {
                if (result) {
                    cbpDelete.PerformCallback(recordID);
                }
            });
        });

        function addRecord() {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var mrn = $('#<%=hdnMRN.ClientID %>').val();
                var type = $('#<%=hdnVaccinationTypeID.ClientID %>').val();
                var name = $('#<%=hdnVaccinationTypeName.ClientID %>').val();
                var isCovid19 = $('#<%=hdnIsCovid19.ClientID %>').val();
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/Vaccination/VaccinationHistoryEntryCtl1.ascx");
                var param = "0|" + type + "|" + name + "|" + isCovid19 + "|" + visitID + "|" + mrn;
                openUserControlPopup(url, param, "Data Vaksinasi", 700, 500);
            }        
        }
        //#endregion

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpDeleteEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Riwayat Vaksinasi', param[1]);
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == "postVaccinationHistoryToIHS") {
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var registrationNo = $('#<%=hdnRegistrationNo.ClientID %>').val();
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var mrn = $('#<%=hdnMRN.ClientID %>').val();
                var param = registrationID + "|" + visitID + "|" + mrn + "|" + registrationNo;
                return param;
            }
            else {
                return $('#<%=hdnMRN.ClientID %>').val(); 
            }

        }
    </script>

    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnRegistrationID" />
    <input type="hidden" runat="server" id="hdnRegistrationNo" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnVaccinationTypeID" value="" />
    <input type="hidden" runat="server" id="hdnVaccinationTypeName" value="" />
    <input type="hidden" runat="server" id="hdnIsCovid19" value="" />

    <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
        <colgroup>
            <col width="300px" />
            <col />
        </colgroup>
        <tr>
            <td style="vertical-align:top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpVaccinTypeList" runat="server" Width="100%" ClientInstanceName="cbpVaccinTypeList"
                        ShowLoadingPanel="false" OnCallback="cbpVaccinTypeList_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                            EndCallback="function(s,e){ oncbpVaccinTypeListEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="panVaccinTypeList" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdVaccinTypeList" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="VaccinationTypeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="IsCovid19" HeaderStyle-CssClass="isCovid19 hiddenColumn" ItemStyle-CssClass="isCovid19 hiddenColumn" />
                                            <asp:BoundField DataField="VaccinationTypeName" HeaderStyle-CssClass="vaccinationTypeName hiddenColumn" ItemStyle-CssClass="vaccinationTypeName hiddenColumn" />
                                            <asp:BoundField DataField="VaccinationTypeName" HeaderText="Vaksinasi" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada jenis/tipe vaksinasi yang tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerHdImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingHd"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td style="vertical-align:top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <input type="hidden" value="" id="hdnFileString" runat="server" />
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="center" ItemStyle-Width="60px">
                                            <HeaderTemplate>
                                                <img class="imgAdd imgLink" title='<%=GetLabel("+ Data Vaksinasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                    alt="" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <img class="imgEditRecord imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" recordID = "<%#:Eval("ID") %>" name = "<%#:Eval("VaccinationTypeName") %>" sequenceNo = "<%#:Eval("SequenceNo") %>" />
                                                        </td>
                                                        <td style="width: 1px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <img class="imgDeleteRecord imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" recordID = "<%#:Eval("ID") %>" name = "<%#:Eval("VaccinationTypeName") %>" sequenceNo = "<%#:Eval("SequenceNo") %>" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="SequenceNo" HeaderText="Vaksinasi Ke" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="70px" />
                                        <asp:BoundField DataField="cfVaccinationDate" HeaderText="Tanggal Vaksin" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="cfVaccinationItemName" HeaderText="Jenis Vaksin" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="BatchNo" HeaderText="Lot/Batch Number" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="110px" />
                                        <asp:BoundField DataField="VaccinationRoute" HeaderText="Rute Vaksin" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfIsBooster" HeaderText="Booster" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="cfProvider" HeaderText="Vaksinasi dilakukan di" HeaderStyle-Width="230px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Catatan" 
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("cfCreatedDate")%></div>
                                                <div>
                                                    <%#: Eval("CreatedByName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada data riwayat vaksinasi untuk pasien ini") %>
                                        <br />
                                        <span class="lblLink" id="lblAddVaccination">
                                            <%= GetLabel("+ Data Vaksinasi")%></span>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpDelete" runat="server" Width="100%" ClientInstanceName="cbpDelete"
            ShowLoadingPanel="false" OnCallback="cbpDelete_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
