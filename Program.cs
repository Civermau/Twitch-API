using TwitchLib.Api.Helix.Models.Bits.ExtensionBitsProducts;
using TwitchLib.Api.Helix.Models.ChannelPoints.GetCustomReward;

class Program
{
    private static string channelId = "<YOUR_CHANNEL_ID>";
    private static string oauthToken = "<YOUR_OAUTH_TOKEN>";
    private static string clientId = "<YOUR_CLIENT_ID>";
    static async Task Main(string[] args)
    {
        var bot = new TwitchPubSubReader(channelId, oauthToken);

        // await CRUDTest();
        // await SameNameTest();
        // await FullReadTest();
        await FullCreateTest();
    }

    private static async Task CRUDTest(){
        var rewardsManager = new TwitchChannelPointsRewardsManager(channelId, oauthToken, clientId);

        GetCustomRewardsResponse rewardsResponse;

        rewardsResponse = await rewardsManager.GetChannelPointRewards();
        Console.WriteLine($"Channel point rewards count: {rewardsResponse.Data.Count()}");

        for (int i = 1; i < 4; i++)
        {
            await rewardsManager.CreateChannelPointReward("Test" + i, "A reward with twitchlib", 2);
        }

        rewardsResponse = await rewardsManager.GetChannelPointRewards();
        Console.WriteLine($"Channel point rewards count: {rewardsResponse.Data.Count()}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press any key to edit a reward...");
        Console.ResetColor();
        Console.ReadKey();

        var reward = await rewardsManager.GetChannelPointRewardsByTitle("Test1");   
        if (reward.IsSuccess)
        {
            await rewardsManager.UpdateChannelPointReward(
                rewardId: reward.Reward.Id,
                title: "Edited Test" + 1,
                prompt: "A edited reward",
                cost: 2
            );
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press any key to clear rewards...");
        Console.ResetColor();
        Console.ReadKey();

        await rewardsManager.ClearCreatedRewards();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press any key to exit...");
        Console.ResetColor();
        Console.ReadKey();
    }

    private static async Task FullReadTest(){
        var rewardsManager = new TwitchChannelPointsRewardsManager(channelId, oauthToken, clientId);

        var rewards = await rewardsManager.GetChannelPointRewards();
        Console.WriteLine($"Channel point rewards count: {rewards.Data.Count()}");

        var sortedRewards = rewards.Data.ToList();
        sortedRewards.Sort((a, b) => a.Cost.CompareTo(b.Cost));

        foreach (var reward in sortedRewards)
        {
            Console.WriteLine($"Reward: {reward.Title, -40} - Cost: {reward.Cost, -8} - index: {sortedRewards.IndexOf(reward), -8}");
        }
    }

    private static async Task SameNameTest(){
        var rewardsManager = new TwitchChannelPointsRewardsManager(channelId, oauthToken, clientId);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Creating 3 test rewards with the same name...");
        Console.ResetColor();

        await rewardsManager.CreateChannelPointReward(
            title: "Test Reward"
        );

        await rewardsManager.CreateChannelPointReward(
            title: "Test Reward"
        );

        await rewardsManager.CreateChannelPointReward(
            title: "Test Reward"
        );        

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press any key to exit...");
        Console.ResetColor();
        Console.ReadKey();

        await rewardsManager.ClearCreatedRewards();
    }

    private static async Task FullCreateTest(){
        var rewardsManager = new TwitchChannelPointsRewardsManager(channelId, oauthToken, clientId);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Creating 5 test rewards...");
        Console.ResetColor();
        var rewards = await rewardsManager.GetChannelPointRewards();
        Console.WriteLine($"Channel point rewards count: {rewards.Data.Count()}");

        await rewardsManager.CreateChannelPointReward(
            cost: 80
        );
            
        await rewardsManager.CreateChannelPointReward(
            title: "Test Reward",
            prompt: "Prompt",
            cost: 80
        );

        //Test with user input
        await rewardsManager.CreateChannelPointReward(
            title: "Test Input Reward",
            prompt: "Prompt",
            cost: 80,
            isUserInputRequired: true
        );

        //Test with cooldown
        await rewardsManager.CreateChannelPointReward(
            title: "Test Cooldown Reward",
            prompt: "Prompt",
            cost: 80,
            isGlobalCooldownEnabled: true,
            globalCooldownSeconds: 10
        );

        //Test with max per stream
        await rewardsManager.CreateChannelPointReward(
            title: "Test Max Per Stream Reward",
            prompt: "Prompt",
            cost: 80,
            isMaxPerStreamEnabled: true,
            maxPerStream: 10
        );
    
        rewards = await rewardsManager.GetChannelPointRewards();
        Console.WriteLine($"Channel point rewards count: {rewards.Data.Count()}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press any key to edit the rewards...");
        Console.ResetColor();
        Console.ReadKey();

        var reward = await rewardsManager.GetChannelPointRewardsByTitle("Test Reward");
        if (reward.IsSuccess)
        {
            await rewardsManager.UpdateChannelPointReward(reward.Reward.Id, title: "Edited Test Reward");
        }

        reward = await rewardsManager.GetChannelPointRewardsByTitle("Test Input Reward");
        if (reward.IsSuccess)
        {
            await rewardsManager.UpdateChannelPointReward(reward.Reward.Id, title: "Edited Test Input Reward");
        }

        reward = await rewardsManager.GetChannelPointRewardsByTitle("Test Cooldown Reward");
        if (reward.IsSuccess)
        {
            await rewardsManager.UpdateChannelPointReward(reward.Reward.Id, title: "Edited Test Cooldown Reward");
        }
        
        reward = await rewardsManager.GetChannelPointRewardsByTitle("Test Max Per Stream Reward");
        if (reward.IsSuccess)
        {
            await rewardsManager.UpdateChannelPointReward(reward.Reward.Id, title: "Edited Test Max Per Stream Reward");
        }

        reward = await rewardsManager.GetChannelPointRewardsByTitle("Default Reward");
        if (reward.IsSuccess)
        {
            await rewardsManager.UpdateChannelPointReward(reward.Reward.Id, title: "Edited Default Reward");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press any key to exit...");
        Console.ResetColor();
        Console.ReadKey();

        await rewardsManager.ClearCreatedRewards();

        rewards = await rewardsManager.GetChannelPointRewards();
        Console.WriteLine($"Channel point rewards count: {rewards.Data.Count()}");  
    }
}