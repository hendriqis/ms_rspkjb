<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" CodeBehind="BloodBankOrder.aspx.cs" 
Inherits="QIS.Medinfras.Web.CommonLibs.Program.BloodBankOrderForm" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomLeftButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
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
<asp:Content ID="Content5" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="testOrderList_script">
        var pageCount = parseInt('<%=PageCount %>');
        var pageCountDt1 = parseInt('<%=PageCountDt1 %>');
        var pageCountDt2 = parseInt('<%=PageCountDt2 %>');
        var pageCountDt3 = parseInt('<%=PageCountDt3 %>');
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnTestOrderIDMP.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnTransactionID.ClientID %>').val($(this).find('.transactionID').html());
                    $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.transactionNo').html());
                    $('#<%=hdnTestOrderGCTransactionStatusMP.ClientID %>').val($(this).find('.gcTransactionStatus').html());
                    SetOrderEntityToControl(this);
                    cbpViewDt.PerformCallback('refresh');
                    cbpViewDt3.PerformCallback('refresh');
                }
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Propose
            $('.btnPropose').die('click');
            $('.btnPropose').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                var mode = 'propose';
                var message = "Kirim order bank darah ke BDRS ?";
                $('#<%=hdnTestOrderIDMP.ClientID %>').val($tr.find('.keyField').html());

                displayConfirmationMessageBox('SEND ORDER :', message, function (result) {
                    if (result) {
                        $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                        onCustomButtonClick(mode);
                    }
                });
            });
            //#endregion

            //#region Reopen
            $('.btnReopen').die('click');
            $('.btnReopen').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnTestOrderIDMP.ClientID %>').val($tr.find('.keyField').html());

                displayConfirmationMessageBox('REOPEN ORDER :', 'Reopen order pemeriksaan penunjang ?', function (result) {
                    if (result) {
                        onCustomButtonClick('ReOpen');
                        $('#<%:hdnIsOutstandingOrder.ClientID %>').val("1");
                    }
                });
            });
            //#endregion

            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');

                if ($contentID == "bloodComponent") {
                    setPaging($("#pagingDt1"), pageCountDt1, function (page) {
                        cbpViewDt.PerformCallback('changepage|' + page);
                    });
                }
                else {
                    setPaging($("#pagingDt3"), pageCountDt2, function (page) {
                        cbpViewDt3.PerformCallback('changepage|' + page);
                    });
                }
            });
            //#endregion
        });


        function GetCurrentSelectedOrder(s) {
            var $tr = s;
            var idx = $('#<%=grdView.ClientID %> tr').index($tr);
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });
            return selectedObj;
        }

        function SetOrderEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedOrder(param);
            $('#<%:txtOrderDateMP.ClientID %>').val(selectedObj.cfOrderDate);
            $('#<%:txtOrderTimeMP.ClientID %>').val(selectedObj.TestOrderTime);
            $('#<%:txtParamedicNameMP.ClientID %>').val(selectedObj.ParamedicName);
            $('#<%:txtBloodTypeInfoMP.ClientID %>').val(selectedObj.cfBloodTypeInfo);
            $('#<%=txtGCSourceTypeMP.ClientID %>').val(selectedObj.SourceType);
            $('#<%=txtGCUsageTypeMP.ClientID %>').val(selectedObj.UsageType);
            $('#<%=txtGCPaymentTypeMP.ClientID %>').val(selectedObj.PaymentType);
            $('#<%:txtPurposeRemarksMP.ClientID %>').val(selectedObj.Remarks);
            $('#<%:txtTransfusionHistoryMP.ClientID %>').val(selectedObj.TransfusionHistory);

            if (selectedObj.GCSourceType == "X533^001") {
                $('#<%=trPaymentTypeInfoMP.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trPaymentTypeInfoMP.ClientID %>').attr("style", "display:none");
            }

            if (selectedObj.IsCITO == "True")
                $('#<%=chkIsCITOMP.ClientID %>').prop('checked', true);
            else
                $('#<%=chkIsCITOMP.ClientID %>').prop('checked', false);

            $('#<%=hdnTestOrderIDMP.ClientID %>').val(selectedObj.TestOrderID);
            $('#<%=hdnTestOrderGCTransactionStatusMP.ClientID %>').val(selectedObj.GCTransactionStatus);
        }

//        function SetRadioButtonListSelectedValue(name, selectedValue) {
//            $('input[name="' + name + '"][value="' + selectedValue + '"]').prop('checked', true);
//        }

        $('#<%=btnRefresh.ClientID %>').click(function (evt) {
            onRefreshControl();
        });


        function onAfterSaveAddRecordEntryPopup(param) {
            afterPopupEntry(param);
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            afterPopupEntry(param);
        }

        function onAfterSaveRecordPatientPageEntry() {
            onRefreshControl();
        }

        function onAfterCustomClickSuccess() {
            onRefreshControl();
        }

        function afterPopupEntry(param) {
            var paramInfo = param.split('|');
            if (paramInfo[0] != "checklist") {
                $('#<%=hdnTestOrderIDMP.ClientID %>').val(param);
                onRefreshControl();
            }
            else {
                cbpViewDt3.PerformCallback('refresh');
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
 //           cbpViewDt.PerformCallback('refresh');
//            cbpViewDt3.PerformCallback('refresh');
        }

        function onAfterPopupControlClosing() {
            onRefreshControl();
        }

        //#region Paging
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

//            setPaging($("#pagingDt1"), pageCountDt1, function (page) {
//                cbpViewDt.PerformCallback('changepage|' + page);
//            });

//            setPaging($("#pagingDt3"), pageCountDt3, function (page) {
//                cbpViewDt3.PerformCallback('changepage|' + page);
//            });
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

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDt3EndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount, function (page) {
                    cbpViewDt3.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeBackToListPage() {
            if ($('#<%:hdnIsOutstandingOrder.ClientID %>').val() == "1") {
                var line1 = "Masih ada order pemeriksaan yang belum dikirim ke Penunjang, Silahkan dikirim order terlebih dahulu dengan meng-klik tombol <b>Send Order</b>";
                var line2 = "<br />Jika masih mengalami kendala, silahkan klik tombol <b>Refresh</b>";
                var messageBody = line1 + line2;
                displayMessageBox('SEND ORDER', messageBody);
            }
            else {
                backToPatientList();
            }
        }

        //#region Checklist Form
        $('#lblAddMonitoring').die('click');
        $('#lblAddMonitoring').live('click', function (evt) {
            addMonitoring();
        });

        $('.imgAddMonitoring.imgLink').die('click');
        $('.imgAddMonitoring.imgLink').live('click', function (evt) {
            addMonitoring();
        });

        function addMonitoring() {
            var allow = true;

            if ($('#<%=hdnTestOrderGCTransactionStatusMP.ClientID %>').val() == "X121^001") {
                allow = false;
            }

            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/PatientMedicalFormEntry.ascx");
                var param = "0" + "|" + "X397^001" + "|" + "X401^009" + "|" + $('#<%=hdnTestOrderIDMP.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "||||||" + $('#<%=hdnRegisterRegistrationID.ClientID %>').val();
                openUserControlPopup(url, param, "Monitoring Transfusi", 800, 500);
            } else {
                displayMessageBox('Informasi', 'Harap send order terlebih dahulu.');
            }
        }

        $('.imgEditMonitoring.imgLink').die('click');
        $('.imgEditMonitoring.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var assessmentDate = $(this).attr('assessmentDate');
            var assessmentTime = $(this).attr('assessmentTime');
            var paramedicID = $(this).attr('paramedicID');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/PatientMedicalFormEntry.ascx");
                var param = recordID + "|" + "X397^001" + "|" + "X401^009" + "|" + $('#<%=hdnTestOrderIDMP.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "|" + assessmentDate + "|" + assessmentTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegisterRegistrationID.ClientID %>').val();
                openUserControlPopup(url, param, "Monitoring Transfusi", 800, 500);
            }
            else {
                displayErrorMessageBox('Monitoring Transfusi', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgDeleteMonitoring.imgLink').die('click');
        $('.imgDeleteMonitoring.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var message = "Hapus informasi monitoring transfusi untuk pasien ini ?";
                displayConfirmationMessageBox("Monitoring Transfusi", message, function (result) {
                    if (result) {
                        cbpDeleteMonitoring.PerformCallback(recordID);
                    }
                });
            }
            else {
                displayErrorMessageBox('Monitoring Transfusi', 'Maaf, tidak diijinkan menghapus pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewMonitoring.imgLink').die('click');
        $('.imgViewMonitoring.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var assessmentDate = $(this).attr('assessmentDate');
            var assessmentTime = $(this).attr('assessmentTime');
            var paramedicName = $(this).attr('paramedicName');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/ViewPatientMedicalFormCtl.ascx");
            var param = recordID + "|" + "X397^001" + "|" + "X401^009" + "|" + $('#<%=hdnTestOrderIDMP.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "|" + assessmentDate + "|" + assessmentTime + "|" + paramedicName + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegisterRegistrationID.ClientID %>').val();
            openUserControlPopup(url, param, "Monitoring Transfusi", 800, 500);
        });

        $('.imgCopyMonitoring.imgLink').die('click');
        $('.imgCopyMonitoring.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var assessmentDate = $(this).attr('assessmentDate');
            var assessmentTime = $(this).attr('assessmentTime');
            var paramedicID = $(this).attr('paramedicID');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            var message = "Lakukan copy form monitoring transfusi pasien ?";
            displayConfirmationMessageBox('Monitoring Transfusi :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/CopyPatientMedicalFormEntry.ascx");
                    var param = "0" + "|" + "X397^001" + "|" + "X401^009" + "|" + $('#<%=hdnTestOrderIDMP.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "|" + assessmentDate + "|" + assessmentTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegisterRegistrationID.ClientID %>').val();
                    openUserControlPopup(url, param, "Monitoring Transfusi", 800, 500);
                }
            });
        });
        //#endregion

        function oncbpDeleteMonitoringEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt3.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Monitoring Transfusi', param[1]);
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnRegisterHSUIsLaboratoryUnit" runat="server" value="" />
    <input type="hidden" id="hdnRegisterHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnRegisterDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnRegisterRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnRegisterVisitID" runat="server" value="" />
    <input type="hidden" id="hdnRegisterMRN" runat="server" value="" />
    <input type="hidden" id="hdnRegisterParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnBloodBankUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsOutstandingOrder" runat="server" value="0" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderIDMP" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionNo" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderGCTransactionStatusMP" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField testOrderID" ItemStyle-CssClass="keyField testOrderID" />
                                            <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="hiddenColumn transactionID" ItemStyle-CssClass="hiddenColumn transactionID" />
                                            <asp:BoundField DataField="TransactionNo" HeaderStyle-CssClass="hiddenColumn transactionNo" ItemStyle-CssClass="hiddenColumn transactionNo" />
                                            <asp:BoundField DataField="GCTransactionStatus" HeaderStyle-CssClass="hiddenColumn gcTransactionStatus" ItemStyle-CssClass="hiddenColumn gcTransactionStatus" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Tanggal ") %> - <%=GetLabel("Jam Order") %>,  <%=GetLabel("No. Order") %></div>
                                                    <div style="font-weight: bold"><span style="color:blue"><%=GetLabel("Diorder Oleh") %></span></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div><%#: Eval("cfTestOrderDateTime")%>,  <span style="font-weight:bold"><%#: Eval("TestOrderNo")%> </span></div>
                                                                <div style="font-weight: bold"> <span style="color:blue"> <%#: Eval("ParamedicName") %></span></div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> ><div><input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND ORDER" style="background-color:Red;color:White; width:100px;" /></div></td>
                                                            <td align="right" <%# Eval("IsAllowReopen").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %> ><div><input type="button" class="btnReopen w3-btn w3-hover-blue" value="REOPEN" style="background-color:Green;color:White; width:100px" /></div></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                 <span style="font-weight:bold; color: red" class="blink-alert"><%#: Eval("TransactionNo")%> </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("TestOrderNo") %>" bindingfield="TestOrderNo" />
                                                    <input type="hidden" value="<%#:Eval("cfOrderDate") %>" bindingfield="cfOrderDate" />
                                                    <input type="hidden" value="<%#:Eval("TestOrderTime") %>" bindingfield="TestOrderTime" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <input type="hidden" value="<%#:Eval("GCBloodType") %>" bindingfield="GCBloodType" />
                                                    <input type="hidden" value="<%#:Eval("BloodRhesus") %>" bindingfield="BloodRhesus" />
                                                    <input type="hidden" value="<%#:Eval("cfBloodTypeInfo") %>" bindingfield="cfBloodTypeInfo" />
                                                    <input type="hidden" value="<%#:Eval("GCSourceType") %>" bindingfield="GCSourceType" />
                                                    <input type="hidden" value="<%#:Eval("SourceType") %>" bindingfield="SourceType" />
                                                    <input type="hidden" value="<%#:Eval("GCUsageType") %>" bindingfield="GCUsageType" />
                                                    <input type="hidden" value="<%#:Eval("UsageType") %>" bindingfield="UsageType" />
                                                    <input type="hidden" value="<%#:Eval("GCPaymentType") %>" bindingfield="GCPaymentType" />
                                                    <input type="hidden" value="<%#:Eval("PaymentType") %>" bindingfield="PaymentType" />
                                                    <input type="hidden" value="<%#:Eval("IsCITO") %>" bindingfield="IsCITO" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("TransfusionHistory") %>" bindingfield="TransfusionHistory" />
                                                    <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                    <input type="hidden" value="<%#:Eval("TransactionID") %>" bindingfield="TransactionID" />
                                                    <input type="hidden" value="<%#:Eval("TransactionNo") %>" bindingfield="TransactionNo" />
                                                    <input type="hidden" value="<%#:Eval("GCTransactionStatus") %>" bindingfield="GCTransactionStatus" />
                                                    <input type="hidden" value="<%#:Eval("TransactionStatus") %>" bindingfield="TransactionStatus" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order Penunjang untuk pasien ini")%>
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
            </td>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="generalContent">
                                        <%=GetLabel("Informasi Order")%></li>
                                    <li contentid="bloodComponent">
                                        <%=GetLabel("Jenis Darah")%></li>
                                    <li contentid="surgeryParamedicTeam" style="display:none">
                                        <%=GetLabel("Team Pelaksana")%></li>
                                    <li contentid="monitoringForm">
                                        <%=GetLabel("Monitoring Transfusi")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="generalContent" style="position: relative;">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 180px" />
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Tanggal ")%>
                                                -
                                                <%=GetLabel("Jam ")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrderDateMP" Width="120px" runat="server" Style="text-align: center" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrderTimeMP" Width="80px" CssClass="time" runat="server" Style="text-align: center" Enabled="false" />
                                        </td>
                                        <td />
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Diorder oleh")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtParamedicNameMP" Width="300px" runat="server" Enabled="false" />
                                        </td>
                                    </tr>  
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Golongan Darah")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBloodTypeInfoMP" Width="80px" runat="server" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCITOMP" Width="100px" runat="server" Text=" CITO" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Sumber/Asal Darah") %></td>
                                        <td colspan="2">                                        
                                            <asp:TextBox ID="txtGCSourceTypeMP" Width="300px" runat="server" Enabled="false" />
                                            <%--<asp:RadioButtonList ID="rblGCSourceTypeMP" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled = "false">
                                                <asp:ListItem Text=" PMI" Value="X533^001" />
                                                <asp:ListItem Text=" Persediaan BDRS" Value="X533^002" />
                                                <asp:ListItem Text=" Pendonor" Value="X533^003" />
                                            </asp:RadioButtonList>--%>
                                        </td>    
                                    </tr>  
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Cara Penyimpanan") %></td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtGCUsageTypeMP" Width="300px" runat="server" Enabled="false" />
                                            <%--<asp:RadioButtonList ID="rblGCUsageTypeMP" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled = "false">
                                                <asp:ListItem Text=" Langsung digunakan" Value="X534^001" />
                                                <asp:ListItem Text=" Dititipkan di BDRS" Value="X534^002" />
                                            </asp:RadioButtonList>--%>
                                        </td>    
                                    </tr> 
                                    <tr id="trPaymentTypeInfoMP" runat="server" style="display: none">
                                        <td class="tdLabel"><%=GetLabel("Cara Pembayaran (Jika PMI)") %></td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtGCPaymentTypeMP" Width="300px" runat="server" Enabled="false" />
                                            <%--<asp:RadioButtonList ID="rblGCPaymentTypeMP" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled = "false">
                                                <asp:ListItem Text=" Dibayar langsung di PMI" Value="X535^001" />
                                                <asp:ListItem Text=" Tagihan Pasien di Rumah Sakit" Value="X534^002" />
                                            </asp:RadioButtonList>--%>
                                        </td>    
                                    </tr>   
                                    <tr>
                                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Catatan Klinis/Diagnosa") %></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtPurposeRemarksMP" runat="server" Width="99%" TextMode="Multiline"
                                                Height="100px" Enabled="false" />
                                        </td>
                                    </tr>      
                                    <tr>
                                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                            <label class="lblNormal">
                                                <%=GetLabel("Riwayat Transfusi") %></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtTransfusionHistoryMP" runat="server" Width="99%" TextMode="Multiline"
                                                Height="100px" Enabled="false" />
                                        </td>
                                    </tr>                                                                                        
                                </table>
                            </div>
                            <div class="containerOrderDt" id="bloodComponent" style="position: relative;display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Jenis Darah")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div><%#: Eval("ItemName1")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Jumlah"  DataField="cfQuantity" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"  />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Belum ada informasi jenis darah untuk order bank darah ini") %>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt1"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="monitoringForm" style="position: relative;display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                           <input type="hidden" value="" id="hdnFileString" runat="server" />
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddMonitoring imgLink" title='<%=GetLabel("+ Checklist")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("AssessmentID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditMonitoring imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("AssessmentID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" assessmentDate = "<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime = "<%#:Eval("AssessmentTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>"
                                                                                formLayout = "<%#:Eval("AssessmentFormLayout") %>" formValue = "<%#:Eval("AssessmentFormValue") %>"/>
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteMonitoring imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("AssessmentID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"  />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgViewMonitoring imgLink" title='<%=GetLabel("Lihat Monitoring")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordID = "<%#:Eval("AssessmentID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" assessmentDate = "<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime = "<%#:Eval("AssessmentTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>" paramedicName = "<%#:Eval("ParamedicName") %>"
                                                                                formLayout = "<%#:Eval("AssessmentFormLayout") %>" formValue = "<%#:Eval("AssessmentFormValue") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgCopyMonitoring imgLink" title='<%=GetLabel("Copy Monitoring")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                alt="" recordID = "<%#:Eval("AssessmentID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" assessmentDate = "<%#:Eval("cfAssessmentDate") %>" assessmentTime = "<%#:Eval("AssessmentTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>" paramedicName = "<%#:Eval("ParamedicName") %>"
                                                                                formLayout = "<%#:Eval("AssessmentFormLayout") %>" formValue = "<%#:Eval("AssessmentFormValue") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                        <asp:BoundField DataField="cfAssessmentDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField DataField="AssessmentTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                                        <asp:BoundField DataField="ParamedicID" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                                        <asp:BoundField DataField="GCAssessmentType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                        <asp:BoundField DataField="AssessmentFormLayout" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                        <asp:BoundField DataField="AssessmentFormValue" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                        <asp:BoundField DataField="IsInitialAssessment" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn" ItemStyle-CssClass="isInitialAssessment hiddenColumn"/>
                                                        <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                        <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada data monitoring transfusi untuk nomor order ini") %>
                                                        <br />
                                                        <span class="lblLink" id="lblAddMonitoring">
                                                            <%= GetLabel("+ Monitoring")%></span>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt3">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteMonitoring" runat="server" Width="100%" ClientInstanceName="cbpDeleteMonitoring"
            ShowLoadingPanel="false" OnCallback="cbpDeleteMonitoring_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteMonitoringEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
