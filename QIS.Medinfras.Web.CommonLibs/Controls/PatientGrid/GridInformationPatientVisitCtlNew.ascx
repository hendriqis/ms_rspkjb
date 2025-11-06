<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridInformationPatientVisitCtlNew.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridInformationPatientVisitCtlNew" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_gridreigsteredpatientctlnew">

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
        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    }
</script>
<style type="text/css">
    .openPatientColor{
        border:1px solid black;
        background-color:#EEEEEE;
    }

    .checkInPatientColor{
        border:1px solid black;
        background-color:#ffffcc;
    }
     
    .receivingTreatmentPatientColor{
        border:1px solid black;
        background-color:#ff66cc;
    }
    
    .physicanDischargePatientColor{
        border:1px solid black;
        background-color:#66a4ff;
    }        
    
    .dischargedPatientColor{
        border:1px solid black;
        background-color:#FFFF69;
    } 
    
    .cancelledPatientColor{
        border:1px solid black;
        background-color:#FF4545;
    }       
        
    .closedPatientColor{
        border:1px solid black;
        background-color:#55FF55;
    }
    
    .transferredPatientColor{
        border:1px solid black;
        background-color:#AAAAAA;
    }
    
    .transferredInpatientColor{
        border:1px solid black;
        background-color:#AAAAAA;
    }
</style>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
        OnClick="btnOpenTransactionDt_Click" />
</div>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                <table style="width:100%">
                    <tr>
                         <td style="min-width:8%">
                            <span class="openPatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="openVisitCounter" runat="server">Opened</span>
                        </td>
                        <td style="min-width:8%">
                            <span class="checkInPatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="checkInVisitCounter" runat="server">Checked-In</span>
                        </td>
                        <td style="min-width:10%">
                            <span class="receivingTreatmentPatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="receivingTreatmentVisitCounter" runat="server">Receiving Treatment</span>
                        </td>
                        <td style="min-width:10%">
                            <span class="physicanDischargePatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="physicanDischargeVisitCounter" runat="server">Physician Discharge</span>
                        </td>
                        <td style="min-width:8%">
                            <span class="dischargedPatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="dischargedVisitCounter" runat="server">Discharged</span>
                        </td>
                        <td style="min-width:8%">
                            <span class="cancelledPatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="cancelledVisitCounter" runat="server">Cancelled</span>
                        </td>
                        <td style="min-width:8%">
                            <span class="closedPatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="closedVisitCounter" runat="server">Closed</span>
                        </td>
                        <td style="min-width:8%" id="transferredPatientColor" runat="server">
                            <span class="transferredPatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="transferredVisitCounter" runat="server">Transferred</span>
                        </td>
                        <td style="min-width:10%">
                            <span class="transferredInpatientColor">&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            <span id="transferredInpatientCounter" runat="server">Transferred Inpatient</span>
                        </td>
                    </tr>
                </table>
                <asp:ListView runat="server" ID="lvwView">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>    
                                <th style="width:15px"></th>
                                <th style="width:250px"><%=GetLabel("Informasi Kunjungan")%></th>
                                <th style="width:400px"><%=GetLabel("Informasi Pasien")%></th>
                                <th style="width:350px"><%=GetLabel("Informasi Kontak")%></th>
                                <th style="width:250px"><%=GetLabel("Pembayar")%></th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="6">
                                    <%=GetLabel("No Data To Display")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:15px"></th>
                                <th style="width:250px"><%=GetLabel("Informasi Kunjungan")%></th>
                                <th style="width:400px"><%=GetLabel("Informasi Pasien")%></th>
                                <th style="width:350px"><%=GetLabel("Informasi Kontak")%></th>
                                <th style="width:250px"><%=GetLabel("Pembayar")%></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr style='<%#: Eval("GCVisitStatus") %>'>
                            <td class="tdExpand" style="text-align:center">
                                <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                            </td>
                            <td>
                                <div><%#: Eval("RegistrationNo") %></span>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>                                                 
                            </td>
                            <td>
                                <div><%#: Eval("PatientName") %> (<%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %>, <%#: Eval("MedicalNo") %>)</div>                                           
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <div><%#: Eval("BusinessPartner")%></div>
                            </td>
                        </tr>
                        <tr style="display:none" class="trDetail">
                            <td><div> </div></td>
                            <td>
                                <div>
                                    <div><%#: Eval("RegistrationNo") %></span></div>
                                    <div style = "color:Red">
                                        <%#: Eval("cfVoidReason") %></span>
                                    </div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                    <div style="float:left"><%#: Eval("VisitDateInString")%></div>
                                    <div style="margin-left:100px"><%#: Eval("VisitTime")%></div>
                                    <div><%#: Eval("ParamedicName")%></div>                                                    
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px">
                                    <img class="imgPatientImage" src='<%# Eval("PatientImageUrl") %>' alt="" height="55px" width="40px" style="float:left;margin-right: 10px;" />
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
        <div id="paging">
        </div>
    </div>
</div>