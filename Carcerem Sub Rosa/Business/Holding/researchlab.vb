Public Class researchlab
    Inherits holding
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aCitysite As citysite, aShellcompany As shellcompany)
        If aName = "" Then name = holding.getRandomCodename Else name = aName
        citysite = aCitysite
        shellcompany = aShellcompany
        shellcompany.addHolding(Me)
        city.addHolding(Me, citysite)
    End Sub
    Friend Overrides Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Dim indd As String = vbSpace(indent + 1) & prefix
        Console.WriteLine(ind & """" & name & """ Laboratory")
        Console.WriteLine(indd & fakeTab("Shell Company: ", 16) & shellcompany.name)
        Console.WriteLine(indd & fakeTab("City: ", 16) & city.name & ", " & city.parseContinent(continent))
        Console.WriteLine(indd & fakeTab("Visibility: ", 16) & withSign(visibility))
        Console.WriteLine(indd & fakeTab("Upkeep: ", 16) & withReverseSign(upkeep, "$"))
        Console.WriteLine(indd & fakeTab("Research: ", 16) & withSign(research))
    End Sub
    Public Overrides Function ToString() As String
        Return name & " Laboratory"
    End Function

    Private _artefact As artefact = Nothing
    Friend ReadOnly Property artefact As artefact
        Get
            Return _artefact
        End Get
    End Property
    Private Property _scientist As scientist = Nothing
    Friend ReadOnly Property scientist As scientist
        Get
            Return _scientist
        End Get
    End Property
    Friend Function addArtefact(artefact As artefact) As problem
        If artefact Is Nothing = False Then Return New problem(Me, problemType.ExceedCapacity)

        _artefact = artefact
        artefact.holding = Me
        Return Nothing
    End Function
    Friend Function getArtefact() As artefact
        getArtefact = _artefact
        _artefact = Nothing
    End Function
    Friend Function addScientist(scientist As scientist) As problem
        If scientist Is Nothing = False Then Return New problem(Me, problemType.ExceedCapacity)

        _scientist = scientist
        scientist.holding = Me
        Return Nothing
    End Function
    Friend Function getScientist() As scientist
        getScientist = _scientist
        _scientist = Nothing
    End Function

    Friend ReadOnly Property research As Integer
        Get
            Dim total As Integer = 0
            Select Case city.standardOfLiving
                Case standardOfLiving.Abyssal : total += 0
                Case standardOfLiving.Low : total += 50
                Case standardOfLiving.Mid : total += 70
                Case standardOfLiving.High : total += 100
                Case Else : Return -1
            End Select

            If artefact Is Nothing = False Then total += artefact.researchBonus
            If scientist Is Nothing = False Then total += scientist.researchBonus

            Return total
        End Get
    End Property
    Friend Overrides ReadOnly Property visibility As Integer
        Get
            Dim total As Integer = 3
            If scientist Is Nothing = False Then total += scientist.visibility
            If artefact Is Nothing = False Then total += artefact.visibility
            Return total
        End Get
    End Property
    Friend Overrides ReadOnly Property upkeep As Integer
        Get
            Return 300
        End Get
    End Property
End Class
