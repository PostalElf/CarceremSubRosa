Public Class squad
    Implements problemReporter
    Friend Shared Function buildRandomSquad(aPlayer As player, aCity As city) As squad
        Dim squad As New squad
        With squad
            .name = getRandomName()
            .player = aPlayer
            ._city = aCity
        End With
        Return squad
    End Function
    Public Overrides Function ToString() As String
        Return "Squad " & name
    End Function
    Friend Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Dim indd As String = vbSpace(indent + 1) & prefix
        Dim inddd As String = vbSpace(indent + 2) & prefix

        Console.WriteLine(ind & "Squad " & name)
        If _city Is Nothing = False Then
            Console.WriteLine(indd & "Location: " & _city.briefReport)
        Else
            Console.WriteLine(indd & "Travelling To: " & _travelDestination.briefReport & " (" & _travelProgress & "/" & _travelCost & ")")
        End If
        Console.WriteLine(indd & "Agents:")
        For Each agent In _agents
            Console.WriteLine(inddd & agent.ToString)
        Next
    End Sub

    Friend Property name As String Implements problemReporter.name
    Private Shared Property _squadNames1 As New List(Of String)
    Private Shared Property _squadNames2 As New List(Of String)
    Private Shared Property _squadNames3 As New List(Of String)
    Private Shared Function getRandomName() As String
        Dim total As String = ""
        Dim roll As Integer = 0
        If _squadNames1.Count = 0 Then _squadNames1 = fileget("data/squadNames.txt")
        If _squadNames2.Count = 0 Then _squadNames2 = fileget("data/squadNames.txt")
        If _squadNames3.Count = 0 Then _squadNames3 = fileget("data/squadNames.txt")

        roll = rng.Next(_squadNames1.Count)
        total = _squadNames1(roll)
        _squadNames1.RemoveAt(roll)
        roll = rng.Next(_squadNames2.Count)
        total &= "-" & _squadNames2(roll)
        _squadNames2.RemoveAt(roll)
        roll = rng.Next(_squadNames3.Count)
        total &= "-" & _squadNames3(roll)
        _squadNames3.RemoveAt(roll)

        Return total
    End Function
    Friend Property player As player

    Friend Sub tick()
        If _city Is Nothing AndAlso _travelDestination Is Nothing = False Then
            _travelProgress += _travelSpeed
            If _travelSpeed >= _travelCost Then teleportTo(_travelDestination)
        End If
    End Sub

    Private Property _agents As New List(Of agent)
    Friend ReadOnly Property agents As List(Of agent)
        Get
            Return _agents
        End Get
    End Property
    Friend Function addAgent(agent As agent) As problem
        If _agents.Contains(agent) Then Return New problem(Me, problemType.Duplicate)
        If _agents.Count + 1 > 3 Then Return New problem(Me, problemType.ExceedCapacity)
        If agent.player.Equals(player) = False Then Return New problem(Me, problemType.NotSuitable)

        _agents.Add(agent)
        agent.squad = Me
        Return Nothing
    End Function
    Friend Function removeAgent(agent As agent) As problem
        If _agents.Contains(agent) = False Then Return New problem(Me, problemType.NotFound)

        _agents.Remove(agent)
        agent.squad = Nothing
        Return Nothing
    End Function

    Private Property _city As city
    Private Property _travelSpeed As Integer
    Private Property _travelProgress As Integer
    Private Property _travelCost As Integer
    Private Property _travelDestination As city
    Friend Function moveTo(destination As city) As problem
        If _city Is Nothing Then Return New problem(Me, problemType.NotSuitable)

        _travelCost = _city.getDistanceTo(destination)
        _travelDestination = destination
        _travelProgress = 0
        _city.removeSquad(Me)
        _city = Nothing
        Return Nothing
    End Function
    Friend Function teleportTo(destination As city) As problem
        If _city Is Nothing = False Then _city.removeSquad(Me)

        _travelProgress = 0
        _travelCost = 0
        _travelDestination = Nothing

        _city = destination
        _city.addSquad(Me)
        Return Nothing
    End Function

End Class
