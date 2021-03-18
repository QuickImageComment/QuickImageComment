using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormPrevNext : Form
    {
        protected FormPrevNext previousWindow;
        protected FormPrevNext nextWindow;
        private static SortedList<string, FormPrevNext> lastWindows = new SortedList<string, FormPrevNext>();

        public FormPrevNext()
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
                // create new chaing
                lastWindows.Add(this.GetType().Name, this);
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
                    // there is no next window, so this is now the last in chain
                    lastWindows[this.GetType().Name] = this;
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

        // close all opened windows
        internal static void closeAllWindows(string typeName)
        {
            FormPrevNext lastFormPrevNext = getLastWindow(typeName);
            if (lastFormPrevNext != null)
            {
                closePreviousWindows(lastFormPrevNext);
                lastFormPrevNext.Close();
            }
        }

        // close referenced previous windows (used when several windows were opened and then only one image is selected)
        internal static void closePreviousWindows(FormPrevNext keep)
        {
            FormPrevNext prev1 = keep.previousWindow;
            while (prev1 != null)
            {
                FormPrevNext prev2 = prev1.previousWindow;
                prev1.Close();
                prev1 = prev2;
            }
        }

        internal static bool windowsAreOpen(string typeName)
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

        // save the configuration data; usually needs overload in derived classes
        protected virtual void saveConfigDefinitions()
        {
        }

        //// reference window is static and thus needs to be declared in derived class
        //// these methods need to be overriden to get and set reference window in derived class
        //protected abstract FormPrevNext getReferenceWindow();
        //protected abstract void setReferenceWindow(FormPrevNext formPrevNext);
    }
}
