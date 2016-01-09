Public Class player
    Implements problemReporter
    Public Sub New(aWorld As world)
        _world = aWorld
        For Each department In enumArrays.departmentArray
            _departmentBudgets.Add(department, 1000)
            _departmentLevelMax.Add(department, 1)
        Next
    End Sub
    Friend Sub consoleReport(indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Dim longnameLength As Integer = "Shell Companies:  ".Length

        Console.WriteLine(ind & fakeTab("Money: ", 11) & withSign(money, "$"))
        Console.WriteLine(ind & fakeTab("Income: ", 11) & withSign(incomeNet, "$"))
        Console.WriteLine(indd & fakeTab("Funding: ", longnameLength) & withSign(_baseIncome, "$"))
        Console.WriteLine(indd & fakeTab("Departments: ", longnameLength) & withReverseSign(_departmentBudgetTotal, "$"))
        Dim totalShellcompanyIncome As Integer = 0
        For Each shellcompany In _shellcompanies
            totalShellcompanyIncome += shellcompany.incomeNet
        Next
        Console.WriteLine(indd & fakeTab("Shell Companies: ", longnameLength) & withSign(totalShellcompanyIncome, "$"))
        Dim totalAgentUpkeep As Integer = 0
        For Each squad In _squads
            totalAgentUpkeep += squad.upkeep
        Next
        For Each agent In _idleAgents
            totalAgentUpkeep += agent.upkeep
        Next
        Console.WriteLine(indd & fakeTab("Agents: ", longnameLength) & withReverseSign(totalAgentUpkeep, "$"))

        Console.Write(ind & fakeTab("Project: ", 11))
        If _researchProject Is Nothing = False Then Console.Write(_researchProject.briefReport) Else Console.Write("Nothing")
        Console.WriteLine()
        Console.WriteLine(ind & fakeTab("Research: ", 11) & withSign(research))
        For Each shellcompany In _shellcompanies
            Console.WriteLine(indd & fakeTab(shellcompany.name & " Corp: ", longnameLength) & withSign(shellcompany.research))
        Next

        If _idleAgents.Count > 0 Then
            Console.WriteLine(ind & fakeTab("Idle Agents: ", 11))
            For Each agent In _idleAgents
                Console.WriteLine(indd & agent.ToString)
            Next
        End If
    End Sub
    Friend Sub fullConsoleReport(indent As Integer)
        Dim ind As String = vbSpace(indent)

        consoleReport(indent)
        Console.WriteLine(vbCrLf)

        If _openCitysites.Count > 0 Then
            Console.WriteLine(ind & "Unused Real Estate")
            For Each citysite In _openCitysites
                citysite.consoleReport(indent + 1)
            Next
            Console.WriteLine(vbCrLf)
        End If

        For Each shellcompany In _shellcompanies
            shellcompany.fullConsoleReport(indent)
            Console.WriteLine()
        Next

        Console.WriteLine()

        For Each squad In _squads
            squad.consoleReport(indent)
        Next
    End Sub

    Friend Property name As String Implements problemReporter.name
    Private Property _world As world

    Private Property _shellcompanies As New List(Of shellcompany)
    Friend Function addShellcompany(shellcompany As shellcompany) As problem
        If _shellcompanies.Contains(shellcompany) Then Return New problem(Me, problemType.Duplicate)

        shellcompany.player = Me
        _shellcompanies.Add(shellcompany)
        Return Nothing
    End Function
    Private Function getLongestShellcompanyNameLength(Optional minValue As Integer = -1, Optional nameSuffix As String = "") As Integer
        Dim total As Integer = minValue
        For Each shellcompany In _shellcompanies
            Dim name As String = shellcompany.name & nameSuffix
            If name.Length > total Then total = name.Length
        Next
        Return total
    End Function
    Private Property _openCitysites As New List(Of citysite)
    Friend Function addOpenCitysite(citysite As citysite) As problem
        If _openCitysites.Contains(citysite) Then Return New problem(Me, problemType.Duplicate)

        citysite.player = Me
        _openCitysites.Add(citysite)
        Return Nothing
    End Function
    Friend Function removeOpenCitysite(citysite As citysite)
        If _openCitysites.Contains(citysite) = False Then Return New problem(Me, problemType.NotFound)

        _openCitysites.Remove(citysite)
        Return Nothing
    End Function

    Private Property _squads As New List(Of squad)
    Friend Function addSquad(squad As squad) As problem
        If _squads.Contains(squad) Then Return New problem(Me, problemType.Duplicate)

        squad.player = Me
        _squads.Add(squad)
        Return Nothing
    End Function
    Friend Function removeSquad(squad As squad) As problem
        If _squads.Contains(squad) = False Then Return New problem(Me, problemType.NotFound)

        squad.player = Nothing
        _squads.Remove(squad)
        Return Nothing
    End Function
    Private Property _idleAgents As New List(Of agent)
    Friend Function addIdleAgent(agent As agent) As problem
        If _idleAgents.Contains(agent) Then Return New problem(Me, problemType.Duplicate)

        _idleAgents.Add(agent)
        Return Nothing
    End Function
    Friend Function removeIdleAgent(agent As agent) As problem
        If _idleAgents.Contains(agent) = False Then Return New problem(Me, problemType.NotFound)

        _idleAgents.Remove(agent)
        Return Nothing
    End Function

    Private Property _blueprints As New List(Of product)
    Friend Function devAddBlueprint(product As product) As problem
        If _blueprints.Contains(product) Then Return New problem(Me, problemType.Duplicate)

        _blueprints.Add(product)
        Return Nothing
    End Function
    Friend Function checkBlueprint(productName As String) As Boolean
        For Each blueprint In _blueprints
            If blueprint.name = productName Then Return True
        Next
        Return False
    End Function

    Private Property _researchProject As researchProject
    Private Property _researchProjectsOpen As New List(Of researchProject)
    Private ReadOnly Property _researchProjectsReady As List(Of researchProject)
        Get
            Dim total As New List(Of researchProject)
            For Each project In _researchProjectsOpen
                If project.requirementsMet = True Then total.Add(project)
            Next
            Return total
        End Get
    End Property
    Private Property _researchProjectsCompleted As New List(Of researchProject)
    Private Property _researchSpilloverProgress As Integer
    Friend Function getActiveResearchRequirements() As List(Of requirement)
        Dim total As New List(Of requirement)
        For Each shellcompany In _shellcompanies
            total.AddRange(shellcompany.activeResearchRequirements)
        Next
        Return total
    End Function
    Friend Function changeResearchProject(newResearch As researchProject) As problem
        If _researchProjectsReady.Contains(newResearch) = False Then Return New problem(Me, problemType.NotFound)

        _researchProject = newResearch
        Return Nothing
    End Function
    Friend Sub devAddResearchProjectOpen(research As researchProject)
        _researchProjectsOpen.Add(research)
    End Sub

    Private Property _money As Integer
    Friend ReadOnly Property money As Integer
        Get
            Return _money
        End Get
    End Property
    Friend Property departmentBudgets As New Dictionary(Of department, Integer)
    Private ReadOnly Property _departmentBudgetTotal As Integer
        Get
            Dim total As Integer = 0
            For Each kvp In departmentBudgets
                total += kvp.Value
            Next
            Return total
        End Get
    End Property
    Friend ReadOnly Property departmentLevel(department As department) As Integer
        Get
            Dim total As Integer = 0
            Select Case _departmentBudgets(department)
                Case 1000 To 1999 : total = 1
                Case 2000 To 2999 : total = 2
                Case 3000 To 3999 : total = 3
                Case 4000 To 4999 : total = 4
                Case Is >= 5000 : total = 5
                Case Else : Return 0
            End Select
            Return Math.Min(total, _departmentLevelMax(department))
        End Get
    End Property
    Private Property _departmentLevelMax() As New Dictionary(Of department, Integer)
    Private Property _hrRecruitment As Integer
    Private Property _hrCityChoice As city
    Private Property _hrOpportunityChoice As String
    Friend Function createTradeRoute(blueprint As product, manufacturer As factory, importer As city) As problem
        Dim product As New product(blueprint)

        Dim total As problem = Nothing
        total = manufacturer.addExportProduct(product)
        If total Is Nothing = False Then Return total
        total = importer.addImportProduct(product)
        If total Is Nothing = False Then Return total

        Return Nothing
    End Function
    Private Property _baseIncome As Integer = 6000
    Private Property _baseResearch As Integer
    Friend ReadOnly Property incomeNet As Integer
        Get
            Dim total As Integer = _baseIncome
            total -= _departmentBudgetTotal

            For Each agent In _idleAgents
                total -= agent.upkeep
            Next

            For Each squad In _squads
                total -= squad.upkeep
            Next

            For Each shellcompany In _shellcompanies
                total += shellcompany.incomeNet
            Next
            Return total
        End Get
    End Property
    Friend ReadOnly Property research As Integer
        Get
            Dim total As Integer = _baseResearch
            For Each shellcompany In _shellcompanies
                total += shellcompany.research
            Next
            Return total
        End Get
    End Property

    Friend Sub tick()
        'check for outstanding interrupts
        If interrupt.Count > 0 Then Exit Sub

        dayTick()
        If _world.dateTimeNewWeek = True Then weekTick()
        If _world.dateTimeNewMonth = True Then monthTick()
    End Sub
    Private Sub dayTick()
        'tick squads
        For n = _squads.Count - 1 To 0 Step -1
            Dim squad As squad = _squads(n)
            squad.dayTick()
        Next
    End Sub
    Private Sub weekTick()
        'tick shellcompanies
        For n = _shellcompanies.Count - 1 To 0 Step -1
            Dim shellcompany As shellcompany = _shellcompanies(n)
            shellcompany.weekTick()
        Next


        'add money and research income
        _money += incomeNet
        researchTick()


        'tick HR department
        hrTick()
    End Sub
    Private Sub monthTick()

    End Sub
    Private Sub hrTick()
        _hrRecruitment += departmentLevel(department.HR)
        If percentRoll(_hrRecruitment) = True Then
            _hrRecruitment = 0

            Dim city As city = Nothing
            Dim opportunity As Integer = 0
            If departmentLevel(department.HR) >= 5 Then
                city = _hrCityChoice
                Select Case _hrOpportunityChoice
                    Case "citysite" : opportunity = 1
                    Case "shellcompany" : opportunity = 2
                    Case "agent" : opportunity = 3
                    Case Else : opportunity = 0
                End Select
            ElseIf departmentLevel(department.HR) >= 3 Then
                If coinFlip() = True Then
                    city = _hrCityChoice
                Else
                    Select Case _hrOpportunityChoice
                        Case "citysite" : opportunity = 1
                        Case "shellcompany" : opportunity = 2
                        Case "agent" : opportunity = 3
                        Case Else : opportunity = 0
                    End Select
                End If
            End If

            If city Is Nothing Then city = _world.getRandomCity()
            If opportunity = 0 Then opportunity = rng.Next(1, 4)

            Select Case opportunity
                Case 1
                    'citysite
                    Dim cost As New cost(1000)
                    interrupt.Add("Real Estate Opportunity", interruptType.YesNo, Me, city, cost, "Purchase real estate in " & city.ToString & " for $" & cost.money & "?")
                Case 2
                    'shell company
                    Dim cost As New cost(1000)
                    interrupt.Add("Business Opportunity", interruptType.YesNo, Me, city, cost, "Purchase a business in " & city.ToString & " for $" & cost.money & "?")
                Case 3
                    'recruit agent
                    Dim cost As New cost(1000)
                    interrupt.Add("Recruitment Opportunity", interruptType.YesNo, Me, city, cost, "Recruit an agent in " & city.ToString & " for $" & cost.money & "?")
            End Select
        End If
    End Sub
    Private Sub researchTick()
        If _researchProject Is Nothing = False Then
            _researchProject.addProgress(research)

            'check for research completion
            If _researchProject.spilloverProgress >= 0 Then
                _researchSpilloverProgress = _researchProject.spilloverProgress
                _researchProjectsOpen.Remove(_researchProject)
                _researchProjectsCompleted.Add(_researchProject)

                If _researchProject.consequences Is Nothing = False Then
                    For Each consequence In _researchProject.consequences
                        addConsequence(consequence)
                    Next
                End If

                If _researchProject.childProjectNames Is Nothing = False Then
                    For Each projectName In _researchProject.childProjectNames
                        Dim newProject As researchProject = researchProject.fileget(projectName)
                        If newProject Is Nothing Then Exit For
                        newProject.player = Me
                        _researchProjectsOpen.Add(newProject)
                    Next
                End If

                _researchProject = Nothing
                If _researchProjectsReady.Count > 0 Then interrupt.Add("Choose Research", interruptType.ListChoice, Me, _researchProjectsReady, New cost(0), "")
            End If
        End If
    End Sub
    Friend Function addConsequence(consequence As String) As problem
        Dim rawstr As String() = consequence.Split(" ")
        If rawstr(0) <> "player" Then Return New problem(Me, problemType.NotSuitable)

        Select Case rawstr(1)
            Case "departmentLevelMax"
                Dim department As department = enumArrays.getEnumFromString(rawstr(2), enumArrays.departmentArray)
                Dim value As Integer = CInt(rawstr(3))
                _departmentLevelMax(department) += value

            Case "govtFunding"
                Dim value As Integer = CInt(rawstr(2))
                _baseIncome += value

            Case "unlockBlueprint"
                Dim blueprintName As String = ""
                For n = 2 To rawstr.Length - 1
                    blueprintName &= rawstr(n) & " "
                Next
                blueprintName = blueprintName.Trim()
                Dim blueprint As product = product.fileget(blueprintName)
                _blueprints.Add(blueprint)
        End Select
        Return Nothing
    End Function
End Class
