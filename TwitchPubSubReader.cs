using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

/// <include file='TwitchPubSubReader.Docs.xml' path='doc/members/member[@name="T:TwitchPubSubReader"]/*'/>
public class TwitchPubSubReader
{
    private TwitchPubSub _client;
    // Replace with your channel's numeric ID (as a string) and your OAuth token.
    private readonly string channelId;
    private readonly string oauthToken;

    /// <include file='TwitchPubSubReader.Docs.xml' path='doc/members/member[@name="M:TwitchPubSubReader.#ctor"]/*'/>
    public TwitchPubSubReader(string channelId, string oauthToken)
    {
        this.channelId = channelId;
        this.oauthToken = oauthToken;
        // Create an instance of the TwitchPubSub client.
        _client = new TwitchPubSub();

        // Hook up event handlers.
        _client.OnPubSubServiceConnected += OnPubSubServiceConnected;
        _client.OnListenResponse += OnListenResponse;
        _client.OnBitsReceivedV2 += OnBitsReceived;
        _client.OnChannelPointsRewardRedeemed += OnChannelPointsRewardRedeemed;

        // Connect to Twitch PubSub service.
        _client.Connect();
    }

    /// <include file='TwitchPubSubReader.Docs.xml' path='doc/members/member[@name="M:TwitchPubSubReader.OnPubSubServiceConnected"]/*'/>
    private void OnPubSubServiceConnected(object? sender, EventArgs e)
    {
        Console.WriteLine("Connected to Twitch PubSub!");

        // Subscribe to bits events for the channel.
        _client.ListenToBitsEventsV2(channelId);

        // Subscribe to channel points reward redemptions.
        _client.ListenToChannelPoints(channelId);

        // Send topics with your OAuth token.
        // This step is required for topics that need authentication.
        _client.SendTopics(oauthToken);
    }

    /// <include file='TwitchPubSubReader.Docs.xml' path='doc/members/member[@name="M:TwitchPubSubReader.OnListenResponse"]/*'/>
    private void OnListenResponse(object? sender, OnListenResponseArgs e)
    {
        if (!e.Successful)
        {
            Console.WriteLine($"Failed to listen! Response: {e.Response}");
        }
        else
        {
            Console.WriteLine($"Now listening to topic: {e.Topic}");
        }
    }

    /// <include file='TwitchPubSubReader.Docs.xml' path='doc/members/member[@name="M:TwitchPubSubReader.OnBitsReceived"]/*'/>
    private void OnBitsReceived(object? sender, OnBitsReceivedV2Args e)
    {
        WriteColoredLine("Bits event received:", ConsoleColor.Green);
        WriteColoredLabel("User: ", ConsoleColor.Blue, e.UserName);
        WriteColoredLabel("Channel: ", ConsoleColor.Blue, e.ChannelName);
        WriteColoredLabel("Bits used: ", ConsoleColor.Blue, e.BitsUsed);
        WriteColoredLabel("Total Bits: ", ConsoleColor.Blue, e.TotalBitsUsed);
        WriteColoredLabel("Cheer type: ", ConsoleColor.Blue, e.ChatMessage);
        Console.WriteLine();
    }

    /// <include file='TwitchPubSubReader.Docs.xml' path='doc/members/member[@name="M:TwitchPubSubReader.OnChannelPointsRewardRedeemed"]/*'/>
    private void OnChannelPointsRewardRedeemed(object? sender, OnChannelPointsRewardRedeemedArgs e)
    {
        WriteColoredLine("Channel point reward redeemed:", ConsoleColor.Blue);
        WriteColoredLabel("User: ", ConsoleColor.Blue, e.RewardRedeemed.Redemption.User.DisplayName);
        WriteColoredLabel("Reward: ", ConsoleColor.Blue, e.RewardRedeemed.Redemption.Reward.Title);
        WriteColoredLabel("Cost: ", ConsoleColor.Blue, e.RewardRedeemed.Redemption.Reward.Cost);
        WriteColoredLabel("User Input: ", ConsoleColor.Blue, e.RewardRedeemed.Redemption.UserInput);
        Console.WriteLine();
    }

    // Add these helper methods to the class
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