<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FractionNormalValueEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Laboratory.Program.FractionNormalValueEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_fractionnormalvalueentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        cboSex.SetValue('');
        $('#<%=txtFromAge.ClientID %>').val('');
        $('#<%=txtToAge.ClientID %>').val('');
        cboAgeUnit.SetValue('');
        $('#hdnConversionFactorTemp').val('1')
        $('#<%=chkIsPregnant.ClientID %>').prop('checked', false);

        $('#<%=txtMetricUnitMin.ClientID %>').val('0');
        $('#<%=txtMetricUnitMax.ClientID %>').val('0');
        $('#<%=txtMetricUnitLabel.ClientID %>').val('');

        $('#<%=txtPanicMetricUnitMin.ClientID %>').val('0');
        $('#<%=txtPanicMetricUnitMax.ClientID %>').val('0');
        $('#<%=txtPanicMetricUnitLabel.ClientID %>').val('');

        $('#<%=txtInternationalUnitMin.ClientID %>').val('0');
        $('#<%=txtInternationalUnitMax.ClientID %>').val('0');
        $('#<%=txtInternationalUnitLabel.ClientID %>').val('');

        $('#<%=txtPanicInternationalUnitMin.ClientID %>').val('0');
        $('#<%=txtPanicInternationalUnitMax.ClientID %>').val('0');
        $('#<%=txtPanicInternationalUnitLabel.ClientID %>').val('');
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
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        cboSex.SetValue(entity.GCsex);
        $('#<%=txtFromAge.ClientID %>').val(entity.fromAge);
        $('#<%=txtToAge.ClientID %>').val(entity.toAge);
        cboAgeUnit.SetValue(entity.GCAgeUnit);
        $('#<%=chkIsPregnant.ClientID %>').prop('checked', entity.isPregnant == 'True');

        $('#<%=txtMetricUnitMin.ClientID %>').val(entity.MetricUnitMin);
        $('#<%=txtMetricUnitMax.ClientID %>').val(entity.MetricUnitMax);
        $('#<%=txtMetricUnitLabel.ClientID %>').val(entity.MetricUnitLabel);

        $('#<%=txtPanicMetricUnitMin.ClientID %>').val(entity.PanicMetricUnitMin);
        $('#<%=txtPanicMetricUnitMax.ClientID %>').val(entity.PanicMetricUnitMax);
        $('#<%=txtPanicMetricUnitLabel.ClientID %>').val(entity.PanicMetricUnitLabel);

        $('#<%=txtInternationalUnitMin.ClientID %>').val(entity.InternationalUnitMin);
        $('#<%=txtInternationalUnitMax.ClientID %>').val(entity.InternationalUnitMax);
        $('#<%=txtInternationalUnitLabel.ClientID %>').val(entity.InternationalUnitLabel);

        $('#<%=txtPanicInternationalUnitMin.ClientID %>').val(entity.PanicInternationalUnitMin);
        $('#<%=txtPanicInternationalUnitMax.ClientID %>').val(entity.PanicInternationalUnitMax);
        $('#<%=txtPanicInternationalUnitLabel.ClientID %>').val(entity.PanicInternationalUnitLabel);

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

    //#region Set Label
    function setLabelMetric(min, max, unit) {
        var label = min + " - " + max + " " + unit;
        $('#<%=txtMetricUnitLabel.ClientID %>').val(label);
    }

    function setLabelPanicMetric(min, max, unit) {
        var label = min + " - " + max + " " + unit;
        $('#<%=txtPanicMetricUnitLabel.ClientID %>').val(label);
    }

    function setLabelInternational(min, max, unit) {
        var label = min + " - " + max + " " + unit;
        $('#<%=txtInternationalUnitLabel.ClientID %>').val(label);
    }

    function setLabelPanicInternational(min, max, unit) {
        var label = min + " - " + max + " " + unit;
        $('#<%=txtPanicInternationalUnitLabel.ClientID %>').val(label);
    }
    //#endregion

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);

        $('#<%=txtMetricUnitMin.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp * conversionFactor;
            $('#<%=txtInternationalUnitMin.ClientID %>').val(result);

            var min1 = temp;
            var max1 = $('#<%=txtMetricUnitMax.ClientID %>').val();
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelMetric(min1, max1, unit1);

            var min2 = result;
            var max2 = $('#<%=txtInternationalUnitMax.ClientID %>').val();
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelInternational(min2, max2, unit2);
        });

        $('#<%=txtMetricUnitMax.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp * conversionFactor;
            $('#<%=txtInternationalUnitMax.ClientID %>').val(result);

            var min1 = $('#<%=txtMetricUnitMin.ClientID %>').val();
            var max1 = temp;
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelMetric(min1, max1, unit1);

            var min2 = $('#<%=txtInternationalUnitMin.ClientID %>').val();
            var max2 = result;
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelInternational(min2, max2, unit2);
        });

        $('#<%=txtPanicMetricUnitMin.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp * conversionFactor;
            $('#<%=txtPanicInternationalUnitMin.ClientID %>').val(result);

            var min1 = temp;
            var max1 = $('#<%=txtPanicMetricUnitMax.ClientID %>').val();
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelPanicMetric(min1, max1, unit1);

            var min2 = result;
            var max2 = $('#<%=txtPanicInternationalUnitMax.ClientID %>').val();
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelPanicInternational(min2, max2, unit2);
        });

        $('#<%=txtPanicMetricUnitMax.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp * conversionFactor;
            $('#<%=txtPanicInternationalUnitMax.ClientID %>').val(result);

            var min1 = $('#<%=txtPanicMetricUnitMin.ClientID %>').val();
            var max1 = temp;
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelPanicMetric(min1, max1, unit1);

            var min2 = $('#<%=txtPanicInternationalUnitMin.ClientID %>').val();
            var max2 = result;
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelPanicInternational(min2, max2, unit2);
        });

        $('#<%=txtInternationalUnitMin.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp / conversionFactor;
            $('#<%=txtMetricUnitMin.ClientID %>').val(result);

            var min1 = result;
            var max1 = $('#<%=txtMetricUnitMax.ClientID %>').val();
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelMetric(min1, max1, unit1);

            var min2 = temp;
            var max2 = $('#<%=txtInternationalUnitMax.ClientID %>').val();
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelInternational(min2, max2, unit2);
        });

        $('#<%=txtInternationalUnitMax.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp / conversionFactor;
            $('#<%=txtMetricUnitMax.ClientID %>').val(result);

            var min1 = $('#<%=txtMetricUnitMin.ClientID %>').val();
            var max1 = result;
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelMetric(min1, max1, unit1);

            var min2 = $('#<%=txtInternationalUnitMin.ClientID %>').val();
            var max2 = temp;
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelInternational(min2, max2, unit2);
        });

        $('#<%=txtPanicInternationalUnitMin.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp / conversionFactor;
            $('#<%=txtPanicMetricUnitMin.ClientID %>').val(result);

            var min1 = result;
            var max1 = $('#<%=txtPanicMetricUnitMax.ClientID %>').val();
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelPanicMetric(min1, max1, unit1);

            var min2 = temp;
            var max2 = $('#<%=txtPanicInternationalUnitMax.ClientID %>').val();
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelPanicInternational(min2, max2, unit2);
        });

        $('#<%=txtPanicInternationalUnitMax.ClientID %>').change(function () {
            var temp = $(this).val();
            var conversionFactor = $('#<%=hdnConversionFactor.ClientID %>').val();
            var result = temp / conversionFactor;
            $('#<%=txtPanicMetricUnitMax.ClientID %>').val(result);

            var min1 = $('#<%=txtPanicMetricUnitMin.ClientID %>').val();
            var max1 = result;
            var unit1 = $('#<%=txtMetricUnit.ClientID %>').val();
            setLabelPanicMetric(min1, max1, unit1);

            var min2 = $('#<%=txtPanicInternationalUnitMin.ClientID %>').val();
            var max2 = temp;
            var unit2 = $('#<%=txtInternationalUnit.ClientID %>').val();
            setLabelPanicInternational(min2, max2, unit2);
        });
    });
    //#endregion

</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnFractionID" value="" runat="server" />
    <input type="hidden" id="hdnConversionFactor" value="" runat="server" />
    <input type="hidden" id="hdnGCMetricUnit" value="" runat="server" />
    <input type="hidden" id="hdnGCInternationalUnit" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 90px" />
                        <col style="width: 70px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Artikel Pemeriksaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFractionCode" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFractionName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 35%" />
                                <col style="width: 65%" />
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <table>
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 300px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Jenis Kelamin")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboSex" ClientInstanceName="cboSex" runat="server" Width="150px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Umur mulai")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromAge" CssClass="number required" runat="server" Width="150px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("s/d umur")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToAge" CssClass="number required" runat="server" Width="150px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Satuan Umur")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboAgeUnit" ClientInstanceName="cboAgeUnit" runat="server"
                                                    Width="150px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Hamil")%></label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsPregnant" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top">
                                    <table class="grdNormal" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 130px" />
                                            <col style="width: 100px" />
                                            <col style="width: 70px" />
                                            <col style="width: 70px" />
                                            <col style="width: 100px" />
                                            <col style="width: 70px" />
                                            <col style="width: 70px" />
                                            <col style="width: 100px" />
                                        </colgroup>
                                        <tr>
                                            <th rowspan="2" align="left">
                                                <%=GetLabel("KETERANGAN")%>
                                            </th>
                                            <th rowspan="2" align="left">
                                                <%=GetLabel("SATUAN")%>
                                            </th>
                                            <th colspan="3" align="center">
                                                <%=GetLabel("NORMAL RANGE")%>
                                            </th>
                                            <th colspan="3" align="center">
                                                <%=GetLabel("PANIC RANGE")%>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th align="right">
                                                <%=GetLabel("Lower")%>
                                            </th>
                                            <th align="right">
                                                <%=GetLabel("Upper")%>
                                            </th>
                                            <th align="right">
                                                <%=GetLabel("Label")%>
                                            </th>
                                            <th align="right">
                                                <%=GetLabel("Lower")%>
                                            </th>
                                            <th align="right">
                                                <%=GetLabel("Upper")%>
                                            </th>
                                            <th align="right">
                                                <%=GetLabel("Label")%>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%=GetLabel("Conventional")%>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMetricUnit" CssClass="number required" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetricUnitMin" CssClass="number required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetricUnitMax" CssClass="number required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetricUnitLabel" CssClass="number required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPanicMetricUnitMin" CssClass="number required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPanicMetricUnitMax" CssClass="number required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPanicMetricUnitLabel" CssClass="number required" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%=GetLabel("International")%>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtInternationalUnit" CssClass="number required" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInternationalUnitMin" CssClass="number required" Width="100%"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInternationalUnitMax" CssClass="number required" Width="100%"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInternationalUnitLabel" CssClass="number required" Width="100%"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPanicInternationalUnitMin" CssClass="number required" Width="100%"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPanicInternationalUnitMax" CssClass="number required" Width="100%"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPanicInternationalUnitLabel" CssClass="number required" Width="100%"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="width: 70px" rowspan="3">
                                                    &nbsp;
                                                </th>
                                                <th style="width: 60px" rowspan="3" align="center">
                                                    <%=GetLabel("Jenis Kelamin")%>
                                                </th>
                                                <th style="width: 60px" rowspan="3" align="center">
                                                    <%=GetLabel("Umur")%>
                                                </th>
                                                <th style="width: 50px" rowspan="3" align="center">
                                                    <%=GetLabel("Hamil")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("CONVENTIONAL")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("INTERNATIONAL")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 50px" rowspan="2" align="center">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Normal Range")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Panic Range")%>
                                                </th>
                                                <th style="width: 50px" rowspan="2" align="center">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Normal Range")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Panic Range")%>
                                                </th>
                                            </tr>                                            
                                            <tr>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="18">
                                                    <%=GetLabel("Data tidak tersedia")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width: 70px" rowspan="3">
                                                    &nbsp;
                                                </th>
                                                <th style="width: 60px" rowspan="3" align="center">
                                                    <%=GetLabel("Jenis Kelamin")%>
                                                </th>
                                                <th style="width: 60px" rowspan="3" align="center">
                                                    <%=GetLabel("Umur")%>
                                                </th>
                                                <th style="width: 50px" rowspan="3" align="center">
                                                    <%=GetLabel("Hamil")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("CONVENTIONAL")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("INTERNATIONAL")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 50px" rowspan="2" align="center">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Normal Range")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Panic Range")%>
                                                </th>
                                                <th style="width: 50px" rowspan="2" align="center">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Normal Range")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Panic Range")%>
                                                </th>
                                            </tr>                                            
                                            <tr>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Lower")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("Upper")%>
                                                </th>
                                                <th style="width: 70px" align="center">
                                                    <%=GetLabel("Label")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" style="margin-left:2px" />

                                                <input type="hidden" value="<%#: Eval("ID")%>" bindingfield="ID"/>
                                                <input type="hidden" value="<%#: Eval("GCAgeUnit")%>" bindingfield="GCAgeUnit" />
                                                <input type="hidden" value="<%#: Eval("GCMetricUnit")%>" bindingfield="GCMetricUnit" />
                                                <input type="hidden" value="<%#: Eval("MetricUnit")%>" bindingfield="GMetricUnit" />
                                                <input type="hidden" value="<%#: Eval("GCInternationalUnit")%>" bindingfield="GCInternationalUnit"/>
                                                <input type="hidden" value="<%#: Eval("InternationalUnit")%>" bindingfield="InternationalUnit"/>
                                                <input type="hidden" value="<%#: Eval("GCsex")%>" bindingfield="GCsex"/>
                                                <input type="hidden" value="<%#: Eval("fromAge")%>" bindingfield="fromAge"/>
                                                <input type="hidden" value="<%#: Eval("toAge")%>" bindingfield="toAge"/>
                                                <input type="hidden" value="<%#: Eval("isPregnant")%>" bindingfield="isPregnant"/>

                                                <input type="hidden" value="<%#: Eval("MetricUnitMin")%>" bindingfield="MetricUnitMin"/>
                                                <input type="hidden" value="<%#: Eval("MetricUnitMax")%>" bindingfield="MetricUnitMax"/>
                                                <input type="hidden" value="<%#: Eval("MetricUnitLabel")%>" bindingfield="MetricUnitLabel"/>

                                                <input type="hidden" value="<%#: Eval("PanicMetricUnitMin")%>" bindingfield="PanicMetricUnitMin"/>
                                                <input type="hidden" value="<%#: Eval("PanicMetricUnitMax")%>" bindingfield="PanicMetricUnitMax"/>
                                                <input type="hidden" value="<%#: Eval("PanicMetricUnitLabel")%>" bindingfield="PanicMetricUnitLabel"/>

                                                <input type="hidden" value="<%#: Eval("InternationalUnitMin")%>" bindingfield="InternationalUnitMin"/>
                                                <input type="hidden" value="<%#: Eval("InternationalUnitMax")%>" bindingfield="InternationalUnitMax"/>
                                                <input type="hidden" value="<%#: Eval("InternationalUnitLabel")%>" bindingfield="InternationalUnitLabel"/>

                                                <input type="hidden" value="<%#: Eval("PanicInternationalUnitMin")%>" bindingfield="PanicInternationalUnitMin"/>
                                                <input type="hidden" value="<%#: Eval("PanicInternationalUnitMax")%>" bindingfield="PanicInternationalUnitMax"/>
                                                <input type="hidden" value="<%#: Eval("PanicInternationalUnitLabel")%>" bindingfield="PanicInternationalUnitLabel"/>

                                                <input type="hidden" value="<%#: Eval("ConversionFactor")%>" bindingfield="ConversionFactor"/>
                                            </td>
                                            <td align="left"><%#: Eval("Sex")%></td>
                                            <td align="center"><%#: Eval("AgeDisplayText")%></td>

                                            <td align="center"><asp:CheckBox ID="CheckBox1" Checked='<%# (bool)(DataBinder.Eval(Container.DataItem, "IsPregnant")) %>' runat="server" Enabled="false" /></td>
                                            
                                            <td align="center"><%#: Eval("MetricUnit")%></td>

                                            <td align="right"><%#: Eval("MetricUnitMin")%></td>
                                            <td align="right"><%#: Eval("MetricUnitMax")%></td>
                                            <td align="center"><%#: Eval("MetricUnitLabel")%></td>

                                            <td align="right"><%#: Eval("PanicMetricUnitMin")%></td>
                                            <td align="right"><%#: Eval("PanicMetricUnitMax")%></td>
                                            <td align="center"><%#: Eval("PanicMetricUnitLabel")%></td>

                                            <td align="center"><%#: Eval("InternationalUnit")%></td>

                                            <td align="right"><%#: Eval("InternationalUnitMin")%></td>
                                            <td align="right"><%#: Eval("InternationalUnitMax")%></td>
                                            <td align="center"><%#: Eval("InternationalUnitLabel")%></td>

                                            <td align="right"><%#: Eval("PanicInternationalUnitMin")%></td>
                                            <td align="right"><%#: Eval("PanicInternationalUnitMax")%></td>
                                            <td align="center"><%#: Eval("PanicInternationalUnitLabel")%></td>
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
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
