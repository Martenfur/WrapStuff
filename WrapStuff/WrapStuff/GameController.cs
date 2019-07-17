using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using System;
using WrapStuff.Wraps;

namespace WrapStuff
{
	public class GameController : Entity
	{
		Camera cam = new Camera(1000, 800);

		Wrap wrap;
		
		public GameController() : base(SceneMgr.GetScene("default")["default"])
		{
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
			

			wrap = new Wrap();
			wrap.ReadFromXML(Environment.CurrentDirectory + "/Content/Wraps/BeerPack.xml");

			cam.Position = wrap.Position + Vector2.UnitX * 500;

		}

		public override void Update()
		{
			
		}

		
		public override void Draw()
		{
			GraphicsMgr.CurrentColor = Color.White * 0.5f;
			wrap.Draw();
		}

	}
}