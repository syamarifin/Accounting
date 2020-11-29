Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxInputCustomerCC
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, sSQLPMS, profinsi, citi, profilCode, foName, Bank, cusDel As String
    Dim cIpx As New iPxClass
    Sub BankClearance()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Paidby where businessid = '" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlBankClear.DataSource = dt
                dlBankClear.DataTextField = "Description"
                dlBankClear.DataValueField = "PaidBy"
                dlBankClear.DataBind()
                dlBankClear.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    'Sub CoyGroup()
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT * FROM iPxAcctAR_Cfg_CoyGroup where businessid = '" & Session("sBusinessID") & "' and isActive='Y'"
    '    Using sda As New SqlDataAdapter()
    '        oSQLCmd.CommandText = sSQL
    '        sda.SelectCommand = oSQLCmd
    '        Using dt As New DataTable()
    '            sda.Fill(dt)
    '            dlCoyGroup.DataSource = dt
    '            dlCoyGroup.DataTextField = "Description"
    '            dlCoyGroup.DataValueField = "CoyGroup"
    '            dlCoyGroup.DataBind()
    '            dlCoyGroup.Items.Insert(0, "")
    '        End Using
    '    End Using
    'End Sub
    Sub cekCusdel()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.CustomerID, a.CoyName, "
        sSQL += "(select (sum(amountdr)-sum(amountcr)) from iPxAcctAR_Transaction "
        sSQL += "where businessid=a.businessid and CustomerID=a.CustomerID and isActive='Y' "
        sSQL += "group by CustomerID) as CusDel "
        sSQL += "from iPxAcctAR_Cfg_Customer as a where a.businessid='" & Session("sBusinessID") & "' and a.CustomerID = '" & lbCustID.Text & "' "
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
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Customer "
        sSQL += "WHERE CustomerID = '" & lbCustID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            'dlCoyGroup.SelectedValue = oSQLReader.Item("CoyGroup").ToString
            tbCoyName.Text = oSQLReader.Item("CoyName").ToString
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
            tbCoaLink.Text = oSQLReader.Item("CoaLink").ToString
            dlBankClear.SelectedValue = oSQLReader.Item("DefaultPaid").ToString
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
        Dim regDate As Date = Date.Now()

        sSQL = "INSERT INTO iPxAcctAR_Cfg_Customer(businessid,CustomerID,arGroup,CoyGroup,CoyName,Address,BilllingAddress,CountryId,provid,CityID,"
        sSQL += "Phone,Fax,Mobile,Email,Web,TaxNo,Notes,CreditLimit,ContPerson,ContPosition,ContAddress,ContPhone,ContMobile,ContEmail,CoaLink,RegDate,RegBy,DefaultPaid,IsActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbCustID.Text & "','CC','','" & Replace(tbCoyName.Text, "'", "''") & "','-'"
        sSQL += ",'-','-','-','-','" & Replace(tbPhone.Text, "'", "''") & "','" & Replace(tbFax.Text, "'", "''") & "','" & Replace(tbMobile.Text, "'", "''") & "','" & Replace(tbEmail.Text, "'", "''") & "'"
        sSQL += ",'" & Replace(tbWeb.Text, "'", "''") & "','" & Replace(tbTax.Text, "'", "''") & "','" & Replace(tbNotes.Text, "'", "''") & "','" & Replace(tbCreditLimit.Text, "'", "''") & "'"
        sSQL += ",'-','-','-','-','-','-','" & Replace(tbCoaLink.Text, "'", "''") & "','" & regDate & "','" & Session("iUserID") & "','" & dlBankClear.SelectedValue & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        saveFOMapping()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data save successfully !');document.getElementById('Buttonx').click()", True)
        Response.Redirect("iPxCustomerAR.aspx")

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
        sSQL = "UPDATE iPxAcctAR_Cfg_Customer SET businessid='" & Session("sBusinessID") & "', arGroup='CC',CoyName='" & Replace(tbCoyName.Text, "'", "''") & "',Address='-',BilllingAddress='-',CountryId='-',provid='-',CityID='-',"
        sSQL += "Phone='" & Replace(tbPhone.Text, "'", "''") & "',Fax='" & Replace(tbFax.Text, "'", "''") & "',Mobile='" & Replace(tbMobile.Text, "'", "''") & "',Email='" & Replace(tbEmail.Text, "'", "''") & "',Web='" & Replace(tbWeb.Text, "'", "''") & "',TaxNo='" & Replace(tbTax.Text, "'", "''") & "',Notes='" & Replace(tbNotes.Text, "'", "''") & "',CreditLimit='" & Replace(tbCreditLimit.Text, "'", "''") & "',ContPerson='-',"
        sSQL += "ContPosition='-',ContAddress='-',ContPhone='-',ContMobile='-',ContEmail='-',CoaLink='" & Replace(tbCoaLink.Text, "'", "''") & "',DefaultPaid='" & dlBankClear.SelectedValue & "',IsActive='" & active & "' "
        sSQL += "WHERE CustomerID = '" & lbCustID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        updateFOMaping()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('updating successfully !');document.getElementById('Buttonx').click()", True)
        Response.Redirect("iPxCustomerAR.aspx")
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
#End Region

#Region "Coy Group"
    Sub saveCoyGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActiveCoy.Checked = True Then
            active = "Y"
        ElseIf cbActiveCoy.Checked = False Then
            active = "N"
        End If

        sSQL = "INSERT INTO iPxAcctAR_Cfg_CoyGroup(businessid,CoyGroup,Description,Contact,Address,Phone,Notes,isActive) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbCoyGroup.Text, "'", "''") & "','" & Replace(tbCoyDescription.Text, "'", "''") & "'"
        sSQL = sSQL & ",'" & Replace(tbCoyContact.Text, "'", "''") & "','" & Replace(tbCoyAddress.Text, "'", "''") & "','" & Replace(tbCoyPhone.Text, "'", "''") & "'"
        sSQL = sSQL & ",'" & Replace(tbCoyNotes.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Add coy group successfully!');", True)
        'CoyGroup()
    End Sub
    'Sub editCoyGroup()
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT * FROM iPxAcctAR_Cfg_CoyGroup "
    '    sSQL += "WHERE CoyGroup ='" & dlCoyGroup.SelectedValue & "'"
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        tbCoyDescription.Text = oSQLReader.Item("Description").ToString
    '        tbCoyContact.Text = oSQLReader.Item("Contact").ToString
    '        tbCoyAddress.Text = oSQLReader.Item("Address").ToString
    '        tbCoyPhone.Text = oSQLReader.Item("Phone").ToString
    '        tbCoyNotes.Text = oSQLReader.Item("Notes").ToString
    '        Dim Active As String = oSQLReader.Item("isActive").ToString
    '        If Active = "Y" Then
    '            cbActiveCoy.Checked = True
    '        Else
    '            cbActiveCoy.Checked = False
    '        End If
    '        oCnct.Close()
    '    Else
    '        oCnct.Close()
    '    End If
    'End Sub
    Sub updateCoyGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActiveCoy.Checked = True Then
            active = "Y"
        ElseIf cbActiveCoy.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcctAR_Cfg_CoyGroup SET Description='" & Replace(tbCoyDescription.Text, "'", "''") & "',Contact='" & Replace(tbCoyContact.Text, "'", "''") & "',Address='" & Replace(tbCoyAddress.Text, "'", "''") & "',"
        sSQL += "Phone='" & Replace(tbCoyPhone.Text, "'", "''") & "',Notes='" & Replace(tbCoyNotes.Text, "'", "''") & "', isActive='" & active & "'"
        sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and CoyGroup ='" & Replace(tbCoyGroup.Text, "'", "''") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Coy group has been update !');", True)
        'CoyGroup()
    End Sub
#End Region
    Sub saveFOMapping()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctAR_Cfg_FOmapping(businessid,CustomerID,Folink,profilecode) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Replace(lbCustID.Text, "'", "''") & "'"
        sSQL = sSQL & ",'" & "" & "','" & dlCreditCard.SelectedValue & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateFOMaping()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Cfg_FOmapping SET Folink='" & "" & "',profilecode='" & dlCreditCard.SelectedValue & "' "
        sSQL += "WHERE CustomerID ='" & Replace(lbCustID.Text, "'", "''") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

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
    Sub saveCommission()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "INSERT INTO iPxAcctAR_Commission(businessid,CustomerID,CommissionPct,CommissionCoa) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbCustID.Text & "','" & Replace(tbkomisi.Text, "'", "''") & "','" & Replace(tbCoaKomisi.Text, "'", "''") & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub updateCommission()
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
        sSQL = "UPDATE iPxAcctAR_Commission SET CommissionPct='" & Replace(tbkomisi.Text, "'", "''") & "', CommissionCoa='" & Replace(tbCoaKomisi.Text, "'", "''") & "' "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' and CustomerID = '" & lbCustID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub DeleteCommission()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "DELETE FROM iPxAcctAR_Commission where businessid='" & Session("sBusinessID") & "' and CustomerID='" & lbCustID.Text & "' "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub cekKomisi()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT CustomerID FROM iPxAcctAR_Commission where businessid='" & Session("sBusinessID") & "' and CustomerID='" & lbCustID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            updateCommission()
        Else
            oSQLReader.Close()
            saveCommission()
        End If
    End Sub
    Sub editKomisi()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Commission "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' and CustomerID = '" & lbCustID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbkomisi.Text = String.Format("{0:0}", (oSQLReader.Item("CommissionPct"))).ToString
            tbCoaKomisi.Text = oSQLReader.Item("CommissionCoa")
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Session("sBiEdit") = "" Then
                lbCustID.Text = cIpx.GetCounterMBR("CA", "CA")
                'CoyGroup()
                BankClearance()
                CreditType()
                'If dlArgroup.Text = "" Then
                '    lbNotifMaping.Visible = True
                'End If
                cbActive.Checked = True
                lbSave.Text = "<i class='fa fa-save'></i> Save"
            ElseIf Session("sBiEdit") <> "" Then
                lbCustID.Text = Session("sBiEdit")
                BankClearance()
                'CoyGroup()
                editCustAR()
                editKomisi()
                editFOMaping()
                CreditType()
                dlCreditCard.SelectedValue = Trim(profilCode)
                cekCusdel()
                lbSave.Text = "<i class='fa fa-save'></i> Update"
            End If
            Session("sCoa") = ""
        End If
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If tbCreditLimit.Text = "" Then
            tbCreditLimit.Text = "0"
        End If
        'If dlCoyGroup.Text = "" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please Select Coy Group!');", True)
        '    dlCoyGroup.Focus()
        If tbCoyName.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Coy Name!');", True)
            tbCoyName.Focus()
        ElseIf tbPhone.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Phone Number!');", True)
            tbPhone.Focus()

        ElseIf dlCreditCard.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Credit Card!');", True)
            dlCreditCard.Focus()
        ElseIf dlBankClear.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter bank clearance!');", True)
            dlBankClear.Focus()
        ElseIf tbkomisi.Text <> "" And tbCoaKomisi.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter COA Commission!');", True)
            tbCoaKomisi.Focus()
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT CustomerID FROM iPxAcctAR_Cfg_Customer WHERE businessid ='" & Session("sBusinessID") & "' and CustomerID = '" & lbCustID.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                If tbkomisi.Text = "" Then
                    DeleteCommission()
                Else
                    cekKomisi()
                End If
                updateCustAR()
            Else
                oSQLReader.Close()
                If tbkomisi.Text <> "" Then
                    saveCommission()
                End If
                saveCustAR()
            End If
        End If
    End Sub

    'Protected Sub dlCoyGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlCoyGroup.SelectedIndexChanged
    '    If dlCoyGroup.Text = "" Then
    '        lbAddCoy.Text = "<i class='fa fa-plus' style='font-size:18px;'></i>"
    '        lbSaveCoy.Text = "<i class='fa fa-save'></i> Save"
    '    Else
    '        lbAddCoy.Text = "<i class='fa fa-edit' style='font-size:18px;'></i>"
    '        lbSaveCoy.Text = "<i class='fa fa-save'></i> Update"
    '    End If
    'End Sub

    'Protected Sub lbAddCoy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCoy.Click
    '    If dlCoyGroup.Text = "" Then
    '        tbCoyGroup.Text = cIpx.GetCounterARG("C", "C")
    '        cbActiveCoy.Checked = True
    '    Else
    '        tbCoyGroup.Text = dlCoyGroup.SelectedValue
    '        editCoyGroup()
    '    End If
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    'End Sub

    Protected Sub lbSaveCoy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveCoy.Click
        If tbCoyDescription.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Description!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
        ElseIf tbCoyContact.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Contact!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
        ElseIf tbCoyAddress.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Address!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
        ElseIf tbCoyPhone.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Phone!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT CoyGroup FROM iPxAcctAR_Cfg_CoyGroup WHERE CoyGroup = '" & tbCoyGroup.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                updateCoyGroup()
            Else
                oSQLReader.Close()
                saveCoyGroup()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            'lbAddCoy.Text = "<i class='fa fa-plus' style='font-size:18px;'></i>"
        End If
    End Sub

    Protected Sub lbFindCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindCoa.Click
        Session("sCoa") = "CoaLink"
        tbFindCoaList.Text = tbCoaLink.Text
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbAbortListCOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortListCOA.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
    End Sub

    Protected Sub lbFindListCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindListCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        If Session("sCoa") = "Komisi" Then
            tbCoaKomisi.Text = tbFindCoaList.Text
        ElseIf Session("sCoa") = "CoaLink" Then
            tbCoaLink.Text = tbFindCoaList.Text
        End If
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
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
            If Session("sCoa") = "Komisi" Then
                tbCoaKomisi.Text = e.CommandArgument
            ElseIf Session("sCoa") = "CoaLink" Then
                tbCoaLink.Text = e.CommandArgument
            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
    End Sub

    Protected Sub lbFindCoaKomisi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindCoaKomisi.Click
        tbFindCoaList.Text = tbCoaKomisi.Text
        Session("sCoa") = "Komisi"
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub
End Class
