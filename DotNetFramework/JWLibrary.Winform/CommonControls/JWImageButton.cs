using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JWLibrary.Winform.CommonControls
{
    public class JWImageButton : Control
    {
        Image backgroundImage, pressedImage, hoverImage, disableImage, selectImage;
        bool pressed = false;
        bool hover = false;
        bool btnselect = false;

        DialogResult dialogResult;
        public DialogResult DialgResult
        {
            get
            {
                return this.dialogResult;
            }
            set
            {
                this.dialogResult = value;
            }
        }

        public bool Btnselectbtn
        {
            get
            {
                return this.btnselect;
            }
            set
            {
                this.Invalidate();
                this.btnselect = value;
            }
        }

        public Image SelectImage
        {
            get
            {
                return this.selectImage;
            }
            set
            {
                this.selectImage = value;
            }
        }


        public Image DisableImage
        {
            get
            {
                return this.disableImage;
            }
            set
            {
                this.disableImage = value;
            }
        }


        // Property for the background image to be drawn behind the button text.
        public Image BackgroundImage
        {
            get
            {
                return this.backgroundImage;
            }
            set
            {
                this.backgroundImage = value;
            }
        }

        // Property for the background image to be drawn behind the button text when
        // the button is pressed.
        public Image PressedImage
        {
            get
            {
                return this.pressedImage;
            }
            set
            {
                this.pressedImage = value;
            }
        }

        public Image HoverImage
        {
            get
            {
                return this.hoverImage;
            }
            set
            {
                this.hoverImage = value;
            }
        }

        // When the mouse button is pressed, set the "pressed" flag to true
        // and invalidate the form to cause a repaint.  The .NET Compact Framework
        // sets the mouse capture automatically.
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //this.btnselect = true;

            this.pressed = false;
            this.hover = false;

            this.DoubleBuffered = true;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        public void selectbtn(bool bResult)
        {
            this.btnselect = bResult;
            this.DoubleBuffered = true;
            //          this.Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (dialogResult == DialogResult.OK)
            {
                this.FindForm().DialogResult = dialogResult;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                this.FindForm().DialogResult = dialogResult;
            }
            base.OnMouseClick(e);
        }

        // When the mouse is released, reset the "pressed" flag
        // and invalidate to redraw the button in the unpressed state.
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.pressed = true;

            this.btnselect = false;
            this.hover = false;

            this.DoubleBuffered = true;
            this.Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            this.hover = true;

            this.DoubleBuffered = true;
            this.Invalidate();
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.hover = false;
            this.btnselect = false;
            this.pressed = false;

            this.DoubleBuffered = true;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        // Override the OnPaint method to draw the background image and the text.
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.hover && !this.pressed && !this.btnselect && this.Enabled && this.backgroundImage != null) //state normal
            {
                e.Graphics.DrawImage(this.backgroundImage, 0, 0, this.backgroundImage.Width, this.backgroundImage.Height);
            }

            if (!this.hover && !this.pressed && !this.btnselect && !this.Enabled && this.disableImage != null) //state disable
            {
                e.Graphics.DrawImage(this.disableImage, 0, 0, this.disableImage.Width, this.disableImage.Height);
            }

            if (!this.hover && this.pressed && !this.btnselect && this.Enabled && this.pressedImage != null) //state pressed
            {
                e.Graphics.DrawImage(this.pressedImage, 0, 0, this.PressedImage.Width, this.PressedImage.Height);
            }

            if (!this.hover && !this.pressed && this.btnselect && this.Enabled && this.selectImage != null) //state select
            {
                e.Graphics.DrawImage(this.selectImage, 0, 0, this.selectImage.Width, this.selectImage.Height);
            }

            if (this.hover && !this.pressed && !this.btnselect && this.Enabled && this.hoverImage != null) //state hover
            {
                e.Graphics.DrawImage(this.hoverImage, 0, 0, this.hoverImage.Width, this.hoverImage.Height);
            }

            base.OnPaint(e);
        }
    }
}
