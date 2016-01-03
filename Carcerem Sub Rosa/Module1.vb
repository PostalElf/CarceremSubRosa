Module Module1

    Sub Main()
        Console.SetWindowSize(80, 50)

        Dim world As world = world.buildWorld
        Dim player As New player
        Dim city As city = world.cities(continent.Africa)(1)
        Dim citysite1 As New citysite(player, city)
        Dim citysite2 As New citysite(player, city)
        Dim citysite3 As New citysite(player, city)
        Dim citysite4 As New citysite(player, city)
        Dim city2 As city = world.cities(continent.China)(0)
        Dim shell As New shellcompany("", player, city)
        Dim research As New researchlab("", citysite1, shell)
        Dim factory As New factory("", citysite2, shell)
        Dim safehouse As New safehouse("", citysite3, shell)
        Dim tacsupport As New tacsupport("", citysite4, shell)
        Dim laundry As New laundry("", shell)

        Dim blueprint As New product("Sheepskin")
        player.addBlueprint(blueprint)
        city2.addDemand(1)
        player.createTradeRoute(blueprint, factory, city2)

        player.consoleReport(0)
        Console.WriteLine(vbCrLf)
        shell.consoleReport(0)
        For Each holding In shell.holdings
            Console.WriteLine()
            holding.consoleReport(1)
        Next
        Console.ReadKey()
    End Sub

End Module
