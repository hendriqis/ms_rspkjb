<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="NursingJournalEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingJournalEntry" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnResultBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />   
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />        
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultChargeClassID" runat="server" />    
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtJournalDate.ClientID %>');
            $('#<%=txtJournalDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            //#region Paramedic
            function OnGetParamedicFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblParamedic.lblLink').click(function () {
                openSearchDialog('paramedic', OnGetParamedicFilterExpression(), function (value) {
                    $('#<%=txtParamedicCode.ClientID %>').val(value);
                    ontxtParamedicCodeChanged(value);
                });
            });

            $('#<%=txtParamedicCode.ClientID %>').change(function () {
                ontxtParamedicCodeChanged($(this).val());
            });

            function ontxtParamedicCodeChanged(value) {
                var filterExpression = OnGetParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicID.ClientID%>').val(result.ParamedicID);
                        $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnParamedicID.ClientID%>').val('');
                        $('#<%=txtParamedicCode.ClientID%>').val('');
                        $('#<%=txtParamedicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=btnResultBack.ClientID %>').click(function () {
                document.location = document.referrer;
            });

            $('#btnSave').click(function () {
                if (IsValid(null, 'fsTrx', 'mpTrx'))
                    cbpProcess.PerformCallback('save');
            });
        }

        $('#lblAddData').live('click',function () {
            $('#<%=hdnEntryID.ClientID%>').val('');
            $('#<%=txtJournalDate.ClientID%>').val(getDateNowDatePickerFormat());
            $('#<%=txtJournalTime.ClientID%>').val(getTimeNow());
            $('#<%=txtRemarks.ClientID %>').val('');
            $('#containerEntry').show();
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);

            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=txtJournalDate.ClientID%>').val(entity.JournalDate);
            $('#<%=txtJournalTime.ClientID%>').val(entity.JournalTime);
            $('#<%=hdnParamedicID.ClientID%>').val(entity.ParamedicID);
            $('#<%=txtParamedicCode.ClientID%>').val(entity.ParamedicCode);
            $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);
            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            $('#containerEntry').show();
        });

        function onCbpProcessEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var regisNo = $('#<%=hdnRegistrationNo.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (regisNo == visitID) visitID = 0;
            filterExpression.text = "VisitID = " + visitID + " AND RegistrationNo = '" + regisNo + "'";
            return true;
            
        }
    </script>    
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />  
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />  
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%= OnGetMenuCaption()%></div>
        </div>
        <table class="tblContentArea">
            <tr>
                <td>
                    <div id="containerEntry" style="margin-top:4px;display:none;">
                        <div class="pageTitle"><%=GetLabel("Entry Jurnal Keperawatan")%></div>
                        <fieldset id="fsTrx" style="margin:0"> 
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table class="tblEntryDetail" width="50%">
                                <colgroup>
                                    <col width="50px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><%=GetLabel("Tanggal") %> / <%=GetLabel("Jam Jurnal") %></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-right: 1px"><asp:TextBox ID="txtJournalDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                                <td style="width:5px">&nbsp;</td>
                                                <td><asp:TextBox ID="txtJournalTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td><label class="lblLink lblMandatory" id="lblParamedic"><%=GetLabel("Dokter") %></label></td>
                                    <td>
                                       <table cellpadding="0" cellspacing="0" width="50%">
                                            <colgroup>
                                                <col style="width:10%"/>
                                                <col style="width:3px"/>
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <input type="hidden" value="" id="hdnParamedicID" runat="server"/>
                                                    <asp:TextBox runat="server" id="txtParamedicCode" />
                                                </td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" id="txtParamedicName" Width="400px" readonly="true" /></td>
                                            </tr>
                                       </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top"><label class="lblMandatory"><%=GetLabel("Catatan") %></td>
                                    <td><asp:TextBox runat="server" Width="600px" Rows="4" id="txtRemarks" TextMode="MultiLine" Wrap="true" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                     </div>     
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                                                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0" runat="server" ID="tblEditDelete">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>' 
                                                                    src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" />
                                                            </td>
                                                            <td style="width:1px">&nbsp;</td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>' 
                                                                    src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <input type="hidden" value="<%#:Eval("JournalDateInStringDatePickerFormat") %>" bindingfield="JournalDate" />
                                                    <input type="hidden" value="<%#:Eval("JournalTime") %>" bindingfield="JournalTime" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                        <div style="width:100%;text-align:center">
                                            <span class="lblLink" id="lblAddData" style="<%=IsEditable.ToString() == "False" ? "display:none" : "" %>"><%= GetLabel("Add Data")%></span>
                                        </div>
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
     <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
            EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>