<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
CodeBehind="ImagingTestResultDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ImagingTestResultDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnLaboratoryResultDecline" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div><%=GetLabel("Decline")%></div></li>
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
        var selectedItemID = "";
        var Verified = "";
        $('.lnkHasil a').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                selectedItemID = $(this).closest('tr').find('.keyField').html();
                var imagingID = $('#<%=hdnID.ClientID %>').val();
                if (imagingID == "0" || imagingID == "") {
                    cbpProcess.PerformCallback();
                }
                else {
                    var paramResultTest = selectedItemID + "|" + imagingID;
                    var isVerified = $(this).closest('tr').find('.hdnIsVerified').val();
                    if (isVerified == "True") {
                        var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/ImagingTestResultDetailVerifiedCtl.ascx");
                        openUserControlPopup(url, paramResultTest, 'Imaging Test Result Detail', 800, 600);
                    }
                    else {
                        var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/ImagingTestResultDetailCtl.ascx");
                        openUserControlPopup(url, paramResultTest, 'Imaging Test Result Detail', 800, 600);
                    }
                }
            }
        });

        $('.imgPrintIndo').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                selectedItemID = $(this).closest('tr').find('.keyField').html();
                var imagingID = $('#<%=hdnID.ClientID %>').val();
                var filter = selectedItemID + "|" + imagingID;
                var errMessage = { text: "" };
                var reportCode = "IS-00001";
                if (reportCode != '') {
                    var isAllowPrint = true;
                }
                else alert("Erorr : " + errMessage);

                if (isAllowPrint) {
                    var filterExpression = "ItemID = '" + selectedItemID + "' AND ImagingID='" + imagingID + "'";
                    openReportViewer(reportCode, filterExpression);
                }
                else {
                    showToast('Warning', errMessage.text);
                }
            }
        });

        $('.imgPrintEng').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                selectedItemID = $(this).closest('tr').find('.keyField').html();
                var imagingID = $('#<%=hdnID.ClientID %>').val();
                var filter = selectedItemID + "|" + imagingID;
                var errMessage = { text: "" };
                var reportCode = "IS-00002";
                if (reportCode != '') {
                    var isAllowPrint = true;
                }
                else alert("Erorr : " + errMessage);

                if (isAllowPrint) {
                    var filterExpression = "ItemID = '" + selectedItemID + "' AND ImagingID='" + imagingID + "'";
                    openReportViewer(reportCode, filterExpression);
                }
                else {
                    showToast('Warning', errMessage.text);
                }
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
                var imagingID = s.cpRetval;
                $('#<%=hdnID.ClientID %>').val(imagingID);
                var paramResultTest = selectedItemID + "|" + imagingID;
                var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/ImagingTestResultDetailCtl.ascx");
                openUserControlPopup(url, paramResultTest, 'Imaging Test Result Detail', 800, 600);
            }
            hideLoadingPanel();
        }

        function onLoad() {
            setDatePicker('<%=txtResultDate.ClientID %>');

            $('#<%=btnClinicTransactionBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/ImagingTestResult.aspx");
            });

            $('#<%=btnLaboratoryResultDecline.ClientID %>').click(function () {
                cbpView.PerformCallback('decline');
            });
        }
     
        $(function () {
            setDatePicker('<%=txtResultDate.ClientID %>');
            $('#<%=txtResultDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtResultDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

        });
    </script> 
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" /> 
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Hasil Pemeriksaan Pasien")%></div>
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
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Order Number")%></td>
                            <td><asp:TextBox ID="txtOrderNo" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Order Date")%> / <%=GetLabel("Time") %></td>
                            <td><input type="hidden" runat="server" id="hdnTestOrderID" />
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px;width:140px"><asp:TextBox ID="txtOrderDate" Width="120px" runat="server" ReadOnly="true" Style="text-align:center"/></td>
                                    <td style="width:5px">&nbsp;</td>
                                    <td><asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" ReadOnly="true"/></td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Ordered By")%></td>
                            <td><asp:TextBox ID="txtOrderBy" ReadOnly="true" Width="200px" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>            
            <tr>
               <td colspan="2">
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <input type="hidden" id="hdnIsVerified" runat="server" value="" />
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback"> 
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderText="Verified By">
                                                <ItemTemplate>
                                                    <div><%# Eval("IsVerified").ToString() == "True" ?  Eval("VerifiedUserName") : "Unverified"%></div>
                                                    <div><%# Eval("IsVerified").ToString() == "True" ? Eval("CustomVerifiedDate") : " "%></div>
                                                    <input type="hidden" class="hdnIsVerified" value='<%# Eval("IsVerified")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" HeaderStyle-Width="300px" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="300px" />
                                            <asp:HyperLinkField HeaderText="Hasil Pemeriksaan" Text="Hasil Pemeriksaan" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkHasil" HeaderStyle-Width="120px" />  
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderText="Print">
                                                <ItemTemplate>
                                                     <img class="imgPrintIndo imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/printstrook.png")%>' alt="" title="Print in Indonesian" />
                                                     <img class="imgPrintEng imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/printstrook.png")%>' alt="" title="Print in English" />
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