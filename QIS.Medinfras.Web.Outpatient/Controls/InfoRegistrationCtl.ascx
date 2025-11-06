<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoRegistrationCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Outpatient.Controls.InfoRegistrationCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_patientvisitctl">
    function onCboInfoRegistrationServiceUnitValueChanged(s) {
        cbpInfoRegistrationView.PerformCallback('refresh');
    }
    
    setDatePicker('<%=txtInfoRegistrationRegistrationDate.ClientID %>');
    $('#<%=txtInfoRegistrationRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#<%=txtInfoRegistrationRegistrationDate.ClientID %>').change(function (evt) {
        cbpInfoRegistrationView.PerformCallback('refresh');
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingInfoRegistrationView"), pageCount, function (page) {
            cbpInfoRegistrationView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpInfoRegistrationViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#pagingInfoRegistrationView"), pageCount, function (page) {
                cbpInfoRegistrationView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion


    $('.lvwInfoRegistration tr:gt(0) td.tdExpand').die('click');
    $('.lvwInfoRegistration tr:gt(0) td.tdExpand').live('click', function () {
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
</script>
<table>
    <colgroup>
        <col style="width:150px"/>
        <col/>
    </colgroup>
    <tr>
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Registrasi")%></label></td>
        <td><asp:TextBox ID="txtInfoRegistrationRegistrationDate" Width="120px" runat="server" CssClass="datepicker" /></td>
    </tr>
    <tr>
        <td><label><%=GetLabel("Klinik") %></label></td>
        <td>
            <dxe:ASPxComboBox ID="cboInfoRegistrationServiceUnit" ClientInstanceName="cboInfoRegistrationServiceUnit" Width="200px" runat="server">
                <ClientSideEvents ValueChanged="function(s,e) { onCboInfoRegistrationServiceUnitValueChanged(s); }" />
            </dxe:ASPxComboBox>
        </td>
    </tr>
</table>

<div style="position: relative;">
    <dxcp:ASPxCallbackPanel ID="cbpInfoRegistrationView" runat="server" Width="100%" ClientInstanceName="cbpInfoRegistrationView"
        ShowLoadingPanel="false" OnCallback="cbpInfoRegistrationView_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
            EndCallback="function(s,e){ onCbpInfoRegistrationViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:320px;font-size:1em;">
                    <asp:ListView runat="server" ID="lvwView">
                        <EmptyDataTemplate>
                            <table id="tblView" runat="server" class="grdCollapsible lvwInfoRegistration" cellspacing="0" rules="all" >
                                <tr>
                                    <th style="width:15px"></th>
                                    <th style="width:220px"><%=GetLabel("Informasi Registrasi")%></th>
                                    <th style="width:400px"><%=GetLabel("Informasi Pasien")%></th>
                                    <th style="width:270px"><%=GetLabel("Informasi Kontak")%></th>
                                    <th><%=GetLabel("Pembayar")%></th>
                                </tr>
                                <tr class="trEmpty">
                                    <td colspan="6">
                                        <%=GetLabel("No Data To Display")%>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="tblView" runat="server" class="grdCollapsible lvwInfoRegistration" cellspacing="0" rules="all" >
                                <tr>
                                    <th style="width:15px"></th>
                                    <th style="width:220px"><%=GetLabel("Informasi Registrasi")%></th>
                                    <th style="width:400px"><%=GetLabel("Informasi Pasien")%></th>
                                    <th style="width:270px"><%=GetLabel("Informasi Kontak")%></th>
                                    <th><%=GetLabel("Pembayar")%></th>
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
                                    <div><%#: Eval("RegistrationNo") %>
                                    <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                    </div>                                                 
                                </td>
                                <td>
                                    <div><%#: Eval("PatientName") %> (<%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %>, <%#: Eval("MedicalNo") %>)</div>                                           
                                </td>
                                <td><div style="display:none"><%#: Eval("BusinessPartner")%></div></td>
                                <td>
                                    <div><%#: Eval("BusinessPartner")%></div>
                                </td>
                            </tr>
                            <tr style="display:none" class="trDetail">
                                <td><div> </div></td>
                                <td>
                                    <div>
                                        <div><%#: Eval("RegistrationNo") %></span></div>
                                        <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                        <div style="float:left"><%#: Eval("RegistrationDateInString")%></div>
                                        <div style="margin-left:100px"><%#: Eval("RegistrationTime")%></div>
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
                                                <col style="width:120px"/>
                                                <col style="width:50px"/>
                                                <col style="width:10px"/>
                                                <col style="width:120px"/>
                                            </colgroup>
                                            <tr>
                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Preferred Name")%></td>
                                                <td>&nbsp;</td>
                                                <td><%#: Eval("PreferredName")%></td>
                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("MRN")%></td>
                                                <td>&nbsp;</td>
                                                <td><%#: Eval("MedicalNo")%></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("DOB")%></td>
                                                <td>&nbsp;</td>
                                                <td><%#: Eval("DateOfBirthInString")%></td>
                                                <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Age")%></td>
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
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="pagingInfoRegistrationView"></div>
        </div>
    </div> 
</div>