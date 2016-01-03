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
        Console.WriteLine(indd & fakeTab("Raw Income: ", 13) & withSign(incomeRaw, "$"))
        Console.WriteLine(indd & fakeTab("Net Income: ", 13) & withSign(incomeNet, "$"))
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
    Friend Function removeHolding(holding As holding) As problem
        If _holdings.Contains(holding) = False Then Return New problem(Me, problemType.NotFound)

        _holdings.Remove(holding)
        Return Nothing
    End Function
    Friend Function activeResearchRequirements() As List(Of requirement)
        Dim total As New List(Of requirement)
        For Each holding In _holdings
            If TypeOf holding Is researchlab Then
                Dim researchlab As researchlab = CType(holding, researchlab)
                If researchlab.scientist Is Nothing = False Then total.Add(researchlab.scientist)
                If researchlab.artefact Is Nothing = False Then total.Add(researchlab.artefact)
            ElseIf TypeOf holding Is safehouse Then
                Dim safehouse As safehouse = CType(holding, safehouse)
                If safehouse.prisoner Is Nothing = False Then total.Add(safehouse.prisoner)
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

    Friend ReadOnly Property incomeRaw As Integer
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
    Friend ReadOnly Property incomeLaundyRate As Integer
        Get
            Dim total As Integer = 0
            For Each holding In holdings
                If TypeOf holding Is laundry Then
                    Dim laundry As laundry = CType(holding, laundry)
                    total += laundry.laundryRate
                End If
            Next
            Return total
        End Get
    End Property
    Friend ReadOnly Property incomeNet As Integer
        Get
            Return Math.Min(incomeRaw, incomeLaundyRate)
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
    Friend Sub tick()

    End Sub
End Class
