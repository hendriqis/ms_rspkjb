<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="NutritionWorkList.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionWorkList" %>

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
    <li id="btnCancelNextStatus" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Cancel")%></div>
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

            $('#<%=btnCancelNextStatus.ClientID %>').click(function () {
                showLoadingPanel();
                GetPreviousOrderStatus();
                hideLoadingPanel();
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
                cbpViewMenu.PerformCallback('refresh');
            });

            $('#divShow2').hide();
            $('#ulTabWorkList li').click(function () {
                $('#ulTabWorkList li.selected').removeAttr('class');
                $('.containerOrder').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('#<%=rblViewType.ClientID %>').change(function () {
                var t = $('#<%=rblViewType.ClientID %> input[type=radio]:checked').val();
                $('.divShow:visible').hide();
                $('#divShow' + t).show();
            });

            setDatePicker('<%=txtDate.ClientID %>');
        });

        $('.lblMealPlanName.lblLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var mealPlanID = entity.MealPlanID;
            //            var isHasRemarks = entity.IsHasRemarks;
            var isHasRemarks = 'False';

            var param = mealPlanID + '|' + cboMealTime.GetValue() + '|' + entity.GCMealDay + '|' + $('#<%=txtDate.ClientID %>').val() + '|' + entity.GCMealStatus + '|' + isHasRemarks;
            var url = ResolveUrl('~/Program/Worklist/NutritionWorkListCtl.ascx');
            openUserControlPopup(url, param, 'Detail Order List', 1400, 600);
        });

        $('.lblMealItemList.lblLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var scheduleDate = entity.cfScheduleDate1;
            var mealPlanID = entity.MealPlanID;
            var mealPlanCode = entity.MealPlanCode;
            var mealPlanName = entity.MealPlanName;
            var mealTime = entity.MealTime;


            var param = scheduleDate + '|' + mealPlanID + '|' + mealPlanCode + '|' + mealPlanName + '|' + mealTime;
            var url = ResolveUrl('~/Program/Worklist/NutritionWorkListMenuCtl.ascx');
            openUserControlPopup(url, param, 'Meal Plan - Menu Summary', 800, 600);
        });
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('.btnStatusMeal').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnGCMealDay.ClientID%>').val(entity.GCMealDay);
            $('#<%=hdnMealPlanID.ClientID%>').val(entity.MealPlanID);
            $('#<%=hdnGCItemDetailStatus.ClientID%>').val(entity.GCItemDetailStatus);
            cbpProcess.PerformCallback('save|mealplan');
        });

        $('.btnStatusMenu').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnGCMealDay.ClientID%>').val(entity.GCMealDay);
            $('#<%=hdnMealID.ClientID%>').val(entity.MealID);
            $('#<%=hdnGCItemDetailStatus.ClientID%>').val(entity.GCItemDetailStatus);
            cbpProcess.PerformCallback('save|menu');
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
            var lstMealDay = $('#<%=hdnSelectedMealDay.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var gcMealDay = "'" + $(this).closest('tr').find('.hdnGCMealDay').val() + "'"; 
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstMealDay.push(gcMealDay);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var gcMealDayKey = $(this).closest('tr').find('.hdnGCMealDay').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstMealDay.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedMealDay.ClientID %>').val(lstMealDay.join(','));
        }

        function GetChangeStatus() {
            getCheckedMember();
            var selectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
            var selectedMemberMealDay = $('#<%=hdnSelectedMealDay.ClientID %>').val().substring(1);
            if (selectedMember != "") {
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                $('#<%=hdnGCMealDay.ClientID%>').val(entity.GCMealDay);
                $('#<%=hdnMealPlanID.ClientID%>').val(selectedMember);
                $('#<%=hdnGCItemDetailStatus.ClientID%>').val(entity.GCItemDetailStatus);
                cbpProcess.PerformCallback('save|mealplan');
            }
            else {
                showToast('WARNING', 'Pilih 1 atau lebih dari 1 dari Order Pasien');
            }
        }

        function GetPreviousOrderStatus() {
            getCheckedMember();
            var selectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
            var selectedMemberMealDay = $('#<%=hdnSelectedMealDay.ClientID %>').val().substring(1);
            if (selectedMember != "") {
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                $('#<%=hdnGCMealDay.ClientID%>').val(entity.GCMealDay);
                $('#<%=hdnMealPlanID.ClientID%>').val(selectedMember);
                $('#<%=hdnGCItemDetailStatus.ClientID%>').val(entity.GCItemDetailStatus);
                cbpProcess.PerformCallback('cancel|mealplan');
            }
            else {
                showToast('WARNING', 'Pilih 1 atau lebih dari 1 dari Order Pasien');
            }
        }
        //#endregion

        //#region right panel
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var scheduleDate = $('#<%:txtDate.ClientID %>').val();
            var mealTime = cboMealTime.GetValue();

            if (code == 'NT090104') {
                filterExpression.text = scheduleDate + '|' + mealTime;
                return true;
            }
        }
        //#endregion
    </script>
    <div>
        <div style="padding:2px;" id="containerOrder" class="containerOrder">
            <input type="hidden" value="" id="hdnGCMealDay" runat="server"/>
            <input type="hidden" value="" id="hdnMealPlanID" runat="server"/>
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
                                <td class="tdLabel"><%=GetLabel("Tanggal Makan") %></td>
                                <td><asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                            </tr>  
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Jadwal Makan") %></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMealTime" Width="200px" runat="server" ClientInstanceName="cboMealTime" >
                                        <%--<ClientSideEvents ValueChanged="function(s,e){onCboMealTimeChanged()}" />--%>
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
                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="MealPlanID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAll" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" class="hdnGCMealDay" value='<%#: Eval("GCMealDay")%>' />
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderStyle-HorizontalAlign = "Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Panel Menu Makan")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblLink lblMealPlanName" ><%#:Eval("MealPlanCode") %>-<%#:Eval("MealPlanName") %></label>
<%--                                               <asp:Label ID="lblRemarks" runat="server" style="font:bold; color:red" Visible='<%# Eval("Remarks").ToString() != "" ? true : false %>'> <%=GetLabel("**")%></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
<%--                                        <asp:BoundField DataField="MealStatus" HeaderText="Status Makan" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="120px" />--%>
                                        <asp:BoundField DataField="MealDay" HeaderStyle-CssClass="hdnMealDay" ItemStyle-CssClass="hdnMealDay" HeaderText="Menu Hari ke - " HeaderStyle-HorizontalAlign = "Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="JumlahPorsi" HeaderText="Jumlah Porsi" HeaderStyle-HorizontalAlign = "Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                        <asp:TemplateField  HeaderStyle-HorizontalAlign = "Center" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%=GetLabel("Jumlah Porsi Per Menu")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblLink lblMealItemList" >Daftar Menu</label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width:80px"><%#: Eval("Status")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td style="padding-left: 10px">
                                                            <div <%#: Eval("GCItemDetailStatus").ToString() == GetItemDetailStatus() ? "style='display:none'" : "" %> >
                                                                <input type="button" class="btnStatusMeal" value='<%#: Eval("NextStatus")%>' style="width: 80px; height: 25px" />    
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--<input type="hidden" value="<%#:Eval("NutritionOrderDate") %>" bindingfield="NutritionOrderDate" />--%>
                                                <input type="hidden" value="<%#:Eval("cfScheduleDate1") %>" bindingfield="cfScheduleDate1" />
                                                <input type="hidden" value="<%#:Eval("MealPlanID") %>" bindingfield="MealPlanID" />
                                                <input type="hidden" value="<%#:Eval("MealPlanCode") %>" bindingfield="MealPlanCode" />
                                                <input type="hidden" value="<%#:Eval("MealPlanName") %>" bindingfield="MealPlanName" />
                                                <input type="hidden" value="<%#:Eval("GCMealTime") %>" bindingfield="GCMealTime" />
                                                <input type="hidden" value="<%#:Eval("MealTime") %>" bindingfield="MealTime" />
                                                <input type="hidden" value="<%#:Eval("GCMealStatus") %>" bindingfield="GCMealStatus" />
                                                <input type="hidden" value="<%#:Eval("MealStatus") %>" bindingfield="MealStatus" />
                                                <input type="hidden" value="<%#:Eval("GCMealDay") %>" bindingfield="GCMealDay" />
                                                <input type="hidden" value="<%#:Eval("MealDay") %>" bindingfield="MealDay" />
                                                <input type="hidden" value="<%#:Eval("JumlahPorsi") %>" bindingfield="JumlahPorsi" />
<%--                                                <input type="hidden" value="<%#:Eval("IsHasRemarks") %>" bindingfield="IsHasRemarks" />--%>
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
            <div id="divShow2" class="divShow">
                <dxcp:ASPxCallbackPanel ID="cbpViewMenu" runat="server" Width="100%" ClientInstanceName="cbpViewMenu"
                    ShowLoadingPanel="false" OnCallback="cbpViewMenu_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px; overflow-y:scroll;">
                                <asp:GridView ID="grdViewMenu" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="MealName" HeaderText="Name" />
                                        <asp:BoundField DataField="JumlahPorsi" HeaderText="Porsi" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="120px" />
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
