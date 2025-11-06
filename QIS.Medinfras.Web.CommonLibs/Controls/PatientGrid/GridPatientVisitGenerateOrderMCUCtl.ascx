<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientVisitGenerateOrderMCUCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientVisitGenerateOrderMCUCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_GridPatientVisitGenerateOrderMCUCtl">
    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        if (!isHoverTdExpand) {
            showLoadingPanel();

            var paramClick = $(this).find('.hdnVisitID').val() + "|" + $(this).find('.hdnConsultVisitItemPackageID').val();
            $('#<%=hdnParamClick.ClientID %>').val(paramClick);
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
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdRegisteredPatient() {
        cbpView.PerformCallback('refresh');
    }

    function onBeforeOpenTransactionDt() {
        return ($('#<%=hdnParamClick.ClientID %>').val() != '');
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
        OnClick="btnOpenTransactionDt_Click" /></div>
<input type="hidden" runat="server" id="hdnParamClick" value="" />
<input type="hidden" runat="server" id="hdnIsDesktopMCU" value="" />
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
                                <th style="width: 30px">
                                    <%=GetLabel("ANTRIAN")%>
                                </th>
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                </th>
                                <th style="width: 400px; text-align: left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 350px; text-align: left">
                                    <%=GetLabel("INFORMASI KONTAK")%>
                                </th>
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("PEMBAYAR")%>
                                </th>
                                <th style="width: 200px; text-align: left">
                                    <%=GetLabel("PAKET MCU")%>
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="17">
                                    <%=GetLabel("Tidak ada kunjungan pada tanggal yang dipilih")%>
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
                                <th style="width: 30px">
                                    <%=GetLabel("ANTRIAN")%>
                                </th>
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                </th>
                                <th style="width: 400px; text-align: left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 350px; text-align: left">
                                    <%=GetLabel("INFORMASI KONTAK")%>
                                </th>
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("PEMBAYAR")%>
                                </th>
                                <th style="width: 200px; text-align: left">
                                    <%=GetLabel("PAKET MCU")%>
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
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
                            <td align="center" runat="server">
                                <div style="font-weight: bold">
                                    <%#: Eval("QueueNo") %></div>
                            </td>
                            <td>
                                <div>
                                    <b>
                                        <%#: Eval("RegistrationNo") %></b></span>
                                    <div>
                                        <%#: Eval("ParamedicName")%></div>
                                    <div>
                                        <%#: Eval("SpecialtyName")%></div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <input type="hidden" class="hdnConsultVisitItemPackageID" value='<%#: Eval("ConsultVisitItemPackageID") %>' />
                                </div>
                            </td>
                            <td>
                                <div>
                                    <b>
                                        <%#: Eval("PatientName") %></b> (<%#: Eval("cfDateOfBirthInString") %>,
                                    <%#: Eval("Sex") %>,
                                    <%#: Eval("MedicalNo") %>)</div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("HomeAddress")%></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("cfBusinessPartner")%></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("cfItemNameCodeMCU")%></div>
                            </td>
                            <td align="center">
                                <img class="imgLock <%# Eval("IsNewPatient").ToString() == "1" ? "imgDisabled" : "imgLink"%>"
                                    title='<%=GetLabel("New Patient")%>' src=' <%# ResolveUrl("~/Libs/Images/Status/done2.png") %>'
                                    style='<%# Eval("IsNewPatient").ToString() == "True" ? "width:25px": "width:25px;display:none" %>'
                                    alt="" />
                            </td>
                            <td align="center" id="tdOutstandingStatus" runat="server">
                                <img id="imgOrderStatus" src='<%= ResolveUrl("~/Libs/Images/Toolbar/outstanding_order.png")%>'
                                    title="<%=GetLabel("Outstanding/Pending Order")%>" alt="" style='<%# Eval("IsHasOutstandingTestOrder").ToString() == "True" ? "cursor: pointer;width:25px": "width:25px;display:none" %>' />
                            </td>
                            <td align="center">
                                <img id="imgFallRisk" src='<%= ResolveUrl("~/Libs/Images/Status/fall_risk.png")%>'
                                    title="<%=GetLabel("Resiko Jatuh")%>" alt="" style='<%# Eval("IsFallRisk").ToString() == "True" ? "cursor: pointer;width:24px": "width:24px;display:none" %>' />
                            </td>
                            <td align="center">
                                <img id="imgFastTrack" src='<%= ResolveUrl("~/Libs/Images/Status/FastTrack.png")%>'
                                    title="<%=GetLabel("Fast Track")%>" alt="" style='<%# Eval("IsFastTrack").ToString() == "True" ? "cursor: pointer;width:24px": "width:24px;display:none" %>' />
                            </td>
                            <td align="center">
                                <img class="imgLock <%# Eval("isLockDown").ToString() == "1" ? "imgDisabled" : "imgLink"%>"
                                    title='<%=GetLabel("TransactionLock")%>' src=' <%# ResolveUrl("~/Libs/Images/Toolbar/unlockdown.png") %>'
                                    style='<%# Eval("isLockDown").ToString() == "True" ? "width:25px": "width:25px;display:none" %>'
                                    alt="" />
                            </td>
                            <td align="center">
                                <img class="imgCOB <%# Eval("IsUsingCOB").ToString() == "1" ? "imgDisabled" : "imgLink"%>"
                                    title='<%# Eval("COB_Name")%>' src=' <%# ResolveUrl("~/Libs/Images/Status/cob.png") %>'
                                    style='<%# Eval("IsUsingCOB").ToString() == "True" ? "width:25px": "width:25px;height:25px;display:none" %>'
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
                                <div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <input type="hidden" class="hdnConsultVisitItemPackageID" value='<%#: Eval("ConsultVisitItemPackageID") %>' />
                                    <div style="float: left">
                                        <%#: Eval("cfVisitDateInString")%></div>
                                    <div style="margin-left: 100px">
                                        <%#: Eval("VisitTime")%></div>
                                    <div>
                                        <%#: Eval("ServiceUnitName")%></div>
                                    <div>
                                        <i>
                                            <%#: Eval("RegistrationStatus")%></i></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <img class="imgPatientImage" src='<%#Eval("cfPatientImageUrl") %>' alt="" height="55px"
                                        width="40px" style="float: left; margin-right: 10px;" />
                                    <div>
                                        <%#: Eval("cfPatientNameSalutation") %></div>
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
                                                <%#: Eval("cfDateOfBirthInString")%>
                                            </td>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Umur")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("cfPatientAge")%>
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
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="paging">
        </div>
    </div>
</div>
