<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="TransactionPatientInformationList2.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPatientInformationList2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region tab
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });
        //#endregion

        //#region listview
//        function onCboChanged() {
//            onRefreshGridView();
//        }

//        function onCboServiceUnitRealization() {
//            cbpViewRealization.PerformCallback('refresh');
//        }

        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefreshR.lblLink').click(function (evt) {
                onRefreshGridView();
            });

//            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
//                onRefreshGridView();
//            });

            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        });
        //#endregion

        //#region PrescriptionOrderF
        $('.lblPrescriptionOrderF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'pof';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region PrescriptionOrderO
        $('.lblPrescriptionOrderO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'poo';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region PrescriptionROrderF
        $('.lblPrescriptionROrderF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'porf';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region PrescriptionROrderO
        $('.lblPrescriptionROrderO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'poro';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region LaboratoryF
        $('.lblLaboratoryF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'lbf';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region LaboratoryO
        $('.lblLaboratoryO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'lbo';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region ImagingF
        $('.lblImagingF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'imf';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region ImagingO
        $('.lblImagingO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'imo';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region OtherDiagnosticF
        $('.lblOtherDiagnosticF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'odf';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region OtherDiagnosticO
        $('.lblOtherDiagnosticO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'odo';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region ServiceF
        $('.lblServiceF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'srf';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region ServiceO
        $('.lblServiceO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'sro';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region AllTransactionF
        $('.lblAllTransactionF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'altf';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region AllTransactionO
        $('.lblAllTransactionO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'alto';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region PostingObat
        $('.lblPostingObat').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'pob';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region OutstandingPostingObat
        $('.lblOutstandingPostingObat').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'opob';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region PostingAlkes
        $('.lblPostingAlkes').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'pal';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region OutstandingPostingAlkes
        $('.lblOutstandingPostingAlkes').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'opal';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region PostingBarangUmum
        $('.lblPostingBarangUmum').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'pbu';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region OutstandingPostingBarangUmum
        $('.lblOutstandingPostingBarangUmum').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'opbu';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region BillingF
        $('.lblBillingF').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'bilf';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region BillingO
        $('.lblBillingO').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val() + '|' + 'bilo';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/TransactionPatientInformationDetailCtl2.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
        });
        //#endregion

        //#region CountOutstanding
        $('.lblCountOutstanding').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyR').val() + '|' + $('#<%=hdnDepartmentID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/PatientInformationRealizationCtl.ascx");
            openUserControlPopup(url, id, 'Detail Informasi Realisasi', 1100, 500);
        });
        //#endregion

        //#region quicksearch
//        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
//        var intervalID = window.setInterval(function () {
//            onRefreshGridView();
//        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
//                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
//                intervalID = window.setInterval(function () {
//                    onRefreshGridView();
//                }, interval);
            }
        }

        //        function onRefreshGridView() {
        //            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
        //                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        //                cbpView.PerformCallback('refresh');
        //            }
        //        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            if (pageCount > 100) pageCount = 100;
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }

        function onCbpViewRealizationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div class="containerUlTabPage" style="margin-bottom: 3px;">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li contentid="containerUnitAsal" class="selected">
                <%=GetLabel("UNIT ASAL")%></li>
            <li contentid="containerUnitRealisasi">
                <%=GetLabel("UNIT REALISASI")%></li>
        </ul>
    </div>
    <div id="containerUnitAsal" class="containerInfo">
        <div style="padding: 15px">
            <div class="pageTitle">
                <%=GetLabel("Informasi Pending Transaksi Pasien ")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientList">
                            <table>
                                <colgroup>
                                    <col style="width: 140px" />
                                </colgroup>
                                <tr id="trServiceUnitName" runat="server">
                                    <td>
                                        <%=GetServiceUnitLabel()%>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnit" Width="200px" ClientInstanceName="cboKlinik"
                                            runat="server">
                                            <%--<ClientSideEvents ValueChanged="function(s,e){ onCboChanged(); }" />--%>
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trRegistrationDate" runat="server">
                                    <td>
                                        <%=GetLabel("Tanggal") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                            Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <%--<%=GetLabel("Halaman Ini Akan")%>--%>
                                <span class="lblLink" id="lblRefresh">[Refresh]</span>
                                <%=GetLabel("halaman ini")%>
                                <%--<%=GetRefreshGridInterval() %>
                                <%=GetLabel("Menit")%>--%>
                            </div>
                            <div style="text-align: right; color: Red">
                                <%=GetLabel("NOTES : O (Outstanding) | F (Finish) | P (Posting)")%>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwViewUnitAsal">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th colspan="2">
                                                            <%=GetLabel("INFORMATION")%>
                                                        </th>
                                                        <th colspan="12">
                                                            <%=GetLabel("ORDER")%>
                                                        </th>
                                                        <th colspan="8">
                                                            <%=GetLabel("TRANSACTION")%>
                                                        </th>
                                                        <th colspan="4" rowspan="2">
                                                            <%=GetLabel("BILLING")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th rowspan="2" style="width: 150px">
                                                            <%=GetLabel("Registrasi") %>
                                                        </th>
                                                        <th rowspan="2" style="width: 300px">
                                                            <%=GetLabel("Pasien") %>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Prescription")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Prescription Return")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Laboratory")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Imaging")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Other Diagnostic")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Service")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("All")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Obat")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Alkes")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Barang Umum")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("P") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("P") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("P") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="24">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th colspan="2">
                                                            <%=GetLabel("INFORMATION")%>
                                                        </th>
                                                        <th colspan="12">
                                                            <%=GetLabel("ORDER")%>
                                                        </th>
                                                        <th colspan="8">
                                                            <%=GetLabel("TRANSACTION")%>
                                                        </th>
                                                        <th colspan="4" rowspan="2">
                                                            <%=GetLabel("BILLING")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th rowspan="2" style="width: 150px">
                                                            <%=GetLabel("Registrasi") %>
                                                        </th>
                                                        <th rowspan="2" style="width: 300px">
                                                            <%=GetLabel("Pasien") %>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Prescription")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Prescription Return")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Laboratory")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Imaging")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Other Diagnostic")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Service")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("All")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Obat")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Alkes")%>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("Barang Umum")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("P") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("P") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("P") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("F") %>
                                                        </th>
                                                        <th style="width: 40px">
                                                            <%=GetLabel("O") %>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#: Eval("RegistrationNo")%><div>
                                                            <%#: Eval("ServiceUnitName")%></div>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("PatientName")%>
                                                        (<%#: Eval("MedicalNo")%>)
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblPrescriptionOrderF">
                                                            <%#: Eval("PrescriptionOrderF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblPrescriptionOrderO">                                                        
                                                            <div <%# Eval("PrescriptionOrderO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("PrescriptionOrderO")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblPrescriptionROrderF">
                                                            <%#: Eval("PrescriptionReturnOrderF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblPrescriptionROrderO">
                                                           <div <%# Eval("PrescriptionReturnOrderO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                            <%#: Eval("PrescriptionReturnOrderO")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblLaboratoryF">
                                                            <%#: Eval("LaboratoryOrderF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblLaboratoryO">
                                                            <div <%# Eval("LaboratoryOrderO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("LaboratoryOrderO")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblImagingF">
                                                            <%#: Eval("ImagingOrderF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblImagingO">
                                                            <div <%# Eval("ImagingOrderO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("ImagingOrderO")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblOtherDiagnosticF">
                                                            <%#: Eval("OtherDiagnosticF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblOtherDiagnosticO">
                                                            <div <%# Eval("OtherDiagnosticO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("OtherDiagnosticO")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblServiceF">
                                                            <%#: Eval("ServiceOrderF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblServiceO">
                                                            <div <%# Eval("ServiceOrderO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("ServiceOrderO")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblAllTransactionF">
                                                            <%#: Eval("AllTransactionF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblAllTransactionO">
                                                            <div <%# Eval("AllTransactionO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("AllTransactionO")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblPostingObat">
                                                            <%#: Eval("PostingObat")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblOutstandingPostingObat">
                                                            <div <%# Eval("OutstandingPostingObat").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("OutstandingPostingObat")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblPostingAlkes">
                                                            <%#: Eval("PostingAlkes")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblOutstandingPostingAlkes">
                                                            <div <%# Eval("OutstandingPostingAlkes").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("OutstandingPostingAlkes")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblPostingBarangUmum">
                                                            <%#: Eval("PostingBarangUmum")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblOutstandingPostingBarangUmum">
                                                            <div <%# Eval("OutstandingPostingBarangUmum").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("OutstandingPostingBarangUmum")%>
                                                            </div>
                                                        </a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblBillingF">
                                                            <%#: Eval("BillingF")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                        <a class="lblLink lblBillingO">              
                                                            <div <%# Eval("BillingO").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                                <%#: Eval("BillingO")%>
                                                            </div>                                                                                                  
                                                        </a>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging">
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="containerUnitRealisasi" class="containerInfo" style="display: none">
        <div style="padding: 15px">
            <div class="pageTitle">
                <%=GetLabel("Informasi Realisasi Transaksi Pasien ")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientListR">
                            <table>
                                <colgroup>
                                    <col style="width: 140px" />
                                </colgroup>
                                <tr id="trServiceUnitNameR" runat="server">
                                    <td>
                                        <%=GetLabel("Unit Realisasi")%>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnitRealization" Width="200px" ClientInstanceName="cboServiceUnitRealization"
                                            runat="server">
                                            <%--<ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitRealization(); }" />--%>
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <%--<%=GetLabel("Halaman Ini Akan")%>--%>
                                <span class="lblLink" id="lblRefreshR">[Refresh]</span>
                                <%=GetLabel("halaman ini")%>
                                <%--<%=GetRefreshGridInterval() %>
                                <%=GetLabel("Menit")%>--%>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxcp:ASPxCallbackPanel ID="cbpViewRealization" runat="server" Width="100%" ClientInstanceName="cbpViewRealization"
                            ShowLoadingPanel="false" OnCallback="cbpViewRealization_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewRealizationEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdViewR" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwViewRealisasi">
                                            <EmptyDataTemplate>
                                                <table id="tblViewRealization" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 230px" align="left">
                                                            <%=GetLabel("Order Realization") %>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Count")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Finish") %>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Outstanding") %>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("ALL") %>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="4">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblViewRealization" runat="server" class="grdView notAllowSelect lvwViewR"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 230px" align="left">
                                                            <%=GetLabel("Order Realization") %>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Count")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Finish") %>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Outstanding") %>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("ALL") %>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="left">
                                                        <%#: Eval("ServiceUnitName")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("CountFinish")%>
                                                    </td>
                                                    <td align="right">
                                                        <input type="hidden" class="hdnKeyR" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                        <a class="lblLink lblCountOutstanding">
                                                        <%#: Eval("CountOutstanding")%></a>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("CountALL")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
