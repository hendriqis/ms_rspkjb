<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingDiagnoseItemIndicatorEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Nursing.Program.NursingDiagnoseItemIndicatorEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>



<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnNursingDiagnoseItemID" value="" runat="server" />
    <input type="hidden" value="" id="hdnIsAdd" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:200px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diagnosis")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtNursingDiagnoseName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kalimat Penghubung")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtGroup" ReadOnly="true" Width="100%" runat="server" TextMode="MultiLine" Wrap="true"/></td>
                    </tr>  
                    <tr valign="top">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Luaran")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtDiagnosisItem" TextMode="MultiLine" Wrap="true" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>
                </table>

                <div id="containerPopupEntryData" style="display:none">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <input type="hidden" runat="server" id="hdnID" />
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:110px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblNursingIndicator"><%=GetLabel("Kriteria Hasil")%></label></td>
                                <td>
                                    <input type="hidden" value="" runat="server" id="hdnNursingIndicatorID" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:20%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtNursingIndicatorCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtNursingIndicatorText" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td colspan="3">
                                    <table width="100%">
                                        <colgroup>
                                            <col style="width:110px"/>
                                        </colgroup> 
                                        <tr>
                                            <td></td>
                                            <td align="center"><label class="lblNormal"><%=GetLabel("Skala I")%></label></td>
                                            <td align="center"><label class="lblNormal"><%=GetLabel("Skala II")%></label></td>
                                            <td align="center"><label class="lblNormal"><%=GetLabel("Skala III")%></label></td>
                                            <td align="center"><label class="lblNormal"><%=GetLabel("Skala IV")%></label></td>
                                            <td align="center"><label class="lblNormal"><%=GetLabel("Skala V")%></label></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Judul Skala")%></label></td>
                                            <td align="center"><asp:TextBox runat="server" ID="txtScale1Text" Width="100%" /></td>
                                            <td align="center"><asp:TextBox runat="server" ID="txtScale2Text" Width="100%" /></td>
                                            <td align="center"><asp:TextBox runat="server" ID="txtScale3Text" Width="100%" /></td>
                                            <td align="center"><asp:TextBox runat="server" ID="txtScale4Text" Width="100%" /></td>
                                            <td align="center"><asp:TextBox runat="server" ID="txtScale5Text" Width="100%" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' class="btnEntryPopupSave w3-btn w3-hover-blue" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' class="btnEntryPopupCancel w3-btn w3-hover-blue"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                             
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView1" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView1"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView1_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback1(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="pnlEntry" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdSaved" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;" /></td>
                                                        <td style="width:3px">&nbsp;</td>
                                                        <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("NursingDiagnoseItemIndicatorID") %>" bindingfield="NursingDiagnoseItemIndicatorID" />
                                                <input type="hidden" value="<%#:Eval("NursingIndicatorID") %>" bindingfield="NursingIndicatorID" />
                                                <input type="hidden" value="<%#:Eval("NursingIndicatorCode") %>" bindingfield="NursingIndicatorCode" />
                                                <input type="hidden" value="<%#:Eval("NursingIndicatorText") %>" bindingfield="NursingIndicatorText" />
                                                <input type="hidden" value="<%#:Eval("Scale1Text") %>" bindingfield="Scale1Text" />
                                                <input type="hidden" value="<%#:Eval("Scale2Text") %>" bindingfield="Scale2Text" />
                                                <input type="hidden" value="<%#:Eval("Scale3Text") %>" bindingfield="Scale3Text" />
                                                <input type="hidden" value="<%#:Eval("Scale4Text") %>" bindingfield="Scale4Text" />
                                                <input type="hidden" value="<%#:Eval("Scale5Text") %>" bindingfield="Scale5Text" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NursingDiagnoseItemIndicatorID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="NursingIndicatorCode" HeaderText="Kode" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"/>
                                        <asp:BoundField DataField="NursingIndicatorText" HeaderText="Kriteria Hasil" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="Scale1Text" HeaderText="Skala-1" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"/>
                                        <asp:BoundField DataField="Scale2Text" HeaderText="Skala-2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"/>
                                        <asp:BoundField DataField="Scale3Text" HeaderText="Skala-3" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"/>
                                        <asp:BoundField DataField="Scale4Text" HeaderText="Skala-4" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"/>
                                        <asp:BoundField DataField="Scale5Text" HeaderText="Skala-5" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"/>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function onCbpEntryPopupViewEndCallback1(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#lblEntryPopupAddData').click();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#<%=hdnIsAdd.ClientID %>').val('1');
        hideLoadingPanel();
    }

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnNursingIndicatorID.ClientID %>').val('0');
        $('#<%=txtNursingIndicatorCode.ClientID %>').val('');
        $('#<%=txtNursingIndicatorText.ClientID %>').val('');
        $('#<%=txtNursingIndicatorCode.ClientID %>').removeAttr('readonly');
        $('#containerPopupEntryData').show();
    });
    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView1.PerformCallback('save');
        return false;
    });

    //#region Nursing Item
    function onGetNursingIndicatorFilterExpression() {
        var filterExpression = "<%:OnGetNursingIndicatorFilterExpression() %>";
        return filterExpression;
    }

    $(function () {
        $('#lblNursingIndicator.lblLink').click(function () {
            openSearchDialog('nursingIndicator', onGetNursingIndicatorFilterExpression(), function (value) {
                $('#<%=txtNursingIndicatorCode.ClientID %>').val(value);
                onTxtNursingIndicatorCodeChanged(value);
            });
        });

        $('#<%=txtNursingIndicatorCode.ClientID %>').change(function () {
            onTxtNursingIndicatorCodeChanged($(this).val());
        });

        function onTxtNursingIndicatorCodeChanged(value) {
            var filterExpression = onGetNursingIndicatorFilterExpression() + " AND NursingIndicatorCode = '" + value + "'";
            Methods.getObject('GetNursingIndicatorList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnNursingIndicatorID.ClientID %>').val(result.NursingIndicatorID);
                    $('#<%=txtNursingIndicatorText.ClientID %>').val(result.NursingIndicatorText);
                    $('#<%=txtScale1Text.ClientID %>').val(result.Scale1Text);
                    $('#<%=txtScale2Text.ClientID %>').val(result.Scale2Text);
                    $('#<%=txtScale3Text.ClientID %>').val(result.Scale3Text);
                    $('#<%=txtScale4Text.ClientID %>').val(result.Scale4Text);
                    $('#<%=txtScale5Text.ClientID %>').val(result.Scale5Text);
                }
                else {
                    $('#<%=hdnNursingIndicatorID.ClientID %>').val('0');
//                    $('#<%=txtNursingIndicatorCode.ClientID %>').val('');
//                    $('#<%=txtNursingIndicatorText.ClientID %>').val('');
//                    $('#<%=txtScale1Text.ClientID %>').val('');
//                    $('#<%=txtScale2Text.ClientID %>').val('');
//                    $('#<%=txtScale3Text.ClientID %>').val('');
//                    $('#<%=txtScale4Text.ClientID %>').val('');
//                    $('#<%=txtScale5Text.ClientID %>').val('');
                }
            });
        }
    });
    //#endregion

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        if (confirm("Are You Sure Want To Delete This Data?")) {
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.NursingDiagnoseItemIndicatorID);
            cbpEntryPopupView1.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.NursingDiagnoseItemIndicatorID);
        $('#<%=hdnNursingIndicatorID.ClientID %>').val(entity.NursingIndicatorID);
        $('#<%=txtNursingIndicatorCode.ClientID %>').val(entity.NursingIndicatorCode);
        $('#<%=txtNursingIndicatorText.ClientID %>').val(entity.NursingIndicatorText);
        $('#<%=txtScale1Text.ClientID %>').val(entity.Scale1Text);
        $('#<%=txtScale2Text.ClientID %>').val(entity.Scale2Text);
        $('#<%=txtScale3Text.ClientID %>').val(entity.Scale3Text);
        $('#<%=txtScale4Text.ClientID %>').val(entity.Scale4Text);
        $('#<%=txtScale5Text.ClientID %>').val(entity.Scale5Text);
        $('#<%=txtNursingIndicatorCode.ClientID %>').attr('readonly','readonly');

        $('#containerPopupEntryData').show();
    });

</script>

