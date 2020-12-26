using Envision.Tanks.Math;
using System.Drawing;
using System.Windows.Forms;

namespace Envision.Tanks
{
    public class UIText : UIObject
    {
        public string text;
        protected SolidBrush brush;
        protected Font font;
        protected SizeF size;

        public UIText(string text, Vector2 pos, float width, float height, Color brushColor) : base(pos, width, height)
        {
            this.text = text;
            brush = new SolidBrush(brushColor);
            font = new Font("Arial", 16);
        }

        public void SetFont(Font font)
        {
            this.font = font;
        }

        public override void Draw(PaintEventArgs e)
        {
            size = e.Graphics.MeasureString(text, font);
            e.Graphics.DrawString(text, font, brush, position.X - size.Width / 2, position.Y);
        }
    }
}