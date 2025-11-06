<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="NutritionDistributionLink.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionDistributionLink" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=txtDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
        });

        $('.btnStatusMeal').die('click');
        $('.btnStatusMeal').live('click',function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnNutritionOrderDtID.ClientID %>').val(entity.NutritionOrderDtID);
            $('#<%=hdnNutritionOrderHdID.ClientID %>').val(entity.NutritionOrderHdID);
            cbpProcess.PerformCallback('save|single');
        });

        $('.btnProsesOut').die('click');
        $('.btnProsesOut').live('click',function () {
            cbpProcess.PerformCallback('save|all');
        });
        $('.btnPrintSlip').die('click');
        $('.btnPrintSlip').live('click', function () {
            cbpProcess.PerformCallback('print|all');
        });
        $('.btnPrintSatuan').die('click');
        $('.btnPrintSatuan').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnNutritionOrderDtID.ClientID %>').val(entity.NutritionOrderDtID);
            $('#<%=hdnNutritionOrderHdID.ClientID %>').val(entity.NutritionOrderHdID);
            cbpProcess.PerformCallback('print|single');
        });
        function onCboMealTimeChanged() {
            cbpView.PerformCallback('refresh');
        }

        function onCboClassChanged() {
            cbpView.PerformCallback('refresh');
        }

        function oncboServiceUnitChanged() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpProcessEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }
        
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

    </script>
    <div>
        <div>
            <input type="hidden" id="hdnGCMealTime" value="" runat="server" />
            <input type="hidden" id="hdnNutritionOrderHdID" value="" runat="server" />
            <input type="hidden" id="hdnNutritionOrderDtID" value="" runat="server" />
            <input type="hidden" id="hdnLstNutritionOrderDtID" value="" runat="server" />
            <input type="hidden" id="hdnLstNutritionOrderHdID" value="" runat="server" />
            <table style="width:100%">
                <colgroup>
                    <col style="width:50%" />
                </colgroup>
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <table class="tblEntryContent" style="width:100%;">
                            <colgroup>
                                <col style="width:100px"/>
                                <col style="width:350px"/>
                                <col style="width:100px"/>
                                <col style="width:300px"/>
                                <col style="width:100px"/>
                                <col style="width:300px"/>
                                <col/>
                            </colgroup>   
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Tanggal") %></td>
                                <td><asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" ReadOnly="true" runat="server" /></td>
                            </tr>  
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Ruang") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" width="350px" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { oncboServiceUnitChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td class="tdLabel"><%=GetLabel("Jadwal Makan") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMealTime" runat="server" >
                                        <ClientSideEvents ValueChanged="function(s,e){onCboMealTimeChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td class="tdLabel"><%=GetLabel("Kelas") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboClass" runat="server" >
                                        <ClientSideEvents ValueChanged="function(s,e){onCboClassChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td><input type="button" class="btnProsesOut" value='<%=GetLabel("Proses Ready") %>' /></td>
                                <td><input type="button" class="btnPrintSlip" value='<%=GetLabel("Print Slip") %>' /></td>
                            </tr>
                        </table>     
                    </td>
                </tr>
            </table>
            <div>
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No. Reg" HeaderStyle-Width="90px"  ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" HeaderStyle-Width="70px"  ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" />
                                        <asp:BoundField DataField="ClassName" HeaderText="Kelas" HeaderStyle-Width="30px" />
                                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Ruang" HeaderStyle-Width="50px" />
                                        <asp:BoundField DataField="BedCode" HeaderText="No. TT" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="MealPlanName" HeaderText="Menu Makan" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="MealTime" HeaderText="Jadwal Makan" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="140px"  />
                                        <asp:BoundField DataField="Diagnose" HeaderText="Diagnosa" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-Width="30px" />
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width:40px"><%#: Eval("Status")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div <%#: Eval("GCItemDetailStatus").ToString() != GetItemDetailStatus() ? "style='display:none'" : "" %> >
                                                                <input type="button" class="btnStatusMeal" value='<%#: Eval("NextStatus")%>' />    
                                                            </div>
                                                            <div <%#: Eval("GCItemDetailStatus").ToString() == GetItemDetailStatus() ? "style='display:none'" : "" %> >
                                                                <input type="button" class="btnPrintSatuan" value='<%=GetLabel("Print") %>'  />    
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("NutritionOrderHdID") %>" bindingfield="NutritionOrderHdID" />
                                                <input type="hidden" value="<%#:Eval("NutritionOrderDtID") %>" bindingfield="NutritionOrderDtID" />
                                                <input type="hidden" value="<%#:Eval("GCMealTime") %>" bindingfield="GCMealTime" />
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
            </div>
            <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
                ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
                    EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
