Public Class safehouse
    Inherits holding
    Friend Shared Function buildSafehouse(aCitysite As citysite, aShellcompany As shellcompany) As safehouse
        If aCitysite.city.Equals(aShellcompany.city) = False Then Return Nothing

        Dim safehouse As New safehouse
        With safehouse
            .name = holding.getRandomCodename
            .citysite = aCitysite
            .city.addHolding(safehouse, .citysite)
            aShellcompany.addHolding(safehouse)
        End With
        Return safehouse
    End Function
    Friend Overrides Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Dim indd As String = vbSpace(indent + 1) & prefix
        Console.WriteLine(ind & """" & name & """ Safehouse")
        Console.WriteLine(indd & fakeTab("Shell Company: ", 16) & shellcompany.name)
        Console.WriteLine(indd & fakeTab("City: ", 16) & city.name & ", " & city.parseContinent(continent))
        Console.WriteLine(indd & fakeTab("Visibility: ", 16) & withSign(visibility))
        Console.WriteLine(indd & fakeTab("Upkeep: ", 16) & withReverseSign(upkeep, "$"))
    End Sub
    Public Overrides Function ToString() As String
        Return name & " Safehouse"
    End Function

    Private _artefact As artefact = Nothing
    Friend ReadOnly Property artefact As artefact
        Get
            Return _artefact
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
    Private Property _scientist As scientist = Nothing
    Friend ReadOnly Property scientist As scientist
        Get
            Return _scientist
        End Get
    End Property
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
    Private Property _prisoner As prisoner = Nothing
    Friend ReadOnly Property prisoner As prisoner
        Get
            Return _prisoner
        End Get
    End Property
    Friend Function addPrisoner(prisoner As prisoner) As problem
        If _prisoner Is Nothing = False Then Return New problem(Me, problemType.ExceedCapacity)

        _prisoner = prisoner
        prisoner.holding = Me
        Return Nothing
    End Function
    Friend Function getPrisoner() As prisoner
        getPrisoner = _prisoner
        _prisoner = Nothing
    End Function

    Friend Overrides ReadOnly Property upkeep As Integer
        Get
            Return 100
        End Get
    End Property
    Friend Overrides ReadOnly Property visibility As Integer
        Get
            Return 1
        End Get
    End Property
End Class
