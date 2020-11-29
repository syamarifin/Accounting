<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="releaseversion.aspx.vb" Inherits="iPxCrmUser_releaseversion" title="Alcor Accounting Version" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        #accordion .panel {
            border: medium none;
            border-radius: 0;
            box-shadow: none;
            margin: 0 0 15px 10px;
        }
        #accordion .panel-heading {
            border-radius: 30px;
            padding: 0;
            margin-bottom : 10px;
        }
        #accordion .panel-title a {
            background: #36b3c1 none repeat scroll 0 0;
            border: 1px solid transparent;
            border-radius: 30px;
            color: #fff;
            display: block;
            font-size: 18px;
            font-weight: 600;
            padding: 12px 20px 12px 50px;
            position: relative;
            transition: all 0.3s ease 0s;
        }
        #accordion .panel-title a.collapsed {
            background: #fff none repeat scroll 0 0;
            border: 1px solid #ddd;
            color: #333;
        }
        #accordion .panel-title a::after, #accordion .panel-title a.collapsed::after {
            background: #31a0ad none repeat scroll 0 0;
            border: 1px solid transparent;
            border-radius: 50%;
            box-shadow: 0 3px 10px rgba(0, 0, 0, 0.58);
            color: #fff;
            content: "";
            font-family: fontawesome;
            font-size: 20px;
            height: 50px;
            left: -20px;
            line-height: 50px;
            position: absolute;
            text-align: center;
            top: -5px;
            transition: all 0.3s ease 0s;
            width: 50px;
        }
        #accordion .panel-title a.collapsed::after {
            background: #fff none repeat scroll 0 0;
            border: 1px solid #ddd;
            box-shadow: none;
            color: #333;
            content: "";
        }
        #accordion .panel-body {
            background: transparent none repeat scroll 0 0;
            border-top: medium none;
            padding: 0px 25px 10px 9px;
            position: relative;
            font-size:x-small ;
        }
        #accordion .panel-body p {
            border-left: 1px dashed #8c8c8c;
            padding-left: 10px;
        }
        <%--===============================================================================--%>
        ul.timeline {
            list-style-type: none;
            position: relative;
        }
        ul.timeline:before {
            content: ' ';
            background: #d4d9df;
            display: inline-block;
            position: absolute;
            left: 29px;
            width: 2px;
            height: 100%;
            z-index: 400;
        }
        ul.timeline > li {
            padding-left: 20px;
        }
        ul.timeline > li:before {
            content: ' ';
            background: white;
            display: inline-block;
            position: absolute;
            border-radius: 50%;
            border: 3px solid #22c0e8;
            left: 20px;
            width: 20px;
            height: 20px;
            z-index: 400;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        (function($) {
            'use strict';

            jQuery(document).on('ready', function() {

                $('a.page-scroll').on('click', function(e) {
                    var anchor = $(this);
                    $('html, body').stop().animate({
                        scrollTop: $(anchor.attr('href')).offset().top - 50
                    }, 1500);
                    e.preventDefault();
                });
            });


        })(jQuery);
    </script>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-sm-12">
    
    <div style="font-weight:bold;font-size:12pt">Release Version</div>
    <div id="dvAccordian" class="panel-group" style="width: 100%;">
        <asp:GridView ID="gvReleaseVersion" runat="server" AutoGenerateColumns="false" GridLines="None" style="width: 100%;"
            DataKeyNames="VID" OnRowDataBound="OnRowDataBound">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <div id="accordion" class="panel panel-default">
                        <div class="panel-heading" role="tab" id="heading<%#Eval("id")%>">
                            <h4 class="panel-title">
                                <a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#dvAccordian"
                                    href="#collapse<%#Eval("id")%>" aria-controls="collapse<%#Eval("id")%>">
                                    <asp:Label ID="lbTitle" runat="server" Text='<%# "versi "+ Eval("verid")%>' Font-Size="Small"></asp:Label>
                                </a> 
                            </h4>
                        </div>
                        <div id="collapse<%#Eval("id")%>" class="panel-collapse collapse" role="tabpanel" aria-expanded="false" aria-labelledby="heading<%#Eval("id")%>">
                            <div class="panel-body">
				                <div style="padding-left: 20px;">
				                    <asp:GridView ID="gvRelease" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="False">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="15%" ItemStyle-Font-Bold="true" HeaderText="" ItemStyle-HorizontalAlign="left">
                                                <ItemTemplate>
                                                    <ul class="timeline" style="margin :0px;">
				                                        <li style="margin :0px;">
    				                                        <div style="padding-left: 25px;">
					                                            <div style="color:#1e90ff;"><%#Eval("description")%></div>
					                                            <p>
                                                                    <%#"- " + Replace(Eval("note"), vbLf, "<br />- ")%>
                                                                </p>
                                                            </div>
				                                        </li>
			                                        </ul>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <%--<asp:Repeater ID="rptAccordian" runat="server">
            <ItemTemplate>
                <div>
                    
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>--%>
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

