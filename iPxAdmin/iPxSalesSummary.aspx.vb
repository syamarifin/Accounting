Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxSalesSummary
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forDate, a, year, yearMonth, yearBudget, MonthBudget, GLid, RecID, AmountSales As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='30'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as Posting "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("Posting").ToString = "Y" Then
                lbPost.Enabled = True
            Else
                lbPost.Enabled = False
            End If
        Else
            lbPost.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub listSummarySales()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select 1 as numb, a.businessid, (a.revenuegroup) as RevGrp,a.poscode,a.description, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate = '" & a & "')as ActTd, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate >= '" & yearMonth & "01' and y.auditdate <= '" & a & "')as ActMtd, "
        sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z where z.Code=a.poscode and z.pmsID=a.businessid and z.date = '" & MonthBudget & "')as BudgetMtd, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate >= '" & year & "-01-01' and y.auditdate <= '" & a & "')as amountYtd, "
        sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z where z.Code=a.poscode and z.pmsID=a.businessid and z.date >= '01-" & year & "' and z.date <= '" & MonthBudget & "')as BudgetYtd "
        sSQL += "from iPxPMS_cfg_pos as a INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid where b.businessid='" & Session("sBusinessID") & "' and a.revenuegroup='RO' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND b.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "UNION "
        sSQL += "select 2 as numb, a.businessid, (a.revenuegroup) as RevGrp,a.poscode,a.description, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate = '" & a & "')as ActTd, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate >= '" & yearMonth & "01' and y.auditdate <= '" & a & "')as ActMtd, "
        sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z where z.Code=a.poscode and z.pmsID=a.businessid and z.date = '" & MonthBudget & "')as BudgetMtd, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate >= '" & year & "-01-01' and y.auditdate <= '" & a & "')as amountYtd, "
        sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z where z.Code=a.poscode and z.pmsID=a.businessid and z.date >= '01-" & year & "' and z.date <= '" & MonthBudget & "')as BudgetYtd "
        sSQL += "from iPxPMS_cfg_pos as a INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid where b.businessid='" & Session("sBusinessID") & "' and a.revenuegroup='FB' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND b.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "UNION "
        sSQL += "select 3 as numb, a.businessid, (a.revenuegroup) as RevGrp,a.poscode,a.description, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate = '" & a & "')as ActTd, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate >= '" & yearMonth & "01' and y.auditdate <= '" & a & "')as ActMtd, "
        sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z where z.Code=a.poscode and z.pmsID=a.businessid and z.date = '" & MonthBudget & "')as BudgetMtd, "
        sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y where y.businessid=a.businessid and y.poscode=a.PosCode and y.auditdate >= '" & year & "-01-01' and y.auditdate <= '" & a & "')as amountYtd, "
        sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z where z.Code=a.poscode and z.pmsID=a.businessid and z.date >= '01-" & year & "' and z.date <= '" & MonthBudget & "')as BudgetYtd "
        sSQL += "from iPxPMS_cfg_pos as a INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid where b.businessid='" & Session("sBusinessID") & "' and a.revenuegroup<>'RO' and a.revenuegroup<>'FB' and a.revenuegroup<>'SY' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND b.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "UNION "
        sSQL += "select 4 as numb, a.businessid,'TAX' as RevGrp,'TX' as poscode,'TAX' as description, "
        sSQL += "(select sum(y.tax) from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.revenuegroup <>'SY' AND y.businessid=a.businessid and y.auditdate = '" & a & "')as ActTd, "
        sSQL += "(select sum(y.tax) from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.revenuegroup <>'SY' AND y.businessid=a.businessid and y.auditdate >= '" & yearMonth & "01' and y.auditdate <= '" & a & "')as ActMtd, (0)as BudgetMtd, "
        sSQL += "(select sum(y.tax) from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.revenuegroup <>'SY' AND y.businessid=a.businessid and y.auditdate >= '" & year & "-01-01' and y.auditdate <= '" & a & "')as amountYtd, (0)as BudgetYtd "
        sSQL += "from iPxPMS_cfg_pos as a INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid where b.businessid='" & Session("sBusinessID") & "' and a.revenuegroup<>'SY' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND b.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "GROUP BY a.businessid "
        sSQL += "UNION "
        sSQL += "select 5 as numb, a.businessid,'SR' as RevGrp,'SRV' as poscode,'SERVICE' as description, "
        sSQL += "(select sum(y.service) from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.revenuegroup <>'SY' AND y.businessid=a.businessid and y.auditdate = '" & a & "')as ActTd, "
        sSQL += "(select sum(y.service) from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.revenuegroup <>'SY' AND y.businessid=a.businessid and y.auditdate >= '" & yearMonth & "01' and y.auditdate <= '" & a & "')as ActMtd, (0)as BudgetMtd, "
        sSQL += "(select sum(y.service) from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.revenuegroup <>'SY' AND y.businessid=a.businessid and y.auditdate >= '" & year & "-01-01' and y.auditdate <= '" & a & "')as amountYtd, (0)as BudgetYtd "
        sSQL += "from iPxPMS_cfg_pos as a INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid where b.businessid='" & Session("sBusinessID") & "' and a.revenuegroup<>'SY' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND b.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "GROUP BY a.businessid "
        sSQL += "UNION "
        sSQL += "select 6 as numb, a.businessid,'DP' as RevGrp,a.poscode,'GUEST DEPOSIT' as description, "
        sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.poscode='SYDP' AND y.businessid=a.businessid and y.auditdate = '" & a & "')as ActTd, "
        sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.poscode='SYDP' AND y.businessid=a.businessid and y.auditdate >= '" & yearMonth & "01' and y.auditdate <= '" & a & "')as ActMtd, (0)as BudgetMtd, "
        sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y inner join iPxPMS_cfg_pos as d on d.poscode=y.poscode and d.businessid=y.businessid where d.poscode='SYDP' AND y.businessid=a.businessid and y.auditdate >= '" & year & "-01-01' and y.auditdate <= '" & a & "')as amountYtd, (0)as BudgetYtd "
        sSQL += "from iPxPMS_cfg_pos as a INNER JOIN iPxAcct_FOlink as b ON b.FoLink=a.businessid where b.businessid='" & Session("sBusinessID") & "' and a.poscode='SYDP' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND b.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "UNION "
        sSQL += "select 7 as numb, a.[order], 'SET' as RevGrp, a.paymenttype, a.description, "
        sSQL += "(select sum(b.amount) from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate = '" & a & "' and b.poscode <> 'SYDP') as ActTd, "
        sSQL += "(select sum(b.amount) from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & yearMonth & "01' AND b.auditdate <= '" & a & "' and b.poscode <> 'SYDP') as ActMtd,(0.00)as BudgetMtd, "
        sSQL += "(select sum(b.amount) from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & year & "-01-01' AND b.auditdate <= '" & a & "' and b.poscode <> 'SYDP') as ActYtd,(0.00)as BudgetYtd "
        sSQL += "from iPx_profile_paymenttype as a where a.[order]='0'"

        sSQL += "UNION "
        sSQL += "select 7 as numb, a.[order], 'SET' as RevGrp, a.paymenttype, a.description, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate = '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActTd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & yearMonth & "01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActMtd,(0.00)as BudgetMtd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & year & "-01-01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActYtd,(0.00)as BudgetYtd "
        sSQL += "from iPx_profile_paymenttype as a where a.[order]='1' "

        sSQL += "UNION "
        sSQL += "select 7 as numb, a.[order], 'SET' as RevGrp, a.paymenttype, a.description, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate = '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActTd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & yearMonth & "01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActMtd,(0.00)as BudgetMtd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & year & "-01-01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActYtd,(0.00)as BudgetYtd "
        sSQL += "from iPx_profile_paymenttype as a where a.[order]='2' "

        sSQL += "UNION "
        sSQL += "select 7 as numb, a.[order], 'SET' as RevGrp, a.paymenttype, a.description, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate = '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActTd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & yearMonth & "01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActMtd,(0.00)as BudgetMtd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & year & "-01-01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActYtd,(0.00)as BudgetYtd "
        sSQL += "from iPx_profile_paymenttype as a where a.[order]='3' "

        sSQL += "UNION "
        sSQL += "select 7 as numb, a.[order], 'SET' as RevGrp, a.paymenttype, a.description, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate = '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActTd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & yearMonth & "01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActMtd,(0.00)as BudgetMtd, "
        sSQL += "(select CASE WHEN sum(b.amount) < 0 THEN sum(b.amount*-1) WHEN sum(b.amount) >=0 THEN sum(b.amount) END as amount from iPxPMS_postingrecord as b "
        sSQL += "inner join iPxAcct_FOlink as c on c.FoLink=b.businessid inner join iPxPMS_cfg_pos as d on d.businessid=b.businessid And d.poscode=b.poscode "
        sSQL += "where c.businessid='" & Session("sBusinessID") & "' "
        If dlFOLink.SelectedValue <> "ALL HOTEL" Then
            sSQL += "AND c.FoLink='" & dlFOLink.SelectedValue & "' "
        End If
        sSQL += "AND b.auditdate >= '" & year & "-01-01' AND b.auditdate <= '" & a & "' AND b.paymenttype=a.paymenttype "
        sSQL += "group by b.paymenttype) as ActYtd,(0.00)as BudgetYtd "
        sSQL += "from iPx_profile_paymenttype as a where a.[order]='4' order by numb, a.poscode"

        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvSummary.DataSource = dt
                    gvSummary.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvSummary.DataSource = dt
                    gvSummary.DataBind()
                    gvSummary.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub FOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.businessid, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON a.FoLink = b.businessid WHERE a.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOLink.DataSource = dt
                dlFOLink.DataTextField = "businessname"
                dlFOLink.DataValueField = "businessid"
                dlFOLink.DataBind()
                dlFOLink.Items.Insert(0, "ALL HOTEL")
            End Using
        End Using
    End Sub
    Sub FOLinkDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.businessid, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON a.FoLink = b.businessid WHERE a.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOLinkDetail.DataSource = dt
                dlFOLinkDetail.DataTextField = "businessname"
                dlFOLinkDetail.DataValueField = "businessid"
                dlFOLinkDetail.DataBind()
            End Using
        End Using
    End Sub

    Sub GroupDetail()
        Dim dateBirthday As Date = tbDateDetail.Text
        a = dateBirthday.ToString("yyy-MM-dd")
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.poscode, b.description "
        sSQL += "from iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.businessid=a.businessid and b.poscode=a.poscode "
        sSQL += "WHERE a.businessid='" & dlFOLinkDetail.SelectedValue & "' and a.auditdate='" & a & "'  and b.revenuegroup<>'SY' "
        sSQL += "group by a.poscode, b.description "
        sSQL += "UNION ALL "
        sSQL += "select a.poscode, b.description "
        sSQL += "from iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.businessid=a.businessid and b.poscode=a.poscode "
        sSQL += "WHERE a.businessid='" & dlFOLinkDetail.SelectedValue & "' and a.auditdate='" & a & "'  and b.revenuegroup='SY' "
        sSQL += "group by a.poscode, b.description"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlGroup.DataSource = dt
                dlGroup.DataTextField = "description"
                dlGroup.DataValueField = "poscode"
                dlGroup.DataBind()
                dlGroup.Items.Insert(0, "ALL DEPARTEMENT")
            End Using
        End Using
    End Sub

    Sub passingPrm()
        Dim cIpx As New iPxClass
        Session("sReport") = "SalesSummary"
        'Session("filename") = "dckCashierSummary_Form.rpt"
        Session("sMapPath") = "~/iPxReportFile/dckCashierSummary_Form.rpt"
        'Session("sFOLink") = txtP2.Text
        Session("sFoLink") = dlFOLink.SelectedValue
        Dim dateBirthday As Date = tbDate.Text
        Session("sDate") = dateBirthday.ToString("yyy-MM-dd")
        Response.Redirect("rptviewer.aspx")
    End Sub
    Sub cekSales()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDate.Text, "dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctGL_JVhdr where businessid='" & Session("sBusinessID") & "' and ReffNo = 'SL " & Transdate & "' and Description like '%" & dlFOLink.SelectedValue & " " & dlFOLink.SelectedItem.Text & "%' and status<>'D' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('posting failed, data has been previously posting !');document.getElementById('Buttonx').click()", True)
        Else
            oCnct.Close()
            cekImportSales()
        End If
    End Sub
    Sub cekImportSales()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDate.Text, "dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.poscode,a.description, "
        sSQL += "(select RevenueCoa from iPxAcctSales_MapPosCode where businessid='" & Session("sBusinessID") & "' and poscode=a.poscode) AS COARev, sum(y.revenue) AS revenue, "
        sSQL += "(select TaxCoa from iPxAcctSales_MapPosCode where businessid='" & Session("sBusinessID") & "' and poscode=a.poscode) AS COATax, sum(y.tax) AS tax, "
        sSQL += "(select ServiceCoa from iPxAcctSales_MapPosCode where businessid='" & Session("sBusinessID") & "' and poscode=a.poscode) AS COAService, sum(y.service) AS service from iPxPMS_postingrecord as y "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as a ON a.businessid=y.businessid and y.poscode=a.PosCode "
        sSQL += "where y.businessid='" & dlFOLink.SelectedValue & "' and y.auditdate = '" & Transdate & "' GROUP BY a.poscode,a.description  "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            GLid = cIpx.GetCounterMBR("GL", "GL")
            saveGLHeader(GLid, Session("sBusinessID"), Transdate, dlFOLink.SelectedValue & " " & dlFOLink.SelectedItem.Text)
            PostingCredit(Session("sBusinessID"), dlFOLink.SelectedValue, Transdate, GLid)
            'PostingDebit(Session("sBusinessID"), dlFOLink.SelectedValue, Transdate, GLid)
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), GLid, TransDateLog, "SL", "Posting", "Posting Sales from the " & dlFOLink.SelectedItem.Text & " on " & Transdate, dlFOLink.SelectedValue, Session("sUserCode"))
            lbPost.Enabled = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('posting successful !');document.getElementById('Buttonx').click()", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('no data can be posting !');document.getElementById('Buttonx').click()", True)
            oCnct.Close()
        End If
    End Sub
    Public Function saveGLHeader(ByVal GLid As String, ByVal businessid As String, ByVal GLDate As String, ByVal GLDesc As String) As Boolean
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd1 As SqlCommand
        Dim oSQLReader1 As SqlDataReader
        Dim sSQL1 As String

        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL1, oCnct1)

        sSQL1 = "INSERT INTO iPxAcctGL_JVhdr(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL1 += "VALUES ('" & businessid & "','" & GLid & "','" & GLDate & "', "
        sSQL1 += "'O','G1','SL " & GLDate & "','" & GLDesc & "') "
        oSQLCmd1.CommandText = sSQL1
        oSQLCmd1.ExecuteNonQuery()

        oCnct1.Close()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting header success !');document.getElementById('Buttonx').click()", True)
    End Function

    Public Function PostingCredit(ByVal businessid As String, ByVal businessfo As String, ByVal auditdate As String, ByVal TransID As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.poscode,a.description, "
        sSQL += "(select RevenueCoa from iPxAcctSales_MapPosCode where businessid='" & businessid & "' and poscode=a.poscode) AS COARev, sum(y.revenue) AS revenue, "
        sSQL += "(select TaxCoa from iPxAcctSales_MapPosCode where businessid='" & businessid & "' and poscode=a.poscode) AS COATax, sum(y.tax) AS tax, "
        sSQL += "(select ServiceCoa from iPxAcctSales_MapPosCode where businessid='" & businessid & "' and poscode=a.poscode) AS COAService, sum(y.service) AS service, "
        sSQL += "(select Coa from iPxAcctSales_MapPayment where businessid='" & businessid & "' and poscode=a.poscode and pmsID='" & businessfo & "' and paymenttype='GL') AS COAGL, "
        sSQL += "(select sum(i.amount) from iPxPMS_postingrecord AS i where i.businessid='" & businessfo & "' and i.poscode=a.poscode and i.auditdate = '" & auditdate & "' and i.paymenttype='GL' ) AS GL, "
        sSQL += "(select Coa from iPxAcctSales_MapPayment where businessid='" & businessid & "' and poscode=a.poscode and pmsID='" & businessfo & "' and paymenttype='CS') AS COACS, "
        sSQL += "(select sum(i.amount) from iPxPMS_postingrecord AS i where i.businessid='" & businessfo & "' and i.poscode=a.poscode and i.auditdate = '" & auditdate & "' and i.paymenttype='CS' ) AS CS, "
        sSQL += "(select Coa from iPxAcctSales_MapPayment where businessid='" & businessid & "' and poscode=a.poscode and pmsID='" & businessfo & "' and paymenttype='CR') AS COACR, "
        sSQL += "(select sum(i.amount) from iPxPMS_postingrecord AS i where i.businessid='" & businessfo & "' and i.poscode=a.poscode and i.auditdate = '" & auditdate & "' and i.paymenttype='CR' ) AS CR, "
        sSQL += "(select Coa from iPxAcctSales_MapPayment where businessid='" & businessid & "' and poscode=a.poscode and pmsID='" & businessfo & "' and paymenttype='CL') AS COACL, "
        sSQL += "(select sum(i.amount) from iPxPMS_postingrecord AS i where i.businessid='" & businessfo & "' and i.poscode=a.poscode and i.auditdate = '" & auditdate & "' and i.paymenttype='CL' ) AS CL, "
        sSQL += "(select Coa from iPxAcctSales_MapPayment where businessid='" & businessid & "' and poscode=a.poscode and pmsID='" & businessfo & "' and paymenttype='WB') AS COAWB, "
        sSQL += "(select sum(i.amount) from iPxPMS_postingrecord AS i where i.businessid='" & businessfo & "' and i.poscode=a.poscode and i.auditdate = '" & auditdate & "' and i.paymenttype='WB' ) AS WB "
        sSQL += "from iPxPMS_postingrecord as y "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as a ON a.businessid=y.businessid and y.poscode=a.PosCode "
        sSQL += "where y.businessid='" & businessfo & "' and y.auditdate = '" & auditdate & "' GROUP BY a.poscode,a.description  "

        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            If oSQLReader.Item("COAGL").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLDetail(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA POST TO GUEST ACCOUNT mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("GL").ToString <> "0.00" And oSQLReader.Item("GL").ToString <> "" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("GL").ToString) * -1
                    Else
                        AmountSales = oSQLReader.Item("GL").ToString
                    End If
                    GLDetailDebit(businessid, TransID, oSQLReader.Item("COAGL").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
            If oSQLReader.Item("COACS").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLHeader(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA CASH mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("CS").ToString <> "0.00" And oSQLReader.Item("CS").ToString <> "" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("CS").ToString) * -1
                    Else
                        AmountSales = oSQLReader.Item("CS").ToString
                    End If
                    GLDetailDebit(businessid, TransID, oSQLReader.Item("COACS").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
            If oSQLReader.Item("COACR").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLHeader(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA CARD mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("CR").ToString <> "0.00" And oSQLReader.Item("CR").ToString <> "" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("CR").ToString) * -1
                    Else
                        AmountSales = oSQLReader.Item("CR").ToString
                    End If
                    GLDetailDebit(businessid, TransID, oSQLReader.Item("COACR").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
            If oSQLReader.Item("COACL").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLHeader(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA CITY LEDGER mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("CL").ToString <> "0.00" And oSQLReader.Item("CL").ToString <> "" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("CL").ToString) * -1
                    Else
                        AmountSales = oSQLReader.Item("CL").ToString
                    End If
                    GLDetailDebit(businessid, TransID, oSQLReader.Item("COACL").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
            If oSQLReader.Item("COAWB").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLHeader(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA WEB BOOKING mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("WB").ToString <> "0.00" And oSQLReader.Item("WB").ToString <> "" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("WB").ToString) * -1
                    Else
                        AmountSales = oSQLReader.Item("WB").ToString
                    End If
                    GLDetailDebit(businessid, TransID, oSQLReader.Item("COAWB").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
            If oSQLReader.Item("COARev").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLHeader(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA REVENUE mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("revenue") <> "0.00" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("revenue")) * -1
                    Else
                        AmountSales = oSQLReader.Item("revenue")
                    End If
                    GLDetailCreditRev(businessid, TransID, oSQLReader.Item("COARev").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
            If oSQLReader.Item("COATax").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLHeader(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA TAX mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("tax") <> "0.00" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("tax")) * -1
                    Else
                        AmountSales = oSQLReader.Item("tax")
                    End If
                    GLDetailCreditTax(businessid, TransID, oSQLReader.Item("COATax").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
            If oSQLReader.Item("COAService").ToString = "" Then
                deleteGLHeader(TransID, businessid)
                deleteGLHeader(TransID, businessid)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting failed, COA SERVICE mapping has not been set !');document.getElementById('Buttonx').click()", True)
                Exit While
            Else
                If oSQLReader.Item("service") <> "0.00" Then
                    If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                        AmountSales = Val(oSQLReader.Item("service")) * -1
                    Else
                        AmountSales = oSQLReader.Item("service")
                    End If
                    GLDetailCreditService(businessid, TransID, oSQLReader.Item("COAService").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
                End If
            End If
        End While
        oCnct.Close()
    End Function
    Public Function PostingDebit(ByVal businessid As String, ByVal businessfo As String, ByVal auditdate As String, ByVal TransID As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.poscode,a.description, y.paymenttype, "
        sSQL += "(select Coa from iPxAcctSales_MapPayment where businessid='" & businessid & "' and pmsID='" & businessfo & "' and paymenttype=y.paymenttype and poscode=a.poscode) AS COAPayment, "
        sSQL += "(select TaxCoa from iPxAcctSales_MapPosCode where businessid='" & businessid & "' and poscode=a.poscode) AS COATax, sum(y.tax) AS tax, "
        sSQL += "sum(y.amount) AS amount from iPxPMS_postingrecord as y INNER JOIN iPxPMS_cfg_pos as a ON a.businessid=y.businessid and y.poscode=a.PosCode "
        sSQL += "where y.businessid='" & businessfo & "' and y.auditdate = '" & auditdate & "' GROUP BY y.paymenttype,a.poscode,a.description "

        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            If oSQLReader.Item("amount") <> "0.00" Then
                If oSQLReader.Item("poscode") = "SYPY" Or oSQLReader.Item("poscode") = "SYDP" Or oSQLReader.Item("poscode") = "SYPO" Or oSQLReader.Item("poscode") = "SYRF" Then
                    AmountSales = Val(oSQLReader.Item("amount")) * -1
                Else
                    AmountSales = oSQLReader.Item("amount")
                End If
                GLDetailDebit(businessid, TransID, oSQLReader.Item("COAPayment").ToString, oSQLReader.Item("description").ToString, oSQLReader.Item("poscode").ToString, AmountSales)
            End If
        End While
        oCnct.Close()
    End Function
    Sub idGLDtl()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT MAX(RecID) as RecID FROM iPxAcctGL_JVdtl where TransID = '" & Session("sTransIDGL") & "'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            RecID = Val(oSQLReader4.Item("RecID").ToString) + 1

        Else
            RecID = "1"
        End If
        oCnct4.Close()
    End Sub
    Public Function GLDetailCreditRev(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal DescDetail As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & DescDetail & "','" & Reff & "','0','" & Amount & "', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Public Function GLDetailCreditTax(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal DescDetail As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & DescDetail & "','" & Reff & "','0','" & Amount & "', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Public Function GLDetailCreditService(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal DescDetail As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & DescDetail & "','" & Reff & "','0','" & Amount & "', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Public Function GLDetailDebit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal DescDetail As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & DescDetail & "','" & Reff & "','" & Amount & "','0', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Sub cekClose()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If

        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDate.Text, "dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT * FROM iPxAcctGL_JVhdr where businessid='" & Session("sBusinessID") & "' "
        sSQL4 += "and ReffNo like 'SL " & Transdate & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            lbPost.Enabled = False
        Else
            lbPost.Enabled = True
        End If
        oCnct4.Close()
    End Sub
    Public Function deleteGLHeader(ByVal GLid As String, ByVal businessid As String) As Boolean
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd1 As SqlCommand
        Dim oSQLReader1 As SqlDataReader
        Dim sSQL1 As String

        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL1, oCnct1)

        sSQL1 = "DELETE FROM iPxAcctGL_JVhdr where businessid='" & businessid & "' and TransID='" & GLid & "' "
        oSQLCmd1.CommandText = sSQL1
        oSQLCmd1.ExecuteNonQuery()

        oCnct1.Close()
    End Function
    Public Function deleteGLDetail(ByVal GLid As String, ByVal businessid As String) As Boolean
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd1 As SqlCommand
        Dim oSQLReader1 As SqlDataReader
        Dim sSQL1 As String

        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL1, oCnct1)

        sSQL1 = "DELETE FROM iPxAcctGL_JVdtl where businessid='" & businessid & "' and TransID='" & GLid & "' "
        oSQLCmd1.CommandText = sSQL1
        oSQLCmd1.ExecuteNonQuery()

        oCnct1.Close()
    End Function
    Sub ListDatePost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim i As Integer = 2
        Dim dateBirthday As Date = "1 " + tbDatePost.Text
        Dim j As Integer = dateBirthday.ToString("MM")
        year = dateBirthday.ToString("yyy")
        Dim x As Integer = Date.DaysInMonth(year, j)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select 1 as numb, "
        sSQL += "(SELECT 'Y' FROM iPxAcctGL_JVhdr where businessid=a.businessid and ReffNo like 'SL " & j & "/1/" & year & "' and status<>'D') as Post, "
        sSQL += "(select COUNT(y.businessid) from iPxPMS_postingrecord as y "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as a ON a.businessid=y.businessid and a.poscode=y.poscode "
        sSQL += "where y.businessid='" & dlFOLink.SelectedValue & "' and y.auditdate = '" & year & "-" & j & "-01' ) as dataPost "
        sSQL += "from iPx_profile_client as a where businessid='" & Session("sBusinessID") & "' "
        Do Until (i > x)
            sSQL += "UNION ALL "
            sSQL += "select " & i & " as numb, "
            sSQL += "(SELECT 'Y' FROM iPxAcctGL_JVhdr where businessid=a.businessid and ReffNo like 'SL " & j & "/" & i & "/" & year & "' and status<>'D') as Post, "
            sSQL += "(select COUNT(y.businessid) from iPxPMS_postingrecord as y "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as a ON a.businessid=y.businessid and a.poscode=y.poscode "
            sSQL += "where y.businessid='" & dlFOLink.SelectedValue & "' and y.auditdate = '" & year & "-" & j & "-" & i & "' ) as dataPost "
            sSQL += "from iPx_profile_client as a where businessid='" & Session("sBusinessID") & "' "
            i += 1
        Loop
        sSQL += "ORDER BY numb asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDatePost.DataSource = dt
                    gvDatePost.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDatePost.DataSource = dt
                    gvDatePost.DataBind()
                    gvDatePost.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Sub ListDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim dateBirthday As Date = tbDateDetail.Text
        a = dateBirthday.ToString("yyy-MM-dd")
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT c.revenuegroup, a.poscode, a.checkno, b.roomno, "
        sSQL += "(b.lastname + b.firstname) AS GuestName, a.shift, a.paymenttype, a.cardtype, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.amount*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.amount) END as amount, a.discount, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.revenue*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.revenue) END as revenue, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.tax*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.tax) END as tax, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.service*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.service) END as service "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservation as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno  AND a.rsvpid = b.id "
        sSQL += "INNER JOIN DBO.iPxPMS_cfg_pos as c ON c.businessid=a.businessid and c.poscode=a.poscode "
        sSQL += "WHERE (a.businessid = '" & dlFOLinkDetail.SelectedValue & "') AND (a.auditdate = '" & a & "') "
        If dlGroup.SelectedValue <> "ALL DEPARTEMENT" Then
            sSQL += "AND a.poscode='" & dlGroup.SelectedValue & "' "
        Else
            sSQL += "AND a.poscode in(select a.poscode "
            sSQL += "from iPxPMS_postingrecord as a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.businessid=a.businessid and b.poscode=a.poscode "
            sSQL += "WHERE a.businessid='" & dlFOLinkDetail.SelectedValue & "' and a.auditdate='" & a & "'  and b.revenuegroup<>'SY' "
            sSQL += "UNION ALL "
            sSQL += "select a.poscode "
            sSQL += "from iPxPMS_postingrecord as a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.businessid=a.businessid and b.poscode=a.poscode "
            sSQL += "WHERE a.businessid='" & dlFOLinkDetail.SelectedValue & "' and a.auditdate='" & a & "'  and b.revenuegroup='SY' "
            sSQL += "group by a.poscode)"
        End If
        sSQL += "UNION ALL "
        sSQL += "SELECT c.revenuegroup, a.poscode, a.checkno, b.roomno, "
        sSQL += "(b.lastname + b.firstname) AS GuestName, a.shift, a.paymenttype, a.cardtype, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.amount*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.amount) END as amount, a.discount, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.revenue*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.revenue) END as revenue, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.tax*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.tax) END as tax, "
        sSQL += "CASE WHEN (c.revenuegroup) ='SY' THEN (a.service*-1) WHEN (c.revenuegroup) <> 'SY' THEN (a.service) END as service "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservationhis as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno  AND a.rsvpid = b.regno "
        sSQL += "INNER JOIN DBO.iPxPMS_cfg_pos as c ON c.businessid=a.businessid and c.poscode=a.poscode "
        sSQL += "WHERE (a.businessid = '" & dlFOLinkDetail.SelectedValue & "') AND (a.auditdate = '" & a & "') "
        If dlGroup.SelectedValue <> "ALL DEPARTEMENT" Then
            sSQL += "AND a.poscode='" & dlGroup.SelectedValue & "' "
        Else
            sSQL += "AND a.poscode in(select a.poscode "
            sSQL += "from iPxPMS_postingrecord as a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.businessid=a.businessid and b.poscode=a.poscode "
            sSQL += "WHERE a.businessid='" & dlFOLinkDetail.SelectedValue & "' and a.auditdate='" & a & "'  and b.revenuegroup<>'SY' "
            sSQL += "UNION ALL "
            sSQL += "select a.poscode "
            sSQL += "from iPxPMS_postingrecord as a "
            sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.businessid=a.businessid and b.poscode=a.poscode "
            sSQL += "WHERE a.businessid='" & dlFOLinkDetail.SelectedValue & "' and a.auditdate='" & a & "'  and b.revenuegroup='SY' "
            sSQL += "group by a.poscode)"
        End If
        sSQL += "order by c.revenuegroup,a.poscode asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvSalesDetail.DataSource = dt
                    gvSalesDetail.DataBind()
                    Dim totalAmount, totalRev, totalTax, totalService As Decimal
                    If dt.Compute("Sum(amount)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(amount)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    If dt.Compute("Sum(revenue)", "").ToString() <> "" Then
                        totalRev = dt.Compute("Sum(revenue)", "").ToString()
                    Else
                        totalRev = 0
                    End If
                    If dt.Compute("Sum(tax)", "").ToString() <> "" Then
                        totalTax = dt.Compute("Sum(tax)", "").ToString()
                    Else
                        totalTax = 0
                    End If
                    If dt.Compute("Sum(service)", "").ToString() <> "" Then
                        totalService = dt.Compute("Sum(service)", "").ToString()
                    Else
                        totalService = 0
                    End If
                    gvSalesDetail.FooterRow.Cells(6).Text = "Total"
                    gvSalesDetail.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvSalesDetail.FooterRow.Cells(7).Text = totalAmount.ToString("N2")
                    gvSalesDetail.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvSalesDetail.FooterRow.Cells(9).Text = totalRev.ToString("N2")
                    gvSalesDetail.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                    gvSalesDetail.FooterRow.Cells(10).Text = totalTax.ToString("N2")
                    gvSalesDetail.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    gvSalesDetail.FooterRow.Cells(11).Text = totalService.ToString("N2")
                    gvSalesDetail.FooterRow.Cells(11).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvSalesDetail.DataSource = dt
                    gvSalesDetail.DataBind()
                    gvSalesDetail.Rows(0).Visible = False
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
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "Sales") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            tbDate.Text = Format(DateAdd(DateInterval.Day, -1, Today), "dd MMMM yyy")
            Dim dateBirthday As Date = tbDate.Text
            forDate = dateBirthday.ToString("yyy-MM-")
            a = dateBirthday.ToString("yyy-MM-dd")
            year = dateBirthday.ToString("yyy")
            yearMonth = dateBirthday.ToString("yyy-MM-")
            MonthBudget = dateBirthday.ToString("MM-yyy")
            FOLink()
            dlFOLink.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "10")
            listSummarySales()
            'cekClose()
        Else
            tmpCategoryName = ""
            group = 0
            Dim dateBirthday As Date = tbDate.Text
            forDate = dateBirthday.ToString("yyy-MM-")
            a = dateBirthday.ToString("yyy-MM-dd")
            year = dateBirthday.ToString("yyy")
            yearMonth = dateBirthday.ToString("yyy-MM-")
            MonthBudget = dateBirthday.ToString("MM-yyy")
            listSummarySales()
            'cekClose()
        End If
        UserAcces()
        If lbPost.Enabled = True Then
            cekClose()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Dim dateBirthday As Date = tbDate.Text
        forDate = dateBirthday.ToString("yyy-MM-")
        a = dateBirthday.ToString("yyy-MM-dd")
        year = dateBirthday.ToString("yyy")
        yearMonth = dateBirthday.ToString("yyy-MM-")
        MonthBudget = dateBirthday.ToString("MM-yyy")
        tmpCategoryName = ""
        group = 0
        dblGrandTotalActTd = 0
        dblGrandTotalActMtd = 0
        dblGrandTotalActYtd = 0
        dblGrandTotalBgdMtd = 0
        dblGrandTotalBgdYtd = 0
        dblGrandTotalAvbMtd = 0
        dblGrandTotalAvbYtd = 0
        listSummarySales()
        cekClose()
    End Sub
    ' To keep track of the previous row Group Identifier
    Dim strPreviousRowID As String = String.Empty
    ' To keep track the Index of Group Total
    Dim intSubTotalIndex As Integer = 1
    Dim group As Integer = 0

    ' To temporarily store Group Total
    Dim dblSubTotalActTd As Double = 0
    Dim dblSubTotalActMtd As Double = 0
    Dim dblSubTotalActYtd As Double = 0
    Dim dblSubTotalAvbMtd As Double = 0
    Dim dblSubTotalBgdMtd As Double = 0
    Dim dblSubTotalBgdYtd As Double = 0
    Dim dblSubTotalAvbYtd As Double = 0
    ' To temporarily store Group Total
    Dim dblGrandTotalActTd As Double = 0
    Dim dblGrandTotalActMtd As Double = 0
    Dim dblGrandTotalActYtd As Double = 0
    Dim dblGrandTotalAvbMtd As Double = 0
    Dim dblGrandTotalBgdMtd As Double = 0
    Dim dblGrandTotalBgdYtd As Double = 0
    Dim dblGrandTotalAvbYtd As Double = 0
    Dim dblRevGrp As String = ""
    Dim dblRevGrand As String = ""
    Protected Sub gvSummary_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSummary.RowCreated
        Dim IsSubTotalRowNeedToAdd As Boolean = False
        Dim IsGrandTotalRowNeedToAdd As Boolean = False

        If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "numb") IsNot Nothing) Then
            If strPreviousRowID <> DataBinder.Eval(e.Row.DataItem, "numb").ToString() Then
                If DataBinder.Eval(e.Row.DataItem, "numb").ToString() <> "5" And DataBinder.Eval(e.Row.DataItem, "numb").ToString() <> "6" And DataBinder.Eval(e.Row.DataItem, "numb").ToString() <> "7" Then
                    IsSubTotalRowNeedToAdd = True
                    If DataBinder.Eval(e.Row.DataItem, "numb").ToString() = "4" Then
                        IsGrandTotalRowNeedToAdd = True
                    End If
                ElseIf DataBinder.Eval(e.Row.DataItem, "numb").ToString() = "7" Then
                    IsGrandTotalRowNeedToAdd = True
                End If
            End If
        End If

        If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "numb") Is Nothing) Then
            IsSubTotalRowNeedToAdd = True
            intSubTotalIndex = 0
        End If

        If IsSubTotalRowNeedToAdd Then
            Dim grdViewProducts As GridView = DirectCast(sender, GridView)

            ' Creating a Row
            Dim SubTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

            'Adding Total Cell 
            Dim HeaderCell As New TableCell()
            HeaderCell.Text = dblRevGrp
            HeaderCell.HorizontalAlign = HorizontalAlign.Left
            'HeaderCell.ColumnSpan = 3
            ' For merging first, second row cells to one
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Direct Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblSubTotalActTd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Referral Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblSubTotalActMtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblSubTotalBgdMtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            If dblSubTotalAvbMtd = "0" Then
                HeaderCell.Text = "0 %"
            Else
                HeaderCell.Text = String.Format("{0:N2}", dblSubTotalAvbMtd) + " %"
            End If
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Referral Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblSubTotalActYtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblSubTotalBgdYtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            If dblSubTotalAvbYtd = "0" Then
                HeaderCell.Text = "0 %"
            Else
                HeaderCell.Text = String.Format("{0:N2}", dblSubTotalAvbYtd) + "% "
            End If
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "SubTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding the Row at the RowIndex position in the Grid
            If e.Row.RowIndex > 0 Then
                If e.Row.RowIndex = group Then
                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow)
                ElseIf e.Row.RowIndex < group Then
                    grdViewProducts.Controls(0).Controls.AddAt(group + intSubTotalIndex, SubTotalRow)
                ElseIf e.Row.RowIndex > group Then
                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + group + intSubTotalIndex, SubTotalRow)
                End If
            ElseIf e.Row.RowIndex <= 0 Then
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow)
            End If
            intSubTotalIndex += 1
            'dblGrandTotalActTd += dblSubTotalActTd
            'dblGrandTotalActMtd += dblSubTotalActMtd
            'dblGrandTotalActYtd += dblSubTotalActYtd
            'dblGrandTotalBgdMtd += dblSubTotalBgdMtd
            'dblGrandTotalBgdYtd += dblSubTotalBgdYtd
            'dblGrandTotalAvbMtd += dblSubTotalAvbMtd
            'dblGrandTotalAvbYtd += dblSubTotalAvbYtd

            dblSubTotalActTd = 0
            dblSubTotalActMtd = 0
            dblSubTotalActYtd = 0
            dblSubTotalBgdMtd = 0
            dblSubTotalBgdYtd = 0
            dblSubTotalAvbMtd = 0
            dblSubTotalAvbYtd = 0
            dblRevGrp = ""
            strPreviousRowID = String.Empty
        End If
        If IsGrandTotalRowNeedToAdd Then
            Dim grdViewProducts As GridView = DirectCast(sender, GridView)
            ' Creating a Row
            Dim SubTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

            'Adding Total Cell 
            Dim HeaderCell As New TableCell()
            HeaderCell.Text = dblRevGrand
            HeaderCell.HorizontalAlign = HorizontalAlign.Left
            'HeaderCell.ColumnSpan = 3
            ' For merging first, second row cells to one
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Direct Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblGrandTotalActTd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Referral Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblGrandTotalActMtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblGrandTotalBgdMtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            If dblGrandTotalAvbMtd = "0" Then
                HeaderCell.Text = "0 %"
            Else
                HeaderCell.Text = String.Format("{0:N2}", dblGrandTotalAvbMtd) + " %"
            End If
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Referral Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblGrandTotalActYtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            HeaderCell.Text = String.Format("{0:N2}", dblGrandTotalBgdYtd)
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding Total Revenue Column
            HeaderCell = New TableCell()
            If dblGrandTotalAvbYtd = "0" Then
                HeaderCell.Text = "0 %"
            Else
                HeaderCell.Text = String.Format("{0:N2}", dblGrandTotalAvbYtd) + "% "
            End If
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.CssClass = "GrandTotalRowStyle"
            SubTotalRow.Cells.Add(HeaderCell)

            'Adding the Row at the RowIndex position in the Grid
            If e.Row.RowIndex > 0 Then
                If e.Row.RowIndex = group Then
                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow)
                ElseIf e.Row.RowIndex < group Then
                    grdViewProducts.Controls(0).Controls.AddAt(group + intSubTotalIndex, SubTotalRow)
                ElseIf e.Row.RowIndex > group Then
                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + group + intSubTotalIndex, SubTotalRow)
                End If
            ElseIf e.Row.RowIndex <= 0 Then
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow)
            End If
            intSubTotalIndex += 1
            If dblRevGrand = "GRAND TOTAL" Then
                dblGrandTotalActTd = 0
                dblGrandTotalActMtd = 0
                dblGrandTotalActYtd = 0
                dblGrandTotalBgdMtd = 0
                dblGrandTotalBgdYtd = 0
                dblGrandTotalAvbMtd = 0
                dblGrandTotalAvbYtd = 0

                dblSubTotalActTd = 0
                dblSubTotalActMtd = 0
                dblSubTotalActYtd = 0
                dblSubTotalBgdMtd = 0
                dblSubTotalBgdYtd = 0
                dblSubTotalAvbMtd = 0
                dblSubTotalAvbYtd = 0
            End If
            dblRevGrand = ""
            strPreviousRowID = String.Empty
        End If
    End Sub
    Private tmpCategoryName As String = ""
    Private tmpHeaderName As String = ""
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dblActTd, dblActMtd, dblBdgMtd, dblActYtd, dblBdgYtd As Double
            strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "numb").ToString()
            If DataBinder.Eval(e.Row.DataItem, "numb").ToString() = "1" Then
                dblRevGrp = "SUB TOTAL ROOM REVENUE"
            ElseIf DataBinder.Eval(e.Row.DataItem, "numb").ToString() = "2" Then
                dblRevGrp = "SUB TOTAL FOOD AND BEVERAGE"
            ElseIf DataBinder.Eval(e.Row.DataItem, "numb").ToString() = "3" Then
                dblRevGrp = "SUB TOTAL OTHER"
                dblRevGrand = "TOTAL REVENUE"
            ElseIf DataBinder.Eval(e.Row.DataItem, "numb").ToString() = "4" Then
                dblRevGrp = "GUEST DEPOSIT"
                dblRevGrand = "GRAND TOTAL"
            ElseIf DataBinder.Eval(e.Row.DataItem, "numb").ToString() = "7" Then
                dblRevGrp = "SUB TOTAL SETTLEMENT"
            End If
            If DataBinder.Eval(e.Row.DataItem, "ActTd").ToString() <> Nothing Then
                dblActTd = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "ActTd").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "ActMtd").ToString() <> Nothing Then
                dblActMtd = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "ActMtd").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "BudgetMtd").ToString() <> Nothing Then
                dblBdgMtd = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "BudgetMtd").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "amountYtd").ToString() <> Nothing Then
                dblActYtd = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amountYtd").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "BudgetYtd").ToString() <> Nothing Then
                dblBdgYtd = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "BudgetYtd").ToString())
            End If

            

            dblSubTotalActTd += dblActTd
            dblSubTotalActMtd += dblActMtd
            dblSubTotalBgdMtd += dblBdgMtd
            If dblSubTotalBgdMtd = "0" Then
                dblSubTotalAvbMtd += 0
            Else
                dblSubTotalAvbMtd = ((dblSubTotalActMtd / dblSubTotalBgdMtd) * 100)
            End If
            dblSubTotalActYtd += dblActYtd
            dblSubTotalBgdYtd += dblBdgYtd
            If dblSubTotalBgdYtd = "0" Then
                dblSubTotalAvbYtd += 0
            Else
                dblSubTotalAvbYtd = ((dblSubTotalActYtd / dblSubTotalBgdYtd) * 100)
            End If


            'Grand Total

            dblGrandTotalActTd += dblActTd
            dblGrandTotalActMtd += dblActMtd
            dblGrandTotalBgdMtd += dblBdgMtd
            If dblGrandTotalBgdMtd = "0" Then
                dblGrandTotalAvbMtd += 0
            Else
                dblGrandTotalAvbMtd = ((dblGrandTotalActMtd / dblGrandTotalBgdMtd) * 100)
            End If
            dblGrandTotalActYtd += dblActYtd
            dblGrandTotalBgdYtd += dblBdgYtd
            If dblGrandTotalBgdYtd = "0" Then
                dblGrandTotalAvbYtd += 0
            Else
                dblGrandTotalAvbYtd = ((dblGrandTotalActYtd / dblGrandTotalBgdYtd) * 100)
            End If
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If drv("RevGrp").ToString() = "TAX" Or drv("RevGrp").ToString() = "SR" Or drv("RevGrp").ToString() = "DP" Then
            Else
                If tmpCategoryName <> drv("numb").ToString() Then
                    tmpCategoryName = drv("numb").ToString()
                    If drv("RevGrp").ToString() = "RO" Then
                        tmpHeaderName = "ROOM REVENUE"
                    ElseIf drv("RevGrp").ToString() = "FB" Then
                        tmpHeaderName = "FOOD AND BEVERAGE"
                    ElseIf drv("RevGrp").ToString() = "TAX" Then
                        tmpHeaderName = "TAX"
                    ElseIf drv("RevGrp").ToString() = "SET" Then
                        tmpHeaderName = "SETTLEMENT"
                    ElseIf drv("RevGrp").ToString() <> "RO" Or drv("RevGrp").ToString() <> "FB" Or drv("RevGrp").ToString() = "SY" Then
                        tmpHeaderName = "OTHER"
                    End If
                    Dim tbl As Table = TryCast(e.Row.Parent, Table)

                    If tbl IsNot Nothing Then
                        Dim row As GridViewRow = New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                        Dim cell As TableCell = New TableCell()
                        cell.ColumnSpan = Me.gvSummary.Columns.Count
                        cell.Width = Unit.Percentage(100)
                        cell.Style.Add("Font-weight", "bold")
                        cell.Style.Add("background-color", "#fffff")
                        cell.Style.Add("color", "black")
                        cell.Style.Add("text-transform", "uppercase")
                        Dim span As HtmlGenericControl = New HtmlGenericControl("span")

                        span.InnerHtml = tmpHeaderName
                        cell.Controls.Add(span)
                        row.Cells.Add(cell)
                        tbl.Rows.AddAt(tbl.Rows.Count - 1, row)
                        group += 1
                    End If
                End If
            End If
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ActMtd As String = e.Row.Cells(1).Text
            Dim BdgMtd As String = e.Row.Cells(2).Text
            Dim ActYtd As String = e.Row.Cells(3).Text
            Dim BdgYtd As String = e.Row.Cells(4).Text
            Dim AvbMtd, AvbYtd As Integer
            If BdgMtd = "&nbsp;" Or BdgMtd = "0.0000" Then
                AvbMtd = 0
            Else
                AvbMtd = ((Val(ActMtd) / Val(BdgMtd)) * 100)
            End If
            If BdgYtd = "&nbsp;" Or BdgYtd = "0.0000" Then
                AvbYtd = 0
            Else
                AvbYtd = ((Val(ActYtd) / Val(BdgYtd)) * 100)
            End If
            If AvbMtd = "0" Then
                e.Row.Cells(8).Text = "0 %"
            Else
                e.Row.Cells(8).Text = String.Format("{0:N2}", (AvbMtd)).ToString + " %"
            End If
            If AvbYtd = "0" Then
                e.Row.Cells(11).Text = "0 %"
            Else
                e.Row.Cells(11).Text = String.Format("{0:N2}", (AvbYtd)).ToString + " %"
            End If
        End If
    End Sub
    Protected Sub OnDataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        Dim cell As New TableHeaderCell()
        cell.Text = "<br/>Description"
        cell.RowSpan = 2
        cell.CssClass = "header-center"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 1
        cell.Text = "Today"
        cell.CssClass = "header-center"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 3
        cell.Text = "Month Todate"
        cell.CssClass = "header-center"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 3
        cell.Text = "Year Todate"
        cell.CssClass = "header-center"
        row.Controls.Add(cell)
        row.BackColor = ColorTranslator.FromHtml("#0a818e")
        gvSummary.HeaderRow.Parent.Controls.AddAt(0, row)
    End Sub

    Protected Sub dlFOLink_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlFOLink.SelectedIndexChanged
        Dim dateBirthday As Date = tbDate.Text
        forDate = dateBirthday.ToString("yyy-MM-")
        a = dateBirthday.ToString("yyy-MM-dd")
        year = dateBirthday.ToString("yyy")
        yearMonth = dateBirthday.ToString("yyy-MM-")
        tmpCategoryName = ""
        group = 0
        dblGrandTotalActTd = 0
        dblGrandTotalActMtd = 0
        dblGrandTotalActYtd = 0
        dblGrandTotalBgdMtd = 0
        dblGrandTotalBgdYtd = 0
        dblGrandTotalAvbMtd = 0
        dblGrandTotalAvbYtd = 0
        listSummarySales()
    End Sub

    Protected Sub lbPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint.Click
        Call passingPrm()
    End Sub

    Protected Sub lbPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPost.Click
        cekSales()
    End Sub

    Protected Sub lbOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOption.Click
        tbDatePost.Text = Format(Now, "MMMM yyy")
        ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
    End Sub

    Protected Sub cariPost(ByVal sender As Object, ByVal e As EventArgs)
        ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvDatePost.PageIndex = e.NewPageIndex
        Me.ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
    End Sub

    Protected Sub gvDatePost_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatePost.PageIndexChanging
        gvDatePost.PageIndex = e.NewPageIndex
        Me.ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
    End Sub
    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvDatePost.PageIndex = e.NewPageIndex
        Me.ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
    End Sub

    Protected Sub lbAbortDatePost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDatePost.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
    End Sub

    Protected Sub lbDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDetail.Click
        FOLinkDetail()
        tbDateDetail.Text = tbDate.Text
        GroupDetail()
        ListDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetail", "showModalDetail()", True)
    End Sub

    Protected Sub cariDetail(ByVal sender As Object, ByVal e As EventArgs)
        GroupDetail()
        ListDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetail", "hideModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetail", "showModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub

    Protected Sub gvSalesDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSalesDetail.PageIndexChanging
        gvSalesDetail.PageIndex = e.NewPageIndex
        GroupDetail()
        Me.ListDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetail", "hideModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetail", "showModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub

    Protected Sub lbAbortDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetail.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetail", "hideModalDetail()", True)
    End Sub

    Protected Sub dlFOLinkDetail_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlFOLinkDetail.SelectedIndexChanged
        GroupDetail()
        ListDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetail", "hideModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetail", "showModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub

    Protected Sub dlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlGroup.SelectedIndexChanged
        ListDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetail", "hideModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetail", "showModalDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub

    Protected Sub lbPrintDtl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrintDtl.Click
        Dim cIpx As New iPxClass
        Session("sReport") = "SalesSummaryDtl"
        'Session("filename") = "dckCashierSummary_Form.rpt"
        Session("sMapPath") = "~/iPxReportFile/dck_SalesTransactionDtl.rpt"
        'Session("sFOLink") = txtP2.Text
        Session("sFoLink") = dlFOLink.SelectedValue
        Dim dateBirthday As Date = tbDate.Text
        Session("sDate") = dateBirthday.ToString("yyy-MM-dd")
        Session("sCode") = dlGroup.SelectedValue
        Response.Redirect("rptviewer.aspx")
    End Sub
End Class
