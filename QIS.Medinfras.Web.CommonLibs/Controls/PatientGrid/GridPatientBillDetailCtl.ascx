<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientBillDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientBillDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_gridpatientbilldetailctl">
    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        //if (!isHoverTdExpand) {
        $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnRegistrationID').val());
        //}
    });

    function getCurrentID() {
        return $('#<%=hdnTransactionNo.ClientID %>').val();
    }

    function getFilterExpression(filterExpression) {
        $row = $('.lvwView tr.selected');
        var registrationID = $row.find('.hdnRegistrationID').val();
        var linkedRegistrationID = $row.find('.hdnLinkedRegistrationID').val();
        if (registrationID == '') {
            return false;
        }
        else {
            filterExpression.text = 'RegistrationID = ' + registrationID;
            if (linkedRegistrationID != "" && linkedRegistrationID != "0") {
                filterExpression.text = '((RegistrationID = ' + linkedRegistrationID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';
            }
            return true;
        }
    }

    var isHoverTdExpand = false;
    $('.lvwView tr:gt(0) td.tdExpand').live({
        mouseenter: function () { isHoverTdExpand = true; },
        mouseleave: function () { isHoverTdExpand = false; }
    });

    $('.lvwView tr:gt(0):not(.trEmpty):not(.trDetail)').live('click', function () {
        $('.lvwView tr.selected').removeClass('selected');
        $(this).addClass('selected');
    });

    $('.lvwView tr:gt(0) td.tdExpand').live('click', function () {
        $tr = $(this).parent().next();
        if (!$tr.is(":visible")) {
            //$trCollapse = $('.trDetail').filter(':visible');
            //$trCollapse.hide();
            //$trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');

            $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
            $tr.show('slow');
        }
        else {
            $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
            $tr.hide('fast');
        }
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('.lvwView tr:eq(1)').click();
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('.lvwView tr:eq(1)').click();
    }
    //#endregion

    function refreshGrdRegisteredPatient() {
        cbpView.PerformCallback('refresh');
    }

    //    function onBeforeOpenTransactionDt() {
    //        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    //    }
</script>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
<ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
        EndCallback="function(s,e){ onCbpViewEndCallback(s); }"></ClientSideEvents>
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdSelected lvwView" cellspacing="0" rules="all">
                            <tr>
                                <th style="width: 15px">
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("Informasi Kunjungan")%>
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("Informasi Pasien")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("Informasi Kontak")%>
                                </th>
                                <th style="width: 130px" align="left">
                                    <%=GetLabel("Tanggal Rencana Pulang")%>
                                </th>
                                <th style="width: 100px" align="left">
                                    <%=GetLabel("Tanggal Pulang")%>
                                </th>
                                <th style="width: 270px" align="left">
                                    <%=GetLabel("Pembayar")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="10">
                                    <%=GetLabel("No Data To Display")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdSelected lvwView" cellspacing="0" rules="all">
                            <tr>
                                <th style="width: 15px;" align="left">
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("Informasi Kunjungan")%>
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("Informasi Pasien")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("Informasi Kontak")%>
                                </th>
                                <th style="width: 130px" align="left">
                                    <%=GetLabel("Tanggal Rencana Pulang")%>
                                </th>
                                 <th style="width:100px" align="left">
                                    <%=GetLabel("Tanggal Pulang")%>
                                </th>

                                <th style="width: 270px" align="left">
                                    <%=GetLabel("Pembayar")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdExpand" style="text-align: center">
                                <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("RegistrationNo") %>
                                    <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                    <input type="hidden" class="hdnLinkedRegistrationID" value='<%#: Eval("LinkedRegistrationID") %>' />
                                </div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("PatientName") %>
                                    (<%#: Eval("DateOfBirthInString") %>,
                                    <%#: Eval("Sex") %>,
                                    <%#: Eval("MedicalNo") %>)</div>
                            </td>
                            <td>
                                <div style="display: none">
                                    <%#: Eval("BusinessPartner")%></div>
                            </td>
                            <td style="text-align: center">
                                <div>
                                    <%#: Eval("cfPlanDischargeDate")%></div>
                            </td>
                            <td style="text-align: center">
                                <div>
                                    <%#: Eval("cfDischargeDate")%></div>
                            </td>
                            <td>
                                <div id="divBusinessPartnerName" runat="server">
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
                                    <div>
                                        <%#: Eval("RegistrationNo") %></span></div>
                                    <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                    <div style="float: left">
                                        <%#: Eval("RegistrationDateInString")%></div>
                                    <div style="margin-left: 100px">
                                        <%#: Eval("RegistrationTime")%></div>
                                    <div>
                                        <%#: Eval("ParamedicName")%></div>
                                    <div>
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
                                            <col style="width: 120px" />
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
                            &nbsp;
                         </td>
                         <td>
                            &nbsp;
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
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="paging">
        </div>
    </div>
</div>
