Imports System.Data
Imports System.Data.SqlClient

Partial Class iPxAdmin_SetupSMTP
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Dim cIpx As New iPxClass
    Dim jobid As String
    Public sSQL As String
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='34'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddOp "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddOp").ToString = "Y" Then
                Linkbutton1.Enabled = True
            Else
                Linkbutton1.Enabled = False
            End If
        Else
            Linkbutton1.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder4"), ContentPlaceHolder)
        Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        label.Text = "Setup SMTP"

        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "Option") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            getData()
        End If
        UserAcces()
    End Sub



    Protected Sub Linkbutton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Linkbutton1.Click
        If (txthost.Text = "") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please Fill Host');", True)
        ElseIf (tbpassword.Text <> tbpasswordConfirm.Text) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Reconfirm Password yang Anda Masukan Tidak Sama !');", True)
        Else
            updatedata()
        End If
    End Sub

    Sub getData()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_SMTP where businessid = '" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        'Host, EnableSsl, UserName, Password, Port, active iPxMBR_SMTP

        If oSQLReader.Read = True Then
            txthost.Text = oSQLReader.Item("Host").ToString.Trim
            drpEnableSsl.SelectedValue = oSQLReader.Item("EnableSsl").ToString.Trim
            txtusername.Text = oSQLReader.Item("UserName").ToString.Trim
            tbpassword.Text = oSQLReader.Item("Password").ToString.Trim
            txtport.Text = oSQLReader.Item("Port").ToString.Trim


            Dim act As String
            act = oSQLReader.Item("active").ToString.Trim

            If act = "Y" Then
                CheckBox1.Checked = True
            Else
                CheckBox1.Checked = False
            End If

        Else
            INSERT_SMTP()
        End If

        oCnct.Close()
    End Sub

    Sub INSERT_SMTP()
        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        'Host, EnableSsl, UserName, Password, Port, active
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO  iPxAcct_SMTP (businessid, Host, EnableSsl, UserName, Password, Port, active) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','','','','','','N') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub updatedata()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim active As String
        If CheckBox1.Checked Then
            active = "Y"
        Else
            active = "N"
        End If
        'Host, EnableSsl, , , Port, active iPxMBR_SMTP


        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcct_SMTP SET Host='" & txthost.Text & "', EnableSsl = '" & drpEnableSsl.SelectedValue & "', UserName = '" & txtusername.Text & "', Password = '" & tbpassword.Text & "', Port = '" & txtport.Text & "', active = '" & active & "'  where  businessid = '" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Session("sMessage") = "Data has been update !| ||"
        Session("sWarningID") = "0"
        Session("sUrlOKONLY") = "SetupSMTP.aspx"
        Session("sUrlYES") = "http://www.thepyxis.net"
        Session("sUrlNO") = "http://www.thepyxis.net"
        Response.Redirect("warningmsg.aspx")
    End Sub

  

    Protected Sub btnCxld_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCxld.Click
        Response.Redirect("home.aspx")
    End Sub


End Class
