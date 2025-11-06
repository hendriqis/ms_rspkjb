<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="NutritionDistributionNewOrder.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionDistributionNewOrder" %>

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
    <li id="btnChangeStatus" crudmode="R" runat="server" style="display:none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Complete")%></div>
    </li>
    <li id="btnPrintAllLabel" crudmode="R" runat="server" style="display:none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Print All Label")%></div>
    </li>
    <li id="btnPrintLabel" crudmode="R" runat="server" style="display:none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Print Label")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDate.ClientID %>');
            $('#<%=txtDate.ClientID %>').datepicker('option', 'minDate', '-1');

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=txtDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
            //#region Paging
            var pageCount = parseInt('<%=PageCount %>');
            $(function () {

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            });
            $('#<%=btnPrintAllLabel.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
                getCheckedMember();
                var selectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
                if (selectedMember != '') {
                    cbpProcess.PerformCallback('print|all|' + selectedMember);
                }
                else {
                    cbpProcess.PerformCallback('print|all');
                }
            });

            $('#<%=btnPrintLabel.ClientID %>').click(function () {
                var nutritionOrderHdID = $('#<%=hdnNutritionOrderHdID.ClientID %>').val();
                var nutritionOrderDtID = $('#<%=hdnNutritionOrderDtID.ClientID %>').val();
                var date = $('#<%=txtDate.ClientID %>').val();
                var time = $('#<%=txtExpiredDate.ClientID %>').val();
                if (nutritionOrderHdID != "") {
                    var param = nutritionOrderHdID + '|' + nutritionOrderDtID + '|' + date + '|' + time;
                    var url = ResolveUrl("~/Program/NutritionDistribution/PrintDistributionList.ascx");
                    openUserControlPopup(url, param, 'Cetak Etiket Gizi', 800, 600);
                }
                else showToast('Warning', 'Belum ada transaksi menu yang dientry');
            });

            $('#<%=btnChangeStatus.ClientID %>').click(function () {
//                cbpView.PerformCallback('refresh');
                GetChangeStatus();
            });
        });

        $('.btnStatusMeal').die('click');
        $('.btnStatusMeal').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnNutritionOrderDtID.ClientID %>').val(entity.NutritionOrderDtID);
            $('#<%=hdnNutritionOrderHdID.ClientID %>').val(entity.NutritionOrderHdID);
            cbpProcess.PerformCallback('save|single');
        });

        $('.btnProsesOut').die('click');
        $('.btnProsesOut').live('click', function () {
            cbpProcess.PerformCallback('save|all');
        });

        $('.btnPrintSlip').die('click');
        $('.btnPrintSlip').live('click', function () {
            var hdnNutritionOrderHdIDLst = $(this).parent().find('.hdnNutritionOrderHdIDLst').val();
            var hdnNutritionOrderDtIDLst = $(this).parent().find('.hdnNutritionOrderDtIDLst').val()
            var param = 'print|label|' + hdnNutritionOrderHdIDLst + '|' + hdnNutritionOrderDtIDLst;
            cbpProcess.PerformCallback(param);
        });

        $('.btnPrintSatuan').die('click');
        $('.btnPrintSatuan').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            var filterExpression = "NutritionOrderDtID = " + entity.NutritionOrderDtID;
            openReportViewer('NT-00001', filterExpression);
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
            $('#<%=hdnSelectedMember.ClientID %>').val('');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
//             
//            var param = s.cpResult.split('|');
//            if (param[0] == 'refresh') {
//                var pageCount = parseInt(param[1]);
//                setPaging($("#paging"), pageCount, function (page) {
//                    cbpView.PerformCallback('changepage|' + page);
//                });
//            }
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
                    var gcMealDayKey = $(this).closest('tr').find('.hdnGCMealDay').html();
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
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                $('#<%=hdnLstNutritionOrderDtID.ClientID%>').val(selectedMember);
                cbpProcess.PerformCallback('save|all');
            }
            else {
                showToast('WARNING', 'Pilih 1 atau lebih dari 1 dari Order Pasien');
            }
        }

        //#region right panel
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var scheduleDate = $('#<%:txtDate.ClientID %>').val();
            var schDate_day = scheduleDate.substring(0, 2);
            var schDate_month = scheduleDate.substring(3, 5);
            var schDate_year = scheduleDate.substring(6, 10);
            schDate = schDate_year + schDate_month + schDate_day;
            var mealTime = "";
            if (cboMealTime.GetValue() != null && cboMealTime.GetValue() != 'null') {
                mealTime = cboMealTime.GetValue();
            }

            getCheckedMember();

            var selectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
            if (selectedMember != "") {
                if (code == 'NT-00002') {
                    filterExpression.text = schDate + "|" + mealTime + "|" + selectedMember;
                    return true;
                }
            }
            else {
                errMessage.text = "Pilih 1 atau lebih dari 1 dari Order Pasien";
                return false;
            }
        }
        function onCboMealTimeChanged() {
            cbpView.PerformCallback('refresh');
        }
        function oncboServiceUnitChanged() {
            cbpView.PerformCallback('refresh');
        }
    </script>
    <div>
        <div>
            <input type="hidden" id="hdnVisitID" value="" runat="server" />
            <input type="hidden" id="hdnGCMealTime" value="" runat="server" />
            <input type="hidden" id="hdnNutritionOrderHdID" value="" runat="server" />
            <input type="hidden" id="hdnNutritionOrderDtID" value="" runat="server" />
            <input type="hidden" id="hdnLstNutritionOrderDtID" value="" runat="server" />
            <input type="hidden" id="hdnLstNutritionOrderHdID" value="" runat="server" />
            <input type="hidden" id="hdnSelectedMember" value="" runat="server" />
            <input type="hidden" value="" id="hdnPageTitle" runat="server" />
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 100%;">
                            <colgroup>
                                <col style="width: 120px" />
                                <col />
                            </colgroup>
                            <tr style="display:none;">
                                <td class="tdLabel">
                                    <%=GetLabel("Tanggal") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                                <td class="tdLabel">
                                    <%=GetLabel("Jam Expired") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpiredDate" Width="80px" runat="server" style="text-align:center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Ruang Perawatan") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { oncboServiceUnitChanged(); }" /> 
                                    </dxe:ASPxComboBox>
                                </td>
                                <td class="tdLabel">
                                    <%=GetLabel("Jadwal Makan") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMealTime" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){onCboMealTimeChanged()}" /> 
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
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <%--  <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%--<input id="chkSelectAll" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                               <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" style="display:none;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    <asp:BoundField DataField="cfPatientLocation" HeaderText="Kamar/Kls/Bed" HeaderStyle-Width="70px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                   
                                    <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-Width="70px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    
                                    <asp:BoundField DataField="cfDateOfBirthInstring" HeaderText="Tgl.Lahir" HeaderStyle-Width="70px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                     <asp:BoundField DataField="cfActualVisitDateInString" HeaderText="Tgl.Masuk" HeaderStyle-Width="70px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                 Diet
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lbldiet" runat="server" id="lbldiet"></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                 Catatan
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblRemarks" runat="server" id="lblRemarks"></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                 Diagnosa
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                  <%#: Eval("DiagnosisText") %> 
                                            </ItemTemplate>
                                        </asp:TemplateField>                               
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                 Jadwal Makan
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                  <asp:Repeater ID="rptJadwalMakan" runat="server">
                                                   <HeaderTemplate>
                                                    <ol>
                                                    </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <li> <%#: Eval("MealTime") %> </li>
                                                        </ItemTemplate>
                                                      <FooterTemplate>
                                                      </ol>
                                                     </FooterTemplate>
                                                  </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         
                                         <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                 
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                               <div id="btnprint" class="btnprint" runat="server" style="display:none;">
                                                <table>
                                                <tr>
                                                    <td> 
                                                       <input type="button" class="btnPrintSlip" value='<%=GetLabel("LABEL") %>'  />
                                                           <input type="hidden" class="hdnNutritionOrderDtIDLst"  id="hdnNutritionOrderDtIDLst" runat="server" bindingfield="NutritionOrderDtID" />
                                                           <input type="hidden" class="hdnNutritionOrderHdIDLst" runat="server" id="hdnNutritionOrderHdIDlst" bindingfield="NutritionOrderHdID" />
                                                
                                                    </td>
                                                </tr>
                                                </table>
                                               </div>
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
                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>

            <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
        </div>
    </div>
</asp:Content>
