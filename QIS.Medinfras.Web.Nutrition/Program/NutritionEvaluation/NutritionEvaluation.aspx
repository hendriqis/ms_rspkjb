<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="NutritionEvaluation.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionEvaluation" %>

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

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            setDatePicker('<%=txtDate.ClientID %>');

//            $('#<%=txtDate.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });
        });

//        function onCboMealTimeChanged() {
//            cbpView.PerformCallback('refresh');
//        }

//        function oncboServiceUnitChanged() {
//            cbpView.PerformCallback('refresh');
//        }

        $('.btnsave').die('click');
        $('.btnsave').live('click', function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            $('#<%=hdnNutritionOrderDtID.ClientID %>').val(key);
            var idx = $tr.find('.hdnItemIndex').val();
            cboHA = eval('cboHA' + idx);
            cboLH = eval('cboLH' + idx);
            cboLN = eval('cboLN' + idx);
            cboSY = eval('cboSY' + idx);
            cboBH = eval('cboBH' + idx);
            cboFluid = eval('cboFluid' + idx);
            cboSnack = eval('cboSnack' + idx);
            cboDessert = eval('cboDessert' + idx);

            var HA = '';
            var LH = '';
            var LN = '';
            var SY = '';
            var BH = '';
            var Fluid = '';
            var Snack = '';
            var Dessert = '';

            if (cboHA.GetValue() != null)
                HA = cboHA.GetValue();
            if (cboLH.GetValue() != null)
                LH = cboLH.GetValue();
            if (cboLN.GetValue() != null)
                LN = cboLN.GetValue();
            if (cboSY.GetValue() != null)
                SY = cboSY.GetValue();
            if (cboBH.GetValue() != null)
                BH = cboBH.GetValue();
            if (cboFluid.GetValue() != null)
                Fluid = cboFluid.GetValue();
            if (cboSnack.GetValue() != null)
                Snack = cboSnack.GetValue();
            if (cboDessert.GetValue() != null)
                Dessert = cboDessert.GetValue();

            var param = 'save|' + HA + '|' + LH + '|' + LN + '|' + SY + '|' + BH + '|' + Fluid + '|' + Snack + '|' + Dessert;
            $btnSave = $(this);
            cbpProcess.PerformCallback(param);
        });


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
            <input type="hidden" id="hdncboHA" value="" runat="server" />
            <input type="hidden" id="hdncboLH" value="" runat="server" />
            <input type="hidden" id="hdncboLN" value="" runat="server" />
            <input type="hidden" id="hdncboSY" value="" runat="server" />
            <input type="hidden" id="hdncboBH" value="" runat="server" />
            <input type="hidden" id="hdncboFluid" value="" runat="server" />
            <input type="hidden" value="" id="hdnPageTitle" runat="server" />
            <table style="width:100%">
                <colgroup>
                    <col style="width:50%" />
                </colgroup>
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <table class="tblEntryContent" style="width:100%;">
                            <colgroup>
                                <col style="width:150px"/>
                                <col/>
                            </colgroup>   
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Tanggal Makan") %></td>
                                <td><asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                            </tr>  
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Ruang Perawatan") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server">
                                        <%--<ClientSideEvents ValueChanged="function(s,e) { oncboServiceUnitChanged(); }" />--%>
                                    </dxe:ASPxComboBox>
                                </td>
                                <td class="tdLabel"><%=GetLabel("Jadwal Makan") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMealTime" runat="server" >
                                        <%--<ClientSideEvents ValueChanged="function(s,e){onCboMealTimeChanged()}" />--%>
                                    </dxe:ASPxComboBox>
                                </td>
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
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="BedCode" HeaderText="No. TT" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NutritionOrderDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" HeaderStyle-Width="80px"  ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Ruang" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"  />
                                        <asp:BoundField DataField="ClassName" HeaderText="Kelas" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"  />
                                        <asp:BoundField DataField="MealPlanName" HeaderText="Menu Makan" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MealTime" HeaderText="Jadwal Makan" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText = "HA" HeaderStyle-Width = "40px">
                                            <ItemTemplate>
                                                <input type="hidden" class="hdnItemIndex" value='<%#: Container.DataItemIndex %>' />
                                                <dxe:ASPxComboBox ID="cboHA" ClientInstanceName = "cboHA" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "LN" HeaderStyle-Width = "50px">
                                            <ItemTemplate>
                                                <dxe:ASPxComboBox ID="cboLN" ClientInstanceName = "cboLH" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "LH" HeaderStyle-Width = "50px">
                                            <ItemTemplate>
                                                <dxe:ASPxComboBox ID="cboLH" ClientInstanceName = "cboLN" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "SY" HeaderStyle-Width = "50px">
                                            <ItemTemplate>
                                                <dxe:ASPxComboBox ID="cboSY" ClientInstanceName = "cboSY" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "BH" HeaderStyle-Width = "50px">
                                            <ItemTemplate>
                                                <dxe:ASPxComboBox ID="cboBH" ClientInstanceName = "cboBH" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "Cairan" HeaderStyle-Width = "50px">
                                            <ItemTemplate>
                                                <dxe:ASPxComboBox ID="cboFluid" ClientInstanceName = "cboFluid" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "SN" HeaderStyle-Width = "50px">
                                            <ItemTemplate>
                                                <dxe:ASPxComboBox ID="cboSnack" ClientInstanceName = "cboSnack" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "DS" HeaderStyle-Width = "50px">
                                            <ItemTemplate>
                                                <dxe:ASPxComboBox ID="cboDessert" ClientInstanceName = "cboDessert" runat="server" Width="50px">
                                                </dxe:ASPxComboBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <input type="button" class="btnsave" value='<%=GetLabel("Save") %>'  /> 
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
