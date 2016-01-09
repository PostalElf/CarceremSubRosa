Public Class mission
    Implements problemReporter
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aCity As city, aMissionStages As Stack(Of missionStage))
        name = aName
        aCity.addMission(Me)
        missionStages = aMissionStages

        If aMissionStages Is Nothing = False Then
            For Each stage In missionStages
                stage.mission = Me
            Next
        End If
    End Sub
    Friend Function briefReport() As String
        Return """" & name & """ in " & city.briefReport
    End Function
    Public Overrides Function ToString() As String
        Return "Mission " & name
    End Function

    Friend Property name As String Implements problemReporter.name
    Friend Property city As city
    Friend Property squad As squad
    Friend Property missionStages As New Stack(Of missionStage)
    Friend Property consequences As New List(Of String)

    Private Property _actingAgent As agent
    Friend Function setAgent(agent As agent) As problem
        If squad.agents.Contains(agent) = False Then Return New problem(Me, problemType.NotFound)

        _actingAgent = agent
        Return Nothing
    End Function

    Friend Sub dayTick()
        If squad.city.Equals(city) AndAlso _actingAgent Is Nothing = False Then
            Dim currentMissionStage As missionStage = missionStages.Pop
            Dim decisionMatrix As List(Of Dictionary(Of choiceComponent, Integer)) = _actingAgent.decisionMatrix
            Dim approach As choiceComponent = getRandomChoice(decisionMatrix(0))
            Dim action As choiceComponent = getRandomChoice(decisionMatrix(1))

            Dim result As missionStageResult = currentMissionStage.tick(_actingAgent, approach, action)
            Select Case result
                Case Nothing
                    'timer still ticking
                    Debug.Print(currentMissionStage.briefReport)
                    missionStages.Push(currentMissionStage)

                Case missionStageResult.Success
                    'mission success
                    Debug.Print(currentMissionStage.name & " success.")
                    currentMissionStage.Dispose()

                Case missionStageResult.Complicated
                    Debug.Print(currentMissionStage.name & " complicated success.")
                    For Each penalty In currentMissionStage.getPenalties(result, approach, action)
                        addConsequence(penalty)
                    Next
                    currentMissionStage.Dispose()

                Case missionStageResult.Failure
                    currentMissionStage.timeProgress = 0
                    missionStages.Push(currentMissionStage)
                    Debug.Print(currentMissionStage.name & " failure.")
                    For Each penalty In currentMissionStage.getPenalties(result, approach, action)
                        addConsequence(penalty)
                    Next
            End Select

            If missionStages.Count = 0 Then
                missionSuccess()
            End If
        End If
    End Sub
    Private Sub addConsequence(consequence As String)
        Dim rawstr As String() = consequence.Split(" ")
        Select Case rawstr(0).ToLower
            Case "city" : city.addConsequence(consequence)
            Case "player" : squad.player.addConsequence(consequence)
            Case "squad" : squad.addConsequence(consequence)
            Case "agent" : _actingAgent.addConsequence(consequence)
            Case "mission"
                Select Case rawstr(1)
                    Case "progress"
                        Dim value As Integer = CInt(rawstr(2))
                        missionStages.Peek.timeProgress = 0
                        missionStages.Peek.timeProgress += value
                        Debug.Print(missionStages.Peek.name & " time progress " & withSign(value))
                End Select
        End Select
    End Sub
    Private Sub missionSuccess()
        Debug.Print("Mission Success: " & name)

        For Each consequence In consequences
            addConsequence(consequence)
        Next

        consequences = Nothing
        squad = Nothing
        missionStages = Nothing
        city.removeMission(Me)
    End Sub
    Private Function getRandomChoice(decisionMatrix As Dictionary(Of choiceComponent, Integer))
        Dim total As New List(Of choiceComponent)
        For Each kvp In decisionMatrix
            For n = 1 To kvp.Value
                total.Add(kvp.Key)
            Next
        Next
        Return total(rng.Next(total.Count))
    End Function
End Class
