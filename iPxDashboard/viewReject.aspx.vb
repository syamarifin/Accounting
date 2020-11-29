
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class iPxDashboard_viewReject
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
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

    Public Sub grid()
        oCnct.Open()

        sSQL = "select * from iPx_profile_user_signup where status ='X' and registerfor = '26' order by registerdate desc "

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
        label.Text = "Reject"

        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            grid()
        End If

    End Sub


    Protected Sub btnAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAbort.Click
        Response.Redirect("viewRequest.aspx")
    End Sub
End Class
