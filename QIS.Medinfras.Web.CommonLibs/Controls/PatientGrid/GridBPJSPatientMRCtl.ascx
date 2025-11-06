<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridBPJSPatientMRCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridBPJSPatientMRCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_gridreigsteredpatientctl">
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
        setPaging($("#paging"), pageCountRegOrder, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });

        $('#containerDaftar').hide();
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
        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    }
</script>

<div style="display:none"><asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();" OnClick="btnOpenTransactionDt_Click" /></div>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />

<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:550px;overflow-y:scroll;">
                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:15px"></th>
                                <th style="width:100px" align="center"><%=GetLabel("NO. RM")%></th>
                                <th style="width:150px" align="center"><%=GetLabel("NO. PESERTA")%></th>
                                <th style="width:150px" align="center"><%=GetLabel("NO. SEP")%></th>
                                <th style="width:250px" align="left"><%=GetLabel("INFORMASI PENDAFTARAN")%></th>
                                <th style="width:400px" align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th align="left"><%=GetLabel("DOKTER - ALAMAT PASIEN & TELEPON")%></th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="8">
                                    <%=GetLabel("Tidak ada informasi pendaftaran pasien pada tanggal yang dipilih")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:15px"></th>
                                <th style="width:100px" align="center"><%=GetLabel("NO. RM")%></th>
                                <th style="width:150px" align="center"><%=GetLabel("NO. PESERTA")%></th>
                                <th style="width:150px" align="center"><%=GetLabel("NO. SEP")%></th>
                                <th style="width:250px" align="left"><%=GetLabel("INFORMASI PENDAFTARAN")%></th>
                                <th style="width:400px" align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th align="left"><%=GetLabel("DOKTER - ALAMAT PASIEN & TELEPON")%></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdExpand" style="text-align:center">
                                <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                            </td>
                            <td align="center"><div><%#: Eval("MedicalNo") %></div></td>
                            <td align="center"><div><%#: Eval("NoPeserta") %></div></td>
                            <td align="center"><div><%#: Eval("NoSEP") %></div></td>
                            <td>
                                <div><%#: Eval("RegistrationNo") %></span>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>                                                 
                            </td>
                            <td>
                                <div><%#: Eval("PatientName") %> (<%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %>)</div>                                           
                            </td>
                            <td><div><%#: Eval("ParamedicName")%></div></td>
                        </tr>
                        <tr style="display:none" class="trDetail">
                            <td><div></div></td>
                            <td><div></div></td>
                            <td><div></div></td>
                            <td><div>&nbsp;</div></td>
                            <td>
                                <div>
                                    <div><%#: Eval("NoSEP") %></div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <div style="float:left"><%#: Eval("VisitDateInString")%></div>
                                    <div style="margin-left:100px"><%#: Eval("VisitTime")%></div>                                          
                                    <div id="divDischargeDate" runat="server"></div>             
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px">
                                    <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px" width="40px" style="float:left;margin-right: 10px;" />
                                    <div><%#: Eval("PatientName") %></div>
                                    <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col style="width:10px"/>
                                            <col style="width:80px"/>
                                            <col style="width:50px"/>
                                            <col style="width:10px"/>
                                            <col style="width:120px"/>
                                        </colgroup>
                                        <tr>
                                            <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Nama Panggilan")%></td>
                                            <td>&nbsp;</td>
                                            <td><%#: Eval("PreferredName")%></td>
                                            <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("No RM")%></td>
                                            <td>&nbsp;</td>
                                            <td><%#: Eval("MedicalNo")%></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Tanggal Lahir")%></td>
                                            <td>&nbsp;</td>
                                            <td><%#: Eval("DateOfBirthInString")%></td>
                                            <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Umur")%></td>
                                            <td>&nbsp;</td>
                                            <td><%#: Eval("PatientAge")%></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px">
                                    <div><%#: Eval("HomeAddress")%></div>
                                    <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                    <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>     
                                </div>
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
        <div id="paging"></div>
    </div>
</div> 