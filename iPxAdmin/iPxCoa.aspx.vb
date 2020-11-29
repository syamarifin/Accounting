Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Data.OleDb
Partial Class iPxAdmin_iPxCoa
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, tipe, d_c, grpLvl, status, active As String
    Dim cIpx As New iPxClass
    Sub SaveMassUpluad()
        Dim excelPath As String = Server.MapPath("~/UploadFile/") + Path.GetFileName(flMassUpoad.PostedFile.FileName)
        flMassUpoad.SaveAs(excelPath)

        Dim connString As String = String.Empty
        Dim extension As String = Path.GetExtension(flMassUpoad.PostedFile.FileName)
        'Select Case extension
        '    Case ".xls"
        '        'Excel 97-03
        '        connString = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
        '        Exit Select
        '    Case ".xlsx"
        '        'Excel 07 or higher
        '        connString = ConfigurationManager.ConnectionStrings("Excel07+ConString").ConnectionString
        '        Exit Select
        'End Select
        If extension = ".xlsx" Then
            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & excelPath & ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'"
            connString = String.Format(connString, excelPath)
            Dim oledbConn As OleDbConnection = New OleDbConnection(connString)
            Try
                oledbConn.Open()
                Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)
                Dim oleda As OleDbDataAdapter = New OleDbDataAdapter()
                oleda.SelectCommand = cmd
                Dim ds As DataSet = New DataSet()
                oleda.Fill(ds, "inventory")
                For Each Drr As DataRow In ds.Tables(0).Rows
                    If oCnct.State = ConnectionState.Closed Then
                        oCnct.Open()
                    End If
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    If Drr(1).ToString().Trim() <> "" Then
                        Dim a As String = Drr(3).ToString()

                        sSQL = "SELECT Coa FROM iPxAcct_Coa WHERE businessid='" & Session("sBusinessID") & "' and Coa='" & Drr(0).ToString() & "'"
                        oSQLCmd.CommandText = sSQL
                        oSQLReader = oSQLCmd.ExecuteReader

                        If oSQLReader.Read Then
                            oSQLReader.Close()
                            UpdateCOAImport(Session("sBusinessID"), Drr(0).ToString(), Drr(1).ToString(), Drr(2).ToString(), Drr(3).ToString(), Drr(4).ToString(), Drr(5).ToString(), Drr(6).ToString(), Drr(7).ToString(), Drr(8).ToString(), Drr(9).ToString(), Drr(10).ToString())
                        Else
                            oSQLReader.Close()
                            saveCOAImport(Session("sBusinessID"), Drr(0).ToString(), Drr(1).ToString(), Drr(2).ToString(), Drr(3).ToString(), Drr(4).ToString(), Drr(5).ToString(), Drr(6).ToString(), Drr(7).ToString(), Drr(8).ToString(), Drr(9).ToString(), Drr(10).ToString())
                        End If
                        oCnct.Close()
                    End If
                Next
            Catch
            Finally
                oledbConn.Close()
                System.IO.File.Delete(excelPath)
            End Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('import successfull !!');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('import failed, file extension must be .xslx !!');", True)
        End If
        ListCOA()
        ListCOALia()
        ListCOACost()
        ListCOAEquity()
        ListCOARev()
        ListCOAStatistic()
    End Sub
    Public Function saveCOAImport(ByVal businessid As String, ByVal COA As String, ByVal desc As String, ByVal type As String, ByVal d_c As String, ByVal grpL As String, ByVal LvlId As String, ByVal devision As String, ByVal departemen As String, ByVal subDep As String, ByVal status As String, ByVal Note As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        'Dim oSQLReader1 As SqlDataReader
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "INSERT INTO iPxAcct_Coa(businessid,Coa, description, type, d_c, grpLevel, levelid, Devision, Departement, SubDepartement, Status, notes, isactive) "
        sSQL = sSQL & "VALUES ('" & businessid & "','" & COA & "','" & desc & "','" & type & "','" & d_c & "','" & grpL & "','" & LvlId & "','" & devision & "','" & departemen & "'"
        sSQL = sSQL & ",'" & subDep & "','" & status & "','" & Note & "','Y') "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Public Function UpdateCOAImport(ByVal businessid As String, ByVal COA As String, ByVal desc As String, ByVal type As String, ByVal d_c As String, ByVal grpL As String, ByVal LvlId As String, ByVal devision As String, ByVal departemen As String, ByVal subDep As String, ByVal status As String, ByVal Note As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        'Dim oSQLReader1 As SqlDataReader
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "UPDATE iPxAcct_Coa SET description = '" & desc & "' ,type = '" & type & "' ,d_c = '" & d_c & "' ,grpLevel = '" & grpL & "' ,levelid = '" & LvlId & "' , "
        sSQL += "Devision = '" & devision & "' ,Departement = '" & departemen & "' ,SubDepartement = '" & subDep & "' ,Status = '" & status & "' ,notes = '" & Note & "' "
        sSQL += "WHERE businessid ='" & businessid & "' and Coa='" & COA & "' "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
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
                lbAddCoa.Enabled = True
            Else
                lbAddCoa.Enabled = False
            End If
        Else
            lbAddCoa.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub ListCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "LEFT JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "LEFT JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "LEFT JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.type='Asset' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " and a.isactive = 'Y'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCoaAsset.DataSource = dt
                    gvCoaAsset.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCoaAsset.DataSource = dt
                    gvCoaAsset.DataBind()
                    gvCoaAsset.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOALia()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "LEFT JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "LEFT JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "LEFT JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.type='Liability' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " and a.isactive = 'Y'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvLia.DataSource = dt
                    gvLia.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvLia.DataSource = dt
                    gvLia.DataBind()
                    gvLia.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOAEquity()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "LEFT JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "LEFT JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "LEFT JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.type='Equity' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " and a.isactive = 'Y'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvEquity.DataSource = dt
                    gvEquity.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvEquity.DataSource = dt
                    gvEquity.DataBind()
                    gvEquity.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOARev()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "LEFT JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "LEFT JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "LEFT JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.type='Revenue' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " and a.isactive = 'Y'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvRev.DataSource = dt
                    gvRev.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvRev.DataSource = dt
                    gvRev.DataBind()
                    gvRev.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOACost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "LEFT JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "LEFT JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "LEFT JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and (a.type='Cost' or a.type='Expenses') "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " and a.isactive = 'Y'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCost.DataSource = dt
                    gvCost.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCost.DataSource = dt
                    gvCost.DataBind()
                    gvCost.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOAStatistic()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='28' and x.active='Y') as editGLConf "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "LEFT JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "LEFT JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "LEFT JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.type='Statistic' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " and a.isactive = 'Y'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                    gvStatistic.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub editCoa()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_Coa "
        sSQL += "WHERE Coa ='" & tbCoa.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            showdata_dropdownType()
            showdata_dropdownGrpLvl()
            showdata_dropdownLvl()
            showdata_dropdownStatus()
            Dim difisi As String = oSQLReader.Item("Devision").ToString
            Dim departemen As String = oSQLReader.Item("Departement").ToString
            Dim subDept As String = oSQLReader.Item("SubDepartement").ToString
            tbCoaDesc.Text = oSQLReader.Item("description").ToString
            tbNotes.Text = oSQLReader.Item("notes").ToString
            dlType.SelectedValue = oSQLReader.Item("type").ToString
            dlLevel.SelectedIndex = oSQLReader.Item("levelid").ToString
            dlStatus.SelectedValue = oSQLReader.Item("Status").ToString
            If oSQLReader.Item("grpLevel").ToString = "G" Then
                dlGrpLevel.SelectedValue = "Group"
            ElseIf oSQLReader.Item("grpLevel").ToString = "D" Then
                dlGrpLevel.SelectedValue = "Detail"
            End If
            If oSQLReader.Item("isactive").ToString = "Y" Then
                cbActiveCoa.Checked = True
            ElseIf oSQLReader.Item("isactive").ToString = "N" Then
                cbActiveCoa.Checked = False
            End If
            oCnct.Close()
            Devision()
            dlDevision.SelectedValue = Trim(difisi)
            Departement()
            dlDepartement.SelectedValue = Trim(departemen)
            SubDepartement()
            dlSubDepartement.SelectedValue = subDept
        Else
            oCnct.Close()
        End If
    End Sub
    Sub saveCoa()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveCoa.Checked = True Then
            active = "Y"
        ElseIf cbActiveCoa.Checked = False Then
            active = "N"
        End If
        If dlType.SelectedIndex = 1 Or dlType.SelectedIndex = 5 Or dlType.SelectedIndex = 6 Or dlType.SelectedIndex = 7 Then
            d_c = "D"
        ElseIf dlType.SelectedIndex = 2 Or dlType.SelectedIndex = 3 Or dlType.SelectedIndex = 4 Then
            d_c = "C"
        End If
        If dlGrpLevel.SelectedIndex = 1 Then
            grpLvl = "G"
        ElseIf dlGrpLevel.SelectedIndex = 2 Then
            grpLvl = "D"
        End If
        sSQL = "INSERT INTO iPxAcct_Coa(businessid,Coa,description,type,d_c,grpLevel,levelid,Devision,Departement,SubDepartement,Status,notes,isactive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & tbCoa.Text & "','" & Replace(tbCoaDesc.Text, "'", "''") & "','" & dlType.SelectedValue & "','" & d_c & "'"
        sSQL += ",'" & grpLvl & "','" & dlLevel.SelectedIndex & "','" & dlDevision.SelectedValue & "','" & dlDepartement.SelectedValue & "','" & dlSubDepartement.SelectedValue & "'"
        sSQL += ",'" & dlStatus.SelectedValue & "','" & Replace(tbNotes.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveCoa.Checked = True Then
            active = "Y"
        ElseIf cbActiveCoa.Checked = False Then
            active = "N"
        End If
        If dlType.SelectedIndex = 1 Or dlType.SelectedIndex = 5 Or dlType.SelectedIndex = 6 Or dlType.SelectedIndex = 7 Then
            d_c = "D"
        ElseIf dlType.SelectedIndex = 2 Or dlType.SelectedIndex = 3 Or dlType.SelectedIndex = 4 Then
            d_c = "C"
        End If
        If dlGrpLevel.SelectedIndex = 1 Then
            grpLvl = "G"
        ElseIf dlGrpLevel.SelectedIndex = 2 Then
            grpLvl = "D"
        End If
        sSQL = "UPDATE iPxAcct_Coa SET description='" & Replace(tbCoaDesc.Text, "'", "''") & "',type='" & dlType.SelectedValue & "',d_c='" & d_c & "',"
        sSQL += "grpLevel='" & grpLvl & "',levelid='" & dlLevel.SelectedIndex & "',Devision='" & dlDevision.SelectedValue & "',Departement='" & dlDepartement.SelectedValue & "',"
        sSQL += "SubDepartement='" & dlSubDepartement.SelectedValue & "',Status='" & dlStatus.SelectedValue & "',notes='" & tbNotes.Text & "',isactive='" & active & "'"
        sSQL = sSQL & "WHERE Coa ='" & tbCoa.Text & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    'Query
    Sub showdata_dropdownGrpLvlQuery()
        dlQGrpLevel.Items.Clear()
        dlQGrpLevel.Items.Insert(0, "")
        dlQGrpLevel.Items.Insert(1, "Group")
        dlQGrpLevel.Items.Insert(2, "Detail")
    End Sub
    Sub showdata_dropdownLvlQuery()
        dlQLevel.Items.Clear()
        dlQLevel.Items.Insert(0, "")
        dlQLevel.Items.Insert(1, "Account type")
        dlQLevel.Items.Insert(2, "Account Group")
        dlQLevel.Items.Insert(3, "Account Sub Group 1")
        dlQLevel.Items.Insert(4, "Account Sub Group 2")
        dlQLevel.Items.Insert(5, "Detail")
    End Sub
    Sub showdata_dropdownStatusQuery()
        dlQStatus.Items.Clear()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "Cash Bank Account")
        dlQStatus.Items.Insert(2, "Clearance Account")
        dlQStatus.Items.Insert(3, "System Account")
        dlQStatus.Items.Insert(4, "General")
    End Sub
    Sub showdata_dropdownStatusActiveQuery()
        dlQactive.Items.Clear()
        dlQactive.Items.Insert(0, "")
        dlQactive.Items.Insert(1, "ALL")
        dlQactive.Items.Insert(2, "NON ACTIVE")
    End Sub
    Sub DevisionQuery()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDivision where businessid = '" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQDevision.DataSource = dt
                dlQDevision.DataTextField = "Description"
                dlQDevision.DataValueField = "Division"
                dlQDevision.DataBind()
                dlQDevision.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub DepartementQuery()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDepartement where businessid = '" & Session("sBusinessID") & "' and Division ='" & dlQDevision.SelectedValue & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQDepartement.DataSource = dt
                dlQDepartement.DataTextField = "Description"
                dlQDepartement.DataValueField = "Departement"
                dlQDepartement.DataBind()
                dlQDepartement.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub SubDepartementQuery()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaSubDepartement where businessid = '" & Session("sBusinessID") & "' and Division ='" & dlQDevision.SelectedValue & "' and Departement ='" & dlQDepartement.SelectedValue & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQSubDepartement.DataSource = dt
                dlQSubDepartement.DataTextField = "Description"
                dlQSubDepartement.DataValueField = "SubDept"
                dlQSubDepartement.DataBind()
                dlQSubDepartement.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    'end Query
    Sub showdata_dropdownType()
        dlType.Items.Clear()
        dlType.Items.Insert(0, "")
        dlType.Items.Insert(1, "Asset")
        dlType.Items.Insert(2, "Liability")
        dlType.Items.Insert(3, "Equity")
        dlType.Items.Insert(4, "Revenue")
        dlType.Items.Insert(5, "Cost")
        dlType.Items.Insert(6, "Expenses")
        dlType.Items.Insert(7, "Statistic")
    End Sub
    Sub showdata_dropdownGrpLvl()
        dlGrpLevel.Items.Clear()
        dlGrpLevel.Items.Insert(0, "")
        dlGrpLevel.Items.Insert(1, "Group")
        dlGrpLevel.Items.Insert(2, "Detail")
    End Sub
    Sub showdata_dropdownLvl()
        dlLevel.Items.Clear()
        dlLevel.Items.Insert(0, "")
        dlLevel.Items.Insert(1, "Account type")
        dlLevel.Items.Insert(2, "Account Group")
        dlLevel.Items.Insert(3, "Account Sub Group 1")
        dlLevel.Items.Insert(4, "Account Sub Group 2")
        dlLevel.Items.Insert(5, "Detail")
    End Sub
    Sub showdata_dropdownStatus()
        dlStatus.Items.Clear()
        dlStatus.Items.Insert(0, "General")
        dlStatus.Items.Insert(1, "Cash Bank Account")
        dlStatus.Items.Insert(2, "Clearance Account")
        dlStatus.Items.Insert(3, "System Account")
    End Sub
    Sub Devision()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDivision where businessid = '" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlDevision.DataSource = dt
                dlDevision.DataTextField = "Description"
                dlDevision.DataValueField = "Division"
                dlDevision.DataBind()
                dlDevision.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub Departement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDepartement where businessid = '" & Session("sBusinessID") & "' and Division ='" & dlDevision.SelectedValue & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlDepartement.DataSource = dt
                dlDepartement.DataTextField = "Description"
                dlDepartement.DataValueField = "Departement"
                dlDepartement.DataBind()
                dlDepartement.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub SubDepartement()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaSubDepartement where businessid = '" & Session("sBusinessID") & "' and Division ='" & dlDevision.SelectedValue & "' and Departement ='" & dlDepartement.SelectedValue & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlSubDepartement.DataSource = dt
                dlSubDepartement.DataTextField = "Description"
                dlSubDepartement.DataValueField = "SubDept"
                dlSubDepartement.DataBind()
                dlSubDepartement.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub DivDesc()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDivision where businessid = '" & Session("sBusinessID") & "' and Division='" & dlDevision.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbDeptDev.Text = oSQLReader.Item("description").ToString
        Else

        End If
        oCnct.Close()
    End Sub
    Sub DivDescSub()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDivision where businessid = '" & Session("sBusinessID") & "' and Division='" & dlDevision.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbSubDev.Text = oSQLReader.Item("description").ToString
        Else

        End If
        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_CoaDepartement where businessid = '" & Session("sBusinessID") & "' and Division='" & dlDevision.SelectedValue & "' and Departement='" & dlDepartement.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbSubDept.Text = oSQLReader.Item("description").ToString
        Else

        End If
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
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbDevID.Text, "'", "''") & "','" & Replace(tbDescDiv.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
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
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbDeptID.Text, "'", "''") & "','" & dlDevision.SelectedValue & "','" & Replace(tbDescDept.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
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
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbSubID.Text, "'", "''") & "','" & dlDevision.SelectedValue & "','" & dlDepartement.SelectedValue & "','" & Replace(tbSubDesc.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
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
            Session("sTab") = ""
            Session("sQueryTicket") = ""
            ListCOA()
            ListCOALia()
            ListCOACost()
            ListCOAEquity()
            ListCOARev()
            ListCOAStatistic()
        End If
        UserAcces()
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoaAsset.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvLia.PageIndex = e.NewPageIndex
        Me.ListCOALia()
        gvCost.PageIndex = e.NewPageIndex
        Me.ListCOACost()
        gvEquity.PageIndex = e.NewPageIndex
        Me.ListCOAEquity()
        gvRev.PageIndex = e.NewPageIndex
        Me.ListCOARev()
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListCOAStatistic()
    End Sub

    Protected Sub gvCoaAsset_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCoaAsset.PageIndexChanging
        gvCoaAsset.PageIndex = e.NewPageIndex
        Me.ListCOA()
    End Sub

    Protected Sub gvLia_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLia.PageIndexChanging
        gvLia.PageIndex = e.NewPageIndex
        Me.ListCOALia()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
    End Sub

    Protected Sub gvCost_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCost.PageIndexChanging
        gvCost.PageIndex = e.NewPageIndex
        Me.ListCOACost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
    End Sub

    Protected Sub gvEquity_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEquity.PageIndexChanging
        gvEquity.PageIndex = e.NewPageIndex
        Me.ListCOAEquity()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
    End Sub

    Protected Sub gvRev_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRev.PageIndexChanging
        gvRev.PageIndex = e.NewPageIndex
        Me.ListCOARev()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
    End Sub

    Protected Sub gvStatistic_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStatistic.PageIndexChanging
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListCOAStatistic()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoaAsset.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvLia.PageIndex = e.NewPageIndex
        Me.ListCOALia()
        gvCost.PageIndex = e.NewPageIndex
        Me.ListCOACost()
        gvEquity.PageIndex = e.NewPageIndex
        Me.ListCOAEquity()
        gvRev.PageIndex = e.NewPageIndex
        Me.ListCOARev()
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListCOAStatistic()
    End Sub
    Protected Sub lbAddCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCoa.Click
        dlLevel.Enabled = True
        showdata_dropdownType()
        showdata_dropdownGrpLvl()
        showdata_dropdownLvl()
        showdata_dropdownStatus()
        Devision()
        dlDepartement.Items.Clear()
        dlSubDepartement.Items.Clear()
        tbCoa.Enabled = True
        cbActiveCoa.Checked = True
        lbSave.Text = "<i class='fa fa-save'></i> Save"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub lbAbortAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortAdd.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        If Session("sTab") = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
        ElseIf Session("sTab") = "Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
        ElseIf Session("sTab") = "Rev" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
        ElseIf Session("sTab") = "Lia" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
        ElseIf Session("sTab") = "Equity" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
        End If
    End Sub

    Protected Sub dlDevision_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlDevision.SelectedIndexChanged
        Departement()
        SubDepartement()
        If dlDevision.Text = "" Then
            lbAddDept.Enabled = False
        Else
            lbAddDept.Enabled = True
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub dlDepartement_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlDepartement.SelectedIndexChanged
        SubDepartement()
        If dlDepartement.Text = "" Then
            lbAddSubDept.Enabled = False
        Else
            lbAddSubDept.Enabled = True
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If tbCoa.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter COA !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf tbCoaDesc.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter COA description !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf dlType.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select type !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf dlGrpLevel.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select group level !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf dlLevel.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select level !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf dlDevision.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select devision !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf dlDepartement.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select departement !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf dlSubDepartement.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select subdepartement !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        ElseIf dlStatus.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select status !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT Coa FROM iPxAcct_Coa WHERE Coa = '" & tbCoa.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                If tbCoa.Enabled = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('COA is duplicated, please enter again !!');", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
                    tbCoa.Text = ""
                Else
                    updateCOA()
                    If Session("sTab") = "Cost" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
                    ElseIf Session("sTab") = "Statistic" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
                    ElseIf Session("sTab") = "Rev" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
                    ElseIf Session("sTab") = "Lia" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
                    ElseIf Session("sTab") = "Equity" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
                    End If
                End If
            Else
                oSQLReader.Close()
                saveCoa()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa()", True)
            ListCOA()
            ListCOALia()
            ListCOACost()
            ListCOAEquity()
            ListCOARev()
            ListCOAStatistic()
        End If
    End Sub

    Protected Sub gvCoaAsset_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoaAsset.RowCommand
        If e.CommandName = "getEdit" Then
            tbCoa.Text = e.CommandArgument.ToString
            tbCoa.Enabled = False
            editCoa()
            lbSave.Text = "<i class='fa fa-edit'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
        End If
    End Sub

    Protected Sub dlGrpLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlGrpLevel.SelectedIndexChanged
        If Me.dlGrpLevel.SelectedIndex = 1 Then
            dlLevel.SelectedIndex = 0
            dlLevel.Items(5).Attributes("disabled") = "disabled"
            dlLevel.Enabled = True
        ElseIf Me.dlGrpLevel.SelectedIndex = 2 Then
            dlLevel.SelectedIndex = 5
            dlLevel.Enabled = False
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub dlLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlLevel.SelectedIndexChanged
        If Me.dlLevel.SelectedIndex = 5 Then
            dlGrpLevel.SelectedIndex = 2
        Else
            dlGrpLevel.SelectedIndex = 1
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        showdata_dropdownGrpLvlQuery()
        showdata_dropdownLvlQuery()
        showdata_dropdownStatusQuery()
        showdata_dropdownStatusActiveQuery()
        DevisionQuery()
        tbQCoa.Text = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery();", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery();", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        Session("sQueryTicket") = ""
        If tbQCoa.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.Coa like '" & Replace(tbQCoa.Text, "'", "''") & "%') "
            
        End If
        If dlQGrpLevel.SelectedIndex = 0 Then

        ElseIf dlQGrpLevel.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.grpLevel = 'G' "
        ElseIf dlQGrpLevel.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.grpLevel = 'D' "
        End If
        If dlQLevel.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.levelid = '" & dlQLevel.SelectedIndex & "') "
        End If
        If dlQStatus.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.Status = '" & dlQStatus.SelectedValue & "') "
        End If
        If dlQDevision.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.Devision = '" & dlQDevision.SelectedValue & "') "
        End If
        If dlQDepartement.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.Departement = '" & dlQDepartement.SelectedValue & "') "
        End If
        If dlQSubDepartement.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.SubDepartement = '" & dlQSubDepartement.SelectedValue & "') "
        End If
        If dlQactive.SelectedIndex = 0 Then
            Session("sCondition") = Session("sCondition") & " and a.IsActive= 'Y' "
        ElseIf dlQactive.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.IsActive = 'Y' or a.IsActive = 'N' "
        ElseIf dlQactive.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.IsActive = 'N'"
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery();", True)
        ListCOA()
        ListCOALia()
        ListCOACost()
        ListCOAEquity()
        ListCOARev()
        ListCOAStatistic()
    End Sub

    Protected Sub dlQDevision_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlQDevision.SelectedIndexChanged
        DepartementQuery()
        SubDepartementQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery();", True)
    End Sub

    Protected Sub dlQDepartement_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlQDepartement.SelectedIndexChanged
        SubDepartementQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery();", True)
    End Sub

    Protected Sub gvCost_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCost.RowCommand
        If e.CommandName = "getEdit" Then
            tbCoa.Text = e.CommandArgument.ToString
            tbCoa.Enabled = False
            editCoa()
            lbSave.Text = "<i class='fa fa-edit'></i> Update"
            Session("sTab") = "Cost"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
        End If
    End Sub

    Protected Sub gvEquity_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEquity.RowCommand
        If e.CommandName = "getEdit" Then
            tbCoa.Text = e.CommandArgument.ToString
            tbCoa.Enabled = False
            editCoa()
            lbSave.Text = "<i class='fa fa-edit'></i> Update"
            Session("sTab") = "Equity"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
        End If
    End Sub

    Protected Sub gvLia_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLia.RowCommand
        If e.CommandName = "getEdit" Then
            tbCoa.Text = e.CommandArgument.ToString
            tbCoa.Enabled = False
            editCoa()
            lbSave.Text = "<i class='fa fa-edit'></i> Update"
            Session("sTab") = "Lia"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
        End If
    End Sub

    Protected Sub gvRev_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRev.RowCommand
        If e.CommandName = "getEdit" Then
            tbCoa.Text = e.CommandArgument.ToString
            tbCoa.Enabled = False
            editCoa()
            lbSave.Text = "<i class='fa fa-edit'></i> Update"
            Session("sTab") = "Rev"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
        End If
    End Sub

    Protected Sub gvStatistic_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvStatistic.RowCommand
        If e.CommandName = "getEdit" Then
            tbCoa.Text = e.CommandArgument.ToString
            tbCoa.Enabled = False
            editCoa()
            lbSave.Text = "<i class='fa fa-edit'></i> Update"
            Session("sTab") = "Statistic"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
        End If
    End Sub

    Protected Sub lbAddDivision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddDivision.Click
        tbDevID.Text = ""
        tbDescDiv.Text = ""
        cbActiveDev.Checked = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev();", True)
    End Sub

    Protected Sub lbAddDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddDept.Click
        DivDesc()
        tbDeptID.Text = ""
        tbDescDept.Text = ""
        cbActiveDept.Checked = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept();", True)
    End Sub

    Protected Sub lbAddSubDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddSubDept.Click
        DivDescSub()
        tbSubID.Text = ""
        tbSubDesc.Text = ""
        cbActiveSub.Checked = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddCoa", "hideModalAddCoa();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept();", True)
    End Sub

    Protected Sub lbAbortAddDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortAddDev.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub lbAbortAddDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortAddDept.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub lbAbortSub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortSub.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
    End Sub

    Protected Sub lbSaveDev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveDev.Click
        If tbDevID.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Devision ID !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev();", True)
        ElseIf tbDescDiv.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Devision description !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev();", True)
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the division id has been used !!');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDev", "showModalAddDev();", True)
            Else
                oSQLReader.Close()
                saveGLDevision()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDev", "hideModalAddDev();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
                Devision()
            End If
        End If
    End Sub

    Protected Sub lbSaveDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveDept.Click
        If tbDeptID.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Departement ID !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept();", True)
        ElseIf tbDescDept.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Departement description !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept();", True)
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the departement id has been used !!');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddDept", "showModalAddDept();", True)
            Else
                oSQLReader.Close()
                saveGLDepartement()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddDept", "hideModalAddDept();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
                Departement()
            End If
        End If
    End Sub

    Protected Sub lbSaveSub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveSub.Click
        If tbSubID.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter SubDepartement ID !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept();", True)
        ElseIf tbSubDesc.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter SubDepartement description !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept();", True)
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the departement id has been used !!');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddSubDept", "showModalAddSubDept();", True)
            Else
                oSQLReader.Close()
                saveGLSubDepartement()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddSubDept", "hideModalAddSubDept();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddCoa", "showModalAddCoa();", True)
                SubDepartement()
            End If
        End If
    End Sub

    Protected Sub lbImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbImport.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalImport", "showModalImport();", True)
    End Sub

    Protected Sub lbAbortImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortImport.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalImport", "hideModalImport();", True)
        If Session("sTab") = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
        ElseIf Session("sTab") = "Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
        ElseIf Session("sTab") = "Rev" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
        ElseIf Session("sTab") = "Lia" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
        ElseIf Session("sTab") = "Equity" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
        End If
    End Sub

    Protected Sub lbStartImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStartImport.Click
        SaveMassUpluad()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalImport", "hideModalImport();", True)
    End Sub

    Protected Sub lbPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint.Click
        Session("sReport") = "GLCOA"
        Session("sMapPath") = "~/iPxReportFile/dckGL_COA.rpt"
        Response.Redirect("rptviewer.aspx")
    End Sub
End Class
