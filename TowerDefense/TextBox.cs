using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace TowerDefense
{
    public class TextBox
    {
        public string text;
        private bool isFocued = false;
        private static GameWindow gw;
        private bool beingPressed;
        private Rectangle textbox;

        public TextBox(Rectangle textbox, GameWindow window)
        {
            text = "";
            isFocued = false;
            beingPressed = false;
            this.textbox = textbox;
            gw = window;
        }

        private void OnInput(object sender, TextInputEventArgs e)
        {
            var k = e.Key;
            var c = e.Character;
            if (k == Keys.Space)
            {
                text += ' ';
            }
            else if (k == Keys.Back)
            {
                text = text.Remove(text.Length - 1, 1);
            }
            else
            {
                text += c;
            }

            Console.WriteLine(text);
        }

        public void FocusOnTextbox()
        {
            Point mousePos = Mouse.GetState().Position;
            bool isMouseClicked = Mouse.GetState().LeftButton == ButtonState.Pressed;

            if (isMouseClicked && textbox.Intersects(new Rectangle(mousePos, new Point(1, 1))))
            {
                if (!beingPressed)
                {
                    beingPressed = true;
                    isFocued = !isFocued;
                    if (isFocued)
                    {
                        gw.TextInput += OnInput;
                    }
                    else if (!isFocued && text != "")
                    {
                        gw.TextInput -= OnInput;
                    }
                }
            }
            else
            {
                beingPressed = false;
            }
        }
    }
}
