Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration

Partial Class iPxMaster_MasterPage
    Inherits System.Web.UI.MasterPage
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String

    Sub logout()
        Dim userCookie As HttpCookie = Request.Cookies("cUser")
        Dim passCookie As HttpCookie = Request.Cookies("cPass")

        If userCookie Is Nothing Then

        Else
            userCookie.Expires = DateTime.Now.AddDays(-1)
            passCookie.Expires = DateTime.Now.AddDays(-1)
            Response.Cookies.Add(userCookie)
            Response.Cookies.Add(passCookie)
        End If

        Session.Clear()
        Response.Redirect("../iPxUser/signin.aspx")
    End Sub

    Protected Sub btnLogoutheader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogoutheader.Click
        Session("sUserLogon") = ""
        Session("sBusinessID") = ""
        removecookie()
        Response.Redirect("logon.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        Else
            lblUser.Text = Session("sUserName")
            Session("sCondition") = ""
            lblValid.Text = "Valid Until " & Format(Session("sValidUntil"), "dd MMM yyyy")
            checkversion()
        End If
    End Sub

    Sub checkversion()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT version, description FROM iPx_general_ver WHERE id = (SELECT max(id) FROM iPx_general_ver) and appid='5' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        If oSQLReader.Read Then
            lbVersion.Text = oSQLReader.Item("version") + " " + oSQLReader.Item("description")
        End If
        oSQLReader.Close()
        oCnct.Close()
    End Sub

    Sub getUserimage()
        'If oCnct.State = ConnectionState.Closed Then
        '    oCnct.Open()
        'End If

        'oSQLCmd = New SqlCommand(sSQL, oCnct)
        'sSQL = "SELECT userimage, usercode FROM iPx_profile_client_userid WHERE businessid = '" & Session("sBusinessid") & "' AND usercode = '" & Session("sUsercode") & "' "
        'oSQLCmd.CommandText = sSQL
        'oSQLReader = oSQLCmd.ExecuteReader
        'oSQLReader.Read()

        'If Not IsDBNull(oSQLReader.Item("userimage")) Then
        '    Userimage.ImageUrl = "Handler.ashx?ID=0|" & oSQLReader.Item("usercode") & "|" & Session("sBusinessid") & ""
        'Else
        '    Userimage.ImageUrl = "../assets/images/icon/user.png"
        'End If
        'Userimage.Width = 75
        'Userimage.Height = 75

        'oCnct.Close()
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
    Protected Sub btnmenulogout_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmenulogout.ServerClick
        Session("sUserLogon") = ""
        Session("sBusinessID") = ""
        removecookie()
        Response.Redirect("logon.aspx")
    End Sub
End Class

