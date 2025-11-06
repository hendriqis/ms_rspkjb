<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateStatusOrderGroupCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GenerateStatusOrderGroupCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
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
                cbpGenerateOrderStatusView.PerformCallback('GenerateUpdateOrder');
            }
        });

    });
    $('#chkAll').live('click', function () {
        $('input:checkbox').prop('checked', this.checked);
    });
    $('#lblBatchNo').live('click', function () {
        var filterexpresion = "RequestBatchNo IS NOT NULL AND GCVisitStatus IN ('X020^002','X020^003') AND GCItemDetailStatus = 'X121^004' AND  DepartmentID = 'MCU'";
        openSearchDialog('appointmentRequestBatchNo', filterexpresion, function (value) {
            onRequestBatchNo(value);
        });
    });

    function onRequestBatchNo(value) {
        if (value != "") {
            $('#<%:hdnBatchNo.ClientID %>').val(value);
            $('#<%:txtBatchNo.ClientID %>').val(value);

            cbpGenerateOrderStatusView.PerformCallback('refresh');
        } else {
            $('#<%:hdnBatchNo.ClientID %>').val('');
            $('#<%:txtBatchNo.ClientID %>').val('');
        }
    }

    function onCbpGenerateOrderStatusViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'generate') {
            if (param[1] == 'fail') {
                showToast("Error", param[2]);
            } else {
                displayMessageBox('SUCCESS', 'Status Order Berhasil Diupdate');
                pcRightPanelContent.Hide()
//                showToast("Process success");
//                cbpGenerateOrderStatusView.PerformCallback('refresh');
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

    function onAfterPopupControlClosing() {
        refreshGrdRegisteredPatient();
    }
    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }

    
</script>
<input type="hidden" runat="server" id="hdnSelectedVisitID" value="" />
<input type="hidden" runat="server" id="hdnSelectedAppointmentRequestID" value="" />

<input type="hidden" runat="server" id="hdnDefaultItemIDMCUPackage" value="" />
<input type="hidden" runat="server" id="hdnImagingServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnLaboratoryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnIsUsingRegistrationParamedicID" value="" />

<div>
    <div>
        <table width="350px">
            <tr>
                <td><label class="lblLink lblMandatory" id="lblBatchNo" > <%:GetLabel("Batch No")%></label></td>
                <td> <asp:TextBox ID="txtBatchNo" style="width:100%" runat="server" ReadOnly="true" />  
                    <input type="hidden" runat="server" id="hdnBatchNo" value="0" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td><input type="button" value="Proses" ID="btnProcess" runat="server" /></td>
            </tr>
        </table>
     </div>
   <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpGenerateOrderStatusView" runat="server" Width="100%" ClientInstanceName="cbpGenerateOrderStatusView"
            ShowLoadingPanel="false" OnCallback="cbpGenerateOrderStatusView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpGenerateOrderStatusViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="AppointmentRequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                   <HeaderTemplate>
                                        <input type="checkbox" id="chkAll" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSelectedID" runat="server" CssClass="chkIsSelectedID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RegistrationNo" HeaderText="Registrasi No" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="cfMedicalNo" HeaderText="Medical / Guest No" HeaderStyle-Width="200px"/>
                                <asp:BoundField DataField="cfPatientName" HeaderText="Patient Name" HeaderStyle-Width="200px"/>
                                <asp:BoundField DataField="cfRegistrationDate" HeaderText="Registration Date" HeaderStyle-Width="200px"/>
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
               <%-- <div id="paging"></div>--%>
            </div>
        </div> 
    </div>
</div>
