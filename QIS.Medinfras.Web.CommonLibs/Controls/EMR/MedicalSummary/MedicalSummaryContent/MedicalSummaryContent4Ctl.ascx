<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalSummaryContent4Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalSummaryContent4Ctl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_MedicalSummaryContent4Ctl">
    $(function () {
        setDatePicker('<%=txtFromDate.ClientID %>');
        $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtToDate.ClientID %>');
        $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');

        $('#<%=txtFromDate.ClientID %>').change(function (evt) {
            cbpMedicalSummaryContent4View.PerformCallback('refresh');
        });

        $('#<%=txtToDate.ClientID %>').change(function (evt) {
            cbpMedicalSummaryContent4View.PerformCallback('refresh');
        });
    });

    $('.btnContentRefresh').die('click');
    $('.btnContentRefresh').live('click', function () {
        alert('Button Clicked');
        cbpMedicalSummaryContent4View.PerformCallback('refresh');
    });


    //#region Paging
    var pageCount = parseInt('<%=Content4PageCount %>');
    $(function () {
        setPaging($("#pagingContent4"), pageCount, function (page) {
            cbpMedicalSummaryContent4View.PerformCallback('changepage|' + page);
        });
    });

    function oncbpMedicalSummaryContent4ViewEndCallback(s) {
        $('#containerImgLoadingContent4View').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdMedicalSummaryContent4.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingContent4"), pageCount, function (page) {
                cbpMedicalSummaryContent4View.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdMedicalSummaryContent4.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>

<input type="hidden" id="hdnContentRegistrationID" runat="server" />
<div class="w3-border divContent w3-animate-left">
    <table style="margin-top:10px; width:100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:130px" />
            <col style="width:10px; text-align: center"/>
            <col />
        </colgroup>
        <tr>
            <td colspan="3">
                <div id="filterArea">
                    <table style="margin-top: 10px; margin-bottom: 10px">
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" style="font-weight: bold">
                                    <%=GetLabel("Tanggal ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td style="text-align: center">
                                <%=GetLabel("s/d") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td style="width: 30px">
                            </td>
                            <td class="tdLabel" style="width: 150px">
                                <label>
                                    <%=GetLabel("Display Option")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                                    Width="300px">
                                    <ClientSideEvents ValueChanged="function() { cbpMedicalSummaryContent4View.PerformCallback('refresh'); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <input type="button" class="btnContentRefresh w3-btn w3-hover-blue" value="Refresh" style="background-color: Red;
                                    color: White; width: 100px;" />
                            </td>
                        </tr>
                    </table>
                </div>            
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <dxcp:ASPxCallbackPanel ID="cbpMedicalSummaryContent4View" runat="server" Width="100%" ClientInstanceName="cbpMedicalSummaryContent4View"
                    ShowLoadingPanel="false" OnCallback="cbpMedicalSummaryContent4View_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ alert('Refresh');$('#containerImgLoadingContent4View').show(); }"
                        EndCallback="function(s,e){ oncbpMedicalSummaryContent4ViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdMedicalSummaryContent4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true"
                                    OnRowDataBound="grdMedicalSummaryContent4_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                        <asp:BoundField DataField="DepartmentID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn departmentID" />
                                        <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                        <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicName" />
                                        <asp:BoundField DataField="GCPatientNoteType" HeaderStyle-CssClass="controlColumn"
                                            ItemStyle-CssClass="controlColumn" />
                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                        <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                        <asp:BoundField DataField="NoteTime" HeaderText="Jam " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                        <asp:TemplateField HeaderText="PPA" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("cfPPA") %>
                                                </div>
                                                <div>
                                                    <img class="imgNeedConfirmation" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                        alt="" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                        cursor: pointer; min-width: 30px;' title="Need confirmation" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SOAP" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <span style="color: blue; font-style: italic; vertical-align: top">
                                                        <%#:Eval("ParamedicName") %>
                                                        - <b>
                                                            <%#:Eval("DepartmentID") %>
                                                            (<%#:Eval("ServiceUnitName") %>)
                                                            <%#:Eval("cfParamedicMasterType") %>
                                                            <span style="float: right; <%# Eval("IsEdited").ToString() == "False" ? "display:none": "" %>">
                                                                    <img class="imgLink" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                                                                        alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                                                            </span>
                                                </div>
                                                <div style="height: 130px; overflow-y: auto; margin-top: 15px;">
                                                    <%#Eval("NoteText").ToString().Replace("\n","<br />")%><br />
                                                </div>
                                                <div id="divView" runat="server" style='margin-top: 5px;'>
                                                    <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue" value="Detail Kajian Awal"
                                                        style='width: 150px; background-color: Green; color: White;'  />
                                                </div>
                                                <div style="margin-top: 10px; text-align: left">
                                                    <table border="0" cellpadding="1" cellspacing="0">
                                                       <tr id="divNursingNotesInfo" runat="server" >
                                                            <td>
                                                               <asp:CheckBox ID="chkIsWritten" runat="server" Enabled="false" Checked='<%# Eval("IsWrite")%>' /> TULIS
                                                               <asp:CheckBox ID="chkIsReadback" runat="server" Enabled="false" Checked='<%# Eval("IsReadback")%>'/> BACA
                                                               <asp:CheckBox ID="chkIsConfirmed" runat="server" Enabled="false" Checked='<%# Eval("IsConfirmed")%>'/> KONFIRMASI
                                                            </td>
                                                        </tr>
                                                        <tr id="divConfirmationInfo" runat="server" >
                                                            <td>
                                                                <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                                    <span style='color: red;'>Konfirmasi : </span>
                                                                    <span style='color: Blue;'>                                                    
                                                                        <%#:Eval("cfConfirmationDateTime") %>, <%#:Eval("ConfirmationPhysicianName") %></span>
                                                                        <div id="divConfirmationRemarks">
                                                                            <br />
                                                                            <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic "
                                                                                readonly><%#: DataBinder.Eval(Container.DataItem, "ConfirmationRemarks") %> </textarea>
                                                                        </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Instruksi" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                                        readonly><%#: DataBinder.Eval(Container.DataItem, "InstructionText") %> </textarea>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <div style="color: blue; font-style: italic; vertical-align: top; display:none">
                                                    <%#:Eval("cfCreatedDate") %>,
                                                </div>
                                                <div>
                                                    <b><label id="lblParamedicName" class='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "lblLink lblParamedicName": "lblNormal" %>'>
                                                        <%#:Eval("cfCreatedByName") %></label></b>                                            
                                                </div>
                                                <div id="divParamedicSignature" runat="server" style='margin-top: 5px; text-align: left'>
                                                    <input type="button" id="btnSignature" runat="server" class="btnSignature" value="Ttd" title="Tanda Tangan"
                                                        style='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "display:none;": "height: 25px; width: 60px; background-color: Red; color: White;" %>' />
                                                </div>
                                                <div>
                                                    <img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                        alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                        cursor: pointer; min-width: 30px; float: left;' title="Using Notification" /></div>
                                                <div id="divVerified" align="center" style='<%# Eval("IsVerified").ToString() == "True" ? "display:none;": "" %>'>
                                                    <br />
                                                    <div>
                                                        <input type="button" id="btnVerify" runat="server" class="btnVerify" value="VERIFY"
                                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                    </div>
                                                </div>
                                                <div id="divVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                                    <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                        <span style='color: red;'>Verifikasi :</span>
                                                        <br />
                                                        <span style='color: Blue;'>
                                                            <%#:Eval("cfVerifiedPrimaryNurseDateTime") %>
                                                            <br />
                                                            <%#:Eval("VerifiedPrimaryNurseName") %></span>
                                                    </div>
                                                </div>
                                                <div id="divPhysicianVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                                    <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                        <span style='color: red;'>Verified :</span>
                                                        <br />
                                                        <span style='color: Blue;'>
                                                            <%#:Eval("cfVerifiedDateTime") %>, <%#:Eval("VerifiedPhysicianName") %></span>
                                                            <div id="divVerificationRemarks">
                                                                <br />
                                                                <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic "
                                                                    readonly><%#: DataBinder.Eval(Container.DataItem, "VerificationRemarks") %> </textarea>
                                                            </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Signature1" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature1" />
                                        <asp:BoundField DataField="Signature2" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature2" />
                                        <asp:BoundField DataField="Signature3" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature3" />
                                        <asp:BoundField DataField="Signature4" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature4" />
                                        <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada catatan terintegrasi untuk pasien ini") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingContent4View">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingContent4">
                        </div>
                    </div>
                </div>       
            </td>
        </tr>
    </table>
</div>

