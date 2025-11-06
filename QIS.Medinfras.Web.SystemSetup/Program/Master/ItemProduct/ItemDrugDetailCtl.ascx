<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemDrugDetailCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ItemDrugDetailCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxis_itemdrugdetailctl" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
<script type="text/javascript" id="dxss_itemdrugdetailctl">
    var grdDrugDetail = new InlineEditing();
    var listParam = new Array();
    var cboAlternateUnitID = '<%=cboAlternateUnit.ClientID%>';
    var drugDetailType = parseInt($('#<%=hdnDrugDetailType.ClientID%>').val());
    var columnCount = parseInt('<%=columnCount%>');

    function init() {
        addParameter();
        grdDrugDetail.setOnIsChangedChangeHandler(function (isChanged) {
            grdDrugDetail.isChanged = isChanged;
        });
        grdDrugDetail.init('tblDrugDetail', listParam);
        grdDrugDetail.addRow(true);
        loadDrugDetail();
    }

    function addParameter() {
        if (drugDetailType == 10) {
            listParam[0] = { "type": "cbo", "className": "cboAlternateUnit", "cboID": cboAlternateUnitID, "isUnique": true, "isEnabled": true };
            listParam[1] = { "type": "txt", "dataType": "float", "className": "txtConversionFactor", "isEnabled": true };
        }
        else {
            for (var i = 0; i < columnCount; ++i) {
                listParam[i] = { "type": "txt", "className": "txt" + i, "isEnabled": true };
            }
        }
    }

    function loadDrugDetail() {
        if (drugDetailType > 0) {
            var filterExpression = "ItemID = " + $('#<%=hdnItemID.ClientID %>').val();
            var methodName = "";
            switch (drugDetailType) {
                case 1: methodName = "GetDrugContentList"; break;
                case 2: methodName = "GetDrugIndicationList"; break;
                case 3: methodName = "GetDrugDosageList"; break;
                case 4: methodName = "GetDrugAdministrationList"; break;
                case 5: methodName = "GetDrugContraIndicationList"; break;
                case 6: methodName = "GetDrugPrecautionList"; break;
                case 7: methodName = "GetDrugReactionList"; break;
                case 10: methodName = "GetItemAlternateUnitList";
                    filterExpression += " AND IsDeleted = 0"; break;
            }
            if (methodName != "")
                Methods.getListObject(methodName, filterExpression, function (result) {
                    for (var i = 0; i < result.length; ++i) {
                        var drugDetail = result[i];
                        grdDrugDetail.addRow();
                        $row = grdDrugDetail.getRow(i);
                        fillTableData($row, drugDetail);
                    }
                });
        }
    }

    function fillTableData($row, drugDetail) {
        if (drugDetailType == 1) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setTextBoxProperties($row, 'txt0', { "value": drugDetail.Keyword });
            grdDrugDetail.setTextBoxProperties($row, 'txt1', { "value": drugDetail.ContentText });
        }
        else if (drugDetailType == 2) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setTextBoxProperties($row, 'txt0', { "value": drugDetail.Keyword });
            grdDrugDetail.setTextBoxProperties($row, 'txt1', { "value": drugDetail.IndicationText1 });
            grdDrugDetail.setTextBoxProperties($row, 'txt2', { "value": drugDetail.IndicationText2 });
        }
        else if (drugDetailType == 3) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setTextBoxProperties($row, 'txt0', { "value": drugDetail.Keyword });
            grdDrugDetail.setTextBoxProperties($row, 'txt1', { "value": drugDetail.DosageText1 });
            grdDrugDetail.setTextBoxProperties($row, 'txt2', { "value": drugDetail.DosageText2 });
        }
        else if (drugDetailType == 4) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setTextBoxProperties($row, 'txt0', { "value": drugDetail.AdministrationText1 });
            grdDrugDetail.setTextBoxProperties($row, 'txt1', { "value": drugDetail.AdministrationText2 });
        }
        else if (drugDetailType == 5) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setTextBoxProperties($row, 'txt0', { "value": drugDetail.ContraIndicationText1 });
            grdDrugDetail.setTextBoxProperties($row, 'txt1', { "value": drugDetail.ContraIndicationText2 });
        }
        else if (drugDetailType == 6) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setTextBoxProperties($row, 'txt0', { "value": drugDetail.PrecautionText1 });
            grdDrugDetail.setTextBoxProperties($row, 'txt1', { "value": drugDetail.PrecautionText2 });
        }
        else if (drugDetailType == 7) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setTextBoxProperties($row, 'txt0', { "value": drugDetail.AdverseReactionText1 });
            grdDrugDetail.setTextBoxProperties($row, 'txt1', { "value": drugDetail.AdverseReactionText2 });
        }
        else if (drugDetailType == 10) {
            grdDrugDetail.setKeyValue($row, drugDetail.ID);
            grdDrugDetail.setComboBoxProperties($row, 'cboAlternateUnit', { "value": drugDetail.GCAlternateUnit, "isEnabled": false });
            grdDrugDetail.setTextBoxProperties($row, 'txtConversionFactor', { "value": drugDetail.ConversionFactor });
        }
    }

    function onBeforeSaveRecord() {
        $('#<%=hdnInlineEditingData.ClientID %>').val(grdDrugDetail.getTableData());
        return true;
    }
</script>

<input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
<input type="hidden" id="hdnItemID" runat="server" />
<input type="hidden" id="hdnDrugDetailType" value="0" runat="server" />

<table class="grdNormal" id="tblDrugDetail" style="width:100%;font-size:0.9em" cellpadding="0" cellspacing="0">
    <tr>
        <th align="center" style="width:30px">#</th>
        <asp:Repeater ID="rptTblDrugDetail" runat="server">
            <ItemTemplate>
                <th align="center"><%#:Eval("Caption") %></th>    
            </ItemTemplate>
        </asp:Repeater>
    </tr>
</table>

<div id="containerCbo" style="display:none">

</div>
<dxe:ASPxComboBox ID="cboAlternateUnit" ClientInstanceName="cboAlternateUnit" runat="server" Width="100%" 
    ValueField="StandardCodeID" ClientVisible="false" TextField="StandardCodeName" IncrementalFilteringMode="Contains" EnableSynchronization="False" >
    <ClientSideEvents Init="function(s,e){ init(); }"
        LostFocus="function(s,e){grdDrugDetail.hideAspxComboBox(s);}" 
        KeyDown="grdDrugDetail.onCboKeyDown"
        EndCallback="function(s,e){ loadDrugDetail(); }" />
</dxe:ASPxComboBox>