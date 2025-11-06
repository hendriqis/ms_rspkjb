<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="BPJSDocument.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.BPJSDocument" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBPJSDocumentBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnAddDocument" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("New")%></div>
    </li>
    <li id="btnEditDocument" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Edit")%></div>
    </li>
    <li id="btnDeleteDocument" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Delete")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=GetLabel("Upload Document")%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnIsFromMenuInitial" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnBPJSDocumentBack.ClientID %>').live('click', function () {
                showLoadingPanel();
                if ($('#<%=hdnIsFromMenuInitial.ClientID %>').val() == "EK") {
                    document.location = ResolveUrl('~/Program/BPJS/EKlaim/EKlaimEntry.aspx?id=' + $('#<%=hdnRegistrationID.ClientID %>').val());
                } else if ($('#<%=hdnIsFromMenuInitial.ClientID %>').val() == "TC") {
                    var queryDepartment = '';
                    if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'INPATIENT') {
                        queryDepartment = 'INPATIENT';
                    }
                    else {
                        queryDepartment = 'OUTPATIENT';
                    }
                    document.location = ResolveUrl('~/Program/BPJS/TemporaryClaim/TemporaryClaim.aspx?id=' + queryDepartment);
                }
            });

            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        $('#<%=btnAddDocument.ClientID %>').live('click', function (evt) {
            var url = ResolveUrl('~/Program/BPJS/TemporaryClaim/UploadDocument/NewPatientDocumentCtl.ascx');
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var id = registrationID + "|";
            openUserControlPopup(url, id, 'Upload Document', 700, 300);
        });

        $('#<%=btnEditDocument.ClientID %>').live('click', function (evt) {
            var url = ResolveUrl('~/Program/BPJS/TemporaryClaim/UploadDocument/NewPatientDocumentCtl.ascx');
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var documentID = $('#<%=hdnID.ClientID %>').val();
            var id = registrationID + "|" + documentID;
            openUserControlPopup(url, id, 'Upload Document', 700, 300);
        });

        $('#<%=btnDeleteDocument.ClientID %>').live('click', function (evt) {
            showToastConfirmation("Are you sure want to delete this record?", function (result) {
                if (result) {
                    cbpView.PerformCallback('delete');
                }
            });
        });

        $('.lnkViewDocument a').live('click', function () {
            var fileName = $(this).closest('tr').find('.fileName').html();
            var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
            window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
        });

        $('.lnkSendStreamPdf a').live('click', function () {
            var fileName = $(this).closest('tr').find('.fileName').html();
            cbpView.PerformCallback('send|' + fileName);
        });

        $('#lnkSendEklaimData').live('click', function () {
            var evID = $(this).attr("data-val");
            if (evID != "") {
                $('#<%=hdnID.ClientID %>').val(evID);

                var url = ResolveUrl('~/Program/BPJS/TemporaryClaim/UploadDocument/EklaimPatientDocumentCtl.ascx');
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var documentID = $('#<%=hdnID.ClientID %>').val();
                var id = registrationID + "|" + documentID;
                openUserControlPopup(url, id, 'Upload Document', 700, 300);

            }
        });

        function onRefreshControl() {
            cbpView.PerformCallback();
        }

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

        function onCbpOpenDocumentEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[1] != 'success') {
                showToast('Open Document Failed', 'Error Message : ' + param[2]);
            }
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });
        //#endregion

        function onBeforeLoadRightPanelContent(code) {
            var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (code == 'uploadDocument'
                    || code == 'infoTransactionParameter' || code == 'infoTransactionDetailParameter' || code == 'infoDiagnosticResult') {
                if (regID != "" && regID != null) {
                    return regID;
                } else {
                    displayErrorMessageBox("Silahkan Coba Lagi", "Pilih nomor registrasi terlebih dahulu.");
                    return false;
                }
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var regID = $('#<%:hdnRegistrationID.ClientID %>').val();

            if (code != "") {
                if (regID != "" && regID != null) {
                    filterExpression.text = "RegistrationID = " + regID;
                    return true;
                } else {
                    displayErrorMessageBox("Silahkan Coba Lagi", "Pilih nomor registrasi terlebih dahulu.");
                    errMessage.text = "Belum pilih nomor registrasi.";
                    return false;
                }
            } else {
                errMessage.text = "No data to display.";
                return false;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnCboDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPatientDocumentUrl" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToGateway" runat="server" />
    <input type="hidden" value="" id="hdnProviderGatewayService" runat="server" />
 
    <div style="height: 435px; overflow-y: auto;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn"
                                    ItemStyle-CssClass="hiddenColumn fileName" />
                                <asp:BoundField DataField="CreatedByName" HeaderText="Created By" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfCreatedDateInString" HeaderText="Created Date" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:HyperLinkField HeaderText=" " Text="Open" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText=" " Text="Send" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkSendStreamPdf" HeaderStyle-Width="100px" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                                    <HeaderTemplate></HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <input type="button" value="Kirim Ke Eklaim" class="lnkSendEklaimData w3-aqua   btn-w3" id="lnkSendEklaimData" data-val="<%#: Eval("ID")%>" />
                                         </div>
                                       </ItemTemplate>
                                 </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No patient document record available in this episode") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpOpenDocument" runat="server" Width="100%" ClientInstanceName="cbpOpenDocument"
            ShowLoadingPanel="false" OnCallback="cbpOpenDocument_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpOpenDocumentEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
