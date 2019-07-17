using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.SceneSystem;
using System;
using WrapStuff.Wraps;

namespace WrapStuff
{
	public class GameController : Entity
	{
		Camera cam;
		Wrap wrap;

		public GameController() : base(SceneMgr.GetScene("default")["default"])
		{
			GraphicsMgr.Device.PresentationParameters.DepthStencilFormat = DepthFormat.Depth24Stencil8;
			var depthState = new DepthStencilState();
			depthState.DepthBufferEnable = true;
			depthState.DepthBufferWriteEnable = true;


			GraphicsMgr.Device.DepthStencilState = DepthStencilState.Default;


			cam = new Camera(1000, 800);

			GameMgr.MaxGameSpeed = 60;
			GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

			cam.BackgroundColor = new Color(38, 38, 38);
			cam.Zoom = 0.25f;
			cam.Offset = cam.Size / 2;

			GameMgr.WindowManager.CanvasSize = cam.Size;
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			GameMgr.WindowManager.CanvasMode = CanvasMode.Fill;

			GraphicsMgr.Sampler = SamplerState.PointClamp;
			GraphicsMgr.Rasterizer = RasterizerState.CullNone;
			
			wrap = new Wrap();
			//wrap.ReadFromXML(Environment.CurrentDirectory + "/Content/Wraps/BeerPack.xml");
			wrap.Position = Vector2.Zero;

			var p1 = new Panel
			{
				Size = new Vector2(100, 500),
				Side = 2,
			};
			var p = new Panel
			{
				Size = new Vector2(500, 100),
				Side = 1,
				Attachments = new Panel[]{p1}
			};
			


			wrap.Root = new Panel
			{
				Size = new Vector2(1000, 1000),
				Attachments = new Panel[]{p, p1},
			};

			cam.Position = wrap.Position + Vector2.UnitX * 500;
			Init3D();



		}

		public override void Update()
		{
			camPos.X += (Input.CheckButton(Buttons.Right).ToInt()  - Input.CheckButton(Buttons.Left).ToInt()) * 8;
			camPos.Y += (Input.CheckButton(Buttons.Down).ToInt() - Input.CheckButton(Buttons.Up).ToInt()) * 8;

		}


		public override void Draw()
		{
			GraphicsMgr.CurrentColor = Color.White * 0.5f;
			wrap.Draw();
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.None);

			Draw3D(1);
		}

		AlphaTestEffect effect;

		void Init3D()
		{
			
			effect = new AlphaTestEffect(GameMgr.Game.GraphicsDevice);
			//effect.AlphaFunction = CompareFunction.Equal;

		}

		Vector2 camPos;

		void Draw3D(float mul = 1f)
		{
			var graphics = GameMgr.Game.GraphicsDevice;
			
			var t = GameMgr.ElapsedTimeTotal;
			var center = wrap.Position;
			var cameraPosition = new Vector3(camPos.X + center.Y, camPos.Y + center.Y, 800);
			
			effect.World = Matrix.CreateTranslation(Vector3.Zero);
			effect.View =
			//Matrix.CreateRotationZ((float)GameMgr.ElapsedTimeTotal * mul)
			Matrix.CreateLookAt(cameraPosition, new Vector3(center.X, center.Y, 100), Vector3.UnitZ);
			//* Matrix.CreateRotationZ((float)GameMgr.ElapsedTimeTotal * mul);

			float aspectRatio = cam.Size.X / cam.Size.Y;
			
			effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 40000);
			
			//effect.TextureEnabled = true;
			effect.Texture = Resources.Sprites.Default.Monofoxe[0].Texture;

			wrap.Draw3D(effect);

		}

	}
}