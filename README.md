# Grapevine Coding Challenge

## Intro
The Grapevine Game (aka Chinese Whispers) is something that many of us will have played in the playground.  The idea being that one person comes up with a message and whispers it into someones ear.  The recipient of the message then says what they think they heard and passes the message on to the next person in the sequence until everyone has heard the message.  The message in its final form is then shared with the group along with the original message.  Normally these messages are very different!

## The task

I want you to create a web service which takes a message, 'interprets' it and then passes the interpretation on to the next recipient in a pre-defined sequence until the message has reached the end of the chain and been passed back to the initiating caller.

The interpretation stage will change the meaning of the message through a basic replacement process:
- Select a random word (preferably not a conjunction) from the message
- Replace it with a word that sounds like it or rhymes with it
- Pass the message on to the next recipient

### Requirements:
- When you receive a message, you should log it somewhere that can be easily read-back from
- Message should be interpretted then passed on unless sentFromId is the last id in the recipient list
- The last recipient in the list should interpret the message and pass it back to the first in the list
- If you receive the final message (sentFromId is the last in the sequence), you should log the final phrase
- If sentFromId is -1, then you are the first recipient.
- The state of a game is logged by the first recipient.
- You can see all games that have been kicked off by your service from the GET /games endpoint
- Get specific game initiated on your service (return 404 if game doesn't exist) from the GET /game/{gameId} endpoint
- If you are the only recipient in the list, the game should still function as described


### Example requests

Initial message from initiator (me):

POST /whisper
```json
{
    gameId: 1,
    message: "Some message for you",
    sentFromId: -1,
    nextWhisperRecipientId: 1,
    whisperRecipients: [
        {
            id: 0,
            url: "https://someurl.com/whisper"
        },
        {
            id: 1,
            url: "https://someurl2.com/whisper"
        },
        {
            id: 2,
            url: "https://someurl3.com/whisper"
        },
        {
            id: 3,
            url: "https://someurl4.com/whisper"
        }
    ]
}
```
### Properties
gameId (int) - The id of the game currently being played

message (string) - The message passed from the previous sender

sentFromId - if this is -1, then it has been sent from the game initiator, if it matches the last id in the recipients list, then you should not pass this message on again as it's the final message.

nextWhisperRecipientId (int) - the id of the recipient you need to pass the message on to.  If the value is 1, then you are the last sender, so 

whisperReceipients (array) - a collection of recipients, showing the full chain/list of players

Message from last recipient:

POST /whisper
```json
{
    gameId: 1,
    message: "Some message for you",
    sentFromId: 3,
    nextWhisperRecipientId: 1,
    whisperRecipients: [
        {
            id: 0,
            url: "https://someurl.com/whisper"
        },
        {
            id: 1,
            url: "https://someurl2.com/whisper"
        },
        {
            id: 2,
            url: "https://someurl3.com/whisper"
        },
        {
            id: 3,
            url: "https://someurl4.com/whisper"
        }
    ]
}
```

List all games initiated on your service:

GET /games
```json
{
    games: [
        {
            gameId: 1,
            gameStarted: "2020-05-06T09:00:00Z",
            gameEnded: "2020-05-06T09:01:00Z",
            startingMessage: "I would love a cup of tea right now!",
            endMessage: "Hi love a cup of wee tight pal!"
        },
        {
            gameId: 2,
            gameStarted: "2020-05-06T09:10:00Z",
            gameEnded: null,
            startingMessage: "I would love a cup of tea right now!",
            endMessage: null
        }
    ]
}
```

Get specific game initiated on your service (return 404 if game doesn't exist):

GET /game/1
```json
{
    gameId: 1,
    gameStarted: "2020-05-06T09:00:00Z",
    gameEnded: "2020-05-06T09:01:00Z",
    startingMessage: "I would love a cup of tea right now!",
    endMessage: "Hi love a cup of wee tight pal!"
}
```

## Resources

You can use the Datamuse API to generate a 'sounds like' word replacement: https://www.datamuse.com/api/

You could alternatively sign up for a free account on https://www.wordsapi.com/ and add some smarter logic to ensure you don't replace conjunctions (e.g. and/or) and use their 'ryhmes' endpoint to generate your replacement word.

## Submission and Results

You will have 2 weeks to get things going.  We can revise the timeline if people think they need more time.

You'll need to submit your code to me (ideally a github branch/fork of this repo) along with the url for your application.

Please do not touch the Grapevine.GameRunner project. That is what I will be working on to test the game.

Once all players have their service up, I will initiate some games and assess people's solutions.  Shout-outs will be for the following categories:

- Hacker
- Hacker's Dream
- Gold-Plated Potato
- Wooden Spoon

You may be asked to present your solution to the other participants.