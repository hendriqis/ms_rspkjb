<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="MedicationOrderProcess.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationOrderProcess" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/Prescription/UDD/InpatientPrescriptionEntryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessMedicationOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Schedule") %></div>
    </li>
    <li id="btnVoidMedicationOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void Item") %></div>
    </li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

        }

        $('#<%=btnProcessMedicationOrder.ClientID %>').click(function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Item First');
                }
                else {
                    onCustomButtonClick('process');
                }
            }
        });

        $('#<%=btnVoidMedicationOrder.ClientID %>').live('click', function (evt) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                showToast('Proses Gagal', 'Paling tidak harus ada 1 Item yang dipilih (check)');
            else
                showDeleteConfirmation(function (data) {
                    var param = data.GCDeleteReason + ';' + data.Reason;
                    $('#<%=hdnVoidReason.ClientID %>').val(param);
                    onCustomButtonClick('void');
                });
        });

        function onAfterCustomClickSuccess(type, retval) {
                var message = ""
                if (type == "process") {
                    message = "<b>Medication Schedule berhasil di-generate</b>";
                }
                else {
                    message = "<b>Pembatalan item berhasil dilakukan</b>";
                }
                showToast('Process Success',message , function () {
                    $('#<%=hdnSelectedMember.ClientID %>').val('');
                    cbpView.PerformCallback('refresh');
                });
        }

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            $chkIsMorning = $(this).closest('tr').find('.chkIsMorning').find('input');
            $chkIsNoon = $(this).closest('tr').find('.chkIsNoon').find('input');
            $chkIsEvening = $(this).closest('tr').find('.chkIsEvening').find('input');
            $chkIsNight = $(this).closest('tr').find('.chkIsNight').find('input');

            if ($(this).is(':checked')) {
                $chkIsMorning.removeAttr("disabled");
                $chkIsNoon.removeAttr("disabled");
                $chkIsEvening.removeAttr("disabled");
                $chkIsNight.removeAttr("disabled");
            }
            else {
                $chkIsMorning.attr("disabled", true);
                $chkIsNoon.attr("disabled", true);
                $chkIsEvening.attr("disabled", true);
                $chkIsNight.attr("disabled", true);
            }
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split('|');
            $('#<%=grdViewDt.ClientID %> .chkIsSelected input').each(function () {
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
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
        });

        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnPrescriptionOrderHdID.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnSelectedMember.ClientID %>').val('');
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                else
                    $('#<%=hdnPrescriptionOrderHdID.ClientID %>').val('');

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });

                cbpViewDt.PerformCallback('refresh');                                                      
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderHdID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderDtID" runat="server" />
    <input type="hidden" value="" id="hdnVoidReason" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfPrescriptionDateText" HeaderText="Date" HeaderStyle-Width="90px"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PrescriptionTime" HeaderText="Time" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center"/>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div style="height:22px"><%=GetLabel("Prescription Order No.")%></div>
                                                    <div style="width:250px;float:left;height:21px"><%=GetLabel("Physician")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("PrescriptionOrderNo")%> </div>
                                                    <div style="width:250px;float:left"><%#: Eval("ParamedicName") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Medication Order To Display")%>
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
                            <div id="paging"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                    OnRowDataBound="grdViewDt_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Generic")%> - <%=GetLabel("Product")%> - <%=GetLabel("Strength")%> - <%=GetLabel("Form")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("InformationLine1")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="StartDateInString" HeaderText="Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" />
                                            <asp:BoundField DataField="StartTime" HeaderText="Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                <HeaderTemplate>
                                                    DOSE
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#: Eval("NumberOfDosage")%> <%#: Eval("DosingUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderText="PRN" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfDoseFrequency" HeaderText="Signa" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <table>
                                                        <tr style="border-bottom:1px">
                                                            <td colspan="4">Time</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:40px">M</td>
                                                            <td style="width:40px">N</td>
                                                            <td style="width:40px">E</td>
                                                            <td style="width:40px">N</td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="margin-top:-3px;">
                                                        <tr>
                                                            <td style="width:40px"><asp:CheckBox ID="chkIsMorning" runat="server" Enabled="false" Checked='<%# Eval("IsMorning")%>' /></td>
                                                            <td style="width:40px"><asp:CheckBox ID="chkIsNoon" runat="server" Enabled="false" Checked='<%# Eval("IsNoon")%>' /></td>
                                                            <td style="width:40px"><asp:CheckBox ID="chkIsEvening" runat="server" Enabled="false" Checked='<%# Eval("IsEvening")%>' /></td>
                                                            <td style="width:40px"><asp:CheckBox ID="chkIsNight" runat="server" Enabled="false" Checked='<%# Eval("IsNight")%>' /></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" HeaderText="PRN" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    Remarks
                                                </HeaderTemplate>
                                                <ItemTemplate>
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
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
