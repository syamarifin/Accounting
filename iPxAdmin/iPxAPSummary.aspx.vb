Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxAPSummary
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forDate, a As String
    Dim cIpx As New iPxClass
    Sub listSummary()
        Dim dateBirthday As Date = tbDate.Text
        forDate = dateBirthday.ToString("yyy-MM-")
        a = dateBirthday.ToString("yyy-MM-dd")

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.VendorID, (c.Description) as arGroup, "
        sSQL += "a.coyName,(SELECT SUM(amount) FROM (SELECT VendorID,(SUM(iPxAcctAP_Transaction.amountcr) -SUM(iPxAcctAP_Transaction.amountdr)) AS amount "
        sSQL += "FROM iPxAcctAP_Transaction where businessid ='" & Session("sBusinessID") & "' and PVno ='' and VendorID = a.VendorID and isActive='Y' and (TransDate < '" & forDate & "01') group by VendorID UNION ALL "
        sSQL += "SELECT iPxAcctAP_Transaction.VendorID,(SUM(iPxAcctAp_Transaction.amountcr) -SUM(iPxAcctAP_Transaction.amountdr)) AS amount "
        sSQL += "FROM iPxAcctAP_PV inner join iPxAcctAP_Transaction on iPxAcctAP_Transaction.PVno=iPxAcctAP_PV.PVNo "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno <> '' and iPxAcctAP_Transaction.VendorID = a.VendorID and iPxAcctAP_Transaction.isActive='Y' "
        sSQL += "and (TransDate < '" & forDate & "01') group by iPxAcctAP_Transaction.VendorID) a GROUP BY VendorID) as open1, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT VendorID,SUM(iPxAcctAp_Transaction.amountdr) AS amount "
        sSQL += "FROM iPxAcctAP_Transaction where businessid ='" & Session("sBusinessID") & "' and PVno ='' and VendorID = a.VendorID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by VendorID UNION ALL "
        sSQL += "SELECT iPxAcctAP_Transaction.VendorID,SUM(iPxAcctAP_Transaction.amountdr) AS amount "
        sSQL += "FROM iPxAcctAP_PV inner join iPxAcctAP_Transaction on iPxAcctAP_Transaction.PVno=iPxAcctAP_PV.PVNo "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno <> '' and iPxAcctAP_Transaction.VendorID = a.VendorID and iPxAcctAP_Transaction.isActive='Y' "
        sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAP_Transaction.VendorID) a GROUP BY VendorID) as debit, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT VendorID,SUM(iPxAcctAp_Transaction.amountcr) AS amount "
        sSQL += "FROM iPxAcctAP_Transaction where businessid ='" & Session("sBusinessID") & "' and PVno ='' and VendorID = a.VendorID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by VendorID UNION ALL "
        sSQL += "SELECT iPxAcctAP_Transaction.VendorID,SUM(iPxAcctAP_Transaction.amountcr) AS amount "
        sSQL += "FROM iPxAcctAP_PV inner join iPxAcctAP_Transaction on iPxAcctAP_Transaction.PVno=iPxAcctAP_PV.PVNo "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno <> '' and iPxAcctAP_Transaction.VendorID = a.VendorID and iPxAcctAP_Transaction.isActive='Y' "
        sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAP_Transaction.VendorID) a GROUP BY VendorID) as credit "
        sSQL += "from iPxAcctAP_Cfg_Vendor as a "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_VendorGrp as c ON c.businessid=a.businessid and c.apGroup=a.apGroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += "ORDER BY c.Description, a.CoyName"
        'sSQL += " order by b.Description asc, a.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvSummary.DataSource = dt
                    gvSummary.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim totalOpen, totalDebit, totalKredit As Decimal
                    If dt.Compute("Sum(open1)", "").ToString() <> "" Then
                        totalOpen = dt.Compute("Sum(open1)", "").ToString()
                    Else
                        totalOpen = 0
                    End If
                    If dt.Compute("Sum(debit)", "").ToString() <> "" Then
                        totalDebit = dt.Compute("Sum(debit)", "").ToString()
                    Else
                        totalDebit = 0
                    End If
                    If dt.Compute("Sum(credit)", "").ToString() <> "" Then
                        totalKredit = dt.Compute("Sum(credit)", "").ToString()
                    Else
                        totalKredit = 0
                    End If
                    gvSummary.FooterRow.Cells(0).Text = "Grand Total"
                    gvSummary.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Left
                    gvSummary.FooterRow.Cells(1).Text = totalOpen.ToString("N2")
                    gvSummary.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
                    gvSummary.FooterRow.Cells(2).Text = totalDebit.ToString("N2")
                    gvSummary.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
                    gvSummary.FooterRow.Cells(3).Text = totalKredit.ToString("N2")
                    gvSummary.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvSummary.FooterRow.Cells(7).Text = (Val(totalOpen) - Val(totalDebit) + Val(totalKredit)).ToString("N2")
                    gvSummary.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvSummary.FooterRow.Cells(4).Visible = False
                    gvSummary.FooterRow.Cells(5).Visible = False
                    gvSummary.FooterRow.Cells(6).Visible = False
                    gvSummary.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvSummary.DataSource = dt
                    gvSummary.DataBind()
                    gvSummary.Rows(0).Visible = False
                    gvSummary.FooterRow.Cells(0).Visible = False
                    gvSummary.FooterRow.Cells(1).Visible = False
                    gvSummary.FooterRow.Cells(2).Visible = False
                    gvSummary.FooterRow.Cells(3).Visible = False
                    gvSummary.FooterRow.Cells(4).Visible = False
                    gvSummary.FooterRow.Cells(5).Visible = False
                    gvSummary.FooterRow.Cells(6).Visible = False
                    gvSummary.FooterRow.Cells(7).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listSummaryCL()
        'Dim dateBirthday As Date = tbDateCL.Text
        'forDate = dateBirthday.ToString("yyy-MM-")
        'a = dateBirthday.ToString("yyy-MM-dd")

        'If oCnct.State = ConnectionState.Closed Then
        '    oCnct.Open()
        'End If
        'oSQLCmd = New SqlCommand(sSQL, oCnct)
        'sSQL = "select a.CustomerID, (c.Description) as arGroup, (b.Description) AS GroupCoy, "
        'sSQL += "a.coyName,(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS amount "
        'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and isActive='Y' and (TransDate < '" & forDate & "01') group by CustomerID UNION ALL "
        'sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS amount "
        'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        'sSQL += "and (TransDate < '" & forDate & "01') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) as open1, "
        ''sSQL += "a.CoyName, (SELECT SUM(amountdr)-SUM(amountcr) FROM iPxAcctAR_Transaction where CustomerID = a.CustomerID and TransDate <'" & forDate & "01') as open1,"
        ''sSQL += "(SELECT SUM(amountdr) FROM iPxAcctAR_Transaction where CustomerID = a.CustomerID and TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') as debit, "
        ''sSQL += "(SELECT SUM(amountcr) FROM iPxAcctAR_Transaction where CustomerID = a.CustomerID and TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') as credit "
        'sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,SUM(iPxAcctAR_Transaction.amountdr) AS amount "
        'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by CustomerID UNION ALL "
        'sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,SUM(iPxAcctAR_Transaction.amountdr) AS amount "
        'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        'sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) as debit, "
        'sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,SUM(iPxAcctAR_Transaction.amountcr) AS amount "
        'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by CustomerID UNION ALL "
        'sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,SUM(iPxAcctAR_Transaction.amountcr) AS amount "
        'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        'sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) as credit "
        'sSQL += "from iPxAcctAR_Cfg_Customer as a inner join iPxAcctAR_Cfg_CoyGroup as b on b.CoyGroup = a.CoyGroup "
        'sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp as c ON c.businessid=a.businessid and c.arGroup=a.arGroup "
        'sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.arGroup<>'CC' "
        'If Session("sQueryTicket") = "" Then
        '    Session("sQueryTicket") = Session("sCondition")
        '    If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
        '        sSQL = sSQL & Session("sQueryTicket")
        '        Session("sCondition") = ""
        '    Else
        '        sSQL = sSQL & " "
        '    End If
        'Else
        '    sSQL = sSQL & Session("sQueryTicket")
        '    Session("sCondition") = ""
        'End If
        'sSQL += "ORDER BY c.Description, b.Description,a.CoyName"
        ''sSQL += " order by b.Description asc, a.CoyName asc"
        'Using sda As New SqlDataAdapter()
        '    oSQLCmd.CommandText = sSQL
        '    sda.SelectCommand = oSQLCmd
        '    Using dt As New DataTable()
        '        sda.Fill(dt)
        '        If dt.Rows.Count <> 0 Then
        '            gvSummaryCL.DataSource = dt
        '            gvSummaryCL.DataBind()
        '            'Calculate Sum and display in Footer Row
        '            Dim totalOpen, totalDebit, totalKredit As Decimal
        '            If dt.Compute("Sum(open1)", "").ToString() <> "" Then
        '                totalOpen = dt.Compute("Sum(open1)", "").ToString()
        '            Else
        '                totalOpen = 0
        '            End If
        '            If dt.Compute("Sum(debit)", "").ToString() <> "" Then
        '                totalDebit = dt.Compute("Sum(debit)", "").ToString()
        '            Else
        '                totalDebit = 0
        '            End If
        '            If dt.Compute("Sum(credit)", "").ToString() <> "" Then
        '                totalKredit = dt.Compute("Sum(credit)", "").ToString()
        '            Else
        '                totalKredit = 0
        '            End If
        '            gvSummaryCL.FooterRow.Cells(0).Text = "Grand Total"
        '            gvSummaryCL.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Left
        '            gvSummaryCL.FooterRow.Cells(1).Text = totalOpen.ToString("N2")
        '            gvSummaryCL.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
        '            gvSummaryCL.FooterRow.Cells(2).Text = totalDebit.ToString("N2")
        '            gvSummaryCL.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
        '            gvSummaryCL.FooterRow.Cells(3).Text = totalKredit.ToString("N2")
        '            gvSummaryCL.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
        '            gvSummaryCL.FooterRow.Cells(7).Text = (Val(totalOpen) + Val(totalDebit) - Val(totalKredit)).ToString("N2")
        '            gvSummaryCL.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
        '            gvSummaryCL.FooterRow.Cells(4).Visible = False
        '            gvSummaryCL.FooterRow.Cells(5).Visible = False
        '            gvSummaryCL.FooterRow.Cells(6).Visible = False
        '            gvSummaryCL.Enabled = True
        '        Else
        '            dt.Rows.Add(dt.NewRow())
        '            gvSummaryCL.DataSource = dt
        '            gvSummaryCL.DataBind()
        '            gvSummaryCL.Rows(0).Visible = False
        '            gvSummaryCL.FooterRow.Cells(0).Visible = False
        '            gvSummaryCL.FooterRow.Cells(1).Visible = False
        '            gvSummaryCL.FooterRow.Cells(2).Visible = False
        '            gvSummaryCL.FooterRow.Cells(3).Visible = False
        '            gvSummaryCL.FooterRow.Cells(4).Visible = False
        '            gvSummaryCL.FooterRow.Cells(5).Visible = False
        '            gvSummaryCL.FooterRow.Cells(6).Visible = False
        '            gvSummaryCL.FooterRow.Cells(7).Visible = False
        '        End If
        '    End Using
        'End Using
        'oCnct.Close()
    End Sub
    Sub clearSummary()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "DELETE FROM iPxAcctAP_Summary "
        sSQL += "WHERE RegBy ='" & Session("sUserCode") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub CreatSummary()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctAP_Summary(businessid,RegBy,VendorID,[open],debit,credit,balance) "
        sSQL += "select a.businessid,'" & Session("sUserCode") & "', a.VendorID, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT VendorID,(SUM(iPxAcctAP_Transaction.amountcr) -SUM(iPxAcctAP_Transaction.amountdr)) AS amount "
        sSQL += "FROM iPxAcctAP_Transaction where businessid ='" & Session("sBusinessID") & "' and PVno ='' and VendorID = a.VendorID and isActive='Y' and (TransDate < '" & forDate & "01') group by VendorID UNION ALL "
        sSQL += "SELECT iPxAcctAP_Transaction.VendorID,(SUM(iPxAcctAP_Transaction.amountcr) -SUM(iPxAcctAP_Transaction.amountdr)) AS amount "
        sSQL += "FROM iPxAcctAP_PV inner join iPxAcctAP_Transaction on iPxAcctAP_Transaction.PVno=iPxAcctAP_PV.PVno "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno <> '' and iPxAcctAP_Transaction.VendorID = a.VendorID and iPxAcctAP_Transaction.isActive='Y' "
        sSQL += "and (TransDate < '" & forDate & "01') group by iPxAcctAP_Transaction.VendorID) a GROUP BY VendorID) as open1, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT VendorID,SUM(iPxAcctAP_Transaction.amountdr) AS amount "
        sSQL += "FROM iPxAcctAP_Transaction where businessid ='" & Session("sBusinessID") & "' and PVno ='' and VendorID = a.VendorID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by VendorID UNION ALL "
        sSQL += "SELECT iPxAcctAP_Transaction.VendorID,SUM(iPxAcctAP_Transaction.amountdr) AS amount "
        sSQL += "FROM iPxAcctAP_PV inner join iPxAcctAP_Transaction on iPxAcctAP_Transaction.PVno=iPxAcctAP_PV.PVno "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno <> '' and iPxAcctAP_Transaction.VendorID = a.VendorID and iPxAcctAP_Transaction.isActive='Y' "
        sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAP_Transaction.VendorID) a GROUP BY VendorID) as debit, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT VendorID,SUM(iPxAcctAP_Transaction.amountcr) AS amount "
        sSQL += "FROM iPxAcctAP_Transaction where businessid ='" & Session("sBusinessID") & "' and PVno ='' and VendorID = a.VendorID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by VendorID UNION ALL "
        sSQL += "SELECT iPxAcctAP_Transaction.VendorID,SUM(iPxAcctAP_Transaction.amountcr) AS amount "
        sSQL += "FROM iPxAcctAP_PV inner join iPxAcctAP_Transaction on iPxAcctAP_Transaction.PVno=iPxAcctAP_PV.PVno "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno <> '' and iPxAcctAP_Transaction.VendorID = a.VendorID and iPxAcctAP_Transaction.isActive='Y' "
        sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAP_Transaction.VendorID) a GROUP BY VendorID) as credit, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT VendorID,(SUM(iPxAcctAP_Transaction.amountcr) -SUM(iPxAcctAP_Transaction.amountdr)) AS 'amount' "
        sSQL += "FROM iPxAcctAP_Transaction where businessid ='" & Session("sBusinessID") & "' and PVno ='' and VendorID = a.VendorID and isActive='Y' and (TransDate <= '" & a & "') group by VendorID UNION ALL "
        sSQL += "SELECT iPxAcctAP_Transaction.VendorID,(SUM(iPxAcctAP_Transaction.amountcr) -SUM(iPxAcctAP_Transaction.amountdr)) AS 'amount' "
        sSQL += "FROM iPxAcctAP_PV inner join iPxAcctAP_Transaction on iPxAcctAP_Transaction.PVno=iPxAcctAP_PV.PVno "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno <> '' and iPxAcctAP_Transaction.VendorID = a.VendorID and iPxAcctAP_Transaction.isActive='Y' "
        sSQL += "and (TransDate <= '" & a & "') group by iPxAcctAP_Transaction.VendorID) a GROUP BY VendorID) as balance "
        sSQL += "from iPxAcctAP_Cfg_Vendor as a inner join iPxAcctAP_Cfg_VendorGrp as b on b.apGroup = a.apGroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and b.apGroup=a.apGroup "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += "ORDER BY b.Description,a.CoyName"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub CreatSummaryCL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctAR_Summary(businessid,RegBy,CustomerID,[open],debit,credit,balance) "
        sSQL += "select a.businessid,'" & Session("sUserCode") & "', a.CustomerID, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS amount "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and isActive='Y' and (TransDate < '" & forDate & "01') group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS amount "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (TransDate < '" & forDate & "01') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) as open1, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,SUM(iPxAcctAR_Transaction.amountdr) AS amount "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,SUM(iPxAcctAR_Transaction.amountdr) AS amount "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) as debit, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,SUM(iPxAcctAR_Transaction.amountcr) AS amount "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and isActive='Y' and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,SUM(iPxAcctAR_Transaction.amountcr) AS amount "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) as credit, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and isActive='Y' and (TransDate <= '" & a & "') group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) as balance "
        sSQL += "from iPxAcctAR_Cfg_Customer as a inner join iPxAcctAR_Cfg_CoyGroup as b on b.CoyGroup = a.CoyGroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.arGroup<>'CC' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += "ORDER BY b.Description,a.CoyName"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub ListDetailSummaryCC()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, 'AP OPEN' as TransID, '1900-01-01' as TransDate, '' as transactiontype, '' as PVno, '' as PONo, '' as RRNo, sum(a.amountcr) as Credit, sum(a.amountdr) as Debit from iPxAcctAP_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.VendorID='" & Session("sCustSummaryCC") & "' and (TransDate < '" & forDate & "01'  and a.isactive='Y') group by a.businessid "
        sSQL += "UNION ALL "
        sSQL += "select a.businessid, a.TransID, a.TransDate, a.transactiontype, a.PVno, a.PONo, a.RRNo, a.amountcr, a.amountdr from iPxAcctAP_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.VendorID='" & Session("sCustSummaryCC") & "' "
        sSQL += "and (TransDate >= '" & forDate & "01' and TransDate <= '" & a & "' and a.isactive='Y') "
        sSQL += "order by TransDate "
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    Dim CurDebit, CurCredit As Decimal
                    If dt.Compute("Sum(Debit)", "").ToString() <> "" Then
                        CurDebit = dt.Compute("Sum(Debit)", "").ToString()
                    Else
                        CurDebit = 0
                    End If
                    If dt.Compute("Sum(Credit)", "").ToString() <> "" Then
                        CurCredit = dt.Compute("Sum(Credit)", "").ToString()
                    Else
                        CurCredit = 0
                    End If

                    gvDetail.FooterRow.Cells(5).Text = "Total"
                    gvDetail.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(6).Text = CurDebit.ToString("N2")
                    gvDetail.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(7).Text = CurCredit.ToString("N2")
                    gvDetail.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    gvDetail.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Management") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            tbDate.Text = Format(Now, "dd MMMM yyy")
            'tbDateCL.Text = Format(Now, "dd MMMM yyy")
            listSummary()
            'listSummaryCL()
        Else
            tmpCategoryName = ""
            group = 0
            listSummary()
            'listSummaryCL()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
        'gvSummaryCL.PageIndex = e.NewPageIndex
        'Me.listSummaryCL()
    End Sub

    Protected Sub gvSummary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSummary.PageIndexChanging
        gvSummary.PageIndex = e.NewPageIndex
        tmpCategoryName = ""
        listSummary()
    End Sub

    'Protected Sub gvSummaryCL_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSummaryCL.PageIndexChanging
    '    gvSummaryCL.PageIndex = e.NewPageIndex
    '    tmpCategoryName = ""
    '    listSummaryCL()
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
    'End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
        'gvSummaryCL.PageIndex = e.NewPageIndex
        'Me.listSummaryCL()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        tmpCategoryName = ""
        group = 0
        listSummary()
    End Sub
    Protected Sub cariCL(ByVal sender As Object, ByVal e As EventArgs)
        tmpCategoryName = ""
        group = 0
        listSummaryCL()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
    End Sub
    Private tmpCategoryName As String = ""
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dblOpen, dblDebit, dblKredit As Double
            strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "arGroup").ToString()
            If DataBinder.Eval(e.Row.DataItem, "open1").ToString() <> Nothing Then
                dblOpen = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "open1").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "debit").ToString() <> Nothing Then
                dblDebit = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "debit").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "credit").ToString() <> Nothing Then
                dblKredit = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "credit").ToString())
            End If

            'Dim lblTotalRevenue As Label = DirectCast(e.Row.FindControl("lblTotalRevenue"), Label)
            'lblTotalRevenue.Text = String.Format("{0:0.00}", (dblDirectRevenue + dblReferralRevenue))

            dblSubTotalOpen += dblOpen
            dblSubTotalDebit += dblDebit
            dblSubTotalKredit += dblKredit
            dblSubTotalBalance += (dblOpen - dblDebit + dblKredit)
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim open1 As String = e.Row.Cells(1).Text
            Dim debit As String = e.Row.Cells(2).Text
            Dim kredit As String = e.Row.Cells(3).Text
            Dim balance As Integer

            balance = ((Val(open1) - Val(debit)) + Val(kredit))
            e.Row.Cells(7).Text = String.Format("{0:N2}", (balance)).ToString
        End If
    End Sub
    ' To keep track of the previous row Group Identifier
    Dim strPreviousRowID As String = String.Empty
    ' To keep track the Index of Group Total
    Dim intSubTotalIndex As Integer = 1
    Dim group As Integer = 0

    ' To temporarily store Group Total
    Dim dblSubTotalOpen As Double = 0
    Dim dblSubTotalDebit As Double = 0
    Dim dblSubTotalKredit As Double = 0
    Dim dblSubTotalBalance As Double = 0
    'Protected Sub gvSummary_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSummary.RowCreated
    '    Dim IsSubTotalRowNeedToAdd As Boolean = False
    '    If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "arGroup") IsNot Nothing) Then
    '        If strPreviousRowID <> DataBinder.Eval(e.Row.DataItem, "arGroup").ToString() Then
    '            IsSubTotalRowNeedToAdd = True
    '        End If
    '    End If

    '    If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "arGroup") Is Nothing) Then
    '        IsSubTotalRowNeedToAdd = True
    '        intSubTotalIndex = 0
    '    End If

    '    If IsSubTotalRowNeedToAdd Then
    '        Dim grdViewProducts As GridView = DirectCast(sender, GridView)

    '        ' Creating a Row
    '        Dim SubTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

    '        'Adding Total Cell 
    '        Dim HeaderCell As New TableCell()
    '        HeaderCell.Text = "Sub Total"
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Left
    '        'HeaderCell.ColumnSpan = 3
    '        ' For merging first, second row cells to one
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Direct Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalOpen)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Referral Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalDebit)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Total Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalKredit)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Total Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalBalance)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding the Row at the RowIndex position in the Grid
    '        If e.Row.RowIndex > 0 Then
    '            If e.Row.RowIndex = group Then
    '                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow)
    '            ElseIf e.Row.RowIndex < group Then
    '                grdViewProducts.Controls(0).Controls.AddAt(group + intSubTotalIndex, SubTotalRow)
    '            ElseIf e.Row.RowIndex > group Then
    '                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + group + intSubTotalIndex, SubTotalRow)
    '            End If
    '        ElseIf e.Row.RowIndex <= 0 Then
    '            grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow)
    '        End If
    '        intSubTotalIndex += 1

    '        dblSubTotalOpen = 0
    '        dblSubTotalDebit = 0
    '        dblSubTotalKredit = 0
    '        dblSubTotalBalance = 0
    '        strPreviousRowID = String.Empty
    '    End If
    'End Sub

    Protected Sub lbPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint.Click
        clearSummary()
        CreatSummary()
        Session("sReport") = "ARSummary"
        'Dim dateBirthday As Date = tbDate.Text
        'Session("sForDate") = dateBirthday.ToString("yyy-MM-dd")
        'Session("sMapPath") = "~/iPxReportFile/dckAR_Summary.rpt"
        'Response.Redirect("rptviewer.aspx")
    End Sub

    Protected Sub gvSummary_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSummary.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCustSummaryCC") = e.CommandArgument
            lbTitleDetailCoa.Text = "Detail Account Payable Summary"
            Dim dateBirthday As Date = tbDate.Text
            forDate = dateBirthday.ToString("yyy-MM-")
            a = dateBirthday.ToString("yyy-MM-dd")
            Session("sForDate") = dateBirthday.ToString("MM-yyy")
            ListDetailSummaryCC()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
    End Sub

    Protected Sub lbAbortDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetail.Click
        If lbTitleDetailCoa.Text = "Detail Summary Credit Card" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub

    'Protected Sub gvSummaryCL_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSummaryCL.RowCommand
    '    If e.CommandName = "getDetail" Then
    '        Session("sCustSummaryCC") = e.CommandArgument
    '        lbTitleDetailCoa.Text = "Detail Summary Travel Agent/Company"
    '        Dim dateBirthday As Date = tbDateCL.Text
    '        forDate = dateBirthday.ToString("yyy-MM-")
    '        a = dateBirthday.ToString("yyy-MM-dd")
    '        Session("sForDate") = dateBirthday.ToString("MM-yyy")
    '        ListDetailSummaryCC()
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    '    End If
    'End Sub

    'Protected Sub lbPrintCL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrintCL.Click
    '    clearSummary()
    '    CreatSummaryCL()
    '    Session("sReport") = "ARSummary"
    '    Dim dateBirthday As Date = tbDate.Text
    '    Session("sForDate") = dateBirthday.ToString("yyy-MM-dd")
    '    Session("sMapPath") = "~/iPxReportFile/dckAR_Summary.rpt"
    '    Response.Redirect("rptviewer.aspx")
    'End Sub

    Protected Sub lbPrintDtl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrintDtl.Click
        Session("sReport") = "ARSummaryDtl"
        Session("sMapPath") = "~/iPxReportFile/dckAR_Subsidiary.rpt"
        Response.Redirect("rptviewer.aspx")
    End Sub
End Class
