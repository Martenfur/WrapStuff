using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

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
				// Origin point for the panel is on the bottom, so we need to center it.
				Matrix.CreateTranslation((Vector2.UnitY * Root.Size.Y / 2f).ToVector3()) 
				* Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation))
				* Matrix.CreateTranslation(Position.ToVector3())
			);
			Root.Draw();
			GraphicsMgr.ResetTransformMatrix();
		}
		

		/// <summary>
		/// Reads the wrap data from XML.
		/// </summary>
		public void ReadFromXML(string path)
		{
			var xml = new XmlDocument();
			xml.Load(path);

			var folding = xml["folding"];

			Position.X = GetXmlFloat(folding, "rootX"); 
			Position.Y = GetXmlFloat(folding,"rootY");

			var panelsXml = folding["panels"];
			Root = ParsePanel(panelsXml["item"]);
		}

		/// <summary>
		/// Reads the panel and its cildren data.
		/// </summary>
		Panel ParsePanel(XmlNode panelXml)
		{
			var panel = new Panel();

			// Panel properties.
			panel.Name = panelXml.Attributes["panelName"].Value;
			
			panel.Size = new Vector2(
				GetXmlFloat(panelXml, "panelWidth"),
				GetXmlFloat(panelXml, "panelHeight")
			);
			panel.Offset = new Vector2(
				GetXmlFloat(panelXml, "hingeOffset"),
				0
			);
			panel.Side = GetXmlInt(panelXml, "attachedToSide");
			// Panel properties.

			// Retrieving children.
			var children = new List<Panel>();

			foreach(XmlNode child in panelXml["attachedPanels"].ChildNodes)
			{
				children.Add(ParsePanel(child));
			}
			panel.Attachments = children.ToArray();
			// Retrieving children.

			return panel;
		}



		public static float GetXmlFloat(XmlNode node, string attribute)
		{
			if (node.Attributes[attribute] != null)
			{
				return float.Parse(node.Attributes[attribute].Value, CultureInfo.InvariantCulture);
			}
			return 0f;
		}

		public static int GetXmlInt(XmlNode node, string attribute)
		{
			if (node.Attributes[attribute] != null)
			{
				return int.Parse(node.Attributes[attribute].Value);
			}
			return 0;
		}


	}
}
