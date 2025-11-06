<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="MCUExternalList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUExternalList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
 
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Proses")%></div>
    </li>
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">    
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            setDatePicker('<%:txtDateRegistration.ClientID %>');
        });
       
        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            cbpViewDetail.PerformCallback();
        }

        function onCboPayerValueChanged(s) {

        }
        $('#<%:lblPayerCompany.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
                $('#<%:txtPayerCompanyCode.ClientID %>').val(value);
                onTxtPayerCompanyCodeChanged(value);
            });
        });

        $('#<%:txtPayerCompanyCode.ClientID %>').live('change', function () {
            if ($(this).val() != "") {
                onTxtPayerCompanyCodeChanged($(this).val());
            }
            else {
                $('#<%:hdnPayerID.ClientID %>').val('');
                $('#<%:txtPayerCompanyName.ClientID %>').val('');
                
            }
        });

        function getPayerCompanyFilterExpression() {
            var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
            if (cboRegistrationPayer.GetValue() == "X004^999") {
                filterExpression = "GCCustomerType = '" + cboRegistrationPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
            }
           
            return filterExpression;
        }

        function onTxtPayerCompanyCodeChanged(value) {
            var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            getPayerCompany(filterExpression);
        }

        function getPayerCompany(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getPayerCompanyFilterExpression();

            Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                var messageBlacklistPayer = '<font size="4">' + 'Rekanan Sedang dilakukan Penutupan Layanan Sementara,' + '<br/>' + ' untuk sementara dilakukan sebagai' + '<b>' + ' PASIEN UMUM' + '</b>' + '</font>';
                if (result != null) {
                    $('#<%:hdnIsBlacklistPayer.ClientID %>').val(result.IsBlackList);
                    if ($('#<%:hdnIsBlacklistPayer.ClientID %>').val() == 'false') {
                        $('#<%:hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:txtPayerCompanyCode.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%:txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                        $('#<%:hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
                        $('#btnPayerNotesDetail').removeAttr('enabled');                        
                    }
                }
                else {
                    $('#<%:hdnIsBlacklistPayer.ClientID %>').val('0');
                    $('#<%:hdnPayerID.ClientID %>').val('');
                    $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                    $('#<%:txtPayerCompanyName.ClientID %>').val('');
                    $('#btnPayerNotesDetail').attr('enabled', 'false');
                    $('#<%:hdnGCTariffScheme.ClientID %>').val('');   
                }
            });
        }

        function getPayerContractFilterExpression() {
            var filterExpression = "BusinessPartnerID = " + $('#<%:hdnPayerID.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
            return filterExpression;
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            if (param[0] == 'process') {
                if (param[1] == "0") {
                    displayMessageBox('SUCCESS', 'Generate Order Sucess');
                    $('#<%=hdnSelectRegistrationID.ClientID %>').val();
                } else {
                    displayMessageBox('Failed', param[2]);
                }

                cbpView.PerformCallback('refresh');
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('#<%=btnProcess.ClientID %>').live("click", function () {
            getCheckedRegistrationID();
            var RegistrationID = $('#<%=hdnSelectRegistrationID.ClientID %>').val();
            if (RegistrationID == "") {
                
                displayMessageBox('Failed', 'Silahkan dipilih dahulu data yang akan diproses');
                return false;
            } else {
                cbpView.PerformCallback('process');
            }

        });
        $('#btnSrc').live("click", function () {
            cbpView.PerformCallback('refresh');

        });
        
        function getCheckedRegistrationID() {
            var lstSelectRegistrationID = $('#<%=hdnSelectRegistrationID.ClientID %>').val().split(',');

            $('.chkProcess input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var oRegistrationID = $tr.find('.hdnRegistrationID').val();

                    var idx = lstSelectRegistrationID.indexOf(oRegistrationID);
                    if (idx < 0) {
                        lstSelectRegistrationID.push(oRegistrationID);                       
                    }                     
                }
                else {
                    var oRegistrationID = $(this).closest('tr').find('.hdnRegistrationID').val();
                    var idx = lstSelectRegistrationID.indexOf(oRegistrationID);
                    if (idx > -1) {
                        lstSelectRegistrationID.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectRegistrationID.ClientID %>').val(lstSelectRegistrationID.join(','));

        }

        $('#chkAll').live('click', function () {
            $('input:checkbox').prop('checked', this.checked);
        });
    </script>
    <style>
        /*hide button crud */
        #ctl00_ctl00_ctl00_plhMPBase_plhMPMain_btnMPListAdd{ display:none;}
        #ctl00_ctl00_ctl00_plhMPBase_plhMPMain_btnMPListEdit{display:none;}
        #ctl00_ctl00_ctl00_plhMPBase_plhMPMain_btnMPListDelete{display:none;}
    </style>
    <div class="">
        <table>
            <tr>
                <td>Tanggal Registrasi</td>
                <td> 
                <asp:TextBox ID="txtDateRegistration" Width="120px" runat="server"  CssClass="datepicker"/>
                </td>
            </tr>
             <tr>
                                <td colspan="2">
                                    <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                </td>
                            </tr>
              <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%:GetLabel("Pembayar")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 3px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRegistrationPayer" ClientInstanceName="cboRegistrationPayer"
                                                    Width="100%" runat="server">
                                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trPayer">
                                <td style="width: 30%" class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblPayerCompany">
                                        <%:GetLabel("Instansi")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPayerID" value="" runat="server" />
                                    <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                                    <input type="hidden" id="hdnIsBlacklistPayer" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" ReadOnly=true />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                           
             <tr> <td></td>
                 <td>  <input type="button" class="w3-button" value="Cari Data" id="btnSrc"  /></td>
            </tr>
        </table>
    </div>
    <br />
    <input type="hidden" value="" id="hdnSelectRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnDefaultItemIDMCUPackage" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingRegistrationParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnGCShift" runat="server" />
    <input type="hidden" value="" id="hdnGCCashierGroup" runat="server" />            
                
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail')); showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                              <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input type="checkbox" id="chkAll" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkProcess" runat="server" CssClass="chkProcess" />
                                                   <input type="hidden" class="hdnRegistrationID" value="<%#:Eval("RegistrationID") %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="RegistrationNo" HeaderText="Nomor Registrasi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfGuestName" HeaderText = "Nama" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ParamedicName" HeaderText = "Dokter Registrasi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfRegistrationDate" HeaderText="Tanggal Registrasi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="StatusSync" HeaderText="Status Sync" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
     
</asp:Content>