<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FractionNormalValueEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.FractionNormalValueEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        cboSex.SetValue('');
        $('#<%=txtFromAge.ClientID %>').val('');
        $('#<%=txtToAge.ClientID %>').val('');
        cboAgeUnit.SetValue('');
        $('#<%=chkIsPregnant.ClientID %>').prop('checked', false);

        $('#<%=txtMetricUnitMin.ClientID %>').val('');
        $('#<%=txtMetricUnitMax.ClientID %>').val('');
        $('#<%=txtMetricUnitLabel.ClientID %>').val('');
        cboMetricUnit.SetValue('');

        $('#<%=txtInternationalUnitMin.ClientID %>').val('');
        $('#<%=txtInternationalUnitMax.ClientID %>').val('');
        $('#<%=txtInternationalUnitLabel.ClientID %>').val('');
        cboInternationalUnit.SetValue('');

        $('#containerPopupEntryData').show();
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
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        var GCAgeUnit = $row.find('.hdnGCAgeUnit').val();
        var GCMetricUnit = $row.find('.hdnGCMetricUnit').val();
        var GCInternationalUnit = $row.find('.hdnGCInternationalUnit').val();

        var sex = $row.find('.tdSex').html();
        var fromAge = $row.find('.tdFromAge').html();
        var toAge = $row.find('.tdToAge').html();
        var isPregnant = $row.find('.tdIsPregnant').find('input').is(':checked');
        var metricUnitMin = $row.find('.tdMetricUnitMin').html();
        var metricUnitMax = $row.find('.tdMetricUnitMax').html();
        var metricUnitLabel = $row.find('.tdMetricUnitLabel').html();
        var internationalUnitMin = $row.find('.tdInternationalUnitMin').html();
        var internationalUnitMax = $row.find('.tdInternationalUnitMax').html();
        var internationalUnitLabel = $row.find('.tdInternationalUnitLabel').html();
        var internationalUnit = $row.find('.tdInternationalUnit').html();

        $('#<%=hdnID.ClientID %>').val(ID);
        cboSex.SetValue(sex);
        $('#<%=txtFromAge.ClientID %>').val(fromAge);
        $('#<%=txtToAge.ClientID %>').val(toAge);
        cboAgeUnit.SetValue(GCAgeUnit);
        $('#<%=chkIsPregnant.ClientID %>').prop('checked', isPregnant);

        $('#<%=txtMetricUnitMin.ClientID %>').val(metricUnitMin);
        $('#<%=txtMetricUnitMax.ClientID %>').val(metricUnitMax);
        $('#<%=txtMetricUnitLabel.ClientID %>').val(metricUnitLabel);
        cboMetricUnit.SetValue(GCMetricUnit);

        $('#<%=txtInternationalUnitMin.ClientID %>').val(internationalUnitMin);
        $('#<%=txtInternationalUnitMax.ClientID %>').val(internationalUnitMax);
        $('#<%=txtInternationalUnitLabel.ClientID %>').val(internationalUnitLabel);
        cboInternationalUnit.SetValue(GCInternationalUnit);

        $('#containerPopupEntryData').show();
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpEntryPopupView.PerformCallback('changepage|' + page);
            }, 8);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('.grdPopup tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnFractionID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Normal Value")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Code")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtFractionCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Name")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtFractionName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:33%"/>
                                <col style="width:33%"/>
                                <col style="width:33%"/>
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <table>
                                        <colgroup>
                                            <col style="width:80px"/>
                                            <col style="width:200px"/>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Sex")%></label></td>
                                            <td><dxe:ASPxComboBox ID="cboSex" ClientInstanceName="cboSex" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("From Age")%></label></td>
                                            <td><asp:TextBox ID="txtFromAge" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("To Age")%></label></td>
                                            <td><asp:TextBox ID="txtToAge" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Age Unit")%></label></td>
                                            <td><dxe:ASPxComboBox ID="cboAgeUnit" ClientInstanceName="cboAgeUnit" runat="server" Width="100px" /></td>
                                        </tr>  
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Is Pregnant")%></label></td>
                                            <td><asp:CheckBox ID="chkIsPregnant" runat="server" /></td>
                                        </tr>                               
                                    </table>
                                </td>
                                <td valign="top">
                                    <table>
                                        <colgroup>
                                            <col style="width:150px"/>
                                            <col style="width:200px"/>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Metric Unit Min")%></label></td>
                                            <td><asp:TextBox ID="txtMetricUnitMin" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Metric Unit Max")%></label></td>
                                            <td><asp:TextBox ID="txtMetricUnitMax" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Metric Unit Label")%></label></td>
                                            <td><asp:TextBox ID="txtMetricUnitLabel" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Metric Unit")%></label></td>
                                            <td><dxe:ASPxComboBox ID="cboMetricUnit" ClientInstanceName="cboMetricUnit" runat="server" Width="100px" /></td>
                                        </tr>                                
                                    </table>
                                </td>
                                <td valign="top">
                                    <table>
                                        <colgroup>
                                            <col style="width:180px"/>
                                            <col style="width:200px"/>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("International Unit Min")%></label></td>
                                            <td><asp:TextBox ID="txtInternationalUnitMin" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("International Unit Max")%></label></td>
                                            <td><asp:TextBox ID="txtInternationalUnitMax" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("International Unit Label")%></label></td>
                                            <td><asp:TextBox ID="txtInternationalUnitLabel" CssClass="number required" runat="server" Width="100px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("International Unit")%></label></td>
                                            <td><dxe:ASPxComboBox ID="cboInternationalUnit" ClientInstanceName="cboInternationalUnit" runat="server" Width="100px" /></td>
                                        </tr>                                
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2">&nbsp;</th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("Sex")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("From Age")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("To Age")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("Age Unit")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("Is Pregnant")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Metric")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("International")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Min")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Max")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Label")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Min")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Max")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Label")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit")%></th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="14">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2">&nbsp;</th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("Sex")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("From Age")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("To Age")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("Age Unit")%></th>
                                                <th style="width:60px" rowspan="2" align="center"><%=GetLabel("Is Pregnant")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Metric")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("International")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Min")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Max")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Label")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Min")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Max")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit Label")%></th>
                                                <th style="width:60px" align="center"><%=GetLabel("Unit")%></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" style="margin-left:2px" />

                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnGCAgeUnit" value="<%#: Eval("GCAgeUnit")%>" />
                                                <input type="hidden" class="hdnGCMetricUnit" value="<%#: Eval("GCMetricUnit")%>" />
                                                <input type="hidden" class="hdnGCInternationalUnit" value="<%#: Eval("GCInternationalUnit")%>" />
                                            </td>
                                            <td class="tdSex"><%#: Eval("Sex")%></td>
                                            <td class="tdFromAge" align="right"><%#: Eval("FromAge")%></td>
                                            <td class="tdToAge" align="right"><%#: Eval("ToAge")%></td>
                                            <td class="tdAgeUnit" align="left"><%#: Eval("AgeUnit")%></td>
                                            <td class="tdIsPregnant" align="center"><asp:CheckBox Checked='<%#: Eval("IsPregnant")%>' runat="server" Enabled="false" /></td>
                                            <td class="tdMetricUnitMin" align="right"><%#: Eval("MetricUnitMin")%></td>
                                            <td class="tdMetricUnitMax" align="right"><%#: Eval("MetricUnitMax")%></td>
                                            <td class="tdMetricUnitLabel" align="right"><%#: Eval("MetricUnitLabel")%></td>
                                            <td class="tdMetricUnit" align="right"><%#: Eval("MetricUnit")%></td>
                                            <td class="tdInternationalUnitMin" align="right"><%#: Eval("InternationalUnitMin")%></td>
                                            <td class="tdInternationalUnitMax" align="right"><%#: Eval("InternationalUnitMax")%></td>
                                            <td class="tdInternationalUnitLabel" align="right"><%#: Eval("InternationalUnitLabel")%></td>
                                            <td class="tdInternationalUnit" align="right"><%#: Eval("InternationalUnit")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
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

