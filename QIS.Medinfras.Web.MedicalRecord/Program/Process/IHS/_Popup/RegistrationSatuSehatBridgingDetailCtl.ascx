<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationSatuSehatBridgingDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.RegistrationSatuSehatBridgingDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<input type="hidden" value="" id="hdnRegistrationID" runat="server" />
<input type="hidden" value="" id="hdnResourceID" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<table style="width: 100%">
    <colgroup>
        <col style="width: 25%" />
        <col style="width: 75%" />
    </colgroup>
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewResource" runat="server" Width="100%" ClientInstanceName="cbpViewResource"
                    ShowLoadingPanel="false" OnCallback="cbpViewResource_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewResourceEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <table style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 70%" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Registrasi")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdViewResourceType" runat="server" CssClass="grdSelected grdPODT" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ResourceID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Item" >
                                            <ItemTemplate>
                                                <%#:Eval("ResourceType") %>
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
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </div>
        </td>
        <td valign="top">
            <div id="divEncounter" style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewEncounter" runat="server" Width="100%" ClientInstanceName="cbpViewEncounter"
                    ShowLoadingPanel="false" OnCallback="cbpViewEncounter_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ oncbpViewEncounterEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdViewEncounter" runat="server" CssClass="grdSelected grdPRDT" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="EncounterID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Kunjungan" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><b><%#:Eval("RegistrationNo") %></b></div>
                                                <div>Unit : <%#:Eval("ServiceUnitName") %></div>
                                                <div>DPJP : <%#:Eval("ParamedicName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Pasien" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><b><%#:Eval("PatientName") %></b></div>
                                                <div><i><%#:Eval("PatientIHSNumber") %></i></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Riwayat Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>Arrived     : <%#:Eval("RegistrationDateTimeInString") %></div>
                                                <div>In-Progress : <%#:Eval("StartServiceDateTimeInString") %></div>
                                                <div>Finished    : <%#:Eval("PhysicianDischargeDateTimeInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="IHS Encounter ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><%#:Eval("EncounterID") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDtEncounter">
                        </div>
                    </div>
                </div>
            </div>
            <div id="divCondition" style="position: relative">
                <dxcp:ASPxCallbackPanel ID="cbpViewCondition" runat="server" Width="100%" ClientInstanceName="cbpViewCondition"
                    ShowLoadingPanel="false" OnCallback="cbpViewCondition_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ oncbpViewConditionEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server">
                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdViewCondition" runat="server" CssClass="grdSelected grdPRDT" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="EncounterID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Jenis Diagnosa" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><b><%#:Eval("DiagnoseType") %></b></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Diagnosa" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><b><%#:Eval("FinalDiagnosisID") %></b></div>
                                                <div><%#:Eval("FinalDiagnosisName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="IHS Condition ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><%#:Eval("IHSConditionID") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="Div2">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDtCondition">
                        </div>
                    </div>
                </div>
            </div>
            <div id="divObservation" style="position: relative">
                <dxcp:ASPxCallbackPanel ID="cbpViewObservation" runat="server" Width="100%" ClientInstanceName="cbpViewObservation"
                    ShowLoadingPanel="false" OnCallback="cbpViewObservation_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ oncbpViewObservationEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent4" runat="server">
                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdViewObservation" runat="server" CssClass="grdSelected grdPRDT" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="EncounterID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Tanda Vital" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><b><%#:Eval("VitalSignName") %></b></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="Nilai" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><%#:Eval("VitalSignValue") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderText="IHS ObservationID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><%#:Eval("IHSObservationID") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="Div3">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="Div4">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
<script type="text/javascript" id="dxss_infopurchaseorderctl">
    $(function () {
        $('#<%=grdViewResourceType.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewResourceType.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnResourceID.ClientID %>').val($(this).find('.keyField').html());

                cbpViewResource.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewEncounter.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewEncounter.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $('#<%=grdViewCondition.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewCondition.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $('#<%=grdViewObservation.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewObservation.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $("#divCondition").hide();
        $('#<%=grdViewResourceType.ClientID %> tr:eq(1)').click();
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpViewResource.PerformCallback('changepage|' + page);
        });
    });

    function oncbpViewResourceEndCallback(s) {
        $('#containerImgLoadingView').hide(); ;
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            if ($('#<%=hdnResourceID.ClientID %>').val() == "encounter") {
                $("#divEncounter").show();
                $("#divCondition").hide();
                $("#divObservation").hide();
                $('#<%=grdViewEncounter.ClientID %> tr:eq(1)').click();
            }
            else if ($('#<%=hdnResourceID.ClientID %>').val() == "condition") {
                $("#divEncounter").hide();
                $("#divCondition").show();
                $("#divObservation").hide();
                $('#<%=grdViewCondition.ClientID %> tr:eq(1)').click();
            }
            else if ($('#<%=hdnResourceID.ClientID %>').val() == "observation") {
                $("#divEncounter").hide();
                $("#divCondition").hide();
                $("#divObservation").show();
                $('#<%=grdViewCondition.ClientID %> tr:eq(1)').click();
            }
        }
        else {
            if ($('#<%=hdnResourceID.ClientID %>').val() == "encounter") {
                $('#<%=grdViewEncounter.ClientID %> tr:eq(1)').click();
            }
            else if ($('#<%=hdnResourceID.ClientID %>').val() == "condition") {
                $('#<%=grdViewCondition.ClientID %> tr:eq(1)').click();
            }
            else if ($('#<%=hdnResourceID.ClientID %>').val() == "observation") {
                $('#<%=grdViewObservation.ClientID %> tr:eq(1)').click();
            }
        }

        setPaging($("#paging"), pageCount, function (page) {
            cbpViewResource.PerformCallback('changepage|' + page);
        });
    }
    //#endregion

    //#region Paging Dt
    function oncbpViewEncounterEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0) {
                $('#<%=grdViewResourceType.ClientID %> tr:eq(1)').click();
            }
        }
        else {
            $('#<%=grdViewResourceType.ClientID %> tr:eq(1)').click();
        }
    }

    function oncbpViewConditionEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0) {
                $('#<%=grdViewResourceType.ClientID %> tr:eq(1)').click();
            }
        }
        else {
            $('#<%=grdViewResourceType.ClientID %> tr:eq(1)').click();
        }
    }

    function oncbpViewObservationEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0) {
                $('#<%=grdViewResourceType.ClientID %> tr:eq(1)').click();
            }
        }
        else {
            $('#<%=grdViewResourceType.ClientID %> tr:eq(1)').click();
        }
    }
    //#endregion
</script>
