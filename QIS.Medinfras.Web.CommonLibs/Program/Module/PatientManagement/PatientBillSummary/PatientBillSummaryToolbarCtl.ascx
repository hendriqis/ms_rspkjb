<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryToolbarCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryToolbarCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<div class="pageTitle" style="height: 43px; margin-top: 5px;">
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnIsCloseRegistrationCheckImagingOutstanding" value=""
        runat="server" />
    <input type="hidden" id="hdnModuleIDToolbar" value="" runat="server" />
    <input type="hidden" id="hdnUrlBack" value="" runat="server" />
    <input type="hidden" id="hdnLinkedRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnIsAllowClosePatientWithoutBill" value="" runat="server" />
    <input type="hidden" id="hdnIsCheckResultTest" value="" runat="server" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnLaboratoryHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnImagingServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnImagingHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnIsHasOutstandingOrder" value="" runat="server" />
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>'
        alt="" style="float: left; margin-top: 3px;" title="<%=GetLabel("Back")%>" />
    <div style="margin-right: 20px">
        <div id="divCloseRegistration" runat="server" style="float: right; margin-top: 3px;" class="CloseRegistration">
            <img class="imgStatus button hvr-pulse-grow" id="imgBillSummaryToolbarCloseRegistration" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_closed.png")%>'
                alt="" title="<%=GetLabel("Tutup Pendaftaran")%>" />
        </div>
        <div id="divVisitNote" runat="server" style="float: right; margin-top: 3px;">
            <img class="imgStatus button hvr-pulse-grow" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                alt="" title="<%=GetLabel("Catatan Kunjungan Pasien")%>" />
        </div>
        <div id="divUnlockTransaction" runat="server" style="float: right; margin-top: 3px;">
            <img id="imgUnlock" class="imgStatus button hvr-pulse-grow" src='<%= ResolveUrl("~/Libs/Images/Toolbar/unlockdown.png")%>'
                title="<%=GetLabel("Unlock Transaction")%>" alt="" />
        </div>
        <div id="divLockTransaction" runat="server" style="float: right; margin-top: 3px;">
            <img id="imgLock" class="imgStatus button hvr-pulse-grow" src='<%= ResolveUrl("~/Libs/Images/Toolbar/lockdown.png")%>'
                title="<%=GetLabel("Lock Transaction")%>" alt="" />
        </div>
    </div>
    <div class="divNavigationPane">
    <asp:Repeater ID="rptHeader" runat="server" OnItemDataBound="rptHeader_ItemDataBound">
        <HeaderTemplate>
            <ul id="ulPatientPageHeader" class="ulNavigationPane">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server" url='<%#: Eval("MenuUrl") %>'>
                <%#: Eval("MenuCaption") %></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    </div>
</div>
<div class="containerUlTabPage">
    <asp:Repeater ID="rptMenuChild" runat="server" OnItemDataBound="rptMenuChild_ItemDataBound">
        <HeaderTemplate>
            <ul class="ulTabPage" id="ulTabMenuChild">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server" url='<%#: Eval("MenuUrl") %>'>
                <%#: Eval("MenuCaption") %></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<script type="text/javascript" id="dxss_patientbillsummarytoolbarctl">
    $(function () {
        $('#imgLock').click(function () {
            var hasOutstandingOrder = $('#<%=hdnIsHasOutstandingOrder.ClientID %>').val();

            if (hasOutstandingOrder == "1") {
                showToastConfirmation('Masih ada OUTSTANDING ORDER, lanjutkan mengunci transaksi?', function (result) {
                    if (result) cbpProcessRegistration.PerformCallback('lock');

                   
                });
            } else {
                showToastConfirmation('Apakah Anda Yakin Akan Mengunci Transaksi?', function (result) {
                    if (result) cbpProcessRegistration.PerformCallback('lock');

                    
                });
            }
        });

        $('#imgUnlock').click(function () {
            showToastConfirmation('Apakah Anda Yakin Akan Membuka Transaksi?', function (result) {
                if (result) cbpProcessRegistration.PerformCallback('unlock');

            });
        });

        $('#imgBillSummaryToolbarCloseRegistration').click(function () {
            var id = $('#<%=hdnRegistrationID.ClientID %>').val();
            var filterExpression = 'RegistrationID = ' + id;
            var messageError = '';

            if ($('#<%=hdnIsCheckResultTest.ClientID %>').val() == 1) {
                Methods.getListObject('GetvTestOrderImagingLabWithoutResultList', filterExpression, function (result) {
                    if (result.length > 0) {
                        for (i = 0; i < result.length; i++) {
                            if (messageError == '') {
                                messageError = "Masih ada pemeriksaan yang belum memiliki hasil, di nomor transaksi <b>" + result[i].TransactionNo + "</b>";
                            }
                            else {
                                var info = "Masih ada pemeriksaan yang belum memiliki hasil, di nomor transaksi <b>" + result[i].TransactionNo + "</b>"; ;
                                messageError = messageError + '<br>' + info;
                            }
                        }
                    }
                });

                if (messageError != '') {
                    messageError = messageError + '<br> Tetap lanjutkan tutup pendaftaran ?';
                }
                else {
                    messageError = 'Apakah Anda Yakin Akan Menutup Pendaftaran ?';
                }
            }
            else {
                messageError = 'Apakah Anda Yakin Akan Menutup Pendaftaran ?';
            }

            showToastConfirmation(messageError, function (result) {
                if (result) cbpProcessRegistration.PerformCallback('closed');
            });

        });

        $('#imgVisitNote.imgStatus').click(function () {
            var id = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
        });

    });

    function onGetUrlReferrer() {
        var moduleID = $('#<%=hdnModuleIDToolbar.ClientID %>').val();
        var url = $('#<%=hdnUrlBack.ClientID %>').val();
        if (moduleID != 'FN') {
            return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=bs");
        }
        else {
            return ResolveUrl(url + "Finance/Libs/Program/Module/PatientManagement/PatientBillSummaryCombine/PatientBillSummaryCombineList.aspx");        
        }
    }

    function onCbpProcessRegistrationEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[1] == 'fail')
            showToast('Tutup Pendaftaran Gagal', 'Error Message : ' + param[2]);
        else {
            if (param[0] == 'lock') {
                $('#<%=divLockTransaction.ClientID %>').css('display', 'none');
                $('#<%=divUnlockTransaction.ClientID %>').css('display', 'block');
                if (typeof onAfterLockUnlock == 'function') {
                    onAfterLockUnlock('lock');
                    /////window.location.reload(true);
                 }
            }
            else if (param[0] == 'unlock') {
                $('#<%=divLockTransaction.ClientID %>').css('display', 'block');
                $('#<%=divUnlockTransaction.ClientID %>').css('display', 'none');
                if (typeof onAfterLockUnlock == 'function') {
                    onAfterLockUnlock('unlock');
                    /////window.location.reload(true);
                }
            }
            else exitPatientPage();
        }

        hideLoadingPanel();
    }
</script>
<div style="display: none">
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcessRegistration" ClientInstanceName="cbpProcessRegistration"
        ShowLoadingPanel="false" OnCallback="cbpProcessRegistration_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessRegistrationEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
