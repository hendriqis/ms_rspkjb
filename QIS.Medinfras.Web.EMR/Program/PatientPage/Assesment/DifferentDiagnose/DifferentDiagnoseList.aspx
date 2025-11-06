<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="DifferentDiagnoseList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.DifferentDiagnoseList" %>

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
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtDifferentialDate.ClientID %>');
            $('#<%=txtDifferentialDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
             
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
            $('#<%=hdnIsMainDiagnosisExists.ClientID %>').val('<%=GetIsMainDiagnosisExist() %>');
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
            var isMainDiagnosisExists = s.cpRetval;

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

            $('#<%=hdnIsMainDiagnosisExists.ClientID %>').val(isMainDiagnosisExists);
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.selected'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onBeforeSaveRecord() {
            //return ledDiagnose.Validate();
            return true;
        }

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.selected').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtDifferentialDate.ClientID %>').val(entity.DifferentialDate);
                $('#<%=txtDifferentialTime.ClientID %>').val(entity.DifferentialTime);
                cboDiagnoseType.SetValue(entity.GCDiagnoseType);
                cboPhysician.SetValue(entity.ParamedicID);
                cboStatus.SetValue(entity.GCDifferentialStatus);
                ledDiagnose.SetValue(entity.DiagnoseID);
                ledMorphology.SetValue(entity.MorphologyID);
                $('#<%=hdnDiagnoseID.ClientID %>').val(entity.DiagnoseID);
                $('#<%=hdnDiagnoseText.ClientID %>').val(entity.DiagnosisText);
                $('#<%=txtDiagnosisText.ClientID %>').val(entity.DiagnosisText);
                $('#<%=hdnMorphologyID.ClientID %>').val(entity.MorphologyID);
                $('#<%=chkIsFollowUp.ClientID %>').prop('checked', (entity.IsFollowUpCase == 'True'));
                $('#<%=chkIsChronic.ClientID %>').prop('checked', (entity.IsChronicDisease == 'True'));
                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('0');

                if ($('#<%=hdnIsMainDiagnosisExists.ClientID %>').val() == "0")
                    cboDiagnoseType.SetValue(Constant.DiagnosisType.MAIN_DIAGNOSIS);
                else 
                    cboDiagnoseType.SetValue(Constant.DiagnosisType.COMPLICATION);
                

                cboPhysician.SetValue($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                cboStatus.SetValue(Constant.DiagnosisStatus.UNDER_INVESTIGATION);
                ledDiagnose.SetValue('');
                ledMorphology.SetValue('');
                $('#<%=chkIsFollowUp.ClientID %>').prop('checked', false);
                $('#<%=chkIsChronic.ClientID %>').prop('checked', false);
                $('#<%=hdnDiagnoseID.ClientID %>').val('');
                $('#<%=hdnDiagnoseText.ClientID %>').val('');
                $('#<%=txtDiagnosisText.ClientID %>').val('');
                $('#<%=hdnMorphologyID.ClientID %>').val('');
                $('#<%=txtRemarks.ClientID %>').val('');
            }
        }
        //#endregion

        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnDiagnoseText.ClientID %>').val(led.GetDisplayText());
            $('#<%=txtDiagnosisText.ClientID %>').val($('#<%=hdnDiagnoseText.ClientID %>').val());
            ledMorphology.SetFilterExpression("DiagnoseID = '" + diagnoseID + "' AND IsDeleted = 0");
        }

        function onLedMorphologyLostFocus(value) {
            $('#<%=hdnMorphologyID.ClientID %>').val(value);
        }
    </script>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="0" id="hdnIsMainDiagnosisExists" runat="server" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
            <td><asp:TextBox ID="txtDifferentialDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtDifferentialTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Diagnose Type")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboDiagnoseType" ClientInstanceName="cboDiagnoseType" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diagnosis")%></label></td>
            <td colspan="2">
                <input type="hidden" value="" id="hdnDiagnoseID" runat="server" />
                <input type="hidden" value="" id="hdnDiagnoseText" runat="server" />
                <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server" Width="500px"
                    ValueText="DiagnoseID" FilterExpression="IsDeleted = 0 AND IsNutritionDiagnosis = 0" DisplayText="DiagnoseName" MethodName="GetDiagnosisList" >
                    <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                    <Columns>
                        <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName" Description="i.e. Cholera" Width="500px" />
                    </Columns>
                </qis:QISSearchTextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel ">
                <label class="lblMandatory"><%=GetLabel("Diagnosis Text")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtDiagnosisText" Width="500px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Morphology")%></label></td>
            <td colspan="2">
                <input type="hidden" value="" id="hdnMorphologyID" runat="server" />
                <qis:QISSearchTextBox ID="ledMorphology" ClientInstanceName="ledMorphology" runat="server" Width="500px"
                    ValueText="MorphologyID" FilterExpression="IsDeleted = 0" DisplayText="MorphologyName" MethodName="GetMorphologyList" >
                    <ClientSideEvents ValueChanged="function(s){ onLedMorphologyLostFocus(s.GetValueText()); }" />
                    <Columns>
                        <qis:QISSearchTextBoxColumn Caption="Morphology Name" FieldName="MorphologyName" Description="i.e. Neoplasm" Width="300px" />
                        <qis:QISSearchTextBoxColumn Caption="Morphology Code" FieldName="MorphologyID" Description="i.e. M8000/0" Width="100px" />
                    </Columns>
                </qis:QISSearchTextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboStatus" ClientInstanceName="cboStatus" Width="300px" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="2"><asp:CheckBox runat="server" ID="chkIsFollowUp" /><%=GetLabel("Follow Up")%></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="2"><asp:CheckBox runat="server" ID="chkIsChronic" /><%=GetLabel("Chronic")%></td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top">
                <label class="lblNormal">
                    <%=GetLabel("Remarks")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3"
                    Width="500px" />
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
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" >
                                    <HeaderTemplate>
                                        <%=GetLabel("Diagnose Information")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("DifferentialDateInString")%>, <%#: Eval("DifferentialTime")%>, <%#: Eval("cfParamedicNameEarlyDiagnosis")%></div>
                                        <div>
                                                <span style="color:Blue; font-size:1.1em"><%#: Eval("DiagnosisText")%></span>
                                                (<b><%#: Eval("DiagnoseID")%></b>)
                                        </div>
                                        <div><%#: Eval("ICDBlockName")%></div>
                                        <div><b><%#: Eval("DiagnoseType")%></b> - <%#: Eval("DifferentialStatus")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Remarks")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("Remarks")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                        <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                        <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                        <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                        <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                        <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                        <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                        <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
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
