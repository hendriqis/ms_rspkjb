<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientResultCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientResultCtl1" %>

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

<div style="display:none"><asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();" OnClick="btnOpenTransactionDt_Click" /></div>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />

<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:15px"></th>
                                <th style="width:350px" align="left" ><%=GetLabel("INFORMASI TRANSAKSI")%></th>
                                <th style="width:330px" align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th style="width:330px" align="left"><%=GetLabel("INFORMASI REGISTRASI")%></th>
                                <th align="left"></th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="6">
                                    <%=GetLabel("Tidak ada Data Pemeriksaan / Pelayanan Pasien")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:15px"></th>
                                <th style="width:350px" align="left" ><%=GetLabel("INFORMASI TRANSAKSI")%></th>
                                <th style="width:330px" align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th style="width:330px" align="left"><%=GetLabel("INFORMASI REGISTRASI")%></th>
                                <th align="left"></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdExpand" style="text-align:center">
                                <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                            </td>
                            <td>
                                <div style="font-weight: bold; font-size: 11pt"><%#: Eval("TransactionNo") %></div> 
                                <div><%#: Eval("TransactionDateInString")%>, <%#: Eval("TransactionTime")%></div>
                                <div><%#: Eval("ParamedicName")%></div>
                                <div><%#: Eval("ServiceUnitName")%></div>          
                                <br />
                                <input type="hidden" class="hdnTransactionID" value='<%#: Eval("TransactionID") %>' />
                                    <span runat="server" id="spnProcessed" class="spnProcessed">(<%=GetLabel("Sudah Diproses")%>)</span>                                                
                            </td>
                            <td>
                                <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px" width="40px" style="float:left;margin-right: 10px;" />
                                <div>
                                    <span style="font-weight: bold; font-size: 11pt">
                                        <%#: Eval("cfPatientNameSalutation") %>
                                    </span>
                                </div>
                                <div><%#: Eval("MedicalNo") %>, <%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %> </div>                                           
                            </td>
                            <td>
                                <div><%#: Eval("RegistrationNo") %></span>
                                <div>
                                    <%#: Eval("ParamedicName")%></div>
                                <div>
                                    <%#: Eval("VisitServiceUnitName")%>
                                    <%#: Eval("BedCode") %></div>
                                <div><%#: Eval("BusinessPartner")%></div>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>                                                 
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr style="display:none" class="trDetail">
                            <td><div> </div></td>
                            <td>
                                <div>
                                    <div><%#: Eval("OrderDetail") %></span></div>                                          
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px">
                                    <div style="padding:3px">
                                        <div><%#: Eval("HomeAddress")%></div>
                                        <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                        <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>     
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div><%#: Eval("RegistrationNo") %></span></div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <div style="float:left"><%#: Eval("VisitDateInString")%></div>
                                    <div style="margin-left:100px"><%#: Eval("VisitTime")%></div>
                                    <div><%#: Eval("ParamedicName")%></div>                                                    
                                </div>
                            </td>
                            <td>

                            </td>
                            <td><div>&nbsp;</div></td>
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