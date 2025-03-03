# Twitch API Usage

## Authorization URL

```
https://id.twitch.tv/oauth2/authorize
    ?response_type=token
    &client_id=<YOUR_CLIENT_ID>
    &redirect_uri=<YOUR_REDIRECT_URI>
    &scope=channel:manage:redemptions+bits:read
```

## Create Custom Reward

```bash
curl -X POST 'https://api.twitch.tv/helix/channel_points/custom_rewards?broadcaster_id=<YOUR_BROADCASTER_ID>' \
-H 'client-id: <YOUR_CLIENT_ID>' \
-H 'Authorization: Bearer <YOUR_ACCESS_TOKEN>' \
-H 'Content-Type: application/json' \
-d '{
  "title":"test reward with curl",
  "cost":2
}'
```

## Delete Custom Reward

```bash
curl -X DELETE 'https://api.twitch.tv/helix/channel_points/custom_rewards?broadcaster_id=<YOUR_BROADCASTER_ID>&id=<YOUR_REWARD_ID>' \
-H 'Client-Id: <YOUR_CLIENT_ID>' \
-H 'Authorization: Bearer <YOUR_ACCESS_TOKEN>'
```

## Get Custom Rewards

```bash
curl -X GET 'https://api.twitch.tv/helix/channel_points/custom_rewards?broadcaster_id=<YOUR_BROADCASTER_ID>' \
-H 'Client-Id: <YOUR_CLIENT_ID>' \
-H 'Authorization: Bearer <YOUR_ACCESS_TOKEN>'
```

All of this can be made in Postman too though.