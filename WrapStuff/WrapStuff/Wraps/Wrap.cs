using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;

namespace WrapStuff.Wraps
{

	public class Wrap
	{
		public Panel Root;
		public Vector2 Position;
		public float Rotation;

		public void Draw()
		{
			GraphicsMgr.AddTransformMatrix(
				// Origin point for the panel is on the bottm, so we need to center it.
				Matrix.CreateTranslation((Vector2.UnitY * Root.Size.Y / 2f).ToVector3()) 
				* Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation))
				* Matrix.CreateTranslation(Position.ToVector3())
			);
			Root.Draw();
			GraphicsMgr.ResetTransformMatrix();
		}
	}
}
