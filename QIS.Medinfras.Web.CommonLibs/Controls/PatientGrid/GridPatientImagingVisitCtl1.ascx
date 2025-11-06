<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientImagingVisitCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientImagingVisitCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_gridreigsteredpatientctl">
    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        if (!isHoverTdExpand) {
            showLoadingPanel();
            $('#<%=hdnVisitID.ClientID %>').val($(this).find('.hdnVisitID').val());
            $('#<%=hdnTransactionID.ClientID %>').val($(this).find('.hdnTransactionID').val());
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
        return ($('#<%=hdnVisitID.ClientID %>').val() != '');
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
        OnClick="btnOpenTransactionDt_Click" /></div>
<input type="hidden" runat="server" id="hdnVisitID" value="" />
<input type="hidden" runat="server" id="hdnTransactionID" value="" />
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
                                </th>
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                </th>
                                <th style="width: 400px; text-align: left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("PENGIRIM")%>
                                </th>
                                <th style="width: 380px; text-align: left">
                                    <%=GetLabel("PEMERIKSAAN")%>
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                                <th style="width: 30px">
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="9">
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
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                </th>
                                <th style="width: 400px; text-align: left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 250px; text-align: left">
                                    <%=GetLabel("PENGIRIM")%>
                                </th>
                                <th style="width: 380px; text-align: left">
                                    <%=GetLabel("PEMERIKSAAN")%>
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
                            <td>
                                <div>
                                    <%#: Eval("RegistrationNo") %></span>
                                    <div>
                                        <%#: Eval("ParamedicName")%></div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <input type="hidden" class="hdnTransactionID" value='<%#: Eval("TransactionID") %>' />
                                </div>
                            </td>
                            <td>
                                <div>
                                    <b>
                                        <%#: Eval("PatientName") %></b> (<%#: Eval("cfDateOfBirth") %>,
                                    <%#: Eval("Sex") %>, <span style="color: Blue">
                                        <%#: Eval("MedicalNo") %>)</span></div>
                                <div>
                                    <%#: Eval("HomeAddress")%></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("ReferralPhysicianName")%></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("OrderDetail")%></div>
                            </td>
                            <td align="center">
                            </td>
                            <td id="tdOutstandingStatus" runat="server">
                                <div id="divOrderStatus" runat="server">
                                    <img id="imgOrderStatus" src='<%= ResolveUrl("~/Libs/Images/Toolbar/outstanding_order.png")%>'
                                        title="<%=GetLabel("Outstanding/Pending Order")%>" alt="" style="cursor: pointer"
                                        width="25" height="25" />
                                </div>
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
                                    <div>
                                        <%#: Eval("RegistrationNo") %></div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <div style="float: left">
                                        <%#: Eval("cfVisitDate")%></div>
                                    <div style="margin-left: 100px">
                                        <%#: Eval("VisitTime")%></div>
                                    <div>
                                        <%#: Eval("ServiceUnitName")%></div>
                                    <div>
                                        <%#: Eval("BusinessPartner")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <img class="imgPatientImage" src='<%#Eval("cfPatientImageUrl") %>' alt="" height="55px"
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
                                                <%#: Eval("cfDateOfBirth")%>
                                            </td>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Umur")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("cfPatientAge1")%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <div>
                                    &nbsp;</div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <div>
                                        <%#: Eval("TransactionNo")%></div>
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
        <div id="paging">
        </div>
    </div>
</div>
