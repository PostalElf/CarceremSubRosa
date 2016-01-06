Public Class interrupt
    Private Shared _interruptQueue As New Queue(Of interrupt)
    Friend Shared ReadOnly Property Count As Integer
        Get
            Return _interruptQueue.Count
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
        _interruptQueue.Enqueue(interrupt)
    End Sub

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
                            Dim shellcompany As New shellcompany("", player, city)

                        Case "Real Estate Opportunity"
                            Dim player As player = CType(parent, player)
                            Dim city As city = CType(target, city)
                            Dim citysite As New citysite(player, city)

                        Case "Recruitment Opportunity"
                            Dim player As player = CType(parent, player)
                            Dim city As city = CType(target, city)
                            Dim agent As New agent()

                    End Select
                End If

        End Select
    End Sub
End Class


Public Enum interruptType
    YesNo = 1

End Enum