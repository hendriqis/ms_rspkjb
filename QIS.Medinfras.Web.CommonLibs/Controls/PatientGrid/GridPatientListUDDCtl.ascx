<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientListUDDCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientListUDDCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_GridPatientListUDDCtl">
    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        if (!isHoverTdExpand) {
            showLoadingPanel();
            $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnVisitID').val());
            __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
        }
    });

    var isHoverTdExpand = false;
    $('.lvwView tr:gt(0) td.tdExpand').live({
        mouseenter: function () { isHoverTdExpand = true; },
        mouseleave: function () { isHoverTdExpand = false; }
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
    var pageCountRegOrder = parseInt('<%=PageCount %>');
    $(function () {
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        setPaging($("#pagingListUDDCtl"), pageCountRegOrder, function (page) {
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
            setPaging($("#pagingListUDDCtl"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdRegisteredPatient() {
        cbpView.PerformCallback('refresh');
    }

    function onBeforeOpenTransactionDt() {
        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
        OnClick="btnOpenTransactionDt_Click" /></div>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 15px">
                                </th>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("NO. BED")%>
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("DATA PASIEN")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("DATA KUNJUNGAN")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("ALAMAT DAN KONTAK PASIEN")%>
                                </th>
                                <th style="width: 30px">
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="6">
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
                                </th>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("NO. BED")%>
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("DATA PASIEN")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("DATA KUNJUNGAN")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("ALAMAT DAN KONTAK PASIEN")%>
                                </th>
                                <th style="width: 30px">
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
                            <td class='tdBedStatus<%#:Eval("GenderCodeSuffix")%>'>
                                <div style="font-size: 13pt; font-weight: bold">
                                    <%#: Eval("BedCode") %></div>
                            </td>
                            <td>
                                <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                    width="40px" style="float: left; margin-right: 10px;" />
                                <div>
                                    <span style="font-weight: bold; font-size: 11pt">
                                        <%#: Eval("cfPatientNameSalutation") %>
                                    </span>
                                </div>
                                <div>
                                    <%#: Eval("MedicalNo") %>,
                                    <%#: Eval("DateOfBirthInString") %>,
                                    <%#: Eval("Sex") %>,
                                    <%#: Eval("Religion") %>
                                </div>
                                <div style="font-style: italic">
                                    <%#: Eval("BusinessPartner")%></div>
                                <div class="blink-alert">
                                    <font color="red">
                                        <%#: Eval("cfTextDischargeDateOrPlanDischargeDateWithNotes") %></font>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("RegistrationNo") %>
                                    <div>
                                        <%#: Eval("ParamedicName")%></div>
                                    <div>
                                        <label>
                                            <%#: Eval("ServiceUnitName")%>,
                                            <%#: Eval("ClassName") %>
                                            <label style="font-weight: bold;">
                                                (<%#: Eval("cfInpatientLOS") %>)</label>
                                        </label>
                                    </div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("HomeAddress")%></div>
                            </td>
                            <td align="center">
                                <img class="imgLock <%# Eval("isLockDown").ToString() == "1" ? "imgDisabled" : "imgLink"%>"
                                    title='<%=GetLabel("TransactionLock")%>' src=' <%# ResolveUrl("~/Libs/Images/Toolbar/unlockdown.png") %>'
                                    style='<%# Eval("isLockDown").ToString() == "True" ? "width:25px": "width:25px;display:none" %>'
                                    alt="" />
                            </td>
                        </tr>
                        <tr style="display: none" class="trDetail">
                            <td>
                                <div>
                                </div>
                            </td>
                            <td>
                                <div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
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
                                <div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <div style="float: left">
                                        <%#: Eval("VisitDateInString")%></div>
                                    <div style="margin-left: 100px">
                                        <%#: Eval("VisitTime")%></div>
                                    <div>
                                        <%#: Eval("SpecialtyName")%></div>
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
        <div id="pagingListUDDCtl">
        </div>
    </div>
</div>
