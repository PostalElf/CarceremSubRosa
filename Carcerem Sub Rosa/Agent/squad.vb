Public Class squad
    Implements problemReporter
    Friend Shared Function buildRandomSquad(aPlayer As player, aCity As city, aAgents As List(Of agent)) As squad
        If aAgents.Count = 0 Then Return Nothing
        If aAgents.Count > 3 Then Return Nothing

        Dim squad As New squad
        With squad
            .name = getRandomName()
            aPlayer.addSquad(squad)
            ._city = aCity
            ._city.addSquad(squad)
            For Each agent In aAgents
                .addAgent(agent)
            Next
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
        Dim faketabLength As Integer = "Travel Speed:  ".Length

        Console.WriteLine(ind & "Squad " & name)
        If _city Is Nothing = False Then
            Console.WriteLine(indd & fakeTab("Location:", faketabLength) & _city.briefReport)
        Else
            Console.WriteLine(indd & fakeTab("Travelling:", faketabLength) & _travelOrigin.briefReport & " to " & _travelDestination.briefReport & " (" & _travelProgress & "/" & _travelCost & ")")
            Console.WriteLine(indd & fakeTab("Travel Speed:", faketabLength) & withSign(_travelSpeed))
        End If
        Console.WriteLine(indd & fakeTab("Upkeep:", faketabLength) & withReverseSign(upkeep, "$"))
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
        'handle mission
        If _city Is Nothing = False Then
            For n = _city.missions.Count - 1 To 0 Step -1
                Dim mission As mission = _city.missions(n)
                If mission.squad.Equals(Me) Then mission.tick()
            Next
        End If


        'movement
        If _city Is Nothing AndAlso _travelDestination Is Nothing = False Then
            _travelProgress += _travelSpeed
            If _travelProgress >= _travelCost Then teleportTo(_travelDestination)
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
        player.removeIdleAgent(agent)
        Return Nothing
    End Function
    Friend Function removeAgent(agent As agent) As problem
        If _agents.Contains(agent) = False Then Return New problem(Me, problemType.NotFound)

        agent.squad = Nothing
        _agents.Remove(agent)
        If _agents.Count = 0 Then dead()
        Return Nothing
    End Function
    Private Sub dead()
        player.removeSquad(Me)
        _city.removeSquad(Me)

        _travelOrigin = Nothing
        _travelDestination = Nothing
    End Sub
    Friend ReadOnly Property upkeep As Integer
        Get
            Dim total As Integer = 0
            For Each agent In _agents
                total += agent.upkeep
            Next
            Return total
        End Get
    End Property

    Friend Function addConsequence(consequence As String) As problem
        Dim rawstr As String() = consequence.Split(" ")
        If rawstr(0) <> "squad" Then Return New problem(Me, problemType.NotSuitable)

        Dim value As Integer = CInt(rawstr(2))
        For Each agent In agents
            Select Case rawstr(1)
                Case "health" : agent.addPenalty("health " & value)
                Case "sanity" : agent.addPenalty("sanity " & value)
                Case "morale" : agent.addPenalty("morale " & value)
            End Select
        Next
        Return Nothing
    End Function

    Private Property _city As city
    Friend ReadOnly Property city As city
        Get
            Return _city
        End Get
    End Property
    Private Property _travelSpeed As Integer = 100
    Private Property _travelProgress As Integer
    Private Property _travelCost As Integer
    Private Property _travelOrigin As city
    Private Property _travelDestination As city
    Friend Function moveTo(destination As city) As problem
        If _city Is Nothing Then Return New problem(Me, problemType.NotSuitable)

        _travelCost = _city.getDistanceTo(destination)
        _travelOrigin = _city
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
        _travelOrigin = Nothing
        _travelDestination = Nothing

        _city = destination
        _city.addSquad(Me)
        Return Nothing
    End Function

End Class
