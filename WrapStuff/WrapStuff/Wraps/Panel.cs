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
		
		/// <summary>
		/// Tells, on which side relative to the parent the panel will be.
		/// 0 - bottom
		/// 1 - right
		/// 2 - top
		/// 3 - left
		/// </summary>
		public int Side;

		public string Name;
		public Vector2 Position;
		public Vector2 Size;

		public Panel[] Attachments;
		

		public void Draw(bool root)
		{
			// Transform matrices took care of the rotations,
			// so all panels assume they are drawn with no rotation\offset.
			RectangleShape.DrawBySize(Position - Vector2.UnitY * Size / 2f, Size, true);
			
			CircleShape.Draw(Position, 4, true);
			
			LineShape.Draw(Position, Position - Vector2.UnitY * 8);


			if (Attachments == null)
			{
				return;
			}

			GraphicsMgr.AddTransformMatrix(
				Matrix.CreateTranslation(-(Vector2.UnitY * Size / 2f).ToVector3())
			);
			foreach (var panel in Attachments)
			{
				// We need to offset the child panel by half of parent's 
				// width or height depending on the side.
				var length = Size.X / 2f;
				var resultSide = panel.Side;
				if (panel.Side % 2 == 0)
				{
					length = Size.Y / 2f;
				}

				GraphicsMgr.AddTransformMatrix(
					//Matrix.CreateRotationZ(0.3f) // Future reference - use this to pivot the panels.
					Matrix.CreateTranslation(-(Vector2.UnitY * length).ToVector3())
					* Matrix.CreateRotationZ((float)(Math.PI - Math.PI / 2f * resultSide))
					* Matrix.CreateTranslation(Position.ToVector3())
				);
				panel.Draw(false);
				GraphicsMgr.ResetTransformMatrix();
			}

			GraphicsMgr.ResetTransformMatrix();
		}
	}
}
