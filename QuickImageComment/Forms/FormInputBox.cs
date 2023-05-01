using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormInputBox : Form
    {
        internal string resultString = "";

        public FormInputBox(string prompt, string defaultResponse)
        {
            InitializeComponent();
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);

            // adjust height of form/label to show all text
            TextFormatFlags textFormatFlags = new TextFormatFlags();
            textFormatFlags |= TextFormatFlags.WordBreak;
            SizeF newSize = TextRenderer.MeasureText(prompt, this.Font, dynamicLabel1.Size, textFormatFlags);
            this.Height += (int)(newSize.Height - dynamicLabel1.Size.Height) + 1;

            dynamicLabel1.Text = prompt;
            textBox1.Text = defaultResponse;

            LangCfg.translateControlTexts(this);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            resultString = textBox1.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            resultString = "";
            Close();
        }
    }
}
