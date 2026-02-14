//using CapstoneCreeper;
using CapstoneCreeper;
using Godot;
using System;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
public partial class AIvAi : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	private static readonly string baseUrl = "https://softserve.harding.edu";
    private static readonly string CreateUrl = baseUrl + "/player/create";
    private static readonly string playStateUrl = baseUrl + "/aivai/play-state";
    private static readonly string actionUrl = baseUrl + "/aivai/submit-action";
    private static readonly string connectionToken = "";
    private static readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
    private Label label;
	public override void _Ready()
	{
        GD.Print("reached scene");
        label = GetNode<Label>("DifferentName");
        label.Text = "AI vs AI Tournament in progress";
        //This blocks the main thread and that is my intention. 
        

        //make method async, do some testing
        //await AiTournament();
    }
	public static async Task<int> CreatePlayer()
    { 
		//https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
        // ^Citation, this code is directly from docs

		/// This method takes no params but returns the token to be used to join the network
		/// this token should be stored and reused and this method should not be called every time
        /// 
        //serialize json object
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            name="Team8",
			email="cyoung10@harding.edu"
        }),
        Encoding.UTF8,
        "application/json");
        //make post request
        using HttpResponseMessage response = await client.PostAsync(CreateUrl,jsonContent);

        //deserialize response
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var resultingToken = JsonSerializer.Deserialize<Token>(jsonResponse);
        int returnValue = resultingToken.token;
        return returnValue;
	}
    public static async Task<Tuple<PlayState, System.Net.HttpStatusCode>> RequestPlayState() {
        // this method comes from same microsoft http client docs, 
        // This returns a tuple of playstate object and int, the int is the status code.
        // This looks complicated, I'm using a tuple because I need both a status code and a 
        // PlayState in the game loop. The rest is from c# docs, I'm using the playState object
        // because it works well with the json deserializer

        //initialize 
        PlayState playState = new PlayState();

        //serialize request
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            player = "Team8", // ??
            token = connectionToken,
            @event = "" // learned something new, how neat. I can use reserved key words.
        }),
        Encoding.UTF8,
        "application/json");

        //make request
        using HttpResponseMessage response = await client.PostAsync(playStateUrl, jsonContent);
        var statusCode = response.StatusCode;

        //deserialize response
        var jsonResponse = await response.Content.ReadAsStringAsync();
        playState = JsonSerializer.Deserialize<PlayState>(jsonResponse);


        //return
        Tuple<PlayState, System.Net.HttpStatusCode> returnValue= new Tuple<PlayState, System.Net.HttpStatusCode> (playState, statusCode);
        return returnValue;
    }
	public static async Task<AIWinnerResult> SendPlayState(int actionId, string Action) { 
        // this method requires the action id from the play state response and the action made by 
        // the AI. This returns an object with one field, a string for the winner
        // if there is no winner, it will say "none". otherwise, it will contain 'h', 't' or "draw"

        //serialize request
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            action = Action,
            player= "Team8", 
            token= connectionToken,
            action_id=actionId
        }),
        Encoding.UTF8,
        "application/json");

        //make request
        using HttpResponseMessage response = await client.PostAsync(actionUrl, jsonContent);

        //deserialize response
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
        var winner = JsonSerializer.Deserialize<AIWinnerResult>(jsonResponse);

        return winner;
    }

    public async Task AiTournament() {
        // the softserve integration guide specifies that this is the order 
        // the ai game loop should use.
        // 1. Request a state,
        // 2. if a 204 response code, wait a few seconds and try again.
        // 3. read the state.
        // 4. Calculate an action using the AI 
        // 5. Post the action to aivai/submit-action
        // 6. Go to step 1.
        while (true) // need to figure out loop condition
        {
            //1.
            Tuple<PlayState, System.Net.HttpStatusCode> playStateResponse = await RequestPlayState();

            //2. 
            if (playStateResponse.Item2 == System.Net.HttpStatusCode.NoContent)
            {
                await Task.Delay(3000);
            }
            else 
            {
                //3.
                PlayState playState = playStateResponse.Item1;
                // For Trent: Carry out the AI action with the given playstate
                //4.
                string aiMove = "";
                //5.
                await SendPlayState(playState.ActionID, aiMove);
            }
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
