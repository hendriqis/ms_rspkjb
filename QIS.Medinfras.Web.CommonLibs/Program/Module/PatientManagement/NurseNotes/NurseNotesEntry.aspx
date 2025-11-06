<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="NurseNotesEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NurseNotesEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtActualVisitDate.ClientID %>');
            $('#<%=txtActualVisitDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtActualVisitDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#trTransactionTime').attr('style', 'display:none');
            $('#<%=chkIsFilterByTime.ClientID %>').change(function () {
                if ($('#<%:chkIsFilterByTime.ClientID %>').is(':checked')) {
                    $('#trTransactionTime').removeAttr('style');
                }
                else {
                    $('#trTransactionTime').attr('style', 'display:none');
                    $('#<%=txtStartTime.ClientID %>').val('00:00');
                    $('#<%=txtEndTime.ClientID %>').val('23:59');
                }
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onCboServiceUnitValueChanged() {
            onRefreshGridView();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        $('.txtNotes').live('keydown', function () {
            $(this).closest('tr').find('.btnSave').removeAttr('enabled');
        });

        $btnSave = null;
        $('.btnSave').live('click', function () {
            if ($(this).attr('enabled') != 'false') {
                $tr = $(this).closest('tr');
                var visitID = $tr.find('.keyField').html();
                var notes = $tr.find('.txtNotes').val();

                var param = visitID + '|' + notes;
                $btnSave = $(this);
                cbpSaveDiagnose.PerformCallback(param);
            }
        });

        function onCbpSaveDiagnoseEndCallback(s) {
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                $tr = $btnSave.closest('tr');
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
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%>
            :
            <%=GetLabel("Pilih Pasien")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label>
                                        <%=GetLabel("Tanggal Kunjungan") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtActualVisitDate" Width="120px" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsFilterByTime" runat="server" Checked="false" /><%:GetLabel("Filter By Time")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trTransactionTime">
                                <td>
                                    <label>
                                        <%=GetLabel("Jam Konfirmasi") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStartTime" Width="54px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndTime" Width="54px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Ruang Perawatan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="200px"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="580px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
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
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Pasien")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("RegistrationNo") %></div>
                                                    <div>
                                                        <%#:Eval("MedicalNo") %>
                                                        -
                                                        <%#:Eval("PatientName") %></div>
                                                    <div>
                                                        Physician :
                                                        <%#:Eval("ParamedicName") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="StartServiceTime" HeaderText="Jam Konfirmasi" HeaderStyle-Width="50px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Unit Pelayanan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("ServiceUnitName") %></div>
                                                    <div>
                                                        <%#:Eval("RoomName") %>
                                                        <%#:Eval("BedCode") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DiagnoseName" HeaderText="Nama Diagnosa" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DiagnosisText" HeaderText="Diagnosa Text" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("EWS") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="text" runat="server" id="txtEWS" class="txtEWS" style="width: 100%"/>
                                                    <div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Catatan") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="display: none" class="divNotesValue">
                                                    </div>
                                                    <input type="text" runat="server" id="txtNotes" class="txtNotes" style="width: 100%" />
                                                    <div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="button" id="btnSave" class="btnSave" enabled="false" value="Simpan"
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
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpSaveDiagnose" runat="server" Width="100%" ClientInstanceName="cbpSaveDiagnose"
            ShowLoadingPanel="false" OnCallback="cbpSaveDiagnose_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpSaveDiagnoseEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
