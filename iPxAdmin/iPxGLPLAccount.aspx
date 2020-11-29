<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLPLAccount.aspx.vb" Inherits="iPxAdmin_iPxGLPLAccount" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="form-group">
        <table class="table table-bordered">
            <tr>
                <td>
		            <label for="" >PL Account</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <div class="input-group">
                                <asp:TextBox ID="tbCoaAccount" runat="server" CssClass ="form-control"></asp:TextBox>
                                <div class="input-group-btn">
                                    <asp:LinkButton ID="lbFindCoaAccount" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-search" style="height:20px;"></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
		        </td>
            </tr>
            <tr>
                <td>
		            <label for="" >PL Clearance Account</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <div class="input-group">
                                <asp:TextBox ID="tbCoaClearance" runat="server" CssClass ="form-control"></asp:TextBox>
                                <div class="input-group-btn">
                                    <asp:LinkButton ID="lbFindCoaClearance" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-search" style="height:20px;"></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
		        </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <div id="footer"  >
        <asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnSave" runat="server" Enabled="true"><span class="fa fa-save"></span> Save</asp:linkbutton>    
        <asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>
    </div>
</asp:Content>

