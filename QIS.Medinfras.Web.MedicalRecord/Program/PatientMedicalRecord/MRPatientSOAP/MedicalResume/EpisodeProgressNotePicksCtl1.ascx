<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeProgressNotePicksCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.EpisodeProgressNotePicksCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<style type="text/css">
        .trSelectedItem {background-color: #ecf0f1 !important;}        
        .highlight    {  background-color:#FE5D15; color: White; }       
</style>

<script type="text/javascript" id="dxss_progressNotesquickpicksHistoryCtl1">
    function onBeforeProcess(param) {
        if (!getCheckedMember()) {
            return false;
        }
        else {
            return true;
        }
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function getCheckedMember() {
        var lstSelectedMember = [];

        var result = '';
        var count = 0;

        $('.grdNormal .chkIsSelected input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').val();
            var noteDate = $tr.find('.noteDate').val();
            var noteTime = $tr.find('.noteTime').val();
            var subjectiveText = $tr.find('.subjectiveText').val();
            var objectiveText = $tr.find('.objectiveText').val();
            var resultText = noteDate + ";" + noteTime;

            if ($('#<%=chkIsSubjective.ClientID %>').is(":checked"))
                resultText = resultText + ";" + subjectiveText;
            else
                resultText = resultText + ";" + "";

            if ($('#<%=chkIsObjective.ClientID %>').is(":checked"))
                resultText = resultText + ";" + objectiveText;
            else
                resultText = resultText + ";" + "";

            lstSelectedMember.push(resultText);

            count += 1;
        });

        if (count == 0) {
            var messageBody = "Belum ada item yang dipilih.";
            displayMessageBox('Lookup : Riwayat Catatan Perkembangan : CPPT', messageBody);
            return false;
        }
        else {
            $('#<%=hdnSelectedItem.ClientID %>').val(lstSelectedMember.join('|'));
            return true;
        }
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        hideLoadingPanel();

        setPaging($("#pagingPopup"), pageCount, function (page) {
            RefreshGrid(true, page);
        });

        $('#<%=lvwPopupView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=lvwPopupView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnOrderID.ClientID %>').val($(this).find('.keyField').html());
            }
        });
        $('#<%=lvwPopupView.ClientID %> tr:eq(1)').click();
    });

    function RefreshGridDetail(mode, pageNo) {
        getCheckedMember();
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                RefreshGrid(true, page);
            });
            $('#<%=lvwPopupView.ClientID %> tr:eq(1)').click();
        }
    }
    //#endregion

    function onAfterProcessPopupEntry(param) {
        if (typeof onAfterLookUpProgressNote == 'function') {
            onAfterLookUpProgressNote(param);
        }
    }
</script>

<div style="padding:3px;">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnOrderID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnPopupVisitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedItem" runat="server" value="" />
    <input type="hidden" id="hdnPopupParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />    
    <input type="hidden" value="1" id="hdnDisplayMode" runat="server" />
    <input type="hidden" value="1" id="hdnMedicationStatus" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <table border="0" cellpadding="1" cellspacing="0">
                    <colgroup>
                        <col width="80px" />
                        <col width="100px" />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Salin Text :")%></label>
                        </td>
                        <td><asp:CheckBox ID="chkIsSubjective" runat="server" Text=" Subjective" ToolTip="Salin Text Subjective" Checked="true" /></td>
                        <td><asp:CheckBox ID="chkIsObjective" runat="server" Text=" Objective" ToolTip="Salin Text Objective" Checked="true" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding:2px;vertical-align:top">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="700px" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPopupViewEndCallback(s); hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage1">
                                <asp:ListView ID="lvwPopupView" runat="server" OnItemDataBound="lvwPopupView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th align="center" style="width:30px">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th align="left">
                                                    <div>
                                                        <%=GetLabel("SOAPI")%></div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th align="center" style="width:30px">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th align="left">
                                                    <div>
                                                        <%=GetLabel("SOAPI")%></div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center" style="width:30px;background:#ecf0f1; vertical-align:middle">
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </td>
                                            <td>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" class = "hdnPatientVisitNoteID" />
                                                <input type="hidden" value="<%#:Eval("cfNoteDate") %>" bindingfield="ID" class = "noteDate" />
                                                <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="ID" class = "noteTime" />
                                                <input type="hidden" value="<%#:Eval("SubjectiveText") %>" bindingfield="ID" class = "subjectiveText" />
                                                <input type="hidden" value="<%#:Eval("ObjectiveText") %>" bindingfield="ID" class = "objectiveText" />
                                                <div>
                                                    <span style="color: blue; font-style: italic; vertical-align: top">
                                                        <%#:Eval("cfNoteDate") %>, <%#:Eval("NoteTime") %>
                                                    </span>
                                                   </span>
                                                </div>
                                                <div style="height:auto; max-height: 130px; overflow-y: auto; margin-top: 15px;">
                                                    <%#Eval("cfNoteSOAP").ToString().Replace("\n","<br />")%><br />
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>