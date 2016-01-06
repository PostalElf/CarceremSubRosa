Public Class interrupt
    Private Shared _interruptStack As New Stack(Of interrupt)
    Friend Shared ReadOnly Property Count As Integer
        Get
            Return _interruptStack.Count
        End Get
    End Property
    Friend Shared Sub Add(aName As String, aType As interruptType, aParent As Object, aTarget As Object, aCost As cost, aDescription As String)
        Dim interrupt As New interrupt
        With interrupt
            .name = aName
            .type = aType
            .parent = aParent
            .target = aTarget
            .cost = aCost
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
    Friend Property cost As cost
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

                        Case "Real Estate Opportunity"
                            Dim player As player = CType(parent, player)
                            Dim city As city = CType(target, city)
                            citysite.buildCitysite(player, city)

                        Case "Recruitment Opportunity"
                            Dim player As player = CType(parent, player)
                            Dim city As city = CType(target, city)
                            Dim agent As agent = agent.buildRandomAgent(player)
                            player.addIdleAgent(agent)
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
                End Select
        End Select
    End Sub
End Class


Public Enum interruptType
    YesNo = 1
    ListChoice
End Enum