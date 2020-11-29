Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxAPInputVendor
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, sSQLPMS, profinsi, citi, profilCode, foName, Bank, cusDel As String
    Dim cIpx As New iPxClass

    Sub ARGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_VendorGrp where businessid = '" & Session("sBusinessID") & "' and isActive = 'Y'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlArgroup.DataSource = dt
                dlArgroup.DataTextField = "Description"
                dlArgroup.DataValueField = "apGroup"
                dlArgroup.DataBind()
                dlArgroup.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub country()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_country order by country asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlCountry.DataSource = dt
                dlCountry.DataTextField = "country"
                dlCountry.DataValueField = "countryid"
                dlCountry.DataBind()
                dlCountry.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub province()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_province where countryid ='" & dlCountry.SelectedValue.Trim & "' order by description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlProvince.DataSource = dt
                dlProvince.DataTextField = "description"
                dlProvince.DataValueField = "provid"
                dlProvince.DataBind()
                dlProvince.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub city()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_city where countryid ='" & dlCountry.SelectedValue.Trim & "' and provid = '" & dlProvince.SelectedValue.Trim & "' order by city asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlCity.DataSource = dt
                dlCity.DataTextField = "city"
                dlCity.DataValueField = "cityid"
                dlCity.DataBind()
                dlCity.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub cekCusdel()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.VendorID, a.CoyName, "
        sSQL += "(select (sum(amountdr)-sum(amountcr)) from iPxAcctAP_Transaction "
        sSQL += "where businessid=a.businessid and VendorID=a.VendorID and isActive='Y' "
        sSQL += "group by VendorID) as CusDel "
        sSQL += "from iPxAcctAP_Cfg_Vendor as a where a.businessid='" & Session("sBusinessID") & "' and a.VendorID = '" & lbCustID.Text & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            cusDel = oSQLReader.Item("CusDel").ToString
            If cusDel = "" Or cusDel = "0.00" Then
                cbActive.Enabled = True
            Else
                cbActive.Enabled = False
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub editCustAR()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_Vendor "
        sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and VendorID = '" & lbCustID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlArgroup.SelectedValue = oSQLReader.Item("apGroup").ToString
            tbCoyName.Text = oSQLReader.Item("CoyName").ToString
            tbAddress.Text = oSQLReader.Item("Address").ToString
            tbBilling.Text = oSQLReader.Item("BilllingAddress").ToString
            dlCountry.SelectedValue = oSQLReader.Item("CountryId").ToString
            profinsi = oSQLReader.Item("provid").ToString
            citi = oSQLReader.Item("CityID").ToString
            tbPhone.Text = oSQLReader.Item("Phone").ToString
            tbFax.Text = oSQLReader.Item("Fax").ToString
            tbMobile.Text = oSQLReader.Item("Mobile").ToString
            tbEmail.Text = oSQLReader.Item("Email").ToString
            tbWeb.Text = oSQLReader.Item("Web").ToString
            tbTax.Text = oSQLReader.Item("taxNo").ToString
            tbNotes.Text = oSQLReader.Item("Notes").ToString
            tbCreditLimit.Text = String.Format("{0:N2}", (oSQLReader.Item("CreditLimit"))).ToString
            tbContPerson.Text = oSQLReader.Item("ContPerson").ToString
            tbContPosition.Text = oSQLReader.Item("ContPosition").ToString
            tbContAdress.Text = oSQLReader.Item("ContAddress").ToString
            tbContPhone.Text = oSQLReader.Item("ContPhone").ToString
            tbContMobile.Text = oSQLReader.Item("ContMobile").ToString
            tbContEmail.Text = oSQLReader.Item("ContEmail").ToString
            tbCoaLink.Text = oSQLReader.Item("CoaLink").ToString
            Dim Active As String = oSQLReader.Item("isActive").ToString
            If Active = "Y" Then
                cbActive.Checked = True
            Else
                cbActive.Checked = False
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub saveCustAR()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActive.Checked = True Then
            active = "Y"
        ElseIf cbActive.Checked = False Then
            active = "N"
        End If
        Bank = ""
        Dim regDate As Date = Date.Now()

        sSQL = "INSERT INTO iPxAcctAP_Cfg_Vendor(businessid,VendorID,apGroup,CoyName,Address,BilllingAddress,CountryId,provid,CityID,"
        sSQL += "Phone,Fax,Mobile,Email,Web,TaxNo,Notes,CreditLimit,ContPerson,ContPosition,ContAddress,ContPhone,ContMobile,ContEmail,CoaLink,RegDate,RegBy,DefaultPaid,IsActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbCustID.Text & "','" & dlArgroup.SelectedValue & "','" & Replace(tbCoyName.Text, "'", "''") & "','" & Replace(tbAddress.Text, "'", "''") & "'"
        sSQL += ",'" & Replace(tbBilling.Text, "'", "''") & "','" & dlCountry.SelectedValue & "','" & dlProvince.SelectedValue & "','" & dlCity.SelectedValue & "'"
        sSQL += ",'" & Replace(tbPhone.Text, "'", "''") & "','" & Replace(tbFax.Text, "'", "''") & "','" & Replace(tbMobile.Text, "'", "''") & "','" & Replace(tbEmail.Text, "'", "''") & "'"
        sSQL += ",'" & Replace(tbWeb.Text, "'", "''") & "','" & Replace(tbTax.Text, "'", "''") & "','" & Replace(tbNotes.Text, "'", "''") & "','" & Replace(tbCreditLimit.Text, "'", "''") & "'"
        sSQL += ",'" & Replace(tbContPerson.Text, "'", "''") & "','" & Replace(tbContPosition.Text, "'", "''") & "','" & Replace(tbContAdress.Text, "'", "''") & "','" & Replace(tbContPhone.Text, "'", "''") & "'"
        sSQL += ",'" & Replace(tbContMobile.Text, "'", "''") & "','" & Replace(tbContEmail.Text, "'", "''") & "','" & Replace(tbCoaLink.Text, "'", "''") & "','" & regDate & "','" & Session("iUserID") & "','" & Bank & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        'saveFOMapping()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data save successfully !');document.getElementById('Buttonx').click()", True)
        Response.Redirect("iPxAPVendor.aspx")

    End Sub
    Sub updateCustAR()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActive.Checked = True Then
            active = "Y"
        ElseIf cbActive.Checked = False Then
            active = "N"
        End If
        Bank = ""
        sSQL = "UPDATE iPxAcctAP_Cfg_Vendor SET apGroup='" & dlArgroup.SelectedValue & "',CoyName='" & Replace(tbCoyName.Text, "'", "''") & "',Address='" & Replace(tbAddress.Text, "'", "''") & "',BilllingAddress='" & Replace(tbBilling.Text, "'", "''") & "',CountryId='" & dlCountry.SelectedValue & "',provid='" & dlProvince.SelectedValue & "',CityID='" & dlCity.SelectedValue & "',"
        sSQL += "Phone='" & Replace(tbPhone.Text, "'", "''") & "',Fax='" & Replace(tbFax.Text, "'", "''") & "',Mobile='" & Replace(tbMobile.Text, "'", "''") & "',Email='" & Replace(tbEmail.Text, "'", "''") & "',Web='" & Replace(tbWeb.Text, "'", "''") & "',TaxNo='" & Replace(tbTax.Text, "'", "''") & "',Notes='" & Replace(tbNotes.Text, "'", "''") & "',CreditLimit='" & Replace(tbCreditLimit.Text, "'", "''") & "',ContPerson='" & Replace(tbContPerson.Text, "'", "''") & "',"
        sSQL += "ContPosition='" & Replace(tbContPosition.Text, "'", "''") & "',ContAddress='" & Replace(tbContAdress.Text, "'", "''") & "',ContPhone='" & Replace(tbContPhone.Text, "'", "''") & "',ContMobile='" & Replace(tbContMobile.Text, "'", "''") & "',ContEmail='" & Replace(tbContEmail.Text, "'", "''") & "',CoaLink='" & Replace(tbCoaLink.Text, "'", "''") & "',DefaultPaid='" & Bank & "',IsActive='" & active & "' "
        sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and VendorID = '" & lbCustID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        'updateFOMaping()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('updating successfully !');document.getElementById('Buttonx').click()", True)
        Response.Redirect("iPxAPVendor.aspx")
    End Sub
#Region "Mapping FO Link"
    Sub CreditType()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_cardtype order by [order] asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlCreditCard.DataSource = dt
                dlCreditCard.DataTextField = "description"
                dlCreditCard.DataValueField = "cardtype"
                dlCreditCard.DataBind()
            End Using
        End Using
    End Sub
    Sub businessName()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select iPx_profile_client.businessid, iPx_profile_client.businessname "
        sSQL += "from iPxAcct_FOlink INNER JOIN iPx_profile_client ON iPxAcct_FOlink.FoLink = iPx_profile_client.businessid "
        sSQL += "WHERE iPxAcct_FOlink.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOName.DataSource = dt
                dlFOName.DataTextField = "businessname"
                dlFOName.DataValueField = "businessid"
                dlFOName.DataBind()
                dlFOName.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub ProfileCode()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxPMS_cfg_companyprofile where businessid ='" & dlFOName.SelectedValue & "' order by companyname asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlProfileCode.DataSource = dt
                dlProfileCode.DataTextField = "companyname"
                dlProfileCode.DataValueField = "profilecode"
                dlProfileCode.DataBind()
                dlProfileCode.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub editFOMaping()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_FOmapping "
        sSQL += "WHERE CustomerID ='" & Replace(lbCustID.Text, "'", "''") & "' and businessid ='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            foName = oSQLReader.Item("Folink").ToString
            profilCode = oSQLReader.Item("profilecode").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub saveFOMapping()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If pnCompany.Visible = True And pnCredit.Visible = False Then
            sSQL = "INSERT INTO iPxAcctAR_Cfg_FOmapping(businessid,CustomerID,Folink,profilecode) "
            sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Replace(lbCustID.Text, "'", "''") & "'"
            sSQL = sSQL & ",'" & dlFOName.SelectedValue & "','" & dlProfileCode.SelectedValue & "') "
        ElseIf pnCompany.Visible = False And pnCredit.Visible = True Then
            sSQL = "INSERT INTO iPxAcctAR_Cfg_FOmapping(businessid,CustomerID,Folink,profilecode) "
            sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Replace(lbCustID.Text, "'", "''") & "'"
            sSQL = sSQL & ",'" & "" & "','" & dlCreditCard.SelectedValue & "') "
        End If
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateFOMaping()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If pnCompany.Visible = True And pnCredit.Visible = False Then
            sSQL = "UPDATE iPxAcctAR_Cfg_FOmapping SET Folink='" & dlFOName.SelectedValue & "',profilecode='" & dlProfileCode.SelectedValue & "' "
            sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and CustomerID ='" & Replace(lbCustID.Text, "'", "''") & "'"
        ElseIf pnCompany.Visible = False And pnCredit.Visible = True Then
            sSQL = "UPDATE iPxAcctAR_Cfg_FOmapping SET Folink='" & "" & "',profilecode='" & dlCreditCard.SelectedValue & "' "
            sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and CustomerID ='" & Replace(lbCustID.Text, "'", "''") & "'"
        End If

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
#End Region
#Region "AP Group"
    Sub saveGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActiveARGroup.Checked = True Then
            active = "Y"
        ElseIf cbActiveARGroup.Checked = False Then
            active = "N"
        End If
        sSQL = "INSERT INTO iPxAcctAP_Cfg_VendorGrp(businessid, apGroup, Description, isActive) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Session("sGroupAR") & "','" & Replace(tbDescription.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Add AP group successfully!');", True)
        ARGroup()
        lbAddAR.Text = "<i class='fa fa-plus' style='font-size:18px;'></i>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub
    Sub editGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_VounderGrp "
        sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and apGroup ='" & dlArgroup.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        'usercode, mobileno, password, signupdate, fullname, status, quid
        If oSQLReader.HasRows Then
            tbDescription.Text = oSQLReader.Item("Description").ToString
            Dim Active As String = oSQLReader.Item("isActive").ToString
            If Active = "Y" Then
                cbActiveARGroup.Checked = True
            Else
                cbActiveARGroup.Checked = False
            End If
            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            oCnct.Close()
        End If
    End Sub
    Sub updateGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActiveARGroup.Checked = True Then
            active = "Y"
        ElseIf cbActiveARGroup.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcctAP_Cfg_VounderGrp SET Description='" & Replace(tbDescription.Text, "'", "''") & "', isActive='" & active & "'"
        sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and arGroup ='" & Session("sGroupAR") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('AP group has been update !');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ARGroup()
        lbAddAR.Text = "<i class='fa fa-plus' style='font-size:18px;'></i>"
    End Sub
#End Region
    Sub ListCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "INNER JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "INNER JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "INNER JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa like '" & Replace(tbFindCoaList.Text, "'", "''") & "%'"
        sSQL += " and a.isactive = 'Y'"
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCoa.DataSource = dt
                    gvCoa.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCoa.DataSource = dt
                    gvCoa.DataBind()
                    gvCoa.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Session("sBiEdit") = "" Then
                lbCustID.Text = cIpx.GetCounterMBR("VE", "VE")
                ARGroup()
                country()
                dlCountry.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "01")
                province()
                dlProvince.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "03")
                city()
                dlCity.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "04")
                cbActive.Checked = True
                If dlArgroup.Text = "" Then
                    lbNotifMaping.Visible = True
                End If
                lbSave.Text = "<i class='fa fa-save'></i> Save"
            ElseIf Session("sBiEdit") <> "" Then
                lbCustID.Text = Session("sBiEdit")
                ARGroup()
                country()
                editCustAR()
                editFOMaping()
                province()
                dlProvince.SelectedValue = profinsi
                city()
                dlCity.SelectedValue = citi
                If dlArgroup.SelectedValue = "CC" Then
                    pnCredit.Visible = True
                    pnCompany.Visible = False
                    CreditType()
                    dlCreditCard.SelectedValue = Trim(profilCode)
                Else
                    pnCompany.Visible = True
                    pnCredit.Visible = False
                    businessName()
                    dlFOName.SelectedValue = Trim(foName)
                    ProfileCode()
                    dlProfileCode.SelectedValue = Trim(profilCode)
                End If
                cekCusdel()
                lbSave.Text = "<i class='fa fa-save'></i> Update"
            End If
        End If
    End Sub

    Protected Sub dlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlCountry.SelectedIndexChanged
        province()
        city()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseTwo", "collapseTwo()", True)
    End Sub

    Protected Sub dlProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlProvince.SelectedIndexChanged
        city()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseTwo", "collapseTwo()", True)
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If tbCreditLimit.Text = "" Then
            tbCreditLimit.Text = "0"
        End If
        If dlArgroup.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please Select AP Group!');", True)
            dlArgroup.Focus()
        ElseIf tbCoyName.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Coy Name!');", True)
            tbCoyName.Focus()
        ElseIf tbAddress.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Address!');", True)
            tbAddress.Focus()
        ElseIf dlCountry.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please Select Country!');", True)
            dlCountry.Focus()
        ElseIf dlProvince.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please Select Province!');", True)
            dlProvince.Focus()
        ElseIf dlCity.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please Select City!');", True)
            dlCity.Focus()
        ElseIf tbBilling.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Billing Adress!');", True)
            tbBilling.Focus()
        ElseIf tbPhone.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Phone Number!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseTwo", "collapseTwo()", True)
            tbPhone.Focus()
        ElseIf tbEmail.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter your email!');", True)
            tbEmail.Focus()
        ElseIf pnCompany.Visible = True Then
            If dlProfileCode.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter FO Link and Profile Code!');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseFour", "collapseFour()", True)
                dlProfileCode.Focus()
            Else
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT VendorID FROM iPxAcctAP_Cfg_Vendor WHERE VendorID = '" & lbCustID.Text & "'"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    updateCustAR()
                Else
                    oSQLReader.Close()
                    saveCustAR()
                End If
            End If
        ElseIf pnCredit.Visible = True Then
            If dlCreditCard.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Credit Card!');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseFour", "collapseFour()", True)
                dlCreditCard.Focus()
            Else
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT VendorID FROM iPxAcctAP_Cfg_Vendor WHERE businessid = '" & Session("sBusinessID") & "' and VendorID = '" & lbCustID.Text & "'"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    updateCustAR()
                Else
                    oSQLReader.Close()
                    saveCustAR()
                End If
            End If
        End If
    End Sub


    Protected Sub dlArgroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlArgroup.SelectedIndexChanged
        If dlArgroup.Text = "" Then
            lbNotifMaping.Visible = True
            pnCredit.Visible = False
            pnCompany.Visible = False
            lbAddAR.Text = "<i class='fa fa-plus' style='font-size:18px;'></i>"
        ElseIf dlArgroup.SelectedValue = "CC" Then
            pnCredit.Visible = True
            pnCompany.Visible = False
            lbNotifMaping.Visible = False
            lbAddAR.Text = "<i class='fa fa-plus' style='font-size:18px;'></i>"
            CreditType()
        Else
            pnCompany.Visible = True
            pnCredit.Visible = False
            lbNotifMaping.Visible = False
            lbAddAR.Text = "<i class='fa fa-edit' style='font-size:18px;'></i>"
            businessName()
            dlFOName.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "10")
            ProfileCode()
        End If
    End Sub

    Protected Sub lbMoveAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbMoveAR.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseOne", "collapseOne()", True)
        dlArgroup.Focus()
    End Sub

    Protected Sub lbAddAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddAR.Click
        If dlArgroup.Text = "CC" Or dlArgroup.Text = "" Then
            Session("sGroupAR") = cIpx.GetCounterARG("G", "G")
            cbActiveARGroup.Checked = True
            tbDescription.Text = ""
            cbActiveARGroup.Checked = True
            lbSaveARGroup.Text = "<i class='fa fa-save'></i> Save"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            Session("sGroupAR") = dlArgroup.SelectedValue
            editGroup()
            lbSaveARGroup.Text = "<i class='fa fa-save'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub

    Protected Sub lbSaveARGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveARGroup.Click
        If tbDescription.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Description!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            'tbMenuName.Focus()
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT apGroup FROM iPxAcctAP_Cfg_VendorGrp WHERE businessid = '" & Session("sBusinessID") & "' and apGroup = '" & Session("sGroupAR") & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                updateGroup()
            Else
                oSQLReader.Close()
                saveGroup()
            End If
        End If
    End Sub

    Protected Sub dlFOName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlFOName.SelectedIndexChanged
        ProfileCode()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseFour", "collapseFour()", True)
    End Sub

    Protected Sub lbSearchCOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSearchCOA.Click
        tbFindCoaList.Text = tbCoaLink.Text
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseFour", "collapseFour()", True)
    End Sub

    Protected Sub lbAbortListCOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortListCOA.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseFour", "collapseFour()", True)
    End Sub

    Protected Sub lbFindListCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindListCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        tbCoaLink.Text = tbFindCoaList.Text
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseFour", "collapseFour()", True)
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
    End Sub

    Protected Sub gvCoa_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCoa.PageIndexChanging
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
    End Sub

    Protected Sub gvCoa_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoa.RowCommand
        If e.CommandName = "getSelect" Then
            tbCoaLink.Text = e.CommandArgument
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapseFour", "collapseFour()", True)
    End Sub
End Class
