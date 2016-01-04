Public Class squad
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

        _agents.Add(agent)
        Return Nothing
    End Function
    Friend Function removeAgent(agent As agent) As problem
        If _agents.Contains(agent) = False Then Return New problem(Me, problemType.NotFound)

        _agents.Remove(agent)
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
