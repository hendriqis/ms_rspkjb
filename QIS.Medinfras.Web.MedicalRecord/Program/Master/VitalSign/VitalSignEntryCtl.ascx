<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VitalSignEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.VitalSignEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>


<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {


    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

     $('.imgDelete.imgLink').die('click');
     $('.imgDelete.imgLink').live('click', function () {
         if (confirm("Are You Sure Want To Delete This Data?")) {
             $row = $(this).closest('tr');
             var mainID = $row.find('.hdnMainID').val();
             $('#<%=hdnMainID.ClientID %>').val(mainID);
 
             cbpEntryPopupView.PerformCallback('delete');
        }
    });

     $('.imgEdit.imgLink').die('click');
     $('.imgEdit.imgLink').live('click', function () {
         $('#lblItemParameter').attr('class', 'lblDisabled');
         $('#<%=txtItemParameterCode.ClientID %>').attr('readonly', 'readonly');



</script>

<div style="height:440px; overflow:auto">
    <input type="hidden" id="hdnTopID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Healthcare")%></div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:100%">
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:70%">
                        <colgroup>
                            <col style="width:160px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Customer")%></label></td>
                            <td colspan="2"><asp:TextBox ID="txtHeaderText" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                    </table>

                    <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                        <input type="hidden" id="hdnIsAdd" runat="server" value="" />
                        <input type="hidden" id="hdnMainID" runat="server" value="" />
                        <div class="pageTitle"><%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopup" style="margin:0">
                            <table class="tblEntryDetail" style="width:100%">
                                <colgroup>
                                    <col style="width:150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboSex" Width="400px" ClientInstanceName="cboDepartment" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("From Age")%></label></td>
                                    <td><asp:TextBox ID="txtFromAge" runat="server" Width="100px" CssClass="required" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("To Age")%></label></td>
                                    <td><asp:TextBox ID="txtToAge" runat="server" Width="100px" CssClass="required" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboAgeUnit" Width="400px" ClientInstanceName="cboAgeUnit" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Min Value")%></label></td>
                                    <td><asp:TextBox ID="txtMinUnit" runat="server" Width="100px" CssClass="required" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Max value")%></label></td>
                                    <td><asp:TextBox ID="txtMaxUnit" runat="server" Width="100px" CssClass="required" /></td>
                                </tr>    
                            </table>
                        </fieldset>
                    </div>

                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView" ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID"pnlVitalSignGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField Header-Style-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                    
                                                    <input type="hidden" class="hdnMainID" value="<%# Eval("ID")%>" />
                                                    <input type="hidden" class="hdnGCAgeUnit" value="<%# Eval("GCAgeUnit")%>" />                                                                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Sex" HeaderText="HealthcareName" ItemStyle-CssClass="tdItemName" />
                                            <asp:BoundField DataField="FromAge" HeaderText="From Age" ItemStyle-CssClass="tdItemName" />
                                            <asp:BoundField DataField="ToAge" HeaderText="To Age" ItemStyle-CssClass="tdItemName" />
                                            <asp:BoundField DataField="AgeUnit" HeaderText="Age Unit" ItemStyle-CssClass="tdItemName" />
                                            <asp:BoundField DataField="MinValue" HeaderText="Min Value" ItemStyle-CssClass="tdItemName" />
                                            <asp:BoundField DataField="MaxValue" HeaderText="Max Value" ItemStyle-CssClass="tdItemName" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>

                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                        <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                    </div>

                </td>
            </tr>
        </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>