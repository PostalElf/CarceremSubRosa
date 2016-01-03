Public Class shellcompany
    Implements problemReporter
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aPlayer As player, aCity As city)
        If aName = "" Then name = holding.getRandomCodename Else name = aName
        player = aPlayer
        player.addShellcompany(Me)
        city = aCity
        city.addShellcompany(Me)
    End Sub
    Friend Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Dim indd As String = vbSpace(indent + 1) & prefix

        Console.WriteLine(ind & """" & name & """ Corp in " & city.name & ", " & city.parseContinent(continent))
        Console.WriteLine(indd & fakeTab("Visibility: ", 13) & visibility)
        Console.WriteLine(indd & fakeTab("Income: ", 13) & withSign(income, "$"))
        Console.WriteLine(indd & fakeTab("Research: ", 13) & withSign(research))
    End Sub
    Public Overrides Function ToString() As String
        Return """" & name & """ Pte Ltd"
    End Function

    Friend Property name As String Implements problemReporter.name
    Friend Property player As player
    Friend Property city As city
    Friend ReadOnly Property continent As continent
        Get
            Return city.continent
        End Get
    End Property

    Private Property _holdings As New List(Of holding)
    Friend ReadOnly Property holdings As List(Of holding)
        Get
            Return _holdings
        End Get
    End Property
    Friend Function addHolding(holding As holding) As problem
        If _holdings.Contains(holding) Then Return New problem(Me, problemType.Duplicate)

        holding.shellcompany = Me
        _holdings.Add(holding)
        Return Nothing
    End Function
    Friend Function activeResearchRequirements() As List(Of requirement)
        Dim total As New List(Of requirement)
        For Each holding In _holdings
            If TypeOf holding Is researchlab Then
                Dim researchlab As researchlab = CType(holding, researchlab)
                If researchlab.scientist Is Nothing = False Then total.Add(researchlab.scientist)
                If researchlab.artefact Is Nothing = False Then total.Add(researchlab.artefact)
                'ElseIf TypeOf holding Is safehouse Then
                '    Dim safehouse As safehouse = CType(holding, safehouse)
                '    If safehouse.scientist Is Nothing = False Then total.Add(safehouse.scientist)
                '    If safehouse.artefact Is Nothing = False Then total.Add(safehouse.artefact)
            End If
        Next
        Return total
    End Function
    Private ReadOnly Property visibility As Integer
        Get
            Dim total As Integer = 0
            For Each holding In holdings
                total += holding.visibility
            Next
            Return total
        End Get
    End Property

    Private Property _illegitMoney As Integer
    Friend ReadOnly Property income As Integer
        Get
            Dim total As Integer = 0
            For Each holding In holdings
                If TypeOf holding Is factory Then
                    Dim factory As factory = CType(holding, factory)
                    total += factory.income
                End If
                total -= holding.upkeep
            Next
            Return total
        End Get
    End Property
    Friend ReadOnly Property research As Integer
        Get
            Dim total As Integer = 0
            For Each holding In holdings
                If TypeOf holding Is researchlab Then
                    Dim researchlab As researchlab = CType(holding, researchlab)
                    total += researchlab.research
                End If
            Next
            Return total
        End Get
    End Property
    Friend Function tick() As Integer()
        Dim rawincome As Integer = 0
        Dim washedincome As Integer = 0
        Dim research As Integer = 0

        For Each holding In holdings
            rawincome -= holding.upkeep

            If TypeOf holding Is factory Then
                Dim factory As factory = CType(holding, factory)
                rawincome += factory.income
            ElseIf TypeOf holding Is researchlab Then
                Dim researchlab As researchlab = CType(holding, researchlab)
                research += researchlab.research
            ElseIf TypeOf holding Is laundry Then
                Dim laundry As laundry = CType(holding, laundry)
                washedincome += laundry.laundryRate
            End If
        Next

        Dim unwashedMoney As Integer = rawincome - washedincome
        If unwashedMoney > 0 Then _illegitMoney = constrain(_illegitMoney + unwashedMoney, 0, 1000)
        Dim money As Integer = Math.Min(rawincome, washedincome)

        Dim total(1) As Integer
        total(0) = money
        total(1) = research
        Return total
    End Function
End Class
