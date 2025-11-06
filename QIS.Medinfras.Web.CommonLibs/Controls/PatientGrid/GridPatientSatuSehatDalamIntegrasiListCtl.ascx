<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientSatuSehatDalamIntegrasiListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientSatuSehatDalamIntegrasiListCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_GridPatientMedicalRecordCtl">
//    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
//        if (!isHoverTdExpand) {
//            showLoadingPanel();
//            $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnVisitID').val());
//            __doPostBack('<%=btnOpenTransactionDt2.UniqueID%>', '');
//        }
//    });

//    var isHoverTdExpand = false;
//    $('.lvwView tr:gt(0) td.tdExpand').live({
//        mouseenter: function () { isHoverTdExpand = true; },
//        mouseleave: function () { isHoverTdExpand = false; }
//    });

//    $('.lvwView tr:gt(0) td.tdExpand').live('click', function () {
//        $tr = $(this).parent().next();
//        if (!$tr.is(":visible")) {
//            $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
//            $tr.show('slow');
//        }
//        else {
//            $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
//            $tr.hide('fast');
//        }
    //    });

    //#region Paging2
    var pageCountRegOrder2 = parseInt('<%=PageCount %>');
    $(function () {
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        setPaging($("#paging2"), pageCountRegOrder2, function (page) {
            cbpViewDalamIntegrasi.PerformCallback('changepage|' + page);
        });

        var pageNo = $('#<%=hdnLastPagging.ClientID %>').val();
        changePagePaggingNo(pageNo);

        $('#containerDiagnosa').hide();
    });

    $('.lblHistoryLogBridgingDalamIntegrasi.lblLink').live('click', function () {
        $tr = $(this).closest('tr');
        var registrationID = $tr.find('.hdnKeyField').val();
        //        //        var encounterID = $tr.find('.hdnEncounterID').val();

        var url = ResolveUrl("~/Program/Process/IHS/_Popup/SatuSehatIntegrationLogCtl.ascx");
        var id = registrationID;
        openUserControlPopup(url, id, 'Log History Bridging', 1200, 500);
    });

    $('.chkIsSelectedDalamIntegrasi input').change(function () {
        $('.chkSelectAllDalamIntegrasi input').prop('checked', false);
    });

    $('#chkSelectAllDalamIntegrasi').die('change');
    $('#chkSelectAllDalamIntegrasi').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedDalamIntegrasi input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    function oncbpViewDalamIntegrasiEndCallback(s) {
        hideLoadingPanel();
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#paging2"), pageCount, function (page) {
                cbpViewDalamIntegrasi.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdDalamIntegrasi() {
        cbpViewDalamIntegrasi.PerformCallback('refresh');
    }

    function changePagePaggingNo(idx) {
        var idx = idx - 1;
        $('.div1').find('li:eq(' + idx + ')').click();
    }

    function onBeforeOpenTransactionDt2() {
        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    }

    function getListCheckedDalamIntegrasi() {
        var result = "";
        if ($('.chkIsSelectedDalamIntegrasi input:checked').length < 1) {
            result = "0|Harap pilih registrasi yang akan diproses";
        }
        else {
            var param = '';
            var lstBillID = "";
            $('.chkIsSelectedDalamIntegrasi input:checked').each(function () {
                var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                if (param != '')
                    param += '|';
                param += trxID;
            });
            var regID = param.replaceAll("|", ",");
            result = "true|" + regID;
        }

        return result;
    }
    function onChkSelectedChangeDalamIntegrasi() {
        $('#chkSelectAllDalamIntegrasi').prop('checked', false);
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDt2" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt2();"
        OnClick="btnOpenTransactionDt2_Click" /></div>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<input type="hidden" runat="server" id="hdnLastPagging" value="" />
<dxcp:ASPxCallbackPanel ID="cbpViewDalamIntegrasi" runat="server" Width="100%" ClientInstanceName="cbpViewDalamIntegrasi"
    ShowLoadingPanel="false" OnCallback="cbpViewDalamIntegrasi_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewDalamIntegrasiEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 550px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwViewCount">
                    <EmptyDataTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all" width="150px" style="float:right; margin-right:10px">
                            <tr>
                                <th style="width: 150px; margin-right:5px" align="right">
                                    <%=GetLabel("Jumlah Data")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td style="width: 150px; margin-right:5px; text-align:right">
                                    <%=GetLabel("0")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all" width="150px" style="float:right; margin-right:10px">
                            <tr>
                                <th style="width: 150px; margin-right:5px" align="right">
                                    <%=GetLabel("Jumlah Transaksi")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 150px; text-align:right">
                                <div>
                                    <%#: Eval("TotalRow") %></span>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 15px">
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("KIRIM KE SATUSEHAT?")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("STATUS INTEGRASI")%>
                                </th>
                                <th style="width: 50px" align="left">
                                    <%=GetLabel("Log Bridging")%>
                                </th>
                                <th align="center" style="width: 30px">
                                    <%=GetLabel("CC")%>
                                </th>
                                <th align="center" style="display:none">
                                    <%=GetLabel("TTV-ROS (O)")%>
                                </th>
                                <th align="center" style="display:none">
                                    <%=GetLabel("P-Dx (Text)")%>
                                </th>
                                <th align="center" style="width: 30px">
                                    <%=GetLabel("PDC")%>
                                </th>
                                <th align="center" style="width: 30px">
                                    <%=GetLabel("Dx RM (ID)")%>
                                </th>
                                <th align="center" style="display:none">
                                    <%=GetLabel("DC")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="30">
                                    <%=GetLabel("Tidak ada informasi pendaftaran pasien pada tanggal yang dipilih")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 15px">
                                    <input id="chkSelectAllDalamIntegrasi" type="checkbox" />
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("KIRIM KE SATUSEHAT?")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("STATUS INTEGRASI")%>
                                </th>
                                <th style="width: 50px" align="left">
                                    <%=GetLabel("Log Bridging")%>
                                </th>
                                <th align="center" style="width: 30px">
                                    <%=GetLabel("CC")%>
                                </th>
                                <th align="center" style="display:none">
                                    <%=GetLabel("TTV-ROS (O)")%>
                                </th>
                                <th align="center" style="display:none">
                                    <%=GetLabel("P-Dx (Text)")%>
                                </th>
                                <th align="center" style="width: 30px">
                                    <%=GetLabel("PDC")%>
                                </th>
                                <th align="center" style="width: 30px">
                                    <%=GetLabel("Dx RM (ID)")%>
                                </th>
                                <th align="center" style="display:none">
                                    <%=GetLabel("DC")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <div style="padding: 3px">
                                    <asp:CheckBox ID="chkIsSelectedDalamIntegrasi" CssClass="chkIsSelectedDalamIntegrasi" runat="server" onclick="onChkSelectedChangeDalamIntegrasi();"/>
                                    <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>
                            </td>
                            <td>
                                <div>Pasien : <b>(<%#: Eval("MedicalNo") %>) <%#: Eval("PatientName") %></b></div>
                                <div>IHS No : <b><%#: Eval("PatientIHSNumber") %></b></div>
                            </td>
                            <td>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                <div>
                                    <b><%#: Eval("RegistrationNo")%></b></div>
                                <div>
                                    Unit : <%#: Eval("ServiceUnitName")%></div>
                                <div>
                                    DPJP : <%#: Eval("ParamedicName")%></div>
                                <div>
                                    DPJP IHS No: <%#: Eval("ParamedicIHSNumber")%></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("PelepasanInformasiSatuSEHATInString")%></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("SatuSehatIntegrationStatus")%></div>
                            </td>
                            <td>
                                <label class="lblLink lblHistoryLogBridgingDalamIntegrasi" id="lblHistoryLogBridgingDalamIntegrasi">
                                        Log</label>
                            </td>
                            <td align="center">
                                <div id="divChiefComplaint" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divPhysicianDischarge" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divPDxID" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none" class="trDetail">
                            <td>
                                <div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    &nbsp;</div>
                            </td>
                            <td>
                                <div>
                                    <div>
                                        <%#: Eval("RegistrationNo") %></span></div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <div style="float: left">
                                        <%#: Eval("VisitDateInString")%></div>
                                    <div style="margin-left: 100px">
                                        <%#: Eval("VisitTime")%></div>
                                    <div id="divDischargeDate" runat="server">
                                    </div>
                                    <div style="float: left">
                                        <%#: Eval("DepartmentID")%></div>
                                    <div style="margin-left: 100px">
                                        <%#: Eval("ServiceUnitName")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                        width="40px" style="float: left; margin-right: 10px;" />
                                    <div>
                                        <%#: Eval("PatientName") %></div>
                                    <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 10px" />
                                            <col style="width: 80px" />
                                            <col style="width: 50px" />
                                            <col style="width: 10px" />
                                            <col style="width: 120px" />
                                        </colgroup>
                                        <tr>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Nama Panggilan")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("PreferredName")%>
                                            </td>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("No RM")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("MedicalNo")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Tanggal Lahir")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("DateOfBirthInString")%>
                                            </td>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Umur")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("PatientAge")%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <div>
                                        <%#: Eval("HomeAddress")%></div>
                                    <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float: left;" /><div
                                        style="margin-left: 30px">
                                        <%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                    <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float: left;" /><div
                                        style="margin-left: 30px">
                                        <%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    &nbsp;</div>
                            </td>
                            <td>
                                <div>
                                    &nbsp;</div>
                            </td>
                            <td>
                                <div>
                                    &nbsp;</div>
                            </td>
                            <td>
                                <div>
                                    &nbsp;</div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
<div class="imgLoadingGrdView" id="containerImgLoadingView">
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
<div class="div1">
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="paging2">
            </div>
        </div>
    </div>
</div>
