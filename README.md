# Twitch API Integration

This project is a C# application that interacts with the Twitch API to manage channel points rewards and listen to PubSub events. It is designed to be used on bigger projects, this is more like a library
![image](https://github.com/user-attachments/assets/6196098a-7b6a-4191-818e-e7d01e3afa4d)

## Features

- **Channel Points Management**: Create, update, and delete custom channel point rewards.
- **PubSub Integration**: Listen to Twitch PubSub events for real-time updates.
- **Testing and Debugging**: Includes various test methods to ensure functionality.

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) installed on your machine.
- A Twitch account with developer access to create an application and obtain OAuth tokens.

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Civermau/Twitch-API.git
   cd Twitch-API
   ```

2. Restore the dependencies:
   ```bash
   dotnet restore
   ```

### Configuration

Replace the placeholders in `Program.cs` with your actual Twitch credentials:
```csharp
private static string channelId = "<YOUR_CHANNEL_ID>";
private static string oauthToken = "<YOUR_OAUTH_TOKEN>";
private static string clientId = "<YOUR_CLIENT_ID>";
```

### Running the Application

To run the application, use the following command:
```bash
dotnet run
```

## Usage

- **CRUDTest**: Test creating, reading, updating, and deleting channel point rewards.
- **FullReadTest**: Retrieve and display all channel point rewards.
- **SameNameTest**: Test creating multiple rewards with the same name.
- **FullCreateTest**: Test creating rewards with various configurations.


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

Special thanks to Randomike for providing a testing channel.

For more details, see the [Credits](CREDITS.md) file.
