<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MCUResultFormEntryDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MCU.Program.MCUResultFormEntryDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxis_mcu1" src='<%= ResolveUrl("~/Libs/Scripts/McuForm/MCUFormRSRT.js")%>'></script>
<script type="text/javascript" id="dxss_MCUResultFormEntryDetailCtl">

    $(function () {
        if ($('#<%=hdnFormValueCtl.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValueCtl.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0]) {
                        $(this).val(temp[1]);
                    }

                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }

        if ($('#<%=hdnIDCtl.ClientID %>').val() == "") {
          
            //openDefault();
        } else {
            //editValue();
        }
    });
  
    function onBeforeSaveRecordEntryPopup() {
        var values = getFormValues();
        var resultValue = getFormResultValues();
        return true;
    }

    function getFormValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=hdnFormValueCtl.ClientID %>').val(controlValues);

        return controlValues;
    }

    function getFormResultValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += "|";
            controlValues += $(this).attr('sortid')+ "^" + $(this).attr('labelname') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += "|";
            controlValues += $(this).attr('sortid')+ "^" + $(this).attr('labelname') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            var value = "0";
            var tag_value = "";
            if (controlValues != '')
                controlValues += "|";
            if ($(this).is(':checked')) {
                tag_value = $(this).attr('tagvalue');
            } else {
                tag_value = "Tidak";
            }
            controlValues += $(this).attr('sortid') + "^" + $(this).attr('labelname') + "=" + tag_value;
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
             
            var tag_value = "";
            if (controlValues != '')
                controlValues += "|";
            if ($(this).is(':checked')) {
                tag_value = $(this).attr('tagvalue');
                controlValues += $(this).attr('sortid') + "^" + $(this).attr('labelname') + "=" + tag_value;
            }
           
        });
        $('#<%=hdnFormResultValueCtl.ClientID %>').val(controlValues);

        return controlValues;
    }
    $(".txtForm").change(function () {

        //BMI kebutuhan royal
        if ($(this).attr('controlID') == 'txtParam24') {
            BMICalNew();
        }
        else if ($(this).attr('controlID') == 'txtParam25') {
            BMICalNew();
        }
        else if ($(this).attr('controlID') == 'txtParam26') {
            BMICalNew();
        }
    });


    function BMICalNew() {
        var tinggiBadan = document.getElementById("txtParam24").value;
        var beratBadan = document.getElementById("txtParam25").value;
      
        var bmi = 0;
        var anjuranBeratBadan = 0;
        if (tinggiBadan != "" && beratBadan != "" && tinggiBadan != "0" && beratBadan != "0") {
            if (tinggiBadan > 160) {
                anjuranBeratBadan = (parseFloat(tinggiBadan) - 100) - (0.1 * (parseFloat(tinggiBadan) - 100));
            } else {
                anjuranBeratBadan = (parseFloat(tinggiBadan) - 100);
            }
            if (tinggiBadan > 150 && tinggiBadan <= 160) {
                anjuranBeratBadan = (parseFloat(tinggiBadan) - 100) - (0.1 * (parseFloat(tinggiBadan) - 100));
            } else {
                anjuranBeratBadan = (parseFloat(tinggiBadan) - 100);
            }
            var tinggi = Math.pow(parseFloat(tinggiBadan) / 100, 2);
            bmi = parseFloat(beratBadan) / tinggi;
            document.getElementById("txtParam26").value = anjuranBeratBadan;
            if (parseFloat(bmi.toFixed(2)) < 18.50) {
                document.getElementById("ddlParameter23").value = "UnderWeight";
            } else if (parseFloat(bmi.toFixed(2)) >= 18.50 && parseFloat(bmi.toFixed(2)) <= 24.90) {
                document.getElementById("ddlParameter23").value = "Normal";
            } else if (parseFloat(bmi.toFixed(2)) > 24.90 && parseFloat(bmi.toFixed(2)) <= 29.90) {
                document.getElementById("ddlParameter23").value = "OverWeight";
            } else if (parseFloat(bmi.toFixed(2)) > 29.90) {
                document.getElementById("ddlParameter23").value = "Obese";
            }
            document.getElementById("txtParam27").value = bmi.toFixed(2);
        }
    }

    var initial = $('#<%=hdnHealthcareInitialID.ClientID %>').val();
    if (initial == "RSRT") {
        $("#ddlParameter59").on('change', function () { //5. Tonus Otot
            var val = this.value;
            if (val != "Abnormal") {
                $("#ddlParameter510").val("").change(); ;
            }

        });
    }

</script>
<style type="text/css">
    #ulVitalSign
    {
        margin: 0;
        padding: 0;
    }
    #ulVitalSign li
    {
        list-style-type: none;
        display: inline-block;
        margin-right: 10px;
    }
    .tdVitalSignHeader
    {
        padding: 1px;
        background-color: #09abd2;
        color: Black;
        border: 1px solid gray;
        text-align: center;
    }
</style>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnHealthcareInitialID"  value="" />
    <input type="hidden" runat="server" id="hdnIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnGCResultTypeCtl" value="" />
    <input type="hidden" runat="server" id="hdnDivHTMLCtl" value="" />
    <input type="hidden" runat="server" id="hdnFormValueCtl" value="" />
      <input type="hidden" runat="server" id="hdnFormResultValueCtl" value="" />
    <table class="tblContentArea" border="0">
        <tr>
            <td colspan="2">
                <hr style="margin: 0 0 0 0;" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <label class="lblNormal" id="lblTitle" runat="server" style="font-size: large; font-style: oblique;
                    text-decoration: blink">
                </label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr style="margin: 0 0 0 0;" />
            </td>
        </tr>
        <tr style="display:none">
            <td>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divFormContent" runat="server" style="height: 650px; overflow-y: auto;">
                </div>
            </td>
        </tr>
    </table>
</div>
