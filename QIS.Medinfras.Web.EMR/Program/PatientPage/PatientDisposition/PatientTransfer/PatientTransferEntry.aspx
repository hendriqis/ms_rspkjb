<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="PatientTransferEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientTransferEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div><%=GetLabel("Process")%></div></li>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnProcess.ClientID %>').click(function (evt) {
                if ($('#<%:hdnParamedicID.ClientID %>').val() == "") {
                    showToast("ERROR", 'Error Message : ' + "Dokter tujuan belum dipilih !");
                }
                else {
                    var message = "Lanjutkan proses transfer pasien ?";
                    showToastConfirmation(message, function (result) {
                        if (result) onCustomButtonClick('process');
                    });
                }
            });
        });

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var fromParamedicID = $('#<%:hdnFromParamedicID.ClientID %>').val();
            if (serviceUnitID == '')
                serviceUnitID = '0';
            var filterExpression = 'IsDeleted = 0';
            if (serviceUnitID != '0')
                filterExpression = "ParamedicID <> " + fromParamedicID + " AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ") AND IsDeleted = 0 AND GCParamedicMasterType = 'X019^001'";
            return filterExpression;
        }

        $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%:txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onCboTransferReasonChanged(s) {
            $txt = $('#<%=txtOtherReason.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onAfterCustomClickSuccess(type) {
            exitPatientPage();
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnFromParamedicID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td colspan="2">
                <table style="width:100%">
                    <colgroup width="70px" />
                    <colgroup />
                    <tr>
                        <td>
                            <img src='<%=ResolveUrl("~/Libs/Images/warning.png")%>' alt="" height="65px" width="65px" />
                        </td>
                        <td style="vertical-align:top;">
                            <h4 style="background-color:transparent;color:red;font-weight:bold"><%=GetLabel("PERINGATAN : Proses tidak bisa dibatalkan")%></h4>
                            <%=GetLabel("Jika terjadi kesalahan, pasien harus ditransfer ulang dari Dokter Tujuan ke Dokter Asal")%>
                            <br />
                            <%=GetLabel("Waktu yang dibutuhkan untuk proses transfer tergantung pada jumlah data pasien.")%>
                            <br />
                            <%=GetLabel("Pastikan Dokter Tujuan sudah benar.")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:200px"/>
                        <col style="width:150px"/>
                        <col style="width:150px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td colspan="4"><h4><%=GetLabel("DATA PASIEN YANG AKAN DITRANSFER :")%></h4></td>                        
                    </tr>
                    <tr>
                        <td><asp:CheckBox runat="server" id="chkVisitInformation" Text= "Data Kunjungan" Checked="true" Enabled="false" /></td>
                    </tr>
                    <tr style="display:none">
                        <td><asp:CheckBox runat="server" id="chkMedicalRecord" Text= "Catatan Rekam Medis" Checked="false" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox runat="server" id="chkPatientTransaction" Text= "Transaksi Pasien" /></td>  
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:200px"/>
                        <col style="width:120px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td colspan="4"><h4><%=GetLabel("DITRANSFER KE :")%></h4></td>                        
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory lblLink" runat="server" id="lblPhysician"><%=GetLabel("Dokter")%></label></td>
                        <td><asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtPhysicianName" Width="99%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Alasan Transfer")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboTransferReason" ClientInstanceName="cboTransferReason" Width="150px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboTransferReasonChanged(s); }" Init="function(s,e){ onCboTransferReasonChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOtherReason" CssClass="txtOtherReason" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>
