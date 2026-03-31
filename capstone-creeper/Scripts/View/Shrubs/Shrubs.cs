using Godot;
using System;

public partial class Shrubs : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		/*sheep thisIsATree = new sheep();
		thisIsATree.Position = new Vector2( 0 + 98, -98 );

		AddChild(thisIsATree );

		bush2 thisIsATree2 = new bush2();
		thisIsATree2.Position = new Vector2(98 + 98, -98 );
		AddChild(thisIsATree2);

		bush3 thisIsATree3 = new bush3();
		thisIsATree3.Position = new Vector2(98 * 2 + 98, -98);

		AddChild(thisIsATree3);

		bush4 thisIsATree4 = new bush4();
		thisIsATree4.Position = new Vector2(98 * 3 + 98, -98);
		AddChild(thisIsATree4);*/

		Vector2[] bushPos = new Vector2[16]
			{
				new Vector2( 98 * 6,98 ),
				new Vector2(98 * 6, 98 * 2),
				new Vector2(98 * 6, 98 * 3),
				new Vector2(98 * 6, 98 * 4),
				new Vector2( -98 ,98  ),
				new Vector2(-98, 98 * 2),
				new Vector2(-98, 98 * 3),
				new Vector2(-98, 98 * 4),
				new Vector2( 0 + 98, -98 ),
				new Vector2( 98 + 98, -98 ),
				new Vector2( 98 * 2 + 98, -98 ),
				new Vector2( 98 * 3 + 98, -98 ),
				new Vector2( 0 + 98, 98 * 6 ),
				new Vector2( 98 + 98, 98 * 6 ),
				new Vector2( 98 * 2 + 98, 98 * 6 ),
				new Vector2( 98 * 3 + 98, 98 * 6 ),
				
			};
		for (int i = 0; i < 16; i++)
		{
			if (i >= 8)
			{
				var ShrubBush1 = new bush3();
				ShrubBush1.Position = bushPos[i];
				AddChild(ShrubBush1);
			}
			else
			{
				var ShrubBush6 = new tree1();
				ShrubBush6.Position = bushPos[i] - new Vector2(0, 30);
				AddChild(ShrubBush6);
			}
				/*Random random = new Random();
				int randomNumber = random.Next(0, 9);

				switch (randomNumber)
				{
					case 0:
						var ShrubBush1 = new sheep();
						ShrubBush1.Position = bushPos[i];
						AddChild(ShrubBush1);
						break;

					case 1:
						var ShrubBush2 = new bush1();
						ShrubBush2.Position = bushPos[i];
						AddChild(ShrubBush2);
						break;

					case 2:
						var ShrubBush3 = new bush2();
						ShrubBush3.Position = bushPos[i];
						AddChild(ShrubBush3);
						break;

					case 3:
						var ShrubBush4 = new bush3();
						ShrubBush4.Position = bushPos[i];
						AddChild(ShrubBush4);
						break;

					case 4:
						var ShrubBush5 = new bush4();
						ShrubBush5.Position = bushPos[i];
						AddChild(ShrubBush5);
						break;

					case 5:
						var ShrubBush6 = new tree1();
						ShrubBush6.Position = bushPos[i];
						AddChild(ShrubBush6);
						break;

					case 6:
						var ShrubBush7 = new tree2();
						ShrubBush7.Position = bushPos[i]; ;
						AddChild(ShrubBush7);
						break;

					case 7:
						var ShrubBush8 = new tree3();
						ShrubBush8.Position = bushPos[i];
						AddChild(ShrubBush8);
						break;

					case 8:
						var ShrubBush9 = new tree4();
						ShrubBush9.Position = bushPos[i];
						AddChild(ShrubBush9);
						break;

					default:
						var ShrubBush = new bush3();
						ShrubBush.Position = bushPos[i];
						AddChild(ShrubBush);
						break;
				}*/
			}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
