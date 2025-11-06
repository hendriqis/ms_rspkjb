<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingDiagnoseItemEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Nursing.Program.NursingDiagnoseItemEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>



<div style="height: auto; max-height:540px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnNursingDiagnoseID" value="" runat="server" />
    <input type="hidden" id="hdnNursingDiagnosisType" value="" runat="server" />
    <input type="hidden" id="hdnNursingOutcomeID" value="" runat="server" />
    <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
    <input type="hidden" value="" id="hdnIsNursingOutcome" runat="server" />
    <input type="hidden" value="" id="hdnIsSubjectiveObjectiveData" runat="server" />
    <input type="hidden" value="" id="hdnIsAdd" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"  style="vertical-align:top"><label class="lblNormal"><%=GetLabel("Diagnosa Keperawatan")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtNursingDiagnoseName" ReadOnly="true" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>  
                </table>
                <table width="100%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col/>
                    </colgroup>
                    <tr valign="top">
                        <td style="padding-top:8px">
                            <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                                ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                    EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                            <asp:GridView ID="grdEntryPopupGrdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="NursingItemGroupSubGroupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <div style="padding-left:3px">
                                                                <%=GetLabel("Komponen Diagnosa")%>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style='margin-left:<%#: Eval("CfMargin") %>0px;'><%#: Eval("NursingItemGroupSubGroupText")%></div>
                                                            <input type="hidden" value="<%#:Eval("NursingItemGroupSubGroupID") %>" bindingfield="NursingItemGroupSubGroupID" />
                                                            <input type="hidden" value="<%#:Eval("NursingItemGroupSubGroupCode") %>" bindingfield="NursingItemGroupSubGroupCode" />
                                                            <input type="hidden" value="<%#:Eval("NursingItemGroupSubGroupText") %>" class="description" bindingfield="NursingItemGroupSubGroupText" />
                                                            <input type="hidden" value="<%#:Eval("IsHeader") %>" class="hdnIsHeader" bindingfield="IsHeader" />
                                                            <input type="hidden" value="<%#:Eval("IsNursingOutcome") %>" class="hdnIsNursingOutcome" bindingfield="IsNursingOutcome" />
                                                            <input type="hidden" value="<%#:Eval("IsSubjectiveObjectiveData") %>" class="hdnIsSubjectiveObjectiveData" bindingfield="IsSubjectiveObjectiveData" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Struktur Diagnosa Keperawatan belum didefinisikan")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingEntry"></div>
                                </div>
                            </div> 
                        </td>
                        <td>
                            <table width="100%" id="tblEntryData">
                                <tr>
                                    <td>
                                         <div id="containerPopupEntryData" style="display:none">
                                            <fieldset id="fsEntryPopup" style="margin:0"> 
                                                <input type="hidden" runat="server" id="hdnID" />
                                                <table class="tblEntryDetail">
                                                    <colgroup>
                                                        <col style="width:150px"/>
                                                        <col style="width:150px"/>
                                                        <col />
                                                    </colgroup>
                                                    <tr valign="top">
                                                        <td class="tdLabel" style="padding-top:5px"><label class="lblMandatory lblLink" id="lblNursingItem"><%=GetLabel("Deskripsi")%></label></td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtNursingItemEntry" runat="server" TextMode="MultiLine" Wrap="true" Columns="55" Rows="3" Width="100%" />
                                                        </td>
                                                    </tr>
                                                    <tr valign="top" style="display:none">
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Judul Skala")%></label></td>
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <colgroup>
                                                                    <col style="width:20%"/>
                                                                    <col style="width:20%"/>
                                                                    <col style="width:20%"/>
                                                                    <col style="width:20%"/>
                                                                    <col style="width:20%"/>
                                                                </colgroup> 
                                                                <tr>
                                                                    <td align="center"><label class="lblNormal"><%=GetLabel("1")%></label></td>
                                                                    <td align="center"><label class="lblNormal"><%=GetLabel("2")%></label></td>
                                                                    <td align="center"><label class="lblNormal"><%=GetLabel("3")%></label></td>
                                                                    <td align="center"><label class="lblNormal"><%=GetLabel("4")%></label></td>
                                                                    <td align="center"><label class="lblNormal"><%=GetLabel("5")%></label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><asp:TextBox runat="server" ID="txtScale1Text" Width="100%" /></td>
                                                                    <td align="center"><asp:TextBox runat="server" ID="txtScale2Text" Width="100%" /></td>
                                                                    <td align="center"><asp:TextBox runat="server" ID="txtScale3Text" Width="100%" /></td>
                                                                    <td align="center"><asp:TextBox runat="server" ID="txtScale4Text" Width="100%" /></td>
                                                                    <td align="center"><asp:TextBox runat="server" ID="txtScale5Text" Width="100%" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="trOutcome">
                                                        <td class="tdLabel" style="padding-top:5px"><label class="lblOutcomeResult" id="lblOutcomeResult" runat="server"><%=GetLabel("Ekspektasi")%></label></td>
                                                        <td><asp:TextBox ID="txtOutcomeResult" runat="server" Width="100%" /></td>
                                                        <td><asp:CheckBox runat="server" ID="chkIsUsingIndicator" Text=" Kriteria Hasil" /> </td>
                                                    </tr>
                                                    <tr id="trSubjectiveObjectiveData">
                                                        <td />
                                                        <td>
                                                            <asp:RadioButtonList ID="rblIsSubjectiveObjectiveData" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text=" Mayor" Value="1" Selected="True" />
                                                                <asp:ListItem Text=" Minor" Value="2"  />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td><asp:CheckBox runat="server" ID="chkIsEditableByUser" Text=" Teks dapat diubah" /> </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <center>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' class="btnEntryPopupSave w3-btn w3-hover-blue"/>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top">
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
                                                                        <input type="hidden" value="<%#:Eval("NursingDiagnoseItemID") %>" bindingfield="NursingDiagnoseItemID" />
                                                                        <input type="hidden" value="<%#:Eval("NursingItemText") %>" bindingfield="NursingItemText" />
                                                                        <input type="hidden" value="<%#:Eval("Scale1Text") %>" bindingfield="Scale1Text" />
                                                                        <input type="hidden" value="<%#:Eval("Scale2Text") %>" bindingfield="Scale2Text" />
                                                                        <input type="hidden" value="<%#:Eval("Scale3Text") %>" bindingfield="Scale3Text" />
                                                                        <input type="hidden" value="<%#:Eval("Scale4Text") %>" bindingfield="Scale4Text" />
                                                                        <input type="hidden" value="<%#:Eval("Scale5Text") %>" bindingfield="Scale5Text" />
                                                                        <input type="hidden" value="<%#:Eval("NursingOutcomeResult") %>" bindingfield="NursingOutcomeResult" />
                                                                        <input type="hidden" value="<%#:Eval("IsUsingIndicator") %>" bindingfield="IsUsingIndicator" />
                                                                        <input type="hidden" value="<%#:Eval("IsEditableByUser") %>" bindingfield="IsEditableByUser" />
                                                                        <input type="hidden" value="<%#:Eval("IsMajorData") %>" bindingfield="IsMajorData" />
                                                                        <input type="hidden" value="<%#:Eval("IsMinorData") %>" bindingfield="IsMinorData" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="NursingDiagnoseItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                <asp:BoundField DataField="NursingItemText" HeaderText="Deskripsi" ItemStyle-HorizontalAlign="Left"/>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <%=GetLabel("Belum ada Detail Struktur Diagnosa Keperawatan yang didefinisikan")%>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                                            <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>   
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#<%=grdEntryPopupGrdView.ClientID %> tr:gt(0)').live('click', function () {
        $('#<%=grdEntryPopupGrdView.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnItemGroupID.ClientID %>').val($(this).find('.keyField').html());
        var isHeader = ($(this).find('.hdnIsHeader').val());
        var isNursingOutcome = ($(this).find('.hdnIsNursingOutcome').val());
        var isSubjectiveObjectiveData = ($(this).find('.hdnIsSubjectiveObjectiveData').val());
        var description = ($(this).find('.description').val());
        $('#<%=hdnIsNursingOutcome.ClientID %>').val(isNursingOutcome);
        
        $('#lblNursingItem').text(description);
        if (isHeader == 'True')
            $('#tblEntryData').hide();
        else
            $('#tblEntryData').show();

        if (isNursingOutcome != 'True') {
            $('#lblNursingItem').removeClass("lblLink");
            $('#trOutcome').attr('style', 'display:none');
        }
        else {
            $('#lblNursingItem').addClass("lblLink");
            $('#trOutcome').removeAttr('style');
        }

        if (isSubjectiveObjectiveData != 'True') {
            $('#lblNursingItem').removeClass("lblLink");
            $('#trSubjectiveObjectiveData').attr('style', 'display:none');
            $('#<%=hdnIsSubjectiveObjectiveData.ClientID %>').val("0");
        }
        else {
            $('#lblNursingItem').addClass("lblLink");
            $('#trSubjectiveObjectiveData').removeAttr('style');
            $('#<%=hdnIsSubjectiveObjectiveData.ClientID %>').val("1");
        }
        cbpEntryPopupView1.PerformCallback('refresh');
    });
    $('#<%=grdEntryPopupGrdView.ClientID %> tr:eq(2)').click();

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    var currPage = parseInt('<%=CurrPage %>');
    $(function () {
        setPaging($("#pagingEntry"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, null, currPage);
    });

    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdEntryPopupGrdView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingEntry"), pageCount, function (page) {
                cbpEntryPopupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdEntryPopupGrdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

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
        hideLoadingPanel();
    }

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtNursingItemEntry.ClientID %>').val('');
        $('#<%=txtScale1Text.ClientID %>').val('');
        $('#<%=txtScale2Text.ClientID %>').val('');
        $('#<%=txtScale3Text.ClientID %>').val('');
        $('#<%=txtScale4Text.ClientID %>').val('');
        $('#<%=txtScale5Text.ClientID %>').val('');
        $('#<%=txtOutcomeResult.ClientID %>').val('');
        $('#<%=chkIsUsingIndicator.ClientID %>').prop('checked', false);
        $('#<%=chkIsEditableByUser.ClientID %>').prop('checked', false);
        $('#<%=rblIsSubjectiveObjectiveData.ClientID %>').find("input[value='1']").prop("checked", true);
        $('#<%=hdnIsAdd.ClientID %>').val('1');
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

    //#region Nursing Outcome
    $('#lblNursingItem.lblLink').live('click', function () {
        openSearchDialog('nursingOutcome', "", function (value) {
            onTxtNursingItemCodeChanged(value);
        });
    });

    function onTxtNursingItemCodeChanged(value) {
        var filterExpression = " NursingOutcomeCode = '" + value + "'";
        Methods.getObject('GetNursingOutcomeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnNursingOutcomeID.ClientID %>').val(result.NursingOutcomeID);
                $('#<%=txtNursingItemEntry.ClientID %>').val(result.NursingOutcomeText);
                $('#<%=txtOutcomeResult.ClientID %>').val(result.NursingOutcomeResult);
            }
            else {
                $('#<%=hdnNursingOutcomeID.ClientID %>').val("0");
                $('#<%=txtNursingItemEntry.ClientID %>').val("");
                $('#<%=txtOutcomeResult.ClientID %>').val("");
            }
        });
        cbpView.PerformCallback('refresh');
    }

    //#endregion

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        if (confirm("Are You Sure Want To Delete This Data?")) {
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.NursingDiagnoseItemID);
            cbpEntryPopupView1.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.NursingDiagnoseItemID);
        $('#<%=txtNursingItemEntry.ClientID %>').val(entity.NursingItemText);
        $('#<%=txtScale1Text.ClientID %>').val(entity.Scale1Text);
        $('#<%=txtScale2Text.ClientID %>').val(entity.Scale2Text);
        $('#<%=txtScale3Text.ClientID %>').val(entity.Scale3Text);
        $('#<%=txtScale4Text.ClientID %>').val(entity.Scale4Text);
        $('#<%=txtScale5Text.ClientID %>').val(entity.Scale5Text);
        $('#<%=txtOutcomeResult.ClientID %>').val(entity.NursingOutcomeResult);
        $('#<%=chkIsUsingIndicator.ClientID %>').prop('checked', entity.IsUsingIndicator == 'True');
        $('#<%=chkIsEditableByUser.ClientID %>').prop('checked', entity.IsEditableByUser == 'True');

        if (entity.IsMajorData == "True") {
            $('#<%=rblIsSubjectiveObjectiveData.ClientID %>').find("input[value='1']").prop("checked", true);
        }
        if (entity.IsMinorData == "True") {
            $('#<%=rblIsSubjectiveObjectiveData.ClientID %>').find("input[value='2']").prop("checked", true);
        }

        $('#containerPopupEntryData').show();
    });

</script>

