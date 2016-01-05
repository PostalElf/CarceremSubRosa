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

    Friend Sub tick()
        If squad.city.Equals(city) Then
            Dim currentMissionStage As missionStage = missionStages.Pop
            Dim agent As agent = squad.agents(rng.Next(squad.agents.Count))
            Dim skill As New skill(skill.getRandomAction, skill.getRandomApproach)

            Dim result As missionStageResult = currentMissionStage.tick(agent, skill)
            Select Case result
                Case Nothing
                    'timer still ticking
                    missionStages.Push(currentMissionStage)

                Case missionStageResult.Success
                    'mission success
                    Debug.Print(currentMissionStage.name & " success.")

                Case missionStageResult.Complicated
                    Dim penalties As List(Of String) = currentMissionStage.getPenalties(result, skill)
                    For Each penalty In penalties
                        addPenalty(agent, penalty)
                    Next
                    Debug.Print(currentMissionStage.name & " complicated success.")

                Case missionStageResult.Failure
                    Dim penalties As List(Of String) = currentMissionStage.getPenalties(result, skill)
                    For Each penalty In penalties
                        addPenalty(agent, penalty)
                    Next
                    currentMissionStage.timeProgress = 0
                    missionStages.Push(currentMissionStage)
                    Debug.Print(currentMissionStage.name & " failure.")
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
            Case Else : city.addPenalty(penaltyStr(0), value)
        End Select
    End Sub
    Private Sub missionSuccess()
        MsgBox("Mission Success")
    End Sub
End Class
