<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="ARFinalClaimv2New.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARFinalClaimv2New" %>

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

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
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

        $('.lblRegCountPerSEP').die('click');
        $('.lblRegCountPerSEP').live('click', function () {
            $tr = $(this).closest('tr');
            var oNoSEP = $tr.find('.NoSEP').val();
            var url = ResolveUrl('~/Program/BPJS/ARClaim/DetailRegistrationInfoPerNoSEPCtl.ascx');
            openUserControlPopup(url, oNoSEP, 'Informasi Registrasi per No SEP', 1200, 500);
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

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            $('.txtGrouperAmountFinal').each(function () {
                $(this).trigger('changeValue');
            });

            var totalRecord = $('#<%=hdnTotalRecord.ClientID %>').val();
            var totalSEP = $('#<%=hdnTotalSEP.ClientID %>').val();
            var totalGrouper = $('#<%=hdnTotalGrouper.ClientID %>').val();

            $('#<%=divTotalRecord.ClientID %>').html(totalRecord);
            $('#<%=divNoSEP.ClientID %>').html(totalSEP);
            $('#<%=divGrouperAmount.ClientID %>').html(totalGrouper);

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }

        function oncbpProcessDetailV2EndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] != '') {
                    displayErrorMessageBox('Save Failed', param[1]);
                }
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
        }

        function onAfterPopupControlClosing() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                if (retval != "Download Failed") {
                    downloadBPJSDocument(retval);
                } else {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');
                }
            }

            $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val("");
            $('#<%=hdnSelectedGrouperCode.ClientID %>').val("");
            $('#<%=hdnSelectedPaymentAmount.ClientID %>').val("");
            $('#<%=hdnSelectedFeedbackStatus.ClientID %>').val("");
        }

        $btnSave = null;
        $('.btnSave').live('click', function () {
            $tr = $(this).closest('tr');
            var keyField = $tr.find('.keyField').val();
            var grouperCode = $tr.find('.txtGrouperCodeFinal').val();
            var paymentAmount = parseFloat(parseFloat($tr.find('.txtGrouperAmountFinal').attr('hiddenVal')).toFixed(2));
            var rowIndex = $tr.find('.hdnRowIndex').val();
            var cboFeedbackStatus = eval('cboFeedbackStatus' + rowIndex);
            var feedbackStatus = cboFeedbackStatus.GetValue();

            if (feedbackStatus != null) {
                var param = 'save|' + keyField + '|' + feedbackStatus + '|' + grouperCode + '|' + paymentAmount;
                cbpProcessDetailV2.PerformCallback(param);
            }
            else {
                displayErrorMessageBox('Save Failed', 'Harap Isi Status Umpan Balik Terlebih Dahulu.');
            }
        });
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPaymentDetailID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedGrouperCode" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPaymentAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedFeedbackStatus" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnBPJSUploadedFile" runat="server" value="" />
    <input type="hidden" id="hdnParamDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnParamIsExclusion" runat="server" value="" />
    <input type="hidden" id="hdnParamPaymentDate" runat="server" value="" />
    <input type="hidden" id="hdnIsUsedClaimFinal" runat="server" value="0" />
    <input type="hidden" id="hdnIsFinalisasiKlaimAfterARInvoice" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
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
                                <td colspan="2" style='display: none'>
                                    <asp:CheckBox runat="server" ID="chkAllThisStatus" Text=" Pilih Semua Data di Bawah dengan Status Ini ?" />
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
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <input type="hidden" id="hdnTotalRecord" runat="server" value="0" />
                                    <input type="hidden" id="hdnTotalSEP" runat="server" value="0" />
                                    <input type="hidden" id="hdnTotalGrouper" runat="server" value="0" />
                                    <asp:Panel runat="server" ID="panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Piutang")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl SEP")%>
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
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Pembayaran")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Grouper")%>
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
                                                        <th>
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
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Pembayaran")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Kode Klaim Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("Nilai Klaim Grouper")%>
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
                                                        <th>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <input type="hidden" class="hdnRowIndex" value='<%#: Container.DataItemIndex %>' />
                                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PaymentDetailID")%>' />
                                                        <input type="hidden" class="PaymentID" id="PaymentID" runat="server" value='<%#: Eval("PaymentID")%>' />
                                                        <input type="hidden" class="RegistrationID" id="RegistrationID" runat="server" value='<%#: Eval("RegistrationID")%>' />
                                                        <input type="hidden" class="NoSEP" id="NoSEP" runat="server" value='<%#: Eval("NoSEP")%>' />
                                                        <input type="hidden" class="RegistrationNo" id="RegistrationNo" runat="server" value='<%#: Eval("RegistrationNo")%>' />														
                                                        <input type="hidden" class="COBDetail" id="COBDetail" runat="server" value='<%#: Eval("COBDetail")%>' />
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
                                                                <%#: Eval("NoSEP") == "" ? "(+ SEP)" : Eval("NoSEP") %></label>
                                                            <br />
                                                            <label class="lblLink lblRegCountPerSEP" style="font-style: italic; font-size: x-small">
                                                                <%=GetLabel("Jmlh Reg per NoSEP : ")%><%#: Eval("CountRegistrationPerSEP") %></label>
                                                        </div>
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
                                                    <td align="center">
                                                        <dxe:ASPxComboBox ID="cboFeedbackStatus" CssClass="cboFeedbackStatus" runat="server"
                                                            Width="90%" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtGrouperCodeFinal" Width="90%" runat="server"
                                                            CssClass="txtGrouperCodeFinal" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtGrouperAmountFinal" Width="90%" runat="server"
                                                            CssClass="txtGrouperAmountFinal txtCurrency" />
                                                    </td>
                                                    <td align="center">
                                                        <input type="button" id="btnSave" class="btnSave" value="Simpan" runat="server" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
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
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpProcessDetailV2" runat="server" Width="100%" ClientInstanceName="cbpProcessDetailV2"
            ShowLoadingPanel="false" OnCallback="cbpProcessDetailV2_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { oncbpProcessDetailV2EndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
