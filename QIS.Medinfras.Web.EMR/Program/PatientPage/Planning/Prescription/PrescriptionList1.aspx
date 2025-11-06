<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList2.master" AutoEventWireup="true" EnableEventValidation = "false"
    CodeBehind="PrescriptionList1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PrescriptionList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table>
        <tr>
            <td>
                <%=GetLabel("Model Tampilan Item") %>
            </td>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server">
                    <asp:ListItem Text="All" Value="0" Selected="True" />
                    <asp:ListItem Text="Diambil oleh Pasien" Value="1" />
                    <asp:ListItem Text="Tidak diambil oleh Pasien" Value="2" />
                    <asp:ListItem Text="Tidak diproses atau dibatalkan" Value="3" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $btnPropose = null;
        $btnReopen = null;
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnPrescriptionOrderNo.ClientID %>').val($(this).find('.prescriptionOrderNo').html());
                    $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
                    $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.isEditable').html());
                    $('#<%=hdnIsShowPPRAIcon.ClientID %>').val($(this).find('.cfIsShowPPRAIcon').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Propose
            $('.btnPropose').die('click');
            $('.btnPropose').live('click', function () {
                $btnPropose = $(this);

                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                var isUsingDrugAlert = $('#<%=hdnIsUsingDrugAlert.ClientID %>').val();
                if (isUsingDrugAlert == "1") {
                    var presNo = $('#<%=hdnPrescriptionOrderNo.ClientID %>').val();

                    DrugAlertService.validateDrugs(presNo, 'Validate', function (resultF1) {
                        var url = ResolveUrl("~/Libs/Program/Information/DrugAlertInformationCtl.ascx");
                        var id = $('#<%=hdnID.ClientID %>').val();

                        var isShowAlertForm = 0;
                        var filterExpressionPresInfo = "PrescriptionOrderID = '" + id + "'";
                        var toFound = "No results found. Absence of interaction result should in no way be interpreted as safe. Clinical judgment should be exercised";
                        Methods.getObject('GetPrescriptionOrderHdInfoList', filterExpressionPresInfo, function (resultInfo) {
                            if (resultInfo != null) {
                                if (!resultInfo.DrugAlertResultInfo1.includes(toFound)) {
                                    isShowAlertForm = 1;
                                }
                                else {
                                    isShowAlertForm = 0;
                                }
                            }
                            else {
                                isShowAlertForm = 0;
                            }
                        });

                        if (isShowAlertForm == 1) {
                            openUserControlPopup(url, id, 'Drug Alert Information', 700, 600);
                        }
                        else {
                            if ($('#<%=hdnIsAllowChangeAnotherDoctorPrescription.ClientID %>').val() == "0") {
                                if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                                    onCustomButtonClick('Propose');
                                    return true;
                                }
                                else {
                                    displayErrorMessageBox('GAGAL', 'Maaf, Order Resep hanya bisa di Send Order oleh user yang membuat order.');
                                    return false;
                                }
                            }
                            else {
                                onCustomButtonClick('Propose');
                            }
                        }
                    });
                }
                else {
                    displayConfirmationMessageBox('SEND ORDER : FARMASI', 'Kirim order resep ke farmasi ?', function (result) {
                        if (result) {
                            if ($('#<%=hdnIsAllowChangeAnotherDoctorPrescription.ClientID %>').val() == "0") {
                                if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                                    onCustomButtonClick('Propose');
                                    $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                                    return true;
                                }
                                else {
                                    displayErrorMessageBox('GAGAL', 'Maaf, Order Resep hanya bisa di Send Order oleh user yang membuat order.');
                                    return false;
                                }
                            }
                            else {
                                onCustomButtonClick('Propose');
                                $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                            }
                        }
                    });
                }
            });
            //#endregion

            //#region Reopen
            $('.btnReopen').die('click');
            $('.btnReopen').live('click', function () {
                $btnReopen = $(this);

                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                displayConfirmationMessageBox('REOPEN ORDER : FARMASI', 'Reopen order resep ke farmasi ?', function (result) {
                    if (result) {
                        if ($('#<%=hdnIsAllowChangeAnotherDoctorPrescription.ClientID %>').val() == "0") {
                            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                                onCustomButtonClick('ReOpen');
                                $('#<%:hdnIsOutstandingOrder.ClientID %>').val("1");
                                return true;
                            }
                            else {
                                displayErrorMessageBox('GAGAL', 'Maaf, Order Resep hanya bisa di Re-Open oleh user yang melakukan order.');
                                return false;
                            }
                        }
                        else {
                            onCustomButtonClick('ReOpen');
                            $('#<%:hdnIsOutstandingOrder.ClientID %>').val("1");
                        }
                    }
                });
            });
            //#endregion

            $('.imgEdit.imgLink').die('click');
            $('.imgEdit.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                if ($('#<%=hdnIsAllowChangeAnotherDoctorPrescription.ClientID %>').val() == "0") {
                    if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                        cbpMPListProcess.PerformCallback('edit');
                        return true;
                    }
                    else {
                        displayErrorMessageBox('GAGAL', 'Maaf, Order Resep hanya bisa dilakukan perubahan oleh user yang melakukan order.');
                        return false;
                    }
                }
                else {
                    cbpMPListProcess.PerformCallback('edit');
                }
            });

            $('.imgDelete.imgLink').die('click');
            $('.imgDelete.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                displayConfirmationMessageBox('ORDER FARMASI', 'Lanjutkan proses hapus order farmasi ?', function (result) {
                    if (result) {
                        if ($('#<%=hdnIsAllowChangeAnotherDoctorPrescription.ClientID %>').val() == "0") {
                            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                                cbpMPListProcess.PerformCallback('delete');
                                return true;
                            }
                            else {
                                displayErrorMessageBox('GAGAL', 'Maaf, Order Resep hanya bisa dihapus oleh user yang melakukan order.');
                                return false;
                            }
                        }
                        else {
                            cbpMPListProcess.PerformCallback('delete');
                        }
                    }
                });
            });

            $('.imgViewLog.imgLink').die('click');
            $('.imgViewLog.imgLink').click(function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Controls/ViewNotes/ViewPrescriptionOrderChangesLogCtl.ascx");
                openUserControlPopup(url, id, 'Catatan Perubahan Order', 900, 500);
            });

            $('.imgViewPPRA.imgLink').die('click');
            $('.imgViewPPRA.imgLink').click(function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Controls/EMR/Information/CPOE/ViewPPRAFormCtl.ascx");
                openUserControlPopup(url, id, 'Program Pengendalian Resistensi Antimikroba (PPRA)', 800, 600);
            });

            $('#<%=ddlViewType.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            });
        });

        $('#<%=btnRefresh.ClientID %>').click(function (evt) {
            onRefreshControl();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess() {
            onRefreshControl();
        }

        function onRefreshControlDeleted() {
            cbpView.PerformCallback('refresh');        
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        function SaveFromDrugAlertPopUp() {
            onCustomButtonClick('Propose');            
            $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
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
                if (pageCount > 0) {
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                }
                else {
                    $('#<%=hdnID.ClientID %>').val('');
                    cbpViewDt.PerformCallback('refresh');
                }
                
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeBackToListPage() {
            if ($('#<%:hdnIsOutstandingOrder.ClientID %>').val() == "1") {
                var line1 = "Masih ada order resep yang belum dikirim ke Farmasi, Silahkan dikirim order terlebih dahulu dengan meng-klik tombol <b>Send Order</b>";
                var line2 = "<br />Jika masih mengalami kendala, silahkan klik tombol <b>Refresh</b>";
                var messageBody = line1 + line2;
                displayMessageBox('ORDER RESEP', messageBody);
            }
            else {
                backToPatientList();
            }
        }

        function onValidateBeforeLoadRightPanelContent(code, errMessage) {
            if (code == 'changeOrderStatus') {
                if ($('#<%:hdnIsEditable.ClientID %>').val() == "False") {
                    errMessage.text = "Proses tidak dapat dilakukan karena order sudah diproses.";
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var prescriptionOrderID = $('#<%=hdnID.ClientID %>').val();
            if (prescriptionOrderID == '' || prescriptionOrderID == '0') {
                errMessage.text = 'Transaksi Resep harap diselesaikan terlebih dahulu.';
                return false;
            }
            else {
                if (code == 'PH-00028' || code == 'PH-00012' || code == 'PH-00044') {
                    filterExpression.text = prescriptionOrderID;
                    return true;
                }
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'changeOrderStatus') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderNo.ClientID %>').val();
                return param;
            }
            else if (code == 'confirmPPRA') {
                var prescriptionOrderID = $('#<%=hdnID.ClientID %>').val();
                return prescriptionOrderID;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionOrderNo" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsOutstandingOrder" runat="server" value="0" />
    <input type="hidden" id="hdnIsUsingDrugAlert" runat="server" value="0" />
    <input type="hidden" id="hdnIsAllowChangeAnotherDoctorPrescription" runat="server" value="0" />
    <input type="hidden" id="hdnIsShowPPRAIcon" runat="server" value="" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PrescriptionOrderNo" HeaderStyle-CssClass="hiddenColumn prescriptionOrderNo"
                                                ItemStyle-CssClass="hiddenColumn prescriptionOrderNo" />
                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn paramedicID"
                                                ItemStyle-CssClass="hiddenColumn paramedicID" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit imgLink" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                    title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" style="float: left" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete imgLink" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" style="float: left" />
                                                            </td>
                                                            <td>
                                                                <img class="imgViewLog imgLink" <%# Eval("IsPrescriptionOrderHasChangesLog").ToString() == "False" ? "Style='display:none'":"" %>
                                                                    src='<%# ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>' alt=""
                                                                    style="float: left" title="Catatan Perubahan Order" width="32px" height="32px" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgProceed" <%# Eval("IsProceed").ToString() == "False" ? "Style='display:none'":"" %>
                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>' alt="" style="float: left"
                                                                    title="Sudah Diproses" />
                                                            </td>
                                                            <td>
                                                                <img class="imgViewPPRA imgLink blink-icon" <%# Eval("cfIsShowPPRAIcon").ToString() == "False" ? "Style='display:none'":"" %>  src='<%# ResolveUrl("~/Libs/Images/Status/ppra.png") %>'
                                                                    title='Lihat Formulir Program Pengendalian Resistensi Antimikroba (PPRA)' alt="" style="height: 24px; width: 24px; cursor:pointer" />   
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Prescription Date - Time")%>,
                                                        <%=GetLabel("Prescription No.")%></div>
                                                    <div style="width: 250px; float: left">
                                                        <%=GetLabel("Physician")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <%#: Eval("PrescriptionDateInString")%>,
                                                                    <%#: Eval("PrescriptionTime") %>,
                                                                    <%#: Eval("PrescriptionOrderNo")%>
                                                                </div>
                                                                <div style="width: 250px; float: left">
                                                                    <%#: Eval("ParamedicName") %>
                                                                    <b>
                                                                        <%# Eval("IsCreatedBySystem").ToString() == "False" ? "":"(Diorder Farmasi)" %></b>
                                                                </div>
                                                                <div style="width: 250px; float: left; font-size: x-small; font-style: italic">
                                                                    <%#: Eval("cfSendOrderDateInformationInString") %>
                                                                    <%#: Eval("cfChargesProposedInformationInString") %>
                                                                </div>
                                                                <div style="width: 250px; float: left; font-size: small; font-style: italic">
                                                                    <%#: Eval("cfPrescriptionType") %>
                                                                </div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %>>
                                                                <div>
                                                                    <input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND ORDER" style="background-color: Red;
                                                                        color: White; width: 100px;" /></div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsAllowReopen").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %>>
                                                                <div>
                                                                    <input type="button" class="btnReopen w3-btn w3-hover-blue" value="REOPEN" style="background-color: Green;
                                                                        color: White; width: 100px" /></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order resep untuk pasien ini")%>
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
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdViewDt_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                                ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemName">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Drug Name")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div <%# Eval("OrderIsDeleted").ToString() == "True"  || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "Style='color:red;font-style:italic; text-decoration: line-through'":"" %>>
                                                        <span style="font-weight: bold">
                                                            <%#: Eval("cfMedicationName")%></span></div>
                                                    <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                        <%#: Eval("cfCompoundDetail")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Frequency" HeaderText="Frequency" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="DosingFrequency" HeaderText="Timeline" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfNumberOfDosage" HeaderText="Dose" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="DosingUnit" HeaderText="Unit" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Signa")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("cfConsumeMethod3")%></div>
                                                    <div>
                                                        <%#: Eval("Route")%></div>
                                                    <div>
                                                        <%#: Eval("MedicationAdministration")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Start Date")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("StartDateInDatePickerFormat")%><BR><%#: Eval("StartTime")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
<%--                                            <asp:BoundField DataField="StartDateInDatePickerFormat" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />--%>
                                            <asp:BoundField DataField="DispenseQtyInString" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTakenQty" HeaderText="Taken" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Unit" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div>
                                                        <img id="imgHAM" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/ham.png") %>'
                                                            title='High Alert Medication' alt="" style="height: 24px; width: 24px;" /></div>
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAllergyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                        title='<%=GetLabel("Allergy Alert") %>' alt="" />
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAdverseReactionAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                        title='<%=GetLabel("Adverse Reaction") %>' alt="" />
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsDuplicateTheraphyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                        title='<%=GetLabel("Duplicate Theraphy") %>' alt="" />
                                                    <div>
                                                        <img id="imgIsHasRestriction" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/drug_alert.png") %>' 
                                                            title='Drug Restriction' alt="" style ="height:24px; width:24px;" /></div>
                                                    <div>
                                                        <img id="imgPPRA" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/ppra.png") %>'
                                                            title='Termasuk dalam Kategori Program Pengendalian Resistensi Antimikroba (PPRA)' alt="" style="height: 24px; width: 24px;" /></div>    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-CssClass="hiddenColumn isHasRestrictionInformation" HeaderStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="text" id="lblHasRestrictionInformation" runat="server" value="0" style="width: 20px"
                                                        class="lblHasRestrictionInformation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <div>
                                                        <img src='<%# ResolveUrl("~/Libs/Images/Button/plus.png") %>' <%# Eval("IsCreatedFromOrder").ToString() == "False" ? "" : "Style ='display:none'" %>
                                                            title='<%=GetLabel("Drug Add by Pharmacist") %>' alt="" width="10" height="10"
                                                            style="padding: 2px" />
                                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/stop_service.png") %>' <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "Style='cursor:pointer;padding-right:10px'" : "Style ='display:none'" %>
                                                            title='<%#: Eval("cfVoidReson")%>' alt="" width="15" height="15" />
                                                    </div>
                                                    <div <%# Eval("IsCreatedFromOrder").ToString() == "False" ? "" : "Style ='display:none'" %>>
                                                        <i>
                                                            <%=GetLabel("C : ")%></i>
                                                        <%#: Eval("CreatedByUserFullName")%>
                                                    </div>
                                                    <div <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "" : "Style ='display:none'" %>>
                                                        <i>
                                                            <%=GetLabel("U : ")%></i>
                                                        <%#: Eval("LastUpdatedByUserFullName")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order resep untuk pasien ini")%>
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
                            <div id="pagingDt">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
