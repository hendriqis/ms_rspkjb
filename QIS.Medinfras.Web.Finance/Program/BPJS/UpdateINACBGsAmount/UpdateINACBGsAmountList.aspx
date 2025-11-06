<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="UpdateINACBGsAmountList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.UpdateINACBGsAmountList" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        }

        $('#btnRefresh').live('click', function () {
            cbpProcessDetail.PerformCallback('refresh');
        });

        $('#<%=btnSave.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah yakin akan proses SAVE ?', function (result) {
                if (result) {
                    if ($('.chkIsSelected input:checked').length < 1)
                        showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                    else {
                        getCheckedMember();
                        onCustomButtonClick('save');
                    }
                }
            });
        });

        $(function () {
            //#region Service Unit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboDepartment.GetValue() + "'"; ;
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('0');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        });

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    cbpProcessDetail.PerformCallback('refresh');
            }, 0);
        }

        function getCheckedMember() {
            var lstRegistrationID = $('#<%=hdnSelectedRegistrationID.ClientID %>').val().split(',');
            var lstINAHakPasien = $('#<%=hdnSelectedINAHakPasien.ClientID %>').val().split(',');
            var lstINADitempati = $('#<%=hdnSelectedINADitempati.ClientID %>').val().split(',');

            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var keyField = $tr.find('.keyField').val();
                    var txtINAHakPasien = parseFloat(parseFloat($tr.find('.txtINAHakPasien').attr('hiddenVal')).toFixed(2));
                    var txtINADitempati = parseFloat(parseFloat($tr.find('.txtINADitempati').attr('hiddenVal')).toFixed(2));

                    var idx = lstRegistrationID.indexOf(keyField);
                    if (idx < 0) {
                        lstRegistrationID.push(keyField);
                        lstINAHakPasien.push(txtINAHakPasien);
                        lstINADitempati.push(txtINADitempati);
                    }
                    else {
                        lstRegistrationID[idx] = keyField;
                        lstINAHakPasien[idx] = txtINAHakPasien;
                        lstINADitempati[idx] = txtINADitempati;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstRegistrationID.indexOf(key);
                    if (idx > -1) {
                        lstRegistrationID.splice(idx, 1);
                        lstINAHakPasien.splice(idx, 1);
                        lstINADitempati.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedRegistrationID.ClientID %>').val(lstRegistrationID.join(','));
            $('#<%=hdnSelectedINAHakPasien.ClientID %>').val(lstINAHakPasien.join(','));
            $('#<%=hdnSelectedINADitempati.ClientID %>').val(lstINADitempati.join(','));
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked')) {
                $tr.find('.txtINAHakPasien').removeAttr('readonly');
                $tr.find('.txtINADitempati').removeAttr('readonly');
            }
            else {
                $tr.find('.txtINAHakPasien').attr('readonly', 'readonly');
                $tr.find('.txtINADitempati').attr('readonly', 'readonly');
            }
        });

        function onCbpProcessDetailEndCallback() {
            $('.txtINAHakPasien').each(function () {
                $(this).trigger('changeValue');
            });
            $('.txtINADitempati').each(function () {
                $(this).trigger('changeValue');
            });
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            cbpProcessDetail.PerformCallback('refresh');

            $('#<%=hdnSelectedRegistrationID.ClientID %>').val("");
            $('#<%=hdnSelectedINAHakPasien.ClientID %>').val("");
            $('#<%=hdnSelectedINADitempati.ClientID %>').val("");
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedINAHakPasien" runat="server" value="" />
    <input type="hidden" id="hdnSelectedINADitempati" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnQuickText" runat="server" value="" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 60%" />
                <col style="width: 40%" />
            </colgroup>
            <tr>
                <td>
                    <h4>
                        <%=GetLabel("Data Pencarian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 100px" />
                                <col style="width: 3px" />
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Periode Pendaftaran") %></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" Style="width: 120px" />
                                    <label>
                                        <%=GetLabel("s/d")%></label>
                                    <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" Style="width: 120px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                        Style="width: 100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <input type="hidden" id="hdnHealthcareServiceUnitID" value="0" runat="server" />
                                    <asp:TextBox ID="txtServiceUnitCode" runat="server" Style="width: 100%" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" runat="server" Style="width: 100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Status Pendaftaran")%></label>
                                </td>
                                <td align="left" colspan="2">
                                    <dxe:ASPxComboBox ID="cboVisitStatus" ClientInstanceName="cboVisitStatus" runat="server"
                                        Style="width: 100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td colspan="3">
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Watermark="Search" Width="100%">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No SEP" FieldName="NoSEP" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h4>
                        <%=GetLabel("Data Registrasi")%></h4>
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                            ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Registrasi")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl SEP")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                              <%=GetLabel("Informasi Registrasi")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Diagnosa Masuk")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Diagnosa Utama")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                         <%=GetLabel("Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Total Amount")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("INACBG Hak Pasien")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("INACBG Ditempati")%>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Remaining Amount")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl Registrasi")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No / Tgl SEP")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                             <%=GetLabel("Informasi Registrasi")%>
                                                        </th>
                                                        <th align="left" style="width: 250px">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Diagnosa Masuk")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Diagnosa Utama")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("Grouper")%>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Total Amount")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("INA Hak Pasien")%>
                                                        </th>
                                                        <th align="right" style="width: 150px">
                                                            <%=GetLabel("INA Ditempati")%>
                                                        </th>
                                                        <th align="right" style="width: 120px">
                                                            <%=GetLabel("Remaining Amount")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("RegistrationID")%>' />
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("RegistrationNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%#: Eval("cfRegistrationDateInString") %></label>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%#: Eval("BusinessPartnerName") %></label>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label>
                                                                <%#: Eval("NoSEP") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("cfTanggalSEPInString") %></label>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label>
                                                                <%#: Eval("DepartmentID") %>
                                                                -
                                                                <%#: Eval("ServiceUnitName") %></label></b>
                                                        <br />
                                                        <%#: Eval("ParamedicName") %>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Kelas Rawat : ")%></label>
                                                        <%#: Eval("ClassName") %>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Kelas Tagih : ")%></label>
                                                        <%#: Eval("ChargeClassName") %>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Hak Kelas : ")%></label>
                                                        <%#: Eval("NamaKelasTanggungan") %>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <label class="lblNormal">
                                                                <%#: Eval("MedicalNo") %></label></b>
                                                        <br />
                                                        <label class="lblNormal">
                                                            <%#: Eval("PatientName") %></label>
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Diagnosa Masuk (Dokter) : ")%></label>
                                                        <%#: Eval("DiagnosisMasuk") %>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Diagnosa Masuk (RM) : ")%></label>
                                                        <%#: Eval("FinalDiagnosisMasuk") %>
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Diagnosa (Dokter) : ")%></label>
                                                        <%#: Eval("Diagnosis") %>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Diagnosa (RM) : ")%></label>
                                                        <%#: Eval("FinalDiagnosis") %>
                                                        <br />
                                                        <label class="lblNormal" style="font-style: italic; font-size: smaller">
                                                            <%=GetLabel("Diagnosa (Klaim) : ")%></label>
                                                        <%#: Eval("ClaimDiagnosis") %>
                                                    </td>
                                                    <td style="width: 150px">
                                                        <b>
                                                           <%#: Eval("GrouperCodeClaim") %>
                                                          </b><br />
                                                           <label  style="font-style: italic; font-size: smaller">
                                                           <%#: Eval("GrouperTypeClaim") %>
                                                           </label>
                                                          
                                                      </td>
                                                    <td align="right">
                                                        <label style="color: Blue">
                                                            <%#: Eval("cfTotalLineAmountInString") %></label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtINAHakPasien" Width="90%" runat="server" ReadOnly="true" CssClass="txtINAHakPasien txtCurrency" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtINADitempati" Width="90%" runat="server" ReadOnly="true" CssClass="txtINADitempati txtCurrency" />
                                                    </td>
                                                    <td align="right">
                                                        <label style="color: Red">
                                                            <%#: Eval("cfINARemainingAmountInString") %></label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
        });
    </script>
</asp:Content>
