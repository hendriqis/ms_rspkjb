<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="PatientVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.PatientVisitList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');

            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = '<%=GetServiceUnitUserRoleFilterParameter() %>;' + cboPatientFrom.GetValue() + ';';
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitroleuser', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getHealthcareServiceUnitFilterExpression() + "ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetServiceUnitUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                    cbpView.PerformCallback('refresh');
                });
            }


        });

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
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onBeforeOpenTransactionDt() {
            return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
        }

        function onCboPatientFromValueChanged() {
            var cboValue = cboPatientFrom.GetValue();
            if (cboValue == Constant.Facility.INPATIENT)
                $('#trRegistrationDate').attr('style', 'display:none');
            else
                $('#trRegistrationDate').removeAttr('style');

            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');

            cbpView.PerformCallback('refresh');
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="padding:15px">
        <div id="containerDaftar" class="containerOrder">
            <div class="pageTitle"><%=GetLabel("Asuhan Keperawatan")%> : <%=GetLabel("Pilih Pasien")%></div>
            <fieldset id="fsPatientListReg"> 
                <table class="tblContentArea" style="width:100%">
                    <tr>
                        <td style="padding:5px;vertical-align:top">
                            <table class="tblEntryContent" style="width:60%;">
                                <colgroup>
                                    <col style="width:25%"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server" Width="100%">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }"/>
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink lblNormal" id="lblServiceUnit"><%=GetLabel("Unit Pelayanan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:120px"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trRegistrationDate">
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                                    <td><asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker"/></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg" ID="txtSearchViewReg"
                                            Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding:7px 0 0 3px;font-size:0.95em">
                                <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%> 
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                                                            <th style="width:15px"></th>
                                                            <th style="width:450px" align="left"><%=GetLabel("PASIEN")%></th>
                                                            <th style="width:350px" align="left"><%=GetLabel("INFORMASI KUNJUNGAN")%></th>
                                                            <th align="left"><%=GetLabel("ALAMAT DAN KONTAK PASIEN")%></th>
                                                            <th style="width:15px"></th>
                                                            <th style="width:15px"></th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="8">
                                                                <%=GetLabel("Tidak ada data kunjungan pasien")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                                                        <tr>
                                                            <th style="width:15px"></th>
                                                            <th style="width:15px"></th>
                                                            <th style="width:450px" align="left"><%=GetLabel("PASIEN")%></th>
                                                            <th style="width:350px" align="left"><%=GetLabel("INFORMASI KUNJUNGAN")%></th>
                                                            <th align="left"><%=GetLabel("ALAMAT DAN KONTAK PASIEN")%></th>
                                                            <th style="width:15px"></th>
                                                            <th style="width:15px"></th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder" ></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="tdExpand" style="text-align:center">
                                                            <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                                        </td>       
                                                        <td align="center" id="tdIndicator" runat="server">
                                                        </td>                                                                  
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="center" valign="top">
                                                                        <div <%# Eval("DepartmentID").ToString() != "OUTPATIENT" ? "Style='display:none'":"" %>>
                                                                            <img id="imgPatientSatisfactionLevelImageUri" runat="server" width="24" height="24" alt="" visible="true" />    
                                                                        </div>
                                                                    </td> 
                                                                    <td>
                                                                        <div style="text-align:left"><img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px" width="40px" style="float:left;margin-right: 10px;" /></div>
                                                                        <div><span style="font-weight: bold; font-size:11pt"><%#: Eval("PatientName") %></span></div>
                                                                        <div><%#: Eval("MedicalNo") %>, <%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %></div>
                                                                        <div style="font-style:italic"><%#: Eval("BusinessPartner")%></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>                                                                                                                                                                                                               
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <%#: Eval("RegistrationNo") %>
                                                                        <div <%#:Eval("DepartmentID").ToString() != "INPATIENT" ? "style='display:none'":"" %>><%#: Eval("cfPatientLocation") %></div>
                                                                        <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />        
                                                                        <div><%#: Eval("ParamedicName")%></div>                                                                                                                
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <div><%#: Eval("HomeAddress")%></div>
                                                            <div style="padding:3px">
                                                                <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                                                <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>     
                                                            </div>
                                                        </td>
                                                        <td  align="center">
                                                            <div id="divChiefComplaint" runat="server" style="text-align:center;color:blue" ></div>
                                                        </td>
                                                        <td align="center">
                                                            <div id="divDiagnosis" runat="server" style="text-align:center;color:blue" ></div>
                                                        </td>
                                                    </tr>
                                                    <tr style="display:none" class="trDetail">
                                                        <td><div> </div></td>
                                                        <td><div>&nbsp;</div></td>
                                                        <td>
                                                            <div style="padding:3px">
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
                                                            <div>
                                                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                <div style="float:left"><%#: Eval("VisitDateInString")%></div>
                                                                <div style="margin-left:100px"><%#: Eval("VisitTime")%></div>                                                 
                                                                <div id="divDischargeDate" runat="server"></div>             
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td><div>&nbsp;</div></td>
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
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
</asp:Content>
