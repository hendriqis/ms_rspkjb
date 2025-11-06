<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="ARFinalClaimv2Approval.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARFinalClaimv2Approval" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnApproved" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnApprovedAll" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Approve All")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        }

        $('#btnRefresh').live('click', function () {
            cbpProcessDetailV2.PerformCallback('refresh');
        });

        $('#<%=btnApproved.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah yakin akan proses APPROVE ?', function (result) {
                if (result) {
                    if ($('.chkIsSelected input:checked').length < 1)
                        showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                    else {
                        getCheckedMember();
                        onCustomButtonClick('approve');
                    }
                }
            });
        });

        $('#<%=btnApprovedAll.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah yakin akan proses APPROVE semua data dibawah ini ?', function (result) {
                if (result) {
                    onCustomButtonClick('approveall');
                }
            });
        });

        function getCheckedMember() {
            var lstPaymentDetailID = $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val().split(',');
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var keyField = $tr.find('.keyField').val();
                    var idx = lstPaymentDetailID.indexOf(keyField);
                    if (idx < 0) {
                        lstPaymentDetailID.push(keyField);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstPaymentDetailID.indexOf(key);
                    if (idx > -1) {
                        lstPaymentDetailID.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val(lstPaymentDetailID.join(','));
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            calculateTotal();
        });

        $('.lblChangeSEP').die('click');
        $('.lblChangeSEP').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.RegistrationID').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/GenerateSEPManualCtl.ascx");
            openUserControlPopup(url, id, 'Ubah SEP Manual', 700, 300);
        });

        $('.lblChangeReferral').die('click');
        $('.lblChangeReferral').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.RegistrationID').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ChangeReferralManualCtl.ascx");
            openUserControlPopup(url, id, 'Ubah Rujukan Manual', 700, 300);
        });

        $('.lblRegCountPerSEP').die('click');
        $('.lblRegCountPerSEP').live('click', function () {
            $tr = $(this).closest('tr');
            var oNoSEP = $tr.find('.NoSEP').val();
            var url = ResolveUrl('~/Program/BPJS/ARClaim/DetailRegistrationInfoPerNoSEPCtl.ascx');
            openUserControlPopup(url, oNoSEP, 'Informasi Registrasi per No SEP', 1200, 500);
        });

        function oncbpProcessDetailV2EndCallback(s) {
            $('.txtGrouperAmountFinal').each(function () {
                $(this).trigger('changeValue');
            });

            $('#<%=chkAllThisStatus.ClientID %>').prop('checked', false);
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', false);
                $(this).change();
            });

            calculateTotalWithChange();
            hideLoadingPanel();
        }

        $('.txtGrouperAmountFinal').live('change', function () {
            calculateTotal();
        });

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpProcessDetailV2.PerformCallback('refresh');
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpProcessDetailV2.PerformCallback('refresh');
        }

        function onAfterPopupControlClosing() {
            cbpProcessDetailV2.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                if (retval != "Download Failed") {
                    downloadBPJSDocument(retval);
                } else {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');
                }
            }

            cbpProcessDetailV2.PerformCallback('refresh');

            $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val("");
        }

        $('#<%:chkAllThisStatus.ClientID %>').live('change', function () {
            $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val("");

            var isChecked = $(this).is(":checked");
            var valueSelected = cboFeedbackStatusFilter.GetValue();

            $('.chkIsSelected input').each(function () {
                if (isChecked) {
                    var $tr = $(this).closest('tr');
                    var rowIndex = $tr.find('.hdnRowIndex').val();
                    var cboFeedbackStatus = eval('cboFeedbackStatus' + rowIndex);
                    if (valueSelected == null) {
                        $(this).prop('checked', true);
                        $(this).change();
                    }
                    else {
                        if (cboFeedbackStatus.GetValue() == valueSelected) {
                            $(this).prop('checked', true);
                            $(this).change();
                        }
                        else {
                            $(this).prop('checked', false);
                            $(this).change();
                        }
                    }
                }
                else {
                    $(this).prop('checked', false);
                    $(this).change();
                }
            });
        });

        function onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }

        function calculateTotal() {
            var amount = parseFloat(0);
            var sepNo = "";
            var recordNo = "";
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var token = ",";
                    var newToken = "";
                    var value = $tr.find('.txtGrouperAmountFinal').val().split(token).join(newToken);
                    var paymentAmount = parseFloat(parseFloat(value));
                    amount += paymentAmount;

                    var noSEP = $tr.find('.NoSEP').val();
                    if (noSEP == "") {
                        noSEP = "~";
                    }

                    if (sepNo == "") {
                        sepNo = noSEP;
                    }
                    else {
                        sepNo += "|" + noSEP;
                    }

                    var keyField = $tr.find('.keyField').val();
                    if (recordNo == "") {
                        recordNo = keyField;
                    }
                    else {
                        recordNo += "|" + keyField;
                    }
                }
            });

            if (recordNo != "") {
                var recordFinal = recordNo.split('|');
                if (recordFinal[0] != "") {
                    $('#<%=divTotalRecord.ClientID %>').html(recordFinal.length);
                }
                else {
                    $('#<%=divTotalRecord.ClientID %>').html('0');
                }
            } else {
                $('#<%=divTotalRecord.ClientID %>').html('0');
            }

            if (sepNo != "") {
                var sepFinal = sepNo.split('|');
                $('#<%=divNoSEP.ClientID %>').html(sepFinal.length);
                //                var unique = sepFinal.filter(onlyUnique);
                //                if (unique[0] != "") {
                //                    $('#<%=divNoSEP.ClientID %>').html(unique.length);
                //                }
                //                else {
                //                    $('#<%=divNoSEP.ClientID %>').html('0');
                //                }
            }
            else {
                $('#<%=divNoSEP.ClientID %>').html('0');
            }

            $('#<%=divGrouperAmount.ClientID %>').html(formatMoneyDiv(amount));
        }

        function calculateTotalWithChange() {
            var amount = parseFloat(0);
            var sepNo = "";
            var recordNo = "";
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $(this).change();
                    var $tr = $(this).closest('tr');
                    var token = ",";
                    var newToken = "";
                    var value = $tr.find('.txtGrouperAmountFinal').val().split(token).join(newToken);
                    var paymentAmount = parseFloat(parseFloat(value));
                    amount += paymentAmount;

                    var noSEP = $tr.find('.NoSEP').val();
                    if (noSEP == "") {
                        noSEP = "~";
                    }

                    if (sepNo == "") {
                        sepNo = noSEP;
                    }
                    else {
                        sepNo += "|" + noSEP;
                    }

                    var keyField = $tr.find('.keyField').val();
                    if (recordNo == "") {
                        recordNo = keyField;
                    }
                    else {
                        recordNo += "|" + keyField;
                    }
                }
            });

            if (recordNo != "") {
                var recordFinal = recordNo.split('|');
                if (recordFinal[0] != "") {
                    $('#<%=divTotalRecord.ClientID %>').html(recordFinal.length);
                }
                else {
                    $('#<%=divTotalRecord.ClientID %>').html('0');
                }
            } else {
                $('#<%=divTotalRecord.ClientID %>').html('0');
            }

            if (sepNo != "") {
                var sepFinal = sepNo.split('|');
                $('#<%=divNoSEP.ClientID %>').html(sepFinal.length);
                //                var unique = sepFinal.filter(onlyUnique);
                //                if (unique[0] != "") {
                //                    $('#<%=divNoSEP.ClientID %>').html(unique.length);
                //                }
                //                else {
                //                    $('#<%=divNoSEP.ClientID %>').html('0');
                //                }
            }
            else {
                $('#<%=divNoSEP.ClientID %>').html('0');
            }

            $('#<%=divGrouperAmount.ClientID %>').html(formatMoneyDiv(amount));
        }

        function formatMoneyDiv(value) {
            var text = '0';
            if (value == '') {
                text = '0.00';
            }
            else {
                value = parseFloat(value).toFixed(2);
                text = parseFloat(value).formatMoney(2, '.', ',');
            }
            return text;
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPaymentDetailID" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowApprove" runat="server" value="0" />
    <input type="hidden" id="hdnBPJSUploadedFile" runat="server" value="" />
    <input type="hidden" id="hdnParamDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnParamIsExclusion" runat="server" value="" />
    <input type="hidden" id="hdnParamPaymentDate" runat="server" value="" />
    <input type="hidden" id="hdnIsUsedClaimFinal" runat="server" value="0" />
    <input type="hidden" id="hdnIsFinalisasiKlaimAfterARInvoice" runat="server" value="" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 70%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <h4>
                        <%=GetLabel("Data Pencarian")%></h4>
                    <div>
                        <table style="width: 100%">
                            <colgroup>
                                <col style="width: 180px" />
                                <col style="width: 170px" />
                                <col style="width: 20px" />
                                <col style="width: 170px" />
                                <col style="width: 250px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label>
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                        Style="width: 350px" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" ID="chkIsExclusionDepartment" Text=" Abaikan Asal Pasien?" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        <%=GetLabel("Filter Tanggal")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboDateType" ClientInstanceName="cboDateType" runat="server"
                                        Style="width: 350px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Periode") %></label>
                                </td>
                                <td align="left" colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" Width="135px" />
                                            </td>
                                            <td>
                                                <label>
                                                    <%=GetLabel("s/d")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" Width="135px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Nilai Klaim Final")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboGrouperAmountFinalFilter" ClientInstanceName="cboGrouperAmountFinalFilter"
                                        runat="server" Style="width: 350px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Status Umpan Balik")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboFeedbackStatusFilter" ClientInstanceName="cboFeedbackStatusFilter"
                                        runat="server" Style="width: 350px">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" ID="chkIsExclusion" Text=" Is Exclusion?" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkAllThisStatus" Text=" Pilih Semua Data di Bawah dengan Status Ini ?" />
                                </td>
                            </tr>
                            <tr style='display:none'>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Tipe Download Data")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboDownloadType" ClientInstanceName="cboDownloadType" runat="server"
                                        Style="width: 350px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Penjamin Bayar")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboBusinessPartner" ClientInstanceName="cboBusinessPartner"
                                        runat="server" Style="width: 350px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left" colspan="2">
                                    <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td>
                                    <asp:FileUpload ID="BPJSDocumentUpload" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <div>
                        <table width="100%">
                            <colgroup>
                                <col style="width: 50%" />
                                <col style="width: 50%" />
                            </colgroup>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <table width="100%">
                                        <colgroup>
                                            <col style="width: 25%" />
                                            <col style="width: 25%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;" id="divEntrySummary" runat="server">
                                                    <div class="pageTitle" style="text-align: center">
                                                        <b>
                                                            <%=GetLabel("TOTAL")%></b>
                                                    </div>
                                                    <div style="background-color: #EAEAEA;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="1px">
                                                            <colgroup>
                                                                <col width="50%" />
                                                                <col width="50%" />
                                                            </colgroup>
                                                            <tr>
                                                                <td runat="server">
                                                                    <div style="text-align: center; width: 150px">
                                                                        <b>
                                                                            <%=GetLabel("Record")%></b>
                                                                    </div>
                                                                    <div runat="server" id="divTotalRecord" style="text-align: center; font-weight: bold;" />
                                                                </td>
                                                                <td runat="server">
                                                                    <div style="text-align: center; width: 150px">
                                                                        <b>
                                                                            <%=GetLabel("Nomor SEP")%></b>
                                                                    </div>
                                                                    <div runat="server" id="divNoSEP" style="text-align: center; font-weight: bold;" />
                                                                </td>
                                                                <td runat="server">
                                                                    <div style="text-align: center; width: 150px">
                                                                        <b>
                                                                            <%=GetLabel("Nilai Grouper")%></b>
                                                                    </div>
                                                                    <div runat="server" id="divGrouperAmount" style="text-align: center; font-weight: bold;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h4>
                        <%=GetLabel("Data Piutang")%></h4>
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpProcessDetailV2" runat="server" Width="100%" ClientInstanceName="cbpProcessDetailV2"
                            ShowLoadingPanel="false" OnCallback="cbpProcessDetailV2_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpProcessDetailV2EndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <input type="hidden" id="hdnTotalSEP" runat="server" value="0" />
                                    <input type="hidden" id="hdnTotalGrouper" runat="server" value="" />
                                    <input type="hidden" id="hdnlstPaymentDetailID" runat="server" value="" />
                                    <asp:Panel runat="server" ID="panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No SEP")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No Pembayaran")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Informasi Registrasi")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Pembayaran")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Selisih")%>
                                                        </th>
                                                        <th align="center" style="width: 200px">
                                                            <%=GetLabel("Status Umpan Balik")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Final Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Final Grouper")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="21">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No SEP")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No Pembayaran")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Informasi Registrasi")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Pembayaran")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Selisih")%>
                                                        </th>
                                                        <th align="center" style="width: 200px">
                                                            <%=GetLabel("Status Umpan Balik")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Final Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Final Grouper")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <input type="hidden" class="hdnRowIndex" value='<%#: Container.DataItemIndex %>' />
                                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PaymentDetailID")%>' />
                                                        <input type="hidden" class="PaymentID" id="PaymentID" runat="server" value='<%#: Eval("PaymentID")%>' />
                                                        <input type="hidden" class="RegistrationID" id="RegistrationID" runat="server" value='<%#: Eval("RegistrationID")%>' />
                                                        <input type="hidden" class="NoSEP" id="NoSEP" runat="server" value='<%#: Eval("NoSEP")%>' />
                                                        <input type="hidden" class="RegistrationNo" id="RegistrationNo" runat="server" value='<%#: Eval("RegistrationNo")%>' />														
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("NoSEP") %></label></b>
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%#: Eval("PaymentNo") %></label></b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("RegistrationNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfRegistrationDateInString") %></label>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%#: Eval("DepartmentID") %></label>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%#: Eval("ServiceUnitName") %></label>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("MedicalNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("PatientName") %></label>
                                                    </td>
                                                    <td align="right">
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfPaymentAmountInString") %></label>
                                                    </td>
                                                    <td align="center">
                                                        <label class="lblNormal">
                                                            <%#: Eval("GrouperCodeClaim") %></label>
                                                    </td>
                                                    <td align="right">
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfGrouperAmountClaimInString") %></label>
                                                    </td>
                                                    <td align="right">
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfSelisihInString") %></label>
                                                    </td>
                                                    <td align="center">
                                                        <dxe:ASPxComboBox ID="cboFeedbackStatus" CssClass="cboFeedbackStatus" runat="server"
                                                            Enabled="false" Width="90%" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtGrouperCodeFinal" Width="90%" runat="server" ReadOnly="true"
                                                            CssClass="txtGrouperCodeFinal" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtGrouperAmountFinal" Width="90%" runat="server" ReadOnly="true"
                                                            CssClass="txtGrouperAmountFinal txtCurrency" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
