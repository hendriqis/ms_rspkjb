<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="CancelPatientDischargeList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.CancelPatientDischargeList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Process") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsBedStatus', 'mpBedStatus'))
                    refreshGrdRegisteredPatient();
            });

            $('#<%=btnProcess.ClientID %>').live('click', function (evt) {
                getCheckedRegistration();
                if ($('#<%=hdnSelectedVisit.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Registration First');
                }
                else {
                    var url = ResolveUrl('~/Program/Tools/CancelPatientDischarge/CancelPatientDischargeListCtl.ascx');
                    var visitID = $('#<%=hdnSelectedVisit.ClientID %>').val();
                    openUserControlPopup(url, visitID, 'Cancel Discharge Reason', 400, 230);
                    //                    cbpCancelPatientDischarge.PerformCallback();
                }
            });

            setDatePicker('<%=txtDischargeDate.ClientID %>');
            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                refreshGrdRegisteredPatient();
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function getCheckedRegistration() {
            var lstSelectedVisit = '';
            var result = '';
            $('#<%=grdView.ClientID %> .chkPatient input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedVisit != '')
                        lstSelectedVisit += ',';
                    lstSelectedVisit += key;
                }
            });
            $('#<%=hdnSelectedVisit.ClientID %>').val(lstSelectedVisit);
        }

        $('#chkSelectAllPatient').die('change');
        $('#chkSelectAllPatient').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkPatient').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

        function onCbpCancelPatientDischargeEndCallback(s) {
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                if (result[1] != '')
                    showToast('Save Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Save Failed', '');
            }
            hideLoadingPanel();
        }

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" id="hdnSelectedVisit" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <div style="padding: 15px">
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
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Pulang")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkPatient" runat="server" CssClass="chkPatient" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No.RM" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Ruang Perawatan" HeaderStyle-Width="250px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="RoomName" HeaderText="Kamar" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BedCode" HeaderText="Tempat Tidur" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ClassName" HeaderText="Kelas" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
        <dxcp:ASPxCallbackPanel ID="cbpCancelPatientDischarge" runat="server" Width="100%"
            ClientInstanceName="cbpCancelPatientDischarge" ShowLoadingPanel="false" OnCallback="cbpCancelPatientDischarge_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpCancelPatientDischargeEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
