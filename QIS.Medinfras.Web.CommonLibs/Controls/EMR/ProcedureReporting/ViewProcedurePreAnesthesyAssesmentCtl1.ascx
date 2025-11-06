<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewProcedurePreAnesthesyAssesmentCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ViewProcedurePreAnesthesyAssesmentCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_viewPreOpAssessmentCtl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#<%=grdAllergyView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdAllergyView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });
        $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

        //#region Form Values
        if ($('#<%=hdnPhysicalExamValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnPhysicalExamValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        if ($('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent2.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        if ($('#<%=hdnAnesthesyPlanningValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnAnesthesyPlanningValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent3.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent3.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        //#endregion

        registerCollapseExpandHandler();

        $('#leftPanel ul li').first().click();
    });

    $(function () {
        setPaging($("#allergyPaging"), parseInt('<%=gridAllergyPageCount %>'), function (page) {
            cbpDiagnosisView.PerformCallback('changepage|' + page);
        });

        setPaging($("#vitalSignPaging"), parseInt('<%=gridVitalSignPageCount %>'), function (page) {
            cbpVitalSignView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpAllergyViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

            setPaging($("#allergyPaging"), pageCount, function (page) {
                cbpAllergyView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
    }

    function onCbpVitalSignViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
    }

    function onCboAnesthesiaTypeValueChanged() {
        if (cboAnesthesiaType.GetValue() == "X277^02") {
            cboRegionalAnesthesiaType.SetEnabled(true);
        }
        else {
            cboRegionalAnesthesiaType.SetEnabled(false);
        }
        cboRegionalAnesthesiaType.SetValue("");
    }

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
</script>
<style type="text/css">
    #leftPanel          { border:1px solid #6E6E6E; width:100%;height:100%; position: relative; }    
    #leftPanel > ul       { margin:0; padding:2px; border-bottom:1px groove black; }
    #leftPanel > ul > li    { list-style-type: none; font-size: 15px; display:list-item; border: 1px solid #fdf5e6!important; padding: 5px 8px; cursor: pointer; background-color:#87CEEB!important; }
    #leftPanel > ul > li.selected { background-color: #ff5722!important; color: White; }   
    .divContent { padding-left: 3px; min-height:490px;} 
</style>
<div style="width:100%;">
    <input type="hidden" runat="server" id="hdnMRN" value="0" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' />
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
    <input type="hidden" id="hdnVisitID" runat="server" value='0' />
    <input type="hidden" id="hdnPatientChargesDtID" runat="server" value='0' />
    <input type="hidden" id="hdnAssessmentID" runat="server" value='0' />
    <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
    <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
    <input type="hidden" runat="server" id="hdnDiagnosticTestCheckListLayout" value="" />
    <input type="hidden" runat="server" id="hdnDiagnosticTestCheckListValue" value="" />
    <input type="hidden" runat="server" id="hdnAnesthesyPlanningLayout" value="" />
    <input type="hidden" runat="server" id="hdnAnesthesyPlanningValue" value="" />
    <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
        <colgroup>
            <col style="width:300px" />
            <col />
        </colgroup>
        <tr>
            <td>
               <div id="lblMedicalNo" runat="server" class="w3-lime w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444; width:100%"></div>
            </td>
            <td>
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("ASESMEN PRA ANESTESI")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Catatan Asesmen dan Riwayat Kesehatan" class="w3-hover-red">Rencana Tindakan dan Riwayat Kesehatan</li>
                        <li contentID="divPage2" title="Riwayat Alergi" class="w3-hover-red">Riwayat Alergi</li>
                        <li contentID="divPage3" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>                                               
                        <li contentID="divPage4" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>      
                        <li contentID="divPage5" title="Hasil Pemeriksaan Penunjang yang telah teridentifikasi secara benar" class="w3-hover-red">Hasil Pemeriksaan Penunjang</li>      
                        <li contentID="divPage6" title="Rencana Anestesi" class="w3-hover-red">Rencana Anestesi</li>
                    </ul>     
                </div> 
                <div>
                    <table class="w3-table-all" style="width:100%">
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class=" w3-small"><%=GetLabel("Dikaji Oleh :")%></div></td>
                        </tr>        
                        <tr>
                            <td style="text-align:left"><div id="lblPhysicianName2" runat="server" class="w3-medium"></div></td>
                        </tr>                                                         
                    </table>
                </div>
            </td>
            <td style="vertical-align:top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display:none"> 
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal dan Waktu")%></label>
                            </td>
                            <td colspan="3">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceDate" Width="120px" runat="server" Style="text-align: center" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblOrderNo">
                                    <%:GetLabel("Nomor Transaksi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="Hidden1" value="" runat="server" />
                                <asp:TextBox ID="txtTransactionNo" Width="225px" runat="server" Enabled="false" />
                            </td>
                            <td class="tdLabel" style="display:none">
                                <label id="Label1">
                                    <%:GetLabel("Diagnosa Pre Op")%></label>
                            </td>
                            <td colspan="3" style="display:none">
                                <asp:TextBox ID="txtPreOpDiagnosisInfo" Width="100%" runat="server" Enabled="false" />
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel">
                                <label id="lblItemName">
                                    <%:GetLabel("Tindakan")%>
                                </label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtItemName" Width="100%" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Rencana Tindakan") %>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtProcedureGroupSummary" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>                                 
                        <tr>
                            <td class="tdLabel" valign="top">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td class="tdLabel" valign="top" style="width:120px">
                                            <label class="lblNormal" id="lblAnesthesyRemarks">
                                                <%=GetLabel("Catatan Anestesi")%></label>
                                        </td>
                                        <td style="display:none"><img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                    </tr>
                                </table> 
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAnamnesisText" runat="server" TextMode="MultiLine" Rows="2" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Riwayat Pembedahan dan Anestesi") %>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPastSurgicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Riwayat Penggunaan Obat") %>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtMedicationHistory" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="3" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 50px">
                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Auto" Checked="false" Enabled="false" />
                                        </td>
                                        <td style="width: 50px">
                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Allo"
                                                Checked="false" Enabled="false" />
                                        </td>
                                        <td class="tdLabel" style="width: 120px">
                                            <label class="lblNormal" id="lblFamilyRelation">
                                                <%=GetLabel("Hubungan dengan Pasien")%></label>
                                        </td>
                                        <td style="width: 130px">
                                            <asp:TextBox ID="txtFamilyRelation" runat="server" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>   
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <td style="width: 50%">
                                        <asp:CheckBox ID="chkIsAsthma" runat="server" Text=" Memiliki Riwayat Asma" Checked="false" Enabled="false" />
                                    </td>
                                </table>
                            </td>
                        </tr>  
                    </table>
                </div>
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display:none">
                    <div style="max-height:450px;overflow-y:auto; padding : 5px 0px 5px 0px">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
                                        Checked="false" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpAllergyView" runat="server" Width="100%" ClientInstanceName="cbpAllergyView"
                                        ShowLoadingPanel="false" OnCallback="cbpAllergyView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage3">
                                                    <asp:GridView ID="grdAllergyView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                                                ItemStyle-CssClass="keyField" >
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                                                                    <input type="hidden" value="<%#:Eval("KnownDate") %>" bindingfield="KnownDate" />
                                                                    <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="200px"
                                                                HeaderStyle-HorizontalAlign="Left" >
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                                HeaderStyle-HorizontalAlign="Left" >
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" >
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                                HeaderStyle-HorizontalAlign="Left" >
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Reaction" HeaderText="Reaction" 
                                                                HeaderStyle-HorizontalAlign="Left" >
                                                            </asp:BoundField>
                                                        </Columns>        
                                                        <EmptyDataRowStyle CssClass="trEmpty">
                                                        </EmptyDataRowStyle>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data alergi pasien dalam episode ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="allergyPaging">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display:none">
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                            ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage4">
                                        <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdVitalSignView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <h3><%=GetLabel("Tanda Vital dan Indikator Lainnya")%></h3> 
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ObservationDateInString")%>,
                                                                <%#: Eval("ObservationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                            <br />
                                                            <span style="font-style:italic">
                                                                <%#: Eval("Remarks") %>
                                                            </span>
                                                            <br />
                                                        </div>
                                                        <div>
                                                            <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px; float: left; width: 350px;">
                                                                        <strong>
                                                                            <div style="width: 110px; float: left;" class="labelColumn">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                            <div style="width: 20px; float: left;">
                                                                                :</div>
                                                                        </strong>
                                                                        <div style="float: left;">
                                                                            <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <br style="clear: both" />
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada pemeriksaan tanda vital") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="vitalSignPaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage4" class="divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="450px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top" colspan="2">
                                <div id="divFormContent1" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage5" class="divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="220px" />
                            <col />
                        </colgroup>
                        <tr style="display:none">
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal" id="lblDiagnosticResultSummary">
                                    <%=GetLabel("Catatan Hasil Penunjang")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtDiagnosticResultSummary" runat="server" TextMode="MultiLine" Rows="12" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align:top">
                                <div id="divFormContent2" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage6" class="divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea" style="width:100%">
                        <colgroup>
                            <col width="200px" />
                            <col width="150px" />
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Mulai Puasa")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartFastingDate" Width="120px" runat="server" ReadOnly="true" />
                            </td>
                            <td colspan="2" style="padding-left: 5px">
                                <asp:TextBox ID="txtStartFastingTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Status Fisik ASA") %></td>
                            <td colspan="3">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                                <asp:RadioButtonList ID="rblGCASAStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled="false">
                                                <asp:ListItem Text=" 1" Value="X455^1" />
                                                <asp:ListItem Text=" 2" Value="X455^2" />
                                                <asp:ListItem Text=" 3" Value="X455^3" />
                                                <asp:ListItem Text=" 4" Value="X455^4" />
                                                <asp:ListItem Text=" 5" Value="X455^5" />
                                            </asp:RadioButtonList>                                               
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsASAStatusE" Checked="false" Text = " E" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>    
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Teknik Anestesi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnesthesiaType" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                            <td class="tdLabel" style="width: 120px">
                                <label class="lblNormal">
                                    <%=GetLabel("Regional Anestesi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegionalAnesthesiaType" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Premedikasi")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPremedication" Width="99%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="vertical-align:top">
                                <div id="divFormContent3" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>         