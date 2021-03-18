//Copyright (C) 2009 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using QuickImageComment;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    class ListBoxComments : System.Windows.Forms.ListBox
    {
        private System.ComponentModel.IContainer components;
        private TextBox textBoxUserComment;
        private string MouseDoubleClickAction;

        private ContextMenuStrip ContextMenuStripComments;
        private ToolStripMenuItem toolStripMenuItemOverwrite;
        private ToolStripMenuItem toolStripMenuItemAppendSpace;
        private ToolStripMenuItem toolStripMenuItemAppendComma;
        private ToolStripMenuItem toolStripMenuItemAppendSemicolon;

        public ListBoxComments()
        {
            //
            // This call is required by the Windows.Forms Form Designer.
            //
            InitializeComponent();

            this.ContextMenuStrip = this.ContextMenuStripComments;
            this.Leave += new System.EventHandler(this.listBoxComments_Leave);
            this.KeyDown += new KeyEventHandler(this.listBoxComments_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxComments_MouseDown);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxComments_MouseDoubleClick);
        }

        #region Component Designer generated code
        /**
     * Clean up any resources being used.
     */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /**
         * Required method for Designer support - do not modify 
         * the contents of this method with the code editor.
         */
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ContextMenuStripComments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOverwrite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAppendSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAppendComma = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAppendSemicolon = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuStripComments.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContextMenuStripComments
            // 
            this.ContextMenuStripComments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOverwrite,
            this.toolStripMenuItemAppendSpace,
            this.toolStripMenuItemAppendComma,
            this.toolStripMenuItemAppendSemicolon}
            );
            this.ContextMenuStripComments.Name = "ContextMenuStripComments";
            this.ContextMenuStripComments.ShowImageMargin = false;
            this.ContextMenuStripComments.Size = new System.Drawing.Size(259, 92);
            this.ContextMenuStripComments.TabStop = true;
            // 
            // toolStripMenuItemOverwrite
            // 
            this.toolStripMenuItemOverwrite.Name = "toolStripMenuItemOverwrite";
            this.toolStripMenuItemOverwrite.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemOverwrite.Text = "Übernehmen [Return]";
            this.toolStripMenuItemOverwrite.Click += new System.EventHandler(this.toolStripMenuItemOverwrite_Click);
            // 
            // toolStripMenuItemAppendSpace
            // 
            this.toolStripMenuItemAppendSpace.Name = "toolStripMenuItemAppendSpace";
            this.toolStripMenuItemAppendSpace.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemAppendSpace.Text = "Anhängen mit Leerzeichen [Leertaste]";
            this.toolStripMenuItemAppendSpace.Click += new System.EventHandler(this.toolStripMenuItemAppendSpace_Click);
            // 
            // toolStripMenuItemAppendComma
            // 
            this.toolStripMenuItemAppendComma.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemAppendComma.Name = "toolStripMenuItemAppendComma";
            this.toolStripMenuItemAppendComma.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemAppendComma.Text = "Anhängen mit Komma [,]";
            this.toolStripMenuItemAppendComma.Click += new System.EventHandler(this.toolStripMenuItemAppendComma_Click);
            // 
            // toolStripMenuItemAppendSemicolon
            // 
            this.toolStripMenuItemAppendSemicolon.Name = "toolStripMenuItemAppendSemicolon";
            this.toolStripMenuItemAppendSemicolon.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemAppendSemicolon.Text = "Anhängen mit Semikolon [;]";
            this.toolStripMenuItemAppendSemicolon.Click += new System.EventHandler(this.toolStripMenuItemAppendSemicolon_Click);
            // 
            // ListBoxComments
            // 
            this.Size = new System.Drawing.Size(120, 95);
            this.ContextMenuStripComments.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        // set TextBox for User Comment
        // not in constructor, because sequence of GUI controls may be changed by layout editor
        // then TextBoxUserComment might not be instantiated, when constructor of this class is called
        public void set_TextBoxUserComment(TextBox givenTextBoxUserComment)
        {
            textBoxUserComment = givenTextBoxUserComment;
        }
        // set mouse double click action
        public void set_MouseDoubleClickAction(string givenMouseDoubleClickAction)
        {
            MouseDoubleClickAction = givenMouseDoubleClickAction;
        }

        // event handler triggered when list is left
        private void listBoxComments_Leave(object sender, System.EventArgs e)
        {
            this.SelectedItems.Clear();
        }

        // key event handler for list box of last user comments
        private void listBoxComments_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Return)
            {
                updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionOverwrite);
            }
            else if (theKeyEventArgs.KeyCode == Keys.Space)
            {
                updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionAppendSpace);
            }
            else if (theKeyEventArgs.KeyCode == Keys.Oemcomma && !theKeyEventArgs.Shift)
            {
                updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionAppendComma);
            }
            else if (theKeyEventArgs.KeyCode == Keys.Oemcomma && theKeyEventArgs.Shift)
            {
                updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionAppendSemicolon);
            }
        }

        // mouse double click event handler for list box of last user comments
        private void listBoxComments_MouseDoubleClick(object sender, MouseEventArgs theMouseEventArgs)
        {
            updateTextBoxUserCommentBySelectedEntry(MouseDoubleClickAction);
        }

        // mouse event handler to select item with right button before opening context menu
        private void listBoxComments_MouseDown(object sender, MouseEventArgs theMouseEventArgs)
        {
            if (theMouseEventArgs.Button == MouseButtons.Right)
            {
                int index = this.IndexFromPoint(new System.Drawing.Point(theMouseEventArgs.X, theMouseEventArgs.Y));
                this.SelectedIndex = index;
            }
        }

        // methods from context menu
        private void toolStripMenuItemOverwrite_Click(object sender, System.EventArgs e)
        {
            updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionOverwrite);
        }
        private void toolStripMenuItemAppendSpace_Click(object sender, System.EventArgs e)
        {
            updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionAppendSpace);
        }
        private void toolStripMenuItemAppendComma_Click(object sender, System.EventArgs e)
        {
            updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionAppendComma);
        }
        private void toolStripMenuItemAppendSemicolon_Click(object sender, System.EventArgs e)
        {
            updateTextBoxUserCommentBySelectedEntry(ConfigDefinition.CommentsActionAppendSemicolon);
        }

        // update text box for user comment with entry of this list
        // given separator will be inserted
        private void updateTextBoxUserCommentBySelectedEntry(string CommentsAction)
        {
            string SelectedString;
            if (this.SelectedItems.Count == 1)
            {
                SelectedString = this.SelectedItem.ToString();
                if (CommentsAction.Equals(ConfigDefinition.CommentsActionOverwrite))
                {
                    textBoxUserComment.Text = SelectedString;
                    // Set Focus to User Comment and set Cursor at end
                    textBoxUserComment.Focus();
                    textBoxUserComment.Select(textBoxUserComment.TextLength, textBoxUserComment.TextLength);
                }
                else
                {
                    bool updateTextBox = true;
                    if (textBoxUserComment.Text.ToLower().Contains(SelectedString.ToLower()))
                    {
                        DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_textAlreadyInComment, SelectedString);
                        if (theDialogResult.Equals(DialogResult.No))
                        {
                            updateTextBox = false;
                        }
                    }
                    if (updateTextBox)
                    {
                        if (CommentsAction.Equals(ConfigDefinition.CommentsActionAppendSpace))
                        {
                            textBoxUserComment.Text = textBoxUserComment.Text + " " + SelectedString;
                        }
                        else if (CommentsAction.Equals(ConfigDefinition.CommentsActionAppendComma))
                        {
                            textBoxUserComment.Text = textBoxUserComment.Text + ", " + SelectedString;
                        }
                        else if (CommentsAction.Equals(ConfigDefinition.CommentsActionAppendSemicolon))
                        {
                            textBoxUserComment.Text = textBoxUserComment.Text + "; " + SelectedString;
                        }
                    }
                }
            }
            if (this.Focused == false)
            {
                this.SelectedItems.Clear();
            }
        }
    }
}
