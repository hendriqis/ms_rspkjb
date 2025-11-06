<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
CodeBehind="ImagingTestResultDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Imaging.Program.ImagingTestResultDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailServiceCtl.ascx" TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailDrugMSCtl.ascx" TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailLogisticCtl.ascx" TagName="LogisticCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnClinicTransactionBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />        
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />        
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />  
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">

        var paramResultTest = "";
        $('.lnkHasil a').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var ItemId = $(this).closest('tr').find('.keyField').html();
                var imagingID = $('#<%=hdnID.ClientID %>').val();
                var paramResultTest = ItemId + "|" + imagingID;
                if ($('#<%=hdnID.ClientID %>').val() == "0") {
                    $('#<%=txtReferenceNo.ClientID %>').attr("readonly", "readonly");
                    $('#<%=txtResultDate.ClientID %>').attr("readonly", "readonly");
                    $('#<%=txtResultTime.ClientID %>').attr("readonly", "readonly");
                    $('#<%=txtProvider.ClientID %>').attr("readonly", "readonly");
                    $('#<%=txtParamedic.ClientID %>').attr("readonly", "readonly");
                    cbpProcess.PerformCallback();

                }

                var url = ResolveUrl("~/Program/WorkList/ImagingTestResultDetailCtl.ascx");
                openUserControlPopup(url, paramResultTest, 'Imaging Test Result Detail', 800, 600);
            }
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

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[1]);
            else {
                var url = ResolveUrl("~/Program/WorkList/ImagingTestResultDetailCtl.ascx");
                openUserControlPopup(url, paramResultTest, 'Imaging Test Result Detail', 800, 600);
            }
            hideLoadingPanel();
        }

        function onLoad() {
            setDatePicker('<%=txtResultDate.ClientID %>');

            $('#<%=btnClinicTransactionBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl("~/Program/WorkList/ImagingTestResult.aspx");
            });
            
        }
     
        $(function () {
            $('#<%=txtResultDate.ClientID %>').attr("readonly", "readonly");

            $('#<%=txtOrderNo.ClientID %>').attr("readonly", "readonly");
            $('#<%=txtOrderDate.ClientID %>').attr("readonly", "readonly");
            $('#<%=txtOrderTime.ClientID %>').attr("readonly", "readonly");
            $('#<%=txtOrderedBy.ClientID %>').attr("readonly", "readonly");

            setDatePicker('<%=txtResultDate.ClientID %>');

            $('#<%=txtResultDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtResultDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            if ($('#<%=txtReferenceNo.ClientID %>').val() != "") {
                $('#<%=txtReferenceNo.ClientID %>').attr("readonly", "readonly");
                $('#<%=txtResultDate.ClientID %>').attr("readonly", "readonly");
                $('#<%=txtResultTime.ClientID %>').attr("readonly", "readonly");
                $('#<%=txtProvider.ClientID %>').attr("readonly", "readonly");
                $('#<%=txtParamedic.ClientID %>').attr("readonly", "readonly");
            }


        });

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var testOrderID = s.cpTestOrderID;
                    onAfterSaveRecordDtSuccess(testOrderID);
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }
    </script> 
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" /> 
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Work List : Imaging Test Result Detail")%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:50%"/>
                <col style="width:50%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal lblMandatory" id="lblTransactionNo"><%=GetLabel("Reference No")%></label></td>
                            <td><asp:TextBox ID="txtReferenceNo" Width="120px" ReadOnly="true" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal lblMandatory" /><%=GetLabel("Result Date") %> - <%=GetLabel("Time") %></td>
                            <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px;width:120"><asp:TextBox ID="txtResultDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                    <td style="width:5px">&nbsp;</td>
                                    <td><asp:TextBox ID="txtResultTime" Width="120px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                </tr>
                            </table>
                        </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal lblMandatory" id="Label3"><%=GetLabel("Provider")%></label></td>
                            <td><asp:TextBox ID="txtProvider" Width="350px" ReadOnly="true" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal lblMandatory" id="Label4"><%=GetLabel("Physician")%></label></td>
                            <td><asp:TextBox ID="txtParamedic" Width="350px" ReadOnly="true" runat="server" /></td>
                        </tr>
                    </table>
                </td>
                <td style="padding:5px;vertical-align:top">
                    <input type="hidden" value="" id="hdnTestOrderID" runat="server" /> 
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label id="Label1" class="lblNormal" runat="server"><%=GetLabel("Order No")%></label></td>
                            <td><asp:TextBox ID="txtOrderNo" Width="120px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Order Date") %> - <%=GetLabel("Time") %></td>
                            <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px;width:140px"><asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                    <td style="width:5px">&nbsp;</td>
                                    <td><asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label id="Label2" class="lblNormal" runat="server"><%=GetLabel("Ordered by")%></label></td>
                            <td><asp:TextBox ID="txtOrderedBy" Width="120px" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr>
               <td colspan="2">
                    
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback"> 
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpViewHasilEndCallback(s); }" />
                    <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" HeaderStyle-Width="300px" />
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="300px" />
                                        <asp:HyperLinkField HeaderText="Hasil Pemeriksaan" Text="Hasil Pemeriksaan" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkHasil" HeaderStyle-Width="120px" />
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
                            <div id="paging"></div>
                            </div>
                        </div> 
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
        EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>