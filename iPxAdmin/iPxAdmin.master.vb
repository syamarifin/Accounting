
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Partial Class iPXADMIN_iPxAdmin
    Inherits System.Web.UI.MasterPage

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' System.Threading.Thread.Sleep(100)
        lblUser.Text = Session("sUserName")
        Session("sCondition") = ""
        checkversion()


        'Dim PR, RD, TR As Integer

        'PR = lblpointreward.Text
        'RD = lblredeem.Text
        'TR = lblTotalRequest.Text

        'lbltotalTrans.Text = PR + RD + TR

    End Sub

    Sub checkversion()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT version, description FROM iPx_general_ver WHERE id = (SELECT max(id) FROM iPx_general_ver) "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        If oSQLReader.Read Then
            lbVersion.Text = oSQLReader.Item("version") + " " + oSQLReader.Item("description")
        End If
        oSQLReader.Close()
        oCnct.Close()
    End Sub


   

  


    Sub removecookie()
        'Fetch the Cookie using its Key.
        Dim userCookie As HttpCookie = Request.Cookies("user")
        Dim passCookie As HttpCookie = Request.Cookies("pass")
        'Set the Expiry date to past date.
        If userCookie Is Nothing Then

        Else
            userCookie.Expires = DateTime.Now.AddDays(-1)
            passCookie.Expires = DateTime.Now.AddDays(-1)
            'Update the Cookie in Browser.
            Response.Cookies.Add(userCookie)
            Response.Cookies.Add(passCookie)
        End If
    End Sub

    Protected Sub btnlogout_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnlogout.ServerClick
        Session("sUserLogon") = ""
        Session("sBusinessID") = ""
        removecookie()
        Response.Redirect("logon.aspx")
    End Sub

    Protected Sub btnmenulogout_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmenulogout.ServerClick
        Session("sUserLogon") = ""
        Session("sBusinessID") = ""
        removecookie()
        Response.Redirect("logon.aspx")
    End Sub
End Class

