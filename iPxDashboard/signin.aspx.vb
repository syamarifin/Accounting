Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxDashboard_signin
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim sActivationCode As String


    Protected Sub Logon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit.Click
        'If chkRememberMe.Checked Then
        '    writecookie()
        'End If


        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If



        sSQL = "Select * From iPx_admin_user Where  usercode = '" & txtUsername.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            If Trim(oSQLReader.Item("password")) = Trim(txtPassword.Text) Then
                'SELECT        TOP (200) id, firstname, lastname, email, password FROM            iPx_dashboard_admin
                Session("sEmailClient") = oSQLReader.Item("usercode")
                Session("sName") = oSQLReader.Item("fullname")
                Response.Redirect("home.aspx")
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Password Is Wrong !');document.getElementById('Buttonx').click()", True)
            End If

        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry User Is Not Registered Yet !');document.getElementById('Buttonx').click()", True)
        End If

        oCnct.Close()
LBL_X:

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        sActivationCode = If(Not String.IsNullOrEmpty(Request.QueryString("ActivationCode")), Request.QueryString("ActivationCode"), Guid.Empty.ToString())

        Session("lPass") = False

    End Sub

    Protected Sub btnRequestPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestPassword.Click
        oCnct.Open()

        sSQL = "Select * From iPx_admin_user Where usercode = '" & txtUsername.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            If SendEmail(Trim(oSQLReader.Item("fullname")), Trim(oSQLReader.Item("usercode")), Trim(oSQLReader.Item("password"))) Then
                MsgBox("Password has been sent to your email !", MsgBoxStyle.OkOnly)

            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Account Has Not Registered Yet !');document.getElementById('Buttonx').click()", True)
        End If
        oCnct.Close()
    End Sub

    Protected Function SendEmail(ByVal cUserName As String, ByVal cUsercode As String, ByVal cPassword As String) As Boolean

        Using mm As New MailMessage(ConfigurationManager.AppSettings("UserName"), cUsercode)
            mm.Subject = "Password Request"
            Dim body As String = "Hello " + cUserName + ","
            body += "<br /><br />Please find below detail of your account login"
            body += "<br /><br />   Username : " & cUsercode & vbCrLf
            body += "<br /><br />   Username : " & cPassword & vbCrLf

            body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("signup.aspx", Convert.ToString("signin.aspx")) + "'>Click here to login to your account.</a>"
            body += "<br /><br />Thanks"
            mm.Body = body
            mm.IsBodyHtml = True

            '=====================================================
            Dim smtp As New SmtpClient()
            smtp.Host = ConfigurationManager.AppSettings("Host")
            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings("EnableSsl"))
            Dim NetworkCred As New NetworkCredential(ConfigurationManager.AppSettings("UserName"), ConfigurationManager.AppSettings("Password"))
            smtp.UseDefaultCredentials = True
            smtp.Credentials = NetworkCred
            smtp.Port = Integer.Parse(ConfigurationManager.AppSettings("Port"))
            ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True

            smtp.Send(mm)
        End Using
        SendEmail = True
    End Function


End Class
