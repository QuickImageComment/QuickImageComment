using System.ComponentModel;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    internal class TextBoxQIC : TextBox
    {
        private bool _singleLineNoBorder = false;

        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool SingleLineNoBorder
        {
            get => _singleLineNoBorder;
            set
            {
                if (_singleLineNoBorder != value)
                {
                    _singleLineNoBorder = value;
                    ApplySingleLineNoBorderMode();
                }
            }
        }

        private void ApplySingleLineNoBorderMode()
        {
            if (SingleLineNoBorder)
            {
                int oldHeight = this.Height;
                this.SingleLineNoBorder = true;
                this.Multiline = false;
                this.ScrollBars = ScrollBars.None;
                this.BorderStyle = BorderStyle.None;
                this.Top += (oldHeight - this.Height) / 2;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            //// Migration logic: detect old single-line borderless TextBoxes
            //if (!this.Multiline &&
            //    (this.BorderStyle == BorderStyle.Fixed3D || this.BorderStyle == BorderStyle.FixedSingle))
            //{
            //    this.SingleLineNoBorder = true;
            //}

            // Apply mode (Designer-safe)
            ApplySingleLineNoBorderMode();
        }
    }
}
