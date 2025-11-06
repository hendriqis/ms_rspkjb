<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckGridPatientVisitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.CheckGridPatientVisitCtl" %>
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

    function getCheckedRow() {
        var param = '';
        $('.chkIsSelected input:checked').each(function () {
            var visitID = $(this).closest('tr').find('.hdnVisitID').val();
            if (param != '')
                param += ',';
            param += visitID;
        });
        return param;
    }


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

    function getFilterExpressionGridCtl() {
        return $('#<%=hdnFilterExpressionGridCtl.ClientID %>').val();
    }

    function refreshGrdRegisteredPatient() {
        cbpView.PerformCallback('refresh');
    }
</script>
<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 550px; overflow-y: scroll;">
                <input type="hidden" value="" id="hdnFilterExpressionGridCtl" runat="server" />
                <asp:ListView runat="server" ID="lvwViewCount">
                    <EmptyDataTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all" width="150px" style="float:right; margin-right:10px">
                            <tr>
                                <th style="width: 150px; margin-right:5px" align="right">
                                    <%=GetLabel("Jumlah Pasien")%>
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
                                    <%=GetLabel("Jumlah Pasien")%>
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
                    <emptydatatemplate>
                        <table id="tblView" runat="server" class="grdCollapsible" cellspacing="0" rules="all" >
                            <tr>
                                <%--<th style="width:15px"></th>--%>
                                <th style="width:30px"></th>
                                <th style="width:50px" align="center"><%=GetLabel("NO . RM")%></th>
                                <th style="width:200px" align="left"><%=GetLabel("NAMA PASIEN")%></th>
                                <th style="width:70px" align="center"><%=GetLabel("JAM DAFTAR")%></th>
                                <th style="width:200px" align="left"><%=GetLabel("NO. REGISTRASI")%></th>
                                <th style="width:250px" align="left"><%=GetLabel("UNIT PELAYANAN")%></th>
                                <th style="width:80px" align="center"><%=GetLabel("STATUS PASIEN")%></th>
                                <th style="width:80px" align="left"><%=GetLabel("STATUS BERKAS")%></th>
                                <th style="width:30px" align="center"><%=GetLabel("Dx")%></th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="9">
                                    <%=GetLabel("Tidak ada informasi pendaftaran pada tanggal tersebut")%>
                                </td>
                            </tr>
                        </table>
                    </emptydatatemplate>
                    <layouttemplate>
                        <table id="tblView" runat="server" class="grdCollapsible" cellspacing="0" rules="all" >
                            <tr>
                                <%--<th style="width:15px"></th>--%>
                                <th style="width:30px"></th>
                                <th style="width:50px" align="center"><%=GetLabel("NO. RM")%></th>
                                <th style="width:200px" align="left"><%=GetLabel("NAMA PASIEN")%></th>
                                <th style="width:70px" align="center"><%=GetLabel("JAM DAFTAR")%></th>
                                <th style="width:200px" align="left"><%=GetLabel("NO. REGISTRASI")%></th>
                                <th style="width:250px" align="left"><%=GetLabel("UNIT PELAYANAN")%></th>
                                <th style="width:80px" align="center"><%=GetLabel("STATUS PASIEN")%></th>
                                <th style="width:80px" align="left"><%=GetLabel("STATUS BERKAS")%></th>
                                <th style="width:30px" align="center"><%=GetLabel("Dx")%></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </layouttemplate>
                    <itemtemplate>
                        <tr runat="server" id="trItem">
                            <td class="keyField"><%#: Eval("MRN")%></td>
                            <td align="center">
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black'":"" %>><asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" AutoPostBack="false" /></div>
                            </td>
                            <td align="center">
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black; font-weight:bold'":"" %>>
                                    <label class="lblMedicalNo lblLink"><%#: Eval("MedicalNo") %></label>                                   
                                </div>                                           
                            </td>
                            <td>
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black'":"" %>><%#: Eval("PatientName") %> <br /> (<%#: Eval("DateOfBirthInString") %>, <%#: Eval("PatientAge") %>, <%#: Eval("Gender") %>)</div>                                           
                            </td>
                            <td align="center">
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black'":"" %>><%#: Eval("VisitTime") %> </div>                                           
                            </td>
                            <td>
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:blue'":"" %>><%#: Eval("RegistrationNo") %> </span>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>                                                
                                <div>
                                    <%=GetLabel("No. Appointment : ")%> <%#: Eval("cfAppointmentNo") %>
                                </div> 
                            </td>
                            <td>
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black'":"" %>><%#: Eval("ServiceUnitName") %> <br /><%#: Eval("BedCode") %></div>
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black'":"" %>><%#: Eval("ParamedicName") %></div>                                           
                            </td>
                            <td align="center">
                                <div <%# Eval("IsNewPatient").ToString() != "True" ? "Style='color:black'":"" %>><%#: Eval("IsNewPasien") %></div>                                           
                            </td>
                            <td>
                                <div>
                                    <label class="lblRemarks lblLink"><%#: Eval("cfLastMRFileTransporterName") %></label>   
                                </div>
                                <div>
                                    <%#: Eval("cfLastMRFileLogDateTime") %>
                                </div>
                            </td>
                            <td align="center">
                                <div><input type="checkbox" class="chkIsHasPhysicianDiagnosis" style="width:20px" /></div>
                            </td>
                        </tr>
                    </itemtemplate>
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
