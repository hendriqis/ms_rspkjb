<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="DownloadPengisianHasilList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.DownloadPengisianHasilList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">    
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            setDatePicker('<%:txtDateRegistration1.ClientID %>');
            setDatePicker('<%:txtDateRegistration2.ClientID %>');

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

         
        //#endregion


        $('#btnSrc').live("click", function () {
            var Tanggal = $('#<%=txtDateRegistration1.ClientID %>').val();
            var instansi = $('#<%=hdnPayerID.ClientID %>').val();
            if (Tanggal != "" && instansi != "") {
                __doPostBack('<%=btnExport.UniqueID%>', '');
            }
            else {
                displayMessageBox('WARNING', 'Mohon di lengkapi untuk semua field');

            }

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
    #ctl00_ctl00_ctl00_plhMPBase_plhMPMain_btnMPEntrySave{display:none;}
    #ctl00_ctl00_ctl00_plhMPBase_plhMPMain_btnMPEntryNew{display:none;}
    
    </style>
    <div class="">
        <table width="40%">
        <col width="10px" />
            <tr>
                <td>  <label class="lblLink lblMandatory">Tanggal Registrasi</label></td>
                <td> 
                    <table>
                        <tr>
                            <td><asp:TextBox ID="txtDateRegistration1" Width="120px" runat="server"  CssClass="datepicker"/></td>
                            <td> - </td>
                            <td><asp:TextBox ID="txtDateRegistration2" Width="120px" runat="server"  CssClass="datepicker"/></td>
                        </tr>
                    </table>
                
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
                 <td>  <input type="button" class="w3-button" value="Download Data" id="btnSrc"  /></td>
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
   
     
      <div style="display: none;">
        <asp:Button ID="btnExport" Visible="true" runat="server" OnClick="btnExport_Click"
            Text="Export" UseSubmitBehavior="false" OnClientClick="return true;" />
        
    </div>

</asp:Content>