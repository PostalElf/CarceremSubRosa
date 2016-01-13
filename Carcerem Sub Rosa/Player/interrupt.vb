Public Class interrupt
    Private Shared _interruptStack As New Stack(Of interrupt)
    Friend Shared ReadOnly Property Count As Integer
        Get
            Return _interruptStack.Count
        End Get
    End Property
    Friend Shared Sub Add(aName As String, aType As interruptType, aParent As Object, aTarget As Object, aConsequences As List(Of String), aDescription As String)
        Dim interrupt As New interrupt
        With interrupt
            .name = aName
            .type = aType
            .parent = aParent
            .target = aTarget
            If aConsequences Is Nothing = False Then .consequences = aConsequences
            .description = aDescription
        End With
        _interruptStack.Push(interrupt)
    End Sub
    Friend Shared Sub Add(aName As String, aType As interruptType, aParent As Object, aTarget As Object, aConsequence As String, aDescription As String)
        Dim interrupt As New interrupt
        With interrupt
            .name = aName
            .type = aType
            .parent = aParent
            .target = aTarget
            If aConsequence <> "" Then .consequences.Add(aConsequence)
            .description = aDescription
        End With
        _interruptStack.Push(interrupt)
    End Sub
    Friend Shared Function Pop() As interrupt
        Return _interruptStack.Pop
    End Function
    Friend Shared Function Peek() As interrupt
        Return _interruptStack.Peek
    End Function

    Friend Property name As String
    Friend Property type As interruptType
    Friend Property parent As Object
    Friend Property target As Object
    Friend Property consequences As New List(Of String)
    Friend Property description As String

    Friend Sub handle()
        Select Case type
            Case interruptType.YesNo
                Console.WriteLine(name)
                If menu.confirmChoice(1, description) = True Then
                    Select Case name
                        Case "Business Opportunity"
                            Dim player As player = CType(parent, player)
                            Dim city As city = CType(target, city)
                            shellcompany.buildShellcompany(player, city)
                            handleConsequences()

                        Case "Real Estate Opportunity"
                            Dim player As player = CType(parent, player)
                            Dim city As city = CType(target, city)
                            citysite.buildCitysite(player, city)
                            handleConsequences()

                        Case "Recruitment Opportunity"
                            Dim player As player = CType(parent, player)
                            Dim city As city = CType(target, city)
                            Dim agent As agent = agent.buildRandomAgent(player)
                            player.addIdleAgent(agent)
                            handleConsequences()
                    End Select
                End If

            Case interruptType.ListChoice
                Console.WriteLine(name)
                Select Case name
                    Case "Choose Research"
                        Dim player As player = CType(parent, player)
                        Dim targetList As List(Of researchProject) = CType(target, List(Of researchProject))
                        Dim targetChoice As researchProject = menu.getListChoice(targetList, 1, description)
                        player.changeResearchProject(targetChoice)
                        handleConsequences()

                    Case "Select Active Agent"
                        Dim mission As mission = CType(parent, mission)
                        Dim squad As squad = CType(target, squad)
                        Dim choice As agent = Nothing
                        While choice Is Nothing
                            choice = menu.getListChoice(squad.agents, 1, description)
                        End While
                        mission.setAgent(choice)
                End Select
        End Select
    End Sub
    Private Sub handleConsequences()
        For Each consequence In consequences
            Dim rawstr As String() = consequence.Split(" ")
            Select Case rawstr(0)
                Case "player"
                    Dim player As player = CType(parent, player)
                    player.addConsequence(consequence)
            End Select
        Next
    End Sub
End Class


Public Enum interruptType
    YesNo = 1
    ListChoice
End Enum