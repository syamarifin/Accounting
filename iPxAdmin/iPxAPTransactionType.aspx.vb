Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxAPTransactionType
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub kosong()
        tbTransType.Text = ""
        tbCoa.Text = ""
        tbDescription.Text = ""
        cbActive.Checked = True
    End Sub
    Sub kosongQuery()
        tbQTransType.Text = ""
        tbQCoa.Text = ""
        tbQDescription.Text = ""
        dlQStatus.SelectedIndex = 0
    End Sub
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "All ")
        dlQStatus.Items.Insert(2, "Non Transaction Type")
        dlQStatus.Items.Insert(3, "Transaction Type")
    End Sub
    Sub listCoa()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_Coa where businessid ='" & Session("sBusinessID") & "' and IsActive='" & "Y" & "' "
        If Session("sQueryTicketCoa") = "" Then
            Session("sQueryTicketCoa") = Session("sConditionCoa")
            If Session("sQueryTicketCoa") <> "" Or Session("sConditionCoa") <> "" Then
                sSQL = sSQL & Session("sQueryTicketCoa")
                Session("sConditionCoa") = ""
            Else
                sSQL = sSQL & " "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicketCoa")
            Session("sConditionCoa") = ""
        End If
        sSQL += " order by Coa asc"
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
    Sub ListTransType()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_Transactiontype where businessid ='" & Session("sBusinessID") & "' and "
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
        sSQL += " order by TransactionType asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransType.DataSource = dt
                    gvTransType.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTransType.DataSource = dt
                    gvTransType.DataBind()
                    gvTransType.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub saveTransType()
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

        sSQL = "INSERT INTO iPxAcctAP_Cfg_Transactiontype (businessid, TransactionType, Description, Coa, isDefault, isActive) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbTransType.Text, "'", "''") & "','" & Replace(tbDescription.Text, "'", "''") & "'"
        sSQL = sSQL & ",'" & Replace(tbCoa.Text, "'", "''") & "','N','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('saved successfully!');", True)
        ListTransType()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub
    Sub updateTransType()
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
        sSQL = "UPDATE iPxAcctAP_Cfg_Transactiontype SET businessid='" & Session("sBusinessID") & "', Description='" & Replace(tbDescription.Text, "'", "''") & "'"
        sSQL += ",Coa='" & Replace(tbCoa.Text, "'", "''") & "', isActive='" & active & "'"
        sSQL = sSQL & "WHERE TransactionType ='" & tbTransType.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Data has been update !');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ListTransType()
    End Sub
    Sub editTransType()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_Transactiontype "
        sSQL += "WHERE TransactionType ='" & tbTransType.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbDescription.Text = oSQLReader.Item("Description").ToString
            tbCoa.Text = oSQLReader.Item("Coa").ToString
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
            ListTransType()
        End If
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvTransType.PageIndex = e.NewPageIndex
        Me.ListTransType()
    End Sub

    Protected Sub gvTransType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTransType.PageIndexChanging
        gvTransType.PageIndex = e.NewPageIndex
        ListTransType()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvTransType.PageIndex = e.NewPageIndex
        Me.ListTransType()
    End Sub
    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        dlQStatus.Items.Clear()
        showdata_dropdownCustStatus()
        kosongQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
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
        If tbQTransType.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (TransactionType like '%" & Replace(tbQTransType.Text.Trim, "'", "''") & "%') "
        End If
        If tbQDescription.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (Description like '%" & Replace(tbQDescription.Text.Trim, "'", "''") & "%') "
        End If
        If tbQCoa.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (Coa like '%" & Replace(tbQCoa.Text.Trim, "'", "''") & "%') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListTransType()
    End Sub

    Protected Sub lbAddType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddType.Click
        kosong()
        tbTransType.Enabled = True
        lbSave.Text = "<i class='fa fa-save'></i> Save"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If tbTransType.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Transaction Type!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf tbDescription.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Description!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf tbCoa.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Coa!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT TransactionType FROM iPxAcctAP_Cfg_Transactiontype WHERE businessid ='" & Session("sBusinessID") & "' and TransactionType = '" & tbTransType.Text & "' and isActive='Y'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                If tbTransType.Enabled = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Transaction type duplicate!');", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
                Else
                    updateTransType()
                End If
            Else
                oSQLReader.Close()
                saveTransType()
            End If
        End If
    End Sub

    Protected Sub gvTransType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransType.RowCommand
        If e.CommandName = "getEdit" Then
            tbTransType.Text = e.CommandArgument
            tbTransType.Enabled = False
            editTransType()
            lbSave.Text = "<i class='fa fa-save'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub

    Protected Sub lbSearchCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSearchCoa.Click
        tbSearchCOAMdl.Text = ""
        Session("sConditionCoa") = ""
        Session("sQueryTicketCoa") = ""
        listCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
    End Sub

    Protected Sub lbCloseCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub gvCoa_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoa.RowCommand
        If e.CommandName = "getSelect" Then
            tbCoa.Text = e.CommandArgument
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub

    Protected Sub lbSearchCOAMdl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSearchCOAMdl.Click
        Session("sConditionCoa") = Session("sConditionCoa") & " and (Coa like '" & Replace(tbSearchCOAMdl.Text.Trim, "'", "''") & "%')"
        listCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
    End Sub
End Class
