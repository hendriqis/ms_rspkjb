<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="VaccinationShotHistoryEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VaccinationShotHistoryEntry" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackVaccinationShotList" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.focus').removeClass('focus');
                $(this).addClass('focus');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

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

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.focus'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.focus');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        var editGCDoseUnit = '';
        //#region Entity To Control
        function entityToControl(entity) {
            cboVaccinationType.SetFocus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cboVaccinationType.SetValue(entity.VaccinationTypeID);
                cboVaccinationRoute.SetValue(entity.GCVaccinationRoute);
                $('#<%=chkIsBooster.ClientID %>').prop('checked', (entity.IsBooster == 'True'));
                $('#<%=txtDosingDose.ClientID %>').val(entity.Dose);
                cboVaccinationRoute.SetValue(entity.GCDoseUnit);
                $('#<%=txtVaccinationNo.ClientID %>').val(entity.VaccinationNo);
                $('#<%=txtVaccineName.ClientID %>').val(entity.VaccineName);          
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                cboVaccinationType.SetValue('');
                cboVaccinationRoute.SetValue('');
                $('#<%=chkIsBooster.ClientID %>').prop('checked', false);
                $('#<%=txtDosingDose.ClientID %>').val('');
                cboDosingUnit.SetValue('');
                $('#<%=txtVaccinationNo.ClientID %>').val('1');
                $('#<%=txtVaccineName.ClientID %>').val('');    
            }
        }
        //#endregion

        $(function () {
            setDatePicker('<%=txtVaccinationDate.ClientID %>');

            $('#<%=btnBackVaccinationShotList.ClientID %>').click(function () {
                document.location = document.referrer;
            });

            var patientDOB = Methods.stringToDate('<%=PatientDOB%>');
            $('#<%=txtVaccinationDate.ClientID %>').change(function () {
                var vaccinationDate = Methods.getDatePickerDate($(this).val());
                var age = Methods.calculateDateDifference(patientDOB, vaccinationDate);
                $('#<%=txtAgeInYear.ClientID %>').val(age.years);
                $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
                $('#<%=txtAgeInDay.ClientID %>').val(age.days);
            });
            $('#<%=txtVaccinationDate.ClientID %>').change();
        });

        function onAfterSaveRecord(param) {
            if ($('#<%=hdnVaccinationShotID.ClientID %>').val() == '') {
                $('#<%=hdnVaccinationShotID.ClientID %>').val(param);
                $('#<%=txtPhysicianName.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVaccinationDate.ClientID %>').attr('readonly', 'readonly');
            }
        }
    </script>
    <input type="hidden" id="hdnVaccinationShotID" runat="server" value="" />
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table id="Table2" runat="server" cellpadding="0">
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Date") %></td>
                        <td style="width: 140px;"><asp:TextBox ID="txtVaccinationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        <td style="width: 100px;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Age")%> (yyyy-MM-dd)</label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:32%"/>
                                    <col style="width:3px"/>
                                    <col style="width:32%"/>
                                    <col style="width:3px"/>
                                    <col style="width:32%"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtAgeInYear" ReadOnly="true" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtAgeInMonth" ReadOnly="true" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtAgeInDay" ReadOnly="true" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:300px;text-align:right" valign="top">
                <table id="Table1" runat="server" cellpadding="0">
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Physician") %></td>
                        <td><asp:TextBox ID="txtPhysicianName" runat="server" Width="200px" /> </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>    
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col style="width:170px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Vaccination Type")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboVaccinationType" ClientInstanceName="cboVaccinationType" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Vaccination Route")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboVaccinationRoute" ClientInstanceName="cboVaccinationRoute" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Vaccination No")%></label></td>
                        <td><asp:TextBox ID="txtVaccinationNo" Width="80px" CssClass="number" runat="server"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Is Booster")%></label></td>
                        <td><asp:CheckBox ID="chkIsBooster" runat="server"/></td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col style="width:160px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                        <td><asp:TextBox ID="txtVaccineName" Width="500px" runat="server"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Dosing")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col style="width:100px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" /></td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("VaccinationTypeID") %>" bindingfield="VaccinationTypeID" />
                                        <input type="hidden" value="<%#:Eval("GCVaccinationRoute") %>" bindingfield="GCVaccinationRoute" />
                                        <input type="hidden" value="<%#:Eval("VaccineName") %>" bindingfield="VaccineName" />
                                        <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                        <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                        <input type="hidden" value="<%#:Eval("VaccinationNo") %>" bindingfield="VaccinationNo" />
                                        <input type="hidden" value="<%#:Eval("IsBooster") %>" bindingfield="IsBooster" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="VaccinationTypeName" HeaderText="Vaccination Type" HeaderStyle-Width="250px" />
                                <asp:BoundField DataField="VaccinationRoute" HeaderText="Vaccination Route" HeaderStyle-Width="300px" />
                                <asp:BoundField DataField="VaccineName" HeaderText="Vaccine Name" />
                                <asp:BoundField DataField="cfDisplayDose" HeaderText="Dose" HeaderStyle-Width="200px" />                                
                            </Columns>
                            <EmptyDataTemplate>
                                No Data To Display
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
</asp:Content>
