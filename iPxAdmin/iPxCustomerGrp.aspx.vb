Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxCustomerGrp
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub showdata_dropdownCustStatus()
        dlStatus.Items.Insert(0, "")
        dlStatus.Items.Insert(1, "All Customer")
        dlStatus.Items.Insert(2, "Non Customer")
        dlStatus.Items.Insert(3, "Customer")
    End Sub
    Sub ListGrup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CustomerGrp where businessid ='" & Session("sBusinessID") & "' and "
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
        sSQL += " order by arGroup asc"
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
    Sub saveGroup()
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
        sSQL = "INSERT INTO iPxAcctAR_Cfg_CustomerGrp(businessid, arGroup, Description, isActive) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Session("sGroupAR") & "','" & Replace(tbDescription.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('saved successfully!');", True)
        ListGrup()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub
    Sub editGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CustomerGrp "
        sSQL += "WHERE arGroup ='" & Session("sBiEdit") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        'usercode, mobileno, password, signupdate, fullname, status, quid
        If oSQLReader.HasRows Then
            tbDescription.Text = oSQLReader.Item("Description").ToString
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
    Sub updateGroup()
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
        sSQL = "UPDATE iPxAcctAR_Cfg_CustomerGrp SET Description='" & Replace(tbDescription.Text, "'", "''") & "', isActive='" & active & "'"
        sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and arGroup ='" & Session("sBiEdit") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Data has been update !');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ListGrup()
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
            ListGrup()
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGrup.PageIndex = e.NewPageIndex
        Me.ListGrup()
    End Sub

    Protected Sub gvGrup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGrup.PageIndexChanging
        gvGrup.PageIndex = e.NewPageIndex
        ListGrup()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGrup.PageIndex = e.NewPageIndex
        Me.ListGrup()
    End Sub
    Protected Sub lbAddGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddGroup.Click
        Session("sGroupAR") = cIpx.GetCounterARG("G", "G")
        tbDescription.Text = ""
        cbActive.Checked = True
        lbSave.Text = "<i class='fa fa-save'></i> Save"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
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
            sSQL = "SELECT arGroup FROM iPxAcctAR_Cfg_CustomerGrp WHERE businessid='" & Session("sBusinessID") & "' and arGroup = '" & Session("sGroupAR") & "'"
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

    Protected Sub gvGrup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGrup.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sBiEdit") = e.CommandArgument.ToString
            editGroup()
            lbSave.Text = "<i class='fa fa-save'></i> Update"
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        dlStatus.Items.Clear()
        showdata_dropdownCustStatus()
        tbQArGroup.Text = ""
        tbQBusinessId.Text = ""
        tbQDescription.Text = ""
        dlStatus.SelectedIndex = 0
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        Session("sCondition") = ""
        Session("sQueryTicket") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListGrup()
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        If dlStatus.SelectedIndex = 0 Then
            Session("sCondition") = Session("sCondition") & " IsActive='Y' "
        ElseIf dlStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " IsActive = 'Y' or IsActive = 'N' "
        ElseIf dlStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " IsActive = 'N'"
        ElseIf dlStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " IsActive = 'Y'"
        End If
        If tbQArGroup.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (arGroup like '%" & Replace(tbQArGroup.Text.Trim, "'", "''") & "%') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListGrup()
    End Sub
End Class
