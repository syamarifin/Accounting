
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Threading
Partial Class iPxAdmin_setupbusiness
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'If Session("sUserCode") = "" Then
            ' Response.Redirect("../signin.aspx")
            'Else
            '=========================================================
            Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder2"), ContentPlaceHolder)
            Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)

            'label.Text = "Business Profile"

            '------------------------------------------------------------

            ' Session("iUserID") = 14
            lstCountry.DataBind()
            ddCity.DataBind()
            lstTaxFormula.DataBind()
            lstBussType.DataBind()
            GetBusinessDetail()

            'End If

        End If

        ScriptManager.RegisterStartupScript(Page, [GetType](), "closeAlret", "<script>closeAlret()</script>", False)
    End Sub

    Protected Sub iPxCNCT0_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles iPxCNCT0.Selecting

    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("home.aspx")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Session("sStatus") = "1" Then
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            sSQL = "update iPx_profile_user set status='2' where id=" & Session("iUserID")
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLCmd.ExecuteNonQuery()
            Session("sStatus") = "2"
            oCnct.Close()
        End If
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        sSQL = "delete ipx_profile_client_usergeneric where genericid=" & Session("iUserID")
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()

        sSQL = "insert into ipx_profile_client_usergeneric values(" & Session("iUserID") & ",'" & txtGenericPassword.Text & "')  "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()


        If UpdateBusinessInfo() Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Your business profile has been saved !');document.getElementById('Buttonx').click()", True)
        End If
        'If Session("sStatus") = "2" Then
        '    Response.Redirect("wizard.aspx")
        'End If
        'nextWizard()
    End Sub

    Private Function UpdateBusinessInfo() As Boolean
        On Error Resume Next
        Dim buffer(FileUpload4.PostedFile.ContentLength) As Byte
        Dim postFile As HttpPostedFile = FileUpload4.PostedFile
        Dim imgLogo As New SqlParameter("@imagelogo", SqlDbType.Image, buffer.Length)

        sSQL = "update ipx_profile_client set businesstype=" & lstBussType.SelectedValue & ","
        txtBusinessName.Text = txtBusinessName.Text.Replace("'", "`")
        sSQL = sSQL & "businessgroup=" & lstBussGroup.SelectedValue & ","
        sSQL = sSQL & "businessname='" & txtBusinessName.Text & "',"
        sSQL = sSQL & "address='" & txtBusinessAddress.Text & "',"
        sSQL = sSQL & "taxno='" & txtBusinessNo.Text & "',"
        sSQL = sSQL & "phone='" & txtBusinessPhone.Text & "',"
        sSQL = sSQL & "fax='" & txtBusinessFax.Text & "',"
        sSQL = sSQL & "contactperson='" & txtBusinessContact.Text & "',"
        sSQL = sSQL & "mobileno='" & txtBusinessMobileNo.Text & "',"
        sSQL = sSQL & "email='" & txtBusinessEmail.Text & "',"
        sSQL = sSQL & "weburl='" & txtBusinessWEB.Text & "',"
        sSQL = sSQL & "city='" & ddCity.SelectedValue & "',"
        sSQL = sSQL & "country='" & lstCountry.SelectedValue & "'"
        'sSQL = sSQL & "userid=" & Session("iUserID") & ","
        'sSQL = sSQL & "registereddate='" & Format(Now, "yyyy/MM/dd hh:mm") & "',"
        'sSQL = sSQL & "registeredpackage='" & 0 & "',"
        'sSQL = sSQL & "status='N',"
        'sSQL = sSQL & "currency='" & Session("sCurrencyCode") & "',"
        'sSQL = sSQL & "promourl='',"
        If Trim(FileUpload4.FileName) <> "" Then
            postFile.InputStream.Read(buffer, 0, CInt(FileUpload4.PostedFile.ContentLength))
            sSQL = sSQL & ", imagelogo=@imagelogo "
            imgLogo.Value = buffer

        End If
        sSQL = sSQL & " where businessid='" & Session("sBusinessID") & "'  "

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If Trim(FileUpload4.FileName) <> "" Then
            oSQLCmd.Parameters.Add(imgLogo)
        End If
        oSQLCmd.ExecuteNonQuery()
        '-------------------------------------
        sSQL = "update iPx_profile_tax set taxdescription='" & txtTaxDescription.Text & "',"
        sSQL = sSQL & "taxpct=" & CDbl(txtTaxPCT.Text) & ","
        sSQL = sSQL & "servicedescription='" & txtSchgDescription.Text & "',"
        sSQL = sSQL & "servicepct=" & CDbl(txtServiceChargePCT.Text) & ","
        sSQL = sSQL & "formula=" & lstTaxFormula.SelectedValue & " "
        sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()


        oCnct.Close()
        UpdateBusinessInfo = True
    End Function


    Protected Sub lstBussType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstBussType.SelectedIndexChanged

    End Sub
    Sub city()
        iPXCNCTCTY.SelectCommand = "SELECT rtrim([cityid]) as cityid, [city] FROM [iPx_profile_geog_city] WHERE countryid='" & lstCountry.SelectedValue & "' ORDER BY [city]  "
        ddCity.DataBind()
    End Sub
    Protected Sub lstCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstCountry.SelectedIndexChanged
        SetTaxService()
        city()
    End Sub

    Protected Sub SetTaxService()
        sSQL = "Select currency, taxdescription, tax, servicedescription, service, taxformula FROM iPx_profile_geog_country WHERE (countryid = '" & lstCountry.SelectedValue & "')"

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            txtTaxDescription.Text = oSQLReader.Item("taxdescription").ToString.Trim
            txtTaxPCT.Text = Format(oSQLReader.Item("tax"), "#,##0.00").ToString.Trim
            txtSchgDescription.Text = oSQLReader.Item("servicedescription").ToString.Trim
            txtServiceChargePCT.Text = Format(oSQLReader.Item("service"), "#,##0.00").ToString.Trim
            Session("sCurrencyCode") = oSQLReader.Item("currency")
            lstTaxFormula.SelectedValue = Val(oSQLReader.Item("taxformula"))
            oSQLReader.Close()

            sSQL = "DELETE FROM iPx_profile_tax WHERE (businessid = '" & Session("sBusinessID") & "') "
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLCmd.ExecuteNonQuery()

            sSQL = "insert into iPx_profile_tax values('" & Session("sBusinessID") & "','" & txtTaxDescription.Text & "'," & CDbl(txtTaxPCT.Text) & ",'" & txtSchgDescription.Text & "'," & CDbl(txtServiceChargePCT.Text) & "," & lstTaxFormula.SelectedValue & ") "
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLCmd.ExecuteNonQuery()

        End If
        oCnct.Close()
    End Sub

    Protected Sub SetBusinessTaxService()
        sSQL = "SELECT taxdescription, taxpct, servicedescription, servicepct, formula FROM iPx_profile_tax WHERE businessid = '" & Session("sBusinessID") & "'"

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            txtTaxDescription.Text = oSQLReader.Item("taxdescription").ToString.Trim
            txtTaxPCT.Text = Format(oSQLReader.Item("taxpct"), "#,##0.00").ToString.Trim
            txtSchgDescription.Text = oSQLReader.Item("servicedescription").ToString.Trim
            txtServiceChargePCT.Text = Format(oSQLReader.Item("servicepct"), "#,##0.00").ToString.Trim
            lstTaxFormula.SelectedValue = Val(oSQLReader.Item("formula"))
            oSQLReader.Close()
        Else
            oSQLReader.Close()
            SetTaxService()
        End If
        oCnct.Close()
    End Sub

    Private Sub GetBusinessDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        sSQL = "select password from ipx_profile_client_usergeneric where genericid='" & Session("iUserID") & " '"

        txtGenericID.Text = Session("iUserID")
        txtGenericID.Enabled = False

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader
        If oSQLReader.Read Then
            txtGenericPassword.Text = Trim(oSQLReader.Item("password"))

        End If
        oSQLReader.Close()



        sSQL = "SELECT a.businesstype, a.businessgroup, a.businessname, a.address, a.taxno, a.phone, a.fax, a.contactperson, a.mobileno, "
        sSQL += "a.email, a.weburl, a.country,a.city, a.imagelogo, b.password FROM iPx_profile_client as a "
        sSQL += "INNER JOIN iPx_profile_user as b on b.usercode = a.email where a.businessid='" & Session("sBusinessID") & "' "

        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            lstBussType.SelectedValue = oSQLReader.Item("businesstype")
            lstBussGroup.SelectedValue = oSQLReader.Item("businessgroup")
            txtBusinessName.Text = oSQLReader.Item("businessname").ToString.Trim
            txtBusinessAddress.Text = oSQLReader.Item("address").ToString.Trim
            txtBusinessNo.Text = oSQLReader.Item("taxno").ToString.Trim
            txtBusinessPhone.Text = oSQLReader.Item("phone").ToString.Trim
            txtBusinessFax.Text = oSQLReader.Item("fax").ToString.Trim
            txtBusinessContact.Text = oSQLReader.Item("contactperson").ToString.Trim
            txtBusinessMobileNo.Text = oSQLReader.Item("mobileno").ToString.Trim
            txtBusinessEmail.Text = oSQLReader.Item("email").ToString.Trim
            txtBusinessWEB.Text = oSQLReader.Item("weburl").ToString.Trim
            lstCountry.SelectedValue = oSQLReader.Item("country")
            city()
            sSQL = oSQLReader.Item("city").ToString.Trim
            ddCity.SelectedValue = oSQLReader.Item("city").ToString.Trim
            txtGenericPassword.Text = oSQLReader.Item("password").ToString.Trim
            Image3.ImageUrl = "~/iPxPOS/Handler.ashx?ID=0|" & Session("sBusinessID")

        End If
        oCnct.Close()
        SetBusinessTaxService()
    End Sub

    Protected Sub btnAddBussGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBussGroup.Click
        sSQL = "insert into ipx_general_businessgroup (userid, description) values('" & Session("iUserID") & "','" & txtBusinessGroup.Text & "' )"

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()
        oCnct.Close()
        lstBussGroup.DataBind()

    End Sub

    Protected Sub btnDelBussGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelBussGroup.Click
        If lstBussGroup.SelectedValue <> 1 Then
            sSQL = "delete ipx_general_businessgroup where userid='" & Session("iUserID") & "' and id=" & lstBussGroup.SelectedValue

            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLCmd.ExecuteNonQuery()

            sSQL = "update ipx_profile_client set businessgroup=1 where businessid='" & Session("sBusinessID") & "' "
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            lstBussGroup.DataBind()
            lstBussGroup.SelectedValue = 1
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry This Default Business Group Can Not Be Deleted !');document.getElementById('Buttonx').click()", True)

        End If

    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        UpdateBusinessInfo()
        GetBusinessDetail()
    End Sub

    Protected Sub txtTaxDescription_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTaxDescription.TextChanged

    End Sub

    Protected Overrides Sub InitializeCulture()
        Dim selectedLanguage As String
        selectedLanguage = Session("sUICulture")
        If Trim(selectedLanguage) <> "" Then
            Dim cultureInfo As System.Globalization.CultureInfo
            cultureInfo = New System.Globalization.CultureInfo(selectedLanguage)
            Thread.CurrentThread.CurrentCulture = cultureInfo
            Thread.CurrentThread.CurrentUICulture = cultureInfo
            MyBase.InitializeCulture()
        End If

    End Sub
    Sub nextWizard()
        Dim status As String
        status = Session("sStatus")
        If status = "1" Then
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            sSQL = "update iPx_profile_user set status='2' where id=" & Session("iUserID")
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
        End If
        Response.Redirect("setuproom.aspx")
    End Sub
    
End Class
