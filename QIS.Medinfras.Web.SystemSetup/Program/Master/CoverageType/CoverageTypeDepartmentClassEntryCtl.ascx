<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoverageTypeDepartmentClassEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.CoverageTypeDepartmentClassEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnClassID.ClientID %>').val('');
        $('#<%=txtClassCode.ClientID %>').val('');
        $('#<%=txtClassName.ClientID %>').val('');

        $('#<%=chkIsMarkupInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtMarkupAmount1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtCoverageAmount1.ClientID %>').val('0').trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtMarkupAmount2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtCoverageAmount2.ClientID %>').val('0').trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtMarkupAmount3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtCoverageAmount3.ClientID %>').val('0').trigger('changeValue');
        
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
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $(this).closest('tr').parent().closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var ID = $row.find('.hdnID').val();
        var classID = $row.find('.hdnClassID').val();
        var classCode = $row.find('.hdnClassCode').val();
        var className = $row.find('.tdClassName').html();

        var markupAmount1 = $row.find('.tdMarkupAmount1').html();
        var discountAmount1 = $row.find('.tdDiscountAmount1').html();
        var coverageAmount1 = $row.find('.tdCoverageAmount1').html();

        var markupAmount2 = $row.find('.tdMarkupAmount2').html();
        var discountAmount2 = $row.find('.tdDiscountAmount2').html();
        var coverageAmount2 = $row.find('.tdCoverageAmount2').html();

        var markupAmount3 = $row.find('.tdMarkupAmount3').html();
        var discountAmount3 = $row.find('.tdDiscountAmount3').html();
        var coverageAmount3 = $row.find('.tdCoverageAmount3').html();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnClassID.ClientID %>').val(classID);
        $('#<%=txtClassCode.ClientID %>').val(classCode);
        $('#<%=txtClassName.ClientID %>').val(className);

        $('#<%=chkIsMarkupInPercentage1.ClientID %>').prop('checked', markupAmount1.indexOf("%") > -1);
        $('#<%=txtMarkupAmount1.ClientID %>').val(markupAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', discountAmount1.indexOf("%") > -1);
        $('#<%=txtDiscountAmount1.ClientID %>').val(discountAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage1.ClientID %>').prop('checked', coverageAmount1.indexOf("%") > -1);
        $('#<%=txtCoverageAmount1.ClientID %>').val(coverageAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage2.ClientID %>').prop('checked', markupAmount2.indexOf("%") > -1);
        $('#<%=txtMarkupAmount2.ClientID %>').val(markupAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', discountAmount2.indexOf("%") > -1);
        $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage2.ClientID %>').prop('checked', coverageAmount2.indexOf("%") > -1);
        $('#<%=txtCoverageAmount2.ClientID %>').val(coverageAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage3.ClientID %>').prop('checked', markupAmount3.indexOf("%") > -1);
        $('#<%=txtMarkupAmount3.ClientID %>').val(markupAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3.ClientID %>').prop('checked', discountAmount3.indexOf("%") > -1);
        $('#<%=txtDiscountAmount3.ClientID %>').val(discountAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage3.ClientID %>').prop('checked', coverageAmount3.indexOf("%") > -1);
        $('#<%=txtCoverageAmount3.ClientID %>').val(coverageAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        

        $('#containerPopupEntryData').show();
    });

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
        hideLoadingPanel();
    }

    //#region Class
    $('#lblClass.lblLink').live('click', function () {
        var filterExpression = "ClassID NOT IN (SELECT ClassID FROM CoverageTypeDepartmentClass WHERE CoverageTypeID = " + $('#<%=hdnCoverageTypeID.ClientID %>').val() + " AND DepartmentID = '" + $('#<%=hdnDepartmentID.ClientID %>').val() + "' AND IsDeleted = 0) AND IsDeleted = 0";
        openSearchDialog('classcare', filterExpression, function (value) {
            $('#<%=txtClassCode.ClientID %>').val(value);
            onTxtCoverageTypeDepartmentClassCodeChanged(value);
        });
    });

    $('#<%=txtClassCode.ClientID %>').live('change', function () {
        onTxtCoverageTypeDepartmentClassCodeChanged($(this).val());
    });

    function onTxtCoverageTypeDepartmentClassCodeChanged(value) {
        var filterExpression = "ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
                $('#<%=txtClassName.ClientID %>').val(result.ClassName);
            }
            else {
                $('#<%=hdnClassID.ClientID %>').val('');
                $('#<%=txtClassCode.ClientID %>').val('');
                $('#<%=txtClassName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Coverage Type Facility Class")%></div>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Coverage Type")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtCoverageType" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Facility")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtDepartment" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col style="width:600px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblClass"><%=GetLabel("Class")%></label></td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnClassID" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtClassCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtClassName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:20px"/>
                                            <col style="width:2px" />
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                        </colgroup>
                                        <tr>
                                            <td colspan="3" style="padding-bottom:2px"><div class="lblComponent"><%=GetLabel("Service")%></div></td>
                                            <td>&nbsp;</td>
                                            <td colspan="3" style="padding-bottom:2px"><div class="lblComponent"><%=GetLabel("Drug MS")%></div></td>
                                            <td>&nbsp;</td>
                                            <td colspan="3" style="padding-bottom:2px"><div class="lblComponent"><%=GetLabel("Logistic")%></div></td>
                                        </tr>
                                        <tr>
                                            <td><div class="lblComponent"><%=GetLabel("%")%></div></td>
                                            <td>&nbsp;</td>
                                            <td><div class="lblComponent"><%=GetLabel("Amount")%></div></td>
                                            <td>&nbsp;</td>
                                            <td><div class="lblComponent"><%=GetLabel("%")%></div></td>
                                            <td>&nbsp;</td>
                                            <td><div class="lblComponent"><%=GetLabel("Amount")%></div></td>
                                            <td>&nbsp;</td>
                                            <td><div class="lblComponent"><%=GetLabel("%")%></div></td>
                                            <td>&nbsp;</td>
                                            <td><div class="lblComponent"><%=GetLabel("Amount")%></div></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Markup")%></label></td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:20px"/>
                                            <col style="width:2px" />
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:CheckBox ID="chkIsMarkupInPercentage1" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtMarkupAmount1" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:CheckBox ID="chkIsMarkupInPercentage2" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtMarkupAmount2" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:CheckBox ID="chkIsMarkupInPercentage3" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtMarkupAmount3" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Discount")%></label></td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:20px"/>
                                            <col style="width:2px" />
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:CheckBox ID="chkIsDiscountInPercentage1" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtDiscountAmount1" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:CheckBox ID="chkIsDiscountInPercentage2" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtDiscountAmount2" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:CheckBox ID="chkIsDiscountInPercentage3" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtDiscountAmount3" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Coverage")%></label></td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:20px"/>
                                            <col style="width:2px" />
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                            <col style="width:2px"/>
                                            <col style="width:20px"/>
                                            <col style="width:2px"/>
                                            <col style="width:160px"/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:CheckBox ID="chkIsCoverageInPercentage1" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtCoverageAmount1" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:CheckBox ID="chkIsCoverageInPercentage2" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtCoverageAmount2" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:CheckBox ID="chkIsCoverageInPercentage3" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtCoverageAmount3" CssClass="txtCurrency" runat="server" Width="100%" /></td>
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
                                                <th style="width:70px" rowspan="2" align="center">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="center"><%=GetLabel("Class")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Services")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Drug & Medical Supply")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Logistic")%></th>    
                                            </tr>
                                            <tr>  
                                                <th style="width:120px" align="center"><%=GetLabel("Markup")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Discount")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Coverage")%></th>  
                                                 
                                                <th style="width:120px" align="center"><%=GetLabel("Markup")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Discount")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Coverage")%></th>  
                                                    
                                                <th style="width:120px" align="center"><%=GetLabel("Markup")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Discount")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Coverage")%></th>                                                 
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="12">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2" align="center">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="center"><%=GetLabel("Class")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Services")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Drug & Medical Supply")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Logistic")%></th>    
                                            </tr>
                                            <tr>  
                                                <th style="width:120px" align="center"><%=GetLabel("Markup")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Discount")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Coverage")%></th>  
                                                 
                                                <th style="width:120px" align="center"><%=GetLabel("Markup")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Discount")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Coverage")%></th>  
                                                    
                                                <th style="width:120px" align="center"><%=GetLabel("Markup")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Discount")%></th>
                                                <th style="width:120px" align="center"><%=GetLabel("Coverage")%></th>                                                 
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt=""  /></td>
                                                        <td style="width:1px">&nbsp;</td>
                                                        <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt=""  /></td>
                                                    </tr>
                                                </table>

                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnClassID" value="<%#: Eval("ClassID")%>" />
                                                <input type="hidden" class="hdnClassCode" value="<%#: Eval("ClassCode")%>" />
                                            </td>
                                            <td class="tdClassName"><%#: Eval("ClassName")%></td>

                                            <td class="tdMarkupAmount1" align="right"><%#: Eval("DisplayMarkupAmount1")%></td>
                                            <td class="tdDiscountAmount1" align="right"><%#: Eval("DisplayDiscountAmount1")%></td>
                                            <td class="tdCoverageAmount1" align="right"><%#: Eval("DisplayCoverageAmount1")%></td>

                                            <td class="tdMarkupAmount2" align="right"><%#: Eval("DisplayMarkupAmount2")%></td>
                                            <td class="tdDiscountAmount2" align="right"><%#: Eval("DisplayDiscountAmount2")%></td>
                                            <td class="tdCoverageAmount2" align="right"><%#: Eval("DisplayCoverageAmount2")%></td>

                                            <td class="tdMarkupAmount3" align="right"><%#: Eval("DisplayMarkupAmount3")%></td>
                                            <td class="tdDiscountAmount3" align="right"><%#: Eval("DisplayDiscountAmount3")%></td>
                                            <td class="tdCoverageAmount3" align="right"><%#: Eval("DisplayCoverageAmount3")%></td>                                            
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

