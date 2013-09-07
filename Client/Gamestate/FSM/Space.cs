using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

using FTLOverdrive.Client.UI;
using FTLOverdrive.Client.Ships;


namespace FTLOverdrive.Client.Gamestate.FSM
{
	/// <summary>
	/// Description of Space.
	/// </summary>
	public class Space : IState, IRenderable
	{
		
		 private RenderWindow window;
        private IntRect rctScreen;

        private Sprite sprBackground;
		private bool finishnow;
		private ShipRenderer shipRenderer = new ShipRenderer();
		  private Library.ShipGenerator currentShipGen;
        private Ship currentShip;
		 private Music FTLjump;
		
	
		 public void OnActivate()
        {
            // Store window
            window = Root.Singleton.Window;
            rctScreen = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);
            Library.ShipGenerator gen = GetDefaultShipGenerator();
			int layout = 0;
            // Load sprites
            Console.WriteLine("Loading Space");
            sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_dullstars.png"));
            sprBackground.Position = new Vector2f(rctScreen.Left, rctScreen.Top);
            sprBackground.Scale = Util.Scale(sprBackground, new Vector2f(rctScreen.Width, rctScreen.Height));
			//Load UI  
            shipRenderer.ShowRooms = true;
       
          	//This sets the ship's currrent position on the screen
            Util.LayoutControl(shipRenderer, 50, 135, 660, 450, rctScreen);
            shipRenderer.Parent = Root.Singleton.Canvas;
            shipRenderer.Init();
            
            // Set current ship
           	currentShipGen = gen;
            currentShip = gen.Generate(layout);
			// Update ship renderer
            shipRenderer.Ship = currentShip;
            
            var btnBottom = new ImageButton();
            btnBottom.Image = Root.Singleton.Material("img/buttons/FTL/FTL_base.png");
             btnBottom.OnClick += (sender) =>
            {
            		//Play sound while when button is clicked and load a new background    
 					FTLjump = Root.Singleton.Music("audio/waves/ship/bp_jump_1.ogg");
            		FTLjump.Stop();
            		FTLjump.Play();
            		
            		//We're jumping to a random space. Pick an image!
            		Random random = new Random();
            		
            		int randNum = random.Next(1,11);
            		switch(randNum)
            		{
            			case 1:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_blueStarcluster.png"));
            			break;
            			case 2:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_darknebula.png"));
            			break;
            			case 3:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_dullstars.png"));
            			break;
            			case 4:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_dullstars2.png"));
            			break;
            			case 5:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_lonelyRedStar.png"));
            			break;
            			case 6:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_lonelystar.png"));
            			break;
            			case 7:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/bg_smallbluenebula.png"));
            			break;
            			case 8:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/low_asteroid.png"));
            			break;
            			case 9:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/low_nebula.png"));
            			break;
            			case 10:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/low_storm.png"));
            			break;
            			case 11:
            				sprBackground = new Sprite(Root.Singleton.Material("img/stars/low_sun.png"));
            			break;
            				
            				
            		}
            		
            	 	
				  
                
            };
            Util.LayoutControl(btnBottom, 700, 10, btnBottom.Image.Size, rctScreen);
            
            btnBottom.Parent = Root.Singleton.Canvas;
            btnBottom.Init();
            
            
            
            var btnNewSpace = new ImageButton();
            btnNewSpace.Image = Root.Singleton.Material("img/buttons/FTL/FTL_JUMP.png");
           
            btnNewSpace.Enabled = true;
        
          
            Util.LayoutControl(btnNewSpace, 700, 10, btnNewSpace.Image.Size, rctScreen);
          
            btnNewSpace.Parent = Root.Singleton.Canvas;
            btnNewSpace.Init();
            var btnSpace = new ImageButton();
            btnSpace.Image = Root.Singleton.Material("img/buttons/FTL/FTL_READY.png");
           
            btnSpace.Enabled = true;
        
           
            Util.LayoutControl(btnSpace, 700, 15, btnSpace.Image.Size, rctScreen);
           
            btnSpace.Parent = Root.Singleton.Canvas;
            btnSpace.Init();
           
        
		 }
		
		private Library.ShipGenerator GetDefaultShipGenerator()
        {
            var lib = Root.Singleton.mgrState.Get<Library>();
            var gens = lib.GetPlayerShipGenerators();
            foreach (var gen in gens)
            {
                if (gen.Default)
                {
                    return gen;
                }
            }
            throw new Exception("No default ship generator!");
        }

		 
		 public void OnDeactivate()
        {
            Root.Singleton.Canvas.Clear();
        }
		 
		public void Think(float delta)
        {
            if (finishnow)
            {
                // Close state
                Root.Singleton.mgrState.Deactivate<NewGame>();

                // Reopen main menu
                Root.Singleton.mgrState.FSMTransist<MainMenu>();
            }
        }

        public void Render(RenderStage stage)
        {
            if (stage == RenderStage.PREGUI)
            {
                window.Draw(sprBackground);
            }
        } 

	}
}