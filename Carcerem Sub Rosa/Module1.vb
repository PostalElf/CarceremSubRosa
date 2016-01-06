Module Module1

    Sub Main()
        Console.SetWindowSize(80, 70)

        Dim world As world = world.buildWorld
        Dim player As New player(world)
        Dim researchProject As New researchProject("Top-Secret Project", 1000, Nothing, Nothing)
        player.devAddResearchProjectOpen(researchProject)
        player.changeResearchProject(researchProject)

        Dim city1 As city = world.cities(continent.China)(2)
        Dim city2 As city = world.cities(continent.AmericaN)(0)
        Dim citysite1 As New citysite(player, city1)
        Dim citysite2 As New citysite(player, city1)
        Dim citysite3 As New citysite(player, city1)
        Dim citysite4 As New citysite(player, city1)
        Dim shell1 As New shellcompany("", player, city1)
        Dim shell2 As New shellcompany("", player, city2)
        Dim research As New researchlab("", citysite1, shell2)
        Dim factory As New factory("", citysite2, shell1)
        Dim safehouse As New safehouse("", citysite3, shell1)
        Dim tacsupport As New tacsupport("", citysite4, shell1)
        Dim laundry As New laundry("", shell1)

        Dim blueprint As New product("Sheepskin")
        player.devAddBlueprint(blueprint)
        city2.addDemand(1)
        player.createTradeRoute(blueprint, factory, city2)

        Dim agent1 As agent = agent.buildRandomAgent(player)
        Dim agent2 As agent = agent.buildRandomAgent(player)
        Dim squad As squad = squad.buildRandomSquad(player, city1)
        squad.addAgent(agent1)
        squad.addAgent(agent2)
        agent1.changeRelationship(agent2, choiceComponent.Practical)
        'squad.moveTo(city2)

        Dim stage1 As New missionStage("Dress Up", 5, 10, New Dictionary(Of choiceComponent, Integer) From {{choiceComponent.Diplomacy, 1}}, "stonewall 0")
        Dim stage2 As New missionStage("Travel to the Party", 7, 10, Nothing, "stonewall 0")
        Dim stage3 As New missionStage("Eat!", 15, 10, Nothing, "stonewall 20")
        Dim stage4 As New missionStage("Assassinate VIP", 18, 10, Nothing, "health 5")
        Dim stageStack As New Stack(Of missionStage)
        stageStack.Push(stage4)
        stageStack.Push(stage3)
        stageStack.Push(stage2)
        stageStack.Push(stage1)
        Dim mission As New mission("Sample Cheese & Wines", city1, stageStack)
        mission.squad = squad

        While True
            Console.Clear()
            player.consoleReport(0)
            Console.WriteLine(vbCrLf)

            player.fullConsoleReport(0)
            Console.ReadKey()

            player.tick()
        End While
    End Sub
End Module
