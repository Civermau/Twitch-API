using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomReward;
using TwitchLib.Api.Helix.Models.ChannelPoints.GetCustomReward;
using TwitchLib.Api.Helix.Models.ChannelPoints;

/// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="T:TwitchChannelPointsRewardsManager"]/*'/>
public class TwitchChannelPointsRewardsManager
{
    private readonly TwitchAPI _api;
    private readonly string _channelId;

    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="P:TwitchChannelPointsRewardsManager.CreatedRewards"]/*'/>
    public List<CustomReward> CreatedRewards { get; set; } = new List<CustomReward>(); 

    // I'm an idiot, I was using the channel ID as the client ID
    // It took me two whole days to figure that out
    // I realized when I was trying manyally with curl
    // I noticed I was not using the client ID I was using the channel ID
    // I changed it and it worked
    // I'm an idiot

    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="M:TwitchChannelPointsRewardsManager.#ctor"]/*'/>
    public TwitchChannelPointsRewardsManager(string channelId, string oauthToken, string clientId)
    {
        _api = new TwitchAPI();
        _api.Settings.AccessToken = oauthToken;
        _api.Settings.ClientId = clientId;
        _channelId = channelId;
    }


    //* -------------------------------------------------------- Create Channel Point Reward --------------------------------------------------------
    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="M:TwitchChannelPointsRewardsManager.CreateChannelPointReward"]/*'/>
    public async Task<RewardResponse> CreateChannelPointReward(
        string title = "Default Reward", 
        string prompt = "Default Prompt", 
        int cost = 100,
        string backgroundColor = "#2596be",
        bool isUserInputRequired = false,
        bool isMaxPerStreamEnabled = false,
        int? maxPerStream = null,
        bool isMaxPerUserPerStreamEnabled = false,
        int? maxPerUserPerStream = null,
        bool isGlobalCooldownEnabled = false,
        int? globalCooldownSeconds = null)
    {
        var ChannelPointsRequest = new CreateCustomRewardsRequest
        {
            Title = title,
            Prompt = prompt,
            Cost = cost,
            IsEnabled = true,
            BackgroundColor = backgroundColor,
            IsUserInputRequired = isUserInputRequired,
            IsMaxPerStreamEnabled = isMaxPerStreamEnabled,
            MaxPerStream = maxPerStream,
            IsMaxPerUserPerStreamEnabled = isMaxPerUserPerStreamEnabled,
            MaxPerUserPerStream = maxPerUserPerStream,
            IsGlobalCooldownEnabled = isGlobalCooldownEnabled,
            GlobalCooldownSeconds = globalCooldownSeconds
        };

        var rewardResponse = await GetChannelPointRewards();
        if (rewardResponse.Data.Count() >= 50)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can only have 50 rewards at a time");
            Console.ResetColor();
            return new RewardResponse
            {
                IsSuccess = false,
                ErrorMessage = "You can only have 50 rewards at a time"
            };
        }
    
        try
        {
            var response = await _api.Helix.ChannelPoints.CreateCustomRewardsAsync(_channelId, ChannelPointsRequest);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Reward created successfully. ID: " + response.Data[0].Id + " Title: " + response.Data[0].Title);
            Console.ResetColor();

            CreatedRewards.Add(response.Data[0]);

            return new RewardResponse
            {
                IsSuccess = true,
                Reward = response.Data[0]
            };
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error creating reward: " + ex.Message);
            Console.ResetColor();
            return new RewardResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
    //* -------------------------------------------------------- Update Channel Point Reward --------------------------------------------------------
    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="M:TwitchChannelPointsRewardsManager.UpdateChannelPointReward"]/*'/>
    public async Task<RewardResponse> UpdateChannelPointReward(
        string rewardId, 
        string? title = null, 
        string? prompt = null, 
        int? cost = null,
        string? backgroundColor = null,
        bool? isUserInputRequired = null,   
        bool? isMaxPerStreamEnabled = null,
        int? maxPerStream = null,
        bool? isMaxPerUserPerStreamEnabled = null,
        int? maxPerUserPerStream = null,
        bool? isGlobalCooldownEnabled = null,
        int? globalCooldownSeconds = null)
    {
        
        var reward = CreatedRewards.FirstOrDefault(reward => reward.Id == rewardId);
        if (reward == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Reward not found");
            Console.ResetColor();
            return new RewardResponse
            {
                IsSuccess = false,
                ErrorMessage = "Reward not found"
            };
        }

        var ChannelPointsRequest = new UpdateCustomRewardRequest
        {
            Title = title ?? reward.Title,
            Prompt = prompt ?? reward.Prompt,
            Cost = cost ?? reward.Cost,
            IsEnabled = true,
            BackgroundColor = backgroundColor ?? reward.BackgroundColor
        };

        // TODO: Add cooldown later
        try
        {
            var response = await _api.Helix.ChannelPoints.UpdateCustomRewardAsync(_channelId, rewardId, ChannelPointsRequest);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Reward updated successfully. ID: " + response.Data[0].Id + " Title: " + response.Data[0].Title);
            Console.ResetColor();
            CreatedRewards.Remove(reward);
            CreatedRewards.Add(response.Data[0]);

            // Update the reward in the list

            return new RewardResponse
            {
                IsSuccess = true,
                Reward = response.Data[0]
            };
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error updating reward: " + ex.Message);
            Console.ResetColor();
            return new RewardResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    //* -------------------------------------------------------- Delete Channel Point Reward --------------------------------------------------------   
    
    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="M:TwitchChannelPointsRewardsManager.DeleteChannelPointReward"]/*'/>
    public async Task DeleteChannelPointReward(string rewardId)
    {
        try
        {
            await _api.Helix.ChannelPoints.DeleteCustomRewardAsync(_channelId, rewardId);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Reward deleted successfully.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error deleting reward: " + ex.Message);
            Console.WriteLine("Youll need to delete the reward manually, sorry!");
            Console.ResetColor();
        }
    }

    //* -------------------------------------------------------- Get Channel Point Rewards --------------------------------------------------------
    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="M:TwitchChannelPointsRewardsManager.GetChannelPointRewards"]/*'/>
    public async Task<GetCustomRewardsResponse> GetChannelPointRewards()
    {
        var response = await _api.Helix.ChannelPoints.GetCustomRewardAsync(_channelId);
        return response;
    }

    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="M:TwitchChannelPointsRewardsManager.GetChannelPointRewardsByTitle"]/*'/>
    public async Task<RewardResponse> GetChannelPointRewardsByTitle(string title)
    {
        var response = await GetChannelPointRewards();
        var reward = response.Data.FirstOrDefault(reward => reward.Title == title);
        if (reward == null)
        {
            return new RewardResponse
            {
                IsSuccess = false,
                ErrorMessage = "Reward not found"
            };      
        }
        return new RewardResponse
        {
            IsSuccess = true,
            Reward = reward
        };
    }

    //* -------------------------------------------------------- Clear Created Rewards --------------------------------------------------------
    /// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="M:TwitchChannelPointsRewardsManager.ClearCreatedRewards"]/*'/>
    public async Task ClearCreatedRewards()
    {
        var rewardsToRemove = new List<CustomReward>(CreatedRewards);
        foreach (var reward in rewardsToRemove)
        {
            try
            {
                await DeleteChannelPointReward(reward.Id);
                CreatedRewards.Remove(reward);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting reward: " + ex.Message);
            }
        }
    }

    //* -------------------------------------------------------- Helpers --------------------------------------------------------
    private void WriteColoredLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private void WriteColoredLabel(string label, ConsoleColor color, object value)
    {
        Console.ForegroundColor = color;
        Console.Write(label);
        Console.ResetColor();
        Console.WriteLine(value);
    }
}

/// <include file='TwitchChannelPointsRewardsManager.Docs.xml' path='doc/members/member[@name="T:TwitchChannelPointsRewardsManager.RewardResponse"]/*'/>
public class RewardResponse
{
    public bool IsSuccess { get; set; }
    public CustomReward? Reward { get; set; }
    public string? ErrorMessage { get; set; }
}
