<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="NursingJournalInfoLink.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingJournalInfoLink" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />        
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultChargeClassID" runat="server" />    
    <dxcp:ASPxCallbackPanel ID="cbpHeader" runat="server" Width="100%" ClientInstanceName="cbpHeader"
        ShowLoadingPanel="false" OnCallback="cbpHeader_Callback">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server">
                <asp:Panel runat="server" ID="Panel1">
                    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
                    <input type="hidden" value="" id="hdnVisitID" runat="server" />  
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        function onLoad() {
            //#region Medical No
            $('#lblMedicalNo.lblLink').click(function () {
                openSearchDialog('patientLink', "", function (value) {
                    $('#<%=txtMedicalNo.ClientID %>').val(value);
                    ontxtMedicalNoChanged(value);
                });
            });

            $('#<%=txtMedicalNo.ClientID %>').change(function () {
                ontxtMedicalNoChanged($(this).val());
            });

            function ontxtMedicalNoChanged(value) {
                var filterExpression = "MedicalNo = '" + value + "'";
                Methods.getObject('GetvPatientListLinkList', filterExpression, function (result) {
                    if (result == null) {
                        $('#<%=txtMedicalNo.ClientID%>').val('');
                    }
                });
                $('#<%=txtRegistrationNo.ClientID%>').val('');
                cbpView.PerformCallback('refresh');
            }
            //#endregion

            //#region Registration No
            function OnGetRegistrationFilterExpression() {
                var filterExpression = "MedicalNo = '" + $('#<%=txtMedicalNo.ClientID%>').val().trim() +"'";
                return filterExpression;
            }

            $('#lblRegistrationNo.lblLink').click(function () {
                openSearchDialog('registrationLink', OnGetRegistrationFilterExpression(), function (value) {
                    $('#<%=txtRegistrationNo.ClientID %>').val(value);
                    ontxtRegistrationNoChanged(value);
                });
            });

            $('#<%=txtRegistrationNo.ClientID %>').change(function () {
                ontxtRegistrationNoChanged($(this).val());
            });

            function ontxtRegistrationNoChanged(value) {
                var filterExpression = OnGetRegistrationFilterExpression() + " AND RegistrationNo = '" + value + "'";
                Methods.getObject('GetvRegistrationListLinkList', filterExpression, function (result) {
                    if (result == null) {
                        $('#<%=txtRegistrationNo.ClientID%>').val('');
                    }
                });
                cbpView.PerformCallback('refresh');
            }
            //#endregion
        }

        function onCbpViewEndCallback(s) {
            cbpHeader.PerformCallback('refresh');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var regisNo = $('#<%=txtRegistrationNo.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (regisNo == visitID) visitID = 0;
            filterExpression.text = "VisitID = " + visitID + " AND RegistrationNo = '" + regisNo + "'";
            alert(filterExpression.text);
            return true;

        }
    </script>    
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />  
    
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Informasi Jurnal Keperawatan")%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col width="10%" />
                <col />
            </colgroup>
            <tr>
                <td><label class="lblLink lblMandatory" id="lblMedicalNo"><%=GetLabel("No Rekam Medis") %></label></td>
                <td>
                    <input type="hidden" value="" id="hdnMedicalNo" runat="server"/>
                    <asp:TextBox runat="server" id="txtMedicalNo" />
                </td>
            </tr>
            <tr>
                <td><label class="lblLink lblMandatory" id="lblRegistrationNo"><%=GetLabel("No Registrasi") %></label></td>
                <td>
                    <input type="hidden" value="" id="hdnRegistrationNo" runat="server"/>
                    <asp:TextBox runat="server" id="txtRegistrationNo" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                        <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="CfJournalDateTime" HeaderText="Tanggal Jurnal" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ParamedicName" HeaderText="Dokter"  HeaderStyle-Width="200px"/>
                                                <asp:TemplateField HeaderText="Catatan">
                                                    <ItemTemplate>
                                                        <asp:TextBox BorderStyle="None" BackColor="Transparent" Rows="4" runat="server" Width="100%" Enabled="false" ID="txtGridRemarks" TextMode="MultiLine" Wrap="true" Text='<%#: Eval("Remarks") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Informasi Jurnal" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td><asp:Label runat="server" Text='<%#: "Dibuat oleh: " + Eval("CreatedByName") + " " + Eval("CreatedDateInString")%>' /></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:Label ID="Label1" runat="server" Text='<%#: "Diedit oleh: " + Eval("LastUpdatedByName") + " " + Eval("LastUpdatedDateInString")%>' /></td>
                                                            </tr>
                                                        </table>
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
                    </div>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>  
    </div>
</asp:Content>