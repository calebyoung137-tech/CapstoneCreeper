using Godot;
using Godot.Collections;
using Model; 
using System;
using static View.BoardView;
namespace View; 

public partial class BoardView : Node2D
{
	[Signal]
	public delegate void PinClickedEventHandler(Vector2I pin);

	//private int pinSize = 16;
	//private int tileSize = 50; 

	public partial class Tile : Sprite2D
	{
		public Vector2I GridPos; // e.g., (x, y) in the grid
	}
	public partial class Pin : Area2D
	{
		public Vector2I GridPos; // e.g., (x, y) in the grid
	}

	public Dictionary<Vector2I, Tile> tiles = new Dictionary<Vector2I, Tile>();
	public Dictionary<Vector2I, Pin> pins = new Dictionary<Vector2I, Pin>();

	// Called when the node enters the scene tree for the first time.
 
	public override void _Ready()
	{
		Position = new Vector2(712, 285);

		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				if (!((i == 0 && j == 0) || (i == 6 && j == 6) || (i == 0 && j == 6) || (i == 6 && j == 0)))
				{
					Pin pin = new Pin();

					pin.GridPos.X = i;
					pin.GridPos.Y = j;
					pins[new Vector2I(i, j)] = pin;


					//setting a collisionshape so that our area2d can be clicked. 
					CollisionShape2D collision = new CollisionShape2D();

					RectangleShape2D rect = new RectangleShape2D();
					rect.Size = new Vector2(64, 64);   // size of clickable area

					collision.Shape = rect;

					pin.AddChild(collision);

					IdleKnight knight = new IdleKnight();
					knight.ZIndex = 4; 
					
					pin.MouseEntered += () => OnMouseEntered(pin);
					pin.MouseExited += () => OnMouseExited(pin);
					pin.AddChild(knight);
					pin.Position = new Vector2((i * 98) - 48, (j * 98) - 48 - 8);
					AddChild(pin); 
					
					pin.InputPickable = true;

					pin.InputEvent += (Node viewport, InputEvent @event, long shapeIdx) =>
					{
						if (@event is InputEventMouseButton mouseEvent &&
							mouseEvent.Pressed &&
							mouseEvent.ButtonIndex == MouseButton.Left)
						{
							EmitSignal("PinClicked", pin.GridPos);
						}
					};
				}
			}
		}
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				Tile tile = new Tile();
				tile.GridPos.X = i;
				tile.GridPos.Y = j;
				tiles[new Vector2I(i, j)] = tile;

				// Set size and color
				//tile.Size = new Vector2(30, 30);


				//tile.Color = new Color(0.2f, 0.6f, 1.0f); // light blue
				if (i == 0 && j == 0)
				{
					tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Blue Buildings/Castle.png");
					tile.Scale = new Vector2(0.5f, 0.5f); 
					tile.Position = new Vector2(i * 98 - 49 - 5, j * 98 - 49 - 15);
				}
				else if (i == 0 && j == 5)
				{
					tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Red Buildings/Castle.png");
					tile.Scale = new Vector2(0.5f, 0.5f);
					tile.Position = new Vector2(i * 98 - 49 - 5, j * 98 + 49 - 15);
				}
				else if (i == 5 && j == 0)
				{
					tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Red Buildings/Castle.png");
					tile.Scale = new Vector2(0.5f, 0.5f);
					tile.Position = new Vector2(i * 98 + 49 + 5, j * 98 - 49 - 15);
				}
				else if (i == 5 && j == 5)
				{
					tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Blue Buildings/Castle.png");
					tile.Scale = new Vector2(0.5f, 0.5f);
					tile.Position = new Vector2(i * 98 + 49 + 5, j * 98 + 49 - 15);
				}
				else
				{
					tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Black Buildings/Tower.png");
					// Optional: position it
					tile.Position = new Vector2(i * 98, j * 98 - 8);
					tile.Scale = new Vector2(0.5f, 0.5f);
				}
				tile.ZIndex = 3; 
				AddChild(tile);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnMouseEntered(Pin pin)
	{
		if (clickable(pin.GridPos))
		{
			pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0).Scale = new Vector2(0.85f, 0.85f); // slightly larger
		}
	}

	private void OnMouseExited(Pin pin)
	{
		if (clickable(pin.GridPos))
		{
			pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0).Scale = new Vector2(0.7f, 0.7f); // back to normal
		}
	}
	public void updateBoard(in GameBoard gameBoard)
	{
		for (int i = 0; i < gameBoard.Pins.GetLength(0); i++)
		{
			for (int j = 0; j < gameBoard.Pins.GetLength(1); j++)
			{
				if (pins.TryGetValue(new Vector2I(i, j), out Pin pin))
				{

					if (ChangedValue(gameBoard.Pins[i, j], pin) 
						&& !(gameBoard.Pins[i,j] == PinType.PossibleMove
						&& pin.GetChild<IdleKnight>(1).knightColor == "Purple"
						|| gameBoard.Pins[i, j] == PinType.Empty
						&& pin.GetChild<IdleKnight>(1).knightColor == "Black"))
					{
						pin.GetChild<IdleKnight>(1).PlayDustEffect(new Vector2(0, -16));
					}
					if (gameBoard.Pins[i, j] == PinType.Empty)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = false;
						pin.GetChild<IdleKnight>(1).setKnight("Purple", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));


					}
					else if (gameBoard.Pins[i, j] == PinType.Black)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = true;
						pin.GetChild<IdleKnight>(1).setKnight("Blue", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));


					}
					else if (gameBoard.Pins[i, j] == PinType.White)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = true;
						pin.GetChild<IdleKnight>(1).setKnight("Red", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));


					}
					else if (gameBoard.Pins[i, j] == PinType.PossibleMove)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = true;
						pin.GetChild<IdleKnight>(1).setKnight("Black", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));

					}
					pin.GetChild<IdleKnight>(1).SetOutline(pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0), false);
				}
			}
			}
		

		for (int i = 0; i < gameBoard.Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < gameBoard.Tiles.GetLength(1); j++)
			{
				if (!(i == 0 && j == 0 || i == 0 && j == 5 || i == 5 && j == 0 || i == 5 && j == 5)) {
					if (tiles.TryGetValue(new Vector2I(i, j), out Tile tile))
					{
						if (gameBoard.Tiles[i, j] == TileType.Empty)
						{
							tile.Texture = null;
						}
						else if (gameBoard.Tiles[i, j] == TileType.Black)
						{
							tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Blue Buildings/Tower.png");

						}
						else if (gameBoard.Tiles[i, j] == TileType.White)
						{
							tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Red Buildings/Tower.png");

						}
					}
				}
			}
		}

		if (GameController.Controller.controllerState == GameController.ControllerState.MakingMove)
		{
		   if (pins.TryGetValue(GameController.Controller.selectedPin, out Pin pin))
			{
			pin.GetChild<IdleKnight>(1).SetOutline(pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0), true); // back to normal

			}

		}
	}
	public void initializeBoard(in GameBoard gameBoard)
	{
		for (int i = 0; i < gameBoard.Pins.GetLength(0); i++)
		{
			for (int j = 0; j < gameBoard.Pins.GetLength(1); j++)
			{
				if (pins.TryGetValue(new Vector2I(i, j), out Pin pin))
				{

					
					if (gameBoard.Pins[i, j] == PinType.Empty)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = false;
						pin.GetChild<IdleKnight>(1).setKnight("Purple", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));


					}
					else if (gameBoard.Pins[i, j] == PinType.Black)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = true;
						pin.GetChild<IdleKnight>(1).setKnight("Blue", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));


					}
					else if (gameBoard.Pins[i, j] == PinType.White)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = true;
						pin.GetChild<IdleKnight>(1).setKnight("Red", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));


					}
					else if (gameBoard.Pins[i, j] == PinType.PossibleMove)
					{
						AnimatedSprite2D sprite = pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0);
						sprite.Visible = true;
						pin.GetChild<IdleKnight>(1).setKnight("Black", pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0));

					}
					pin.GetChild<IdleKnight>(1).SetOutline(pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0), false);
				}
			}
		}


		for (int i = 0; i < gameBoard.Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < gameBoard.Tiles.GetLength(1); j++)
			{
				if (!(i == 0 && j == 0 || i == 0 && j == 5 || i == 5 && j == 0 || i == 5 && j == 5))
				{
					if (tiles.TryGetValue(new Vector2I(i, j), out Tile tile))
					{
						if (gameBoard.Tiles[i, j] == TileType.Empty)
						{
							tile.Texture = null;
						}
						else if (gameBoard.Tiles[i, j] == TileType.Black)
						{
							tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Blue Buildings/Tower.png");

						}
						else if (gameBoard.Tiles[i, j] == TileType.White)
						{
							tile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Buildings/Red Buildings/Tower.png");

						}
					}
				}
			}
		}

		if (GameController.Controller.controllerState == GameController.ControllerState.MakingMove)
		{
			if (pins.TryGetValue(GameController.Controller.selectedPin, out Pin pin))
			{
				pin.GetChild<IdleKnight>(1).SetOutline(pin.GetChild<IdleKnight>(1).GetChild<AnimatedSprite2D>(0), true); // back to normal

			}

		}
	}


	private bool ChangedValue(PinType pinType, Pin pin)
	{
		if (pinType == PinType.Black && pin.GetChild<IdleKnight>(1).knightColor == "Blue"
			|| pinType == PinType.White && pin.GetChild<IdleKnight>(1).knightColor == "Red"
			|| pinType == PinType.Empty && pin.GetChild<IdleKnight>(1).knightColor == "Purple"
			|| pinType == PinType.PossibleMove && pin.GetChild<IdleKnight>(1).knightColor == "Black")
		{
			return false; 
		}
		return true; 
	}
	private bool clickable(Vector2I pos)
	{
		if (GameController.Controller.controllerState == GameController.ControllerState.SelectingPin)
		{
			if (GameController.Controller.turn == GameController.Turn.Black
				&& GameController.Controller.gameBoard.Pins[pos.X, pos.Y] == PinType.Black
				||
				GameController.Controller.turn == GameController.Turn.White
				&& GameController.Controller.gameBoard.Pins[pos.X, pos.Y] == PinType.White)
			{
				return true; 
			}
			else
			{
				return false; 
			}
		}
		else if (GameController.Controller.controllerState == GameController.ControllerState.MakingMove)
		{
			if (pos == GameController.Controller.selectedPin
				|| GameController.Controller.gameBoard.Pins[pos.X, pos.Y] == PinType.PossibleMove)
			{
				return true;
			}
			else
			{
				return true;
			}
		}
		else
		{
			return false;
		}

	}
	
}
