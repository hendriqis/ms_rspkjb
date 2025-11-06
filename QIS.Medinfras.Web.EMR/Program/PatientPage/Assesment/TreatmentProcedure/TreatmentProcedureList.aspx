<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="TreatmentProcedureList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.TreatmentProcedureList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.focus').removeClass('focus');
                $(this).addClass('focus');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtObservationDate.ClientID %>');
            $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onBeforeSaveRecord() {
            return ledProcedure.Validate();
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

        function onBeforeEditRecord(entity, errMessage) {
            if (entity.IsCreatedBySystem == 'True') {
                errMessage.text = 'Record Is Created By System';
                return false;
            }
            return true;
        }

        function onBeforeDeleteRecord(entity, errMessage) {
            if (entity.IsCreatedBySystem == 'True') {
                errMessage.text = 'Record Is Created By System';
                return false;
            }
            return true;
        }

        //#region Entity To Control
        var paramedicID = '<%=ParamedicID %>';
        function entityToControl(entity) {
            $('#<%=txtObservationDate.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');

                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtObservationDate.ClientID %>').val(entity.ProcedureDate);
                $('#<%=txtObservationTime.ClientID %>').val(entity.ProcedureTime);
                cboParamedicID.SetValue(entity.ParamedicID);
                $('#<%=hdnProcedureID.ClientID %>').val(entity.ProcedureID);
                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);

                ledProcedure.SetValue(entity.ProcedureID);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                cboParamedicID.SetValue(paramedicID);
                $('#<%=hdnProcedureID.ClientID %>').val('');
                $('#<%=txtRemarks.ClientID %>').val('');

                ledProcedure.SetValue('');
            }
        }
        //#endregion

        function onLedProcedureLostFocus(value) {
            $('#<%=hdnProcedureID.ClientID %>').val(value);
        }
    </script>

    <style type="text/css">
    .createSystem-True              { display:block; }
    .createSystem-False             { display:none; }
    </style>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
            <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Treatment Procedure")%></label></td>
            <td colspan="2">
                <input type="hidden" value="" id="hdnProcedureID" runat="server" />
                <qis:QISSearchTextBox ID="ledProcedure" ClientInstanceName="ledProcedure" runat="server" Width="400px"
                    ValueText="ProcedureID" FilterExpression="1 = 0" DisplayText="ProcedureName" MethodName="GetvSpecialtyProceduresList" >
                    <ClientSideEvents ValueChanged="function(s){ onLedProcedureLostFocus(s.GetValueText()); }" />
                    <Columns>
                        <qis:QISSearchTextBoxColumn Caption="Procedure Code" FieldName="ProcedureID" Description="i.e. 1-16" Width="100px" />
                        <qis:QISSearchTextBoxColumn Caption="Procedure Name" FieldName="ProcedureName" Description="i.e. Consultation" Width="300px" />
                    </Columns>
                </qis:QISSearchTextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Remarks")%></label></td>
            <td colspan="2"><asp:TextBox ID="txtRemarks" Width="300px" runat="server" TextMode="MultiLine" /></td>
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
                                        <input type="hidden" value="<%#:Eval("ProcedureDateInDatePickerFormat") %>" bindingfield="ProcedureDate" />
                                        <input type="hidden" value="<%#:Eval("ProcedureTime") %>" bindingfield="ProcedureTime" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                        <input type="hidden" value="<%#:Eval("IsCreatedBySystem") %>" bindingfield="IsCreatedBySystem" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <%=GetLabel("Treatment Procedure") %>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="float:left;width:120px;"><%#: Eval("ProcedureDateInString")%>, <%#: Eval("ProcedureTime")%></div>
                                        <div style="float:left;width:300px;"><%#: Eval("ParamedicName")%></div>
                                        <div style="margin-left:300px;margin-top:-3px;" class="createSystem-<%#: DataBinder.Eval(Container.DataItem, "IsCreatedBySystem")%>"><input type="checkbox" disabled="disabled" checked="checked" /><div style="margin-top:-16px;margin-left:140px;"><%=GetLabel("Created By System ")%> </div></div>
                                        <div style="margin-left:120px; clear:both;"><%#: Eval("ProcedureName")%> (<%#: Eval("ProcedureID")%>)</div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display") %>
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
