<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoDocumentCustomerContentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Information.InfoDocumentCustomerContentCtl" %>

<style type="text/css">
    .templateDocumentContract
    {
        font-size:12px;
    }
    .templateDocumentContract .containerDocumentContractImage
    {
        float:left;
        display: table-cell;
        vertical-align:middle;
        border: 1px solid #9C9898;
        width:200px;
        height:200px;
        margin-right: 20px;
        text-align:center;
        position:relative;
    }
    .templateDocumentContract .containerDocumentContractImage img
    {
        max-height:200px;
        max-width:200px;
        position:absolute;top:0;bottom:0;left:0;right:0;margin:auto;
    }
</style>
<script>

    $('#lblView').click(function () {
        var file = $('#<%=hdnURLFile.ClientID %>').val();
        window.open(file, '_blank');
    });
</script>
<div id="DocumentContractContent" style="width:100%;">
    <div class="templateDocumentContract">
        <input type="hidden" id="hdnID" runat="server" value='' />  
        <input type="hidden" id="hdnURLFile" runat="server" value="" />
        <input type="hidden" id="hdnPayerID" runat="server" value='' />  
        <input type="hidden" id="hdnContractID" runat="server" value='' />  
        <input type="hidden" id="hdnImagFormat" runat="server" value='JPG|BMP|GIF|PNG' />
        <table style="width:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:200px" valign="top">                    
                    <div class="containerDocumentContractImage boxShadow">
                        <img alt="" id="imgContractImage" runat="server" />  
                        
                    </div>
                   <div style="margin-top:10px">
                        <center>
                            <label id="lblView" class="lblLink"><b>[Klik Disini]</b></label>
                        </center>
                   </div>
                </td>
                <td valign="top">
                    <table style="width:100%">
                        <colgroup style="width:40px"/>
                        <colgroup style="width:5px"/>
                        <tr>
                            <td><%=GetLabel("Nama File") %></td>
                            <td>:</td>
                            <td><span class="spValue" id="spnFileName" runat="server"></span></td>
                        </tr>
                        <tr>
                            <td><%=GetLabel("Tipe")%></td>
                            <td>:</td>
                            <td><span class="spValue" id="spnDocumentType" runat="server"></span></td>
                        </tr>
                        <tr>
                            <td valign="top"><%=GetLabel("Keterangan")%></td>
                            <td valign="top">:</td>
                            <td valign="top"><span class="spValue" id="spnRemarks" runat="server"></span></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
