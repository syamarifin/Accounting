
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

Partial Class iPxDashboard_UserAccess
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    Dim cIpx As New iPxClass

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
        label.Text = "User Client Access"

        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then


            showdata_dropdowncountry()

            If Session("sNewCode") = True Then
                txtusercode.Text = ""
                txtmobile.Text = ""
                txtbsname.Text = ""


            Else
                txtusercode.Text = Session("sEmailClient")
                getData()
                getDataClient()
                guestphoto.ImageUrl = "Handler.ashx?ID=0|" & Session("sBusinessID") & ""

            End If

            If Session("sRequest") = True Then
                getRequest()
            End If
        End If

    End Sub

    Sub getRequest()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM   iPx_profile_user_signup where email='" & Session("sEmailClient").ToString.Trim & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        txtusercode.ReadOnly = True
        While oSQLReader.Read
            'SELECT        id, , noofroom, , , , password, , , registerfor, registerdate, status, clerknotes, guestnotes, approvalnotes, approvaldate()
            txtusercode.Text = oSQLReader.Item("email").ToString.Trim
            txtmobile.Text = oSQLReader.Item("mobile").ToString.Trim
            txtbsname.Text = oSQLReader.Item("hotelname").ToString.Trim
            txtaddrs.Text = oSQLReader.Item("address").ToString.Trim
            txttaxno.Text = ""
            txtphone.Text = oSQLReader.Item("mobile").ToString.Trim
            txtfax.Text = ""
            txtconpers.Text = oSQLReader.Item("contactperson").ToString.Trim
            txtweb.Text = oSQLReader.Item("website").ToString.Trim

            showdata_dropdowncountry()
            'drpCountry.SelectedValue = oSQLReader.Item("country").ToString.Trim
            showdata_dropdowncity()
            'drpCity.SelectedValue = oSQLReader.Item("city").ToString.Trim

        End While
    End Sub

    Function update(ByVal status As String) As String
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPx_profile_user_signup SET status= '" & status & "' , approvaldate='" & Date.Today & "' where email='" & Session("sEmailClient") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        update = ""
    End Function

    Protected Function SendEmail() As Boolean

        Dim mm As New MailMessage(ConfigurationManager.AppSettings("UserName"), txtusercode.Text)
        Dim body, cBody As String

        cBody = "Hello " + txtbsname.Text + ","

        cBody += "<br/>Your userid/email account:"


        cBody += "<br/>Email  : <strong>" + txtusercode.Text + "</strong> "
        cBody += "<br/>Password : <strong>" + Session("sPasswordCl") + "</strong>"
        cBody += "<br /><h4><a href = 'accounting.alcorsys.com/ipxadmin/signin.aspx'>Click here to login.</a></h4>"
        cBody += "<br /> or visit http://accounting.alcorsys.com/ipxadmin/signin.aspx "

        cBody += "<br /><br />Thank you for using ALCOR "
        cBody += "<br /><br /><br /><br />Thanks"
        cBody += "<br /><br /><br /><br />ALCOR ACCOUNTING ADMIN"



        body = File.ReadAllText(Server.MapPath("~/iPxEmailThemplate/emailclient.html"))


        body = body.Replace("{ipx_emailbody}", cBody)

        Dim fromEmail As String = ConfigurationManager.AppSettings("UserName")
        mm.[To].Add(txtusercode.Text)
        mm.From = New MailAddress(ConfigurationManager.AppSettings("from"), ConfigurationManager.AppSettings("website"))
        mm.Subject = "ALCOR"

        mm.Body = body
        mm.IsBodyHtml = True
        Dim smtp As SmtpClient = New SmtpClient()
        smtp.Host = ConfigurationManager.AppSettings("Host")
        'smtp.EnableSsl = False
        Dim NetworkCred As NetworkCredential = New NetworkCredential()
        NetworkCred.UserName = ConfigurationManager.AppSettings("UserName")
        NetworkCred.Password = ConfigurationManager.AppSettings("Password")
        smtp.UseDefaultCredentials = True
        smtp.Credentials = NetworkCred
        smtp.Port = Integer.Parse(ConfigurationManager.AppSettings("Port"))
        smtp.Send(mm)



        '=====================================================


    End Function

    Sub showdata_dropdowncountry()
        Dim cmd As New SqlCommand("SELECT * FROM iPx_profile_geog_country order by country ASC")
        drpCountry.DataSource = ExecuteQuery(cmd, "SELECT")
        drpCountry.DataTextField = "country"
        drpCountry.DataValueField = "countryid"
        drpCountry.DataBind()
        drpCountry.Items.Insert(0, "-Select-")

    End Sub

    Sub showdata_dropdowncity()
        Dim cmd As New SqlCommand("SELECT  RTRIM(cityid) AS cityid,city FROM iPx_profile_geog_city where countryid='" & drpCountry.SelectedValue.Trim & "' order by city ASC")
        drpCity.DataSource = ExecuteQuery(cmd, "SELECT")
        drpCity.DataTextField = "city"
        drpCity.DataValueField = "cityid"
        drpCity.DataBind()
        drpCity.Items.Insert(0, "-Select-")
    End Sub

    Sub updateClient()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim status As String
        If chkStatus.Checked Then
            status = "Y"
        Else
            status = "N"
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE       iPx_profile_client SET  businessname ='" & txtbsname.Text & "', address ='" & txtaddrs.Text & "', taxno ='" & txttaxno.Text & "', phone ='" & txtphone.Text & "', fax ='" & txtfax.Text & "', "
        sSQL = sSQL & " contactperson ='" & txtconpers.Text & "', mobileno ='" & txtmobile.Text & "', email ='" & txtusercode.Text & "', weburl ='" & txtweb.Text & "', city ='" & drpCity.SelectedValue & "', country ='" & drpCountry.SelectedValue & "',  "
        sSQL = sSQL & " currency ='" & txtcurrancy.Text & "',status='" & status & "', promourl ='" & txtpromo.Text & "' where businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub getData()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_user where usercode='" & Session("sEmailClient").ToString.Trim & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        txtusercode.ReadOnly = True
        While oSQLReader.Read
            'SELECT        TOP (200) id, usercode, mobileno, password, signupdate, fullname, status, guid FROM            iPx_profile_user
            txtusercode.Text = oSQLReader.Item("usercode").ToString.Trim
            txtmobile.Text = oSQLReader.Item("mobileno").ToString.Trim
            txtbsname.Text = oSQLReader.Item("fullname").ToString.Trim

        End While

        oCnct.Close()
    End Sub

    Sub getDataClient()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim active As String
        If chkStatus.Checked Then
            active = "Y"
        Else
            active = "N"
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_client where email='" & Session("sEmailClient").ToString.Trim & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        txtusercode.ReadOnly = True
        While oSQLReader.Read

            txtaddrs.Text = oSQLReader.Item("address").ToString.Trim
            txttaxno.Text = oSQLReader.Item("taxno").ToString.Trim
            txtphone.Text = oSQLReader.Item("phone").ToString.Trim
            txtfax.Text = oSQLReader.Item("fax").ToString.Trim
            txtconpers.Text = oSQLReader.Item("contactperson").ToString.Trim
            txtcurrancy.Text = oSQLReader.Item("currency").ToString.Trim
            txtpromo.Text = oSQLReader.Item("promourl").ToString.Trim
            txtmobile.Text = oSQLReader.Item("mobileno").ToString.Trim
            txtweb.Text = oSQLReader.Item("weburl").ToString.Trim

            showdata_dropdowncountry()
            drpCountry.SelectedValue = oSQLReader.Item("country").ToString.Trim
            showdata_dropdowncity()
            drpCity.SelectedValue = oSQLReader.Item("city").ToString.Trim

            Dim act As String
            act = oSQLReader.Item("status").ToString.Trim

            If act = "Y" Then
                chkStatus.Checked = True
            Else
                chkStatus.Checked = False
            End If
        End While

        oCnct.Close()
    End Sub

    Sub UploadImage()
        If Trim(uploadphoto.FileName) <> "" Then
            Dim buffer(uploadphoto.PostedFile.ContentLength) As Byte
            Dim postFile As HttpPostedFile = uploadphoto.PostedFile


            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE   iPx_profile_client SET  "
            sSQL = sSQL & " imagelogo=@filephoto WHERE businessid='" & Session("sBusinessID") & "' "
            postFile.InputStream.Read(buffer, 0, CInt(uploadphoto.PostedFile.ContentLength))
            Dim imgLogo As New SqlParameter("@filephoto", SqlDbType.Image, buffer.Length)
            imgLogo.Value = buffer

            oSQLCmd.Parameters.Add(imgLogo)


            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
        End If

    End Sub

    Sub savedata_user()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim status As String
        If chkStatus.Checked Then
            status = "Y"
        Else
            status = "N"
        End If



        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "INSERT INTO  iPx_profile_user ( usercode, mobileno, password, signupdate, fullname, status, guid) "
        sSQL = sSQL & "VALUES ('" & txtusercode.Text & "','" & txtmobile.Text & "','" & Session("sPasswordCl") & "','" & Date.Now & "','" & txtbsname.Text & "','" & status & "','') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub

    Sub savedata_cilent()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim status As String
        If chkStatus.Checked Then
            status = "Y"
        Else
            status = "N"
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO  iPx_profile_client (businessgroup, businessid, businesstype, businessname, address, taxno, phone, fax, contactperson, mobileno, email, weburl, city, country, userid, registereddate, registeredpackage, status, currency, promourl, imagelogo) "
        sSQL = sSQL & "VALUES ('1', '" & Session("sBusinessID") & "', '26', '" & txtbsname.Text.Trim & "', '" & txtaddrs.Text.Trim & "', '" & txttaxno.Text.Trim & "','" & txtphone.Text.Trim & "', '" & txtfax.Text.Trim & "', "
        sSQL += "'" & txtconpers.Text.Trim & "', '" & txtmobile.Text.Trim & "', '" & txtusercode.Text.Trim & "','" & txtweb.Text.Trim & "', '" & drpCity.SelectedValue.Trim & "', '" & drpCountry.SelectedValue.Trim & "', "
        sSQL += "(select id from iPx_profile_user where usercode= '" & txtusercode.Text.Trim & "'), '" & Date.Now & "', '','" & status & "','" & txtcurrancy.Text.Trim & "', '" & txtpromo.Text.Trim & "',(SELECT imagedata FROM iPx_profile_imagelib where id=1) )"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub saveRegPckg()
        'SELECT    businessid, registeredpackage, program, noofterminal, validity, refferalid FROM            iPx_profile_clientprogram
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim valid As Date
        valid = Date.Today
        'seven days valid
        valid = valid.AddDays(7)

        Dim status As String
        If chkStatus.Checked Then
            status = "Y"
        Else
            status = "N"
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO  iPx_profile_clientprogram (businessid, registeredpackage, program, noofterminal, validity, refferalid) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','1','00','1','" & valid & "','0' )"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()


    End Sub



    Sub updatedata()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim status As String
        If chkStatus.Checked Then
            status = "Y"
        Else
            status = "N"
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPx_profile_user SET mobileno= '" & txtmobile.Text & "', fullname= '" & txtbsname.Text & "',status= '" & status & "' where usercode='" & Session("sEmailClient") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        updateClient()
        UploadImage()

        Session("sMessage") = "Data has been update !| ||"
        Session("sWarningID") = "0"
        Session("sUrlOKONLY") = "UserAccess.aspx"
        Session("sUrlYES") = "http://www.thepyxis.net"
        Session("sUrlNO") = "http://www.thepyxis.net"
        Response.Redirect("warningmsg.aspx")
    End Sub

    Sub cek_username()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT usercode FROM  iPx_profile_user WHERE usercode = '" & txtusercode.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            txtusercode.Focus()
            txtusercode.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Username already exist!');", True)
        Else
            txtmobile.Focus() 'focus to next field
        End If
        oCnct.Close()
    End Sub

    Sub INSERT_TMP_USERACCESS()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPx_profile_client_userid( businessid, usergroup, userid, usercode, password, username, neverexpired, registereddate, expiredafter, islocked, userimage) "
        sSQL = sSQL & "SELECT '" & Session("sBusinessID") & "', '0', userid, 'ADMIN', 'ADMIN123', 'ADMIN', 'Y', registereddate, '', 'N', '' FROM  iPx_profile_client WHERE businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        'Acces Module
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess (businessid,usercode,moduleid,active) "
        sSQL += "SELECT '" & Session("sBusinessID") & "','ADMIN',id,'Y' FROM iPxAcct_profile_client_useraccess_mdl WHERE active='Y'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        'Acces Function
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess_dtl (businessid,usercode,moduleid,funtionid,active) "
        sSQL += "SELECT '" & Session("sBusinessID") & "','ADMIN', moduleid, id,'Y' FROM iPxAcct_profile_client_useraccess_fn WHERE active='Y'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsave.Click
        If (txtbsname.Text = "" Or txtaddrs.Text = "" Or txtphone.Text = "" Or drpCountry.SelectedValue = "-Select-" Or drpCity.SelectedValue = "" Or drpCity.SelectedValue = "-Select-" Or txtusercode.Text = "") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please Complete the Data!');", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT id  FROM iPx_profile_user WHERE usercode = '" & txtusercode.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                updatedata()
            Else
                oSQLReader.Close()
                Session("sBusinessID") = cIpx.GetUniqueNo() 'businessid
                Session("sPasswordCl") = cIpx.getCodeUnx() 'password

                savedata_user()
                savedata_cilent()
                saveRegPckg()

                update("A")
                UploadImage()
              

                INSERT_TMP_USERACCESS()

                'INSERT_SMTP()
                SendEmail()

                Session("sMessage") = "Data has been saved !| ||"
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "viewUserAcc.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            oCnct.Close()
        End If
    End Sub

    Protected Sub btnview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnview.Click
        Response.Redirect("viewUserAcc.aspx")
    End Sub

    Protected Sub btnCxld_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCxld.Click
        Response.Redirect("viewUserAcc.aspx")
    End Sub

    Protected Sub txtusercode_TextChanged(sender As Object, e As EventArgs) Handles txtusercode.TextChanged
        cek_username()

    End Sub

    Protected Sub drpCountry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpCountry.SelectedIndexChanged
        showdata_dropdowncity()

    End Sub
End Class