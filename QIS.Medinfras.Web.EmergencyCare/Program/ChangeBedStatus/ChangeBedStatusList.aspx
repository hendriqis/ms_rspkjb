<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="ChangeBedStatusList.aspx.cs" Inherits="QIS.Medinfras.Web.EmergencyCare.Program.ChangeBedStatusList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsBedStatus', 'mpBedStatus'))
                    onRefreshGridView();
            });

            //#region Room
            $('#<%=lblRoom.ClientID %>.lblLink').live('click', function () {
                var serviceUnitID = cboServiceUnit.GetValue();
                var filterExpression = '';
                if (serviceUnitID != '') {
                    filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
                    openSearchDialog('serviceunitroom', filterExpression, function (value) {
                        $('#<%=txtRoomCode.ClientID %>').val(value);
                        onTxtRoomCodeChanged(value);
                    });
                }
            });

            $('#<%=txtRoomCode.ClientID %>').live('change', function () {
                onTxtRoomCodeChanged($(this).val());
            });

            function onTxtRoomCodeChanged(value) {
                var filterExpression = '';
                var serviceUnitID = cboServiceUnit.GetValue();
                if (serviceUnitID != '') {
                    filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ";
                    filterExpression += "RoomCode = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnRoomID.ClientID %>').val(result.RoomID);
                            $('#<%=txtRoomName.ClientID %>').val(result.RoomName);
                            $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
                        }
                        else {
                            $('#<%=hdnRoomID.ClientID %>').val('');
                            $('#<%=txtRoomCode.ClientID %>').val('');
                            $('#<%=txtRoomName.ClientID %>').val('');
                            $('#<%=hdnClassID.ClientID %>').val('');
                        }
                        onRefreshGridView();
                    });
                }
                else {
                    $('#<%=hdnRoomID.ClientID %>').val('');
                    $('#<%=txtRoomCode.ClientID %>').val('');
                    $('#<%=txtRoomName.ClientID %>').val('');
                }
            }
            //#endregion
        });

        function onAfterSaveRecordPatientPageEntry(param) {
            onRefreshGridView();
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onRefreshGridView();
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsBedStatus', 'mpBedStatus')) {
                window.clearInterval(intervalID);
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onCboServiceUnitValueChanged() {
            $('#<%=hdnRoomID.ClientID %>').val('');
            $('#<%=txtRoomCode.ClientID %>').val('');
            $('#<%=txtRoomName.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

        }

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        function onCboBedStatusChanged(s) {
            $tr = $(s.GetInputElement()).closest('tr').parent().closest('tr');
            var GCBedStatus = $tr.find('.divBedStatus').html();
            if (s.GetValue() == GCBedStatus)
                $tr.find('.btnSave').attr('enabled', 'false');
            else
                $tr.find('.btnSave').removeAttr('enabled');

            $tr.find('.divBedStatusCurrText').html(s.GetText());
            $tr.find('.divBedStatusCurrValue').html(s.GetValue());
        }

        $btnSave = null;
        $('.btnSave').live('click', function () {
            if ($(this).attr('enabled') != 'false') {
                var bedID = $(this).closest('tr').find('.keyField').html();
                var bedStatus = $(this).closest('tr').find('.divBedStatusCurrValue').html();

                var param = bedID + '|' + bedStatus;
                $btnSave = $(this);
                cbpSaveBedStatus.PerformCallback(param);
            }
        });

        $btnBedQuickPicks = null;
        $('.btnBedQuickPicks').live('click', function () {
            if ($(this).attr('enabled') != 'false') {
                var key = $(this).closest('tr').find('.keyField').html();
                var url = ResolveUrl('~/Program/PatientTransfer/PatientTransferCtl.ascx');
                var id = key;
                openUserControlPopup(url, id, 'Pasien Pindah', 1100, 500);
            }
        });

        $btnBedQuickPicks = null;
        $('.btnDisposisi').live('click', function () {
            if ($(this).attr('enabled') != 'false') {
                var key = $(this).closest('tr').find('.keyField').html();
                var url = ResolveUrl('~/Program/PatientDischarge/PatientDischargeEntryCtl.ascx');
                var id = key;
                openUserControlPopup(url, id, 'Pasien Disposisi', 500, 500);
            }
        });

        function onCbpSaveBedStatusEndCallback(s) {
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                $tr = $btnSave.closest('tr');
                $tr.find('.divBedStatus').html($tr.find('.divBedStatusCurrValue').html());
                $tr.find('.tdCurrentStatus').html($tr.find('.divBedStatusCurrText').html());
                $btnSave.attr('enabled', 'false');
            }
            else {
                if (result[1] != '')
                    showToast('Save Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Save Failed', '');
            }
            hideLoadingPanel();
        }

    </script>
    <input type="hidden" runat="server" id="hdnBedID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitICUID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitPICUID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitNICUID" value="" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsBedStatus">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Ruang Perawatan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" runat="server" id="lblRoom">
                                        <%=GetLabel("Kamar")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                    <input type="hidden" id="hdnClassID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRoomName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("Setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("Menit")%>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="BedID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RoomCode" HeaderText="Kode Kamar" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("No Tempat Tidur")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="<%#: Eval("GCGender").ToString() == "0003^M" ? "background-color:#4ac5e3" : Eval("GCGender").ToString() == "0003^F" ? "background-color:#ffbdde" : "background-color:white" %>;
                                                        font-size: 15pt; font-weight: bold">
                                                        <%#:Eval("BedCode")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No.RM" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-Width="150px" />
                                            <asp:CheckBoxField DataField="IsTemporary" HeaderText="Sementara" HeaderStyle-Width="50px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="BedStatus" HeaderText="Status Saat Ini" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-Width="145px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Ubah Status") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="display: none" class="divBedStatusCurrText">
                                                        <%#:Eval("BedStatus") %></div>
                                                    <div style="display: none" class="divBedStatus">
                                                        <%#:Eval("GCBedStatus") %></div>
                                                    <div style="display: none" class="divBedStatusCurrValue">
                                                        <%#:Eval("GCBedStatus")%></div>
                                                    <dxe:ASPxComboBox ID="cboBedStatus" runat="server" Width="90%">
                                                        <ClientSideEvents ValueChanged="function(s,e){ onCboBedStatusChanged(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Simpan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="button" id="btnSave" class="btnSave" enabled="false" value="Simpan"
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Pasien Pindah")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="button" id="btnBedQuickPicks" class="btnBedQuickPicks" value="Pindah"
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Pasien Disposisi")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="button" id="btnDisposisi" class="btnDisposisi" value="Disposisi"
                                                        runat="server" />
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
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpSaveBedStatus" runat="server" Width="100%" ClientInstanceName="cbpSaveBedStatus"
            ShowLoadingPanel="false" OnCallback="cbpSaveBedStatus_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpSaveBedStatusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
