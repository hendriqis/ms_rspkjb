<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="ARClaim.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARClaim" %>

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
    <li id="btnSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnApproved" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnDownload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdownload.png")%>' alt="" /><div>
            <%=GetLabel("Download")%></div>
    </li>
    <li id="btnUpload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbupload.png")%>' alt="" /><div>
            <%=GetLabel("Upload")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
            setDatePicker('<%=txtClaimDate.ClientID %>');
        }

        $('#btnRefresh').die('click');
        $('#btnRefresh').live('click', function () {
            cbpProcessDetail.PerformCallback('refresh');
        });

        $('#<%=btnSave.ClientID %>').die('click');
        $('#<%=btnSave.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah yakin akan proses SAVE ?', function (result) {
                if (result) {
                    if ($('.chkIsSelected input:checked').length < 1)
                        showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                    else {
                        getCheckedMember();
                        onCustomButtonClick('save');
                    }
                }
            });
        });

        $('#<%=btnApproved.ClientID %>').die('click');
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

        function getCheckedMember() {
            var lstPaymentDetailID = $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val().split(',');
            var lstGrouperCode = $('#<%=hdnSelectedGrouperCode.ClientID %>').val().split(',');
            var lstPaymentAmount = $('#<%=hdnSelectedPaymentAmount.ClientID %>').val().split(',');
            var lstRegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val().split(',');

            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var keyField = $tr.find('.keyField').val();
                    var grouperCode = $tr.find('.txtGrouperCode').val();
                    var paymentAmount = parseFloat(parseFloat($tr.find('.txtPaymentAmount').attr('hiddenVal')).toFixed(2));
                    var registrationID = $tr.find('.RegistrationID').val();

                    var idx = lstPaymentDetailID.indexOf(keyField);
                    if (idx < 0) {
                        lstPaymentDetailID.push(keyField);
                        lstGrouperCode.push(grouperCode);
                        lstPaymentAmount.push(paymentAmount);
                        lstRegistrationID.push(registrationID);
                    }
                    else {
                        lstGrouperCode[idx] = grouperCode;
                        lstPaymentAmount[idx] = paymentAmount;
                        lstRegistrationID[idx] = registrationID;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstPaymentDetailID.indexOf(key);
                    if (idx > -1) {
                        lstPaymentDetailID.splice(idx, 1);
                        lstGrouperCode.splice(idx, 1);
                        lstPaymentAmount.splice(idx, 1);
                        lstRegistrationID.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val(lstPaymentDetailID.join(','));
            $('#<%=hdnSelectedGrouperCode.ClientID %>').val(lstGrouperCode.join(','));
            $('#<%=hdnSelectedPaymentAmount.ClientID %>').val(lstPaymentAmount.join(','));
            $('#<%=hdnRegistrationID.ClientID %>').val(lstRegistrationID.join(','));
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });

            calculateTotal();
        });

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked')) {
                $tr.find('.txtGrouperCode').removeAttr('readonly');
                $tr.find('.txtPaymentAmount').removeAttr('readonly');
            }
            else {
                $tr.find('.txtGrouperCode').attr('readonly', 'readonly');
                $tr.find('.txtPaymentAmount').attr('readonly', 'readonly');
            }
        });

        $('.txtPaymentAmount').live('change', function () {
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

        $('.lblCOBDetail').die('click');
        $('.lblCOBDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var oRegistrationNo = $tr.find('.RegistrationNo').val();
            var oCOBDetail = $tr.find('.COBDetail').val();
            var message = "Informasi COB Registrasi <b>" + oRegistrationNo + "</b> : " + oCOBDetail;
            showToast("", message);
        });

        //#region Download & Upload

        $('#<%=btnDownload.ClientID %>').die('click');
        $('#<%=btnDownload.ClientID %>').live('click', function () {
            onCustomButtonClick('download');
        });

        function downloadBPJSDocument(stringparam) {
            var fileName = $('#<%=hdnFileName.ClientID %>').val();

            var link = document.createElement("a");
            link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
            link.download = fileName;
            link.click();
        }

        $('#<%=btnUpload.ClientID %>').die('click');
        $('#<%=btnUpload.ClientID %>').live('click', function () {
            document.getElementById('<%=BPJSDocumentUpload.ClientID %>').click();
        });

        $('#<%=BPJSDocumentUpload.ClientID %>').die('change');
        $('#<%=BPJSDocumentUpload.ClientID %>').live('change', function () {
            readURL(this);

            if ($('#<%=hdnBPJSUploadedFile.ClientID %>').val() != "" && $('#<%=hdnBPJSUploadedFile.ClientID %>').val() != null) {
                onCustomButtonClick('upload');
            } else {
                displayErrorMessageBox('Upload Failed', 'Silahkan coba lagi.');
            }
        });

        function readURL(input) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#<%=hdnBPJSUploadedFile.ClientID %>').val(e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
        //#endregion


        function calculateTotal() {
            var amount = parseFloat(0);
            var sepNo = "";
            var recordNo = "";
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var token = ",";
                    var newToken = "";
                    var value = $tr.find('.txtPaymentAmount').val().split(token).join(newToken);
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
                    var value = $tr.find('.txtPaymentAmount').val().split(token).join(newToken);
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

        function onCbpProcessDetailEndCallback() {
            $('.txtPaymentAmount').each(function () {
                $(this).trigger('changeValue');
            });
            calculateTotalWithChange();
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                if (retval != "Download Failed") {
                    downloadBPJSDocument(retval);
                } else {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');
                }
            }

            cbpProcessDetail.PerformCallback('refresh');

            $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val("");
            $('#<%=hdnSelectedGrouperCode.ClientID %>').val("");
            $('#<%=hdnSelectedPaymentAmount.ClientID %>').val("");
        }

        function onBeforeLoadRightPanelContent(code) {
            getCheckedMember();
            param = $('#<%=hdnRegistrationID.ClientID %>').val();
            return param;
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPaymentDetailID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedGrouperCode" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPaymentAmount" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowApprove" runat="server" value="0" />
    <input type="hidden" id="hdnBPJSUploadedFile" runat="server" value="" />
    <input type="hidden" id="hdnParamDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnParamIsExclusion" runat="server" value="" />
    <input type="hidden" id="hdnParamPaymentDate" runat="server" value="" />
    <input type="hidden" id="hdnIsUsedClaimFinal" runat="server" value="0" />
    <input type="hidden" id="hdnIsFinalisasiKlaimAfterARInvoice" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 60%" />
                <col style="width: 40%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <h4>
                        <%=GetLabel("Data Pencarian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 170px" />
                                <col style="width: 140px" />
                                <col style="width: 30px" />
                                <col style="width: 140px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                        Width="100%" />
                                </td>
                                <td align="left">
                                    <asp:CheckBox runat="server" ID="chkIsExclusion" Text=" Is Exclusion?" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Periode Pengakuan Piutang") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" />
                                </td>
                                <td align="center">
                                    <label>
                                        <%=GetLabel("s/d")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Nilai Klaim")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboGrouperAmountClaimFilter" ClientInstanceName="cboGrouperAmountClaimFilter"
                                        runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Penjamin Bayar")%></label>
                                </td>
                                <td align="left" colspan="3">
                                    <dxe:ASPxComboBox ID="cboBusinessPartner" ClientInstanceName="cboBusinessPartner"
                                        runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
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
                    <h4>
                        <%=GetLabel("Data Pengisian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 170px" />
                                <col style="width: 140px" />
                                <col style="width: 30px" />
                                <col style="width: 140px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label>
                                        <%=GetLabel("Tanggal Klaim")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtClaimDate" CssClass="datepicker" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="padding: 5px; vertical-align: top;">
                    <div>
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
                                                    <td id="Td1" runat="server">
                                                        <div style="text-align: center; width: 150px">
                                                            <b>
                                                                <%=GetLabel("Record")%></b>
                                                        </div>
                                                        <div runat="server" id="divTotalRecord" style="text-align: center; font-weight: bold;" />
                                                    </td>
                                                    <td id="Td2" runat="server">
                                                        <div style="text-align: center; width: 150px">
                                                            <b>
                                                                <%=GetLabel("Nomor SEP")%></b>
                                                        </div>
                                                        <div runat="server" id="divNoSEP" style="text-align: center; font-weight: bold;" />
                                                    </td>
                                                    <td id="Td3" runat="server">
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
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h4>
                        <%=GetLabel("Data Piutang")%></h4>
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                            ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
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
                                                            <%=GetLabel("No / Tgl Piutang")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl SEP")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Informasi Registrasi")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("COB")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Piutang (Awal)")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Grouper")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
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
                                                            <%=GetLabel("No / Tgl Piutang")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Informasi SEP & Rujukan")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Informasi Registrasi")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("COB")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Piutang (Awal)")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Grouper")%>
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
                                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PaymentDetailID")%>' />
                                                        <input type="hidden" class="PaymentID" id="PaymentID" runat="server" value='<%#: Eval("PaymentID")%>' />
                                                        <input type="hidden" class="RegistrationID" id="RegistrationID" runat="server" value='<%#: Eval("RegistrationID")%>' />
                                                        <input type="hidden" class="RegistrationNo" id="RegistrationNo" runat="server" value='<%#: Eval("RegistrationNo")%>' />
                                                        <input type="hidden" class="COBDetail" id="COBDetail" runat="server" value='<%#: Eval("COBDetail")%>' />
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("PaymentNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfPaymentDateInString") %></label>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <i>
                                                                <%=GetLabel("Jenis Klaim = ")%></i><%#: Eval("BPJSClaimType") %></label>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <label style="font-style: italic; font-size: x-small">
                                                                <%=GetLabel("No.SEP : ") %></label>
                                                            <label class="lblLink lblChangeSEP">
                                                                <%#: Eval("NoSEP") == "" ? "(+ SEP)" : Eval("NoSEP") %></label></div>
                                                        <div>
                                                            <label style="font-style: italic; font-size: x-small">
                                                                <%=GetLabel("No.Rujukan : ") %></label>
                                                            <label class="lblLink lblChangeReferral">
                                                                <%#: Eval("ReferralNo") == "" ? "(+ Rujukan)" : Eval("ReferralNo")%></label></div>
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
                                                    <td align="center">
                                                        <asp:CheckBox runat="server" Enabled="false" ID="chkIsUsingCOB" />
                                                        <br />
                                                        <label class="lblLink lblCOBDetail" <%# Eval("IsUsingCOB").ToString() == "True" ?  "" : "style='display:none'" %>>
                                                            <%=GetLabel("COB")%></label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtGrouperCode" Width="90%" runat="server" ReadOnly="true" CssClass="txtGrouperCode" />
                                                    </td>
                                                    <td align="right">
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfPaymentAmountInString") %></label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtPaymentAmount" Width="90%" runat="server" ReadOnly="true" CssClass="txtPaymentAmount txtCurrency" />
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
