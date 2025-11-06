<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MCUExternalProcessCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUExternalProcessCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<script type="text/javascript" id="dxss_erewrwerwerwectl">
    $(function () {
        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVisitID.ClientID %>').val($(this).find('.VisitID').val());
                $('#<%=hdnDefaultRegistrationParamedicID.ClientID %>').val($(this).find('.hdnRegistrationParamedicID').val());                
                cbpGeneratOrderDtView.PerformCallback('refresh');
            }
        });

        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $('#<%=grdView.ClientID %> tr:eq(1)').click();


        $('#<%=btnProcess.ClientID %>').click(function () {
            var param = '';
            $('.chkIsSelectedID input:checked').each(function () {
                var VisitID = $(this).closest('tr').find('.keyField').html();
                if (param != '')
                    param += ',';
                param += VisitID;
            });

            if (param == "") {
                showToast('Warning', 'Please Select Item Request First');
            }
            else {
                $('#<%=hdnSelectedAppointmentRequestID.ClientID %>').val(param);
                cbpGeneratOrderView.PerformCallback('GenerateOrder');
            }
        });
    });

    

    $('#btnUpdateParamedic').live('click',function () {
    var ParamedicID  =$('#<%:hdnParamedicID.ClientID %>').val();
      <%--   var serviceUnitID = cboServiceUnit.GetValue(); --%>
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if(ParamedicID == "" || serviceUnitID == ""){
        showToast('Warning', 'Silahkan di isi dahulu');
        }else{
        cbpCboView.PerformCallback('update');
        }                
    });
    
    $('#chkAll').live('click',function () {
        $('input:checkbox').prop('checked', this.checked);
    });

    $('#lblBatchNo').live('click', function () {
        var filterexpresion = "RequestBatchNo IS NOT NULL AND GCVisitStatus IN ('X020^002','X020^003') AND GCItemDetailStatus = 'X121^001' AND  DepartmentID = 'MCU'";
        openSearchDialog('appointmentRequestBatchNo', filterexpresion, function (value) {
            onRequestBatchNo(value);
        });
    });
  
     $('#lblParanedic').live('click', function () {
        var filterexpresion = " HealthcareServiceUnitID='"+ $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val() +"' AND IsDeleted =0";
        openSearchDialog('serviceUnitParamedicCodeMaster', filterexpresion, function (value) {
            onParamedic(value);
        });
    });

    $('#lblSeriveUnit').live('click', function () {

        var HsuID = $('#<%=hdnlstHealthcareServiceUnitID.ClientID %>').val();
        if (HsuID == "") {
            showToast('Warning', 'Silahkan dipilih dahulu batch no.');
            return false;
        }
        openSearchDialog('serviceunitperhealthcare', ' HealthcareServiceUnitID IN (' + HsuID + ') and IsDeleted = 0', function (value) {
            onTxtSeriveUnitChanged(value);
        });
    });

    function onTxtSeriveUnitChanged(value) {
        var filterExpression = "ServiceUnitCode='" + value + "'";
        Methods.getObject("GetvHealthcareServiceUnitList", filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                resetFormParamedic();
            } else {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
                 resetFormParamedic();
            }
        });
    }
    function resetHSU(){
         $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
         $('#<%:txtServiceUnitCode.ClientID %>').val('');
         $('#<%:txtServiceUnitName.ClientID %>').val('');
    }
    $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
        onTxtSeriveUnitChanged($(this).val());
    });

    $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
        onParamedic($(this).val());
    });
    function onParamedic(value){
          var filterExpression = "ParamedicCode='"+value + "'";
          
          Methods.getObject("GetvParamedicMasterList", filterExpression, function (result) {
                if(result != null){
                 
                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtParamedicCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:txtParamedicName.ClientID %>').val(result.ParamedicName);
                }else{
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtParamedicCode.ClientID %>').val('');
                    $('#<%:txtParamedicName.ClientID %>').val('');
                }
          });
    }

   
    function resetFormParamedic() {
        $('#<%:hdnParamedicID.ClientID %>').val('');
        $('#<%:txtParamedicCode.ClientID %>').val('');
        $('#<%:txtParamedicName.ClientID %>').val('');
    }
    function onRequestBatchNo(value) {
        if (value != "") {
            $('#<%:hdnBatchNo.ClientID %>').val(value);
            $('#<%:txtBatchNo.ClientID %>').val(value);
            cbpGeneratOrderView.PerformCallback('changebatch');
            resetFormParamedic();
        } else {
            $('#<%:hdnBatchNo.ClientID %>').val('');
            $('#<%:txtBatchNo.ClientID %>').val('');
        }
    }

    function onCbpGeneratOrderViewEndCallback(s) {        
        var param = s.cpResult.split('|');
        if (param[0] == 'GenerateOrder') {
            if (param[1] == 'fail') {
                showToast("Error", param[2]);
                cbpGeneratOrderView.PerformCallback('refresh');
            } else {
                displayMessageBox('SUCCESS', 'Upload Pasien Berhasil');
                pcRightPanelContent.Hide()
            }
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

        refreshGrdRegisteredPatient();
        hideLoadingPanel();
    }

    function refreshGrdRegisteredPatient() {       
        onRefreshGridView();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }

    function onCbpGeneratOrderDtViewEndCallback(s){
     hideLoadingPanel();
    }

  

     $('.lblParamedicName.lblLink').live('click', function () {
                var hdnKey = $(this).closest('tr').find('.hdnKey').val();
                $('#<%=hdnKeyDtSession.ClientID %>').val(hdnKey);
                $td = $(this).parent();
                var paramedicID = $td.children('.hdnParamedicID').val();
                openSearchDialog('paramedic', 'IsDeleted = 0', function (value) {
                    onTxtParamedicChanged(value, $td);
                });
            });

    function onTxtParamedicChanged(value, $td) {
                var filterExpression = "ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $td.find('.lblParamedicName').html(result.ParamedicName);
                        $td.find('.hdnParamedicID').val(result.ParamedicID);
                        
                        $('#<%=hdnParamedicIDDtSession.ClientID %>').val(result.ParamedicID);
                        cbpGeneratOrderDtView.PerformCallback('changeParamedic');
                    }
                });
            }
                
    function onCbpCboViewEndCallback(s){
     var param = s.cpResult.split('|');
        if (param[0] == 'update') {
            if (param[1] == 'fail') {
                showToast("Error", param[2]);
                cbpGeneratOrderView.PerformCallback('refresh');
            } else {
                 cbpGeneratOrderView.PerformCallback('refresh');

                 
            }
        }
        resetHSU();
         resetFormParamedic();
             hideLoadingPanel();
    }
    function getCbo(){
        cbpCboView.PerformCallback('refresh');
    }
    
        
       
</script>
<style>
    .divcbo
    {
        width: 100%;
        height: 100px;
    }
</style>
<input type="hidden" runat="server" id="hdnSelectedVisitID" value="" />
<input type="hidden" runat="server" id="hdnSelectedAppointmentRequestID" value="" />
<input type="hidden" runat="server" id="hdnDefaultItemIDMCUPackage" value="" />
<input type="hidden" runat="server" id="hdnImagingServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnLaboratoryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnIsUsingRegistrationParamedicID" value="" />
<input type="hidden" runat="server" id="hdnDefaultParamedicID" value="0" />
<input type="hidden" runat="server" id="hdnDefaultParamedicName" value="0" />
<input type="hidden" runat="server" id="hdnVisitID" value="0" />
<input type="hidden" runat="server" id="hdnDefaultRegistrationParamedicID" value="0" />
<input type="hidden" value="" id="hdnListKey" runat="server" />
<input type="hidden" value="" id="hdnListDetailItemID" runat="server" />
<input type="hidden" value="" id="hdnListParamedicID" runat="server" />
<input type="hidden" value="" id="hdnListIsChecked" runat="server" />
<input type="hidden" value="" id="hdnKeyDtSession" runat="server" />
<input type="hidden" value="" id="hdnParamedicIDDtSession" runat="server" />
<input type="hidden" id="hdnParamedicID" runat="server" />

<div>
    <div>
        <table width="100%">
            <col style="width: 45%" />
            <col style="width: 55%" />
            <tr>
                <td>
                    <table width="300px">
                        <tr>
                            <td>
                                <label class="lblLink lblMandatory" id="lblBatchNo">
                                    <%:GetLabel("Batch No")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBatchNo" Style="width: 100%" runat="server" ReadOnly="true" />
                                <input type="hidden" runat="server" id="hdnBatchNo" value="0" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" value="Proses" id="btnProcess" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpCboView" runat="server" Width="100%" ClientInstanceName="cbpCboView"
                        ShowLoadingPanel="false" OnCallback="cbpCboView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpCboViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                         
                                <asp:Panel runat="server" ID="Panel2" CssClass="divcbo">
                              
                                    <table width="500px">
                                        <colgroup>
                                            <col style="width: 45%" />
                                            <col style="width: 55%" />
                                        </colgroup>
                                        <tr>
                                            <%--<td>
                                                <label class="lblMandatory">
                                                    <%:GetLabel("Service Unit")%>
                                                </label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                                    runat="server">
                                                    <ClientSideEvents ValueChanged="function(s){ onCboreset(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>--%>
                                             <td>
                                                <label class="lblLink lblMandatory" id="lblSeriveUnit">
                                                    <%:GetLabel("Service Unit")%>
                                                </label>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="hidden" id="hdnlstHealthcareServiceUnitID" runat="server" value="" />

                                                            <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
                                                            <asp:TextBox ID="txtServiceUnitCode" runat="server" Width="70px" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceUnitName" runat="server" Width="250px" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblLink lblMandatory" id="lblParanedic">
                                                    <%:GetLabel("Dokter Pelaksana")%>
                                                </label>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtParamedicCode" runat="server" Width="70px" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtParamedicName" runat="server" Width="250px" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <input type="button" value="Update Dokter" id="btnUpdateParamedic" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 45%" />
            <col style="width: 55%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpGeneratOrderView" runat="server" Width="100%" ClientInstanceName="cbpGeneratOrderView"
                        ShowLoadingPanel="false" OnCallback="cbpGeneratOrderView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpGeneratOrderViewEndCallback(s); getCbo(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdMCUDT" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="AppointmentRequestID" HeaderStyle-CssClass="keyField"
                                                ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("VisitID") %>" class="VisitID" bindingfield="VisitID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" class="hdnRegistrationParamedicID"
                                                        bindingfield="hdnRegistrationParamedicID" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input type="checkbox" id="chkAll" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelectedID" runat="server" CssClass="chkIsSelectedID" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="Registrasi No" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="cfMedicalNo" HeaderText="Medical / Guest No" HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="cfPatientName" HeaderText="Patient Name" HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="cfRegistrationDate" HeaderText="Registration Date" HeaderStyle-Width="200px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <%-- <div id="paging"></div>--%>
                        </div>
                    </div>
                </div>
            </td>
            <td>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpGeneratOrderDtView" runat="server" Width="100%" ClientInstanceName="cbpGeneratOrderDtView"
                        ShowLoadingPanel="false" OnCallback="cbpGeneratOrderDtView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpGeneratOrderDtViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="DetailItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="DetailItemCode" HeaderText="kode item" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="DetailItemName1" HeaderText="Nama Item" HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="ServiceUnitName" HeaderStyle-Width="200px" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Dokter/Paramedis
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                    <input type="hidden" value="" class="hdnDetailItemID" id="hdnDetailItemID" runat="server" />
                                                    <input type="hidden" value="" class="hdnParamedicID" id="hdnParamedicID" runat="server" />
                                                    <label class="lblParamedicName lblLink" runat="server" id="lblParamedicName">
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="Div1">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <%-- <div id="paging"></div>--%>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
