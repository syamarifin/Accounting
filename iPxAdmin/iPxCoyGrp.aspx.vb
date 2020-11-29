Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxCoyGrp
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub kosong()
        tbAddress.Text = ""
        tbContact.Text = ""
        tbDescription.Text = ""
        tbNotes.Text = ""
        tbPhone.Text = ""
        cbActive.Checked = True
    End Sub
    Sub kosongQuery()
        tbQCoyGroup.Text = ""
        tbQAddress.Text = ""
        tbQContact.Text = ""
        tbQDescription.Text = ""
        dlQStatus.SelectedIndex = 0
    End Sub
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "All")
        dlQStatus.Items.Insert(2, "Non Coy Group")
        dlQStatus.Items.Insert(3, "Coy Group")
    End Sub
    Sub ListCoyGrup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CoyGroup where businessid ='" & Session("sBusinessID") & "' and "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by Description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGrup.DataSource = dt
                    gvGrup.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGrup.DataSource = dt
                    gvGrup.DataBind()
                    gvGrup.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub saveCoyGroup()
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

        sSQL = "INSERT INTO iPxAcctAR_Cfg_CoyGroup(businessid,CoyGroup,Description,Contact,Address,Phone,Notes,isActive) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbCoyGroup.Text, "'", "''") & "','" & Replace(tbDescription.Text, "'", "''") & "'"
        sSQL = sSQL & ",'" & Replace(tbContact.Text, "'", "''") & "','" & Replace(tbAddress.Text, "'", "''") & "','" & Replace(tbPhone.Text, "'", "''") & "'"
        sSQL = sSQL & ",'" & Replace(tbNotes.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('saved successfully!');", True)
        ListCoyGrup()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub
    Sub editCoyGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CoyGroup "
        sSQL += "WHERE CoyGroup ='" & Session("sBiEdit") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbCoyGroup.Text = oSQLReader.Item("CoyGroup").ToString
            tbDescription.Text = oSQLReader.Item("Description").ToString
            tbContact.Text = oSQLReader.Item("Contact").ToString
            tbAddress.Text = oSQLReader.Item("Address").ToString
            tbPhone.Text = oSQLReader.Item("Phone").ToString
            tbNotes.Text = oSQLReader.Item("Notes").ToString
            Dim Active As String = oSQLReader.Item("isActive").ToString
            If Active = "Y" Then
                cbActive.Checked = True
            Else
                cbActive.Checked = False
            End If
            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            oCnct.Close()
        End If
    End Sub
    Sub updateCoyGroup()
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
        sSQL = "UPDATE iPxAcctAR_Cfg_CoyGroup SET businessid='" & Session("sBusinessID") & "', Description='" & Replace(tbDescription.Text, "'", "''") & "'"
        sSQL += ",Contact='" & Replace(tbContact.Text, "'", "''") & "',Address='" & Replace(tbAddress.Text, "'", "''") & "',Phone='" & Replace(tbPhone.Text, "'", "''") & "',Notes='" & Replace(tbNotes.Text, "'", "''") & "', isActive='" & active & "'"
        sSQL = sSQL & "WHERE CoyGroup ='" & Session("sBiEdit") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Data has been update !');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ListCoyGrup()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Configuration") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            ListCoyGrup()
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGrup.PageIndex = e.NewPageIndex
        Me.ListCoyGrup()
    End Sub

    Protected Sub gvGrup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGrup.PageIndexChanging
        gvGrup.PageIndex = e.NewPageIndex
        ListCoyGrup()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGrup.PageIndex = e.NewPageIndex
        Me.ListCoyGrup()
    End Sub
    Protected Sub lbAddCoy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCoy.Click
        tbCoyGroup.Text = cIpx.GetCounterARG("C", "C")
        kosong()
        lbSave.Text = "<i class='fa fa-save'></i> Save"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If tbDescription.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Description!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf tbContact.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Contact!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf tbAddress.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Address!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf tbPhone.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Phone!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
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
        End If
    End Sub

    Protected Sub gvGrup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGrup.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sBiEdit") = e.CommandArgument.ToString
            editCoyGroup()
            lbSave.Text = "<i class='fa fa-save'></i> Update"
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        dlQStatus.Items.Clear()
        showdata_dropdownCustStatus()
        kosongQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        Session("sCondition") = ""
        Session("sQueryTicket") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListCoyGrup()
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            Session("sCondition") = Session("sCondition") & " IsActive='Y' "
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " IsActive = 'Y' or IsActive = 'N' "
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " IsActive = 'N'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " IsActive = 'Y'"
        End If
        If tbQCoyGroup.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (CoyGroup like '%" & Replace(tbQCoyGroup.Text.Trim, "'", "''") & "%') "
        End If
        If tbQDescription.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (Description like '%" & Replace(tbQDescription.Text.Trim, "'", "''") & "%') "
        End If
        If tbQAddress.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (Address like '%" & Replace(tbQAddress.Text.Trim, "'", "''") & "%') "
        End If
        If tbQContact.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (Contact like '%" & Replace(tbQContact.Text.Trim, "'", "''") & "%') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListCoyGrup()
    End Sub
End Class
