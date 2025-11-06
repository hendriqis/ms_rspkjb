<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="MCUResultDetail.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUResultDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnClinicTransactionBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table style="float: right" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnListKey" runat="server" />
    <input type="hidden" value="" id="hdnListIsChecked" runat="server" />
    <input type="hidden" value="" id="hdnListRemarks" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnStandardCodeID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtActualVisitDate.ClientID %>');
            $('#<%=txtActualVisitDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#ulTabMCUTransaction li:first-child').addClass('selected');
            var stdID = $(this).find('.stdID').val();
            $('#ulTabMCUTransaction li').click(function () {
                var stdID = $(this).find('.stdID').val();
                $('#<%=hdnStandardCodeID.ClientID %>').val(stdID);
                $('#ulTabMCUTransaction li.selected').removeClass('selected');
                $(this).addClass('selected');
                cbpView.PerformCallback('changeTab');
                getCheckedMember();

            });
        }

        function getCheckedMember() {
            var listKey = [];
            var listIsChecked = [];
            var listRemarks = [];
            $('.grdResult tr:gt(0)').each(function () {
                $tr = $(this).closest('tr');
                var key = $tr.find('.hdnKey').val();
                var remarks = $tr.find('.txtRemarks').val();
                var isChecked = '0';
                if ($tr.find('.chkIsSelected input').is(':checked')) {
                    isChecked = '1';
                };
                listKey.push(key);
                listIsChecked.push(isChecked);
                listRemarks.push(remarks);

            });
            $('#<%=hdnListKey.ClientID %>').val(listKey.join('|'));
            $('#<%=hdnListIsChecked.ClientID %>').val(listIsChecked.join('|'));
            $('#<%=hdnListRemarks.ClientID %>').val(listRemarks.join('|'));
        }

        $('#<%=btnClinicTransactionBack.ClientID %>').click(function () {
            showLoadingPanel();
            document.location = ResolveUrl("~/Program/PatientList/VisitList.aspx?id=hsl"); ;
        });

        $('#<%=btnSave.ClientID %>').click(function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                getCheckedMember();
                cbpView.PerformCallback('save');
            }
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                if (param[1] == 'save') {
                    cbpView.PerformCallback('refresh');
                }
            }
            else
                showToast('Error', param[1]);
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            if (visitID == '') {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
            else {
                if (code == 'MC-00002' || code == 'MC-00004' || code == 'MC-00010') {
                    filterExpression.text = 'VisitID = ' + visitID;
                    return true;
                }
                else {
                    filterExpression.text = 'VisitID = ' + visitID;
                    return true;
                }
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var param = $('#<%:hdnVisitID.ClientID %>').val();
            return param;
        }

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == '')
                serviceUnitID = '0';
            var filterExpression = 'IsDeleted = 0';
            if (serviceUnitID != '0')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ") AND IsDeleted = 0 AND (GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "' OR IsHasPhysicianRole = 1)";
            return filterExpression;
        }

        $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%:txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                }
            });
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });
    </script>
    <div>
        <table>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal lblMandatory" />
                    <%=GetLabel("Tanggal") %>
                    -
                    <%=GetLabel("Jam") %>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-right: 1px; width: 145px">
                                <asp:TextBox ID="txtActualVisitDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td style="width: 5px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtActualVisitTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <%=GetLabel("No. Hadir") %>
                </td>
                <td>
                    <asp:TextBox ID="txtQueueNo" Width="100px" runat="server" CssClass="number" input
                        type="number" Style="text-align: center" />
                </td>
            </tr>
            <tr id="trPhysician" runat="server">
                <td class="tdLabel" style="width: 30%">
                    <label class="lblLink" runat="server" id="lblPhysician">
                        <%:GetLabel("Dokter")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnParamedicID" value="" runat="server" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="300%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="tblContentArea">
            <tr>
                <td>
                    <div class="containerUlTabPage" style="overflow-y: scroll; height: 70px;">
                        <asp:Repeater ID="rptHeader" runat="server">
                            <HeaderTemplate>
                                <ul class="ulTabPage" id="ulTabMCUTransaction">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li style='border: solid grey 1px; border-radius: 5px; width=10px; height=10px; margin: 2px;
                                    padding-left: 2px; padding-right: 2px'>
                                    <input type="hidden" value="<%#:Eval("StandardCodeID") %>" class="stdID" />
                                    <b>
                                        <%#: Eval("StandardCodeName")%>
                                    </b></li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){;showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdResult grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                        <th style="width: 550px" align="left">
                                                            <b>
                                                                <%=GetLabel("Judul")%>
                                                            </b>
                                                        </th>
                                                        <th style="width: 30px" align="center">
                                                        </th>
                                                        <th style="width: 800px" align="left">
                                                            <b>
                                                                <%=GetLabel("Catatan")%>
                                                            </b>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="3">
                                                            <%=GetLabel("Tidak ada Data.")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdResult grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                        <th style="width: 550px" align="left">
                                                            <b>
                                                                <%=GetLabel("Judul")%>
                                                            </b>
                                                        </th>
                                                        <th style="width: 30px" align="center">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th style="width: 800px" align="left">
                                                            <b>
                                                                <%=GetLabel("Catatan")%>
                                                            </b>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField">
                                                        <%#: Eval("StandardCodeID")%>
                                                    </td>
                                                    <td align="left">
                                                        <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                        <%#: Eval("StandardCodeName")%>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtRemarks" Width="100%" class="txtRemarks" TextMode="MultiLine"
                                                            Rows="4" runat="server" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
