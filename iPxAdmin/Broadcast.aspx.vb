
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Imports System.Text
Imports System.Web
Imports System.Net.Mime
Imports System.Globalization
Imports System.IO

Partial Class iPxAdmin_Broadcast
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    Dim ciPx As New iPxClass
    Public Sub grid()
        oCnct.Open()
        Dim abc = Session("sBusinessID").ToString
        'Session("sCondition") = " AND (iPxMBR_profile_guestgeneral.Active='N') "
        sSQL = "SELECT  *,  day(specialdate) as tanggal , month(specialdate) as bulan"
        sSQL = sSQL & " FROM            iPxMBR_profile_guestgeneral where businessid='" & Session("sBusinessID") & "'"
        If Session("sCondition") <> "" Then
            sSQL = sSQL & Session("sCondition")
            Session("sCondition") = ""
        End If

        Dim cmd As SqlDataAdapter = New SqlDataAdapter(sSQL, oCnct)
        Dim dt As DataTable = New DataTable()
        cmd.Fill(dt)
        GridView1.DataSource = dt
        GridView1.DataBind()
        oCnct.Close()

    End Sub

    Public Function ExecuteQuery(ByVal cmd As SqlCommand, ByVal action As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
        Using con As New SqlConnection(conString)
            cmd.Connection = con
            Select Case action
                Case "SELECT"
                    Using sda As New SqlDataAdapter()
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            Return dt
                        End Using
                    End Using
                Case "UPDATE"
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                    Exit Select
            End Select
            Return Nothing
        End Using
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder4"), ContentPlaceHolder)
        Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        label.Text = "Broadcast"

        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If

        If Not Page.IsPostBack Then
            'If ciPx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "EV") <> True Then

            '    Session("sMessage") = "Sorry, you dont have access in this module |"
            '    Session("sMemberid") = ""
            '    Session("sWarningID") = "0"
            '    Session("sUrlOKONLY") = "home.aspx"
            '    Session("sUrlYES") = "http://www.thepyxis.net"
            '    Session("sUrlNO") = "http://www.thepyxis.net"
            '    Response.Redirect("warningmsg.aspx")
            'End If
            rbContent.Checked = True
            Panel2.Visible = False

            'grid()
            'showdata_TEMPLATE()
        End If

        imgHeader.ImageUrl = "Handler.ashx?ID=1|" & Session("sBusinessID") & "|" & drptemplate.Text & ""
        imgContent.ImageUrl = "Handler.ashx?ID=2|" & Session("sBusinessID") & "|" & drptemplate.Text & ""
        imgFooter.ImageUrl = "Handler.ashx?ID=3|" & Session("sBusinessID") & "|" & drptemplate.SelectedValue & ""

        If rbContent.Checked = True Then
            Panel1.Visible = True
        Else
            Panel2.Visible = False
        End If

    End Sub


    Sub UploadImage_header()
        If Trim(up_header.FileName) <> "" Then
            Dim buffer(up_header.PostedFile.ContentLength) As Byte
            Dim postFile As HttpPostedFile = up_header.PostedFile


            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE  iPxMBR_emailtemplate SET  "


            sSQL = sSQL & " imageHeader=@filephoto WHERE id = '" & drptemplate.SelectedValue & "' and businessid='" & Session("sBusinessID") & "' "
            'sSQL = "UPDATE       iPxMBR_profile_imagelib SET  imagedata=@filephoto where imagecode='01'"

            postFile.InputStream.Read(buffer, 0, CInt(up_header.PostedFile.ContentLength))
            Dim imgLogo As New SqlParameter("@filephoto", SqlDbType.Image, buffer.Length)
            imgLogo.Value = buffer

            oSQLCmd.Parameters.Add(imgLogo)


            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
        End If

    End Sub

    Sub UploadImage_Content()
        If Trim(up_content.FileName) <> "" Then
            Dim buffer(up_content.PostedFile.ContentLength) As Byte
            Dim postFile As HttpPostedFile = up_content.PostedFile


            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE  iPxMBR_emailtemplate SET  "


            sSQL = sSQL & " imageContent =@filephoto WHERE id = '" & drptemplate.SelectedValue & "' and businessid='" & Session("sBusinessID") & "' "
            'sSQL = "UPDATE       iPxMBR_profile_imagelib SET  imagedata=@filephoto where imagecode='01'"

            postFile.InputStream.Read(buffer, 0, CInt(up_content.PostedFile.ContentLength))
            Dim imgLogo As New SqlParameter("@filephoto", SqlDbType.Image, buffer.Length)
            imgLogo.Value = buffer

            oSQLCmd.Parameters.Add(imgLogo)


            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
        End If

    End Sub

    Sub UploadImage_Footer()
        If Trim(up_footer.FileName) <> "" Then
            Dim buffer(up_footer.PostedFile.ContentLength) As Byte
            Dim postFile As HttpPostedFile = up_footer.PostedFile


            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE  iPxMBR_emailtemplate SET  "


            sSQL = sSQL & " imageFooter =@filephoto WHERE id = '" & drptemplate.SelectedValue & "' and businessid='" & Session("sBusinessID") & "' "
            'sSQL = "UPDATE       iPxMBR_profile_imagelib SET  imagedata=@filephoto where imagecode='01'"

            postFile.InputStream.Read(buffer, 0, CInt(up_footer.PostedFile.ContentLength))
            Dim imgLogo As New SqlParameter("@filephoto", SqlDbType.Image, buffer.Length)
            imgLogo.Value = buffer

            oSQLCmd.Parameters.Add(imgLogo)


            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
        End If

    End Sub


    Protected Function SendEmail() As Boolean
        Try
            Dim aRet() As String
            Dim host, enablessl, username, password, port, active As String

            aRet = Split(ciPx.GetSMTP(Session("sBusinessID")), "|") 'parameter dan pemisah
            active = aRet(0)
            If active = "Y" Then
                host = aRet(1)
                enablessl = aRet(2)
                username = aRet(3)
                password = aRet(4)
                port = aRet(5)
            Else
                host = ConfigurationManager.AppSettings("Host")
                enablessl = ConfigurationManager.AppSettings("EnableSsl")
                username = ConfigurationManager.AppSettings("UserName")
                password = ConfigurationManager.AppSettings("Password")
                port = ConfigurationManager.AppSettings("Port")
            End If

            Dim mm As New MailMessage(username, Session("sEmailMember"))
            Dim body As String


            body = File.ReadAllText(Server.MapPath("~/iPxEmailThemplate/emailbroadcast.html"))
            Dim urlheader, urlfooter, urlcontent, imgHDR, imgfooter, imgcontent As String

            imgHDR = "http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=1|" & Session("sBusinessID") & "|" & drptemplate.Text & ""
            imgfooter = "http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=3|" & Session("sBusinessID") & "|" & drptemplate.Text & ""
            imgcontent = "http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=2|" & Session("sBusinessID") & "|" & drptemplate.Text & ""

            urlheader = "<img src=""" & imgHDR & """  width=""100%"" />"
            urlfooter = "<img src=""" & imgfooter & """ width=""100%"" />"
            urlcontent = "<img src=""" & imgcontent & """ width=""100%"" />"

            Dim htmlstr, subject As String
            subject = txtSubject.Text
            htmlstr = txtContentEmail.Text.Replace(vbCrLf, "<br/>")

            body = body.Replace("{imageLogo}", urlheader)
            body = body.Replace("{imageFooter}", urlfooter)
            If rbImgContent.Checked = True Then 'if image content
                body = body.Replace("{ipx_emailbody}", urlcontent)

            Else 'if content=text
                body = body.Replace("{ipx_emailbody}", Session("sEmailTitle") & htmlstr)
                body = body.Replace("'", "`") 'replace petik jadi petik kebalik
            End If


            Dim fromEmail As String = username
            mm.[To].Add(Session("sEmailMember"))
            mm.From = New MailAddress(ConfigurationManager.AppSettings("from"), ConfigurationManager.AppSettings("website"))
            mm.Subject = subject

            mm.Body = body
            mm.IsBodyHtml = True
            Dim smtp As SmtpClient = New SmtpClient()
            smtp.Host = host
            smtp.EnableSsl = enablessl
            Dim NetworkCred As NetworkCredential = New NetworkCredential()
            NetworkCred.UserName = username
            NetworkCred.Password = password
            smtp.UseDefaultCredentials = True
            smtp.Credentials = NetworkCred
            smtp.Port = Integer.Parse(port)
            smtp.Send(mm)


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Email can't send!');", True)

        End Try

    End Function

    Protected Sub btnSendEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendEmail.Click
        If chkconfirm.Checked = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please check Agree!');", True)

        End If

        If chkconfirm.Checked = True Then


            For Each row As GridViewRow In GridView1.Rows
                Session("sEmailMember") = row.Cells(0).Text.Trim

                Session("sEmailTitle") = "Dear " & row.Cells(3).Text.Trim & "." & row.Cells(1).Text.Trim & " " & row.Cells(2).Text.Trim & "," & "<br/><br/>"
                SendEmail()


            Next
            Session("sMessage") = "Email has been sent !| ||"
            Session("sWarningID") = "0"
            Session("sUrlOKONLY") = "Broadcast.aspx"
            Session("sUrlYES") = "http://www.thepyxis.net"
            Session("sUrlNO") = "http://www.thepyxis.net"
            Response.Redirect("warningmsg.aspx")
        End If
    End Sub

    Protected Sub radAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radAll.CheckedChanged
        If radAll.Checked = True Then
            Session("sCondition") = ""
            grid()

        End If

    End Sub

    Protected Sub radMember_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radMember.CheckedChanged
        
        If radMember.Checked = True Then
            Session("sCondition") = " AND (iPxMBR_profile_guestgeneral.profilegroup ='M') "
        End If
        grid()
    End Sub

    Protected Sub radMemActv_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radMemActv.CheckedChanged
        
        If radMemActv.Checked = True Then
            Session("sCondition") = "AND (iPxMBR_profile_guestgeneral.profilegroup ='M' and iPxMBR_profile_guestgeneral.Active='Y')"
        End If
        grid()
    End Sub

    Protected Sub radProsCst_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radProsCst.CheckedChanged

        If radProsCst.Checked = True Then
            Session("sCondition") = " AND (iPxMBR_profile_guestgeneral.profilegroup ='N')"
        End If
        grid()
    End Sub

    Protected Sub btnAddTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTemplate.Click
        If txttemplate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please fill Template!');", True)

        Else
            savedata()

            showdata_TEMPLATE()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been save!');", True)

        End If

    End Sub

    Sub savedata()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        'SELECT   businessid, tempid, tempdesc, description, imageContent, imageHeader, imageFooter, type, active, subject FROM  iPxMBR_emailtemplate
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT   INTO iPxMBR_emailtemplate( businessid,  tempdesc,  active) VALUES('" & Session("sBusinessID") & "', '" & txttemplate.Text & "', 'Y' )"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub savedatacontent()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxMBR_emailtemplate SET subject='" & txtSubject.Text & "', description ='" & txtContentEmail.Text & "' where  id='" & drptemplate.Text & "' "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub


    Sub showdata_TEMPLATE()
        Dim cmd As New SqlCommand("select * from iPxMBR_emailtemplate where businessid='" & Session("sBusinessID") & "' and tempid is null  ")
        drptemplate.DataSource = ExecuteQuery(cmd, "SELECT")
        drptemplate.DataTextField = "tempdesc"
        drptemplate.DataValueField = "id"
        drptemplate.DataBind()
        drptemplate.Items.Insert(0, "-Select-")
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        If drptemplate.SelectedIndex = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please select your Template!');", True)
        Else

            savedatacontent()
            UploadImage_header()
            UploadImage_Content()
            UploadImage_Footer()

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been save!');", True)
        End If
    End Sub

    Sub getdatatemplate()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select businessid, tempid, tempdesc, isnull(description,'-') as description, imageContent, imageHeader, imageFooter, active, isnull(subject,'Alcor Loyalty Management') as subject  from  iPxMBR_emailtemplate where businessid='" & Session("sBusinessID") & "' and id='" & drptemplate.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            txtContentEmail.Text = oSQLReader.Item("description")
            txtSubject.Text = oSQLReader.Item("subject")
            imgHeader.ImageUrl = "Handler.ashx?ID=1|" & Session("sBusinessID") & "|" & drptemplate.Text & ""
            imgContent.ImageUrl = "Handler.ashx?ID=2|" & Session("sBusinessID") & "|" & drptemplate.Text & ""
            imgFooter.ImageUrl = "Handler.ashx?ID=3|" & Session("sBusinessID") & "|" & drptemplate.SelectedValue & ""

        End If
        oCnct.Close()
    End Sub

    Protected Sub drptemplate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drptemplate.SelectedIndexChanged
        If drptemplate.SelectedIndex = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please select your template or Add new template!');", True)
        Else
            getdatatemplate()
        End If
    End Sub

    Protected Sub rbContent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbContent.CheckedChanged
        Panel2.Visible = False
        Panel1.Visible = True
    End Sub

    Protected Sub rbImgContent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbImgContent.CheckedChanged
        Panel1.Visible = False
        Panel2.Visible = True
    End Sub

    Protected Sub delHDR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delHDR.Click
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "  UPDATE  iPxMBR_emailtemplate SET  imageHeader = NULL where  businessid ='" & Session("sBusinessID") & "' and id='" & drptemplate.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        getdatatemplate()
    End Sub

    Protected Sub delFTR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delFTR.Click
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "  UPDATE  iPxMBR_emailtemplate SET imageFooter  = NULL where  businessid ='" & Session("sBusinessID") & "' and id='" & drptemplate.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        getdatatemplate()
    End Sub


    Protected Sub delCNTN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delCNTN.Click
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "  UPDATE  iPxMBR_emailtemplate SET  imageContent = NULL where  businessid ='" & Session("sBusinessID") & "' and id='" & drptemplate.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        getdatatemplate()
    End Sub

    Protected Sub radBirth_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radBirth.CheckedChanged
        If radBirth.Checked = True Then
            Session("sCondition") = " AND ( day(iPxMBR_profile_guestgeneral.specialdate)=day( GETDATE()) and month(iPxMBR_profile_guestgeneral.specialdate)= month( GETDATE()))"
        End If
        grid()
    End Sub

    Protected Sub btnConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfig.Click
        Response.Redirect("viewConfig.aspx")
    End Sub
End Class

