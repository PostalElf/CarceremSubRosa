Public Class player
    Implements problemReporter
    Public Sub New()
        For Each department In enumArrays.departmentArray
            _departmentBudgets.Add(department, 1000)
        Next
    End Sub
    Friend Sub consoleReport(indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Dim longnameLength As Integer = getLongestShellcompanyNameLength("Departments:  ".Length, " Corp:  ")

        Console.WriteLine(ind & fakeTab("Money: ", 11) & withSign(money, "$"))
        Console.WriteLine(ind & fakeTab("Income: ", 11) & withSign(income, "$"))
        Console.WriteLine(indd & fakeTab("Base Income: ", longnameLength) & withSign(_baseIncome, "$"))
        Console.WriteLine(indd & fakeTab("Departments: ", longnameLength) & withReverseSign(departmentBudgetTotal, "$"))
        For Each shellcompany In _shellcompanies
            Console.WriteLine(indd & fakeTab(shellcompany.name & " Corp: ", longnameLength) & withSign(shellcompany.income, "$"))
        Next
        Console.WriteLine(ind & fakeTab("Research: ", 11) & withSign(research))
        For Each shellcompany In _shellcompanies
            Console.WriteLine(indd & fakeTab(shellcompany.name & " Corp: ", longnameLength) & withSign(shellcompany.research))
        Next
    End Sub

    Friend Property name As String Implements problemReporter.name

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

    Private Property _blueprints As New List(Of product)
    Friend Function addBlueprint(product As product) As problem
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
    Friend ReadOnly Property activeResearchRequirements()
        Get
            Dim total As New List(Of requirement)
            For Each shellcompany In _shellcompanies
                total.AddRange(shellcompany.activeResearchRequirements)
            Next
            Return total
        End Get
    End Property

    Private Property _money As Integer
    Friend ReadOnly Property money As Integer
        Get
            Return _money
        End Get
    End Property
    Friend Property departmentBudgets As New Dictionary(Of department, Integer)
    Friend ReadOnly Property departmentBudgetTotal As Integer
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
            Select Case _departmentBudgets(department)
                Case 1000 To 1999 : Return 1
                Case 2000 To 2999 : Return 2
                Case 3000 To 3999 : Return 3
                Case 4000 To 4999 : Return 4
                Case Is >= 5000 : Return 5
                Case Else : Return 0
            End Select
        End Get
    End Property
    Friend Function createTradeRoute(blueprint As product, manufacturer As factory, importer As city) As problem
        Dim product As New product(blueprint)

        Dim total As problem = Nothing
        total = manufacturer.addExportProduct(product)
        If total Is Nothing = False Then Return total
        total = importer.addImportProduct(product)
        If total Is Nothing = False Then Return total

        Return Nothing
    End Function
    Private Property _baseIncome As Integer = 5000
    Private Property _baseResearch As Integer
    Friend ReadOnly Property income As Integer
        Get
            Dim total As Integer = _baseIncome
            For Each shellcompany In _shellcompanies
                total += shellcompany.income
            Next

            For Each kvp In _departmentBudgets
                total -= kvp.Value
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

    Private Property _modifiers As New List(Of modifier)
    Friend Function addModifier(modifier As modifier) As problem
        If _modifiers.Contains(modifier) Then Return New problem(Me, problemType.Duplicate)

        modifier.parent = _modifiers
        Select Case modifier.typeName
            Case "UnlockBlueprint"
                Dim blueprint As product = product.fileget(modifier.unlockName)
                _blueprints.Add(blueprint)
        End Select
        _modifiers.Add(modifier)
        Return Nothing
    End Function
    Friend Function addModifiers(modlist As List(Of modifier)) As problem
        Dim total As problem = Nothing
        For Each modifier In modlist
            total = addModifier(modifier)
            If total Is Nothing = False Then Return total
        Next

        Return Nothing
    End Function

    Friend Sub tick()
        'add income
        _money += income
        _researchProject.addProgress(research)


        'check for research completion
        If _researchProject.spilloverProgress >= 0 Then
            _researchSpilloverProgress = _researchProject.spilloverProgress
            _researchProjectsOpen.Remove(_researchProject)
            _researchProjectsCompleted.Add(_researchProject)

            addModifiers(_researchProject.modifiers)

            For Each projectName In _researchProject.childProjectNames
                Dim newProject As researchProject = researchProject.fileget(projectName)
                newProject.player = Me
                _researchProjectsOpen.Add(newProject)
            Next

            _researchProject = Nothing
        End If
    End Sub
End Class
