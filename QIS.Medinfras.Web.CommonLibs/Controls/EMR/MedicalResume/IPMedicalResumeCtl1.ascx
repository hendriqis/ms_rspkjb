<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPMedicalResumeCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.IPMedicalResumeCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_IPMedicalResumeCtl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });
        $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

        $('#leftPanel ul li').first().click();
    });

    $(function () {
        setPaging($("#diagnosisPaging"), parseInt('<%=gridDiagnosisPageCount %>'), function (page) {
            cbpDiagnosisView.PerformCallback('changepage|' + page);
        });

        setPaging($("#vitalSignPaging"), parseInt('<%=gridVitalSignPageCount %>'), function (page) {
            cbpVitalSignView.PerformCallback('changepage|' + page);
        });

    });

    function onCbpDiagnosisViewEndCallback(s) {
        var param = s.cpResult.split('|');
        var isMainDiagnosisExists = s.cpRetval;

        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            setPaging($("#diagnosisPaging"), pageCount, function (page) {
                cbpDiagnosisView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
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
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("RESUME MEDIS")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentID="divPage2" title="Ringkasan Riwayat Penyakit" class="w3-hover-red">Ringkasan Riwayat Penyakit</li>
                        <li contentID="divPage3" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>
                        <li contentID="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>
                        <li contentID="divPage5" title="Pemeriksaan Penunjang" class="w3-hover-red">Pemeriksaan Penunjang</li>
                        <li contentID="divPage6" title="Diagnosa" class="w3-hover-red">Diagnosa</li>
                        <li contentID="divPage7" title="Ringkasan Terapi Pengobatan" class="w3-hover-red">Ringkasan Terapi Pengobatan</li>
                        <li contentID="divPage8" title="Perkembangan selama Perawatan" class="w3-hover-red">Perkembangan selama Perawatan</li>
                        <li contentID="divPage9" title="Prosedur Terapi dan Tindakan" class="w3-hover-red">Prosedur Terapi dan Tindakan</li>
                        <li contentID="divPage10" title="Kondisi dan Cara Pulang" class="w3-hover-red">Kondisi dan Cara Pulang</li>
                        <li contentID="divPage11" title="Instruksi dan Rencana Tindak Lanjut" class="w3-hover-red">Instruksi dan Rencana Tindak Lanjut</li>
                    </ul>     
                </div> 
                <div>
                    <table class="w3-table-all" style="width:100%">
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class="w3-small"><%=GetLabel("Tanggal Resume Medis:")%></div></td>
                        </tr>
                        <tr>
                            <td style="text-align:left"><div id="lblMedicalResumeDateTime" runat="server" class="w3-medium"></div></td>
                        </tr>   
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class=" w3-small"><%=GetLabel("Dibuat Oleh :")%></div></td>
                        </tr>        
                        <tr>
                            <td style="text-align:left"><div id="lblResumeParamedicName" runat="server" class="w3-medium"></div></td>
                        </tr>
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class="w3-small"><%=GetLabel("Tanggal Revisi:")%></div></td>
                        </tr>
                        <tr>
                            <td style="text-align:left"><div id="lblRevisionDateTime" runat="server" class="w3-medium"></div></td>
                        </tr>                                                         
                    </table>
                </div>
            </td>
            <td style="vertical-align:top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display:none"> 
                    <table style="margin-top:5px; width:100%" cellpadding="0" cellspacing="0">
                        <colgroup style="width:130px"/>
                            <colgroup style="width:10px; text-align: center"/>
                            <colgroup />
                        <colgroup style="width:130px"/>
                        <tr>
                            <td style="vertical-align:top">
                                <img style="width:110px;height:125px" runat="server" runat="server" id="imgPatientImage" />
                            </td>
                            <td />
                            <td>
                                <table border = "0" cellpadding="0" cellspacing="0">
                                    <colgroup style="width:160px"/>
                                    <colgroup style="width:10px; text-align: center"/>
                                    <colgroup />   
                                    <tr>
                                        <td colspan="3" style="width:100%"><span id="lblPatientName" runat="server" class="w3-sand w3-large" style="font-weight: bold; width:100%"></span></td>
                                    </tr> 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Jenis Kelamin")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblGender" runat="server" style="color:Black"></span></td>
                                    </tr>                                 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Tanggal Lahir (Umur)")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblDateOfBirth" runat="server" style="color:Black"></span></td>
                                    </tr>       
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Tanggal & Jam Registrasi")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><div id="lblRegistrationDateTime" runat="server" style="color:Black"></div></td>
                                    </tr>      
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("No. Registrasi")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><div id="lblRegistrationNo" runat="server" style="color:Black"></div></td>
                                    </tr> 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("DPJP Utama")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPhysician" runat="server" style="color:Black"></span></td>
                                    </tr>                  
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Pembayar")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPayerInformation" runat="server" style="color:Black"></span></td>
                                    </tr>     
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Lokasi Pasien")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPatientLocation" runat="server" style="color:Black"></span></td>
                                    </tr>          
                                    <tr>
                                        <td class="tdLabel" style="vertical-align:top"><%=GetLabel("Diagnosa")%></td>
                                        <td class="tdLabel" style="vertical-align:top"><%=GetLabel(":")%></td>
                                        <td>
                                            <textarea id="lblDiagnosis" runat="server" style="border:0; width:100%; height:120px; background-color: transparent" readonly></textarea>
                                        </td>
                                    </tr>                                                                                                                                                                                                                                                          
                                </table>
                            </td>
                            <td style="vertical-align:top">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display:none">       
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 220px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal dan Waktu")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtResumeDate" Width="120px" runat="server" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:TextBox ID="txtResumeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal" id="lblIndication">
                                    <%=GetLabel("Indikasi Rawat Inap")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHospitalIndication" runat="server" TextMode="MultiLine" Rows="2" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 150px; vertical-align: top">
                                <label class="lblNormal" id="lblHPI">
                                    <%=GetLabel("Anamnesa/Riwayat Penyakit")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubjectiveResumeText" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="8" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Komorbiditas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtComorbiditiesText" runat="server" TextMode="MultiLine" Rows="2" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>         
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup> 
                            <col style="width:150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                                        ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                            EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent4" runat="server">
                                                <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdROSView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                        OnRowDataBound="grdROSView_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <h3><%=GetLabel("Pemeriksaan Fisik")%></h3> 
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <b>
                                                                            <%#: Eval("ObservationDateInString")%>,
                                                                            <%#: Eval("ObservationTime") %>,
                                                                            <%#: Eval("ParamedicName") %>
                                                                        </b>
                                                                    </div>
                                                                    <div>
                                                                        <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                                            <ItemTemplate>
                                                                                <div style="padding-left: 20px; float: left; width: 300px;">
                                                                                    <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                        <strong>
                                                                                            <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                                                            : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                                <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
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
                                                            <%=GetLabel("Tidak ada data pemeriksaan fisik untuk pasien ini") %>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="rosPaging">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label id="lblObjectiveResumeText">
                                    <%=GetLabel("Catatan Tambahan Pemeriksaan Fisik") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtObjectiveResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="5" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>    
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display:none">
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
                <div id="divPage5" class="divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Catatan Pemeriksaan Penunjang") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlanningResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="15" ReadOnly="True" />
                            </td>
                        </tr>
                    </table>
                </div>  
                <div id="divPage6" class="w3-border divContent w3-animate-left" style="display:none">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpDiagnosisView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosisView"
                            ShowLoadingPanel="false" OnCallback="cbpDiagnosisView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent6" runat="server">
                                    <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                        <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage" 
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                                    ItemStyle-CssClass="keyField" >
                                                    <HeaderStyle CssClass="keyField"></HeaderStyle>
                                                    <ItemStyle CssClass="keyField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                    <ItemTemplate>
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                        <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                        <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                        <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="hiddenColumn"></HeaderStyle>
                                                    <ItemStyle CssClass="hiddenColumn"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                            <%#: Eval("DifferentialDateInString")%>,
                                                            <%#: Eval("DifferentialTime")%>,                                                                           
                                                            <%#: Eval("ParamedicName")%></div>
                                                        <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                            <span Style="color: Blue; font-size: 1.1em">
                                                                <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                        </div>
                                                        <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                            <%#: Eval("ICDBlockName")%></div>
                                                        <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                            <b>
                                                                <%#: Eval("DiagnoseType")%></b> -
                                                            <%#: Eval("DifferentialStatus")%></div>
                                                        <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                            <%#: Eval("Remarks")%></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                    <ItemTemplate>
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                        <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                        <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                        <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                        <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                        <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                        <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                        <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                        <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataRowStyle CssClass="trEmpty">
                                            </EmptyDataRowStyle>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Belum ada informasi diagnosa untuk pasien ini") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="diagnosisPaging">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage7" class="divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label id="lblEpisodeMedication">
                                    <%=GetLabel("Ringkasan Terapi Pengobatan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="15" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label id="lblDischargePrescription">
                                    <%=GetLabel("Terapi Obat Pulang") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDischargeMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>    
                <div id="divPage8" class="divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label id="lblProgressNote">
                                    <%=GetLabel("Perkembangan Selama Perawatan") %>
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="15" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage9" class="divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Prosedur Terapi dan Tindakan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSurgeryResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="18" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage10" class="divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="190px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Kondisi Klinis saat Pulang") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDischargeMedicalSummary" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Keadaan Keluar")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientOutcome" runat="server" Width="98%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cara Keluar")%></label>
                            </td>
                                <asp:TextBox ID="txtDischargeRoutine" runat="server" Width="98%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trDeathInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal dan Jam Meninggal")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDateOfDeath" CssClass="datepicker" Width="120px" ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTimeOfDeath" CssClass="time" Width="80px" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trReferrerGroup" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Rujuk Ke")%></label>
                            </td>
                            <td colspan="2">
                                 <asp:TextBox runat="server" ID="txtReferrerGroup" CssClass="time" Width="80px" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trReferrer" style="display: none">
                            <td class="tdLabel">
                                <label class="lblLink" id="lbReferrerCode" runat="server">
                                    <%:GetLabel("Rumah Sakit / Faskes")%></label>
                            </td>
                            <td colspan="5">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                            <asp:TextBox ID="txtReferrerCode" Width="150px" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferrerName" Width="330px" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trDischargeReason" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Alasan Pasien Dirujuk")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDischargeReason" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trDischargeOtherReason" style="display: none">
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDischargeOtherReason" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage11" class="divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Surat Keterangan Sakit ")%></label></td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblIsHasSickLetter" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                                <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                                <asp:ListItem Text=" Ya" Value="1"  />
                                            </asp:RadioButtonList>
                                        </td>
                                        <td><asp:TextBox ID="txtNoOfDays" Width="60px" runat="server" CssClass="number" Text="0" ReadOnly="true" /></td>
                                        <td class="tdLabel"><label><%=GetLabel("hari ")%></label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Rencana Kontrol Kembali ")%></label></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPlanFollowUpVisitDate" Width="120px" ReadOnly="true" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="lblPhysicianInstruction">
                                    <%=GetLabel("Instruksi dan Rencana Tindak Lanjut") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstructionResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="10" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>         