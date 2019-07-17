using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Cameras;
using Resources.Sprites;
using WrapStuff.Wraps;

namespace WrapStuff
{
	public class GameController : Entity
	{
		Camera cam = new Camera(800, 600);

		Wrap wrap;
		
		public GameController() : base(SceneMgr.GetScene("default")["default"])
		{
			GameMgr.MaxGameSpeed = 60;
			GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

			cam.BackgroundColor = new Color(38, 38, 38);

			GameMgr.WindowManager.CanvasSize = new Vector2(800, 600);
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			GameMgr.WindowManager.CanvasMode = CanvasMode.Fill;
			
			GraphicsMgr.Sampler = SamplerState.PointClamp;


			var a = new Panel
			{
				Offset = new Vector2(20, 0),
				Size = new Vector2(16, 64),
				Side = 2,
			};
			var a1 = new Panel
			{
				Offset = new Vector2(0, 0),
				Size = new Vector2(16, 64),
				Side = 0,
				Attachments = new Panel[] {a}
			};
			var a2 = new Panel
			{
				Offset = new Vector2(0, 0),
				Size = new Vector2(16, 16),
				Side = 1
			};
			var a3 = new Panel
			{
				Offset = new Vector2(0, 0),
				Size = new Vector2(16, 32),
				Side = 2,
				Attachments = new Panel[] { a }
			};
			var a4 = new Panel
			{
				Offset = new Vector2(0, 0),
				Size = new Vector2(16, 48),
				Side = 3,
				Attachments = new Panel[] { a, a2 }
			};
			var panel = new Panel
			{
				Offset = new Vector2(0, 0),
				Size = new Vector2(100, 200),
				Attachments = new Panel[]{a1, a2, a3, a4}
			};

			wrap = new Wrap();
			wrap.Position = new Vector2(400, 400);
			wrap.Root = panel;
			

		}

		public override void Update()
		{
			
		}

		
		public override void Draw()
		{
			//Default.Monofoxe.Draw(new Vector2(400, 300), Default.Monofoxe.Origin);
			GraphicsMgr.CurrentColor = Color.White * 0.5f;
			wrap.Draw();
			wrap.Rotation += 1;
		}

	}
}