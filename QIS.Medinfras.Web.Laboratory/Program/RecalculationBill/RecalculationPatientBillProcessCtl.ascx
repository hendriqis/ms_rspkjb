<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecalculationPatientBillProcessCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Laboratory.Program.RecalculationPatientBillProcessCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx" TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtProductViewCtl.ascx" TagName="DrugMSCtl" TagPrefix="uc1" %>

<script type="text/javascript" id="dxss_patiententryctl">
    $('#btnRecalculateOk').click(function () {
        if (confirm('Are You Sure?')) {
            var param = '';
            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                if (param != '')
                    param += '|';
                param += $tr.find('.hdnKeyField').val();
            });
            $('#<%=hdnParam.ClientID %>').val(param);
            cbpRecalculationPatientBillProcess.PerformCallback();
        }
    });

    function onCbpRecalculationPatientBillProcessEndCallback(s) {
        $('#ulTabRecalculatePatientBillProcess li').click(function () {
            $('#ulTabRecalculatePatientBillProcess li.selected').removeAttr('class');
            $('.containerRecalculationProcess').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });
        hideLoadingPanel();
    }

    $('#ulTabRecalculatePatientBillProcess li').click(function () {
        $('#ulTabRecalculatePatientBillProcess li.selected').removeAttr('class');
        $('.containerRecalculationProcess').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

    function onBeforeSaveRecord(errMessage) {
        var param = '';
        $('.chkIsSelectedCtl input:checked').each(function () {
            var $td = $(this).parent().parent();
            var input = $td.find('.hdnKeyField').val();
            if (param != "") {
                param += "|";
            }
            param += input;
        });
        if (param != "") {
            $('#<%=hdnParam.ClientID %>').val(param);
            return true;
        }
        else {
            errMessage.text = 'Pilih Item Terlebih Dahulu';
            return false;
        }
    }
</script>
<style type="text/css">
    .containerRecalculationProcess                   { height: 280px;overflow-y:auto; border: 1px solid #EAEAEA; }
</style>

<input type="hidden" value="" id="hdnParam" runat="server" />
<div class="pageTitle"><%=GetLabel("Recalculate Patient Bill")%></div>
<div>
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnLinkedRegistrationID" value="" />
    <div style="text-align: left; width: 100%; margin-bottom: 10px;">
        <table>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width:250px"/>
                        </colgroup>
                        <tr>
                            <td><%=GetLabel("Include Variable Tariff")%></td>
                            <td><asp:CheckBox ID="chkIsIncludeVariableTariff" runat="server" /></td>
                        </tr>        
                        <tr>
                            <td><%=GetLabel("Hitung Ulang Berdasarkan Buku Tarif")%></td>
                            <td><asp:CheckBox ID="chkIsResetItemTariff" runat="server" /></td>
                        </tr>                                              
                    </table>  
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <input type="button" id="btnRecalculateOk" value='<%= GetLabel("Recalculate")%>' />
                </td>
                <td>
                    <input type="button" id="btnRecalculateClose" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
                </td>
            </tr>
        </table>
    </div>        

    <dxcp:ASPxCallbackPanel ID="cbpRecalculationPatientBillProcess" runat="server" Width="100%" ClientInstanceName="cbpRecalculationPatientBillProcess"
        ShowLoadingPanel="false" OnCallback="cbpRecalculationPatientBillProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
            EndCallback="function(s,e){ onCbpRecalculationPatientBillProcessEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <div class="containerUlTabPage">
                    <ul class="ulTabPage" id="ulTabRecalculatePatientBillProcess">
                        <li class="selected" contentid="containerService"><%=GetLabel("Pelayanan") %></li>
                    </ul>
                </div>

                <div id="containerService" class="containerRecalculationProcess">
                    <uc1:ServiceCtl ID="ctlService" runat="server" />
                </div>

                <table style="width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:15%"/>
                        <col style="width:35%"/>
                        <col style="width:15%"/>
                        <col style="width:35%"/>
                    </colgroup>
                    <tr>
                        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total") %> : </div></td>
                        <td style="text-align:right;padding-right: 10px;">
                            Rp. <asp:TextBox ID="txtTotal" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Instansi") %> : </div></td>
                        <td style="text-align:right;padding-right: 10px;">
                            Rp. <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                        </td>
                        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Pasien") %>  : </div></td>
                        <td style="text-align:right;padding-right: 10px;">
                            Rp. <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                        </td>
                    </tr>
                </table> 
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>