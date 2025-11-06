<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientMedicalRecordCodificationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientMedicalRecordCodificationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_GridPatientMedicalRecordCodificationCtl">
    $('.lvwView1 > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        if (!isHoverTdExpand) {
            showLoadingPanel();
            $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnVisitID').val());
            __doPostBack('<%=btnOpenTransactionDt1.UniqueID%>', '');
        }
    });

    var isHoverTdExpand = false;
    $('.lvwView1 tr:gt(0) td.tdExpand').live({
        mouseenter: function () { isHoverTdExpand = true; },
        mouseleave: function () { isHoverTdExpand = false; }
    });

    $('.lvwView1 tr:gt(0) td.tdExpand').live('click', function () {
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
    var pageCountRegOrder = parseInt('<%=PageCount %>');
    $(function () {
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        setPaging($("#paging3"), pageCountRegOrder, function (page) {
            cbpView1.PerformCallback('changepage|' + page);
        });

        var pageNo = $('#<%=hdnLastPagging3.ClientID %>').val();
        changePagePaggingNo3(pageNo);

        $('#containerDiagnosa').hide();
    });

    function oncbpView1EndCallback(s) {
        hideLoadingPanel();
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#paging3"), pageCount, function (page) {
                cbpView1.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdRegisteredPatientCodification() {
        cbpView1.PerformCallback('refresh');
    }

    function changePagePaggingNo3(idx) {
        var idx = idx - 1;
        $('.div3').find('li:eq(' + idx + ')').click();
    }

    function onBeforeOpenTransactionDt1() {
        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDt1" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt1();"
        OnClick="btnOpenTransactionDt1_Click" /></div>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<input type="hidden" runat="server" id="hdnLastPagging3" value="" />
<input type="hidden" runat="server" id="hdnDiagnosisNotOnlyMainDiagnosis" value="" />
<dxcp:ASPxCallbackPanel ID="cbpView1" runat="server" Width="100%" ClientInstanceName="cbpView1"
    ShowLoadingPanel="false" OnCallback="cbpView1_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpView1EndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 550px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwViewCount">
                    <EmptyDataTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all"
                            width="150px" style="float: right; margin-right: 10px">
                            <tr>
                                <th style="width: 150px; margin-right: 5px" align="right">
                                    <%=GetLabel("Jumlah Transaksi")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td style="width: 150px; margin-right: 5px; text-align: right">
                                    <%=GetLabel("0")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all"
                            width="150px" style="float: right; margin-right: 10px">
                            <tr>
                                <th style="width: 150px; margin-right: 5px" align="right">
                                    <%=GetLabel("Jumlah Transaksi")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 150px; text-align: right">
                                <div>
                                    <%#: Eval("TotalRow") %></span>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <asp:ListView runat="server" ID="lvwView1" OnItemDataBound="lvwView1_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView1" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 15px">
                                </th>
                                <th align="center">
                                    <%=GetLabel("NO. RM")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("INFORMASI PENDAFTARAN")%>
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("DOKTER | ALAMAT PASIEN & TELEPON")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("CC")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("TTV-ROS (O)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("P-Dx (ID)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("P-Dx (Text)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("MR-Dx (ID)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("MR-Dx (Text)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("PDC")%>
                                </th>
                                <th align="center">
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
                        <table id="tblView" runat="server" class="grdCollapsible lvwView1" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 15px">
                                </th>
                                <th align="center">
                                    <%=GetLabel("NO. RM")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("INFORMASI PENDAFTARAN")%>
                                </th>
                                <th style="width: 400px" align="left">
                                    <%=GetLabel("INFORMASI PASIEN")%>
                                </th>
                                <th style="width: 350px" align="left">
                                    <%=GetLabel("DOKTER | ALAMAT PASIEN & TELEPON")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("CC")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("TTV-ROS (O)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("P-Dx (ID)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("P-Dx (Text)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("MR-Dx (ID)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("MR-Dx (Text)")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("PDC")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("DC")%>
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
                            <td align="center">
                                <div style="font-weight: bold">
                                    <%#: Eval("MedicalNo") %></div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("RegistrationNo") %></span>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>
                                <div id="divMedicalFileStatus" runat="server" style="font-style: italic">
                                </div>
                            </td>
                            <td>
                                <div>
                                    <b>
                                        <%#: Eval("PatientName") %></b> (<%#: Eval("DateOfBirthInString") %>,
                                    <%#: Eval("Sex") %>)</div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("ParamedicName")%></div>
                            </td>
                            <td align="center">
                                <div id="divChiefComplaint" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divO" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divPDxID" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divPDxText" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divMRDxID" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divMRDxText" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divPhysicianDischarge" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divDischarge" runat="server" style="text-align: center; color: blue">
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
<div class="div3">
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="paging3">
            </div>
        </div>
    </div>
</div>
