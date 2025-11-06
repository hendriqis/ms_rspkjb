<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="ReorderMealPlanListX.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ReorderMealPlanListX" %>

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
    <li id="btnProcessNextStatus" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnProcessNextStatus.ClientID %>').click(function () {
                showLoadingPanel();
                GetChangeStatus();
                hideLoadingPanel();
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
                cbpViewMenu.PerformCallback('refresh');
            });

            $('#<%=rblViewType.ClientID %>').change(function () {
                var t = $('#<%=rblViewType.ClientID %> input[type=radio]:checked').val();
                $('.divShow:visible').hide();
                $('#divShow' + t).show();
            });

            $('#<%=txtScheduleDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
        });


        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onCbpProcessEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else
                    showToast('Save Success!', 'Re-order berhasil');
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'cancel') {
                if (param[1] == 'fail')
                    showToast('Cancel Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        //#region Check Box
        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        function GetChangeStatus() {
            getCheckedMember();
            var selectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
            if (selectedMember != "") {
                $('#<%=hdnNutritionOrderHdID.ClientID%>').val(selectedMember);
                cbpProcess.PerformCallback('save|mealplan');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
            }
            else {
                showToast('WARNING', 'Pilih 1 atau lebih dari 1 dari Order Pasien');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
            }
        }
        //#endregion
    </script>
    <div>
        <div style="padding:2px;" id="containerOrder" class="containerOrder">
            <input type="hidden" value="" id="hdnGCMealDay" runat="server"/>
            <input type="hidden" value="" id="hdnMealDay" runat="server"/>
            <input type="hidden" value="" id="hdnNutritionOrderHdID" runat="server"/>
            <input type="hidden" value="" id="hdnGCItemDetailStatus" runat="server"/>
            <input type="hidden" value="" id="hdnMealID" runat="server"/>
            <input type="hidden" value="" id="hdnPageTitle" runat="server" />
            <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
            <input type="hidden" value="" id="hdnSelectedMealDay" runat="server" />
            <table style="width:100%">
                <colgroup>
                    <col style="width:50%" />
                </colgroup>
                <tr>
                    <td style="padding:5px;vertical-align:top">
                        <table class="tblEntryContent" style="width:100%;">
                            <colgroup>
                                <col style="width:100px"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Unit Pelayanan") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" Width="200px" runat="server" ClientInstanceName="cboServiceUnit" >
                                    </dxe:ASPxComboBox>
                                </td>
                                <td class="tdLabel"><%=GetLabel("Tanggal Makan") %></td>
                                <td><asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                            </tr>  
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Jadwal Makan") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMealTime" Width="200px" runat="server" ClientInstanceName="cboMealTime" >
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>     
                    </td>
                    <td style="padding:5px;vertical-align:top; display:none">
                        <div><%=GetLabel("View Type") %></div>
                        <div><asp:RadioButtonList ID="rblViewType" runat="server" RepeatDirection="Vertical" /></div>
                    </td>
                </tr>
            </table>
            <div id="divShow1" class="divShow">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:450px;overflow-y:scroll;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="NutritionOrderHdID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAll" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" class="hdnGCMealDay" value='<%#: Eval("GCMealDay")%>' />
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ServiceUnitName" HeaderStyle-CssClass="hdnServiceUnitName" ItemStyle-CssClass="hdnServiceUnitName" HeaderText="Unit" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                        <asp:BoundField DataField="BedCode" HeaderStyle-CssClass="hdnBedCode" ItemStyle-CssClass="hdnBedCode" HeaderText="No. TT" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                                        <asp:BoundField DataField="RegistrationNo" HeaderStyle-CssClass="hdnRegistrationNo" ItemStyle-CssClass="hdnRegistrationNo" HeaderText="No. Registrasi" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                        <asp:TemplateField  HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Nama Pasien")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblPatientName" >(<%#:Eval("MedicalNo") %>) <%#:Eval("PatientName") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderStyle-HorizontalAlign = "Left" HeaderStyle-Width="200px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Panel Menu Makan")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblLastOrder" ><%#:Eval("LastOrder") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MealDay" HeaderStyle-CssClass="hdnMealDay" ItemStyle-CssClass="hdnMealDay" HeaderText="Menu Hari ke - " HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                        <asp:BoundField DataField="MealTime" HeaderStyle-CssClass="hdnMealTime" ItemStyle-CssClass="hdnMealTime" HeaderText="Jadwal Makan" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                        <asp:TemplateField  HeaderStyle-HorizontalAlign = "Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%=GetLabel("Diet Pasien")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblMealItemList" ><%#:Eval("DietType") %></label>
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
</asp:Content>
