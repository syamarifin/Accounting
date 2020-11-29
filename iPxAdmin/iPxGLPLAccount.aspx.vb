Imports System.Data
Imports System.Data.SqlClient
Partial Class iPxAdmin_iPxGLPLAccount
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    'Dim sCnctPMS As String = ConfigurationManager.ConnectionStrings("iPxCNCTPMS").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    'Dim oCnctPMS As SqlConnection = New SqlConnection(sCnctPMS)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL, sSQLPMS As String
    Dim cIpx As New iPxClass
    Dim jobid As String
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='27'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddGLConf "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddGLConf").ToString = "Y" Then
                btnSave.Enabled = True
            Else
                btnSave.Enabled = False
            End If
        Else
            btnSave.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub showData()
        tbCoaAccount.Text = cIpx.getDefaultParameter(Session("sBusinessID"), "11")

        tbCoaClearance.Text = cIpx.getDefaultParameter(Session("sBusinessID"), "12")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then

            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "GL Configuration") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            showData()
        End If
        UserAcces()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        cIpx.setDefaultParam(Session("sBusinessID"), "11", "PL Account", tbCoaAccount.Text)
        cIpx.setDefaultParam(Session("sBusinessID"), "12", "PL Clearance Acount", tbCoaClearance.Text)
        
        Session("sMessage") = "Data has been save !| ||"
        Session("sWarningID") = "0"
        Session("sUrlOKONLY") = "iPxGLPLAccount.aspx"
        Session("sUrlYES") = "http://www.thepyxis.net"
        Session("sUrlNO") = "http://www.thepyxis.net"
        Response.Redirect("warningmsg.aspx")
    End Sub

    Protected Sub btnCxld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCxld.Click
        Response.Redirect("iPxGLTransaction.aspx")
    End Sub
End Class
