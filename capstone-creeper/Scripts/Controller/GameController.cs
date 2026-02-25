using Godot;
using Model;
using View;
using System;
using static Model.GameBoard;
public partial class GameController : Node
{
    //private BoardView boardView;
    // Called when the node enters the scene tree for the first time.
    enum ControllerState
    {
        SelectingPin,
        MakingMove,
        GameOver
    }
    private enum Turn
    {
        Black,
        White
    }
    private Turn turn;
    private ControllerState controllerState;
    public GameBoard gameBoard;
    public BoardView boardView;
    private Vector2I selectedPin;
    public string NetworkGameOver = ""; 
    public static GameController Controller { get; set; }
    public override void _Ready()
    {
        gameBoard = new GameBoard();
        gameBoard.InitBoard();
        boardView = GetNode<BoardView>("../BoardView");
        boardView.updateBoard(gameBoard);

        turn = new Turn();
        turn = Turn.White;
        controllerState = new ControllerState();
        controllerState = ControllerState.SelectingPin;

        selectedPin = new Vector2I();

        boardView.Connect("PinClicked", new Callable(this, nameof(HandlePinClicked)));

    }
    public bool GameOver(Vector2I from, Vector2I to) {
        var tempGameBoard = gameBoard;
        tempGameBoard.makeMove(from, to);
        tempGameBoard.clearPossibleMoves();
        if (tempGameBoard.checkDraw()) {
            return true;
        }
        if (tempGameBoard.checkWin()) {
            return true;
        }
        return false;
    }
    public override void _EnterTree()
    {
        //instantiate controller before rpcs
        Controller = this;
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
   
    public void ApplyMove(Vector2I from, Vector2I to)
    {
        gameBoard.makeMove(from, to);
        gameBoard.clearPossibleMoves();
        boardView.updateBoard(gameBoard);

        if (turn == Turn.Black)
        {
            turn = Turn.White;
        }
        else
        {
            turn = Turn.Black;
        }
        selectedPin = new Vector2I(-1, -1);
    }
    private void HandlePinClicked(Vector2I boardPos)
    {
        if (controllerState == ControllerState.SelectingPin)
        {
            if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.White && turn == Turn.White)
            {
                if (GameSettings.Mode == GameMode.OnlineMultiplayer)
                {
                    if (Multiplayer.IsServer())
                    {
                        selectedPin = boardPos;
                        gameBoard.HighlightPossibleMoves(boardPos);
                        controllerState = ControllerState.MakingMove;
                        boardView.updateBoard(gameBoard);
                    }
                }
                else
                {
                    selectedPin = boardPos;
                    gameBoard.HighlightPossibleMoves(boardPos);
                    controllerState = ControllerState.MakingMove;
                    boardView.updateBoard(gameBoard);
                }
            }
            else if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.Black && turn == Turn.Black)
            {
                if (GameSettings.Mode == GameMode.OnlineMultiplayer)
                {
                    if (!Multiplayer.IsServer())
                    {
                        selectedPin = boardPos;
                        gameBoard.HighlightPossibleMoves(boardPos);
                        controllerState = ControllerState.MakingMove;
                        boardView.updateBoard(gameBoard);
                    }
                }
                else
                {
                    selectedPin = boardPos;
                    gameBoard.HighlightPossibleMoves(boardPos);
                    controllerState = ControllerState.MakingMove;
                    boardView.updateBoard(gameBoard);
                }
            }
        }
        else if (controllerState == ControllerState.MakingMove)
        {
            if (boardPos == selectedPin)
            {
                gameBoard.clearPossibleMoves();
                controllerState = ControllerState.SelectingPin;
                selectedPin = new Vector2I(-1, -1);
                boardView.updateBoard(gameBoard);
            }
            else
            {
                if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.PossibleMove)
                {

                    if (GameSettings.Mode == GameMode.OnlineMultiplayer)
                    { // if multiplayer, send the move, and apply locally
                        GetNode<NetworkManager>("/root/Network").SendMove(selectedPin, boardPos);
                    }
                    gameBoard.makeMove(selectedPin, boardPos);
                    gameBoard.clearPossibleMoves();
                    boardView.updateBoard(gameBoard);
                    if (gameBoard.checkWin())
                    {
                        controllerState = ControllerState.GameOver;
                        
                    }
                    else if (gameBoard.checkDraw())
                    {
                        controllerState = ControllerState.GameOver;
                        
                    }
                    else
                    {
                        controllerState = ControllerState.SelectingPin;
                    }
                    if (turn == Turn.White)
                    {
                        turn = Turn.Black;
                    }
                    else
                    {
                        turn = Turn.White;
                    }
                    selectedPin = new Vector2I(-1, -1);

                }
            }
        }
    }
}