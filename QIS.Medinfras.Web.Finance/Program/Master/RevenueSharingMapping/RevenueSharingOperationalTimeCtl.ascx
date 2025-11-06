<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingOperationalTimeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingOperationalTimeCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_RevenueSharingOperationalTimeCtl">

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#lblEntryPopupAddDataOperationalTimeCtl').live('click', function () {
        $('#<%=hdnDayNumberCtl.ClientID %>').val("");
        cboDay.SetValue("");

        $('#<%=txtStartTime.ClientID %>').val("00:00");
        $('#<%=txtEndTime.ClientID %>').val("00:00");

        $('#<%=hdnOperationalTypeCtl.ClientID %>').val("");
        cboOperationalType.SetValue("");

        $('#containerPopupEntryData').show();
    });

    $('#<%=txtStartTime.ClientID %>').live('change', function () {
        var startTime = $('#<%=txtStartTime.ClientID %>').val();
        checkTimeFormat(startTime);
    });

    $('#<%=txtEndTime.ClientID %>').live('change', function () {
        var endTime = $('#<%=txtEndTime.ClientID %>').val();
        checkTimeFormat(endTime);
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var ID = $row.find('.ID').val();

            $('#<%=hdnIDCtl.ClientID %>').val(ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.ID').val();
        var DayNumber = $row.find('.DayNumber').val();
        var StartTime = $row.find('.StartTime').val();
        var EndTime = $row.find('.EndTime').val();
        var GCOperationalType = $row.find('.GCOperationalType').val();
        var OperationalType = $row.find('.OperationalType').val();

        $('#<%=hdnIDCtl.ClientID %>').val(ID);

        $('#<%=hdnDayNumberCtl.ClientID %>').val(DayNumber);
        cboDay.SetValue(DayNumber);

        $('#<%=txtStartTime.ClientID %>').val(StartTime);
        $('#<%=txtEndTime.ClientID %>').val(EndTime);

        $('#<%=hdnOperationalTypeCtl.ClientID %>').val(GCOperationalType);
        cboOperationalType.SetValue(GCOperationalType);

        $('#containerPopupEntryData').show();
    });

    function checkTimeFormat(value) {
        if (value.substr(2, 1) == ':') {
            if (!value.match(/^\d\d:\d\d/)) {
                displayErrorMessageBox('ERROR', "Format jam salah !");
            }
            else if (parseInt(value.substr(0, 2)) >= 24 || parseInt(value.substr(3, 2)) >= 60) {
                displayErrorMessageBox('ERROR', "Format jam salah !");
            }
        }
        else {
            displayErrorMessageBox('ERROR', "Format jam salah !");
        }
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                $('#containerImgLoadingViewPopup').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                $('#containerImgLoadingViewPopup').hide();
            }
        }
        $('#containerPopupEntryData').hide();
        $('#containerImgLoadingViewPopup').hide();
    }

</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnRevenueSharingIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnIDCtl" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 1px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Jasa Medis")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRevenueSharingCodeCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Jasa Medis")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRevenueSharingNameCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Hari")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDayNumberCtl" runat="server" value="" />
                                    <dxe:ASPxComboBox runat="server" Width="120px" ID="cboDay" ClientInstanceName="cboDay" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 90px" />
                                            <col style="width: 30px" />
                                            <col style="width: 90px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Mulai")%></label>
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Akhir")%></label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jam")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 90px" />
                                            <col style="width: 30px" />
                                            <col style="width: 90px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartTime" Width="100%" runat="server" MaxLength="5" style="text-align:center" />
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("s/d")%></label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndTime" Width="100%" runat="server" MaxLength="5" style="text-align:center"/>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Status Jam")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnOperationalTypeCtl" runat="server" value="" />
                                    <dxe:ASPxComboBox runat="server" Width="120px" ID="cboOperationalType" ClientInstanceName="cboOperationalType" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; position: relative;
                                font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="DayNumber" value="<%#: Eval("DayNumber")%>" />
                                                <input type="hidden" class="StartTime" value="<%#: Eval("StartTime")%>" />
                                                <input type="hidden" class="EndTime" value="<%#: Eval("EndTime")%>" />
                                                <input type="hidden" class="GCOperationalType" value="<%#: Eval("GCOperationalType")%>" />
                                                <input type="hidden" class="OperationalType" value="<%#: Eval("OperationalType")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfDayNameInIndonesian" ItemStyle-CssClass="cfDayNameInIndonesian"
                                            HeaderText="Hari" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="StartTime" ItemStyle-CssClass="StartTime" HeaderText="Jam Mulai"
                                            HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="EndTime" ItemStyle-CssClass="EndTime" HeaderText="Jam Akhir"
                                            HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="OperationalType" ItemStyle-CssClass="OperationalType"
                                            HeaderText="Status Jam" />
                                    </Columns>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="Div1">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddDataOperationalTimeCtl">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
