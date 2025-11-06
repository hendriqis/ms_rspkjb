<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="NursingTransactionInfoLink.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingTransactionInfoLink" %>

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
    <input type="hidden" value="" id="hdnVisitID" runat="server" />   
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
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        var GCAssessment = '<%= GetGCNursingEvaluationAssessment() %>';
        var GCPlanning = '<%= GetGCNursingEvaluationPlanning() %>';

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
                resetTransactionInfo();
                loadNursingTransactionInformation();
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
                cbpHeader.PerformCallback('refresh');
                resetTransactionInfo();
                loadNursingTransactionInformation();
            }
            //#endregion

            //#region Transaction No
            function onGetTransactionNoFilterExpression() {
                var filterExpression = "LinkField LIKE '" + $('#<%=txtRegistrationNo.ClientID%>').val() +"%'";
                return filterExpression;
            }

            $('#lblTransactionNo.lblLink').click(function () {
                openSearchDialog('nursingTransaction', onGetTransactionNoFilterExpression(), function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtTransactionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').change(function () {
                onTxtTransactionNoChanged($(this).val());
            });

            function onTxtTransactionNoChanged(value) {
                var filterExpression = onGetTransactionNoFilterExpression() + " AND TransactionNo = '" + value + "'";
                Methods.getObject('GetvNursingTransactionHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnTransactionID.ClientID%>').val(result.TransactionID);
                        $('#<%=txtTransactionDate.ClientID%>').val(result.TransactionDateInDatePickerFormatString);
                        $('#<%=txtTransactionTime.ClientID%>').val(result.TransactionTime);
                        $('#<%=hdnDiagnoseID.ClientID%>').val(result.NursingDiagnoseID);
                        $('#<%=txtDiagnoseName.ClientID%>').val(result.NurseDiagnoseName);
                        $('#<%=txtDiagnoseText.ClientID%>').val(result.DiagnoseText);
                    }
                    else {
                        resetTransactionInfo();
                    }
                });
                loadNursingTransactionInformation();
            }
            //#endregion

            $('#ulTabWorkList li').click(function () {
                $('#ulTabWorkList li.selected').removeAttr('class');
                $('.containerTransaction').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('#<%=grdView.ClientID %> tr:gt(0)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnNursingItemGroupID.ClientID %>').val($(this).find('.keyField').html());
                cbpView1.PerformCallback('refresh');

            });

            $('#<%=grdViewOutcome.ClientID %> tr:gt(0)').live('click', function () {
                $('#<%=grdViewOutcome.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnNursingDiagnoseItemID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewOutcome1.PerformCallback('refresh');
            });

            $('#<%=grdViewIntervention.ClientID %> tr:gt(0)').live('click', function () {
                $('#<%=grdViewIntervention.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnInterventionID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewIntervention1.PerformCallback('refresh');
            });

            $('#<%=grdViewImplementation.ClientID %> tr:gt(0)').live('click', function () {
                $('#<%=grdViewImplementation.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnInterventionDtID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewIntervention1.PerformCallback('refresh');
            });

            
            $('#<%=grdViewEvaluation.ClientID %> tr:gt(0)').live('click', function () {
                $('#<%=grdViewEvaluation.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnGCEvaluationType.ClientID %>').val($(this).find('.keyField').html());
                cbpViewEvaluation1.PerformCallback('refresh');


                
            });
        }

        function resetTransactionInfo() {
            $('#<%=hdnTransactionID.ClientID%>').val('0');
            $('#<%=hdnNursingItemGroupID.ClientID%>').val('0');
            $('#<%=hdnNursingDiagnoseItemID.ClientID%>').val('0');
            $('#<%=hdnInterventionID.ClientID%>').val('0');
            $('#<%=txtTransactionNo.ClientID%>').val('');
            $('#<%=txtTransactionDate.ClientID%>').val('');
            $('#<%=txtTransactionTime.ClientID%>').val('');
            $('#<%=hdnDiagnoseID.ClientID%>').val('0');
            $('#<%=txtDiagnoseName.ClientID%>').val('');
            $('#<%=txtDiagnoseText.ClientID%>').val('');
        }

        //#region LoadInformation
        function loadNursingTransactionInformation() {
            cbpView.PerformCallback('refresh');
        }
        function onCbpViewEndCallback(s) {
            cbpView1.PerformCallback('refresh');
        }
        function onCbpView1EndCallback(s) {
            cbpViewOutcome.PerformCallback('refresh');
        }
        function onCbpViewOutcomeEndCallback(s) {
            cbpViewOutcome1.PerformCallback('refresh');
        }
        function onCbpViewOutcome1EndCallback(s) {
            cbpViewIntervention.PerformCallback('refresh');
        }
        function onCbpViewInterventionEndCallback(s) {
            cbpViewIntervention1.PerformCallback('refresh');
        }
        function onCbpViewIntervention1EndCallback(s) {
            cbpViewImplementation.PerformCallback('refresh');
        }
        function onCbpViewImplementationEndCallback(s) {
            cbpViewImplementation1.PerformCallback('refresh');
        }
        function onCbpViewImplementation1EndCallback(s) {
            cbpViewEvaluation.PerformCallback('refresh');
        }
        function onCbpViewEvaluationEndCallback(s) {
            cbpViewEvaluation1.PerformCallback('refresh');
        }
        function onCbpViewEvaluation1EndCallback(s) {
            if ($('#<%=hdnGCEvaluationType.ClientID %>').val().trim() == GCAssessment.trim()) {
                $('.divSubjectiveObjective').hide();
                $('.divAssessment').show();
                $('.divPlanning').hide();
            }
            else if ($('#<%=hdnGCEvaluationType.ClientID %>').val().trim() == GCPlanning.trim()) {
                $('.divSubjectiveObjective').hide();
                $('.divAssessment').hide();
                $('.divPlanning').show();
            }
            else {
                $('.divSubjectiveObjective').show();
                $('.divAssessment').hide();
                $('.divPlanning').hide();
            }
            hideLoadingPanel();
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Please Set Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "TransactionID = " + transactionID;
                return true;
            }
        }

        
    </script>    
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />  
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />  
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Informasi Jurnal Keperawatan")%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col width="15%" />
                <col />
                <col width="5%" />
                <col width="15%" />
                <col />    
            </colgroup>
            <tr>
                <td><label class="lblLink" id="lblMedicalNo"><%=GetLabel("No Rekam Medis") %></label></td>
                <td>
                    <input type="hidden" value="" id="hdnMedicalNo" runat="server"/>
                    <asp:TextBox runat="server" id="txtMedicalNo" />
                </td>
                <td></td>
                <td class="tdLabel"><%=GetLabel("Tanggal") %> / <%=GetLabel("Waktu") %> Transaksi</td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                            <td style="width:5px">&nbsp;</td>
                            <td><asp:TextBox ID="txtTransactionTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td><label class="lblLink" id="lblRegistrationNo"><%=GetLabel("No Registrasi") %></label></td>
                <td>
                    <input type="hidden" value="" id="hdnRegistrationNo" runat="server"/>
                    <asp:TextBox runat="server" id="txtRegistrationNo" />
                </td>
                <td></td>
                <td><label class="lblNormal" id="Label1"><%=GetLabel("Diagnosa Medis") %></label></td>
                <td>
                    <asp:TextBox runat="server" ID="txtDiagnoseText" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><label class="lblLink" id="lblTransactionNo"><%=GetLabel("No Transaksi") %></label></td>
                <td>
                    <input type="hidden" value="" id="hdnTransactionID" runat="server"/>
                    <asp:TextBox runat="server" id="txtTransactionNo" />
                </td>
                <td></td>
                <td><label class="lblNormal" id="Label2"><%=GetLabel("NANDA-I") %></label></td>
                <td>
                    <input type="hidden" value="" id="hdnDiagnoseID" runat="server"/>
                    <asp:TextBox runat="server" ID="txtDiagnoseName" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <div class="containerUlTabPage">
                       <ul class="ulTabPage" id="ulTabWorkList">
                            <li class="selected" contentid="containerDiagnoseItem"><%=GetLabel("Diagnosis Keperawatan")%></li>
                            <li contentid="containerOutcome"><%=GetLabel("Nursing Outcome Classification (NOC)")%></li>
                            <li contentid="containerIntervention"><%=GetLabel("Nursing Intervention Classification (NIC)")%></li>
                            <li contentid="containerImplementation"><%=GetLabel("Implementasi")%></li>
                            <li contentid="containerSOAP"><%=GetLabel("Evaluasi")%></li>
                       </ul>
                    </div>

                    <div style="padding:2px;" id="containerDiagnoseItem" class="containerTransaction">
                        <table width="100%">
                            <colgroup>
                                <col width="30%" />
                                <col width="50%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid">
                                                    <input type="hidden" value="" id="hdnNursingItemGroupID" runat="server"/>
                                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="NursingItemGroupSubGroupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="padding-left:3px">
                                                                        <%=GetLabel("Text")%>
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style='margin-left:<%#: Eval("CfMargin") %>0px;'><%#: Eval("NursingItemGroupSubGroupText")%></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Jml" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalSelected"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>    
                                    
                                </td>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpView1" runat="server" Width="100%" ClientInstanceName="cbpView1"
                                        ShowLoadingPanel="false" OnCallback="cbpView1_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpView1EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView1" CssClass="pnlContainerGrid">
                                                    <asp:GridView ID="grdView1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="NursingItemText" HeaderStyle-CssClass="Item Diagnosis" HeaderText="Item Diagnosis" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>  
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="padding:2px;display:none" id="containerOutcome" class="containerTransaction">
                        <table width="100%">
                            <colgroup>
                                <col width="30%" />
                                <col width="70%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewOutcome" runat="server" Width="100%" ClientInstanceName="cbpViewOutcome"
                                        ShowLoadingPanel="false" OnCallback="cbpViewOutcome_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                                        EndCallback="function(s,e){ onCbpViewOutcomeEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent4" runat="server">
                                                <input type="hidden" value="" id="hdnNursingDiagnoseItemID" runat="server"/>
                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                                    <asp:GridView ID="grdViewOutcome" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewOutcome_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="NursingDiagnoseItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="NursingItemText" HeaderText="Item Diagnosis" />
                                                            <asp:TemplateField HeaderText="Jml" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalSelected"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>    
                                </td>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewOutcome1" runat="server" Width="100%" ClientInstanceName="cbpViewOutcome1"
                                        ShowLoadingPanel="false" OnCallback="cbpViewOutcome1_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewOutcome1EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent5" runat="server">
                                                <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGrid">
                                                    <asp:GridView ID="grdViewOutcome1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewOutcome1_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField HeaderText="Item Indikator" DataField="NursingIndicatorText" HeaderStyle-Width="200px" />
                                                            <asp:TemplateField HeaderStyle-Width="400px">
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <colgroup>
                                                                            <col width="80px" />
                                                                            <col width="80px" />
                                                                            <col width="80px" />
                                                                            <col width="80px" />
                                                                            <col width="80px" />
                                                                        </colgroup>
                                                                        <tr valign="top">
                                                                            <td><asp:RadioButton ID="rbtScale1Text" runat="server" Enabled="false" CssClass="rbtScale1Text" GroupName="ScaleText" Text='<%#: "1-" + Eval("Scale1Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale2Text" runat="server" Enabled="false" CssClass="rbtScale2Text" GroupName="ScaleText" Text='<%#: "2-" + Eval("Scale2Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale3Text" runat="server" Enabled="false" CssClass="rbtScale3Text" GroupName="ScaleText" Text='<%#: "3-" + Eval("Scale3Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale4Text" runat="server" Enabled="false" CssClass="rbtScale4Text" GroupName="ScaleText" Text='<%#: "4-" + Eval("Scale4Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale5Text" runat="server" Enabled="false" CssClass="rbtScale5Text" GroupName="ScaleText" Text='<%#: "5-" + Eval("Scale5Text") %>' /></td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Keterangan" DataField="Remarks" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>     
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="padding:2px;display:none" id="containerIntervention" class="containerTransaction">
                        <table width="100%">
                            <colgroup>
                                <col width="30%" />
                                <col width="70%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewIntervention" runat="server" Width="100%" ClientInstanceName="cbpViewIntervention"
                                        ShowLoadingPanel="false"  OnCallback="cbpViewIntervention_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewInterventionEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGrid">
                                                    <input type="hidden" value="" id="hdnInterventionID" runat="server" />
                                                    <asp:GridView ID="grdViewIntervention" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewIntervention_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="NurseInterventionName" HeaderText="Intervensi" />
                                                            <asp:TemplateField HeaderText="Jml" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalSelected"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>    
                                </td>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewIntervention1" runat="server" Width="100%" ClientInstanceName="cbpViewIntervention1"
                                        ShowLoadingPanel="false" OnCallback="cbpViewIntervention1_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewIntervention1EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent7" runat="server">
                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid">
                                                    <asp:GridView ID="grdViewIntervention1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="NursingItemText" HeaderText="Item Intervensi" />
                                                            <asp:BoundField DataField="InterventionImplementation" HeaderText="Implementasi"/>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>     
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="padding:2px;display:none" id="containerImplementation" class="containerTransaction">
                        <table width="100%">
                            <colgroup>
                                <col width="50%" />
                                <col width="50%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewImplementation" runat="server" Width="100%" ClientInstanceName="cbpViewImplementation"
                                        ShowLoadingPanel="false" OnCallback="cbpViewImplementation_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewImplementationEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent9" runat="server">
                                                <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGrid">
                                                    <input type="hidden" value="" id="hdnInterventionDtID" runat="server" />
                                                    <asp:GridView ID="grdViewImplementation" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewImplementation_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="NursingItemText" HeaderText="Item Intervensi" />
                                                            <asp:TemplateField HeaderText="Jml" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalSelected"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>    
                                </td>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewImplementation1" runat="server" Width="100%" ClientInstanceName="cbpViewImplementation1"
                                        ShowLoadingPanel="false" OnCallback="cbpViewImplementation1_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewImplementation1EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent10" runat="server">
                                                <asp:Panel runat="server" ID="Panel8" CssClass="pnlContainerGrid">
                                                    <asp:GridView ID="GridView1" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="CfJournalDateTime" HeaderText="Tanggal Jurnal" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField HeaderText="Catatan">
                                                                <ItemTemplate>
                                                                    <asp:TextBox BorderStyle="None" BackColor="Transparent" Rows="4" runat="server" Width="100%" Enabled="false" ID="txtGridRemarks" TextMode="MultiLine" Wrap="true" Text='<%#: Eval("Remarks") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Informasi Jurnal" HeaderStyle-Width="250px">
                                                                <ItemTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td><asp:Label ID="Label1" runat="server" Text='<%#: "Dibuat oleh: " + Eval("CreatedByName") + " " + Eval("CreatedDateInString")%>' /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><asp:Label ID="Label2" runat="server" Text='<%#: "Diedit oleh: " + Eval("LastUpdatedByName") + " " + Eval("LastUpdatedDateInString")%>' /></td>
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
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="padding:2px;display:none" id="containerSOAP" class="containerTransaction">
                        <table width="100%">
                            <colgroup>
                                <col width="30%" />
                                <col width="70%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewEvaluation" runat="server" Width="100%" ClientInstanceName="cbpViewEvaluation"
                                        ShowLoadingPanel="false" OnCallback="cbpViewEvaluation_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewEvaluationEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent11" runat="server">
                                                <input type="hidden" value="" id="hdnGCEvaluationType" runat="server" />
                                                <asp:Panel runat="server" ID="Panel9" CssClass="pnlContainerGrid">
                                                    <asp:GridView ID="grdViewEvaluation" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="StandardCodeName" HeaderText="Jenis Evaluasi" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Data To Display
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>  
                                </td>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewEvaluation1" runat="server" Width="100%" ClientInstanceName="cbpViewEvaluation1"
                                        ShowLoadingPanel="false" OnCallback="cbpViewEvaluation1_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpViewEvaluation1EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent8" runat="server">
                                                <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGrid">
                                                    <div class="divSubjectiveObjective">
                                                        <asp:GridView ID="grdViewSubjectiveObjective" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" >
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                <asp:BoundField DataField="NursingItemText" HeaderText="Diagnosis Keperawatan" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No Data To Display
                                                            </EmptyDataTemplate>
                                                        </asp:GridView> 
                                                    </div>
                                                    <div class="divAssessment">
                                                        <asp:GridView ID="grdViewAssessment" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" 
                                                            OnRowDataBound="grdViewAssessment_RowDataBound" OnRowCreated="grdViewAssessment_RowCreated">
                                                            <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField HeaderText="Item Indikator" DataField="NursingIndicatorText" ItemStyle-CssClass="indicatorTextEvaluation" />
                                                            <asp:BoundField HeaderText="Target" DataField="ScaleScore" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField HeaderStyle-Width="500px" HeaderText="Evaluasi">
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <colgroup>
                                                                            <col width="100px" />
                                                                            <col width="100px" />
                                                                            <col width="100px" />
                                                                            <col width="100px" />
                                                                            <col width="100px" />
                                                                        </colgroup>
                                                                        <tr valign="top">
                                                                            <td><asp:RadioButton ID="rbtScale1Text" runat="server" Enabled="false" CssClass="rbtScale1Text" GroupName="ScaleText" Text='<%#: "1-" + Eval("Scale1Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale2Text" runat="server" Enabled="false" CssClass="rbtScale2Text" GroupName="ScaleText" Text='<%#: "2-" + Eval("Scale2Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale3Text" runat="server" Enabled="false" CssClass="rbtScale3Text" GroupName="ScaleText" Text='<%#: "3-" + Eval("Scale3Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale4Text" runat="server" Enabled="false" CssClass="rbtScale4Text" GroupName="ScaleText" Text='<%#: "4-" + Eval("Scale4Text") %>' /></td>
                                                                            <td><asp:RadioButton ID="rbtScale5Text" runat="server" Enabled="false" CssClass="rbtScale5Text" GroupName="ScaleText" Text='<%#: "5-" + Eval("Scale5Text") %>' /></td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                            <EmptyDataTemplate>
                                                                No Data To Display
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="divPlanning">
                                                        <asp:GridView ID="grdViewPlanning" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" 
                                                            OnRowDataBound="grdViewPlanning_RowDataBound" OnRowCreated="grdViewPlanning_RowCreated">
                                                            <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField HeaderText="Kode" DataField="DisplayOrder" HeaderStyle-Width="100px" />
                                                            <asp:BoundField HeaderText="Item Indikator" DataField="NursingItemText" ItemStyle-CssClass="interventionItemEvaluation" />
                                                            <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Evaluasi" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <colgroup>
                                                                            <col width="100px" />
                                                                            <col width="100px" />
                                                                        </colgroup>
                                                                        <tr valign="top">
                                                                            <td><asp:RadioButton ID="rbtContinue" runat="server" Enabled="false" CssClass="rbtContinue" GroupName="ContinueIntervention" Text='Lanjutkan' /></td>
                                                                            <td><asp:RadioButton ID="rbtNotContinue" runat="server" Enabled="false" CssClass="rbtNotContinue" GroupName="ContinueIntervention" Text='Hentikan' /></td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                            <EmptyDataTemplate>
                                                                No Data To Display
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>  
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="Div1">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>