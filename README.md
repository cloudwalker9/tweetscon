#TweetsCon

TweetsCon - .NET 6 C# console app sample to consume http stream from tweeter and persist the tweets in SQLLite DB

## How to on your local machine
In ContentStream\appsettings.json file, update the 'TwitterAuthToken' value with the auth token you received from [Twitter Developer] (https://developer.twitter.com/en/products/twitter-api).  After update open .sln file from visual studio and clieck on run.


```json
{
  "TwitterAuthToken": "<<< Add tweeter api token >>>",
  "TwitterStreamURI": "https://api.twitter.com/2/tweets/sample/stream",

}
``` 

## Output
Successful execution will print the Total Tweets and total transaction processed / minutes.
```bash
info: TweetStreamCon.Publisher.ConsolePublisher[0]
      **************2022-08-21T20:28:44 Tweet Count: 197775 ************** Processed / Minute:  74884
info: TweetStreamCon.Publisher.ConsolePublisher[0]
      **************2022-08-21T20:28:54 Tweet Count: 198211 ************** Processed / Minute:  75323
info: TweetStreamCon.Publisher.ConsolePublisher[0]
      **************2022-08-21T20:29:04 Tweet Count: 198654 ************** Processed / Minute:  75764
info: TweetStreamCon.Publisher.ConsolePublisher[0]
      **************2022-08-21T20:29:14 Tweet Count: 199124 ************** Processed / Minute:  76234
info: TweetStreamCon.Publisher.ConsolePublisher[0]
      **************2022-08-21T20:29:24 Tweet Count: 199521 ************** Processed / Minute:  76630
```
## Known Issues
- "Total transaction processed / minutes" value calculaion is incorrect.

## License
[MIT](https://choosealicense.com/licenses/mit/)
