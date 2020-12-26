using System.Drawing;
using System.Windows.Forms;
using Envision.Tanks.Math;

namespace Envision.Tanks
{
    public class UIFramedText : UIText
    {
        Pen pen;
        public UIFramedText(string text, Vector2 pos, float width, float height, Color brushColor, Color frameColor) : base(text, pos, width, height, brushColor)
        {
            pen = new Pen(frameColor);
        }

        public void ChangeFrameColor(Color color)
        {
            pen.Color = color;
        }

        public override void Draw(PaintEventArgs e)
        {
            base.Draw(e);
            e.Graphics.ResetTransform();
            e.Graphics.DrawRectangle(pen, position.X - width / 2, position.Y, width, height);
        }
    }
}