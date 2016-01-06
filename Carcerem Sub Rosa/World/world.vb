Public Class world
    Friend Shared Function buildWorld() As world
        Dim world As New world
        For Each continent In enumArrays.continentArray
            world._cities.Add(continent, New List(Of city))
            world._tlaGoodwill.Add(continent, 10)
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
    Friend Function getRandomCity(Optional continent As continent = Nothing) As city
        If continent = Nothing Then continent = enumArrays.continentArray(rng.Next(enumArrays.continentArray.Length))
        Dim roll As Integer = rng.Next(_cities.Count)
        Return _cities(continent)(roll)
    End Function

    Friend ReadOnly Property continentIncome(continent As continent) As Integer
        Get
            Dim total As Integer = 0
            For Each city In _cities(continent)
                total += city.income
            Next
            Return total
        End Get
    End Property
    Private Property _tlaGoodwill As New Dictionary(Of continent, Integer)
    Friend ReadOnly Property tlaGoodwill As Dictionary(Of continent, Integer)
        Get
            Return _tlaGoodwill
        End Get
    End Property
    Private Property _travelDistances As Dictionary(Of String, Dictionary(Of String, Integer)) = buildTravelDistances()
    Private Function buildTravelDistances() As Dictionary(Of String, Dictionary(Of String, Integer))
        Dim total As New Dictionary(Of String, Dictionary(Of String, Integer))
        Dim csv As List(Of String()) = csvFileget("data/distances.csv")
        For Each line In csv
            Dim origin As String = line(0)
            Dim destination As String = line(1)
            Dim distance As Integer = CInt(line(2))

            If origin <> destination Then
                If total.ContainsKey(origin) = False Then total.Add(origin, New Dictionary(Of String, Integer))
                If total(origin).ContainsKey(destination) = False Then total(origin).Add(destination, distance)
            End If
        Next
        Return total
    End Function
    Friend Function getDistance(origin As city, destination As city) As Integer
        If origin.name = destination.name Then Return 0

        Return _travelDistances(origin.name)(destination.name)
    End Function
End Class
