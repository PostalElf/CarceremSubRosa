Public MustInherit Class holding
    Implements problemReporter
    Friend MustOverride Sub consoleReport(indent As Integer, Optional prefix As String = "")

    Friend Property shellcompany As shellcompany
    Friend Function changeShellcompany(newShellcompany As shellcompany) As problem
        If newShellcompany.city.Equals(shellcompany.city) = False Then Return New problem(Me, problemType.NotSuitable)

        Dim total As problem = Nothing
        total = shellcompany.removeHolding(Me)
        If total Is Nothing = False Then Return total
        shellcompany = newShellcompany
        total = shellcompany.addHolding(Me)
        If total Is Nothing = False Then Return total
        Return Nothing
    End Function
    Friend Property citysite As citysite
    Friend ReadOnly Property city As city
        Get
            Return shellcompany.city
        End Get
    End Property
    Friend ReadOnly Property continent As continent
        Get
            Return shellcompany.continent
        End Get
    End Property
    Friend MustOverride ReadOnly Property visibility As Integer
    Friend MustOverride ReadOnly Property upkeep As Integer

    Friend Property name As String Implements problemReporter.name
    Private Shared _codenames As New List(Of String)
    Friend Shared Function getRandomCodename() As String
        If _codenames.Count = 0 Then _codenames = fileget("data/codenames.txt")
        Dim roll As Integer = rng.Next(_codenames.Count)
        getRandomCodename = _codenames(roll)
        _codenames.RemoveAt(roll)
    End Function
End Class
