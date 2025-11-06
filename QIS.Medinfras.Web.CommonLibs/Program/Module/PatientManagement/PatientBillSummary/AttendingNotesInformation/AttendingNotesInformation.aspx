<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="AttendingNotesInformation.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AttendingNotesInformation" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li crudmode="R" runat="server" />        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erevaluationnotes">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onCboPhysicianListChanged(s) {
            if (s.GetValue() != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
            }
            else {
                $('#<%=hdnParamedicID.ClientID %>').val('');
            }
            cbpView.PerformCallback('refresh');
        }

        $('.imgAddDoctorFee.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var noteDate = $row.find('.cfDateInDatePicker').val();
            var transactionID = "0";
            var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
            var id = transactionID + "|" + hsuID + "|" + paramedicID + "|" + registrationID + "|" + visitID + "|" + chargeClassID + "|" + noteDate;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/AttendingNotesInformation/ChargesEntryQuickPicksCtl.ascx");
            openUserControlPopup(url, id, 'Doctor Fee', 1200, 500);
        });

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                cbpView.PerformCallback('refresh');
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var startdate = $('#<%=hdnStartDate.ClientID %>').val();
            var enddate = $('#<%=hdnEndDate.ClientID %>').val()
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();

            if (code == 'IP-00114') {
                filterExpression.text = registrationID + "|" + startdate + "|" + enddate + "|" + paramedicID;
                return true;
            }
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnRegistrationDate" runat="server" />
    <input type="hidden" id="hdnParamEndDate" runat="server" />
    <input type="hidden" id="hdnStartDate" runat="server" />
    <input type="hidden" id="hdnEndDate" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnChargeClassID" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsLockDown" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDLaboratory" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDImaging" runat="server" value="" />
    <div style="position: relative;">
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col width="200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Dokter ") %></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboPhysicianList" ClientInstanceName="cboPhysicianList" Width="350px"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboPhysicianListChanged(s); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
                            <Columns>
                                <asp:BoundField DataField="TempDate" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Date")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="hidden" class="cfDateInDatePicker" value='<%#:Eval("cfDateInDatePicker")%>' />
                                        <%#:Eval("cfDate")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfIsHasSOAP" HeaderText="Visite" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cfIsHasHonorVisite" HeaderText="Honor Visite" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TransactionNoHonorVisite" HeaderText="Honor Visite Transaction"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <img class="imgAddDoctorFee <%# (oIsLockDown.ToString() == "True") ? "imgDisabled" : Eval("cfIsHasSOAP") != "X" && Eval("cfIsHasHonorVisite") != "V" ? "imgLink" : "imgDisabled" %>"
                                            title='<%=GetLabel("Add Doctor Fee")%>' alt="" style="margin-right: 2px"
                                            src='<%# (oIsLockDown.ToString() == "True") ? ResolveUrl("~/Libs/Images/Button/plus_disabled.png") : Eval("cfIsHasSOAP") != "X" && Eval("cfIsHasHonorVisite") != "V" ? ResolveUrl("~/Libs/Images/Button/plus.png") : ResolveUrl("~/Libs/Images/Button/plus_disabled.png")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No data to display.") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>
