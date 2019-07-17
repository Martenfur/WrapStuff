using System;
using System.Collections.Generic;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WrapStuff.Wraps
{
	public class Panel
	{
		static readonly Vector2[] _sideRotations = {
			Vector2.UnitY,
			Vector2.UnitX,
			-Vector2.UnitY,
			-Vector2.UnitX,
		};

		public string Name;
		public Vector2 Position;
		public Vector2 Size;

		public Panel[] Attachments;
		
		public int Side;

		public void Draw()
		{
			RectangleShape.DrawBySize(Position, Size, true);

			if (Attachments == null)
			{
				return;
			}

			foreach(var panel in Attachments)
			{
				GraphicsMgr.AddTransformMatrix(
					Matrix.CreateTranslation(((Size + panel.Size) / 2f * _sideRotations[Side]).ToVector3()) 
					* Matrix.CreateRotationZ((float)Math.PI / 2f * panel.Side)
					* Matrix.CreateTranslation((Position).ToVector3())

				);
				panel.Draw();
				GraphicsMgr.ResetTransformMatrix();
			}
		}
	}
}
