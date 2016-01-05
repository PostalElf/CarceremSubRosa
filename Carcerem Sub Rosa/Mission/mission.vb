Public Class mission
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

    Friend Property name As String
    Friend Property city As city
    Friend Property squad As squad
    Friend Property missionStages As New Stack(Of missionStage)
    Friend Property consequences As New List(Of String)

    Friend Sub tick()
        If squad.city.Equals(city) Then
            Dim currentMissionStage As missionStage = missionStages.Pop
            Dim agent As agent = squad.agents(rng.Next(squad.agents.Count))
            Dim skill As New skill(skill.getRandomAction, skill.getRandomApproach)

            Dim result As missionStageResult = currentMissionStage.tick(agent, skill)
            Select Case result
                Case Nothing
                    'timer still ticking
                    Debug.Print(currentMissionStage.briefReport)
                    missionStages.Push(currentMissionStage)

                Case missionStageResult.Success
                    'mission success
                    Debug.Print(currentMissionStage.name & " success.")

                Case missionStageResult.Complicated
                    Debug.Print(currentMissionStage.name & " complicated success.")
                    For Each penalty In currentMissionStage.getPenalties(result, skill)
                        addPenalty(agent, penalty)
                    Next

                Case missionStageResult.Failure
                    currentMissionStage.timeProgress = 0
                    missionStages.Push(currentMissionStage)
                    Debug.Print(currentMissionStage.name & " failure.")
                    For Each penalty In currentMissionStage.getPenalties(result, skill)
                        addPenalty(agent, penalty)
                    Next
            End Select

            If missionStages.Count = 0 Then
                missionSuccess()
            End If
        End If
    End Sub
    Private Sub addPenalty(agent As agent, penalty As String)
        Dim penaltyStr As String() = penalty.Split(" ")
        Dim value As Integer = CInt(penaltyStr(1))

        Select Case penaltyStr(0).ToLower
            Case "health" : agent.addPenalty(penaltyStr(0), value)
            Case "sanity" : agent.addPenalty(penaltyStr(0), value)
            Case "morale" : agent.addPenalty(penaltyStr(0), value)
            Case "stonewall"
                missionStages.Peek.timeProgress = 0
                missionStages.Peek.timeProgress -= value
                Debug.Print(missionStages.Peek.name & " time progress -" & value)
            Case Else : city.addPenalty(penaltyStr(0), value)
        End Select
    End Sub
    Private Sub addConsequence(consequence As String)
        Dim rawstr As String() = consequence.Split(" ")
        Select Case rawstr(0).ToLower
            Case "city" : city.addConsequence(consequence)
            Case "squad" : squad.addConsequence(consequence)
            Case "player" : squad.player.addConsequence(consequence)
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
End Class
