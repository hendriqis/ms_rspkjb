<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientScheduledOrderCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientScheduledOrderCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_gridreigsteredpatientctl">
    $('.lvwViewOrder > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        if (!isHoverTdExpandOrder) {
            $('#<%=hdnTransactionOrderNo.ClientID %>').val($(this).find('.hdnVisitIDOrder').val());
            $('#<%=hdnTestOrderNo.ClientID %>').val($(this).find('.hdnTestOrderIDOrder').val());
        }
    });

    var isHoverTdExpandOrder = false;
    $('.lvwViewOrder tr:gt(0) td.tdExpand').live({
        mouseenter: function () { isHoverTdExpandOrder = true; },
        mouseleave: function () { isHoverTdExpandOrder = false; }
    });

    $('.lvwViewOrder tr:gt(0) td.tdExpand').live('click', function () {
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
        setPaging($("#paging2"), pageCount, function (page) {
            cbpViewOrder.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewOrderEndCallback(s) {
        hideLoadingPanel();
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#paging2"), pageCount, function (page) {
                cbpViewOrder.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdOrderPatient() {
        cbpViewOrder.PerformCallback('refresh');
    }
</script>

<input type="hidden" runat="server" id="hdnTransactionOrderNo" value="" />
<input type="hidden" runat="server" id="hdnTestOrderNo" value="" />

<dxcp:ASPxCallbackPanel ID="cbpViewOrder" runat="server" Width="100%" ClientInstanceName="cbpViewOrder"
    ShowLoadingPanel="false" OnCallback="cbpViewOrder_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e){ onCbpViewOrderEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridViewOrder" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                <asp:ListView runat="server" ID="lvwViewOrder" OnItemDataBound="lvwViewOrder_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblViewOrder" runat="server" class="grdCollapsible lvwViewOrder" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:15px"></th>
                                <th style="width:50px" align="center"></th>
                                <th style="width:250px" align="left"><%=GetLabel("INFORMASI ORDER")%></th>
                                <th style="width:250px" align="left"><%=GetLabel("INFORMASI REGISTRASI")%></th>
                                <th style="width:400px" align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th style="width:350px" align="left"><%=GetLabel("CATATAN")%></th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="6">
                                    <%=GetLabel("No Data To Display")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblViewOrder" runat="server" class="grdCollapsible lvwViewOrder" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:15px"></th>
                                <th style="width:50px" align="center"></th>
                                <th style="width:250px" align="left"><%=GetLabel("INFORMASI ORDER")%></th>
                                <th style="width:250px" align="left"><%=GetLabel("INFORMASI REGISTRASI")%></th>
                                <th style="width:400px" align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th style="width:350px" align="left"><%=GetLabel("CATATAN")%></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdExpand" style="text-align:center">
                                <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                            </td>
                            <td align="center">
                                <div><%#: Eval("ScheduledTime") %></div>                                           
                            </td>
                            <td>
                                <div><%#: Eval("TestOrderNo") %>
                                    <input type="hidden" class="hdnTestOrderIDOrder" value='<%#: Eval("TestOrderID") %>' />
                                    <br />
                                    <span runat="server" id="spnProcessed" class="spnProcessed">(<%=GetLabel("Sudah Diproses")%>)</span>
                                </div>                                                 
                            </td>
                            <td>
                                <div><%#: Eval("RegistrationNo") %> ( <%#:Eval("BedCode") %> )
                                    <input type="hidden" class="hdnVisitIDOrder" value='<%#: Eval("VisitID") %>' />
                                </div>                                                 
                            </td>
                            <td>
                                <div><%#: Eval("PatientName") %> (<%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %>, <%#: Eval("MedicalNo") %>)</div>                                           
                            </td>
                            <td><%#: Eval("Remarks") %></td>
                        </tr>
                        <tr style="display:none" class="trDetail">
                            <td><div> </div></td>
                            <td><div> </div></td>
                            <td>
                                <div>
                                    <div><%#: Eval("TestOrderNo") %></span></div>
                                    <input type="hidden" class="hdnTestOrderIDOrder" value='<%#: Eval("TestOrderID") %>' />
                                    <div style="float:left"><%#: Eval("TestOrderDateInString")%></div>
                                    <div style="margin-left:100px"><%#: Eval("TestOrderTime")%></div>
                                    <div><%#: Eval("ParamedicName")%></div>     
                                    <div><%#: Eval("ServiceUnitName")%></div>                                                                                      
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div><%#: Eval("RegistrationNo") %></span></div>
                                    <input type="hidden" class="hdnVisitIDOrder" value='<%#: Eval("VisitID") %>' />
                                    <div style="float:left"><%#: Eval("VisitDateInString")%></div>
                                    <div style="margin-left:100px"><%#: Eval("VisitTime")%></div>
                                    <div><%#: Eval("ParamedicName")%></div>                                                    
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
                                    <div><%#: Eval("Remarks")%></div>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
<div class="imgLoadingGrdView" id="containerImgLoadingView2">
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="paging2"></div>
    </div>
</div> 