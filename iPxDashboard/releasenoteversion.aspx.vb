Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Partial Class iPxDashboard_releasenoteversion
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

            Response.Redirect("releasenotedtl.aspx")
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('" & cod & "');document.getElementById('Buttonx').click()", True)

        End If
    End Sub

    Public Sub grid()
        oCnct.Open()
        sSQL = "select * FROM  iPx_general_verGRP "


        Dim cmd As SqlDataAdapter = New SqlDataAdapter(sSQL, oCnct)
        Dim dt As DataTable = New DataTable()
        cmd.Fill(dt)

        GridView1.DataSource = dt
        GridView1.DataBind()

        oCnct.Close()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            grid()
        End If
    End Sub

    Protected Sub btnview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnview.Click
        Session("sCode") = ""
        Session("sNewCode") = True
        'Response.Redirect("SalesProfile.aspx")
    End Sub
End Class
