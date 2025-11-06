<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckGridPatientAppointmentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.CheckGridPatientAppointmentCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<style type="text/css">
    .LvColor
    {
        background-color: Silver !important;
    }
</style>
<script type="text/javascript" id="dxss_gridreigsteredpatientctl">
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

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    function getCheckedRow() {
        var param = '';
        $('.chkIsSelected input:checked').each(function () {
            var AppointmentID = $(this).closest('tr').find('.hdnAppointmentID').val();
            if (param != '')
                param += ',';
            param += AppointmentID;
        });
        return param;
    }


    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        setPaging($("#paging1"), pageCount, function (page) {
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
            setPaging($("#paging1"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function getFilterExpressionGridCtl() {
        return $('#<%=hdnFilterExpressionGridCtl.ClientID %>').val();
    }

    function refreshGrdRegisteredPatient() {
        cbpView.PerformCallback('refresh');
    }
</script>
<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:panel runat="server" id="pnlGridView" style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                <input type="hidden" value="" id="hdnFilterExpressionGridCtl" runat="server" />
                <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
                <asp:listview runat="server" id="lvwView" onitemdatabound="lvwView_ItemDataBound">
                    <emptydatatemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>    
                                <th style="width:30px"></th>
                                <th style="width:250px"><%=GetLabel("No Appointment")%></th>
                                <th><%=GetLabel("Informasi Kunjungan")%></th>
                                <th style="width:150px"><%=GetLabel("Waktu Kunjungan")%></th>
                                <th style="width:350px"><%=GetLabel("Informasi Pasien")%></th>
                                <th style="width:100px"><%=GetLabel("Pasien Baru")%></th>
                             </tr>
                                <tr class="trEmpty">
                                    <td colspan="6">
                                        <%=GetLabel("No Data To Display")%>
                                    </td>
                                </tr>
                           </table>
                     </emptydatatemplate>
                     <layouttemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:30px">
                                <input id="chkSelectAll" type="checkbox" /></th>
                                <th style="width:200px"><%=GetLabel("No Appointment")%></th>
                                <th><%=GetLabel("Informasi Kunjungan")%></th>
                                <th style="width:140px"><%=GetLabel("Waktu Kunjungan")%></th>
                                <th style="width:400px"><%=GetLabel("Informasi Pasien")%></th>
                                <th style="width:80px"><%=GetLabel("Pasien Baru")%></th>
                             </tr>
                             <tr runat="server" id="itemPlaceholder" ></tr>
                          </table>
                     </layouttemplate>
                     <itemtemplate>
                        <tr runat="server" id="trItem">
                            <td class="keyField"><%#: Eval("AppointmentID")%></td>
                            <td align="center">
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black'":"" %>><asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" AutoPostBack="false" /></div>
                            </td>
                            <td align="center">
                                <div style="font-weight:bold">
                                    <%#: Eval("AppointmentNo") %>
                                </div>                                                 
                            </td>
                            <td align="center">
                                <div>
                                    <%#: Eval("ServiceUnitName") %> - <%#: Eval("ParamedicName") %> 
                                    <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID") %>' />
                                </div>                                                 
                             </td>
                             <td align="center">
                                <div><%#: Eval("StartDateTimeInString") %> </div>                                           
                             </td>
                             <td align="left">
                                <div><%#: Eval("cfMedicalNo") %> | <%#: Eval("PatientName") %>, <%#: Eval("Gender") %></div>                                           
                             </td>
                             <td align="center">
                                <div style="padding: 3px; text-align: center;">
                                    <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsNewPatient")%>'
                                        Enabled="false" />
                                 </div>
                            </td>
                        </tr>
                     </itemtemplate>
             </asp:listview>
        </asp:panel>
     </dx:PanelContent>
  </PanelCollection>
</dxcp:ASPxCallbackPanel>
<div class="imgLoadingGrdView" id="containerImgLoadingView">
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="paging1">
        </div>
    </div>
</div>