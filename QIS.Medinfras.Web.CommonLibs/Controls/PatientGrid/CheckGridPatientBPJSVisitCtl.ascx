<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckGridPatientBPJSVisitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.CheckGridPatientBPJSVisitCtl" %>
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
                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                    <emptydatatemplate>
                        <table id="tblView" runat="server" class="grdCollapsible" cellspacing="0" rules="all" >
                            <tr>
                                <%--<th style="width:15px"></th>--%>
                                <th style="width:30px"></th>
                                <th style="width:80px" align="center"><%=GetLabel("NO. RM")%></th>
                                <th style="width:100px" align="center"><%=GetLabel("NO. PESERTA")%></th>
                                <th style="width:100px" align="center"><%=GetLabel("NO. SEP")%></th>
                                <th style="width:300px" align="left"><%=GetLabel("NAMA PASIEN")%></th>
                                <th style="width:150px" align="left"><%=GetLabel("NO. REGISTRASI")%></th>
                                <th style="width:350px" align="left"><%=GetLabel("UNIT PELAYANAN")%></th>
                                <th align="left"><%=GetLabel("CATATAN")%></th>
                                <th style="width:80px" align="center"><%=GetLabel("STATUS SEP")%></th>
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
                                <th style="width:80px" align="center"><%=GetLabel("NO. RM")%></th>
                                <th style="width:100px" align="center"><%=GetLabel("NO. PESERTA")%></th>
                                <th style="width:100px" align="center"><%=GetLabel("TANGGAL SEP")%></th>
                                <th style="width:300px" align="left"><%=GetLabel("NAMA PASIEN")%></th>
                                <th style="width:150px" align="left"><%=GetLabel("NO. REGISTRASI")%></th>
                                <th style="width:350px" align="left"><%=GetLabel("UNIT PELAYANAN")%></th>
                                <th align="left"><%=GetLabel("CATATAN")%></th>
                                <th style="width:80px" align="center"><%=GetLabel("STATUS SEP")%></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </layouttemplate>
                    <itemtemplate>
                        <tr runat="server" id="trItem">
                            <td class="keyField"><%#: Eval("MRN")%></td>
                            <td align="center">
                                <div><asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" AutoPostBack="false" /></div>
                            </td>
                            <td align="center">
                                <div>
                                    <label class="lblMedicalNo lblLink"><%#: Eval("MedicalNo") %></label>                                   
                                </div>                                           
                            </td>
                            <td align="center">
                                <div>
                                    <label class="lblMedicalNo"><%#: Eval("NoPeserta") %></label>                                   
                                </div>                                           
                            </td>
                            <td align="center">
                                <div>
                                    <label class="lblMedicalNo"><%#: Eval("cfVisitDate") %></label>                                   
                                </div>                                           
                            </td>
                            <td>
                                <div><%#: Eval("PatientName") %> <br /> (<%#: Eval("DateOfBirthInString") %>, <%#: Eval("PatientAge") %>, <%#: Eval("Gender") %>)</div>                                           
                            </td>
                            <td>
                                <div ><%#: Eval("RegistrationNo") %> </span>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>                                                
                            </td>
                            <td>
                                <div><%#: Eval("ServiceUnitName") %></div>
                                <div><%#: Eval("ParamedicName") %></div>                                           
                            </td>
                            <td align="center">  
                                <div><%#: Eval("Catatan") %></div>                                     
                            </td>
                            <td align="center">  
                                <div><%#: Eval("SEPStatus") %></div>                                     
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
