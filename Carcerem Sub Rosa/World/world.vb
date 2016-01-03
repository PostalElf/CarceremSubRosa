Public Class world
    Friend Shared Function buildWorld() As world
        Dim world As New world
        For Each continent In enumArrays.continentArray
            world._cities.Add(continent, New List(Of city))
        Next

        Dim rawdata As List(Of String()) = csvFileget("data/worldcities.csv")
        For Each line In rawdata
            Dim city As city = city.buildCity(line)
            city.world = world
            world._cities(city.continent).Add(city)
        Next

        Return world
    End Function
    Friend Sub consoleReport(indent As Integer)
        For Each kvp In cities
            Dim continent As continent = kvp.Key
            For Each city In cities(continent)
                city.consoleReport(indent)
                Console.WriteLine()
            Next
        Next
    End Sub

    Private Property _cities As New Dictionary(Of continent, List(Of city))
    Friend ReadOnly Property cities As Dictionary(Of continent, List(Of city))
        Get
            Return _cities
        End Get
    End Property
    Friend ReadOnly Property continentIncome(continent As continent) As Integer
        Get
            Dim total As Integer = 0
            For Each city In _cities(continent)
                total += city.income
            Next
            Return total
        End Get
    End Property
End Class
