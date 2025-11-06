<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridImagingResultCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridImagingResultCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_gridpatientresultctl">
    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        if (!isHoverTdExpand) {
            showLoadingPanel();
            $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnTransactionID').val());
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

    function refreshGrdResultPatient() {
        cbpView.PerformCallback('refresh');
    }

    function onBeforeOpenTransactionDt() {
        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    }
</script>
<%--<div style="float:right">
    <table class="tblCount" runat="server">
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Jumlah Transaksi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTransactionCount" Width="100px" runat="server" ReadOnly="true"
                    CssClass="number" />
            </td>
        </tr>
    </table>
</div>--%>
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
                <asp:ListView runat="server" ID="lvwViewCount">
                    <EmptyDataTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all" width="150px" style="float:right; margin-right:10px">
                            <tr>
                                <th style="width: 150px; margin-right:5px" align="right">
                                    <%=GetLabel("Jumlah Transaksi")%>
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
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("NO TRANSAKSI")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("NO REGISTRASI")%>
                                </th>
                                <th style="width: 100px" align="center">
                                    <%=GetLabel("TANGGAL")%>
                                </th>
                                <th style="width: 90px" align="center">
                                    <%=GetLabel("NO RM")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("NAMA PASIEN")%>
                                </th>
                                <th style="width: 200px" align="left">
                                    <%=GetLabel("PEMERIKSAAN")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("DOKTER PELAKSANA")%>
                                </th>
                                <th style="width: 120px" align="left">
                                    <%=GetLabel("ASAL PASIEN")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="15">
                                    <%=GetLabel("Tidak ada Data Pemeriksaan / Pelayanan Pasien")%>
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
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("NO TRANSAKSI")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("NO REGISTRASI")%>
                                </th>
                                <th style="width: 100px" align="center">
                                    <%=GetLabel("TANGGAL")%>
                                </th>
                                <th style="width: 90px" align="center">
                                    <%=GetLabel("NO RM")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("NAMA PASIEN")%>
                                </th>
                                <th style="width: 200px" align="left">
                                    <%=GetLabel("PEMERIKSAAN")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("DOKTER PELAKSANA")%>
                                </th>
                                <th style="width: 120px" align="left">
                                    <%=GetLabel("ASAL PASIEN")%>
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
                                    <%#: Eval("TransactionNo") %></span>
                                    <input type="hidden" class="hdnTransactionID" value='<%#: Eval("TransactionID") %>' />
                                    <span runat="server" id="spnProcessed" class="spnProcessed">(<%=GetLabel("Sudah Diproses")%>)</span>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("RegistrationNo") %></div>
                            </td>
                            <td align="center">
                                <div>
                                    <%#: Eval("TransactionDateInString") %></div>
                            </td>
                            <td align="center">
                                <div>
                                    <%#: Eval("MedicalNo") %></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("PatientName") %></span>
                                    <br>
                                    <span><%#: Eval("BusinessPartner") %></span></br>
                                    </div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("OrderDetail")%>
                                    </div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("ParamedicDetail")%></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("VisitDepartmentID") %></div>
                            </td>
                        </tr>
                        <tr style="display: none" class="trDetail">
                            <td>
                                <div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <input type="hidden" class="hdnTransactionID" value='<%#: Eval("TransactionID") %>' />
                                    <div style="float: left">
                                        <%#: Eval("TransactionDateInString")%></div>
                                    <div style="margin-left: 100px">
                                        <%#: Eval("TransactionTime")%></div>
                                    <div>
                                        <%#: Eval("ParamedicName")%></div>
                                    <div>
                                        <%#: Eval("ServiceUnitName")%></div>
                                    <div>
                                        <%#: Eval("BusinessPartner")%></div>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                        width="40px" style="float: left; margin-right: 10px;" />
                                    <div>
                                        <%#: Eval("PatientName") %>
                                        ,
                                        <%#: Eval("MedicalNo") %></div>
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
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic" width="200px">
                                                <%=GetLabel("Nama Panggilan")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("PreferredName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Tanggal Lahir")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="3">
                                                <%#: Eval("DateOfBirthInString")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float: left;" /><div
                                                    style="margin-left: 30px">
                                                    <%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float: left;" /><div
                                                    style="margin-left: 30px">
                                                    <%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
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
