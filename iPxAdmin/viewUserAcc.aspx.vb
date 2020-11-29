
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class iPxAdmin_viewUserAcc
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    Dim ciPx As New iPxClass
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

        For Each row As GridViewRow In GridView1.Rows

            If row.RowIndex = GridView1.SelectedIndex Then
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
            Else
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Click to select this row."
            End If
        Next

    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "getcode" Then
            Dim cod As String
            cod = e.CommandArgument.ToString
            Session("sCode") = cod
            Session("sNewCode") = False
            Response.Redirect("UserAccess.aspx")
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('" & cod & "');document.getElementById('Buttonx').click()", True)

        End If
    End Sub

    Public Sub grid()
        oCnct.Open()

        sSQL = "SELECT * FROM iPx_profile_client_userid where businessid= '" & Session("sBusinessID") & "'  "
        If Session("sCondition") <> "" Then
            sSQL = sSQL & Session("sCondition")
            Session("sCondition") = ""
        Else
            sSQL = sSQL & ""
        End If

        Dim cmd As SqlDataAdapter = New SqlDataAdapter(sSQL, oCnct)
        Dim dt As DataTable = New DataTable()
        cmd.Fill(dt)

        GridView1.DataSource = dt
        GridView1.DataBind()

        oCnct.Close()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder4"), ContentPlaceHolder)
        Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        label.Text = "User Access Profile"

        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then

            'If ciPx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "CN") <> True Then

            '    Session("sMessage") = "Sorry, you dont have access in this module |"
            '    Session("sMemberid") = ""
            '    Session("sWarningID") = "0"
            '    Session("sUrlOKONLY") = "home.aspx"
            '    Session("sUrlYES") = "http://www.thepyxis.net"
            '    Session("sUrlNO") = "http://www.thepyxis.net"
            '    Response.Redirect("warningmsg.aspx")
            'End If
            grid()
        End If

    End Sub

    Protected Sub btnview_Click(sender As Object, e As EventArgs) Handles btnview.Click
        Session("sCode") = ""
        Session("sNewCode") = True
        Response.Redirect("UserAccess.aspx")
    End Sub


    Protected Sub btnfilter_Click(sender As Object, e As EventArgs) Handles btnfilter.Click
        Response.Redirect("queryUserAcc.aspx")
    End Sub
End Class
