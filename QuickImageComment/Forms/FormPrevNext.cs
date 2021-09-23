using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormPrevNext : Form
    {
        protected FormPrevNext previousWindow;
        protected FormPrevNext nextWindow;
        protected string displayedFileName;
        private static SortedList<string, FormPrevNext> lastWindows = new SortedList<string, FormPrevNext>();

        public FormPrevNext(ExtendedImage extendedImage)
        {
            InitializeComponent();
            if (lastWindows.ContainsKey(this.GetType().Name))
            {
                // new window added to existing chain
                previousWindow = lastWindows[this.GetType().Name];
                previousWindow.nextWindow = this;
                lastWindows[this.GetType().Name] = this;
            }
            else
            {
                // create new chain
                lastWindows.Add(this.GetType().Name, this);
            }
            displayedFileName = "";
            if (extendedImage != null)
            {
                displayedFileName = extendedImage.getImageFileName();
            }
        }

        // event closing form
        protected virtual void FormPrevNext_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (nextWindow != null || previousWindow != null)
            {
                // adjust references previous/next
                if (nextWindow != null)
                {
                    nextWindow.previousWindow = previousWindow;
                }
                else
                {
                    // there is no next window, so this was the last in chain
                    lastWindows[this.GetType().Name] = this.previousWindow;
                }
                if (previousWindow != null)
                {
                    previousWindow.nextWindow = nextWindow;
                }
            }
            else
            {
                // last window, save configuration
                lastWindows.Remove(this.GetType().Name);
                saveConfigDefinitions();
            }
        }

        // get last window of type
        protected static FormPrevNext getLastWindow(string typeName)
        {
            if (lastWindows.ContainsKey(typeName))
            {
                return lastWindows[typeName];
            }
            else
            {
                return null;
            }
        }

        // get window displaying file
        protected static FormPrevNext getWindowForImage(string typeName, ExtendedImage extendedImage)
        {
            string fullFileName = "";
            if (extendedImage != null)
            {
                fullFileName = extendedImage.getImageFileName();
            }
            FormPrevNext prev1 = getLastWindow(typeName);
            while (prev1 != null)
            {
                FormPrevNext prev2 = prev1.previousWindow;
                if (prev1.displayedFileName.Equals(fullFileName))
                {
                    return prev1;
                }
                prev1 = prev2;
            }
            return null;
        }

        // close all opened windows
        protected static void closeAllWindows(string typeName)
        {
            FormPrevNext lastFormPrevNext = getLastWindow(typeName);
            if (lastFormPrevNext != null)
            {
                closePreviousWindows(lastFormPrevNext);
                lastFormPrevNext.Close();
            }
        }

        // close referenced previous windows (used when several windows were opened and then only one image is selected)
        private static void closePreviousWindows(FormPrevNext keep)
        {
            FormPrevNext prev1 = keep.previousWindow;
            while (prev1 != null)
            {
                FormPrevNext prev2 = prev1.previousWindow;
                prev1.Close();
                prev1 = prev2;
            }
        }

        protected static bool windowsAreOpen(string typeName)
        {
            if (lastWindows.ContainsKey(typeName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected static void closeUnusedWindows(string typeName)
        {
            FormPrevNext prev1 = getLastWindow(typeName);
            while (prev1 != null)
            {
                FormPrevNext prev2 = prev1.previousWindow;
                // displayed file is not selected
                if (!MainMaskInterface.isFileSelected(prev1.displayedFileName))
                {
                    prev1.Close();
                }
                prev1 = prev2;
            }
        }

        protected static bool onlyOneWindow(string typeName)
        {
            FormPrevNext prev1 = getLastWindow(typeName);
            if (prev1 == null)
                return false;
            else
                return prev1.previousWindow == null;
        }

        // save the configuration data; usually needs overload in derived classes
        protected virtual void saveConfigDefinitions()
        {
        }
    }
}
