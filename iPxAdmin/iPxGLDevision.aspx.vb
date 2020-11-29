Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxGLDevision
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, active As String
    Dim cIpx As New iPxClass
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
                lbCreateDev.Enabled = True
                lbCreateDept.Enabled = True
                lbCreateSubDept.Enabled = True
            Else
                lbCreateDev.Enabled = False
                lbCreateDept.Enabled = False
                lbCreateSubDept.Enabled = False
            End If
        Else
            lbCreateDev.Enabled = False
            lbCreateDept.Enabled = False
            lbCreateSubDept.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub showdata_dropdownStatusQuery()
        dlQStatus.Items.Clear()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "ALL")
        dlQStatus.Items.Insert(2, "Non Devision")
    End Sub
    Sub listGLDevision()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT *, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid='" & Session("sBusinessID") & "' and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "from iPxAcct_CoaDivision "
        sSQL += "where businessid='" & Session("sBusinessID") & "'"
        If Session("sQueryDev") = "" Then
            Session("sQueryDev") = Session("sConditionDev")
            If Session("sQueryTicketDev") <> "" Or Session("sConditionDev") <> "" Then
                sSQL = sSQL & Session("sQueryDev")
                Session("sConditionDev") = ""
            Else
                sSQL = sSQL & " and IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryDev")
            Session("sConditionDev") = ""
        End If
        sSQL += " order by description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLDev.DataSource = dt
                    gvGLDev.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLDev.DataSource = dt
                    gvGLDev.DataBind()
                    gvGLDev.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub saveGLDevision()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveDev.Checked = True Then
            active = "Y"
        ElseIf cbActiveDev.Checked = False Then
            active = "N"
        End If
        sSQL = "INSERT INTO iPxAcct_CoaDivision(businessid,Division,description,isactive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbDevID.Text, "'", "''") & "','" & Replace(tbDesc.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateGLDevision()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveDev.Checked = True Then
            active = "Y"
        ElseIf cbActiveDev.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcct_CoaDivision SET description='" & Replace(tbDesc.Text, "'", "''") & "',isactive='" & active & "'"
        sSQL = sSQL & "WHERE Division ='" & tbDevID.Text & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub editGLDevision()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDivision "
        sSQL += "WHERE Division ='" & tbDevID.Text & "' and businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbDesc.Text = oSQLReader.Item("description").ToString
            If oSQLReader.Item("isactive").ToString = "Y" Then
                cbActiveDev.Checked = True
            ElseIf oSQLReader.Item("isactive").ToString = "N" Then
                cbActiveDev.Checked = False
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    'Departement tab
    Sub listGLDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) as DescDiv, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "from iPxAcct_CoaDepartement AS a "
        sSQL += "INNER JOIN iPxAcct_CoaDivision AS b ON b.Division = a.Division "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.Division ='" & Session("sDeptID") & "'"
        If Session("sQueryDept") = "" Then
            Session("sQueryDept") = Session("sConditionDept")
            If Session("sQueryDept") <> "" Or Session("sConditionDept") <> "" Then
                sSQL = sSQL & Session("sQueryDept")
                Session("sConditionDept") = ""
            Else
                sSQL = sSQL & " and a.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryDept")
            Session("sConditionDept") = ""
        End If
        sSQL += " order by a.Departement asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLDept.DataSource = dt
                    gvGLDept.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLDept.DataSource = dt
                    gvGLDept.DataBind()
                    gvGLDept.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub tampilGLDevision()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDivision "
        sSQL += "WHERE Division ='" & Session("sDeptID") & "' and businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbDeptDev.Text = oSQLReader.Item("description").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub saveGLDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveDept.Checked = True Then
            active = "Y"
        ElseIf cbActiveDept.Checked = False Then
            active = "N"
        End If
        sSQL = "INSERT INTO iPxAcct_CoaDepartement(businessid,Departement,Division,description,isactive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbDeptID.Text, "'", "''") & "','" & Session("sDeptID") & "','" & Replace(tbDescDept.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateGLDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveDept.Checked = True Then
            active = "Y"
        ElseIf cbActiveDept.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcct_CoaDepartement SET description='" & Replace(tbDescDept.Text, "'", "''") & "',isactive='" & active & "'"
        sSQL = sSQL & "WHERE Division ='" & Session("sDeptID") & "' and businessid='" & Session("sBusinessID") & "' and Departement='" & tbDeptID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub editGLDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDepartement "
        sSQL += "WHERE Departement ='" & tbDeptID.Text & "' and businessid='" & Session("sBusinessID") & "' and Division='" & Session("sDeptID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbDescDept.Text = oSQLReader.Item("description").ToString
            If oSQLReader.Item("isactive").ToString = "Y" Then
                cbActiveDept.Checked = True
            ElseIf oSQLReader.Item("isactive").ToString = "N" Then
                cbActiveDept.Checked = False
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    'Departement tab end
    'SubDepartement tab
    Sub listGLSubDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.SubDept, (b.Description) as DescDiv, (c.Description) as DescDept, a.Description,a.isActive, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "from iPxAcct_CoaSubDepartement as a "
        sSQL += "INNER JOIN iPxAcct_CoaDivision as b ON b.businessid=a.businessid AND b.Division=a.Division "
        sSQL += "INNER JOIN iPxAcct_CoaDepartement as c ON c.businessid=a.businessid AND c.Division=a.Division AND c.Departement=a.Departement "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.Division ='" & Session("sDeptID") & "' AND a.Departement ='" & Session("sSubDeptID") & "' "
        If Session("sQuerySubDept") = "" Then
            Session("sQuerySubDept") = Session("sConditionSubDept")
            If Session("sQuerySubDept") <> "" Or Session("sConditionSubDept") <> "" Then
                sSQL = sSQL & Session("sQuerySubDept")
                Session("sConditionSubDept") = ""
            Else
                sSQL = sSQL & " and a.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQuerySubDept")
            Session("sConditionSubDept") = ""
        End If
        sSQL += " order by a.SubDept asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLSubDept.DataSource = dt
                    gvGLSubDept.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLSubDept.DataSource = dt
                    gvGLSubDept.DataBind()
                    gvGLSubDept.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub tampilGLDept()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.SubDept, (b.Description) as DescDiv, (c.Description) as DescDept from iPxAcct_CoaSubDepartement as a "
        sSQL += "INNER JOIN iPxAcct_CoaDivision as b ON b.businessid=a.businessid AND b.Division=a.Division "
        sSQL += "INNER JOIN iPxAcct_CoaDepartement as c ON c.businessid=a.businessid AND c.Division=a.Division AND c.Departement=a.Departement "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.Division ='" & Session("sDeptID") & "' AND a.Departement ='" & Session("sSubDeptID") & "' "
        sSQL = sSQL & " and a.IsActive='" & "Y" & "'"
        sSQL += " order by a.SubDept asc"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbSubDev.Text = oSQLReader.Item("DescDiv").ToString
            tbSubDept.Text = oSQLReader.Item("DescDept").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub

    Sub saveGLSubDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveSub.Checked = True Then
            active = "Y"
        ElseIf cbActiveSub.Checked = False Then
            active = "N"
        End If
        sSQL = "INSERT INTO iPxAcct_CoaSubDepartement(businessid,SubDept,Division,Departement,Description,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbSubID.Text, "'", "''") & "','" & Session("sDeptID") & "','" & Session("sSubDeptID") & "','" & Replace(tbSubDesc.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateGLSubDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveSub.Checked = True Then
            active = "Y"
        ElseIf cbActiveSub.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcct_CoaSubDepartement SET description='" & Replace(tbSubDesc.Text, "'", "''") & "',isactive='" & active & "'"
        sSQL = sSQL & "WHERE Division ='" & Session("sDeptID") & "' and businessid='" & Session("sBusinessID") & "' and Departement='" & Session("sSubDeptID") & "' and SubDept='" & tbSubID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub editGLSubDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaSubDepartement "
        sSQL += "WHERE SubDept ='" & tbSubID.Text & "' and Departement ='" & Session("sSubDeptID") & "' and businessid='" & Session("sBusinessID") & "' and Division='" & Session("sDeptID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbSubDesc.Text = oSQLReader.Item("description").ToString
            If oSQLReader.Item("isactive").ToString = "Y" Then
                cbActiveSub.Checked = True
            ElseIf oSQLReader.Item("isactive").ToString = "N" Then
                cbActiveSub.Checked = False
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    'SubDepartement tab end
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "GL Configuration") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryDev") = ""
            listGLDevision()
            Session("sDeptID") = ""
            Session("sSubDeptID") = ""
        End If
        UserAcces()
    End Sub

    Protected Sub lbCreateDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCreateDev.Click
        tbDevID.Text = ""
        tbDesc.Text = ""
        tbDevID.Enabled = True
        cbActiveDev.Checked = True
        lbSaveDev.Text = "<i class='fa fa-save'></i> Save"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev()", True)
    End Sub

    Protected Sub lbAbortAddDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortAddDev.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev()", True)
    End Sub

    Protected Sub lbSaveDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveDev.Click
        If tbDevID.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Devision ID !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev()", True)
        ElseIf tbDesc.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Devision description !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT Division FROM iPxAcct_CoaDivision WHERE businessid='" & Session("sBusinessID") & "' and Division = '" & tbDevID.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                updateGLDevision()
            Else
                oSQLReader.Close()
                saveGLDevision()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev()", True)
            listGLDevision()
        End If
    End Sub

    Protected Sub gvGLDev_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLDev.RowCommand
        If e.CommandName = "getEdit" Then
            tbDevID.Text = e.CommandArgument.ToString
            tbDevID.Enabled = False
            editGLDevision()
            lbSaveDev.Text = "<i class='fa fa-edit'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev()", True)
        ElseIf e.CommandName = "getDetailDept" Then
            Session("sDeptID") = e.CommandArgument.ToString
            Session("sSubDeptID") = ""
            listGLDepartement()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        End If
    End Sub

    Protected Sub lbQueryDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQueryDev.Click
        Session("sQuery") = "Devision"
        tbQDesc.Text = ""
        dlQStatus.Items.Clear()
        showdata_dropdownStatusQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQueryDev", "showModalQueryDev()", True)
    End Sub

    Protected Sub lbAbortQueryDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQueryDev.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQueryDev", "hideModalQueryDev()", True)
    End Sub

    Protected Sub lblQueryDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQueryDev.Click
        If Session("sQuery") = "Devision" Then
            Session("sQueryDev") = ""
            If tbQDesc.Text.Trim <> "" Then
                Session("sConditionDev") = Session("sConditionDev") & " and (Description like '%" & Replace(tbQDesc.Text, "'", "''") & "%') "
            End If
            If dlQStatus.SelectedIndex = 0 Then
                Session("sConditionDev") = Session("sConditionDev") & " and IsActive= 'Y' "
            ElseIf dlQStatus.SelectedIndex = 1 Then
                Session("sConditionDev") = Session("sConditionDev") & " and IsActive = 'Y' or IsActive = 'N' "
            ElseIf dlQStatus.SelectedIndex = 2 Then
                Session("sConditionDev") = Session("sConditionDev") & " and IsActive = 'N'"
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQueryDev", "hideModalQueryDev()", True)
            listGLDevision()
        ElseIf Session("sQuery") = "Dept" Then
            Session("sQueryDept") = ""
            If tbQDesc.Text.Trim <> "" Then
                Session("sConditionDept") = Session("sConditionDept") & " and (a.Description like '%" & Replace(tbQDesc.Text, "'", "''") & "%') "
            End If
            If dlQStatus.SelectedIndex = 0 Then
                Session("sConditionDept") = Session("sConditionDept") & " and a.IsActive= 'Y' "
            ElseIf dlQStatus.SelectedIndex = 1 Then
                Session("sConditionDept") = Session("sConditionDept") & " and a.IsActive = 'Y' or a.IsActive = 'N' "
            ElseIf dlQStatus.SelectedIndex = 2 Then
                Session("sConditionDept") = Session("sConditionDept") & " and a.IsActive = 'N'"
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQueryDev", "hideModalQueryDev()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
            listGLDepartement()
        ElseIf Session("sQuery") = "SubDept" Then
            Session("sQuerySubDept") = ""
            If tbQDesc.Text.Trim <> "" Then
                Session("sConditionSubDept") = Session("sConditionSubDept") & " and (a.Description like '%" & Replace(tbQDesc.Text, "'", "''") & "%') "
            End If
            If dlQStatus.SelectedIndex = 0 Then
                Session("sConditionSubDept") = Session("sConditionSubDept") & " and a.IsActive= 'Y' "
            ElseIf dlQStatus.SelectedIndex = 1 Then
                Session("sConditionSubDept") = Session("sConditionSubDept") & " and a.IsActive = 'Y' or a.IsActive = 'N' "
            ElseIf dlQStatus.SelectedIndex = 2 Then
                Session("sConditionSubDept") = Session("sConditionSubDept") & " and a.IsActive = 'N'"
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQueryDev", "hideModalQueryDev()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
            listGLSubDepartement()
        End If
    End Sub

    Protected Sub lbCreateDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCreateDept.Click
        If Session("sDeptID") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Devision first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DevActive", "DevActive()", True)
        Else
            tbDeptID.Text = ""
            tbDescDept.Text = ""
            tbDeptID.Enabled = True
            cbActiveDept.Checked = True
            tampilGLDevision()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        End If
    End Sub

    Protected Sub lbQueryDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQueryDept.Click
        If Session("sDeptID") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Devision first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DevActive", "DevActive()", True)
        Else
            Session("sQuery") = "Dept"
            tbQDesc.Text = ""
            dlQStatus.Items.Clear()
            showdata_dropdownStatusQuery()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQueryDev", "showModalQueryDev()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        End If
    End Sub

    Protected Sub lbQuerySubDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuerySubDept.Click
        If Session("sDeptID") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Devision first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DevActive", "DevActive()", True)
        ElseIf Session("sSubDeptID") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Departement first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        Else
            Session("sQuery") = "SubDept"
            tbQDesc.Text = ""
            dlQStatus.Items.Clear()
            showdata_dropdownStatusQuery()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQueryDev", "showModalQueryDev()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
        End If
    End Sub

    Protected Sub lbAbortAddDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortAddDept.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
    End Sub

    Protected Sub lbSaveDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveDept.Click
        If tbDeptID.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Departement ID !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        ElseIf tbDescDept.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Departement description !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT Departement FROM iPxAcct_CoaDepartement WHERE businessid='" & Session("sBusinessID") & "' and Departement='" & tbDeptID.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                updateGLDepartement()
            Else
                oSQLReader.Close()
                saveGLDepartement()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
            listGLDepartement()
        End If
    End Sub

    Protected Sub gvGLDept_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLDept.RowCommand
        If e.CommandName = "getDetailSubDept" Then
            Session("sSubDeptID") = e.CommandArgument
            listGLSubDepartement()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
        ElseIf e.CommandName = "getEdit" Then
            tbDeptID.Text = e.CommandArgument.ToString
            tbDeptID.Enabled = False
            tampilGLDevision()
            editGLDepartement()
            lbSaveDept.Text = "<i class='fa fa-edit'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        End If
    End Sub

    Protected Sub lbCreateSubDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCreateSubDept.Click
        If Session("sDeptID") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Devision first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DevActive", "DevActive()", True)
        ElseIf Session("sSubDeptID") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Departement first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DeptActive", "DeptActive()", True)
        Else
            tbSubID.Text = ""
            tbSubDesc.Text = ""
            tbSubID.Enabled = True
            tampilGLDept()
            cbActiveSub.Checked = True
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
        End If
    End Sub

    Protected Sub lbAbortSub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortSub.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
    End Sub

    Protected Sub lbSaveSub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveSub.Click
        If tbSubID.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Departement ID !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
        ElseIf tbSubDesc.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Departement description !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT SubDept FROM iPxAcct_CoaSubDepartement WHERE businessid='" & Session("sBusinessID") & "' and SubDept='" & tbSubID.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                updateGLSubDepartement()
            Else
                oSQLReader.Close()
                saveGLSubDepartement()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
            listGLSubDepartement()
        End If
    End Sub

    Protected Sub gvGLSubDept_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLSubDept.RowCommand
        If e.CommandName = "getEdit" Then
            tbSubID.Text = e.CommandArgument.ToString
            tbSubID.Enabled = False
            tampilGLDept()
            editGLSubDepartement()
            lbSaveSub.Text = "<i class='fa fa-edit'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubDeptActive", "SubDeptActive()", True)
        End If
    End Sub
End Class
